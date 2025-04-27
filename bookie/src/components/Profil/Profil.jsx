import { React, useEffect, useState } from "react"
import './Profil.css'
import '../Vasarlas/Vasarlas.css'
import { useNavigate } from "react-router-dom";
import { Patch } from "../assets/scripts/patch";
import { validation } from "../assets/scripts/validation";
import { nologin } from "../assets/scripts/nologin";
import { Kapcsolat } from "../assets/scripts/kapcsolat";

function Profil() {
    nologin()
    var user = JSON.parse(localStorage.getItem("user"))
    function update(e,type) {

        var vane=e.target.classList.contains("kijelolve")
        document.querySelectorAll(".kijelolve").forEach(element=>{
            element.classList.remove("kijelolve")
        })
        if(!vane){
            e.target.classList.add("kijelolve")
        }
        var input = document.getElementById(type + "update")
        document.querySelectorAll(".nemkivalaszott").forEach(element=>{
            element==input && element.disabled==true ? element.disabled=false : element.disabled=true
        })
        
        switch (type) {
            case "fullname": {
                if (input.value == "") {
                    input.value = user.TeljesNev
                }
            }
            case "telefon": {
                if (input.value == "") {
                    input.value = user.Telefonszam
                }
            }
            case "telepules": {
                if (input.value == "") {
                    input.value = user.Telepules.TelepulesNev
                }
            }
        }
    }
    function save() {
        var joe = true
        document.querySelectorAll(".kiir").forEach(element => {
            if (element.innerHTML != "") joe = false
        })
        if (joe) {
            var oldpasswd = prompt("Kérlek add meg a jelszavad: ")
            Patch(oldpasswd)
        }
        else {
            alert("Hibás adat(ok)!")
        }

    }
    var bovebb;
    var updatepatch = (<div className="bovebben">
        <div>
            <span>Új Jelszó: </span>
            <input type="password" id="passwordupdate" className="nemkivalaszott" onChange={(e) => { validation("passwd", e.target.value, 0) }} disabled={true} />
            <button className="bovebbenButton" onClick={(e) => { update(e,"password") }}>Módosítás</button>
        </div>
        <div className="kiir hidden"></div>
        <div>
            <span>Teljes Név: </span>
            <input type="text" id="fullnameupdate" className="nemkivalaszott" placeholder={user.TeljesNev} disabled={true} />
            <button className="bovebbenButton" onClick={(e) => { update(e,"fullname") }}>Módosítás</button>
        </div>
        <div>
            <span>Telefonszám: </span>
            <input type="text" id="telefonupdate" className="nemkivalaszott" placeholder={user.Telefonszam} onChange={(e) => { validation("telefonszam", e.target.value, 1) }} disabled={true} />
            <button className="bovebbenButton" onClick={(e) => { update(e,"telefon") }}>Módosítás</button>
        </div>
        <div className="kiir hidden"></div>
        <div>
            <span>Település: </span>
            <input type="text" id="telepulesupdate" className="nemkivalaszott" placeholder={user.Telepules.TelepulesNev} disabled={true} />
            <button className="bovebbenButton" onClick={(e) => { update(e,"telepules") }}>Módosítás</button>
        </div>
        <div className="kiir hidden"></div>
        <button className="hatter" id="mentesButton" onClick={() => save()} >Mentés</button>
    </div>)
    const [bovebben, setbovebben] = useState(updatepatch)
    function profilesettings() {
        setbovebben(updatepatch)
    }
    const navigate = useNavigate();
    function HandleClick(id) {
        navigate(`/Profil/Termekeim/${id}`);
    }
    var bovebbek = []
    useEffect(()=>{
        fetch("https://localhost:44317/api/EladoTermek")
            .then((response) => {
                return response.json()
            })
            .then(data => {
                //console.log(data)
                bovebbek = []
                data.forEach(termek => {
                    if (termek.Profil.Felhasznalonev == user.Felhasznalonev) {
                        var asd = (
                            <div className={`profiltermek termek ${!termek.Aktiv ? "eladvatermek" : ""}`}>
                                <div><img src={termek.Kep[0].EleresiUt} onClick={() => HandleClick(termek.Id)} /></div>
                                <div><span id='termekname'>{termek.Konyv.Konyv.Cim}</span></div>
                                <div><span id='termekpriceprofil'>{termek.Ar} Ft</span></div>
                            </div>)
                        if(!bovebbek.includes(asd)){
                            bovebbek.push(asd)
                        }
                    }
                });
            })
    })
    function myproducts() {
        document.querySelectorAll(".kiir").forEach(element => {
            element.innerHTML=""
        })
        var legbovebb = (
            <div className="profiltermekek">
                {bovebbek}
            </div>
        )
        setbovebben(legbovebb)
    }
    function getintouch() {
        bovebb = (
            <div className="bovebben">
                <span id="fennakadas">Ha fennakadtál, hibát észleltél vagy csak problémába ütköztél, akkor vedd fel velünk a kapcsolatot, mi pedig megpróbálunk minél hamarabb segíteni Neked!</span>
                <textarea id="getintouchTextarea">

                </textarea>
                <button id="getintouchButton" className="hatter" onClick={() => {Kapcsolat(document.getElementById("getintouchTextarea").value) }}>
                    Küldés
                </button>
            </div>
        )
        setbovebben(bovebb)
    }
    function logout() {
        localStorage.clear()
        window.location.href = "http://localhost:3000/Login"
    }


    return (

        <div className="settingspage">
            <div className="settings">
                <input type="button" value="Profilbeállítások" onClick={profilesettings} />
                <input type="button" value="Termékeim" onClick={myproducts} />
                <input type="button" value="Kapcsolat" onClick={getintouch} />
                <span id="copyright"> Minden jog fenntartva © <br /> bookie.hu <br /> 1.0.1 </span>
                <input type="button" value="Kijelentkezés" onClick={logout} />
            </div>
            <div className="bovebb">
                {bovebben}
            </div>
        </div>
    )
}

export default Profil