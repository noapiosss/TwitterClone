const username = document.getElementById("username");
const email = document.getElementById("email");
const password = document.getElementById("password");
const signUpBtn = document.getElementById("sign-up-button");
const usernameIsAlreadyTakenLabel = document.getElementById("username-is-already-taken-label");
const emailIsAlreadyTakenLabel = document.getElementById("email-is-already-taken-label");

document.body.onload = async () => 
{
    if (await fetch(`${window.location.origin}/api/users/username`)
        .then((response) => response.text()) !== "")
    {
        window.location.replace(`${document.location.origin}/home`);
    }
}

signUpBtn.addEventListener("click", () => 
{
    let newUser = {
        username: `${username.value}`, 
        email: `${email.value}`, 
        password: `${password.value}`
    };

    fetch(`${window.location.origin}/api/users`,{
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(newUser)
    })
    .then((response) => {
        if (response.ok)
        {
            window.location.replace(`${document.location.origin}/home`);
        }
        return response.json();
    })
    .then((result) => {
        if (result.message === "username is already in use")
        {
            usernameIsAlreadyTakenLabel.innerText = "Username is already in use!";
        }
        else
        {
            usernameIsAlreadyTakenLabel.innerText = "";
        }

        if (result.message === "email is already in use")
        {
            emailIsAlreadyTakenLabel.innerText = "Email is already in use!";
        }
        else
        {
            emailIsAlreadyTakenLabel.innerText = "";
        }
    })
});