using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace bookieAPI.Models
{
    public class Telepules
    {
        /*
          `KSH` int(11) NOT NULL,
          `TelepulesNev` varchar(255) NOT NULL
         */
        [Key]
        public int KSH { get; set; }
        public string TelepulesNev { get; set; }
        public double SzelessegiFok { get; set; }
        public double HosszusagiFok { get; set; }
        public Telepules() { }
        public Telepules(int KSH, string cityname, double latitude, double longitude)
        {
            this.KSH = KSH;
            TelepulesNev = cityname;
            SzelessegiFok = latitude;
            HosszusagiFok = longitude;
        }
    }
}