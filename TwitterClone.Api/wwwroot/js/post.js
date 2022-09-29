const sendButton = document.querySelector("#make-new-comment-button");
const basePostWrapper = document.querySelector("#base-post-wrapper");
const comments = document.querySelector("#comments-wrapper");

async function BuildPage(inputPostId)
{

    const postData = await fetch(`${window.location.origin}/api/posts/${inputPostId}`)
        .then((response) => response.json()); 

    const post = postData.post;
    const allComments = postData.comments;
    const likes = postData.likedByUsername;

    
    const basePost = await RenderBasePost(post, likes);    
    basePostWrapper.appendChild(basePost);
    
    allComments.forEach(oldComment => {
        RenderComment(oldComment);
    });
}

async function RenderBasePost(post, likes)
{
    const basePost = document.createElement('div');
    basePost.id = 'base-post';
    basePost.className = 'base-post';

    const basePostAuthorUsername = document.createElement('a');
    basePostAuthorUsername.id = 'base-post-author-username';
    basePostAuthorUsername.className = 'base-post-author-username';
    basePostAuthorUsername.innerText = post.authorUsername;
    basePostAuthorUsername.href = `${window.location.origin}/users/${post.authorUsername}`

    const basePostPostDate = document.createElement('a');
    basePostPostDate.id = 'base-post-post-date';
    basePostPostDate.className = 'base-post-post-date';
    basePostPostDate.innerText = (new Date(Date.parse(post.postDate))).toUTCString();

    const basePostTextArea = document.createElement('textarea');
    basePostTextArea.id = 'base-post-message';
    basePostTextArea.className = 'base-post-message';
    basePostTextArea.value = post.message;
    basePostTextArea.readOnly = true;
    basePostTextArea.style.height = `${parseFloat(window.getComputedStyle(basePostTextArea, null).getPropertyValue('font-size'))*basePostTextArea.value.split('\n').length}px`;

    const likeCount = document.createElement('a')
    likeCount.id = 'like-count';
    likeCount.className = 'like-count';
    likeCount.innerHTML = `<b>${likes.length}</b> likes`;
    likeCount.style.textDecoration = 'none';
    likeCount.style.color = 'black';

    const separator = document.getElementById('separator');

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

    const likeIconContainer = document.createElement('div');
    likeIconContainer.className = 'like-icon-container';
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

    likeIconContainer.onclick = async () =>
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
        
        likeCount.innerHTML = `<b>${newLikesCount.usersThatLikePost.length}</b> likes`;
    }

    likeIconContainer.appendChild(likeIcon)
    separator.appendChild(likeIconContainer)
    //separator.innerText = 'here will be like, commemt, and may be repost buttons'

    basePost.appendChild(basePostAuthorUsername);
    basePost.appendChild(document.createElement('tr'));
    basePost.appendChild(basePostPostDate);
    basePost.appendChild(document.createElement('tr'));
    basePost.appendChild(basePostTextArea);
    basePost.appendChild(document.createElement('tr'));
    basePost.appendChild(likeCount);

    basePostWrapper.appendChild(basePost);

    likeCount.addEventListener('mouseout', () =>
    {
        likeCount.style.textDecoration = 'none';
    })

    likeCount.addEventListener('mouseover', () =>
    {
        likeCount.style.textDecoration = 'underline';
    });

    likeCount.addEventListener('click', () =>
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

    return basePost;
}

function RenderComment(oldComment)
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
       
        comment.appendChild(commentAuthorUsername);
        comment.appendChild(document.createElement('tr'));
        comment.appendChild(commentPostDate);
        comment.appendChild(document.createElement('tr'));
        comment.appendChild(commetTextArea);
        
        comments.appendChild(comment);
}