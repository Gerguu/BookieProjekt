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
    public class CsevegesCreateModel
    {
        public string Uzenet { get; set; }
        public DateTime Datum { get; set; }
        public string KuldoFelhasznalonev { get; set; }
        public string KapoFelhasznalonev { get; set; }
    }
    public class CsevegesUpdateModel
    {
        public string Uzenet { get; set; }
        public DateTime? Datum { get; set; }
    }
    public class CsevegesResponseModel
    {
        public int Id { get; set; }
        public string Uzenet { get; set; }
        public DateTime Datum { get; set; }
        public string KuldoFelhasznalonev { get; set; }
        public string KapoFelhasznalonev { get; set; }

        public CsevegesResponseModel(Cseveges cseveges)
        {
            Id = cseveges.Id;
            Uzenet = cseveges.Uzenet;
            Datum = cseveges.Datum;
            KuldoFelhasznalonev = cseveges.KuldoFelhasznalonev;
            KapoFelhasznalonev = cseveges.KapoFelhasznalonev;
        }
    }

    public class CsevegesController : ApiController
    {
        BookieContext ctx;

        // GET api/<controller>
        [ResponseType(typeof(CsevegesResponseModel))]
        public HttpResponseMessage Get()
        {
            using (ctx = new BookieContext())
            {
                var res = ctx.Csevegesek.ToList();
                var response = new List<CsevegesResponseModel>();

                foreach (var item in res)
                {
                    response.Add(new CsevegesResponseModel(item));
                }

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
        }

        // GET api/<controller>/5
        [Route("api/Cseveges/getId")]
        [ResponseType(typeof(CsevegesResponseModel))]
        public HttpResponseMessage Get(int id)
        {
            using (ctx = new BookieContext())
            {
                var result = ctx.Csevegesek.Where(x => x.Id == id).FirstOrDefault();
                if (result != null)
                {
                    var response = new CsevegesResponseModel(result);
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
        [Route("api/Cseveges/{felhasznalonev}")]
        public HttpResponseMessage Get(string felhasznalonev)
        {
            using (ctx = new BookieContext())
            {
                var result = ctx.Csevegesek.Where(x => x.KuldoFelhasznalonev == felhasznalonev).ToList();
                var result2 = ctx.Csevegesek.Where(x => x.KapoFelhasznalonev == felhasznalonev).ToList();
                var felhasznalok = result.Select(x => x.KapoFelhasznalonev).Distinct().ToList();
                foreach (var item in result2.Select(x => x.KuldoFelhasznalonev).Distinct().ToList())
                {
                    if (!felhasznalok.Contains(item))
                    {
                        felhasznalok.Add(item);
                    }
                }
                felhasznalok.Sort();
                var response = new List<Cseveges>();
                foreach (var item in felhasznalok)
                {
                    var res = ctx.Csevegesek.Where(x => x.KuldoFelhasznalonev == item && x.KapoFelhasznalonev == felhasznalonev || x.KuldoFelhasznalonev == felhasznalonev && x.KapoFelhasznalonev == item).OrderByDescending(y => y.Id).FirstOrDefault();
                    if(!response.Contains(res) && res!=null) response.Add(res);
                }
                if (response!=null)
                {
                    var actualResponse = response.Select(x => new { Felhasznalonev = x.KuldoFelhasznalonev == felhasznalonev ? x.KapoFelhasznalonev : x.KuldoFelhasznalonev, UtolsoUzenet = x.Uzenet, Datum = x.Datum });
                    if (actualResponse != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, actualResponse);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
        [Route("api/Cseveges/kettofelhasznalo")]
        [ResponseType(typeof(CsevegesResponseModel))]
        public HttpResponseMessage Get(string kuldo, string kapo)
        {
            using (ctx = new BookieContext())
            {
                var result = ctx.Csevegesek.Where(x => x.KuldoFelhasznalonev == kuldo && x.KapoFelhasznalonev==kapo).ToList();
                var result2 = ctx.Csevegesek.Where(x => x.KuldoFelhasznalonev == kapo && x.KapoFelhasznalonev==kuldo).ToList();
                if (result != null || result2!=null)
                {
                    var response = new List<CsevegesResponseModel>();
                    var results=result;
                    foreach (var item in result2)
                    {
                        results.Add(item);
                    }
                    foreach (var item in results.OrderBy(x=>x.Id))
                    {
                        response.Add(new CsevegesResponseModel(item));
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        // POST api/<controller>
        [ResponseType(typeof(CsevegesResponseModel))]
        public HttpResponseMessage Post([FromBody] CsevegesCreateModel value)
        {
            using (ctx = new BookieContext())
            {
                try
                {
                    Cseveges uj = new Cseveges(value.Uzenet, value.Datum, value.KuldoFelhasznalonev, value.KapoFelhasznalonev);
                    ctx.Csevegesek.Add(uj);

                    ctx.SaveChanges();
                    var response = new CsevegesResponseModel(uj);

                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, $"{ex.Message}");
                }

            }
        }

        // PUT api/<controller>/5
        [Route("api/Cseveges/put")]
        [ResponseType(typeof(CsevegesResponseModel))]
        public HttpResponseMessage Put(int id, [FromBody] CsevegesUpdateModel value)
        {
            using (ctx = new BookieContext())
            {
                try
                {
                    var result = ctx.Csevegesek.Where(x => x.Id == id).FirstOrDefault();
                    if (result == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }

                    result.Uzenet = value.Uzenet;
                    result.Datum = (DateTime)value.Datum;

                    var response = new CsevegesResponseModel(result);

                    ctx.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, $"{ex.Message}");
                }
            }
        }

        [Route("api/Cseveges/patch")]
        [ResponseType(typeof(CsevegesResponseModel))]
        public HttpResponseMessage Patch(int id, [FromBody] CsevegesUpdateModel value)
        {
            using (ctx = new BookieContext())
            {
                try
                {
                    var result = ctx.Csevegesek.Where(x => x.Id == id).FirstOrDefault();
                    if (result == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }
                    if (value.Uzenet != null) result.Uzenet = value.Uzenet;
                    if (value.Datum.ToString()[0]==0) result.Datum = (DateTime)value.Datum;


                    var response = new CsevegesResponseModel(result);

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
        [Route("api/Cseveges/delete")]
        [ResponseType(typeof(CsevegesResponseModel))]
        public HttpResponseMessage Delete(int id)
        {
            using (ctx = new BookieContext())
            {
                var result = ctx.Csevegesek.Where(x => x.Id == id).FirstOrDefault();
                try
                {
                    if (result != null)
                    {
                        ctx.Csevegesek.Remove(result);
                        ctx.SaveChanges();
                        var response = new CsevegesResponseModel(result);
                        return Request.CreateResponse(HttpStatusCode.OK, result);
                    }
                }
                catch
                {

                }
                return Request.CreateResponse(HttpStatusCode.NotFound);

            }
        }
    }
}