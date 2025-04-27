using bookieAPI.Controllers;
using bookieAPI.ProfilManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace bookieAPI.Models
{
    public class Profil
    {
        /*
           `Felhasznalonev` varchar(255) NOT NULL,
          `Jelszo` varchar(255) NOT NULL,
          `Email` varchar(255) NOT NULL,
          `Telefonszam` varchar(255) NOT NULL,
          `TeljesNev` varchar(255) NOT NULL,
          `TelepulesId` int(11) NOT NULL
         */
        [Key]
        public string Felhasznalonev { get; set; }
        [Column("Jelszo_hash")]
        public byte[] JelszoHash { get; set; }
        [Column("Jelszo_salt")]
        public byte[] JelszoSalt { get; set; }
        public string Email { get; set; }
        public string Telefonszam { get; set; }
        public string TeljesNev { get; set; }
        public int TelepulesKSH { get; set; }
        public virtual Telepules Telepules { get; set; }
        public Profil() { }
        public Profil(string username, string passwd, string email, string phonenumber, string fullname, int cityid)
        {
            Felhasznalonev = username;
            PasswordManager.CreatePasswordHash(passwd, out byte[] hash, out byte[] salt);
            JelszoHash = hash;
            JelszoSalt = salt;
            Email = email;
            Telefonszam = phonenumber;
            TeljesNev = fullname;
            TelepulesKSH = cityid;
        }
        public Profil(string username, string passwd, string email, string phonenumber, string fullname, int cityid, Telepules city)
        {
            Felhasznalonev = username;
            PasswordManager.CreatePasswordHash(passwd, out byte[] hash, out byte[] salt);
            JelszoHash = hash;
            JelszoSalt = salt;
            Email = email;
            Telefonszam = phonenumber;
            TeljesNev = fullname;
            TelepulesKSH = cityid;
            Telepules = city;
        }
    }
}