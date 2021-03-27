using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using QuickType;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CincinnatiFireServices.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            using (var webClient = new WebClient())
            {
                string jsonString = webClient.DownloadString("https://data.cincinnati-oh.gov/resource/vnsz-a3wp.json");
                JSchema schema = JSchema.Parse(System.IO.File.ReadAllText("incidentjsonschema.json"));
                JArray jsonArray = JArray.Parse(jsonString);
                IList<string> validationEvents = new List<string>();
                if(jsonArray.IsValid(schema))
                {
                
                    var incident = Incident.FromJson(jsonString);
                    ViewData["Incidents"] = incident;
                }
                else
                {
                    foreach(string evt in validationEvents)
                    {
                        Console.WriteLine(evt);
                    }
                    ViewData["Incidents"] = new List<Incident>();
                }
                
             }
        
        }
    }
}
