Imports System.Data
Imports System.Runtime.InteropServices

Module MainModule

    Sub Main()
        ShowCurrentEnvironmentArchitecture()

        'Demo mode
        'RunDemo()

        'Try to (not) get an OutOfMemoryException
        TestForOutOfMemoryExceptionWithHugeLogData_StringBuilderArguments()
        TestForOutOfMemoryExceptionWithHugeLogData_StringArguments()
    End Sub

    Private Sub ShowCurrentEnvironmentArchitecture()
        System.Console.WriteLine("Aktuelle .NET Plattform: " & RuntimeInformation.FrameworkDescription.ToString)

        ' Prüfen, ob die Anwendung als 32- oder 64-Bit-Anwendung läuft
        Dim architecture As String = If(Environment.Is64BitProcess, "64-Bit", "32-Bit")
        Console.WriteLine($"Architektur: {architecture}")
    End Sub

    Private Sub RunDemo()
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

    ''' <summary>
    ''' Test for OutOfMemoryException
    ''' </summary>
    ''' <remarks>
    ''' System.OutOfMemoryException
    '''  HResult=0x8007000E
    '''  Nachricht = Eine Ausnahme vom Typ "System.OutOfMemoryException" wurde ausgelöst.
    '''  Quelle = mscorlib
    '''  Stapelüberwachung:
    '''   at System.Text.StringBuilder.ToString()
    '''   at System.Text.RegularExpressions.RegexReplacement.Replace(Regex regex, String input, Int32 count, Int32 startat)
    '''   at System.Text.RegularExpressions.Regex.Replace(String input, String replacement, Int32 count, Int32 startat)
    '''   at System.Text.RegularExpressions.Regex.Replace(String input, String replacement)
    '''   at System.Text.RegularExpressions.Regex.Replace(String input, String pattern, String replacement)
    '''   at CompuMaster.Console.IndentText(String text, Int32 indentLevel, Boolean continueStartedLine)
    '''   at CompuMaster.Console.IndentText(String text, Boolean continueStartedLine)
    '''   at CompuMaster.Console._WriteDual(StringBuilder text, StringBuilder html, Boolean showConsoleOutput)
    '''   at CompuMaster.Console.WriteDual(StringBuilder text, StringBuilder html, ConsoleColor colorForeground, ConsoleColor colorBackground)
    '''   at CompuMaster.Console.WriteDual(StringBuilder text, StringBuilder html)
    '''   at CompuMaster.Console.WarnDual(StringBuilder text, StringBuilder html)
    '''   at CompuMaster.Console.WarnLineDual(StringBuilder text, StringBuilder html)
    '''   at KmzExport2.MainModule.Main() In D:\svn_repository\cmmCsvExport\KmzExport2\MainModule.vb:line 95
    ''' </remarks>
    Private Sub TestForOutOfMemoryExceptionWithHugeLogData_StringBuilderArguments()
        Dim PreviewTextTable As System.Text.StringBuilder = VeryLargePlainTextTable()
        Dim PreviewHtmlTable As System.Text.StringBuilder = VeryLargeHtmlTable()

        CompuMaster.Console.WriteLineDual(PreviewTextTable, PreviewHtmlTable)

        Dim LogFile As String = System.IO.Path.Combine(AppDirPath, "export-log-" & Now.ToString("yyyyMMdd-HHmm") & ".html")
        CompuMaster.Console.SaveHtmlLog(LogFile)
        System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo(LogFile) With {.UseShellExecute = True})
    End Sub

    Private Function AppDirPath() As String
        Return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)
    End Function

    Private Sub TestForOutOfMemoryExceptionWithHugeLogData_StringArguments()
        Dim PreviewTextTable As String = VeryLargePlainTextTable.ToString
        Dim PreviewHtmlTable As String = VeryLargeHtmlTable.ToString

        CompuMaster.Console.WriteLineDual(PreviewTextTable, PreviewHtmlTable)

        Dim LogFile As String = System.IO.Path.Combine(AppDirPath, "export-log-" & Now.ToString("yyyyMMdd-HHmm") & ".html")
        CompuMaster.Console.SaveHtmlLog(LogFile)
        System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo(LogFile) With {.UseShellExecute = True})
    End Sub

    Private Function VeryLargePlainTextTable() As System.Text.StringBuilder
        Static _Result As System.Text.StringBuilder
        If _Result Is Nothing Then
            Const RequiredRowCount As Integer = 200000
            System.Console.Write("Creating very large plain text table with " & RequiredRowCount.ToString("#,##0") & " rows . . . ")
            Dim Result As New System.Text.StringBuilder(
                "ID            |Title                   |MainKeyword                     |Schlagwort                   |PublishOnGis|AlreadyExportedToGis|Ort                  |Region|Landschaft     |Zeitraum|CreationDate    |Column1              |Column2             |Remarks                                                                              |Info             |RemoteID             |RemoteDetails      " & ControlChars.CrLf &
                "--------------+------------------------+--------------------------------+-----------------------------+------------+--------------------+---------------------+------+---------------+--------+----------------+---------------------+--------------------+-------------------------------------------------------------------------------------+-----------------+---------------------+-------------------" & ControlChars.CrLf)
            For MyCounter = 1 To 1000000
                Result.Append(MyCounter.ToString("00000000000000"))
                Result.AppendLine("|Title data for this item|This item has got a main keyword|And there are additional tags|            |                    |Hawaii, Isle Paradise|Beach |Beautyful ocean|ab 2000 |01.01.2004 00:00|Somebody made a photo|Some kind of picture|This will be a long text to get some huge text data for getting OutOfMemoryExceptions|More info to come|ID in 3rd pary system|there was some data")
            Next
            System.Console.WriteLine("DONE")
            _Result = Result
        End If
        Return _Result
    End Function

    Private Function VeryLargeHtmlTable() As System.Text.StringBuilder
        Static _Result As System.Text.StringBuilder
        If _Result Is Nothing Then
            Const RequiredRowCount As Integer = 200000
            System.Console.Write("Creating very large HTML table with " & RequiredRowCount.ToString("#,##0") & " rows . . . ")
            Dim Result As New System.Text.StringBuilder("<table><tbody>" & ControlChars.CrLf &
                "<tr><th>ID</th><th>Title                   </th><th>MainKeyword                     </th><th>Schlagwort                   </th><th>PublishOnGis</th><th>AlreadyExportedToGis</th><th>Ort                  </th><th>Region</th><th>Landschaft     </th><th>Zeitraum</th><th>CreationDate    </th><th>Column1              </th><th>Column2             </th><th>Remarks                                                                              </th><th>Info             </th><th>RemoteID             </th><th>RemoteDetails</th></tr>" & ControlChars.CrLf)
            For MyCounter = 1 To 1000000
                Result.Append("<tr><td>")
                Result.Append(MyCounter.ToString("00000000000000"))
                Result.Append("</td><td>Title data for this item</td><td>This item has got a main keyword</td><td>And there are additional tags</td><td>            </td><td>                    </td><td>Hawaii, Isle Paradise</td><td>Beach </td><td>Beautyful ocean</td><td>ab 2000 </td><td>01.01.2004 00:00</td><td>Somebody made a photo</td><td>Some kind of picture</td><td>This will be a long text to get some huge text data for getting OutOfMemoryExceptions</td><td>More info to come</td><td>ID in 3rd pary system</td><td>there was some data")
                Result.AppendLine("</td></tr>")
            Next
            Result.AppendLine("</tbody></table>")
            System.Console.WriteLine("DONE")
            _Result = Result
        End If
        Return _Result
    End Function

End Module
