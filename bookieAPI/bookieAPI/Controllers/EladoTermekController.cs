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
    public class EladoTermekCreateModel
    {
        public int Ar { get; set; }
        public bool Csere { get; set; }
        public bool Aktiv { get; set; }
        public DateTime FelrakasDatum { get; set; }
        public DateTime EladasDatum { get; set; }
        public string Felhasznalonev { get; set; }
        public int KonyvId { get; set; }
    }
    public class EladoTermekUpdateModel
    {
        public int? Ar { get; set; }
        public bool? Csere { get; set; }
        public bool? Aktiv { get; set; }
        public DateTime? FelrakasDatum { get; set; }
        public DateTime? EladasDatum { get; set; }
        public string Felhasznalonev { get; set; }
        public int? KonyvId { get; set; }
    }
    public class EladoTermekResponseModel
    {
        public int Id { get; set; }
        public int Ar { get; set; }
        public bool Csere { get; set; }
        public bool Aktiv { get; set; }
        public DateTime FelrakasDatum { get; set; }
        public DateTime EladasDatum { get; set; }
        public string Felhasznalonev { get; set; }
        public ProfilResponseModel Profil { get; set; }
        public List<KepResponseModel> Kep { get; set; }
        public int KonyvId { get; set; }
        public KapcsoloResponseModel Konyv { get; set; }

        public EladoTermekResponseModel(EladoTermek eladotermek)
        {
            Id = eladotermek.Id;
            Ar = eladotermek.Ar;
            Csere = eladotermek.Csere;
            Aktiv = eladotermek.Aktiv;
            FelrakasDatum = eladotermek.FelrakasDatum;
            EladasDatum = eladotermek.EladasDatum;
            Felhasznalonev = eladotermek.Felhasznalonev;
            using (var ctx = new BookieContext())
            {
                Profil = new ProfilResponseModel(ctx.Profilok.Where(x => x.Felhasznalonev == eladotermek.Felhasznalonev).FirstOrDefault());
            }
            try
            {
                using (var ctx = new BookieContext())
                {
                    var result = ctx.Kepek.Where(x => x.TermekId == eladotermek.Id).ToList();
                    Kep = new List<KepResponseModel>();
                    foreach (var item in result)
                    {
                        Kep.Add(new KepResponseModel(item));
                    }
                    
                }
            }
            catch
            {
            }
            KonyvId = eladotermek.KonyvId;
            using (var ctx=new BookieContext())
            {
                Konyv = new KapcsoloResponseModel(ctx.Kapcsolok.Where(x => x.KonyvId == eladotermek.KonyvId).FirstOrDefault());
            }
        }
    }

    public class EladoTermekController : ApiController
    {
        BookieContext ctx;

        // GET api/<controller>
        [ResponseType(typeof(EladoTermekResponseModel))]
        public HttpResponseMessage Get()
        {
            using (ctx = new BookieContext())
            {
                var res = ctx.EladoTermekek.ToList();
                var response = new List<EladoTermekResponseModel>();

                foreach (var item in res)
                {
                    response.Add(new EladoTermekResponseModel(item));
                }

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
        }

        // GET api/<controller>/5
        [ResponseType(typeof(EladoTermekResponseModel))]
        public HttpResponseMessage Get(int id)
        {
            using (ctx = new BookieContext())
            {
                var result = ctx.EladoTermekek.Where(x => x.Id == id).FirstOrDefault();
                if (result != null)
                {
                    var response = new EladoTermekResponseModel(result);
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        // POST api/<controller>
        [ResponseType(typeof(EladoTermekResponseModel))]
        public HttpResponseMessage Post([FromBody] EladoTermekCreateModel value)
        {

            using (ctx = new BookieContext())
            {
                try
                {
                    EladoTermek uj = new EladoTermek(value.Ar, value.Csere, value.Aktiv, value.FelrakasDatum, value.EladasDatum, value.Felhasznalonev, value.KonyvId);
                    ctx.EladoTermekek.Add(uj);

                    ctx.SaveChanges();
                    var response = new EladoTermekResponseModel(uj);

                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, $"{ex.Message}");
                }

            }
        }

        // PUT api/<controller>/5
        [ResponseType(typeof(EladoTermekResponseModel))]
        public HttpResponseMessage Put(int id, [FromBody] EladoTermekUpdateModel value)
        {
            using (ctx = new BookieContext())
            {
                try
                {
                    var result = ctx.EladoTermekek.Where(x => x.Id == id).FirstOrDefault();
                    if (result == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }

                    result.Ar = (int)value.Ar;
                    result.Csere = (bool)value.Csere;
                    result.Aktiv = (bool)value.Aktiv;
                    result.FelrakasDatum = (DateTime)value.FelrakasDatum;
                    result.EladasDatum = (DateTime)value.EladasDatum;
                    result.Felhasznalonev = value.Felhasznalonev;
                    result.KonyvId = (int)value.KonyvId;

                    var response = new EladoTermekResponseModel(result);

                    ctx.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, $"{ex.Message}");
                }
            }
        }

        [ResponseType(typeof(EladoTermekResponseModel))]
        public HttpResponseMessage Patch(int id, [FromBody] EladoTermekUpdateModel value)
        {
            using (ctx = new BookieContext())
            {
                try
                {
                    var result = ctx.EladoTermekek.Where(x => x.Id == id).FirstOrDefault();
                    if (result == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }
                    if (value.Ar != null) result.Ar = (int)value.Ar;
                    if (value.Csere != null) result.Csere = (bool)value.Csere;
                    if (value.Aktiv != null) result.Aktiv = (bool)value.Aktiv;
                    if (value.FelrakasDatum != null) result.FelrakasDatum = (DateTime)value.FelrakasDatum;
                    if (value.EladasDatum != null) result.EladasDatum = (DateTime)value.EladasDatum;
                    if (value.Felhasznalonev != null) result.Felhasznalonev = value.Felhasznalonev;
                    if (value.KonyvId != null) result.KonyvId = (int)value.KonyvId;


                    var response = new EladoTermekResponseModel(result);

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
        [ResponseType(typeof(EladoTermekResponseModel))]
        public HttpResponseMessage Delete(int id)
        {
            using (ctx = new BookieContext())
            {
                var result = ctx.EladoTermekek.Where(x => x.Id == id).FirstOrDefault();
                if (result != null)
                {
                    var resultImages = ctx.Kepek.Where(x => x.TermekId == id).ToList();
                    if (resultImages != null)
                    {
                        foreach (var item in resultImages)
                        {
                            ctx.Kepek.Remove(item);
                        }
                    }

                    var response = new EladoTermekResponseModel(result);
                    ctx.EladoTermekek.Remove(result);
                    ctx.SaveChanges();
                    
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);

            }
        }
    }
}