using IzgodnoKupi.Data.Contracts;
using IzgodnoKupi.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IzgodnoKupi.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public DbInitializer(
            ApplicationDbContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        //This example just creates an Administrator role and one Admin users
        public void Initialize()
        {
            //create database schema if none exists
            context.Database.EnsureCreated();

            //Create the Administartor Role

            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Administrator";

                IdentityResult roleResult = roleManager.CreateAsync(role).Result;

                if (!roleResult.Succeeded)
                {
                    return;
                }
            }


            //Create the default Admin account and apply the Administrator role

            string email = "caves.computers@gmail.com";
            string password = "Test12";

            var user = new User { UserName = email, Email = email };

            this.userManager.CreateAsync(user, password).Wait();

            this.userManager.AddToRoleAsync(user, "Administrator").Wait();
        }
    }
}
