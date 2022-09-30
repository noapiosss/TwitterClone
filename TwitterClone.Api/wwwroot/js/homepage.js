const userPostsWrapper = document.querySelector("#user-posts-wrapper");

async function BuildHeader()
{
    await fetch("/header.html")
        .then(response => {return response.text()})
        .then(data => {document.getElementById("header-side-bar").innerHTML = data});

    const usernameHeader = document.getElementById("username-p"); 
    const homeBtn = document.getElementById("home-button");
    const favBtn = document.getElementById("favorites-button");
    const signOutBtn = document.getElementById("sign-out-button");

    const yourUsername = await fetch(`${window.location.origin}/api/users/username`)
        .then((response) => response.json());

    usernameHeader.innerHTML = yourUsername.username;

    homeBtn.onclick = () =>
    {
        window.location = `${window.location.origin}/home`
    }

    signOutBtn.onclick = () =>
    {

    }
}

async function BuildPage()
{
    BuildHeader()

    const allData = await fetch(`${window.location.origin}/api/users/homepage`)
        .then((response) => response.json());
    
    const data = allData.postsFromFollowings;
    
    data.forEach(post => {

        const userPost = document.createElement('div');
        userPost.id = 'user-post';
        userPost.className = 'user-post';

        const postAuthorUsername = document.createElement('a');
        postAuthorUsername.id = 'post-author-username';
        postAuthorUsername.className = 'post-author-username';
        postAuthorUsername.innerText = post.authorUsername;
        postAuthorUsername.href = `${window.location.origin}/users/${post.authorUsername}`
        
        const postDate = document.createElement('a');
        postDate.id = 'post-date';
        postDate.className = 'post-date';
        postDate.innerText = post.postDate;
        postDate.href = `${window.location.origin}/posts/${post.postId}`;

        const postMessage = document.createElement('textarea');
        postMessage.id = 'post-message';
        postMessage.className = 'post-message';
        postMessage.value = post.message;
        postMessage.readOnly = true;
        postMessage.style.height = `${parseFloat(window.getComputedStyle(postMessage, null).getPropertyValue('font-size'))*postMessage.value.split('\n').length}px`;
        
        const likeCount = document.createElement('p')
        likeCount.id = 'like-count';
        likeCount.className = 'like-count';
        likeCount.innerText = `0 likes`;
        //likeCount.innerText = `${post.likedByUsername.length} likes`;

        userPost.appendChild(postAuthorUsername);
        userPost.appendChild(document.createElement('tr'));
        userPost.appendChild(postDate);
        userPost.appendChild(document.createElement('tr'));
        userPost.appendChild(postMessage);
        userPost.appendChild(document.createElement('tr'));
        userPost.appendChild(likeCount);
        
        userPostsWrapper.appendChild(userPost);
    });
}