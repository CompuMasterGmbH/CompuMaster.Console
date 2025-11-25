Imports NUnit.Framework

Namespace ConsoleTest


    <Parallelizable(ParallelScope.All)>
    Public Class ParallelizedVirtualConsole

        <SetUp>
        Public Sub Setup()
        End Sub

        ''' <summary>
        ''' Test non-async (but tagged with parallel execution in class attribute) without simulated warning message, with output to sytem's console disabled (=default)
        ''' </summary>
        <Test>
        Public Sub Test1()
            Dim vc = LongRunningTask(1, False, False)
            System.Console.WriteLine(vc.RawPlainTextLog.ToString)
            Dim ExpectedOutput As String = ExpectedOutputForTask(1, False, False)
            Assert.That(vc.RawPlainTextLog.ToString, [Is].EqualTo(ExpectedOutput))
        End Sub

        ''' <summary>
        ''' Test non-async (but tagged with parallel execution in class attribute) without simulated warning message, but with output to sytem's console enabled
        ''' </summary>
        <Test>
        Public Sub Test2()
            Dim vc = LongRunningTask(2, False, True)
            System.Console.WriteLine(vc.RawPlainTextLog.ToString)
            Dim ExpectedOutput As String = ExpectedOutputForTask(2, False, False)
            Assert.That(vc.RawPlainTextLog.ToString, [Is].EqualTo(ExpectedOutput))
        End Sub

        ''' <summary>
        ''' Test non-async (but tagged with parallel execution in class attribute) with simulated warning message and with output to sytem's console enabled
        ''' </summary>
        <Test>
        Public Sub Test3()
            Dim vc = LongRunningTask(3, True, True)
            System.Console.WriteLine(vc.RawPlainTextLog.ToString)
            Dim ExpectedOutput As String = ExpectedOutputForTask(3, True, False)
            Assert.That(vc.RawPlainTextLog.ToString, [Is].EqualTo(ExpectedOutput))
        End Sub

        ''' <summary>
        ''' Test async without simulated warning message, with output to sytem's console disabled (=default)
        ''' </summary>
        ''' <returns></returns>
        <Test>
        Public Async Function AsyncTest1() As Task
            Dim vc = Await LongRunningTaskAsync(1, False, False)
            Dim ExpectedOutput As String = ExpectedOutputForTask(1, False, True)
            Assert.That(vc.RawPlainTextLog.ToString, [Is].EqualTo(ExpectedOutput))
        End Function

        ''' <summary>
        ''' Test async without simulated warning message, but with output to sytem's console enabled
        ''' </summary>
        ''' <returns></returns>
        <Test>
        Public Async Function AsyncTest2() As Task
            Dim vc = Await LongRunningTaskAsync(2, False, True)
            Dim ExpectedOutput As String = ExpectedOutputForTask(2, False, True)
            Assert.That(vc.RawPlainTextLog.ToString, [Is].EqualTo(ExpectedOutput))
        End Function

        ''' <summary>
        ''' Test async with simulated warning message and with output to sytem's console enabled
        ''' </summary>
        ''' <returns></returns>
        <Test>
        Public Async Function AsyncTest3() As Task
            Dim vc = Await LongRunningTaskAsync(3, True, True)
            Dim ExpectedOutput As String = ExpectedOutputForTask(3, True, True)
            Assert.That(vc.RawPlainTextLog.ToString, [Is].EqualTo(ExpectedOutput))
        End Function


        Private Function LongRunningTask(number As Integer, simulateWarning As Boolean, enableConsoleOutput As Boolean) As CompuMaster.VirtualConsole
            Dim VirtualConsole As New CompuMaster.VirtualConsole("Async Task #" & number)
            VirtualConsole.ConsoleOutputDisabledByDefault = Not enableConsoleOutput

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
            VirtualConsole.WriteLine("Completed long-running task #" & number)

            Return VirtualConsole
        End Function

        Private Async Function LongRunningTaskAsync(number As Integer, simulateWarning As Boolean, enableConsoleOutput As Boolean) As Task(Of CompuMaster.VirtualConsole)
            Dim VirtualConsole As New CompuMaster.VirtualConsole("Async Task #" & number)
            VirtualConsole.ConsoleOutputDisabledByDefault = Not enableConsoleOutput

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
            VirtualConsole.WriteLine("Completed long-running task #" & number)

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
            ExpectedOutput &= "Starting long-running " & AsyncMethodIdentifier & " task #" & number & " ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 1/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 2/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 3/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 4/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 5/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 6/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 7/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 8/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 9/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 10/50 ..." & System.Environment.NewLine
            If simulateWarning Then
                ExpectedOutput &= "<FORECOLOR:Red>        Warning in Step 10/50: This is a pseudo message." & System.Environment.NewLine
                ExpectedOutput &= "        There are more details than shown here." & System.Environment.NewLine
                ExpectedOutput &= "</FORECOLOR:Red><FORECOLOR:Red>        This is further additional warning information with same text indentation level." & System.Environment.NewLine
                ExpectedOutput &= "</FORECOLOR:Red>    Step 11/50 ..." & System.Environment.NewLine
            Else
                ExpectedOutput &= "    Step 11/50 ..." & System.Environment.NewLine
            End If
            ExpectedOutput &= "    Step 12/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 13/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 14/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 15/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 16/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 17/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 18/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 19/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 20/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 21/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 22/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 23/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 24/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 25/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 26/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 27/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 28/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 29/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 30/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 31/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 32/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 33/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 34/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 35/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 36/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 37/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 38/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 39/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 40/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 41/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 42/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 43/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 44/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 45/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 46/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 47/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 48/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 49/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "    Step 50/50 ..." & System.Environment.NewLine
            ExpectedOutput &= "Completed long-running task #" & number & System.Environment.NewLine
            ExpectedOutput &= ""
            Return ExpectedOutput
        End Function
    End Class

End Namespace
