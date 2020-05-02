Module Module1

    Sub Main()

        Dim ccNumber As String = "1234567890"
        Dim ccPos As Integer = ccNumber.Length - 4
        Dim maskedCCNumber As String = New String("x", (ccPos - 1)) & ccNumber.Substring(ccPos)

        Console.WriteLine(maskedCCNumber)

        Console.Read()
    End Sub

End Module
