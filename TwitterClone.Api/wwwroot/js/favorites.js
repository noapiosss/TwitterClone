const postsWrapper = document.getElementById("posts-wrapper");
import {BuildHeader} from "./modules/header.js";
import {BuildPost} from "./modules/build-post.js";

window.document.body.onload = async () => 
{
    BuildHeader();

    const favoritesPosts = await fetch(`${window.location.origin}/api/users/favorites`)
        .then((response) => response.json())
        .then((result) => result.favoritesPosts);
    
    for (let i = 0; i <favoritesPosts.length; i++)
    {
        const favoritesPost = await BuildPost(favoritesPosts[i]);
        postsWrapper.appendChild(favoritesPost);
    };
}