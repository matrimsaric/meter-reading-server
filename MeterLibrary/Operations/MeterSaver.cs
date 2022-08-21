using MeterLibrary.Model.Display;
using MeterLibrary.Model.MeterReadings;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeterLibrary.Operations
{
    internal class MeterSaver
    {
        internal List<string> SaveReadings(List<MeterReading> liveReads, string operation)
        {
            List<string> errorReadings = new List<string>();
            foreach(MeterReading met in liveReads)
            {
                Type readType = met.GetType();

                var errorMethod = readType.GetMethod("InvalidRecord");

                bool success = met.SaveMeterRead(operation);

                if (errorMethod != null || success == false) errorReadings.Add(met.GetSummaryLine());
                
            }

            return errorReadings;
        }

    }
}
