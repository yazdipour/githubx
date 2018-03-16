# GithubX.UWP

# Libs

* Json.NET
* [HTML2Markdown](https://github.com/baynezy/Html2Markdown): Some readmes also have html tags
* UWPCommunityToolkit 2.2

# First Release Tasks

## Todo
- [ ] Something To Manage Api Keys
- [ ] Find a better HTML2Markdown Lib
- [ ] Try https://github.com/lunet-io/markdig + if good : replace Html2Markdown Lib
- [ ] Push to Store
- [ ] Use Error Msg in better ways
- [ ] `Api` How UserAgent has effect and is it important?!

## Critical
- [ ] Why Login is Sooo Slow
- [ ] Do not Reload whole Page for refreshing! > When Chaning Categories Also it Goes Back to Tab[0]
- [ ] `Bug` When UnToggle a Repo from Its Category it does not update automaticly
- [ ] Cover UWPCommunityToolkit.Markdown Parser Failiers

## Future
- [ ] Add animations
- [ ] Backup online
- [ ] Backup offline
- [ ] Live Tile Maybe?
- [ ] Pocket.Api https://getpocket.com/developer/docs/authentication
- [ ] MD2Html and show it Webview / OR Webview beside MDView
- [ ] MoveTo Button inside RepoPage
- [ ] Extract Private Repo and never request them
- [ ] Unit Test

## Done
- [x] Push to AppCenter
- [x] After categoryDialog opened> if no change : no refresh
- [x] Make LoginAnimation UnStatic and Local to LoginFrame
- [x] `Bug` CategoryDialog Manager Save process, not working!!!
- [x] CategoryDialog Move Repo
- [x] Change Saving Model for Categories from ID to Name (this way it will be easier to track MoveTo later)
- [x]  Items Width in Stars_GridView have problems
- [x] Fix loadMore30 button or replace it with sth else
- [x] Complete About_Page
- [x] Compressed UI Images  
- [x] CategoryDialog List Handler
- [x] Test Login Speed Again
- [x] Page Save State
- [x] Set Icon
- [x] Context Menu for StarItem in MainPage
- [x] Change Checkboxes to Bulletpoint
- [x] Add Card to http://shahriar.in/app
- [x] CategoryDialog Design
- [x] loading stuff
- [x] caching stuff
- [x] md button bar events
- [x] md Cache
- [x] Fix 403 forbidden response
- [x] Main Xaml Design
- [x] Main Api Design