<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmIA_Ledger
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
        Me.dgvLedger = New System.Windows.Forms.DataGridView()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dgvWHSE = New System.Windows.Forms.DataGridView()
        Me.dgcSLCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgcSLWHSE = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgcSLQTY = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvLedger, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvWHSE, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvLedger
        '
        Me.dgvLedger.AllowUserToAddRows = False
        Me.dgvLedger.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.dgvLedger.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvLedger.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvLedger.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvLedger.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvLedger.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgvLedger.Location = New System.Drawing.Point(2, 25)
        Me.dgvLedger.Name = "dgvLedger"
        Me.dgvLedger.RowHeadersVisible = False
        Me.dgvLedger.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvLedger.Size = New System.Drawing.Size(710, 494)
        Me.dgvLedger.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 5)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(116, 15)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Inventory Movement"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(726, 5)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(87, 15)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Stock Location"
        '
        'dgvWHSE
        '
        Me.dgvWHSE.AllowUserToAddRows = False
        Me.dgvWHSE.AllowUserToDeleteRows = False
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.dgvWHSE.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvWHSE.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvWHSE.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWHSE.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dgcSLCode, Me.dgcSLWHSE, Me.dgcSLQTY})
        Me.dgvWHSE.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgvWHSE.Location = New System.Drawing.Point(718, 25)
        Me.dgvWHSE.Name = "dgvWHSE"
        Me.dgvWHSE.RowHeadersVisible = False
        Me.dgvWHSE.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvWHSE.Size = New System.Drawing.Size(356, 494)
        Me.dgvWHSE.TabIndex = 4
        '
        'dgcSLCode
        '
        Me.dgcSLCode.HeaderText = "Code"
        Me.dgcSLCode.Name = "dgcSLCode"
        Me.dgcSLCode.Width = 80
        '
        'dgcSLWHSE
        '
        Me.dgcSLWHSE.HeaderText = "Store Location"
        Me.dgcSLWHSE.Name = "dgcSLWHSE"
        Me.dgcSLWHSE.Width = 180
        '
        'dgcSLQTY
        '
        Me.dgcSLQTY.HeaderText = "Quantity"
        Me.dgcSLQTY.Name = "dgcSLQTY"
        Me.dgcSLQTY.Width = 120
        '
        'frmIA_Ledger
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1086, 520)
        Me.Controls.Add(Me.dgvWHSE)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.dgvLedger)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmIA_Ledger"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Item Ledger"
        CType(Me.dgvLedger, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvWHSE, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dgvLedger As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dgvWHSE As System.Windows.Forms.DataGridView
    Friend WithEvents dgcSLCode As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgcSLWHSE As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgcSLQTY As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
