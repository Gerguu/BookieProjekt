using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace bookieAPI.Models
{
    public class Kapcsolo
    {
        /*
           `KonyvId` int(11) NOT NULL,
            `SzerzoId` int(11) NOT NULL
         */
        [Key]
        [Column(Order = 1)]
        public int KonyvId { get; set; }
        public virtual Konyv Konyv { get; set; }

        [Key]
        [Column(Order = 2)]
        public int SzerzoId { get; set; }
        public virtual Szerzo Szerzo { get; set; }
        public Kapcsolo() { }
        public Kapcsolo(int bookid, int authorid, Konyv book, Szerzo author)
        {
            KonyvId = bookid;
            SzerzoId = authorid;
            Konyv = book;
            Szerzo = author;
        }
    }
}