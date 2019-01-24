function OnDeleteButtonPressed(Name,Time) {
//function OnDeleteButtonPressed() {

    var urlBase = "http://" + window.location.hostname + ( window.location.port == "" ? "" : ( ":" + window.location.port )) + "/messages/delete?";
    var xhr = new XMLHttpRequest();

    var urlFull = urlBase + "toName=" + Name + "&dateTime="+Time ;
    xhr.open("DELETE", encodeURI(urlFull), true);  
    xhr.setRequestHeader("RequestVerificationToken", document.getElementById('RequestVerificationToken').value)

    xhr.onload = function () {window.location.hostname
        if (xhr.readyState == 4 && xhr.status == "200") {
            alert("Message has been deleted");
            location.reload();
        } else {
            alert("Message delete has failed");
        }
    }

    xhr.send();
}
