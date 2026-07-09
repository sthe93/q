using System;
using System.Collections.Generic; 
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QualificationVerificationWeb.Helper
{
    public class Validator
    {
        public int ID { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
    }

    public class Files
    {
        public string FileName { set; get; }
        public string ContentType { set; get; }
        public Byte[] Data { set; get; }
        public string extension { set; get; }
    }
    public class AttachmentObject
    {
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public Guid EventId { get; set; }
        public string Extension { get; set; }
        public string Name { get; set; }
        public Guid ID { get; set; }
    }

   public enum Status
    {
        Active = 0,
        Inactive = 1,
        Deleted = 2,
        Pending = 3,
        Expired = 4,
        Archive =5,
        Inprogress =6,
        Completed = 7,

    }

   public enum FeesGrantStatus
   {
       Pending = 0,
       All =1,
       InOrder = 2,
       NotInOrder = 3, 
       Complete = 4,
       FinancialBlock = 5,
       Draft = 6,
    }

   public enum AppointmentStatus
   {
       Booked = 0,
       Arrived = 1,
       NoShow = 2,
       ClientReschedule = 3,
       ClientCancel = 4,
       ProfessionalReschedule = 5,
       ProfessionalCancel = 6,
       Cancelled = 8
   }

   public enum ClientType
   {
       External =0,
       Staff =1,
       Student = 2
   }

   public enum Race
   { 
       African,
       Asian,
       Chinese,
       Coloured,
       Indian,
       Unknown,
       White
       
   }
   public enum Campus
   {
       APB,
       APK,
       DFC,
       SWC
   }

   public enum Gender
   {
       Female,
       Male

   }
   public enum FeesGrantDocumentType
   {
       //RE-ARANGING BELOW WILL AFFECT RESULTS TO AND FROM THE DATABASE
       IDCopy = 1,
       ConsentForm = 2,
       PaymentProof = 3,
       SpecialInstruction = 4
    }
    public class Constants
    {
        public const string Success = "Success";
        public const string Error = "Error";
        public const string Failure = "Failure";
        public const string FileName = "EventFile";
        public const string SuccessMessage = "Record Saved Successfully";
        public const string Yes = "Yes";
        public const string No = "No";
        public const string Exist = "Exist";
        public const string NotExist = "NotExist";
        public const string NotRecord = "Not record found";
        public const int EmailSent = 1;
        public const int EmailPending = 1;
        public const string ALL = "All";
        public const string AcademicRecordDecline = "Academic Record Decline";
        public const string TranscriptSupplementDecline = "Transcript Supplement Decline";
        public const string ConfirmationLetterDecline = "Confirmation Letter Decline";
        public const string DocumentsOrderDecline = "Documents Order Decline";
        public const string Currency = "ZAR";
        public const string Locale = "en-za";
        public const string Country = "ZAF";
        //public const string ReturnUrl = "/AcademicRequest/OnlinePayment";
      
    }

    public enum ResultCodes
    {
        Call_for_Approval = 900001,
        Card_Expired = 900002,
        Insufficient_Funds = 900003,
        Invalid_Card_Number = 900004,
        Bank_Interface_Timeout = 900005,
        Invalid_Card = 900006,
        Declined = 900007,
        Lost_Card = 900009,
        Invalid_Card_Length = 900010,
        Suspected_Fraud = 900011,
        Card_Reported_as_Stolen = 900012,
        Restricted_Card = 900013,
        Excessive_Card_Usage = 900014,
        Card_Blacklisted = 900015,
        Declined_Authentication_failed = 900207,
        Auth_Declined = 990020,
        ThreeD_Secure_Lookup_Timeout = 900210,
        Invalid_expiry_date = 991001,
        Invalid_amount = 991002
    }

    public class Helper
    {
        public static bool IsEmailValid(string email)
        {
            try
            {
                return Regex.IsMatch(email,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }        

        public static int DaysLeft(DateTime startDate, DateTime endDate, Boolean excludeWeekends, List<DateTime> excludeDates)
        {
            int count = 0;
            for (DateTime index = startDate; index < endDate; index = index.AddDays(1))
            {
                if (excludeWeekends && index.DayOfWeek != DayOfWeek.Sunday && index.DayOfWeek != DayOfWeek.Saturday)
                {
                    bool excluded = false; ;
                    for (int i = 0; i < excludeDates.Count; i++)
                    {
                        if (index.Date.CompareTo(excludeDates[i].Date) == 0)
                        {
                            excluded = true;
                            break;
                        }
                    }

                    if (!excluded)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        //public List<StudentQualification> GetStudentQualificationByStudentNum (string studentNumber)
        //{
        //    HttpHelper _httpHelper = new HttpHelper();
        //    string url = Settings.GetSQByStudentNumber + studentNumber;
        //    var results = HttpHelper.HttpCallJson<List<StudentQualification>>(url, "Get");

        //    //_3YearsPropertyList = BusinessRules.HttpHelper.HttpCallJson<List<DataObjects.Property>>(_3YearsUrl, WebRequestMethods.Http.Get);
        //    //var 3YearAccreditations = 3YearsPropertyList.Where(o => o.StartYear != currentRegistrationYear && Convert.ToInt32(o.CurrentYear) == currentRegistrationYear).ToList();

        //    return results.ToList();
        //}
    }
}
