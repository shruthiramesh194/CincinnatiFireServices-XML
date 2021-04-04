using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using IncidentAPI;
using System;
using System.Collections.Generic;
using System.Net;

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
                //Creating Dictionary for future use
                IDictionary<string, IncidentAPI.Incident> allIncidents = new Dictionary<string, IncidentAPI.Incident>();
                IDictionary<long, QuickTypeHydrant.Hydrant> allHydrants = new Dictionary<long, QuickTypeHydrant.Hydrant>();
                //downloading incident json string from source
                string incidentJsonString = webClient.DownloadString("https://data.cincinnati-oh.gov/resource/vnsz-a3wp.json");
                List<IncidentAPI.Incident> incidents = IncidentAPI.Incident.FromJson(incidentJsonString);
                //adding incident objects to dictionary
                foreach (IncidentAPI.Incident incident in incidents)
                {
                    allIncidents.Add(incident.EventNumber, incident);
                }

                //parsing the json schema for incidents
                JSchema schema = JSchema.Parse(System.IO.File.ReadAllText("incidentjsonschema.json"));
                JArray incidentArray = JArray.Parse(incidentJsonString);
                IList<string> validationEvents = new List<string>();
                //validating with the json schema
                if (incidentArray.IsValid(schema))
                {

                    ViewData["allIncidents"] = incidents;
                }
                else
                {
                    foreach (string invalidEvent in validationEvents)
                    {
                        Console.WriteLine(invalidEvent);
                    }
                    ViewData["allIncidents"] = new List<IncidentAPI.Incident>();
                }
                //downloading hydrant json string from source
                string hydrantjson = webClient.DownloadString("https://data.cincinnati-oh.gov/resource/qhw6-ujsg.json");
                List<QuickTypeHydrant.Hydrant> hydrantsList = QuickTypeHydrant.Hydrant.FromJson(hydrantjson);
                //parsing the json schema for hydrants
                JSchema hydrantschema = JSchema.Parse(System.IO.File.ReadAllText("hydrantjsonschema.json"));
                JArray hydrantJsonArray = JArray.Parse(hydrantjson);
                IList<string> hydrantValidationEvents = new List<string>();
                //adding hydrant objects to dictionary
                foreach (QuickTypeHydrant.Hydrant hydrant in hydrantsList)
                {
                    allHydrants.Add(hydrant.Objectid, hydrant);
                }
                //validating with the json schema
                if (hydrantJsonArray.IsValid(hydrantschema))
                {

                    ViewData["allHydrants"] = hydrantsList;
                }
                else
                {
                    foreach (string invalidEvent in hydrantValidationEvents)
                    {
                        Console.WriteLine(invalidEvent);
                    }
                    ViewData["allHydrants"] = new List<QuickTypeHydrant.Hydrant>();
                }

            }

        }
    }
}