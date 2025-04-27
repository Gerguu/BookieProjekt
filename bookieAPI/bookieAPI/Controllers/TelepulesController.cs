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
    public class TelepulesResponseModel
    {
        public int KSH { get; set; }
        public string TelepulesNev { get; set; }
        public double SzelessegiFok { get; set; }
        public double HosszusagiFok { get; set; }

        public TelepulesResponseModel(Telepules Telepules)
        {
            KSH = Telepules.KSH;
            TelepulesNev = Telepules.TelepulesNev;
            SzelessegiFok = Telepules.SzelessegiFok;
            HosszusagiFok = Telepules.HosszusagiFok;
        }
    }

    public class TelepulesController : ApiController
    {
        BookieContext ctx;

        // GET api/<controller>
        [ResponseType(typeof(TelepulesResponseModel))]
        public HttpResponseMessage Get()
        {
            using (ctx = new BookieContext())
            {
                var res = ctx.Telepulesek.ToList();
                var response = new List<TelepulesResponseModel>();

                foreach (var item in res)
                {
                    response.Add(new TelepulesResponseModel(item));
                }

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
        }

        // GET api/<controller>/5
        [ResponseType(typeof(TelepulesResponseModel))]
        public HttpResponseMessage Get(int id)
        {
            using (ctx = new BookieContext())
            {
                var result = ctx.Telepulesek.Where(x => x.KSH == id).FirstOrDefault();
                if (result != null)
                {
                    var response = new TelepulesResponseModel(result);
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}