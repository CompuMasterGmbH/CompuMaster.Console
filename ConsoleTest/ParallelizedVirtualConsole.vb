Imports NUnit.Framework

Namespace ConsoleTest


    <Parallelizable(ParallelScope.All)>
    Public Class ParallelizedVirtualConsole

        <SetUp>
        Public Sub Setup()
        End Sub

        <Test>
        Public Sub Test1()
            Dim vc = LongRunningTask(1, False)
            System.Console.WriteLine(vc.RawPlainTextLog.ToString)
            Dim ExpectedOutput As String = ExpectedOutputForTask(1, False, False)
            Assert.That(vc.RawPlainTextLog.ToString, [Is].EqualTo(ExpectedOutput))
        End Sub

        <Test>
        Public Sub Test2()
            Dim vc = LongRunningTask(2, False)
            System.Console.WriteLine(vc.RawPlainTextLog.ToString)
            Dim ExpectedOutput As String = ExpectedOutputForTask(2, False, False)
            Assert.That(vc.RawPlainTextLog.ToString, [Is].EqualTo(ExpectedOutput))
        End Sub

        <Test>
        Public Sub Test3()
            Dim vc = LongRunningTask(3, True)
            System.Console.WriteLine(vc.RawPlainTextLog.ToString)
            Dim ExpectedOutput As String = ExpectedOutputForTask(3, True, False)
            Assert.That(vc.RawPlainTextLog.ToString, [Is].EqualTo(ExpectedOutput))
        End Sub

        <Test>
        Public Async Function AsyncTest1() As Task
            Dim vc = Await LongRunningTaskAsync(1, False)
            Dim ExpectedOutput As String = ExpectedOutputForTask(1, False, True)
            Assert.That(vc.RawPlainTextLog.ToString, [Is].EqualTo(ExpectedOutput))
        End Function

        <Test>
        Public Async Function AsyncTest2() As Task
            Dim vc = Await LongRunningTaskAsync(2, False)
            Dim ExpectedOutput As String = ExpectedOutputForTask(2, False, True)
            Assert.That(vc.RawPlainTextLog.ToString, [Is].EqualTo(ExpectedOutput))
        End Function

        <Test>
        Public Async Function AsyncTest3() As Task
            Dim vc = Await LongRunningTaskAsync(3, True)
            Dim ExpectedOutput As String = ExpectedOutputForTask(3, True, True)
            Assert.That(vc.RawPlainTextLog.ToString, [Is].EqualTo(ExpectedOutput))
        End Function


        Private Function LongRunningTask(number As Integer, simulateWarning As Boolean) As CompuMaster.VirtualConsole
            Dim VirtualConsole As New CompuMaster.VirtualConsole()

            VirtualConsole.WriteLine("Starting long-running non-async task #" & number & " ...")
            VirtualConsole.CurrentIndentationLevel += 1

            ' Simulate a long-running task
            Dim TotalStepCount As Integer = 50
            For i As Integer = 1 To 50
                VirtualConsole.WriteLine("Step " & i & "/" & TotalStepCount & " ...")
                VirtualConsole.CurrentIndentationLevel += 1
                System.Threading.Thread.Sleep(135)
                If simulateWarning AndAlso i = 10 Then
                    VirtualConsole.WarnLine(
                        "Warning in Step " & i & "/" & TotalStepCount & ": This is a pseudo message." & System.Environment.NewLine &
                        "There are more details than shown here.")
                    VirtualConsole.WarnLine("This is further additional warning information with same text indentation level.")
                End If
                VirtualConsole.CurrentIndentationLevel -= 1
            Next

            VirtualConsole.CurrentIndentationLevel -= 1
            Console.WriteLine("Completed long-running task #" & number & " ...")

            Return VirtualConsole
        End Function

        Private Async Function LongRunningTaskAsync(number As Integer, simulateWarning As Boolean) As Task(Of CompuMaster.VirtualConsole)
            Dim VirtualConsole As New CompuMaster.VirtualConsole()

            VirtualConsole.WriteLine("Starting long-running ASYNC task #" & number & " ...")
            VirtualConsole.CurrentIndentationLevel += 1

            ' Simulate a long-running task
            Dim t As Task = Task.Run(Sub()
                                         Dim TotalStepCount As Integer = 50
                                         For i As Integer = 1 To 50
                                             VirtualConsole.WriteLine("Step " & i & "/" & TotalStepCount & " ...")
                                             VirtualConsole.CurrentIndentationLevel += 1
                                             System.Threading.Thread.Sleep(135)
                                             If simulateWarning AndAlso i = 10 Then
                                                 VirtualConsole.WarnLine(
                                                    "Warning in Step " & i & "/" & TotalStepCount & ": This is a pseudo message." & System.Environment.NewLine &
                                                    "There are more details than shown here.")
                                                 VirtualConsole.WarnLine("This is further additional warning information with same text indentation level.")
                                             End If
                                             VirtualConsole.CurrentIndentationLevel -= 1
                                         Next
                                     End Sub)
            Await t

            VirtualConsole.CurrentIndentationLevel -= 1
            Console.WriteLine("Completed long-running task #" & number & " ...")

            Return VirtualConsole
        End Function

        Private Function ExpectedOutputForTask(number As Integer, simulateWarning As Boolean, isAsyncOutput As Boolean) As String
            Dim AsyncMethodIdentifier As String
            If isAsyncOutput Then
                AsyncMethodIdentifier = "ASYNC"
            Else
                AsyncMethodIdentifier = "non-async"
            End If
            Dim ExpectedOutput As String = ""
            ExpectedOutput &= "<FORECOLOR:White>Starting long-running " & AsyncMethodIdentifier & " task #" & number & " ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 1/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 2/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 3/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 4/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 5/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 6/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 7/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 8/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 9/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 10/50 ..." & System.Environment.NewLine
            If simulateWarning Then
                ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:Red>        Warning in Step 10/50: This is a pseudo message." & System.Environment.NewLine
                ExpectedOutput &= "        There are more details than shown here." & System.Environment.NewLine
                ExpectedOutput &= "</FORECOLOR:Red><FORECOLOR:Red>        This is further additional warning information with same text indentation level." & System.Environment.NewLine
                ExpectedOutput &= "</FORECOLOR:Red><FORECOLOR:White>    Step 11/50 ..." & System.Environment.NewLine
            Else
                ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 11/50 ..." & System.Environment.NewLine
            End If
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 12/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 13/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 14/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 15/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 16/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 17/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 18/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 19/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 20/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 21/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 22/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 23/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 24/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 25/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 26/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 27/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 28/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 29/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 30/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 31/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 32/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 33/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 34/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 35/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 36/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 37/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 38/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 39/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 40/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 41/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 42/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 43/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 44/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 45/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 46/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 47/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 48/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 49/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White><FORECOLOR:White>    Step 50/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "</FORECOLOR:White>"
            Return ExpectedOutput
        End Function
    End Class

End Namespace
