using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bookieAPI.Models
{
    public class Konyv
    {
        /*
           `Id` int(11) NOT NULL,
          `Cim` varchar(255) NOT NULL,
          `KiadasEv` date NOT NULL
         */
        public int Id { get; set; }
        public string Cim { get; set; }
        public int KiadasEv { get; set; }
        public Konyv() { }
        public Konyv(string title, int releasedate)
        {
            Cim = title;
            KiadasEv = releasedate;
        }
    }
}