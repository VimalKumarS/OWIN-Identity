namespace WebApplication1.Migrations
{
    using MinimalOwinWebApiSelfHost.Models;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Security.Claims;
    internal sealed class Configuration : DbMigrationsConfiguration<WebApplicagtion1.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "WebApplicagtion1.ApplicationDbContext";
        }

        protected override async void Seed(WebApplicagtion1.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //context.Companies.Add(new Company { Name = "Microsoft" });
            //context.Companies.Add(new Company { Name = "Google" });
            //context.Companies.Add(new Company { Name = "Apple" });
            //context.SaveChanges();
            var john = new MyUser { Email = "john@example.com" };
            var jimi = new MyUser { Email = "jimi@Example.com" };

            john.Claims.Add(new MyUserClaim
            {
                ClaimType = ClaimTypes.Name,
                UserId = john.Id,
                ClaimValue = john.Email
            });
            john.Claims.Add(new MyUserClaim
            {
                ClaimType = ClaimTypes.Role,
                UserId = john.Id,
                ClaimValue = "Admin"
            });

            jimi.Claims.Add(new MyUserClaim
            {
                ClaimType = ClaimTypes.Name,
                UserId = jimi.Id,
                ClaimValue = jimi.Email
            });
            jimi.Claims.Add(new MyUserClaim
            {
                ClaimType = ClaimTypes.Role,
                UserId = john.Id,
                ClaimValue = "User"
            });

            var store = new MyUserStore(context);
            context.Users.Add(john);
            context.Users.Add(jimi);

           // await store.AddUserAsync(john, "JohnsPassword");
            //await store.AddUserAsync(jimi, "JimisPassword");
        }
    }
}
