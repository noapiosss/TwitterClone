import {BuildHeader} from "./modules/header.js";
import {BuildPost} from "./modules/build-post.js";

const postsWrapper = document.getElementById("posts-wrapper");

window.document.body.onload = async () =>
{
    BuildHeader();

    const inputUsername = window.location.href.split('/').pop();
    const userPosts = await fetch(`${window.location.origin}/api/users/${inputUsername}/posts`)
        .then((response) => response.json())
        .then((result) => result.userPosts);

    const followingsFollowersWrapper = await GetFollowingsFollowersWrapper(inputUsername);

    const userInfo = document.createElement('div');
    userInfo.className = 'username-info';

    const username = document.createElement('h1');
    username.className = 'username';
    username.innerText = inputUsername;

    const postsCount = document.createElement('p');
    postsCount.className = 'posts-count';
    postsCount.innerText = `${userPosts.length} messages`;

    const usernameHeader = document.createElement('div');
    usernameHeader.className = 'username-header';    

    usernameHeader.appendChild(username);
    usernameHeader.appendChild(postsCount);

    
    userInfo.appendChild(usernameHeader);
    userInfo.appendChild(followingsFollowersWrapper);

    postsWrapper.appendChild(userInfo);
    
    BuildBody(userPosts);
}

async function BuildBody(userPosts)
{   
    for (let i = 0; i <userPosts.length; i++)
    {
        const post = await BuildPost(userPosts[i]);
        postsWrapper.appendChild(post);
    };
}

async function GetFollowingsFollowersWrapper(inputUsername)
{
    const followingsWrapper = document.createElement('div');
    followingsWrapper.className = 'followings-wrapper';

    const allFollowings = await fetch(`${window.location.origin}/api/${inputUsername}/followings`)
        .then((response) => response.json())
        .then((result) => result.followings);

    const followings = document.createElement('div');
    followings.className = 'followers';
    followings.innerHTML = `<b>${allFollowings.length}</b> followings`
    followingsWrapper.appendChild(followings);

    followingsWrapper.onclick = () =>
    {
        const newWindowWidht = 500, newWindowHeight = 800;
        window.open(`${window.location.origin}/users/${inputUsername}/followings`, 'targetWindow',
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
    }

    const followersWrapper = document.createElement('div');
    followersWrapper.className = 'followers-wrapper';    

    const allFollowers = await fetch(`${window.location.origin}/api/${inputUsername}/followers`)
        .then((response) => response.json())
        .then((result) => result.followers);

    const followers = document.createElement('div');
    followers.className = 'followers';
    followers.innerHTML = `<b>${allFollowers.length}</b> followers`
    followersWrapper.appendChild(followers);

    followersWrapper.onclick = () =>
    {
        const newWindowWidht = 500, newWindowHeight = 800;
        window.open(`${window.location.origin}/users/${inputUsername}/followers`, 'targetWindow',
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
    }

    const followingsFollowersWrapper = document.createElement('div');
    followingsFollowersWrapper.className = 'followings-followers-wrapper';

    followingsFollowersWrapper.appendChild(followingsWrapper);
    followingsFollowersWrapper.appendChild(followersWrapper);
    

    return followingsFollowersWrapper;
}