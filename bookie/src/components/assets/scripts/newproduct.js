export async function SellProduct(path) {
    fetch(`https://localhost:44317/api/Konyv/${document.getElementById("konyvvv").value}`)
        .then((response) => {
            return response.json()
        })
        .then((data) => {
            fetch("https://localhost:44317/api/EladoTermek", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    Ar: document.getElementById("prizeinput").value,
                    Csere: document.getElementById("cserebox").checked/* ==true ? 1 : 0*/,
                    Aktiv: true,
                    FelrakasDatum: new Date().toISOString().substring(0, 10),
                    EladasDatum: null,
                    Felhasznalonev: JSON.parse(localStorage.getItem("user")).Felhasznalonev,
                    KonyvId: data[0].Id
                })
            })
                .then((response) => {
                    if (!response.ok) {
                        if (response.status === 401) {
                        }
                        else {
                            throw new Error(`HTTP hiba! Státuszkód: ${response.status}`)
                        }
                    }
                    else {
                        //alert("Sikeres feltöltés!")
                    }
                    return response.json()
                })
                .then(adat => {
                    fetch("https://localhost:44317/api/Kep",{
                        method: "POST",
                        headers: {
                            "Content-Type": "application/json"
                        },
                        body: JSON.stringify({
                            EleresiUt: path,
                            KepNev: document.getElementById("inputfile").files[0].name,
                            TermekId: adat.Id
                          })
                    })
                    .then(response=>{
                        if (!response.ok) {
                            if (response.status === 401) {
                            }
                            else {
                                throw new Error(`HTTP hiba! Státuszkód: ${response.status}`)
                            }
                        }
                        else {
                            alert("Sikeres feltöltés!")
                        }
                        //window.location.href = "http://localhost:3000/"
                    })
                })
                .catch((error) => {
                    console.error("Hiba történt:", error);
                    alert("Server hiba. Kérlek próbáld meg később!");
                })
        })


}