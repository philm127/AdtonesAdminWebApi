using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels
{
    public class CampaignProfileAdvertFormModel : QuestionOptionsModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignProfileAdvertFormModel"/> class.
        /// </summary>
        public CampaignProfileAdvertFormModel() { }
        

        public CampaignProfileAdvertFormModel(int CountryId, List<string> foodProfileLabel, List<string> sweetSaltySnacksProfileLabel, List<string> alcoholicDrinksProfileLabel,
                                                List<string> nonAlcoholicDrinksProfileLabel, List<string> householdproductsProfileLabel, List<string> toiletriesCosmeticsProfileLabel,
                                                List<string> pharmaceuticalChemistsProductsProfileLabel, List<string> tobaccoProductsProfileLabel,
                                                List<string> petsPetFoodProfileLabel, List<string> shoppingRetailClothingProfileLabel, List<string> dIYGardeningProfileLabel,
                                                List<string> electronicsOtherPersonalItemsProfileLabel, List<string> communicationsInternetProfileLabel,
                                                List<string> financialServicesProfileLabel, List<string> holidaysTravelProfileLabel, List<string> sportsLeisureProfileLabel,
                                                List<string> motoringProfileLabel, List<string> newspapersProfileLabel, List<string> tVProfileLabel, List<string> cinemaProfileLabel,
                                                List<string> socialNetworkingProfileLabel, List<string> shoppingProfileLabel, List<string> fitnessProfileLabel,
                                                List<string> environmentProfileLabel, List<string> goingOutProfileLabel, List<string> religionProfileLabel,
                                                List<string> musicProfileLabel, List<string> businessOrOpportunitiesProfileLabel, List<string> gamblingProfileLabel,
                                                List<string> restaurantsProfileLabel, List<string> insuranceProfileLabel, List<string> furnitureProfileLabel,
                                                List<string> informationTechnologyProfileLabel, List<string> energyProfileLabel, List<string> supermarketsProfileLabel,
                                                List<string> healthcareProfileLabel, List<string> jobsAndEducationProfileLabel, List<string> giftsProfileLabel,
                                                List<string> advocacyOrLegalProfileLabel, List<string> datingAndPersonalProfileLabel, List<string> realEstateProfileLabel, 
                                                List<string> gamesProfileLabel)
        {

            //Food
            Dictionary<string, bool> food = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> foodlist = new List<Dictionary<string, bool>>();

            foreach (var item in foodProfileLabel)
            {
                food = new Dictionary<string, bool> { { item, false } };
                foodlist.Add(food);
            }
            FoodQuestion = CompileQuestionsDynamic(foodlist);
            foreach (var item in FoodQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //SweetSaltySnacks
            Dictionary<string, bool> sweetSaltySnacks = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> sweetSaltySnackslist = new List<Dictionary<string, bool>>();

            foreach (var item in sweetSaltySnacksProfileLabel)
            {
                sweetSaltySnacks = new Dictionary<string, bool> { { item, false } };
                sweetSaltySnackslist.Add(sweetSaltySnacks);
            }
            SweetSaltySnacksQuestion = CompileQuestionsDynamic(sweetSaltySnackslist);
            foreach (var item in SweetSaltySnacksQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //AlcoholicDrinks
            Dictionary<string, bool> alcoholicDrinks = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> alcoholicDrinkslist = new List<Dictionary<string, bool>>();

            foreach (var item in alcoholicDrinksProfileLabel)
            {
                alcoholicDrinks = new Dictionary<string, bool> { { item, false } };
                alcoholicDrinkslist.Add(alcoholicDrinks);
            }
            AlcoholicDrinksQuestion = CompileQuestionsDynamic(alcoholicDrinkslist);
            foreach (var item in AlcoholicDrinksQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //NonAlcoholicDrinks
            Dictionary<string, bool> nonAlcoholicDrinks = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> nonAlcoholicDrinkslist = new List<Dictionary<string, bool>>();

            foreach (var item in nonAlcoholicDrinksProfileLabel)
            {
                nonAlcoholicDrinks = new Dictionary<string, bool> { { item, false } };
                nonAlcoholicDrinkslist.Add(nonAlcoholicDrinks);
            }
            NonAlcoholicDrinksQuestion = CompileQuestionsDynamic(nonAlcoholicDrinkslist);
            foreach (var item in NonAlcoholicDrinksQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //Householdproducts
            Dictionary<string, bool> householdproducts = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> householdproductslist = new List<Dictionary<string, bool>>();

            foreach (var item in householdproductsProfileLabel)
            {
                householdproducts = new Dictionary<string, bool> { { item, false } };
                householdproductslist.Add(householdproducts);
            }
            HouseholdproductsQuestion = CompileQuestionsDynamic(householdproductslist);
            foreach (var item in HouseholdproductsQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //ToiletriesCosmetics
            Dictionary<string, bool> toiletriesCosmetics = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> toiletriesCosmeticslist = new List<Dictionary<string, bool>>();

            foreach (var item in toiletriesCosmeticsProfileLabel)
            {
                toiletriesCosmetics = new Dictionary<string, bool> { { item, false } };
                toiletriesCosmeticslist.Add(toiletriesCosmetics);
            }
            ToiletriesCosmeticsQuestion = CompileQuestionsDynamic(toiletriesCosmeticslist);
            foreach (var item in ToiletriesCosmeticsQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //PharmaceuticalChemistsProducts
            Dictionary<string, bool> pharmaceuticalChemistsProducts = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> pharmaceuticalChemistsProductslist = new List<Dictionary<string, bool>>();

            foreach (var item in pharmaceuticalChemistsProductsProfileLabel)
            {
                pharmaceuticalChemistsProducts = new Dictionary<string, bool> { { item, false } };
                pharmaceuticalChemistsProductslist.Add(pharmaceuticalChemistsProducts);
            }
            PharmaceuticalChemistsProductsQuestion = CompileQuestionsDynamic(pharmaceuticalChemistsProductslist);
            foreach (var item in PharmaceuticalChemistsProductsQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //TobaccoProducts
            Dictionary<string, bool> tobaccoProducts = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> tobaccoProductslist = new List<Dictionary<string, bool>>();

            foreach (var item in tobaccoProductsProfileLabel)
            {
                tobaccoProducts = new Dictionary<string, bool> { { item, false } };
                tobaccoProductslist.Add(tobaccoProducts);
            }
            TobaccoProductsQuestion = CompileQuestionsDynamic(tobaccoProductslist);
            foreach (var item in TobaccoProductsQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //PetsPetFood
            Dictionary<string, bool> petsPetFood = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> petsPetFoodlist = new List<Dictionary<string, bool>>();

            foreach (var item in petsPetFoodProfileLabel)
            {
                petsPetFood = new Dictionary<string, bool> { { item, false } };
                petsPetFoodlist.Add(petsPetFood);
            }
            PetsPetFoodQuestion = CompileQuestionsDynamic(petsPetFoodlist);
            foreach (var item in PetsPetFoodQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //ShoppingRetailClothing
            Dictionary<string, bool> shoppingRetailClothing = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> shoppingRetailClothinglist = new List<Dictionary<string, bool>>();

            foreach (var item in shoppingRetailClothingProfileLabel)
            {
                shoppingRetailClothing = new Dictionary<string, bool> { { item, false } };
                shoppingRetailClothinglist.Add(shoppingRetailClothing);
            }
            ShoppingRetailClothingQuestion = CompileQuestionsDynamic(shoppingRetailClothinglist);
            foreach (var item in ShoppingRetailClothingQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //DIYGardening
            Dictionary<string, bool> dIYGardening = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> dIYGardeninglist = new List<Dictionary<string, bool>>();

            foreach (var item in dIYGardeningProfileLabel)
            {
                dIYGardening = new Dictionary<string, bool> { { item, false } };
                dIYGardeninglist.Add(dIYGardening);
            }
            DIYGardeningQuestion = CompileQuestionsDynamic(dIYGardeninglist);
            foreach (var item in DIYGardeningQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //ElectronicsOtherPersonalItems
            Dictionary<string, bool> electronicsOtherPersonalItems = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> electronicsOtherPersonalItemslist = new List<Dictionary<string, bool>>();

            foreach (var item in electronicsOtherPersonalItemsProfileLabel)
            {
                electronicsOtherPersonalItems = new Dictionary<string, bool> { { item, false } };
                electronicsOtherPersonalItemslist.Add(electronicsOtherPersonalItems);
            }
            ElectronicsOtherPersonalItemsQuestion = CompileQuestionsDynamic(electronicsOtherPersonalItemslist);
            foreach (var item in ElectronicsOtherPersonalItemsQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //CommunicationsInternet
            Dictionary<string, bool> communicationsInternet = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> communicationsInternetlist = new List<Dictionary<string, bool>>();

            foreach (var item in communicationsInternetProfileLabel)
            {
                communicationsInternet = new Dictionary<string, bool> { { item, false } };
                communicationsInternetlist.Add(communicationsInternet);
            }
            CommunicationsInternetQuestion = CompileQuestionsDynamic(communicationsInternetlist);
            foreach (var item in CommunicationsInternetQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //FinancialServices
            Dictionary<string, bool> financialServices = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> financialServiceslist = new List<Dictionary<string, bool>>();

            foreach (var item in financialServicesProfileLabel)
            {
                financialServices = new Dictionary<string, bool> { { item, false } };
                financialServiceslist.Add(financialServices);
            }
            FinancialServicesQuestion = CompileQuestionsDynamic(financialServiceslist);
            foreach (var item in FinancialServicesQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //HolidaysTravel
            Dictionary<string, bool> holidaysTravel = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> holidaysTravellist = new List<Dictionary<string, bool>>();

            foreach (var item in holidaysTravelProfileLabel)
            {
                holidaysTravel = new Dictionary<string, bool> { { item, false } };
                holidaysTravellist.Add(holidaysTravel);
            }
            HolidaysTravelQuestion = CompileQuestionsDynamic(holidaysTravellist);
            foreach (var item in HolidaysTravelQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //SportsLeisure
            Dictionary<string, bool> sportsLeisure = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> sportsLeisurelist = new List<Dictionary<string, bool>>();

            foreach (var item in sportsLeisureProfileLabel)
            {
                sportsLeisure = new Dictionary<string, bool> { { item, false } };
                sportsLeisurelist.Add(sportsLeisure);
            }
            SportsLeisureQuestion = CompileQuestionsDynamic(sportsLeisurelist);
            foreach (var item in SportsLeisureQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //Motoring
            Dictionary<string, bool> motoring = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> motoringlist = new List<Dictionary<string, bool>>();

            foreach (var item in motoringProfileLabel)
            {
                motoring = new Dictionary<string, bool> { { item, false } };
                motoringlist.Add(motoring);
            }
            MotoringQuestion = CompileQuestionsDynamic(motoringlist);
            foreach (var item in MotoringQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //Newspapers
            Dictionary<string, bool> newspapers = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> newspaperslist = new List<Dictionary<string, bool>>();

            foreach (var item in newspapersProfileLabel)
            {
                newspapers = new Dictionary<string, bool> { { item, false } };
                newspaperslist.Add(newspapers);
            }
            NewspapersQuestion = CompileQuestionsDynamic(newspaperslist);
            foreach (var item in NewspapersQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //TV
            Dictionary<string, bool> tV = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> tVlist = new List<Dictionary<string, bool>>();

            foreach (var item in tVProfileLabel)
            {
                tV = new Dictionary<string, bool> { { item, false } };
                tVlist.Add(tV);
            }
            TVQuestion = CompileQuestionsDynamic(tVlist);
            foreach (var item in TVQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //Cinema
            Dictionary<string, bool> cinema = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> cinemalist = new List<Dictionary<string, bool>>();

            foreach (var item in cinemaProfileLabel)
            {
                cinema = new Dictionary<string, bool> { { item, false } };
                cinemalist.Add(cinema);
            }
            CinemaQuestion = CompileQuestionsDynamic(cinemalist);
            foreach (var item in CinemaQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //SocialNetworking
            Dictionary<string, bool> socialNetworking = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> socialNetworkinglist = new List<Dictionary<string, bool>>();

            foreach (var item in socialNetworkingProfileLabel)
            {
                socialNetworking = new Dictionary<string, bool> { { item, false } };
                socialNetworkinglist.Add(socialNetworking);
            }
            SocialNetworkingQuestion = CompileQuestionsDynamic(socialNetworkinglist);
            foreach (var item in SocialNetworkingQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //Shopping
            Dictionary<string, bool> shopping = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> shoppinglist = new List<Dictionary<string, bool>>();

            foreach (var item in shoppingProfileLabel)
            {
                shopping = new Dictionary<string, bool> { { item, false } };
                shoppinglist.Add(shopping);
            }
            ShoppingQuestion = CompileQuestionsDynamic(shoppinglist);
            foreach (var item in ShoppingQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //Fitness
            Dictionary<string, bool> fitness = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> fitnesslist = new List<Dictionary<string, bool>>();

            foreach (var item in fitnessProfileLabel)
            {
                fitness = new Dictionary<string, bool> { { item, false } };
                fitnesslist.Add(fitness);
            }
            FitnessQuestion = CompileQuestionsDynamic(fitnesslist);
            foreach (var item in FitnessQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //Environment
            Dictionary<string, bool> environment = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> environmentlist = new List<Dictionary<string, bool>>();

            foreach (var item in environmentProfileLabel)
            {
                environment = new Dictionary<string, bool> { { item, false } };
                environmentlist.Add(environment);
            }
            EnvironmentQuestion = CompileQuestionsDynamic(environmentlist);
            foreach (var item in EnvironmentQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //GoingOut
            Dictionary<string, bool> goingOut = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> goingOutlist = new List<Dictionary<string, bool>>();

            foreach (var item in goingOutProfileLabel)
            {
                goingOut = new Dictionary<string, bool> { { item, false } };
                goingOutlist.Add(goingOut);
            }
            GoingOutQuestion = CompileQuestionsDynamic(goingOutlist);
            foreach (var item in GoingOutQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //Religion
            Dictionary<string, bool> religion = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> religionlist = new List<Dictionary<string, bool>>();

            foreach (var item in religionProfileLabel)
            {
                religion = new Dictionary<string, bool> { { item, false } };
                religionlist.Add(religion);
            }
            ReligionQuestion = CompileQuestionsDynamic(religionlist);
            foreach (var item in ReligionQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //Music
            Dictionary<string, bool> music = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> musiclist = new List<Dictionary<string, bool>>();

            foreach (var item in musicProfileLabel)
            {
                music = new Dictionary<string, bool> { { item, false } };
                musiclist.Add(music);
            }
            MusicQuestion = CompileQuestionsDynamic(musiclist);
            foreach (var item in MusicQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //BusinessOrOpportunities
            Dictionary<string, bool> businessOrOpportunities = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> businessOrOpportunitieslist = new List<Dictionary<string, bool>>();

            foreach (var item in businessOrOpportunitiesProfileLabel)
            {
                businessOrOpportunities = new Dictionary<string, bool> { { item, false } };
                businessOrOpportunitieslist.Add(businessOrOpportunities);
            }
            BusinessOrOpportunitiesQuestion = CompileQuestionsDynamic(businessOrOpportunitieslist);
            foreach (var item in BusinessOrOpportunitiesQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //Gambling
            Dictionary<string, bool> gambling = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> gamblinglist = new List<Dictionary<string, bool>>();

            foreach (var item in gamblingProfileLabel)
            {
                gambling = new Dictionary<string, bool> { { item, false } };
                gamblinglist.Add(gambling);
            }
            GamblingQuestion = CompileQuestionsDynamic(gamblinglist);
            foreach (var item in GamblingQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //Restaurants
            Dictionary<string, bool> restaurants = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> restaurantslist = new List<Dictionary<string, bool>>();

            foreach (var item in restaurantsProfileLabel)
            {
                restaurants = new Dictionary<string, bool> { { item, false } };
                restaurantslist.Add(restaurants);
            }
            RestaurantsQuestion = CompileQuestionsDynamic(restaurantslist);
            foreach (var item in RestaurantsQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //Insurance
            Dictionary<string, bool> insurance = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> insurancelist = new List<Dictionary<string, bool>>();

            foreach (var item in insuranceProfileLabel)
            {
                insurance = new Dictionary<string, bool> { { item, false } };
                insurancelist.Add(insurance);
            }
            InsuranceQuestion = CompileQuestionsDynamic(insurancelist);
            foreach (var item in InsuranceQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //Furniture
            Dictionary<string, bool> furniture = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> furniturelist = new List<Dictionary<string, bool>>();

            foreach (var item in furnitureProfileLabel)
            {
                furniture = new Dictionary<string, bool> { { item, false } };
                furniturelist.Add(furniture);
            }
            FurnitureQuestion = CompileQuestionsDynamic(furniturelist);
            foreach (var item in FurnitureQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //InformationTechnology
            Dictionary<string, bool> informationTechnology = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> informationTechnologylist = new List<Dictionary<string, bool>>();

            foreach (var item in informationTechnologyProfileLabel)
            {
                informationTechnology = new Dictionary<string, bool> { { item, false } };
                informationTechnologylist.Add(informationTechnology);
            }
            InformationTechnologyQuestion = CompileQuestionsDynamic(informationTechnologylist);
            foreach (var item in InformationTechnologyQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //Energy
            Dictionary<string, bool> energy = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> energylist = new List<Dictionary<string, bool>>();

            foreach (var item in energyProfileLabel)
            {
                energy = new Dictionary<string, bool> { { item, false } };
                energylist.Add(energy);
            }
            EnergyQuestion = CompileQuestionsDynamic(energylist);
            foreach (var item in EnergyQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //Supermarkets
            Dictionary<string, bool> supermarkets = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> supermarketslist = new List<Dictionary<string, bool>>();

            foreach (var item in supermarketsProfileLabel)
            {
                supermarkets = new Dictionary<string, bool> { { item, false } };
                supermarketslist.Add(supermarkets);
            }
            SupermarketsQuestion = CompileQuestionsDynamic(supermarketslist);
            foreach (var item in SupermarketsQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //Healthcare
            Dictionary<string, bool> healthcare = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> healthcarelist = new List<Dictionary<string, bool>>();

            foreach (var item in healthcareProfileLabel)
            {
                healthcare = new Dictionary<string, bool> { { item, false } };
                healthcarelist.Add(healthcare);
            }
            HealthcareQuestion = CompileQuestionsDynamic(healthcarelist);
            foreach (var item in HealthcareQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //JobsAndEducation
            Dictionary<string, bool> jobsAndEducation = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> jobsAndEducationlist = new List<Dictionary<string, bool>>();

            foreach (var item in jobsAndEducationProfileLabel)
            {
                jobsAndEducation = new Dictionary<string, bool> { { item, false } };
                jobsAndEducationlist.Add(jobsAndEducation);
            }
            JobsAndEducationQuestion = CompileQuestionsDynamic(jobsAndEducationlist);
            foreach (var item in JobsAndEducationQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //Gifts
            Dictionary<string, bool> gifts = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> giftslist = new List<Dictionary<string, bool>>();

            foreach (var item in giftsProfileLabel)
            {
                gifts = new Dictionary<string, bool> { { item, false } };
                giftslist.Add(gifts);
            }
            GiftsQuestion = CompileQuestionsDynamic(giftslist);
            foreach (var item in GiftsQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //AdvocacyOrLegal
            Dictionary<string, bool> advocacyOrLegal = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> advocacyOrLegallist = new List<Dictionary<string, bool>>();

            foreach (var item in advocacyOrLegalProfileLabel)
            {
                advocacyOrLegal = new Dictionary<string, bool> { { item, false } };
                advocacyOrLegallist.Add(advocacyOrLegal);
            }
            AdvocacyOrLegalQuestion = CompileQuestionsDynamic(advocacyOrLegallist);
            foreach (var item in AdvocacyOrLegalQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //DatingAndPersonal
            Dictionary<string, bool> datingAndPersonal = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> datingAndPersonallist = new List<Dictionary<string, bool>>();

            foreach (var item in datingAndPersonalProfileLabel)
            {
                datingAndPersonal = new Dictionary<string, bool> { { item, false } };
                datingAndPersonallist.Add(datingAndPersonal);
            }
            DatingAndPersonalQuestion = CompileQuestionsDynamic(datingAndPersonallist);
            foreach (var item in DatingAndPersonalQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //RealEstate
            Dictionary<string, bool> realEstate = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> realEstatelist = new List<Dictionary<string, bool>>();

            foreach (var item in realEstateProfileLabel)
            {
                realEstate = new Dictionary<string, bool> { { item, false } };
                realEstatelist.Add(realEstate);
            }
            RealEstateQuestion = CompileQuestionsDynamic(realEstatelist);
            foreach (var item in RealEstateQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            //Games
            Dictionary<string, bool> games = new Dictionary<string, bool>();
            List<Dictionary<string, bool>> gameslist = new List<Dictionary<string, bool>>();

            foreach (var item in gamesProfileLabel)
            {
                games = new Dictionary<string, bool> { { item, false } };
                gameslist.Add(games);
            }
            GamesQuestion = CompileQuestionsDynamic(gameslist);
            foreach (var item in GamesQuestion)
            {
                if (item.QuestionName == "Neutral")
                {
                    item.DefaultAnswer = true;
                }
            }

            

        }

            /// <summary>
            /// Gets or sets the campaign profile adverts identifier.
            /// </summary>
            /// <value>The campaign profile adverts identifier.</value>
        public int CampaignProfileAdvertsId { get; set; }

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
        // // [Display(Name = "Food")]
        public List<QuestionOptionModel> FoodQuestion { get; set; }

        /// <summary>
        /// Gets or sets the food.
        /// </summary>
        /// <value>The food.</value>
        //// [Display(Name = "Food")]
        public string Food_Advert { get; set; }
        

        /// <summary>
        /// Gets or sets the sweet salty snacks question.
        /// </summary>
        /// <value>The sweet salty snacks question.</value>
        //// [Display(Name = "Sweets/Snacks")]
        public List<QuestionOptionModel> SweetSaltySnacksQuestion { get; set; }

        /// <summary>
        /// Gets or sets the sweet salty snacks.
        /// </summary>
        /// <value>The sweet salty snacks.</value>
        //// [Display(Name = "Sweets/Snacks")]
        public string SweetSaltySnacks_Advert { get; set; }
        

        /// <summary>
        /// Gets or sets the alcoholic drinks question.
        /// </summary>
        /// <value>The alcoholic drinks question.</value>
        // // [Display(Name = "Alcoholic Drinks")]
        public List<QuestionOptionModel> AlcoholicDrinksQuestion { get; set; }

        /// <summary>
        /// Gets or sets the alcoholic drinks.
        /// </summary>
        /// <value>The alcoholic drinks.</value>
        // // [Display(Name = "Alcoholic Drinks")]
        public string AlcoholicDrinks_Advert { get; set; }
        


        /// <summary>
        /// Gets or sets the non alcoholic drinks question.
        /// </summary>
        /// <value>The non alcoholic drinks question.</value>
        // // [Display(Name = "Non-Alcoholic Drinks")]
        public List<QuestionOptionModel> NonAlcoholicDrinksQuestion { get; set; }

        /// <summary>
        /// Gets or sets the non alcoholic drinks.
        /// </summary>
        /// <value>The non alcoholic drinks.</value>
        // // [Display(Name = "Non-Alcoholic Drinks")]
        public string NonAlcoholicDrinks_Advert { get; set; }
        

        /// <summary>
        /// Gets or sets the householdproducts question.
        /// </summary>
        /// <value>The householdproducts question.</value>
        // // [Display(Name = "Household Appliances/Products")]
        public List<QuestionOptionModel> HouseholdproductsQuestion { get; set; }

        /// <summary>
        /// Gets or sets the householdproducts.
        /// </summary>
        /// <value>The householdproducts.</value>
        // // [Display(Name = "Household Appliances/Products")]
        public string Householdproducts_Advert { get; set; }
        

        /// <summary>
        /// Gets or sets the toiletries cosmetics question.
        /// </summary>
        /// <value>The toiletries cosmetics question.</value>
        // // [Display(Name = "Toiletries/Cosmetics")]
        public List<QuestionOptionModel> ToiletriesCosmeticsQuestion { get; set; }

        /// <summary>
        /// Gets or sets the toiletries cosmetics.
        /// </summary>
        /// <value>The toiletries cosmetics.</value>
        // // [Display(Name = "Toiletries/Cosmetics")]
        public string ToiletriesCosmetics_Advert { get; set; }
        

        /// <summary>
        /// Gets or sets the pharmaceutical chemists products question.
        /// </summary>
        /// <value>The pharmaceutical chemists products question.</value>
        // // [Display(Name = "Pharmaceutical/Chemists Products")]
        public List<QuestionOptionModel> PharmaceuticalChemistsProductsQuestion { get; set; }

        /// <summary>
        /// Gets or sets the pharmaceutical chemists products.
        /// </summary>
        /// <value>The pharmaceutical chemists products.</value>
        // // [Display(Name = "Pharmaceutical/Chemists Products")]
        public string PharmaceuticalChemistsProducts_Advert { get; set; }
        

        /// <summary>
        /// Gets or sets the tobacco products question.
        /// </summary>
        /// <value>The tobacco products question.</value>
        // // [Display(Name = "Tobacco Products")]
        public List<QuestionOptionModel> TobaccoProductsQuestion { get; set; }

        /// <summary>
        /// Gets or sets the tobacco products.
        /// </summary>
        /// <value>The tobacco products.</value>
        // // [Display(Name = "Tobacco Products")]
        public string TobaccoProducts_Advert { get; set; }
        

        /// <summary>
        /// Gets or sets the pets pet food question.
        /// </summary>
        /// <value>The pets pet food question.</value>
        // // [Display(Name = "Pets")]
        public List<QuestionOptionModel> PetsPetFoodQuestion { get; set; }

        /// <summary>
        /// Gets or sets the pets pet food.
        /// </summary>
        /// <value>The pets pet food.</value>
        // // [Display(Name = "Pets")]
        public string PetsPetFood_Advert { get; set; }
        

        /// <summary>
        /// Gets or sets the shopping retail clothing question.
        /// </summary>
        /// <value>The shopping retail clothing question.</value>
        // // [Display(Name = "Clothing/Fashion")]
        public List<QuestionOptionModel> ShoppingRetailClothingQuestion { get; set; }

        /// <summary>
        /// Gets or sets the shopping retail clothing.
        /// </summary>
        /// <value>The shopping retail clothing.</value>
        // // [Display(Name = "Clothing/Fashion")]
        public string ShoppingRetailClothing_Advert { get; set; }
        

        /// <summary>
        /// Gets or sets the diy gardening question.
        /// </summary>
        /// <value>The diy gardening question.</value>
        // // [Display(Name = "DIY/Gardening")]
        public List<QuestionOptionModel> DIYGardeningQuestion { get; set; }

        /// <summary>
        /// Gets or sets the diy gardening.
        /// </summary>
        /// <value>The diy gardening.</value>
        // // [Display(Name = "DIY/Gardening")]
        public string DIYGardening_Advert { get; set; }
        

        
        /// <summary>
        /// Gets or sets the electronics other personal items question.
        /// </summary>
        /// <value>The electronics other personal items question.</value>
        // // [Display(Name = "Electronics/Other Personal Items")]
        public List<QuestionOptionModel> ElectronicsOtherPersonalItemsQuestion { get; set; }

        /// <summary>
        /// Gets or sets the electronics other personal items.
        /// </summary>
        /// <value>The electronics other personal items.</value>
        // // [Display(Name = "Electronics/Other Personal Items")]
        public string ElectronicsOtherPersonalItems_Advert { get; set; }
        


        /// <summary>
        /// Gets or sets the communications internet question.
        /// </summary>
        /// <value>The communications internet question.</value>
        // // [Display(Name = "Communications/Internet Telecom")]
        public List<QuestionOptionModel> CommunicationsInternetQuestion { get; set; }

        /// <summary>
        /// Gets or sets the communications internet.
        /// </summary>
        /// <value>The communications internet.</value>
        // // [Display(Name = "Communications/Internet Telecom")]
        public string CommunicationsInternet_Advert { get; set; }
        


        /// <summary>
        /// Gets or sets the financial services question.
        /// </summary>
        /// <value>The financial services question.</value>
        // // [Display(Name = "Financial Services")]
        public List<QuestionOptionModel> FinancialServicesQuestion { get; set; }

        /// <summary>
        /// Gets or sets the financial services.
        /// </summary>
        /// <value>The financial services.</value>
        // // [Display(Name = "Financial Services")]
        public string FinancialServices_Advert { get; set; }
        


        /// <summary>
        /// Gets or sets the holidays travel question.
        /// </summary>
        /// <value>The holidays travel question.</value>
        // // [Display(Name = "Holidays/Travel Tourism")]
        public List<QuestionOptionModel> HolidaysTravelQuestion { get; set; }

        /// <summary>
        /// Gets or sets the holidays travel.
        /// </summary>
        /// <value>The holidays travel.</value>
        // // [Display(Name = "Holidays/Travel Tourism")]
        public string HolidaysTravel_Advert { get; set; }
        


        /// <summary>
        /// Gets or sets the sports leisure question.
        /// </summary>
        /// <value>The sports leisure question.</value>
        // // [Display(Name = "Sports/Leisure")]
        public List<QuestionOptionModel> SportsLeisureQuestion { get; set; }

        /// <summary>
        /// Gets or sets the sports leisure.
        /// </summary>
        /// <value>The sports leisure.</value>
        // // [Display(Name = "Sports/Leisure")]
        public string SportsLeisure_Advert { get; set; }
        


        /// <summary>
        /// Gets or sets the motoring question.
        /// </summary>
        /// <value>The motoring question.</value>
        // // [Display(Name = "Motoring/Automotive")]
        public List<QuestionOptionModel> MotoringQuestion { get; set; }

        /// <summary>
        /// Gets or sets the motoring.
        /// </summary>
        /// <value>The motoring.</value>
        // // [Display(Name = "Motoring/Automotive")]
        public string Motoring_Advert { get; set; }
        


        /// <summary>
        /// Gets or sets the newspapers question.
        /// </summary>
        /// <value>The newspapers question.</value>
        // // [Display(Name = "Newspapers/Magazines")]
        public List<QuestionOptionModel> NewspapersQuestion { get; set; }

        /// <summary>
        /// Gets or sets the newspapers.
        /// </summary>
        /// <value>The newspapers.</value>
        // // [Display(Name = "Newspapers/Magazines")]
        public string Newspapers_Advert { get; set; }
        

        /// <summary>
        /// Gets or sets the tv question.
        /// </summary>
        /// <value>The tv question.</value>
        // // [Display(Name = "TV/Video/ Radio")]
        public List<QuestionOptionModel> TVQuestion { get; set; }

        /// <summary>
        /// Gets or sets the tv.
        /// </summary>
        /// <value>The tv.</value>
        // // [Display(Name = "TV/Video/Radio")]
        public string TV_Advert { get; set; }
        



        /// <summary>
        /// Gets or sets the cinema question.
        /// </summary>
        /// <value>The cinema question.</value>
        // // [Display(Name = "Cinema")]
        public List<QuestionOptionModel> CinemaQuestion { get; set; }

        /// <summary>
        /// Gets or sets the cinema.
        /// </summary>
        /// <value>The cinema.</value>
        // // [Display(Name = "Cinema")]
        public string Cinema_Advert { get; set; }
        


        /// <summary>
        /// Gets or sets the social networking question.
        /// </summary>
        /// <value>The social networking question.</value>
        // // [Display(Name = "Social Networking")]
        public List<QuestionOptionModel> SocialNetworkingQuestion { get; set; }

        /// <summary>
        /// Gets or sets the social networking.
        /// </summary>
        /// <value>The social networking.</value>
        // // [Display(Name = "Social Networking")]
        public string SocialNetworking_Advert { get; set; }
        


        /// <summary>
        /// Gets or sets the shopping question.
        /// </summary>
        /// <value>The shopping question.</value>
        // // [Display(Name = "Shopping(retail gen merc)")]
        public List<QuestionOptionModel> ShoppingQuestion { get; set; }

        /// <summary>
        /// Gets or sets the shopping.
        /// </summary>
        /// <value>The shopping.</value>
        // // [Display(Name = "Shopping(retail gen merc)")]
        public string Shopping_Advert { get; set; }
        


        /// <summary>
        /// Gets or sets the fitness question.
        /// </summary>
        /// <value>The fitness question.</value>
        // // [Display(Name = "Fitness")]
        public List<QuestionOptionModel> FitnessQuestion { get; set; }

        /// <summary>
        /// Gets or sets the fitness.
        /// </summary>
        /// <value>The fitness.</value>
        // // [Display(Name = "Fitness")]
        public string Fitness_Advert { get; set; }
        



        /// <summary>
        /// Gets or sets the environment question.
        /// </summary>
        /// <value>The environment question.</value>
        // // [Display(Name = "Environment")]
        public List<QuestionOptionModel> EnvironmentQuestion { get; set; }

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        /// <value>The environment.</value>
        // // [Display(Name = "Environment")]
        public string Environment_Advert { get; set; }
        


        /// <summary>
        /// Gets or sets the going out question.
        /// </summary>
        /// <value>The going out question.</value>
        // // [Display(Name = "Going Out/Entertainment")]
        public List<QuestionOptionModel> GoingOutQuestion { get; set; }

        /// <summary>
        /// Gets or sets the going out.
        /// </summary>
        /// <value>The going out.</value>
        // // [Display(Name = "Going Out/Entertainment")]
        public string GoingOut_Advert { get; set; }
        


        /// <summary>
        /// Gets or sets the religion question.
        /// </summary>
        /// <value>The religion question.</value>
        // // [Display(Name = "Religion")]
        public List<QuestionOptionModel> ReligionQuestion { get; set; }

        /// <summary>
        /// Gets or sets the religion.
        /// </summary>
        /// <value>The religion.</value>
        // // [Display(Name = "Religion")]
        public string Religion_Advert { get; set; }
        


        /// <summary>
        /// Gets or sets the music question.
        /// </summary>
        /// <value>The music question.</value>
        // // [Display(Name = "Music")]
        public List<QuestionOptionModel> MusicQuestion { get; set; }

        /// <summary>
        /// Gets or sets the music.
        /// </summary>
        /// <value>The music.</value>
        // // [Display(Name = "Music")]
        public string Music_Advert { get; set; }
        

        // New Add Type
        // // [Display(Name = "Business/opportunities")]
        public List<QuestionOptionModel> BusinessOrOpportunitiesQuestion { get; set; }


        // // [Display(Name = "Business/opportunities")]
        public string BusinessOrOpportunities_AdType { get; set; }
        

        // // [Display(Name = "Over 18/Gambling")]
        public List<QuestionOptionModel> GamblingQuestion { get; set; }

        // // [Display(Name = "Over 18/Gambling")]
        public string Gambling_AdType { get; set; }
        


        // // [Display(Name = "Restaurants")]
        public List<QuestionOptionModel> RestaurantsQuestion { get; set; }

        // // [Display(Name = "Restaurants")]
        public string Restaurants_AdType { get; set; }
        

        // // [Display(Name = "Insurance")]
        public List<QuestionOptionModel> InsuranceQuestion { get; set; }

        // // [Display(Name = "Insurance")]
        public string Insurance_AdType { get; set; }
        


        // // [Display(Name = "Furniture")]
        public List<QuestionOptionModel> FurnitureQuestion { get; set; }

        // // [Display(Name = "Furniture")]
        public string Furniture_AdType { get; set; }
        

        // [Display(Name = "Information technology")]
        public List<QuestionOptionModel> InformationTechnologyQuestion { get; set; }

        // [Display(Name = "Information technology")]
        public string InformationTechnology_AdType { get; set; }
        


        // [Display(Name = "Energy")]
        public List<QuestionOptionModel> EnergyQuestion { get; set; }

        // [Display(Name = "Energy")]
        public string Energy_AdType { get; set; }
        


        // [Display(Name = "Supermarkets")]
        public List<QuestionOptionModel> SupermarketsQuestion { get; set; }

        // [Display(Name = "Supermarkets")]
        public string Supermarkets_AdType { get; set; }
        

        // [Display(Name = "Healthcare")]
        public List<QuestionOptionModel> HealthcareQuestion { get; set; }

        // [Display(Name = "Healthcare")]
        public string Healthcare_AdType { get; set; }
        


        // [Display(Name = "Jobs and Education")]
        public List<QuestionOptionModel> JobsAndEducationQuestion { get; set; }

        // [Display(Name = "Jobs and Education")]
        public string JobsAndEducation_AdType { get; set; }
        


        // [Display(Name = "Gifts")]
        public List<QuestionOptionModel> GiftsQuestion { get; set; }

        // [Display(Name = "Gifts")]
        public string Gifts_AdType { get; set; }
        


        // [Display(Name = "Advocacy/Legal")]
        public List<QuestionOptionModel> AdvocacyOrLegalQuestion { get; set; }

        // [Display(Name = "Advocacy/Legal")]
        public string AdvocacyOrLegal_AdType { get; set; }
        

        // [Display(Name = "Dating & Personal")]
        public List<QuestionOptionModel> DatingAndPersonalQuestion { get; set; }

        // [Display(Name = "Dating & Personal")]
        public string DatingAndPersonal_AdType { get; set; }
        


        // [Display(Name = "Real Estate/Property")]
        public List<QuestionOptionModel> RealEstateQuestion { get; set; }

        // [Display(Name = "Real Estate/Property")]
        public string RealEstate_AdType { get; set; }
        

        // [Display(Name = "Games")]
        public List<QuestionOptionModel> GamesQuestion { get; set; }

        // [Display(Name = "Games")]
        public string Games_AdType { get; set; }
        


        // [Display(Name = "Hustlers")]
        public List<QuestionOptionModel> HustlersQuestion { get; set; }

        // [Display(Name = "Hustlers")]
        public string Hustlers_AdType { get; set; }
        


        // [Display(Name = "Youth")]
        public List<QuestionOptionModel> YouthQuestion { get; set; }

        // [Display(Name = "Youth")]
        public string Youth_AdType { get; set; }
        

        // [Display(Name = "Discerning Professionals")]
        public List<QuestionOptionModel> DiscerningProfessionalsQuestion { get; set; }

        // [Display(Name = "Discerning Professionals")]
        public string DiscerningProfessionals_AdType { get; set; }
        

        // [Display(Name = "Mass")]
        public List<QuestionOptionModel> MassQuestion { get; set; }

        // [Display(Name = "Mass")]
        public string Mass_AdType { get; set; }
        


        #region Country Wise Hide Show Option
        public bool Food { get; set; }
        public bool SweetsSnacks { get; set; }
        public bool AlcoholicDrinks { get; set; }
        public bool NonAlcoholicDrinks { get; set; }
        public bool HouseholdAppliancesProducts { get; set; }
        public bool ToiletriesCosmetics { get; set; }
        public bool PharmaceuticalChemistsProducts { get; set; }
        public bool TobaccoProducts { get; set; }
        public bool Pets { get; set; }
        public bool ClothingFashion { get; set; }
        public bool DIYGardening { get; set; }
        public bool ElectronicsOtherPersonalItems { get; set; }
        public bool CommunicationsInternetTelecom { get; set; }
        public bool FinancialServices { get; set; }
        public bool HolidaysTravelTourism { get; set; }
        public bool SportsLeisure { get; set; }
        public bool MotoringAutomotive { get; set; }
        public bool NewspapersMagazines { get; set; }
        public bool TvVideoRadio { get; set; }
        public bool Cinema { get; set; }
        public bool SocialNetworking { get; set; }
        public bool Shopping { get; set; }
        public bool Fitness { get; set; }
        public bool Environment { get; set; }
        public bool GoingOutEntertainment { get; set; }
        public bool Religion { get; set; }
        public bool Music { get; set; }
        public bool BusinessOpportunities { get; set; }
        public bool Over18Gambling { get; set; }
        public bool Restaurants { get; set; }
        public bool Insurance { get; set; }
        public bool Furniture { get; set; }
        public bool Informationtechnology { get; set; }
        public bool Energy { get; set; }
        public bool Supermarkets { get; set; }
        public bool Healthcare { get; set; }
        public bool JobsandEducation { get; set; }
        public bool Gifts { get; set; }
        public bool AdvocacyLegal { get; set; }
        public bool DatingPersonal { get; set; }
        public bool RealEstateProperty { get; set; }
        public bool Games { get; set; }
        public bool SkizaProfile { get; set; }
        #endregion
    }
}
