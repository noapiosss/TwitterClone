
const userLikesWrapper = document.querySelector("#user-likes-wrapper");

function BuildPage(likedByUsername)
{
    const likes = JSON.parse(likedByUsername);

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
            user.href = `http://localhost:3000/users/${username}`;

        const buttonWrapper = document.createElement('div');
            buttonWrapper.id = 'button-wrapper';
            buttonWrapper.className = 'col button-wrapper';

        const followButton = document.createElement('button');
            followButton.id = 'follow-button';
            followButton.className = 'follow-button';
            followButton.innerHTML = 'Follow';

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