# GithubX

# Github Api

### Readme

** Attention: Sometimes dev use html tags

Dir > https://api.github.com/repos/{user}/{repo}/contents/

Raw > https://raw.githubusercontent.com/{user}/{repo}/master/README.md

    {
    "name": "README.md",
    "path": "README.md",
    "sha": "c8cfde2e782d4aef736203b0d374af084de2d5aa",
    "size": 1113,
    "url": "https://api.github.com/repos/yazdipour/DM17/contents/README.md?ref=master",
    "html_url": "https://github.com/yazdipour/DM17/blob/master/README.md",
    "git_url": "https://api.github.com/repos/yazdipour/DM17/git/blobs/c8cfde2e782d4aef736203b0d374af084de2d5aa",
    "download_url": "https://raw.githubusercontent.com/yazdipour/DM17/master/README.md",
    "type": "file",
    "_links": {
                "self": "https://api.github.com/repos/yazdipour/DM17/contents/README.md?ref=master",
                "git": "https://api.github.com/repos/yazdipour/DM17/git/blobs/c8cfde2e782d4aef736203b0d374af084de2d5aa",
                "html": "https://github.com/yazdipour/DM17/blob/master/README.md"
            }
    }


## StarList

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

## UserInfo
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
