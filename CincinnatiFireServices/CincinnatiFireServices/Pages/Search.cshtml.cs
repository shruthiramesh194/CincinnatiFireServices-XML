using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using QuickType;
using GoogleMaps.LocationServices;

namespace CincinnatiFireServices.Pages
{

    public class SearchModel : PageModel
    {
        IDictionary<string, QuickType.Incident> allIncidentsDict = new Dictionary<string, QuickType.Incident>();
        IDictionary<long, QuickTypeHydrant.Hydrant> allHydrantsDict = new Dictionary<long, QuickTypeHydrant.Hydrant>();
        public void OnGet()
        {
            using (var webClient = new WebClient())
            {
                //Creating Dictionary for future use

                //downloading incident json string from source
                string incidentJsonString = webClient.DownloadString("https://data.cincinnati-oh.gov/resource/vnsz-a3wp.json");
                List<QuickType.Incident> incidents = QuickType.Incident.FromJson(incidentJsonString);
                List<string> neighborhoodList = new List<string>();
                //adding incident objects to dictionary
                foreach (QuickType.Incident incident in incidents)
                {
                    allIncidentsDict.Add(incident.EventNumber, incident);
                    neighborhoodList.Add(incident.Neighborhood);

                }

                //HttpContext.Session.Set("neighborhoodList", byte[](neighborhoodList.Distinct().ToList().ToArray));

                ViewData["neighborhoodList"] = neighborhoodList.Distinct().ToList();
                //parsing the json schema for incidents
                JSchema schema = JSchema.Parse(System.IO.File.ReadAllText("incidentjsonschema.json"));
                JArray jsonArray = JArray.Parse(incidentJsonString);
                IList<string> validationEvents = new List<string>();
                //validating with the json schema
                if (jsonArray.IsValid(schema))
                {

                    ViewData["allIncidents"] = incidents;
                }
                else
                {
                    foreach (string evt in validationEvents)
                    {
                        Console.WriteLine(evt);
                    }
                    ViewData["allIncidents"] = new List<Incident>();
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
                    allHydrantsDict.Add(hydrant.Objectid, hydrant);
                }
                //validating with the json schema
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

        public void OnPost()
        {
            var neighborhood = Request.Form["neighborhood"];
            using (var webClient = new WebClient())
            {

                //downloading incident json string from source
                string incidentJsonString = webClient.DownloadString("https://data.cincinnati-oh.gov/resource/vnsz-a3wp.json");
                List<QuickType.Incident> incidents = QuickType.Incident.FromJson(incidentJsonString);
                List<string> neighborhoodList = new List<string>();

                //parsing the json schema for incidents
                JSchema schema = JSchema.Parse(System.IO.File.ReadAllText("incidentjsonschema.json"));
                JArray jsonArray = JArray.Parse(incidentJsonString);
                IList<string> validationEvents = new List<string>();
                List<Incident> neighborhoodIncidentList = new List<Incident>();
                List<QuickTypeHydrant.Hydrant> neighborhoodHydrantList = new List<QuickTypeHydrant.Hydrant>();
                //validating with the json schema
                if (jsonArray.IsValid(schema))
                {

                    ViewData["allIncidents"] = incidents;
                    //adding incident objects to dictionary
                    foreach (QuickType.Incident incident in incidents)
                    {
                        allIncidentsDict.Add(incident.EventNumber, incident);
                        neighborhoodList.Add(incident.Neighborhood);

                    }


                    //adding incident objects to dictionary
                    foreach (Incident inc in allIncidentsDict.Values)
                    {
                        if (String.Equals(inc.Neighborhood, neighborhood))
                        {
                            neighborhoodIncidentList.Add(inc);
                        }
                    }

                }
                else
                {
                    foreach (string evt in validationEvents)
                    {
                        Console.WriteLine(evt);
                    }
                    ViewData["allIncidents"] = new List<Incident>();
                }


                ViewData["neighborhoodList"] = neighborhoodList.Distinct().ToList();
                ViewData["neighborhoodIncidentList"] = neighborhoodIncidentList;

                //downloading hydrant json string from source
                string hydrantjson = webClient.DownloadString("https://data.cincinnati-oh.gov/resource/qhw6-ujsg.json");
                List<QuickTypeHydrant.Hydrant> hydrantsList = QuickTypeHydrant.Hydrant.FromJson(hydrantjson);
                //parsing the json schema for hydrants
                JSchema hydrantschema = JSchema.Parse(System.IO.File.ReadAllText("hydrantjsonschema.json"));
                JArray hydrantJsonArray = JArray.Parse(hydrantjson);
                IList<string> hydrantValidationEvents = new List<string>();

                //validating with the json schema
                if (hydrantJsonArray.IsValid(hydrantschema))
                {

                    ViewData["allHydrants"] = hydrantsList;

                    //adding hydrant objects to dictionary
                    foreach (QuickTypeHydrant.Hydrant hydrant in hydrantsList)
                    {
                        allHydrantsDict.Add(hydrant.Objectid, hydrant);
                    }

                    //adding incident objects to dictionary
                    foreach (QuickTypeHydrant.Hydrant hyd in allHydrantsDict.Values)
                    {
                        if (String.Equals(hyd.Neighborhood, neighborhood))
                        {
                            neighborhoodHydrantList.Add(hyd);

                        }
                    }
                }
                else
                {
                    foreach (string evt in hydrantValidationEvents)
                    {
                        Console.WriteLine(evt);
                    }
                    ViewData["allHydrants"] = new List<QuickTypeHydrant.Hydrant>();
                }

                ViewData["neighborhoodHydrantList"] = neighborhoodHydrantList;
            }

        }
    }
}
