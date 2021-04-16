using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CincinnatiFireServices.Pages
{
    public class JsonFeedModel : PageModel
    {
        public JsonResult OnGet()
        {

            string cfdIncType = HttpContext.Request.Query["cfdIncType"];

            using (var webClient = new WebClient())
            {
                //downloading incident json string from source
                string url = "https://data.cincinnati-oh.gov/resource/vnsz-a3wp.json?";
                if (!string.IsNullOrEmpty(cfdIncType))
                {
                    url = $"{url}cfd_incident_type={cfdIncType}";
                }
                string incidentJsonString = webClient.DownloadString(url);
                List<QuickType.Incident> incidents = QuickType.Incident.FromJson(incidentJsonString);
                return new JsonResult(incidents);
            }

        }
    }
}

