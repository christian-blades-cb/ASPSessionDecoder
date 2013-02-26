using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SessionDecoder.Session;
using System.Configuration;
using Newtonsoft.Json;

namespace SessionDecoder
{
    class Program
    {
        static void Main(string[] args)
        {
            var outputFileName = ConfigurationManager.AppSettings["outputFile"];
            var connectionString = ConfigurationManager.ConnectionStrings["CBToolSession"];

            var sessions = Session.Session.Retrieve(connectionString.ConnectionString);
            System.IO.File.WriteAllText(outputFileName, JsonConvert.SerializeObject(sessions, Formatting.Indented));
        }
    }
}
