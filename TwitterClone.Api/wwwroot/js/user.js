import {BuildHeader} from "./modules/header.js";
import {BuildPost} from "./modules/build-post.js";

const postsWrapper = document.getElementById("posts-wrapper");

window.document.body.onload = async () =>
{
    BuildHeader();

    const usernameInput = window.location.href.split('/').pop();
    const userPosts = await fetch(`${window.location.origin}/api/users/${usernameInput}/posts`)
        .then((response) => response.json())
        .then((result) => result.userPosts);   
    
    BuildBody(userPosts);
}

async function BuildBody(userPosts)
{
    const username = document.createElement('h1');
    username.id = 'username';
    username.className = 'username';
    username.innerText = userPosts[0].authorUsername;

    postsWrapper.appendChild(username);
    postsWrapper.appendChild(document.createElement('tr'));
    
    for (let i = 0; i <userPosts.length; i++)
    {
        const post = await BuildPost(userPosts[i]);
        postsWrapper.appendChild(post);
    };
}