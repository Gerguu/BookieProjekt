export async function Patch(oldpasswd) {
    var user = JSON.parse(localStorage.getItem("user"))
    console.log(user)
    var newpasswd = document.getElementById("passwordupdate")
    var telefon = document.getElementById("telefonupdate")
    var fullname = document.getElementById("fullnameupdate")
    var telepules = document.getElementById("telepulesupdate")
    fetch(`https://localhost:44317/api/Profil/${user.Felhasznalonev}`, {
        method: "PATCH",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            RegiJelszo: oldpasswd,
            UjJelszo: newpasswd == null ? null : newpasswd.value,
            Telefonszam: telefon == null ? null : telefon.value,
            TeljesNev: fullname == null ? null : fullname.value,
            TelepulesNev: telepules == null ? null : telepules.value
        })
    })
        .then((response) => {
            if (!response.ok) {
                if (response.status === 401) {
                    alert("Rossz jelszó")
                }
                if (response.status === 409) {
                    alert("Az új jelszavad nem lehet a régi jelszavad")
                }
                if (response.status === 404) {
                    alert("Nem találtunk ilyen települést")
                }   
                else {

                    throw new Error(`HTTP hiba! Státuszkód: ${response.status}`)
                }
            }
            else {
                alert("Sikeres módosítás!")
                return response.json()
            }
            
        })
        .then((data)=>{
            if(data!=null){
                localStorage.setItem("user",JSON.stringify(data))
            }
            window.location.href="http://localhost:3000/Profil"
        })
        .catch((error) => {
            console.error("Hiba történt:", error);
            alert("Server hiba. Kérlek próbáld meg később!");
        })
}