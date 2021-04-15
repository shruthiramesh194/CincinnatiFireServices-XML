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
            using (var webClient = new WebClient())
            {
                //downloading incident json string from source

                string incidentJsonString = webClient.DownloadString("https://data.cincinnati-oh.gov/resource/vnsz-a3wp.json");
                List<QuickType.Incident> incidents = QuickType.Incident.FromJson(incidentJsonString);
                Wrapperclass wrp = new Wrapperclass();
                wrp.incidents = incidents;
                return new JsonResult(incidents);
            }

        }
    }
}
