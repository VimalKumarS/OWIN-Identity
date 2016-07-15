using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Owin;
using Microsoft.Owin.Hosting;
using Microsoft.Owin;
namespace Owin_Test_App
{
    using System.IO;
    using AppFunc = Func<IDictionary<string, object>, Task>;
    class Program
    {

        static void Main(string[] args)
        {
            WebApp.Start<Startup>("http://localhost:8080");
            Console.WriteLine("Press enter to Quit");
            Console.ReadLine();


        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var middleware = new Func<AppFunc, AppFunc>(MyMiddleWare);
            var otherMiddleware = new Func<AppFunc, AppFunc>(MyOtherMiddleWare);
            app.Use(middleware);
            app.Use(otherMiddleware);

            app.Use<MyMiddlewareComponent>();
            app.Use<MyOtherMiddlewareComponent>();
           
        }

        public AppFunc MyMiddleWare(AppFunc next)
        {
            AppFunc appFunc = async (IDictionary<string, object> environment) =>
            {
                // Do something with the incoming request:
                var response = environment["owin.ResponseBody"] as Stream;
                using (var writer = new StreamWriter(response))
                {
                    await writer.WriteAsync("<h1>Hello from My First Middleware</h1>");
                }
                // Call the next Middleware in the chain:
                await next.Invoke(environment);
            };
            return appFunc;
        }

        public AppFunc MyOtherMiddleWare(AppFunc next)
        {
            AppFunc appFunc = async (IDictionary<string, object> environment) =>
            {
                // Do something with the incoming request:
                var response = environment["owin.ResponseBody"] as Stream;
                using (var writer = new StreamWriter(response))
                {
                    await writer.WriteAsync("<h1>Hello from My Second Middleware</h1>");
                }
                // Call the next Middleware in the chain:
                await next.Invoke(environment);
            };
            return appFunc;
        }

        public AppFunc MyMiddleWare1(AppFunc next)
        {
            AppFunc appFunc = async (IDictionary<string, object> environment) =>
            {
                IOwinContext context = new OwinContext(environment);
                await context.Response.WriteAsync("<h1>Hello from My First Middleware</h1>");
                await next.Invoke(environment);
            };
            return appFunc;
        }

        public AppFunc MyOtherMiddleWare1(AppFunc next)
        {
            AppFunc appFunc = async (IDictionary<string, object> environment) =>
            {
                IOwinContext context = new OwinContext(environment);
                await context.Response.WriteAsync("<h1>Hello from My Second Middleware</h1>");
                await next.Invoke(environment);
            };
            return appFunc;
        }
    }

    public class MyMiddlewareComponent
    {
        AppFunc _next;
        public MyMiddlewareComponent(AppFunc next)
        {
            _next = next;
        }
        public async Task Invoke(IDictionary<string, object> environment)
        {
            IOwinContext context = new OwinContext(environment);
            await context.Response.WriteAsync("<h1>Hello from My First Middleware</h1>");
            await _next.Invoke(environment);
        }
    }

    public class MyOtherMiddlewareComponent
    {
        AppFunc _next;
        public MyOtherMiddlewareComponent(AppFunc next)
        {
            _next = next;
        }
        public async Task Invoke(IDictionary<string, object> environment)
        {
            IOwinContext context = new OwinContext(environment);
            await context.Response.WriteAsync("<h1>Hello from My Second Middleware</h1>");
            await _next.Invoke(environment);
        }
    }

    public class SillyAuthenticationComponent
    {
        AppFunc _next;

        public SillyAuthenticationComponent(AppFunc next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> env)
        {
            IOwinContext context = new OwinContext(env);
            var isAuthorozed = context.Request.QueryString.Value == "vimal";
            if (!isAuthorozed)
            {
                context.Response.StatusCode = 401;
                context.Response.ReasonPhrase = "Not Authorized";
                await context.Response.WriteAsync(string.Format("<h1>Error {0}-{1}",
                    context.Response.StatusCode,
                    context.Response.ReasonPhrase));
            }
            else
            {
                // _next is only invoked is authentication succeeds:
                context.Response.StatusCode = 200;
                context.Response.ReasonPhrase = "OK";
                await _next.Invoke(env);
            }
        }
    }
    public class SillyLoggingComponent
    {
        AppFunc _next;
        public SillyLoggingComponent(AppFunc next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            // Pass everything up through the pipeline first:
            await _next.Invoke(environment);

            // Do the logging on the way out:
            IOwinContext context = new OwinContext(environment);
            Console.WriteLine("URI: {0} Status Code: {1}",
                context.Request.Uri, context.Response.StatusCode);
        }
    }
    public static class AppBuilderExtensions
    {
        public static void UseMyMiddleware(this IAppBuilder app,
            MyMiddlewareConfigOptions configOptions)
        {
            app.Use<MyMiddlewareComponent>(configOptions);
        }

        public static void UseMyOtherMiddleware(this IAppBuilder app)
        {
            app.Use<MyOtherMiddlewareComponent>();
        }

        public static void UseSillyAuthentication(this IAppBuilder app)
        {
            app.Use<SillyAuthenticationComponent>();
        }

        public static void UseSillyLogging(this IAppBuilder app)
        {
            app.Use<SillyLoggingComponent>();
        }

    }
    public class MyMiddlewareConfigOptions
    {
        string _greetingTextFormat = "{0} from {1}{2}";
        public MyMiddlewareConfigOptions(string greeting, string greeter)
        {
            GreetingText = greeting;
            Greeter = greeter;
            Date = DateTime.Now;
        }

        public string GreetingText { get; set; }
        public string Greeter { get; set; }
        public DateTime Date { get; set; }

        public bool IncludeDate { get; set; }

        public string GetGreeting()
        {
            string DateText = "";
            if (IncludeDate)
            {
                DateText = string.Format(" on {0}", Date.ToShortDateString());
            }
            return string.Format(_greetingTextFormat, GreetingText, Greeter, DateText);
        }
    }
}

