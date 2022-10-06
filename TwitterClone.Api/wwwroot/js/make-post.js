const username = document.getElementById('username');
const message = document.getElementById('new-post-message');
const sendBtn = document.getElementById('make-new-post-button');

async function BuildPage()
{
    const usernameFromSession = await fetch(`${window.location.origin}/api/users/username`)
        .then((response) => response.text());
    
    username.innerHTML = usernameFromSession;

    sendBtn.onclick = async () =>
    {
        let post = 
        {
            authorUsername: `${usernameFromSession}`,
            message: `${message.value}`
        };
        
        await fetch(`${window.location.origin}/api/posts`,{
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(post)
        });

        window.close();
    };
    
}