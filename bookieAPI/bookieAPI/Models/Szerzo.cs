using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bookieAPI.Models
{
    public class Szerzo
    {
        /*
           `Id` int(11) NOT NULL,
            `Nev` varchar(255) NOT NULL
         */
        public int Id { get; set; }
        public string Nev { get; set; }
        public Szerzo() { }
        public Szerzo(string name)
        {
            Nev = name;
        }
    }
}