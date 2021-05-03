using GPT.Data;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GPT.Pages
{
    public partial class MyPosts
    {
        class User
        {
            public string Id { get; set; } = "";
            public string Name { get; set; } = "";
            public string Mail { get; set; } = "";
        }

        User CurrentUser = new();

        private AuthenticationState _context;

        List<Post> PostsList = new();

        protected override async Task OnInitializedAsync()
        //protected override void OnInitialized()     // При загрузке страницы
        {
            await GetPosts();
        }

        private async Task GetPosts()
        {
            while (_context == null)
            {
                await Task.Delay(1);
                if (_context != null)
                {
                    CurrentUser.Id = _context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    //CurrentUser.Name = _context.User.FindFirst(ClaimTypes.Name).Value;
                    //CurrentUser.Mail = _context.User.FindFirst(ClaimTypes.Email).Value;

                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        //LS = db.Posts.Where(p => p.Published == true).ToList();     // Вывести все опубликованные (Published = tru) посты
                        PostsList = db.Posts.Where(u => u.UserId == CurrentUser.Id).ToList();      // Вывести только мои посты
                    }
                    break;
                }
                else
                {

                }

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
