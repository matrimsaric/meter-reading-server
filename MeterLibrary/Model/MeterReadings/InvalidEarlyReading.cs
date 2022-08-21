using MeterLibrary.Model.Display;
using MeterLibrary.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MeterLibrary.Model.MeterReadings
{
    internal class InvalidEarlyReading : MeterReading
    {
        private string invalidValue = String.Empty;

        internal InvalidEarlyReading(int safeAccount, string safeDateRead, string safeMeterValue) : base(safeAccount, DateTime.Now, 0)
        {
            invalidValue = safeDateRead;
        }

        public void InvalidRecord() { }
        internal override string GetSummaryLine()
        {
            return $"Account: {AccountNumber.ToString()} Date Reported: {DateRead.ToShortDateString()} Meter Value Provided: {invalidValue} - {"Date is earlier than existing entry"}";
        }

        internal override bool SaveMeterRead(string operation)
        {
            SqlClientOperations access = new SqlClientOperations();

            SqlCommand cmd = new SqlCommand("SaveMeterReadingInvalid");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@userId", SqlDbType.NVarChar).Value = Environment.UserName;
            cmd.Parameters.Add("@operation", SqlDbType.NVarChar).Value = operation;
            cmd.Parameters.Add("@accountValid", SqlDbType.Bit).Value = 1;
            cmd.Parameters.Add("@accountProvided", SqlDbType.NVarChar).Value = AccountNumber.ToString();
            cmd.Parameters.Add("@dateValid", SqlDbType.Bit).Value = 0;
            cmd.Parameters.Add("@dateProvided", SqlDbType.NVarChar).Value = invalidValue;
            cmd.Parameters.Add("@meterValueValid", SqlDbType.Bit).Value = 0;
            cmd.Parameters.Add("@meterValueProvided", SqlDbType.NVarChar).Value = "0";

            try
            {
                access.ExecuteCommand(cmd);
            }
            catch
            {
                // track exception eventually, for now return false so this can be added
                // to error list.
                return false;
            }
            return true;
        }

        internal override MeterDisplay GenerateDisplay()
        {
            MeterDisplay responseLine = new MeterDisplay
            {
                accountSet = AccountNumber.ToString(),
                dateSet = invalidValue,
                meterValueSet = String.Empty,
                successfull = false,
                reason = "Date is earlier than existing entry"
            };

            return responseLine;
        }


    }
}
