export async function Authenticate(emailinput, passwordinput) {
    fetch("https://localhost:44317/api/Profil/authenticate", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            Email: emailinput,
            Jelszo: passwordinput
        })
    })
        .then((response) => {
            if (!response.ok) {
                if (response.status === 401) {
                    alert("Rossz jelszó")
                }
                if (response.status === 404) {
                    alert("Az email cím nem található")
                }
                else {
                    throw new Error(`HTTP hiba! Státuszkód: ${response.status}`)
                }
            }
            else {
                //console.log(response.body)
                alert("Sikeres bejelentkezés!")
            }
            return response.json()

        })
        .then(data=>{
            //console.log(data)
            localStorage.setItem("user",JSON.stringify(data))
            //localStorage.getItem("user")
            //setUser(data)
            window.location.href="http://localhost:3000/"
        })
        .catch((error) => {
            console.error("Hiba történt:", error);
            //console.log(error)
            alert("Server hiba. Kérlek próbáld meg később!");
        })
}