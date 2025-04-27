export async function NewBook() {
    fetch("https://localhost:44317/api/Kapcsolo/MindenPost", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            KonyvId: 0,
            Konyv: {
                Cim: document.getElementById("ujkonyvvv").value,
                KiadasEv: document.getElementById("kiadasev").value
            },
            SzerzoId: 0,
            Szerzo: {
              Nev: document.getElementById("newauthor").value
            }
          })
    })
        .then((response) => {
            if (!response.ok) {
                if (response.status === 401) {
                }
                if (response.status === 404) {
                }
                else {
                    throw new Error(`HTTP hiba! Státuszkód: ${response.status}`)
                }
            }
            else {
                alert("Sikeres feltöltés!")
            }
            return response.json()

        })
        .then(data=>{
            window.location.href="http://localhost:3000/Eladas"
        })
        .catch((error) => {
            console.error("Hiba történt:", error);
            alert("Server hiba. Kérlek próbáld meg később!");
        })
}