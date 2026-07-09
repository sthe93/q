using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace QualificationVerificationWeb.ViewModels
{
    public class EmailBuilder
    {
       
            public string Requester { get; set; }
            public string DeclineReason { get; set; }
            public string RefNumber { get; set; }

            public string EmailBodyBuilder(string emailBody)
            {
                var variables = new Dictionary<string, string>
                {
                    ["[Requester]"] = CapitalizeWords(Requester),
                    ["[DeclineReason]"] = DeclineReason,
                    ["[RefNumber]"] = RefNumber
                };
                foreach (var variable in variables)
                {
                    string pattern = Regex.Escape(variable.Key);
                    emailBody = Regex.Replace(emailBody, pattern, variable.Value, RegexOptions.IgnoreCase);
                }

                return emailBody;
            }
            static string CapitalizeWords(string input)
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                return textInfo.ToTitleCase(input.ToLower());
            }
       
    }
}