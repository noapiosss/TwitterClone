const username = document.getElementById("username");
const email = document.getElementById("email");
const password = document.getElementById("password");
const signUpBtn = document.getElementById("sign-up-button");



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
    .then((response) => response.json())
    .then((result) => {
        console.log(result.isRegistrationSuccessful);
        console.log(result.usernameIsAlreadyInUse);
        console.log(result.emailIsAlreadyInUse);
        if (!result.isRegistrationSuccessful)
        {
            if (result.usernameIsAlreadyInUse)
            {
                const usernameIsAlreadyTakenLabel = document.createElement('label');
                usernameIsAlreadyTakenLabel.innerText = "Username is already in use!";
                usernameIsAlreadyTakenLabel.style.color = "red";
                const usernameContainer = document.getElementById('username-container');
                usernameContainer.appendChild(usernameIsAlreadyTakenLabel);
            }
            if (result.emailIsAlreadyInUse)
            {
                const emailIsAlreadyTakenLabel = document.createElement('label');
                emailIsAlreadyTakenLabel.innerText = "Email is already in use!";
                emailIsAlreadyTakenLabel.style.color = "red";
                const usernameContainer = document.getElementById('email-container');
                usernameContainer.appendChild(emailIsAlreadyTakenLabel);
            }
        }
        else
        {
            window.location.replace(`${document.location.origin}/sign-in`);
        }
    })

    //username.value = "";
    //email.value = "";
    //password.value = "";
});