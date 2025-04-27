using bookieAPI.Database;
using bookieAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace bookieAPI.Controllers
{
    public class KapcsoloCreateModel
    {
        public int KonyvId { get; set; }
        public int SzerzoId { get; set; }
    }
    public class KapcsoloMindenCreateModel
    {
        public int KonyvId { get; set; }
        public KonyvCreateModel Konyv { get; set; }
        public int SzerzoId { get; set; }
        public SzerzoCreateModel Szerzo { get; set; }
    }
    public class KapcsoloResponseModel
    {
        public int KonyvId { get; set; }
        public Konyv Konyv { get; set; }
        public int SzerzoId { get; set; }
        public Szerzo Szerzo { get; set; }

        public KapcsoloResponseModel(Kapcsolo kapcsolo)
        {
            KonyvId = kapcsolo.KonyvId;
            Konyv = kapcsolo.Konyv;
            SzerzoId = kapcsolo.SzerzoId;
            Szerzo = kapcsolo.Szerzo;
        }
    }

    public class KapcsoloController : ApiController
    {
        
        BookieContext ctx;

        // GET api/<controller>
        [ResponseType(typeof(KapcsoloResponseModel))]
        public HttpResponseMessage Get()
        {
            using (ctx = new BookieContext())
            {
                var res = ctx.Kapcsolok.Include(x=>x.Konyv).Include(y=>y.Szerzo).ToList();
                var response = new List<KapcsoloResponseModel>();

                foreach (var item in res)
                {
                    response.Add(new KapcsoloResponseModel(item));
                }

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
        }
        
        // POST api/<controller>
        [ResponseType(typeof(KapcsoloResponseModel))]
        public HttpResponseMessage Post([FromBody] KapcsoloCreateModel value)
        {

            using (ctx = new BookieContext())
            {
                try
                {
                    Kapcsolo uj = new Kapcsolo(value.KonyvId, value.SzerzoId,ctx.Konyvek.First(x=>x.Id==value.KonyvId),ctx.Szerzok.First(x=>x.Id==value.SzerzoId));
                    ctx.Kapcsolok.Add(uj);

                    ctx.SaveChanges();
                    var response = new KapcsoloResponseModel(uj);

                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, $"{ex.Message}");
                }

            }
        }
        // POST2 api/<controller>
        
        [ResponseType(typeof(KapcsoloResponseModel))]
        [Route("api/Kapcsolo/MindenPost")]
        public HttpResponseMessage Post([FromBody] KapcsoloMindenCreateModel value)
        {

            using (ctx = new BookieContext())
            {
                try
                {
                    var result = ctx.Szerzok.Where(x => x.Nev == value.Szerzo.Nev).FirstOrDefault();
                    Kapcsolo uj;
                    if (result!=null)
                    {
                        uj = new Kapcsolo(value.KonyvId, value.SzerzoId, new Konyv(value.Konyv.Cim, value.Konyv.KiadasEv), result);
                    }
                    else
                    {
                        uj = new Kapcsolo(value.KonyvId, value.SzerzoId, new Konyv(value.Konyv.Cim, value.Konyv.KiadasEv), new Szerzo(value.Szerzo.Nev));
                    }
                    ctx.Kapcsolok.Add(uj);

                    ctx.SaveChanges();
                    var response = new KapcsoloResponseModel(uj);

                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, $"{ex.Message}");
                }

            }
        }
        // DELETE api/<controller>/5
        [ResponseType(typeof(KapcsoloResponseModel))]
        public HttpResponseMessage Delete(int konyvid, int szerzoid)
        {
            using (ctx = new BookieContext())
            {
                var result = ctx.Kapcsolok.Where(x => x.KonyvId == konyvid && x.SzerzoId == szerzoid).FirstOrDefault();
                if (result != null)
                {
                    ctx.Kapcsolok.Remove(result);
                    ctx.SaveChanges();
                    result.Konyv = ctx.Konyvek.First(x => x.Id == result.KonyvId);
                    result.Szerzo = ctx.Szerzok.First(x => x.Id == result.SzerzoId);
                    var response = new KapcsoloResponseModel(result);
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);

            }
        }
    }
}