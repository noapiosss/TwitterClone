export async function BuildHeader()
{
    const yourUsername = await CheckSession();

    await fetch("/header.html")
        .then(response => {return response.text()})
        .then(data => {document.getElementById("header-side-bar").innerHTML = data});

    const usernameHeader = document.getElementById("username-label");
    const makePostBtn = document.getElementById("make-post-button"); 
    const homeBtn = document.getElementById("home-button");
    const favBtn = document.getElementById("favorites-button");
    const signOutBtn = document.getElementById("sign-out-button");
    
    usernameHeader.innerHTML = yourUsername;

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

    signOutBtn.onclick = async () =>
    {
        await fetch(`${window.location.origin}/sign-out`);
        window.location.replace(`${window.location.origin}/sign-in`);
    }
}

async function CheckSession()
{
    const username = await fetch(`${window.location.origin}/api/users/username`)
        .then((response) => response.json())
        .then((result) => result.username);
    
    if (username === 'null')
    {
        window.location.replace(`${window.location.origin}/sign-in`)
    }

    return username;
}