export function validation(element, elementvalue,index) {
    var uzenet=[]    
    var email_regex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/
    if (element == "email") {
        if (elementvalue === "" || elementvalue === null) {
            uzenet.push("Nem adtál meg e-mail címet!")
        }
        if (!elementvalue.match(email_regex)) {
            uzenet.push("Nem megfelelő az e-mail cím formátuma!")
        }
    }
    if(element=="fnev"){
        if (elementvalue === "" || elementvalue === null) {
            uzenet.push("Nem adtál meg nevet!")
        }
    }
    
    if(element=="telefonszam"){
        if(!/^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/gm.test(elementvalue)){
            uzenet.push("Rossz telefonszám")
        }
    }
    if(element=="teljesnev"){
        if (elementvalue === "" || elementvalue === null) {
            uzenet.push("Nem hagyhatod üresen!")
        }
    }
    if(element=="telepules"){
        if (elementvalue === "" || elementvalue === null) {
            uzenet.push("Nem hagyhatod üresen!")
        }
    }
    if(element=="passwd"){
        if (elementvalue.length < 8) {
            uzenet.push("A jelszó túl rövid!")
        }
        if (!/[A-Z]/.test(elementvalue)) {
            uzenet.push("A jelszóban nem szerepel nagybetű!")
        }
        if (!/[0-9]/.test(elementvalue)) {
            uzenet.push("A jelszóban nem szerepel szám!")
        }
        if(!(/[*!$,%?;+@#<>\-_=\/:\\]/.test(elementvalue))){
            uzenet.push("A jelszóban nem szerepel különleges karakter!")
        }
    }
    if(element=="repeatpasswd"){
        if (document.getElementById("passwd1").value !== elementvalue) {
            uzenet.push("A két jelszó nem egyezik meg!")
        }
    }
    var errorspan=""
    var div=document.querySelectorAll(".kiir")[index]
    div.innerHTML=""
    if(elementvalue!=""){
        div.classList.remove("hidden")
        uzenet.forEach(message=>{
            //errorspan=(<span className="errorspan">{message}</span>)
            errorspan=document.createElement("span")
            errorspan.innerHTML=message
            errorspan.classList.add("errorspan")
            div.appendChild(errorspan)
        })
    }

    if(errorspan==""){
        div.classList.add("hidden")
        div.innerHTML=""
    }
    else{
        div.classList.remove("hidden")

    }
}
