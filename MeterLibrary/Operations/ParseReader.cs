using MeterLibrary.Model.MeterReadings;
using MeterLibrary.Security;
using MeterLibrary.Security.ValidationOperations;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MeterLibrary.Operations
{
    internal abstract class ParseReader
    {
        internal abstract string GetOperationType();


        internal abstract List<MeterReading> Parse(HttpRequestMessage request, out int fullCount, out int errorCount);
    }
}
