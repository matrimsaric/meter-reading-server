using MeterLibrary.Model.MeterReadings;
using MeterLibrary.Security.ValidationOperations;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeterLibrary.Operations
{
    internal class CreateMeterReading
    {
        internal MeterReading CreateErrorMeterReading(ValidationMaster failClass, string safeAccount,
            string safeDate, string safeMeterRead, int safeAccountNumber, DateTime safeDateRead, int safeMeterValue)
        {
            MeterReading created = null;
            switch (failClass.targetClass)
            {
                case ValidationMaster.TARGET_CLASS.DUPLICATE:
                    created = new InvalidDuplicateReading(safeAccountNumber, safeDateRead, safeMeterValue);
                    break;
                case ValidationMaster.TARGET_CLASS.INVALID_ACCOUNT:
                    created = new InvalidAccountReading(safeAccount, safeDate, safeMeterRead);
                    break;
                case ValidationMaster.TARGET_CLASS.INVALID_DATE:
                    created = new InvalidDateReading(safeAccountNumber, safeDate, safeMeterRead);
                    break;
                case ValidationMaster.TARGET_CLASS.INVALID_METER:
                    created = new InvalidMeterReading(safeAccountNumber, safeDateRead, safeMeterRead);
                    break;
                case ValidationMaster.TARGET_CLASS.TOO_EARLY:
                    created = new InvalidEarlyReading(safeAccountNumber, safeDate, safeMeterRead);
                    break;
            }
            return created;
        }


    }
}
