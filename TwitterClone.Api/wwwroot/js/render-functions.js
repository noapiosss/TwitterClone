function RenderBasePost(post, likes)
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
    separator.innerText = 'here will be like, commemt, and may be repost buttons'

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

function RenderLikes(likes)
{

}