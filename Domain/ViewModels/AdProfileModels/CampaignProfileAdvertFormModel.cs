//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Domain.ViewModels.AdProfilrModels
//{
//    public class CampaignProfileAdvertFormModel : ArtharFormModel
//    {
//        /// <summary>
//        /// Initializes a new instance of the <see cref="CampaignProfileAdvertFormModel"/> class.
//        /// </summary>
//        public CampaignProfileAdvertFormModel()
//        {
//            FoodQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            SweetSaltySnacksQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            AlcoholicDrinksQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            NonAlcoholicDrinksQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            HouseholdproductsQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            ToiletriesCosmeticsQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            PharmaceuticalChemistsProductsQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            TobaccoProductsQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            PetsPetFoodQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            ShoppingRetailClothingQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            DIYGardeningQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //AppliancesOtherHouseholdDurablesQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            ElectronicsOtherPersonalItemsQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            CommunicationsInternetQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            FinancialServicesQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            HolidaysTravelQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            SportsLeisureQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            MotoringQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            NewspapersQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //MagazinesQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            TVQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //RadioQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            CinemaQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            SocialNetworkingQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //GeneralUseQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            ShoppingQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            FitnessQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //HolidaysQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            EnvironmentQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            GoingOutQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //FinancialProductsQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            ReligionQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //FashionQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            MusicQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});


//            // New Added Question

//            BusinessOrOpportunitiesQuestion =
//              CompileQuestions(new Dictionary<string, bool>
//                                   {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            GamblingQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                       {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            RestaurantsQuestion =
//              CompileQuestions(new Dictionary<string, bool>
//                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            InsuranceQuestion =
//            CompileQuestions(new Dictionary<string, bool>
//                   {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            FurnitureQuestion =
//               CompileQuestions(new Dictionary<string, bool>
//                      {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            InformationTechnologyQuestion =
//               CompileQuestions(new Dictionary<string, bool>
//                      {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            EnergyQuestion =
//               CompileQuestions(new Dictionary<string, bool>
//                      {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            SupermarketsQuestion =
//              CompileQuestions(new Dictionary<string, bool>
//                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            HealthcareQuestion =
//                  CompileQuestions(new Dictionary<string, bool>
//                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            JobsAndEducationQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                       {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            GiftsQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                       {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            AdvocacyOrLegalQuestion =
//                CompileQuestions(new Dictionary<string, bool>
//                       {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            DatingAndPersonalQuestion =
//               CompileQuestions(new Dictionary<string, bool>
//                      {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            RealEstateQuestion =
//              CompileQuestions(new Dictionary<string, bool>
//                     {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            GamesQuestion =
//             CompileQuestions(new Dictionary<string, bool>
//                    {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            //SkizaProfileQuestion =
//            //CompileQuestions(new Dictionary<string, bool>
//            //       {{"Networked Youth", false}, {"Stable Hustler", false}, {"Savvy Loyalist", false},
//            //        {"Tween", false}, {"Hi-Pot students", true}, {"Prudent Young", false},
//            //        {"Young Flashers", false}, {"Mature trendies", true}, {"Settled Middle Mgmt", false},
//            //        {"Affluent Influencers", false}, {"Young cautious caller", true}, {"Toa Mpango", false},
//            //        {"Young progressive worker", false}, {"Older Toa Mpango", true}, {"Progressive worker", false}
//            //       });

//            HustlersQuestion =
//            CompileQuestions(new Dictionary<string, bool>
//                   {{"Networked Youth", false}, {"Stable Hustler", false}, {"Savvy Loyalist", false}});

//            YouthQuestion =
//           CompileQuestions(new Dictionary<string, bool>
//                  {{"Tween", false}, {"Hi-Pot students", true}, {"Prudent Young", false}});

//            DiscerningProfessionalsQuestion =
//           CompileQuestions(new Dictionary<string, bool>
//                  {{"Young Flashers", false}, {"Mature trendies", true}, {"Settled Middle Mgmt", false},{"Affluent Influencers", false}});

//            MassQuestion =
//           CompileQuestions(new Dictionary<string, bool>
//                  {{"Young cautious caller", true}, {"Toa Mpango", false},{"Young progressive worker", false}, {"Older Toa Mpango", true}, {"Progressive worker", false}});
//        }

//        public CampaignProfileAdvertFormModel(int CountryId)//int CountryId
//        {
//            EFMVCDataContex db = new EFMVCDataContex();

//            //Food
//            var foodProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Food".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var foodProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == foodProfileMatchId).ToList();

//            Dictionary<string, bool> food = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> foodlist = new List<Dictionary<string, bool>>();

//            foreach (var item in foodProfileLabel)
//            {
//                food = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                foodlist.Add(food);
//            }
//            FoodQuestion = CompileQuestionsDynamic(foodlist);
//            foreach (var item in FoodQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //SweetSaltySnacks
//            var sweetSaltySnacksProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Sweets/Snacks".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var sweetSaltySnacksProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == sweetSaltySnacksProfileMatchId).ToList();

//            Dictionary<string, bool> sweetSaltySnacks = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> sweetSaltySnackslist = new List<Dictionary<string, bool>>();

//            foreach (var item in sweetSaltySnacksProfileLabel)
//            {
//                sweetSaltySnacks = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                sweetSaltySnackslist.Add(sweetSaltySnacks);
//            }
//            SweetSaltySnacksQuestion = CompileQuestionsDynamic(sweetSaltySnackslist);
//            foreach (var item in SweetSaltySnacksQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //AlcoholicDrinks
//            var alcoholicDrinksProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Alcoholic Drinks".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var alcoholicDrinksProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == alcoholicDrinksProfileMatchId).ToList();

//            Dictionary<string, bool> alcoholicDrinks = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> alcoholicDrinkslist = new List<Dictionary<string, bool>>();

//            foreach (var item in alcoholicDrinksProfileLabel)
//            {
//                alcoholicDrinks = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                alcoholicDrinkslist.Add(alcoholicDrinks);
//            }
//            AlcoholicDrinksQuestion = CompileQuestionsDynamic(alcoholicDrinkslist);
//            foreach (var item in AlcoholicDrinksQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //NonAlcoholicDrinks
//            var nonAlcoholicDrinksProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Non-Alcoholic Drinks".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var nonAlcoholicDrinksProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == nonAlcoholicDrinksProfileMatchId).ToList();

//            Dictionary<string, bool> nonAlcoholicDrinks = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> nonAlcoholicDrinkslist = new List<Dictionary<string, bool>>();

//            foreach (var item in nonAlcoholicDrinksProfileLabel)
//            {
//                nonAlcoholicDrinks = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                nonAlcoholicDrinkslist.Add(nonAlcoholicDrinks);
//            }
//            NonAlcoholicDrinksQuestion = CompileQuestionsDynamic(nonAlcoholicDrinkslist);
//            foreach (var item in NonAlcoholicDrinksQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //Householdproducts
//            var householdproductsProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Household Appliances/Products".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var householdproductsProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == householdproductsProfileMatchId).ToList();

//            Dictionary<string, bool> householdproducts = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> householdproductslist = new List<Dictionary<string, bool>>();

//            foreach (var item in householdproductsProfileLabel)
//            {
//                householdproducts = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                householdproductslist.Add(householdproducts);
//            }
//            HouseholdproductsQuestion = CompileQuestionsDynamic(householdproductslist);
//            foreach (var item in HouseholdproductsQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //ToiletriesCosmetics
//            var toiletriesCosmeticsProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Toiletries/Cosmetics".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var toiletriesCosmeticsProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == toiletriesCosmeticsProfileMatchId).ToList();

//            Dictionary<string, bool> toiletriesCosmetics = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> toiletriesCosmeticslist = new List<Dictionary<string, bool>>();

//            foreach (var item in toiletriesCosmeticsProfileLabel)
//            {
//                toiletriesCosmetics = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                toiletriesCosmeticslist.Add(toiletriesCosmetics);
//            }
//            ToiletriesCosmeticsQuestion = CompileQuestionsDynamic(toiletriesCosmeticslist);
//            foreach (var item in ToiletriesCosmeticsQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //PharmaceuticalChemistsProducts
//            var pharmaceuticalChemistsProductsProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Pharmaceutical/Chemists Products".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var pharmaceuticalChemistsProductsProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == pharmaceuticalChemistsProductsProfileMatchId).ToList();

//            Dictionary<string, bool> pharmaceuticalChemistsProducts = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> pharmaceuticalChemistsProductslist = new List<Dictionary<string, bool>>();

//            foreach (var item in pharmaceuticalChemistsProductsProfileLabel)
//            {
//                pharmaceuticalChemistsProducts = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                pharmaceuticalChemistsProductslist.Add(pharmaceuticalChemistsProducts);
//            }
//            PharmaceuticalChemistsProductsQuestion = CompileQuestionsDynamic(pharmaceuticalChemistsProductslist);
//            foreach (var item in PharmaceuticalChemistsProductsQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //TobaccoProducts
//            var tobaccoProductsProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Tobacco Products".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var tobaccoProductsProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == tobaccoProductsProfileMatchId).ToList();

