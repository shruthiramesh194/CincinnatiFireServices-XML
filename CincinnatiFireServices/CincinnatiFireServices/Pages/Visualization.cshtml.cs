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
    public class VisualizationModel : PageModel
    {
        public void OnGet()
        {
            using (var webClient = new WebClient())
            {
                //Creating Dictionary for future use
                IDictionary<string, List<QuickType.Incident>> neighborhoodIncidents = new Dictionary<string, List<QuickType.Incident>>();
                //Creating Dictionary for future use
                IDictionary<string, int> neighborhoodIncidentCount = new Dictionary<string, int>();
                //downloading incident json string from source
                string incidentJsonString = webClient.DownloadString("https://data.cincinnati-oh.gov/resource/vnsz-a3wp.json");
                List<QuickType.Incident> incidents = QuickType.Incident.FromJson(incidentJsonString);
                List<string> neighborhoodList = new List<string>();
                List<string> distinctNeighborhoodList = new List<string>();
                //adding incident objects to dictionary
                foreach (QuickType.Incident incident in incidents)
                {
                    string neighborhood = incident.Neighborhood;
                    if (neighborhoodIncidents.ContainsKey(neighborhood))
                    {

                        neighborhoodIncidents[neighborhood].Add(incident);
                    }
                    else
                    {
                        neighborhoodIncidents.Add(neighborhood, new List<Incident>());
                    }
                    //allIncidents.Add(incident.EventNumber, incident);
                    neighborhoodList.Add(incident.Neighborhood);

                }
                distinctNeighborhoodList = neighborhoodList.Distinct().ToList();
                foreach (string name in distinctNeighborhoodList)
                {
                    neighborhoodIncidentCount.Add(name, neighborhoodIncidents[name].Count);
                }
                ViewData["neighborhoodCount"] = neighborhoodIncidentCount;
            }
        }
    }
}
