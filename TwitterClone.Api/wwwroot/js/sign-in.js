const signInBtn = document.getElementById('sign-in-button');
const usernameField = document.getElementById('username');
const passwordField = document.getElementById('password');
const userNotFoundLabel = document.getElementById('user-not-found-label');
const wrongPasswordLabel = document.getElementById('wrong-password-label');

document.body.onload = async () => 
{
    if (await fetch(`${window.location.origin}/api/users/username`)
        .then((response) => response.text()) !== "")
    {
        window.location.replace(`${document.location.origin}/home`);
    }
}

signInBtn.onclick = async (event) =>
{
    event.preventDefault();
    
    const username = usernameField.value;
    const password = passwordField.value;
    
    await fetch(`${window.location.origin}/sign-in`, {
        method: 'POST',
        body: JSON.stringify({
            username,
            password
        }),
        headers: {
            'content-type': 'application/json'
        }
    })
    .then((response) => {
        if (response.ok)
        {
            window.location.replace(`${document.location.origin}/home`);
        }
        return response.json();
    })
    .then((result) => {        
        if (result.message === "user not found")
        {            
            userNotFoundLabel.innerText = "User not found";
        }
        else
        {
            userNotFoundLabel.innerText = "";
        }

        if (result.message === "wrong password")
        {
            wrongPasswordLabel.innerText = "Wrong password";
        }
        else 
        {
            wrongPasswordLabel.innerText = "";
        }
    });
}