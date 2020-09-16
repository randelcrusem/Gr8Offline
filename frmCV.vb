Public Class frmCV
    Dim TransID, RefID, JETransiD As String
    Dim CVNo As String
    Dim disableEvent As Boolean = False
    Dim bankEvent As Boolean = False
    Dim ModuleID As String = "CV"
    Dim ColumnPK As String = "CV_No"
    Dim ColumnID As String = "TransID"
    Dim DBTable As String = "tblCV"
    Dim TransAuto As Boolean
    Dim AccntCode As String
    Dim APV_ID, ADV_ID, RFP_ID, LOAN_ID, PCV_ID, CA_ID As Integer
    Dim bankID As Integer
    Dim tpHidden As New Dictionary(Of String, System.Windows.Forms.TabPage)
    Dim tpOrder As New List(Of String)

    Public Overloads Function ShowDialog(ByVal DocNumber As String) As Boolean
        TransID = DocNumber
        MyBase.ShowDialog()
        Return True
    End Function

    Private Sub Disbursement_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.Text = "(" & database & ") - Check Voucher "
            TransAuto = GetTransSetup(ModuleID)
            LoadBankList()
            LoadMultipleBank()
            LoadDisbursementType()
            If cbPaymentType.Items.Count > 0 Then
                cbPaymentType.SelectedIndex = 0
            End If

            If TransID <> "" Then
                If Not AllowAccess("CV_VIEW") Then
                    msgRestricted()
                    dtpDocDate.Value = Date.Today.Date
                    tsbOption.Enabled = False
                    tsbSearch.Enabled = True
                    tsbNew.Enabled = True
                    tsbEdit.Enabled = False
                    tsbSave.Enabled = False
                    tsbCancel.Enabled = False
                    tsbDelete.Enabled = False
                    tsbClose.Enabled = False
                    tsbPrevious.Enabled = False
                    tsbNext.Enabled = False
                    tsbExit.Enabled = True
                    tsbPrint.Enabled = False
                    tsbCopy.Enabled = False
                    EnableControl(False)
                Else
                    LoadCV(TransID)
                End If
            Else
                dtpDocDate.Value = Date.Today.Date
                tsbOption.Enabled = False
                tsbSearch.Enabled = True
                tsbNew.Enabled = True
                tsbEdit.Enabled = False
                tsbSave.Enabled = False
                tsbCancel.Enabled = False
                tsbDelete.Enabled = False
                tsbClose.Enabled = False
                tsbPrevious.Enabled = False
                tsbNext.Enabled = False
                tsbExit.Enabled = True
                tsbPrint.Enabled = False
                tsbCopy.Enabled = False
                EnableControl(False)
            End If
            
        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        End Try
    End Sub

    Private Sub LoadDisbursementType()
        Dim query As String
        query = " SELECT DISTINCT Expense_Description FROM tblCV_ExpenseType WHERE Status ='Active' "
        SQL.ReadQuery(query)
        cbDisburseType.Items.Clear()
        While SQL.SQLDR.Read
            cbDisburseType.Items.Add(SQL.SQLDR("Expense_Description").ToString)
        End While
    End Sub

    Private Sub LoadMultipleBank()
        Try
            Dim dgvMultiBank As New DataGridViewComboBoxColumn
            dgvMultiBank = dgvMultipleCheck.Columns(dgcBank.Index)
            dgvMultiBank.Items.Clear()
            ' ADD ALL BranchCode
            Dim query As String
            query = " SELECT  CAST(BankID AS nvarchar) + '-' + Bank + ' ' + Branch + " & _
                 "         CASE WHEN AccountNo <> '' THEN ' (' + AccountNo  +  ')' ELSE '' END AS Bank " & _
                 " FROM    tblBank_Master " & _
                 " WHERE   Status ='Active'"
            SQL.ReadQuery(query)
            dgvMultiBank.Items.Clear()
            While SQL.SQLDR.Read
                dgvMultiBank.Items.Add(SQL.SQLDR("Bank").ToString)
            End While
        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        End Try
    End Sub

    Private Sub EnableControl(ByVal Value As Boolean)

        dtpDocDate.Enabled = Value
        txtVCEName.Enabled = Value
        btnSearchVCE.Enabled = Value
        'dgvEntry.Enabled = Value
        dgvEntry.AllowUserToAddRows = Value
        dgvEntry.AllowUserToDeleteRows = Value
        If Value = True Then
            dgvEntry.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2
            LoadBranch()
        Else
            dgvEntry.EditMode = DataGridViewEditMode.EditProgrammatically
        End If
        txtRemarks.Enabled = Value
        txtAmount.Enabled = Value
        txtORNo.Enabled = Value
        cbPaymentType.Enabled = Value
        cbDisburseType.Enabled = Value
        tcPayment.Enabled = Value
        If TransAuto Then
            txtTransNum.Enabled = False
        Else
            txtTransNum.Enabled = Value
        End If
    End Sub

    Private Sub dgvEntry_DataError(sender As System.Object, e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles dgvEntry.DataError
        Try

        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        End Try
    End Sub

    Private Sub LoadCV(ByVal ID As String)
        If dgvEntry.ColumnCount >= 11 AndAlso dgvEntry.Columns(10).HeaderText = "Balance" Then
            dgvEntry.Columns.Remove("Balance")
        End If
        Dim query, payment_type As String
        query = " SELECT  TransID, CV_No, PaymentType, tblCV.VCECode, VCEName, DateCV, TotalAmount, Remarks, " & _
                "         ISNULL(APV_Ref,0) as APV_Ref, OR_Ref, ISNULL(LN_Ref,0) as LN_Ref, ISNULL(CA_Ref,0) as CA_Ref, tblCV.Status " & _
                " FROM    tblCV LEFT JOIN viewVCE_Master " & _
                " ON      tblCV.VCECode = viewVCE_Master.VCECode " & _
                " WHERE   TransID = '" & ID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            dtpDocDate.MinDate = "01-01-1900"
            TransID = SQL.SQLDR("TransID").ToString
            CVNo = SQL.SQLDR("CV_No").ToString
            LOAN_ID = SQL.SQLDR("LN_Ref").ToString
            APV_ID = SQL.SQLDR("APV_Ref").ToString
            CA_ID = SQL.SQLDR("CA_Ref").ToString
            txtTransNum.Text = CVNo
            payment_type = SQL.SQLDR("PaymentType").ToString
            txtVCECode.Text = SQL.SQLDR("VCECode").ToString
            txtVCEName.Text = SQL.SQLDR("VCEName").ToString
            disableEvent = True
            dtpDocDate.Value = SQL.SQLDR("DateCV")
            disableEvent = False
            txtAmount.Text = CDec(SQL.SQLDR("TotalAmount")).ToString("N2")
            txtRemarks.Text = SQL.SQLDR("Remarks").ToString
            txtORNo.Text = SQL.SQLDR("OR_Ref").ToString
            txtStatus.Text = SQL.SQLDR("Status").ToString
            txtLoanRef.Text = GetLoanNo(LOAN_ID)
            txtAPVRef.Text = GetAPVNo(APV_ID)
            txtCARef.Text = GetCANo(CA_ID)
            cbPaymentType.SelectedItem = payment_type
            If payment_type = "Check" Then
                LoadCVRef(TransID)
            ElseIf payment_type = "Multiple Check" Then
                LoadCVRefMulti(TransID)
            ElseIf payment_type = "Cash" Then
                txtCashAmount.Text = txtAmount.Text
            End If
            LoadEntry(TransID)

            ' TOOLSTRIP BUTTONS
            tsbCancel.Text = "Cancel"
            If txtStatus.Text = "Cancelled" Then
                tsbEdit.Enabled = False
                tsbCancel.Enabled = True
                tsbCancel.Text = "Un-Can"
                tsbDelete.Enabled = True
            ElseIf txtStatus.Text = "Closed" Then
                tsbEdit.Enabled = False
                tsbCancel.Enabled = False
                tsbDelete.Enabled = False
            Else
                tsbEdit.Enabled = True
                tsbCancel.Enabled = True
                tsbDelete.Enabled = True
            End If
            tsbPrint.Enabled = True
            tsbClose.Enabled = True
            tsbPrevious.Enabled = True
            tsbNext.Enabled = True
            tsbPrint.Enabled = True
            tsbSave.Enabled = False
            tsbNew.Enabled = True
            tsbSearch.Enabled = True
            tsbExit.Enabled = True
            tsbCopy.Enabled = False
            tsbOption.Enabled = True
            If dtpDocDate.Value < GetMaxPEC() Then
                tsbEdit.Enabled = False
                tsbCancel.Enabled = False
                tsbDelete.Enabled = False
            End If
            EnableControl(False)
        Else
            ClearText()
        End If
    End Sub

    Private Sub LoadCVRef(ByVal CVNo As Integer)
        Dim query As String
        query = " SELECT CAST(tblBank_Master.BankID AS nvarchar) + '-' + Bank + ' ' + Branch + CASE WHEN AccountNo <> '' THEN ' (' + AccountNo  +  ')' ELSE '' END AS Bank, " & _
                " 	     RefNo, RefDate, RefAmount, tblCV_BankRef.Status, RefName " & _
                " FROM   tblCV_BankRef INNER JOIN tblBank_Master " & _
                " ON     tblCV_BankRef.BankID = tblBank_Master.BankID " & _
                " WHERE  CV_No ='" & CVNo & "' AND tblCV_BankRef.Status <> 'Cancelled' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            disableEvent = True
            gbBank.Visible = True
            txtBankRef.Text = SQL.SQLDR("RefNo").ToString
            dtpBankRefDate.Value = SQL.SQLDR("RefDate")
            txtBankRefAmount.Text = CDec(SQL.SQLDR("RefAmount")).ToString("N2")
            cbBank.SelectedItem = SQL.SQLDR("Bank").ToString
            txtRefStatus.Text = SQL.SQLDR("Status").ToString
            txtBankRefName.Text = SQL.SQLDR("RefName").ToString
            If txtRefStatus.Text = "Unreleased" Then
                tsmUnreleased.Enabled = True
            Else
                tsmUnreleased.Enabled = False
            End If
            bankID = GetBankID(cbBank.SelectedItem)
            disableEvent = False
        ElseIf cbPaymentType.SelectedItem = "Check" Then
            gbBank.Visible = True
            txtBankRef.Text = ""
            dtpBankRefDate.Value = dtpDocDate.Value.Date
            txtBankRefAmount.Text = CDec(txtAmount.Text)
            cbBank.SelectedIndex = -1
            tsmUnreleased.Enabled = False
        Else
            gbBank.Visible = False
            tsmUnreleased.Enabled = False
        End If
    End Sub

    Private Sub LoadCVRefMulti(ByVal CVNo As Integer)
        Dim query As String
        query = " SELECT tblBank_Master.BankID, CAST(tblBank_Master.BankID AS nvarchar) + '-' + Bank + ' ' + Branch + CASE WHEN AccountNo <> '' THEN ' (' + AccountNo  +  ')' ELSE '' END AS Bank, " & _
                " 	     RefNo, RefDate, RefAmount, tblCV_BankRef.Status, RefName , ISNULL(RefVCECode,'') as RefVCECode" & _
                " FROM   tblCV_BankRef INNER JOIN tblBank_Master " & _
                " ON     tblCV_BankRef.BankID = tblBank_Master.BankID " & _
                " WHERE  CV_No ='" & CVNo & "' AND tblCV_BankRef.Status <> 'Cancelled' "
        SQL.ReadQuery(query)

        dgvMultipleCheck.Rows.Clear()
        While SQL.SQLDR.Read
            dgvMultipleCheck.Rows.Add(SQL.SQLDR("BankID").ToString, SQL.SQLDR("Bank").ToString, SQL.SQLDR("RefNo").ToString, SQL.SQLDR("RefDate").ToString, CDec(SQL.SQLDR("RefAmount")).ToString("N2"), _
                               SQL.SQLDR("RefVCECode").ToString, SQL.SQLDR("RefName").ToString, SQL.SQLDR("Status").ToString)
        End While
    End Sub

    Private Sub LoadEntry(ByVal CVNo As Integer)
        Dim query As String
        query = " SELECT ID, JE_No, View_GL.BranchCode, View_GL.AccntCode, AccountTitle, View_GL.VCECode, View_GL.VCEName, Debit, Credit, Particulars, RefNo   " & _
                " FROM   View_GL INNER JOIN tblCOA_Master " & _
                " ON     View_GL.AccntCode = tblCOA_Master.AccountCode " & _
                " WHERE JE_No = (SELECT  JE_No FROM tblJE_Header WHERE RefType = 'CV' AND RefTransID = " & CVNo & ") " & _
                " ORDER BY LineNumber "
        SQL.ReadQuery(query)
        dgvEntry.Rows.Clear()
        While SQL.SQLDR.Read
            JETransiD = SQL.SQLDR("JE_No")
            dgvEntry.Rows.Add(SQL.SQLDR("BranchCode").ToString, SQL.SQLDR("AccntCode").ToString, SQL.SQLDR("AccountTitle").ToString, CDec(SQL.SQLDR("Debit")).ToString("N2"), _
                               CDec(SQL.SQLDR("Credit")).ToString("N2"), SQL.SQLDR("VCECode").ToString, SQL.SQLDR("VCEName").ToString, _
                               SQL.SQLDR("Particulars").ToString, SQL.SQLDR("RefNo").ToString)
        End While
        LoadBranch()
        TotalDBCR()
    End Sub

    Private Sub LoadBankList()
        Dim query As String
        query = " SELECT  CAST(BankID AS nvarchar) + '-' + Bank + ' ' + Branch + " & _
                "         CASE WHEN AccountNo <> '' THEN ' (' + AccountNo  +  ')' ELSE '' END AS Bank " & _
                " FROM    tblBank_Master " & _
                " WHERE   Status ='Active'"
        SQL.ReadQuery(query)
        cbBank.Items.Clear()
        While SQL.SQLDR.Read
            cbBank.Items.Add(SQL.SQLDR("Bank").ToString)
        End While
    End Sub

    Private Sub LoadDisburseType()
        Dim query As String
        query = " SELECT  DISTINCT ISNULL(Expense_Description,'') AS Expense_Description " & _
                " FROM    tblCV_ExpenseType " & _
                " WHERE   Status ='Active' "
        SQL.ReadQuery(query)
        cbDisburseType.Items.Clear()
        While SQL.SQLDR.Read
            cbDisburseType.Items.Add(SQL.SQLDR("Expense_Description").ToString)
        End While
    End Sub


    Private Sub cbPaymentType_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbPaymentType.SelectedIndexChanged
        If disableEvent = False Then
            HideTabPageAll()
            ShowTabPage(cbPaymentType.SelectedItem)
            If cbPaymentType.SelectedItem = "Cash" Then
                tcPayment.SelectedTab = tpCash
                bankEvent = False
            ElseIf cbPaymentType.SelectedItem = "Check" Then
                tcPayment.SelectedTab = tpCheck
                bankEvent = True
            ElseIf cbPaymentType.SelectedItem = "Multiple Check" Then
                tcPayment.SelectedTab = tpMultipleCheck
                bankEvent = True
            ElseIf cbPaymentType.SelectedItem = "Manager's Check" Then
                tcPayment.SelectedTab = tpMC
                bankEvent = False
            ElseIf cbPaymentType.SelectedItem = "Credit Card" Then
                tcPayment.SelectedTab = tpCreditCard
                bankEvent = False
            ElseIf cbPaymentType.SelectedItem = "Bank Transfer" Then
                tcPayment.SelectedTab = tpBankTransfer
                bankEvent = False
            ElseIf cbPaymentType.SelectedItem = "(Multiple Payment Method)" Then
                tcPayment.SelectedTab = tpCheck
                bankEvent = False
                ShowTabPageAll()
            End If
            'bankID = 0
            'LoadBankDetails()
            'If cbBank.Items.Count = 1 Then
            '    cbBank.SelectedIndex = 0
            '    bankID = GetBankID(cbBank.SelectedItem)
            '    If cbPaymentType.SelectedItem = "Check" Then
            '        txtBankRef.Text = GetCheckNo(bankID)
            '    End If
            'End If
        End If
    End Sub



    Private Sub LoadBankDetails()
        txtBankRef.Text = ""
        Dim query, prefix As String
        query = " SELECT  Payment_Type, WithBank, WithCheck, RefNo_Prefix, Account_Code " & _
                " FROM    tblCV_PaymentType " & _
                " WHERE   Payment_Type = @Payment_Type "
        SQL.FlushParams()
        SQL.AddParam("Payment_Type", cbPaymentType.SelectedItem)
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            AccntCode = SQL.SQLDR("Account_Code").ToString
            gbBank.Visible = SQL.SQLDR("WithBank")
            If SQL.SQLDR("WithBank") Then
                prefix = SQL.SQLDR("RefNo_Prefix").ToString
                If Not SQL.SQLDR("WithCheck") Then
                    txtBankRef.Text = GetBankRef(prefix)
                End If
            End If
        End If
    End Sub

    Private Sub cbCategory_SelectedIndexChanged(sender As System.Object, e As System.EventArgs)
        Try
            LoadDisburseType()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Function GetBankRef(ByVal Prefix As String) As String
        Dim query As String
        query = " SELECT  (ISNULL(MAX(CAST(REPLACE(RefNo,'" & Prefix & "','') AS int)),0) + 1) AS RefNo " & _
                " FROM    tblCV_Bankref " & _
                " WHERE   RefNo LIKE '" & Prefix & "%' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return Prefix + SQL.SQLDR("RefNo").ToString
        Else
            Return Prefix + "1"
        End If
    End Function

    Private Sub cbBank_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbBank.SelectedIndexChanged
        Try
            If disableEvent = False Then
                If cbBank.SelectedIndex <> -1 Then
                    bankID = GetBankID(cbBank.SelectedItem)
                    If cbPaymentType.SelectedItem = "Check" Then
                        txtBankRef.Text = GetCheckNo(bankID)
                    End If
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Function GetCheckNo(ByVal Bank_ID As String) As String
        Dim query As String
        query = " SELECT  RIGHT('000000000000' + CAST((CAST(MAX(RefNo) AS bigint) + 1) AS nvarchar), LEN(SeriesFrom)) AS RefNo, SeriesFrom " & _
                " FROM    tblCV_BankRef RIGHT JOIN tblBank_Master " & _
                " ON      tblCV_BankRef.BankID = tblBank_Master.BankID " & _
                " AND     tblCV_BankRef.RefNo BETWEEN SeriesFrom AND SeriesTo " & _
                " WHERE   tblBank_Master.BankID = '" & Bank_ID & "'   " & _
                " GROUP BY LEN(SeriesFrom), SeriesFrom  "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read AndAlso Not IsDBNull(SQL.SQLDR("RefNo")) Then
            Return SQL.SQLDR("RefNo").ToString
        Else
            Return SQL.SQLDR("SeriesFrom").ToString
        End If
    End Function

    Private Function GetBankID(ByVal Bank As String) As Integer
        Dim query As String
        query = " SELECT BankID FROM tblBank_Master WHERE BankID = LEFT('" & Bank & "',CHARINDEX('-','" & Bank & "',1)-1) "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return SQL.SQLDR("BankID")
        Else
            Return 0
        End If
    End Function

    Private Function GetLoanNo(ByVal ID As Integer) As String
        Dim query As String
        query = " SELECT Loan_No FROM tblLoan WHERE TransID = '" & ID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return SQL.SQLDR("Loan_No")
        Else
            Return ""
        End If
    End Function

    Private Function GetAPVNo(ByVal ID As Integer) As String
        Dim query As String
        query = " SELECT APV_No FROM tblAPV WHERE TransID = '" & ID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return SQL.SQLDR("APV_No")
        Else
            Return 0
        End If
    End Function


    Private Function GetCANo(ByVal ID As Integer) As String
        Dim query As String
        query = " SELECT CA_No FROM tblCA WHERE TransID = '" & ID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return SQL.SQLDR("CA_No")
        Else
            Return 0
        End If
    End Function


    Public Sub TotalDBCR()
        Try
            'debit compute & print in textbox
            Dim a As Double = 0
            For i As Integer = 0 To dgvEntry.Rows.Count - 1
                If dgvEntry.Item(chAccntCode.Index, i).Value <> "" AndAlso Val(dgvEntry.Item(chDebit.Index, i).Value) <> 0 Then
                    a = a + Double.Parse(dgvEntry.Item(chDebit.Index, i).Value).ToString("N2")
                End If
            Next
            txtTotalDebit.Text = a.ToString("N2")
            'credit compute & print in textbox
            Dim b As Double = 0
            For i As Integer = 0 To dgvEntry.Rows.Count - 1
                If dgvEntry.Item(chAccntCode.Index, i).Value <> "" AndAlso Val(dgvEntry.Item(chCredit.Index, i).Value) <> 0 Then
                    b = b + Double.Parse(dgvEntry.Item(chCredit.Index, i).Value).ToString("N2")
                End If
            Next
            txtTotalCredit.Text = b.ToString("N2")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub dtpCVDate_ValueChanged(sender As System.Object, e As System.EventArgs) Handles dtpDocDate.ValueChanged
        dtpBankRefDate.Value = dtpDocDate.Value
        If disableEvent = False Then
            If TransID = "" Then
                txtTransNum.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
            End If
        End If
    End Sub

    Private Sub txtAmount_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtAmount.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                If txtAmount.Text = "" Then
                    MsgBox("Please enter an amount", MsgBoxStyle.Exclamation)
                Else
                    txtAmount.Text = CDec(txtAmount.Text).ToString("N2")
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub dgvManual_RowsRemoved(sender As System.Object, e As System.Windows.Forms.DataGridViewRowsRemovedEventArgs) Handles dgvEntry.RowsRemoved
        Try
            TotalDBCR()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    Private Sub ClearText()
        If dgvEntry.ColumnCount >= 11 AndAlso dgvEntry.Columns(10).HeaderText = "Balance" Then
            dgvEntry.Columns.Remove("Balance")
        End If
        APV_ID = 0
        ADV_ID = 0
        CA_ID = 0
        RFP_ID = 0
        LOAN_ID = 0
        PCV_ID = 0
        txtVCECode.Text = ""
        txtVCEName.Text = ""
        txtAmount.Text = ""
        txtRemarks.Text = ""
        cbPaymentType.SelectedIndex = 0
        cbBank.SelectedIndex = -1
        txtBankRef.Text = ""
        txtBankRefAmount.Text = ""
        txtBankRefName.Text = ""
        txtTransNum.Text = ""
        txtAPVRef.Text = ""
        txtCARef.Text = ""
        txtLoanRef.Text = ""
        txtORNo.Text = ""
        txtStatus.Text = ""
        dgvEntry.Rows.Clear()
        dgvMultipleCheck.Rows.Clear()
        txtTotalDebit.Text = "0.00"
        txtTotalCredit.Text = "0.00"
        dtpDocDate.MinDate = GetMaxPEC()
    End Sub

    Private Sub SaveCV()
        Try
            Dim insertSQL As String
            activityStatus = True
            insertSQL = " INSERT INTO " & _
                        " tblCV (TransID, CV_No, PaymentType, VCECode, DateCV, TotalAmount, Remarks, APV_Ref, ADV_Ref, PCV_Ref, LN_Ref, OR_Ref, CA_Ref, WhoCreated, TransAuto, BranchCode, BusinessCode ) " & _
                        " VALUES (@TransID, @CV_No, @PaymentType, @VCECode, @DateCV, @TotalAmount, @Remarks, @APV_Ref, @ADV_Ref, @PCV_Ref, @LN_Ref, @OR_Ref, @CA_Ref, @WhoCreated, @TransAuto, @BranchCode, @BusinessCode)"
            SQL.FlushParams()
            SQL.AddParam("@TransID", TransID)
            SQL.AddParam("@CV_No", CVNo)
            SQL.AddParam("@PaymentType", cbPaymentType.SelectedItem)
            SQL.AddParam("@VCECode", txtVCECode.Text)
            SQL.AddParam("@DateCV", dtpDocDate.Value.Date)
            SQL.AddParam("@TotalAmount", CDec(txtAmount.Text))
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@APV_Ref", IIf(txtAPVRef.Text = "", DBNull.Value, APV_ID))
            SQL.AddParam("@ADV_Ref", IIf(txtADVRef.Text = "", DBNull.Value, ADV_ID))
            SQL.AddParam("@CA_Ref", IIf(txtCARef.Text = "", DBNull.Value, CA_ID))
            SQL.AddParam("@PCV_Ref", PCV_ID)
            SQL.AddParam("@LN_Ref", LOAN_ID)
            SQL.AddParam("@OR_Ref", txtORNo.Text)
            SQL.AddParam("@TransAuto", TransAuto)
            SQL.AddParam("@WhoCreated", UserID)
            SQL.AddParam("@BranchCode", BranchCode)
            SQL.AddParam("@BusinessCode", BusinessType)
            SQL.ExecNonQuery(insertSQL)

            If ADV_ID > 0 Then
                Dim updateSQL As String
                updateSQL = " UPDATE tblADV SET Status ='Closed' WHERE TransID = '" & ADV_ID & "'"
                SQL.ExecNonQuery(updateSQL)
            End If

            If CA_ID > 0 Then
                Dim updateSQL As String
                updateSQL = " UPDATE tblCA SET Status ='Closed' WHERE TransID = '" & CA_ID & "'"
                SQL.ExecNonQuery(updateSQL)
            End If

            If LOAN_ID > 0 Then
                Dim updateSQL As String
                updateSQL = " UPDATE tblLoan SET Status ='Released', DateRelease = '" & dtpDocDate.Value.Date & "', RefType = 'CV', RefTransID = '" & TransID & "' WHERE TransID = '" & LOAN_ID & "'"
                SQL.ExecNonQuery(updateSQL)
            End If

            If RFP_ID > 0 Then
                Dim updateSQL As String
                updateSQL = " UPDATE tblRFP SET Status ='Closed' WHERE TransID = '" & RFP_ID & "'"
                SQL.ExecNonQuery(updateSQL)
            End If
            If bankEvent = True Then
                If cbPaymentType.SelectedItem = "Check" Then
                    SaveCVRef(TransID)
                ElseIf cbPaymentType.SelectedItem = "Multiple Check" Then
                    SaveCVMultiRef(TransID)
                End If
            End If

            insertSQL = " INSERT INTO " & _
                        " tblJE_Header (AppDate, BranchCode, BusinessCode, RefType, RefTransID, Book, TotalDBCR, Remarks, WhoCreated) " & _
                        " VALUES(@AppDate, @BranchCode, @BusinessCode, @RefType, @RefTransID, @Book, @TotalDBCR, @Remarks, @WhoCreated)"
            SQL.FlushParams()
            SQL.AddParam("@AppDate", dtpDocDate.Value.Date)
            SQL.AddParam("@RefType", "CV")
            SQL.AddParam("@RefTransID", TransID)
            SQL.AddParam("@Book", "Cash Disbursements")
            SQL.AddParam("@TotalDBCR", CDec(txtTotalCredit.Text))
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@BranchCode", BranchCode)
            SQL.AddParam("@BusinessCode", BusinessType)
            SQL.AddParam("@WhoCreated", UserID)
            SQL.ExecNonQuery(insertSQL)

            JETransiD = LoadJE("CV", TransID)

            Dim strRefNo As String = ""

                Dim line As Integer = 1
                For Each item As DataGridViewRow In dgvEntry.Rows
                    If item.Cells(chAccntCode.Index).Value <> Nothing Then
                        insertSQL = " INSERT INTO " & _
                                    " tblJE_Details(JE_No, AccntCode, VCECode, Debit, Credit, Particulars, RefNo, LineNumber, BranchCode) " & _
                                    " VALUES(@JE_No, @AccntCode, @VCECode, @Debit, @Credit, @Particulars, @RefNo, @LineNumber, @BranchCode)"
                        SQL.FlushParams()
                        SQL.AddParam("@JE_No", JETransiD)
                        SQL.AddParam("@AccntCode", item.Cells(chAccntCode.Index).Value.ToString)
                        If item.Cells(chVCECode.Index).Value <> Nothing AndAlso item.Cells(chVCECode.Index).Value <> "" Then
                            SQL.AddParam("@VCECode", item.Cells(chVCECode.Index).Value.ToString)
                        Else
                            SQL.AddParam("@VCECode", txtVCECode.Text)
                        End If
                        If item.Cells(chDebit.Index).Value <> Nothing AndAlso IsNumeric(item.Cells(chDebit.Index).Value) Then
                            SQL.AddParam("@Debit", CDec(item.Cells(chDebit.Index).Value))
                        Else
                            SQL.AddParam("@Debit", 0)
                        End If
                        If item.Cells(chCredit.Index).Value <> Nothing AndAlso IsNumeric(item.Cells(chCredit.Index).Value) Then
                            SQL.AddParam("@Credit", CDec(item.Cells(chCredit.Index).Value))
                        Else
                            SQL.AddParam("@Credit", 0)
                        End If
                        If item.Cells(chParticulars.Index).Value <> Nothing AndAlso item.Cells(chParticulars.Index).Value <> "" Then
                            SQL.AddParam("@Particulars", item.Cells(chParticulars.Index).Value.ToString)
                        Else
                            SQL.AddParam("@Particulars", "")
                        End If
                        If item.Cells(chRef.Index).Value <> Nothing AndAlso item.Cells(chRef.Index).Value <> "" Then
                        SQL.AddParam("@RefNo", item.Cells(chRef.Index).Value.ToString)
                        If strRefNo.Length = 0 Then
                            strRefNo = item.Cells(chRef.Index).Value.ToString
                        Else
                            strRefNo = strRefNo & "-" & item.Cells(chRef.Index).Value.ToString
                        End If
                        Else
                            SQL.AddParam("@RefNo", "")
                        End If
                        SQL.AddParam("@LineNumber", line)
                        If item.Cells(dgcBranchCode.Index).Value <> Nothing AndAlso item.Cells(dgcBranchCode.Index).Value <> "" Then
                            SQL.AddParam("@BranchCode", item.Cells(dgcBranchCode.Index).Value.ToString)
                        Else
                            SQL.AddParam("@BranchCode", BranchCode)
                        End If
                        SQL.ExecNonQuery(insertSQL)
                        line += 1
                    End If
            Next

            If strRefNo.Contains("LN:") Then
                strRefNo = strRefNo.Replace("LN:", "")
                Dim count As Integer = strRefNo.Split("-").Length - 1
                For i As Integer = 0 To count
                    Dim selectSQL As String = "SELECT * FROM tblLoan WHERE Loan_No = '" & strRefNo.Split("-")(i) & "'"
                    SQL.ReadQuery(selectSQL)
                    If SQL.SQLDR.Read Then
                        Dim updateSQL As String
                        updateSQL = " UPDATE tblLoan SET Status ='Released', DateRelease = '" & dtpDocDate.Value.Date & "', RefType = 'CV', RefTransID = '" & TransID & "' WHERE Loan_No = '" & strRefNo.Split("-")(i) & "'"
                        SQL.ExecNonQuery(updateSQL)
                    End If
                Next
            End If
        Catch ex As Exception
            activityStatus = False
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        Finally
            RecordActivity(UserID, ModuleID, Me.Name.ToString, "INSERT", "CV_No", txtTransNum.Text, BusinessType, BranchCode, "", activityStatus)
            SQL.FlushParams()
        End Try
    End Sub


    Private Sub UpdateCV()
        Try
            Dim insertSQL, updateSQL, deleteSQL As String
            activityStatus = True
            updateSQL = " UPDATE tblCV  " & _
                        " SET    CV_No = @CV_No, PaymentType = @PaymentType, VCECode = @VCECode, DateCV = @DateCV, " & _
                        "        TotalAmount = @TotalAmount, Remarks = @Remarks, APV_Ref = @APV_Ref, ADV_Ref = @ADV_Ref, CA_Ref = @CA_Ref, OR_Ref = @OR_Ref, WhoModified = @WhoModified, DateModified = GETDATE(), " & _
                        "        BranchCode = @BranchCode, BusinessCode = @BusinessCode" & _
                        " WHERE TransID = @TransID "
            SQL.FlushParams()
            SQL.AddParam("@TransID", TransID)
            SQL.AddParam("@CV_No", CVNo)
            SQL.AddParam("@PaymentType", cbPaymentType.SelectedItem)
            SQL.AddParam("@VCECode", txtVCECode.Text)
            SQL.AddParam("@DateCV", dtpDocDate.Value.Date)
            SQL.AddParam("@TotalAmount", CDec(txtAmount.Text))
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@APV_Ref", IIf(txtAPVRef.Text = "", DBNull.Value, APV_ID))
            SQL.AddParam("@ADV_Ref", IIf(txtADVRef.Text = "", DBNull.Value, ADV_ID))
            SQL.AddParam("@CA_Ref", IIf(txtCARef.Text = "", DBNull.Value, CA_ID))
            SQL.AddParam("@OR_Ref", txtORNo.Text)
            SQL.AddParam("@WhoModified", UserID)
            SQL.AddParam("@BranchCode", BranchCode)
            SQL.AddParam("@BusinessCode", BusinessType)
            SQL.ExecNonQuery(updateSQL)

            If LOAN_ID > 0 Then
                updateSQL = " UPDATE tblLoan SET Status ='Released', DateRelease = '" & dtpDocDate.Value.Date & "', RefType = 'CV', RefTransID = '" & TransID & "' WHERE TransID = '" & LOAN_ID & "'"
                SQL.ExecNonQuery(updateSQL)
            End If

            If bankEvent = True Then
                If cbPaymentType.SelectedItem = "Check" Then
                    SaveCVRef(TransID)
                ElseIf cbPaymentType.SelectedItem = "Multiple Check" Then
                    SaveCVMultiRef(TransID)
                End If
            End If
            JETransiD = LoadJE("CV", TransID)
            If JETransiD = 0 Then
                insertSQL = " INSERT INTO " & _
                       " tblJE_Header (AppDate, BranchCode, BusinessCode, RefType, RefTransID, Book, TotalDBCR, Remarks, WhoCreated) " & _
                       " VALUES(@AppDate, @BranchCode, @BusinessCode, @RefType, @RefTransID, @Book, @TotalDBCR, @Remarks, @WhoCreated)"
                SQL.FlushParams()
                SQL.AddParam("@AppDate", dtpDocDate.Value.Date)
                SQL.AddParam("@RefType", "CV")
                SQL.AddParam("@RefTransID", TransID)
                SQL.AddParam("@Book", "Cash Disbursements")
                SQL.AddParam("@TotalDBCR", CDec(txtTotalCredit.Text))
                SQL.AddParam("@Remarks", txtRemarks.Text)
                SQL.AddParam("@BranchCode", BranchCode)
                SQL.AddParam("@BusinessCode", BusinessType)
                SQL.AddParam("@WhoCreated", UserID)
                SQL.ExecNonQuery(insertSQL)

                JETransiD = LoadJE("CV", TransID)
            Else
                updateSQL = " UPDATE tblJE_Header " & _
                           " SET    AppDate = @AppDate, BranchCode = @BranchCode, BusinessCode = @BusinessCode, " & _
                           "        RefType = @RefType, RefTransID = @RefTransID, Book = @Book, TotalDBCR = @TotalDBCR, " & _
                           "        Remarks = @Remarks, WhoModified = @WhoModified, DateModified = GETDATE() " & _
                           " WHERE  JE_No = @JE_No "
                SQL.FlushParams()
                SQL.AddParam("@JE_No", JETransiD)
                SQL.AddParam("@AppDate", dtpDocDate.Value.Date)
                SQL.AddParam("@RefType", "CV")
                SQL.AddParam("@RefTransID", TransID)
                SQL.AddParam("@Book", "Cash Disbursements")
                SQL.AddParam("@TotalDBCR", CDec(txtTotalCredit.Text))
                SQL.AddParam("@Remarks", txtRemarks.Text)
                SQL.AddParam("@BranchCode", BranchCode)
                SQL.AddParam("@BusinessCode", BusinessType)
                SQL.AddParam("@WhoModified", UserID)
                SQL.ExecNonQuery(updateSQL)
            End If


            ' DELETE ACCOUNTING ENTRIES
            deleteSQL = " DELETE FROM tblJE_Details  WHERE  JE_No = @JE_No "
            SQL.FlushParams()
            SQL.AddParam("@JE_No", JETransiD)
            SQL.ExecNonQuery(deleteSQL)

            Dim strRefNo As String = ""

            Dim line As Integer = 1
            For Each item As DataGridViewRow In dgvEntry.Rows
                If item.Cells(chAccntCode.Index).Value <> Nothing Then
                    insertSQL = " INSERT INTO " & _
                                " tblJE_Details(JE_No, AccntCode, VCECode, Debit, Credit, Particulars, RefNo, LineNumber, BranchCode) " & _
                                " VALUES(@JE_No, @AccntCode, @VCECode, @Debit, @Credit, @Particulars, @RefNo, @LineNumber, @BranchCode)"
                    SQL.FlushParams()
                    SQL.AddParam("@JE_No", JETransiD)
                    SQL.AddParam("@AccntCode", item.Cells(chAccntCode.Index).Value.ToString)
                    If item.Cells(chVCECode.Index).Value <> Nothing AndAlso item.Cells(chVCECode.Index).Value <> "" Then
                        SQL.AddParam("@VCECode", item.Cells(chVCECode.Index).Value.ToString)
                    Else
                        SQL.AddParam("@VCECode", txtVCECode.Text)
                    End If
                    If item.Cells(chDebit.Index).Value AndAlso IsNumeric(item.Cells(chDebit.Index).Value) Then
                        SQL.AddParam("@Debit", CDec(item.Cells(chDebit.Index).Value))
                    Else
                        SQL.AddParam("@Debit", 0)
                    End If
                    If item.Cells(chCredit.Index).Value <> Nothing AndAlso IsNumeric(item.Cells(chCredit.Index).Value) Then
                        SQL.AddParam("@Credit", CDec(item.Cells(chCredit.Index).Value))
                    Else
                        SQL.AddParam("@Credit", 0)
                    End If
                    If item.Cells(chParticulars.Index).Value <> Nothing AndAlso item.Cells(chParticulars.Index).Value <> "" Then
                        SQL.AddParam("@Particulars", item.Cells(chParticulars.Index).Value.ToString)
                    Else
                        SQL.AddParam("@Particulars", "")
                    End If
                    If item.Cells(chRef.Index).Value <> Nothing AndAlso item.Cells(chRef.Index).Value <> "" Then
                        SQL.AddParam("@RefNo", item.Cells(chRef.Index).Value.ToString)
                        If strRefNo.Length = 0 Then
                            strRefNo = item.Cells(chRef.Index).Value.ToString
                        Else
                            strRefNo = strRefNo & "-" & item.Cells(chRef.Index).Value.ToString
                        End If
                    Else
                        SQL.AddParam("@RefNo", "")
                    End If
                    SQL.AddParam("@LineNumber", line)
                    If item.Cells(dgcBranchCode.Index).Value <> Nothing AndAlso item.Cells(dgcBranchCode.Index).Value <> "" Then
                        SQL.AddParam("@BranchCode", item.Cells(dgcBranchCode.Index).Value.ToString)
                    Else
                        SQL.AddParam("@BranchCode", BranchCode)
                    End If
                    SQL.ExecNonQuery(insertSQL)
                    line += 1
                End If
            Next

            If strRefNo.Contains("LN:") Then
                strRefNo = strRefNo.Replace("LN:", "")
                Dim count As Integer = strRefNo.Split("-").Length - 1
                For i As Integer = 0 To count
                    Dim selectSQL As String = "SELECT * FROM tblLoan WHERE Loan_No = '" & strRefNo.Split("-")(i) & "'"
                    SQL.ReadQuery(selectSQL)
                    If SQL.SQLDR.Read Then
                        Dim updateSQL1 As String
                        updateSQL1 = " UPDATE tblLoan SET Status ='Released', DateRelease = '" & dtpDocDate.Value.Date & "', RefType = 'CV', RefTransID = '" & TransID & "' WHERE Loan_No = '" & strRefNo.Split("-")(i) & "'"
                        SQL.ExecNonQuery(updateSQL1)
                    End If
                Next
            End If
        Catch ex As Exception
            activityStatus = False
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        Finally
            RecordActivity(UserID, ModuleID, Me.Name.ToString, "UPDATE", "CV_No", txtTransNum.Text, BusinessType, BranchCode, "", activityStatus)
            SQL.FlushParams()
        End Try
    End Sub


    Private Sub SaveCVRef(ByVal CVNo As Integer)
        Dim deleteSQL As String
        deleteSQL = "DELETE FROM tblCV_BankRef WHERE CV_No ='" & CVNo & "'"
        SQL.ExecNonQuery(deleteSQL)
        Dim insertSQL As String
        insertSQL = " INSERT INTO " & _
                    " tblCV_BankRef (CV_No, BankID, RefNo, RefDate, RefAmount, RefName) " & _
                    " VALUES(@CV_No, @BankID, @RefNo, @RefDate, @RefAmount, @RefName)"
        SQL.FlushParams()
        SQL.AddParam("@CV_No", CVNo)
        SQL.AddParam("@BankID", bankID)
        SQL.AddParam("@RefNo", txtBankRef.Text)
        SQL.AddParam("@RefDate", dtpBankRefDate.Value.Date)
        SQL.AddParam("@RefAmount", CDec(txtAmount.Text))
        SQL.AddParam("@RefName", txtBankRefName.Text)
        SQL.ExecNonQuery(insertSQL)
    End Sub

    Private Sub SaveCVMultiRef(ByVal CVNo As Integer)
        Dim deleteSQL As String
        Dim insertSQL As String
        Dim bankIDMulti As Integer
        deleteSQL = "DELETE FROM tblCV_BankRef WHERE CV_No ='" & CVNo & "'"
        SQL.ExecNonQuery(deleteSQL)

        For Each item As DataGridViewRow In dgvMultipleCheck.Rows
            If item.Cells(dgcBankID.Index).Value <> Nothing Then
                bankIDMulti = item.Cells(dgcBankID.Index).Value
                insertSQL = " INSERT INTO " & _
                  " tblCV_BankRef (CV_No, BankID, RefNo, RefDate, RefAmount, RefVCECode, RefName) " & _
                  " VALUES(@CV_No, @BankID, @RefNo, @RefDate, @RefAmount, @RefVCECode, @RefName)"
                SQL.FlushParams()
                SQL.AddParam("@CV_No", CVNo)
                SQL.AddParam("@BankID", bankIDMulti)
                If item.Cells(dgcCheckNo.Index).Value <> Nothing Then
                    SQL.AddParam("@RefNo", item.Cells(dgcCheckNo.Index).Value)
                Else
                    SQL.AddParam("@RefNo", "")
                End If
                SQL.AddParam("@RefDate", item.Cells(dgcCheckDate.Index).Value)
                If IsNumeric(item.Cells(dgcAmount.Index).Value) Then
                    SQL.AddParam("@RefAmount", CDec(item.Cells(dgcAmount.Index).Value))
                Else
                    SQL.AddParam("@RefAmount", 0)
                End If
                If item.Cells(dgcCheckVCECode.Index).Value <> "" Then
                    SQL.AddParam("@RefVCECode", item.Cells(dgcCheckVCECode.Index).Value)
                Else
                    SQL.AddParam("@RefVCECode", "")
                End If
                If item.Cells(dgcCheckName.Index).Value <> "" Then
                    SQL.AddParam("@RefName", item.Cells(dgcCheckName.Index).Value)
                Else
                    SQL.AddParam("@RefName", txtVCEName.Text)
                End If
                SQL.ExecNonQuery(insertSQL)
            End If
        Next
    End Sub

    Private Sub cbDisburseType_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbDisburseType.SelectedIndexChanged
        Try
            If disableEvent = False Then
                If cbDisburseType.SelectedIndex <> -1 Then
                    Dim query As String
                    Dim amount As Decimal
                    query = " SELECT  Account_Code, AccountTitle, Amount  " & _
                            " FROM    tblCV_ExpenseType INNER JOIN tblCOA_Master " & _
                            " ON      tblCV_ExpenseType.Account_Code = tblCOA_Master.AccountCode " & _
                            " WHERE   tblCV_ExpenseType.Status ='Active' AND Expense_Description = @Expense_Description "
                    SQL.FlushParams()
                    SQL.AddParam("@Expense_Description", cbDisburseType.SelectedItem)
                    SQL.ReadQuery(query)
                    If SQL.SQLDR.Read Then
                        If txtAmount.Text = "" Then
                            amount = CDec(SQL.SQLDR("Amount"))
                        Else
                            amount = CDec(txtAmount.Text) - IIf(txtTotalDebit.Text = "", 0, txtTotalDebit.Text)
                        End If
                        dgvEntry.Rows.Add(BranchCode, SQL.SQLDR("Account_Code").ToString, SQL.SQLDR("AccountTitle").ToString, CDec(amount).ToString("N2"), "0.00", "", "", cbDisburseType.SelectedItem)
                        TotalDBCR()
                    End If

                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            SQL.FlushParams()
        End Try
    End Sub

    Private Sub dgvEntry_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvEntry.CellContentClick
        Try
            If e.ColumnIndex = Column12.Index Then
                If txtTotalDebit.Text <> txtTotalCredit.Text Then
                    Dim query As String
                    Dim bankIDMulti As Integer
                    query = " SELECT  WithBank, Account_Code " & _
                            " FROM    tblCV_PaymentType LEFT JOIN tblCOA_Master " & _
                            " ON      tblCV_PaymentType.Account_Code = tblCOA_Master.AccountCode " & _
                            " WHERE   Payment_Type ='" & cbPaymentType.SelectedItem & "' "
                    SQL.ReadQuery(query)
                    If SQL.SQLDR.Read Then
                        If SQL.SQLDR("WithBank") Then
                            If cbPaymentType.SelectedItem = "Check" Then
                                If cbBank.SelectedIndex <> -1 Then
                                    query = " SELECT    tblBank_Master.AccountCode, AccountTitle" & _
                                            " FROM      tblBank_Master INNER JOIN tblCOA_Master " & _
                                            " ON        tblBank_Master.AccountCode = tblCOA_Master.AccountCode " & _
                                            " WHERE     BankID ='" & bankID & "' "
                                    SQL.ReadQuery(query)
                                    If SQL.SQLDR.Read Then
                                        dgvEntry.Rows.Add(BranchCode, SQL.SQLDR("AccountCode").ToString, SQL.SQLDR("AccountTitle").ToString, "0.00", CDec(CDec(txtTotalDebit.Text) - CDec(txtTotalCredit.Text)).ToString("N2"), "", "", "", "")
                                        LoadBranch()
                                    End If
                                    txtAmount.Text = CDec(CDec(txtTotalDebit.Text) - CDec(txtTotalCredit.Text)).ToString("N2")
                                    txtBankRefAmount.Text = txtAmount.Text
                                    TotalDBCR()
                                End If
                            ElseIf cbPaymentType.SelectedItem = "Multiple Check" Then
                                Dim Amount As Decimal = 0
                                For i As Integer = 0 To dgvMultipleCheck.Rows.Count - 1
                                    If dgvMultipleCheck.Item(dgcBankID.Index, i).Value <> "" Then
                                        bankIDMulti = dgvMultipleCheck.Item(dgcBankID.Index, i).Value
                                        query = " SELECT    tblBank_Master.AccountCode, AccountTitle" & _
                                             " FROM      tblBank_Master INNER JOIN tblCOA_Master " & _
                                             " ON        tblBank_Master.AccountCode = tblCOA_Master.AccountCode " & _
                                             " WHERE     BankID ='" & bankIDMulti & "' "
                                        SQL.ReadQuery(query)
                                        If SQL.SQLDR.Read Then
                                            dgvEntry.Rows.Add(BranchCode, SQL.SQLDR("AccountCode").ToString, SQL.SQLDR("AccountTitle").ToString, "0.00", CDec(dgvMultipleCheck.Item(dgcAmount.Index, i).Value).ToString("N2"), dgvMultipleCheck.Item(dgcCheckVCECode.Index, i).Value, dgvMultipleCheck.Item(dgcCheckName.Index, i).Value, "", "")
                                            LoadBranch()
                                            Amount = Amount + dgvMultipleCheck.Item(dgcAmount.Index, i).Value
                                        End If
                                        TotalDBCR()
                                    End If
                                Next
                                txtAmount.Text = CDec(Amount).ToString("N2")
                            End If
                        Else
                            Dim code As String = SQL.SQLDR("Account_Code").ToString
                            dgvEntry.Rows.Add(BranchCode, code, GetAccntTitle(code), "0.00", CDec(CDec(txtTotalDebit.Text) - CDec(txtTotalCredit.Text)).ToString("N2"), "", "", "", "")
                            LoadBranch()
                            txtAmount.Text = CDec(CDec(txtTotalDebit.Text) - CDec(txtTotalCredit.Text)).ToString("N2")
                            txtBankRefAmount.Text = txtAmount.Text
                            TotalDBCR()
                        End If

                        TotalDBCR()
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Function GetCashAccount(ByVal PaymentType As String, BankID As Integer)
        Dim cashAccount As String = ""
        Dim query As String
        query = " SELECT  WithBank, Account_Code " & _
                " FROM    tblCV_PaymentType LEFT JOIN tblCOA_Master " & _
                " ON      tblCV_PaymentType.Account_Code = tblCOA_Master.AccountCode " & _
                " WHERE   Payment_Type ='" & PaymentType & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            If SQL.SQLDR("WithBank") Then
                If cbBank.SelectedIndex <> -1 Then
                    query = " SELECT    tblBank_Master.AccountCode, AccountTitle " & _
                            " FROM      tblBank_Master INNER JOIN tblCOA_Master " & _
                            " ON        tblBank_Master.AccountCode = tblCOA_Master.AccountCode " & _
                            " WHERE     BankID ='" & BankID & "' "
                    SQL.ReadQuery(query)
                    If SQL.SQLDR.Read Then
                        cashAccount = SQL.SQLDR("AccountCode").ToString
                    End If
                End If
            Else
                cashAccount = SQL.SQLDR("Account_Code").ToString
            End If
        End If
        Return cashAccount
    End Function



    Private Sub dgvManual_CellEndEdit(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvEntry.CellEndEdit
        If e.ColumnIndex = chDebit.Index Or e.ColumnIndex = chCredit.Index Then
            If IsNumeric(dgvEntry.Item(e.ColumnIndex, e.RowIndex).Value) Then
                dgvEntry.Item(e.ColumnIndex, e.RowIndex).Value = CDec(dgvEntry.Item(e.ColumnIndex, e.RowIndex).Value).ToString("N2")
            End If
            TotalDBCR()
        ElseIf e.ColumnIndex = chAccntCode.Index Or e.ColumnIndex = chAccntTitle.Index Then
            If dgvEntry.Item(e.ColumnIndex, e.RowIndex).Value <> Nothing Then
                Dim f As New frmCOA_Search
                Dim filter As String
                If e.ColumnIndex = 0 Then
                    filter = "AccntCode"
                Else
                    filter = "AccntTitle"
                End If
                f.ShowDialog(filter, dgvEntry.Item(e.ColumnIndex, e.RowIndex).Value.ToString)
                If f.accountcode <> "" And f.accttile <> "" Then
                    dgvEntry.Item(chAccntCode.Index, e.RowIndex).Value = f.accountcode
                    dgvEntry.Item(chAccntTitle.Index, e.RowIndex).Value = f.accttile
                    dgvEntry.SelectedCells(0).Selected = False
                    dgvEntry.Item(chDebit.Index, e.RowIndex).Selected = True
                End If
                f.Dispose()
                'Dim strVCECode As String = ""
                'Dim strAccntCode As String = ""
                'strVCECode = txtVCECode.Text
                'strAccntCode = dgvEntry.Item(chAccntCode.Index, e.RowIndex).Value
                'If dgvEntry.Item(chVCECode.Index, e.RowIndex).Value <> "" Then
                '    strVCECode = dgvEntry.Item(chVCECode.Index, e.RowIndex).Value
                'End If
                'dgvEntry.Item(chRef.Index, e.RowIndex).Value = GetRefNo(strVCECode, strAccntCode)
                'Dim strRefNo As String = ""
                'Dim strRefNoLoan As String = ""
                'strRefNo = dgvEntry.Item(chRef.Index, e.RowIndex).Value
                'strRefNoLoan = GetRefNoLoan(strRefNo)
                'If strRefNoLoan <> "" Then
                '    dgvEntry.Rows.Add("")
                '    dgvEntry.Item(chVCECode.Index, e.RowIndex + 1).Value = GetRefNoVCECode(strRefNo)
                '    dgvEntry.Item(chVCEName.Index, e.RowIndex + 1).Value = GetVCEName(dgvEntry.Item(chVCECode.Index, e.RowIndex + 1).Value)
                '    dgvEntry.Item(chAccntCode.Index, e.RowIndex + 1).Value = strRefNoLoan
                '    dgvEntry.Item(chAccntTitle.Index, e.RowIndex + 1).Value = GetAccntTitle(dgvEntry.Item(chAccntCode.Index, e.RowIndex + 1).Value)
                '    dgvEntry.Item(chRef.Index, e.RowIndex + 1).Value = strRefNo
                'End If
            End If
        ElseIf e.ColumnIndex = chVCECode.Index Or e.ColumnIndex = chVCEName.Index Then
            Dim f As New frmVCE_Search
            f.txtFilter.Text = dgvEntry.Item(e.ColumnIndex, e.RowIndex).Value.ToString
            f.ShowDialog()
            dgvEntry.Item(chVCECode.Index, e.RowIndex).Value = f.VCECode
            dgvEntry.Item(chVCEName.Index, e.RowIndex).Value = f.VCEName
            f.Dispose()



            ''Auto Entry RefNo
            'Dim strVCECode As String = ""
            'Dim strAccntCode As String = ""
            'strVCECode = txtVCECode.Text
            'strAccntCode = dgvEntry.Item(chAccntCode.Index, e.RowIndex).Value
            'If dgvEntry.Item(chVCECode.Index, e.RowIndex).Value <> "" Then
            '    strVCECode = dgvEntry.Item(chVCECode.Index, e.RowIndex).Value
            'End If
            'If strAccntCode <> "" Then
            '    dgvEntry.Item(chRef.Index, e.RowIndex).Value = GetRefNo(strVCECode, strAccntCode)
            '    Dim strRefNo As String = ""
            '    Dim strRefNoLoan As String = ""
            '    strRefNo = dgvEntry.Item(chRef.Index, e.RowIndex).Value
            '    strRefNoLoan = GetRefNoLoan(strRefNo)
            '    If strRefNoLoan <> "" Then
            '        dgvEntry.Rows.Add("")
            '        dgvEntry.Item(chVCECode.Index, e.RowIndex + 1).Value = GetRefNoVCECode(strRefNo)
            '        dgvEntry.Item(chVCEName.Index, e.RowIndex + 1).Value = GetVCEName(dgvEntry.Item(chVCECode.Index, e.RowIndex + 1).Value)
            '        dgvEntry.Item(chAccntCode.Index, e.RowIndex + 1).Value = strRefNoLoan
            '        dgvEntry.Item(chAccntTitle.Index, e.RowIndex + 1).Value = GetAccntTitle(dgvEntry.Item(chAccntCode.Index, e.RowIndex + 1).Value)
            '        dgvEntry.Item(chRef.Index, e.RowIndex + 1).Value = strRefNo
            '    End If
            'End If
            'ElseIf e.ColumnIndex = chRef.Index Then
            'Dim strRefNo As String = ""
            'strRefNo = dgvEntry.Item(chRef.Index, e.RowIndex).Value
            'If strRefNo <> "" Then
            '    dgvEntry.Item(chVCECode.Index, e.RowIndex).Value = GetRefNoVCECode(strRefNo)
            '    dgvEntry.Item(chVCEName.Index, e.RowIndex).Value = GetVCEName(dgvEntry.Item(chVCECode.Index, e.RowIndex).Value)
            '    dgvEntry.Item(chAccntCode.Index, e.RowIndex).Value = GetRefNoAccntCode(strRefNo)
            '    dgvEntry.Item(chAccntTitle.Index, e.RowIndex).Value = GetAccntTitle(dgvEntry.Item(chAccntCode.Index, e.RowIndex).Value)
            '    Dim strRefNoLoan As String = ""
            '    strRefNo = dgvEntry.Item(chRef.Index, e.RowIndex).Value
            '    strRefNoLoan = GetRefNoLoan(strRefNo)
            '    If strRefNoLoan <> "" Then
            '        dgvEntry.Rows.Add("")
            '        dgvEntry.Item(chVCECode.Index, e.RowIndex + 1).Value = GetRefNoVCECode(strRefNo)
            '        dgvEntry.Item(chVCEName.Index, e.RowIndex + 1).Value = GetVCEName(dgvEntry.Item(chVCECode.Index, e.RowIndex + 1).Value)
            '        dgvEntry.Item(chAccntCode.Index, e.RowIndex + 1).Value = strRefNoLoan
            '        dgvEntry.Item(chAccntTitle.Index, e.RowIndex + 1).Value = GetAccntTitle(dgvEntry.Item(chAccntCode.Index, e.RowIndex + 1).Value)
            '        dgvEntry.Item(chRef.Index, e.RowIndex + 1).Value = strRefNo
            '    End If
            'End If
        End If
    End Sub


    Private Sub LoadBranch()
        Try
            Dim dgvCB As New DataGridViewComboBoxColumn
            dgvCB = dgvEntry.Columns(dgcBranchCode.Index)
            dgvCB.Items.Clear()
            ' ADD ALL BranchCode
            Dim query As String
            query = " SELECT BranchCode FROM tblBranch "
            SQL.ReadQuery(query)
            dgvCB.Items.Clear()
            While SQL.SQLDR.Read
                dgvCB.Items.Add(SQL.SQLDR("BranchCode").ToString)
            End While
            dgvCB.Items.Add("")
        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        End Try
    End Sub


    Private Sub btnPrintCheck_Click(sender As System.Object, e As System.EventArgs)
        If txtBankRef.Text <> "" Then
            frmCheckPrinting.CVTransID = txtTransNum.Text
            frmCheckPrinting.ShowDialog()
            frmCheckPrinting.Dispose()
        Else
            MsgBox("No Check No. to print!", MsgBoxStyle.Exclamation)
        End If
    End Sub

    Private Sub LoadRFP(ByVal RFPID As String)
        Try
            Dim query As String
            query = " SELECT    TransID, RFP_No, BranchCode, tblRFP.VCECode, VCEName " & _
                    " FROM      tblRFP LEFT JOIN tblVCE_Master " & _
                    " ON		tblRFP.VCECode = tblVCE_Master.VCECode " & _
                    " WHERE     TransID ='" & RFPID & "' "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                txtVCECode.Text = SQL.SQLDR("VCECode").ToString
                txtVCEName.Text = SQL.SQLDR("VCEName").ToString
                RFP_ID = SQL.SQLDR("TransID")
                txtRFPRef.Text = SQL.SQLDR("RFP_No")
                query = " SELECT		tblRFP.TransID, tblRFP.BranchCode, AccountCode, AccountTitle, CodePayee, RecordPayee, tblRFP_Details.Type, tblRFP_Details.BaseAmount, tblRFP_Details.InputVAT " & _
                        " FROM		tblRFP INNER JOIN tblRFP_Details " & _
                        " ON			tblRFP.TransID = tblRFP_Details.TransID " & _
                        " LEFT JOIN	tblRFP_Type " & _
                        " ON		 	tblRFP_Details.Type = tblRFP_Type.Type " & _
                        " LEFT JOIN	tblCOA_Master " & _
                        " ON			tblCOA_Master.AccountCode = tblRFP_Type.DefaultAccount " & _
                        " WHERE tblRFP.TransID = '" & RFPID & "' "
                SQL.ReadQuery(query)
                While SQL.SQLDR.Read
                    dgvEntry.Rows.Add(SQL.SQLDR("BranchCode").ToString, SQL.SQLDR("AccountCode").ToString, SQL.SQLDR("AccountTitle").ToString, _
                                      CDec(SQL.SQLDR("BaseAmount")).ToString("N2"), "0.00", SQL.SQLDR("CodePayee").ToString, SQL.SQLDR("RecordPayee").ToString, SQL.SQLDR("Type").ToString, "RFP:" & txtRFPRef.Text)
                    If SQL.SQLDR("InputVAT") <> 0 Then
                        dgvEntry.Rows.Add(SQL.SQLDR("BranchCode").ToString, "18060", " Vat Input", _
                                            CDec(SQL.SQLDR("InputVAT")).ToString("N2"), "0.00", SQL.SQLDR("CodePayee").ToString, SQL.SQLDR("RecordPayee").ToString, SQL.SQLDR("Type").ToString, "RFP:" & txtRFPRef.Text)
                    End If

                End While
                LoadBranch()
            End If
            TotalDBCR()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub LoadAPV(ByVal APV As String)
        Try
            Dim query As String
            query = " SELECT Ref_TransID AS TransID, APV_No, VCECode, Supplier AS  VCEName, Date AS Date_APV, Amount AS Net_Purchase, Amount/1.12 * 0.12 AS Input_VAT, Amount/1.12 * 0.02 AS EWT, Remarks, Reference AS Reference_Other, AccntCode, AccountTitle " & _
                    " FROM View_APV_Balance " & _
                    " WHERE  Ref_TransID ='" & APV & "' "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                txtVCECode.Text = SQL.SQLDR("VCECode").ToString
                txtVCEName.Text = SQL.SQLDR("VCEName").ToString
                APV_ID = SQL.SQLDR("TransID")
                txtAPVRef.Text = SQL.SQLDR("APV_No")
                dgvEntry.Rows.Add(BranchCode, SQL.SQLDR("AccntCode").ToString, SQL.SQLDR("AccountTitle").ToString, CDec(SQL.SQLDR("Net_Purchase")).ToString("N2"), "0.00", "", "", "", "APV:" & txtAPVRef.Text)
                LoadBranch()
            End If
            TotalDBCR()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub LoadADV(ByVal ADV As String)
        Try
            Dim query As String
            query = " SELECT TransID, ADV_No,  tblADV.VCECode, VCEName, DateADV AS DateADV, ADV_Amount AS Net_Purchase, Remarks,  AccntCode, AccountTitle " & _
                    " FROM   tblADV INNER JOIN tblVCE_Master " & _
                    " ON     tblADV.VCECode = tblVCE_Master.VCECode " & _
                    " INNER JOIN tblCOA_Master " & _
                    " ON     tblADV.AccntCode = tblCOA_Master.AccountCode " & _
                    " WHERE  TransID ='" & ADV & "' "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                txtVCECode.Text = SQL.SQLDR("VCECode").ToString
                txtVCEName.Text = SQL.SQLDR("VCEName").ToString
                txtADVRef.Text = SQL.SQLDR("ADV_No")
                ADV_ID = SQL.SQLDR("TransID")
                dgvEntry.Rows.Add(SQL.SQLDR("AccntCode").ToString, SQL.SQLDR("AccountTitle").ToString, CDec(SQL.SQLDR("Net_Purchase")).ToString("N2"), "0.00", "", "", "", "ADV:" & txtAPVRef.Text)
            End If
            TotalDBCR()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub LoadCA(ByVal CA As String)
        Try
            Dim query As String
            query = " SELECT TransID, CA_No,  tblCA.VCECode, VCEName, DateCA AS DateCA, Amount AS Net_Purchase, Remarks,  AccntCode, AccountTitle " & _
                    " FROM   tblCA INNER JOIN viewVCE_Master " & _
                    " ON     tblCA.VCECode = viewVCE_Master.VCECode " & _
                    " INNER JOIN tblCOA_Master " & _
                    " ON     tblCA.AccntCode = tblCOA_Master.AccountCode " & _
                    " WHERE  TransID ='" & CA & "' "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                CA_ID = SQL.SQLDR("TransID")
                txtCARef.Text = SQL.SQLDR("CA_No")
                txtVCECode.Text = SQL.SQLDR("VCECode").ToString
                txtVCEName.Text = SQL.SQLDR("VCEName").ToString
                dgvEntry.Rows.Add(BranchCode, SQL.SQLDR("AccntCode").ToString, SQL.SQLDR("AccountTitle").ToString, CDec(SQL.SQLDR("Net_Purchase")).ToString("N2"), "0.00", SQL.SQLDR("VCECode").ToString, SQL.SQLDR("VCEName").ToString, "", "CA:" & SQL.SQLDR("CA_No").ToString)
            End If
           

            LoadBranch()
            TotalDBCR()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub LoadLoan(ByVal Loan As String)
        Try
            Dim LoanAccount, IntIncomeAccount, UnearnedAccount, IntRecAccount, Loan_No, VCECode, IntAmortMethod, cashAccount As String
            Dim SetupUnearned As Boolean
            Dim LoanAmount, IntAmount, cashAmount As Decimal
            Dim query As String
            query = " SELECT	tblLoan.LoanCode, SetupUnearned, LoanAccount, IntIncomeAccount, UnearnedAccount, IntRecAccount, " & _
                    " 		    LoanAmount, IntAmount, tblLoan.IntAmortMethod, Loan_No, VCECode " & _
                    " FROM	    tblLoan_Type INNER JOIN tblLoan " & _
                    " ON		tblLoan_Type.LoanCode = tblLoan.LoanCode " & _
                    " WHERE     TransID = '" & Loan & "' "
            SQL.GetQuery(query)
            If SQL.SQLDS.Tables(0).Rows.Count > 0 Then
                With SQL.SQLDS.Tables(0).Rows(0)
                    LOAN_ID = Loan
                    SetupUnearned = .Item(1)
                    LoanAccount = .Item(2)
                    IntIncomeAccount = .Item(3)
                    UnearnedAccount = .Item(4)
                    IntRecAccount = .Item(5)
                    LoanAmount = .Item(6)
                    IntAmount = .Item(7)
                    IntAmortMethod = .Item(8)
                    Loan_No = .Item(9)
                    VCECode = .Item(10)
                    txtLoanRef.Text = Loan_No
                    If cbPaymentType.SelectedItem = "Check" Then
                        cashAccount = GetCashAccount(cbPaymentType.SelectedItem, bankID)
                    End If
                    If IntAmortMethod = "Less to Proceeds" Then    ' IF INTEREST IS LESS TO PROCEED 
                        ' LOAN RECEIVABLE ENTRY
                        dgvEntry.Rows.Add(BranchCode, LoanAccount, GetAccntTitle(LoanAccount), CDec(LoanAmount).ToString("N2"), "0.00", VCECode, GetVCEName(VCECode), "", "LN:" & Loan_No)
                        ' INTEREST INCOME ENTRY
                        dgvEntry.Rows.Add(BranchCode, IntIncomeAccount, GetAccntTitle(IntIncomeAccount), "0.00", CDec(IntAmount).ToString("N2"), VCECode, GetVCEName(VCECode), "", Loan_No)
                        cashAmount = LoanAmount - IntAmount
                    ElseIf IntAmortMethod = "Add On" Then    ' IF INTEREST IS Add On
                        ' LOAN RECEIVABLE ENTRY
                        dgvEntry.Rows.Add(BranchCode, LoanAccount, GetAccntTitle(LoanAccount), CDec(LoanAmount).ToString("N2"), "0.00", "", VCECode, GetVCEName(VCECode), "LN:" & Loan_No)
                        ' INTEREST INCOME ENTRY
                        dgvEntry.Rows.Add(BranchCode, IntIncomeAccount, GetAccntTitle(IntIncomeAccount), "0.00", CDec(IntAmount).ToString("N2"), "", VCECode, GetVCEName(VCECode), Loan_No)
                        cashAmount = LoanAmount - IntAmount
                    ElseIf IntAmortMethod = "Amortize" Then
                        If SetupUnearned = False Then  ' IF WITHOUT SETUP OF UNEARNED INCOME
                            ' LOAN RECEIVABLE ENTRY
                            dgvEntry.Rows.Add(BranchCode, LoanAccount, GetAccntTitle(LoanAccount), CDec(LoanAmount).ToString("N2"), "0.00", VCECode, GetVCEName(VCECode), "", "LN:" & Loan_No)
                        ElseIf SetupUnearned = True AndAlso LoanAccount = IntIncomeAccount Then  ' IF WITH SETUP OF UNEARNED INCOME AND INT. REC IS SAME AS LOAN REC. ACCOUNT
                            ' LOAN RECEIVABLE ENTRY
                            dgvEntry.Rows.Add(BranchCode, LoanAccount, GetAccntTitle(LoanAccount), CDec(LoanAmount + IntAmount).ToString("N2"), "0.00", VCECode, GetVCEName(VCECode), "", "LN:" & Loan_No)
                            ' UNEARNED INCOME ENTRY
                            dgvEntry.Rows.Add(BranchCode, UnearnedAccount, GetAccntTitle(UnearnedAccount), "0.00", CDec(IntAmount).ToString("N2"), VCECode, GetVCEName(VCECode), "", Loan_No)
                        ElseIf SetupUnearned = True AndAlso LoanAccount <> IntIncomeAccount Then  ' IF WITH SETUP OF UNEARNED INCOME AND INT. REC IS SAME AS LOAN REC. ACCOUNT
                            ' LOAN RECEIVABLE ENTRY
                            dgvEntry.Rows.Add(BranchCode, LoanAccount, GetAccntTitle(LoanAccount), CDec(LoanAmount).ToString("N2"), "0.00", VCECode, GetVCEName(VCECode), "", "LN:" & Loan_No)
                            ' INTEREST RECEIVABLE ENTRY
                            dgvEntry.Rows.Add(BranchCode, IntIncomeAccount, GetAccntTitle(IntIncomeAccount), CDec(IntAmount).ToString("N2"), "0.00", VCECode, GetVCEName(VCECode), "", Loan_No)
                            ' UNEARNED INCOME ENTRY
                            dgvEntry.Rows.Add(BranchCode, UnearnedAccount, GetAccntTitle(UnearnedAccount), "0.00", CDec(IntAmount).ToString("N2"), VCECode, GetVCEName(VCECode), "", Loan_No)
                        End If
                        cashAmount = LoanAmount
                    End If
                End With


                query = "    SELECT TransID, AccountCode, Amount, Description, VCECode, " & _
                        "       CASE WHEN SetupCharges = 1  " & _
                        " 	    THEN '' ELSE  RefID END AS RefID    " & _
                        "    FROM tblLoan_Details " & _
                        "    WHERE	    tblLoan_Details.TransID = '" & Loan & "' AND tblLoan_Details.AmortMethod = 'Less to Proceeds'  "

                'query = " SELECT	TransID,  " & _
                '        " 		    CASE WHEN tblLoan_Charges.RecordID IS NULL THEN tblLoan_Details.AccountCode ELSE tblLoan_Charges.DefaultAccount END AS AccountCode,  " & _
                '        " 		    Amount, Description, VCECode, CASE WHEN SetupCharges = 1 THEN '' ELSE  'LN:' + RefID END AS RefID  " & _
                '        " FROM	    tblLoan_Details LEFT JOIN tblLoan_Charges " & _
                '        " on		tblLoan_Details.RefID = tblLoan_Charges.ChargeID	 " & _
                '        " WHERE	    TransID = '" & Loan & "' AND tblLoan_Details.AmortMethod = 'Less to Proceeds' "
                SQL.GetQuery(query)
                If SQL.SQLDS.Tables(0).Rows.Count > 0 Then
                    For Each row As DataRow In SQL.SQLDS.Tables(0).Rows
                        If row(2) > 0 Then
                            dgvEntry.Rows.Add(BranchCode, row(1).ToString, GetAccntTitle(row(1).ToString), "0.00", CDec(row(2)).ToString("N2"), row(4).ToString, GetVCEName(row(4).ToString), row(3).ToString, "")
                            cashAmount -= row(2)
                        End If
                    Next
                End If
                If cashAccount <> "" Then
                    ' CASH ENTRY
                    dgvEntry.Rows.Add(BranchCode, cashAccount, GetAccntTitle(cashAccount), "0.00", CDec(cashAmount).ToString("N2"), "", "", "", "")
                    txtBankRefAmount.Text = CDec(cashAmount).ToString("N2")
                End If
                LoadBranch()
            End If
            TotalDBCR()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


        'Try
        '    Dim LoanAccount, IntIncomeAccount, UnearnedAccount, IntRecAccount, Loan_No, VCECode, cashAccount As String
        '    Dim SetupUnearned, AmortizeInterest As Boolean
        '    Dim LoanAmount, IntAmount, cashAmount As Decimal
        '    Dim query As String
        '    query = " SELECT	tblLoan.LoanCode, SetupUnearned, LoanAccount, IntIncomeAccount, UnearnedAccount, IntRecAccount, " & _
        '            " 		    LoanAmount, IntAmount, 0 AS AmortizeInterest, Loan_No, VCECode " & _
        '            " FROM	    tblLoan_Type INNER JOIN tblLoan " & _
        '            " ON		tblLoan_Type.LoanCode = tblLoan.LoanCode " & _
        '            " WHERE     TransID = '" & Loan & "' "

        '    SQL.GetQuery(query)
        '    If SQL.SQLDS.Tables(0).Rows.Count > 0 Then
        '        With SQL.SQLDS.Tables(0).Rows(0)
        '            SetupUnearned = .Item(1)
        '            LoanAccount = .Item(2)
        '            IntIncomeAccount = .Item(3)
        '            UnearnedAccount = .Item(4)
        '            IntRecAccount = .Item(5)
        '            LoanAmount = .Item(6)
        '            IntAmount = .Item(7)
        '            AmortizeInterest = .Item(8)
        '            Loan_No = .Item(9)
        '            VCECode = .Item(10)
        '            cashAccount = GetCashAccount(cbPaymentType.SelectedItem, bankID)
        '            If AmortizeInterest = False Then    ' IF INTEREST IS LESS TO PROCEED 
        '                ' LOAN RECEIVABLE ENTRY
        '                dgvEntry.Rows.Add(BranchCode, LoanAccount, GetAccntTitle(LoanAccount), CDec(LoanAmount).ToString("N2"), "0.00", VCECode, GetVCEName(VCECode), "", "LN:" & Loan_No)
        '                ' INTEREST INCOME ENTRY
        '                dgvEntry.Rows.Add(BranchCode, IntIncomeAccount, GetAccntTitle(IntIncomeAccount), "0.00", CDec(IntAmount).ToString("N2"), VCECode, GetVCEName(VCECode), "", "LN:" & Loan_No)
        '                cashAmount = LoanAmount - IntAmount
        '            Else
        '                If SetupUnearned = False Then  ' IF WITHOUT SETUP OF UNEARNED INCOME
        '                    ' LOAN RECEIVABLE ENTRY
        '                    dgvEntry.Rows.Add(BranchCode, LoanAccount, GetAccntTitle(LoanAccount), CDec(LoanAmount).ToString("N2"), "0.00", VCECode, GetVCEName(VCECode), "", "LN:" & Loan_No)
        '                ElseIf SetupUnearned = True AndAlso LoanAccount = IntIncomeAccount Then  ' IF WITH SETUP OF UNEARNED INCOME AND INT. REC IS SAME AS LOAN REC. ACCOUNT
        '                    ' LOAN RECEIVABLE ENTRY
        '                    dgvEntry.Rows.Add(BranchCode, LoanAccount, GetAccntTitle(LoanAccount), CDec(LoanAmount + IntAmount).ToString("N2"), "0.00", VCECode, GetVCEName(VCECode), "", "LN:" & Loan_No)
        '                    ' UNEARNED INCOME ENTRY
        '                    dgvEntry.Rows.Add(BranchCode, UnearnedAccount, GetAccntTitle(UnearnedAccount), "0.00", CDec(IntAmount).ToString("N2"), VCECode, GetVCEName(VCECode), "", "LN:" & Loan_No)
        '                ElseIf SetupUnearned = True AndAlso LoanAccount <> IntIncomeAccount Then  ' IF WITH SETUP OF UNEARNED INCOME AND INT. REC IS SAME AS LOAN REC. ACCOUNT
        '                    ' LOAN RECEIVABLE ENTRY
        '                    dgvEntry.Rows.Add(BranchCode, LoanAccount, GetAccntTitle(LoanAccount), CDec(LoanAmount).ToString("N2"), "0.00", VCECode, GetVCEName(VCECode), "", "LN:" & Loan_No)
        '                    ' INTEREST RECEIVABLE ENTRY
        '                    dgvEntry.Rows.Add(BranchCode, IntIncomeAccount, GetAccntTitle(IntIncomeAccount), CDec(IntAmount).ToString("N2"), "0.00", VCECode, GetVCEName(VCECode), "", "LN:" & Loan_No)
        '                    ' UNEARNED INCOME ENTRY
        '                    dgvEntry.Rows.Add(BranchCode, UnearnedAccount, GetAccntTitle(UnearnedAccount), "0.00", CDec(IntAmount).ToString("N2"), VCECode, GetVCEName(VCECode), "", "LN:" & Loan_No)
        '                End If
        '                cashAmount = LoanAmount
        '            End If

        '        End With

        '        query = " SELECT TransID, AccountCode, Amount, Description, VCECode, RefID FROM tblLoan_Details  WHERE TransID = '" & Loan & "' "
        '        SQL.GetQuery(query)
        '        If SQL.SQLDS.Tables(0).Rows.Count > 0 Then
        '            For Each row As DataRow In SQL.SQLDS.Tables(0).Rows
        '                If row(2) > 0 Then
        '                    dgvEntry.Rows.Add(BranchCode, row(1).ToString, GetAccntTitle(row(1).ToString), "0.00", CDec(row(2)).ToString("N2"), row(4).ToString, GetVCEName(row(4).ToString), row(3).ToString, "LN:" & Loan_No)
        '                    cashAmount -= row(2)
        '                End If
        '            Next
        '        End If
        '        If cashAccount <> "" Then
        '            ' CASH ENTRY
        '            dgvEntry.Rows.Add(BranchCode, cashAccount, GetAccntTitle(cashAccount), "0.00", CDec(cashAmount).ToString("N2"), "", "", "", "")
        '        End If
        '        LoadBranch()
        '    End If


        '    TotalDBCR()
        'Catch ex As Exception
        '    MsgBox(ex.Message)
        'End Try
    End Sub

    Private Sub tsbSearch_Click(sender As System.Object, e As System.EventArgs) Handles tsbSearch.Click
        If Not AllowAccess("CV_VIEW") Then
            msgRestricted()
        Else
            Dim f As New frmLoadTransactions
            f.ShowDialog("CV")
            TransID = f.transID
            LoadCV(TransID)
            f.Dispose()
        End If
    End Sub

    Private Sub tsbNew_Click(sender As System.Object, e As System.EventArgs) Handles tsbNew.Click
        If Not AllowAccess("CV_ADD") Then
            msgRestricted()
        Else
            ClearText()
            TransID = ""
            CVNo = ""

            ' Toolstrip Buttons
            tsbSearch.Enabled = False
            tsbNew.Enabled = False
            tsbEdit.Enabled = False
            tsbSave.Enabled = True
            tsbCancel.Enabled = False
            tsbDelete.Enabled = False
            tsbClose.Enabled = True
            tsbPrevious.Enabled = False
            tsbNext.Enabled = False
            tsbExit.Enabled = False
            tsbPrint.Enabled = False
            tsbCopy.Enabled = True
            tsbOption.Enabled = False
            txtStatus.Text = "Open"
            EnableControl(True)

            txtTransNum.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
            txtVCEName.Select()
        End If
    End Sub

    Private Function GenerateTransNum(ByRef Auto As Boolean, ModuleID As String, ColumnPK As String, Table As String) As String
        Dim TransNum As String = ""
        If Auto = True Then
            ' GENERATE TRANS ID 
            Dim query As String
            Do
                query = " SELECT	GlobalSeries, ISNULL(BranchCode,'All') AS BranchCode, ISNULL(BusinessCode,'All') AS BusinessCode,  " & _
                                    " 		    ISNULL(TransGroup,0) AS TransGroup, ISNULL(Prefix,'') AS Prefix, ISNULL(Digits,6) AS Digits, " & _
                                    "           ISNULL(StartRecord,1) AS StartRecord, LEN(ISNULL(Prefix,'')) + ISNULL(Digits,6) AS ID_Length " & _
                                    " FROM	    tblTransactionSetup LEFT JOIN tblTransactionDetails " & _
                                    " ON		tblTransactionSetup.TransType = tblTransactionDetails.TransType " & _
                                    " WHERE	    tblTransactionSetup.TransType ='" & ModuleID & "' "
                SQL.ReadQuery(query)
                If SQL.SQLDR.Read Then
                    If SQL.SQLDR("GlobalSeries") = True Then
                        If SQL.SQLDR("BranchCode") = "All" AndAlso SQL.SQLDR("BusinessCode") = "All" Then
                            Dim digits As Integer = SQL.SQLDR("Digits")
                            Dim prefix As String = Month(dtpDocDate.Value).ToString("MM") & Year(dtpDocDate.Value).ToString("YYYY")
                            Dim startrecord As Integer = SQL.SQLDR("StartRecord")
                            query = " SELECT    ISNULL(MAX(SUBSTRING(" & ColumnPK & "," & prefix.Length + 1 & "," & digits & "))+ 1,1) AS TransID  " & _
                                    " FROM      " & Table & "  " & _
                                    " WHERE     " & ColumnPK & " LIKE '" & prefix & "%' AND LEN(" & ColumnPK & ") = '" & digits & "'  AND TransAuto = 1 "
                            SQL.ReadQuery(query)
                            If SQL.SQLDR.Read Then
                                TransNum = SQL.SQLDR("TransID")
                                For i As Integer = 1 To digits
                                    TransNum = "0" & TransNum
                                Next
                                TransNum = prefix & Strings.Right(TransNum, digits)

                                ' CHECK IF GENERATED TRANSNUM ALREADY EXIST
                                query = " SELECT    " & ColumnPK & " AS TransID  " & _
                                        " FROM      " & Table & "  " & _
                                        " WHERE     " & ColumnPK & " = '" & TransNum & "' "
                                SQL.ReadQuery(query)
                                If SQL.SQLDR.Read Then
                                    Dim updateSQL As String
                                    updateSQL = " UPDATE  " & Table & "  SET TransAuto = 1 WHERE " & ColumnPK & " = '" & TransNum & "' "
                                    SQL.ExecNonQuery(updateSQL)
                                    TransNum = -1
                                End If
                            End If

                        End If
                    Else

                        Dim digits As Integer = SQL.SQLDR("Digits")
                        ' Dim prefix As String = Year(Date.Today) & DateTime.Now.ToString("MM")
                        Dim prefix As String = dtpDocDate.Value.ToString("MM") & Year(dtpDocDate.Value)
                        Dim startrecord As Integer = SQL.SQLDR("StartRecord")
                        query = " SELECT    ISNULL(MAX(SUBSTRING(" & ColumnPK & "," & prefix.Length + 1 & "," & digits & "))+ 1,1) AS TransID  " & _
                                " FROM      " & Table & "  " & _
                                " WHERE     " & ColumnPK & " LIKE '" & prefix & "%'   AND TransAuto = 1 AND BranchCode = '" & BranchCode & "'"
                        SQL.ReadQuery(query)
                        If SQL.SQLDR.Read Then
                            TransNum = SQL.SQLDR("TransID")
                            For i As Integer = 1 To digits
                                TransNum = "0" & TransNum
                            Next
                            TransNum = prefix & Strings.Right(TransNum, digits)

                            ' CHECK IF GENERATED TRANSNUM ALREADY EXIST
                            query = " SELECT    " & ColumnPK & " AS TransID  " & _
                                    " FROM      " & Table & "  " & _
                                    " WHERE     " & ColumnPK & " = '" & TransNum & "' AND BranchCode = '" & BranchCode & "'"
                            SQL.ReadQuery(query)
                            If SQL.SQLDR.Read Then
                                Dim updateSQL As String
                                updateSQL = " UPDATE  " & Table & "  SET TransAuto = 1 WHERE " & ColumnPK & " = '" & TransNum & "' "
                                SQL.ExecNonQuery(updateSQL)
                                TransNum = -1
                            End If
                        End If
                    End If
                End If
                If TransNum <> -1 Then Exit Do
            Loop

            Return TransNum
        Else
            Return ""
        End If
    End Function

    Private Sub tsbEdit_Click(sender As System.Object, e As System.EventArgs) Handles tsbEdit.Click
        If Not AllowAccess("CV_EDIT") Then
            msgRestricted()
        Else
            EnableControl(True)

            ' Toolstrip Buttons
            tsbSearch.Enabled = False
            tsbNew.Enabled = False
            tsbEdit.Enabled = False
            tsbSave.Enabled = True
            tsbCancel.Enabled = False
            tsbDelete.Enabled = False
            tsbClose.Enabled = True
            tsbPrevious.Enabled = False
            tsbNext.Enabled = False
            tsbExit.Enabled = False
            tsbPrint.Enabled = False
            tsbCopy.Enabled = False
            tsbOption.Enabled = True
        End If
    End Sub

    Private Sub tsbCancel_Click(sender As System.Object, e As System.EventArgs) Handles tsbCancel.Click
        If Not AllowAccess("CV_DEL") Then
            msgRestricted()
        Else
            If txtTransNum.Text <> "" Then
                If txtStatus.Text <> "Cancelled" AndAlso MsgBox("Are you sure you want to cancel this record?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "GR8 Message Alert") = MsgBoxResult.Yes Then
                    Try
                        activityStatus = True
                        Dim updateSQL As String
                        updateSQL = " UPDATE  tblCV SET Status ='Cancelled' WHERE TransID = @TransID "
                        SQL.FlushParams()
                        CVNo = txtTransNum.Text
                        SQL.AddParam("@TransID", TransID)
                        SQL.ExecNonQuery(updateSQL)

                        updateSQL = " UPDATE  tblJE_Header SET Status ='Cancelled' WHERE RefTransID = @RefTransID  AND RefType ='CV' "
                        SQL.FlushParams()
                        SQL.AddParam("@RefTransID", TransID)
                        SQL.ExecNonQuery(updateSQL)


                        Dim insertSQL As String
                        insertSQL = " INSERT INTO tblJE_Details(JE_No, AccntCode, VCECode, Debit, Credit, Particulars, RefNo, LineNumber, OtherRef) " & _
                                    " SELECT JE_No, AccntCode, VCECode, Credit, Debit, Particulars, RefNo, 0 AS LineNumber, OtherRef FROM tblJE_Details " & _
                                    " WHERE JE_No IN (SELECT DISTINCT JE_No FROM tblJE_Header WHERE RefType ='CV' AND RefTransID ='" & TransID & "') "
                        SQL.ExecNonQuery(insertSQL)
                        Msg("Record cancelled successfully", MsgBoxStyle.Information)


                        If LOAN_ID > 0 Then
                            updateSQL = " UPDATE tblLoan SET Status ='Approved', DateRelease = '', RefType = '', RefTransID = '' WHERE TransID = '" & LOAN_ID & "'"
                            SQL.ExecNonQuery(updateSQL)
                        End If

                        If CA_ID > 0 Then
                            updateSQL = " UPDATE tblCA SET Status ='Active' WHERE TransID = '" & CA_ID & "'"
                            SQL.ExecNonQuery(updateSQL)
                        End If

                        tsbSearch.Enabled = True
                        tsbNew.Enabled = True
                        tsbEdit.Enabled = False
                        tsbSave.Enabled = False
                        tsbCancel.Enabled = False
                        tsbDelete.Enabled = False
                        tsbClose.Enabled = False
                        tsbPrevious.Enabled = False
                        tsbNext.Enabled = False
                        tsbExit.Enabled = True
                        tsbPrint.Enabled = True
                        tsbCopy.Enabled = False
                        EnableControl(False)

                        CVNo = txtTransNum.Text
                        LoadCV(TransID)
                    Catch ex As Exception
                        activityStatus = True
                        SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
                    Finally
                        RecordActivity(UserID, ModuleID, Me.Name.ToString, "CANCEL", "CV_No", CVNo, BusinessType, BranchCode, "", activityStatus)
                        SQL.FlushParams()
                    End Try
                ElseIf txtStatus.Text = "Cancelled" AndAlso MsgBox("Are you sure you want to un-cancel this record?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "GR8 Message Alert") = MsgBoxResult.Yes Then
                    Try
                        activityStatus = True
                        Dim updateSQL As String
                        updateSQL = " UPDATE  tblCV SET Status ='Active' WHERE TransID = @TransID "
                        SQL.FlushParams()
                        CVNo = txtTransNum.Text
                        SQL.AddParam("@TransID", TransID)
                        SQL.ExecNonQuery(updateSQL)

                        updateSQL = " UPDATE  tblJE_Header SET Status ='Saved' WHERE RefTransID = @RefTransID  AND RefType ='CV' "
                        SQL.FlushParams()
                        SQL.AddParam("@RefTransID", TransID)
                        SQL.ExecNonQuery(updateSQL)

                        Dim deleteSQL As String
                        deleteSQL = " DELETE FROM tblJE_Details " & _
                                    " WHERE JE_No IN (SELECT DISTINCT JE_No FROM tblJE_Header WHERE RefType ='CV' AND RefTransID ='" & TransID & "') AND LineNumber = 0 "
                        SQL.ExecNonQuery(deleteSQL)
                        Msg("Record cancelled successfully", MsgBoxStyle.Information)


                        If LOAN_ID > 0 Then
                            updateSQL = " UPDATE tblLoan SET Status ='Released', DateRelease = '" & dtpDocDate.Text & "', RefType = 'CV', RefTransID = '" & TransID & "' WHERE TransID = '" & LOAN_ID & "'"
                            SQL.ExecNonQuery(updateSQL)
                        End If

                        tsbSearch.Enabled = True
                        tsbNew.Enabled = True
                        tsbEdit.Enabled = False
                        tsbSave.Enabled = False
                        tsbCancel.Enabled = False
                        tsbDelete.Enabled = False
                        tsbClose.Enabled = False
                        tsbPrevious.Enabled = False
                        tsbNext.Enabled = False
                        tsbExit.Enabled = True
                        tsbPrint.Enabled = True
                        tsbCopy.Enabled = False
                        EnableControl(False)

                        CVNo = txtTransNum.Text
                        LoadCV(TransID)
                    Catch ex As Exception
                        activityStatus = True
                        SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
                    Finally
                        RecordActivity(UserID, ModuleID, Me.Name.ToString, "CANCEL", "CV_No", CVNo, BusinessType, BranchCode, "", activityStatus)
                        SQL.FlushParams()
                    End Try
                End If
            End If
        End If
    End Sub

    Private Sub tsbCopy_Click(sender As System.Object, e As System.EventArgs) Handles tsbCopy.Click

    End Sub

    Private Sub tsbPrint_Click(sender As System.Object, e As System.EventArgs)
        Dim f As New frmReport_Display
        f.ShowDialog("CV", TransID)
        f.Dispose()
    End Sub

    Private Sub tsbPrevious_Click(sender As System.Object, e As System.EventArgs) Handles tsbPrevious.Click
        If CVNo <> "" Then
            Dim query As String
            query = " SELECT Top 1 TransID FROM tblCV  " & _
                    "   INNER JOIN tblBranch  ON	          " & _
                    "   tblCV.BranchCode = tblBranch.BranchCode    " & _
                    "     WHERE          " & _
                    " 	( tblCV.BranchCode IN  " & _
                    " 	  ( " & _
                    "       SELECT    DISTINCT tblBranch.BranchCode  " & _
                    " 	    FROM      tblBranch   " & _
                    " 	    INNER JOIN tblUser_Access    ON          " & _
                    " 	    tblBranch.BranchCode = tblUser_Access.Code   " & _
                    " 	    AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                    " 	    AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                    " 	    WHERE     UserID ='" & UserID & "' " & _
                    " 	   ) " & _
                    "    )   " & _
                    " AND CV_No < '" & CVNo & "' ORDER BY CV_No DESC "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                TransID = SQL.SQLDR("TransID").ToString
                LoadCV(TransID)
            Else
                Msg("Reached the beginning of record!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub

    Private Sub tsbNext_Click(sender As System.Object, e As System.EventArgs) Handles tsbNext.Click
        If CVNo <> "" Then
            Dim query As String
            query = " SELECT Top 1 TransID FROM tblCV  " & _
                    "   INNER JOIN tblBranch  ON	          " & _
                    "   tblCV.BranchCode = tblBranch.BranchCode    " & _
                    "     WHERE          " & _
                    " 	( tblCV.BranchCode IN  " & _
                    " 	  ( " & _
                    "       SELECT    DISTINCT tblBranch.BranchCode  " & _
                    " 	    FROM      tblBranch   " & _
                    " 	    INNER JOIN tblUser_Access    ON          " & _
                    " 	    tblBranch.BranchCode = tblUser_Access.Code   " & _
                    " 	    AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                    " 	    AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                    " 	    WHERE     UserID ='" & UserID & "' " & _
                    " 	   ) " & _
                    "    )   " & _
                    " AND CV_No > '" & CVNo & "' ORDER BY CV_No ASC "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                TransID = SQL.SQLDR("TransID").ToString
                LoadCV(TransID)
            Else
                Msg("Reached the end of record!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub

    Private Sub tsbClose_Click(sender As System.Object, e As System.EventArgs) Handles tsbClose.Click

        ' Toolstrip Buttons
        If CVNo = "" Then
            ClearText()
            EnableControl(False)
            tsbEdit.Enabled = False
            tsbCancel.Enabled = False
            tsbDelete.Enabled = False
            tsbPrevious.Enabled = False
            tsbNext.Enabled = False
            tsbPrint.Enabled = False
        Else
            LoadCV(TransID)
            tsbEdit.Enabled = True
            tsbCancel.Enabled = True
            tsbDelete.Enabled = False
            tsbPrevious.Enabled = True
            tsbNext.Enabled = True
            tsbPrint.Enabled = True
        End If
        tsbSearch.Enabled = True
        tsbNew.Enabled = True
        tsbSave.Enabled = False
        tsbClose.Enabled = False
        tsbExit.Enabled = True
        tsbCopy.Enabled = False
    End Sub

    Private Sub tsbExit_Click(sender As System.Object, e As System.EventArgs) Handles tsbExit.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub tsbSave_Click(sender As System.Object, e As System.EventArgs) Handles tsbSave.Click
        If txtVCECode.Text = "" Then
            Msg("Please enter VCECode!", MsgBoxStyle.Exclamation)
        ElseIf txtTotalDebit.Text <> txtTotalCredit.Text Then
            MsgBox("Please check the Debit and Credit Amount!", MsgBoxStyle.Exclamation)
        ElseIf txtAmount.Text = "" Then
            MsgBox("Please check amount!", MsgBoxStyle.Exclamation)
        ElseIf dgvEntry.Rows.Count = 0 Then
            MsgBox("No entries, Please check!", MsgBoxStyle.Exclamation)
        ElseIf bankEvent = True And cbPaymentType.SelectedItem = "Check" And cbBank.SelectedIndex = -1 Then
            MsgBox("No bank selected, Please check!", MsgBoxStyle.Exclamation)
        ElseIf bankEvent = True And cbPaymentType.SelectedItem = "Multiple Check" And dgvMultipleCheck.Rows.Count = -1 Then
            MsgBox("No bank selected, Please check!", MsgBoxStyle.Exclamation)
        ElseIf TransAuto = False And txtTransNum.Text = "" Then
            MsgBox("Please Enter Voucher Number!", MsgBoxStyle.Exclamation)
        ElseIf TransAuto = False AndAlso IfExist(txtTransNum.Text) And TransID = "" Then
            MsgBox("CV" & " " & txtTransNum.Text & " already exist!", MsgBoxStyle.Exclamation)
        ElseIf TransID = "" Then
            If MsgBox("Saving New Record, Click Yes to confirm", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "GR8 Message Alert") = MsgBoxResult.Yes Then
                TransID = GenerateTransID(ColumnID, DBTable)
                If TransAuto Then
                    CVNo = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
                Else
                    CVNo = txtTransNum.Text
                End If
                txtTransNum.Text = CVNo
                SaveCV()
                Msg("Record Saved Succesfully!", MsgBoxStyle.Information)
                LoadCV(TransID)
            End If
        Else
            If MsgBox("Updating Record, Click Yes to confirm", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "GR8 Message Alert") = MsgBoxResult.Yes Then
                If CVNo = txtTransNum.Text Then
                    CVNo = txtTransNum.Text
                    UpdateCV()
                    Msg("Record Updated Succesfully!", MsgBoxStyle.Information)
                    CVNo = txtTransNum.Text
                    LoadCV(TransID)
                Else
                    If Not IfExist(txtTransNum.Text) Then
                        CVNo = txtTransNum.Text
                        UpdateCV()
                        Msg("Record Updated Succesfully!", MsgBoxStyle.Information)
                        CVNo = txtTransNum.Text
                        LoadCV(TransID)
                    Else
                        MsgBox("CV" & " " & txtTransNum.Text & " already exist!", MsgBoxStyle.Exclamation)
                    End If
                End If
            End If
        End If
    End Sub

    Private Function IfExist(ByVal ID As String) As Boolean
        Dim query As String
        query = " SELECT * FROM tblCV WHERE CV_No ='" & ID & "'  "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub TestToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub btnSearchVCE_Click(sender As System.Object, e As System.EventArgs) Handles btnSearchVCE.Click
        Dim f As New frmVCE_Search
        f.ShowDialog()
        txtVCECode.Text = f.VCECode
        txtVCEName.Text = f.VCEName
    End Sub

    Private Sub txtVCEName_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtVCEName.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim f As New frmVCE_Search
            f.cbFilter.SelectedItem = "VCEName"
            f.txtFilter.Text = txtVCEName.Text
            f.ShowDialog()
            txtVCECode.Text = f.VCECode
            txtVCEName.Text = f.VCEName
            f.Dispose()
        End If
    End Sub

    Private Sub frmCV_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.Control = True Then
            If e.KeyCode = Keys.S Then
                If tsbSave.Enabled = True Then tsbSave.PerformClick()
            ElseIf e.KeyCode = Keys.F Then
                If tsbSearch.Enabled = True Then tsbSearch.PerformClick()
            ElseIf e.KeyCode = Keys.N Then
                If tsbNew.Enabled = True Then tsbNew.PerformClick()
            ElseIf e.KeyCode = Keys.E Then
                If tsbEdit.Enabled = True Then tsbEdit.PerformClick()
            ElseIf e.KeyCode = Keys.D Then
                If tsbCancel.Enabled = True Then tsbCancel.PerformClick()
            ElseIf e.KeyCode = Keys.Left Then
                If tsbPrevious.Enabled = True Then tsbPrevious.PerformClick()
                e.SuppressKeyPress = True
            ElseIf e.KeyCode = Keys.Right Then
                If tsbNext.Enabled = True Then tsbNext.PerformClick()
                e.SuppressKeyPress = True
            ElseIf e.KeyCode = Keys.P Then
                If tsbPrint.Enabled = True Then tsbPrint.ShowDropDown()
            ElseIf e.KeyCode = Keys.R Then
                If tsbReports.Enabled = True Then tsbReports.PerformClick()
            ElseIf e.KeyCode = Keys.C Then
                If tsbCopy.Enabled = True Then tsbCopy.ShowDropDown()
            End If
        ElseIf e.Alt = True Then
            If e.KeyCode = Keys.F4 Then
                If tsbExit.Enabled = True Then
                    tsbExit.PerformClick()
                Else
                    e.SuppressKeyPress = True
                End If
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            If tsbClose.Enabled = True Then tsbClose.PerformClick()
        End If
    End Sub

    Private Sub tsbCopyAPV_Click(sender As System.Object, e As System.EventArgs) Handles tsbCopyAPV.Click
        Dim f As New frmLoadTransactions
        f.cbFilter.SelectedItem = "Status"
        f.txtFilter.Text = "Active"
        f.txtFilter.Enabled = False
        f.cbFilter.Enabled = False
        f.btnSearch.Enabled = False

        f.ShowDialog("APV")
        LoadAPV(f.transID)
        f.Dispose()
    End Sub

    Private Sub FromAdvancesToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles tsbCopyADV.Click
        Dim f As New frmLoadTransactions
        f.cbFilter.SelectedItem = "Status"
        f.txtFilter.Text = "Active"
        f.txtFilter.Enabled = False
        f.cbFilter.Enabled = False
        f.btnSearch.Enabled = False
        f.ShowDialog("ADV")
        LoadADV(f.transID)
        f.Dispose()
    End Sub

    Private Sub PrintCVToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PrintCVToolStripMenuItem.Click
        Dim f As New frmReport_Display
        f.ShowDialog("CV", TransID)
        f.Dispose()
    End Sub

    Private Sub tsbPrint_ButtonClick(sender As System.Object, e As System.EventArgs) Handles tsbPrint.ButtonClick
        'Dim f As New frmReport_Display
        'f.ShowDialog("CV", TransID)
        'f.Dispose()
    End Sub

    Private Sub ChequieToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ChequieToolStripMenuItem.Click
        Dim f As New frmCheckPrinting
        f.CVTransID = TransID
        f.ShowDialog()
        f.Dispose()
    End Sub

    Private Sub BIR2307ToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles BIR2307ToolStripMenuItem.Click
        Dim f As New frmReport_Display
        f.ShowDialog("BIR_2307", TransID)
        f.Dispose()
    End Sub

    Private Sub tsmUnreleased_Click(sender As System.Object, e As System.EventArgs) Handles tsmUnreleased.Click
        Dim updateSQl, CheckNo As String
        Dim bankIDMulti As Integer
        If MsgBox("Tagging cheque as released" & vbNewLine & "Click Yes to Confirm", MsgBoxStyle.YesNo + MsgBoxStyle.Information, "GR8 Message Alert") = MsgBoxResult.Yes Then
            If cbPaymentType.SelectedItem = "Check" Then

                updateSQl = " UPDATE tblCV_BankRef " & _
                            " SET   Status ='Released' WHERE CV_No ='" & TransID & "' AND BankID = '" & bankID & "' AND RefNo ='" & txtBankRef.Text & "' "
                SQL.ExecNonQuery(updateSQl)
                txtRefStatus.Text = "Released"
                tsmUnreleased.Enabled = False
            ElseIf cbPaymentType.SelectedItem = "Multiple Check" Then
                For Each item As DataGridViewRow In dgvMultipleCheck.Rows
                    If item.Cells(dgcBankID.Index).Value <> Nothing Then
                        bankIDMulti = item.Cells(dgcBankID.Index).Value
                        CheckNo = item.Cells(dgcCheckNo.Index).Value
                        updateSQl = " UPDATE tblCV_BankRef " & _
                          " SET   Status ='Released' WHERE CV_No ='" & TransID & "' AND BankID = '" & bankIDMulti & "' AND RefNo ='" & CheckNo & "' "
                        SQL.ExecNonQuery(updateSQl)
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub CancelCheckToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CancelCheckToolStripMenuItem.Click
        Dim updateSQl, CheckNo As String
        Dim bankIDMulti As Integer
        If MsgBox("Tagging cheque as cancelled" & vbNewLine & "Click Yes to Confirm", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation) = MsgBoxResult.Yes Then
            If cbPaymentType.SelectedItem = "Check" Then

                updateSQl = " UPDATE tblCV_BankRef " & _
                            " SET   Status ='Cancelled' WHERE CV_No ='" & TransID & "' AND BankID = '" & bankID & "' AND RefNo ='" & txtBankRef.Text & "' "
                SQL.ExecNonQuery(updateSQl)
                MsgBox("Please enter new cheque")
                txtBankRef.Select()

            ElseIf cbPaymentType.SelectedItem = "Multiple Check" Then
                For Each item As DataGridViewRow In dgvMultipleCheck.Rows
                    If item.Cells(dgcBankID.Index).Value <> Nothing Then
                        bankIDMulti = item.Cells(dgcBankID.Index).Value
                        CheckNo = item.Cells(dgcCheckNo.Index).Value
                        updateSQl = " UPDATE tblCV_BankRef " & _
                          " SET   Status ='Cancelled' WHERE CV_No ='" & TransID & "' AND BankID = '" & bankIDMulti & "' AND RefNo ='" & CheckNo & "' "
                        SQL.ExecNonQuery(updateSQl)
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub dgvEntry_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles dgvEntry.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Select Case dgvEntry.SelectedCells(0).ColumnIndex
                Case chRef.Index
                    Dim f As New frmSeachSL
                    f.ShowDialog()
                    If f.strVCECode <> "" Then
                        dgvEntry.Rows.Add("")
                        dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chVCECode.Index).Value = f.strVCECode
                        dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chVCEName.Index).Value = f.strVCEName
                        dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chAccntCode.Index).Value = f.strAccntCode
                        dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chAccntTitle.Index).Value = f.strAccntTitle
                        dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chRef.Index).Value = f.strRefNo
                        Dim selectSQL As String = " SELECT * FROM tblLoan_Type WHERE LoanAccount = '" & f.strAccntCode & "' "
                        SQL.ReadQuery(selectSQL)
                        If SQL.SQLDR.Read Then
                            dgvEntry.Rows.Add("")
                            dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chVCECode.Index).Value = f.strVCECode
                            dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chVCEName.Index).Value = f.strVCEName
                            dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chAccntCode.Index).Value = SQL.SQLDR("IntIncomeAccount").ToString
                            dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chAccntTitle.Index).Value = GetAccntTitle(SQL.SQLDR("IntIncomeAccount").ToString)
                            dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chRef.Index).Value = f.strRefNo
                        End If
                    End If
            End Select
        End If
    End Sub

    Private Sub FromRFPToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FromRFPToolStripMenuItem.Click
        Dim f As New frmLoadTransactions
        f.cbFilter.SelectedItem = "Status"
        f.txtFilter.Text = "Active"
        f.txtFilter.Enabled = False
        f.cbFilter.Enabled = False
        f.btnSearch.Enabled = False
        f.ShowDialog("RFP")
        LoadRFP(f.transID)
        f.Dispose()
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        Dim f As New frmMasterfile_Bank
        f.ShowDialog()
        f.Dispose()
        LoadBankList()
    End Sub

    Private Sub FromLoanToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FromLoanToolStripMenuItem.Click
        If Not AllowAccess("CV_LOAN") Then
            msgRestricted()
        Else
            Dim f As New frmLoadTransactions
            f.cbFilter.SelectedItem = "Status"
            f.txtFilter.Text = "Approved"
            f.txtFilter.Enabled = False
            f.cbFilter.Enabled = False
            f.btnSearch.Enabled = False
            f.ShowDialog("Copy Loan")
            If f.batch = True Then
                For Each row As DataGridViewRow In f.dgvList.Rows
                    If row.Cells(f.dgvList.Columns.Count - 1).Value = True Then
                        LoadLoan(row.Cells(0).Value)
                    End If
                Next
            Else
                LoadLoan(f.transID)
            End If

            f.Dispose()
        End If
    End Sub

