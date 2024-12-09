using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SurveyApp.Core;
using SurveyApp.Web.Areas.Identity.Data;
using SurveyApp.Web.Models;
using SurveyApp.Web.Models.ViewModel;
using SurveyApp.Web.Services;

namespace SurveyApp.Web.Controllers
{
    [Authorize]
    public class SurveyController : Controller
    {
        private readonly SurveyService _surveyService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtectionProvider _dataProtectionProvider;

        private readonly ILogger<SurveyController> _logger;

        // Constructor to initialize services, user manager, data protection, and logger
        public SurveyController(SurveyService surveyService, UserManager<ApplicationUser> userManager,
            IDataProtectionProvider dataProtectionProvider, ILogger<SurveyController> logger)
        {
            _surveyService = surveyService;
            _userManager = userManager;
            _dataProtectionProvider = dataProtectionProvider;
            _logger = logger;
        }

        // Displays a paginated list of surveys for the logged-in user
        public async Task<IActionResult> Index(int? page)
        {
            try
            {
                _logger.LogDebug("Test Debug Message!!");
                //throw new Exception();
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null) return Challenge();

                int currentPageIndex = page.HasValue ? page.Value : 1;
                var surveys = await _surveyService.GetAllSurveysAsync(currentUser, currentPageIndex);
                int surveycount = await _surveyService.CountSurveyAsync(currentUser);
                var paging = new Pagination(surveycount, currentPageIndex, 10);

                ViewBag.paged = paging;
                var model = new SurveyListViewModel()
                {
                    Surveys = surveys.ToArray()
                };
                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw e;
            }
        }

