const userPostsWrapper = document.querySelector("#user-posts-wrapper");

async function BuildPage(usernameInput)
{
    const data = await fetch(`${window.location.origin}/api/users/${usernameInput}/posts`)
        .then((response) => response.json());   
    
    BuildBody(data);    
}

function BuildBody(data)
{
    const username = document.createElement('h1');
    username.id = 'username';
    username.className = 'username';
    username.innerText = data.userPosts[0].authorUsername;
    userPostsWrapper.appendChild(username);
    userPostsWrapper.appendChild(document.createElement('tr'));

    data.userPosts.forEach(async post => {

        const userPost = document.createElement('div');
        userPost.id = 'user-post';
        userPost.className = 'user-post';
        
        const postDate = document.createElement('a');
        postDate.id = 'post-date';
        postDate.className = 'post-date';
        postDate.innerText = (new Date(Date.parse(post.postDate))).toUTCString();;
        postDate.href = `${window.location.origin}/posts/${post.postId}`;

        const postMessage = document.createElement('textarea');
        postMessage.id = 'post-message';
        postMessage.className = 'post-message';
        postMessage.value = post.message;
        postMessage.readOnly = true;
        postMessage.style.height = `${parseFloat(window.getComputedStyle(postMessage, null).getPropertyValue('font-size'))*postMessage.value.split('\n').length}px`;
        

        const likes = await fetch(`${window.location.origin}/api/likes/${post.postId}`)
            .then((response) => response.json());

        const likeCount = document.createElement('p')
        likeCount.id = 'like-count';
        likeCount.className = 'like-count';
        likeCount.innerHTML = `<b>${likes.usersThatLikePost.length}</b> likes`;
        likeCount.style.textDecoration = 'none';
        likeCount.style.color = 'black';

        userPost.appendChild(postDate);
        userPost.appendChild(document.createElement('tr'));
        userPost.appendChild(postMessage);
        userPost.appendChild(document.createElement('tr'));
        userPost.appendChild(likeCount);
        
        userPostsWrapper.appendChild(userPost);

        likeCount.addEventListener('mouseout', () =>
        {
            likeCount.style.textDecoration = 'none';
        })

        likeCount.addEventListener('mouseover', () =>
        {
            likeCount.style.textDecoration = 'underline';
        });

        likeCount.addEventListener('click', () =>
        {
            const newWindowWidht = 500, newWindowHeight = 800;
            window.open(`${window.location.origin}/posts/${post.postId}/likes`, 'targetWindow',
                                    `toolbar=no,
                                    location=no,
                                    status=no,
                                    menubar=no,
                                    scrollbars=yes,
                                    resizable=no,
                                    width=${newWindowWidht}px,
                                    height=${newWindowHeight}px,
                                    left=${window.screen.width/2 - newWindowWidht/2},
                                    top=${window.screen.height/2 - newWindowHeight/2}`);
            
        });
    });
}