using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bookieAPI.Models
{
    public class Cseveges
    {
        /*
           `Id` int(11) NOT NULL,
          `Uzenet` varchar(255) NOT NULL,
          `Datum` date NOT NULL,
          `KuldoFelhasznalonev` varchar(255) NOT NULL,
          `KapoFelhasznalonev` varchar(255) NOT NULL
         */

        public int Id { get; set; }
        public string Uzenet { get; set; }
        public DateTime Datum { get; set; }
        public string KuldoFelhasznalonev { get; set; }
        public string KapoFelhasznalonev { get; set; }

        public Cseveges() { }

        public Cseveges(string message, DateTime date, string sender, string receiver)
        {
            Uzenet = message;
            Datum = date;
            KuldoFelhasznalonev = sender;
            KapoFelhasznalonev = receiver;
        }
    }
}