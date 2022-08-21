using MeterLibrary.Model.MeterReadings;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MeterLibrary.Operations
{
    public class CsvProvider
    {
        private ParseReader reader = new ParseCsvReadings();

        public List<string> LoadCSVFile(HttpRequestMessage request,out int fullCount, out int errorCount)
        {
            List<string> errorReadings = new List<string>();
            fullCount = 0;
            errorCount = 0;
            
            List<MeterReading> fullList = reader.Parse(request, out fullCount, out errorCount);

            // save list to db
            MeterSaver meterSave = new MeterSaver();

            List<string> resultData = meterSave.SaveReadings(fullList, reader.GetOperationType());

            return resultData;
        }
    }
}