//            Dictionary<string, bool> tobaccoProducts = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> tobaccoProductslist = new List<Dictionary<string, bool>>();

//            foreach (var item in tobaccoProductsProfileLabel)
//            {
//                tobaccoProducts = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                tobaccoProductslist.Add(tobaccoProducts);
//            }
//            TobaccoProductsQuestion = CompileQuestionsDynamic(tobaccoProductslist);
//            foreach (var item in TobaccoProductsQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //PetsPetFood
//            var petsPetFoodProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Pets".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var petsPetFoodProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == petsPetFoodProfileMatchId).ToList();

//            Dictionary<string, bool> petsPetFood = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> petsPetFoodlist = new List<Dictionary<string, bool>>();

//            foreach (var item in petsPetFoodProfileLabel)
//            {
//                petsPetFood = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                petsPetFoodlist.Add(petsPetFood);
//            }
//            PetsPetFoodQuestion = CompileQuestionsDynamic(petsPetFoodlist);
//            foreach (var item in PetsPetFoodQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //ShoppingRetailClothing
//            var shoppingRetailClothingProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Clothing/Fashion".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var shoppingRetailClothingProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == shoppingRetailClothingProfileMatchId).ToList();

//            Dictionary<string, bool> shoppingRetailClothing = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> shoppingRetailClothinglist = new List<Dictionary<string, bool>>();

//            foreach (var item in shoppingRetailClothingProfileLabel)
//            {
//                shoppingRetailClothing = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                shoppingRetailClothinglist.Add(shoppingRetailClothing);
//            }
//            ShoppingRetailClothingQuestion = CompileQuestionsDynamic(shoppingRetailClothinglist);
//            foreach (var item in ShoppingRetailClothingQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //DIYGardening
//            var dIYGardeningProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("DIY/Gardening".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var dIYGardeningProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == dIYGardeningProfileMatchId).ToList();

//            Dictionary<string, bool> dIYGardening = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> dIYGardeninglist = new List<Dictionary<string, bool>>();

//            foreach (var item in dIYGardeningProfileLabel)
//            {
//                dIYGardening = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                dIYGardeninglist.Add(dIYGardening);
//            }
//            DIYGardeningQuestion = CompileQuestionsDynamic(dIYGardeninglist);
//            foreach (var item in DIYGardeningQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //ElectronicsOtherPersonalItems
//            var electronicsOtherPersonalItemsProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Electronics/Other Personal Items".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var electronicsOtherPersonalItemsProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == electronicsOtherPersonalItemsProfileMatchId).ToList();

//            Dictionary<string, bool> electronicsOtherPersonalItems = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> electronicsOtherPersonalItemslist = new List<Dictionary<string, bool>>();

//            foreach (var item in electronicsOtherPersonalItemsProfileLabel)
//            {
//                electronicsOtherPersonalItems = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                electronicsOtherPersonalItemslist.Add(electronicsOtherPersonalItems);
//            }
//            ElectronicsOtherPersonalItemsQuestion = CompileQuestionsDynamic(electronicsOtherPersonalItemslist);
//            foreach (var item in ElectronicsOtherPersonalItemsQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //CommunicationsInternet
//            var communicationsInternetProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Communications/Internet Telecom".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var communicationsInternetProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == communicationsInternetProfileMatchId).ToList();

//            Dictionary<string, bool> communicationsInternet = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> communicationsInternetlist = new List<Dictionary<string, bool>>();

//            foreach (var item in communicationsInternetProfileLabel)
//            {
//                communicationsInternet = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                communicationsInternetlist.Add(communicationsInternet);
//            }
//            CommunicationsInternetQuestion = CompileQuestionsDynamic(communicationsInternetlist);
//            foreach (var item in CommunicationsInternetQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //FinancialServices
//            var financialServicesProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Financial Services".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var financialServicesProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == financialServicesProfileMatchId).ToList();

//            Dictionary<string, bool> financialServices = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> financialServiceslist = new List<Dictionary<string, bool>>();

//            foreach (var item in financialServicesProfileLabel)
//            {
//                financialServices = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                financialServiceslist.Add(financialServices);
//            }
//            FinancialServicesQuestion = CompileQuestionsDynamic(financialServiceslist);
//            foreach (var item in FinancialServicesQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //HolidaysTravel
//            var holidaysTravelProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Holidays/Travel Tourism".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var holidaysTravelProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == holidaysTravelProfileMatchId).ToList();

//            Dictionary<string, bool> holidaysTravel = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> holidaysTravellist = new List<Dictionary<string, bool>>();

//            foreach (var item in holidaysTravelProfileLabel)
//            {
//                holidaysTravel = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                holidaysTravellist.Add(holidaysTravel);
//            }
//            HolidaysTravelQuestion = CompileQuestionsDynamic(holidaysTravellist);
//            foreach (var item in HolidaysTravelQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //SportsLeisure
//            var sportsLeisureProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Sports/Leisure".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var sportsLeisureProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == sportsLeisureProfileMatchId).ToList();

//            Dictionary<string, bool> sportsLeisure = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> sportsLeisurelist = new List<Dictionary<string, bool>>();

//            foreach (var item in sportsLeisureProfileLabel)
//            {
//                sportsLeisure = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                sportsLeisurelist.Add(sportsLeisure);
//            }
//            SportsLeisureQuestion = CompileQuestionsDynamic(sportsLeisurelist);
//            foreach (var item in SportsLeisureQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //Motoring
//            var motoringProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Motoring/Automotive".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var motoringProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == motoringProfileMatchId).ToList();

//            Dictionary<string, bool> motoring = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> motoringlist = new List<Dictionary<string, bool>>();

//            foreach (var item in motoringProfileLabel)
//            {
//                motoring = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                motoringlist.Add(motoring);
//            }
//            MotoringQuestion = CompileQuestionsDynamic(motoringlist);
//            foreach (var item in MotoringQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //Newspapers
//            var newspapersProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Newspapers/Magazines".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var newspapersProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == newspapersProfileMatchId).ToList();

//            Dictionary<string, bool> newspapers = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> newspaperslist = new List<Dictionary<string, bool>>();

//            foreach (var item in newspapersProfileLabel)
//            {
//                newspapers = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                newspaperslist.Add(newspapers);
//            }
//            NewspapersQuestion = CompileQuestionsDynamic(newspaperslist);
//            foreach (var item in NewspapersQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //TV
//            var tVProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("TV/Video/ Radio".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var tVProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == tVProfileMatchId).ToList();

//            Dictionary<string, bool> tV = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> tVlist = new List<Dictionary<string, bool>>();

//            foreach (var item in tVProfileLabel)
//            {
//                tV = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                tVlist.Add(tV);
//            }
//            TVQuestion = CompileQuestionsDynamic(tVlist);
//            foreach (var item in TVQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //Cinema
//            var cinemaProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Cinema".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var cinemaProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == cinemaProfileMatchId).ToList();

//            Dictionary<string, bool> cinema = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> cinemalist = new List<Dictionary<string, bool>>();

//            foreach (var item in cinemaProfileLabel)
//            {
//                cinema = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                cinemalist.Add(cinema);
//            }
//            CinemaQuestion = CompileQuestionsDynamic(cinemalist);
//            foreach (var item in CinemaQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //SocialNetworking
//            var socialNetworkingProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Social Networking".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var socialNetworkingProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == socialNetworkingProfileMatchId).ToList();

//            Dictionary<string, bool> socialNetworking = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> socialNetworkinglist = new List<Dictionary<string, bool>>();

//            foreach (var item in socialNetworkingProfileLabel)
//            {
//                socialNetworking = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                socialNetworkinglist.Add(socialNetworking);
//            }
//            SocialNetworkingQuestion = CompileQuestionsDynamic(socialNetworkinglist);
//            foreach (var item in SocialNetworkingQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //Shopping
//            var shoppingProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Shopping(retail gen merc)".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var shoppingProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == shoppingProfileMatchId).ToList();

//            Dictionary<string, bool> shopping = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> shoppinglist = new List<Dictionary<string, bool>>();

//            foreach (var item in shoppingProfileLabel)
//            {
//                shopping = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                shoppinglist.Add(shopping);
//            }
//            ShoppingQuestion = CompileQuestionsDynamic(shoppinglist);
//            foreach (var item in ShoppingQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //Fitness
//            var fitnessProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Fitness".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var fitnessProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == fitnessProfileMatchId).ToList();

//            Dictionary<string, bool> fitness = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> fitnesslist = new List<Dictionary<string, bool>>();

//            foreach (var item in fitnessProfileLabel)
//            {
//                fitness = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                fitnesslist.Add(fitness);
//            }
//            FitnessQuestion = CompileQuestionsDynamic(fitnesslist);
//            foreach (var item in FitnessQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //Environment
//            var environmentProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Environment".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var environmentProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == environmentProfileMatchId).ToList();

//            Dictionary<string, bool> environment = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> environmentlist = new List<Dictionary<string, bool>>();

