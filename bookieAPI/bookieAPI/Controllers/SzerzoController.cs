using bookieAPI.Database;
using bookieAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace bookieAPI.Controllers
{
    public class SzerzoCreateModel
    {
        public string Nev { get; set; }
    }
    public class SzerzoUpdateModel
    {
        public string Nev { get; set; }
    }
    public class SzerzoResponseModel
    {
        public int Id { get; set; }
        public string Nev { get; set; }

        public SzerzoResponseModel(Szerzo szerzo)
        {
            Id = szerzo.Id;
            Nev = szerzo.Nev;

        }
    }

    public class SzerzoController : ApiController
    {
        BookieContext ctx;

        // GET api/<controller>
        [ResponseType(typeof(SzerzoResponseModel))]
        public HttpResponseMessage Get()
        {
            using (ctx = new BookieContext())
            {
                var res = ctx.Szerzok.ToList();
                var response = new List<SzerzoResponseModel>();

                foreach (var item in res)
                {
                    response.Add(new SzerzoResponseModel(item));
                }

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
        }

        // GET api/<controller>/5
        [ResponseType(typeof(SzerzoResponseModel))]
        public HttpResponseMessage Get(int id)
        {
            using (ctx = new BookieContext())
            {
                var result = ctx.Szerzok.Where(x => x.Id == id).FirstOrDefault();
                if (result != null)
                {
                    var response = new SzerzoResponseModel(result);
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        // POST api/<controller>
        [ResponseType(typeof(SzerzoResponseModel))]
        public HttpResponseMessage Post([FromBody] SzerzoCreateModel value)
        {

            using (ctx = new BookieContext())
            {
                try
                {
                    var nev = ctx.Szerzok.Where(x => x.Nev == value.Nev).FirstOrDefault();
                    Szerzo uj = new Szerzo(value.Nev);
                    if (nev!=null)
                    {
                        return Request.CreateResponse(HttpStatusCode.Found, "SZERZO_NEV_EXISTS");
                    }
                    else
                    {                      
                        ctx.Szerzok.Add(uj);
                    }
                    ctx.SaveChanges();
                    var response = new SzerzoResponseModel(uj);

                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, $"{ex.Message}");
                }

            }
        }
        // DELETE api/<controller>/5
        [ResponseType(typeof(SzerzoResponseModel))]
        public HttpResponseMessage Delete(int id)
        {
            using (ctx = new BookieContext())
            {
                var result = ctx.Szerzok.Where(x => x.Id == id).FirstOrDefault();
                if (result != null)
                {
                    ctx.Szerzok.Remove(result);
                    ctx.SaveChanges();
                    var response = new SzerzoResponseModel(result);
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);

            }
        }
    }
}