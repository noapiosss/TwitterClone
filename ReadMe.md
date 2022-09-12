# Final Project

As a final project was chosen "Twitter clone"

## User stiries

1. As a user I want signup/signin on the website
2. As a user I want to make/delete my post
3. As a user I want to observe other users and their posts
4. As a user I want comment posts of other users
5. As a user I want "like" posts of other users
6. As a user I want to follow/unfollow another user
7. As a user I want to see posts of users that I follow on the home page

## HTTP API

- PUT /api/users BODY=username+login+password (signup)
- GET /api/users (signin)
- PUT /api/posts BODY=username+postDate+message (make post)
- DELETE /api/posts/{postId} (delete post)
- GET /api/users/{username} (observe another user)
- PUT /apo/posts/{postId} BODY=username+postDate+message (make comment)
- PATCH /api/users/{username(another user)}/posts/{postId} BODY=like/unlike+username(from session)
- PATCH /api/users/{username(from session)} BODY=follow/unfollow+username(another user)
- GET /api/users/{username{from sesseion}}/posts (home page)
  
## Models

### User

- username
- email
- password
- followTo (username list)

### Post

- id
- username
- commentTo (comment to another post (as id) optional field: if it is comment should be a reference on post)
- postDate
- message
- likedBy (list of usernames)