//            foreach (var item in environmentProfileLabel)
//            {
//                environment = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                environmentlist.Add(environment);
//            }
//            EnvironmentQuestion = CompileQuestionsDynamic(environmentlist);
//            foreach (var item in EnvironmentQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //GoingOut
//            var goingOutProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Going Out/Entertainment".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var goingOutProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == goingOutProfileMatchId).ToList();

//            Dictionary<string, bool> goingOut = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> goingOutlist = new List<Dictionary<string, bool>>();

//            foreach (var item in goingOutProfileLabel)
//            {
//                goingOut = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                goingOutlist.Add(goingOut);
//            }
//            GoingOutQuestion = CompileQuestionsDynamic(goingOutlist);
//            foreach (var item in GoingOutQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //Religion
//            var religionProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Religion".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var religionProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == religionProfileMatchId).ToList();

//            Dictionary<string, bool> religion = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> religionlist = new List<Dictionary<string, bool>>();

//            foreach (var item in religionProfileLabel)
//            {
//                religion = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                religionlist.Add(religion);
//            }
//            ReligionQuestion = CompileQuestionsDynamic(religionlist);
//            foreach (var item in ReligionQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //Music
//            var musicProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Music".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var musicProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == musicProfileMatchId).ToList();

//            Dictionary<string, bool> music = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> musiclist = new List<Dictionary<string, bool>>();

//            foreach (var item in musicProfileLabel)
//            {
//                music = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                musiclist.Add(music);
//            }
//            MusicQuestion = CompileQuestionsDynamic(musiclist);
//            foreach (var item in MusicQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //BusinessOrOpportunities
//            var businessOrOpportunitiesProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Business/opportunities".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var businessOrOpportunitiesProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == businessOrOpportunitiesProfileMatchId).ToList();

//            Dictionary<string, bool> businessOrOpportunities = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> businessOrOpportunitieslist = new List<Dictionary<string, bool>>();

//            foreach (var item in businessOrOpportunitiesProfileLabel)
//            {
//                businessOrOpportunities = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                businessOrOpportunitieslist.Add(businessOrOpportunities);
//            }
//            BusinessOrOpportunitiesQuestion = CompileQuestionsDynamic(businessOrOpportunitieslist);
//            foreach (var item in BusinessOrOpportunitiesQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //Gambling
//            var gamblingProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Over 18/Gambling".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var gamblingProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == gamblingProfileMatchId).ToList();

//            Dictionary<string, bool> gambling = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> gamblinglist = new List<Dictionary<string, bool>>();

//            foreach (var item in gamblingProfileLabel)
//            {
//                gambling = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                gamblinglist.Add(gambling);
//            }
//            GamblingQuestion = CompileQuestionsDynamic(gamblinglist);
//            foreach (var item in GamblingQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //Restaurants
//            var restaurantsProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Restaurants".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var restaurantsProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == restaurantsProfileMatchId).ToList();

//            Dictionary<string, bool> restaurants = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> restaurantslist = new List<Dictionary<string, bool>>();

//            foreach (var item in restaurantsProfileLabel)
//            {
//                restaurants = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                restaurantslist.Add(restaurants);
//            }
//            RestaurantsQuestion = CompileQuestionsDynamic(restaurantslist);
//            foreach (var item in RestaurantsQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //Insurance
//            var insuranceProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Insurance".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var insuranceProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == insuranceProfileMatchId).ToList();

//            Dictionary<string, bool> insurance = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> insurancelist = new List<Dictionary<string, bool>>();

//            foreach (var item in insuranceProfileLabel)
//            {
//                insurance = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                insurancelist.Add(insurance);
//            }
//            InsuranceQuestion = CompileQuestionsDynamic(insurancelist);
//            foreach (var item in InsuranceQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //Furniture
//            var furnitureProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Furniture".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var furnitureProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == furnitureProfileMatchId).ToList();

//            Dictionary<string, bool> furniture = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> furniturelist = new List<Dictionary<string, bool>>();

//            foreach (var item in furnitureProfileLabel)
//            {
//                furniture = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                furniturelist.Add(furniture);
//            }
//            FurnitureQuestion = CompileQuestionsDynamic(furniturelist);
//            foreach (var item in FurnitureQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //InformationTechnology
//            var informationTechnologyProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Information technology".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var informationTechnologyProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == informationTechnologyProfileMatchId).ToList();

//            Dictionary<string, bool> informationTechnology = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> informationTechnologylist = new List<Dictionary<string, bool>>();

//            foreach (var item in informationTechnologyProfileLabel)
//            {
//                informationTechnology = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                informationTechnologylist.Add(informationTechnology);
//            }
//            InformationTechnologyQuestion = CompileQuestionsDynamic(informationTechnologylist);
//            foreach (var item in InformationTechnologyQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //Energy
//            var energyProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Energy".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var energyProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == energyProfileMatchId).ToList();

//            Dictionary<string, bool> energy = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> energylist = new List<Dictionary<string, bool>>();

//            foreach (var item in energyProfileLabel)
//            {
//                energy = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                energylist.Add(energy);
//            }
//            EnergyQuestion = CompileQuestionsDynamic(energylist);
//            foreach (var item in EnergyQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //Supermarkets
//            var supermarketsProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Supermarkets".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var supermarketsProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == supermarketsProfileMatchId).ToList();

//            Dictionary<string, bool> supermarkets = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> supermarketslist = new List<Dictionary<string, bool>>();

//            foreach (var item in supermarketsProfileLabel)
//            {
//                supermarkets = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                supermarketslist.Add(supermarkets);
//            }
//            SupermarketsQuestion = CompileQuestionsDynamic(supermarketslist);
//            foreach (var item in SupermarketsQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //Healthcare
//            var healthcareProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Healthcare".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var healthcareProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == healthcareProfileMatchId).ToList();

//            Dictionary<string, bool> healthcare = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> healthcarelist = new List<Dictionary<string, bool>>();

//            foreach (var item in healthcareProfileLabel)
//            {
//                healthcare = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                healthcarelist.Add(healthcare);
//            }
//            HealthcareQuestion = CompileQuestionsDynamic(healthcarelist);
//            foreach (var item in HealthcareQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //JobsAndEducation
//            var jobsAndEducationProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Jobs and Education".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var jobsAndEducationProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == jobsAndEducationProfileMatchId).ToList();

//            Dictionary<string, bool> jobsAndEducation = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> jobsAndEducationlist = new List<Dictionary<string, bool>>();

//            foreach (var item in jobsAndEducationProfileLabel)
//            {
//                jobsAndEducation = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                jobsAndEducationlist.Add(jobsAndEducation);
//            }
//            JobsAndEducationQuestion = CompileQuestionsDynamic(jobsAndEducationlist);
//            foreach (var item in JobsAndEducationQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //Gifts
//            var giftsProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Gifts".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var giftsProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == giftsProfileMatchId).ToList();

//            Dictionary<string, bool> gifts = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> giftslist = new List<Dictionary<string, bool>>();

//            foreach (var item in giftsProfileLabel)
//            {
//                gifts = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                giftslist.Add(gifts);
//            }
//            GiftsQuestion = CompileQuestionsDynamic(giftslist);
//            foreach (var item in GiftsQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //AdvocacyOrLegal
//            var advocacyOrLegalProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Advocacy/Legal".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var advocacyOrLegalProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == advocacyOrLegalProfileMatchId).ToList();

//            Dictionary<string, bool> advocacyOrLegal = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> advocacyOrLegallist = new List<Dictionary<string, bool>>();

//            foreach (var item in advocacyOrLegalProfileLabel)
//            {
//                advocacyOrLegal = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                advocacyOrLegallist.Add(advocacyOrLegal);
//            }
//            AdvocacyOrLegalQuestion = CompileQuestionsDynamic(advocacyOrLegallist);
//            foreach (var item in AdvocacyOrLegalQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //DatingAndPersonal
//            var datingAndPersonalProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Dating & Personal".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var datingAndPersonalProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == datingAndPersonalProfileMatchId).ToList();

//            Dictionary<string, bool> datingAndPersonal = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> datingAndPersonallist = new List<Dictionary<string, bool>>();

//            foreach (var item in datingAndPersonalProfileLabel)
//            {
//                datingAndPersonal = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                datingAndPersonallist.Add(datingAndPersonal);
//            }
//            DatingAndPersonalQuestion = CompileQuestionsDynamic(datingAndPersonallist);
//            foreach (var item in DatingAndPersonalQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //RealEstate
//            var realEstateProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Real Estate/Property".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var realEstateProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == realEstateProfileMatchId).ToList();

//            Dictionary<string, bool> realEstate = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> realEstatelist = new List<Dictionary<string, bool>>();

//            foreach (var item in realEstateProfileLabel)
//            {
//                realEstate = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                realEstatelist.Add(realEstate);
//            }
//            RealEstateQuestion = CompileQuestionsDynamic(realEstatelist);
//            foreach (var item in RealEstateQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //Games
//            var gamesProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Games".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var gamesProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == gamesProfileMatchId).ToList();

//            Dictionary<string, bool> games = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> gameslist = new List<Dictionary<string, bool>>();

