#WebExpress beta
This is advanced internet browser created in C# WPF. Currently WebExpress is in beta, so there aren't any features such as history, bookmarks or extensions, but the features will be added. This also have some bugs and they also will be removed.

#WebExpress will be better
We are rewriting WebExpress to C# in WPF from scratch using CefSharp. It will have more features and it will faster.

#Features

* Colored tabs
* History
* Suggestions
* Menu
* Downloads
* Context menu
* Incognito
* New window
* Fullscreen
* Screenshot function
* News
* Weather
* Settings
* Material Design UI
* Bookmarks
* Extensions
* Developer tools

#Engine
This browser is using CefSharp. You can find it on: https://github.com/cefsharp/CefSharp

#Extensions
  Files structure
  Extensions
  * name.json
  * name.js
  * name.png
  
  name.json:
  ```
  {
    "id":"name",
    "description":"description",
    "logo":"name.png",
    "scripts": [
        {"file":"name.js"}
    ]
  }
  ```

#Authors
This code was created by Eryk Rakowski (Sential)

