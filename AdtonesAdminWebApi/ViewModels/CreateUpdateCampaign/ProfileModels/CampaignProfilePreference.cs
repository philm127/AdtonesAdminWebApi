using System;
using AdtonesAdminWebApi.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels
{
    public class CampaignProfilePreference
    {
        public int Id { get; set; }

        public int CampaignProfileId { get; set; }


        public string Food_Advert { get; set; }
        public string SweetSaltySnacks_Advert { get; set; }

        public string AlcoholicDrinks_Advert { get; set; }

        public string NonAlcoholicDrinks_Advert { get; set; }

        public string Householdproducts_Advert { get; set; }

        public string ToiletriesCosmetics_Advert { get; set; }

        public string PharmaceuticalChemistsProducts_Advert { get; set; }

        public string TobaccoProducts_Advert { get; set; }

        public string PetsPetFood_Advert { get; set; }

        public string ShoppingRetailClothing_Advert { get; set; }

        public string DIYGardening_Advert { get; set; }

        public string AppliancesOtherHouseholdDurables_Advert { get; set; }

        public string ElectronicsOtherPersonalItems_Advert { get; set; }

        public string CommunicationsInternet_Advert { get; set; }

        public string FinancialServices_Advert { get; set; }

        public string HolidaysTravel_Advert { get; set; }

        public string SportsLeisure_Advert { get; set; }

        public string Motoring_Advert { get; set; }

        public string Newspapers_Advert { get; set; }

        public string Magazines_Advert { get; set; }

        public string TV_Advert { get; set; }

        public string Radio_Advert { get; set; }

        public string Cinema_Advert { get; set; }

        public string SocialNetworking_Advert { get; set; }

        public string GeneralUse_Advert { get; set; }

        public string Shopping_Advert { get; set; }

        public string Fitness_Advert { get; set; }

        public string Holidays_Advert { get; set; }

        public string Environment_Advert { get; set; }

        public string GoingOut_Advert { get; set; }

        public string FinancialProducts_Advert { get; set; }

        public string Religion_Advert { get; set; }

        public string Fashion_Advert { get; set; }

        public string Music_Advert { get; set; }

        public string Fitness_Attitude { get; set; }

        public string Holidays_Attitude { get; set; }

        public string Environment_Attitude { get; set; }

        public string GoingOut_Attitude { get; set; }

        public string FinancialStabiity_Attitude { get; set; }

        public string Religion_Attitude { get; set; }

        public string Fashion_Attitude { get; set; }

        public string Music_Attitude { get; set; }

        public string Cinema_Cinema { get; set; }

        public DateTime? DOBStart_Demographics { get; set; }

        public DateTime? DOBEnd_Demographics { get; set; }

        public string Gender_Demographics { get; set; }

        public string IncomeBracket_Demographics { get; set; }

        public string WorkingStatus_Demographics { get; set; }

        public string RelationshipStatus_Demographics { get; set; }
        public string Education_Demographics { get; set; }

        public string HouseholdStatus_Demographics { get; set; }

        public string Location_Demographics { get; set; }

        public string Age_Demographics { get; set; }

        public string SocialNetworking_Internet { get; set; }

        public string Video_Internet { get; set; }

        public string Research_Internet { get; set; }

        public string Auctions_Internet { get; set; }

        public string Shopping_Internet { get; set; }

        public string ContractType_Mobile { get; set; }

        public string Spend_Mobile { get; set; }

        public string Local_Press { get; set; }

        public string National_Press { get; set; }

        public string FreeNewpapers_Press { get; set; }

        public string Magazines_Press { get; set; }

        public string Food_ProductsService { get; set; }

        public string SweetSaltySnacks_ProductsService { get; set; }

        public string AlcoholicDrinks_ProductsService { get; set; }

        public string NonAlcoholicDrinks_ProductsService { get; set; }

        public string Householdproducts_ProductsService { get; set; }

        public string ToiletriesCosmetics_ProductsService { get; set; }

        public string PharmaceuticalChemistsProducts_ProductsService { get; set; }

        public string TobaccoProducts_ProductsService { get; set; }

        public string PetsPetFood_ProductsService { get; set; }

        public string ShoppingRetailClothing_ProductsService { get; set; }

        public string DIYGardening_ProductsService { get; set; }

        public string AppliancesOtherHouseholdDurables_ProductsService { get; set; }

        public string ElectronicsOtherPersonalItems_ProductsService { get; set; }

        public string CommunicationsInternet_ProductsService { get; set; }

        public string FinancialServices_ProductsService { get; set; }

        public string HolidaysTravel_ProductsService { get; set; }

        public string SportsLeisure_ProductsService { get; set; }

        public string Motoring_ProductsService { get; set; }

        public string National_Radio { get; set; }

        public string Local_Radio { get; set; }

        public string Music_Radio { get; set; }

        public string Sport_Radio { get; set; }

        public string Talk_Radio { get; set; }

        public string Satallite_TV { get; set; }

        public string Cable_TV { get; set; }

        public string Terrestrial_TV { get; set; }

        public string Internet_TV { get; set; }

        public string Postcode { get; set; }

        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        public string BusinessOrOpportunities_AdType { get; set; }
        public string Gambling_AdType { get; set; }
        public string Restaurants_AdType { get; set; }
        public string Insurance_AdType { get; set; }
        public string Furniture_AdType { get; set; }
        public string InformationTechnology_AdType { get; set; }
        public string Energy_AdType { get; set; }
        public string Supermarkets_AdType { get; set; }
        public string Healthcare_AdType { get; set; }
        public string JobsAndEducation_AdType { get; set; }
        public string Gifts_AdType { get; set; }
        public string AdvocacyOrLegal_AdType { get; set; }
        public string DatingAndPersonal_AdType { get; set; }
        public string RealEstate_AdType { get; set; }
        public string Games_AdType { get; set; }
        //public string SkizaProfile_AdType { get; set; }

        public string Hustlers_AdType { get; set; }
        public string Youth_AdType { get; set; }
        public string DiscerningProfessionals_AdType { get; set; }
        public string Mass_AdType { get; set; }

        public bool NextStatus { get; set; }

        public int? AdtoneServerCampaignProfilePrefId { get; set; }
        public virtual CampaignProfile CampaignProfile { get; set; }
    }

}
