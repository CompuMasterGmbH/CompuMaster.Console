Module Module1

    Sub Main()
        'Create some wonderful output to the console
        CompuMaster.Console.Write("Hello ")
        CompuMaster.Console.WriteLine("World!")

        CompuMaster.Console.ForegroundColor = System.ConsoleColor.White
        CompuMaster.Console.BackgroundColor = System.ConsoleColor.Blue
        CompuMaster.Console.WriteLine("This is white text on blue background.")
        CompuMaster.Console.WriteLine("This is another yellow text on gray background.", System.ConsoleColor.Yellow, System.ConsoleColor.DarkGray)
        CompuMaster.Console.ResetColor()

        CompuMaster.Console.Warn("WARNING: ")
        CompuMaster.Console.OkayLine("This nice sample has completed!")

        'Write log to disk
        CompuMaster.Console.SavePlainTextLog("sample-log.txt")
        CompuMaster.Console.SaveHtmlLog("sample-log.html")
    End Sub

End Module
