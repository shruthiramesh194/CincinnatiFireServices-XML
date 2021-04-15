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

namespace CincinnatiFireServices.Pages
{
    public class SearchTypeModel : PageModel

    {
        IDictionary<string, QuickType.Incident> allIncidentsDict = new Dictionary<string, QuickType.Incident>();
        public void OnGet()
        {
            using (var webClient = new WebClient())
            {
                //Creating Dictionary for future use

                //downloading incident json string from source
                string incidentJsonString = webClient.DownloadString("https://data.cincinnati-oh.gov/resource/vnsz-a3wp.json");
                List<QuickType.Incident> incidents = QuickType.Incident.FromJson(incidentJsonString);
              
                List<string> serviceList = new List<string>();

                //adding incident objects to dictionary
                foreach (QuickType.Incident incident in incidents)
                {
                    serviceList.Add(incident.CfdIncidentType);

                }

                //HttpContext.Session.Set("neighborhoodList", byte[](neighborhoodList.Distinct().ToList().ToArray));

                ViewData["serviceList"] = serviceList.ToList();


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
               

            }
        }


        public void OnPost()
        {
            

            var medicservice = Request.Form["medicservice"];
            using (var webClient = new WebClient())
            {

                //downloading incident json string from source
                string incidentJsonString = webClient.DownloadString("https://data.cincinnati-oh.gov/resource/vnsz-a3wp.json");
                List<QuickType.Incident> incidents = QuickType.Incident.FromJson(incidentJsonString);
             
                List<string> serviceList = new List<string>();

                //parsing the json schema for incidents
                JSchema schema = JSchema.Parse(System.IO.File.ReadAllText("incidentjsonschema.json"));
                JArray jsonArray = JArray.Parse(incidentJsonString);
                IList<string> validationEvents = new List<string>();

                List<Incident> serviceIncidentList = new List<Incident>();
              
                //validating with the json schema
                if (jsonArray.IsValid(schema))
                {

                   
                    //adding incident objects to dictionary
                    foreach (QuickType.Incident incident in incidents)
                    {
                        allIncidentsDict.Add(incident.EventNumber, incident);
                      
                        serviceList.Add(incident.CfdIncidentType);

                    }


                    //adding incident objects to dictionary
                    foreach (Incident incid in allIncidentsDict.Values)
                    {
                        if (String.Equals(incid.CfdIncidentType,medicservice))
                        {
                            serviceIncidentList.Add(incid);
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
                
                ViewData["serviceIncidentList"] = serviceIncidentList;


               

        }
    }
}
    }






