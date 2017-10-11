# CompuMaster.Console

.NET library CompuMaster.Console which additionally provides logging as plain text and HTML

[![Github Release](https://img.shields.io/github/release/CompuMasterGmbH/CompuMaster.Console.svg?maxAge=2592000&label=GitHub%20Release)](https://github.com/CompuMasterGmbH/CompuMaster.Console/releases) 
[![NuGet CompuMaster.Data](https://img.shields.io/nuget/v/CompuMaster.Console.svg?maxAge=2592000&label=NuGet%20CM.Console)](https://www.nuget.org/packages/CompuMaster.Console/) 

## Simple download/installation using NuGet
```powershell
Install-Package CompuMaster.Console
```

## Simple use
* Just use CompuMaster.Console instead of System.Console
* At any time when required, write a full log into a variable or file on disk
```vb.net
'Create some wonderful output to the console
CompuMaster.Console.Write("Hello ")
CompuMaster.Console.WriteLine("World!")

CompuMaster.Console.ForegroundColor = System.ConsoleColor.Blue
CompuMaster.Console.BackgroundColor = System.ConsoleColor.White
CompuMaster.Console.WriteLine("This is white text on blue background.")
CompuMaster.Console.WriteLine("This is another yellow text on brown background.", System.ConsoleColor.Yellow, System.ConsoleColor.Brown)

CompuMaster.Console.Warn("WARNING: ")
CompuMaster.Console.OkayLine("This nice sample has completed!")

'Write log to disk
CompuMaster.Console.SavePlainTextLog("sample-log.txt")
CompuMaster.Console.SaveHtmlLog("sample-log.html")
```
