import { React, useState, useEffect } from "react"
import './Chat.css'
import { Link } from 'react-router-dom';
import { SendMessage } from "../assets/scripts/sendmessage";
import { nologin } from "../assets/scripts/nologin";

function Chat() {
    nologin()
    const [chat, setChat] = useState(null)
    const [uzenetek, setUzenetek] = useState()
    const [user, setUser] = useState()
    const [users, setUsers] = useState("")
    const [element, setElement] = useState()
    const [value, setValue] = useState("")
    function MessagesGen(user) {
        setElement(user)
        if (users.includes(user)) {
            localStorage.setItem("utsobeszelgetes", user)
        }
        fetch(`https://localhost:44317/api/Cseveges/kettofelhasznalo?kuldo=${JSON.parse(localStorage.getItem("user")).Felhasznalonev}&kapo=${user}`)
            .then(response => {
                return response.json()
            })
            .then(data => {
                var uzenet = []
                data.forEach((adat) => {
                    if (adat.Uzenet != "") {
                        uzenet.push(
                            <li className={adat.KapoFelhasznalonev == JSON.parse(localStorage.getItem("user")).Felhasznalonev ? "me" : "you"}>
                                <div className="entete">
                                    <h3>{new Date(adat.Datum).toISOString().substring(0, 10)}</h3>
                                </div>
                                <div className="message">
                                    {adat.Uzenet}
                                </div>
                            </li>)
                    }
                })
                setUser(<header id="userbg">
                    <h2>{user}</h2>
                </header>)
                setUzenetek(uzenet.reverse())
            })
    }
    useEffect(() => {
        fetch(`https://localhost:44317/api/Cseveges/${JSON.parse(localStorage.getItem("user")).Felhasznalonev}`)
            .then(response => {
                return response.json()
            })
            .then(data => {
                var chats = []
                var userek = []
                data.sort(function (a, b) {
                    if (a.Felhasznalonev < b.Felhasznalonev) { return -1; }
                    if (a.Felhasznalonev > b.Felhasznalonev) { return 1; }
                    return 0;
                })
                
                data.forEach(element => {
                    if (element.Felhasznalonev.includes(value)) {
                        chats.push(
                            <li id={`${element.Felhasznalonev}id`} onClick={() => { MessagesGen(element.Felhasznalonev) }}>
                                <div>
                                    <h2>{element.Felhasznalonev}</h2>
                                    <span>{element.UtolsoUzenet.substring(0,20)}{element.UtolsoUzenet.length>20 ? "..." : ""} {element.UtolsoUzenet == "" ? "" : new Date(element.Datum).toISOString().substring(5, 10)}</span>
                                </div>
                            </li>)
                        userek.push(element.Felhasznalonev)
                    }
                    setUsers(userek)
                })
                setChat(chats)
            })
    })
    
    document.body.addEventListener("load", () => {
        var lastuser = localStorage.getItem("utsobeszelgetes")
        if (window.location.href == "http://localhost:3000/Chat") {
            if (lastuser!=null) {
                try {
                    setTimeout(() => {
                        //document.getElementById(`${lastuser}id`).click()
                        MessagesGen(lastuser)
                    }, 100);
                } catch{
                    
                }
            }
        }
    }, true)
    return (
        <div id="container">
            <aside>
                <header>
                    <input type="text" placeholder="Keresés" onChange={(e) => { setValue(e.target.value) }} />
                </header>
                <ul id="chatek">
                    {chat}
                </ul>
            </aside>
            <div className="main">
                {user}
                <ul id="chat">
                    {uzenetek}
                </ul>
                {
                    !user ? ""
                        : (<footer>
                            <textarea id="uzenet" placeholder="Írd be az üzeneted"></textarea>
                            <a id="uzenetkuld" onClick={() => { SendMessage(element, document.getElementById("uzenet").value) }}>Küldés</a>
                        </footer>)
                }
            </div>
        </div>
    )
}

export default Chat