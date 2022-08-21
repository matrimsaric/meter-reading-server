using System;
using System.Collections.Generic;
using System.Text;

namespace MeterLibrary.Security.ValidationOperations
{
    internal class MeterValueParsed : ValidationMaster
    {
        internal int parsedMeterValue;

        internal MeterValueParsed()
        {
            this.warningMessage = "Meter Reading Invalid";
            this.targetClass = TARGET_CLASS.INVALID_METER;
        }

        internal override void Validate(string safeAccountString, string safeDateString, string safeMeterReadString, int arrayLength)
        {
            if (!base.SizeMatches(arrayLength, 1))
            {
                this.validationStatus = VALIDATION_STATUS.VALIDATION_FAILED;
            }
            else
            {
                bool result = Int32.TryParse(safeMeterReadString, out parsedMeterValue);

                if (result == true) this.validationStatus = VALIDATION_STATUS.VALIDATION_PASSED;
                else this.validationStatus = VALIDATION_STATUS.VALIDATION_FAILED;
            }
            

        }
    }
}
