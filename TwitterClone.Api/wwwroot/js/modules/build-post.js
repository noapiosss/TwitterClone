import {GetLikes, BuildLikeCount} from "./likes-count.js";

export async function BuildPost(post)
{
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
    

    const likes = await GetLikes(post);
    const likeCount = await BuildLikeCount(post, likes);

    userPost.appendChild(postDate);
    userPost.appendChild(document.createElement('tr'));
    userPost.appendChild(postMessage);
    userPost.appendChild(document.createElement('tr'));
    userPost.appendChild(likeCount);

    return userPost;
}