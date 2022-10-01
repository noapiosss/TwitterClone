const userPostsWrapper = document.getElementById("comments-wrapper");
import {BuildHeader} from "./modules/header.js";
import {BuildComment} from "./modules/build-comment.js";

window.document.body.onload = async () => 
{
    BuildHeader();

    const allData = await fetch(`${window.location.origin}/api/users/homepage`)
        .then((response) => response.json());
    
    const data = allData.postsFromFollowings;
    
    for (let i = 0; i <data.length; i++)
    {
        const comment = await BuildComment(data[i]);
        userPostsWrapper.appendChild(comment);
    };
}