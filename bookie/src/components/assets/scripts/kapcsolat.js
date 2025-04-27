export function Kapcsolat(uzenet) {
    var user = JSON.parse(localStorage.getItem("user"))
    if(uzenet!=""){
        fetch("https://discord.com/api/webhooks/1359125959244120074/4NkGVfrDlMSMC_HbynGpd7cnZ8VcMlLGpsRT1PgCUvMgImzAC_r1tPO2Qg_mnJAHvbqG", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body:
                JSON.stringify({
                    content: `
                    ## **${"`"}Felhasznalonev${"`"}** \n - ${user.Felhasznalonev} 
                    \n## **${"`"}TeljesNev${"`"}** \n - ${user.TeljesNev} 
                    \n## **${"`"}Email${"`"}** \n - ${user.Email} 
                    \n## **${"`"}Uzenet${"`"}** \n - ${uzenet} `
                })
        })
        var textarea = document.getElementById("getintouchTextarea")
        textarea.value=""
        alert("Sikeres kapcsolatfelvétel! Üzenetedre emailben fogunk válaszolni.")
    }
}