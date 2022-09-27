const sendButton = document.querySelector("#make-new-comment-button");
const basePostWrapper = document.querySelector("#base-post-wrapper");
const comments = document.querySelector("#comments-wrapper");

function BuildPage(postData, likesData)
{
    console.log(postData)
    const data = JSON.parse(postData);
    const likes = JSON.parse(likesData);
    console.log(data)


    const basePost = document.createElement('div');
    basePost.id = 'base-post';
    basePost.className = 'base-post';

    const basePostAuthorUsername = document.createElement('a');
    basePostAuthorUsername.id = 'base-post-author-username';
    basePostAuthorUsername.className = 'base-post-author-username';
    basePostAuthorUsername.innerText = data.post.authorUsername;
    basePostAuthorUsername.href = `http://localhost:3000/users/${data.post.authorUsername}`

    const basePostPostDate = document.createElement('a');
    basePostPostDate.id = 'base-post-post-date';
    basePostPostDate.className = 'base-post-post-date';
    basePostPostDate.innerText = (new Date(Date.parse(data.post.postDate))).toUTCString();

    const basePostTextArea = document.createElement('textarea');
    basePostTextArea.id = 'base-post-message';
    basePostTextArea.className = 'base-post-message';
    basePostTextArea.value = data.post.message;
    basePostTextArea.readOnly = true;
    basePostTextArea.style.height = `${parseFloat(window.getComputedStyle(basePostTextArea, null).getPropertyValue('font-size'))*basePostTextArea.value.split('\n').length}px`;

    const likeCount = document.createElement('a')
    likeCount.id = 'like-count';
    likeCount.className = 'like-count';
    likeCount.innerHTML = `<b>${data.likedByUsername.length}</b> likes`;
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


    data.comments.forEach(oldComment => {
        const comment = document.createElement('div');
        comment.id = 'comment';
        comment.className = 'comment';

        
        const commentAuthorUsername = document.createElement('a');
        commentAuthorUsername.id = 'comment-author-username';
        commentAuthorUsername.className = 'comment-author-username';
        commentAuthorUsername.innerText = oldComment.authorUsername;
        commentAuthorUsername.href = `http://localhost:3000/users/${oldComment.authorUsername}`

        const commentPostDate = document.createElement('a');
        commentPostDate.id = 'comment-date';
        commentPostDate.className = 'comment-date';
        commentPostDate.innerText = (new Date(Date.parse(oldComment.postDate))).toUTCString();;
        commentPostDate.href = `http://localhost:3000/posts/${oldComment.postId}`;

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
    });
}

function AutoGrow(element) {
    element.style.height = "40px";
    element.style.height = (element.scrollHeight)+"px";
}

sendButton.addEventListener('click', ()=>{
    const newPost = document.createElement('div');
    newPost.id = "old-post";
    newPost.className = "old-post";

    const newPostTextArea = document.createElement('textarea');
    newPostTextArea.id = "old-post-textarea";
    newPostTextArea.className = "old-post-textarea";
    newPostTextArea.value = newPostText.value;
    newPostTextArea.readOnly = true;
    newPostTextArea.style.height = 17*newPostTextArea.value.split("\n").length + "px";

    newPost.appendChild(newPostTextArea);
    allPosts.appendChild(newPost);

    newPostText.style.height ="40px";
    newPostText.value = "";
});