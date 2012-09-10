What's Shaking, NZ? for Windows Phone
=================

To run this app, you need the Windows Phone 7.1 SDK, NuGet and a Bing Maps API Key.

## Windows Phone SDK

You can get the Windows Phone 7.1 SDK here:
http://www.microsoft.com/en-us/download/details.aspx?id=27570

## Nuget

You can get NuGet here:
http://nuget.codeplex.com/

If you have an older version of NuGet installed, it may not support the Package Restore option which enables NuGet to automatically download packages. Since the Windows Phone Tools don't have support for the Visual Studio Extension Manager, you will have to manually uninstall NuGet and then install the latest version.

This is simple enough. You can run this command:
    vsixinstaller.exe /uninstall:NuPackToolsVsix.Microsoft.67e54e40-0ae3-42c5-a949-fddf5739e7a5
	
VSIXInstaller.exe is located at C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE

This will remove NuGet, and you can then install the latest version. Make sure you go to Tools -> Options -> Package Manager and check 'Allow NuGet to download missing packages during build.'

## Bing Maps

You can sign up for a Bing Maps API Key here:
http://www.microsoft.com/maps/developers/mobile.aspx

The Bing Maps API Key is required to use the Maps controls. You simply put your key into the QuakeDisplayPage.xaml file and the MapPage.xaml file:

```
<maps:Map Grid.Row="1" Margin="4,6,4,4" x:Name="QuakeMap" 
	CredentialsProvider="your_maps_api_key_here" 
	... />
```