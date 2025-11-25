Option Explicit On
Option Strict On

Imports CompuMaster.VisualBasicCompatibility

<Assembly: Runtime.CompilerServices.InternalsVisibleTo("ConsoleTest")>

Namespace CompuMaster

    <CodeAnalysis.SuppressMessage("Style", "IDE1006:Benennungsstile", Justification:="<Ausstehend>")>
    <CodeAnalysis.SuppressMessage("Style", "CA1416", Justification:="Non-Windows-Platforms might not support these properties/methods")>
    Partial Public NotInheritable Class Console

        Private Shared ReadOnly _PlainTextWarningsLog As New System.Text.StringBuilder
        Private Shared ReadOnly _HtmlWarningsLog As New System.Text.StringBuilder
        Private Shared ReadOnly _RawPlainTextLog As New System.Text.StringBuilder
        Private Shared ReadOnly _HtmlLog As New System.Text.StringBuilder

        ''' <summary>
        ''' The initial foreground color on first access by this class
        ''' </summary>
        Public Shared ReadOnly SystemConsoleDefaultForegroundColor As System.ConsoleColor = InitialForegroundColor()

        ''' <summary>
        ''' The initial background color on first access by this class
        ''' </summary>
        Public Shared ReadOnly SystemConsoleDefaultBackgroundColor As System.ConsoleColor = InitialBackgroundColor()

        ''' <summary>
        ''' Indicates if System.Console has been redirected or by other reason not able to initialize valid ConsoleColors
        ''' </summary>
        ''' <remarks>Especially on Linux/Unix platforms when calling from X11 File Manager (e.g. Nautilus), the Console App is started with a redirected Console Output. In this case, no color assignments to System.Color.Fore/BackColor are stored. In this case, the HTML log output is the only output format that maintains colored output. But since every Color setup is initially and remainingly at 0 (black), the HTML log appears in browser with colors black font on black background. This situation must be prevented, so the fore/background colors are stored in this very condition in local variables instead of System.Console properties.</remarks>
        Friend Shared ReadOnly NoSystemConsoleConnectedOrConsoleIsRedirected_BufferColorChangesByOurself As Boolean = (System.Console.IsOutputRedirected OrElse ((System.Console.ForegroundColor = 0 OrElse System.Console.ForegroundColor = -1) AndAlso (System.Console.BackgroundColor = 0 OrElse System.Console.BackgroundColor = -1)))

        ''' <summary>
        ''' Depending on a connected/redirected System.Console Output, the system default for ForegroundColor must be assigned differently
        ''' </summary>
        ''' <returns></returns>
        Private Shared Function InitialForegroundColor() As System.ConsoleColor
            Dim Result As ConsoleColor
            If NoSystemConsoleConnectedOrConsoleIsRedirected_BufferColorChangesByOurself Then
                Result = ConsoleColor.White
            Else
                Result = System.Console.ForegroundColor
            End If
            If Result = -1 OrElse Result = InitialBackgroundColor() Then
                'Background and foreground color are equal, but must be different
                Select Case Result
                    Case ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.Gray, ConsoleColor.Magenta
                        Result = ConsoleColor.Black
                    Case Else
                        Result = ConsoleColor.Gray
                End Select
            End If
            Return Result
        End Function

        ''' <summary>
        ''' Depending on a connected/redirected System.Console Output, the system default for BackgroundColor must be assigned differently
        ''' </summary>
        ''' <returns></returns>
        Private Shared Function InitialBackgroundColor() As System.ConsoleColor
            Dim Result As ConsoleColor
            If NoSystemConsoleConnectedOrConsoleIsRedirected_BufferColorChangesByOurself Then
                Result = ConsoleColor.Black
            Else
                Result = System.Console.BackgroundColor
            End If
            If Result = -1 Then
                Result = ConsoleColor.Black
            End If
            Return Result
        End Function

        ''' <summary>
        ''' Warnings will always be displayed with this ForegroundColor
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property WarningForegroundColor As System.ConsoleColor = ConsoleColor.Red
        ''' <summary>
        ''' Warnings will always be displayed with this BackgroundColor
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property WarningBackgroundColor As System.ConsoleColor = BackgroundColor
        ''' <summary>
        ''' Info/Okay messages will always be displayed with this ForegroundColor
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property OkayMessageForegroundColor As System.ConsoleColor = ConsoleColor.Green
        ''' <summary>
        ''' Info/Okay messages will always be displayed with this BackgroundColor
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property OkayMessageBackgroundColor As System.ConsoleColor = BackgroundColor

        Private Shared _ForegroundColor As System.ConsoleColor = ConsoleColor.White
        ''' <summary>
        ''' Regular console output will always be displayed with this ForegroundColor
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property ForegroundColor As System.ConsoleColor
            Get
                If NoSystemConsoleConnectedOrConsoleIsRedirected_BufferColorChangesByOurself Then
                    Return _ForegroundColor
                Else
                    Return System.Console.ForegroundColor
                End If
            End Get
            Set(value As System.ConsoleColor)
                _ForegroundColor = value
                System.Console.ForegroundColor = value
            End Set
        End Property

        Private Shared _BackgroundColor As System.ConsoleColor = ConsoleColor.Black
        ''' <summary>
        ''' Regular console output will always be displayed with this BackgroundColor
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property BackgroundColor As System.ConsoleColor
            Get
                If NoSystemConsoleConnectedOrConsoleIsRedirected_BufferColorChangesByOurself Then
                    Return _BackgroundColor
                Else
                    Return System.Console.BackgroundColor
                End If
            End Get
            Set(value As System.ConsoleColor)
                _BackgroundColor = value
                System.Console.BackgroundColor = value
            End Set
        End Property

        Public Enum SystemColorModesInLogs As Byte
            ''' <summary>
            ''' If a system fore/background color is used, don't add HTML code for color usage, leading to default color of HTML document
            ''' </summary>
            ApplyDefaultColorInLoglIfSystemDefaultColorIsUsed = 0
            ''' <summary>
            ''' If a system fore/background color is used, always add HTML code for color usage
            ''' </summary>
            AlwaysApplySystemColorInLog = 1
        End Enum

        ''' <summary>
        ''' The intended behaviour for color usage in logs if a system fore/background color is used
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property SystemColorsInLogs As SystemColorModesInLogs

        ''' <summary>
        ''' Log message without output to console
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        Public Shared Sub Log(text As String)
            _Write(text, False)
        End Sub

        ''' <summary>
        ''' Log message without output to console
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        Public Shared Sub Log(text As System.Text.StringBuilder)
            _Write(text, False)
        End Sub

        ''' <summary>
        ''' Log message without output to console
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        Public Shared Sub LogDual(text As String, html As String)
            _WriteDual(text, html, False)
        End Sub

        ''' <summary>
        ''' Log message without output to console
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        Public Shared Sub LogDual(text As System.Text.StringBuilder, html As System.Text.StringBuilder)
            _WriteDual(text, html, False)
        End Sub

        ''' <summary>
        ''' Log message without output to console
        ''' </summary>
        Public Shared Sub LogLine()
            _Write(System.Environment.NewLine, False)
        End Sub

        ''' <summary>
        ''' Log message without output to console
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        Public Shared Sub LogLine(text As String)
            _Write(text & System.Environment.NewLine, False)
        End Sub

        ''' <summary>
        ''' Log message without output to console
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        Public Shared Sub LogLine(text As System.Text.StringBuilder)
            _Write(text, False)
            _Write(System.Environment.NewLine, False)
        End Sub

        ''' <summary>
        ''' Log message without output to console
        ''' </summary>
        Public Shared Sub LogLineDual()
            _WriteDual(System.Environment.NewLine, "<br />", False)
        End Sub

        ''' <summary>
        ''' Log message without output to console
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        Public Shared Sub LogLineDual(text As String, html As String)
            _WriteDual(text & System.Environment.NewLine, html & "<br />", False)
        End Sub

        ''' <summary>
        ''' Log message without output to console
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        Public Shared Sub LogLineDual(text As System.Text.StringBuilder, html As System.Text.StringBuilder)
            _WriteDual(text, html, False)
            _WriteDual(System.Environment.NewLine, "<br />", False)
        End Sub

        Private Shared IsNewOutputLineAtConsole As Boolean = True
        Private Shared IsNewOutputLineAtLog As Boolean = True

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged and (optionally) printed to system's console</param>
        ''' <param name="showConsoleOutput">True to print plain text to system's console, False to log only</param>
        Private Shared Sub _Write(text As String, showConsoleOutput As Boolean)
            If IsControlCKeyPressed AndAlso ThrowControlCKeyPressedExceptionOnNextConsoleCommand Then
                Dim innerEx As ControlCKeyPressedException = ControlCKeyPressed
                _ControlCKeyPressed = Nothing 'don't raise for a 2nd time!
                Throw New ControlCKeyPressedException(innerEx)
            End If
            If text = Nothing Then Return 'Empty content - nothing to do

            If showConsoleOutput Then
                'System console
                AppendToConsole(IndentText(New System.Text.StringBuilder(text), Not IsNewOutputLineAtConsole))
            End If

            'Plain text log
            If text = System.Environment.NewLine Then
                _RawPlainTextLog.Append(System.Environment.NewLine)
            Else
                If BackgroundColor <> SystemConsoleDefaultBackgroundColor OrElse SystemColorsInLogs = SystemColorModesInLogs.AlwaysApplySystemColorInLog Then
                    _RawPlainTextLog.Append("<BACKCOLOR:" & ConsoleColorSystemName(BackgroundColor) & ">")
                End If
                If ForegroundColor <> SystemConsoleDefaultForegroundColor OrElse SystemColorsInLogs = SystemColorModesInLogs.AlwaysApplySystemColorInLog Then
                    _RawPlainTextLog.Append("<FORECOLOR:" & ConsoleColorSystemName(ForegroundColor) & ">")
                End If
                AppendToStringBuilder(_RawPlainTextLog, IndentText(New System.Text.StringBuilder(text), Not IsNewOutputLineAtLog))
                If ForegroundColor <> SystemConsoleDefaultForegroundColor OrElse SystemColorsInLogs = SystemColorModesInLogs.AlwaysApplySystemColorInLog Then
                    _RawPlainTextLog.Append("</FORECOLOR:" & ConsoleColorSystemName(ForegroundColor) & ">")
                End If
                If BackgroundColor <> SystemConsoleDefaultBackgroundColor OrElse SystemColorsInLogs = SystemColorModesInLogs.AlwaysApplySystemColorInLog Then
                    _RawPlainTextLog.Append("</BACKCOLOR:" & ConsoleColorSystemName(BackgroundColor) & ">")
                End If
            End If

            'Html log
            If text = System.Environment.NewLine Then
                _HtmlLog.Append("<br />")
            Else
                Dim TextAsHtml As System.Text.StringBuilder = IndentText(New System.Text.StringBuilder(System.Net.WebUtility.HtmlEncode(text)), Not IsNewOutputLineAtLog).Replace(" ", "&nbsp;").Replace(System.Environment.NewLine, "<br />")
                If BackgroundColor <> SystemConsoleDefaultBackgroundColor Then
                    _HtmlLog.Append("<span style=""background-color: " & ConsoleColorCssName(BackgroundColor) & ";"">")
                End If
                If ForegroundColor <> SystemConsoleDefaultForegroundColor OrElse SystemColorsInLogs = SystemColorModesInLogs.AlwaysApplySystemColorInLog Then
                    _HtmlLog.Append("<span style=""color: " & ConsoleColorCssName(ForegroundColor) & ";"">")
                End If
                AppendToStringBuilder(_HtmlLog, TextAsHtml)
                If ForegroundColor <> SystemConsoleDefaultForegroundColor OrElse SystemColorsInLogs = SystemColorModesInLogs.AlwaysApplySystemColorInLog Then
                    _HtmlLog.Append("</span>")
                End If
                If BackgroundColor <> SystemConsoleDefaultBackgroundColor Then
                    _HtmlLog.Append("</span>")
                End If
            End If

            'Remember state of completed line for correct indentation on next line
            If text.EndsWith(ControlChars.Cr) OrElse text.EndsWith(ControlChars.Lf) Then
                'Line has been completed
                If showConsoleOutput Then
                    IsNewOutputLineAtConsole = True
                    IsNewOutputLineAtLog = True
                Else
                    IsNewOutputLineAtLog = True
                End If
            Else
                'Line is to be continued
                If showConsoleOutput Then
                    IsNewOutputLineAtConsole = False
                    IsNewOutputLineAtLog = False
                Else
                    IsNewOutputLineAtLog = False
                End If
            End If
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged and (optionally) printed to system's console</param>
        ''' <param name="showConsoleOutput">True to print plain text to system's console, False to log only</param>
        Private Shared Sub _Write(text As System.Text.StringBuilder, showConsoleOutput As Boolean)
            If IsControlCKeyPressed AndAlso ThrowControlCKeyPressedExceptionOnNextConsoleCommand Then
                Dim innerEx As ControlCKeyPressedException = ControlCKeyPressed
                _ControlCKeyPressed = Nothing 'don't raise for a 2nd time!
                Throw New ControlCKeyPressedException(innerEx)
            End If
            If text Is Nothing OrElse text.Length = 0 Then Return 'Empty content - nothing to do

            If showConsoleOutput Then
                'System console
                AppendToConsole(IndentText(text, Not IsNewOutputLineAtConsole))
            End If

            'Plain text log
            If text.Length < 3 AndAlso text.ToString = System.Environment.NewLine Then
                _RawPlainTextLog.Append(System.Environment.NewLine)
            Else
                If BackgroundColor <> SystemConsoleDefaultBackgroundColor OrElse SystemColorsInLogs = SystemColorModesInLogs.AlwaysApplySystemColorInLog Then
                    _RawPlainTextLog.Append("<BACKCOLOR:" & ConsoleColorSystemName(BackgroundColor) & ">")
                End If
                If ForegroundColor <> SystemConsoleDefaultForegroundColor OrElse SystemColorsInLogs = SystemColorModesInLogs.AlwaysApplySystemColorInLog Then
                    _RawPlainTextLog.Append("<FORECOLOR:" & ConsoleColorSystemName(ForegroundColor) & ">")
                End If
                AppendToStringBuilder(_RawPlainTextLog, IndentText(text, Not IsNewOutputLineAtLog))
                If ForegroundColor <> SystemConsoleDefaultForegroundColor OrElse SystemColorsInLogs = SystemColorModesInLogs.AlwaysApplySystemColorInLog Then
                    _RawPlainTextLog.Append("</FORECOLOR:" & ConsoleColorSystemName(ForegroundColor) & ">")
                End If
                If BackgroundColor <> SystemConsoleDefaultBackgroundColor OrElse SystemColorsInLogs = SystemColorModesInLogs.AlwaysApplySystemColorInLog Then
                    _RawPlainTextLog.Append("</BACKCOLOR:" & ConsoleColorSystemName(BackgroundColor) & ">")
                End If
            End If

            'Html log
            If text.Length < 3 AndAlso text.ToString = System.Environment.NewLine Then
                _HtmlLog.Append("<br />")
            Else
                Dim TextAsHtml As System.Text.StringBuilder = IndentText(New System.Text.StringBuilder(System.Net.WebUtility.HtmlEncode(text.ToString)), Not IsNewOutputLineAtLog).Replace(" ", "&nbsp;").Replace(System.Environment.NewLine, "<br />")
                If BackgroundColor <> SystemConsoleDefaultBackgroundColor Then
                    _HtmlLog.Append("<span style=""background-color: " & ConsoleColorCssName(BackgroundColor) & ";"">")
                End If
                If ForegroundColor <> SystemConsoleDefaultForegroundColor OrElse SystemColorsInLogs = SystemColorModesInLogs.AlwaysApplySystemColorInLog Then
                    _HtmlLog.Append("<span style=""color: " & ConsoleColorCssName(ForegroundColor) & ";"">")
                End If
                AppendToStringBuilder(_HtmlLog, TextAsHtml)
                If ForegroundColor <> SystemConsoleDefaultForegroundColor OrElse SystemColorsInLogs = SystemColorModesInLogs.AlwaysApplySystemColorInLog Then
                    _HtmlLog.Append("</span>")
                End If
                If BackgroundColor <> SystemConsoleDefaultBackgroundColor Then
                    _HtmlLog.Append("</span>")
                End If
            End If

            'Remember state of completed line for correct indentation on next line
            If text.Chars(text.Length - 1) = ControlChars.Cr OrElse text.Chars(text.Length - 1) = ControlChars.Lf Then
                'Line has been completed
                If showConsoleOutput Then
                    IsNewOutputLineAtConsole = True
                    IsNewOutputLineAtLog = True
                Else
                    IsNewOutputLineAtLog = True
                End If
            Else
                'Line is to be continued
                If showConsoleOutput Then
                    IsNewOutputLineAtConsole = False
                    IsNewOutputLineAtLog = False
                Else
                    IsNewOutputLineAtLog = False
                End If
            End If
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged and (optionally) printed to system's console</param>
        ''' <param name="html">HTML code to be logged</param>
        ''' <param name="showConsoleOutput">True to print plain text to system's console, False to log only</param>
        Private Shared Sub _WriteDual(text As String, html As String, showConsoleOutput As Boolean)
            If IsControlCKeyPressed AndAlso ThrowControlCKeyPressedExceptionOnNextConsoleCommand Then
                Dim innerEx As ControlCKeyPressedException = ControlCKeyPressed
                _ControlCKeyPressed = Nothing 'don't raise for a 2nd time!
                Throw New ControlCKeyPressedException(innerEx)
            End If
            If text <> Nothing Then
                If showConsoleOutput Then
                    'System console
                    AppendToConsole(IndentText(New System.Text.StringBuilder(text), Not IsNewOutputLineAtConsole))
                End If

                'Plain text log
                If text = System.Environment.NewLine Then
                    _RawPlainTextLog.Append(System.Environment.NewLine)
                Else
                    If BackgroundColor <> SystemConsoleDefaultBackgroundColor OrElse SystemColorsInLogs = SystemColorModesInLogs.AlwaysApplySystemColorInLog Then
                        _RawPlainTextLog.Append("<BACKCOLOR:" & ConsoleColorSystemName(BackgroundColor) & ">")
                    End If
                    If ForegroundColor <> SystemConsoleDefaultForegroundColor OrElse SystemColorsInLogs = SystemColorModesInLogs.AlwaysApplySystemColorInLog Then
                        _RawPlainTextLog.Append("<FORECOLOR:" & ConsoleColorSystemName(ForegroundColor) & ">")
                    End If
                    AppendToStringBuilder(_RawPlainTextLog, IndentText(New System.Text.StringBuilder(text), Not IsNewOutputLineAtLog))
                    If ForegroundColor <> SystemConsoleDefaultForegroundColor OrElse SystemColorsInLogs = SystemColorModesInLogs.AlwaysApplySystemColorInLog Then
                        _RawPlainTextLog.Append("</FORECOLOR:" & ConsoleColorSystemName(ForegroundColor) & ">")
                    End If
                    If BackgroundColor <> SystemConsoleDefaultBackgroundColor OrElse SystemColorsInLogs = SystemColorModesInLogs.AlwaysApplySystemColorInLog Then
                        _RawPlainTextLog.Append("</BACKCOLOR:" & ConsoleColorSystemName(BackgroundColor) & ">")
                    End If
                End If
            End If

            'Html log
            If html <> Nothing Then
                If html = "<br />" Then
                    _HtmlLog.Append("<br />")
                Else
                    Dim TextAsHtml As System.Text.StringBuilder = IndentTextInHtml(New System.Text.StringBuilder(html), Not IsNewOutputLineAtLog)
                    If BackgroundColor <> SystemConsoleDefaultBackgroundColor Then
                        _HtmlLog.Append("<span style=""background-color: " & ConsoleColorCssName(BackgroundColor) & ";"">")
                    End If
                    If ForegroundColor <> SystemConsoleDefaultForegroundColor OrElse SystemColorsInLogs = SystemColorModesInLogs.AlwaysApplySystemColorInLog Then
                        _HtmlLog.Append("<span style=""color: " & ConsoleColorCssName(ForegroundColor) & ";"">")
                    End If
                    AppendToStringBuilder(_HtmlLog, TextAsHtml)
                    If ForegroundColor <> SystemConsoleDefaultForegroundColor OrElse SystemColorsInLogs = SystemColorModesInLogs.AlwaysApplySystemColorInLog Then
                        _HtmlLog.Append("</span>")
                    End If
                    If BackgroundColor <> SystemConsoleDefaultBackgroundColor Then
                        _HtmlLog.Append("</span>")
                    End If
                End If
            End If

            'Remember state of completed line for correct indentation on next line
            If text.EndsWith(ControlChars.Cr) OrElse text.EndsWith(ControlChars.Lf) Then
                'Line has been completed
                If showConsoleOutput Then
                    IsNewOutputLineAtConsole = True
                    IsNewOutputLineAtLog = True
                Else
                    IsNewOutputLineAtLog = True
                End If
            Else
                'Line is to be continued
                If showConsoleOutput Then
                    IsNewOutputLineAtConsole = False
                    IsNewOutputLineAtLog = False
                Else
                    IsNewOutputLineAtLog = False
                End If
            End If
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged and (optionally) printed to system's console</param>
        ''' <param name="html">HTML code to be logged</param>
        ''' <param name="showConsoleOutput">True to print plain text to system's console, False to log only</param>
        Private Shared Sub _WriteDual(text As System.Text.StringBuilder, html As System.Text.StringBuilder, showConsoleOutput As Boolean)
            If IsControlCKeyPressed AndAlso ThrowControlCKeyPressedExceptionOnNextConsoleCommand Then
                Dim innerEx As ControlCKeyPressedException = ControlCKeyPressed
                _ControlCKeyPressed = Nothing 'don't raise for a 2nd time!
                Throw New ControlCKeyPressedException(innerEx)
            End If
            If Not (text Is Nothing OrElse text.Length = 0) Then
                If showConsoleOutput Then
                    'System console
                    AppendToConsole(IndentText(text, Not IsNewOutputLineAtConsole))
                End If

                'Plain text log
                If text.Length < 3 AndAlso text.ToString = System.Environment.NewLine Then
                    _RawPlainTextLog.Append(System.Environment.NewLine)
                Else
                    If BackgroundColor <> SystemConsoleDefaultBackgroundColor OrElse SystemColorsInLogs = SystemColorModesInLogs.AlwaysApplySystemColorInLog Then
                        _RawPlainTextLog.Append("<BACKCOLOR:" & ConsoleColorSystemName(BackgroundColor) & ">")
                    End If
                    If ForegroundColor <> SystemConsoleDefaultForegroundColor OrElse SystemColorsInLogs = SystemColorModesInLogs.AlwaysApplySystemColorInLog Then
                        _RawPlainTextLog.Append("<FORECOLOR:" & ConsoleColorSystemName(ForegroundColor) & ">")
                    End If
                    AppendToStringBuilder(_RawPlainTextLog, IndentText(text, Not IsNewOutputLineAtLog))
                    If ForegroundColor <> SystemConsoleDefaultForegroundColor OrElse SystemColorsInLogs = SystemColorModesInLogs.AlwaysApplySystemColorInLog Then
                        _RawPlainTextLog.Append("</FORECOLOR:" & ConsoleColorSystemName(ForegroundColor) & ">")
                    End If
                    If BackgroundColor <> SystemConsoleDefaultBackgroundColor OrElse SystemColorsInLogs = SystemColorModesInLogs.AlwaysApplySystemColorInLog Then
                        _RawPlainTextLog.Append("</BACKCOLOR:" & ConsoleColorSystemName(BackgroundColor) & ">")
                    End If
                End If
            End If

            'Html log
            If Not (html Is Nothing OrElse html.Length = 0) Then
                If html.Length = 6 AndAlso html.ToString = "<br />" Then
                    _HtmlLog.Append("<br />")
                Else
                    Dim TextAsHtml As System.Text.StringBuilder = IndentTextInHtml(html, Not IsNewOutputLineAtLog)
                    If BackgroundColor <> SystemConsoleDefaultBackgroundColor Then
                        _HtmlLog.Append("<span style=""background-color: " & ConsoleColorCssName(BackgroundColor) & ";"">")
                    End If
                    If ForegroundColor <> SystemConsoleDefaultForegroundColor OrElse SystemColorsInLogs = SystemColorModesInLogs.AlwaysApplySystemColorInLog Then
                        _HtmlLog.Append("<span style=""color: " & ConsoleColorCssName(ForegroundColor) & ";"">")
                    End If
                    AppendToStringBuilder(_HtmlLog, TextAsHtml)
                    If ForegroundColor <> SystemConsoleDefaultForegroundColor OrElse SystemColorsInLogs = SystemColorModesInLogs.AlwaysApplySystemColorInLog Then
                        _HtmlLog.Append("</span>")
                    End If
                    If BackgroundColor <> SystemConsoleDefaultBackgroundColor Then
                        _HtmlLog.Append("</span>")
                    End If
                End If
            End If

            'Remember state of completed line for correct indentation on next line
            If text.Chars(text.Length - 1) = ControlChars.Cr OrElse text.Chars(text.Length - 1) = ControlChars.Lf Then
                'Line has been completed
                If showConsoleOutput Then
                    IsNewOutputLineAtConsole = True
                    IsNewOutputLineAtLog = True
                Else
                    IsNewOutputLineAtLog = True
                End If
            Else
                'Line is to be continued
                If showConsoleOutput Then
                    IsNewOutputLineAtConsole = False
                    IsNewOutputLineAtLog = False
                Else
                    IsNewOutputLineAtLog = False
                End If
            End If
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        Private Shared Sub WriteToWarningLog(text As String)
            If text = Nothing Then Return
            _PlainTextWarningsLog.Append(text)
            Dim TextAsHtml As String = System.Net.WebUtility.HtmlEncode(text.ToString).Replace(System.Environment.NewLine, "<br />")
            _HtmlWarningsLog.Append(TextAsHtml)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        Private Shared Sub WriteToWarningLog(text As System.Text.StringBuilder)
            _PlainTextWarningsLog.Append(text)
            Dim TextAsHtml As String = System.Net.WebUtility.HtmlEncode(text.ToString).Replace(System.Environment.NewLine, "<br />")
            _HtmlWarningsLog.Append(TextAsHtml)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        Private Shared Sub WriteToWarningLogDual(text As String, html As String)
            If text <> Nothing Then
                _PlainTextWarningsLog.Append(text)
            End If
            If html <> Nothing Then
                _HtmlWarningsLog.Append(html)
            End If
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        Private Shared Sub WriteToWarningLogDual(text As System.Text.StringBuilder, html As System.Text.StringBuilder)
            _PlainTextWarningsLog.Append(text)
            _HtmlWarningsLog.Append(html)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        Public Shared Sub Write(text As String)
            _Write(text, True)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="arg0"></param>
        Public Shared Sub Write(text As String, arg0 As Object)
            Write(String.Format(text, arg0))
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="args"></param>
        Public Shared Sub Write(text As String, ParamArray args As Object())
            Write(String.Format(text, args))
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="colorForeground"></param>
        Public Shared Sub Write(text As String, colorForeground As System.ConsoleColor)
            Write(text, colorForeground, BackgroundColor)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        Public Shared Sub Write(text As System.Text.StringBuilder)
            Write(text, ForegroundColor, BackgroundColor)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="colorForeground"></param>
        Public Shared Sub Write(text As System.Text.StringBuilder, colorForeground As System.ConsoleColor)
            Write(text, colorForeground, BackgroundColor)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="colorForeground"></param>
        ''' <param name="colorBackground"></param>
        Public Shared Sub Write(text As System.Text.StringBuilder, colorForeground As System.ConsoleColor, colorBackground As System.ConsoleColor)
            Dim CurrentForeColor As System.ConsoleColor = ForegroundColor
            Dim CurrentBackColor As System.ConsoleColor = BackgroundColor
            ForegroundColor = colorForeground
            BackgroundColor = colorBackground
            _Write(text, True)
            ForegroundColor = CurrentForeColor
            BackgroundColor = CurrentBackColor
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="colorForeground"></param>
        ''' <param name="colorBackground"></param>
        Public Shared Sub Write(text As String, colorForeground As System.ConsoleColor, colorBackground As System.ConsoleColor)
            Dim CurrentForeColor As System.ConsoleColor = ForegroundColor
            Dim CurrentBackColor As System.ConsoleColor = BackgroundColor
            ForegroundColor = colorForeground
            BackgroundColor = colorBackground
            Write(text)
            ForegroundColor = CurrentForeColor
            BackgroundColor = CurrentBackColor
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        Public Shared Sub WriteDual(text As String, html As String)
            _WriteDual(text, html, True)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        ''' <param name="arg0"></param>
        Public Shared Sub WriteDual(text As String, html As String, arg0 As Object)
            WriteDual(String.Format(text, arg0), String.Format(text, arg0))
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        ''' <param name="args"></param>
        Public Shared Sub WriteDual(text As String, html As String, ParamArray args As Object())
            WriteDual(String.Format(text, args), String.Format(html, args))
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        ''' <param name="colorForeground"></param>
        Public Shared Sub WriteDual(text As String, html As String, colorForeground As System.ConsoleColor)
            WriteDual(text, html, colorForeground, BackgroundColor)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        Public Shared Sub WriteDual(text As System.Text.StringBuilder, html As System.Text.StringBuilder)
            WriteDual(text, html, ForegroundColor, BackgroundColor)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        ''' <param name="colorForeground"></param>
        Public Shared Sub WriteDual(text As System.Text.StringBuilder, html As System.Text.StringBuilder, colorForeground As System.ConsoleColor)
            WriteDual(text, html, colorForeground, BackgroundColor)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        ''' <param name="colorForeground"></param>
        ''' <param name="colorBackground"></param>
        Public Shared Sub WriteDual(text As System.Text.StringBuilder, html As System.Text.StringBuilder, colorForeground As System.ConsoleColor, colorBackground As System.ConsoleColor)
            Dim CurrentForeColor As System.ConsoleColor = ForegroundColor
            Dim CurrentBackColor As System.ConsoleColor = BackgroundColor
            ForegroundColor = colorForeground
            BackgroundColor = colorBackground
            _WriteDual(text, html, True)
            ForegroundColor = CurrentForeColor
            BackgroundColor = CurrentBackColor
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        ''' <param name="colorForeground"></param>
        ''' <param name="colorBackground"></param>
        Public Shared Sub WriteDual(text As String, html As String, colorForeground As System.ConsoleColor, colorBackground As System.ConsoleColor)
            Dim CurrentForeColor As System.ConsoleColor = ForegroundColor
            Dim CurrentBackColor As System.ConsoleColor = BackgroundColor
            ForegroundColor = colorForeground
            BackgroundColor = colorBackground
            WriteDual(text, html)
            ForegroundColor = CurrentForeColor
            BackgroundColor = CurrentBackColor
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        Public Shared Sub WriteLine()
            WriteLine("", SystemConsoleDefaultForegroundColor, SystemConsoleDefaultBackgroundColor)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        Public Shared Sub WriteLine(text As String)
            Write(text & System.Environment.NewLine)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        Public Shared Sub WriteLine(text As System.Text.StringBuilder)
            WriteLine(text, ForegroundColor, BackgroundColor)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="colorForeground"></param>
        Public Shared Sub WriteLine(text As System.Text.StringBuilder, colorForeground As System.ConsoleColor)
            WriteLine(text, colorForeground, BackgroundColor)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="colorForeground"></param>
        ''' <param name="colorBackground"></param>
        Public Shared Sub WriteLine(text As System.Text.StringBuilder, colorForeground As System.ConsoleColor, colorBackground As System.ConsoleColor)
            Dim CurrentForeColor As System.ConsoleColor = ForegroundColor
            Dim CurrentBackColor As System.ConsoleColor = BackgroundColor
            ForegroundColor = colorForeground
            BackgroundColor = colorBackground
            Write(text)
            Write(System.Environment.NewLine)
            ForegroundColor = CurrentForeColor
            BackgroundColor = CurrentBackColor
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="arg0"></param>
        Public Shared Sub WriteLine(text As String, arg0 As Object)
            WriteLine(String.Format(text, arg0))
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="args"></param>
        Public Shared Sub WriteLine(text As String, ParamArray args As Object())
            WriteLine(String.Format(text, args))
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="colorForeground"></param>
        Public Shared Sub WriteLine(text As String, colorForeground As System.ConsoleColor)
            Write(text & System.Environment.NewLine, colorForeground)
        End Sub

        ''' <summary>
        ''' Write message with specified color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="colorForeground"></param>
        ''' <param name="colorBackground"></param>
        Public Shared Sub WriteLine(text As String, colorForeground As System.ConsoleColor, colorBackground As System.ConsoleColor)
            Write(text & System.Environment.NewLine, colorForeground, colorBackground)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        Public Shared Sub WriteLineDual()
            WriteDual(System.Environment.NewLine, "<br />")
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        Public Shared Sub WriteLineDual(text As String, html As String)
            WriteDual(text & System.Environment.NewLine, html & "<br />")
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        Public Shared Sub WriteLineDual(text As System.Text.StringBuilder, html As System.Text.StringBuilder)
            WriteLineDual(text, html, ForegroundColor, BackgroundColor)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        ''' <param name="colorForeground"></param>
        Public Shared Sub WriteLineDual(text As System.Text.StringBuilder, html As System.Text.StringBuilder, colorForeground As System.ConsoleColor)
            WriteLineDual(text, html, colorForeground, BackgroundColor)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        ''' <param name="colorForeground"></param>
        ''' <param name="colorBackground"></param>
        Public Shared Sub WriteLineDual(text As System.Text.StringBuilder, html As System.Text.StringBuilder, colorForeground As System.ConsoleColor, colorBackground As System.ConsoleColor)
            Dim CurrentForeColor As System.ConsoleColor = ForegroundColor
            Dim CurrentBackColor As System.ConsoleColor = BackgroundColor
            ForegroundColor = colorForeground
            BackgroundColor = colorBackground
            WriteDual(text, html)
            Write(System.Environment.NewLine)
            ForegroundColor = CurrentForeColor
            BackgroundColor = CurrentBackColor
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        ''' <param name="arg0"></param>
        Public Shared Sub WriteLineDual(text As String, html As String, arg0 As Object)
            WriteLineDual(String.Format(text, arg0), String.Format(html, arg0))
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        ''' <param name="args"></param>
        Public Shared Sub WriteLineDual(text As String, html As String, ParamArray args As Object())
            WriteLineDual(String.Format(text, args), String.Format(html, args))
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        ''' <param name="colorForeground"></param>
        Public Shared Sub WriteLineDual(text As String, html As String, colorForeground As System.ConsoleColor)
            WriteDual(text & System.Environment.NewLine, html & "<br />", colorForeground)
        End Sub

        ''' <summary>
        ''' Write message with specified color settings
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        ''' <param name="colorForeground"></param>
        ''' <param name="colorBackground"></param>
        Public Shared Sub WriteLineDual(text As String, html As String, colorForeground As System.ConsoleColor, colorBackground As System.ConsoleColor)
            WriteDual(text & System.Environment.NewLine, html & "<br />", colorForeground, colorBackground)
        End Sub

        ''' <summary>
        ''' Write with color setting for status warning messages
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        Public Shared Sub Warn(text As String)
            HasWarnings = True
            Dim CurrentForeColor As System.ConsoleColor = ForegroundColor
            Dim CurrentBackColor As System.ConsoleColor = BackgroundColor
            ForegroundColor = WarningForegroundColor
            BackgroundColor = WarningBackgroundColor
            Write(text)
            WriteToWarningLog(text)
            ForegroundColor = CurrentForeColor
            BackgroundColor = CurrentBackColor
        End Sub

        ''' <summary>
        ''' Write with color setting for status warning messages
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        Public Shared Sub Warn(text As System.Text.StringBuilder)
            HasWarnings = True
            Dim CurrentForeColor As System.ConsoleColor = ForegroundColor
            Dim CurrentBackColor As System.ConsoleColor = BackgroundColor
            ForegroundColor = WarningForegroundColor
            BackgroundColor = WarningBackgroundColor
            Write(text)
            WriteToWarningLog(text)
            ForegroundColor = CurrentForeColor
            BackgroundColor = CurrentBackColor
        End Sub

        ''' <summary>
        ''' Write with color setting for status warning messages
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        Public Shared Sub WarnDual(text As String, html As String)
            HasWarnings = True
            Dim CurrentForeColor As System.ConsoleColor = ForegroundColor
            Dim CurrentBackColor As System.ConsoleColor = BackgroundColor
            ForegroundColor = WarningForegroundColor
            BackgroundColor = WarningBackgroundColor
            WriteDual(text, html)
            WriteToWarningLogDual(text, html)
            ForegroundColor = CurrentForeColor
            BackgroundColor = CurrentBackColor
        End Sub

        ''' <summary>
        ''' Write with color setting for status warning messages
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        Public Shared Sub WarnDual(text As System.Text.StringBuilder, html As System.Text.StringBuilder)
            HasWarnings = True
            Dim CurrentForeColor As System.ConsoleColor = ForegroundColor
            Dim CurrentBackColor As System.ConsoleColor = BackgroundColor
            ForegroundColor = WarningForegroundColor
            BackgroundColor = WarningBackgroundColor
            WriteDual(text, html)
            WriteToWarningLogDual(text, html)
            ForegroundColor = CurrentForeColor
            BackgroundColor = CurrentBackColor
        End Sub

        ''' <summary>
        ''' Write with color setting for status warning messages
        ''' </summary>
        Public Shared Sub WarnLine()
            HasWarnings = True
            WriteLine("", SystemConsoleDefaultForegroundColor, SystemConsoleDefaultBackgroundColor)
            WriteToWarningLog(System.Environment.NewLine)
        End Sub

        ''' <summary>
        ''' Write with color setting for status warning messages
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        Public Shared Sub WarnLine(text As String)
            Warn(text & System.Environment.NewLine)
        End Sub

        ''' <summary>
        ''' Write with color setting for status warning messages
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        Public Shared Sub WarnLine(text As System.Text.StringBuilder)
            Warn(text)
            Warn(System.Environment.NewLine)
        End Sub

        ''' <summary>
        ''' Write with color setting for status warning messages
        ''' </summary>
        Public Shared Sub WarnLineDual()
            HasWarnings = True
            WriteLineDual("", "", SystemConsoleDefaultForegroundColor, SystemConsoleDefaultBackgroundColor)
            WriteToWarningLog(System.Environment.NewLine)
        End Sub

        ''' <summary>
        ''' Write with color setting for status warning messages
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        Public Shared Sub WarnLineDual(text As String, html As String)
            WarnDual(text & System.Environment.NewLine, html & "<br />")
        End Sub

        ''' <summary>
        ''' Write with color setting for status warning messages
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        Public Shared Sub WarnLineDual(text As System.Text.StringBuilder, html As System.Text.StringBuilder)
            WarnDual(text, html)
            Warn(System.Environment.NewLine)
        End Sub

        ''' <summary>
        ''' Write with color setting for status okay messages
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        Public Shared Sub Okay(text As String)
            Dim CurrentForeColor As System.ConsoleColor = ForegroundColor
            Dim CurrentBackColor As System.ConsoleColor = BackgroundColor
            ForegroundColor = OkayMessageForegroundColor
            BackgroundColor = OkayMessageBackgroundColor
            Write(text)
            ForegroundColor = CurrentForeColor
            BackgroundColor = CurrentBackColor
        End Sub

        ''' <summary>
        ''' Write with color setting for status okay messages
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        Public Shared Sub Okay(text As System.Text.StringBuilder)
            Dim CurrentForeColor As System.ConsoleColor = ForegroundColor
            Dim CurrentBackColor As System.ConsoleColor = BackgroundColor
            ForegroundColor = OkayMessageForegroundColor
            BackgroundColor = OkayMessageBackgroundColor
            Write(text)
            ForegroundColor = CurrentForeColor
            BackgroundColor = CurrentBackColor
        End Sub

        ''' <summary>
        ''' Write with color setting for status okay messages
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        Public Shared Sub OkayDual(text As String, html As String)
            Dim CurrentForeColor As System.ConsoleColor = ForegroundColor
            Dim CurrentBackColor As System.ConsoleColor = BackgroundColor
            ForegroundColor = OkayMessageForegroundColor
            BackgroundColor = OkayMessageBackgroundColor
            WriteDual(text, html)
            ForegroundColor = CurrentForeColor
            BackgroundColor = CurrentBackColor
        End Sub

        ''' <summary>
        ''' Write with color setting for status okay messages
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        Public Shared Sub OkayDual(text As System.Text.StringBuilder, html As System.Text.StringBuilder)
            Dim CurrentForeColor As System.ConsoleColor = ForegroundColor
            Dim CurrentBackColor As System.ConsoleColor = BackgroundColor
            ForegroundColor = OkayMessageForegroundColor
            BackgroundColor = OkayMessageBackgroundColor
            WriteDual(text, html)
            ForegroundColor = CurrentForeColor
            BackgroundColor = CurrentBackColor
        End Sub


        ''' <summary>
        ''' Write with color setting for status okay messages
        ''' </summary>
        Public Shared Sub OkayLine()
            WriteLine("", SystemConsoleDefaultForegroundColor, SystemConsoleDefaultBackgroundColor)
        End Sub

        ''' <summary>
        ''' Write with color setting for status okay messages
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        Public Shared Sub OkayLine(text As String)
            Okay(text & System.Environment.NewLine)
        End Sub

        ''' <summary>
        ''' Write with color setting for status okay messages
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        Public Shared Sub OkayLine(text As System.Text.StringBuilder)
            Okay(text)
            Okay(System.Environment.NewLine)
        End Sub

        '''''''''

        ''' <summary>
        ''' Write with color setting for status okay messages
        ''' </summary>
        Public Shared Sub OkayLineDual()
            WriteLine("", "", SystemConsoleDefaultForegroundColor, SystemConsoleDefaultBackgroundColor)
        End Sub

        ''' <summary>
        ''' Write with color setting for status okay messages
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        Public Shared Sub OkayLineDual(text As String, html As String)
            OkayDual(text & System.Environment.NewLine, html & "<br />")
        End Sub

        ''' <summary>
        ''' Write with color setting for status okay messages
        ''' </summary>
        ''' <param name="text">Plain text to be logged</param>
        ''' <param name="html">HTML code to be logged</param>
        Public Shared Sub OkayLineDual(text As System.Text.StringBuilder, html As System.Text.StringBuilder)
            OkayDual(text, html)
            Okay(System.Environment.NewLine)
        End Sub

        ''' <summary>
        ''' The log data as raw text content 
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function PlainTextLog() As System.Text.StringBuilder
            Dim FileContent As System.Text.StringBuilder = CloneStringBuilder(_RawPlainTextLog)
            Dim Colors As Array = [Enum].GetValues(GetType(System.ConsoleColor))
            For Each Color In Colors
                Dim ColorName As String = ConsoleColorSystemName(CType(Color, System.ConsoleColor))
                FileContent.Replace("<BACKCOLOR:" & ColorName & ">", "")
                FileContent.Replace("</BACKCOLOR:" & ColorName & ">", "")
                FileContent.Replace("<FORECOLOR:" & ColorName & ">", "")
                FileContent.Replace("</FORECOLOR:" & ColorName & ">", "")
            Next
            Return FileContent
        End Function

        ''' <summary>
        ''' The log data as raw text content incl. some tags for style of output
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function RawPlainTextLog() As System.Text.StringBuilder
            Return _RawPlainTextLog
        End Function

        ''' <summary>
        ''' The log data as HTML content
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function HtmlLog() As System.Text.StringBuilder
            Return _HtmlLog
        End Function

        ''' <summary>
        ''' The log data as raw text content incl. some tags for style of output
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function WarningsPlainTextLog() As System.Text.StringBuilder
            Return _PlainTextWarningsLog
        End Function

        ''' <summary>
        ''' The log data as HTML content
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function WarningsHtmlLog() As System.Text.StringBuilder
            Return _HtmlWarningsLog
        End Function

        ''' <summary>
        ''' Add tags HTML + BODY around the log data
        ''' </summary>
        ''' <param name="wrapAroundHtmlAndBodyTags"></param>
        ''' <returns></returns>
        Public Shared Function HtmlLog(wrapAroundHtmlAndBodyTags As Boolean) As System.Text.StringBuilder
            Return HtmlLog(_HtmlLog, wrapAroundHtmlAndBodyTags, "", "", "")
        End Function

        ''' <summary>
        ''' Add tags HTML + BODY around the log data
        ''' </summary>
        ''' <param name="wrapAroundHtmlAndBodyTags"></param>
        ''' <returns></returns>
        Private Shared Function HtmlLog(log As System.Text.StringBuilder, wrapAroundHtmlAndBodyTags As Boolean) As System.Text.StringBuilder
            Return HtmlLog(log, wrapAroundHtmlAndBodyTags, "", "", "")
        End Function

        ''' <summary>
        ''' Add tags HTML + HEAD incl. TITLE + BODY around the log data
        ''' </summary>
        ''' <param name="pageTitle">A page title (will be HTML encoded)</param>
        ''' <returns></returns>
        Public Shared Function HtmlLog(pageTitle As String) As System.Text.StringBuilder
            Return HtmlLog(_HtmlLog, True, "<title>" & System.Net.WebUtility.HtmlEncode(pageTitle) & "</title>", "", "")
        End Function

        ''' <summary>
        ''' Add tags HTML + HEAD incl. TITLE + BODY around the log data
        ''' </summary>
        ''' <param name="pageTitle">A page title (will be HTML encoded)</param>
        ''' <returns></returns>
        Private Shared Function HtmlLog(log As System.Text.StringBuilder, pageTitle As String) As System.Text.StringBuilder
            Return HtmlLog(log, True, "<title>" & System.Net.WebUtility.HtmlEncode(pageTitle) & "</title>", "", "")
        End Function

        ''' <summary>
        ''' Add tags HTML + HEAD incl. TITLE + BODY around the log data
        ''' </summary>
        ''' <param name="headContent">HTML code for inside HEAD tag</param>
        ''' <param name="bodyPreContent">HTML code for inside BODY tag before the log data</param>
        ''' <param name="bodyPostContent">HTML code for inside BODY tag after the log data</param>
        ''' <returns></returns>
        Public Shared Function HtmlLog(headContent As String, bodyPreContent As String, bodyPostContent As String) As System.Text.StringBuilder
            If headContent <> Nothing OrElse bodyPreContent <> Nothing OrElse bodyPostContent <> Nothing Then
                Return HtmlLog(_HtmlLog, True, headContent, bodyPreContent, bodyPostContent)
            Else
                Return HtmlLog(_HtmlLog, False, "", "", "")
            End If
        End Function

        ''' <summary>
        ''' Add tags HTML + HEAD incl. TITLE + BODY around the log data
        ''' </summary>
        ''' <param name="wrapAroundHtmlAndBodyTags"></param>
        ''' <param name="headContent">HTML code for inside HEAD tag</param>
        ''' <param name="bodyPreContent">HTML code for inside BODY tag before the log data</param>
        ''' <param name="bodyPostContent">HTML code for inside BODY tag after the log data</param>
        ''' <returns></returns>
        Private Shared Function HtmlLog(log As System.Text.StringBuilder, wrapAroundHtmlAndBodyTags As Boolean, headContent As String, bodyPreContent As String, bodyPostContent As String) As System.Text.StringBuilder
            If wrapAroundHtmlAndBodyTags = False And headContent <> Nothing Then
                Throw New ArgumentException("wrapAroundHtmlAndBodyTags must be true if pageTitle has got a value")
            End If
            Dim BackColorname As String = ConsoleColorCssName(SystemConsoleDefaultBackgroundColor)
            Dim ForeColorname As String = ConsoleColorCssName(SystemConsoleDefaultForegroundColor)
            Dim FileContent As New System.Text.StringBuilder
            If wrapAroundHtmlAndBodyTags Then
                FileContent.Append("<html>" & System.Environment.NewLine)
                If headContent <> Nothing Then
                    FileContent.Append("<head>" & headContent & "</head>" & System.Environment.NewLine)
                End If
                FileContent.Append("<body style=""background-color: " & BackColorname & "; color: " & ForeColorname & ";"">" & System.Environment.NewLine)
                FileContent.Append(bodyPreContent)
                FileContent.Append("<span style=""color: " & ForeColorname & ";"">" + log.ToString + "</span>" & System.Environment.NewLine)
                FileContent.Append(bodyPostContent)
                FileContent.Append("</body></html>")
                Return FileContent
            ElseIf bodyPreContent <> Nothing OrElse bodyPostContent <> Nothing Then
                FileContent.Append(bodyPreContent)
                FileContent.Append(log)
                FileContent.Append(bodyPostContent)
                Return FileContent
            Else
                Return log
            End If
        End Function

        ''' <summary>
        ''' Save the log data as plain text to a file
        ''' </summary>
        ''' <param name="path"></param>
        Public Shared Sub SavePlainTextLog(path As String)
            SavePlainTextLog(path, DefaultSaveFileEncoding)
        End Sub

        ''' <summary>
        ''' Save the log data as plain text to a file
        ''' </summary>
        ''' <param name="path"></param>
        Public Shared Sub SavePlainTextLog(path As String, encoding As System.Text.Encoding)
            System.IO.File.WriteAllText(path, PlainTextLog.ToString, encoding)
        End Sub

        ''' <summary>
        ''' Save the log data as raw text (contains internal tags for e.g. color output) to a file
        ''' </summary>
        ''' <param name="path"></param>
        Public Shared Sub SaveRawPlainTextLog(path As String)
            SaveRawPlainTextLog(path, DefaultSaveFileEncoding)
        End Sub

        ''' <summary>
        ''' Save the log data as raw text (contains internal tags for e.g. color output) to a file
        ''' </summary>
        ''' <param name="path"></param>
        Public Shared Sub SaveRawPlainTextLog(path As String, encoding As System.Text.Encoding)
            System.IO.File.WriteAllText(path, RawPlainTextLog.ToString, encoding)
        End Sub

        ''' <summary>
        ''' Save the log data as HTML text to a file
        ''' </summary>
        ''' <param name="path"></param>
        Public Shared Sub SaveHtmlLog(path As String)
            SaveHtmlLog(path, "Log: " & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
        End Sub

        ''' <summary>
        ''' Save the log data as HTML text to a file
        ''' </summary>
        ''' <param name="path"></param>
        ''' <param name="pageTitle"></param>
        Public Shared Sub SaveHtmlLog(path As String, pageTitle As String)
            SaveHtmlLog(path, pageTitle, DefaultSaveFileEncoding)
        End Sub

        ''' <summary>
        ''' Save the log data as HTML text to a file
        ''' </summary>
        ''' <param name="path"></param>
        ''' <param name="pageTitle"></param>
        Public Shared Sub SaveHtmlLog(path As String, pageTitle As String, encoding As System.Text.Encoding)
            System.IO.File.WriteAllText(path, HtmlLog(pageTitle).ToString, encoding)
        End Sub

        ''' <summary>
        ''' Save the log data as HTML text to a file
        ''' </summary>
        ''' <param name="path"></param>
        ''' <param name="headContent">HTML code for inside HEAD tag</param>
        ''' <param name="bodyPreContent">HTML code for inside BODY tag before the log data</param>
        ''' <param name="bodyPostContent">HTML code for inside BODY tag after the log data</param>
        Public Shared Sub SaveHtmlLog(path As String, headContent As String, bodyPreContent As String, bodyPostContent As String)
            SaveHtmlLog(path, headContent, bodyPreContent, bodyPostContent, DefaultSaveFileEncoding)
        End Sub

        ''' <summary>
        ''' Save the log data as HTML text to a file
        ''' </summary>
        ''' <param name="path"></param>
        ''' <param name="headContent">HTML code for inside HEAD tag</param>
        ''' <param name="bodyPreContent">HTML code for inside BODY tag before the log data</param>
        ''' <param name="bodyPostContent">HTML code for inside BODY tag after the log data</param>
        Public Shared Sub SaveHtmlLog(path As String, headContent As String, bodyPreContent As String, bodyPostContent As String, encoding As System.Text.Encoding)
            If headContent <> Nothing OrElse bodyPreContent <> Nothing OrElse bodyPostContent <> Nothing Then
                System.IO.File.WriteAllText(path, HtmlLog(_HtmlLog, True, headContent, bodyPreContent, bodyPostContent).ToString, encoding)
            Else
                System.IO.File.WriteAllText(path, HtmlLog(_HtmlLog, False).ToString, encoding)
            End If
        End Sub

        ''' <summary>
        ''' Save the log data as plain text to a file
        ''' </summary>
        ''' <param name="path"></param>
        Public Shared Sub SaveWarningsPlainTextLog(path As String)
            SaveWarningsPlainTextLog(path, DefaultSaveFileEncoding)
        End Sub

        ''' <summary>
        ''' Save the log data as plain text to a file
        ''' </summary>
        ''' <param name="path"></param>
        Public Shared Sub SaveWarningsPlainTextLog(path As String, encoding As System.Text.Encoding)
            System.IO.File.WriteAllText(path, WarningsPlainTextLog.ToString, encoding)
        End Sub

        ''' <summary>
        ''' Save the log data as HTML text to a file
        ''' </summary>
        ''' <param name="path"></param>
        Public Shared Sub SaveWarningsHtmlLog(path As String)
            SaveWarningsHtmlLog(path, "Log: " & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
        End Sub

        ''' <summary>
        ''' Save the log data as HTML text to a file
        ''' </summary>
        ''' <param name="path"></param>
        ''' <param name="pageTitle"></param>
        Public Shared Sub SaveWarningsHtmlLog(path As String, pageTitle As String)
            SaveWarningsHtmlLog(path, pageTitle, DefaultSaveFileEncoding)
        End Sub

        ''' <summary>
        ''' Save the log data as HTML text to a file
        ''' </summary>
        ''' <param name="path"></param>
        ''' <param name="pageTitle"></param>
        Public Shared Sub SaveWarningsHtmlLog(path As String, pageTitle As String, encoding As System.Text.Encoding)
            System.IO.File.WriteAllText(path, HtmlLog(_HtmlWarningsLog, pageTitle).ToString, encoding)
        End Sub

        ''' <summary>
        ''' Save the log data as HTML text to a file
        ''' </summary>
        ''' <param name="path"></param>
        ''' <param name="headContent">HTML code for inside HEAD tag</param>
        ''' <param name="bodyPreContent">HTML code for inside BODY tag before the log data</param>
        ''' <param name="bodyPostContent">HTML code for inside BODY tag after the log data</param>
        Public Shared Sub SaveWarningsHtmlLog(path As String, headContent As String, bodyPreContent As String, bodyPostContent As String)
            SaveWarningsHtmlLog(path, headContent, bodyPreContent, bodyPostContent, DefaultSaveFileEncoding)
        End Sub

        ''' <summary>
        ''' Save the log data as HTML text to a file
        ''' </summary>
        ''' <param name="path"></param>
        ''' <param name="headContent">HTML code for inside HEAD tag</param>
        ''' <param name="bodyPreContent">HTML code for inside BODY tag before the log data</param>
        ''' <param name="bodyPostContent">HTML code for inside BODY tag after the log data</param>
        Public Shared Sub SaveWarningsHtmlLog(path As String, headContent As String, bodyPreContent As String, bodyPostContent As String, encoding As System.Text.Encoding)
            If headContent <> Nothing OrElse bodyPreContent <> Nothing OrElse bodyPostContent <> Nothing Then
                System.IO.File.WriteAllText(path, HtmlLog(WarningsHtmlLog, True, headContent, bodyPreContent, bodyPostContent).ToString, encoding)
            Else
                System.IO.File.WriteAllText(path, HtmlLog(WarningsHtmlLog, False).ToString, encoding)
            End If
        End Sub

        Private Shared ReadOnly Property DefaultSaveFileEncoding As System.Text.Encoding = New System.Text.UTF8Encoding(True)

        ''' <summary>
        ''' The color name as used in .NET
        ''' </summary>
        ''' <param name="color"></param>
        ''' <returns></returns>
        Friend Shared Function ConsoleColorSystemName(color As System.ConsoleColor) As String
            Return [Enum].GetName(GetType(System.ConsoleColor), color)
        End Function

        ''' <summary>
        ''' The color name as used in CSS styles
        ''' </summary>
        ''' <param name="color"></param>
        ''' <returns></returns>
        Friend Shared Function ConsoleColorCssName(color As System.ConsoleColor) As String
            Dim SystemColorName As String = ConsoleColorSystemName(color)
            If SystemColorName = Nothing Then
                If color.ToString("d") = "-1" Then
                    'Linux/Mac environments: value for default differs from Windows platforms
                    Return "Black"
                Else
                    Return "invalid-console-color(" & color.ToString("d") & ")"
                End If
            ElseIf SystemColorName.ToLowerInvariant.StartsWith("dark") Then
                Return SystemColorName
            Else
                Return SystemColorName.Replace("Gray", "LightGray")
            End If
        End Function

        '''' <summary>
        '''' Convert a ConsoleColor to a CSS representation of rgb(xx,xx,xx)
        '''' </summary>
        '''' <param name="color"></param>
        '''' <returns></returns>
        'Private Shared Function ConsoleColorToCssColor(color As System.ConsoleColor) As String
        '    Dim Hex As String = color.ToString("x")
        '    Dim Red As Integer = Convert.ToInt32(Hex.Substring(2, 2), 16)
        '    Dim Green As Integer = Convert.ToInt32(Hex.Substring(4, 2), 16)
        '    Dim Blue As Integer = Convert.ToInt32(Hex.Substring(6, 2), 16)
        '    Return "rgb(" & Red & "," & Green & "," & Blue & ")"
        'End Function

        ''' <summary>
        ''' Involves the system speaker or system audio system to make a beep sound
        ''' </summary>
        Public Shared Sub Beep()
            System.Console.Beep()
        End Sub

        ''' <summary>
        ''' Reset the foreground and background color to the system defaults
        ''' </summary>
        Public Shared Sub ResetColor()
            'following statements typically should lead to the very same result as just executing System.Console.ResetColor()
            ForegroundColor = SystemConsoleDefaultForegroundColor
            BackgroundColor = SystemConsoleDefaultBackgroundColor
        End Sub

