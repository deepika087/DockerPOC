using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;

namespace InvokeExe.Controllers
{
   [ApiController]
   [Route( "[controller]" )]
   public class InvokeExeController : ControllerBase
   {
      private static readonly string[] Summaries = new[]
      {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

      private readonly ILogger<InvokeExeController> _logger;

      public InvokeExeController( ILogger<InvokeExeController> logger )
      { 
         _logger = logger;
      }

      [HttpGet]
      public String Get()
      {
         ProcessStartInfo startInfo = new ProcessStartInfo();
         startInfo.CreateNoWindow = false;
         startInfo.UseShellExecute = false;
         startInfo.FileName = System.IO.Path.Combine( Environment.CurrentDirectory, "Data", "HelloWorld" );
         startInfo.WindowStyle = ProcessWindowStyle.Hidden;
         startInfo.RedirectStandardOutput = true;
         startInfo.RedirectStandardError = true;

         string msg = "";
         try
         {
            // Start the process with the info we specified.
            // Call WaitForExit and then the using statement will close.
            msg += "Starting New   ";
            using( Process exeProcess = Process.Start( startInfo ) )
            {
               msg += exeProcess.StandardOutput.ReadToEnd();
               msg += exeProcess.StandardError.ReadToEnd();
               exeProcess.WaitForExit();
            }
         }
         catch( Exception e )
         {
            Console.WriteLine( e.Message );
            msg += " stack trace: " + e.ToString()  + "Exception: " + e.Message + " Path: " + startInfo.FileName;
         }
         return msg ;
      }
   }
}
