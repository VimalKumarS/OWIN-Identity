using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using WebApplicagtion1;
using WebApplication1.Models;

namespace WebApplication1.Provider
{
    public class ApplicationOAuthServerProvider
        : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(
            OAuthValidateClientAuthenticationContext context)
        {
            // This call is required...
            // but we're not using client authentication, so validate and move on...
            await Task.FromResult(context.Validated());
        }


        public override async Task GrantResourceOwnerCredentials(
            OAuthGrantResourceOwnerCredentialsContext context)
        {

            using (ApplicationDbContext _repo = new ApplicationDbContext())
            {
                var store = new MyUserStore(_repo);
                var user = await store.FindByEmailAsync(context.UserName);
            }
               
           

            // DEMO ONLY: Pretend we are doing some sort of REAL checking here:
            //if (context.Password != "password")
            //{
            //    context.SetError(
            //        "invalid_grant", "The user name or password is incorrect.");
            //    context.Rejected();
            //    return;
            //}

            // Create or retrieve a ClaimsIdentity to represent the 
            // Authenticated user:
            //ClaimsIdentity identity =
            //    new ClaimsIdentity(context.Options.AuthenticationType);
            //identity.AddClaim(new Claim("user_name", context.UserName));
            //identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            // Identity info will ultimately be encoded into an Access Token
            // as a result of this call:

            if (user == null || !store.PasswordIsValid(user, context.Password))
            {
                context.SetError(
                    "invalid_grant", "The user name or password is incorrect.");
                context.Rejected();
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            foreach (var userClaim in user.Claims)
            {
                identity.AddClaim(new Claim(userClaim.ClaimType, userClaim.ClaimValue));
            }
            context.Validated(identity);
            
        }
    }
}