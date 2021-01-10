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

        CompuMaster.Console.OkayLine("OKAY: This nice sample has completed!")

        'Write log to disk
        CompuMaster.Console.SavePlainTextLog("sample-log.txt")
        CompuMaster.Console.SaveHtmlLog("sample-log.html")
        CompuMaster.Console.SaveWarningsPlainTextLog("sample-warnings.txt")
        CompuMaster.Console.SaveWarningsHtmlLog("sample-warnings.html")

        System.Diagnostics.Process.Start("sample-log.html")
    End Sub

End Module
