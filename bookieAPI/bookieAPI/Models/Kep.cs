using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace bookieAPI.Models
{
    public class Kep
    {
        /*
           `Id` int(11) NOT NULL,
          `EleresiUt` varchar(255) NOT NULL,
          `KepNev` varchar(255) NOT NULL
         */
        public int Id { get; set; }
        public string EleresiUt { get; set; }
        public string KepNev { get; set; }
        [ForeignKey("EladoTermek")]
        public int TermekId { get; set; }
        public virtual EladoTermek EladoTermek { get; set; }
        public Kep() { }
        public Kep(string path, string picturename, int productid, EladoTermek product)
        {
            EleresiUt = path;
            KepNev = picturename;
            TermekId = productid;
            EladoTermek = product;
        }
    }
}