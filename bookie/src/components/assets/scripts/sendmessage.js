export function SendMessage(element, uzenet, kezdo) {
    if (uzenet != "" || kezdo) {
        fetch("https://localhost:44317/api/Cseveges", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                Uzenet: uzenet,
                Datum: new Date().toISOString().substring(0, 10),
                KuldoFelhasznalonev: JSON.parse(localStorage.getItem("user")).Felhasznalonev,
                KapoFelhasznalonev: element
            })
        })
            .then((response) => {
                if (!response.ok) {
                }
                else {
                    window.location.href = "http://localhost:3000/Chat"
                }
                return response.json()

            })
            .then(data => {
            })
            .catch((error) => {
                console.error("Hiba történt:", error);
                //alert("Server hiba. Kérlek próbáld meg később!");
            })
    }
}