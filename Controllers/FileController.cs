using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MeterLibrary.Model.Display;
using MeterLibrary.Model.MeterReadings;
using MeterLibrary.Operations;
using MeterLibrary.Security;
using MeterSandbox.App_Start;
using Newtonsoft.Json;

namespace MeterSandbox.Controllers
{
    public class FileController : ApiController
    {
        private string connectionString = String.Empty;

        public FileController()
        {
            foreach (System.Configuration.ConnectionStringSettings c in System.Configuration.ConfigurationManager.ConnectionStrings)
            {
                if (string.Compare(c.Name, "SystemConnectionString", true) == 0)
                {
                    connectionString = c.ConnectionString;
                    DatabaseConnection.SetConnection(connectionString);// in NET5 would sort this out in middlewarwe just the once
                }
                    
            }
        }



        [AllowCrossSiteJson]
        public HttpResponseMessage Post()
        {
            
            HttpRequestMessage request = this.Request;

            

            if (request.Content.Headers.ContentType.MediaType == "text/csv")
            {
                CsvProvider csvProvider = new CsvProvider();
                int fullCount = 0;
                int errorCount = 0;
                List<string> results = csvProvider.LoadCSVFile(request, out fullCount, out errorCount);

                results.Insert(0, $" Total Meter Readings Found: {fullCount} Successfully Updated: {fullCount - errorCount}, Failures: {errorCount}");
                results.Insert(1, "  ");

                string json = JsonConvert.SerializeObject(results);

                return request.CreateResponse<string>(HttpStatusCode.OK, json);
                
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }





            
            
        }

        
    }
}
