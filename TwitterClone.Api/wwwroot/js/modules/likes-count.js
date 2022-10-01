export async function BuildLikeCount(post, likes)
{
    const likeCount = document.createElement('p')
    likeCount.id = 'like-count';
    likeCount.className = 'like-count';
    likeCount.innerHTML = `<b>${likes.usersThatLikePost.length}</b> likes`;
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

export async function GetLikes(post)
{
    return await fetch(`${window.location.origin}/api/likes/${post.postId}`)
        .then((response) => response.json());
}