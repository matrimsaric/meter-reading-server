using MeterLibrary.Model.MeterReadings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeterLibrary.Security.ValidationOperations
{
    internal class ValidateDuplicates : ValidationMaster
    {
        internal ValidateDuplicates()
        {
            this.warningMessage = "2nd entry on same day";
            this.targetClass = TARGET_CLASS.DUPLICATE;
        }

        internal bool LookForDuplicates(List<MeterReading> meterReadings, DateTime safeDateRead, int safeAccount)
        {
            MeterReading rec = meterReadings.Where(s => s.AccountNumber == safeAccount).FirstOrDefault();

            if (rec != null)
            {
                // same account, check date (y/m/d ignore m/s)
                if ((safeDateRead.Year == rec.DateRead.Year && safeDateRead.Month == rec.DateRead.Month
                    && safeDateRead.Day == rec.DateRead.Day))
                {
                    return true;
                }

            }
            return false;
        }

        internal override void Validate(string safeAccountString, string safeDateString, string safeMeterReadString, int arrayLength)
        {
            throw new NotImplementedException();
        }
    }
}
