const signInBtn = document.getElementById('sign-in-button');
const usernameField = document.getElementById('username');
const passwordField = document.getElementById('password');
signInBtn.onclick = async (event) =>
{
    event.preventDefault();
    
    const username = usernameField.value;
    const password = passwordField.value;

    const response = await fetch(`${window.location.origin}/sign-in`, {
        method: 'POST',
        body: JSON.stringify({
            username,
            password
        }),
        headers: {
            'content-type': 'application/json'
        }
    });

    if (!response.ok)
    {
        throw new Error(`status code is ${response.status}`);
    }

    window.location.replace(`${document.location.origin}/home`);
}