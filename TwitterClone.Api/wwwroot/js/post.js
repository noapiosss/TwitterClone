import {BuildHeader} from "./modules/header.js";
import {BuildMainPost, BuildPost} from "./modules/build-post.js";

const pageWrapper = document.getElementById("page-wrapper");

window.document.body.onload = async () =>
{
    BuildHeader();

    const mainPostId = window.location.href.split('/').pop();
    const postData = await fetch(`${window.location.origin}/api/posts/${mainPostId}`)
        .then((response) => response.json()); 

    const post = postData.post;
    const comments = postData.comments;
    
    const mainPostWrapper = await BuildMainPost(post);
    pageWrapper.append(mainPostWrapper);
    
    for (let i = 0; i <comments.length; i++)
    {
        const postWrapper = await BuildPost(comments[i]);
        pageWrapper.appendChild(postWrapper);
    };
}