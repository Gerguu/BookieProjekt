export function MarkSold(id) {
    fetch(`https://localhost:44317/api/EladoTermek/${id}`, {
        method: "PATCH",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            Aktiv: false,
            EladasDatum: new Date().toISOString().substring(0, 10)
        })
    })
}