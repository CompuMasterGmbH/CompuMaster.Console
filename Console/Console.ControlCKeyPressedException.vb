Option Explicit On
Option Strict On

Namespace CompuMaster

    Partial Public Class Console

        ''' <summary>
        ''' An exception that is fired when the user has pressed the key combination Control-C
        ''' </summary>
        Public Class ControlCKeyPressedException
            Inherits Exception

            Friend Sub New()
                MyBase.New()
            End Sub

            ''' <summary>
            ''' Internal workaround to force stack trace to show up with origin line numbers after re-throwing the origin exception
            ''' </summary>
            ''' <param name="innerException"></param>
            Friend Sub New(innerException As ControlCKeyPressedException)
                MyBase.New("", innerException)
            End Sub

            Public Overrides ReadOnly Property StackTrace As String
                Get
                    If Me.InnerException IsNot Nothing AndAlso GetType(ControlCKeyPressedException).IsInstanceOfType(Me.InnerException) Then
                        Return Me.InnerException.StackTrace
                    Else
                        Return MyBase.StackTrace
                    End If
                End Get
            End Property
            Public Overrides ReadOnly Property Message As String
                Get
                    If Me.InnerException IsNot Nothing AndAlso GetType(ControlCKeyPressedException).IsInstanceOfType(Me.InnerException) Then
                        Return Me.InnerException.Message
                    Else
                        Return MyBase.Message
                    End If
                End Get
            End Property
            Public Overrides Function ToString() As String
                If Me.InnerException IsNot Nothing AndAlso GetType(ControlCKeyPressedException).IsInstanceOfType(Me.InnerException) Then
                    Return Me.InnerException.ToString
                Else
                    Return MyBase.ToString
                End If
            End Function
        End Class

    End Class

End Namespace