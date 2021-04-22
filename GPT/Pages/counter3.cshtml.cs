using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GPT.Pages
{
    public class counter3Model : PageModel
    {
        public string Id { get; set; }
        public string Code { get; set; }

        public void OnGet(string id, string code)
        {
            Id = id;
            Code = code;
            Console.WriteLine("Id: {0}, code {1}", Id, Code);
        }
    }
}
