using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SurveyApp.Web.Models;
using SurveyApp.Web.Services;

namespace SurveyApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SurveyService _surveyService;
        public HomeController(ILogger<HomeController> logger, SurveyService surveyService)
        {
            _logger = logger;
            _surveyService = surveyService;
        }

        public IActionResult Index()
        {
            ViewBag.totalFeedback = _surveyService.CountTotalFeedback();
            ViewBag.uniqueFeedback = _surveyService.CountUniqueFeedback();
            ViewBag.countfromKazipara = _surveyService.CountFromKazipara();
            ViewBag.countfromUttara = _surveyService.CountFromUttara();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
