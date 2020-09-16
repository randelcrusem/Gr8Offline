<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmVerifier_DetailOthers
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmVerifier_DetailOthers))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.btnPrintTransaction = New System.Windows.Forms.Button()
        Me.txtLine = New System.Windows.Forms.TextBox()
        Me.btnMin = New System.Windows.Forms.Button()
        Me.txtPage = New System.Windows.Forms.TextBox()
        Me.btnMax = New System.Windows.Forms.Button()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.btnPrev = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lblBalance = New System.Windows.Forms.Label()
        Me.txtVCEName = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtVCECode = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtAccount = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.dgvLedger = New System.Windows.Forms.DataGridView()
        Me.colNum = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colRefType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colRefID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDeposit = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colWithdrawal = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBalance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel1.SuspendLayout()
        CType(Me.dgvLedger, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.btnPrintTransaction)
        Me.Panel1.Controls.Add(Me.txtLine)
        Me.Panel1.Controls.Add(Me.btnMin)
        Me.Panel1.Controls.Add(Me.txtPage)
        Me.Panel1.Controls.Add(Me.btnMax)
        Me.Panel1.Controls.Add(Me.btnNext)
        Me.Panel1.Controls.Add(Me.btnPrev)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.lblBalance)
        Me.Panel1.Controls.Add(Me.txtVCEName)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.txtVCECode)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.txtAccount)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.dgvLedger)
        Me.Panel1.Location = New System.Drawing.Point(2, 2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(685, 531)
        Me.Panel1.TabIndex = 1199
        '
        'Label11
        '
        Me.Label11.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label11.Location = New System.Drawing.Point(562, 508)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(53, 13)
        Me.Label11.TabIndex = 1427
        Me.Label11.Text = "Line No. :"
        '
        'btnPrintTransaction
        '
        Me.btnPrintTransaction.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrintTransaction.Font = New System.Drawing.Font("Wingdings 2", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnPrintTransaction.Location = New System.Drawing.Point(652, 498)
        Me.btnPrintTransaction.Name = "btnPrintTransaction"
        Me.btnPrintTransaction.Size = New System.Drawing.Size(30, 30)
        Me.btnPrintTransaction.TabIndex = 1426
        Me.btnPrintTransaction.Text = ""
        Me.btnPrintTransaction.UseVisualStyleBackColor = True
        '
        'txtLine
        '
        Me.txtLine.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLine.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.txtLine.Location = New System.Drawing.Point(621, 499)
        Me.txtLine.MinimumSize = New System.Drawing.Size(30, 28)
        Me.txtLine.Name = "txtLine"
        Me.txtLine.ReadOnly = True
        Me.txtLine.Size = New System.Drawing.Size(30, 20)
        Me.txtLine.TabIndex = 1425
        Me.txtLine.Text = "0"
        Me.txtLine.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnMin
        '
        Me.btnMin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnMin.Font = New System.Drawing.Font("Webdings", 12.0!)
        Me.btnMin.Location = New System.Drawing.Point(1, 498)
        Me.btnMin.Name = "btnMin"
        Me.btnMin.Size = New System.Drawing.Size(30, 30)
        Me.btnMin.TabIndex = 1424
        Me.btnMin.Text = ""
        Me.btnMin.UseVisualStyleBackColor = True
        '
        'txtPage
        '
        Me.txtPage.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtPage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.txtPage.Location = New System.Drawing.Point(62, 499)
        Me.txtPage.MinimumSize = New System.Drawing.Size(30, 28)
        Me.txtPage.Name = "txtPage"
        Me.txtPage.ReadOnly = True
        Me.txtPage.Size = New System.Drawing.Size(30, 20)
        Me.txtPage.TabIndex = 1423
        Me.txtPage.Text = "1"
        Me.txtPage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnMax
        '
        Me.btnMax.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnMax.Font = New System.Drawing.Font("Webdings", 12.0!)
        Me.btnMax.Location = New System.Drawing.Point(123, 498)
        Me.btnMax.Name = "btnMax"
        Me.btnMax.Size = New System.Drawing.Size(30, 30)
        Me.btnMax.TabIndex = 1422
        Me.btnMax.Text = ""
        Me.btnMax.UseVisualStyleBackColor = True
        '
        'btnNext
        '
        Me.btnNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnNext.Font = New System.Drawing.Font("Webdings", 12.0!)
        Me.btnNext.Location = New System.Drawing.Point(93, 498)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(30, 30)
        Me.btnNext.TabIndex = 1421
        Me.btnNext.Text = "4"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'btnPrev
        '
        Me.btnPrev.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPrev.Font = New System.Drawing.Font("Webdings", 12.0!)
        Me.btnPrev.Location = New System.Drawing.Point(31, 498)
        Me.btnPrev.Name = "btnPrev"
        Me.btnPrev.Size = New System.Drawing.Size(30, 30)
        Me.btnPrev.TabIndex = 1420
        Me.btnPrev.Text = ""
        Me.btnPrev.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.Black
        Me.Label10.ForeColor = System.Drawing.Color.White
        Me.Label10.Location = New System.Drawing.Point(418, 5)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(49, 13)
        Me.Label10.TabIndex = 1198
        Me.Label10.Text = "Balance:"
        '
        'lblBalance
        '
        Me.lblBalance.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBalance.BackColor = System.Drawing.Color.Black
        Me.lblBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!)
        Me.lblBalance.ForeColor = System.Drawing.Color.White
        Me.lblBalance.Location = New System.Drawing.Point(415, 2)
        Me.lblBalance.Name = "lblBalance"
        Me.lblBalance.Size = New System.Drawing.Size(266, 64)
        Me.lblBalance.TabIndex = 1199
        Me.lblBalance.Text = "0.00"
        Me.lblBalance.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'txtVCEName
        '
        Me.txtVCEName.BackColor = System.Drawing.Color.White
        Me.txtVCEName.Location = New System.Drawing.Point(82, 24)
        Me.txtVCEName.Name = "txtVCEName"
        Me.txtVCEName.ReadOnly = True
        Me.txtVCEName.Size = New System.Drawing.Size(165, 20)
        Me.txtVCEName.TabIndex = 1197
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(3, 49)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(50, 13)
        Me.Label6.TabIndex = 1196
        Me.Label6.Text = "Account:"
        '
        'txtVCECode
        '
        Me.txtVCECode.BackColor = System.Drawing.Color.White
        Me.txtVCECode.Location = New System.Drawing.Point(82, 2)
        Me.txtVCECode.Name = "txtVCECode"
        Me.txtVCECode.ReadOnly = True
        Me.txtVCECode.Size = New System.Drawing.Size(165, 20)
        Me.txtVCECode.TabIndex = 1197
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(3, 27)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(62, 13)
        Me.Label5.TabIndex = 1196
        Me.Label5.Text = "VCE Name:"
        '
        'txtAccount
        '
        Me.txtAccount.BackColor = System.Drawing.Color.White
        Me.txtAccount.Location = New System.Drawing.Point(82, 46)
        Me.txtAccount.Name = "txtAccount"
        Me.txtAccount.ReadOnly = True
        Me.txtAccount.Size = New System.Drawing.Size(165, 20)
        Me.txtAccount.TabIndex = 1197
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(3, 5)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(59, 13)
        Me.Label4.TabIndex = 1196
        Me.Label4.Text = "VCE Code:"
        '
        'dgvLedger
        '
        Me.dgvLedger.AllowUserToAddRows = False
        Me.dgvLedger.AllowUserToDeleteRows = False
        Me.dgvLedger.AllowUserToResizeColumns = False
        Me.dgvLedger.AllowUserToResizeRows = False
        Me.dgvLedger.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvLedger.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvLedger.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colNum, Me.colRefType, Me.colRefID, Me.colDate, Me.colDeposit, Me.colWithdrawal, Me.colBalance})
        Me.dgvLedger.Location = New System.Drawing.Point(2, 68)
        Me.dgvLedger.MultiSelect = False
        Me.dgvLedger.Name = "dgvLedger"
        Me.dgvLedger.ReadOnly = True
        Me.dgvLedger.RowHeadersVisible = False
        Me.dgvLedger.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvLedger.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvLedger.Size = New System.Drawing.Size(679, 429)
        Me.dgvLedger.TabIndex = 0
        '
        'colNum
        '
        Me.colNum.HeaderText = "No."
        Me.colNum.Name = "colNum"
        Me.colNum.ReadOnly = True
        Me.colNum.Width = 30
        '
        'colRefType
        '
        Me.colRefType.HeaderText = "RefType"
        Me.colRefType.Name = "colRefType"
        Me.colRefType.ReadOnly = True
        Me.colRefType.Width = 50
        '
        'colRefID
        '
        Me.colRefID.HeaderText = "RefID"
        Me.colRefID.Name = "colRefID"
        Me.colRefID.ReadOnly = True
        Me.colRefID.Width = 60
        '
        'colDate
        '
        Me.colDate.HeaderText = "Date"
        Me.colDate.Name = "colDate"
        Me.colDate.ReadOnly = True
        Me.colDate.Width = 70
        '
        'colDeposit
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.colDeposit.DefaultCellStyle = DataGridViewCellStyle1
        Me.colDeposit.HeaderText = "Deposit"
        Me.colDeposit.Name = "colDeposit"
        Me.colDeposit.ReadOnly = True
        Me.colDeposit.Width = 150
        '
        'colWithdrawal
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.colWithdrawal.DefaultCellStyle = DataGridViewCellStyle2
        Me.colWithdrawal.HeaderText = "Withdrawal"
        Me.colWithdrawal.Name = "colWithdrawal"
        Me.colWithdrawal.ReadOnly = True
        Me.colWithdrawal.Width = 150
        '
        'colBalance
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.colBalance.DefaultCellStyle = DataGridViewCellStyle3
        Me.colBalance.HeaderText = "Balance"
        Me.colBalance.Name = "colBalance"
        Me.colBalance.ReadOnly = True
        Me.colBalance.Width = 150
        '
        'frmVerifier_DetailOthers
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(689, 535)
        Me.Controls.Add(Me.Panel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmVerifier_DetailOthers"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Details"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.dgvLedger, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents btnPrintTransaction As System.Windows.Forms.Button
    Friend WithEvents txtLine As System.Windows.Forms.TextBox
    Friend WithEvents btnMin As System.Windows.Forms.Button
    Friend WithEvents txtPage As System.Windows.Forms.TextBox
    Friend WithEvents btnMax As System.Windows.Forms.Button
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents btnPrev As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lblBalance As System.Windows.Forms.Label
    Friend WithEvents txtVCEName As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtVCECode As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtAccount As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents dgvLedger As System.Windows.Forms.DataGridView
    Friend WithEvents colNum As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colRefType As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colRefID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colDeposit As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colWithdrawal As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBalance As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
