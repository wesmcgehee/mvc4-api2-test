using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestApi.Models;
//Ref: http://tech.pro/tutorial/1289/building-rest-api-with-mvc-4-web-api-part-2
namespace TestApi.Controllers
{
    public class ChuckController : ApiController
    {
       private static List<ChuckFact> facts = new List<ChuckFact>()
       {
            new ChuckFact { Id = 1, Rating = 4, 
                Text = "Chuck Norris doesn't call the wrong number. You answer the wrong phone." },
            new ChuckFact { Id = 2, Rating = 3, 
                Text = "Chuck Norris counted to infinity. Twice." },
            new ChuckFact { Id = 3, Rating = 4, 
                Text = "When Alexander Bell invented the telephone he had 3 missed calls from Chuck Norris." },
       };
       private static List<ChuckMovie> movies = new List<ChuckMovie>()
       {
            new ChuckMovie { Id = 1, Rating = 4, 
                Text = "The Delta Force" },
            new ChuckMovie { Id = 2, Rating = 3, 
                Text = "Return of the Dragon" },
            new ChuckMovie { Id = 3, Rating = 4, 
                Text = "Missing in Action" },
       };
       
        [ActionName("facts")]
       public IEnumerable<ChuckFact> GetAllFacts()
       {
           return facts;
       }
       [ActionName("movies")]
       public IEnumerable<ChuckMovie> GetAllMovies()
       {
            return movies;
       }
       public ChuckFact GetFactByID(int id)
       {
           ChuckFact fact = (from f in facts where f.Id == id select f).FirstOrDefault();
           if (fact == null)
               throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NotFound));

           return fact;
       }
       public HttpResponseMessage PostFact(ChuckFact fact)
       {
           if (!this.ModelState.IsValid)
               throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

           facts.Add(fact);
           HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.Created, fact);
           response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = fact.Id }));

           return response;
       }
       public HttpResponseMessage PutFact(ChuckFact fact)
       {
           if (!this.ModelState.IsValid)
               throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

           ChuckFact existFact = (from f in facts where f.Id == fact.Id select f).FirstOrDefault();
           if (existFact == null)
               throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NotFound));

           facts.Remove(existFact);
           facts.Add(fact);
           return this.Request.CreateResponse(HttpStatusCode.NoContent);
       }

       public HttpResponseMessage DeleteFact(int id)
       {
           ChuckFact fact = (from f in facts where f.Id == id select f).FirstOrDefault();
           if (fact == null)
               throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NotFound));

           facts.Remove(fact);
           return this.Request.CreateResponse(HttpStatusCode.NoContent);
       }
       public IEnumerable<ChuckFact> GetFactsByRating(int minRating)
       {
           return from f in facts where f.Rating >= minRating select f;
       }
    }
}
