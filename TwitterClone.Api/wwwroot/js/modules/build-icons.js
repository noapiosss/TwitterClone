export async function BuildLikeIconForMainPost(post, likes)
{
    const commentLikeCount = document.createElement('a')
    commentLikeCount.id = 'like-count';
    commentLikeCount.className = 'like-count';
    commentLikeCount.innerHTML = `<b>${likes.length}</b> likes`;
    commentLikeCount.style.textDecoration = 'none';
    commentLikeCount.style.color = 'black';

    const likesData = await fetch(`${window.location.origin}/api/likes`, 
        {
            method: 'GET',
            headers:
            {
                'content-type': 'application/json'
            }
        })
        .then((response) => response.json());
    const userLikes = likesData.postIdsThatUserLike;

    const likeIconWrapper = document.createElement('div');
    likeIconWrapper.className = 'like-icon-wrapper';
    const likeIcon = document.createElement('a');    
    likeIcon.id = 'like-icon';
    likeIcon.className = 'like-icon';    

    if (userLikes.includes(post.postId))
    {
        likeIcon.innerHTML = '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 358.299 358.299" style="enable-background:new 0 0 358.299 358.299;"><path id="XMLID_169_" d="M 251.787 27.034 C 224.391 27.034 198.721 37.281 179.149 55.615 C 159.577 37.281 133.907 27.034 106.511 27.034 C 47.781 27.034 0 74.816 0 133.547 C 0 162.476 9.521 190.956 28.298 218.194 C 42.816 239.254 62.901 259.611 87.993 278.698 C 130.085 310.718 171.642 328.162 173.391 328.889 L 179.106 331.265 L 184.834 328.92 C 186.584 328.204 228.168 311.007 270.282 279.103 C 295.389 260.083 315.484 239.733 330.009 218.617 C 348.781 191.327 358.299 162.706 358.299 133.548 C 358.299 74.816 310.518 27.034 251.787 27.034 Z" style="paint-order: fill; fill-rule: nonzero; fill: rgb(252, 0, 0);"/></svg>';
    }
    else
    {
        likeIcon.innerHTML = '<svg viewBox="0 0 24 24" aria-hidden="true" class="r-4qtqp9 r-yyyyoo r-1xvli5t r-dnmrzs r-bnwqim r-1plcrui r-lrvibr r-1hdv0qi"><g><path d="M12 21.638h-.014C9.403 21.59 1.95 14.856 1.95 8.478c0-3.064 2.525-5.754 5.403-5.754 2.29 0 3.83 1.58 4.646 2.73.814-1.148 2.354-2.73 4.645-2.73 2.88 0 5.404 2.69 5.404 5.755 0 6.376-7.454 13.11-10.037 13.157H12zM7.354 4.225c-2.08 0-3.903 1.988-3.903 4.255 0 5.74 7.034 11.596 8.55 11.658 1.518-.062 8.55-5.917 8.55-11.658 0-2.267-1.823-4.255-3.903-4.255-2.528 0-3.94 2.936-3.952 2.965-.23.562-1.156.562-1.387 0-.014-.03-1.425-2.965-3.954-2.965z"></path></g></svg>';
        likeIcon.onmouseover = () => {likeIcon.style.filter = 'invert(27%) sepia(51%) saturate(2878%) hue-rotate(346deg) brightness(104%) contrast(97%)'};
        likeIcon.onmouseout = () => {likeIcon.style.filter = ''};
    }

    likeIconWrapper.onclick = async () =>
    {
        await fetch(`${window.location.origin}/api/likes`, 
            {
                method: 'PATCH',
                headers:
                {
                    'content-type': 'application/json'
                },
                body: JSON.stringify({
                    likedPostId: `${post.postId}`,
                    likedByUsername: ''
                })
            });

        const newLikesData = await fetch(`${window.location.origin}/api/likes`, 
            {
                method: 'GET',
                headers:
                {
                    'content-type': 'application/json'
                }
            })
            .then((response) => response.json());

        const newUserLikes = newLikesData.postIdsThatUserLike;

        if (newUserLikes.includes(post.postId))
        {
            likeIcon.innerHTML = '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 358.299 358.299" style="enable-background:new 0 0 358.299 358.299;"><path id="XMLID_169_" d="M 251.787 27.034 C 224.391 27.034 198.721 37.281 179.149 55.615 C 159.577 37.281 133.907 27.034 106.511 27.034 C 47.781 27.034 0 74.816 0 133.547 C 0 162.476 9.521 190.956 28.298 218.194 C 42.816 239.254 62.901 259.611 87.993 278.698 C 130.085 310.718 171.642 328.162 173.391 328.889 L 179.106 331.265 L 184.834 328.92 C 186.584 328.204 228.168 311.007 270.282 279.103 C 295.389 260.083 315.484 239.733 330.009 218.617 C 348.781 191.327 358.299 162.706 358.299 133.548 C 358.299 74.816 310.518 27.034 251.787 27.034 Z" style="paint-order: fill; fill-rule: nonzero; fill: rgb(252, 0, 0);"/></svg>';
        }
        else
        {
            likeIcon.innerHTML = '<svg viewBox="0 0 24 24" aria-hidden="true" class="r-4qtqp9 r-yyyyoo r-1xvli5t r-dnmrzs r-bnwqim r-1plcrui r-lrvibr r-1hdv0qi"><g><path d="M12 21.638h-.014C9.403 21.59 1.95 14.856 1.95 8.478c0-3.064 2.525-5.754 5.403-5.754 2.29 0 3.83 1.58 4.646 2.73.814-1.148 2.354-2.73 4.645-2.73 2.88 0 5.404 2.69 5.404 5.755 0 6.376-7.454 13.11-10.037 13.157H12zM7.354 4.225c-2.08 0-3.903 1.988-3.903 4.255 0 5.74 7.034 11.596 8.55 11.658 1.518-.062 8.55-5.917 8.55-11.658 0-2.267-1.823-4.255-3.903-4.255-2.528 0-3.94 2.936-3.952 2.965-.23.562-1.156.562-1.387 0-.014-.03-1.425-2.965-3.954-2.965z"></path></g></svg>';
            likeIcon.onmouseover = () => {likeIcon.style.filter = 'invert(27%) sepia(51%) saturate(2878%) hue-rotate(346deg) brightness(104%) contrast(97%)'};
            likeIcon.onmouseout = () => {likeIcon.style.filter = ''};
        }

        const newLikesCount = await fetch(`${window.location.origin}/api/likes/${post.postId}`, 
        {
            method: 'GET',
            headers:
            {
                'content-type': 'application/json'
            }
        })
        .then((response) => response.json());
        
        commentLikeCount.innerHTML = `<b>${newLikesCount.usersThatLikePost.length}</b> likes`;
    }

    commentLikeCount.addEventListener('mouseout', () =>
    {
        commentLikeCount.style.textDecoration = 'none';
    })

    commentLikeCount.addEventListener('mouseover', () =>
    {
        commentLikeCount.style.textDecoration = 'underline';
    });

    commentLikeCount.addEventListener('click', () =>
    {
        const newWindowWidht = 500, newWindowHeight = 800;
        window.open(`${document.URL}/likes`, 'targetWindow',
                                `toolbar=no,
                                location=no,
                                status=no,
                                menubar=no,
                                scrollbars=yes,
                                resizable=no,
                                width=${newWindowWidht}px,
                                height=${newWindowHeight}px,
                                left=${window.screen.width/2 - newWindowWidht/2},
                                top=${window.screen.height/2 - newWindowHeight/2}`);
        
    });    

    likeIconWrapper.appendChild(likeIcon);

    return likeIconWrapper;
}