#Region "Catch Control-C"
        ''' <summary>
        ''' Raise the exception for ControlC KeyPressed 
        ''' </summary>
        Public Shared Sub RaiseControlCKeyPressedException()
            RaiseControlCKeyPressedException(New ControlCKeyPressedException)
        End Sub

        Private Shared Sub RaiseControlCKeyPressedException(ex As ControlCKeyPressedException)
            _ControlCKeyPressed = ex
        End Sub

        Private Shared Sub ControlC_KeyPressed(ByVal sender As Object, ByVal args As ConsoleCancelEventArgs)
            args.Cancel = True
            Console.WriteLine()
            Console.WriteLine()
            Console.WarnLine("The operation has been interrupted by user (<CTRL>+<C>), aborting . . .")
            Console.RaiseControlCKeyPressedException(New Console.ControlCKeyPressedException)
        End Sub

        Private Shared _ControlCKeyPressed As ControlCKeyPressedException
        ''' <summary>
        ''' When Control-C keyboard shortcut has been used, this exception provides details
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property ControlCKeyPressed As ControlCKeyPressedException
            Get
                Return _ControlCKeyPressed
            End Get
        End Property

        ''' <summary>
        ''' Indicates if Control-C keyboard shortcut has been used
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property IsControlCKeyPressed As Boolean
            Get
                If ControlCKeyPressed Is Nothing Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        ''' <summary>
        ''' If enabled, the next call to CompuMaster.Console.Write/Warn/Okay(Line) will throw the catched ControlCKeyPressedException (defaults to true)
        ''' </summary>
        ''' <remarks>
        ''' If set to false, the application must check regularly by itself for IsControlCKeyPressed to handle the Control-C input from user
        ''' </remarks>
        ''' <returns></returns>
        Public Shared Property ThrowControlCKeyPressedExceptionOnNextConsoleCommand As Boolean = True

        Private Shared _CatchControlC As Boolean = False
        ''' <summary>
        ''' Shall keyboard shortcut Control-C be catched so that the application can continue running (e.g. until it's able to write log entries and shutdown safely)
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property CatchControlC As Boolean
            Get
                Return _CatchControlC
            End Get
            Set(value As Boolean)
                'Always remove handler first (also in case we enable it immediately again - but this ensures that it's not a duplicate adding of the same handler)
                RemoveHandler System.Console.CancelKeyPress, AddressOf ControlC_KeyPressed
                If value = True Then
                    AddHandler System.Console.CancelKeyPress, AddressOf ControlC_KeyPressed
                End If
                _CatchControlC = value
            End Set
        End Property
