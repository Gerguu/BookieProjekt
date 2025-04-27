import { React, useEffect, useState } from "react"
import { useNavigate, useParams } from "react-router";
import "../Termek/Termek.css"
import { MarkSold } from "../assets/scripts/booksold";
import { DeleteProduct } from "../assets/scripts/deleteproduct";
import { nologin } from "../assets/scripts/nologin";

function Termekeim() {
    nologin()
    const productId = useParams();
    const [belso, setBelso] = useState();
    var href = window.location.href.toString().split('/')
    const navigate= useNavigate()

    useEffect(()=>{
        fetch(`https://localhost:44317/api/EladoTermek/${productId.termekemId}`)
        .then(response => { return response.json() })
        .then(product => {
            if(JSON.parse(localStorage.getItem("user")).Felhasznalonev != product.Profil.Felhasznalonev && !window.location.href.includes("/Termek/")){
                navigate(`/Termek/${productId.termekemId}`);
            }
            setBelso(<div id="sajatTermek">
                <div id="sajatTermekKepDiv">
                    <img id="sajatTermekKep" src={product.Kep[0].EleresiUt} alt={product.Konyv.Nev} />
                </div>
                <div id="sajatTermekDiv">
                    <h3>{product.Konyv.Szerzo.Nev}:</h3>
                    <h1 id="sajatTermekCim">{product.Konyv.Konyv.Cim}</h1>
                    <h2>{product.Ar} Ft</h2>
                    <button id="sajatTermekGomb1" className={`hatter ${!product.Aktiv ? "eladvatermek" : ""}`} onClick={() => { MarkSold(product.Id, href[3]) }} disabled={!product.Aktiv} >Megjelölés Eladottként</button>
                    <br id="break"></br>
                    <button id="sajatTermekGomb2" className="hatter" onClick={() => { DeleteProduct(product.Id, href[3]) }}>Hirdetés Törlése</button>
                </div>
            </div>)
        })
    })
    return (
        <div className="maintermek">
            {belso}
        </div>
    );
};

export default Termekeim;
