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
            LogAndWarningsOutput(False)
        End Sub

        <Test>
        Public Sub LogAndWarningsOutput_WithExactHtmlComparison()
            LogAndWarningsOutput(True)
        End Sub

        Public Sub LogAndWarningsOutput(compareHtml As Boolean)
            CompuMaster.Console.ResetColor()
            If compareHtml Then
                CompuMaster.Console.SystemColorsInLogs = CompuMaster.Console.SystemColorModesInLogs.AlwaysApplySystemColorInLog
            End If

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
            Result = System.IO.File.ReadAllText("sample-warnings.txt")
            Expected =
                "WARNING: Now, let's see the indentation feature" & System.Environment.NewLine
            Console.WriteLine()
            Console.WriteLine("### EXPECTED: ")
            Console.WriteLine(Expected)
            Console.WriteLine()
            Console.WriteLine("### RESULT:   ")
            Console.WriteLine(Result)
            Assert.AreEqual(Expected, Result)

            Console.WriteLine()
            Console.WriteLine("## Sample Log (Plain Text)")
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
            Console.WriteLine()
            Console.WriteLine("### EXPECTED: ")
            Console.WriteLine(Expected)
            Console.WriteLine()
            Console.WriteLine("### RESULT:   ")
            Console.WriteLine(Result)
            'WARNING: indentation might not work as expected right now --> TODO
            Assert.AreEqual(Expected, Result)

            Console.WriteLine()
            Console.WriteLine("## Sample Warnings (HTML)")
            Result = System.IO.File.ReadAllText("sample-warnings.html")
            Assert.IsTrue(Result.StartsWith("<html>" & System.Environment.NewLine & "<head><title>"))
            Result = Result.Substring(Result.IndexOf("</head>") + "</head>".Length)
            Expected = System.Environment.NewLine &
                "<body style=""background-color: Black; color: LightGray;"">" & System.Environment.NewLine &
                "<span style=""color: LightGray;"">WARNING: Now, let&#39;s see the indentation feature<br /></span>" & System.Environment.NewLine &
                "</body></html>"
            Console.WriteLine()
            Console.WriteLine("### EXPECTED: ")
            Console.WriteLine(Expected)
            Console.WriteLine()
            Console.WriteLine("### RESULT:   ")
            Console.WriteLine(Result)
            Assert.AreEqual(Expected, Result)

            Console.WriteLine()
            Console.WriteLine("## Sample Log (HTML)")
            Result = System.IO.File.ReadAllText("sample-log.html")
            Assert.IsTrue(Result.StartsWith("<html>" & System.Environment.NewLine & "<head><title>"))
            Result = Result.Substring(Result.IndexOf("</head>") + "</head>".Length)
            Expected = System.Environment.NewLine &
                "<body style=""background-color: Black; color: LightGray;"">" & System.Environment.NewLine &
                "<span style=""color: LightGray;""><span style=""color: " & CompuMaster.Console.ConsoleColorCssName(CompuMaster.Console.SystemConsoleDefaultForegroundColor) & ";"">Hello&nbsp;</span><span style=""color: " & CompuMaster.Console.ConsoleColorCssName(CompuMaster.Console.SystemConsoleDefaultForegroundColor) & ";"">World!<br /></span><span style=""background-color: Blue;""><span style=""color: White;"">This&nbsp;is&nbsp;white&nbsp;text&nbsp;on&nbsp;blue&nbsp;background.<br /></span></span><span style=""background-color: DarkGray;""><span style=""color: Yellow;"">This&nbsp;is&nbsp;another&nbsp;yellow&nbsp;text&nbsp;on&nbsp;gray&nbsp;background.<br /></span></span><br /><span style=""color: Red;"">WARNING:&nbsp;Now,&nbsp;let&#39;s&nbsp;see&nbsp;the&nbsp;indentation&nbsp;feature<br /></span><span style=""color: " & CompuMaster.Console.ConsoleColorCssName(CompuMaster.Console.SystemConsoleDefaultForegroundColor) & ";"">&nbsp;&nbsp;&nbsp;&nbsp;Okay,&nbsp;let&#39;s&nbsp;start&nbsp;indentation:&nbsp;</span><span style=""color: " & CompuMaster.Console.ConsoleColorCssName(CompuMaster.Console.SystemConsoleDefaultForegroundColor) & ";"">This&nbsp;is&nbsp;a&nbsp;text&nbsp;with&nbsp;1&nbsp;levels&nbsp;of&nbsp;indentation<br /></span><span style=""color: " & CompuMaster.Console.ConsoleColorCssName(CompuMaster.Console.SystemConsoleDefaultForegroundColor) & ";"">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;It&#39;s&nbsp;time&nbsp;to&nbsp;continue:&nbsp;</span><span style=""color: " & CompuMaster.Console.ConsoleColorCssName(CompuMaster.Console.SystemConsoleDefaultForegroundColor) & ";"">This&nbsp;is&nbsp;a&nbsp;text&nbsp;with&nbsp;2&nbsp;levels&nbsp;of&nbsp;indentation<br /></span><span style=""color: " & CompuMaster.Console.ConsoleColorCssName(CompuMaster.Console.SystemConsoleDefaultForegroundColor) & ";"">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Please&nbsp;note:&nbsp;</span><span style=""color: " & CompuMaster.Console.ConsoleColorCssName(CompuMaster.Console.SystemConsoleDefaultForegroundColor) & ";"">This&nbsp;line&nbsp;will&nbsp;only&nbsp;appear&nbsp;in&nbsp;log,&nbsp;but&nbsp;with&nbsp;2&nbsp;levels&nbsp;of&nbsp;indentation<br /></span><span style=""color: " & CompuMaster.Console.ConsoleColorCssName(CompuMaster.Console.SystemConsoleDefaultForegroundColor) & ";"">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Yeah,&nbsp;get&nbsp;some&nbsp;more&nbsp;indentation:&nbsp;</span><span style=""color: " & CompuMaster.Console.ConsoleColorCssName(CompuMaster.Console.SystemConsoleDefaultForegroundColor) & ";"">This&nbsp;is&nbsp;a&nbsp;text&nbsp;with&nbsp;3&nbsp;levels&nbsp;of&nbsp;indentation<br /></span><span style=""color: " & CompuMaster.Console.ConsoleColorCssName(CompuMaster.Console.SystemConsoleDefaultForegroundColor) & ";"">&nbsp;&nbsp;&nbsp;&nbsp;This&nbsp;is&nbsp;a&nbsp;multiline&nbsp;text,&nbsp;line&nbsp;1<br />&nbsp;&nbsp;&nbsp;&nbsp;This&nbsp;is&nbsp;a&nbsp;multiline&nbsp;text,&nbsp;line&nbsp;2<br />&nbsp;&nbsp;&nbsp;&nbsp;This&nbsp;is&nbsp;a&nbsp;multiline&nbsp;text,&nbsp;line&nbsp;3<br />&nbsp;&nbsp;&nbsp;&nbsp;<br />&nbsp;&nbsp;&nbsp;&nbsp;This&nbsp;is&nbsp;a&nbsp;multiline&nbsp;text,&nbsp;line&nbsp;1&nbsp;of&nbsp;paragraph&nbsp;2<br />&nbsp;&nbsp;&nbsp;&nbsp;This&nbsp;is&nbsp;a&nbsp;multiline&nbsp;text,&nbsp;line&nbsp;2&nbsp;of&nbsp;paragraph&nbsp;2<br /></span><br /><span style=""color: Green;"">OKAY:&nbsp;This&nbsp;nice&nbsp;sample&nbsp;has&nbsp;completed!<br /></span></span>" & System.Environment.NewLine &
                "</body></html>"
            Console.WriteLine()
            Console.WriteLine("### EXPECTED: ")
            Console.WriteLine(Expected)
            Console.WriteLine()
            Console.WriteLine("### RESULT:   ")
            Console.WriteLine(Result)
            'Result differs if tests are executed from commandline or from within VS.NET (console default colors might differ, resulting in different span tags for colors)
            If compareHtml Then
                Assert.AreEqual(Expected, Result)
            End If
            Assert.Less(Expected.Length, Result.Length * 1.3)
        End Sub

        <Test>
        Public Sub LogAndWarningsOutputDual()
            LogAndWarningsOutputDual(False)
        End Sub

        <Test>
        Public Sub LogAndWarningsOutputDual_WithExactHtmlComparison()
            LogAndWarningsOutputDual(True)
        End Sub

        Private Sub LogAndWarningsOutputDual(compareHtml As Boolean)
            CompuMaster.Console.ResetColor()
            If compareHtml Then
                CompuMaster.Console.SystemColorsInLogs = CompuMaster.Console.SystemColorModesInLogs.AlwaysApplySystemColorInLog
            End If

            'Create some wonderful output to the console
            CompuMaster.Console.WriteDual("T:Hello ", "H:Hello ")
            CompuMaster.Console.WriteLineDual("T:World!", "H:World!")

            CompuMaster.Console.ForegroundColor = System.ConsoleColor.White
            CompuMaster.Console.BackgroundColor = System.ConsoleColor.Blue
            CompuMaster.Console.WriteLineDual("T:This is white text on blue background.", "H:This is white text on blue background.")
            CompuMaster.Console.WriteLineDual("T:This is another yellow text on gray background.", "H:This is another yellow text on gray background.", System.ConsoleColor.Yellow, System.ConsoleColor.DarkGray)
            CompuMaster.Console.WriteLineDual()
            CompuMaster.Console.ResetColor()

            CompuMaster.Console.WarnLineDual("T:WARNING: Now, let's see the indentation feature", "H:WARNING: Now, let's see the indentation feature")
            CompuMaster.Console.CurrentIndentationLevel += 1
            CompuMaster.Console.WriteDual("T:Okay, let's start indentation: ", "H:Okay, let's start indentation: ")
            CompuMaster.Console.WriteLineDual("T:This is a text with " & CompuMaster.Console.CurrentIndentationLevel & " levels of indentation", "H:This is a text with " & CompuMaster.Console.CurrentIndentationLevel & " levels of indentation")
            CompuMaster.Console.CurrentIndentationLevel += 1
            CompuMaster.Console.WriteDual("T:It's time to continue: ", "H:It's time to continue: ")
            CompuMaster.Console.WriteLineDual("T:This is a text with " & CompuMaster.Console.CurrentIndentationLevel & " levels of indentation", "H:This is a text with " & CompuMaster.Console.CurrentIndentationLevel & " levels of indentation")
            CompuMaster.Console.LogDual("T:Please note: ", "H:Please note: ")
            CompuMaster.Console.LogLineDual("T:This line will only appear in log, but with " & CompuMaster.Console.CurrentIndentationLevel & " levels of indentation", "H:This line will only appear in log, but with " & CompuMaster.Console.CurrentIndentationLevel & " levels of indentation")
            CompuMaster.Console.CurrentIndentationLevel += 1
            CompuMaster.Console.WriteDual("T:Yeah, get some more indentation: ", "H:Yeah, get some more indentation: ")
            CompuMaster.Console.WriteLineDual("T:This is a text with " & CompuMaster.Console.CurrentIndentationLevel & " levels of indentation", "H:This is a text with " & CompuMaster.Console.CurrentIndentationLevel & " levels of indentation")
            CompuMaster.Console.CurrentIndentationLevel = 1
            CompuMaster.Console.WriteLineDual("T:This is a multiline text, line 1" & ControlChars.CrLf & "This is a multiline text, line 2" & ControlChars.Cr & "This is a multiline text, line 3" & ControlChars.Lf & ControlChars.CrLf & "This is a multiline text, line 1 of paragraph 2" & ControlChars.CrLf & "This is a multiline text, line 2 of paragraph 2",
                                             ("H:This is a multiline text, line 1" & ControlChars.CrLf & "This is a multiline text, line 2" & ControlChars.Cr & "This is a multiline text, line 3" & ControlChars.Lf & ControlChars.CrLf & "This is a multiline text, line 1 of paragraph 2" & ControlChars.CrLf & "This is a multiline text, line 2 of paragraph 2").Replace(ControlChars.CrLf, "<br />").Replace(ControlChars.Cr, "<br />").Replace(ControlChars.Lf, "<br />"))
            CompuMaster.Console.WriteLineDual()
            CompuMaster.Console.CurrentIndentationLevel = 0

            CompuMaster.Console.OkayLineDual("T:OKAY: This nice sample has completed!", "H:OKAY: This nice sample has completed!")

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
            Result = System.IO.File.ReadAllText("sample-warnings.txt")
            Expected =
                "T:WARNING: Now, let's see the indentation feature" & System.Environment.NewLine
            Console.WriteLine()
            Console.WriteLine("### EXPECTED: ")
            Console.WriteLine(Expected)
            Console.WriteLine()
            Console.WriteLine("### RESULT:   ")
            Console.WriteLine(Result)
            Assert.AreEqual(Expected, Result)

            Console.WriteLine()
            Console.WriteLine("## Sample Log (Plain Text)")
            Result = System.IO.File.ReadAllText("sample-log.txt")
            Expected =
                "T:Hello T:World!" & System.Environment.NewLine &
                "T:This is white text on blue background." & System.Environment.NewLine &
                "T:This is another yellow text on gray background." & System.Environment.NewLine &
                "" & System.Environment.NewLine &
                "T:WARNING: Now, let's see the indentation feature" & System.Environment.NewLine &
                "    T:Okay, let's start indentation: T:This is a text with 1 levels of indentation" & System.Environment.NewLine &
                "        T:It's time to continue: T:This is a text with 2 levels of indentation" & System.Environment.NewLine &
                "        T:Please note: T:This line will only appear in log, but with 2 levels of indentation" & System.Environment.NewLine &
                "            T:Yeah, get some more indentation: T:This is a text with 3 levels of indentation" & System.Environment.NewLine &
                "    T:This is a multiline text, line 1" & System.Environment.NewLine &
                "    This is a multiline text, line 2" & System.Environment.NewLine &
                "    This is a multiline text, line 3" & System.Environment.NewLine &
                "    " & System.Environment.NewLine &
                "    This is a multiline text, line 1 of paragraph 2" & System.Environment.NewLine &
                "    This is a multiline text, line 2 of paragraph 2" & System.Environment.NewLine &
                "" & System.Environment.NewLine &
                "T:OKAY: This nice sample has completed!" & System.Environment.NewLine
            Console.WriteLine()
            Console.WriteLine("### EXPECTED: ")
            Console.WriteLine(Expected)
            Console.WriteLine()
            Console.WriteLine("### RESULT:   ")
            Console.WriteLine(Result)
            'WARNING: indentation might not work as expected right now --> TODO
            Assert.AreEqual(Expected, Result)

            Console.WriteLine()
            Console.WriteLine("## Sample Warnings (HTML)")
            Result = System.IO.File.ReadAllText("sample-warnings.html")
            Assert.IsTrue(Result.StartsWith("<html>" & System.Environment.NewLine & "<head><title>"))
            Result = Result.Substring(Result.IndexOf("</head>") + "</head>".Length)
            Expected = System.Environment.NewLine &
                "<body style=""background-color: Black; color: LightGray;"">" & System.Environment.NewLine &
                "<span style=""color: LightGray;"">H:WARNING: Now, let's see the indentation feature<br /></span>" & System.Environment.NewLine &
                "</body></html>"
            Console.WriteLine()
            Console.WriteLine("### EXPECTED: ")
            Console.WriteLine(Expected)
            Console.WriteLine()
            Console.WriteLine("### RESULT:   ")
            Console.WriteLine(Result)
            Assert.AreEqual(Expected, Result)

            Console.WriteLine()
            Console.WriteLine("## Sample Log (HTML)")
            Result = System.IO.File.ReadAllText("sample-log.html")
            Assert.IsTrue(Result.StartsWith("<html>" & System.Environment.NewLine & "<head><title>"))
            Result = Result.Substring(Result.IndexOf("</head>") + "</head>".Length)
            Expected = System.Environment.NewLine &
                "<body style=""background-color: Black; color: LightGray;"">" & System.Environment.NewLine &
                "<span style=""color: LightGray;""><span style=""color: " & CompuMaster.Console.ConsoleColorCssName(CompuMaster.Console.SystemConsoleDefaultForegroundColor) & ";"">H:Hello </span><span style=""color: " & CompuMaster.Console.ConsoleColorCssName(CompuMaster.Console.SystemConsoleDefaultForegroundColor) & ";"">H:World!<br /></span><span style=""background-color: Blue;""><span style=""color: White;"">H:This is white text on blue background.<br /></span></span><span style=""background-color: DarkGray;""><span style=""color: Yellow;"">H:This is another yellow text on gray background.<br /></span></span><br /><span style=""color: Red;"">H:WARNING: Now, let's see the indentation feature<br /></span><span style=""color: " & CompuMaster.Console.ConsoleColorCssName(CompuMaster.Console.SystemConsoleDefaultForegroundColor) & ";"">&nbsp;&nbsp;&nbsp;&nbsp;H:Okay, let's start indentation: </span><span style=""color: " & CompuMaster.Console.ConsoleColorCssName(CompuMaster.Console.SystemConsoleDefaultForegroundColor) & ";"">H:This is a text with 1 levels of indentation<br /></span><span style=""color: " & CompuMaster.Console.ConsoleColorCssName(CompuMaster.Console.SystemConsoleDefaultForegroundColor) & ";"">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;H:It's time to continue: </span><span style=""color: " & CompuMaster.Console.ConsoleColorCssName(CompuMaster.Console.SystemConsoleDefaultForegroundColor) & ";"">H:This is a text with 2 levels of indentation<br /></span><span style=""color: " & CompuMaster.Console.ConsoleColorCssName(CompuMaster.Console.SystemConsoleDefaultForegroundColor) & ";"">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;H:Please note: </span><span style=""color: " & CompuMaster.Console.ConsoleColorCssName(CompuMaster.Console.SystemConsoleDefaultForegroundColor) & ";"">H:This line will only appear in log, but with 2 levels of indentation<br /></span><span style=""color: " & CompuMaster.Console.ConsoleColorCssName(CompuMaster.Console.SystemConsoleDefaultForegroundColor) & ";"">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;H:Yeah, get some more indentation: </span><span style=""color: " & CompuMaster.Console.ConsoleColorCssName(CompuMaster.Console.SystemConsoleDefaultForegroundColor) & ";"">H:This is a text with 3 levels of indentation<br /></span><span style=""color: " & CompuMaster.Console.ConsoleColorCssName(CompuMaster.Console.SystemConsoleDefaultForegroundColor) & ";"">&nbsp;&nbsp;&nbsp;&nbsp;H:This is a multiline text, line 1<br />&nbsp;&nbsp;&nbsp;&nbsp;This is a multiline text, line 2<br />&nbsp;&nbsp;&nbsp;&nbsp;This is a multiline text, line 3<br />&nbsp;&nbsp;&nbsp;&nbsp;<br />&nbsp;&nbsp;&nbsp;&nbsp;This is a multiline text, line 1 of paragraph 2<br />&nbsp;&nbsp;&nbsp;&nbsp;This is a multiline text, line 2 of paragraph 2<br /></span><br /><span style=""color: Green;"">H:OKAY: This nice sample has completed!<br /></span></span>" & System.Environment.NewLine &
                "</body></html>"
            Console.WriteLine()
            Console.WriteLine("### EXPECTED: ")
            Console.WriteLine(Expected)
            Console.WriteLine()
            Console.WriteLine("### RESULT:   ")
            Console.WriteLine(Result)
            Assert.True(Result.Contains("H:"))
            Assert.False(Result.Contains("T:"))
            'Result differs if tests are executed from commandline or from within VS.NET (console default colors might differ, resulting in different span tags for colors)
            If compareHtml Then
                Assert.AreEqual(Expected, Result)
            End If
            Assert.Less(Expected.Length, Result.Length * 1.3)
        End Sub

        <Test> Public Sub ConsoleColorsCurrentValues()
            System.Console.WriteLine("System.Console.BackgroundColor=" & System.Console.BackgroundColor.ToString("d") & "=" & System.Console.BackgroundColor.ToString("g"))
            System.Console.WriteLine("System.Console.ForegroundColor=" & System.Console.ForegroundColor.ToString("d") & "=" & System.Console.ForegroundColor.ToString("g"))
            System.Console.WriteLine("CompuMaster.Console.BackgroundColor=" & CompuMaster.Console.BackgroundColor.ToString("d") & "=" & CompuMaster.Console.BackgroundColor.ToString("g"))
            System.Console.WriteLine("CompuMaster.Console.ForegroundColor=" & CompuMaster.Console.ForegroundColor.ToString("d") & "=" & CompuMaster.Console.ForegroundColor.ToString("g"))

            Select Case System.Environment.OSVersion.Platform
                Case PlatformID.Win32NT
                    Assert.AreEqual("Black", System.Console.BackgroundColor.ToString("g"))
                Case Else
                    Assert.AreEqual("-1", System.Console.BackgroundColor.ToString("g"))
            End Select
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
                    Assert.AreEqual("-1", System.Console.ForegroundColor.ToString("g"))
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

            CompuMaster.Console.ResetColor()

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
                        Assert.AreEqual("Gray", CompuMaster.Console.ForegroundColor.ToString("g"))
                        'Assert.AreEqual("White", CompuMaster.Console.ForegroundColor.ToString("g"))
                    Else
                        Assert.AreEqual("Gray", CompuMaster.Console.ForegroundColor.ToString("g"))
                    End If
                Case Else
                    Assert.AreEqual("Gray", System.Console.ForegroundColor.ToString("g"))
                    Assert.AreEqual("Gray", CompuMaster.Console.ForegroundColor.ToString("g"))
            End Select


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

        <Test> Public Sub DualFeatureStringArgsCompletionTest()
            Dim Text As String = "Plain Text"
            Dim Html As String = "<h2>HTML code</h2>"
            Dim ExpectedTextWithLineBreak As String = Text & System.Environment.NewLine
            Dim ExpectedHtmlWithLineBreak As String = Html & "<br />"

            CompuMaster.Console.Clear(True, True, True, True)
            CompuMaster.Console.WriteLineDual()
            Assert.AreEqual(System.Environment.NewLine, CompuMaster.Console.PlainTextLog.ToString)
            Assert.AreEqual(System.Environment.NewLine, CompuMaster.Console.RawPlainTextLog.ToString)
            Assert.AreEqual("<br />", CompuMaster.Console.HtmlLog.ToString)

            CompuMaster.Console.Clear(True, True, True, True)
            CompuMaster.Console.LogLineDual()
            Assert.AreEqual(System.Environment.NewLine, CompuMaster.Console.PlainTextLog.ToString)
            Assert.AreEqual(System.Environment.NewLine, CompuMaster.Console.RawPlainTextLog.ToString)
            Assert.AreEqual("<br />", CompuMaster.Console.HtmlLog.ToString)

            CompuMaster.Console.Clear(True, True, True, True)
            CompuMaster.Console.WriteDual(Text, Html)
            Assert.AreEqual(Text, CompuMaster.Console.PlainTextLog.ToString)
            Assert.AreEqual(Text, CompuMaster.Console.RawPlainTextLog.ToString)
            Assert.AreEqual(Html, CompuMaster.Console.HtmlLog.ToString)

            CompuMaster.Console.Clear(True, True, True, True)
            CompuMaster.Console.WriteLineDual(Text, Html)
            Assert.AreEqual(ExpectedTextWithLineBreak, CompuMaster.Console.PlainTextLog.ToString)
            Assert.AreEqual(ExpectedTextWithLineBreak, CompuMaster.Console.RawPlainTextLog.ToString)
            Assert.AreEqual(ExpectedHtmlWithLineBreak, CompuMaster.Console.HtmlLog.ToString)

            CompuMaster.Console.Clear(True, True, True, True)
            CompuMaster.Console.WarnDual(Text, Html)
            Assert.AreEqual(Text, CompuMaster.Console.PlainTextLog.ToString)
            Assert.AreEqual("<FORECOLOR:Red>" & Text & "</FORECOLOR:Red>", CompuMaster.Console.RawPlainTextLog.ToString)
            Assert.AreEqual("<span style=""color: Red;"">" & Html & "</span>", CompuMaster.Console.HtmlLog.ToString)

            CompuMaster.Console.Clear(True, True, True, True)
            CompuMaster.Console.WarnLineDual(Text, Html)
            Assert.AreEqual(ExpectedTextWithLineBreak, CompuMaster.Console.PlainTextLog.ToString)
            Assert.AreEqual("<FORECOLOR:Red>" & ExpectedTextWithLineBreak & "</FORECOLOR:Red>", CompuMaster.Console.RawPlainTextLog.ToString)
            Assert.AreEqual("<span style=""color: Red;"">" & ExpectedHtmlWithLineBreak.ToString & "</span>", CompuMaster.Console.HtmlLog.ToString)

            CompuMaster.Console.Clear(True, True, True, True)
            CompuMaster.Console.OkayDual(Text, Html)
            Assert.AreEqual(Text, CompuMaster.Console.PlainTextLog.ToString)
            Assert.AreEqual("<FORECOLOR:Green>" & Text & "</FORECOLOR:Green>", CompuMaster.Console.RawPlainTextLog.ToString)
            Assert.AreEqual("<span style=""color: Green;"">" & Html.ToString & "</span>", CompuMaster.Console.HtmlLog.ToString)

            CompuMaster.Console.Clear(True, True, True, True)
            CompuMaster.Console.OkayLineDual(Text, Html)
            Assert.AreEqual(ExpectedTextWithLineBreak, CompuMaster.Console.PlainTextLog.ToString)
            Assert.AreEqual("<FORECOLOR:Green>" & ExpectedTextWithLineBreak & "</FORECOLOR:Green>", CompuMaster.Console.RawPlainTextLog.ToString)
            Assert.AreEqual("<span style=""color: Green;"">" & ExpectedHtmlWithLineBreak.ToString & "</span>", CompuMaster.Console.HtmlLog.ToString)

            CompuMaster.Console.Clear(True, True, True, True)
            CompuMaster.Console.LogDual(Text, Html)
            Assert.AreEqual(Text, CompuMaster.Console.PlainTextLog.ToString)
            Assert.AreEqual(Text, CompuMaster.Console.RawPlainTextLog.ToString)
            Assert.AreEqual(Html, CompuMaster.Console.HtmlLog.ToString)

            CompuMaster.Console.Clear(True, True, True, True)
            CompuMaster.Console.LogLineDual(Text, Html)
            Assert.AreEqual(ExpectedTextWithLineBreak, CompuMaster.Console.PlainTextLog.ToString)
            Assert.AreEqual(ExpectedTextWithLineBreak, CompuMaster.Console.RawPlainTextLog.ToString)
            Assert.AreEqual(ExpectedHtmlWithLineBreak, CompuMaster.Console.HtmlLog.ToString)
        End Sub

        <Test> Public Sub DualFeatureStringBuilderArgsCompletionTest()
            Dim Text As New System.Text.StringBuilder("Plain Text")
            Dim Html As New System.Text.StringBuilder("<h2>HTML code</h2>")
            Dim ExpectedTextWithLineBreak As String = Text.ToString & System.Environment.NewLine
            Dim ExpectedHtmlWithLineBreak As String = Html.ToString & "<br />"

            CompuMaster.Console.Clear(True, True, True, True)
            CompuMaster.Console.WriteLineDual()
            Assert.AreEqual(System.Environment.NewLine, CompuMaster.Console.PlainTextLog.ToString)
            Assert.AreEqual(System.Environment.NewLine, CompuMaster.Console.RawPlainTextLog.ToString)
            Assert.AreEqual("<br />", CompuMaster.Console.HtmlLog.ToString)

            CompuMaster.Console.Clear(True, True, True, True)
            CompuMaster.Console.LogLineDual()
            Assert.AreEqual(System.Environment.NewLine, CompuMaster.Console.PlainTextLog.ToString)
            Assert.AreEqual(System.Environment.NewLine, CompuMaster.Console.RawPlainTextLog.ToString)
            Assert.AreEqual("<br />", CompuMaster.Console.HtmlLog.ToString)

            CompuMaster.Console.Clear(True, True, True, True)
            CompuMaster.Console.WriteDual(Text, Html)
            Assert.AreEqual(Text.ToString, CompuMaster.Console.PlainTextLog.ToString)
            Assert.AreEqual(Text.ToString, CompuMaster.Console.RawPlainTextLog.ToString)
            Assert.AreEqual(Html.ToString, CompuMaster.Console.HtmlLog.ToString)

            CompuMaster.Console.Clear(True, True, True, True)
            CompuMaster.Console.WriteLineDual(Text, Html)
            Assert.AreEqual(ExpectedTextWithLineBreak.ToString, CompuMaster.Console.PlainTextLog.ToString)
            Assert.AreEqual(ExpectedTextWithLineBreak.ToString, CompuMaster.Console.RawPlainTextLog.ToString)
            Assert.AreEqual(ExpectedHtmlWithLineBreak.ToString, CompuMaster.Console.HtmlLog.ToString)

            CompuMaster.Console.Clear(True, True, True, True)
            CompuMaster.Console.WarnDual(Text, Html)
            Assert.AreEqual(Text.ToString, CompuMaster.Console.PlainTextLog.ToString)
            Assert.AreEqual("<FORECOLOR:Red>" & Text.ToString & "</FORECOLOR:Red>", CompuMaster.Console.RawPlainTextLog.ToString)
            Assert.AreEqual("<span style=""color: Red;"">" & Html.ToString & "</span>", CompuMaster.Console.HtmlLog.ToString)

            CompuMaster.Console.Clear(True, True, True, True)
            CompuMaster.Console.WarnLineDual(Text, Html)
            Assert.AreEqual(ExpectedTextWithLineBreak.ToString, CompuMaster.Console.PlainTextLog.ToString)
            Assert.AreEqual("<FORECOLOR:Red>" & ExpectedTextWithLineBreak.ToString.Replace(System.Environment.NewLine, "</FORECOLOR:Red>" & System.Environment.NewLine), CompuMaster.Console.RawPlainTextLog.ToString)
            Assert.AreEqual("<span style=""color: Red;"">" & ExpectedHtmlWithLineBreak.ToString.Replace("<br />", "</span><br />"), CompuMaster.Console.HtmlLog.ToString)

            CompuMaster.Console.Clear(True, True, True, True)
            CompuMaster.Console.OkayDual(Text, Html)
            Assert.AreEqual(Text.ToString, CompuMaster.Console.PlainTextLog.ToString)
            Assert.AreEqual("<FORECOLOR:Green>" & Text.ToString & "</FORECOLOR:Green>", CompuMaster.Console.RawPlainTextLog.ToString)
            Assert.AreEqual("<span style=""color: Green;"">" & Html.ToString & "</span>", CompuMaster.Console.HtmlLog.ToString)

            CompuMaster.Console.Clear(True, True, True, True)
            CompuMaster.Console.OkayLineDual(Text, Html)
            Assert.AreEqual(ExpectedTextWithLineBreak.ToString, CompuMaster.Console.PlainTextLog.ToString)
            Assert.AreEqual("<FORECOLOR:Green>" & ExpectedTextWithLineBreak.ToString.Replace(System.Environment.NewLine, "</FORECOLOR:Green>" & System.Environment.NewLine), CompuMaster.Console.RawPlainTextLog.ToString)
            Assert.AreEqual("<span style=""color: Green;"">" & ExpectedHtmlWithLineBreak.ToString.Replace("<br />", "</span><br />"), CompuMaster.Console.HtmlLog.ToString)

            CompuMaster.Console.Clear(True, True, True, True)
            CompuMaster.Console.LogDual(Text, Html)
            Assert.AreEqual(Text.ToString, CompuMaster.Console.PlainTextLog.ToString)
            Assert.AreEqual(Text.ToString, CompuMaster.Console.RawPlainTextLog.ToString)
            Assert.AreEqual(Html.ToString, CompuMaster.Console.HtmlLog.ToString)

            CompuMaster.Console.Clear(True, True, True, True)
            CompuMaster.Console.LogLineDual(Text, Html)
            Assert.AreEqual(ExpectedTextWithLineBreak.ToString, CompuMaster.Console.PlainTextLog.ToString)
            Assert.AreEqual(ExpectedTextWithLineBreak.ToString, CompuMaster.Console.RawPlainTextLog.ToString)
            Assert.AreEqual(ExpectedHtmlWithLineBreak.ToString, CompuMaster.Console.HtmlLog.ToString)
        End Sub

        <Test> Public Sub CloneStringBuilder()
            Dim sb As New System.Text.StringBuilder

            sb.Clear()
            Assert.AreEqual(sb.ToString, CompuMaster.Console.CloneStringBuilder(sb).ToString)

            sb.Clear()
            sb.Append("kkkkkaaakkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk")
            Assert.AreEqual(sb.ToString, CompuMaster.Console.CloneStringBuilder(sb).ToString)

            sb.Clear()
            sb.Append("kkkkkkaaakkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk")
            Assert.AreEqual(sb.ToString, CompuMaster.Console.CloneStringBuilder(sb).ToString)

            sb.Clear()
            sb.Append("kkkkkkaaakkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk".Substring(0, 40))
            Assert.AreEqual(sb.ToString, CompuMaster.Console.CloneStringBuilder(sb).ToString)

            sb.Clear()
            sb.Append("kkkkkkaaakkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk".Substring(0, 39))
            Assert.AreEqual(sb.ToString, CompuMaster.Console.CloneStringBuilder(sb).ToString)

            sb.Clear()
            sb.Append("kkkkkkaaakkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk".Substring(0, 41))
            Assert.AreEqual(sb.ToString, CompuMaster.Console.CloneStringBuilder(sb).ToString)
        End Sub

        <Test> Public Sub SavedEncodings()
            CompuMaster.Console.WriteLine("TEST ÄÖÜß")
            Dim TempFile As String = System.IO.Path.GetTempFileName()
            System.Console.WriteLine("Test-File: " & TempFile)

            CompuMaster.Console.SaveHtmlLog(TempFile, "TestTitle")
            SavedEncodings_TestUtf8BOM(System.IO.File.ReadAllBytes(TempFile))

            CompuMaster.Console.SaveHtmlLog(TempFile, "TestTitle", New System.Text.UTF8Encoding(True))
            SavedEncodings_TestUtf8BOM(System.IO.File.ReadAllBytes(TempFile))

            CompuMaster.Console.SaveHtmlLog(TempFile, "<head-item />", "<pre>", "</pre>")
            SavedEncodings_TestUtf8BOM(System.IO.File.ReadAllBytes(TempFile))

            CompuMaster.Console.SaveHtmlLog(TempFile, "<head-item />", "<pre>", "</pre>", New System.Text.UTF8Encoding(True))
            SavedEncodings_TestUtf8BOM(System.IO.File.ReadAllBytes(TempFile))

            CompuMaster.Console.SavePlainTextLog(TempFile)
            SavedEncodings_TestUtf8BOM(System.IO.File.ReadAllBytes(TempFile))

            CompuMaster.Console.SavePlainTextLog(TempFile, New System.Text.UTF8Encoding(True))
            SavedEncodings_TestUtf8BOM(System.IO.File.ReadAllBytes(TempFile))

            CompuMaster.Console.SavePlainTextLog(TempFile, New System.Text.UTF8Encoding(False))
            SavedEncodings_TestNoUtf8BOM(System.IO.File.ReadAllBytes(TempFile))
        End Sub

        Private Sub SavedEncodings_TestUtf8BOM(value As Byte())
            Assert.AreEqual(239, CType(value(0), Integer))
            Assert.AreEqual(187, CType(value(1), Integer))
            Assert.AreEqual(191, CType(value(2), Integer))
        End Sub
        Private Sub SavedEncodings_TestNoUtf8BOM(value As Byte())
            Assert.AreNotEqual(239, CType(value(0), Integer))
            Assert.AreNotEqual(187, CType(value(1), Integer))
            Assert.AreNotEqual(191, CType(value(2), Integer))
        End Sub


    End Class

End Namespace