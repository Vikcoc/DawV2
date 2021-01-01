var zin = 0;
function myFunction(val) {
    var id = "myDIV" + val;
    //console.log(id);
    var x = document.getElementById(id);
    if (x.style.display === "none") {
        x.style.display = "inline-block";
        zin++;
        x.style.zIndex = zin;
    } else {
        x.style.display = "none";
        x.style.zIndex = 0;
        //zin--;
    }
}