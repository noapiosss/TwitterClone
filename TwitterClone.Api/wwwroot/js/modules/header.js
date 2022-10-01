export async function BuildHeader()
{
    await fetch("/header.html")
        .then(response => {return response.text()})
        .then(data => {document.getElementById("header-side-bar").innerHTML = data});

    const usernameHeader = document.getElementById("username-p");
    const makePostBtn = document.getElementById("make-post-button"); 
    const homeBtn = document.getElementById("home-button");
    const favBtn = document.getElementById("favorites-button");
    const signOutBtn = document.getElementById("sign-out-button");

    const yourUsername = await fetch(`${window.location.origin}/api/users/username`)
        .then((response) => response.json());

    usernameHeader.innerHTML = yourUsername.username;

    makePostBtn.onclick = () =>
    {
        const newWindowWidht = 500, newWindowHeight = 150;
        window.open(`${window.location.origin}/make-post`, 'targetWindow',
                                `toolbar=no,
                                location=no,
                                status=no,
                                menubar=no,
                                scrollbars=yes,
                                resizable=no,
                                width=${newWindowWidht}px,
                                height=${newWindowHeight}px,
                                left=${window.screen.width/2 - newWindowWidht/2},
                                top=${window.screen.height/2 - newWindowHeight/2}`);
    }
    
    homeBtn.onclick = () =>
    {
        window.location = `${window.location.origin}/home`
    }

    favBtn.onclick = () =>
    {
        window.location = `${window.location.origin}/favorites`
    }

    signOutBtn.onclick = () =>
    {

    }
}