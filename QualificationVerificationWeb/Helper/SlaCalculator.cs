using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;

namespace QualificationVerificationWeb.Helper
{
    public static class SlaCalculator
    {
        private const int AcademicRecordSlaDays = 3;
        private const int ConfirmationLetterSlaDays = 3;
        private const int FormsForOfficialBodiesSlaDays = 7;
        private const int AcademicTranscriptSupplementSlaDays = 20;

        private static readonly string[] DateFormats =
        {
            "yyyy/MM/dd",
            "yyyy-MM-dd",
            "dd/MM/yyyy",
            "dd-MM-yyyy",
            "MM/dd/yyyy",
            "MM-dd-yyyy",
            "yyyy/MM/dd HH:mm:ss",
            "yyyy-MM-dd HH:mm:ss",
            "dd/MM/yyyy HH:mm:ss",
            "dd-MM-yyyy HH:mm:ss",
            "MM/dd/yyyy HH:mm:ss",
            "MM-dd-yyyy HH:mm:ss"
        };

        public static SlaCalculationResult Calculate(string createdOnDate, string documentType)
        {
            DateTime createdDate;
            int slaWorkingDays = GetSlaWorkingDays(documentType);

            if (slaWorkingDays == 0 || !TryParseDate(createdOnDate, out createdDate))
            {
                return SlaCalculationResult.NotAvailable();
            }

            DateTime expiryDate = AddWorkingDays(createdDate.Date, slaWorkingDays);
            DateTime today = DateTime.Today;
            int workingDaysUntilExpiry = CountWorkingDaysAfterStartUntilEnd(today, expiryDate);

            if (today >= expiryDate)
            {
                return new SlaCalculationResult
                {
                    SlaWorkingDays = slaWorkingDays,
                    ExpiryDate = expiryDate,
                    WorkingDaysUntilExpiry = 0,
                    Status = "Expired",
                    CssClass = "sla-indicator sla-indicator-red",
                    DisplayText = "SLA Expired",
                    ToolTip = BuildToolTip(slaWorkingDays, expiryDate, "SLA expiry date has been reached or passed.")
                };
            }

            if (workingDaysUntilExpiry == 1)
            {
                return new SlaCalculationResult
                {
                    SlaWorkingDays = slaWorkingDays,
                    ExpiryDate = expiryDate,
                    WorkingDaysUntilExpiry = workingDaysUntilExpiry,
                    Status = "DueSoon",
                    CssClass = "sla-indicator sla-indicator-orange",
                    DisplayText = "SLA Due Soon",
                    ToolTip = BuildToolTip(slaWorkingDays, expiryDate, "One working day remains before SLA expiry.")
                };
            }

            return new SlaCalculationResult
            {
                SlaWorkingDays = slaWorkingDays,
                ExpiryDate = expiryDate,
                WorkingDaysUntilExpiry = workingDaysUntilExpiry,
                Status = "OnTrack",
                CssClass = "sla-indicator sla-indicator-green",
                DisplayText = "SLA On Track",
                ToolTip = BuildToolTip(slaWorkingDays, expiryDate, "SLA is not within the warning window.")
            };
        }

        public static int GetSlaWorkingDays(string documentType)
        {
            if (String.IsNullOrWhiteSpace(documentType))
            {
                return 0;
            }

            var matchingSlaDays = new List<int>();
            string normalizedDocumentType = documentType.ToLowerInvariant();

            if (normalizedDocumentType.Contains("academic transcript supplement") || normalizedDocumentType.Contains("transcript supplement"))
            {
                matchingSlaDays.Add(AcademicTranscriptSupplementSlaDays);
            }

            if (normalizedDocumentType.Contains("form for official bodies") || normalizedDocumentType.Contains("forms for official bodies") || normalizedDocumentType.Contains("official bodies"))
            {
                matchingSlaDays.Add(FormsForOfficialBodiesSlaDays);
            }

            if (normalizedDocumentType.Contains("academic record"))
            {
                matchingSlaDays.Add(AcademicRecordSlaDays);
            }

            if (normalizedDocumentType.Contains("confirmation letter") || normalizedDocumentType.Contains("general letters"))
            {
                matchingSlaDays.Add(ConfirmationLetterSlaDays);
            }

            return matchingSlaDays.Any() ? matchingSlaDays.Max() : 0;
        }

        public static DateTime AddWorkingDays(DateTime startDate, int workingDays)
        {
            if (workingDays <= 0)
            {
                return startDate.Date;
            }

            DateTime currentDate = startDate.Date;
            int countedWorkingDays = 0;

            while (countedWorkingDays < workingDays)
            {
                currentDate = currentDate.AddDays(1);

                if (IsWorkingDay(currentDate))
                {
                    countedWorkingDays++;
                }
            }

            return currentDate;
        }

        public static bool IsWorkingDay(DateTime date)
        {
            DateTime dateOnly = date.Date;

            if (dateOnly.DayOfWeek == DayOfWeek.Saturday || dateOnly.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }

            return !GetHolidayDates(dateOnly.Year).Contains(dateOnly);
        }