//            foreach (var item in gamesProfileLabel)
//            {
//                games = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                gameslist.Add(games);
//            }
//            GamesQuestion = CompileQuestionsDynamic(gameslist);
//            foreach (var item in GamesQuestion)
//            {
//                if (item.QuestionName == "Neutral")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //Hustlers
//            var hustlersProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Hustlers".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var hustlersProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == hustlersProfileMatchId).ToList();

//            Dictionary<string, bool> hustlers = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> hustlerslist = new List<Dictionary<string, bool>>();

//            foreach (var item in hustlersProfileLabel)
//            {
//                hustlers = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                hustlerslist.Add(hustlers);
//            }
//            HustlersQuestion = CompileQuestionsDynamic(hustlerslist);

//            //Youth
//            var youthProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Youth".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var youthProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == youthProfileMatchId).ToList();

//            Dictionary<string, bool> youth = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> youthlist = new List<Dictionary<string, bool>>();

//            foreach (var item in youthProfileLabel)
//            {
//                youth = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                youthlist.Add(youth);
//            }
//            YouthQuestion = CompileQuestionsDynamic(youthlist);
//            foreach (var item in YouthQuestion)
//            {
//                if (item.QuestionName == "Hi-Pot students")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //DiscerningProfessionals
//            var discerningProfessionalsProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Discerning Professionals".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var discerningProfessionalsProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == discerningProfessionalsProfileMatchId).ToList();

//            Dictionary<string, bool> discerningProfessionals = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> discerningProfessionalslist = new List<Dictionary<string, bool>>();

//            foreach (var item in discerningProfessionalsProfileLabel)
//            {
//                discerningProfessionals = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                discerningProfessionalslist.Add(discerningProfessionals);
//            }
//            DiscerningProfessionalsQuestion = CompileQuestionsDynamic(discerningProfessionalslist);
//            foreach (var item in DiscerningProfessionalsQuestion)
//            {
//                if (item.QuestionName == "Mature trendies")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //Mass
//            var massProfileMatchId = db.ProfileMatchInformation.Where(top => top.CountryId == CountryId && top.ProfileName.ToLower().Equals("Mass".ToLower()) && top.IsActive == true).Select(top => top.Id).FirstOrDefault();
//            var massProfileLabel = db.ProfileMatchLabel.Where(top => top.ProfileMatchInformationId == massProfileMatchId).ToList();

//            Dictionary<string, bool> mass = new Dictionary<string, bool>();
//            List<Dictionary<string, bool>> masslist = new List<Dictionary<string, bool>>();

//            foreach (var item in massProfileLabel)
//            {
//                mass = new Dictionary<string, bool> { { item.ProfileLabel, false } };
//                masslist.Add(mass);
//            }
//            MassQuestion = CompileQuestionsDynamic(masslist);
//            foreach (var item in MassQuestion)
//            {
//                if (item.QuestionName == "Young cautious caller")
//                {
//                    item.DefaultAnswer = true;
//                }
//                if (item.QuestionName == "Older Toa Mpango")
//                {
//                    item.DefaultAnswer = true;
//                }
//            }

//            //FoodQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //SweetSaltySnacksQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //AlcoholicDrinksQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //NonAlcoholicDrinksQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //HouseholdproductsQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //ToiletriesCosmeticsQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //PharmaceuticalChemistsProductsQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //TobaccoProductsQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //PetsPetFoodQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //ShoppingRetailClothingQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //DIYGardeningQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            ////AppliancesOtherHouseholdDurablesQuestion =
//            ////    CompileQuestions(new Dictionary<string, bool>
//            ////                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //ElectronicsOtherPersonalItemsQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //CommunicationsInternetQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //FinancialServicesQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //HolidaysTravelQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //SportsLeisureQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //MotoringQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //NewspapersQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            ////MagazinesQuestion =
//            ////    CompileQuestions(new Dictionary<string, bool>
//            ////                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //TVQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            ////RadioQuestion =
//            ////    CompileQuestions(new Dictionary<string, bool>
//            ////                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //CinemaQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //SocialNetworkingQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            ////GeneralUseQuestion =
//            ////    CompileQuestions(new Dictionary<string, bool>
//            ////                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //ShoppingQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //FitnessQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            ////HolidaysQuestion =
//            ////    CompileQuestions(new Dictionary<string, bool>
//            ////                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //EnvironmentQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //GoingOutQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            ////FinancialProductsQuestion =
//            ////    CompileQuestions(new Dictionary<string, bool>
//            ////                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //ReligionQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            ////FashionQuestion =
//            ////    CompileQuestions(new Dictionary<string, bool>
//            ////                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});
//            //MusicQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //                         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});


//            // New Added Question

//            //BusinessOrOpportunitiesQuestion =
//            //  CompileQuestions(new Dictionary<string, bool>
//            //                       {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            //GamblingQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //           {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            //RestaurantsQuestion =
//            //  CompileQuestions(new Dictionary<string, bool>
//            //         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            //InsuranceQuestion =
//            //CompileQuestions(new Dictionary<string, bool>
//            //       {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            //FurnitureQuestion =
//            //   CompileQuestions(new Dictionary<string, bool>
//            //          {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            //InformationTechnologyQuestion =
//            //   CompileQuestions(new Dictionary<string, bool>
//            //          {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            //EnergyQuestion =
//            //   CompileQuestions(new Dictionary<string, bool>
//            //          {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            //SupermarketsQuestion =
//            //  CompileQuestions(new Dictionary<string, bool>
//            //         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            //HealthcareQuestion =
//            //      CompileQuestions(new Dictionary<string, bool>
//            //             {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            //JobsAndEducationQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //           {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            //GiftsQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //           {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            //AdvocacyOrLegalQuestion =
//            //    CompileQuestions(new Dictionary<string, bool>
//            //           {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            //DatingAndPersonalQuestion =
//            //   CompileQuestions(new Dictionary<string, bool>
//            //          {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            //RealEstateQuestion =
//            //  CompileQuestions(new Dictionary<string, bool>
//            //         {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            //GamesQuestion =
//            // CompileQuestions(new Dictionary<string, bool>
//            //        {{"Not Important", false}, {"Neutral", true}, {"Important", false}});

//            // //SkizaProfileQuestion =
//            // //CompileQuestions(new Dictionary<string, bool>
//            // //       {{"Networked Youth", false}, {"Stable Hustler", false}, {"Savvy Loyalist", false},
//            // //        {"Tween", false}, {"Hi-Pot students", true}, {"Prudent Young", false},
//            // //        {"Young Flashers", false}, {"Mature trendies", true}, {"Settled Middle Mgmt", false},
//            // //        {"Affluent Influencers", false}, {"Young cautious caller", true}, {"Toa Mpango", false},
//            // //        {"Young progressive worker", false}, {"Older Toa Mpango", true}, {"Progressive worker", false}
//            // //       });

//            // HustlersQuestion =
//            // CompileQuestions(new Dictionary<string, bool>
//            //        {{"Networked Youth", false}, {"Stable Hustler", false}, {"Savvy Loyalist", false}});

//            // YouthQuestion =
//            //CompileQuestions(new Dictionary<string, bool>
//            //       {{"Tween", false}, {"Hi-Pot students", true}, {"Prudent Young", false}});

//            // DiscerningProfessionalsQuestion =
//            //CompileQuestions(new Dictionary<string, bool>
//            //       {{"Young Flashers", false}, {"Mature trendies", true}, {"Settled Middle Mgmt", false},{"Affluent Influencers", false}});

//            // MassQuestion =
//            //CompileQuestions(new Dictionary<string, bool>
//            //       {{"Young cautious caller", true}, {"Toa Mpango", false},{"Young progressive worker", false}, {"Older Toa Mpango", true}, {"Progressive worker", false}});
//        }

//        /// <summary>
//        /// Gets or sets the campaign profile adverts identifier.
//        /// </summary>
//        /// <value>The campaign profile adverts identifier.</value>
//        public int CampaignProfileAdvertsId { get; set; }

//        /// <summary>
//        /// Gets or sets the campaign profile identifier.
//        /// </summary>
//        /// <value>The campaign profile identifier.</value>
//        public int CampaignProfileId { get; set; }

//        /// <summary>
//        /// Gets or sets the campaign profile.
//        /// </summary>
//        /// <value>The campaign profile.</value>
//        public CampaignProfileFormModel CampaignProfile { get; set; }

//        /// <summary>
//        /// Gets or sets the food question.
//        /// </summary>
//        /// <value>The food question.</value>
//        [Display(Name = "Food")]
//        public List<QuestionOptionModel> FoodQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the food.
//        /// </summary>
//        /// <value>The food.</value>
//        [Display(Name = "Food")]
//        public string Food_Advert
//        {
//            get
//            {
//                if (FoodQuestion == null)
//                    FoodQuestion = new List<QuestionOptionModel>();

//                return CompileAnswers(SortList(FoodQuestion));
//            }
//            set
//            {
//                if (FoodQuestion != null && FoodQuestion.Count() > 0)
//                {
//                    if (value == null) return;

