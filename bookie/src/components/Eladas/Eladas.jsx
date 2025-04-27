import { React, useEffect } from "react"
import './Eladas.css'
import { SearchResultsList } from "./SearchResultsList";
import { SellProduct } from "../assets/scripts/newproduct";
import { useState } from "react"
import { nologin } from "../assets/scripts/nologin";
import { useNavigate } from "react-router-dom";

function Eladas() {
    nologin()
    //searchbar
    const [input, setInput] = useState("");
    const [results, setResults] = useState([]);
    const [price, setPrice] = useState();
    const [trade, setTrade] = useState();
    const [files, setFiles] = useState();
    const navigate=useNavigate()
    const fetchData = (value) => {
        fetch("https://localhost:44317/api/Konyv")
            .then(response => {
                return response.json()
            })
            .then((data) => {
                const results = data.filter((adat) => {
                    return (
                        value &&
                        adat &&
                        adat.Cim &&
                        adat.Cim.toLowerCase().includes(value.toLowerCase())
                    );
                });
                setResults(results);
            });
    };
    const handleChange = (value) => {
        setInput(value);
        fetchData(value);
    };
    function hataratlepes(e){
        e.target.value<0 ?  setPrice(0) : setPrice(e.target.value)
    }
    //searchbar
    function uploadproduct() {
        if (document.getElementById("inputfile").files[0]!="" && input!="" && price!="") {
            const reader = new FileReader();
            reader.readAsDataURL(document.getElementById("inputfile").files[0]);
            reader.onload = () => {
                SellProduct(reader.result)
            };
        }
        else{
            alert("Töltsél ki minden mezőt!")
        }
    }
    document.body.addEventListener("load", () => {
        var eladotermek = JSON.parse(localStorage.getItem("eladotermek"))
        if (window.location.href == "http://localhost:3000/Eladas") {
            if (eladotermek != null) {
                setInput(eladotermek.Cim)
              
                setPrice(eladotermek.Price)
                setTrade(eladotermek.Trade)
                setFiles(eladotermek.Pictures)
            }
        }
    }, true)
    return (
        <div className="eladas">
            <div className="input-wrapper">
                <input className="search"
                    id="konyvvv"
                    placeholder="Keresés"
                    value={input}
                    onChange={(e) => handleChange(e.target.value)}
                />
                <SearchResultsList results={results} />
                <button className="nokonyv" onClick={() => { navigate("/NewBook") }}>
                    Nincs itt a könyved?
                </button>
            </div>
            <div>
                <span>Ár (Ft): </span>
                <input type="number" id="prizeinput" min="0" value={price} onChange={(e) => hataratlepes(e)} />
            </div>
            <div>
                <span>Csere:</span>
                <input type="checkbox" name="Csere" id="cserebox" checked={trade} onChange={() => { setTrade(!trade) }} />
            </div>
            <div>
                <span id="kepSpan">Kép (max 3 MB): </span>
                <input type="file" accept=".png, .jpeg, .jpg" id="inputfile" file={files} onChange={(e) => { setFiles(e.target.value) }} />
            </div>
            <div></div>
            <div>
                <button id="elad" onClick={uploadproduct}>Eladás</button>
            </div>
        </div>
    )
}

export default Eladas;
