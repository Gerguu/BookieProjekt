import { React, useState } from "react"
import { Link } from 'react-router-dom';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import "./Registration.css"
import { validation } from "../assets/scripts/validation";
import { register } from "../assets/scripts/register";

function Registration() {
    function emailChanged(){
        validation("email",document.getElementById("email").value,0)
    }
    function usernameChanged(){
        validation("fnev",document.getElementById("fnev").value,1)
    }
    function phonenumberChanged(){
        validation("telefonszam",document.getElementById("telefonszam").value,2)

    }
    function fullNameChanged(){
        validation("teljesnev",document.getElementById("teljesnev").value,3)

    }
    function cityChanged(){
        validation("telepules",document.getElementById("telepules").value,4)

    }
    function passwordChanged(){
        validation("passwd",document.getElementById("passwd1").value,5)
        validation("repeatpasswd",document.getElementById("passwd2").value,6)
    }
    function passwordSameChanged(){
        validation("repeatpasswd",document.getElementById("passwd2").value,6)

    }
    function buttonPressed(){
        if(document.querySelectorAll(".hidden").length==7){
            register()
        }
        else{
            alert("Tölts ki minden mezőt!")
        }
    }

    return (
        <div className="mainreg" id="main">
            <div id="belso">
                <div>
                    <span className="hatter">E-mail: </span>
                    <input type="email" name="" id="email" className="hatter" onChange={emailChanged} />
                </div>
                <div className="kiir hidden"></div>
                <div>
                    <span className="hatter">Felhasználónév: </span>
                    <input type="text" name="" id="fnev" className="hatter" onChange={usernameChanged} />
                </div>
                <div className="kiir hidden"></div>
                <div>
                    <span className="hatter">Telefonszám: </span>
                    <input type="text" name="" id="telefonszam" className="hatter" onChange={phonenumberChanged} />
                </div>
                <div className="kiir hidden"></div>
                <div>
                    <span className="hatter">Teljes Név: </span>
                    <input type="text" name="" id="teljesnev" className="hatter" onChange={fullNameChanged} />
                </div>
                <div className="kiir hidden"></div>
                <div>
                    <span className="hatter">Település: </span>
                    <input type="text" name="" id="telepules" className="hatter" onChange={cityChanged} />
                </div>
                <div className="kiir hidden"></div>
                <div>
                    <span className="hatter">Jelszó: </span>
                    <input type="password" name="" id="passwd1" className="hatter" onChange={passwordChanged} />
                </div>
                <div id="passwdiv" className="kiir hidden"></div>
                <div>
                    <span className="hatter">Jelszó újra: </span>
                    <input type="password" name="" id="passwd2" className="hatter" onChange={passwordSameChanged} />
                </div>
                <div className="kiir hidden"></div>
                <div>
                    <input type="button" value="Regisztrálás" id="submit" className="hatter" onClick={buttonPressed} />
                </div>
            </div>
        </div>
    )
}

export default Registration;