//                    for (int i = 0; i < value.Length; i++)
//                        FoodQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }

//        /// <summary>
//        /// Gets or sets the sweet salty snacks question.
//        /// </summary>
//        /// <value>The sweet salty snacks question.</value>
//        [Display(Name = "Sweets/Snacks")]
//        public List<QuestionOptionModel> SweetSaltySnacksQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the sweet salty snacks.
//        /// </summary>
//        /// <value>The sweet salty snacks.</value>
//        [Display(Name = "Sweets/Snacks")]
//        public string SweetSaltySnacks_Advert
//        {
//            get
//            {
//                if (SweetSaltySnacksQuestion == null)
//                    SweetSaltySnacksQuestion = new List<QuestionOptionModel>();

//                return CompileAnswers(SortList(SweetSaltySnacksQuestion));
//            }
//            set
//            {
//                if (SweetSaltySnacksQuestion != null && SweetSaltySnacksQuestion.Count() > 0)
//                {
//                    if (value == null) return;

//                    for (int i = 0; i < value.Length; i++)
//                        SweetSaltySnacksQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }

//        /// <summary>
//        /// Gets or sets the alcoholic drinks question.
//        /// </summary>
//        /// <value>The alcoholic drinks question.</value>
//        [Display(Name = "Alcoholic Drinks")]
//        public List<QuestionOptionModel> AlcoholicDrinksQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the alcoholic drinks.
//        /// </summary>
//        /// <value>The alcoholic drinks.</value>
//        [Display(Name = "Alcoholic Drinks")]
//        public string AlcoholicDrinks_Advert
//        {
//            get
//            {
//                if (AlcoholicDrinksQuestion == null)
//                    AlcoholicDrinksQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(AlcoholicDrinksQuestion));
//            }
//            set
//            {
//                if (AlcoholicDrinksQuestion != null && AlcoholicDrinksQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        AlcoholicDrinksQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        /// <summary>
//        /// Gets or sets the non alcoholic drinks question.
//        /// </summary>
//        /// <value>The non alcoholic drinks question.</value>
//        [Display(Name = "Non-Alcoholic Drinks")]
//        public List<QuestionOptionModel> NonAlcoholicDrinksQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the non alcoholic drinks.
//        /// </summary>
//        /// <value>The non alcoholic drinks.</value>
//        [Display(Name = "Non-Alcoholic Drinks")]
//        public string NonAlcoholicDrinks_Advert
//        {
//            get
//            {
//                if (NonAlcoholicDrinksQuestion == null)
//                    NonAlcoholicDrinksQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(NonAlcoholicDrinksQuestion));
//            }
//            set
//            {
//                if (NonAlcoholicDrinksQuestion != null && NonAlcoholicDrinksQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        NonAlcoholicDrinksQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }

//        /// <summary>
//        /// Gets or sets the householdproducts question.
//        /// </summary>
//        /// <value>The householdproducts question.</value>
//        [Display(Name = "Household Appliances/Products")]
//        public List<QuestionOptionModel> HouseholdproductsQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the householdproducts.
//        /// </summary>
//        /// <value>The householdproducts.</value>
//        [Display(Name = "Household Appliances/Products")]
//        public string Householdproducts_Advert
//        {
//            get
//            {
//                if (HouseholdproductsQuestion == null)
//                    HouseholdproductsQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(HouseholdproductsQuestion));
//            }
//            set
//            {
//                if (HouseholdproductsQuestion != null && HouseholdproductsQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        HouseholdproductsQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }

//        /// <summary>
//        /// Gets or sets the toiletries cosmetics question.
//        /// </summary>
//        /// <value>The toiletries cosmetics question.</value>
//        [Display(Name = "Toiletries/Cosmetics")]
//        public List<QuestionOptionModel> ToiletriesCosmeticsQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the toiletries cosmetics.
//        /// </summary>
//        /// <value>The toiletries cosmetics.</value>
//        [Display(Name = "Toiletries/Cosmetics")]
//        public string ToiletriesCosmetics_Advert
//        {
//            get
//            {
//                if (ToiletriesCosmeticsQuestion == null)
//                    ToiletriesCosmeticsQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(ToiletriesCosmeticsQuestion));
//            }
//            set
//            {
//                if (ToiletriesCosmeticsQuestion != null && ToiletriesCosmeticsQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        ToiletriesCosmeticsQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }

//        /// <summary>
//        /// Gets or sets the pharmaceutical chemists products question.
//        /// </summary>
//        /// <value>The pharmaceutical chemists products question.</value>
//        [Display(Name = "Pharmaceutical/Chemists Products")]
//        public List<QuestionOptionModel> PharmaceuticalChemistsProductsQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the pharmaceutical chemists products.
//        /// </summary>
//        /// <value>The pharmaceutical chemists products.</value>
//        [Display(Name = "Pharmaceutical/Chemists Products")]
//        public string PharmaceuticalChemistsProducts_Advert
//        {
//            get
//            {
//                if (PharmaceuticalChemistsProductsQuestion == null)
//                    PharmaceuticalChemistsProductsQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(PharmaceuticalChemistsProductsQuestion));
//            }
//            set
//            {
//                if (PharmaceuticalChemistsProductsQuestion != null && PharmaceuticalChemistsProductsQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        PharmaceuticalChemistsProductsQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }

//        /// <summary>
//        /// Gets or sets the tobacco products question.
//        /// </summary>
//        /// <value>The tobacco products question.</value>
//        [Display(Name = "Tobacco Products")]
//        public List<QuestionOptionModel> TobaccoProductsQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the tobacco products.
//        /// </summary>
//        /// <value>The tobacco products.</value>
//        [Display(Name = "Tobacco Products")]
//        public string TobaccoProducts_Advert
//        {
//            get
//            {
//                if (TobaccoProductsQuestion == null)
//                    TobaccoProductsQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(TobaccoProductsQuestion));
//            }
//            set
//            {
//                if (TobaccoProductsQuestion != null && TobaccoProductsQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        TobaccoProductsQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }

//        /// <summary>
//        /// Gets or sets the pets pet food question.
//        /// </summary>
//        /// <value>The pets pet food question.</value>
//        [Display(Name = "Pets")]
//        public List<QuestionOptionModel> PetsPetFoodQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the pets pet food.
//        /// </summary>
//        /// <value>The pets pet food.</value>
//        [Display(Name = "Pets")]
//        public string PetsPetFood_Advert
//        {
//            get
//            {
//                if (PetsPetFoodQuestion == null)
//                    PetsPetFoodQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(PetsPetFoodQuestion));
//            }
//            set
//            {
//                if (PetsPetFoodQuestion != null && PetsPetFoodQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        PetsPetFoodQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }

//        /// <summary>
//        /// Gets or sets the shopping retail clothing question.
//        /// </summary>
//        /// <value>The shopping retail clothing question.</value>
//        [Display(Name = "Clothing/Fashion")]
//        public List<QuestionOptionModel> ShoppingRetailClothingQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the shopping retail clothing.
//        /// </summary>
//        /// <value>The shopping retail clothing.</value>
//        [Display(Name = "Clothing/Fashion")]
//        public string ShoppingRetailClothing_Advert
//        {
//            get
//            {
//                if (ShoppingRetailClothingQuestion == null)
//                    ShoppingRetailClothingQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(ShoppingRetailClothingQuestion));
//            }
//            set
//            {
//                if (ShoppingRetailClothingQuestion != null && ShoppingRetailClothingQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        ShoppingRetailClothingQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }

//        /// <summary>
//        /// Gets or sets the diy gardening question.
//        /// </summary>
//        /// <value>The diy gardening question.</value>
//        [Display(Name = "DIY/Gardening")]
//        public List<QuestionOptionModel> DIYGardeningQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the diy gardening.
//        /// </summary>
//        /// <value>The diy gardening.</value>
//        [Display(Name = "DIY/Gardening")]
//        public string DIYGardening_Advert
//        {
//            get
//            {
//                if (DIYGardeningQuestion == null)
//                    DIYGardeningQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(DIYGardeningQuestion));
//            }
//            set
//            {
//                if (DIYGardeningQuestion != null && DIYGardeningQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        DIYGardeningQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }

//        /// <summary>
//        /// Gets or sets the appliances other household durables question.
//        /// </summary>
//        /// <value>The appliances other household durables question.</value>
//        //[Display(Name = "Appliances/Other Household Durables")]
//        //public List<QuestionOptionModel> AppliancesOtherHouseholdDurablesQuestion { get; set; }

//        ///// <summary>
//        ///// Gets or sets the appliances other household durables.
//        ///// </summary>
//        ///// <value>The appliances other household durables.</value>
//        //[Display(Name = "Appliances/Other Household Durables")]
//        //public string AppliancesOtherHouseholdDurables_Advert
//        //{
//        //    get
//        //    {
//        //        if (AppliancesOtherHouseholdDurablesQuestion == null)
//        //            AppliancesOtherHouseholdDurablesQuestion = new List<QuestionOptionModel>();
//        //        return CompileAnswers(SortList(AppliancesOtherHouseholdDurablesQuestion));
//        //    }
//        //    set
//        //    {
//        //        if (value == null) return;
//        //        for (int i = 0; i < value.Length; i++)
//        //            AppliancesOtherHouseholdDurablesQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).
//        //                Selected = true;
//        //    }
//        //}