#Region "TabControlMethod"
    Public Sub HideTabPageAll()
        If tpOrder Is Nothing Then
            ' The first time the Hide method is called, save the original order of the TabPages
            For Each TabPageCurrent As TabPage In tcPayment.TabPages
                tpOrder.Add(TabPageCurrent.Name)
            Next
        End If
        For Each TabPageCurrent As TabPage In tcPayment.TabPages
            Dim TabPageToHide As TabPage

            ' Get the TabPage object
            TabPageToHide = tcPayment.TabPages(TabPageCurrent.Name)
            ' Add the TabPage to the internal List
            tpHidden.Add(TabPageCurrent.Text, TabPageToHide)
            ' Remove the TabPage from the TabPages collection of the TabControl
            tcPayment.TabPages.Remove(TabPageToHide)
        Next
    End Sub

    Public Sub ShowTabPage(ByVal TabPageName As String)
        If tpHidden.ContainsKey(TabPageName) Then
            Dim TabPageToShow As TabPage

            ' Get the TabPage object
            TabPageToShow = tpHidden(TabPageName)
            ' Add the TabPage to the TabPages collection of the TabControl
            tcPayment.TabPages.Insert(GetTabPageInsertionPoint(TabPageName), TabPageToShow)
            ' Remove the TabPage from the internal List
            tpHidden.Remove(TabPageName)
        End If
    End Sub


    Public Sub ShowTabPageAll()
        For Each kvp As KeyValuePair(Of String, System.Windows.Forms.TabPage) In tpHidden
            Dim v1 As String = kvp.Key
            Dim v2 As TabPage = kvp.Value
            tcPayment.TabPages.Insert(GetTabPageInsertionPoint(v1), v2)
        Next
        tpHidden.Clear()
        '       tpHidden = Nothing
        '       tpOrder = Nothing
    End Sub

    Private Function GetTabPageInsertionPoint(ByVal TabPageName As String) As Integer
        Dim TabPageIndex As Integer
        Dim TabPageCurrent As TabPage
        Dim TabNameIndex As Integer
        Dim TabNameCurrent As String

        For TabPageIndex = 0 To tcPayment.TabPages.Count - 1
            TabPageCurrent = tcPayment.TabPages(TabPageIndex)
            For TabNameIndex = TabPageIndex To tpOrder.Count - 1
                TabNameCurrent = tpOrder(TabNameIndex)
                If TabNameCurrent = TabPageCurrent.Name Then
                    Exit For
                End If
                If TabNameCurrent = TabPageName Then
                    Return TabPageIndex
                End If
            Next
        Next
        Return TabPageIndex
    End Function
