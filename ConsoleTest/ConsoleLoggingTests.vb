Imports NUnit.Framework

Namespace ConsoleTest

    Public Class ConsoleLoggingTests

        <SetUp>
        Public Sub Setup()
            CompuMaster.Console.Clear(True, True, True, True)
        End Sub

        '<Test> Public Sub TestMustFailForTestingGitHubWorkflowsTestDisplay()
        '    Assert.Fail("This is a demo test failure")
        'End Sub

        '<Test>
        'Public Sub ConsoleColorToCssColor()
        '    Assert.AreEqual("rgb(0,0,0)", CompuMaster.Console.ConsoleColorToCssColor(ConsoleColor.Black))
        '    Assert.AreEqual("rgb(255,255,255)", CompuMaster.Console.ConsoleColorToCssColor(ConsoleColor.White))
        '    Assert.AreEqual("rgb(255,0,0)", CompuMaster.Console.ConsoleColorToCssColor(ConsoleColor.Red))
        '    Assert.AreEqual("rgb(0,255,0)", CompuMaster.Console.ConsoleColorToCssColor(ConsoleColor.Green))
        '    Assert.AreEqual("rgb(0,0,255)", CompuMaster.Console.ConsoleColorToCssColor(ConsoleColor.Blue))
        'End Sub

        <Test>
        Public Sub LogAndWarningsOutput()
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

            'Compare results
            Dim Expected As String
            Dim Result As String

            Console.WriteLine()
            Console.WriteLine("## Sample Warnings (Plain Text)")
            Console.WriteLine()
            Result = System.IO.File.ReadAllText("sample-warnings.txt")
            Expected =
                "WARNING: Now, let's see the indentation feature" & System.Environment.NewLine
            Console.WriteLine("EXPECTED: " & Expected)
            Console.WriteLine("RESULT:   " & Result)
            Assert.AreEqual(Expected, Result)

            Console.WriteLine()
            Console.WriteLine("## Sample Log (Plain Text)")
            Console.WriteLine()
            Result = System.IO.File.ReadAllText("sample-log.txt")
            Expected =
                "Hello World!" & System.Environment.NewLine &
                "This is white text on blue background." & System.Environment.NewLine &
                "This is another yellow text on gray background." & System.Environment.NewLine &
                "" & System.Environment.NewLine &
                "WARNING: Now, let's see the indentation feature" & System.Environment.NewLine &
                "    Okay, let's start indentation: This is a text with 1 levels of indentation" & System.Environment.NewLine &
                "        It's time to continue: This is a text with 2 levels of indentation" & System.Environment.NewLine &
                "        Please note: This line will only appear in log, but with 2 levels of indentation" & System.Environment.NewLine &
                "            Yeah, get some more indentation: This is a text with 3 levels of indentation" & System.Environment.NewLine &
                "    This is a multiline text, line 1" & System.Environment.NewLine &
                "    This is a multiline text, line 2" & System.Environment.NewLine &
                "    This is a multiline text, line 3" & System.Environment.NewLine &
                "    " & System.Environment.NewLine &
                "    This is a multiline text, line 1 of paragraph 2" & System.Environment.NewLine &
                "    This is a multiline text, line 2 of paragraph 2" & System.Environment.NewLine &
                "" & System.Environment.NewLine &
                "OKAY: This nice sample has completed!" & System.Environment.NewLine
            Console.WriteLine("EXPECTED: " & Expected)
            Console.WriteLine("RESULT:   " & Result)
            'WARNING: indentation might not work as expected right now --> TODO
            Assert.AreEqual(Expected, Result)

            Console.WriteLine()
            Console.WriteLine("## Sample Warnings (HTML)")
            Console.WriteLine()
            Result = System.IO.File.ReadAllText("sample-warnings.html")
            Assert.IsTrue(Result.StartsWith("<html>" & System.Environment.NewLine & "<head><title>"))
            Result = Result.Substring(Result.IndexOf("</head>") + "</head>".Length)
            Expected = System.Environment.NewLine &
                "<body style=""background-color: Black;"">" & System.Environment.NewLine &
                "<span style=""color: LightGray;"">WARNING: Now, let&#39;s see the indentation feature<br /></span>" & System.Environment.NewLine &
                "</body></html>"
            Console.WriteLine("EXPECTED: " & Expected)
            Console.WriteLine("RESULT:   " & Result)
            Assert.AreEqual(Expected, Result)

            Console.WriteLine()
            Console.WriteLine("## Sample Log (HTML)")
            Console.WriteLine()
            Result = System.IO.File.ReadAllText("sample-log.html")
            Assert.IsTrue(Result.StartsWith("<html>" & System.Environment.NewLine & "<head><title>"))
            Result = Result.Substring(Result.IndexOf("</head>") + "</head>".Length)
            Expected = System.Environment.NewLine &
                "<body style=""background-color: Black;"">" & System.Environment.NewLine &
                "<span style=""color: LightGray;""><span style=""color: White;"">Hello&nbsp;</span><span style=""color: White;"">World!<br /></span><span style=""background-color: Blue;""><span style=""color: White;"">This&nbsp;is&nbsp;white&nbsp;text&nbsp;on&nbsp;blue&nbsp;background.<br /></span></span><span style=""background-color: DarkGray;""><span style=""color: Yellow;"">This&nbsp;is&nbsp;another&nbsp;yellow&nbsp;text&nbsp;on&nbsp;gray&nbsp;background.<br /></span></span><br /><span style=""color: Red;"">WARNING:&nbsp;Now,&nbsp;let&#39;s&nbsp;see&nbsp;the&nbsp;indentation&nbsp;feature<br /></span>Okay,&nbsp;let&#39;s&nbsp;start&nbsp;indentation:&nbsp;This&nbsp;is&nbsp;a&nbsp;text&nbsp;with&nbsp;1&nbsp;levels&nbsp;of&nbsp;indentation<br />It&#39;s&nbsp;time&nbsp;to&nbsp;continue:&nbsp;This&nbsp;is&nbsp;a&nbsp;text&nbsp;with&nbsp;2&nbsp;levels&nbsp;of&nbsp;indentation<br />Please&nbsp;note:&nbsp;This&nbsp;line&nbsp;will&nbsp;only&nbsp;appear&nbsp;in&nbsp;log,&nbsp;but&nbsp;with&nbsp;2&nbsp;levels&nbsp;of&nbsp;indentation<br />Yeah,&nbsp;get&nbsp;some&nbsp;more&nbsp;indentation:&nbsp;This&nbsp;is&nbsp;a&nbsp;text&nbsp;with&nbsp;3&nbsp;levels&nbsp;of&nbsp;indentation<br />This&nbsp;is&nbsp;a&nbsp;multiline&nbsp;text,&nbsp;line&nbsp;1<br />&nbsp;&nbsp;&nbsp;&nbsp;This&nbsp;is&nbsp;a&nbsp;multiline&nbsp;text,&nbsp;line&nbsp;2<br />&nbsp;&nbsp;&nbsp;&nbsp;This&nbsp;is&nbsp;a&nbsp;multiline&nbsp;text,&nbsp;line&nbsp;3<br />&nbsp;&nbsp;&nbsp;&nbsp;<br />&nbsp;&nbsp;&nbsp;&nbsp;This&nbsp;is&nbsp;a&nbsp;multiline&nbsp;text,&nbsp;line&nbsp;1&nbsp;of&nbsp;paragraph&nbsp;2<br />&nbsp;&nbsp;&nbsp;&nbsp;This&nbsp;is&nbsp;a&nbsp;multiline&nbsp;text,&nbsp;line&nbsp;2&nbsp;of&nbsp;paragraph&nbsp;2<br /><br /><span style=""color: Green;"">OKAY:&nbsp;This&nbsp;nice&nbsp;sample&nbsp;has&nbsp;completed!<br /></span></span>" & System.Environment.NewLine &
                "</body></html>"
            Console.WriteLine("EXPECTED: " & Expected)
            Console.WriteLine("RESULT:   " & Result)
            'Result differs if tests are executed from commandline or from within VS.NET
            'Assert.AreEqual(Expected, Result)
            Assert.Less(Expected.Length, Result.Length * 1.2)
        End Sub

        <Test> Public Sub ConsoleColorsCurrentValues()
            System.Console.WriteLine("System.Console.BackgroundColor=" & System.Console.BackgroundColor.ToString("d") & "=" & System.Console.BackgroundColor.ToString("g"))
            System.Console.WriteLine("System.Console.ForegroundColor=" & System.Console.ForegroundColor.ToString("d") & "=" & System.Console.ForegroundColor.ToString("g"))
            System.Console.WriteLine("CompuMaster.Console.BackgroundColor=" & CompuMaster.Console.BackgroundColor.ToString("d") & "=" & CompuMaster.Console.BackgroundColor.ToString("g"))
            System.Console.WriteLine("CompuMaster.Console.ForegroundColor=" & CompuMaster.Console.ForegroundColor.ToString("d") & "=" & CompuMaster.Console.ForegroundColor.ToString("g"))

            Assert.AreEqual("Black", System.Console.BackgroundColor.ToString("g"))
            Assert.AreEqual("Black", CompuMaster.Console.BackgroundColor.ToString("g"))
            Select Case System.Environment.OSVersion.Platform
                Case PlatformID.Win32NT
                    Assert.AreEqual("Gray", System.Console.ForegroundColor.ToString("g"))
                    If CompuMaster.Console.NoSystemConsoleConnectedOrConsoleIsRedirected_BufferColorChangesByOurself Then
                        Assert.AreEqual("White", CompuMaster.Console.ForegroundColor.ToString("g"))
                    Else
                        Assert.AreEqual("Gray", CompuMaster.Console.ForegroundColor.ToString("g"))
                    End If
                Case Else
                    Assert.AreEqual("White", System.Console.ForegroundColor.ToString("g"))
                    Assert.AreEqual("White", CompuMaster.Console.ForegroundColor.ToString("g"))
            End Select

            CompuMaster.Console.BackgroundColor = ConsoleColor.DarkGray
            CompuMaster.Console.ForegroundColor = ConsoleColor.Yellow

            System.Console.WriteLine("System.Console.BackgroundColor=" & System.Console.BackgroundColor.ToString("d") & "=" & System.Console.BackgroundColor.ToString("g"))
            System.Console.WriteLine("System.Console.ForegroundColor=" & System.Console.ForegroundColor.ToString("d") & "=" & System.Console.ForegroundColor.ToString("g"))
            System.Console.WriteLine("CompuMaster.Console.BackgroundColor=" & CompuMaster.Console.BackgroundColor.ToString("d") & "=" & CompuMaster.Console.BackgroundColor.ToString("g"))
            System.Console.WriteLine("CompuMaster.Console.ForegroundColor=" & CompuMaster.Console.ForegroundColor.ToString("d") & "=" & CompuMaster.Console.ForegroundColor.ToString("g"))

            Assert.AreEqual("DarkGray", CompuMaster.Console.BackgroundColor.ToString("g"))
            Assert.AreEqual("Yellow", CompuMaster.Console.ForegroundColor.ToString("g"))

        End Sub

        <Test> Public Sub ConsoleColorSystemName()
            Dim MyCounter As Integer = -1
            For Each Color As System.ConsoleColor In [Enum].GetValues(GetType(System.ConsoleColor))
                MyCounter += 1
                Dim ColorName As String = CompuMaster.Console.ConsoleColorSystemName(Color)
                System.Console.WriteLine(Color.ToString("d") & "=" & ColorName & "=" & Color.ToString("g"))
                Dim ExpectedColorName As String
                Select Case MyCounter
                    Case 0 : ExpectedColorName = "Black"
                    Case 1 : ExpectedColorName = "DarkBlue"
                    Case 2 : ExpectedColorName = "DarkGreen"
                    Case 3 : ExpectedColorName = "DarkCyan"
                    Case 4 : ExpectedColorName = "DarkRed"
                    Case 5 : ExpectedColorName = "DarkMagenta"
                    Case 6 : ExpectedColorName = "DarkYellow"
                    Case 7 : ExpectedColorName = "Gray"
                    Case 8 : ExpectedColorName = "DarkGray"
                    Case 9 : ExpectedColorName = "Blue"
                    Case 10 : ExpectedColorName = "Green"
                    Case 11 : ExpectedColorName = "Cyan"
                    Case 12 : ExpectedColorName = "Red"
                    Case 13 : ExpectedColorName = "Magenta"
                    Case 14 : ExpectedColorName = "Yellow"
                    Case 15 : ExpectedColorName = "White"
                    Case Else
                        Throw New NotImplementedException
                End Select
                Assert.AreEqual(MyCounter.ToString, Color.ToString("d"))
                Assert.AreEqual(ExpectedColorName, Color.ToString("g"))
                Assert.AreEqual(ExpectedColorName, ColorName)
            Next
        End Sub

        <Test> Public Sub ConsoleColorCssName()
            Dim MyCounter As Integer = -1
            For Each Color As System.ConsoleColor In [Enum].GetValues(GetType(System.ConsoleColor))
                MyCounter += 1
                Dim ColorName As String = CompuMaster.Console.ConsoleColorCssName(Color)
                Dim ExpectedColorName As String
                Select Case MyCounter
                    Case 0 : ExpectedColorName = "Black"
                    Case 1 : ExpectedColorName = "DarkBlue"
                    Case 2 : ExpectedColorName = "DarkGreen"
                    Case 3 : ExpectedColorName = "DarkCyan"
                    Case 4 : ExpectedColorName = "DarkRed"
                    Case 5 : ExpectedColorName = "DarkMagenta"
                    Case 6 : ExpectedColorName = "DarkYellow"
                    Case 7 : ExpectedColorName = "LightGray"
                    Case 8 : ExpectedColorName = "DarkGray"
                    Case 9 : ExpectedColorName = "Blue"
                    Case 10 : ExpectedColorName = "Green"
                    Case 11 : ExpectedColorName = "Cyan"
                    Case 12 : ExpectedColorName = "Red"
                    Case 13 : ExpectedColorName = "Magenta"
                    Case 14 : ExpectedColorName = "Yellow"
                    Case 15 : ExpectedColorName = "White"
                    Case Else
                        Throw New NotImplementedException
                End Select
                Assert.AreEqual(MyCounter.ToString, Color.ToString("d"))
                Assert.AreEqual(ExpectedColorName, ColorName)
            Next
        End Sub

    End Class

End Namespace