        // Processes and returns a list of surveys in a paginated format (for AJAX requests)
        [HttpPost]
        public async Task<IActionResult> SurveyList(int draw = 0, int start = 0, int length = 0,
            string clientId = "", string linkId = "", string finalplanId = "")
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null) return Challenge();
                var surveys = await _surveyService.GetAllSurveysAsync(currentUser, start, length);
                int surveycount = await _surveyService.CountSurveyAsync(currentUser);

                var data = new List<object>();

                int sl = start + 1;

                foreach (var d in surveys)
                {
                    var str = new List<string>();

                    str.Add(sl.ToString());
                    str.Add(d.Title);
                    str.Add(d.Questions.Count.ToString());
                    str.Add(d.FilledSurveys.Count.ToString());
                    str.Add(d.CreatedAt.ToString());
                    str.Add(LinkGenerationEngine.GetDetailsLink("Details", "Survey", d.Id, "") + LinkGenerationEngine.GetShareLink(d.Id, "generate sharable link") + LinkGenerationEngine.GetDeleteLink("Delete", "Survey", d.Id, "delete this survey permanantly"));

                    data.Add(str);
                    sl++;
                }
                return Json(new
                {
                    draw = draw,
                    recordsTotal = surveycount,
                    recordsFiltered = surveycount,
                    start = start,
                    length = length,
                    data = new List<object>()
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Json(new
                {
                    draw = draw,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    start = start,
                    length = length,
                    data = new List<object>()
                });
            }
        }

        // Displays survey creation form
        public IActionResult Create(SurveyCreateViewModel model)
        {
            int maxQuestions = 15;
            bool isInvalid = model == null
                || model.NumberOfQuestions <= 0
                || model.NumberOfQuestions > maxQuestions;

            if (isInvalid) return NotFound();
            List<Center> centers = _surveyService.LoadCenters();
            ViewBag.Centre = new SelectList(centers, "Id", "Name");

            ViewBag.Numbers = model;
            return View();
        }

        // Handles survey creation form submission and saves survey data
        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost([Bind("Id", "Title", "Questions")] SurveyViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    string messages = string.Join("; ", ModelState.Values
                                                  .SelectMany(x => x.Errors)
                                                  .Select(x => x.ErrorMessage));
                    return RedirectToAction("Create");
                }

                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null) return Challenge();

                var isSuccessful = await _surveyService.CreateSurveyAsync(model, currentUser);

                if (!isSuccessful)
                {
                    return RedirectToAction("Create");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return RedirectToAction("Index");
        }

        // Handles survey creation with AJAX and returns a JSON response
        [HttpPost]
        public async Task<IActionResult> CreateSurvey(SurveyViewModel surveyData)
        {
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                             .SelectMany(x => x.Errors)
                                             .Select(x => x.ErrorMessage));
                return RedirectToAction("Create");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var isSuccessful = await _surveyService.CreateSurveyAsync(surveyData, currentUser);

            if (!isSuccessful)
            {
                return Json(new AjaxResponse() { IsSuccess = false, Message = "failed! please try again later" });
            }

            return Json(new AjaxResponse() { IsSuccess = true });
        }

        // Deletes a survey by ID for the logged-in user
        public async Task<IActionResult> Delete(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            if (id <= 0) return RedirectToAction("Index");

            var survey = await _surveyService.GetSurveyOfUserByIdAsync(id, currentUser);

            if (survey == null) return RedirectToAction("Index");

            await _surveyService.DeleteSurveyAsync(survey, currentUser);

            return RedirectToAction("Index");
        }

        // Retrieves survey details and displays them to the user
        public async Task<IActionResult> Details(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            if (id <= 0) return RedirectToAction("Index");

            var survey = await _surveyService.GetSurveyOfUserByIdAsync(id, currentUser);

            if (survey == null) RedirectToAction("Index");

            var model = new SurveyViewModel()
            {
                Id = survey.Id,
                Title = survey.Title,
                CreatedAt = survey.CreatedAt,
                UpdatedAt = survey.UpdatedAt,
                Questions = survey.Questions,
                FilledSurveys = survey.FilledSurveys
            };

            return View(model);
        }

        // Handles survey response submission
        [AllowAnonymous]
        public async Task<IActionResult> Answer(int id, int type = 0, string message = "")
        {
            if (id <= 0) return RedirectToAction("Index", "Home");

            var survey = await _surveyService.GetSurveyByIdAsync(id);
            List<Center> centers = await _surveyService.LoadCentersAsync();
            ViewBag.Centre = new SelectList(centers, "Id", "Name");

            if (survey == null) RedirectToAction("Index", "Home");

            var surveyModel = new SurveyViewModel()
            {
                Id = survey.Id,
                Title = survey.Title,
                CreatedAt = survey.CreatedAt,
                UpdatedAt = survey.UpdatedAt,
                Questions = survey.Questions,
                FilledSurveys = survey.FilledSurveys
            };

            var filledSurveyModel = new FilledSurveyViewModel()
            {
                Survey = survey
            };

            if (!String.IsNullOrEmpty(message) && type == 1) ViewBag.ErrorMessage = message;
            if (!String.IsNullOrEmpty(message) && type == 2) ViewBag.SuccessMessage = message;
            var tupleModel = new Tuple<SurveyViewModel, FilledSurveyViewModel>(surveyModel, filledSurveyModel);

            return View(tupleModel);
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName("Answer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnswerPost(FilledSurveyViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    string messages = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                    return RedirectToAction("Answer");
                }

                var isSuccessful = await _surveyService.CreateFilledSurveyAsync(model);

                if (!isSuccessful)
                {
                    //show a error message here 
                    return RedirectToAction("SurveyCapture", "Survey", new { type = 1, id = model.SurveyId, message = "Sorry! Something went wrong. please try again." });
                }
                return RedirectToAction("SurveyCapture", "Survey", new { type = 2, id = model.SurveyId, message = "Success! Survey has been captured." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("SurveyCapture", "Survey", new { type = 1, id = model.SurveyId, message = "Sorry! Something went wrong. please try again." });
            }

        }

        // Displays the error view in case of unhandled exceptions
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        #region Report

        public ActionResult Report()
        {

            return View();
        }

        #endregion


        #region Version-2 Feedback Capture

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> SurveyCapture(int id, int type = 0, string message = "")
        
        {
            try
            {
                var questypeList = await _surveyService.LoadQuestionTypeAsync();
                if (id <= 0) return RedirectToAction("Index", "Home");
                var survey = await _surveyService.GetSurveyByIdAsync(id);
                if (survey == null) RedirectToAction("Index", "Home");

                List<Center> centers = await _surveyService.LoadCentersAsync();
                ViewBag.Centre = new SelectList(centers, "Id", "Name");

                var surveyModel = new SurveyViewModel()
                {
                    Id = survey.Id,
                    Title = survey.Title,
                    CreatedAt = survey.CreatedAt,
                    UpdatedAt = survey.UpdatedAt,
                    Questions = _surveyService.LoadQuestionByQuesTypeandSurveyId(survey.Id, new string[] { }),
                    FilledSurveys = survey.FilledSurveys,
                    QuestionTypes = questypeList
                };

                var filledSurveyModel = new FilledSurveyViewModel()
                {
                    Survey = survey
                };

                if (!String.IsNullOrEmpty(message) && type == 1) ViewBag.ErrorMessage = message;
                if (!String.IsNullOrEmpty(message) && type == 2) ViewBag.SuccessMessage = message;

                var tupleModel = new Tuple<SurveyViewModel, FilledSurveyViewModel>(surveyModel, filledSurveyModel);
                return View(tupleModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw e;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GetSurveyData(string[] selectedValues, int surveyId)
        {
            try
            {
                // Get only question and answer which is relevant to this survey and question type
                if (surveyId <= 0) return PartialView("");
                var survey = await _surveyService.GetSurveyByIdAsync(surveyId);
                if (survey == null) return PartialView("");

                var surveyModel = new SurveyViewModel()
                {
                    Id = survey.Id,
                    Title = survey.Title,
                    CreatedAt = survey.CreatedAt,
                    UpdatedAt = survey.UpdatedAt,
                    Questions = selectedValues.Length == 0 ? new List<Question>() : _surveyService.LoadQuestionByQuesTypeandSurveyId(surveyId, selectedValues),
                };

                var filledSurveyModel = new FilledSurveyViewModel()
                {
                    Survey = survey
                };

                var tupleModel = new Tuple<SurveyViewModel, FilledSurveyViewModel>(surveyModel, filledSurveyModel);

                return PartialView("_GetSurveyData", tupleModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw e;
            }
        }

        #endregion

        #region Report 

        public async Task<FileStreamResult> Download(int id)
        {
            var survey = await _surveyService.GetSurveyByIdAsync(id);
            return await _surveyService.DownloadReport(survey.Id);
        }

        #endregion
    }
}
