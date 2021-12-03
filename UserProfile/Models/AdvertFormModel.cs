using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserProfile.Models
{
    public class AdvertFormModel
    {
        /// <summary>
        /// Gets or sets the advert identifier.
        /// </summary>
        /// <value>The advert identifier.</value>
        public int AdvertId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserId { get; set; }
        //[Required(ErrorMessage = "The Client field is required.")]
        public int? ClientId { get; set; }

        /// <summary>
        /// Gets or sets the name of the advert.
        /// </summary>
        /// <value>The name of the advert.</value>
        // [Display(Name = "Name")]
        public string AdvertName { get; set; }

        /// <summary>
        /// Gets or sets the advert description.
        /// </summary>
        /// <value>The advert description.</value>
        // [Display(Name = "Description")]
        public string AdvertDescription { get; set; }

        /// <summary>
        /// Gets or sets the brand.
        /// </summary>
        /// <value>The brand.</value>
        // [Display(Name = "Brand")]
        public string Brand { get; set; }

        /// <summary>
        /// Gets or sets the script.
        /// </summary>
        /// <value>
        /// The script.
        /// </value>
        public string Script { get; set; }

        /// <summary>
        /// Gets or sets the script file location.
        /// </summary>
        /// <value>
        /// The script file location.
        /// </value>
        public string ScriptFileLocation { get; set; }

        /// <summary>
        /// Gets or sets the media file location.
        /// </summary>
        /// <value>The media file location.</value>
        // [Display(Name = "File")]
        public string MediaFileLocation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [uploaded to media server].
        /// </summary>
        /// <value><c>true</c> if [uploaded to media server]; otherwise, <c>false</c>.</value>
        public bool UploadedToMediaServer { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>The created date time.</value>
        // [Display(Name = "Created Date/Time")]
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the updated date time.
        /// </summary>
        /// <value>The updated date time.</value>
        // [Display(Name = "Updated Date/Time")]
        public DateTime UpdatedDateTime { get; set; }

        public virtual Client Clients { get; set; }

        /// <summary>
        /// Gets or sets the campaign adverts.
        /// </summary>
        /// <value>The campaign adverts.</value>
        public virtual ICollection<CampaignAdvertFormModel> CampaignAdverts { get; set; }

        //[Required]
        public int Status { get; set; }

        public bool IsAdminApproval { get; set; }

        public int? CampaignProfileId { get; set; }

        public int? AdvertCategoryId { get; set; }

        public int CountryId { get; set; }

        public int? OperatorId { get; set; }

        public bool IsTermChecked { get; set; }
    }


    public class CampaignAdvertFormModel
    {
        /// <summary>
        /// Gets or sets the campaign advert identifier.
        /// </summary>
        /// <value>The campaign advert identifier.</value>
        public int CampaignAdvertId { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile identifier.
        /// </summary>
        /// <value>The campaign profile identifier.</value>
        public int CampaignProfileId { get; set; }

        /// <summary>
        /// Gets or sets the advert identifier.
        /// </summary>
        /// <value>The advert identifier.</value>
        public int AdvertId { get; set; }
        public bool NextStatus { get; set; }

        public int? AdtoneServerCampaignAdvertId { get; set; }

        /// <summary>
        /// Gets or sets the advert.
        /// </summary>
        /// <value>The advert.</value>
        public virtual AdvertFormModel Advert { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile.
        /// </summary>
        /// <value>The campaign profile.</value>
        public virtual CampaignProfileFormModel CampaignProfile { get; set; }
    }
}