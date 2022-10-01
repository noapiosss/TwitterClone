const postWrapper = document.getElementById("post-wrapper");
const separator = document.getElementById("separator");
const comments = document.getElementById("comments-wrapper");
import {BuildHeader} from "./modules/header.js";
import {BuildPost} from "./modules/build-post.js";
import {BuildComment} from "./modules/build-comment.js";
import {BuildLikeIconForMainPost, BuildLikeIconForComment, BuildCommentIcon} from "./modules/build-icons.js";

window.document.body.onload = async () =>
{
    BuildHeader();

    const inputPostId = window.location.href.split('/').pop();
    const postData = await fetch(`${window.location.origin}/api/posts/${inputPostId}`)
        .then((response) => response.json()); 

    const post = postData.post;
    const allComments = postData.comments;
    const likes = postData.likedByUsername;
    
    const basePost = await BuildBasePost(post, likes);    
    postWrapper.appendChild(basePost);
    
    for (let i = 0; i <allComments.length; i++)
    {
        const comment = await BuildComment(allComments[i]);
        comments.appendChild(comment);
    };
}

async function BuildBasePost(post, likes)
{
    const basePost = await BuildPost(post);    
    const commentIconWrapper = await BuildCommentIcon(post.postId);
    const likeIconWrapper = await BuildLikeIconForMainPost(post, likes);

    separator.appendChild(commentIconWrapper)
    separator.appendChild(likeIconWrapper);

    postWrapper.appendChild(basePost);   

    return basePost;
}