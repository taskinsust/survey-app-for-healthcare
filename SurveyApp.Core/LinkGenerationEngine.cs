using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SurveyApp.Core
{
    public static class LinkGenerationEngine
    {
        public static string GetDetailsLink(string action, string controller, long id, string tooltip = "")
        {
            var link = @" <a data-toggle='tooltip' title='" + tooltip + "' data-placement='bottom' asp-route-Id='" + id + "' asp-action= '" + action + "' asp-controller='" + controller + "' asp-area='' class='btn btn-outline-info btn-sm'> Details </a>";
            return link;
        }

        public static string GetShareLink(long id, string tooltip = "")
        {
            if (id <= 0) return "";
            var link = @"<button data-toggle='tooltip' title='" + tooltip + "' class='btn btn-outline-success btn-sm share-button' data-toggle='modal' data-target='#shareModal' onclick='shareSurvey('" + id + "')'> Share</button>";

            return link;
        }
        public static string GetDeleteLink(string action, string controller, long id, string tooltip = "")
        {
            var generateLink = @"<a data-toggle='tooltip' title='" + tooltip + "' asp-route-Id='" + id + "' asp-action='" + action + "' asp-controller='" + controller + "' asp-area='' class='btn btn-outline-danger btn-sm'>Delete</a>";

            return generateLink;
        }
    }
}
