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
            neighborhoods.Add("SEDAMSVILLE");
            neighborhoods.Add("WESTWOOD");
            neighborhoods.Add("BOND HILL");
            neighborhoods.Add("DOWNTOWN");
            neighborhoods.Add("HARTWELL");

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
