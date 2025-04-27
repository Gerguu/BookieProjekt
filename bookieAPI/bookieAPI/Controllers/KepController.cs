using bookieAPI.Database;
using bookieAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace bookieAPI.Controllers
{
    public class KepCreateModel
    {
        public string EleresiUt { get; set; }
        public string KepNev { get; set; }
        public int TermekId { get; set; }
    }
    public class KepResponseModel
    {
        public int Id { get; set; }
        public string EleresiUt { get; set; }
        public string KepNev { get; set; }
        public int TermekId { get; set; }

        public KepResponseModel(Kep kep)
        {
            Id = kep.Id;
            EleresiUt = kep.EleresiUt;
            KepNev = kep.KepNev;
            TermekId = kep.TermekId;
        }
    }

    public class KepController : ApiController
    {
        BookieContext ctx;

        // GET api/<controller>
        [ResponseType(typeof(KepResponseModel))]
        public HttpResponseMessage Get()
        {
            using (ctx = new BookieContext())
            {
                var res = ctx.Kepek.ToList();
                var response = new List<KepResponseModel>();

                foreach (var item in res)
                {
                    response.Add(new KepResponseModel(item));
                }

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
        }

        // GET api/<controller>/5
        [ResponseType(typeof(KepResponseModel))]
        public HttpResponseMessage Get(int id)
        {
            using (ctx = new BookieContext())
            {
                var result = ctx.Kepek.Where(x => x.Id == id).FirstOrDefault();
                if (result != null)
                {
                    var response = new KepResponseModel(result);
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
        // GET api/<controller>/5
        [ResponseType(typeof(KepResponseModel))]
        [Route("api/Kep/GetFromTermekId")]
        public HttpResponseMessage Get(string termekId)
        {
            using (ctx = new BookieContext())
            {
                var result = ctx.Kepek.Where(x => x.TermekId.ToString() == termekId).ToList();
                if (result != null)
                {
                    var response = new List<KepResponseModel>();

                    foreach (var item in result)
                    {
                        response.Add(new KepResponseModel(item));
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        // POST api/<controller>
        [ResponseType(typeof(KepResponseModel))]
        public HttpResponseMessage Post([FromBody] KepCreateModel value)
        {
            using (ctx = new BookieContext())
            {
                try
                {

                    var result = ctx.EladoTermekek.Where(x => x.Id == value.TermekId).FirstOrDefault();
                    if (result != null)
                    {

                        Account account = new Account("dlzeae5mn", "781352964829158", "0ll3sXB3DZJlX8LY4ged2IG2amM");
                        Cloudinary cloudinary = new Cloudinary(account);
                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(value.EleresiUt)
                        };

                        var uploadResult = cloudinary.Upload(uploadParams);

                        Kep uj = new Kep(uploadResult.SecureUrl.ToString(), value.KepNev, value.TermekId, result);
                        ctx.Kepek.Add(uj);

                        ctx.SaveChanges();
                        var response = new KepResponseModel(uj);
                        return Request.CreateResponse(HttpStatusCode.OK, response);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Can't find EladoTermek with this Id");
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, $"{ex.Message}");
                }

            }
        }
        // DELETE api/<controller>/5
        [ResponseType(typeof(KepResponseModel))]
        public HttpResponseMessage Delete(int id)
        {
            using (ctx = new BookieContext())
            {
                var result = ctx.Kepek.Where(x => x.Id == id).FirstOrDefault();
                if (result != null)
                {
                    ctx.Kepek.Remove(result);
                    ctx.SaveChanges();
                    var response = new KepResponseModel(result);
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);

            }
        }
    }
}