#End Region

    Private Sub gbPayee_Enter(sender As System.Object, e As System.EventArgs) Handles gbPayee.Enter

    End Sub

    Private Sub txtVCEName_MouseCaptureChanged(sender As Object, e As System.EventArgs) Handles txtVCEName.MouseCaptureChanged

    End Sub

    Private Sub txtVCEName_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtVCEName.TextChanged

    End Sub


    Dim eColIndex As Integer = 0
    Private Sub dgvMultipleCheck_CellBeginEdit(sender As Object, e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles dgvMultipleCheck.CellBeginEdit
        eColIndex = e.ColumnIndex
    End Sub

    Private Sub dgvMultipleCheck_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvMultipleCheck.CellContentClick
      
    End Sub

    'Start of Cost Center insert to Table
    Dim strDefaultGridBankID As String = ""
    Dim strDefaultGridCheckNo As String = ""
    Dim strDefaultGridCheckDate As String = ""
    Dim strDefaultGridBankDesc As String = ""
    Private Sub dgvMultipleCheck_CellEndEdit(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvMultipleCheck.CellEndEdit
        Dim rowIndex As Integer = dgvMultipleCheck.CurrentCell.RowIndex
        Dim colIndex As Integer = dgvMultipleCheck.CurrentCell.ColumnIndex
        Dim BankIDMulti As Integer
        Try
            Select Case colIndex
                Case dgcCheckDate.Index
                    If Not IsDate(dgvMultipleCheck.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) Then
                        dgvMultipleCheck.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Date.Today.Date
                    End If
                Case dgcAmount.Index
                    If IsNumeric(dgvMultipleCheck.Item(e.ColumnIndex, e.RowIndex).Value) Then
                        dgvMultipleCheck.Item(e.ColumnIndex, e.RowIndex).Value = CDec(dgvMultipleCheck.Item(e.ColumnIndex, e.RowIndex).Value).ToString("N2")
                    End If
                Case dgcBank.Index
                    Dim BankDesc As String
                    BankDesc = dgvMultipleCheck.Rows(e.RowIndex).Cells(dgcBank.Index).Value
                    BankIDMulti = GetBankID(BankDesc)
                    LoadBankMultipleDetails(BankIDMulti, e.RowIndex, dgcBankID.Index, dgcCheckNo.Index, dgcCheckDate.Index)
                Case dgcCheckName.Index
                    Dim f As New frmVCE_Search
                    f.txtFilter.Text = dgvMultipleCheck.Item(e.ColumnIndex, e.RowIndex).Value.ToString
                    f.ShowDialog()
                    dgvMultipleCheck.Item(dgcCheckVCECode.Index, e.RowIndex).Value = f.VCECode
                    dgvMultipleCheck.Item(dgcCheckName.Index, e.RowIndex).Value = f.VCEName
                    f.Dispose()
            End Select
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub LoadBankMultipleDetails(ByVal BankIDMulti As Integer, ByVal RowIndex As Integer, ByVal BankIDIndex As Integer, ByVal CheckNoIndex As Integer, ByVal CheckDateIndex As Integer)

        Dim selectSQL As String
        selectSQL = " SELECT  RIGHT('000000000000' + CAST((CAST(MAX(RefNo) AS bigint) + 1) AS nvarchar), LEN(SeriesFrom)) AS RefNo, SeriesFrom " & _
                " FROM    tblCV_BankRef RIGHT JOIN tblBank_Master " & _
                " ON      tblCV_BankRef.BankID = tblBank_Master.BankID " & _
                " AND     tblCV_BankRef.RefNo BETWEEN SeriesFrom AND SeriesTo " & _
                " WHERE   tblBank_Master.BankID = '" & BankIDMulti & "'   " & _
                " GROUP BY LEN(SeriesFrom), SeriesFrom  "
        SQL.ReadQuery(selectSQL, 2)

        strDefaultGridBankID = ""
        strDefaultGridCheckNo = ""
        strDefaultGridCheckDate = ""
        strDefaultGridBankDesc = ""

        While SQL.SQLDR2.Read
            strDefaultGridBankID = BankIDMulti
            If Not IsDBNull(SQL.SQLDR2("RefNo")) Then
                strDefaultGridCheckNo = SQL.SQLDR2("RefNo").ToString()
            Else
                strDefaultGridCheckNo = SQL.SQLDR2("SeriesFrom").ToString()
            End If
            strDefaultGridCheckDate = Date.Today.Date
        End While
        dgvMultipleCheck.Rows(RowIndex).Cells(BankIDIndex).Value = strDefaultGridBankID
        dgvMultipleCheck.Rows(RowIndex).Cells(CheckNoIndex).Value = strDefaultGridCheckNo
        dgvMultipleCheck.Rows(RowIndex).Cells(CheckDateIndex).Value = strDefaultGridCheckDate


    End Sub


    Private Sub dgvMultipleCheck_CurrentCellDirtyStateChanged(sender As Object, e As System.EventArgs) Handles dgvMultipleCheck.CurrentCellDirtyStateChanged
        If eColIndex = dgcBank.Index And TypeOf (dgvMultipleCheck.CurrentRow.Cells(dgcBank.Index)) Is DataGridViewComboBoxCell Then
            dgvMultipleCheck.EndEdit()
        End If
    End Sub

    Private Sub tsbDelete_Click(sender As System.Object, e As System.EventArgs) Handles tsbDelete.Click
        If Not AllowAccess("CV_DEL") Then
            msgRestricted()
        Else
            If MsgBox("Are you sure you want to delete this transaction?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Delete") = MsgBoxResult.Yes Then
                Dim updateSQL As String

                Dim deleteSQL As String = "DELETE FROM tblCV WHERE TransID = '" & TransID & "'"
                SQL.ExecNonQuery(deleteSQL)

                deleteSQL = "DELETE FROM tblJE_Header WHERE RefType = 'CV' AND RefTransID = '" & TransID & "'"
                SQL.ExecNonQuery(deleteSQL)

                If LOAN_ID > 0 Then
                    updateSQL = " UPDATE tblLoan SET Status ='Approved', DateRelease = '', RefType = '', RefTransID = '' WHERE TransID = '" & LOAN_ID & "'"
                    SQL.ExecNonQuery(updateSQL)
                End If
                MsgBox("Successfully Deleted!", MsgBoxStyle.Information, "Delete")
                tsbNew.PerformClick()
            End If
        End If
    End Sub

    Private Sub FromFundsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FromFundsToolStripMenuItem.Click
        frmFund_Copy.strType = "CV"
        frmFund_Copy.ShowDialog()
    End Sub

    Private Sub txtBankRefName_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtBankRefName.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim f As New frmVCE_Search
            f.cbFilter.SelectedItem = "VCEName"
            f.txtFilter.Text = txtBankRefName.Text
            f.ShowDialog()
            txtBankRefName.Text = f.VCEName
            f.Dispose()
        End If
    End Sub

    Private Sub txtBankRefName_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtBankRefName.TextChanged

    End Sub

    Private Sub FromPCVToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FromPCVToolStripMenuItem.Click
        Dim f As New frmLoadTransactions
        f.cbFilter.SelectedItem = "Status"
        f.txtFilter.Text = "Active"
        f.txtFilter.Enabled = False
        f.cbFilter.Enabled = False
        f.btnSearch.Enabled = False

        f.ShowDialog("PCV-CV")
        LoadPCV(f.transID)
        f.Dispose()
    End Sub

    Private Sub LoadPCV(ByVal PCV_NO As String)
        Dim query As String
        'query = " SELECT TransNo, View_GL.AccntCode, AccntTitle, SUM(Credit) as Credit " & _
        '        " FROM View_GL " & _
        '        " WHERE JE_No = (SELECT DISTINCT  tblJE_Header.JE_No FROM tblJE_Header INNER JOIN tblJE_Details ON tblJE_Details.JE_No = tblJE_Header.JE_No WHERE RefType = 'PCV' AND " & _
        '         "tblJE_Details.RefNo LIKE '%" & PCV_NO & "%')  " & _
        '        " AND AccntCode IN (SELECT Account_Code FROM tblPCV_Entry_PaymentType) " & _
        '        " GROUP BY TransNo, View_GL.AccntCode, AccntTitle "
        'SQL.ReadQuery(query)
        'dgvEntry.Rows.Clear()
        'While SQL.SQLDR.Read
        '    dgvEntry.Rows.Add(BranchCode, SQL.SQLDR("AccntCode").ToString, SQL.SQLDR("AccntTitle").ToString, CDec(SQL.SQLDR("Credit")).ToString("N2"), _
        '                      "0.00", "", "", _
        '                       "", "PCV:" & SQL.SQLDR("TransNo").ToString)
        'End While
        'TotalDBCR()

        Try
            query = " SELECT Ref_TransID AS TransID, PCV_No, VCECode, Supplier AS  VCEName, Date AS Date_PCV, Amount , Remarks,  AccntCode, AccountTitle " & _
                    " FROM View_PCV_Balance " & _
                    " WHERE  Ref_TransID ='" & PCV_NO & "' "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                txtVCECode.Text = SQL.SQLDR("VCECode").ToString
                txtVCEName.Text = SQL.SQLDR("VCEName").ToString
                dgvEntry.Rows.Add(BranchCode, SQL.SQLDR("AccntCode").ToString, SQL.SQLDR("AccountTitle").ToString, CDec(SQL.SQLDR("Amount")).ToString("N2"), "0.00", SQL.SQLDR("VCECode").ToString, SQL.SQLDR("VCEName").ToString, "", "PCV:" & SQL.SQLDR("PCV_No").ToString)
                LoadBranch()
            End If
            TotalDBCR()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub FromCAToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FromCAToolStripMenuItem.Click
        Dim f As New frmLoadTransactions
        f.cbFilter.SelectedItem = "Status"
        f.txtFilter.Text = "Active"
        f.txtFilter.Enabled = False
        f.cbFilter.Enabled = False
        f.btnSearch.Enabled = False
        f.ShowDialog("CA")
        LoadCA(f.transID)
        f.Dispose()
    End Sub

    Private Sub btnUOMGroup_Click(sender As System.Object, e As System.EventArgs) Handles btnUOMGroup.Click
        frmDisbursement_Type.ShowDialog()
        LoadDisbursementType()
    End Sub
End Class