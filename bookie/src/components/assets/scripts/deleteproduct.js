export function DeleteProduct(id,href){
    fetch(`https://localhost:44317/api/EladoTermek/${id}`,{
        method: "DELETE",
        headers:{
            "Content-Type":"application/json"
        },
    })
    .then(response=>{
        if(response.ok){
            localStorage.removeItem(`termek${id}`)
            //window.location.href="http://localhost:3000/Profil"
            window.location.href= href=="Profil" ? "http://localhost:3000/Profil" : "http://localhost:3000/Vasarlas"
        }
         return response.json()})
}