using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CincinnatiFireServices.Pages
{
    public class AutocompleteJsonModel : PageModel
    {
       
             private IList<string> neighborhoods = new List<string>();

        public JsonResult OnGet(String term)
        {
            neighborhoods.Add("AYUSHI");
            neighborhoods.Add("NIKIT");
            neighborhoods.Add("SHREEYA");
            neighborhoods.Add("SHRUTI");
            

            IList<string> matchingneighborhoods = new List<string>();

            // iterate over all plant names.
            foreach (string neighborhood in neighborhoods)
            {
                if (neighborhood.Contains(term))
                {
                    matchingneighborhoods.Add(neighborhood);
                }
            }

            return new JsonResult(matchingneighborhoods);
        }
        
    
    }
}
