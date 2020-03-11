using System;
using System.Collections.Generic;

namespace HourglassServer.Data
{
    public partial class FederalContractsFy2019
    {
        public int Id { get; set; }
        public decimal? TotalDollarsObligated { get; set; }
        public decimal? CurrentTotalValueOfAward { get; set; }
        public decimal? PotentialTotalValueOfAward { get; set; }
        public DateTime? ActionDate { get; set; }
        public DateTime? PeriodOfPerformanceStartDate { get; set; }
        public DateTime? PeriodOfPerformanceCurrentEndDate { get; set; }
        public DateTime? PeriodOfPerformancePotentialEndDate { get; set; }
        public string AwardingAgencyName { get; set; }
        public string FundingAgencyName { get; set; }
        public string RecipientName { get; set; }
        public string RecipientParentName { get; set; }
        public string RecipientCountryName { get; set; }
        public string RecipientAddressLine1 { get; set; }
        public string RecipientCityName { get; set; }
        public string RecipientStateName { get; set; }
        public string RecipientZip4Code { get; set; }
        public string PrimaryPlaceOfPerformanceCountryCode { get; set; }
        public string PrimaryPlaceOfPerformanceCountryName { get; set; }
        public string PrimaryPlaceOfPerformanceCityName { get; set; }
        public string PrimaryPlaceOfPerformanceCountyName { get; set; }
        public string PrimaryPlaceOfPerformanceStateCode { get; set; }
        public string PrimaryPlaceOfPerformanceStateName { get; set; }
        public string PrimaryPlaceOfPerformanceZip4 { get; set; }
        public string AwardType { get; set; }
        public string AwardDescription { get; set; }
    }
}
