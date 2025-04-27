using bookieAPI;
using bookieAPI.Controllers;
using bookieAPI.Database;
using bookieAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Web.Http.SelfHost;
using Ubiety.Dns.Core;

namespace BookieTests
{
    [TestClass]
    public class ProfilControllerTest
    {
        HttpClient _client;

        /* private HttpSelfHostServer _server;
         const string _serverAddress = "https://localhost:44317";*/
        [TestInitialize]
        public void Setup()
        {
            /*
            var config = new HttpSelfHostConfiguration(_serverAddress)
            {
                HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.Exact
            };
            
            WebApiConfig.Register(config);
            _server = new HttpSelfHostServer(config);
            _server.OpenAsync().Wait();
            _client = new HttpClient { BaseAddress = new Uri(_serverAddress) };
            */
            
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:44317");
            
        }
        
        [TestCleanup]
        public void Shutdown()
        {
            try
            {
                _client.DeleteAsync($"/api/Profil/testfelhasznalonev");
            }
            catch
            {
            }
        }
        
        [TestMethod]
        public void PostSucceeds()
        {
            var response = _client.PostAsJsonAsync("/api/Profil", new ProfilCreateModel
            {
                Felhasznalonev = "testfelhasznalonev",
                Email = "testemail",
                Jelszo = "testjelszo",
                Telefonszam = "testtelefonszam",
                TelepulesNev = "Szombathely",
                TeljesNev = "testteljesnev"
            }).Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        [TestMethod]
        public void PostReturnsNotFound()
        {
            var response = _client.PostAsJsonAsync("/api/Profil", new ProfilCreateModel
            {
                Felhasznalonev = "testfelhasznalonev",
                Email = "testemail",
                Jelszo = "testjelszo",
                Telefonszam = "testtelefonszam",
                TelepulesNev = "nincsilyentelepules",
                TeljesNev = "testteljesnev"
            }).Result;
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
        [TestMethod]
        public void PostReturnsEmailExists()
        {
            var response = _client.PostAsJsonAsync("/api/Profil", new ProfilCreateModel
            {
                Felhasznalonev = "testfelhasznalonev",
                Email = "test.email@gmail.com",//pelda felhasznalo emailje
                Jelszo = "testjelszo",
                Telefonszam = "testtelefonszam",
                TelepulesNev = "Szombathely",
                TeljesNev = "testteljesnev"
            }).Result;
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
        }

        [TestMethod]
        public void AuthenticateWorks()
        {
            var response = _client.PostAsJsonAsync("/api/Profil", new ProfilCreateModel
            {
                Felhasznalonev = "testfelhasznalonev",
                Email = "testemail",
                Jelszo = "testjelszo",
                Telefonszam = "testtelefonszam",
                TelepulesNev = "Szombathely",
                TeljesNev = "testteljesnev"
            }).Result;

            //Ok
            var response3 = _client.PostAsJsonAsync("/api/Profil/authenticate", new AuthenticationModel
            {
                Email = "testemail",
                Jelszo = "testjelszo"
            }).Result;
            Assert.AreEqual(HttpStatusCode.OK, response3.StatusCode);

            //Nincs ilyen email
            var response4 = _client.PostAsJsonAsync("/api/Profil/authenticate", new AuthenticationModel
            {
                Email = "nincsilyenemail",
                Jelszo = "testjelszo"
            }).Result;
            Assert.AreEqual(HttpStatusCode.NotFound, response4.StatusCode);

            //Rossz jelszó
            var response5 = _client.PostAsJsonAsync("/api/Profil/authenticate", new AuthenticationModel
            {
                Email = "testemail",
                Jelszo = "nemezajelszo"
            }).Result;
            Assert.AreEqual(HttpStatusCode.Unauthorized, response5.StatusCode);
        }
        
        [TestMethod]
        public void PutWorks()
        {

            var response = _client.PostAsJsonAsync("/api/Profil", new ProfilCreateModel
            {
                Felhasznalonev = "testfelhasznalonev",
                Email = "testemail",
                Jelszo = "testjelszo",
                Telefonszam = "testtelefonszam",
                TelepulesNev = "Szombathely",
                TeljesNev = "testteljesnev"
            }).Result;

            var adatok = response.Content.ReadAsStringAsync().Result;
            var adat= JsonConvert.DeserializeObject<ProfilResponseModel>(adatok);

            //Ok
            var response6 = _client.PutAsJsonAsync($"/api/Profil/{adat.Felhasznalonev}", new ProfilUpdateModel
            {
                RegiJelszo="testjelszo",
                UjJelszo="tesztesebbjelszo",
                Email = "testemail",
                Telefonszam = "testtelefonszam",
                TelepulesNev = "Sopron",
                TeljesNev = "testteljesnev"
            }).Result;
            Assert.AreEqual(HttpStatusCode.OK, response6.StatusCode);

            //Rossz jelszo
            var response7 = _client.PutAsJsonAsync($"/api/Profil/{adat.Felhasznalonev}", new ProfilUpdateModel
            {
                RegiJelszo = "nemezajelszavam",
                UjJelszo = "tesztesebbjelszo",
                Email = "testemail",
                Telefonszam = "testtelefonszam",
                TelepulesNev = "Sopron",
                TeljesNev = "testteljesnev"
            }).Result;
            Assert.AreEqual(HttpStatusCode.Unauthorized, response7.StatusCode);

            //Nem talal ilyen felhasznalot 
            var response8 = _client.PutAsJsonAsync("/api/Profil/nincsilyenfelhasznalonev", new ProfilUpdateModel
            {
                RegiJelszo = "testjelszo",
                UjJelszo = "tesztesebbjelszo",
                Email = "testemail",
                Telefonszam = "testtelefonszam",
                TelepulesNev = "Sopron",
                TeljesNev = "testteljesnev"
            }).Result;
            Assert.AreEqual(HttpStatusCode.NotFound, response8.StatusCode);

            //Mar van ilyen email
            var response9 = _client.PutAsJsonAsync($"/api/Profil/{adat.Felhasznalonev}", new ProfilUpdateModel
            {
                RegiJelszo = "tesztesebbjelszo",
                UjJelszo = "legtesztesebbjelszo",
                Email = "test.email@gmail.com",//pelda felhasznalo emailje
                Telefonszam = "testtelefonszam",
                TelepulesNev = "Sopron",
                TeljesNev = "testteljesnev"
            }).Result;
            Assert.AreEqual(HttpStatusCode.Conflict, response9.StatusCode);

            //telepules nem talalhato
            var response10 = _client.PutAsJsonAsync($"/api/Profil/{adat.Felhasznalonev}", new ProfilUpdateModel
            {
                RegiJelszo = "tesztesebbjelszo",
                UjJelszo = "tesztjelszo",
                Email = "legtestesebbemail",
                Telefonszam = "testtelefonszam",
                TelepulesNev = "nincsilyentelepules",
                TeljesNev = "testteljesnev"
            }).Result;
            Assert.AreEqual(HttpStatusCode.NotFound, response10.StatusCode);
        }

        [TestMethod]
        public void GetGets()
        {
            var respnse = _client.GetAsync("/api/Profil").Result;
            var adatok = respnse.Content.ReadAsStringAsync().Result;
            List<ProfilResponseModel> lissta = new List<ProfilResponseModel>();
            lissta = JsonConvert.DeserializeObject<List<ProfilResponseModel>>(adatok);
            Assert.IsNotNull(lissta);
        }
        [TestMethod]
        public void GetWithUsernameGets()
        {
            var response = _client.PostAsJsonAsync("/api/Profil", new ProfilCreateModel
            {
                Felhasznalonev = "testfelhasznalonev",
                Email = "testemail",
                Jelszo = "testjelszo",
                Telefonszam = "testtelefonszam",
                TelepulesNev = "Szombathely",
                TeljesNev = "testteljesnev"
            }).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            var adat = JsonConvert.DeserializeObject<ProfilResponseModel>(result);

            var respnse = _client.GetAsync($"/api/Profil/{adat.Felhasznalonev}").Result;
            var adatok = respnse.Content.ReadAsStringAsync().Result;
            //List<ProfilResponseModel> lissta = new List<ProfilResponseModel>();
            var lissta = JsonConvert.DeserializeObject<ProfilResponseModel>(adatok);
            Assert.IsNotNull(lissta);
        }
        [TestMethod]
        public void GetWithUsernameReturnsNotFound()
        {
            var response = _client.GetAsync("/api/Profil/nincsilyenfelhasznalonev").Result;
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void DeleteWorks()
        {
            var response = _client.PostAsJsonAsync("/api/Profil", new ProfilCreateModel
            {
                Felhasznalonev = "testfelhasznalonev",
                Email = "testemail",
                Jelszo = "testjelszo",
                Telefonszam = "testtelefonszam",
                TelepulesNev = "Szombathely",
                TeljesNev = "testteljesnev"
            }).Result;
            var adatok = response.Content.ReadAsStringAsync().Result;
            var adat = JsonConvert.DeserializeObject<ProfilResponseModel>(adatok);

            var response2 = _client.DeleteAsync($"/api/Profil/{adat.Felhasznalonev}").Result;
            Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);

            var response8 = _client.DeleteAsync($"/api/Profil/nincsilyenfelhasznalonev").Result;
            Assert.AreEqual(HttpStatusCode.NotFound, response8.StatusCode);
        }
    }
}
