# CompuMaster.Console

.NET library CompuMaster.Console which additionally provides logging as plain text and HTML

[![Github Release](https://img.shields.io/github/release/CompuMasterGmbH/CompuMaster.Console.svg?maxAge=592000&label=GitHub%20Release)](https://github.com/CompuMasterGmbH/CompuMaster.Console/releases) 
[![NuGet CompuMaster.Data](https://img.shields.io/nuget/v/CompuMaster.Console.svg?maxAge=2592000&label=NuGet%20CM.Console)](https://www.nuget.org/packages/CompuMaster.Console/) 

## Simple download/installation using NuGet
```powershell
Install-Package CompuMaster.Console
```

## Simple use
* Just use CompuMaster.Console instead of System.Console
* At any time when required, write a full log into a variable or file on disk
* Supports text indentation with multiple indentation levels
* Use with plain text and/or HTML

## Sample output

### Classic console
![Console output](https://user-images.githubusercontent.com/3033827/180450273-9743e9eb-1149-42d8-92eb-6b11216e357c.png)

### HTML document
![HTML log output](https://user-images.githubusercontent.com/3033827/180450443-89c9db06-15cd-4268-88e4-b32fdf518ab8.png)

### Plain text log
![Plain text log output](https://user-images.githubusercontent.com/3033827/180452228-c060dce3-2aee-450c-ae38-3b011592902a.png)

## Source code for output above 

### C# 
```C#
// Create some wonderful output to the console
CompuMaster.Console.Write("Hello ");
CompuMaster.Console.WriteLine("World!");

CompuMaster.Console.ForegroundColor = System.ConsoleColor.White;
CompuMaster.Console.BackgroundColor = System.ConsoleColor.Blue;
CompuMaster.Console.WriteLine("This is white text on blue background.");
CompuMaster.Console.WriteLine("This is another yellow text on gray background.", System.ConsoleColor.Yellow, System.ConsoleColor.DarkGray);
CompuMaster.Console.WriteLine();
CompuMaster.Console.ResetColor();

CompuMaster.Console.WarnLine("WARNING: Now, let's see the indentation feature");
CompuMaster.Console.CurrentIndentationLevel += 1;
CompuMaster.Console.Write("Okay, let's start indentation: ");
CompuMaster.Console.WriteLine("This is a text with " + CompuMaster.Console.CurrentIndentationLevel + " levels of indentation");
CompuMaster.Console.CurrentIndentationLevel += 1;
CompuMaster.Console.Write("It's time to continue: ");
CompuMaster.Console.WriteLine("This is a text with " + CompuMaster.Console.CurrentIndentationLevel + " levels of indentation");
CompuMaster.Console.Log("Please note: ");
CompuMaster.Console.LogLine("This line will only appear in log, but with " + CompuMaster.Console.CurrentIndentationLevel + " levels of indentation");
CompuMaster.Console.CurrentIndentationLevel += 1;
CompuMaster.Console.Write("Yeah, get some more indentation: ");
CompuMaster.Console.WriteLine("This is a text with " + CompuMaster.Console.CurrentIndentationLevel + " levels of indentation");
CompuMaster.Console.CurrentIndentationLevel = 1;
CompuMaster.Console.WriteLine("This is a multiline text, line 1" + ControlChars.CrLf + "This is a multiline text, line 2" + ControlChars.Cr + "This is a multiline text, line 3" + ControlChars.Lf + ControlChars.CrLf + "This is a multiline text, line 1 of paragraph 2" + ControlChars.CrLf + "This is a multiline text, line 2 of paragraph 2");
CompuMaster.Console.WriteLine();
CompuMaster.Console.CurrentIndentationLevel = 0;

CompuMaster.Console.WriteDual("Now, let's write plain text tables to console + plaint text log, but write nicely formatted table to HTML log" + System.Environment.NewLine, "<h2>Now, let's write plain text tables to console + plaint text log, but write nicely formatted table to HTML log</h2>");
DataTable DemoTable = CompuMaster.Data.Csv.ReadDataTableFromCsvString("Key,Description,DemoData" + ControlChars.CrLf + "Row 1,Hello world!,You'll see nicely formatted plain text at console and plain text log" + ControlChars.CrLf + "Row 2,Great news!,This table will be formatted very nicely at HTML log", true, ',', '"', false, true);
DemoTable.TableName = "Pretty demo table";
CompuMaster.Console.LogDual("", "<style>" + ControlChars.CrLf + "    tbody tr:nth-child(odd){background-color: #3c3c3c;}; " + ControlChars.CrLf + "    tbody td{padding: 30px;}" + ControlChars.CrLf + "</style>");
CompuMaster.Console.WriteLineDual(CompuMaster.Data.DataTables.ConvertToPlainTextTableFixedColumnWidths(DemoTable), CompuMaster.Data.DataTables.ConvertToHtmlTable(DemoTable, "<h3>", "</h3>", "style=\"width: 100%\""));
CompuMaster.Console.WriteLineDual();


CompuMaster.Console.OkayLine("OKAY: This nice sample has completed!");

// Write log to disk
CompuMaster.Console.SavePlainTextLog("sample-log.txt");
CompuMaster.Console.SaveHtmlLog("sample-log.html");
CompuMaster.Console.SaveWarningsPlainTextLog("sample-warnings.txt");
CompuMaster.Console.SaveWarningsHtmlLog("sample-warnings.html");
```

### VB.NET 

```vb.net
 'Create some wonderful output to the console
CompuMaster.Console.Write("Hello ")
CompuMaster.Console.WriteLine("World!")

CompuMaster.Console.ForegroundColor = System.ConsoleColor.White
CompuMaster.Console.BackgroundColor = System.ConsoleColor.Blue
CompuMaster.Console.WriteLine("This is white text on blue background.")
CompuMaster.Console.WriteLine("This is another yellow text on gray background.", System.ConsoleColor.Yellow, System.ConsoleColor.DarkGray)
CompuMaster.Console.WriteLine()
CompuMaster.Console.ResetColor()

CompuMaster.Console.WarnLine("WARNING: Now, let's see the indentation feature")
CompuMaster.Console.CurrentIndentationLevel += 1
CompuMaster.Console.Write("Okay, let's start indentation: ")
CompuMaster.Console.WriteLine("This is a text with " & CompuMaster.Console.CurrentIndentationLevel & " levels of indentation")
CompuMaster.Console.CurrentIndentationLevel += 1
CompuMaster.Console.Write("It's time to continue: ")
CompuMaster.Console.WriteLine("This is a text with " & CompuMaster.Console.CurrentIndentationLevel & " levels of indentation")
CompuMaster.Console.Log("Please note: ")
CompuMaster.Console.LogLine("This line will only appear in log, but with " & CompuMaster.Console.CurrentIndentationLevel & " levels of indentation")
CompuMaster.Console.CurrentIndentationLevel += 1
CompuMaster.Console.Write("Yeah, get some more indentation: ")
CompuMaster.Console.WriteLine("This is a text with " & CompuMaster.Console.CurrentIndentationLevel & " levels of indentation")
CompuMaster.Console.CurrentIndentationLevel = 1
CompuMaster.Console.WriteLine("This is a multiline text, line 1" & ControlChars.CrLf & "This is a multiline text, line 2" & ControlChars.Cr & "This is a multiline text, line 3" & ControlChars.Lf & ControlChars.CrLf & "This is a multiline text, line 1 of paragraph 2" & ControlChars.CrLf & "This is a multiline text, line 2 of paragraph 2")
CompuMaster.Console.WriteLine()
CompuMaster.Console.CurrentIndentationLevel = 0

CompuMaster.Console.WriteDual("Now, let's write plain text tables to console + plaint text log, but write nicely formatted table to HTML log" & System.Environment.NewLine,
    "<h2>Now, let's write plain text tables to console + plaint text log, but write nicely formatted table to HTML log</h2>")
Dim DemoTable As DataTable = CompuMaster.Data.Csv.ReadDataTableFromCsvString("Key,Description,DemoData" & ControlChars.CrLf &
                                                                             "Row 1,Hello world!,You'll see nicely formatted plain text at console and plain text log" & ControlChars.CrLf &
                                                                             "Row 2,Great news!,This table will be formatted very nicely at HTML log", True, ","c, """"c, False, True)
DemoTable.TableName = "Pretty demo table"
CompuMaster.Console.LogDual("",
                            "<style>" & ControlChars.CrLf &
                            "    tbody tr:nth-child(odd){background-color: #3c3c3c;}; " & ControlChars.CrLf &
                            "    tbody td{padding: 30px;}" & ControlChars.CrLf &
                            "</style>")
CompuMaster.Console.WriteLineDual(CompuMaster.Data.DataTables.ConvertToPlainTextTableFixedColumnWidths(DemoTable),
                                  CompuMaster.Data.DataTables.ConvertToHtmlTable(DemoTable, "<h3>", "</h3>", "style=""width: 100%"""))
CompuMaster.Console.WriteLineDual()


CompuMaster.Console.OkayLine("OKAY: This nice sample has completed!")

'Write log to disk
CompuMaster.Console.SavePlainTextLog("sample-log.txt")
CompuMaster.Console.SaveHtmlLog("sample-log.html")
CompuMaster.Console.SaveWarningsPlainTextLog("sample-warnings.txt")
CompuMaster.Console.SaveWarningsHtmlLog("sample-warnings.html")
```
