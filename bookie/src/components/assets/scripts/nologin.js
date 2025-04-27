import { deletemessages } from "./deletemessages"

export function nologin() {
    try {
        document.querySelector("#chatselection").classList.add("selected")
        document.querySelector("#sellselection").classList.add("selected")
    } catch (error) {
    }
    
    if (localStorage.getItem("user") == null) {
        window.location.href = "http://localhost:3000/Login"
    }
    var kezdo = localStorage.getItem("ujbeszelgetes")
    if (kezdo != null) {
        localStorage.removeItem("ujbeszelgetes")
    }
    if(window.location.href != "http://localhost:3000/Chat"){
        deletemessages()
        try {
            document.querySelector("#chatselection").classList.remove("selected")
        } catch (error) {
            
        }
    }
    if(window.location.href != "http://localhost:3000/NewBook"){
        try {
            document.querySelector("#sellselection").classList.remove("selected")
        } catch (error) {
            
        }
    }
    if(window.location.href == "http://localhost:3000/"){
        try {
            document.querySelector("#vasarlas").classList.add("selected")
        } catch (error) {
            
        }
    }
    if(window.location.href == "http://localhost:3000/Vasarlas"){
        try {
            document.querySelector("#vasarlas").classList.add("selected")
        } catch (error) {
            
        }
    }
}