using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace rideaway_backend {
    public class Program {
        /// <summary>
        /// Main method of the application. Builds the webhost and runs it.
        /// </summary>
        /// <param name="args">arguments.</param>
        public static void Main (string[] args) {
            var host = new WebHostBuilder ()
                .UseKestrel ()
                .UseContentRoot (Directory.GetCurrentDirectory ())
                .UseIISIntegration ()
                .UseStartup<Startup> ()
                .Build ();

            host.Run ();
        }
    }
}