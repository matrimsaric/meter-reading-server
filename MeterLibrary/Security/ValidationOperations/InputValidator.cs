using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace MeterLibrary.Security.ValidationOperations
{
    public static class InputValidator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unsafeString"></param>
        /// <param name="blockContinue">if block continue any illegal string will stop continuation</param>
        /// <returns></returns>
        public static string ValidateString(string unsafeString, bool blockContinuation)
        {
            
            bool matched = Regex.IsMatch(unsafeString, @"^[\w\\\.]*$");

            if (!matched)
            {
                if (blockContinuation) throw new HttpRequestException($"Invalid Parameter: {unsafeString}");
                else return "UNSAFE";
            }
                

            return unsafeString;// checked now safe
        }

       
    }
}
