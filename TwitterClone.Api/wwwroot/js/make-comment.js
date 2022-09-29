const username = document.getElementById('username');
const message = document.getElementById('new-comment-message');
const sendBtn = document.getElementById('make-new-comment-button');

async function BuildPage(inputPostId)
{
    username.innerHTML = await fetch(`${window.location.origin}/api/users/username`)
        .then((response) => response.json())
        .then((result) => result.username);    
    
    sendBtn.onclick = async () =>
    {
        let comment = 
        {
            authorUsername: `${username}`, 
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


        window.close()
    };

    
}