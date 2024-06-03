using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyApp.Web.Models
{
    public class Question
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string ShortTitle { get; set; }

        public int SurveyId { get; set; }

        public virtual Survey Survey { get; set; }

        public virtual QuestionType QuestionType { get; set; }

        public virtual List<Option> Options { get; set; }
    }
}
