import './Vasarlas.css'
import { Link, useNavigate } from "react-router-dom";
import { React, useState, useEffect } from "react"
import { nologin } from '../assets/scripts/nologin';


function Vasarlas() {
    nologin()
    const [product, setProduct] = useState()
    const [konyvcim, setKonyvcim] = useState("")
    const [sort, setSort] = useState()
    const [szerzoNev, setSzerzoNev] = useState("")
    const [trade, setTrade] = useState()
    const navigate = useNavigate();
    function HandleClick(id) {
        navigate(`/Termek/${id}`);
    }
    function HandleMyClick(id) {
        navigate(`/Vasarlas/Termekeim/${id}`);
    }
    const [felhasznalo, SetUser] = useState(null)
    useEffect(() => {
        let storedUser = localStorage.getItem("user")
        if (storedUser) {
            SetUser(storedUser)
        }
    }, [])

    function ProductClick(adat) {
        localStorage.setItem("productsort", JSON.stringify({ Trade: trade, KonyvCim: konyvcim, Sort: sort, SzerzoNev: szerzoNev }))
        //console.log()
        if(adat.Felhasznalonev==JSON.parse(localStorage.getItem("user")).Felhasznalonev){
            HandleMyClick(adat.Id)
        }
        else{
            HandleClick(adat.Id)
        }
    }
    function deg2rad(deg) {
        return deg * (Math.PI/180)
      }
    function DistanceTo(latitude, longitude)
    {
        var user = JSON.parse(localStorage.getItem("user"))
            var R = 6371;
            var dLat = deg2rad(latitude - user.Telepules.SzelessegiFok);
            var dLon = deg2rad(longitude - user.Telepules.HosszusagiFok); 
            var a = 
              Math.sin(dLat/2) * Math.sin(dLat/2) +
              Math.cos(deg2rad(latitude)) * Math.cos(deg2rad(user.Telepules.SzelessegiFok)) * 
              Math.sin(dLon/2) * Math.sin(dLon/2)
              ; 
            var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1-a)); 
            var d = R * c;
            return Math.round(d); 
    }
    useEffect(() => {
        fetch("https://localhost:44317/api/EladoTermek")
            .then((response) => {
                return response.json()
            })
            .then(data => {
                var cards = []
                switch (sort) {
                    case ("asc"): data.sort(function (a, b) { return a.Ar - b.Ar })
                        break;
                    case ("desc"): data.sort(function (a, b) { return b.Ar - a.Ar })
                        break;
                    case ("closest"): data.sort(function (a, b) { return DistanceTo(a.Profil.Telepules.SzelessegiFok, a.Profil.Telepules.HosszusagiFok) - DistanceTo(b.Profil.Telepules.SzelessegiFok,  b.Profil.Telepules.HosszusagiFok) })//idk
                        break;
                    case ("newest"): data.sort(function (a, b) { return new Date(a.FelrakasDatum).toISOString().substring(0, 10) > new Date(b.FelrakasDatum).toISOString().substring(0, 10) ? 1 : -1 })
                        break;
                    case ("oldest"): data.sort(function (a, b) { return new Date(a.FelrakasDatum).toISOString().substring(0, 10) < new Date(b.FelrakasDatum).toISOString().substring(0, 10) ? 1 : -1 })
                        break;
                }
                //console.log(data)
                data.forEach(adat => {
                    //localStorage.setItem(`termek${adat.Id}`, JSON.stringify(adat))
                    if (adat.Aktiv) {
                        var card = (
                            <div className={`termek ${adat.Felhasznalonev==JSON.parse(localStorage.getItem("user")).Felhasznalonev ? "sajat" : ""}`} id={`termek${adat.Id}`}>
                                <div><img src={adat.Kep[0].EleresiUt} onClick={() => { !felhasznalo ? window.location.href = "http://localhost:3000/Login" : ProductClick(adat) }} /></div>
                                <div><span id='termekname'>{adat.Konyv.Konyv.Cim}</span></div>
                                <div><span id='termekprice'>{adat.Ar} Ft</span><span id='termekdistance' >{adat.Felhasznalonev==JSON.parse(localStorage.getItem("user")).Felhasznalonev ? "" : DistanceTo(adat.Profil.Telepules.SzelessegiFok, adat.Profil.Telepules.HosszusagiFok) +" km"}</span></div>
                            </div>)
                        var cserebox = document.getElementById("csereidspan")
                        var csere = document.getElementById("cseresearch")
                        if (csere != null) {
                            if (csere.checked) {
                                if (adat.Konyv.Konyv.Cim.toLowerCase().includes(konyvcim.toLowerCase()) && adat.Konyv.Szerzo.Nev.toLowerCase().includes(szerzoNev.toLowerCase())) {
                                    if (adat.Csere) {
                                        cards.push(card)
                                    }
                                }
                                cserebox.innerText = "Csak cserélhető"
                                cserebox.style.color = "lightgreen"
                            }
                            else {
                                if (adat.Konyv.Konyv.Cim.toLowerCase().includes(konyvcim.toLowerCase()) && adat.Konyv.Szerzo.Nev.toLowerCase().includes(szerzoNev.toLowerCase())) {
                                    cards.push(card)
                                }
                                cserebox.innerText = "Összes"
                                cserebox.style.color = "black"
                            }
                        }
                    }
                });
                setProduct(cards)
            })
            .catch((error) => {
                console.error("Hiba történt:", error);
            })
    })

    document.body.addEventListener("load", () => {
        var productsorts = localStorage.getItem("productsort")
        var productsort = JSON.parse(productsorts)
        //if(igaz) localStorage.setItem("productsort",null)
        if (window.location.href == ( "http://localhost:3000/Vasarlas" || "http://localhost:3000/")) {
            if (productsort) {
                //console.log("productsort")
                setKonyvcim(productsort.KonyvCim)
                setSort(productsort.Sort)
                setSzerzoNev(productsort.SzerzoNev)
                setTrade(productsort.Trade)

                setTimeout(() => {
                    localStorage.removeItem("productsort")
                }, 100);
            }
        }
    }, true)
    return (
        <div className="content">
            <div className="sort">
                <span>Szűrés könyvre: </span>
                <input type="text" placeholder="Könyv címe" value={konyvcim} onChange={(e) => { setKonyvcim(e.target.value) }} />
                <span>Szűrés szerzőre: </span>
                <input type="text" placeholder="Szerző neve" value={szerzoNev} onChange={(e) => { setSzerzoNev(e.target.value) }} />
                <div id="csereDiv">
                    <span id="csereidspan" >Minden Termék</span>
                    <input type="checkbox" name="" id="cseresearch" checked={trade} onChange={() => { setTrade(!trade)  }} />
                </div>
                <select name="" id="" value={sort} onChange={(e) => { setSort(e.target.value) }}>
                    <option value="asc">Legolcsóbb</option>
                    <option value="desc">Legdrágább</option>
                    <option value="closest">Legközelebb</option>
                    <option value="newest">Legújabb</option>
                    <option value="oldest">Legrégebbi</option>
                </select>
                
            </div>
            <div className="termekek" id="termekek">
                {product}
            </div>
        </div>
    )
}

export default Vasarlas