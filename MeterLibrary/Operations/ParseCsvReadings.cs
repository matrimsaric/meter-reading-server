using MeterLibrary.Model.MeterReadings;
using MeterLibrary.Security;
using MeterLibrary.Security.ValidationOperations;
using MeterLibrary.SqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;


namespace MeterLibrary.Operations
{
    internal class ParseCsvReadings : ParseReader
    {
        private const int ACCOUNT_POS = 0;
        private const int DATE_POS = 1;
        private const int METER_VALUE_POS = 2;

        internal override string GetOperationType()
        {
            return "CSV";
        }
        internal override List<MeterReading> Parse(HttpRequestMessage request, out int fullCount, out int errorCount)
        {
            List<MeterReading> meterReadings = new List<MeterReading>();
            List<MeterReading> liveList = new List<MeterReading>();// tracks entries being saved to prevent errors generating duplicates
            fullCount = 0;
            errorCount = 0;
     
            using (var stream = request.Content.ReadAsStreamAsync())
            {
                using (var reader = new StreamReader(stream.Result))
                {
                    string line;
                    char[] splitter = new char[1];
                    int count = 0;
            
                    splitter[0] = ',';
                    while ((line = reader.ReadLine()) != null)
                    {
                        if(count == 0)
                        {
                            count += 1;
                            continue;// expecting header, could validate contents versus expected if chance of no header..
                        }
                        fullCount += 1;
                        string[] unsafeSplitArray = line.Split(splitter, StringSplitOptions.RemoveEmptyEntries);

                        string safeAccountStr = String.Empty;
                        string safeDateStr = String.Empty;
                        string safeMeterStr = String.Empty;

                        int safeAccountNumber = 0;
                        int safeMeterValue = 0;
                        DateTime safeDateRead = new DateTime(1900, 1, 1);

                        if (unsafeSplitArray.Length >= 3)
                        {
                            safeMeterStr = InputValidator.ValidateString(unsafeSplitArray[METER_VALUE_POS], false);
                            bool success = Int32.TryParse(unsafeSplitArray[METER_VALUE_POS], out safeMeterValue);
                        }
                        if (unsafeSplitArray.Length >= 2)
                        {
                            
                            bool success = DateTime.TryParse(unsafeSplitArray[DATE_POS], out safeDateRead);
                            safeDateStr = safeDateRead.ToString();
 
                        }
                        if (unsafeSplitArray.Length >= 1)
                        {
                            safeAccountStr = InputValidator.ValidateString(unsafeSplitArray[ACCOUNT_POS], false);
                            bool success = Int32.TryParse(unsafeSplitArray[ACCOUNT_POS], out safeAccountNumber);
                        }


                        // set up validators
                        ValidationBuilder valBuilder = new ValidationBuilder();
                        List<ValidationMaster> validationsToRun = valBuilder.GenerateValidations();
                        CreateMeterReading createMeterReadings = new CreateMeterReading();
                        bool errorFound = false;

                        foreach(ValidationMaster valToRun in validationsToRun)
                        {
                            valToRun.Validate(safeAccountStr, safeDateStr, safeMeterStr, unsafeSplitArray.Length);

                            if(valToRun.validationStatus == ValidationMaster.VALIDATION_STATUS.VALIDATION_FAILED)
                            {
                                meterReadings.Add(createMeterReadings.CreateErrorMeterReading(valToRun, safeAccountStr,
                                    safeDateStr, safeMeterStr, safeAccountNumber, safeDateRead, safeMeterValue ));
                                if (errorFound == false)
                                {
                                    errorCount += 1;
                                    errorFound = true;
                                }
                            }
                        }

                        // look for duplicates
                        ValidateDuplicates searchForDuplicates = new ValidateDuplicates();
                        if (searchForDuplicates.LookForDuplicates(liveList, safeDateRead, safeAccountNumber) == true)
                        {
                            meterReadings.Add(createMeterReadings.CreateErrorMeterReading(searchForDuplicates, safeAccountStr,
                                safeDateStr, safeMeterStr, safeAccountNumber, safeDateRead, safeMeterValue));
                            if(errorFound == false)
                            {
                                errorCount += 1;
                                errorFound = true;
                            }
                            
                        }
                        if(errorFound == false)
                        {
                            meterReadings.Add(new LiveRead(safeAccountNumber, safeDateRead, safeMeterValue));
                            liveList.Add(new LiveRead(safeAccountNumber, safeDateRead, safeMeterValue));
                        }

                        
                    }
                }
            }

            return meterReadings;
        }

    }
}
