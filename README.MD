# My.Jdownloader.Api .NET Wrapper

This is a simple wrapper around the MyJDownloader API.

This started as a fork of https://github.com/Cr1TiKa7/My.Jdownloader.Api, which was 
based on https://github.com/noone2407/MyJDownloaderApi , but became more independent.


Currently it supports: .NET 4.0, .NET 4.5, .NET 4.6 and .NETCore 2.0

> Feel free to request missing calls, bugs or suggestions for improvements.

#### Usage

First of all you need an instance of the JDownloaderHandler
``` c#
JDownloaderHandler _jdownloaderHandler = new JDownloaderHandler();
_jdownloaderHandler.Connect("YOURMYJDOWNLOADERUSERNAME", "YOURMYJDOWNLOADERPASSWORD");
//Alternaitve
JDownloaderHandler _jdownloaderHandler = new JDownloaderHandler("YOURMYJDOWNLOADERUSERNAME", "YOURMYJDOWNLOADERPASSWORD");
```

The JDownloaderHandler contains the following methods:
- Connect(email, password) : boolean
- Reconnect() : boolean
- Disconnect() : boolean
- GetDevices() : List<DeviceObject>

#### Namespaces

For a better overview, the calls are split into different namespaces.
So far the library contains the following namespaces and methods:

- AccountsV2 | 14/14 calls done
  
- DownloadController | 6/6 calls done

- DownloadsV2 | 2/x calls done
	
- Extensions | 5/5 calls done

- Extraction | 6/7 calls done

- JD | 6/6 calls done
	
- LinkCrawler | 1/1 calls done
    
- LinkGrabberV2 | 19/30 calls done
	
- System | 7/7 calls done

- Update | 3/3 calls done

#### Samples

Here is a simple sample code which shows you how to connect, get a device handler and how to call a method:

``` c#
JDownloaderHandler _JDownloaderHandler = new JDownloaderHandler("YOUREMAIL","YOURPASSWORD");
if (_JDownloaderHandler.IsConnected)
{
    var devices = _JDownloaderHandler.GetDevices();
    if (devices.Count = 0)
    return;
    
    var deviceHandler = _JDownloaderHandler.GetDeviceInstance(devices[0]);
    deviceHandler.AccountV2.AddAccount("mega.co.nz", "test123", "test123");
}
``` 

#### Thanks to
Newtonsoft.Json
Fody
