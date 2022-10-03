import {PatchLikes, GetUserLikes, GetPostLikes, BuildMainPostLikeCount} from "./likes-helper.js";

export async function BuildMainPostInteractiveSection(post)
{
    const {postLikesCount, likeIconWrapper} = await BuildLikeIconForMainPost(post);
    const commentIconWrapper = await BuildMaintPostCommentIcon(post);

    const mainPostInteractiveSection = document.createElement('div');
    mainPostInteractiveSection.className = 'main-post-interactive-section';

    const iconsWrapper = document.createElement('div');
    iconsWrapper.className = 'main-post-icons-wrapper';
    iconsWrapper.appendChild(commentIconWrapper);
    iconsWrapper.appendChild(likeIconWrapper);
    
    mainPostInteractiveSection.appendChild(postLikesCount);
    mainPostInteractiveSection.appendChild(iconsWrapper);

    return mainPostInteractiveSection;
}

async function BuildLikeIconForMainPost(post)
{
    const postLikesCount = await BuildMainPostLikeCount(post);

    const likeIconWrapper = document.createElement('div');
    likeIconWrapper.className = 'main-post-like-icon-wrapper';

    const likeIcon = document.createElement('a');    
    likeIcon.id = 'main-postlike-icon';
    likeIcon.className = 'main-post-like-icon';

    const userLikes = await GetUserLikes();
    SetMainPostInitialIcon(post, userLikes, likeIcon);

    likeIconWrapper.onclick = async () =>
    {
        await PatchLikes(post);

        const newUserLikes = await GetUserLikes();
        SetMainPostInitialIcon(post, newUserLikes, likeIcon)

        const newLikesCount = await GetPostLikes(post);
        postLikesCount.innerHTML = `<b>${newLikesCount.length}</b> likes`;
    }
    
    likeIconWrapper.appendChild(likeIcon);

    return {postLikesCount, likeIconWrapper};
}

function SetMainPostInitialIcon(post, userLikes, likeIcon)
{
    if (userLikes.includes(post.postId))
    {
        likeIcon.innerHTML = '<svg width="20" height="20" viewBox="0 0 358.299 358.299" fill="red" version="1.1"> <g><path id="XMLID_169_" d="M 251.787 27.034 C 224.391 27.034 198.721 37.281 179.149 55.615 C 159.577 37.281 133.907 27.034 106.511 27.034 C 47.781 27.034 0 74.816 0 133.547 C 0 162.476 9.521 190.956 28.298 218.194 C 42.816 239.254 62.901 259.611 87.993 278.698 C 130.085 310.718 171.642 328.162 173.391 328.889 L 179.106 331.265 L 184.834 328.92 C 186.584 328.204 228.168 311.007 270.282 279.103 C 295.389 260.083 315.484 239.733 330.009 218.617 C 348.781 191.327 358.299 162.706 358.299 133.548 C 358.299 74.816 310.518 27.034 251.787 27.034 Z"/></g></svg>';
    }
    else
    {
        likeIcon.innerHTML = '<svg width="20" height="20" viewBox="0 0 358.299 358.299" version="1.1"><g><path id="XMLID_169_" d="m251.787,27.034c-27.396,0 -53.066,10.247 -72.638,28.581c-19.572,-18.334 -45.242,-28.581 -72.638,-28.581c-58.73,0 -106.511,47.782 -106.511,106.513c0,28.929 9.521,57.409 28.298,84.647c14.518,21.06 34.603,41.417 59.695,60.504c42.092,32.02 83.649,49.464 85.398,50.191l5.715,2.376l5.728,-2.345c1.75,-0.716 43.334,-17.913 85.448,-49.817c25.107,-19.02 45.202,-39.37 59.727,-60.486c18.772,-27.29 28.29,-55.911 28.29,-85.069c0,-58.732 -47.781,-106.514 -106.512,-106.514zm-72.606,271.591c-28.032,-12.983 -149.181,-74.371 -149.181,-165.078c0,-42.189 34.323,-76.512 76.512,-76.512c23.976,0 46.114,10.929 60.738,29.985l11.9,15.507l11.9,-15.507c14.624,-19.056 36.762,-29.985 60.738,-29.985c42.188,0 76.512,34.323 76.512,76.512c-0.001,91.359 -121.099,152.224 -149.119,165.078z"/></g></svg>';
        likeIcon.onmouseover = () => {likeIcon.style.filter = 'invert(27%) sepia(51%) saturate(2878%) hue-rotate(346deg) brightness(104%) contrast(97%)'};
        likeIcon.onmouseout = () => {likeIcon.style.filter = ''};
    }
}

