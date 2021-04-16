using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using NutrientQuickType;

namespace CincinnatiFireServices.Pages
{
    public class AppleModel : PageModel
    {

        public List<NutrientQuickType.Nutrient> nutrients;
        public void   OnGet()
        {
            using (var webClient = new WebClient())

            {

                string appleKhaoge = webClient.DownloadString("http://localhost:33974/?query=Apple");
                JArray jsonArray = JArray.Parse(appleKhaoge);
                
               var nutrients = Nutrient.FromJson(appleKhaoge);
          
                ViewData["test"] = nutrients;





            }
        }
    }
}
