using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SurveyApp.Web.Models;
using SurveyApp.Web.Models.ViewModel;
using SurveyApp.Web.Services;

namespace SurveyApp.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SurveyService _surveyService;
        public HomeController(ILogger<HomeController> logger, SurveyService surveyService)
        {
            _logger = logger;
            _surveyService = surveyService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                ViewBag.totalFeedback = _surveyService.CountTotalFeedback();
                ViewBag.uniqueFeedback = _surveyService.CountUniqueFeedback();

                List<CentreSurveyViewModel> centrewiseSurveys = _surveyService.LoadCentreWiseSurveyCount();
                ViewBag.CentreSurveys = centrewiseSurveys;

                ViewBag.countfromKazipara = _surveyService.CountFromKazipara();
                ViewBag.countfromUttara = _surveyService.CountFromUttara();

                List<FeedbackViewModel> list = _surveyService.LoadLastFiveFeedback();

                ViewBag.feedbackList = list;
                ViewBag.todaysFeedback = _surveyService.CountTodaysFeedback();
            }
            catch (Exception ex) { _logger.LogError(ex.Message); }

            return View();
        }

        [HttpGet]
        public IActionResult FeedbackList()
        {
            List<FeedbackViewModel> list = _surveyService.LoadFeedback();

            return View(list);
        }

        [HttpGet]
        public IActionResult FeedbackDetail(int surveyId, string patientId, string surveycollectionDate)
        {
            return View(_surveyService.LoadFeedbackDetails(surveyId, patientId, surveycollectionDate));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
