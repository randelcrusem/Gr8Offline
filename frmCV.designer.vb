﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCV
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCV))
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtBankRef = New System.Windows.Forms.TextBox()
        Me.txtTransNum = New System.Windows.Forms.TextBox()
        Me.txtBankRefAmount = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtTotalDebit = New System.Windows.Forms.TextBox()
        Me.txtTotalCredit = New System.Windows.Forms.TextBox()
        Me.dtpDocDate = New System.Windows.Forms.DateTimePicker()
        Me.dgvEntry = New System.Windows.Forms.DataGridView()
        Me.dgcBranchCode = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.chAccntCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.chAccntTitle = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.chDebit = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.chCredit = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.chVCECode = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.chVCEName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.chParticulars = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.chRef = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.chCostID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.chCostCenter = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.Column12 = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.txtRemarks = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.dtpBankRefDate = New System.Windows.Forms.DateTimePicker()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.cbPaymentType = New System.Windows.Forms.ComboBox()
        Me.cbBank = New System.Windows.Forms.ComboBox()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.cbDisburseType = New System.Windows.Forms.ComboBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.txtORNo = New System.Windows.Forms.TextBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.gbPayee = New System.Windows.Forms.GroupBox()
        Me.txtCARef = New System.Windows.Forms.TextBox()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.txtLoanRef = New System.Windows.Forms.TextBox()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.tcPayment = New System.Windows.Forms.TabControl()
        Me.tpCash = New System.Windows.Forms.TabPage()
        Me.txtCashAmount = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.tpCheck = New System.Windows.Forms.TabPage()
        Me.gbBank = New System.Windows.Forms.GroupBox()
        Me.txtBankRefName = New System.Windows.Forms.TextBox()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.txtRefStatus = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.tpMultipleCheck = New System.Windows.Forms.TabPage()
        Me.dgvMultipleCheck = New System.Windows.Forms.DataGridView()
        Me.dgcBankID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgcBank = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.dgcCheckNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgcCheckDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgcAmount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgcCheckVCECode = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgcCheckName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgcStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.tpBankTransfer = New System.Windows.Forms.TabPage()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.tpMC = New System.Windows.Forms.TabPage()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.tpCreditCard = New System.Windows.Forms.TabPage()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.txtRFPRef = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.btnUOMGroup = New System.Windows.Forms.Button()
        Me.txtADVRef = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.btnSearchVCE = New System.Windows.Forms.Button()
        Me.txtVCEName = New System.Windows.Forms.TextBox()
        Me.txtVCECode = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtStatus = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtAPVRef = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtAmount = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.EditEntriesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbSearch = New System.Windows.Forms.ToolStripButton()
        Me.tsbNew = New System.Windows.Forms.ToolStripButton()
        Me.tsbEdit = New System.Windows.Forms.ToolStripButton()
        Me.tsbSave = New System.Windows.Forms.ToolStripButton()
        Me.tsbCancel = New System.Windows.Forms.ToolStripButton()
        Me.tsbDelete = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbOption = New System.Windows.Forms.ToolStripDropDownButton()
        Me.CancelCheckToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmUnreleased = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsbCopy = New System.Windows.Forms.ToolStripDropDownButton()
        Me.tsbCopyAPV = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsbCopyADV = New System.Windows.Forms.ToolStripMenuItem()
        Me.FromRFPToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FromLoanToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FromFundsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FromCAToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FromPCVToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbPrint = New System.Windows.Forms.ToolStripSplitButton()
        Me.PrintCVToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChequieToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BIR2307ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsbReports = New System.Windows.Forms.ToolStripDropDownButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbPrevious = New System.Windows.Forms.ToolStripButton()
        Me.tsbNext = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbClose = New System.Windows.Forms.ToolStripButton()
        Me.tsbExit = New System.Windows.Forms.ToolStripButton()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        CType(Me.dgvEntry, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbPayee.SuspendLayout()
        Me.tcPayment.SuspendLayout()
        Me.tpCash.SuspendLayout()
        Me.tpCheck.SuspendLayout()
        Me.gbBank.SuspendLayout()
        Me.tpMultipleCheck.SuspendLayout()
        CType(Me.dgvMultipleCheck, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpBankTransfer.SuspendLayout()
        Me.tpMC.SuspendLayout()
        Me.tpCreditCard.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(15, 72)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(77, 16)
        Me.Label6.TabIndex = 91
        Me.Label6.Text = "Check No. :"
        '
        'txtBankRef
        '
        Me.txtBankRef.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankRef.Location = New System.Drawing.Point(93, 69)
        Me.txtBankRef.Name = "txtBankRef"
        Me.txtBankRef.Size = New System.Drawing.Size(210, 22)
        Me.txtBankRef.TabIndex = 12
        '
        'txtTransNum
        '
        Me.txtTransNum.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTransNum.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTransNum.Location = New System.Drawing.Point(939, 13)
        Me.txtTransNum.Name = "txtTransNum"
        Me.txtTransNum.Size = New System.Drawing.Size(132, 22)
        Me.txtTransNum.TabIndex = 16
        '
        'txtBankRefAmount
        '
        Me.txtBankRefAmount.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankRefAmount.Location = New System.Drawing.Point(93, 119)
        Me.txtBankRefAmount.Name = "txtBankRefAmount"
        Me.txtBankRefAmount.ReadOnly = True
        Me.txtBankRefAmount.Size = New System.Drawing.Size(210, 22)
        Me.txtBankRefAmount.TabIndex = 14
        Me.txtBankRefAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(31, 121)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(61, 16)
        Me.Label7.TabIndex = 92
        Me.Label7.Text = "Amount :"
        '
        'Label8
        '
        Me.Label8.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(292, 536)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(73, 16)
        Me.Label8.TabIndex = 98
        Me.Label8.Text = "Total Debit:"
        '
        'txtTotalDebit
        '
        Me.txtTotalDebit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtTotalDebit.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalDebit.Location = New System.Drawing.Point(371, 536)
        Me.txtTotalDebit.Name = "txtTotalDebit"
        Me.txtTotalDebit.ReadOnly = True
        Me.txtTotalDebit.Size = New System.Drawing.Size(161, 22)
        Me.txtTotalDebit.TabIndex = 97
        Me.txtTotalDebit.TabStop = False
        '
        'txtTotalCredit
        '
        Me.txtTotalCredit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtTotalCredit.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalCredit.Location = New System.Drawing.Point(616, 536)
        Me.txtTotalCredit.Name = "txtTotalCredit"
        Me.txtTotalCredit.ReadOnly = True
        Me.txtTotalCredit.Size = New System.Drawing.Size(161, 22)
        Me.txtTotalCredit.TabIndex = 96
        Me.txtTotalCredit.TabStop = False
        '
        'dtpDocDate
        '
        Me.dtpDocDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dtpDocDate.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpDocDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDocDate.Location = New System.Drawing.Point(939, 38)
        Me.dtpDocDate.Name = "dtpDocDate"
        Me.dtpDocDate.Size = New System.Drawing.Size(132, 22)
        Me.dtpDocDate.TabIndex = 17
        '
        'dgvEntry
        '
        Me.dgvEntry.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvEntry.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvEntry.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dgcBranchCode, Me.chAccntCode, Me.chAccntTitle, Me.chDebit, Me.chCredit, Me.chVCECode, Me.chVCEName, Me.chParticulars, Me.chRef, Me.chCostID, Me.chCostCenter, Me.Column12})
        Me.dgvEntry.Location = New System.Drawing.Point(3, 2)
        Me.dgvEntry.Name = "dgvEntry"
        Me.dgvEntry.RowHeadersWidth = 25
        Me.dgvEntry.Size = New System.Drawing.Size(1063, 203)
        Me.dgvEntry.TabIndex = 21
        '
        'dgcBranchCode
        '
        Me.dgcBranchCode.HeaderText = "Branch Code"
        Me.dgcBranchCode.Name = "dgcBranchCode"
        Me.dgcBranchCode.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgcBranchCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.dgcBranchCode.Width = 50
        '
        'chAccntCode
        '
        Me.chAccntCode.HeaderText = "Account Code"
        Me.chAccntCode.Name = "chAccntCode"
        '
        'chAccntTitle
        '
        Me.chAccntTitle.HeaderText = "Account Title"
        Me.chAccntTitle.Name = "chAccntTitle"
        Me.chAccntTitle.Width = 250
        '
        'chDebit
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle1.Format = "999,999,999.00"
        DataGridViewCellStyle1.NullValue = "0.00"
        Me.chDebit.DefaultCellStyle = DataGridViewCellStyle1
        Me.chDebit.HeaderText = "Debit"
        Me.chDebit.Name = "chDebit"
        '
        'chCredit
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.Format = "999,999,999.00"
        DataGridViewCellStyle2.NullValue = "0.00"
        Me.chCredit.DefaultCellStyle = DataGridViewCellStyle2
        Me.chCredit.HeaderText = "Credit"
        Me.chCredit.Name = "chCredit"
        '
        'chVCECode
        '
        Me.chVCECode.HeaderText = "VCECode"
        Me.chVCECode.Name = "chVCECode"
        Me.chVCECode.Width = 80
        '
        'chVCEName
        '
        Me.chVCEName.HeaderText = "VCEName"
        Me.chVCEName.Name = "chVCEName"
        Me.chVCEName.Width = 160
        '
        'chParticulars
        '
        Me.chParticulars.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomRight
        Me.chParticulars.DefaultCellStyle = DataGridViewCellStyle3
        Me.chParticulars.HeaderText = "Particulars"
        Me.chParticulars.Name = "chParticulars"
        Me.chParticulars.Width = 74
        '
        'chRef
        '
        Me.chRef.HeaderText = "RefNo"
        Me.chRef.Name = "chRef"
        '
        'chCostID
        '
        Me.chCostID.HeaderText = "Cost ID"
        Me.chCostID.Name = "chCostID"
        Me.chCostID.Visible = False
        '
        'chCostCenter
        '
        Me.chCostCenter.HeaderText = "Cost Center"
        Me.chCostCenter.Name = "chCostCenter"
        '
        'Column12
        '
        Me.Column12.HeaderText = ">>"
        Me.Column12.Name = "Column12"
        Me.Column12.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column12.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Column12.Width = 50
        '
        'txtRemarks
        '
        Me.txtRemarks.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRemarks.Location = New System.Drawing.Point(129, 167)
        Me.txtRemarks.Multiline = True
        Me.txtRemarks.Name = "txtRemarks"
        Me.txtRemarks.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtRemarks.Size = New System.Drawing.Size(316, 51)
        Me.txtRemarks.TabIndex = 9
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Arial", 9.75!)
        Me.Label10.Location = New System.Drawing.Point(60, 167)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(68, 16)
        Me.Label10.TabIndex = 108
        Me.Label10.Text = "Remarks :"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(8, 98)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(84, 16)
        Me.Label11.TabIndex = 113
        Me.Label11.Text = "Check Date :"
        '
        'dtpBankRefDate
        '
        Me.dtpBankRefDate.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpBankRefDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpBankRefDate.Location = New System.Drawing.Point(93, 94)
        Me.dtpBankRefDate.Name = "dtpBankRefDate"
        Me.dtpBankRefDate.Size = New System.Drawing.Size(210, 22)
        Me.dtpBankRefDate.TabIndex = 13
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(46, 22)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(46, 16)
        Me.Label12.TabIndex = 116
        Me.Label12.Text = "Bank :"
        '
        'cbPaymentType
        '
        Me.cbPaymentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPaymentType.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbPaymentType.FormattingEnabled = True
        Me.cbPaymentType.Items.AddRange(New Object() {"Cash", "Check", "Multiple Check", "Manager's Check", "Bank Transfer", "Credit Card", "(Multiple Payment Method)"})
        Me.cbPaymentType.Location = New System.Drawing.Point(129, 64)
        Me.cbPaymentType.Name = "cbPaymentType"
        Me.cbPaymentType.Size = New System.Drawing.Size(319, 24)
        Me.cbPaymentType.TabIndex = 4
        '
        'cbBank
        '
        Me.cbBank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbBank.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbBank.FormattingEnabled = True
        Me.cbBank.Location = New System.Drawing.Point(93, 18)
        Me.cbBank.Name = "cbBank"
        Me.cbBank.Size = New System.Drawing.Size(210, 24)
        Me.cbBank.TabIndex = 10
        '
        'Button3
        '
        Me.Button3.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Button3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button3.Location = New System.Drawing.Point(308, 16)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(33, 28)
        Me.Button3.TabIndex = 11
        Me.Button3.Text = ">>"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Label20
        '
        Me.Label20.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(541, 536)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(77, 16)
        Me.Label20.TabIndex = 129
        Me.Label20.Text = "Total Credit:"
        '
        'cbDisburseType
        '
        Me.cbDisburseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbDisburseType.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbDisburseType.FormattingEnabled = True
        Me.cbDisburseType.Location = New System.Drawing.Point(129, 116)
        Me.cbDisburseType.Name = "cbDisburseType"
        Me.cbDisburseType.Size = New System.Drawing.Size(167, 24)
        Me.cbDisburseType.TabIndex = 6
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Arial", 9.75!)
        Me.Label22.Location = New System.Drawing.Point(30, 117)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(98, 16)
        Me.Label22.TabIndex = 140
        Me.Label22.Text = "Expense Type :"
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Arial", 9.75!)
        Me.Label23.Location = New System.Drawing.Point(9, 142)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(119, 16)
        Me.Label23.TabIndex = 142
        Me.Label23.Text = "Costumer OR No. :"
        '
        'txtORNo
        '
        Me.txtORNo.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtORNo.Location = New System.Drawing.Point(129, 143)
        Me.txtORNo.Name = "txtORNo"
        Me.txtORNo.Size = New System.Drawing.Size(167, 22)
        Me.txtORNo.TabIndex = 8
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Font = New System.Drawing.Font("Arial", 9.75!)
        Me.Label25.Location = New System.Drawing.Point(13, 67)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(115, 16)
        Me.Label25.TabIndex = 1340
        Me.Label25.Text = "Payment Method :"
        '
        'gbPayee
        '
        Me.gbPayee.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbPayee.Controls.Add(Me.txtCARef)
        Me.gbPayee.Controls.Add(Me.Label32)
        Me.gbPayee.Controls.Add(Me.txtLoanRef)
        Me.gbPayee.Controls.Add(Me.Label31)
        Me.gbPayee.Controls.Add(Me.tcPayment)
        Me.gbPayee.Controls.Add(Me.txtRFPRef)
        Me.gbPayee.Controls.Add(Me.Label17)
        Me.gbPayee.Controls.Add(Me.Label16)
        Me.gbPayee.Controls.Add(Me.btnUOMGroup)
        Me.gbPayee.Controls.Add(Me.txtADVRef)
        Me.gbPayee.Controls.Add(Me.Label14)
        Me.gbPayee.Controls.Add(Me.btnSearchVCE)
        Me.gbPayee.Controls.Add(Me.txtVCEName)
        Me.gbPayee.Controls.Add(Me.txtVCECode)
        Me.gbPayee.Controls.Add(Me.Label2)
        Me.gbPayee.Controls.Add(Me.Label3)
        Me.gbPayee.Controls.Add(Me.txtStatus)
        Me.gbPayee.Controls.Add(Me.Label15)
        Me.gbPayee.Controls.Add(Me.txtAPVRef)
        Me.gbPayee.Controls.Add(Me.Label4)
        Me.gbPayee.Controls.Add(Me.txtAmount)
        Me.gbPayee.Controls.Add(Me.Label1)
        Me.gbPayee.Controls.Add(Me.Label13)
        Me.gbPayee.Controls.Add(Me.Label9)
        Me.gbPayee.Controls.Add(Me.txtORNo)
        Me.gbPayee.Controls.Add(Me.Label23)
        Me.gbPayee.Controls.Add(Me.Label10)
        Me.gbPayee.Controls.Add(Me.txtRemarks)
        Me.gbPayee.Controls.Add(Me.cbPaymentType)
        Me.gbPayee.Controls.Add(Me.cbDisburseType)
        Me.gbPayee.Controls.Add(Me.Label22)
        Me.gbPayee.Controls.Add(Me.dtpDocDate)
        Me.gbPayee.Controls.Add(Me.Label25)
        Me.gbPayee.Controls.Add(Me.txtTransNum)
        Me.gbPayee.Location = New System.Drawing.Point(8, 40)
        Me.gbPayee.Name = "gbPayee"
        Me.gbPayee.Size = New System.Drawing.Size(1077, 243)
        Me.gbPayee.TabIndex = 47
        Me.gbPayee.TabStop = False
        '
        'txtCARef
        '
        Me.txtCARef.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCARef.Enabled = False
        Me.txtCARef.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCARef.Location = New System.Drawing.Point(939, 88)
        Me.txtCARef.Name = "txtCARef"
        Me.txtCARef.Size = New System.Drawing.Size(132, 22)
        Me.txtCARef.TabIndex = 1374
        Me.txtCARef.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label32
        '
        Me.Label32.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label32.AutoSize = True
        Me.Label32.Font = New System.Drawing.Font("Arial", 9.75!)
        Me.Label32.Location = New System.Drawing.Point(877, 90)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(60, 16)
        Me.Label32.TabIndex = 1375
        Me.Label32.Text = "CA Ref. :"
        '
        'txtLoanRef
        '
        Me.txtLoanRef.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLoanRef.Enabled = False
        Me.txtLoanRef.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLoanRef.Location = New System.Drawing.Point(939, 164)
        Me.txtLoanRef.Name = "txtLoanRef"
        Me.txtLoanRef.Size = New System.Drawing.Size(132, 22)
        Me.txtLoanRef.TabIndex = 1372
        Me.txtLoanRef.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtLoanRef.Visible = False
        '
        'Label31
        '
        Me.Label31.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label31.AutoSize = True
        Me.Label31.Font = New System.Drawing.Font("Arial", 9.75!)
        Me.Label31.Location = New System.Drawing.Point(866, 167)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(71, 16)
        Me.Label31.TabIndex = 1373
        Me.Label31.Text = "Loan Ref. :"
        Me.Label31.Visible = False
        '
        'tcPayment
        '
        Me.tcPayment.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcPayment.Controls.Add(Me.tpCash)
        Me.tcPayment.Controls.Add(Me.tpCheck)
        Me.tcPayment.Controls.Add(Me.tpMultipleCheck)
        Me.tcPayment.Controls.Add(Me.tpBankTransfer)
        Me.tcPayment.Controls.Add(Me.tpMC)
        Me.tcPayment.Controls.Add(Me.tpCreditCard)
        Me.tcPayment.ItemSize = New System.Drawing.Size(41, 25)
        Me.tcPayment.Location = New System.Drawing.Point(485, 14)
        Me.tcPayment.Name = "tcPayment"
        Me.tcPayment.SelectedIndex = 0
        Me.tcPayment.Size = New System.Drawing.Size(372, 216)
        Me.tcPayment.TabIndex = 1371
        '
        'tpCash
        '
        Me.tpCash.Controls.Add(Me.txtCashAmount)
        Me.tpCash.Controls.Add(Me.Label18)
        Me.tpCash.Location = New System.Drawing.Point(4, 29)
        Me.tpCash.Name = "tpCash"
        Me.tpCash.Padding = New System.Windows.Forms.Padding(3)
        Me.tpCash.Size = New System.Drawing.Size(364, 183)
        Me.tpCash.TabIndex = 0
        Me.tpCash.Text = "Cash"
        Me.tpCash.UseVisualStyleBackColor = True
        '
        'txtCashAmount
        '
        Me.txtCashAmount.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCashAmount.Location = New System.Drawing.Point(119, 17)
        Me.txtCashAmount.Name = "txtCashAmount"
        Me.txtCashAmount.Size = New System.Drawing.Size(215, 22)
        Me.txtCashAmount.TabIndex = 1349
        Me.txtCashAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(19, 21)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(94, 16)
        Me.Label18.TabIndex = 1350
        Me.Label18.Text = "Cash Amount :"
        '
        'tpCheck
        '
        Me.tpCheck.Controls.Add(Me.gbBank)
        Me.tpCheck.Location = New System.Drawing.Point(4, 29)
        Me.tpCheck.Name = "tpCheck"
        Me.tpCheck.Padding = New System.Windows.Forms.Padding(3)
        Me.tpCheck.Size = New System.Drawing.Size(364, 183)
        Me.tpCheck.TabIndex = 1
        Me.tpCheck.Text = "Check"
        Me.tpCheck.UseVisualStyleBackColor = True
        '
        'gbBank
        '
        Me.gbBank.Controls.Add(Me.txtBankRefName)
        Me.gbBank.Controls.Add(Me.Label30)
        Me.gbBank.Controls.Add(Me.txtRefStatus)
        Me.gbBank.Controls.Add(Me.Label5)
        Me.gbBank.Controls.Add(Me.cbBank)
        Me.gbBank.Controls.Add(Me.Label12)
        Me.gbBank.Controls.Add(Me.dtpBankRefDate)
        Me.gbBank.Controls.Add(Me.Button3)
        Me.gbBank.Controls.Add(Me.Label11)
        Me.gbBank.Controls.Add(Me.txtBankRefAmount)
        Me.gbBank.Controls.Add(Me.Label7)
        Me.gbBank.Controls.Add(Me.txtBankRef)
        Me.gbBank.Controls.Add(Me.Label6)
        Me.gbBank.Location = New System.Drawing.Point(3, 7)
        Me.gbBank.Name = "gbBank"
        Me.gbBank.Size = New System.Drawing.Size(398, 176)
        Me.gbBank.TabIndex = 10
        Me.gbBank.TabStop = False
        Me.gbBank.Text = "Bank Details"
        '
        'txtBankRefName
        '
        Me.txtBankRefName.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBankRefName.Location = New System.Drawing.Point(93, 45)
        Me.txtBankRefName.Name = "txtBankRefName"
        Me.txtBankRefName.Size = New System.Drawing.Size(210, 22)
        Me.txtBankRefName.TabIndex = 131
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label30.Location = New System.Drawing.Point(42, 48)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(50, 16)
        Me.Label30.TabIndex = 132
        Me.Label30.Text = "Name :"
        '
        'txtRefStatus
        '
        Me.txtRefStatus.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRefStatus.Location = New System.Drawing.Point(93, 143)
        Me.txtRefStatus.Name = "txtRefStatus"
        Me.txtRefStatus.ReadOnly = True
        Me.txtRefStatus.Size = New System.Drawing.Size(210, 22)
        Me.txtRefStatus.TabIndex = 15
        Me.txtRefStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(31, 145)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(54, 16)
        Me.Label5.TabIndex = 130
        Me.Label5.Text = "Status :"
        '
        'tpMultipleCheck
        '
        Me.tpMultipleCheck.Controls.Add(Me.dgvMultipleCheck)
        Me.tpMultipleCheck.Location = New System.Drawing.Point(4, 29)
        Me.tpMultipleCheck.Name = "tpMultipleCheck"
        Me.tpMultipleCheck.Padding = New System.Windows.Forms.Padding(3)
        Me.tpMultipleCheck.Size = New System.Drawing.Size(364, 183)
        Me.tpMultipleCheck.TabIndex = 2
        Me.tpMultipleCheck.Text = "Multiple Check"
        Me.tpMultipleCheck.UseVisualStyleBackColor = True
        '
        'dgvMultipleCheck
        '
        Me.dgvMultipleCheck.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvMultipleCheck.BackgroundColor = System.Drawing.Color.White
        Me.dgvMultipleCheck.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMultipleCheck.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dgcBankID, Me.dgcBank, Me.dgcCheckNo, Me.dgcCheckDate, Me.dgcAmount, Me.dgcCheckVCECode, Me.dgcCheckName, Me.dgcStatus})
        Me.dgvMultipleCheck.Location = New System.Drawing.Point(6, 2)
        Me.dgvMultipleCheck.Name = "dgvMultipleCheck"
        Me.dgvMultipleCheck.RowHeadersWidth = 25
        Me.dgvMultipleCheck.Size = New System.Drawing.Size(352, 173)
        Me.dgvMultipleCheck.TabIndex = 22
        '
        'dgcBankID
        '
        Me.dgcBankID.HeaderText = "BankID"
        Me.dgcBankID.Name = "dgcBankID"
        Me.dgcBankID.Visible = False
        '
        'dgcBank
        '
        Me.dgcBank.HeaderText = "Bank"
        Me.dgcBank.Name = "dgcBank"
        Me.dgcBank.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgcBank.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'dgcCheckNo
        '
        Me.dgcCheckNo.HeaderText = "Check No."
        Me.dgcCheckNo.Name = "dgcCheckNo"
        '
        'dgcCheckDate
        '
        DataGridViewCellStyle4.Format = "MM/dd/yyyy"
        DataGridViewCellStyle4.NullValue = Nothing
        Me.dgcCheckDate.DefaultCellStyle = DataGridViewCellStyle4
        Me.dgcCheckDate.HeaderText = "Check Date"
        Me.dgcCheckDate.Name = "dgcCheckDate"
        '
        'dgcAmount
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle5.Format = "999,999,999.00"
        DataGridViewCellStyle5.NullValue = "0.00"
        Me.dgcAmount.DefaultCellStyle = DataGridViewCellStyle5
        Me.dgcAmount.HeaderText = "Amount"
        Me.dgcAmount.Name = "dgcAmount"
        '
        'dgcCheckVCECode
        '
        Me.dgcCheckVCECode.HeaderText = "VCE Code"
        Me.dgcCheckVCECode.Name = "dgcCheckVCECode"
        Me.dgcCheckVCECode.Visible = False
        '
        'dgcCheckName
        '
        Me.dgcCheckName.HeaderText = "Check Name"
        Me.dgcCheckName.Name = "dgcCheckName"
        Me.dgcCheckName.Width = 200
        '
        'dgcStatus
        '
        Me.dgcStatus.HeaderText = "Status"
        Me.dgcStatus.Name = "dgcStatus"
        '
        'tpBankTransfer
        '
        Me.tpBankTransfer.Controls.Add(Me.TextBox3)
        Me.tpBankTransfer.Controls.Add(Me.Label26)
        Me.tpBankTransfer.Controls.Add(Me.DateTimePicker1)
        Me.tpBankTransfer.Controls.Add(Me.Label21)
        Me.tpBankTransfer.Controls.Add(Me.TextBox2)
        Me.tpBankTransfer.Controls.Add(Me.Label24)
        Me.tpBankTransfer.Controls.Add(Me.ComboBox1)
        Me.tpBankTransfer.Controls.Add(Me.Label19)
        Me.tpBankTransfer.Controls.Add(Me.Button1)
        Me.tpBankTransfer.Location = New System.Drawing.Point(4, 29)
        Me.tpBankTransfer.Name = "tpBankTransfer"
        Me.tpBankTransfer.Padding = New System.Windows.Forms.Padding(3)
        Me.tpBankTransfer.Size = New System.Drawing.Size(364, 183)
        Me.tpBankTransfer.TabIndex = 3
        Me.tpBankTransfer.Text = "Bank Transfer"
        Me.tpBankTransfer.UseVisualStyleBackColor = True
        '
        'TextBox3
        '
        Me.TextBox3.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox3.Location = New System.Drawing.Point(94, 93)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(210, 22)
        Me.TextBox3.TabIndex = 124
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.Location = New System.Drawing.Point(16, 96)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(74, 16)
        Me.Label26.TabIndex = 125
        Me.Label26.Text = "Reference :"
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateTimePicker1.Location = New System.Drawing.Point(94, 43)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(210, 22)
        Me.DateTimePicker1.TabIndex = 120
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(9, 46)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(84, 16)
        Me.Label21.TabIndex = 123
        Me.Label21.Text = "Check Date :"
        '
        'TextBox2
        '
        Me.TextBox2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(94, 68)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(210, 22)
        Me.TextBox2.TabIndex = 121
        Me.TextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(32, 70)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(61, 16)
        Me.Label24.TabIndex = 122
        Me.Label24.Text = "Amount :"
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(94, 15)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(210, 24)
        Me.ComboBox1.TabIndex = 117
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(47, 19)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(46, 16)
        Me.Label19.TabIndex = 119
        Me.Label19.Text = "Bank :"
        '
        'Button1
        '
        Me.Button1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(309, 13)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(33, 28)
        Me.Button1.TabIndex = 118
        Me.Button1.Text = ">>"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'tpMC
        '
        Me.tpMC.Controls.Add(Me.TextBox5)
        Me.tpMC.Controls.Add(Me.Label29)
        Me.tpMC.Location = New System.Drawing.Point(4, 29)
        Me.tpMC.Name = "tpMC"
        Me.tpMC.Padding = New System.Windows.Forms.Padding(3)
        Me.tpMC.Size = New System.Drawing.Size(364, 183)
        Me.tpMC.TabIndex = 5
        Me.tpMC.Text = "Manager's Check"
        Me.tpMC.UseVisualStyleBackColor = True
        '
        'TextBox5
        '
        Me.TextBox5.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox5.Location = New System.Drawing.Point(126, 20)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.ReadOnly = True
        Me.TextBox5.Size = New System.Drawing.Size(210, 22)
        Me.TextBox5.TabIndex = 123
        Me.TextBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.Location = New System.Drawing.Point(36, 24)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(84, 16)
        Me.Label29.TabIndex = 124
        Me.Label29.Text = "MC Amount :"
        '
        'tpCreditCard
        '
        Me.tpCreditCard.Controls.Add(Me.TextBox4)
        Me.tpCreditCard.Controls.Add(Me.Label28)
        Me.tpCreditCard.Controls.Add(Me.ComboBox2)
        Me.tpCreditCard.Controls.Add(Me.Label27)
        Me.tpCreditCard.Location = New System.Drawing.Point(4, 29)
        Me.tpCreditCard.Name = "tpCreditCard"
        Me.tpCreditCard.Padding = New System.Windows.Forms.Padding(3)
        Me.tpCreditCard.Size = New System.Drawing.Size(364, 183)
        Me.tpCreditCard.TabIndex = 4
        Me.tpCreditCard.Text = "Credit Card"
        Me.tpCreditCard.UseVisualStyleBackColor = True
        '
        'TextBox4
        '
        Me.TextBox4.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox4.Location = New System.Drawing.Point(114, 44)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.ReadOnly = True
        Me.TextBox4.Size = New System.Drawing.Size(210, 22)
        Me.TextBox4.TabIndex = 123
        Me.TextBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.Location = New System.Drawing.Point(52, 46)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(61, 16)
        Me.Label28.TabIndex = 124
        Me.Label28.Text = "Amount :"
        '
        'ComboBox2
        '
        Me.ComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Location = New System.Drawing.Point(114, 15)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(210, 24)
        Me.ComboBox2.TabIndex = 120
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label27.Location = New System.Drawing.Point(27, 19)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(81, 16)
        Me.Label27.TabIndex = 121
        Me.Label27.Text = "Credit Card :"
        '
        'txtRFPRef
        '
        Me.txtRFPRef.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRFPRef.Enabled = False
        Me.txtRFPRef.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRFPRef.Location = New System.Drawing.Point(939, 138)
        Me.txtRFPRef.Name = "txtRFPRef"
        Me.txtRFPRef.Size = New System.Drawing.Size(132, 22)
        Me.txtRFPRef.TabIndex = 1369
        Me.txtRFPRef.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtRFPRef.Visible = False
        '
        'Label17
        '
        Me.Label17.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Arial", 9.75!)
        Me.Label17.Location = New System.Drawing.Point(868, 140)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(69, 16)
        Me.Label17.TabIndex = 1370
        Me.Label17.Text = "RFP Ref. :"
        Me.Label17.Visible = False
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Arial", 9.75!)
        Me.Label16.Location = New System.Drawing.Point(51, 17)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(77, 16)
        Me.Label16.TabIndex = 1368
        Me.Label16.Text = "VCE Code :"
        '
        'btnUOMGroup
        '
        Me.btnUOMGroup.BackgroundImage = Global.jade.My.Resources.Resources._New
        Me.btnUOMGroup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnUOMGroup.Location = New System.Drawing.Point(299, 114)
        Me.btnUOMGroup.Name = "btnUOMGroup"
        Me.btnUOMGroup.Size = New System.Drawing.Size(25, 25)
        Me.btnUOMGroup.TabIndex = 7
        Me.btnUOMGroup.UseVisualStyleBackColor = True
        '
        'txtADVRef
        '
        Me.txtADVRef.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtADVRef.Enabled = False
        Me.txtADVRef.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtADVRef.Location = New System.Drawing.Point(939, 192)
        Me.txtADVRef.Name = "txtADVRef"
        Me.txtADVRef.Size = New System.Drawing.Size(132, 22)
        Me.txtADVRef.TabIndex = 20
        Me.txtADVRef.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtADVRef.Visible = False
        '
        'Label14
        '
        Me.Label14.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Arial", 9.75!)
        Me.Label14.Location = New System.Drawing.Point(867, 194)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(70, 16)
        Me.Label14.TabIndex = 1366
        Me.Label14.Text = "ADV Ref. :"
        Me.Label14.Visible = False
        '
        'btnSearchVCE
        '
        Me.btnSearchVCE.BackgroundImage = Global.jade.My.Resources.Resources.view
        Me.btnSearchVCE.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSearchVCE.Location = New System.Drawing.Point(454, 13)
        Me.btnSearchVCE.Name = "btnSearchVCE"
        Me.btnSearchVCE.Size = New System.Drawing.Size(25, 25)
        Me.btnSearchVCE.TabIndex = 2
        Me.btnSearchVCE.UseVisualStyleBackColor = True
        '
        'txtVCEName
        '
        Me.txtVCEName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVCEName.Location = New System.Drawing.Point(129, 39)
        Me.txtVCEName.Name = "txtVCEName"
        Me.txtVCEName.Size = New System.Drawing.Size(319, 22)
        Me.txtVCEName.TabIndex = 3
        '
        'txtVCECode
        '
        Me.txtVCECode.Enabled = False
        Me.txtVCECode.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVCECode.Location = New System.Drawing.Point(129, 14)
        Me.txtVCECode.Name = "txtVCECode"
        Me.txtVCECode.Size = New System.Drawing.Size(319, 22)
        Me.txtVCECode.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 9.75!)
        Me.Label2.Location = New System.Drawing.Point(47, 42)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(81, 16)
        Me.Label2.TabIndex = 1360
        Me.Label2.Text = "VCE Name :"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 9.75!)
        Me.Label3.Location = New System.Drawing.Point(31, 17)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(16, 16)
        Me.Label3.TabIndex = 1361
        Me.Label3.Text = "  "
        '
        'txtStatus
        '
        Me.txtStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtStatus.Enabled = False
        Me.txtStatus.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStatus.Location = New System.Drawing.Point(939, 63)
        Me.txtStatus.Name = "txtStatus"
        Me.txtStatus.Size = New System.Drawing.Size(132, 22)
        Me.txtStatus.TabIndex = 18
        Me.txtStatus.Text = "Open"
        '
        'Label15
        '
        Me.Label15.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Arial", 9.75!)
        Me.Label15.Location = New System.Drawing.Point(883, 66)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(54, 16)
        Me.Label15.TabIndex = 1355
        Me.Label15.Text = "Status :"
        '
        'txtAPVRef
        '
        Me.txtAPVRef.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAPVRef.Enabled = False
        Me.txtAPVRef.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAPVRef.Location = New System.Drawing.Point(938, 114)
        Me.txtAPVRef.Name = "txtAPVRef"
        Me.txtAPVRef.Size = New System.Drawing.Size(132, 22)
        Me.txtAPVRef.TabIndex = 19
        Me.txtAPVRef.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtAPVRef.Visible = False
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Arial", 9.75!)
        Me.Label4.Location = New System.Drawing.Point(866, 116)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(70, 16)
        Me.Label4.TabIndex = 1350
        Me.Label4.Text = "APV Ref. :"
        Me.Label4.Visible = False
        '
        'txtAmount
        '
        Me.txtAmount.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmount.Location = New System.Drawing.Point(129, 91)
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.Size = New System.Drawing.Size(167, 22)
        Me.txtAmount.TabIndex = 5
        Me.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 9.75!)
        Me.Label1.Location = New System.Drawing.Point(67, 92)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 16)
        Me.Label1.TabIndex = 1348
        Me.Label1.Text = "Amount :"
        '
        'Label13
        '
        Me.Label13.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Arial", 9.75!)
        Me.Label13.Location = New System.Drawing.Point(872, 42)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(65, 16)
        Me.Label13.TabIndex = 1346
        Me.Label13.Text = "CV Date :"
        '
        'Label9
        '
        Me.Label9.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Arial", 9.75!)
        Me.Label9.Location = New System.Drawing.Point(879, 16)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(58, 16)
        Me.Label9.TabIndex = 1345
        Me.Label9.Text = "CV No. :"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditEntriesToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(125, 26)
        '
        'EditEntriesToolStripMenuItem
        '
        Me.EditEntriesToolStripMenuItem.Name = "EditEntriesToolStripMenuItem"
        Me.EditEntriesToolStripMenuItem.Size = New System.Drawing.Size(124, 22)
        Me.EditEntriesToolStripMenuItem.Text = "Edit Entry"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.AutoSize = False
        Me.ToolStrip1.BackColor = System.Drawing.Color.FromArgb(CType(CType(89, Byte), Integer), CType(CType(136, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbSearch, Me.tsbNew, Me.tsbEdit, Me.tsbSave, Me.tsbCancel, Me.tsbDelete, Me.ToolStripSeparator1, Me.tsbOption, Me.tsbCopy, Me.ToolStripSeparator4, Me.tsbPrint, Me.tsbReports, Me.ToolStripSeparator2, Me.tsbPrevious, Me.tsbNext, Me.ToolStripSeparator3, Me.tsbClose, Me.tsbExit})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1091, 40)
        Me.ToolStrip1.TabIndex = 1344
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbSearch
        '
        Me.tsbSearch.AutoSize = False
        Me.tsbSearch.ForeColor = System.Drawing.Color.White
        Me.tsbSearch.Image = Global.jade.My.Resources.Resources.view
        Me.tsbSearch.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.tsbSearch.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSearch.Name = "tsbSearch"
        Me.tsbSearch.Size = New System.Drawing.Size(50, 35)
        Me.tsbSearch.Text = "Search"
        Me.tsbSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.tsbSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'tsbNew
        '
        Me.tsbNew.AutoSize = False
        Me.tsbNew.ForeColor = System.Drawing.Color.White
        Me.tsbNew.Image = Global.jade.My.Resources.Resources.circle_document_documents_extension_file_page_sheet_icon_7
        Me.tsbNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbNew.Name = "tsbNew"
        Me.tsbNew.Size = New System.Drawing.Size(50, 35)
        Me.tsbNew.Text = "New"
        Me.tsbNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'tsbEdit
        '
        Me.tsbEdit.AutoSize = False
        Me.tsbEdit.ForeColor = System.Drawing.Color.White
        Me.tsbEdit.Image = Global.jade.My.Resources.Resources.edit_pen_write_notes_document_3c679c93cb5d1fed_512x512
        Me.tsbEdit.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbEdit.Name = "tsbEdit"
        Me.tsbEdit.Size = New System.Drawing.Size(50, 35)
        Me.tsbEdit.Text = "Edit"
        Me.tsbEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'tsbSave
        '
        Me.tsbSave.AutoSize = False
        Me.tsbSave.ForeColor = System.Drawing.Color.White
        Me.tsbSave.Image = Global.jade.My.Resources.Resources.Save_Icon
        Me.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSave.Name = "tsbSave"
        Me.tsbSave.Size = New System.Drawing.Size(50, 35)
        Me.tsbSave.Text = "Save"
        Me.tsbSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'tsbCancel
        '
        Me.tsbCancel.AutoSize = False
        Me.tsbCancel.ForeColor = System.Drawing.Color.White
        Me.tsbCancel.Image = Global.jade.My.Resources.Resources.close_icon
        Me.tsbCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbCancel.Name = "tsbCancel"
        Me.tsbCancel.Size = New System.Drawing.Size(50, 35)
        Me.tsbCancel.Text = "Cancel"
        Me.tsbCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.tsbCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'tsbDelete
        '
        Me.tsbDelete.AutoSize = False
        Me.tsbDelete.ForeColor = System.Drawing.Color.White
        Me.tsbDelete.Image = Global.jade.My.Resources.Resources.close_icon
        Me.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDelete.Name = "tsbDelete"
        Me.tsbDelete.Size = New System.Drawing.Size(50, 35)
        Me.tsbDelete.Text = "Delete"
        Me.tsbDelete.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.tsbDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 40)
        '
        'tsbOption
        '
        Me.tsbOption.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CancelCheckToolStripMenuItem, Me.tsmUnreleased})
        Me.tsbOption.ForeColor = System.Drawing.Color.White
        Me.tsbOption.Image = Global.jade.My.Resources.Resources.finance_report_infographic_512
        Me.tsbOption.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbOption.Name = "tsbOption"
        Me.tsbOption.Size = New System.Drawing.Size(57, 37)
        Me.tsbOption.Text = "Option"
        Me.tsbOption.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'CancelCheckToolStripMenuItem
        '
        Me.CancelCheckToolStripMenuItem.Name = "CancelCheckToolStripMenuItem"
        Me.CancelCheckToolStripMenuItem.Size = New System.Drawing.Size(157, 22)
        Me.CancelCheckToolStripMenuItem.Text = "Cancel Cheque"
        '
        'tsmUnreleased
        '
        Me.tsmUnreleased.Enabled = False
        Me.tsmUnreleased.Name = "tsmUnreleased"
        Me.tsmUnreleased.Size = New System.Drawing.Size(157, 22)
        Me.tsmUnreleased.Text = "Release Cheque"
        '
        'tsbCopy
        '
        Me.tsbCopy.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbCopyAPV, Me.tsbCopyADV, Me.FromRFPToolStripMenuItem, Me.FromLoanToolStripMenuItem, Me.FromFundsToolStripMenuItem, Me.FromCAToolStripMenuItem, Me.FromPCVToolStripMenuItem})
        Me.tsbCopy.ForeColor = System.Drawing.Color.White
        Me.tsbCopy.Image = Global.jade.My.Resources.Resources.finance_report_infographic_512
        Me.tsbCopy.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbCopy.Name = "tsbCopy"
        Me.tsbCopy.Size = New System.Drawing.Size(48, 37)
        Me.tsbCopy.Text = "Copy"
        Me.tsbCopy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'tsbCopyAPV
        '
        Me.tsbCopyAPV.Name = "tsbCopyAPV"
        Me.tsbCopyAPV.Size = New System.Drawing.Size(156, 22)
        Me.tsbCopyAPV.Text = "From APV"
        Me.tsbCopyAPV.Visible = False
        '
        'tsbCopyADV
        '
        Me.tsbCopyADV.Name = "tsbCopyADV"
        Me.tsbCopyADV.Size = New System.Drawing.Size(156, 22)
        Me.tsbCopyADV.Text = "From Advances"
        Me.tsbCopyADV.Visible = False
        '
        'FromRFPToolStripMenuItem
        '
        Me.FromRFPToolStripMenuItem.Name = "FromRFPToolStripMenuItem"
        Me.FromRFPToolStripMenuItem.Size = New System.Drawing.Size(156, 22)
        Me.FromRFPToolStripMenuItem.Text = "From RFP"
        Me.FromRFPToolStripMenuItem.Visible = False
        '
        'FromLoanToolStripMenuItem
        '
        Me.FromLoanToolStripMenuItem.Name = "FromLoanToolStripMenuItem"
        Me.FromLoanToolStripMenuItem.Size = New System.Drawing.Size(156, 22)
        Me.FromLoanToolStripMenuItem.Text = "From Loans"
        Me.FromLoanToolStripMenuItem.Visible = False
        '
        'FromFundsToolStripMenuItem
        '
        Me.FromFundsToolStripMenuItem.Name = "FromFundsToolStripMenuItem"
        Me.FromFundsToolStripMenuItem.Size = New System.Drawing.Size(156, 22)
        Me.FromFundsToolStripMenuItem.Text = "From Funds"
        Me.FromFundsToolStripMenuItem.Visible = False
        '
        'FromCAToolStripMenuItem
        '
        Me.FromCAToolStripMenuItem.Name = "FromCAToolStripMenuItem"
        Me.FromCAToolStripMenuItem.Size = New System.Drawing.Size(156, 22)
        Me.FromCAToolStripMenuItem.Text = "From CA"
        '
        'FromPCVToolStripMenuItem
        '
        Me.FromPCVToolStripMenuItem.Name = "FromPCVToolStripMenuItem"
        Me.FromPCVToolStripMenuItem.Size = New System.Drawing.Size(156, 22)
        Me.FromPCVToolStripMenuItem.Text = "From PCV"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 40)
        '
        'tsbPrint
        '
        Me.tsbPrint.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PrintCVToolStripMenuItem, Me.ChequieToolStripMenuItem, Me.BIR2307ToolStripMenuItem})
        Me.tsbPrint.ForeColor = System.Drawing.Color.White
        Me.tsbPrint.Image = Global.jade.My.Resources.Resources.printer_circle_blue_512
        Me.tsbPrint.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPrint.Name = "tsbPrint"
        Me.tsbPrint.Size = New System.Drawing.Size(48, 37)
        Me.tsbPrint.Text = "Print"
        Me.tsbPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'PrintCVToolStripMenuItem
        '
        Me.PrintCVToolStripMenuItem.Name = "PrintCVToolStripMenuItem"
        Me.PrintCVToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.PrintCVToolStripMenuItem.Text = "Check Voucher"
        '
        'ChequieToolStripMenuItem
        '
        Me.ChequieToolStripMenuItem.Name = "ChequieToolStripMenuItem"
        Me.ChequieToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.ChequieToolStripMenuItem.Text = "Check"
        '
        'BIR2307ToolStripMenuItem
        '
        Me.BIR2307ToolStripMenuItem.Name = "BIR2307ToolStripMenuItem"
        Me.BIR2307ToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.BIR2307ToolStripMenuItem.Text = "BIR 2307"
        '
        'tsbReports
        '
        Me.tsbReports.ForeColor = System.Drawing.Color.White
        Me.tsbReports.Image = Global.jade.My.Resources.Resources.finance_report_infographic_512
        Me.tsbReports.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbReports.Name = "tsbReports"
        Me.tsbReports.Size = New System.Drawing.Size(60, 37)
        Me.tsbReports.Text = "Reports"
        Me.tsbReports.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 40)
        '
        'tsbPrevious
        '
        Me.tsbPrevious.AutoSize = False
        Me.tsbPrevious.ForeColor = System.Drawing.Color.White
        Me.tsbPrevious.Image = Global.jade.My.Resources.Resources.arrows_147746_960_720
        Me.tsbPrevious.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPrevious.Name = "tsbPrevious"
        Me.tsbPrevious.Size = New System.Drawing.Size(50, 35)
        Me.tsbPrevious.Text = "Previous"
        Me.tsbPrevious.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'tsbNext
        '
        Me.tsbNext.AutoSize = False
        Me.tsbNext.ForeColor = System.Drawing.Color.White
        Me.tsbNext.Image = Global.jade.My.Resources.Resources.red_arrow_png_15
        Me.tsbNext.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbNext.Name = "tsbNext"
        Me.tsbNext.Size = New System.Drawing.Size(50, 35)
        Me.tsbNext.Text = "Next"
        Me.tsbNext.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 40)
        '
        'tsbClose
        '
        Me.tsbClose.AutoSize = False
        Me.tsbClose.ForeColor = System.Drawing.Color.White
        Me.tsbClose.Image = Global.jade.My.Resources.Resources.close_button_icon_transparent_background_247604
        Me.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbClose.Name = "tsbClose"
        Me.tsbClose.Size = New System.Drawing.Size(50, 35)
        Me.tsbClose.Text = "Close"
        Me.tsbClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'tsbExit
        '
        Me.tsbExit.AutoSize = False
        Me.tsbExit.ForeColor = System.Drawing.Color.White
        Me.tsbExit.Image = Global.jade.My.Resources.Resources.exit_button_icon_18
        Me.tsbExit.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbExit.Name = "tsbExit"
        Me.tsbExit.Size = New System.Drawing.Size(50, 35)
        Me.tsbExit.Text = "Exit"
        Me.tsbExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Location = New System.Drawing.Point(8, 287)
        Me.TabControl1.Margin = New System.Windows.Forms.Padding(4)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1077, 248)
        Me.TabControl1.TabIndex = 1345
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.dgvEntry)
        Me.TabPage1.Location = New System.Drawing.Point(4, 24)
        Me.TabPage1.Margin = New System.Windows.Forms.Padding(4)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(4)
        Me.TabPage1.Size = New System.Drawing.Size(1069, 220)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Entries"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'frmCV
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BackColor = System.Drawing.Color.White
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ClientSize = New System.Drawing.Size(1091, 561)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.txtTotalDebit)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtTotalCredit)
        Me.Controls.Add(Me.gbPayee)
        Me.Controls.Add(Me.Label20)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmCV"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = " Check Voucher"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvEntry, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbPayee.ResumeLayout(False)
        Me.gbPayee.PerformLayout()
        Me.tcPayment.ResumeLayout(False)
        Me.tpCash.ResumeLayout(False)
        Me.tpCash.PerformLayout()
        Me.tpCheck.ResumeLayout(False)
        Me.gbBank.ResumeLayout(False)
        Me.gbBank.PerformLayout()
        Me.tpMultipleCheck.ResumeLayout(False)
        CType(Me.dgvMultipleCheck, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpBankTransfer.ResumeLayout(False)
        Me.tpBankTransfer.PerformLayout()
        Me.tpMC.ResumeLayout(False)
        Me.tpMC.PerformLayout()
        Me.tpCreditCard.ResumeLayout(False)
        Me.tpCreditCard.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtBankRef As System.Windows.Forms.TextBox
    Friend WithEvents txtTransNum As System.Windows.Forms.TextBox
    Friend WithEvents txtBankRefAmount As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtTotalDebit As System.Windows.Forms.TextBox
    Friend WithEvents txtTotalCredit As System.Windows.Forms.TextBox
    Friend WithEvents dtpDocDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtRemarks As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents dtpBankRefDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents cbPaymentType As System.Windows.Forms.ComboBox
    Friend WithEvents dgvEntry As System.Windows.Forms.DataGridView
    Friend WithEvents cbBank As System.Windows.Forms.ComboBox
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents cbDisburseType As System.Windows.Forms.ComboBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents txtORNo As System.Windows.Forms.TextBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents gbPayee As System.Windows.Forms.GroupBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents gbBank As System.Windows.Forms.GroupBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtAmount As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtAPVRef As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtStatus As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents EditEntriesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbSearch As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbEdit As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbCancel As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbCopy As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsbCopyAPV As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbReports As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbPrevious As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbNext As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbClose As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbExit As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnSearchVCE As System.Windows.Forms.Button
    Friend WithEvents txtVCEName As System.Windows.Forms.TextBox
    Friend WithEvents txtVCECode As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents tsbCopyADV As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbPrint As System.Windows.Forms.ToolStripSplitButton
    Friend WithEvents PrintCVToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChequieToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BIR2307ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtRefStatus As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents tsbOption As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents tsmUnreleased As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CancelCheckToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtADVRef As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents btnUOMGroup As System.Windows.Forms.Button
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents FromRFPToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtRFPRef As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents FromLoanToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tcPayment As System.Windows.Forms.TabControl
    Friend WithEvents tpCash As System.Windows.Forms.TabPage
    Friend WithEvents txtCashAmount As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents tpCheck As System.Windows.Forms.TabPage
    Friend WithEvents tpMultipleCheck As System.Windows.Forms.TabPage
    Friend WithEvents dgvMultipleCheck As System.Windows.Forms.DataGridView
    Friend WithEvents tpBankTransfer As System.Windows.Forms.TabPage
    Friend WithEvents tpCreditCard As System.Windows.Forms.TabPage
    Friend WithEvents tpMC As System.Windows.Forms.TabPage
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents txtBankRefName As System.Windows.Forms.TextBox
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents txtLoanRef As System.Windows.Forms.TextBox
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents tsbDelete As System.Windows.Forms.ToolStripButton
    Friend WithEvents FromFundsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dgcBankID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgcBank As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents dgcCheckNo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgcCheckDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgcAmount As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgcCheckVCECode As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgcCheckName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgcStatus As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents FromPCVToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FromCAToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtCARef As System.Windows.Forms.TextBox
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents dgcBranchCode As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents chAccntCode As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents chAccntTitle As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents chDebit As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents chCredit As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents chVCECode As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents chVCEName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents chParticulars As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents chRef As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents chCostID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents chCostCenter As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Column12 As System.Windows.Forms.DataGridViewButtonColumn
End Class
