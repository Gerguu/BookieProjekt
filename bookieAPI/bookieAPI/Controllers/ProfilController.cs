using bookieAPI.Database;
using bookieAPI.Models;
using bookieAPI.ProfilManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace bookieAPI.Controllers
{
    public class ProfilCreateModel
    {
        public string Felhasznalonev { get; set; }
        public string Jelszo { get; set; }
        public string Email { get; set; }
        public string Telefonszam { get; set; }
        public string TeljesNev { get; set; }
        public string TelepulesNev { get; set; }
    }
    public class ProfilUpdateModel
    {
        public string RegiJelszo { get; set; }
        public string UjJelszo { get; set; }
        public string Email { get; set; }
        public string Telefonszam { get; set; }
        public string TeljesNev { get; set; }
        public string TelepulesNev { get; set; }
    }
    public class ProfilResponseModel
    {
        public string Felhasznalonev { get; set; }
        public string Email { get; set; }
        public string Telefonszam { get; set; }
        public string TeljesNev { get; set; }
        public int TelepulesKSH { get; set; }
        public Telepules Telepules { get; set; }

        public ProfilResponseModel(Profil profil)
        {
            Felhasznalonev = profil.Felhasznalonev;
            Email = profil.Email;
            Telefonszam = profil.Telefonszam;
            TeljesNev = profil.TeljesNev;
            TelepulesKSH = profil.TelepulesKSH;
            using (var ctx = new BookieContext())
            {
                Telepules = ctx.Telepulesek.Where(x => x.KSH == profil.TelepulesKSH).FirstOrDefault();
            }
        }
        public ProfilResponseModel()
        {
        }
    }

    public class AuthenticationModel
    {
        public string Email { get; set; }
        public string Jelszo { get; set; }
    }

    public class ProfilController : ApiController
    {
        // GET api/<controller>
        BookieContext ctx;

        [ResponseType(typeof(ProfilResponseModel))]
        public HttpResponseMessage Get()
        {
            using (ctx = new BookieContext())
            {
                var res = ctx.Profilok.Include(x=>x.Telepules).ToList();
                var response = new List<ProfilResponseModel>();

                foreach (var item in res)
                {
                    response.Add(new ProfilResponseModel(item));
                }

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
        }

        // GET api/<controller>/5
        [Route("api/Profil/{felhasznalonev}")]
        [ResponseType(typeof(ProfilResponseModel))]
        public HttpResponseMessage Get(string felhasznalonev)
        {
            using (ctx = new BookieContext())
            {
                var result = ctx.Profilok.Where(x => x.Felhasznalonev == felhasznalonev).Include(y=>y.Telepules).FirstOrDefault();
                if (result != null)
                {
                    var response = new ProfilResponseModel(result);
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        // POST api/<controller>
        [ResponseType(typeof(ProfilResponseModel))]
        public HttpResponseMessage Post([FromBody] ProfilCreateModel value)
        {
            using (ctx = new BookieContext())
            {
                try
                {
                    var felhasznalo = ctx.Profilok.Where(x => x.Felhasznalonev == value.Felhasznalonev).FirstOrDefault();
                    if (felhasznalo != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.Conflict, "FELHASZNALO_EXISTS");
                    }
                    var email = ctx.Profilok.Where(x => x.Email == value.Email).FirstOrDefault();
                    if (email != null)
                        return Request.CreateResponse(HttpStatusCode.Conflict, "EMAIL_EXISTS");

                    var result = ctx.Telepulesek.Where(x=> x.TelepulesNev == value.TelepulesNev).FirstOrDefault();
                    if (result!=null)
                    {
                        ctx.Profilok.Add(new Profil
                            (value.Felhasznalonev, value.Jelszo, value.Email, value.Telefonszam, value.TeljesNev, result.KSH));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "LOCATION_NOT_FOUND");
                    }

                    //ctx.Telepulesek.Add(new Telepules(value.Telepules.Iranyitoszam, value.Telepules.TelepulesNev));

                    ctx.SaveChanges();
                    var res = ctx.Profilok.Where(x => x.Email == value.Email).Include(y => y.Telepules).FirstOrDefault();
                    var response = new ProfilResponseModel(res);

                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, $"{ex.Message}");
                }

            }
        }

        // PUT api/<controller>/5
        [Route("api/Profil/{felhasznalonev}")]
        [ResponseType(typeof(ProfilResponseModel))]
        public HttpResponseMessage Put(string felhasznalonev, [FromBody] ProfilUpdateModel value)
        {
            using (ctx = new BookieContext())
            {
                try
                {
                    var result = ctx.Profilok.Where(x => x.Felhasznalonev == felhasznalonev).FirstOrDefault();
                    if (result == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }

                    var valid = PasswordManager.VerifyPasswordHash(value.RegiJelszo, result.JelszoHash, result.JelszoSalt);
                    if (!valid)
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized);
                    }

                    if (value.Email != result.Email)
                    {
                        var email = ctx.Profilok.Where(x => x.Email == value.Email).FirstOrDefault();
                        if (email != null)
                            return Request.CreateResponse(HttpStatusCode.Conflict, "EMAIL_EXISTS");
                    }

                    result.Email = value.Email;
                    PasswordManager.CreatePasswordHash(value.UjJelszo, out byte[] hash, out byte[] salt);
                    result.JelszoHash = hash;
                    result.JelszoSalt = salt;
                    result.Telefonszam = value.Telefonszam;
                    result.TeljesNev = value.TeljesNev;
                    var city = ctx.Telepulesek.Where(x =>x.TelepulesNev == value.TelepulesNev).FirstOrDefault();
                    if (city==null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }
                    else
                    {
                        result.TelepulesKSH = city.KSH;
                        result.Telepules = city;
                    }

                    var response = new ProfilResponseModel(result);

                    ctx.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, $"{ex.Message}");
                }
            }
        }

        [Route("api/Profil/{felhasznalonev}")]
        [ResponseType(typeof(ProfilResponseModel))]
        public HttpResponseMessage Patch(string felhasznalonev, [FromBody] ProfilUpdateModel value)
        {
            using (ctx = new BookieContext())
            {
                try
                {
                    var result = ctx.Profilok.Where(x => x.Felhasznalonev == felhasznalonev).FirstOrDefault();
                    if (result == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "FELHASZNALO_NOT_FOUND");
                    }

                    if (value.Email != result.Email)
                    {
                        var email = ctx.Profilok.Where(x => x.Email == value.Email).FirstOrDefault();
                        if (email != null)
                            return Request.CreateResponse(HttpStatusCode.Conflict, "EMAIL_EXISTS");
                    }

                    var valid = PasswordManager.VerifyPasswordHash(value.RegiJelszo, result.JelszoHash, result.JelszoSalt);
                    if (!valid)
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, "WRONG_PASSWORD");
                    }
                    if (value.UjJelszo != "")
                    {
                        PasswordManager.CreatePasswordHash(value.UjJelszo, out byte[] hash, out byte[] salt);
                        result.JelszoHash = hash;
                        result.JelszoSalt = salt;
                    }
                    if (value.Telefonszam != "") result.Telefonszam = value.Telefonszam;
                    if (value.TeljesNev != "") result.TeljesNev = value.TeljesNev;

                    if (value.TelepulesNev != "")
                    {
                        var location= ctx.Telepulesek.Where(x => x.TelepulesNev == value.TelepulesNev).FirstOrDefault();
                        if (location==null)
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound, "LOCATION_NOT_FOUND");
                        }
                        result.Telepules = location;
                    }

                    ctx.SaveChanges();
                    var response = new ProfilResponseModel(result);
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, $"{ex.Message}");
                }
            }
        }

        // DELETE api/<controller>/5
        [Route("api/Profil/{felhasznalonev}")]
        [ResponseType(typeof(ProfilResponseModel))]
        public HttpResponseMessage Delete(string felhasznalonev)
        {
            using (ctx = new BookieContext())
            {
                var result = ctx.Profilok.Where(x => x.Felhasznalonev == felhasznalonev).FirstOrDefault();
                if (result != null)
                {
                    ctx.Profilok.Remove(result);
                    ctx.SaveChanges();
                    var response = new ProfilResponseModel(result);
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound, "USER_NOT_FOUND");

            }
        }

        [HttpPost]
        [Route("api/Profil/authenticate")]
        [ResponseType(typeof(ProfilResponseModel))]
        public HttpResponseMessage Authenticate([FromBody] AuthenticationModel value)
        {
            using (ctx = new BookieContext())
            {
                var result = ctx.Profilok.Where(x => x.Email == value.Email).FirstOrDefault();
                if (result==null)
                {
                    result = ctx.Profilok.Where(x => x.Felhasznalonev == value.Email).FirstOrDefault();
                }
                if (result != null)
                {
                    var valid = PasswordManager.VerifyPasswordHash
                        (value.Jelszo, result.JelszoHash, result.JelszoSalt);
                    var response = new ProfilResponseModel(result);

                    if (valid)
                        return Request.CreateResponse(HttpStatusCode.OK, response);
                    else
                        return Request.CreateResponse
                            (HttpStatusCode.Unauthorized, response);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}