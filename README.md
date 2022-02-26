# SE Threading Fixes

## Notes about template modifications
* I converted the csproj files to the new SDK-style csproj so you can build it with the new .NET SDK
* I removed the dedicated and torch plugins, because they are not needed

## How to build (Windows) (not tested)
* Install .NET Core/.NET 5+ SDK
* Run `Edit-and-run-before-opening-solution.bat`
* Open `ClientProject/ClientProject.csproj` and uncomment the line under `<!-- Windows -->`
* Run `dotnet build`

## How to build (GNU+Linux)
* Install .NET Core/.NET 5+ SDK
* Symlink SE's Bin64 folder (`ln -s /path/to/SteamLibrary/steamapps/common/SpaceEngineers/Bin64 Bin64`)
* Open `ClientProject/ClientProject.csproj` and uncomment the line under `<!-- Linux -->`
* Set up `wineprefix-shell` with your paths
* Run `dotnet build`

## Clipboard threading issue

This plugin fixes the bug, when you Ctrl+C for the second time, the game will freeze for 10 seconds, and the text does not get copied.

For details about how I found it, see [Keen's discord](https://discord.com/channels/125011928711036928/630756768435142668/946801680664657960)

I may write a more detailed README later. And an updated Linux-compatibe template 