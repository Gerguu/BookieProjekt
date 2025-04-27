import { React, useState, useEffect } from "react"
import bookielogo from '../assets/bookielogo.png'
import './Menu.css'
import { Link } from 'react-router-dom';

function Menu() {
    const [isOpen, setIsOpen] = useState(false);
    const [user, SetUser] = useState(null)
    useEffect(() => {
        let storedUser = localStorage.getItem("user")
        if (storedUser) {
            SetUser(storedUser)
        }
    }, [])

    function gomble(e) {
        var notselectedelements = document.querySelectorAll(".selected")
        notselectedelements.forEach(element => {
            element.classList.remove("selected")
        })
        if (e != "vasarlas") {
            var selectedelement = e.target
            selectedelement.classList.add("selected")
        } else {
            document.getElementById("vasarlas").classList.add("selected")
            
        }
    }
    var loggedin = (
        <ul className={`header_List ${isOpen ? "open" : ""}`}>
            <li id="vasarlas" onClick={(e) => gomble(e)} ><Link to="/Vasarlas">Vásárlás</Link></li>
            <li id="sellselection" onClick={(e) => gomble(e)} ><Link to="/Eladas">Eladás</Link></li>
            <li id="chatselection" onClick={(e) => gomble(e)} ><Link to="/Chat">Chat</Link></li>
            <li onClick={(e) => gomble(e)} ><Link to="/Profil">Profilom</Link></li>
        </ul>
    )
    var loggedout = (
        <ul className={`header_List ${isOpen ? "open" : ""}`}>
            <li className="selected" onClick={(e) => gomble(e)} ><Link to="/Login">Bejelentkezés</Link></li>

        </ul>
    )
    return (
        <div className="header">
            <Link to="/Vasarlas"><img src={bookielogo} alt="log" className="logo" onClick={() => gomble("vasarlas")} /></Link>
            <button className="menu-toggle" onClick={() => setIsOpen(!isOpen)}>
                ☰
            </button>
            {
                !user ?
                    (
                        loggedout
                    ) : (loggedin)
            }
        </div>
    );
}
export default Menu;
