export function register(){
    fetch("https://localhost:44317/api/Profil", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
                Felhasznalonev: document.getElementById("fnev").value,
                Jelszo: document.getElementById("passwd1").value,
                Email: document.getElementById("email").value,
                Telefonszam: document.getElementById("telefonszam").value,
                TeljesNev: document.getElementById("teljesnev").value,
                TelepulesNev: document.getElementById("telepules").value,
        })
    })
        .then((response) => {
            if (!response.ok) {
                if (response.status === 409) {
                    alert("Ez az e-mail cím/felhasználónév már regisztrálva van.")
                }
                if (response.status === 404) {
                    alert("Nem található ilyen település.")
                }
                else {

                    throw new Error(`HTTP hiba! Státuszkód: ${response.status}`)
                }
            }
            else {
                console.log(response.status)
                alert("Sikeres regisztráció!")
                window.location.href="http://localhost:3000/Login"
            }
            //return response.json()
        })
        .catch((error) => {
            console.error("Hiba történt:", error);
            alert("Server hiba. Kérlek próbáld meg később!");
        })
}