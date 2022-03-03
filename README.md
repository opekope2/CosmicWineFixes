# SE Clipboard Fix

## Notes about template modifications
* I converted the csproj files to the new SDK-style csproj, so you can build it with the new .NET SDK
* I removed the dedicated and torch plugins, because they are not needed

I'll publish an updated Linux-compatible template soon

## How to build (Windows) (not tested)
* Install [.NET SDK](https://get.dot.net)
* Open `Setup-links.bat` and edit SE Bin64 path
* Run `Setup-links.bat`
* To build from command line, run `dotnet build`

(Your IDE should be able to build the solution)

## How to build (GNU+Linux)
* Install [.NET SDK](https://get.dot.net)
* Symlink SE's Bin64 folder to project root (`ln -s /path/to/SteamLibrary/steamapps/common/SpaceEngineers/Bin64 Bin64`)
* Run `dotnet build`

## The clipboard issue

This code in SE is causing the issue:

```csharp
public static void SetClipboard(string text)
{
    Thread thread = new Thread(delegate(object arg)
    {
        for (int i = 0; i < 10; i++)
        {
            try
            {
                Clipboard.Clear();
                Clipboard.SetText((string)arg);
                return;
            }
            catch (ExternalException)
            {
            }
        }
    });
    thread.SetApartmentState(ApartmentState.STA);
    thread.Start(text);
    thread.Join();
}
```

When you press `Ctrl+C` for the second time, the game starts a new thread, which causes the clipboard OLE API to fail for some reason
([.NET Framework loops internally](https://referencesource.microsoft.com/#System.Windows.Forms/winforms/Managed/System/WinForms/Clipboard.cs,171)).
The exception will be thrown 10 times, causing the game to freeze for 10 seconds (.NET Framework delays 10x100ms)

For the conversation about how I found it, see [Keen's discord](https://discord.com/channels/125011928711036928/630756768435142668/946801680664657960)

## How it is fixed

This plugin creates a thread with the same configuration when it gets loaded the thread will run till the game exits. This new thread accepts commands from a queue and executes them. The original method in the game does not get called.
