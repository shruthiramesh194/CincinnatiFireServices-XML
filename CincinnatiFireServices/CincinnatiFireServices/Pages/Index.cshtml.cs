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
                List<Incident> alsIncident = new List<Incident>();
                IDictionary<string, QuickType.Incident> allIncidents = new Dictionary<string, QuickType.Incident>();
                IDictionary<long, QuickTypeHydrant.Hydrant> allHydrants = new Dictionary<long, QuickTypeHydrant.Hydrant>();
                string jsonString = webClient.DownloadString("https://data.cincinnati-oh.gov/resource/vnsz-a3wp.json");
                List<QuickType.Incident> incidents = QuickType.Incident.FromJson(jsonString);
                foreach (QuickType.Incident incident in incidents)
                {
                    allIncidents.Add(incident.EventNumber, incident);
                }

                JSchema schema = JSchema.Parse(System.IO.File.ReadAllText("incidentjsonschema.json"));
                JArray jsonArray = JArray.Parse(jsonString);
                IList<string> validationEvents = new List<string>();
                if(jsonArray.IsValid(schema))
                {
                
                    ViewData["allIncidents"] = incidents;
                }
                else
                {
                    foreach(string evt in validationEvents)
                    {
                        Console.WriteLine(evt);
                    }
                    ViewData["allIncidents"] = new List<Incident>();
                }

                string hydrantjson = webClient.DownloadString("https://data.cincinnati-oh.gov/resource/qhw6-ujsg.json");
                List<QuickTypeHydrant.Hydrant> hydrantsList = QuickTypeHydrant.Hydrant.FromJson(hydrantjson);
                JSchema hydrantschema = JSchema.Parse(System.IO.File.ReadAllText("hydrantjsonschema.json"));
                JArray hydrantJsonArray = JArray.Parse(hydrantjson);
                IList<string> hydrantValidationEvents = new List<string>();

                foreach (QuickTypeHydrant.Hydrant hydrant in hydrantsList)
                {
                    allHydrants.Add(hydrant.Objectid, hydrant);
                }

                if (hydrantJsonArray.IsValid(hydrantschema))
                {

                    ViewData["allHydrants"] = hydrantsList;
                }
                else
                {
                    foreach (string evt in hydrantValidationEvents)
                    {
                        Console.WriteLine(evt);
                    }
                    ViewData["allHydrants"] = new List<QuickTypeHydrant.Hydrant>();
                }

            }

        }
    }
}
