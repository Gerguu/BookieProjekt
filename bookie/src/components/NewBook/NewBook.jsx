import { React } from "react"
import './NewBook.css'
import { NewBook } from "../assets/scripts/newbook.js"
import { nologin } from "../assets/scripts/nologin.js"

function Eladas() {
    nologin()
    function felvclicked(){
        NewBook()
    }
    function hataratlepes(e){
        e.target.value>2025 ? e.target.value=2025 : e.target.value=e.target.value
        e.target.value<0 ? e.target.value=0 : e.target.value=e.target.value
    }
    return (
        <div className="ujkonyv">
            <div>
                <span>Könyv címe: </span>
                <input type="text" id="ujkonyvvv" />
            </div>
            <div>
                <span>Kiadási éve: </span>
                <input type="number" min="0" max="2025" name="" id="kiadasev" onChange={(e)=>hataratlepes(e)} />
            </div>
            <div>
                <span>Szerző/Kiadó neve:</span>
                <input type="text" name="" id="newauthor" />
            </div>
            <div></div>
            <div>
                <button id="ujkonyvgomb" onClick={()=>felvclicked()}>Felvétel</button>
            </div>
        </div>
    )
}

export default Eladas;
