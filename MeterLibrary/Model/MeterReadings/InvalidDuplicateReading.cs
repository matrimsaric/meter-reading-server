using MeterLibrary.Model.Display;
using MeterLibrary.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MeterLibrary.Model.MeterReadings
{
    internal class InvalidDuplicateReading : MeterReading
    {

        internal InvalidDuplicateReading(int safeAccount, DateTime safeDateRead, int safeMeterValue) : base(safeAccount, safeDateRead, safeMeterValue)
        {

        }

        public void InvalidRecord() { }
        internal override string GetSummaryLine()
        {
            return $"Account: {AccountNumber.ToString()} Date Reported: {DateRead.ToShortDateString()} Meter Value Provided: {MeterValue.ToString()} - {"Duplicate Meter Reading"}";
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
            cmd.Parameters.Add("@meterValueProvided", SqlDbType.NVarChar).Value = MeterValue.ToString();

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
                meterValueSet = MeterValue.ToString(),
                successfull = false,
                reason = "Duplicate Meter Read Account/Date"
            };
            return responseLine;
        }


    }
}
