using MeterLibrary.Model.Display;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeterLibrary.Model.MeterReadings
{
    internal abstract class MeterReading 
    {
        internal int AccountNumber { get; set; }
        internal DateTime DateRead { get; set; }
        internal int MeterValue { get; set; }



        internal MeterReading(int safeAccount, DateTime safeDateRead,int safeMeterValue )
        {
            AccountNumber = safeAccount;
            DateRead = safeDateRead;
            MeterValue = safeMeterValue;
        }

        internal abstract MeterDisplay GenerateDisplay();
        internal abstract string GetSummaryLine();


        internal abstract bool SaveMeterRead(string operation);
    }
}
