using System.Collections.Generic;

namespace SurveyApp.Web.Models.ViewModel
{
    public class SurveyCollectionViewModel
    {
        public SurveyCollectionViewModel()
        {
            QuesTypesInt = new List<int>();
        }

        public Survey Survey { get; set; }
        public List<int> QuesTypesInt { get; set; }
        public virtual List<QuestionType> QuestionTypes { get; set; }

    }
}
