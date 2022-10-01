const userPostsWrapper = document.querySelector("#user-posts-wrapper");
import {BuildHeader} from "./modules/header.js";

window.document.body.onload = async () => 
{
    BuildHeader();

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