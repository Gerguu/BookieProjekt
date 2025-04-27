export function deletemessages() {
    function deletemessage(id) {
        fetch(`https://localhost:44317/api/Cseveges/delete?id=${id}`,{
            method: "DELETE",
            headers:{
                "Content-Type":"application/json"
            },
        })
    }
    fetch("https://localhost:44317/api/Cseveges")
        .then(response => {
            return response.json()
        })
        .then(data => {
            
            data.forEach(element => {
                var db=0
                data.forEach(uzenet => {
                    if (uzenet.Uzenet == "" && (uzenet.KapoFelhasznalonev == element.Felhasznalonev || uzenet.KuldoFelhasznalonev == element.Felhasznalonev)) {
                        db++
                    }
                });
                if (element.Uzenet == "") {
                    deletemessage(element.Id)
                }
            });
        })
}