async function BuildMaintPostCommentIcon(post)
{
    const commentIconWrapper = document.createElement('div');
    commentIconWrapper.className = 'main-post-comment-icon-wrapper';

    const commentIcon = document.createElement('a');
    commentIcon.id = 'main-post-comment-icon';
    commentIcon.className = 'main-post-comment-icon';
    commentIcon.innerHTML = '<svg width="24px" height="24px" viewBox="0 0 24 24"><path fill-rule="evenodd" d="M3.25 4a.25.25 0 00-.25.25v12.5c0 .138.112.25.25.25h2.5a.75.75 0 01.75.75v3.19l3.72-3.72a.75.75 0 01.53-.22h10a.25.25 0 00.25-.25V4.25a.25.25 0 00-.25-.25H3.25zm-1.75.25c0-.966.784-1.75 1.75-1.75h17.5c.966 0 1.75.784 1.75 1.75v12.5a1.75 1.75 0 01-1.75 1.75h-9.69l-3.573 3.573A1.457 1.457 0 015 21.043V18.5H3.25a1.75 1.75 0 01-1.75-1.75V4.25z"/></svg>';
    commentIcon.onclick = () => 
    {
        const newWindowWidht = 500, newWindowHeight = 150;
        window.open(`${window.location.origin}/posts/${post.postId}/make-comment`, 'targetWindow',
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

export async function BuildPostInteractiveSection(post)
{
    const likeIconWrapper = await BuildLikeIconForPost(post);
    const commentIconWrapper = await BuildPostCommentIcon(post);

    const postInteractiveSection = document.createElement('div');
    postInteractiveSection.className = 'post-interactive-section';

    const iconsWrapper = document.createElement('div');
    iconsWrapper.className = 'post-icons-wrapper';
    iconsWrapper.appendChild(commentIconWrapper);
    iconsWrapper.appendChild(likeIconWrapper);
    
    postInteractiveSection.appendChild(iconsWrapper);

    return postInteractiveSection;
}

async function BuildLikeIconForPost(post)
{
    const likeCount = await GetPostLikes(post);
    
    const postLikeCountWrapper = document.createElement('div');
    postLikeCountWrapper.className = 'post-like-count-wrapper';
    const postLikeCount = document.createElement('p');
    postLikeCount.className = 'post-like-count';
    postLikeCount.innerHTML = `${likeCount.length}`;

    const likeIconWrapper = document.createElement('div');
    likeIconWrapper.className = 'post-like-icon-wrapper';

    const likeIcon = document.createElement('a');    
    likeIcon.id = 'post-like-icon';
    likeIcon.className = 'post-like-icon';
    
    postLikeCountWrapper.appendChild(postLikeCount);
    likeIconWrapper.appendChild(likeIcon);
    
    const postLikeSection = document.createElement('div');
    postLikeSection.className = 'post-like-section';

    postLikeSection.appendChild(likeIconWrapper);
    postLikeSection.appendChild(postLikeCountWrapper);

    const userLikes = await GetUserLikes();
    SetPostInitialIcon(post, userLikes, likeIcon, postLikeCount, postLikeSection);

    postLikeSection.onclick = async () =>
    {
        await PatchLikes(post);

        const newUserLikes = await GetUserLikes();
        SetPostInitialIcon(post, newUserLikes, likeIcon, postLikeCount, postLikeSection);

        const newLikesCount = await GetPostLikes(post);
        postLikeCount.innerHTML = `${newLikesCount.length}`;
    }
    
    

    return postLikeSection;
}

function SetPostInitialIcon(post, userLikesData, likeIcon, postLikeCount, postLikeSection)
{
    if (userLikesData.includes(post.postId))
    {
        likeIcon.innerHTML = '<svg width="20" height="20" viewBox="0 0 358.299 358.299" fill="red" version="1.1"> <g><path id="XMLID_169_" d="M 251.787 27.034 C 224.391 27.034 198.721 37.281 179.149 55.615 C 159.577 37.281 133.907 27.034 106.511 27.034 C 47.781 27.034 0 74.816 0 133.547 C 0 162.476 9.521 190.956 28.298 218.194 C 42.816 239.254 62.901 259.611 87.993 278.698 C 130.085 310.718 171.642 328.162 173.391 328.889 L 179.106 331.265 L 184.834 328.92 C 186.584 328.204 228.168 311.007 270.282 279.103 C 295.389 260.083 315.484 239.733 330.009 218.617 C 348.781 191.327 358.299 162.706 358.299 133.548 C 358.299 74.816 310.518 27.034 251.787 27.034 Z"/></g></svg>';
        postLikeCount.style.color = "red";        
        postLikeSection.onmouseover = () => {};
        postLikeSection.onmouseout = () => {};
    }
    else
    {
        likeIcon.innerHTML = '<svg width="20" height="20" viewBox="0 0 358.299 358.299" version="1.1"><g><path id="XMLID_169_" d="m251.787,27.034c-27.396,0 -53.066,10.247 -72.638,28.581c-19.572,-18.334 -45.242,-28.581 -72.638,-28.581c-58.73,0 -106.511,47.782 -106.511,106.513c0,28.929 9.521,57.409 28.298,84.647c14.518,21.06 34.603,41.417 59.695,60.504c42.092,32.02 83.649,49.464 85.398,50.191l5.715,2.376l5.728,-2.345c1.75,-0.716 43.334,-17.913 85.448,-49.817c25.107,-19.02 45.202,-39.37 59.727,-60.486c18.772,-27.29 28.29,-55.911 28.29,-85.069c0,-58.732 -47.781,-106.514 -106.512,-106.514zm-72.606,271.591c-28.032,-12.983 -149.181,-74.371 -149.181,-165.078c0,-42.189 34.323,-76.512 76.512,-76.512c23.976,0 46.114,10.929 60.738,29.985l11.9,15.507l11.9,-15.507c14.624,-19.056 36.762,-29.985 60.738,-29.985c42.188,0 76.512,34.323 76.512,76.512c-0.001,91.359 -121.099,152.224 -149.119,165.078z"/></g></svg>';
        postLikeCount.style.color = "black";
        postLikeSection.onmouseover = () => 
        {
            likeIcon.style.filter = 'invert(27%) sepia(51%) saturate(2878%) hue-rotate(346deg) brightness(104%) contrast(97%)';
            postLikeCount.style.color = "red";
        };
        postLikeSection.onmouseout = () => 
        {
            likeIcon.style.filter = '';
            postLikeCount.style.color = "black";
        };
    }
}

async function BuildPostCommentIcon(post)
{
    const commentIconWrapper = document.createElement('div');
    commentIconWrapper.className = 'post-comment-icon-wrapper';

    const commentIcon = document.createElement('a');
    commentIcon.id = 'postcomment-icon';
    commentIcon.className = 'post-comment-icon';
    commentIcon.innerHTML = '<svg width="24px" height="24px" viewBox="0 0 24 24"><path fill-rule="evenodd" d="M3.25 4a.25.25 0 00-.25.25v12.5c0 .138.112.25.25.25h2.5a.75.75 0 01.75.75v3.19l3.72-3.72a.75.75 0 01.53-.22h10a.25.25 0 00.25-.25V4.25a.25.25 0 00-.25-.25H3.25zm-1.75.25c0-.966.784-1.75 1.75-1.75h17.5c.966 0 1.75.784 1.75 1.75v12.5a1.75 1.75 0 01-1.75 1.75h-9.69l-3.573 3.573A1.457 1.457 0 015 21.043V18.5H3.25a1.75 1.75 0 01-1.75-1.75V4.25z"/></svg>';
    commentIcon.onclick = () => 
    {
        const newWindowWidht = 500, newWindowHeight = 150;
        window.open(`${window.location.origin}/posts/${post.postId}/make-comment`, 'targetWindow',
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