export async function BuildHeader()
{
    await fetch("/header.html")
        .then(response => {return response.text()})
        .then(data => {document.getElementById("header-side-bar").innerHTML = data});

    const usernameHeader = document.getElementById("username-p"); 
    const homeBtn = document.getElementById("home-button");
    const favBtn = document.getElementById("favorites-button");
    const signOutBtn = document.getElementById("sign-out-button");

    const yourUsername = await fetch(`${window.location.origin}/api/users/username`)
        .then((response) => response.json());

    usernameHeader.innerHTML = yourUsername.username;

    homeBtn.onclick = () =>
    {
        window.location = `${window.location.origin}/home`
    }

    signOutBtn.onclick = () =>
    {

    }
}