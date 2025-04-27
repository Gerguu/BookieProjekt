import { React, useState, useEffect } from "react"
import {useParams } from "react-router";
import "./Termek.css"
import { useNavigate } from "react-router-dom";
import { SendMessage } from "../assets/scripts/sendmessage";
import { nologin } from "../assets/scripts/nologin";
import MyMap from "./Map";

function Termek() {
    nologin()
    const productId = useParams();
    const [belso, setBelso] = useState();
    const navigate = useNavigate();
    useEffect(() => {
        fetch(`https://localhost:44317/api/EladoTermek/${productId.termekId}`)
            .then(response => { return response.json() })
            .then(product => {
                if(JSON.parse(localStorage.getItem("user")).Felhasznalonev == product.Profil.Felhasznalonev){
                    navigate(`/Vasarlas/Termekeim/${productId.termekId}`);
                }
                setBelso(<div id="termekNagyDiv">
                    <div id="termekKepDiv">
                        <img id="termekKep" src={product.Kep[0].EleresiUt} alt={product.Konyv.Nev} />
                    </div>
                    <div className="termekMindenDiv">
                        <span id="szerzokSpan">{product.Konyv.Szerzo.Nev}:</span>
                        <h1 id="termekcim">{product.Konyv.Konyv.Cim}</h1>
                        <h2>{product.Ar} Ft</h2><h3 id="csereidszin" > {product.Csere ? "Cserélhető" : ""}</h3>
                        <h5>Feltöltés Dátuma: {new Date(product.FelrakasDatum).toISOString().substring(0, 10)}</h5>
                        <div id="termekEgySorDiv">
                            <div id="termekFelhasznalonevDiv">
                                <span id="termekFelhasznalonev">
                                    {product.Profil.Felhasznalonev}
                                </span>
                            </div>
                            <div id="termekUzenetDiv">
                                <button className="hatter" id="termekUzenet" onClick={() => { uzenet() }}>Üzenet</button>
                            </div>
                            <div id="termekTelepulesDiv">
                                <span id="termekTelepules">
                                    {product.Profil.Telepules.TelepulesNev}
                                </span>
                            </div>
                        </div>
                        
                        <MyMap lati={product.Profil.Telepules.SzelessegiFok} longi={product.Profil.Telepules.HosszusagiFok} />
                    </div>
                </div>)
                function uzenet() {
                    SendMessage(product.Felhasznalonev, "", true)
                    localStorage.setItem("ujbeszelgetes", product.Felhasznalonev)
                    localStorage.setItem("utsobeszelgetes", product.Felhasznalonev)
                    navigate("/Chat")
                    //window.location.href="http://localhost:3000/Chat"
                }
            })
    })

    return (
        <div className="maintermek">
            {belso}
        </div>
    );
};

export default Termek;
