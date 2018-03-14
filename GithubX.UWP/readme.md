# GithubX.UWP

# Tasks

- [ ] Use Error Msg in better ways
- [ ] Unit Test
- [ ] Pocket.Api [Time Consuming] https://getpocket.com/developer/docs/authentication
- [ ] Push to Store
- [ ] Add Card to http://shahriar.in/app
- [ ] Test By Hand
- [ ] Set Icon
- [ ] CategoryDialog List Handler
- [ ] Context Menu for StarItem in MainPage
- [ ] Test Login Speed Again
- [ ] Page Save State
- [ ] Change Saving Model for Categories from ID to Name (this way it will be easier to track MoveTo later)
- [x] CategoryDialog Design
- [x] loading stuff
- [x] caching stuff
- [x] md button bar events
- [x] md Cache
- [x] Fix 403 forbidden response
- [x] Main Xaml Design
- [x] Main Api Design

# Libs

* Json.NET
* [HTML2Markdown](https://github.com/baynezy/Html2Markdown): Some readmes also have html tags
* UWPCommunityToolkit 2.2

# Cache

* [WindowsCacheHandler](/GithubX.UWP/Services/Cache/WindowsCacheHandler.cs) [For User Profile and Settings]
* [LocalCacheHandler](/GithubX.UWP/Services/Cache/LocalCacheHandler.cs) [For Offline Mode and MD Files]
* [ServerCacheHandler](/GithubX.UWP/Services/Cache/ServerCacheHandler.cs) [For Saving Categories in Server]