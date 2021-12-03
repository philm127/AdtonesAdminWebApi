using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using UserProfile.Helpers;
using UserProfile.Models;

namespace UserProfile.Geographic
{
    public class CreateGeographicModels : ICreateGeographicModels
    {
        //private readonly IHttpContextAccessor _httpAccessor;

        //private readonly ICampaignDAL _campaignDAL;
        //private readonly ICreateUpdateCampaignDAL _createDAL;
        //private readonly IConnectionStringService _connService;
        //private readonly IPrematchProcess _matchProcess;
        //private readonly IUserMatchDAL _matchDAL;
        //private readonly IMapper _mapper;
        //public readonly IConfiguration _configuration;
        // ReturnResult result = new ReturnResult();


        public CreateGeographicModels(//IHttpContextAccessor httpAccessor, ICampaignDAL campaignDAL, IPrematchProcess matchProcess,
                                            //ICreateUpdateCampaignDAL createDAL, IConnectionStringService connService,
                                            //IUserMatchDAL matchDAL, IMapper mapper, IConfiguration configuration
            )
        {
            //_httpAccessor = httpAccessor;
            //_campaignDAL = campaignDAL;
            //_createDAL = createDAL;
            //_connService = connService;
            //_matchProcess = matchProcess;
            //_matchDAL = matchDAL;
            //_mapper = mapper;
            //_configuration = configuration;
        }


        #region GeoGraphic


        /// <summary>
        /// Gets a blank Profile Model
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public CampaignProfileGeographicFormModel GetGeographicModel(int countryId, int profileId = 0)
        {
            var locationProfileMatchId = _matchDAL.GetProfileMatchInformationId(countryId, "location").Result;
            var locationProfileLabel = _matchDAL.GetProfileMatchLabels(locationProfileMatchId).Result;
            CampaignProfileGeographicFormModel CampaignProfileGeo = new CampaignProfileGeographicFormModel(countryId, locationProfileLabel.ToList()) { CampaignProfileId = 0 };

            return CampaignProfileGeo;
        }


        public async Task<CampaignProfileGeographicFormModel> GetGeographicData(int profileId, CampaignProfilePreference CampaignProfileGeograph)
        {
            var campaignProfile = await _campaignDAL.GetCampaignProfileDetail(profileId);
            CampaignProfileGeographicFormModel CampaignProfileGeo = GetGeographicModel(campaignProfile.CountryId.Value);

            int CountryId = campaignProfile.CountryId.Value;

            if (campaignProfile != null)
            {
                if (CampaignProfileGeograph.CountryId == 0)
                {
                    CampaignProfileGeograph.CountryId = CountryId;
                }

                if (CampaignProfileGeograph.Location_Demographics != null && CampaignProfileGeograph.Location_Demographics.Length > 0)
                {

                    for (int i = 0; i < CampaignProfileGeograph.Location_Demographics.Length; i++)
                        CampaignProfileGeo.LocationQuestion.Find(x => x.QuestionValue == CampaignProfileGeograph.Location_Demographics.Substring(i, 1)).Selected = true;
                }

                CampaignProfileGeo.CampaignProfileGeographicId = CampaignProfileGeograph.Id;
                CampaignProfileGeo.CampaignProfileId = profileId;
                CampaignProfileGeo.CountryId = CountryId;
            }

            return CampaignProfileGeo;
        }


        public async Task<bool> SaveGeographicWizard(CampaignProfileGeographicFormModel model, string connString)
        {
            var compileSort = new CompileSortQuestions();
            try
            {
                var command = new CreateOrUpdateCampaignProfileGeographicCommand();

                command.CampaignProfileId = model.CampaignProfileId;
                command.CountryId = model.CountryId;
                command.PostCode = model.PostCode;
                command.Location_Demographics = compileSort.CompileAnswers(compileSort.SortList(model.LocationQuestion));

                var result = await _matchDAL.UpdateGeographicProfile(command, connString);

                var result2 = await _matchDAL.UpdateMatchCampaignGeographic(command, connString);
            }
            catch
            {
                throw;
            }
            return true;
        }



        #endregion



        ///// <summary>
        ///// Sorts the list.
        ///// </summary>
        ///// <param name="questionOptions">The question options.</param>
        ///// <returns>IEnumerable&lt;QuestionOptionModel&gt;.</returns>
        //internal IEnumerable<QuestionOptionModel> SortList(List<QuestionOptionModel> questionOptions)
        //{
        //    questionOptions.Sort((x, y) => String.Compare(x.QuestionValue, y.QuestionValue, StringComparison.Ordinal));
        //    return questionOptions;
        //}

        ///// <summary>
        ///// Compiles the answers.
        ///// </summary>
        ///// <param name="questionOptions">The question options.</param>
        ///// <returns>System.String.</returns>
        //internal string CompileAnswers(IEnumerable<QuestionOptionModel> questionOptions)
        //{
        //    IEnumerable<QuestionOptionModel> questionOptionModels = questionOptions as QuestionOptionModel[] ??
        //                                                            questionOptions.ToArray();

        //    string answers = questionOptionModels.Where(q => q.Selected).Aggregate(
        //        string.Empty,
        //        (current, q) => current + q.QuestionValue
        //        );

        //    if (string.IsNullOrEmpty(answers))
        //    {
        //        foreach (
        //            QuestionOptionModel questionOptionModel in
        //                questionOptionModels.Where(questionOptionModel => questionOptionModel.DefaultAnswer))
        //            answers = questionOptionModel.QuestionValue;
        //    }

        //    return answers;
        //}

    }
}
