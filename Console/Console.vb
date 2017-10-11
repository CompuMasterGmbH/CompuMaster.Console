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

        Public Shared Sub Write(text As String)
            If IsControlCKeyPressed AndAlso ThrowControlCKeyPressedExceptionOnNextConsoleCommand Then
                Dim innerEx As ControlCKeyPressedException = ControlCKeyPressed
                _ControlCKeyPressed = Nothing 'don't raise for a 2nd time!
                Throw New ControlCKeyPressedException(innerEx)
            End If

            'System console
            System.Console.Write(text)

            'Plain text log
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

            'Html log
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
        End Sub
        Public Shared Sub Write(text As String, arg0 As Object)
            Write(String.Format(text, arg0))
        End Sub
        Public Shared Sub Write(text As String, ParamArray args As Object())
            Write(String.Format(text, args))
        End Sub
        Public Shared Sub Write(text As String, colorForeground As System.ConsoleColor)
            Write(text, colorForeground, BackgroundColor)
        End Sub
        Public Shared Sub Write(text As String, colorForeground As System.ConsoleColor, colorBackground As System.ConsoleColor)
            Dim CurrentForeColor As System.ConsoleColor = ForegroundColor
            Dim CurrentBackColor As System.ConsoleColor = BackgroundColor
            ForegroundColor = colorForeground
            BackgroundColor = colorBackground
            Write(text)
            ForegroundColor = CurrentForeColor
            BackgroundColor = CurrentBackColor
        End Sub
        Public Shared Sub WriteLine()
            WriteLine("", SystemConsoleDefaultForegroundColor, SystemConsoleDefaultBackgroundColor)
        End Sub
        Public Shared Sub WriteLine(text As String)
            Write(text & vbNewLine)
        End Sub
        Public Shared Sub WriteLine(text As String, arg0 As Object)
            WriteLine(String.Format(text, arg0))
        End Sub
        Public Shared Sub WriteLine(text As String, ParamArray args As Object())
            WriteLine(String.Format(text, args))
        End Sub
        Public Shared Sub WriteLine(text As String, colorForeground As System.ConsoleColor)
            Write(text & vbNewLine, colorForeground)
        End Sub
        Public Shared Sub WriteLine(text As String, colorForeground As System.ConsoleColor, colorBackground As System.ConsoleColor)
            Write(text & vbNewLine, colorForeground, colorBackground)
        End Sub
        Public Shared Sub Warn(text As String)
            Dim CurrentForeColor As System.ConsoleColor = ForegroundColor
            Dim CurrentBackColor As System.ConsoleColor = BackgroundColor
            ForegroundColor = WarningForegroundColor
            BackgroundColor = WarningBackgroundColor
            Write(text)
            ForegroundColor = CurrentForeColor
            BackgroundColor = CurrentBackColor
        End Sub

        Public Shared Sub WarnLine()
            WriteLine("", SystemConsoleDefaultForegroundColor, SystemConsoleDefaultBackgroundColor)
        End Sub
        Public Shared Sub WarnLine(text As String)
            Warn(text & vbNewLine)
        End Sub

        Public Shared Sub Okay(text As String)
            Dim CurrentForeColor As System.ConsoleColor = ForegroundColor
            Dim CurrentBackColor As System.ConsoleColor = BackgroundColor
            ForegroundColor = OkayMessageForegroundColor
            BackgroundColor = OkayMessageBackgroundColor
            Write(text & vbNewLine)
            ForegroundColor = CurrentForeColor
            BackgroundColor = CurrentBackColor
        End Sub

        Public Shared Sub OkayLine()
            WriteLine("", SystemConsoleDefaultForegroundColor, SystemConsoleDefaultBackgroundColor)
        End Sub
        Public Shared Sub OkayLine(text As String)
            Okay(text & vbNewLine)
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
            Return _HtmlLog
        End Function

        ''' <summary>
        ''' Add tags HTML + HEAD incl. TITLE + BODY around the log data
        ''' </summary>
        ''' <param name="pageTitle"></param>
        ''' <returns></returns>
        Public Shared Function HtmlLog(pageTitle As String) As System.Text.StringBuilder
            Return _HtmlLog
        End Function

        ''' <summary>
        ''' Add tags HTML + HEAD incl. TITLE + BODY around the log data
        ''' </summary>
        ''' <param name="wrapAroundHtmlAndBodyTags"></param>
        ''' <param name="pageTitle"></param>
        ''' <returns></returns>
        Private Shared Function HtmlLog(wrapAroundHtmlAndBodyTags As Boolean, pageTitle As String) As System.Text.StringBuilder
            If wrapAroundHtmlAndBodyTags = False And pageTitle <> Nothing Then
                Throw New ArgumentException("wrapAroundHtmlAndBodyTags must be true if pageTitle has got a value")
            End If
            Dim BackColorname As String = ConsoleColorCssName(SystemConsoleDefaultBackgroundColor)
            Dim ForeColorname As String = ConsoleColorCssName(SystemConsoleDefaultForegroundColor)
            If wrapAroundHtmlAndBodyTags Then
                Dim FileContent As New System.Text.StringBuilder
                FileContent.Append("<html>" & vbNewLine)
                If pageTitle <> Nothing Then
                    FileContent.Append("<head><title>" & pageTitle & "</title></head>" & vbNewLine)
                End If
                FileContent.Append("<body style=""background-color: " & BackColorname & ";"">" & vbNewLine)
                FileContent.Append("<span style=""color: " & ForeColorname & ";"">" + _HtmlLog.ToString + "</span>" & vbNewLine)
                FileContent.Append("</body></html>")
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

    End Class

End Namespace