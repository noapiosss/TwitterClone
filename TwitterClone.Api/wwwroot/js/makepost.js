const username = document.getElementById("username");
const message = document.getElementById("message");
const sendBtn = document.getElementById("send-button");

sendBtn.addEventListener("click", () =>
{
    fetch("http://localhost:5134/api/posts",{
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          username: `${username.value}`, 
          message: `${message.value}`
        })
    }).then((response) => {
        console.log(response.json())
    })

    message.value = "";
});