//        /// <summary>
//        /// Gets or sets the electronics other personal items question.
//        /// </summary>
//        /// <value>The electronics other personal items question.</value>
//        [Display(Name = "Electronics/Other Personal Items")]
//        public List<QuestionOptionModel> ElectronicsOtherPersonalItemsQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the electronics other personal items.
//        /// </summary>
//        /// <value>The electronics other personal items.</value>
//        [Display(Name = "Electronics/Other Personal Items")]
//        public string ElectronicsOtherPersonalItems_Advert
//        {
//            get
//            {
//                if (ElectronicsOtherPersonalItemsQuestion == null)
//                    ElectronicsOtherPersonalItemsQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(ElectronicsOtherPersonalItemsQuestion));
//            }
//            set
//            {
//                if (ElectronicsOtherPersonalItemsQuestion != null && ElectronicsOtherPersonalItemsQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        ElectronicsOtherPersonalItemsQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        /// <summary>
//        /// Gets or sets the communications internet question.
//        /// </summary>
//        /// <value>The communications internet question.</value>
//        [Display(Name = "Communications/Internet Telecom")]
//        public List<QuestionOptionModel> CommunicationsInternetQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the communications internet.
//        /// </summary>
//        /// <value>The communications internet.</value>
//        [Display(Name = "Communications/Internet Telecom")]
//        public string CommunicationsInternet_Advert
//        {
//            get
//            {
//                if (CommunicationsInternetQuestion == null)
//                    CommunicationsInternetQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(CommunicationsInternetQuestion));
//            }
//            set
//            {
//                if (CommunicationsInternetQuestion != null && CommunicationsInternetQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        CommunicationsInternetQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        /// <summary>
//        /// Gets or sets the financial services question.
//        /// </summary>
//        /// <value>The financial services question.</value>
//        [Display(Name = "Financial Services")]
//        public List<QuestionOptionModel> FinancialServicesQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the financial services.
//        /// </summary>
//        /// <value>The financial services.</value>
//        [Display(Name = "Financial Services")]
//        public string FinancialServices_Advert
//        {
//            get
//            {
//                if (FinancialServicesQuestion == null)
//                    FinancialServicesQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(FinancialServicesQuestion));
//            }
//            set
//            {
//                if (FinancialServicesQuestion != null && FinancialServicesQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        FinancialServicesQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        /// <summary>
//        /// Gets or sets the holidays travel question.
//        /// </summary>
//        /// <value>The holidays travel question.</value>
//        [Display(Name = "Holidays/Travel Tourism")]
//        public List<QuestionOptionModel> HolidaysTravelQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the holidays travel.
//        /// </summary>
//        /// <value>The holidays travel.</value>
//        [Display(Name = "Holidays/Travel Tourism")]
//        public string HolidaysTravel_Advert
//        {
//            get
//            {
//                if (HolidaysTravelQuestion == null)
//                    HolidaysTravelQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(HolidaysTravelQuestion));
//            }
//            set
//            {
//                if (HolidaysTravelQuestion != null && HolidaysTravelQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        HolidaysTravelQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        /// <summary>
//        /// Gets or sets the sports leisure question.
//        /// </summary>
//        /// <value>The sports leisure question.</value>
//        [Display(Name = "Sports/Leisure")]
//        public List<QuestionOptionModel> SportsLeisureQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the sports leisure.
//        /// </summary>
//        /// <value>The sports leisure.</value>
//        [Display(Name = "Sports/Leisure")]
//        public string SportsLeisure_Advert
//        {
//            get
//            {
//                if (SportsLeisureQuestion == null)
//                    SportsLeisureQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(SportsLeisureQuestion));
//            }
//            set
//            {
//                if (SportsLeisureQuestion != null && SportsLeisureQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        SportsLeisureQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        /// <summary>
//        /// Gets or sets the motoring question.
//        /// </summary>
//        /// <value>The motoring question.</value>
//        [Display(Name = "Motoring/Automotive")]
//        public List<QuestionOptionModel> MotoringQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the motoring.
//        /// </summary>
//        /// <value>The motoring.</value>
//        [Display(Name = "Motoring/Automotive")]
//        public string Motoring_Advert
//        {
//            get
//            {
//                if (MotoringQuestion == null)
//                    MotoringQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(MotoringQuestion));
//            }
//            set
//            {
//                if (MotoringQuestion != null && MotoringQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        MotoringQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        /// <summary>
//        /// Gets or sets the newspapers question.
//        /// </summary>
//        /// <value>The newspapers question.</value>
//        [Display(Name = "Newspapers/Magazines")]
//        public List<QuestionOptionModel> NewspapersQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the newspapers.
//        /// </summary>
//        /// <value>The newspapers.</value>
//        [Display(Name = "Newspapers/Magazines")]
//        public string Newspapers_Advert
//        {
//            get
//            {
//                if (NewspapersQuestion == null)
//                    NewspapersQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(NewspapersQuestion));
//            }
//            set
//            {
//                if (NewspapersQuestion != null && NewspapersQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        NewspapersQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        /// <summary>
//        /// Gets or sets the magazines question.
//        /// </summary>
//        /// <value>The magazines question.</value>
//        //[Display(Name = "Magazines")]
//        //public List<QuestionOptionModel> MagazinesQuestion { get; set; }

//        ///// <summary>
//        ///// Gets or sets the magazines.
//        ///// </summary>
//        ///// <value>The magazines.</value>
//        //[Display(Name = "Magazines")]
//        //public string Magazines_Advert
//        //{
//        //    get
//        //    {
//        //        if (MagazinesQuestion == null)
//        //            MagazinesQuestion = new List<QuestionOptionModel>();
//        //        return CompileAnswers(SortList(MagazinesQuestion));
//        //    }
//        //    set
//        //    {
//        //        if (value == null) return;
//        //        for (int i = 0; i < value.Length; i++)
//        //            MagazinesQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//        //    }
//        //}


//        /// <summary>
//        /// Gets or sets the tv question.
//        /// </summary>
//        /// <value>The tv question.</value>
//        [Display(Name = "TV/Video/ Radio")]
//        public List<QuestionOptionModel> TVQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the tv.
//        /// </summary>
//        /// <value>The tv.</value>
//        [Display(Name = "TV/Video/Radio")]
//        public string TV_Advert
//        {
//            get
//            {
//                if (TVQuestion == null)
//                    TVQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(TVQuestion));
//            }
//            set
//            {
//                if (TVQuestion != null && TVQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        TVQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        /// <summary>
//        /// Gets or sets the radio question.
//        /// </summary>
//        /// <value>The radio question.</value>
//        //[Display(Name = "Radio")]
//        //public List<QuestionOptionModel> RadioQuestion { get; set; }

//        ///// <summary>
//        ///// Gets or sets the radio.
//        ///// </summary>
//        ///// <value>The radio.</value>
//        //[Display(Name = "Radio")]
//        //public string Radio_Advert
//        //{
//        //    get
//        //    {
//        //        if (RadioQuestion == null)
//        //            RadioQuestion = new List<QuestionOptionModel>();
//        //        return CompileAnswers(SortList(RadioQuestion));
//        //    }
//        //    set
//        //    {
//        //        if (value == null) return;
//        //        for (int i = 0; i < value.Length; i++)
//        //            RadioQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//        //    }
//        //}


//        /// <summary>
//        /// Gets or sets the cinema question.
//        /// </summary>
//        /// <value>The cinema question.</value>
//        [Display(Name = "Cinema")]
//        public List<QuestionOptionModel> CinemaQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the cinema.
//        /// </summary>
//        /// <value>The cinema.</value>
//        [Display(Name = "Cinema")]
//        public string Cinema_Advert
//        {
//            get
//            {
//                if (CinemaQuestion == null)
//                    CinemaQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(CinemaQuestion));
//            }
//            set
//            {
//                if (CinemaQuestion != null && CinemaQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        CinemaQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        /// <summary>
//        /// Gets or sets the social networking question.
//        /// </summary>
//        /// <value>The social networking question.</value>
//        [Display(Name = "Social Networking")]
//        public List<QuestionOptionModel> SocialNetworkingQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the social networking.
//        /// </summary>
//        /// <value>The social networking.</value>
//        [Display(Name = "Social Networking")]
//        public string SocialNetworking_Advert
//        {
//            get
//            {
//                if (SocialNetworkingQuestion == null)
//                    SocialNetworkingQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(SocialNetworkingQuestion));
//            }
//            set
//            {
//                if (SocialNetworkingQuestion != null && SocialNetworkingQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        SocialNetworkingQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        /// <summary>
//        /// Gets or sets the general use question.
//        /// </summary>
//        /// <value>The general use question.</value>
//        //[Display(Name = "General Use")]
//        //public List<QuestionOptionModel> GeneralUseQuestion { get; set; }

