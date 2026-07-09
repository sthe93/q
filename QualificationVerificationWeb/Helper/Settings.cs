using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualificationVerificationWeb.Helper
{
   public class Settings
    {
        public static string GetSQByIdNumber
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["GetStudentQualificationByIdNumber"].ToString();
            }

        }

        public static string GetSQByStudentNumber
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["GetStudentQualificationByStudentNumber"].ToString();
            }

        }

        public static string GetStudentQualificationAPI
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["GetStudentQualification"].ToString();
            }

        }

        public static string GetOutstandingFees
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["GetStudentFeeStatus"].ToString();
            }

        }  
        public static string GetPayGateUrl
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["PayGateUrl"].ToString();
            }

        }

        public static string GetPayGateQueryUrl
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["PayGateQueryUrl"].ToString();
            }

        }

        public static string ReturnUrl
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ReturnUrl"].ToString();
            }

        }

        public static string GetPayGateId
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["PayGateId"].ToString();
            }

        }        
        
        public static string GetEncryptionKey
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["EncryptionKey"].ToString();
            }

        }
    }
}
