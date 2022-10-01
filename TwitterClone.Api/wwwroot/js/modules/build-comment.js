import {BuildLikeIconForMainPost, BuildLikeIconForComment, BuildCommentIcon} from "./build-icons.js";
import {GetLikes, BuildLikeCount} from "./likes-count.js";

export async function BuildComment(oldComment)
{
    const comment = document.createElement('div');
        comment.id = 'comment';
        comment.className = 'comment';

        
        const commentAuthorUsername = document.createElement('a');
        commentAuthorUsername.id = 'comment-author-username';
        commentAuthorUsername.className = 'comment-author-username';
        commentAuthorUsername.innerText = oldComment.authorUsername;
        commentAuthorUsername.href = `${window.location.origin}/users/${oldComment.authorUsername}`

        const commentPostDate = document.createElement('a');
        commentPostDate.id = 'comment-date';
        commentPostDate.className = 'comment-date';
        commentPostDate.innerText = (new Date(Date.parse(oldComment.postDate))).toUTCString();;
        commentPostDate.href = `${window.location.origin}/posts/${oldComment.postId}`;

        const commetTextArea = document.createElement('textarea');
        commetTextArea.id = 'comment-message';
        commetTextArea.className = 'comment-message';
        commetTextArea.value = oldComment.message;
        commetTextArea.readOnly = true;
        commetTextArea.style.height = `${parseFloat(window.getComputedStyle(commetTextArea, null).getPropertyValue('font-size'))*commetTextArea.value.split('\n').length}px`;;
       
        const commentSeparator = document.createElement('div')
        commentSeparator.id = 'comment-separator';
        commentSeparator.className = 'comment-separator';

        const commentIconWrapper = await BuildCommentIcon(oldComment.postId);

        const likes = await fetch(`${window.location.origin}/api/likes/${oldComment.postId}`)
            .then((response) => response.json());
        const likeIconWrapper = await BuildLikeIconForComment(oldComment, likes.usersThatLikePost);

        commentSeparator.appendChild(commentIconWrapper);
        commentSeparator.appendChild(likeIconWrapper);
        
        comment.appendChild(commentAuthorUsername);
        comment.appendChild(document.createElement('tr'));
        comment.appendChild(commentPostDate);
        comment.appendChild(document.createElement('tr'));
        comment.appendChild(commetTextArea);
        comment.appendChild(document.createElement('tr'));
        comment.appendChild(commentSeparator);

        
        
        return comment;
}