//        ///// <summary>
//        ///// Gets or sets the general use.
//        ///// </summary>
//        ///// <value>The general use.</value>
//        //[Display(Name = "General Use")]
//        //public string GeneralUse_Advert
//        //{
//        //    get
//        //    {
//        //        if (GeneralUseQuestion == null)
//        //            GeneralUseQuestion = new List<QuestionOptionModel>();
//        //        return CompileAnswers(SortList(GeneralUseQuestion));
//        //    }
//        //    set
//        //    {
//        //        if (value == null) return;
//        //        for (int i = 0; i < value.Length; i++)
//        //            GeneralUseQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//        //    }
//        //}


//        /// <summary>
//        /// Gets or sets the shopping question.
//        /// </summary>
//        /// <value>The shopping question.</value>
//        [Display(Name = "Shopping(retail gen merc)")]
//        public List<QuestionOptionModel> ShoppingQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the shopping.
//        /// </summary>
//        /// <value>The shopping.</value>
//        [Display(Name = "Shopping(retail gen merc)")]
//        public string Shopping_Advert
//        {
//            get
//            {
//                if (ShoppingQuestion == null)
//                    ShoppingQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(ShoppingQuestion));
//            }
//            set
//            {
//                if (ShoppingQuestion != null && ShoppingQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        ShoppingQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        /// <summary>
//        /// Gets or sets the fitness question.
//        /// </summary>
//        /// <value>The fitness question.</value>
//        [Display(Name = "Fitness")]
//        public List<QuestionOptionModel> FitnessQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the fitness.
//        /// </summary>
//        /// <value>The fitness.</value>
//        [Display(Name = "Fitness")]
//        public string Fitness_Advert
//        {
//            get
//            {
//                if (FitnessQuestion == null)
//                    FitnessQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(FitnessQuestion));
//            }
//            set
//            {
//                if (FitnessQuestion != null && FitnessQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        FitnessQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        /// <summary>
//        /// Gets or sets the holidays question.
//        /// </summary>
//        /// <value>The holidays question.</value>
//        //[Display(Name = "Holidays")]
//        //public List<QuestionOptionModel> HolidaysQuestion { get; set; }

//        ///// <summary>
//        ///// Gets or sets the holidays.
//        ///// </summary>
//        ///// <value>The holidays.</value>
//        //[Display(Name = "Holidays")]
//        //public string Holidays_Advert
//        //{
//        //    get
//        //    {
//        //        if (HolidaysQuestion == null)
//        //            HolidaysQuestion = new List<QuestionOptionModel>();
//        //        return CompileAnswers(SortList(HolidaysQuestion));
//        //    }
//        //    set
//        //    {
//        //        if (value == null) return;
//        //        for (int i = 0; i < value.Length; i++)
//        //            HolidaysQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//        //    }
//        //}


//        /// <summary>
//        /// Gets or sets the environment question.
//        /// </summary>
//        /// <value>The environment question.</value>
//        [Display(Name = "Environment")]
//        public List<QuestionOptionModel> EnvironmentQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the environment.
//        /// </summary>
//        /// <value>The environment.</value>
//        [Display(Name = "Environment")]
//        public string Environment_Advert
//        {
//            get
//            {
//                if (EnvironmentQuestion == null)
//                    EnvironmentQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(EnvironmentQuestion));
//            }
//            set
//            {
//                if (EnvironmentQuestion != null && EnvironmentQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        EnvironmentQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        /// <summary>
//        /// Gets or sets the going out question.
//        /// </summary>
//        /// <value>The going out question.</value>
//        [Display(Name = "Going Out/Entertainment")]
//        public List<QuestionOptionModel> GoingOutQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the going out.
//        /// </summary>
//        /// <value>The going out.</value>
//        [Display(Name = "Going Out/Entertainment")]
//        public string GoingOut_Advert
//        {
//            get
//            {
//                if (GoingOutQuestion == null)
//                    GoingOutQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(GoingOutQuestion));
//            }
//            set
//            {
//                if (GoingOutQuestion != null && GoingOutQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        GoingOutQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        /// <summary>
//        /// Gets or sets the financial products question.
//        /// </summary>
//        /// <value>The financial products question.</value>
//        //[Display(Name = "Financial Products")]
//        //public List<QuestionOptionModel> FinancialProductsQuestion { get; set; }

//        ///// <summary>
//        ///// Gets or sets the financial products.
//        ///// </summary>
//        ///// <value>The financial products.</value>
//        //[Display(Name = "Financial Products")]
//        //public string FinancialProducts_Advert
//        //{
//        //    get
//        //    {
//        //        if (FinancialProductsQuestion == null)
//        //            FinancialProductsQuestion = new List<QuestionOptionModel>();
//        //        return CompileAnswers(SortList(FinancialProductsQuestion));
//        //    }
//        //    set
//        //    {
//        //        if (value == null) return;
//        //        for (int i = 0; i < value.Length; i++)
//        //            FinancialProductsQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//        //    }
//        //}


//        /// <summary>
//        /// Gets or sets the religion question.
//        /// </summary>
//        /// <value>The religion question.</value>
//        [Display(Name = "Religion")]
//        public List<QuestionOptionModel> ReligionQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the religion.
//        /// </summary>
//        /// <value>The religion.</value>
//        [Display(Name = "Religion")]
//        public string Religion_Advert
//        {
//            get
//            {
//                if (ReligionQuestion == null)
//                    ReligionQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(ReligionQuestion));
//            }
//            set
//            {
//                if (ReligionQuestion != null && ReligionQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        ReligionQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        /// <summary>
//        /// Gets or sets the fashion question.
//        /// </summary>
//        /// <value>The fashion question.</value>
//        //[Display(Name = "Fashion")]
//        //public List<QuestionOptionModel> FashionQuestion { get; set; }

//        ///// <summary>
//        ///// Gets or sets the fashion.
//        ///// </summary>
//        ///// <value>The fashion.</value>
//        //[Display(Name = "Fashion")]
//        //public string Fashion_Advert
//        //{
//        //    get
//        //    {
//        //        if (FashionQuestion == null)
//        //            FashionQuestion = new List<QuestionOptionModel>();
//        //        return CompileAnswers(SortList(FashionQuestion));
//        //    }
//        //    set
//        //    {
//        //        if (value == null) return;
//        //        for (int i = 0; i < value.Length; i++)
//        //            FashionQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//        //    }
//        //}


//        /// <summary>
//        /// Gets or sets the music question.
//        /// </summary>
//        /// <value>The music question.</value>
//        [Display(Name = "Music")]
//        public List<QuestionOptionModel> MusicQuestion { get; set; }

//        /// <summary>
//        /// Gets or sets the music.
//        /// </summary>
//        /// <value>The music.</value>
//        [Display(Name = "Music")]
//        public string Music_Advert
//        {
//            get
//            {
//                if (MusicQuestion == null)
//                    MusicQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(MusicQuestion));
//            }
//            set
//            {
//                if (MusicQuestion != null && MusicQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        MusicQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }

//        // New Add Type
//        [Display(Name = "Business/opportunities")]
//        public List<QuestionOptionModel> BusinessOrOpportunitiesQuestion { get; set; }


//        [Display(Name = "Business/opportunities")]
//        public string BusinessOrOpportunities_AdType
//        {
//            get
//            {
//                if (BusinessOrOpportunitiesQuestion == null)
//                    BusinessOrOpportunitiesQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(BusinessOrOpportunitiesQuestion));
//            }
//            set
//            {
//                if (BusinessOrOpportunitiesQuestion != null && BusinessOrOpportunitiesQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        BusinessOrOpportunitiesQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }

//        [Display(Name = "Over 18/Gambling")]
//        public List<QuestionOptionModel> GamblingQuestion { get; set; }

//        [Display(Name = "Over 18/Gambling")]
//        public string Gambling_AdType
//        {
//            get
//            {
//                if (GamblingQuestion == null)
//                    GamblingQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(GamblingQuestion));
//            }
//            set
//            {
//                if (GamblingQuestion != null && GamblingQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        GamblingQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        [Display(Name = "Restaurants")]
//        public List<QuestionOptionModel> RestaurantsQuestion { get; set; }

//        [Display(Name = "Restaurants")]
//        public string Restaurants_AdType
//        {
//            get
//            {
//                if (RestaurantsQuestion == null)
//                    RestaurantsQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(RestaurantsQuestion));
//            }
//            set
//            {
//                if (RestaurantsQuestion != null && RestaurantsQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        RestaurantsQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }

//        [Display(Name = "Insurance")]
//        public List<QuestionOptionModel> InsuranceQuestion { get; set; }

//        [Display(Name = "Insurance")]
//        public string Insurance_AdType
//        {
//            get
//            {
//                if (InsuranceQuestion == null)
//                    InsuranceQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(InsuranceQuestion));
//            }
//            set
//            {
//                if (InsuranceQuestion != null && InsuranceQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        InsuranceQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        [Display(Name = "Furniture")]
//        public List<QuestionOptionModel> FurnitureQuestion { get; set; }

