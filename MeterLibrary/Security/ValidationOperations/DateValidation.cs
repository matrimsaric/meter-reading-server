using MeterLibrary.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MeterLibrary.Security.ValidationOperations
{
    internal class DateValidation : ValidationMaster
    {
        internal DateValidation()
        {
            this.warningMessage = "Meter Reading Earlier than existing Reading";
            this.targetClass = TARGET_CLASS.INVALID_DATE;
        }
//        internal bool ValidateDate(int account, DateTime meterRead)
//        {

//            SqlClientOperations client = new SqlClientOperations();

//            string sql = @"
//SELECT 1 FROM [MeterReadings] WHERE [AccountId] = @account AND [DateTaken] >= @dateOfReading
//";
//            SqlParameter acctParam = new SqlParameter("@account", account);
//            SqlParameter dateParam = new SqlParameter("@dateOfReading", meterRead);
//            List<SqlParameter> paramList = new List<SqlParameter>();
//            paramList.Add(acctParam);
//            paramList.Add(dateParam);
//            DataTable data = client.GetData(sql, sqlParams: paramList);

//            if (data.Rows.Count == 1)
//            {
//                return false;
//            }



//            return true;
//        }

        internal override void Validate(string safeAccountString, string safeDateString, string safeMeterReadString, int arrayLength)
        {
            if (!base.SizeMatches(arrayLength, 1))
            {
                this.validationStatus = VALIDATION_STATUS.VALIDATION_FAILED;
            }
            else
            {
                SqlClientOperations client = new SqlClientOperations();

                string sql = @"
SELECT 1 FROM [MeterReadings] WHERE [AccountId] = @account AND [DateTaken] >= @dateOfReading
";
                SqlParameter acctParam = new SqlParameter("@account", safeAccountString);
                DateTime useDate;
                bool isDate = DateTime.TryParse(safeDateString, out useDate);

                if (isDate == true)
                {
                    SqlParameter dateParam = new SqlParameter("@dateOfReading", useDate);
                    List<SqlParameter> paramList = new List<SqlParameter>();
                    paramList.Add(acctParam);
                    paramList.Add(dateParam);
                    DataTable data = client.GetData(sql, sqlParams: paramList);

                    if (data.Rows.Count == 1)
                    {

                        this.validationStatus = VALIDATION_STATUS.VALIDATION_FAILED;
                    }
                    else
                    {
                        this.validationStatus = VALIDATION_STATUS.VALIDATION_PASSED;
                    }

                }
                else
                {
                    this.validationStatus = VALIDATION_STATUS.VALIDATION_PASSED;
                }
            }
            

        }
    }
}
