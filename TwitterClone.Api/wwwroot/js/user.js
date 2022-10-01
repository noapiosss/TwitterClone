const userPostsWrapper = document.querySelector("#user-posts-wrapper");
import {BuildHeader} from "./modules/header.js";
import {BuildPost} from "./modules/build-post.js";

window.document.body.onload = async () =>
{
    BuildHeader();

    const usernameInput = window.location.href.split('/').pop();
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

    data.userPosts.forEach(async post => 
    {
        const userPost = await BuildPost(post);        
        userPostsWrapper.appendChild(userPost);        
    });
}