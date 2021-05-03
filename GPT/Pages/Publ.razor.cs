using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace GPT.Pages
{
    public partial class Publ
    {
        class User
        {
            public string Id { get; set; } = "";
            public string Name { get; set; } = "";
            public string Mail { get; set; } = "";
        }

        User CurrentUser = new();

        //string UserName = "mcls@list.ru";

        private readonly UserManager<IdentityUser> _userManager;
        private AuthenticationState _context;

        private void HandleSubmit()
        {
            //UserId = _userManager.GetUserId(U);
            //User.Identity.IsAuthenticated
            //ClaimsPrincipal currentUser = U;
            CurrentUser.Id = _context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            CurrentUser.Name = _context.User.FindFirst(ClaimTypes.Name).Value;
            CurrentUser.Mail = _context.User.FindFirst(ClaimTypes.Email).Value;
        }
        
    }
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal) =>
            principal.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
