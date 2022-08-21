using System;
using System.Collections.Generic;
using System.Text;

namespace MeterLibrary.Security.ValidationOperations
{
    internal class AccountParsed : ValidationMaster
    {
        internal int parsedAccount;

        internal AccountParsed()
        {
            this.warningMessage = "Invalid Account Number";
            this.targetClass = TARGET_CLASS.INVALID_ACCOUNT;
        }

        internal override void Validate(string safeAccountString, string safeDateString, string safeMeterReadString, int arrayLength)
        {
            if (!base.SizeMatches(arrayLength, 1)) {
                this.validationStatus = VALIDATION_STATUS.VALIDATION_FAILED;
            }
            else
            {
                bool result = Int32.TryParse(safeAccountString, out parsedAccount);

                if (result == true) this.validationStatus = VALIDATION_STATUS.VALIDATION_PASSED;
                else this.validationStatus = VALIDATION_STATUS.VALIDATION_FAILED;
            }

        }
    }
}
