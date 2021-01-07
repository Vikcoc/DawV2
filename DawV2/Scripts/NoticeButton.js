function myFunction(val) {
    var id = "myDIV" + val;
    //console.log(id);
    var x = document.getElementById(id);
    if (x.style.display === "none") {
        x.style.display = "inline-block";
        x.style.zIndex = 2;
    } else {
        x.style.display = "none";
    }
}