using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels
{
    public class CampaignProfileProductsServiceFormModel : QuestionOptionsModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileProductsServiceFormModel"/> class.
        /// </summary>
        public CampaignProfileProductsServiceFormModel()
        {
            FoodQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            SweetSaltySnacksQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            AlcoholicDrinksQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            NonAlcoholicDrinksQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            HouseholdproductsQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            ToiletriesCosmeticsQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            PharmaceuticalChemistsProductsQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            TobaccoProductsQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            PetsPetFoodQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            ShoppingRetailClothingQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            DIYGardeningQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            AppliancesOtherHouseholdDurablesQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            ElectronicsOtherPersonalItemsQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            CommunicationsInternetQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            FinancialServicesQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            HolidaysTravelQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            SportsLeisureQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
            MotoringQuestion =
                CompileQuestions(new Dictionary<string, bool>
                                     {{"Don't Know", true}, {"Never", false}, {"Rarely", false}, {"Regular", false}});
        }

        /// <summary>
        /// Gets or sets the campaign profile products services identifier.
        /// </summary>
        /// <value>The campaign profile products services identifier.</value>
        public int CampaignProfileProductsServicesId { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile identifier.
        /// </summary>
        /// <value>The campaign profile identifier.</value>
        public int CampaignProfileId { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile.
        /// </summary>
        /// <value>The campaign profile.</value>
        public CampaignProfileFormModel CampaignProfile { get; set; }

        /// <summary>
        /// Gets or sets the food question.
        /// </summary>
        /// <value>The food question.</value>
        // [Display(Name = "Food")]
        public List<QuestionOptionModel> FoodQuestion { get; set; }

        /// <summary>
        /// Gets or sets the food.
        /// </summary>
        /// <value>The food.</value>
        // [Display(Name = "Food")]
        public string Food_ProductsService
        {
            get
            {
                if (FoodQuestion == null)
                    FoodQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(FoodQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    FoodQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the sweet salty snacks question.
        /// </summary>
        /// <value>The sweet salty snacks question.</value>
        // [Display(Name = "Sweet/Salty Snacks")]
        public List<QuestionOptionModel> SweetSaltySnacksQuestion { get; set; }

        /// <summary>
        /// Gets or sets the sweet salty snacks.
        /// </summary>
        /// <value>The sweet salty snacks.</value>
        // [Display(Name = "Sweet/Salty Snacks")]
        public string SweetSaltySnacks_ProductsService
        {
            get
            {
                if (SweetSaltySnacksQuestion == null)
                    SweetSaltySnacksQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(SweetSaltySnacksQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    SweetSaltySnacksQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the alcoholic drinks question.
        /// </summary>
        /// <value>The alcoholic drinks question.</value>
        // [Display(Name = "Alcoholic Drinks")]
        public List<QuestionOptionModel> AlcoholicDrinksQuestion { get; set; }

        /// <summary>
        /// Gets or sets the alcoholic drinks.
        /// </summary>
        /// <value>The alcoholic drinks.</value>
        // [Display(Name = "Alcoholic Drinks")]
        public string AlcoholicDrinks_ProductsService
        {
            get
            {
                if (AlcoholicDrinksQuestion == null)
                    AlcoholicDrinksQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(AlcoholicDrinksQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    AlcoholicDrinksQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the non alcoholic drinks question.
        /// </summary>
        /// <value>The non alcoholic drinks question.</value>
        // [Display(Name = "Non-Alcoholic Drinks")]
        public List<QuestionOptionModel> NonAlcoholicDrinksQuestion { get; set; }

        /// <summary>
        /// Gets or sets the non alcoholic drinks.
        /// </summary>
        /// <value>The non alcoholic drinks.</value>
        // [Display(Name = "Non-Alcoholic Drinks")]
        public string NonAlcoholicDrinks_ProductsService
        {
            get
            {
                if (NonAlcoholicDrinksQuestion == null)
                    NonAlcoholicDrinksQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(NonAlcoholicDrinksQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    NonAlcoholicDrinksQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the householdproducts question.
        /// </summary>
        /// <value>The householdproducts question.</value>
        // [Display(Name = "Household Products")]
        public List<QuestionOptionModel> HouseholdproductsQuestion { get; set; }

        /// <summary>
        /// Gets or sets the householdproducts.
        /// </summary>
        /// <value>The householdproducts.</value>
        // [Display(Name = "Household Products")]
        public string Householdproducts_ProductsService
        {
            get
            {
                if (HouseholdproductsQuestion == null)
                    HouseholdproductsQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(HouseholdproductsQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    HouseholdproductsQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the toiletries cosmetics question.
        /// </summary>
        /// <value>The toiletries cosmetics question.</value>
        // [Display(Name = "Toiletries/Cosmetics")]
        public List<QuestionOptionModel> ToiletriesCosmeticsQuestion { get; set; }

        /// <summary>
        /// Gets or sets the toiletries cosmetics.
        /// </summary>
        /// <value>The toiletries cosmetics.</value>
        // [Display(Name = "Toiletries/Cosmetics")]
        public string ToiletriesCosmetics_ProductsService
        {
            get
            {
                if (ToiletriesCosmeticsQuestion == null)
                    ToiletriesCosmeticsQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(ToiletriesCosmeticsQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    ToiletriesCosmeticsQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the pharmaceutical chemists products question.
        /// </summary>
        /// <value>The pharmaceutical chemists products question.</value>
        // [Display(Name = "Pharmaceutical/Chemists Products")]
        public List<QuestionOptionModel> PharmaceuticalChemistsProductsQuestion { get; set; }

        /// <summary>
        /// Gets or sets the pharmaceutical chemists products.
        /// </summary>
        /// <value>The pharmaceutical chemists products.</value>
        // [Display(Name = "Pharmaceutical/Chemists Products")]
        public string PharmaceuticalChemistsProducts_ProductsService
        {
            get
            {
                if (PharmaceuticalChemistsProductsQuestion == null)
                    PharmaceuticalChemistsProductsQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(PharmaceuticalChemistsProductsQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    PharmaceuticalChemistsProductsQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected
                        = true;
            }
        }


        /// <summary>
        /// Gets or sets the tobacco products question.
        /// </summary>
        /// <value>The tobacco products question.</value>
        // [Display(Name = "Tobacco Products")]
        public List<QuestionOptionModel> TobaccoProductsQuestion { get; set; }

        /// <summary>
        /// Gets or sets the tobacco products.
        /// </summary>
        /// <value>The tobacco products.</value>
        // [Display(Name = "Tobacco Products")]
        public string TobaccoProducts_ProductsService
        {
            get
            {
                if (TobaccoProductsQuestion == null)
                    TobaccoProductsQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(TobaccoProductsQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    TobaccoProductsQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the pets pet food question.
        /// </summary>
        /// <value>The pets pet food question.</value>
        // [Display(Name = "Pet Food")]
        public List<QuestionOptionModel> PetsPetFoodQuestion { get; set; }

        /// <summary>
        /// Gets or sets the pets pet food.
        /// </summary>
        /// <value>The pets pet food.</value>
        // [Display(Name = "Pet Food")]
        public string PetsPetFood_ProductsService
        {
            get
            {
                if (PetsPetFoodQuestion == null)
                    PetsPetFoodQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(PetsPetFoodQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    PetsPetFoodQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the shopping retail clothing question.
        /// </summary>
        /// <value>The shopping retail clothing question.</value>
        // [Display(Name = "Shopping/Retail/Clothing")]
        public List<QuestionOptionModel> ShoppingRetailClothingQuestion { get; set; }

        /// <summary>
        /// Gets or sets the shopping retail clothing.
        /// </summary>
        /// <value>The shopping retail clothing.</value>
        // [Display(Name = "Shopping/Retail/Clothing")]
        public string ShoppingRetailClothing_ProductsService
        {
            get
            {
                if (ShoppingRetailClothingQuestion == null)
                    ShoppingRetailClothingQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(ShoppingRetailClothingQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    ShoppingRetailClothingQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the diy gardening question.
        /// </summary>
        /// <value>The diy gardening question.</value>
        // [Display(Name = "DIY/Gardening")]
        public List<QuestionOptionModel> DIYGardeningQuestion { get; set; }

        /// <summary>
        /// Gets or sets the diy gardening.
        /// </summary>
        /// <value>The diy gardening.</value>
        // [Display(Name = "DIY/Gardening")]
        public string DIYGardening_ProductsService
        {
            get
            {
                if (DIYGardeningQuestion == null)
                    DIYGardeningQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(DIYGardeningQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    DIYGardeningQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the appliances other household durables question.
        /// </summary>
        /// <value>The appliances other household durables question.</value>
        // [Display(Name = "Appliances/Other Household Durables")]
        public List<QuestionOptionModel> AppliancesOtherHouseholdDurablesQuestion { get; set; }

        /// <summary>
        /// Gets or sets the appliances other household durables.
        /// </summary>
        /// <value>The appliances other household durables.</value>
        // [Display(Name = "Appliances/Other Household Durables")]
        public string AppliancesOtherHouseholdDurables_ProductsService
        {
            get
            {
                if (AppliancesOtherHouseholdDurablesQuestion == null)
                    AppliancesOtherHouseholdDurablesQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(AppliancesOtherHouseholdDurablesQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    AppliancesOtherHouseholdDurablesQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).
                        Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the electronics other personal items question.
        /// </summary>
        /// <value>The electronics other personal items question.</value>
        // [Display(Name = "Electronics/Other Personal Items")]
        public List<QuestionOptionModel> ElectronicsOtherPersonalItemsQuestion { get; set; }

        /// <summary>
        /// Gets or sets the electronics other personal items.
        /// </summary>
        /// <value>The electronics other personal items.</value>
        // [Display(Name = "Electronics/Other Personal Items")]
        public string ElectronicsOtherPersonalItems_ProductsService
        {
            get
            {
                if (ElectronicsOtherPersonalItemsQuestion == null)
                    ElectronicsOtherPersonalItemsQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(ElectronicsOtherPersonalItemsQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    ElectronicsOtherPersonalItemsQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected =
                        true;
            }
        }


        /// <summary>
        /// Gets or sets the communications internet question.
        /// </summary>
        /// <value>The communications internet question.</value>
        // [Display(Name = "Communications/Internet")]
        public List<QuestionOptionModel> CommunicationsInternetQuestion { get; set; }

        /// <summary>
        /// Gets or sets the communications internet.
        /// </summary>
        /// <value>The communications internet.</value>
        // [Display(Name = "Communications/Internet")]
        public string CommunicationsInternet_ProductsService
        {
            get
            {
                if (CommunicationsInternetQuestion == null)
                    CommunicationsInternetQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(CommunicationsInternetQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    CommunicationsInternetQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the financial services question.
        /// </summary>
        /// <value>The financial services question.</value>
        // [Display(Name = "Financial Services")]
        public List<QuestionOptionModel> FinancialServicesQuestion { get; set; }

        /// <summary>
        /// Gets or sets the financial services.
        /// </summary>
        /// <value>The financial services.</value>
        // [Display(Name = "Financial Services")]
        public string FinancialServices_ProductsService
        {
            get
            {
                if (FinancialServicesQuestion == null)
                    FinancialServicesQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(FinancialServicesQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    FinancialServicesQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the holidays travel question.
        /// </summary>
        /// <value>The holidays travel question.</value>
        // [Display(Name = "Holidays/Travel")]
        public List<QuestionOptionModel> HolidaysTravelQuestion { get; set; }

        /// <summary>
        /// Gets or sets the holidays travel.
        /// </summary>
        /// <value>The holidays travel.</value>
        // [Display(Name = "Holidays/Travel")]
        public string HolidaysTravel_ProductsService
        {
            get
            {
                if (HolidaysTravelQuestion == null)
                    HolidaysTravelQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(HolidaysTravelQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    HolidaysTravelQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the sports leisure question.
        /// </summary>
        /// <value>The sports leisure question.</value>
        // [Display(Name = "Sports/Leisure")]
        public List<QuestionOptionModel> SportsLeisureQuestion { get; set; }

        /// <summary>
        /// Gets or sets the sports leisure.
        /// </summary>
        /// <value>The sports leisure.</value>
        // [Display(Name = "Sports/Leisure")]
        public string SportsLeisure_ProductsService
        {
            get
            {
                if (SportsLeisureQuestion == null)
                    SportsLeisureQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(SportsLeisureQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    SportsLeisureQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }


        /// <summary>
        /// Gets or sets the motoring question.
        /// </summary>
        /// <value>The motoring question.</value>
        // [Display(Name = "Motoring")]
        public List<QuestionOptionModel> MotoringQuestion { get; set; }

        /// <summary>
        /// Gets or sets the motoring.
        /// </summary>
        /// <value>The motoring.</value>
        // [Display(Name = "Motoring")]
        public string Motoring_ProductsService
        {
            get
            {
                if (MotoringQuestion == null)
                    MotoringQuestion = new List<QuestionOptionModel>();
                return CompileAnswers(SortList(MotoringQuestion));
            }
            set
            {
                if (value == null) return;
                for (int i = 0; i < value.Length; i++)
                    MotoringQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
            }
        }
    }
}