//        [Display(Name = "Furniture")]
//        public string Furniture_AdType
//        {
//            get
//            {
//                if (FurnitureQuestion == null)
//                    FurnitureQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(FurnitureQuestion));
//            }
//            set
//            {
//                if (FurnitureQuestion != null && FurnitureQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        FurnitureQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }

//        [Display(Name = "Information technology")]
//        public List<QuestionOptionModel> InformationTechnologyQuestion { get; set; }

//        [Display(Name = "Information technology")]
//        public string InformationTechnology_AdType
//        {
//            get
//            {
//                if (InformationTechnologyQuestion == null)
//                    InformationTechnologyQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(InformationTechnologyQuestion));
//            }
//            set
//            {
//                if (InformationTechnologyQuestion != null && InformationTechnologyQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        InformationTechnologyQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        [Display(Name = "Energy")]
//        public List<QuestionOptionModel> EnergyQuestion { get; set; }

//        [Display(Name = "Energy")]
//        public string Energy_AdType
//        {
//            get
//            {
//                if (EnergyQuestion == null)
//                    EnergyQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(EnergyQuestion));
//            }
//            set
//            {
//                if (EnergyQuestion != null && EnergyQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        EnergyQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        [Display(Name = "Supermarkets")]
//        public List<QuestionOptionModel> SupermarketsQuestion { get; set; }

//        [Display(Name = "Supermarkets")]
//        public string Supermarkets_AdType
//        {
//            get
//            {
//                if (SupermarketsQuestion == null)
//                    SupermarketsQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(SupermarketsQuestion));
//            }
//            set
//            {
//                if (SupermarketsQuestion != null && SupermarketsQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        SupermarketsQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }

//        [Display(Name = "Healthcare")]
//        public List<QuestionOptionModel> HealthcareQuestion { get; set; }

//        [Display(Name = "Healthcare")]
//        public string Healthcare_AdType
//        {
//            get
//            {
//                if (HealthcareQuestion == null)
//                    HealthcareQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(HealthcareQuestion));
//            }
//            set
//            {
//                if (HealthcareQuestion != null && HealthcareQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        HealthcareQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        [Display(Name = "Jobs and Education")]
//        public List<QuestionOptionModel> JobsAndEducationQuestion { get; set; }

//        [Display(Name = "Jobs and Education")]
//        public string JobsAndEducation_AdType
//        {
//            get
//            {
//                if (JobsAndEducationQuestion == null)
//                    JobsAndEducationQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(JobsAndEducationQuestion));
//            }
//            set
//            {
//                if (JobsAndEducationQuestion != null && JobsAndEducationQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        JobsAndEducationQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        [Display(Name = "Gifts")]
//        public List<QuestionOptionModel> GiftsQuestion { get; set; }

//        [Display(Name = "Gifts")]
//        public string Gifts_AdType
//        {
//            get
//            {
//                if (GiftsQuestion == null)
//                    GiftsQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(GiftsQuestion));
//            }
//            set
//            {
//                if (GiftsQuestion != null && GiftsQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        GiftsQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        [Display(Name = "Advocacy/Legal")]
//        public List<QuestionOptionModel> AdvocacyOrLegalQuestion { get; set; }

//        [Display(Name = "Advocacy/Legal")]
//        public string AdvocacyOrLegal_AdType
//        {
//            get
//            {
//                if (AdvocacyOrLegalQuestion == null)
//                    AdvocacyOrLegalQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(AdvocacyOrLegalQuestion));
//            }
//            set
//            {
//                if (AdvocacyOrLegalQuestion != null && AdvocacyOrLegalQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        AdvocacyOrLegalQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }

//        [Display(Name = "Dating & Personal")]
//        public List<QuestionOptionModel> DatingAndPersonalQuestion { get; set; }

//        [Display(Name = "Dating & Personal")]
//        public string DatingAndPersonal_AdType
//        {
//            get
//            {
//                if (DatingAndPersonalQuestion == null)
//                    DatingAndPersonalQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(DatingAndPersonalQuestion));
//            }
//            set
//            {
//                if (DatingAndPersonalQuestion != null && DatingAndPersonalQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        DatingAndPersonalQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        [Display(Name = "Real Estate/Property")]
//        public List<QuestionOptionModel> RealEstateQuestion { get; set; }

//        [Display(Name = "Real Estate/Property")]
//        public string RealEstate_AdType
//        {
//            get
//            {
//                if (RealEstateQuestion == null)
//                    RealEstateQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(RealEstateQuestion));
//            }
//            set
//            {
//                if (RealEstateQuestion != null && RealEstateQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        RealEstateQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }




//        [Display(Name = "Games")]
//        public List<QuestionOptionModel> GamesQuestion { get; set; }

//        [Display(Name = "Games")]
//        public string Games_AdType
//        {
//            get
//            {
//                if (GamesQuestion == null)
//                    GamesQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(GamesQuestion));
//            }
//            set
//            {
//                if (GamesQuestion != null && GamesQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        GamesQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        [Display(Name = "Hustlers")]
//        public List<QuestionOptionModel> HustlersQuestion { get; set; }

//        [Display(Name = "Hustlers")]
//        public string Hustlers_AdType
//        {
//            get
//            {
//                if (HustlersQuestion == null)
//                    HustlersQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(HustlersQuestion));
//            }
//            set
//            {
//                if (HustlersQuestion != null && HustlersQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        HustlersQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        [Display(Name = "Youth")]
//        public List<QuestionOptionModel> YouthQuestion { get; set; }

//        [Display(Name = "Youth")]
//        public string Youth_AdType
//        {
//            get
//            {
//                if (YouthQuestion == null)
//                    YouthQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(YouthQuestion));
//            }
//            set
//            {
//                if (YouthQuestion != null && YouthQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        YouthQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }

//        [Display(Name = "Discerning Professionals")]
//        public List<QuestionOptionModel> DiscerningProfessionalsQuestion { get; set; }

//        [Display(Name = "Discerning Professionals")]
//        public string DiscerningProfessionals_AdType
//        {
//            get
//            {
//                if (DiscerningProfessionalsQuestion == null)
//                    DiscerningProfessionalsQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(DiscerningProfessionalsQuestion));
//            }
//            set
//            {
//                if (DiscerningProfessionalsQuestion != null && DiscerningProfessionalsQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        DiscerningProfessionalsQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }

//        [Display(Name = "Mass")]
//        public List<QuestionOptionModel> MassQuestion { get; set; }

//        [Display(Name = "Mass")]
//        public string Mass_AdType
//        {
//            get
//            {
//                if (MassQuestion == null)
//                    MassQuestion = new List<QuestionOptionModel>();
//                return CompileAnswers(SortList(MassQuestion));
//            }
//            set
//            {
//                if (MassQuestion != null && MassQuestion.Count() > 0)
//                {
//                    if (value == null) return;
//                    for (int i = 0; i < value.Length; i++)
//                        MassQuestion.Find(x => x.QuestionValue == value.Substring(i, 1)).Selected = true;
//                }
//                else
//                {
//                    return;
//                }
//            }
//        }


//        #region Country Wise Hide Show Option
//        public bool Food { get; set; }
//        public bool SweetsSnacks { get; set; }
//        public bool AlcoholicDrinks { get; set; }
//        public bool NonAlcoholicDrinks { get; set; }
//        public bool HouseholdAppliancesProducts { get; set; }
//        public bool ToiletriesCosmetics { get; set; }
//        public bool PharmaceuticalChemistsProducts { get; set; }
//        public bool TobaccoProducts { get; set; }
//        public bool Pets { get; set; }
//        public bool ClothingFashion { get; set; }
//        public bool DIYGardening { get; set; }
//        public bool ElectronicsOtherPersonalItems { get; set; }
//        public bool CommunicationsInternetTelecom { get; set; }
//        public bool FinancialServices { get; set; }
//        public bool HolidaysTravelTourism { get; set; }
//        public bool SportsLeisure { get; set; }
//        public bool MotoringAutomotive { get; set; }
//        public bool NewspapersMagazines { get; set; }
//        public bool TvVideoRadio { get; set; }
//        public bool Cinema { get; set; }
//        public bool SocialNetworking { get; set; }
//        public bool Shopping { get; set; }
//        public bool Fitness { get; set; }
//        public bool Environment { get; set; }
//        public bool GoingOutEntertainment { get; set; }
//        public bool Religion { get; set; }
//        public bool Music { get; set; }
//        public bool BusinessOpportunities { get; set; }
//        public bool Over18Gambling { get; set; }
//        public bool Restaurants { get; set; }
//        public bool Insurance { get; set; }
//        public bool Furniture { get; set; }
//        public bool Informationtechnology { get; set; }
//        public bool Energy { get; set; }
//        public bool Supermarkets { get; set; }
//        public bool Healthcare { get; set; }
//        public bool JobsandEducation { get; set; }
//        public bool Gifts { get; set; }
//        public bool AdvocacyLegal { get; set; }
//        public bool DatingPersonal { get; set; }
//        public bool RealEstateProperty { get; set; }
//        public bool Games { get; set; }
//        public bool SkizaProfile { get; set; }
//        #endregion

//    }
//}