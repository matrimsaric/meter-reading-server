using System;
using System.Collections.Generic;
using System.Text;

namespace MeterLibrary.Security.ValidationOperations
{
    internal class ValidationBuilder
    {
        internal List<ValidationMaster> GenerateValidations()
        {
            AccountParsed acctParse = new AccountParsed();
            AccountValidation acctValidate = new AccountValidation();
            DateParsed dateParse = new DateParsed();
            DateValidation dateValidation = new DateValidation();
            MeterValueParsed meterParsed = new MeterValueParsed();
            MeterValueMaxLength meterValueMaxLength = new MeterValueMaxLength();

            List < ValidationMaster > validationsToRun = new List<ValidationMaster>();
            validationsToRun.Add(acctParse);
            validationsToRun.Add(acctValidate);
            validationsToRun.Add(dateParse);
            validationsToRun.Add(dateValidation);
            validationsToRun.Add(meterParsed);
            validationsToRun.Add(meterValueMaxLength);

            return validationsToRun;


        }
    }
}