export async function BuildLikeIconForComment(post, likes)
{
    const commentLikeCount = document.createElement('a')
    commentLikeCount.id = 'comment-like-count';
    commentLikeCount.className = 'comment-like-count';
    commentLikeCount.innerHTML = `${likes.length}`;
    commentLikeCount.style.textDecoration = 'none';
    commentLikeCount.style.color = 'black';

    
    const likeSectionWrapper = document.createElement('div');
    likeSectionWrapper.className = 'like-section-wrapper';

    const likesData = await fetch(`${window.location.origin}/api/likes`, 
        {
            method: 'GET',
            headers:
            {
                'content-type': 'application/json'
            }
        })
        .then((response) => response.json());
    const userLikes = likesData.postIdsThatUserLike;

    const likeIconWrapper = document.createElement('div');
    likeIconWrapper.className = 'like-icon-wrapper';
    const likeIcon = document.createElement('a');    
    likeIcon.id = 'like-icon';
    likeIcon.className = 'like-icon';    

    
    if (userLikes.includes(post.postId))
    {
        likeIcon.innerHTML = '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 358.299 358.299" style="enable-background:new 0 0 358.299 358.299;"><path id="XMLID_169_" d="M 251.787 27.034 C 224.391 27.034 198.721 37.281 179.149 55.615 C 159.577 37.281 133.907 27.034 106.511 27.034 C 47.781 27.034 0 74.816 0 133.547 C 0 162.476 9.521 190.956 28.298 218.194 C 42.816 239.254 62.901 259.611 87.993 278.698 C 130.085 310.718 171.642 328.162 173.391 328.889 L 179.106 331.265 L 184.834 328.92 C 186.584 328.204 228.168 311.007 270.282 279.103 C 295.389 260.083 315.484 239.733 330.009 218.617 C 348.781 191.327 358.299 162.706 358.299 133.548 C 358.299 74.816 310.518 27.034 251.787 27.034 Z" style="paint-order: fill; fill-rule: nonzero; fill: rgb(252, 0, 0);"/></svg>';
        commentLikeCount.style.color = 'red';
    }
    else
    {
        likeIcon.innerHTML = '<svg viewBox="0 0 24 24" aria-hidden="true" class="r-4qtqp9 r-yyyyoo r-1xvli5t r-dnmrzs r-bnwqim r-1plcrui r-lrvibr r-1hdv0qi"><g><path d="M12 21.638h-.014C9.403 21.59 1.95 14.856 1.95 8.478c0-3.064 2.525-5.754 5.403-5.754 2.29 0 3.83 1.58 4.646 2.73.814-1.148 2.354-2.73 4.645-2.73 2.88 0 5.404 2.69 5.404 5.755 0 6.376-7.454 13.11-10.037 13.157H12zM7.354 4.225c-2.08 0-3.903 1.988-3.903 4.255 0 5.74 7.034 11.596 8.55 11.658 1.518-.062 8.55-5.917 8.55-11.658 0-2.267-1.823-4.255-3.903-4.255-2.528 0-3.94 2.936-3.952 2.965-.23.562-1.156.562-1.387 0-.014-.03-1.425-2.965-3.954-2.965z"></path></g></svg>';
        likeSectionWrapper.onmouseover = () => 
            {
                likeIconWrapper.style.backgroundColor = 'rgb(255, 219, 219)';
                commentLikeCount.style.color = 'red';
                likeIcon.style.filter = 'invert(27%) sepia(51%) saturate(2878%) hue-rotate(346deg) brightness(104%) contrast(97%)';
            };
        likeSectionWrapper.onmouseout = () => 
            {
                likeIconWrapper.style.backgroundColor = 'white';
                commentLikeCount.style.color = 'black';
                likeIcon.style.filter = '';
            };
    }

    

    likeIconWrapper.appendChild(likeIcon);
    
    const commentLikeCountWrapper = document.createElement('div');
    commentLikeCountWrapper.className = 'like-count-wrapper';
    commentLikeCountWrapper.appendChild(commentLikeCount);

    
    likeSectionWrapper.appendChild(likeIconWrapper);
    likeSectionWrapper.appendChild(commentLikeCountWrapper);

    likeSectionWrapper.onclick = async () =>
    {
        await fetch(`${window.location.origin}/api/likes`, 
            {
                method: 'PATCH',
                headers:
                {
                    'content-type': 'application/json'
                },
                body: JSON.stringify({
                    likedPostId: `${post.postId}`,
                    likedByUsername: ''
                })
            });

        const newLikesData = await fetch(`${window.location.origin}/api/likes`, 
            {
                method: 'GET',
                headers:
                {
                    'content-type': 'application/json'
                }
            })
            .then((response) => response.json());

        const newUserLikes = newLikesData.postIdsThatUserLike;
        if (newUserLikes.includes(post.postId))
        {
            commentLikeCount.style.color = 'red';
            likeIcon.innerHTML = '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 358.299 358.299" style="enable-background:new 0 0 358.299 358.299;"><path id="XMLID_169_" d="M 251.787 27.034 C 224.391 27.034 198.721 37.281 179.149 55.615 C 159.577 37.281 133.907 27.034 106.511 27.034 C 47.781 27.034 0 74.816 0 133.547 C 0 162.476 9.521 190.956 28.298 218.194 C 42.816 239.254 62.901 259.611 87.993 278.698 C 130.085 310.718 171.642 328.162 173.391 328.889 L 179.106 331.265 L 184.834 328.92 C 186.584 328.204 228.168 311.007 270.282 279.103 C 295.389 260.083 315.484 239.733 330.009 218.617 C 348.781 191.327 358.299 162.706 358.299 133.548 C 358.299 74.816 310.518 27.034 251.787 27.034 Z" style="paint-order: fill; fill-rule: nonzero; fill: rgb(252, 0, 0);"/></svg>';
            likeSectionWrapper.onmouseover = () => 
                {
                    likeIconWrapper.style.backgroundColor = 'rgb(255, 219, 219)';
                };
            likeSectionWrapper.onmouseout = () => 
                {
                    likeIconWrapper.style.backgroundColor = 'white';
                };
        }
        else
        {
            commentLikeCount.style.color = 'black';
            likeIcon.innerHTML = '<svg viewBox="0 0 24 24" aria-hidden="true" class="r-4qtqp9 r-yyyyoo r-1xvli5t r-dnmrzs r-bnwqim r-1plcrui r-lrvibr r-1hdv0qi"><g><path d="M12 21.638h-.014C9.403 21.59 1.95 14.856 1.95 8.478c0-3.064 2.525-5.754 5.403-5.754 2.29 0 3.83 1.58 4.646 2.73.814-1.148 2.354-2.73 4.645-2.73 2.88 0 5.404 2.69 5.404 5.755 0 6.376-7.454 13.11-10.037 13.157H12zM7.354 4.225c-2.08 0-3.903 1.988-3.903 4.255 0 5.74 7.034 11.596 8.55 11.658 1.518-.062 8.55-5.917 8.55-11.658 0-2.267-1.823-4.255-3.903-4.255-2.528 0-3.94 2.936-3.952 2.965-.23.562-1.156.562-1.387 0-.014-.03-1.425-2.965-3.954-2.965z"></path></g></svg>';
            likeSectionWrapper.onmouseover = () => 
                {
                    likeIconWrapper.style.backgroundColor = 'rgb(255, 219, 219)';
                    commentLikeCount.style.color = 'red';
                    likeIcon.style.filter = 'invert(27%) sepia(51%) saturate(2878%) hue-rotate(346deg) brightness(104%) contrast(97%)';
                };
            likeSectionWrapper.onmouseout = () => 
                {
                    likeIconWrapper.style.backgroundColor = 'white';
                    commentLikeCount.style.color = 'black'
                    likeIcon.style.filter = ''
                };
        }

        const newLikesCount = await fetch(`${window.location.origin}/api/likes/${post.postId}`, 
        {
            method: 'GET',
            headers:
            {
                'content-type': 'application/json'
            }
        })
        .then((response) => response.json());
        
        commentLikeCount.innerHTML = `${newLikesCount.usersThatLikePost.length}`;
    }

    return likeSectionWrapper;
}

