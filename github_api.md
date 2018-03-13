# GithubX

# Github Api

### Get Readme
https://raw.githubusercontent.com/{user}/{repo}/master/README.md

* Sometimes dev use html tags

https://raw.githubusercontent.com/pqina/filepond/master/README.md

### Get Readme.Base64 (Hard)

https://api.github.com/repos/yazdipour/dm17/contents/README.md


## Api.StarList

https://api.github.com/users/yazdipour/starred


    [
    {
        "id": 106279637,
        "name": "filepond",
        "full_name": "pqina/filepond",
        "owner": {
        "login": "pqina",
        "id": 22966117,
        "avatar_url": "https://avatars0.githubusercontent.com/u/22966117?v=4"},
        "private": false,
        "html_url": "https://github.com/pqina/filepond",
        "description": "🌊 A Flexible and Fun JavaScript File Upload Library",
        "fork": false,
        "url": "https://api.github.com/repos/pqina/filepond",
        "homepage": "https://pqina.nl/filepond",
        "stargazers_count": 1557,
        "watchers_count": 1557,
        "language": "JavaScript",
        "has_issues": true,
        "has_projects": false,
        "has_downloads": true,
        "has_wiki": false,
        "has_pages": false,
        "forks": 48,
        "default_branch": "master"
    }
    ]

## Api.UserInfo
https://api.github.com/users/{user}

    {
    "login": "pqina",
    "id": 22966117,
    "avatar_url": "https://avatars0.githubusercontent.com/u/22966117?v=4",
    "html_url": "https://github.com/pqina",
    "starred_url": "https://api.github.com/users/pqina/starred{/owner}{/repo}",
        "type": "User",
    "site_admin": false,
    "name": "pqina",
    "company": "pqina",
    "blog": "https://pqina.nl",
    "location": "The Netherlands",
    "email": null,
    "hireable": null,
    "bio": "@pqina is @rikschennink's tiny one man web outpost.",
    "public_repos": 17,
    "public_gists": 1,
    "followers": 25,
    "following": 1
    }
