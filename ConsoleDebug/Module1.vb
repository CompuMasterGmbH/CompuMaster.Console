Module Module1

    Sub Main()
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

        System.Diagnostics.Process.Start("sample-log.html")

        System.Console.WriteLine()
        System.Console.WriteLine()
        System.Console.WriteLine()
        System.Console.WriteLine("## Review of logged PlainText output")
        System.Console.WriteLine()
        System.Console.WriteLine(CompuMaster.Console.PlainTextLog)
    End Sub

End Module
