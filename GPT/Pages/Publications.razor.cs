using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GPT.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace GPT.Pages
{
    public partial class Publications
    {
        /*class User
        {
            public string Id { get; set; } = "";
            public string Name { get; set; } = "";
            public string Mail { get; set; } = "";
        }*/

        //User CurrentUser = new();
        //private AuthenticationState _context;
        
        List<Post> PostsList = new();
        bool Waiting = true;

        protected override async Task OnInitializedAsync()
        {

            await GetPosts();
        }

        private async Task GetPosts()
        {
            await Task.Delay(1);
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //PostsList = db.Posts.Where(p => p.Published == true).ToList();     // Вывести все опубликованные (Published = tru) посты
                PostsList = db.Posts.Where(p => p.Published == true).ToList();     // Вывести все опубликованные (Published = tru) посты
                Waiting = false;
            }
        }

        private string GetAuthor(string id)
        {
            string ret = string.Empty;
            var UsersContext = new ApplicationDbContext();
            ret = UsersContext.Users.Where((i => i.Id == id)).First().UserName;
            return ret;
        }
        
    }
}
