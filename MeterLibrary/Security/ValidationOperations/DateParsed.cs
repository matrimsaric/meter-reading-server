using System;
using System.Collections.Generic;
using System.Text;

namespace MeterLibrary.Security.ValidationOperations
{
    internal class DateParsed : ValidationMaster
    {
        internal DateTime parsedDate;


        internal DateParsed()
        {
            this.warningMessage = "Invalid Date";
            this.targetClass = TARGET_CLASS.INVALID_DATE;
        }

        internal override void Validate(string safeAccountString, string safeDateString, string safeMeterReadString, int arrayLength)
        {
            if (!base.SizeMatches(arrayLength, 1))
            {
                this.validationStatus = VALIDATION_STATUS.VALIDATION_FAILED;
            }
            else
            {
                bool result = DateTime.TryParse(safeDateString, out parsedDate);

                if (result == true) this.validationStatus = VALIDATION_STATUS.VALIDATION_PASSED;
                else this.validationStatus = VALIDATION_STATUS.VALIDATION_FAILED;
            }
            
        }
    }
}
