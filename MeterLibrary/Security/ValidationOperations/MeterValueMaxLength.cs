using System;
using System.Collections.Generic;
using System.Text;

namespace MeterLibrary.Security.ValidationOperations
{
    internal class MeterValueMaxLength : ValidationMaster
    {
        internal MeterValueMaxLength()
        {
            this.warningMessage = "Meter Reading Exceeds permitted limits";
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
                if (safeMeterReadString.Length > 6)
                {
                    this.validationStatus = VALIDATION_STATUS.VALIDATION_FAILED;
                }
                else this.validationStatus = VALIDATION_STATUS.VALIDATION_PASSED;
            }
            

        }
    }
}
