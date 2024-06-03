using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SurveyApp.Web.Models
{
    public class FilledSurveyViewModel
    {
        //[Required]
        //[EmailAddress]
        //public string Email { get; set; }

        [Required]
        public int CentreId { get; set; }

        [Required]
        public string PatientId { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required]
        public int SurveyId { get; set; }

        public Survey Survey { get; set; }

        public List<FilledSurveyOption> FilledSurveyOptions { get; set; }
    }
}
