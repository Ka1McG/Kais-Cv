Imports System.Windows.Forms.AxHost
Imports Microsoft.VisualBasic.Logging

Public Class Form1
    Dim Array() As Integer
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim intArraySize As Integer = InputBox("How many elements?") - 1
        ReDim Array(intArraySize)
    End Sub

    Private Sub BtnPopulate_Click(sender As Object, e As EventArgs) Handles BtnPopulate.Click
        Randomize()
        For i = 0 To Array.Length() - 1
            Array(i) = Math.Floor(Rnd() * 100)
            LstOutput.Items.Add(Array(i))
        Next
    End Sub

    Private Sub BtnBubbleSort_Click(sender As Object, e As EventArgs) Handles BtnBubbleSort.Click
        Dim n As Integer = Array.Length() - 1
        Dim temp As Integer
        Dim swapped As Boolean = True

        Do Until swapped = False        'will keep doing passes until there have been no swaps
            swapped = False
            For i = 0 To n - 1
                If Array(i) > Array(i + 1) Then
                    temp = Array(i + 1)
                    Array(i + 1) = Array(i)
                    Array(i) = temp
                    swapped = True

                End If
            Next
        Loop
        Output()
    End Sub

    Sub Output()
        LstOutput.Items.Clear()
        For i = 0 To Array.Length() - 1
            LstOutput.Items.Add(Array(i))
        Next
    End Sub

    Private Sub BtnInsertSort_Click(sender As Object, e As EventArgs) Handles BtnInsertSort.Click
        Dim i, j, n, currentItem As Integer
        Dim inserted As Boolean

        n = UBound(Array) + 1

        For i = 1 To n - 1
            currentItem = Array(i)
            inserted = False
            j = i - 1

            Do While (j >= 0 And inserted = False)
                If (currentItem < Array(j)) Then
                    Array(j + 1) = Array(j)
                    j = j - 1
                    Array(j + 1) = currentItem
                Else
                    inserted = True
                End If
            Loop
        Next
        Output()
    End Sub

    Private Sub BtnClear_Click(sender As Object, e As EventArgs) Handles BtnClear.Click
        LstOutput.Items.Clear()
    End Sub

    Private Sub BtnBinarySearch_Click(sender As Object, e As EventArgs) Handles BtnBinarySearch.Click
        Dim StartP, EndP, MidP, SearchValue As Integer
        Dim Found As Boolean = False

        StartP = 0
        EndP = UBound(Array)

        SearchValue = InputBox("What Value are you looking for?")

        Do Until Found = True
            MidP = (StartP + EndP) \ 2
            If StartP > EndP Then
                MsgBox("Item not in list")
                Exit Do

            ElseIf SearchValue = Array(MidP) Then
                Found = True
                MsgBox("Item found at index " & MidP + 1)

            ElseIf SearchValue > Array(MidP) Then
                StartP = MidP + 1

            ElseIf SearchValue < Array(MidP) Then
                EndP = MidP - 1
            End If
        Loop

    End Sub

    Private Sub BtnLinearSearch_Click(sender As Object, e As EventArgs) Handles BtnLinearSearch.Click
        Dim Found As Boolean = False
        Dim SearchValue As Integer = InputBox("What Value are you looking for?")

        For i = 0 To UBound(Array)
            If Array(i) = SearchValue Then
                MsgBox("item found at index " & i + 1)
                Found = True
            End If
        Next
        If Found = False Then
            MsgBox("Item not in list")
        End If
    End Sub

    Private Sub BtnQuickSort_Click(sender As Object, e As EventArgs) Handles BtnQuickSort.Click
        quickSortProc(Array, 0, Array.Length - 1)
    End Sub

    Sub quickSortProc(Array() As Integer, indexLow As Integer, indexHigh As Integer)
        Dim Pivot, tempSwap, tempLow, tempHigh As Integer

        tempLow = indexLow
        tempHigh = indexHigh

        Pivot = (Array((indexLow + indexHigh) / 2))

        While (tempLow <= tempHigh)
            'increment
            While (Array(tempLow) < Pivot And tempLow < indexHigh)
                tempLow += 1
            End While

            'decrement
            While (Pivot < Array(tempHigh) And tempHigh > indexLow)
                tempHigh -= 1
            End While

            'swap
            If (tempLow <= tempHigh) Then
                tempSwap = Array(tempLow)
                Array(tempLow) = Array(tempHigh)
                Array(tempHigh) = tempSwap

                tempLow += 1
                tempHigh -= 1

            End If

        End While

        If (indexLow < tempHigh) Then
            quickSortProc(Array, indexLow, tempHigh)
        End If

        If tempLow < indexHigh Then
            quickSortProc(Array, tempLow, indexHigh)
        End If

        Output()
    End Sub
End Class