using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPT.Pages
{
    [AllowAnonymous]
    public partial class Counter2
    {
        public string Id { get; set; }
        public string Code { get; set; }

        //public void OnGet(string id, string code)
        //{
        //    Id = id;
        //    Code = code;
        //    //Console.WriteLine("Id: {0}, code {1}", Id, Code);
        //}

        protected override void OnInitialized()     // При загрузке страницы
        {
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri); //you can use IURIHelper for older versions
            string uriStr = uri.ToString().Replace("&amp;", "&");
            uri = new Uri(uriStr);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("id", out var id)) // Берём id из строки запроса (из GET)
            {
                Id = id.First().ToString();
            }
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("code", out var code)) // Берём id из строки запроса (из GET)
            {
                Code = code.First().ToString();
            }
        }
    }
}