        private static bool TryParseDate(string value, out DateTime date)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                date = DateTime.MinValue;
                return false;
            }

            if (DateTime.TryParseExact(value.Trim(), DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out date))
            {
                return true;
            }

            return DateTime.TryParse(value, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out date);
        }

        private static int CountWorkingDaysAfterStartUntilEnd(DateTime startDate, DateTime endDate)
        {
            if (startDate.Date >= endDate.Date)
            {
                return 0;
            }

            int workingDays = 0;
            DateTime currentDate = startDate.Date;

            while (currentDate < endDate.Date)
            {
                currentDate = currentDate.AddDays(1);

                if (IsWorkingDay(currentDate))
                {
                    workingDays++;
                }
            }

            return workingDays;
        }

        private static HashSet<DateTime> GetHolidayDates(int year)
        {
            var holidays = new HashSet<DateTime>();
            AddSouthAfricanPublicHolidays(year, holidays);
            AddConfiguredHolidays(holidays, "QualificationVerification:AdditionalPublicHolidayDates");
            AddConfiguredHolidays(holidays, "QualificationVerification:UniversityHolidayDates");
            return holidays;
        }

        private static void AddSouthAfricanPublicHolidays(int year, HashSet<DateTime> holidays)
        {
            AddHolidayWithSundayObservation(holidays, new DateTime(year, 1, 1));
            AddHolidayWithSundayObservation(holidays, new DateTime(year, 3, 21));
            AddHolidayWithSundayObservation(holidays, new DateTime(year, 4, 27));
            AddHolidayWithSundayObservation(holidays, new DateTime(year, 5, 1));
            AddHolidayWithSundayObservation(holidays, new DateTime(year, 6, 16));
            AddHolidayWithSundayObservation(holidays, new DateTime(year, 8, 9));
            AddHolidayWithSundayObservation(holidays, new DateTime(year, 9, 24));
            AddHolidayWithSundayObservation(holidays, new DateTime(year, 12, 16));
            AddHolidayWithSundayObservation(holidays, new DateTime(year, 12, 25));
            AddHolidayWithSundayObservation(holidays, new DateTime(year, 12, 26));

            DateTime easterSunday = GetEasterSunday(year);
            holidays.Add(easterSunday.AddDays(-2));
            holidays.Add(easterSunday.AddDays(1));
        }

        private static void AddHolidayWithSundayObservation(HashSet<DateTime> holidays, DateTime holiday)
        {
            holidays.Add(holiday.Date);

            if (holiday.DayOfWeek == DayOfWeek.Sunday)
            {
                holidays.Add(holiday.AddDays(1).Date);
            }
        }

        private static DateTime GetEasterSunday(int year)
        {
            int a = year % 19;
            int b = year / 100;
            int c = year % 100;
            int d = b / 4;
            int e = b % 4;
            int f = (b + 8) / 25;
            int g = (b - f + 1) / 3;
            int h = (19 * a + b - d - g + 15) % 30;
            int i = c / 4;
            int k = c % 4;
            int l = (32 + 2 * e + 2 * i - h - k) % 7;
            int m = (a + 11 * h + 22 * l) / 451;
            int month = (h + l - 7 * m + 114) / 31;
            int day = ((h + l - 7 * m + 114) % 31) + 1;

            return new DateTime(year, month, day);
        }

        private static void AddConfiguredHolidays(HashSet<DateTime> holidays, string appSettingKey)
        {
            string configuredHolidayDates = ConfigurationManager.AppSettings[appSettingKey];

            if (String.IsNullOrWhiteSpace(configuredHolidayDates))
            {
                return;
            }

            foreach (string configuredHolidayDate in configuredHolidayDates.Split(new[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries))
            {
                DateTime holidayDate;
                if (TryParseDate(configuredHolidayDate, out holidayDate))
                {
                    holidays.Add(holidayDate.Date);
                }
            }
        }

        private static string BuildToolTip(int slaWorkingDays, DateTime expiryDate, string statusText)
        {
            return String.Format("{0} SLA working days. SLA expiry date: {1:yyyy/MM/dd}. {2}", slaWorkingDays, expiryDate, statusText);
        }
    }

    public class SlaCalculationResult
    {
        public int SlaWorkingDays { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int WorkingDaysUntilExpiry { get; set; }
        public string Status { get; set; }
        public string CssClass { get; set; }
        public string DisplayText { get; set; }
        public string ToolTip { get; set; }

        public static SlaCalculationResult NotAvailable()
        {
            return new SlaCalculationResult
            {
                SlaWorkingDays = 0,
                ExpiryDate = null,
                WorkingDaysUntilExpiry = 0,
                Status = "NotAvailable",
                CssClass = "sla-indicator sla-indicator-neutral",
                DisplayText = "SLA N/A",
                ToolTip = "SLA could not be calculated because the created date or document type was unavailable."
            };
        }
    }
}