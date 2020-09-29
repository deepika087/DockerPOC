using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace WebApplication2.Controllers
{
   [ApiController]
   [Route( "[controller]" )]
   public class WeatherForecastController : ControllerBase
   {
      private static readonly string[] Summaries = new[]
      {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
      };
      static HttpClient client = new HttpClient()
      {
         BaseAddress = new Uri( "http://backend:8083/" ),
         //BaseAddress = new Uri( "http://localhost:8083/" ),
         //BaseAddress = new Uri( "http://host.docker.internal:8083/" ),
         Timeout = TimeSpan.FromSeconds( 5 )
      };
      
      private static string gproduct = "Empty String from frontend";

      private readonly ILogger<WeatherForecastController> _logger;

      public WeatherForecastController( ILogger<WeatherForecastController> logger )
      {
         _logger = logger;
      }

      [HttpGet]
      public IEnumerable<WeatherForecast> Get()
      {
         RunAsync().GetAwaiter().GetResult();
         var rng = new Random();
         return Enumerable.Range( 1, 5 ).Select( index => new WeatherForecast
         {
            Date = DateTime.Now.AddDays( index ),
            TemperatureC = rng.Next( -20, 55 ),
            Summary = gproduct
         } )
         .ToArray();
      }

      static async Task RunAsync()
      {
         // Update port # in the following line.
         try
         {
            gproduct = await GetProductAsync( "invokeexe" );
         }
         catch( Exception e )
         {
            gproduct = e.Message;
         }
      }

      static async Task<string> GetProductAsync( string path )
      {
         string product = "local Empty String from frontend";
         HttpResponseMessage response = await client.GetAsync( path );
         if( response.IsSuccessStatusCode )
         {
            product = await response.Content.ReadAsStringAsync();
         }
         return product;
      }
   }
}
