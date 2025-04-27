using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bookieAPI.Models
{
    public class EladoTermek
    {
        /*
           `Id` int(11) NOT NULL,
          `Ar` int(11) NOT NULL,
          `Csere` tinyint(1) NOT NULL,
          `Aktiv` tinyint(1) NOT NULL,
          `FelrakasDatum` date NOT NULL,
          `EladasDatum` date NOT NULL,
          `Felhasznalonev` varchar(255) NOT NULL,
          `KepId` int(11) NOT NULL,
          `KonyvId` int(11) NOT NULL
         */
        public int Id { get; set; }
        public int Ar { get; set; }
        public bool Csere { get; set; }
        public bool Aktiv { get; set; }
        public DateTime FelrakasDatum { get; set; }
        public DateTime EladasDatum { get; set; }
        public string Felhasznalonev { get; set; }
        public virtual Profil Profil { get; set; }
        public int KonyvId { get; set; }
        public virtual Konyv Konyv { get; set; }
        public EladoTermek() { }

        public EladoTermek(int price, bool trade, bool active, DateTime uploaddate, DateTime selldate, string username, int bookid)
        {
            Ar = price;
            Csere = trade;
            Aktiv = active;
            FelrakasDatum = uploaddate;
            EladasDatum = selldate;
            Felhasznalonev = username;
            KonyvId = bookid;
        }
    }
}