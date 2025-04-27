import { useState } from "react";
import "./SearchResult.css";

export const SearchResult = ({ result }) => {
  function gomble() {
    var konyv=document.getElementById("konyvvv")
    konyv.value = result
    var resultsList=document.querySelectorAll(".results-list")[0]
    resultsList.innerHTML=""
    var json={
        Cim: result,
        Price: document.getElementById("prizeinput").value,
        Trade: document.getElementById("cserebox").checked,
        Pictures:document.getElementById("inputfile").files[0]
    }
    localStorage.setItem("eladotermek",JSON.stringify(json))
    window.location.href="http://localhost:3000/Eladas"
  }
  return (
    <div
      className="search-result"
      onClick={() => { gomble() }}
    >
      {result}
    </div>
  );
};
