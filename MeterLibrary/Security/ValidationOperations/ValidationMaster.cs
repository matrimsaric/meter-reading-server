using System;
using System.Collections.Generic;
using System.Text;

namespace MeterLibrary.Security.ValidationOperations
{
    internal abstract class ValidationMaster
    {
        internal enum VALIDATION_STATUS { NOT_APPLICABLE = 0,VALIDATION_PASSED = 1, VALIDATION_FAILED = 2};
        internal enum TARGET_CLASS { LIVE = 0, INVALID_ACCOUNT = 1, INVALID_DATE = 2, DUPLICATE = 3, TOO_EARLY = 4, INVALID_METER = 5}

        internal VALIDATION_STATUS validationStatus = VALIDATION_STATUS.NOT_APPLICABLE;
        internal string warningMessage = String.Empty;
        internal TARGET_CLASS targetClass = TARGET_CLASS.LIVE;


        internal abstract void Validate(string safeAccountString, string safeDateString, string safeMeterReadString, int arrayLength);

        internal bool SizeMatches(int targetArrayLength, int requiredSize)
        {
            if (requiredSize <= targetArrayLength) return true;
            return false;
        }
    }
}
