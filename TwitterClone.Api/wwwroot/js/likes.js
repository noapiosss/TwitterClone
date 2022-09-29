
const userLikesWrapper = document.querySelector("#user-likes-wrapper");

async function BuildPage(inputPostId)
{
    const likes = await fetch(`${window.location.origin}/api/likes/${inputPostId}`)
        .then((response) => response.json());
    
    const followings = await fetch(`${window.location.origin}/api/followings`)
        .then((response) => response.json());

    const yourUsername = await fetch(`${window.location.origin}/api/users/username`)
        .then((response) => response.json());

    likes.usersThatLikePost.forEach(username => 
    {
        const userWrapper = document.createElement('div');
            userWrapper.id = 'user-wrapper';
            userWrapper.className = 'row user-wrapper';
        
        const userLinkWrapper = document.createElement('div');
            userLinkWrapper.id = 'user-link-wrapper';
            userLinkWrapper.className = 'col user-link-wrapper';

        const user = document.createElement('a');
            user.id = 'user-link';
            user.className = 'user-link';
            user.innerText = username;
            user.href = `${window.location.origin}/users/${username}`;

        const buttonWrapper = document.createElement('div');
            buttonWrapper.id = 'button-wrapper';
            buttonWrapper.className = 'col button-wrapper';

        const followButton = document.createElement('button');
        
        if (username === yourUsername.username)
        {
            followButton.id = 'you-button';
            followButton.className = 'you-button';
            followButton.innerHTML = 'You';
        }
        else 
        {
            if(!followings.followings.includes(username))
            {
                followButton.id = 'follow-button';
                followButton.className = 'follow-button';
                followButton.innerHTML = 'Follow';
            }
            else
            {
                followButton.id = 'unfollow-button';
                followButton.className = 'unfollow-button';
                followButton.innerHTML = 'Following';            
            }
        }

        followButton.addEventListener('mouseover', () => {
            if(followButton.id==='unfollow-button')
            {
                followButton.innerHTML='Unfollow'
            }
        });

        followButton.addEventListener('mouseout', () => {
            if(followButton.id==='unfollow-button')
            {
                followButton.innerHTML='Following'
            }
        });

        followButton.onclick = async () =>
        {
            if (username === yourUsername.username)
            {
                return;
            }

            if (followButton.id === 'follow-button')
            {
                followButton.id = 'unfollow-button';
                followButton.className = 'unfollow-button';
                followButton.innerHTML = 'Following';
            }
            else
            {
                followButton.id = 'follow-button';
                followButton.className = 'follow-button';
                followButton.innerHTML = 'Follow';                
            }

            await fetch(`${window.location.origin}/api/follow`, 
            {
                method: 'PATCH',
                headers:
                {
                    'content-type': 'application/json'
                },
                body: JSON.stringify({
                    followByUsername: '',
                    followForUsername: `${username}`
                })
            })
        }

        const newRow = document.createElement('div');
        newRow.className = 'w-100 d-none d-md-block';

        userLinkWrapper.appendChild(user);
        buttonWrapper.appendChild(followButton);
        userWrapper.appendChild(userLinkWrapper);
        userWrapper.appendChild(buttonWrapper);
        userLikesWrapper.appendChild(userWrapper);
    });

}

function AutoGrow(element) {
    element.style.height = "40px";
    element.style.height = (element.scrollHeight)+"px";
}