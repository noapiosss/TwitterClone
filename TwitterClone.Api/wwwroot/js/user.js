const userPostsWrapper = document.querySelector("#user-posts-wrapper");

async function BuildPage(usernameInput)
{
    //const data = JSON.parse(input)
    //console.log(data)

    let data;
    await fetch(` http://localhost:5134/api/users/${usernameInput}/posts`)
        .then((response) => response.json())
        .then((inputData) => 
        {
            data = inputData;
        });
    
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

    data.userPosts.forEach(post => {

        const userPost = document.createElement('div');
        userPost.id = 'user-post';
        userPost.className = 'user-post';
        
        const postDate = document.createElement('a');
        postDate.id = 'post-date';
        postDate.className = 'post-date';
        postDate.innerText = (new Date(Date.parse(post.postDate))).toUTCString();;
        postDate.href = `http://localhost:3000/posts/${post.postId}`;

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

        userPost.appendChild(postDate);
        userPost.appendChild(document.createElement('tr'));
        userPost.appendChild(postMessage);
        userPost.appendChild(document.createElement('tr'));
        userPost.appendChild(likeCount);
        
        userPostsWrapper.appendChild(userPost);
    });
}