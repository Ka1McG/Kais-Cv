<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.BtnPopulate = New System.Windows.Forms.Button()
        Me.BtnBubbleSort = New System.Windows.Forms.Button()
        Me.LstOutput = New System.Windows.Forms.ListBox()
        Me.BtnInsertSort = New System.Windows.Forms.Button()
        Me.BtnClear = New System.Windows.Forms.Button()
        Me.BtnBinarySearch = New System.Windows.Forms.Button()
        Me.BtnLinearSearch = New System.Windows.Forms.Button()
        Me.BtnQuickSort = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'BtnPopulate
        '
        Me.BtnPopulate.Location = New System.Drawing.Point(12, 12)
        Me.BtnPopulate.Name = "BtnPopulate"
        Me.BtnPopulate.Size = New System.Drawing.Size(199, 96)
        Me.BtnPopulate.TabIndex = 0
        Me.BtnPopulate.Text = "Populate Array"
        Me.BtnPopulate.UseVisualStyleBackColor = True
        '
        'BtnBubbleSort
        '
        Me.BtnBubbleSort.Location = New System.Drawing.Point(12, 114)
        Me.BtnBubbleSort.Name = "BtnBubbleSort"
        Me.BtnBubbleSort.Size = New System.Drawing.Size(199, 96)
        Me.BtnBubbleSort.TabIndex = 1
        Me.BtnBubbleSort.Text = "Bubble Sort"
        Me.BtnBubbleSort.UseVisualStyleBackColor = True
        '
        'LstOutput
        '
        Me.LstOutput.FormattingEnabled = True
        Me.LstOutput.Location = New System.Drawing.Point(218, 13)
        Me.LstOutput.Name = "LstOutput"
        Me.LstOutput.Size = New System.Drawing.Size(298, 290)
        Me.LstOutput.TabIndex = 2
        '
        'BtnInsertSort
        '
        Me.BtnInsertSort.Location = New System.Drawing.Point(12, 216)
        Me.BtnInsertSort.Name = "BtnInsertSort"
        Me.BtnInsertSort.Size = New System.Drawing.Size(199, 96)
        Me.BtnInsertSort.TabIndex = 3
        Me.BtnInsertSort.Text = "Insertion Sort"
        Me.BtnInsertSort.UseVisualStyleBackColor = True
        '
        'BtnClear
        '
        Me.BtnClear.Location = New System.Drawing.Point(522, 216)
        Me.BtnClear.Name = "BtnClear"
        Me.BtnClear.Size = New System.Drawing.Size(199, 96)
        Me.BtnClear.TabIndex = 4
        Me.BtnClear.Text = "Clear Box"
        Me.BtnClear.UseVisualStyleBackColor = True
        '
        'BtnBinarySearch
        '
        Me.BtnBinarySearch.Location = New System.Drawing.Point(522, 12)
        Me.BtnBinarySearch.Name = "BtnBinarySearch"
        Me.BtnBinarySearch.Size = New System.Drawing.Size(199, 96)
        Me.BtnBinarySearch.TabIndex = 5
        Me.BtnBinarySearch.Text = "Binary Search"
        Me.BtnBinarySearch.UseVisualStyleBackColor = True
        '
        'BtnLinearSearch
        '
        Me.BtnLinearSearch.Location = New System.Drawing.Point(522, 114)
        Me.BtnLinearSearch.Name = "BtnLinearSearch"
        Me.BtnLinearSearch.Size = New System.Drawing.Size(199, 96)
        Me.BtnLinearSearch.TabIndex = 6
        Me.BtnLinearSearch.Text = "Linear Search"
        Me.BtnLinearSearch.UseVisualStyleBackColor = True
        '
        'BtnQuickSort
        '
        Me.BtnQuickSort.Location = New System.Drawing.Point(12, 318)
        Me.BtnQuickSort.Name = "BtnQuickSort"
        Me.BtnQuickSort.Size = New System.Drawing.Size(199, 96)
        Me.BtnQuickSort.TabIndex = 7
        Me.BtnQuickSort.Text = "Quick Sort"
        Me.BtnQuickSort.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(732, 422)
        Me.Controls.Add(Me.BtnQuickSort)
        Me.Controls.Add(Me.BtnLinearSearch)
        Me.Controls.Add(Me.BtnBinarySearch)
        Me.Controls.Add(Me.BtnClear)
        Me.Controls.Add(Me.BtnInsertSort)
        Me.Controls.Add(Me.LstOutput)
        Me.Controls.Add(Me.BtnBubbleSort)
        Me.Controls.Add(Me.BtnPopulate)
        Me.Name = "Form1"
        Me.Text = "Sorts"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents BtnPopulate As Button
    Friend WithEvents BtnBubbleSort As Button
    Friend WithEvents LstOutput As ListBox
    Friend WithEvents BtnInsertSort As Button
    Friend WithEvents BtnClear As Button
    Friend WithEvents BtnBinarySearch As Button
    Friend WithEvents BtnLinearSearch As Button
    Friend WithEvents BtnQuickSort As Button
End Class
