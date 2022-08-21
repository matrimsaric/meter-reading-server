using MeterLibrary.Model.Display;
using MeterLibrary.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MeterLibrary.Model.MeterReadings
{
    internal class InvalidMeterReading : MeterReading
    {
        private string invalidValue = String.Empty;

        internal InvalidMeterReading(int safeAccount, DateTime safeDateRead, string safeMeterValue) : base(safeAccount, safeDateRead, 0)
        {
            invalidValue = safeMeterValue;
        }

        public void InvalidRecord() { }
        internal override string GetSummaryLine()
        {
            return $"Account: {AccountNumber.ToString()} Date Reported: {DateRead.ToShortDateString()} Meter Value Provided: {invalidValue} - {"Meter Reading invalid"}";
        }

        internal override bool SaveMeterRead(string operation)
        {
            SqlClientOperations access = new SqlClientOperations();

            SqlCommand cmd = new SqlCommand("SaveMeterReadingInvalid");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@userId", SqlDbType.NVarChar).Value = Environment.UserName;// NOTE this is the same currently as server user. When User has to log in that will be used instead.
            cmd.Parameters.Add("@operation", SqlDbType.NVarChar).Value = operation;
            cmd.Parameters.Add("@accountValid", SqlDbType.Bit).Value = 1;
            cmd.Parameters.Add("@accountProvided", SqlDbType.NVarChar).Value = AccountNumber.ToString();
            cmd.Parameters.Add("@dateValid", SqlDbType.Bit).Value = 1;
            cmd.Parameters.Add("@dateProvided", SqlDbType.NVarChar).Value = DateRead.ToString();
            cmd.Parameters.Add("@meterValueValid", SqlDbType.Bit).Value = 0;
            cmd.Parameters.Add("@meterValueProvided", SqlDbType.NVarChar).Value = invalidValue;

            try
            {
                access.ExecuteCommand(cmd);
            }
            catch 
            {
                // track exceptions eventually
                return false;
            }
            return true;
        }

        internal override MeterDisplay GenerateDisplay()
        {
            MeterDisplay responseLine = new MeterDisplay
            {
                accountSet = AccountNumber.ToString(),
                dateSet = DateRead.ToShortDateString(),
                meterValueSet = invalidValue,
                successfull = false,
                reason = "Meter Reading invalid"
            };
            return responseLine;
        }

        
    }
}
