import {BuildMainPostInteractiveSection} from "./build-icons.js"

export async function BuildMainPost(post)
{
    const mainPostWrapper = BuildMainPostWrapper();
    const postAuthorUsername = BuildAuthorUsername(post);
    const postDate = BuildPostDate(post);
    const postMessage =  BuildPostMessage(post);
    const interactiveSetion = await BuildMainPostInteractiveSection(post);

    mainPostWrapper.appendChild(postAuthorUsername);
    mainPostWrapper.appendChild(document.createElement('tr'));
    mainPostWrapper.appendChild(postDate);
    mainPostWrapper.appendChild(document.createElement('tr'));
    mainPostWrapper.appendChild(postMessage);
    mainPostWrapper.appendChild(document.createElement('tr'));
    mainPostWrapper.appendChild(interactiveSetion);

    return mainPostWrapper;
}

export async function BuildPost(post)
{
    const postWrapper = BuildPostWrapper();
    const postAuthorUsername = BuildAuthorUsername(post);
    const postDate = BuildPostDate(post);
    const postMessage =  BuildPostMessage(post);

    postWrapper.appendChild(postAuthorUsername);
    postWrapper.appendChild(document.createElement('tr'));
    postWrapper.appendChild(postDate);
    postWrapper.appendChild(document.createElement('tr'));
    postWrapper.appendChild(postMessage);

    return postWrapper;
}

function BuildMainPostWrapper()
{
    const postWrapper = document.createElement('div');
    postWrapper.id = 'main-post-wrapper';
    postWrapper.className = 'main-post-wrapper';
    return postWrapper;
}

function BuildPostWrapper()
{
    const postWrapper = document.createElement('div');
    postWrapper.id = 'post-wrapper';
    postWrapper.className = 'post-wrapper';
    return postWrapper;
}

function BuildPostDate(post)
{
    const postDate = document.createElement('a');
    postDate.id = 'post-date';
    postDate.className = 'post-date';
    postDate.innerText = (new Date(Date.parse(post.postDate))).toUTCString();;
    postDate.href = `${window.location.origin}/posts/${post.postId}`;

    return postDate;
}

function BuildPostMessage(post)
{
    const postMessage = document.createElement('textarea');
    postMessage.id = 'post-message';
    postMessage.className = 'post-message';
    postMessage.value = post.message;
    postMessage.readOnly = true;
    postMessage.style.height = `${parseFloat(window.getComputedStyle(postMessage, null).getPropertyValue('font-size'))*postMessage.value.split('\n').length}px`;

    return postMessage;
}

function BuildAuthorUsername(post)
{
    const postAuthorUsername = document.createElement('a')
    postAuthorUsername.id = 'post-author-username';
    postAuthorUsername.className = 'post-author-username';
    postAuthorUsername.innerText = post.authorUsername;
    postAuthorUsername.href = `${window.location.origin}/users/${post.authorUsername}`;

    return postAuthorUsername;
}