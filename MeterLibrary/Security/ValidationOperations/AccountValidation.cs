using MeterLibrary.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MeterLibrary.Security.ValidationOperations
{
    internal class AccountValidation : ValidationMaster
    {
        internal AccountValidation()
        {
            this.warningMessage = "Invalid/Non-existant Account";
            this.targetClass = TARGET_CLASS.INVALID_ACCOUNT;
        }
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
SELECT [AccountId],[AccountStatus] FROM [Account] WHERE AccountId = @account
";
                SqlParameter acctParam = new SqlParameter("@account", safeAccountString);
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(acctParam);
                DataTable data = client.GetData(sql, sqlParams: paramList);

                if (data.Rows.Count == 1) this.validationStatus = VALIDATION_STATUS.VALIDATION_PASSED;
                else this.validationStatus = VALIDATION_STATUS.VALIDATION_FAILED;
            }
            
        }
    }
}
