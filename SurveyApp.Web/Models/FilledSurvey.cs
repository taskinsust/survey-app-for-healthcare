using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyApp.Web.Models
{
    public class FilledSurvey
    {
        public int Id { get; set; }

        //[Required]
        //[EmailAddress]
        //public string Email { get; set; }

        [Required]
        public string PatientId { get; set; }
        public DateTime CreatedAt { get; set; }

        public int SurveyId { get; set; }
        public virtual Survey Survey { get; set; }
        public virtual Center Center { get; set; }

        public virtual List<FilledSurveyOption> FilledSurveyOptions { get; set; }
    }
}
