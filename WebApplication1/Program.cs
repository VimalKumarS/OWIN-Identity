﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Add reference to:
using Microsoft.Owin.Hosting;
using System.Data.Entity;
using MinimalOwinWebApiSelfHost;

namespace WebApplicagtion1
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Initializing and seeding database...");
           // Database.SetInitializer(new ApplicationDbInitializer());
           // var db = new ApplicationDbContext();
           // int count = db.Companies.Count();
            //Console.WriteLine("Initializing and seeding database with {0} company records...", count);



            // Specify the URI to use for the local host:
            string baseUri = "http://localhost:8080";

            Console.WriteLine("Starting web Server...");
            WebApp.Start<Startup>(baseUri);
            Console.WriteLine("Server running at {0} - press Enter to quit. ", baseUri);
            Console.ReadLine();
        }
    }
}