export async function BuildCommentIcon(commetToPostId)
{
    const commentIconWrapper = document.createElement('div');
    commentIconWrapper.className = 'comment-icon-wrapper';

    const commentIcon = document.createElement('a');
    commentIcon.id = 'comment-icon';
    commentIcon.className = 'comment-icon';
    commentIcon.innerHTML = '<svg xmlns="http://www.w3.org/2000/svg" width="24px" height="24px" viewBox="0 0 24 24"><path fill-rule="evenodd" d="M3.25 4a.25.25 0 00-.25.25v12.5c0 .138.112.25.25.25h2.5a.75.75 0 01.75.75v3.19l3.72-3.72a.75.75 0 01.53-.22h10a.25.25 0 00.25-.25V4.25a.25.25 0 00-.25-.25H3.25zm-1.75.25c0-.966.784-1.75 1.75-1.75h17.5c.966 0 1.75.784 1.75 1.75v12.5a1.75 1.75 0 01-1.75 1.75h-9.69l-3.573 3.573A1.457 1.457 0 015 21.043V18.5H3.25a1.75 1.75 0 01-1.75-1.75V4.25z"/></svg>';
    commentIcon.onclick = () => 
    {
        const newWindowWidht = 500, newWindowHeight = 150;
        window.open(`${window.location.origin}/posts/${commetToPostId}/make-comment`, 'targetWindow',
                                `toolbar=no,
                                location=no,
                                status=no,
                                menubar=no,
                                scrollbars=yes,
                                resizable=no,
                                width=${newWindowWidht}px,
                                height=${newWindowHeight}px,
                                left=${window.screen.width/2 - newWindowWidht/2},
                                top=${window.screen.height/2 - newWindowHeight/2}`);
    };

    commentIconWrapper.appendChild(commentIcon);
    
    return commentIconWrapper;
}