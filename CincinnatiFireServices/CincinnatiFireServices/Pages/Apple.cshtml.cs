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

                string peerApple = webClient.DownloadString("https://nutrientdiary20210416013720.azurewebsites.net/?query=Apple");
                JArray jsonArray = JArray.Parse(peerApple);  
               var nutrients = Nutrient.FromJson(peerApple);
                ViewData["peerpage"] = nutrients;





            }
        }
    }
}
