import { React, useState } from "react"
import { Link } from 'react-router-dom';
import "./Login.css"
import { Authenticate } from "../assets/scripts/authenticate";

function Login() {
    function bejelentkezes() {
        
        //console.log(Authenticate(document.getElementById("email"), document.getElementById("passwd1")))
        localStorage.setItem("user",Authenticate(document.getElementById("email").value, document.getElementById("passwd1").value))
        //window.location.href="http://localhost:3000/Profil"
    }

    return (
        <div className="mainreg" id="main">
            <div id="belso" className="login">
                <div>
                    <span className="hatter">E-mail / Felhasználónév: </span>
                    <input type="text" name="" id="email" className="hatter" />
                </div>
                <div>
                    <span className="hatter">Jelszó: </span>
                    <input type="password" name="" id="passwd1" className="hatter" />
                </div>
                <div>
                    <input type="button" value="Bejelentkezés" id="login" className="hatter" onClick={bejelentkezes} />
                </div>
            </div>
            <div className="hatter">
                <span>Nincs fiókod? </span>
                <Link to="/Registration" id="nologin">Regisztrálok</Link>
            </div>
        </div>
    )
}

export default Login;