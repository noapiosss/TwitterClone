const postsWrapper = document.getElementById("posts-wrapper");
import {BuildHeader} from "./modules/header.js";
import {BuildPost} from "./modules/build-post.js";

window.document.body.onload = async () => 
{
    BuildHeader();

    const postsFromFollowings = await fetch(`${window.location.origin}/api/users/homepage`)
        .then((response) => response.json())
        .then((result) => result.homepagePosts);
    
    for (let i = 0; i < postsFromFollowings.length; i++)
    {
        const post = await BuildPost(postsFromFollowings[i]);
        postsWrapper.appendChild(post);
    };
}