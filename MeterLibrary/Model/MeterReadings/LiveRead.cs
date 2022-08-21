using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MeterLibrary.SqlClient;
using MeterLibrary.Model.Display;

namespace MeterLibrary.Model.MeterReadings
{
    internal class LiveRead : MeterReading
    {
        internal LiveRead(int safeAccount, DateTime safeDateRead, int safeMeterValue) : base(safeAccount, safeDateRead, safeMeterValue)
        {

        }
        internal override MeterDisplay GenerateDisplay()
        {
            MeterDisplay responseLine = new MeterDisplay
            {
                accountSet = AccountNumber.ToString(),
                dateSet = DateRead.ToShortDateString(),
                meterValueSet = MeterValue.ToString(),
                successfull = true,
                reason = ""
            };
            return responseLine;
        }

        internal override string GetSummaryLine()
        {
            return $"Account: {AccountNumber.ToString()} Date Reported: {DateRead.ToShortDateString()} Meter Value Provided: {MeterValue.ToString()} - {"Good"}";
        }

        internal override bool SaveMeterRead(string operation)
        {
            SqlClientOperations access = new SqlClientOperations();

            SqlCommand cmd = new SqlCommand("SaveMeterReading");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@accountId", SqlDbType.Int).Value = AccountNumber;
            cmd.Parameters.Add("@dateTaken", SqlDbType.DateTime).Value = DateRead;
            cmd.Parameters.Add("@meterValue", SqlDbType.Int).Value = MeterValue;
            cmd.Parameters.Add("@userId", SqlDbType.NVarChar).Value = Environment.UserName;// NOTE this is the same currently as server user. When User has to log in that will be used instead.
            cmd.Parameters.Add("@operation", SqlDbType.NVarChar).Value = operation;

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
    }
}
