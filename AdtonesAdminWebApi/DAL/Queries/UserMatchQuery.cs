using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{

    public static class UserMatchQuery
    {
        public static string UpdateMediaLocation => "UPDATE CampaignMatches SET MEDIA_URL=@media WHERE MSCampaignProfileId=@campaignProfileId";

        public static string GetBudgetUpdateAmount => "SELECT MaxHourlyBudget, MaxBid FROM CampaignProfile WHERE CampaignProfileId=@Id";

        public static string UpdateBucketAmount => "Update CampaignProfile SET BucketCount=@BucketCount WHERE CampaignProfileId=@Id";


        public static string InsertNewCampaignMatchData => @"INSERT INTO CampaignMatches(UserId,ClientId,CampaignName,CampaignDescription,TotalBudget,
                                                        MaxDailyBudget,MaxBid,MaxMonthBudget,MaxWeeklyBudget,MaxHourlyBudget,TotalCredit,
                                                        EmailFileLocation,Active,
                                                        EmailSubject,EmailBody,EmailSenderAddress,SmsOriginator,SmsBody,SMSFileLocation,CreatedDateTime,
                                                        UpdatedDateTime,Status,StartDate,EndDate,NumberInBatch,CountryId,
                                                        NextStatus,MSCampaignProfileId,EMAIL_MESSAGE,SMS_MESSAGE,ORIGINATOR)
                                                    VALUES(@UserId,@ClientId,@CampaignName,@CampaignDescription,@TotalBudget,@MaxDailyBudget,
                                                        @MaxBid,@MaxMonthBudget,@MaxWeeklyBudget,@MaxHourlyBudget,@TotalCredit,
                                                        @EmailFileLocation,@Active,@EmailSubject,@EmailBody,@EmailSenderAddress,@SmsOriginator,@SmsBody,
                                                        @SMSFileLocation,GETDATE(),GETDATE(),@Status,@StartDate,@EndDate,@NumberInBatch,
                                                        @CountryId,1,
                                                        @CampaignProfileId,@EmailBody,@SmsBody,@SmsOriginator);
                                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";


        public static string UpdateCampaignMatchData => @"UPDATE CampaignMatches SET CampaignDescription=@CampaignDescription,
                                                    MaxDailyBudget=@MaxDailyBudget,MaxBid=@MaxBid,MaxMonthBudget=@MaxMonthBudget,
                                                    MaxWeeklyBudget=@MaxWeeklyBudget,MaxHourlyBudget=@MaxHourlyBudget,
                                                    EmailSubject=@EmailSubject,EmailBody=@EmailBody,SmsOriginator=@SmsOriginator,SmsBody=@SmsBody,
                                                    UpdatedDateTime=GETDATE(),StartDate=@StartDate,EndDate=@EndDate
                                                    WHERE MSCampaignProfileId=@CampaignProfileId ";


        public static string GetCampaignMatchesDetailsById => @"SELECT * FROM CampaignMatches WHERE MSCampaignProfileId=@Id;";


        //        public static string UpdateNewCampaignData => @"UPDATE CampaignMatches SET UserId=@UserId,ClientId=@ClientId,CampaignName,CampaignDescription,TotalBudget,
        //                                                    MaxDailyBudget,MaxBid,MaxMonthBudget,MaxWeeklyBudget,MaxHourlyBudget,TotalCredit,
        //                                                    AvailableCredit,PlaysToDate,PlaysLastMonth,PlaysCurrentMonth,CancelledToDate,CancelledLastMonth,CancelledCurrentMonth,
        //                                                    SmsToDate,SmsLastMonth,SmsCurrentMonth,EmailToDate,EmailsLastMonth,EmailsCurrentMonth,
        //                                                    EmailFileLocation,Active,NumberOfPlays,AverageDailyPlays,SmsRequests,EmailsDelievered,
        //                                                    EmailSubject,EmailBody,EmailSenderAddress,SmsOriginator,SmsBody,SMSFileLocation,CreatedDateTime,
        //                                                    UpdatedDateTime,Status,StartDate,EndDate,NumberInBatch,CountryId,RemainingMaxMonthBudget,
        //                                                    RemainingMaxDailyBudget,RemainingMaxWeeklyBudget,RemainingMaxHourlyBudget,
        //                                                    NextStatus,MSCampaignProfileId,EMAIL_MESSAGE,SMS_MESSAGE,ORIGINATOR
        //WHERE CampaignProfileId=@id ";


        public static string GetProfileMatchInformationId => @"SELECT Id FROM ProfileMatchInformations WHERE LOWER(ProfileName)=@profileName 
                                                                AND IsActive=1 AND CountryId=@Id";

        public static string GetProfileMatchLabels => @"SELECT ProfileLabel FROM ProfileMatchLabels 
                                                                WHERE ProfileMatchInformationId=@Id";


        public static string GetCampaignProfilePreference = @"SELECT * FROM CampaignProfilePreference";


        public static string GetCampaignProfilePreferenceId = @"SELECT Id FROM CampaignProfilePreference WHERE CampaignProfileId=@Id";


        public static string GetCampaignProfilePreferenceDetailsByCampaignId = @"SELECT * FROM CampaignProfilePreference WHERE CampaignProfileId=@Id";


        public static string InsertMatchFinancial = @"UPDATE CampaignMatches SET TotalBudget=@TotalBudget,TotalCredit=@TotalCredit,
                                                        AvailableCredit=@AvailableCredit,Status=@Status WHERE MSCampaignProfileId=@Id";



        public static string InsertProfilePreference = @"INSERT INTO CampaignProfilePreference(CountryId,CampaignProfileId,NextStatus,AdtoneServerCampaignProfilePrefId)
                                                            VALUES(@CountryId,@CampaignProfileId,0, @AdtoneServerCampaignProfilePrefId);
                                                            SELECT CAST(SCOPE_IDENTITY() AS INT);";


        public static string UpdateGeographicProfile = @"UPDATE CampaignProfilePreference SET PostCode=@PostCode,Location_Demographics=@Location_Demographics
                                                            WHERE CampaignProfileId=@Id";


        public static string UpdateDemographicProfile = @"UPDATE CampaignProfilePreference SET DOBEnd_Demographics=@DOBEnd_Demographics,
                                                            DOBStart_Demographics=@DOBStart_Demographics,Age_Demographics=@Age_Demographics,
                                                            Education_Demographics=@Education_Demographics,Gender_Demographics=@Gender_Demographics,
                                                            HouseholdStatus_Demographics=@HouseholdStatus_Demographics,IncomeBracket_Demographics=@IncomeBracket_Demographics,
                                                            RelationshipStatus_Demographics=@RelationshipStatus_Demographics,
                                                            WorkingStatus_Demographics=@WorkingStatus_Demographics WHERE CampaignProfileId=@Id";


        public static string InsertMatchCampaignGeographic = @"INSERT INTO CampaignMatches(Location_Demographics, MSCampaignProfileId)
                                                                VALUES(@Location_Demographics,@Id)";


        public static string InsertMatchCampaignDemographic = @"INSERT INTO CampaignMatches(Age_Demographics,Education_Demographics,Gender_Demographics,
                                                            HouseholdStatus_Demographics,IncomeBracket_Demographics,RelationshipStatus_Demographics,
                                                            WorkingStatus_Demographics, MSCampaignProfileId)
                                                                VALUES(@Age_Demographics,
                                                            @Education_Demographics,@Gender_Demographics,@HouseholdStatus_Demographics,@IncomeBracket_Demographics,
                                                            @RelationshipStatus_Demographics, @WorkingStatus_Demographics,@Id)";


        public static string UpdateMatchCampaignGeographic = @"UPDATE CampaignMatches SET Location_Demographics=@Location_Demographics
                                                                WHERE MSCampaignProfileId=@Id";


        public static string UpdateMatchCampaignDemographic = @"UPDATE CampaignMatches SET Age_Demographics=@Age_Demographics,
                                                            Education_Demographics=@Education_Demographics,Gender_Demographics=@Gender_Demographics,
                                                            HouseholdStatus_Demographics=@HouseholdStatus_Demographics,IncomeBracket_Demographics=@IncomeBracket_Demographics,
                                                            RelationshipStatus_Demographics=@RelationshipStatus_Demographics,
                                                            WorkingStatus_Demographics=@WorkingStatus_Demographics
                                                            WHERE MSCampaignProfileId=@Id";


        public static string GetCampaignTimeSettings => @"SELECT * FROM CampaignProfileTimeSetting WHERE CampaignProfileId=@Id";


        public static string InsertTimeSettingsProfile => @"INSERT INTO CampaignProfileTimeSetting
                                                                (CampaignProfileId,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday,
                                                                    AdtoneServerCampaignProfileTimeId)
                                                            VALUES
                                                                (@CampaignProfileId,@Monday,@Tuesday,@Wednesday,@Thursday,@Friday,@Saturday,
                                                                    @Sunday,@AdtoneServerCampaignProfileTimeId)";

        public static string UpdateTimeSettingsProfile => @"UPDATE CampaignProfileTimeSetting SET Monday=@Monday,Tuesday=@Tuesday,Wednesday=@Wednesday,
                                                                    Thursday=@Thursday,Friday=@Friday,Saturday=@Saturday,Sunday=@Sunday
                                                                    WHERE CampaignProfileId=@Id";


        public static string UpdateMobileProfile => @"UPDATE CampaignProfilePreference SET ContractType_Mobile=@ContractType_Mobile,
                                                        Spend_Mobile=@Spend_Mobile WHERE CampaignProfileId=@Id";


        public static string UpdateMatchCampaignMobile => @"UPDATE CampaignMatches SET ContractType_Mobile=@ContractType_Mobile,
                                                        Spend_Mobile=@Spend_Mobile WHERE MSCampaignProfileId=@Id";


        public static string UpdateQuestionnaireProfile => @"UPDATE CampaignProfilePreference SET Hustlers_AdType=@Hustlers_AdType,Youth_AdType=@Youth_AdType,
                                                            DiscerningProfessionals_AdType=@DiscerningProfessionals_AdType,Mass_AdType=@Mass_AdType
                                                            WHERE CampaignProfileId=@Id";


        public static string UpdateMatchCampaignQuestionnaire => @"UPDATE CampaignMatches SET Hustlers_AdType=@Hustlers_AdType,Youth_AdType=@Youth_AdType,
                                                                    DiscerningProfessionals_AdType=@DiscerningProfessionals_AdType,Mass_AdType=@Mass_AdType 
                                                                    WHERE MSCampaignProfileId-@Id";


        public static string UpdateAdvertProfile => @"UPDATE CampaignProfilePreference SET AlcoholicDrinks_Advert=@AlcoholicDrinks_Advert,Cinema_Advert=@Cinema_Advert,
                                                        CommunicationsInternet_Advert=@CommunicationsInternet_Advert,DIYGardening_Advert=@DIYGardening_Advert,
                                                        ElectronicsOtherPersonalItems_Advert=@ElectronicsOtherPersonalItems_Advert,
                                                        Environment_Advert=@Environment_Advert,FinancialServices_Advert=@FinancialServices_Advert,
                                                        Fitness_Advert=@Fitness_Advert,Food_Advert=@Food_Advert,GoingOut_Advert=@GoingOut_Advert,
                                                        HolidaysTravel_Advert=@HolidaysTravel_Advert,Householdproducts_Advert=@Householdproducts_Advert,
                                                        Motoring_Advert=@Motoring_Advert,Music_Advert=@Music_Advert,Newspapers_Advert=@Newspapers_Advert,
                                                        NonAlcoholicDrinks_Advert=@NonAlcoholicDrinks_Advert,PetsPetFood_Advert=@PetsPetFood_Advert,
                                                        PharmaceuticalChemistsProducts_Advert=@PharmaceuticalChemistsProducts_Advert,
                                                        Religion_Advert=@Religion_Advert,Shopping_Advert=@Shopping_Advert,
                                                        ShoppingRetailClothing_Advert=@ShoppingRetailClothing_Advert,
                                                        SocialNetworking_Advert=@SocialNetworking_Advert,SportsLeisure_Advert=@SportsLeisure_Advert,
                                                        SweetSaltySnacks_Advert=@SweetSaltySnacks_Advert,TV_Advert=@TV_Advert,TobaccoProducts_Advert=@TobaccoProducts_Advert,
                                                        ToiletriesCosmetics_Advert=@ToiletriesCosmetics_Advert,BusinessOrOpportunities_AdType=@BusinessOrOpportunities_AdType,
                                                        Gambling_AdType=@Gambling_AdType,Restaurants_AdType=@Restaurants_AdType,
                                                        Insurance_AdType=@Insurance_AdType,Furniture_AdType=@Furniture_AdType,
                                                        InformationTechnology_AdType=@InformationTechnology_AdType,Energy_AdType=@Energy_AdType,
                                                        Supermarkets_AdType=@Supermarkets_AdType,Healthcare_AdType=@Healthcare_AdType,
                                                        JobsAndEducation_AdType=@JobsAndEducation_AdType,Gifts_AdType=@Gifts_AdType,
                                                        AdvocacyOrLegal_AdType=@AdvocacyOrLegal_AdType,DatingAndPersonal_AdType=@DatingAndPersonal_AdType,
                                                        RealEstate_AdType=@RealEstate_AdType,Games_AdType=@Games_AdType 
                                                        WHERE CampaignProfileId=@CampaignProfileId";


        public static string UpdateMatchCampaignAdvert => @"UPDATE CampaignMatches SET AlcoholicDrinks_Advert=@AlcoholicDrinks_Advert,Cinema_Advert=@Cinema_Advert,
                                                        CommunicationsInternet_Advert=@CommunicationsInternet_Advert,DIYGardening_Advert=@DIYGardening_Advert,
                                                        ElectronicsOtherPersonalItems_Advert=@ElectronicsOtherPersonalItems_Advert,
                                                        Environment_Advert=@Environment_Advert,FinancialServices_Advert=@FinancialServices_Advert,
                                                        Fitness_Advert=@Fitness_Advert,Food_Advert=@Food_Advert,GoingOut_Advert=@GoingOut_Advert,
                                                        HolidaysTravel_Advert=@HolidaysTravel_Advert,Householdproducts_Advert=@Householdproducts_Advert,
                                                        Motoring_Advert=@Motoring_Advert,Music_Advert=@Music_Advert,Newspapers_Advert=@Newspapers_Advert,
                                                        NonAlcoholicDrinks_Advert=@NonAlcoholicDrinks_Advert,PetsPetFood_Advert=@PetsPetFood_Advert,
                                                        PharmaceuticalChemistsProducts_Advert=@PharmaceuticalChemistsProducts_Advert,
                                                        Religion_Advert=@Religion_Advert,Shopping_Advert=@Shopping_Advert,
                                                        ShoppingRetailClothing_Advert=@ShoppingRetailClothing_Advert,
                                                        SocialNetworking_Advert=@SocialNetworking_Advert,SportsLeisure_Advert=@SportsLeisure_Advert,
                                                        SweetSaltySnacks_Advert=@SweetSaltySnacks_Advert,TV_Advert=@TV_Advert,TobaccoProducts_Advert=@TobaccoProducts_Advert,
                                                        ToiletriesCosmetics_Advert=@ToiletriesCosmetics_Advert,BusinessOrOpportunities_AdType=@BusinessOrOpportunities_AdType,
                                                        Gambling_AdType=@Gambling_AdType,Restaurants_AdType=@Restaurants_AdType,
                                                        Insurance_AdType=@Insurance_AdType,Furniture_AdType=@Furniture_AdType,
                                                        InformationTechnology_AdType=@InformationTechnology_AdType,Energy_AdType=@Energy_AdType,
                                                        Supermarkets_AdType=@Supermarkets_AdType,Healthcare_AdType=@Healthcare_AdType,
                                                        JobsAndEducation_AdType=@JobsAndEducation_AdType,Gifts_AdType=@Gifts_AdType,
                                                        AdvocacyOrLegal_AdType=@AdvocacyOrLegal_AdType,DatingAndPersonal_AdType=@DatingAndPersonal_AdType,
                                                        RealEstate_AdType=@RealEstate_AdType,Games_AdType=@Games_AdType 
                                                        WHERE MSCampaignProfileId=@CampaignProfileId";

        



        public static string InsertGeographicProfile = @"INSERT INTO CampaignProfilePreference(PostCode,CountryId,CampaignProfileId,
                                                            Location_Demographics,AdtoneServerCampaignProfilePrefId)
                                                        VALUES(@PostCode,@CountryId,@CampaignProfileId,@Location_Demographics, @AdtoneServerCampaignProfilePrefId);
                                                        SELECT CAST(SCOPE_IDENTITY() AS INT);";


        public static string InsertDemographicProfile => @"INSERT INTO CampaignProfilePreference(CampaignProfileId,DOBEnd_Demographics,
                                                            DOBStart_Demographics,Age_Demographics,Education_Demographics,Gender_Demographics,
                                                            HouseholdStatus_Demographics,IncomeBracket_Demographics,RelationshipStatus_Demographics,
                                                            WorkingStatus_Demographics,AdtoneServerCampaignProfilePrefId)
                                                           VALUES(@CampaignProfileId,@DOBEnd_Demographics,@DOBStart_Demographics,@Age_Demographics,
                                                            @Education_Demographics,@Gender_Demographics,@HouseholdStatus_Demographics,@IncomeBracket_Demographics,
                                                            @RelationshipStatus_Demographics, @WorkingStatus_Demographics,@AdtoneServerCampaignProfilePrefId);
                                                            SELECT CAST(SCOPE_IDENTITY() AS INT);";


        public static string InsertAdvertProfile => @"INSERT INTO CampaignProfilePreference(CampaignProfileId,AlcoholicDrinks_Advert,Cinema_Advert,
                                                        CommunicationsInternet_Advert,DIYGardening_Advert,ElectronicsOtherPersonalItems_Advert,Environment_Advert,
                                                        FinancialServices_Advert,Fitness_Advert,Food_Advert,GoingOut_Advert,
                                                        HolidaysTravel_Advert,Householdproducts_Advert,Motoring_Advert,Music_Advert,
                                                        Newspapers_Advert,NonAlcoholicDrinks_Advert,PetsPetFood_Advert=@PetsPetFood_Advert,
                                                        PharmaceuticalChemistsProducts_Advert,Religion_Advert,Shopping_Advert,
                                                        ShoppingRetailClothing_Advert,SocialNetworking_Advert,SportsLeisure_Advert,
                                                        SweetSaltySnacks_Advert,TV_Advert,TobaccoProducts_Advert,ToiletriesCosmetics_Advert,
                                                        BusinessOrOpportunities_AdType,Gambling_AdType,Restaurants_AdType,Insurance_AdType,
                                                        Furniture_AdType,InformationTechnology_AdType,Energy_AdType,Supermarkets_AdType,
                                                        Healthcare_AdType,JobsAndEducation_AdType,Gifts_AdType,AdvocacyOrLegal_AdType,
                                                        DatingAndPersonal_AdType,RealEstate_AdType,Games_AdType)
                                                        VALUES(@CampaignProfileId,@AlcoholicDrinks_Advert,@Cinema_Advert,@CommunicationsInternet_Advert,
                                                            @DIYGardening_Advert,@ElectronicsOtherPersonalItems_Advert,@Environment_Advert,
                                                            @FinancialServices_Advert,@Fitness_Advert,@Food_Advert,@GoingOut_Advert,@HolidaysTravel_Advert,
                                                            @Householdproducts_Advert,@Motoring_Advert,@Music_Advert,@Newspapers_Advert,
                                                            @NonAlcoholicDrinks_Advert,@PetsPetFood_Advert=@PetsPetFood_Advert,@PharmaceuticalChemistsProducts_Advert,
                                                            @Religion_Advert,@Shopping_Advert,@ShoppingRetailClothing_Advert,@SocialNetworking_Advert,
                                                            @SportsLeisure_Advert,@SweetSaltySnacks_Advert,@TV_Advert,@TobaccoProducts_Advert,
                                                            @ToiletriesCosmetics_Advert,@BusinessOrOpportunities_AdType,@Gambling_AdType,@Restaurants_AdType,
                                                            @Insurance_AdType,@Furniture_AdType,@InformationTechnology_AdType,@Energy_AdType,
                                                            @Supermarkets_AdType,@Healthcare_AdType,@JobsAndEducation_AdType,@Gifts_AdType,
                                                            @AdvocacyOrLegal_AdType,@DatingAndPersonal_AdType,@RealEstate_AdType,@Games_AdType)";

        public static string InsertQuestionnaireProfile => @"INSERT INTO CampaignProfilePreference(CampaignProfileId,Hustlers_AdType,Youth_AdType,
                                                            DiscerningProfessionals_AdType,Mass_AdType,AdtoneServerCampaignProfilePrefId,CountryId)
                                                        VALUES(@CampaignProfileId,@Hustlers_AdType,@Youth_AdType,@DiscerningProfessionals_AdType,
                                                            @Mass_AdType,@AdtoneServerCampaignProfilePrefId,@CountryId)";

        public static string InsertMobileProfile => @"INSERT INTO CampaignProfilePreference(CampaignProfileId,ContractType_Mobile,Spend_Mobile,AdtoneServerCampaignProfilePrefId)
                                                        VALUES(@CampaignProfileId,@ContractType_Mobile,@Spend_Mobile,@AdtoneServerCampaignProfilePrefId)";



    }
}
