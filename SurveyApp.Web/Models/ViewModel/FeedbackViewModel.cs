using System;

namespace SurveyApp.Web.Models.ViewModel
{
    public class FeedbackViewModel
    {
        //select fs.CreatedAt, fs.PatientId, c.Name, s.Title from FilledSurveys fs inner join Center c on c.Id = fs.CenterId inner join Surveys s on s.id = fs.SurveyId

        public DateTime CreatedAt { get; set; }
        public string PatientId { get; set; }
        public string CentreName { get; set; }
        public string Survey { get; set; }
        public int SurveyId { get; set; }
    }


}
