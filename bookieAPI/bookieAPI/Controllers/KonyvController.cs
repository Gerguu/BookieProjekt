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
    public class KonyvCreateModel
    {
        public string Cim { get; set; }
        public int KiadasEv { get; set; }
    }
    public class KonyvUpdateModel
    {
        public string Cim { get; set; }
        public int? KiadasEv { get; set; }
    }
    public class KonyvResponseModel
    {
        public int Id { get; set; }
        public string Cim { get; set; }
        public int KiadasEv { get; set; }

        public KonyvResponseModel(Konyv konyv)
        {
            Id = konyv.Id;
            Cim = konyv.Cim;
            KiadasEv = konyv.KiadasEv;
        }
    }

    public class KonyvController : ApiController
    {
        BookieContext ctx;

        // GET api/<controller>
        [ResponseType(typeof(KonyvResponseModel))]
        public HttpResponseMessage Get()
        {
            using (ctx = new BookieContext())
            {
                var res = ctx.Konyvek.ToList();
                var response = new List<KonyvResponseModel>();

                foreach (var item in res)
                {
                    response.Add(new KonyvResponseModel(item));
                }

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
        }

        // GET api/<controller>/5
        [ResponseType(typeof(KonyvResponseModel))]
        public HttpResponseMessage Get(int id)
        {
            using (ctx = new BookieContext())
            {
                var result = ctx.Konyvek.Where(x => x.Id == id).FirstOrDefault();
                if (result != null)
                {
                    var response = new KonyvResponseModel(result);
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        
        [Route("api/Konyv/{cim}")]
        [ResponseType(typeof(KonyvResponseModel))]
        public HttpResponseMessage Get(string cim)
        {
            using (ctx = new BookieContext())
            {
                var result = ctx.Konyvek.Where(x => x.Cim == cim).ToList();
                if (result != null)
                {
                    var response = new List<KonyvResponseModel>();

                    foreach (var item in result)
                    {
                        response.Add(new KonyvResponseModel(item));
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        // POST api/<controller>
        [ResponseType(typeof(KonyvResponseModel))]
        public HttpResponseMessage Post([FromBody] KonyvCreateModel value)
        {

            using (ctx = new BookieContext())
            {
                try
                {
                    Konyv uj = new Konyv(value.Cim, value.KiadasEv);
                    ctx.Konyvek.Add(uj);

                    ctx.SaveChanges();
                    var response = new KonyvResponseModel(uj);

                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, $"{ex.Message}");
                }

            }
        }

        // PUT api/<controller>/5
        [ResponseType(typeof(KonyvResponseModel))]
        public HttpResponseMessage Put(int id, [FromBody] KonyvUpdateModel value)
        {
            using (ctx = new BookieContext())
            {
                try
                {
                    var result = ctx.Konyvek.Where(x => x.Id == id).FirstOrDefault();
                    if (result == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }

                    result.Cim = value.Cim;
                    result.KiadasEv = (int)value.KiadasEv;
                   
                    var response = new KonyvResponseModel(result);
                    ctx.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, $"{ex.Message}");
                }
            }
        }

        [ResponseType(typeof(KonyvResponseModel))]
        public HttpResponseMessage Patch(int id, [FromBody] KonyvUpdateModel value)
        {
            using (ctx = new BookieContext())
            {
                try
                {
                    var result = ctx.Konyvek.Where(x => x.Id == id).FirstOrDefault();
                    if (result == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }
                    
                    if (value.Cim != null) result.Cim = value.Cim;
                    if (value.KiadasEv != null) result.KiadasEv = (int)value.KiadasEv;

                    var response = new KonyvResponseModel(result);

                    ctx.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, $"{ex.Message}");
                }
            }
        }

        // DELETE api/<controller>/5
        [ResponseType(typeof(KonyvResponseModel))]
        public HttpResponseMessage Delete(int id)
        {
            using (ctx = new BookieContext())
            {
                var result = ctx.Konyvek.Where(x => x.Id == id).FirstOrDefault();
                if (result != null)
                {
                    ctx.Konyvek.Remove(result);
                    ctx.SaveChanges();
                    var response = new KonyvResponseModel(result);
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);

            }
        }
    }
}