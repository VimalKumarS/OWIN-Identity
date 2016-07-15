using MinimalOwinWebApiSelfHost.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplicagtion1
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("MyDatabase")
        {
        }

       public static ApplicationDbContext create()
        {
            //Database.SetInitializer(new ApplicationDbInitializer());
            return new ApplicationDbContext();
        }
        public IDbSet<Company> Companies { get; set; }
        public IDbSet<MyUser> Users { get; set; }
        public IDbSet<MyUserClaim> Claims { get; set; }
    }


    public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected async override void Seed(ApplicationDbContext context)
        {
           // base.Seed(context);
            context.Companies.Add(new Company { Name = "Microsoft" });
            context.Companies.Add(new Company { Name = "Google" });
            context.Companies.Add(new Company { Name = "Apple" });
            context.SaveChanges();
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
            await store.AddUserAsync(john, "JohnsPassword");
            await store.AddUserAsync(jimi, "JimisPassword");
        }
    }
}
