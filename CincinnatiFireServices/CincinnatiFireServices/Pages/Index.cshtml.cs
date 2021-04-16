using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using QuickType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CincinnatiFireServices.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public string error;

        public void OnGet()
        {
            using (var webClient = new WebClient())
            {
                //Creating Dictionary for future use
                IDictionary<string, QuickType.Incident> allIncidents = new Dictionary<string, QuickType.Incident>();
                IDictionary<long, QuickTypeHydrant.Hydrant> allHydrants = new Dictionary<long, QuickTypeHydrant.Hydrant>();
                ViewData["allIncidents"] = new List<Incident>();
                ViewData["allHydrants"] = new List<QuickTypeHydrant.Hydrant>();
                //downloading incident json string from source

                try
                {
                    string incidentJsonString = webClient.DownloadString("https://data.cincinnati-oh.gov/resource/vnsz-a3wp.json");
                    List<QuickType.Incident> incidents = QuickType.Incident.FromJson(incidentJsonString);
                    List<string> neighborhoodList = new List<string>();
                    //adding incident objects to dictionary
                    foreach (QuickType.Incident incident in incidents)
                    {
                        allIncidents.Add(incident.EventNumber, incident);
                        neighborhoodList.Add(incident.Neighborhood);

                    }

                    //HttpContext.Session.Set("neighborhoodList", byte[](neighborhoodList.Distinct().ToList().ToArray));

                    ViewData["neighborhoodList"] = neighborhoodList.Distinct().ToList();
                    //parsing the json schema for incidents
                    JSchema schema = JSchema.Parse(System.IO.File.ReadAllText("incidentjsonschema.json"));
                    JArray jsonArray = JArray.Parse(incidentJsonString);
                    IList<string> validationEvents = new List<string>();
                    //validating with the json schema
                    if(jsonArray.IsValid(schema))
                    {
                        ViewData["allIncidents"] = incidents;
                    }
                    else
                    {
                        foreach(string evt in validationEvents)
                        {
                            ViewData["allIncidents"] = incidents;
                        }
                    }
                }
                catch(Exception ex)
                {
                    error = "Something went wrong! Unable to retrieve incidents list";
                    Console.WriteLine(ex.Message);
                }

                try
                {
                    string hydrantJsonString = webClient.DownloadString("https://data.cincinnati-oh.gov/resource/qhw6-ujsg.json");
                    List<QuickTypeHydrant.Hydrant> hydrantsList = QuickTypeHydrant.Hydrant.FromJson(hydrantJsonString);
                    //parsing the json schema for hydrants
                    JSchema hydrantSchema = JSchema.Parse(System.IO.File.ReadAllText("hydrantjsonschema.json"));
                    JArray hydrantJsonArray = JArray.Parse(hydrantJsonString);
                    //adding hydrant objects to dictionary
                    foreach (QuickTypeHydrant.Hydrant hydrant in hydrantsList)
                    {
                        allHydrants.Add(hydrant.Objectid, hydrant);
                    }
                    
                    //validating with the json schema
                    if (hydrantJsonArray.IsValid(hydrantSchema))
                    {
                        ViewData["allHydrants"] = hydrantsList;
                    }
                } 
                catch(Exception ex)
                {
                    error = "Something went wrong! Unable to retrieve hydrants list";
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