#End Region

        ''' <summary>
        ''' Obtains the next character or function key press by the user. The pressed key is displayed at console only (not in logs).
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function ReadKey() As System.ConsoleKeyInfo
            Return System.Console.ReadKey
        End Function

        ''' <summary>
        ''' Ask for user input confirmed with a Enter key
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function ReadLine() As String
            Return ReadLine(True)
        End Function

        ''' <summary>
        ''' Ask for user input confirmed with a Enter key
        ''' </summary>
        ''' <param name="showInputInLogs">For sensitive data, you might want to hide input text in logs</param>
        ''' <returns></returns>
        Public Shared Function ReadLine(showInputInLogs As Boolean) As String
            Dim Result As String = System.Console.ReadLine()
            If showInputInLogs = True Then
                _Write(Result, False)
                _Write(System.Environment.NewLine, False)
            End If
            Return Result
        End Function

        Public Shared Property TreatControlCAsInput As Boolean
            Get
                Return System.Console.TreatControlCAsInput
            End Get
            Set(value As Boolean)
                System.Console.TreatControlCAsInput = value
            End Set
        End Property
        ''' <summary>
        ''' The console window title bar
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property Title As String
            Get
                Return System.Console.Title
            End Get
            Set(value As String)
                System.Console.Title = value
            End Set
        End Property
        Public Shared Property WindowTop As Integer
            Get
                Return System.Console.WindowTop
            End Get
            Set(value As Integer)
                System.Console.WindowTop = value
            End Set
        End Property
        Public Shared Property WindowLeft As Integer
            Get
                Return System.Console.WindowLeft
            End Get
            Set(value As Integer)
                System.Console.WindowLeft = value
            End Set
        End Property
        Public Shared Property WindowHeight As Integer
            Get
                Return System.Console.WindowHeight
            End Get
            Set(value As Integer)
                System.Console.WindowHeight = value
            End Set
        End Property
        Public Shared Property WindowWidth As Integer
            Get
                Return System.Console.WindowWidth
            End Get
            Set(value As Integer)
                System.Console.WindowWidth = value
            End Set
        End Property
        Public Shared Property BufferWidth As Integer
            Get
                Return System.Console.BufferWidth
            End Get
            Set(value As Integer)
                System.Console.BufferWidth = value
            End Set
        End Property
        Public Shared Property BufferHeight As Integer
            Get
                Return System.Console.BufferHeight
            End Get
            Set(value As Integer)
                System.Console.BufferHeight = value
            End Set
        End Property
        Public Shared ReadOnly Property CursorLeft As Integer
            Get
                Return System.Console.CursorLeft
            End Get
        End Property
        Public Shared Property CursorSize As Integer
            Get
                Return System.Console.CursorSize
            End Get
            Set(value As Integer)
                System.Console.CursorSize = value
            End Set
        End Property
        Public Shared ReadOnly Property CursorTop As Integer
            Get
                Return System.Console.CursorTop
            End Get
        End Property
        Public Shared ReadOnly Property CapsLock As Boolean
            Get
                Return System.Console.CapsLock
            End Get
        End Property
        Public Shared ReadOnly Property KeyAvailable As Boolean
            Get
                Return System.Console.KeyAvailable
            End Get
        End Property
        Public Shared Property CursorVisible As Boolean
            Get
                Return System.Console.CursorVisible
            End Get
            Set(value As Boolean)
                System.Console.CursorVisible = value
            End Set
        End Property
        Public Shared ReadOnly Property LargestWindowHeight As Integer
            Get
                Return System.Console.LargestWindowHeight
            End Get
        End Property
        Public Shared ReadOnly Property LargestWindowWidth As Integer
            Get
                Return System.Console.LargestWindowWidth
            End Get
        End Property
        Public Shared ReadOnly Property NumberLock As Boolean
            Get
                Return System.Console.NumberLock
            End Get
        End Property
        Public Shared Sub SetWindowsSize(width As Integer, height As Integer)
            System.Console.SetWindowSize(width, height)
        End Sub
        Public Shared Sub SetWindowPosition(left As Integer, top As Integer)
            System.Console.SetWindowPosition(left, top)
        End Sub

        Public Shared Sub SetBufferSize(width As Integer, height As Integer)
            System.Console.SetBufferSize(width, height)
            System.Console.Clear()
        End Sub

        ''' <summary>
        ''' Clear the content of console window (logs will append 3 line breaks)
        ''' </summary>
        Public Shared Sub Clear()
            Clear(True, True, False, False)
            _Write(System.Environment.NewLine & System.Environment.NewLine & System.Environment.NewLine, False)
        End Sub

        ''' <summary>
        ''' Clear the content of console window and/or logs
        ''' </summary>
        ''' <param name="warningStatus">Clears all warning logs (plain + HTML) and warning status</param>
        ''' <param name="consoleWindow">Clears the system's console buffer and corresponding console window of display information</param>
        ''' <param name="plainTextLog">Clears the log for plain text output</param>
        ''' <param name="htmlLog">Clears the log for HTML output</param>
        Public Shared Sub Clear(warningStatus As Boolean, consoleWindow As Boolean, plainTextLog As Boolean, htmlLog As Boolean)
            If warningStatus Then
                HasWarnings = False
                _HtmlWarningsLog.Clear()
                _PlainTextWarningsLog.Clear()
            End If
            If consoleWindow Then
                Try
                    System.Console.Clear()
                Catch ex As System.IO.IOException 'Das Handle ist ungültig
                    'ignore if console handle is invalid
                End Try
            End If
            If plainTextLog Then _RawPlainTextLog.Clear()
            If htmlLog Then _HtmlLog.Clear()
        End Sub

        ''' <summary>
        ''' Indicate if at least 1 warning has been written
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property HasWarnings As Boolean

        ''' <summary>
        ''' Use this string for each additional indentation level
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property IndentationStringPerIndentLevel As String = "".PadLeft(4)

        Private Shared _CurrentIndentationLevel As Integer = 0
        ''' <summary>
        ''' The number of indentations for every text output 
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property CurrentIndentationLevel As Integer
            Get
                Return _CurrentIndentationLevel
            End Get
            Set(value As Integer)
                If value < 0 Then Throw New ArgumentOutOfRangeException(NameOf(value), "Value must be > 0")
                _CurrentIndentationLevel = value
            End Set
        End Property

        ''' <summary>
        ''' Indent text based on the current number of indentation levels
        ''' </summary>
        ''' <param name="text"></param>
        ''' <returns></returns>
        Public Shared Function IndentText(text As System.Text.StringBuilder, continueStartedLine As Boolean) As System.Text.StringBuilder
            Return IndentText(text, _CurrentIndentationLevel, continueStartedLine)
        End Function

        ''' <summary>
        ''' Indent text based on the current number of indentation levels
        ''' </summary>
        ''' <param name="text"></param>
        ''' <param name="indentLevel"></param>
        ''' <returns></returns>
        Public Shared Function IndentText(text As System.Text.StringBuilder, indentLevel As Integer, continueStartedLine As Boolean) As System.Text.StringBuilder
            Dim FullIndentationPrefix As String = IndentationStringForCurrentIndentLevel(indentLevel)
            Dim Result As System.Text.StringBuilder = CloneStringBuilder(text)
            Result.Replace(ControlChars.CrLf, ControlChars.Lf)
            Result.Replace(ControlChars.Cr, ControlChars.Lf)
            Result.Replace(ControlChars.Lf, System.Environment.NewLine & FullIndentationPrefix)
            If FullIndentationPrefix <> Nothing Then
                If Result.Length >= FullIndentationPrefix.Length AndAlso Result.ToString(Result.Length - FullIndentationPrefix.Length, FullIndentationPrefix.Length) = FullIndentationPrefix Then
                    Result.Remove(Result.Length - FullIndentationPrefix.Length, FullIndentationPrefix.Length)
                End If
            End If
            If continueStartedLine = False Then
                Result.Insert(0, FullIndentationPrefix)
            End If
            Return Result
            'Return Replace(text, System.Environment.NewLine, System.Environment.NewLine & FullIndentationPrefix )
        End Function

        ''' <summary>
        ''' Indent text based on the current number of indentation levels
        ''' </summary>
        ''' <param name="text"></param>
        ''' <returns></returns>
        ''' <remarks>Only line breaks with HTML tag &lt;br /&gt; are considered (no paragraph tags, CSS or other types of line breaks)</remarks>
        Public Shared Function IndentTextInHtml(text As System.Text.StringBuilder, continueStartedLine As Boolean) As System.Text.StringBuilder
            Return IndentTextInHtml(text, _CurrentIndentationLevel, continueStartedLine)
        End Function

        ''' <summary>
        ''' Indent text based on the current number of indentation levels
        ''' </summary>
        ''' <param name="text"></param>
        ''' <param name="indentLevel"></param>
        ''' <returns></returns>
        ''' <remarks>Only line breaks with HTML tag &lt;br /&gt; are considered (no paragraph tags, CSS or other types of line breaks)</remarks>
        Public Shared Function IndentTextInHtml(text As System.Text.StringBuilder, indentLevel As Integer, continueStartedLine As Boolean) As System.Text.StringBuilder
            Dim FullIndentationPrefix As String = IndentationStringForCurrentIndentLevel(indentLevel).Replace(" ", "&nbsp;")
            Dim Result As System.Text.StringBuilder = CloneStringBuilder(text)
            Result.Replace("<br />", "<br />" & FullIndentationPrefix)
            Result.Replace("<br/>", "<br />" & FullIndentationPrefix)
            Result.Replace("<br>", "<br />" & FullIndentationPrefix)
            Result.Replace("<BR />", "<br />" & FullIndentationPrefix)
            Result.Replace("<BR/>", "<br />" & FullIndentationPrefix)
            Result.Replace("<BR>", "<br />" & FullIndentationPrefix)
            If FullIndentationPrefix <> Nothing Then
                If Result.Length >= FullIndentationPrefix.Length AndAlso Result.ToString(Result.Length - FullIndentationPrefix.Length, FullIndentationPrefix.Length) = FullIndentationPrefix Then
                    Result.Remove(Result.Length - FullIndentationPrefix.Length, FullIndentationPrefix.Length)
                End If
            End If
            If continueStartedLine = False Then
                Result.Insert(0, FullIndentationPrefix)
            End If
            Return Result
        End Function

        ''' <summary>
        ''' The full indentation string for the current indentation level 
        ''' </summary>
        ''' <returns></returns>
        Private Shared Function IndentationStringForCurrentIndentLevel(indentLevel As Integer) As String
            Dim Result As String = ""
            For MyCounter As Integer = 0 To indentLevel - 1
                Result &= IndentationStringPerIndentLevel
            Next
            Return Result
        End Function

        ''' <summary>
        ''' Clone a string builder while reducing required memory amount
        ''' </summary>
        ''' <param name="sb"></param>
        ''' <returns></returns>
        Friend Shared Function CloneStringBuilder(sb As System.Text.StringBuilder) As System.Text.StringBuilder
            Dim Result As New System.Text.StringBuilder
            AppendToStringBuilder(Result, sb)
            Return Result
        End Function

        ''' <summary>
        ''' Clone a string builder while reducing required memory amount
        ''' </summary>
        ''' <param name="source"></param>
        Friend Shared Sub AppendToStringBuilder(target As System.Text.StringBuilder, source As System.Text.StringBuilder)
            Dim MaxChunkSize As Integer = 100000 'for unit tests, use 20 instead
            Dim StartIndex As Integer = 0
            Dim ReadLength As Integer = MaxChunkSize
            Do
                If StartIndex + ReadLength > source.Length Then
                    ReadLength = source.Length - StartIndex
                End If
                If ReadLength <> 0 Then
                    target.Append(source.ToString(StartIndex, ReadLength))
                    StartIndex += ReadLength
                End If
            Loop Until ReadLength = 0
        End Sub

        ''' <summary>
        ''' Console output for StringBuilder and low memory usage
        ''' </summary>
        ''' <param name="source"></param>
        Friend Shared Sub AppendToConsole(source As System.Text.StringBuilder)
            Dim MaxChunkSize As Integer = 100000 'for unit tests, use 20 instead
            Dim StartIndex As Integer = 0
            Dim ReadLength As Integer = MaxChunkSize
            Do
                If StartIndex + ReadLength > source.Length Then
                    ReadLength = source.Length - StartIndex
                End If
                If ReadLength <> 0 Then
                    System.Console.Write(source.ToString(StartIndex, ReadLength))
                    StartIndex += ReadLength
                End If
            Loop Until ReadLength = 0
        End Sub

    End Class

End Namespace