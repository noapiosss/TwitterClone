export async function BuildMainPostLikeCount(post)
{
    const likes = await GetPostLikes(post);

    const likeCount = document.createElement('a')
    likeCount.id = 'main-post-like-count';
    likeCount.className = 'main-post-like-count';
    likeCount.innerHTML = `<b>${likes.length}</b> likes`;
    likeCount.style.textDecoration = 'none';
    likeCount.style.color = 'black';

    likeCount.addEventListener('mouseout', () =>
    {
        likeCount.style.textDecoration = 'none';
    });

    likeCount.addEventListener('mouseover', () =>
    {
        likeCount.style.textDecoration = 'underline';
    });

    likeCount.addEventListener('click', () =>
    {
        const newWindowWidht = 500, newWindowHeight = 800;
        window.open(`${window.location.origin}/posts/${post.postId}/likes`, 'targetWindow',
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

    return likeCount;

}

export async function GetPostLikes(post)
{
    return await fetch(`${window.location.origin}/api/likes/${post.postId}`)
        .then((response) => response.json())
        .then((result) => result.usersThatLikePost);
}

export async function GetUserLikes()
{
    return await fetch(`${window.location.origin}/api/likes`)
        .then((response) => response.json())
        .then((result) => result.postIdsThatUserLike);
}

export async function PatchLikes(post)
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
}