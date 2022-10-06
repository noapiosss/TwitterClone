const username = document.getElementById('username');
const message = document.getElementById('new-comment-message');
const sendBtn = document.getElementById('make-new-comment-button');

async function BuildPage(inputPostId)
{
    const usernameFromSession = await fetch(`${window.location.origin}/api/users/username`)
        .then((response) => response.text());
    
    username.innerHTML = usernameFromSession;
    
    sendBtn.onclick = async () =>
    {
        let comment = 
        {
            authorUsername: `${usernameFromSession}`, 
            commentTo: `${inputPostId}`, 
            message: `${message.value}`
        };

        await fetch(`${window.location.origin}/api/posts`,{
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(comment)
        });
        
        window.close();
    };
    
}