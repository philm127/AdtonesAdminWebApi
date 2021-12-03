using AdtonesAdminWebApi.DAL;
using AdtonesAdminWebApi.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels.CreateUpdateCampaign.ProfileModels
{
    public class CampaignProfileGeographicFormModel : QuestionOptionsModel
    {
        // private readonly IUserMatchDAL _matchDAL;
        //public CampaignProfileGeographicFormModel(IUserMatchDAL matchDAL)
        //{
        //    //_matchDAL = matchDAL;

        //    LocationQuestion =
        //        CompileQuestions(new Dictionary<string, bool>
        //                             {{"Nairobi East", false}, {"Nairobi West", false}, {"Mt.Kenya", false}, {"Rift", false}, {"Western Nyanza", false}, {"Coast", false}});


        //}

        public CampaignProfileGeographicFormModel() { }

        public CampaignProfileGeographicFormModel(int CountryId, List<string> locationProfileLabel)
        {

            Dictionary<string, bool> location = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> locationlist = new List<Dictionary<string, bool>>();

            foreach (var item in locationProfileLabel)
            {
                location = new Dictionary<string, bool> { { item, false } };
                locationlist.Add(location);
            }
            LocationQuestion = CompileQuestionsDynamic(locationlist);


        }

        public string PostCode { get; set; }
        // [Required]
        public int CountryId { get; set; }
        // public string CountryName { get; set; }
        public int CampaignProfileId { get; set; }
        public int CampaignProfileGeographicId { get; set; }


        // [Display(Name = "Location")]
        public List<QuestionOptionModel> LocationQuestion { get; set; }

        // [Display(Name = "Location")]
        public string Location_Demographics { get; set; }
        //{
        //    get
        //    {
        //        if (LocationQuestion == null)
        //            LocationQuestion = new List<QuestionOptionModel>();

        //        return CompileAnswers(SortList(LocationQuestion));
        //    }
        //    set
        //    {
        //        if (LocationQuestion != null && LocationQuestion.Count() > 0)
        //        {
        //            if (value == null) return;

        //            for (int i = 0; i < value.Length; i++)
        //                LocationQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //}

        public bool Location { get; set; }

        
    }
}