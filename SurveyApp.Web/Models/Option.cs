using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyApp.Web.Models
{
    public class Option
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public bool IsTextValue { get; set; } = false;
        public int Rank { get; set; }

        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }

        public virtual List<FilledSurveyOption> FilledSurveyOptions { get; set; }
    }
}
