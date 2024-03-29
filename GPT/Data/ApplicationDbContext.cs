﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPT.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Post> Posts { get; set; }
        //public DbSet<User> AspNetUsers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(@"Server=Secret;Database=Secret;User=Secret;Password=Secret", new MySqlServerVersion(new Version(5, 7, 21)));
        }
    }
}
