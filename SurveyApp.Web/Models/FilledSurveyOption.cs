using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyApp.Web.Models
{
    public class FilledSurveyOption
    {
        public int Id { get; set; }

        public int OptionId { get; set; }
        public virtual Option Option { get; set; }

        [NotMapped]
        public int QuestionNo { get; set; }
        public string Value { get; set; }

        public int FilledSurveyId { get; set; }
        public virtual FilledSurvey FilledSurvey { get; set; }

    }
}
