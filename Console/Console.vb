Option Explicit On
Option Strict On

Namespace CompuMaster

    Partial Public Class Console

        Private Shared _PlainTextLog As New System.Text.StringBuilder
        Private Shared _HtmlLog As New System.Text.StringBuilder
        Private Shared ReadOnly SystemConsoleDefaultForegroundColor As System.ConsoleColor = System.Console.ForegroundColor
        Private Shared ReadOnly SystemConsoleDefaultBackgroundColor As System.ConsoleColor = System.Console.BackgroundColor

        Public Shared Property WarningForegroundColor As System.ConsoleColor = ConsoleColor.Red
        Public Shared Property WarningBackgroundColor As System.ConsoleColor = BackgroundColor
        Public Shared Property OkayMessageForegroundColor As System.ConsoleColor = ConsoleColor.Green
        Public Shared Property OkayMessageBackgroundColor As System.ConsoleColor = BackgroundColor

        Public Shared Property ForegroundColor As System.ConsoleColor
            Get
                Return System.Console.ForegroundColor
            End Get
            Set(value As System.ConsoleColor)
                System.Console.ForegroundColor = value
            End Set
        End Property

        Public Shared Property BackgroundColor As System.ConsoleColor
            Get
                Return System.Console.BackgroundColor
            End Get
            Set(value As System.ConsoleColor)
                System.Console.BackgroundColor = value
            End Set
        End Property

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text"></param>
        Public Shared Sub Write(text As String)
            _Write(text, True)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text"></param>
        ''' <param name="showConsoleOutput"></param>
        Private Shared Sub _Write(text As String, showConsoleOutput As Boolean)
            If IsControlCKeyPressed AndAlso ThrowControlCKeyPressedExceptionOnNextConsoleCommand Then
                Dim innerEx As ControlCKeyPressedException = ControlCKeyPressed
                _ControlCKeyPressed = Nothing 'don't raise for a 2nd time!
                Throw New ControlCKeyPressedException(innerEx)
            End If

            'System console
            System.Console.Write(text)

            'Plain text log
            If text = vbNewLine Then
                _PlainTextLog.Append(vbNewLine)
            Else
                If BackgroundColor <> SystemConsoleDefaultBackgroundColor Then
                    _PlainTextLog.Append("<BACKCOLOR:" & ConsoleColorSystemName(BackgroundColor) & ">")
                End If
                If ForegroundColor <> SystemConsoleDefaultForegroundColor Then
                    _PlainTextLog.Append("<FORECOLOR:" & ConsoleColorSystemName(ForegroundColor) & ">")
                End If
                _PlainTextLog.Append(text)
                If ForegroundColor <> SystemConsoleDefaultForegroundColor Then
                    _PlainTextLog.Append("</FORECOLOR:" & ConsoleColorSystemName(ForegroundColor) & ">")
                End If
                If BackgroundColor <> SystemConsoleDefaultBackgroundColor Then
                    _PlainTextLog.Append("</BACKCOLOR:" & ConsoleColorSystemName(BackgroundColor) & ">")
                End If
            End If

            'Html log
            If text = vbNewLine Then
                _HtmlLog.Append("<br />")
            Else
                Dim TextAsHtml As String = System.Net.WebUtility.HtmlEncode(text).Replace(vbNewLine, "<br />")
                If BackgroundColor <> SystemConsoleDefaultBackgroundColor Then
                    _HtmlLog.Append("<span style=""background-color: " & ConsoleColorCssName(BackgroundColor) & ";"">")
                End If
                If ForegroundColor <> SystemConsoleDefaultForegroundColor Then
                    _HtmlLog.Append("<span style=""color: " & ConsoleColorCssName(ForegroundColor) & ";"">")
                End If
                _HtmlLog.Append(TextAsHtml)
                If ForegroundColor <> SystemConsoleDefaultForegroundColor Then
                    _HtmlLog.Append("</span>")
                End If
                If BackgroundColor <> SystemConsoleDefaultBackgroundColor Then
                    _HtmlLog.Append("</span>")
                End If
            End If
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text"></param>
        ''' <param name="showConsoleOutput"></param>
        Private Shared Sub _Write(text As System.Text.StringBuilder, showConsoleOutput As Boolean)
            If IsControlCKeyPressed AndAlso ThrowControlCKeyPressedExceptionOnNextConsoleCommand Then
                Dim innerEx As ControlCKeyPressedException = ControlCKeyPressed
                _ControlCKeyPressed = Nothing 'don't raise for a 2nd time!
                Throw New ControlCKeyPressedException(innerEx)
            End If
            If text Is Nothing OrElse text.Length = 0 Then Return 'Empty content - nothing to do

            'System console
            System.Console.Write(text)

            'Plain text log
            If text.Length < 3 AndAlso text.ToString = vbNewLine Then
                _PlainTextLog.Append(vbNewLine)
            Else
                If BackgroundColor <> SystemConsoleDefaultBackgroundColor Then
                    _PlainTextLog.Append("<BACKCOLOR:" & ConsoleColorSystemName(BackgroundColor) & ">")
                End If
                If ForegroundColor <> SystemConsoleDefaultForegroundColor Then
                    _PlainTextLog.Append("<FORECOLOR:" & ConsoleColorSystemName(ForegroundColor) & ">")
                End If
                _PlainTextLog.Append(text)
                If ForegroundColor <> SystemConsoleDefaultForegroundColor Then
                    _PlainTextLog.Append("</FORECOLOR:" & ConsoleColorSystemName(ForegroundColor) & ">")
                End If
                If BackgroundColor <> SystemConsoleDefaultBackgroundColor Then
                    _PlainTextLog.Append("</BACKCOLOR:" & ConsoleColorSystemName(BackgroundColor) & ">")
                End If
            End If

            'Html log
            If text.Length < 3 AndAlso text.ToString = vbNewLine Then
                _HtmlLog.Append("<br />")
            Else
                Dim TextAsHtml As String = System.Net.WebUtility.HtmlEncode(text.ToString).Replace(vbNewLine, "<br />")
                If BackgroundColor <> SystemConsoleDefaultBackgroundColor Then
                    _HtmlLog.Append("<span style=""background-color: " & ConsoleColorCssName(BackgroundColor) & ";"">")
                End If
                If ForegroundColor <> SystemConsoleDefaultForegroundColor Then
                    _HtmlLog.Append("<span style=""color: " & ConsoleColorCssName(ForegroundColor) & ";"">")
                End If
                _HtmlLog.Append(TextAsHtml)
                If ForegroundColor <> SystemConsoleDefaultForegroundColor Then
                    _HtmlLog.Append("</span>")
                End If
                If BackgroundColor <> SystemConsoleDefaultBackgroundColor Then
                    _HtmlLog.Append("</span>")
                End If
            End If
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text"></param>
        ''' <param name="arg0"></param>
        Public Shared Sub Write(text As String, arg0 As Object)
            Write(String.Format(text, arg0))
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text"></param>
        ''' <param name="args"></param>
        Public Shared Sub Write(text As String, ParamArray args As Object())
            Write(String.Format(text, args))
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text"></param>
        ''' <param name="colorForeground"></param>
        Public Shared Sub Write(text As String, colorForeground As System.ConsoleColor)
            Write(text, colorForeground, BackgroundColor)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text"></param>
        Public Shared Sub Write(text As System.Text.StringBuilder)
            Write(text, ForegroundColor, BackgroundColor)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text"></param>
        Public Shared Sub Write(text As System.Text.StringBuilder, colorForeground As System.ConsoleColor)
            Write(text, colorForeground, BackgroundColor)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text"></param>
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
        ''' <param name="text"></param>
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
        Public Shared Sub WriteLine()
            WriteLine("", SystemConsoleDefaultForegroundColor, SystemConsoleDefaultBackgroundColor)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text"></param>
        Public Shared Sub WriteLine(text As String)
            Write(text & vbNewLine)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text"></param>
        Public Shared Sub WriteLine(text As System.Text.StringBuilder)
            WriteLine(text, ForegroundColor, BackgroundColor)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text"></param>
        Public Shared Sub WriteLine(text As System.Text.StringBuilder, colorForeground As System.ConsoleColor)
            WriteLine(text, colorForeground, BackgroundColor)
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text"></param>
        Public Shared Sub WriteLine(text As System.Text.StringBuilder, colorForeground As System.ConsoleColor, colorBackground As System.ConsoleColor)
            Dim CurrentForeColor As System.ConsoleColor = ForegroundColor
            Dim CurrentBackColor As System.ConsoleColor = BackgroundColor
            ForegroundColor = colorForeground
            BackgroundColor = colorBackground
            Write(text)
            Write(vbNewLine)
            ForegroundColor = CurrentForeColor
            BackgroundColor = CurrentBackColor
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text"></param>
        ''' <param name="arg0"></param>
        Public Shared Sub WriteLine(text As String, arg0 As Object)
            WriteLine(String.Format(text, arg0))
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text"></param>
        ''' <param name="args"></param>
        Public Shared Sub WriteLine(text As String, ParamArray args As Object())
            WriteLine(String.Format(text, args))
        End Sub

        ''' <summary>
        ''' Write message with current color settings
        ''' </summary>
        ''' <param name="text"></param>
        ''' <param name="colorForeground"></param>
        Public Shared Sub WriteLine(text As String, colorForeground As System.ConsoleColor)
            Write(text & vbNewLine, colorForeground)
        End Sub

        ''' <summary>
        ''' Write message with specified color settings
        ''' </summary>
        ''' <param name="text"></param>
        ''' <param name="colorForeground"></param>
        ''' <param name="colorBackground"></param>
        Public Shared Sub WriteLine(text As String, colorForeground As System.ConsoleColor, colorBackground As System.ConsoleColor)
            Write(text & vbNewLine, colorForeground, colorBackground)
        End Sub

        ''' <summary>
        ''' Write with color setting for status warning messages
        ''' </summary>
        ''' <param name="text"></param>
        Public Shared Sub Warn(text As String)
            Dim CurrentForeColor As System.ConsoleColor = ForegroundColor
            Dim CurrentBackColor As System.ConsoleColor = BackgroundColor
            ForegroundColor = WarningForegroundColor
            BackgroundColor = WarningBackgroundColor
            Write(text)
            ForegroundColor = CurrentForeColor
            BackgroundColor = CurrentBackColor
        End Sub

        ''' <summary>
        ''' Write with color setting for status warning messages
        ''' </summary>
        ''' <param name="text"></param>
        Public Shared Sub Warn(text As System.Text.StringBuilder)
            Dim CurrentForeColor As System.ConsoleColor = ForegroundColor
            Dim CurrentBackColor As System.ConsoleColor = BackgroundColor
            ForegroundColor = WarningForegroundColor
            BackgroundColor = WarningBackgroundColor
            Write(text)
            ForegroundColor = CurrentForeColor
            BackgroundColor = CurrentBackColor
        End Sub

        ''' <summary>
        ''' Write with color setting for status warning messages
        ''' </summary>
        Public Shared Sub WarnLine()
            WriteLine("", SystemConsoleDefaultForegroundColor, SystemConsoleDefaultBackgroundColor)
        End Sub

        ''' <summary>
        ''' Write with color setting for status warning messages
        ''' </summary>
        ''' <param name="text"></param>
        Public Shared Sub WarnLine(text As String)
            Warn(text & vbNewLine)
        End Sub

        ''' <summary>
        ''' Write with color setting for status warning messages
        ''' </summary>
        ''' <param name="text"></param>
        Public Shared Sub WarnLine(text As System.Text.StringBuilder)
            Warn(text)
            Warn(vbNewLine)
        End Sub

        ''' <summary>
        ''' Write with color setting for status okay messages
        ''' </summary>
        ''' <param name="text"></param>
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
        ''' <param name="text"></param>
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
        Public Shared Sub OkayLine()
            WriteLine("", SystemConsoleDefaultForegroundColor, SystemConsoleDefaultBackgroundColor)
        End Sub

        ''' <summary>
        ''' Write with color setting for status okay messages
        ''' </summary>
        ''' <param name="text"></param>
        Public Shared Sub OkayLine(text As String)
            Okay(text & vbNewLine)
        End Sub

        ''' <summary>
        ''' Write with color setting for status okay messages
        ''' </summary>
        ''' <param name="text"></param>
        Public Shared Sub OkayLine(text As System.Text.StringBuilder)
            Okay(text)
            Okay(vbNewLine)
        End Sub

        ''' <summary>
        ''' The log data as raw text content 
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function PlainTextLog() As System.Text.StringBuilder
            Dim FileContent As New System.Text.StringBuilder(_PlainTextLog.ToString)
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
            Return _PlainTextLog
        End Function

        ''' <summary>
        ''' The log data as HTML content
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function HtmlLog() As System.Text.StringBuilder
            Return _HtmlLog
        End Function

        ''' <summary>
        ''' Add tags HTML + BODY around the log data
        ''' </summary>
        ''' <param name="wrapAroundHtmlAndBodyTags"></param>
        ''' <returns></returns>
        Public Shared Function HtmlLog(wrapAroundHtmlAndBodyTags As Boolean) As System.Text.StringBuilder
            Return HtmlLog(wrapAroundHtmlAndBodyTags, "", "", "")
        End Function

        ''' <summary>
        ''' Add tags HTML + HEAD incl. TITLE + BODY around the log data
        ''' </summary>
        ''' <param name="pageTitle">A page title (will be HTML encoded)</param>
        ''' <returns></returns>
        Public Shared Function HtmlLog(pageTitle As String) As System.Text.StringBuilder
            Return HtmlLog(True, "<title>" & System.Net.WebUtility.HtmlEncode(pageTitle) & "</title>", "", "")
        End Function

        ''' <summary>
        ''' Add tags HTML + HEAD incl. TITLE + BODY around the log data
        ''' </summary>
        ''' <param name="wrapAroundHtmlAndBodyTags"></param>
        ''' <param name="headContent"></param>
        ''' <param name="bodyPreContent"></param>
        ''' <param name="bodyPostContent"></param>
        ''' <returns></returns>
        Private Shared Function HtmlLog(wrapAroundHtmlAndBodyTags As Boolean, headContent As String, bodyPreContent As String, bodyPostContent As String) As System.Text.StringBuilder
            If wrapAroundHtmlAndBodyTags = False And headContent <> Nothing Then
                Throw New ArgumentException("wrapAroundHtmlAndBodyTags must be true if pageTitle has got a value")
            End If
            Dim BackColorname As String = ConsoleColorCssName(SystemConsoleDefaultBackgroundColor)
            Dim ForeColorname As String = ConsoleColorCssName(SystemConsoleDefaultForegroundColor)
            Dim FileContent As New System.Text.StringBuilder
            If wrapAroundHtmlAndBodyTags Then
                FileContent.Append("<html>" & vbNewLine)
                If headContent <> Nothing Then
                    FileContent.Append("<head>" & System.Net.WebUtility.HtmlEncode(headContent) & "</head>" & vbNewLine)
                End If
                FileContent.Append("<body style=""background-color: " & BackColorname & ";"">" & vbNewLine)
                FileContent.Append(bodyPreContent)
                FileContent.Append("<span style=""color: " & ForeColorname & ";"">" + _HtmlLog.ToString + "</span>" & vbNewLine)
                FileContent.Append(bodyPostContent)
                FileContent.Append("</body></html>")
                Return FileContent
            ElseIf bodyPreContent <> Nothing OrElse bodyPostContent <> Nothing Then
                FileContent.Append(bodyPreContent)
                FileContent.Append(_HtmlLog)
                FileContent.Append(bodyPostContent)
                Return FileContent
            Else
                Return _HtmlLog
            End If
        End Function

        ''' <summary>
        ''' Save the log data as plain text to a file
        ''' </summary>
        ''' <param name="path"></param>
        Public Shared Sub SavePlainTextLog(path As String)
            System.IO.File.WriteAllText(path, PlainTextLog.ToString)
        End Sub

        ''' <summary>
        ''' Save the log data as raw text (contains internal tags for e.g. color output) to a file
        ''' </summary>
        ''' <param name="path"></param>
        Public Shared Sub SaveRawPlainTextLog(path As String)
            System.IO.File.WriteAllText(path, RawPlainTextLog.ToString)
        End Sub

        ''' <summary>
        ''' Save the log data as HTML text to a file
        ''' </summary>
        ''' <param name="path"></param>
        Public Shared Sub SaveHtmlLog(path As String)
            SaveHtmlLog(path, "Log: " & Now.ToString("yyyy-MM-dd HH:mm:ss"))
        End Sub

        ''' <summary>
        ''' Save the log data as HTML text to a file
        ''' </summary>
        ''' <param name="path"></param>
        ''' <param name="pageTitle"></param>
        Public Shared Sub SaveHtmlLog(path As String, pageTitle As String)
            System.IO.File.WriteAllText(path, HtmlLog(pageTitle).ToString)
        End Sub

        ''' <summary>
        ''' Save the log data as HTML text to a file
        ''' </summary>
        ''' <param name="path"></param>
        ''' <param name="headContent">Content for the head area</param>
        ''' <param name="bodyPreContent">Content for the body area before the log data</param>
        ''' <param name="bodyPostContent">Content for the body area after the log data</param>
        Public Shared Sub SaveHtmlLog(path As String, headContent As String, bodyPreContent As String, bodyPostContent As String)
            If headContent <> Nothing OrElse bodyPreContent <> Nothing OrElse bodyPostContent <> Nothing Then
                System.IO.File.WriteAllText(path, HtmlLog(True, headContent, bodyPreContent, bodyPostContent).ToString)
            Else
                System.IO.File.WriteAllText(path, HtmlLog(False).ToString)
            End If
        End Sub

        Private Shared Function ConsoleColorSystemName(color As System.ConsoleColor) As String
            Return [Enum].GetName(GetType(System.ConsoleColor), color)
        End Function
        Private Shared Function ConsoleColorCssName(color As System.ConsoleColor) As String
            Dim SystemColorName As String = ConsoleColorSystemName(color)
            If SystemColorName.ToLowerInvariant.StartsWith("dark") Then
                Return SystemColorName
            Else
                Return SystemColorName.Replace("Gray", "LightGray")
            End If
        End Function

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

        Public Shared _ControlCKeyPressed As ControlCKeyPressedException
        Public Shared ReadOnly Property ControlCKeyPressed As ControlCKeyPressedException
            Get
                Return _ControlCKeyPressed
            End Get
        End Property

        Public Shared ReadOnly Property IsControlCKeyPressed As Boolean
            Get
                If ControlCKeyPressed Is Nothing Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property
        Public Shared Property ThrowControlCKeyPressedExceptionOnNextConsoleCommand As Boolean = True

        Private Shared _CatchControlC As Boolean = False
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
            End Set
        End Property
#End Region

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
                _Write(vbNewLine, False)
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
            System.Console.Clear()
            _Write(vbNewLine & vbNewLine & vbNewLine, False)
        End Sub

        ''' <summary>
        ''' Clear the content of console window and/or logs
        ''' </summary>
        ''' <param name="consoleWindow"></param>
        ''' <param name="plainTextLog"></param>
        ''' <param name="htmlLog"></param>
        Public Shared Sub Clear(consoleWindow As Boolean, plainTextLog As Boolean, htmlLog As Boolean)
            If consoleWindow Then System.Console.Clear()
            If plainTextLog Then _PlainTextLog.Clear()
            If htmlLog Then _HtmlLog.Clear()
        End Sub

    End Class

End Namespace