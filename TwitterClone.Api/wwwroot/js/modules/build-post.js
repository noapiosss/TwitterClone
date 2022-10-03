import {BuildMainPostInteractiveSection, BuildPostInteractiveSection} from "./build-icons.js"
import {GetPostDate, GetMainPostDate} from "./date-helper.js";

export async function BuildMainPost(post)
{
    const mainPostWrapper = BuildMainPostWrapper();
    const postAuthorUsername = BuildMainAuthorUsername(post);
    const postDate = BuildMainPostDate(post);
    const postMessage =  BuildMainPostMessage(post);
    const interactiveSetion = await BuildMainPostInteractiveSection(post);

    mainPostWrapper.appendChild(postAuthorUsername);
    mainPostWrapper.appendChild(document.createElement('tr'));
    mainPostWrapper.appendChild(postMessage);
    mainPostWrapper.appendChild(document.createElement('tr'));
    mainPostWrapper.appendChild(postDate);
    mainPostWrapper.appendChild(document.createElement('tr'));
    mainPostWrapper.appendChild(interactiveSetion);

    return mainPostWrapper;
}

function BuildMainPostWrapper()
{
    const postWrapper = document.createElement('div');
    postWrapper.id = 'main-post-wrapper';
    postWrapper.className = 'main-post-wrapper';
    return postWrapper;
}

function BuildMainAuthorUsername(post)
{
    const postAuthorUsername = document.createElement('a')
    postAuthorUsername.id = 'main-post-author-username';
    postAuthorUsername.className = 'main-post-author-username';
    postAuthorUsername.innerText = post.authorUsername;
    postAuthorUsername.href = `${window.location.origin}/users/${post.authorUsername}`;

    return postAuthorUsername;
}

function BuildMainPostDate(post)
{
    const postDate = document.createElement('a');
    postDate.id = 'main-post-date';
    postDate.className = 'main-post-date';
    postDate.innerText = GetMainPostDate(new Date(Date.parse(post.postDate)));
    postDate.href = `${window.location.origin}/posts/${post.postId}`;

    return postDate;
}

function BuildMainPostMessage(post)
{
    const postMessage = document.createElement('p');
    postMessage.id = 'main-post-message';
    postMessage.className = 'main-post-message';
    postMessage.innerText = post.message;

    return postMessage;
}

export async function BuildPost(post)
{
    const postWrapper = BuildPostWrapper();
    const postAuthorUsername = BuildAuthorUsername(post);
    const postDate = BuildPostDate(post);
    const postMessage =  BuildPostMessage(post);
    const interactiveSetion = await BuildPostInteractiveSection(post);
    
    postWrapper.appendChild(postAuthorUsername);    
    postWrapper.appendChild(postDate);
    postWrapper.appendChild(document.createElement('tr'));
    postWrapper.appendChild(postMessage);
    postWrapper.appendChild(document.createElement('tr'));
    postWrapper.appendChild(interactiveSetion);

    return postWrapper;
}

function BuildPostWrapper()
{
    const postWrapper = document.createElement('div');
    postWrapper.id = 'post-wrapper';
    postWrapper.className = 'post-wrapper';
    return postWrapper;
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

function BuildPostDate(post)
{
    const postDate = document.createElement('a');
    postDate.id = 'post-date';
    postDate.className = 'post-date';
    postDate.innerText = GetPostDate(new Date(Date.parse(post.postDate)));
    postDate.href = `${window.location.origin}/posts/${post.postId}`;

    return postDate;
}

function BuildPostMessage(post)
{
    const postMessage = document.createElement('p');
    postMessage.id = 'post-message';
    postMessage.className = 'post-message';
    postMessage.innerText = post.message;

    return postMessage;
}
