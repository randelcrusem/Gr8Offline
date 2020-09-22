Public Class frmCollection
    Public TransType, Book As String
    Dim TransID, RefID, JETransiD, DBAccount As String
    Dim TransNo As String
    Dim disableEvent As Boolean = False
    Dim ModuleID As String = "OR"
    Dim ColumnID As String = "TransID"
    Dim ColumnPK As String = "TransNo"
    Dim DBTable As String = "tblCollection"
    Dim TransAuto As Boolean
    Dim AccntCode As String
    Dim bankID, CA_ID As Integer
    Dim SI_ID As Integer


    'Dim DBAccount, DBTitle As String
    'Public VCECode, VCEName, CashPayment, CheckNO, BankName, Amount, DocNum, BSNO, PRNO, ORNO, WithholdingTax As String
    'Public CheckDate, DocDate, ApplicationDate, TaxDate As Date
    'Public billing_Period As String
    'Public BankAccountNo, Bank, BankBranch, BankAccountCode, BankAccountTitle As String
    'Dim EnableEvent As Boolean = True
    'Dim a As String
    'Dim activityResult As Boolean = True
    'Dim allowEdit As Boolean = True
    'Dim allowEvent As Boolean = True

    Public Overloads Function ShowDialog(ByVal DocNumber As String) As Boolean
        TransID = DocNumber
        MyBase.ShowDialog()
        Return True
    End Function


    Private Sub frmCollection_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.Text = "Collection - " & TransType
            Label5.Text = TransType & " No. :"
            TransAuto = GetTransSetup(TransType)
            LoadPaymentType()
            LoadCollectionType()
            LoadCollectionCompany()
            If cbPaymentType.Items.Count > 0 Then
                cbPaymentType.SelectedIndex = 0
            End If
            dtpDate.Value = Date.Today.Date

            If TransID <> "" Then
                If Not AllowAccess("OR_VIEW") Then
                    msgRestricted()
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
                    LoadCollection(TransID)
                End If
            Else
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
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, TransType)
        End Try
    End Sub

    Private Sub EnableControl(ByVal Value As Boolean)
        txtVCECode.Enabled = Value
        txtVCEName.Enabled = Value
        btnSearchVCE.Enabled = Value
        dgvEntry.AllowUserToAddRows = Value
        dgvEntry.AllowUserToDeleteRows = Value
        gbBank.Enabled = Value
        dtpDate.Enabled = Value

        If Value = True Then
            dgvEntry.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2
        Else
            dgvEntry.EditMode = DataGridViewEditMode.EditProgrammatically
        End If
        txtRemarks.Enabled = Value
        txtAmount.Enabled = Value
        cbPaymentType.Enabled = Value
        cbCollectionCompany.Enabled = Value
        cbCollectionType.Enabled = Value
        btnTypeMaintenance.Enabled = Value
        txtRemarks.Enabled = Value
        If TransAuto Then
            txtTransNum.Enabled = False
        Else
            txtTransNum.Enabled = Value
        End If
    End Sub

    Private Sub LoadCollection(ByVal ID As String)
        Dim query, PaymentType, CollectionComapany As String
        query = " SELECT  TransID, TransNo, DateTrans, PaymentType, tblCollection.VCECode, VCEName, Amount, CheckRef, BankRef, CheckDate, Remarks, tblCollection.Status, ISNULL(Collection_Company, 'KASAMA MPC') as Collection_Company " & _
                " FROM    tblCollection LEFT JOIN viewVCE_Master " & _
                " ON      tblCollection.VCECode = viewVCE_Master.VCECode " & _
                " WHERE   TransID = '" & ID & "' AND TransType = '" & TransType & "'"
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            TransID = SQL.SQLDR("TransID").ToString
            TransNo = SQL.SQLDR("TransNo").ToString
            txtTransNum.Text = TransNo
            PaymentType = SQL.SQLDR("PaymentType").ToString
            CollectionComapany = SQL.SQLDR("Collection_Company").ToString
            txtVCECode.Text = SQL.SQLDR("VCECode").ToString
            txtVCEName.Text = SQL.SQLDR("VCEName").ToString
            dtpDate.Value = SQL.SQLDR("DateTrans")
            txtAmount.Text = CDec(SQL.SQLDR("Amount")).ToString("N2")
            txtRemarks.Text = SQL.SQLDR("Remarks").ToString
            txtBankRef.Text = SQL.SQLDR("CheckRef").ToString
            cbBank.Text = SQL.SQLDR("BankRef").ToString
            If IsDate(SQL.SQLDR("CheckDate")) Then
                dtpBankRefDate.Value = SQL.SQLDR("CheckDate")
            End If
            txtRemarks.Text = SQL.SQLDR("Remarks").ToString
            txtStatus.Text = SQL.SQLDR("Status").ToString
            cbPaymentType.SelectedItem = PaymentType
            cbCollectionCompany.SelectedItem = CollectionComapany
            LoadEntry(TransID)

            tsbCancel.Text = "Cancel"
            ' TOOLSTRIP BUTTONS
            If txtStatus.Text = "Cancelled" Then
                tsbCancel.Text = "Un-Can"
                tsbEdit.Enabled = False
                tsbCancel.Enabled = True
                tsbDelete.Enabled = True
            Else
                tsbEdit.Enabled = True
                tsbCancel.Enabled = True
                tsbDelete.Enabled = True
            End If
            tsbPrint.Enabled = True
            tsbClose.Enabled = False
            tsbPrevious.Enabled = True
            tsbNext.Enabled = True
            tsbPrint.Enabled = True
            tsbSave.Enabled = False
            tsbNew.Enabled = True
            tsbSearch.Enabled = True
            tsbExit.Enabled = True
            tsbCopy.Enabled = False
            EnableControl(False)
        Else
            ClearText()
        End If
    End Sub

    Private Sub LoadEntry(ByVal CollectionID As Integer)
        Dim query As String
        query = " SELECT ID, JE_No, tblJE_Details.AccntCode, AccountTitle, tblJE_Details.VCECode, VCEName, Debit, Credit, Particulars, RefNo " & _
                " FROM  tblJE_Details LEFT JOIN viewVCE_Master " & _
                " ON     tblJE_Details.VCECode = viewVCE_Master.VCECode " & _
                " INNER JOIN tblCOA_Master " & _
                " ON     tblJE_Details.AccntCode = tblCOA_Master.AccountCode " & _
                " WHERE JE_No = (SELECT  JE_No FROM tblJE_Header WHERE RefType = '" & TransType & "' AND RefTransID = " & CollectionID & ") " & _
                " ORDER BY Debit DESC, Credit ASC "
        SQL.ReadQuery(query)
        dgvEntry.Rows.Clear()
        While SQL.SQLDR.Read
            JETransiD = SQL.SQLDR("JE_No")
            dgvEntry.Rows.Add(SQL.SQLDR("AccntCode").ToString, SQL.SQLDR("AccountTitle").ToString, CDec(SQL.SQLDR("Debit")).ToString("N2"), _
                               CDec(SQL.SQLDR("Credit")).ToString("N2"), SQL.SQLDR("VCECode").ToString, SQL.SQLDR("VCEName").ToString, _
                               SQL.SQLDR("Particulars").ToString, SQL.SQLDR("RefNo").ToString)
        End While
        TotalDBCR()
    End Sub

    Private Sub LoadPaymentType()
        Dim query As String
        query = " SELECT DISTINCT PaymentType FROM tblCollection_PaymentType WHERE Status ='Active' "
        SQL.ReadQuery(query)
        cbPaymentType.Items.Clear()
        While SQL.SQLDR.Read
            cbPaymentType.Items.Add(SQL.SQLDR("PaymentType").ToString)
        End While
        If cbPaymentType.Items.Count > 0 Then
            cbPaymentType.SelectedIndex = 0
        End If
    End Sub

    Private Sub cbPaymentType_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbPaymentType.SelectedIndexChanged
        Dim query As String
        query = " SELECT WithBank, Account_Code " & _
                " FROM   tblCollection_PaymentType " & _
                " WHERE  Status ='Active' AND PaymentType ='" & cbPaymentType.SelectedItem & "' "
        SQL.ReadQuery(query)
        cbCollectionType.Items.Clear()
        If SQL.SQLDR.Read Then
            gbBank.Visible = SQL.SQLDR("WithBank")
            DBAccount = SQL.SQLDR("Account_Code").ToString
        End If
    End Sub

    Private Sub LoadCollectionType()
        Dim query As String
        query = " SELECT DISTINCT Collection_Type FROM tblCollection_Type WHERE Status ='Active' "
        SQL.ReadQuery(query)
        cbCollectionType.Items.Clear()
        While SQL.SQLDR.Read
            cbCollectionType.Items.Add(SQL.SQLDR("Collection_Type").ToString)
        End While
    End Sub

    Private Sub LoadCollectionCompany()
        Dim query As String
        query = " SELECT DISTINCT Collection_Company FROM tblCollection_Company "
        SQL.ReadQuery(query)
        cbCollectionCompany.Items.Clear()
        While SQL.SQLDR.Read
            cbCollectionCompany.Items.Add(SQL.SQLDR("Collection_Company").ToString)
        End While
        cbCollectionCompany.SelectedIndex = 1
    End Sub

    Private Sub btnTypeMaintenance_Click(sender As System.Object, e As System.EventArgs) Handles btnTypeMaintenance.Click
        frmCollection_Type.ShowDialog()
        LoadCollectionType()
    End Sub

    Public Function getduplicateOR(orno As String) As Boolean
        Dim query As String
        query = "SELECT * FROM Collection WHERE (ORNO = N'" & orno & "')"
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub cbCollectionType_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbCollectionType.SelectedIndexChanged
        Try
            If disableEvent = False Then
                Dim query As String
                Dim amount As Decimal
                query = " SELECT  Account_Code, AccountTitle, Amount, Collection_Type  " & _
                        " FROM    tblCollection_Type INNER JOIN tblCOA_Master " & _
                        " ON      tblCollection_Type.Account_Code = tblCOA_Master.AccountCode " & _
                        " WHERE   tblCollection_Type.Status ='Active' AND Collection_Type = @Collection_Type "
                SQL.FlushParams()
                SQL.AddParam("@Collection_Type", cbCollectionType.SelectedItem)
                SQL.ReadQuery(query)
                If SQL.SQLDR.Read Then
                    If txtAmount.Text = "" Then
                        amount = CDec(SQL.SQLDR("Amount"))
                    Else
                        amount = CDec(txtAmount.Text) - IIf(txtTotalDebit.Text = "", 0, txtTotalDebit.Text)
                    End If
                    dgvEntry.Rows.Add(SQL.SQLDR("Account_Code").ToString, SQL.SQLDR("AccountTitle").ToString, "0.00", CDec(amount).ToString("N2"), "", "", cbCollectionType.SelectedItem, IIf(cbRef.Text = "", txtRef.Text, cbRef.Text & ":" & txtRef.Text))
                    TotalDBCR()
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            SQL.FlushParams()
        End Try

    End Sub

    Private Sub dgvProducts_CellEndEdit(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvEntry.CellEndEdit
        Try
            If e.ColumnIndex = chDebit.Index Or e.ColumnIndex = chCredit.Index Then
                If IsNumeric(dgvEntry.Item(e.ColumnIndex, e.RowIndex).Value) Then
                    dgvEntry.Item(e.ColumnIndex, e.RowIndex).Value = CDec(dgvEntry.Item(e.ColumnIndex, e.RowIndex).Value).ToString("N2")
                End If
                TotalDBCR()
            ElseIf e.ColumnIndex = chAccntCode.Index Or e.ColumnIndex = chAccntTitle.Index Then
                Dim f As New frmCOA_Search
                f.ShowDialog("AccntTitle", dgvEntry.Item(e.ColumnIndex, e.RowIndex).Value.ToString)
                dgvEntry.Item(chAccntCode.Index, e.RowIndex).Value = f.accountcode
                dgvEntry.Item(chAccntTitle.Index, e.RowIndex).Value = f.accttile
                f.Dispose()
                dgvEntry.Item(chDebit.Index, e.RowIndex).Selected = True

                ''Auto Entry Ref No
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
            ElseIf e.ColumnIndex = chVCECode.Index Or e.ColumnIndex = chVCEName.Index Then
                Dim f As New frmVCE_Search

                f.cbFilter.SelectedItem = "VCEName"
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
            ElseIf e.ColumnIndex = chRef.Index Then
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
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


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

    Private Sub txtVCEName_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtVCEName.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                Dim f As New frmVCE_Search
                f.cbFilter.SelectedItem = "VCEName"
                f.txtFilter.Text = txtVCEName.Text
                f.ShowDialog()
                txtVCECode.Text = f.VCECode
                txtVCEName.Text = f.VCEName
                f.Dispose()
                If TransType = "OR" Then
                    If txtVCECode.Text <> "" Then
                        f.Dispose()
                        Dim r As New frmReceivables
                        r.VCECode = txtVCECode.Text
                        r.CollectDate = dtpDate.Value.Date
                        r.Show()
                        'TotalCashAmount()
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub txtBankRefAmount_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtAmount.KeyDown
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

    Private Function IfExist(ByVal ID As Integer, Type As String) As Boolean
        Dim query As String
        query = " SELECT * FROM tblCollection WHERE TransNo ='" & ID & "' AND TransType ='" & Type & "' AND BranchCode = '" & BranchCode & "'"
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub SaveCollection()
        Try
            Dim insertSQL As String
            activityStatus = True
            insertSQL = " INSERT INTO " & _
                        " tblCollection (TransID, TransType, TransNo, DateTrans, VCECode, PaymentType, Amount,  CheckRef, BankRef, CheckDate, Remarks, TransAuto, WhoCreated, BranchCode, BusinessCode, Collection_Company) " & _
                        " VALUES(@TransID, @TransType, @TransNo, @DateTrans, @VCECode, @PaymentType, @Amount, @CheckRef, @BankRef, @CheckDate, @Remarks, @TransAuto, @WhoCreated, @BranchCode, @BusinessCode, @Collection_Company)"
            SQL.FlushParams()
            SQL.AddParam("@TransID", TransID)
            SQL.AddParam("@TransType", TransType)
            SQL.AddParam("@TransNo", TransNo)
            SQL.AddParam("@DateTrans", dtpDate.Value.Date)
            SQL.AddParam("@PaymentType", cbPaymentType.SelectedItem)
            SQL.AddParam("@Collection_Company", cbCollectionCompany.SelectedItem)
            SQL.AddParam("@VCECode", txtVCECode.Text)
            If IsNumeric(txtAmount.Text) Then
                SQL.AddParam("@Amount", CDec(txtAmount.Text))
            Else
                SQL.AddParam("@Amount", 0)
            End If
            SQL.AddParam("@CheckRef", txtBankRef.Text)
            SQL.AddParam("@BankRef", cbBank.Text)
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@TransAuto", TransAuto)
            SQL.AddParam("@CheckDate", IIf(gbBank.Visible, DBNull.Value, dtpBankRefDate.Value.Date))
            SQL.AddParam("@WhoCreated", UserID)
            SQL.AddParam("@BranchCode", BranchCode)
            SQL.AddParam("@BusinessCode", BusinessType)
            SQL.ExecNonQuery(insertSQL)

            insertSQL = " INSERT INTO " & _
                        " tblJE_Header (AppDate, BranchCode, BusinessCode, RefType, RefTransID, Book, TotalDBCR, Remarks, WhoCreated) " & _
                        " VALUES(@AppDate, @BranchCode, @BusinessCode, @RefType, @RefTransID, @Book, @TotalDBCR, @Remarks, @WhoCreated)"
            SQL.FlushParams()
            SQL.AddParam("@AppDate", dtpDate.Value.Date)
            SQL.AddParam("@RefType", TransType)
            SQL.AddParam("@RefTransID", TransID)
            SQL.AddParam("@Book", Book)
            SQL.AddParam("@TotalDBCR", CDec(txtTotalCredit.Text))
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@BranchCode", BranchCode)
            SQL.AddParam("@BusinessCode", BusinessType)
            SQL.AddParam("@WhoCreated", UserID)
            SQL.ExecNonQuery(insertSQL)

            JETransiD = LoadJE(TransType, TransID)

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
                    Else
                        SQL.AddParam("@RefNo", "")
                    End If
                    SQL.AddParam("@LineNumber", line)
                    SQL.AddParam("@BranchCode", BranchCode)
                    SQL.ExecNonQuery(insertSQL)
                    line += 1
                End If
            Next
            If CA_ID > 0 Then
                Dim updateSQL As String
                updateSQL = " UPDATE tblCA SET Status ='Closed' WHERE TransID = '" & CA_ID & "'"
                SQL.ExecNonQuery(updateSQL)
            End If
        Catch ex As Exception
            activityStatus = False
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, TransType)
        Finally
            RecordActivity(UserID, TransType, Me.Name.ToString, "INSERT", "TransNo", txtTransNum.Text, BusinessType, BranchCode, "", activityStatus)
            SQL.FlushParams()
        End Try
    End Sub

    Private Sub UpdateCollection()
        Try
            Dim insertSQL, UpdateSQL, deleteSQL As String
            activityStatus = True
            UpdateSQL = " UPDATE tblCollection  " & _
                        " SET    TransType = @TransType, TransNo = @TransNo, DateTrans = @DateTrans, PaymentType = @PaymentType, " & _
                        "        VCECode = @VCECode, Amount = @Amount, CheckRef = @CheckRef, BankRef = @BankRef, CheckDate = @CheckDate, Remarks = @Remarks, WhoModified = @WhoModified, DateModified = GETDATE(), Collection_Company = @Collection_Company" & _
                        " WHERE TransID = @TransID "
            SQL.FlushParams()
            SQL.AddParam("@TransID", TransID)
            SQL.AddParam("@TransType", TransType)
            SQL.AddParam("@TransNo", TransNo)
            SQL.AddParam("@DateTrans", dtpDate.Value.Date)
            SQL.AddParam("@PaymentType", cbPaymentType.SelectedItem)
            SQL.AddParam("@Collection_Company", cbCollectionCompany.SelectedItem)
            SQL.AddParam("@VCECode", txtVCECode.Text)
            If IsNumeric(txtAmount.Text) Then
                SQL.AddParam("@Amount", CDec(txtAmount.Text))
            Else
                SQL.AddParam("@Amount", 0)
            End If
            SQL.AddParam("@CheckRef", txtBankRef.Text)
            SQL.AddParam("@BankRef", cbBank.Text)
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@CheckDate", IIf(gbBank.Visible, DBNull.Value, dtpBankRefDate.Value.Date))
            SQL.AddParam("@WhoModified", UserID)
            SQL.ExecNonQuery(UpdateSQL)

            JETransiD = LoadJE(TransType, TransID)
            If JETransiD = 0 Then
                insertSQL = " INSERT INTO " & _
                       " tblJE_Header (AppDate, BranchCode, BusinessCode, RefType, RefTransID, Book, TotalDBCR, Remarks, WhoCreated) " & _
                       " VALUES(@AppDate, @BranchCode, @BusinessCode, @RefType, @RefTransID, @Book, @TotalDBCR, @Remarks, @WhoCreated)"
                SQL.FlushParams()
                SQL.AddParam("@AppDate", dtpDate.Value.Date)
                SQL.AddParam("@RefType", TransType)
                SQL.AddParam("@RefTransID", TransID)
                SQL.AddParam("@Book", Book)
                SQL.AddParam("@TotalDBCR", CDec(txtTotalCredit.Text))
                SQL.AddParam("@Remarks", txtRemarks.Text)
                SQL.AddParam("@BranchCode", BranchCode)
                SQL.AddParam("@BusinessCode", BusinessType)
                SQL.AddParam("@WhoCreated", "")
                SQL.ExecNonQuery(insertSQL)

                JETransiD = LoadJE(TransType, TransID)
            Else
                updateSQL = " UPDATE tblJE_Header " & _
                           " SET    AppDate = @AppDate, BranchCode = @BranchCode, BusinessCode = @BusinessCode, " & _
                           "        RefType = @RefType, RefTransID = @RefTransID, Book = @Book, TotalDBCR = @TotalDBCR, " & _
                           "        Remarks = @Remarks, WhoModified = @WhoModified, DateModified = GETDATE() " & _
                           " WHERE  JE_No = @JE_No "
                SQL.FlushParams()
                SQL.AddParam("@JE_No", JETransiD)
                SQL.AddParam("@AppDate", dtpDate.Value.Date)
                SQL.AddParam("@RefType", TransType)
                SQL.AddParam("@RefTransID", TransID)
                SQL.AddParam("@Book", Book)
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
                    Else
                        SQL.AddParam("@RefNo", "")
                    End If
                    SQL.AddParam("@LineNumber", line)
                    SQL.AddParam("@BranchCode", BranchCode)
                    SQL.ExecNonQuery(insertSQL)
                    line += 1
                End If
            Next
        Catch ex As Exception
            activityStatus = False
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, TransType)
        Finally
            RecordActivity(UserID, TransType, Me.Name.ToString, "UPDATE", "TransNo", txtTransNum.Text, BusinessType, BranchCode, "", activityStatus)
            SQL.FlushParams()
        End Try
    End Sub

    Private Sub ClearText()
        txtVCECode.Text = ""
        txtVCEName.Text = ""
        txtAmount.Text = ""
        txtRemarks.Text = ""
        cbBank.Items.Clear()
        txtBankRef.Text = ""
        txtStatus.Text = ""
        dgvEntry.Rows.Clear()
        tcCollection.SelectedTab = tpCollection
        cbCollectionCompany.SelectedIndex = 1
        txtTotalDebit.Text = "0.00"
        txtTotalCredit.Text = "0.00"
    End Sub

    Private Sub dgvManual_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvEntry.CellContentClick
        Try
            If e.ColumnIndex = 8 Then
                If IsNumeric(txtAmount.Text) AndAlso txtAmount.Text > 0 Then
                    dgvEntry.Rows.Add(DBAccount, GetAccntTitle(DBAccount), CDec(txtAmount.Text).ToString("N2"), "0.00", "", "", "", "")
                    txtAmount.Text = CDec(txtAmount.Text).ToString("N2")
                Else
                    If CDec(txtTotalCredit.Text) > CDec(txtTotalDebit.Text) Then
                        dgvEntry.Rows.Add(DBAccount, GetAccntTitle(DBAccount), CDec(CDec(txtTotalCredit.Text) - CDec(txtTotalDebit.Text)).ToString("N2"), "0.00", "", "", "", "")
                        txtAmount.Text = CDec(CDec(txtTotalCredit.Text) - CDec(txtTotalDebit.Text)).ToString("N2")
                    End If
                End If
                TotalDBCR()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub dgvEntry_RowsRemoved(sender As System.Object, e As System.Windows.Forms.DataGridViewRowsRemovedEventArgs) Handles dgvEntry.RowsRemoved
        Try
            TotalDBCR()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub tsbSearch_Click(sender As System.Object, e As System.EventArgs) Handles tsbSearch.Click
        If Not AllowAccess(TransType & "_VIEW") Then
            msgRestricted()
        Else
            Dim f As New frmLoadTransactions
            f.ShowDialog(TransType)
            TransID = f.transID
            LoadCollection(TransID)
            f.Dispose()
        End If
    End Sub

    Private Sub tsbNew_Click(sender As System.Object, e As System.EventArgs) Handles tsbNew.Click
        If Not AllowAccess(TransType & "_ADD") Then
            msgRestricted()
        Else
            ClearText()
            TransID = ""
            TransNo = ""

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
            txtStatus.Text = "Open"
            EnableControl(True)

            txtTransNum.Text = GenerateTransNum(TransAuto, TransType, ColumnPK, DBTable)
        End If
    End Sub

    Private Sub tsbEdit_Click(sender As System.Object, e As System.EventArgs) Handles tsbEdit.Click
        If Not AllowAccess(TransType & "_EDIT") Then
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
        End If
    End Sub

    Private Sub tsbSave_Click(sender As System.Object, e As System.EventArgs) Handles tsbSave.Click
        If txtVCECode.Text = "" Then
            Msg("Please enter VCECode!", MsgBoxStyle.Exclamation)
        ElseIf txtRemarks.Text = "" Then
            MsgBox("Please enter a remark/short explanation", MsgBoxStyle.Exclamation)
        ElseIf txtTotalDebit.Text <> txtTotalCredit.Text Then
            MsgBox("Please check the Debit and Credit Amount!", MsgBoxStyle.Exclamation)
        ElseIf txtAmount.Text = "" Then
            MsgBox("Please check amount!", MsgBoxStyle.Exclamation)
        ElseIf dgvEntry.Rows.Count = 0 Then
            MsgBox("No entries, Please check!", MsgBoxStyle.Exclamation)
        ElseIf gbBank.Visible = True And cbBank.Text = "" Then
            MsgBox("Please enter bank of the received cheque", MsgBoxStyle.Exclamation)
        ElseIf TransAuto = False AndAlso txtTransNum.Text = "" Then
            MsgBox("Please enter " & TransType & " Number", MsgBoxStyle.Exclamation)
        ElseIf TransAuto = False AndAlso IfExist(txtTransNum.Text, TransType) And TransID = "" Then
            MsgBox(TransType & " " & txtTransNum.Text & " already exist!", MsgBoxStyle.Exclamation)
        ElseIf TransID = "" Then
            If MsgBox("Saving New Record, Click Yes to confirm", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "GR8 Message Alert") = MsgBoxResult.Yes Then
                TransID = GenerateTransID(ColumnID, DBTable)
                If TransAuto Then
                    TransNo = GenerateTransNum(TransAuto, TransType, ColumnPK, DBTable)
                Else
                    TransNo = txtTransNum.Text
                End If
                txtTransNum.Text = TransNo
                SaveCollection()
                Msg("Record Saved Succesfully!", MsgBoxStyle.Information)
                LoadCollection(TransID)
            End If
        Else
            If MsgBox("Updating Record, Click Yes to confirm", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "GR8 Message Alert") = MsgBoxResult.Yes Then
                If TransNo = txtTransNum.Text Then
                    TransNo = txtTransNum.Text
                    UpdateCollection()
                    Msg("Record Updated Succesfully!", MsgBoxStyle.Information)
                    TransNo = txtTransNum.Text
                    LoadCollection(TransID)
                Else
                    If Not IfExist(txtTransNum.Text, TransType) Then
                        TransNo = txtTransNum.Text
                        UpdateCollection()
                        Msg("Record Updated Succesfully!", MsgBoxStyle.Information)
                        TransNo = txtTransNum.Text
                        LoadCollection(TransID)
                    Else
                        MsgBox(TransType & " " & txtTransNum.Text & " already exist!", MsgBoxStyle.Exclamation)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub tsbCancel_Click(sender As System.Object, e As System.EventArgs) Handles tsbCancel.Click
        If Not AllowAccess(TransType & "_DEL") Then
            msgRestricted()
        Else
            If txtTransNum.Text <> "" Then
                If txtStatus.Text <> "Cancelled" AndAlso MsgBox("Are you sure you want to cancel this record?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "GR8 Message Alert") = MsgBoxResult.Yes Then
                    Try
                        activityStatus = True
                        Dim updateSQL As String
                        updateSQL = " UPDATE  tblCollection SET Status ='Cancelled' WHERE TransID = @TransID AND TransType = @TransType"
                        SQL.FlushParams()
                        SQL.AddParam("@TransID", TransID)
                        SQL.AddParam("@TransType", TransType)
                        SQL.ExecNonQuery(updateSQL)

                        updateSQL = " UPDATE  tblJE_Header SET Status ='Cancelled' WHERE RefTransID = @RefTransID  AND RefType = @RefType "
                        SQL.FlushParams()
                        SQL.AddParam("@RefTransID", TransID)
                        SQL.AddParam("@RefType", TransType)
                        SQL.ExecNonQuery(updateSQL)

                        Dim insertSQL As String
                        insertSQL = " INSERT INTO tblJE_Details(JE_No, AccntCode, VCECode, Debit, Credit, Particulars, RefNo, LineNumber, OtherRef) " & _
                                    " SELECT JE_No, AccntCode, VCECode, Credit, Debit, Particulars, RefNo, 0 AS LineNumber, OtherRef FROM tblJE_Details " & _
                                    " WHERE JE_No IN (SELECT DISTINCT JE_No FROM tblJE_Header WHERE RefType ='" & TransType & "' AND RefTransID ='" & TransID & "') "
                        SQL.ExecNonQuery(insertSQL)
                        Msg("Record Cancelled successfully", MsgBoxStyle.Information)

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

                        LoadCollection(TransID)
                    Catch ex As Exception
                        activityStatus = True
                        SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, TransType)
                    Finally
                        RecordActivity(UserID, TransType, Me.Name.ToString, "CANCEL", "TransNo", TransNo, BusinessType, BranchCode, "", activityStatus)
                        SQL.FlushParams()
                    End Try
                ElseIf txtStatus.Text = "Cancelled" AndAlso MsgBox("Are you sure you want to cancel this record?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "GR8 Message Alert") = MsgBoxResult.Yes Then
                    Try
                        activityStatus = True
                        Dim updateSQL As String
                        updateSQL = " UPDATE  tblCollection SET Status ='Active' WHERE TransID = @TransID AND TransType = @TransType"
                        SQL.FlushParams()
                        SQL.AddParam("@TransID", TransID)
                        SQL.AddParam("@TransType", TransType)
                        SQL.ExecNonQuery(updateSQL)

                        updateSQL = " UPDATE  tblJE_Header SET Status ='Saved' WHERE RefTransID = @RefTransID  AND RefType = @RefType "
                        SQL.FlushParams()
                        SQL.AddParam("@RefTransID", TransID)
                        SQL.AddParam("@RefType", TransType)
                        SQL.ExecNonQuery(updateSQL)

                        Dim insertSQL As String
                        insertSQL = " DELETE FROM tblJE_Details " & _
                                    " WHERE JE_No IN (SELECT DISTINCT JE_No FROM tblJE_Header WHERE RefType ='" & TransType & "' AND RefTransID ='" & TransID & "') AND LineNumber = 0 "
                        SQL.ExecNonQuery(insertSQL)
                        Msg("Record deleted successfully", MsgBoxStyle.Information)

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

                        LoadCollection(TransID)
                    Catch ex As Exception
                        activityStatus = True
                        SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, TransType)
                    Finally
                        RecordActivity(UserID, TransType, Me.Name.ToString, "CANCEL", "TransNo", TransNo, BusinessType, BranchCode, "", activityStatus)
                        SQL.FlushParams()
                    End Try
                End If
            End If
        End If
    End Sub


    Private Sub frmCollection_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
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
                If tsbPrint.Enabled = True Then tsbPrint.PerformClick()
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

    Private Sub tsbCopyPO_Click(sender As System.Object, e As System.EventArgs) Handles tsbCopyPO.Click
        Dim f As New frmLoadTransactions
        f.cbFilter.SelectedItem = "Status"
        f.txtFilter.Text = "Active"
        f.txtFilter.Enabled = False
        f.cbFilter.Enabled = False
        f.btnSearch.Enabled = False

        f.ShowDialog("SI")
        LoadSI(f.transID)
        f.Dispose()
    End Sub
    Private Sub LoadSI(ByVal APV As String)
        Try
            Dim query As String
            query = " SELECT Ref_TransID AS TransID, SI_No, VCECode, Supplier AS  VCEName, Date AS Date_SI, Amount AS Net_Purchase, Amount/1.12 * 0.12 AS Input_VAT, Amount/1.12 * 0.02 AS EWT, Remarks, Reference AS Reference_Other, AccntCode, AccountTitle " & _
                    " FROM View_SI_Balance " & _
                    " WHERE  Ref_TransID ='" & APV & "' "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                txtVCECode.Text = SQL.SQLDR("VCECode").ToString
                txtVCEName.Text = SQL.SQLDR("VCEName").ToString
                SI_ID = SQL.SQLDR("TransID")
                txtAPVRef.Text = SQL.SQLDR("SI_No")
                dgvEntry.Rows.Add(SQL.SQLDR("AccntCode").ToString, SQL.SQLDR("AccountTitle").ToString, "0.00", CDec(SQL.SQLDR("Net_Purchase")).ToString("N2"), "", "", "", "SI:" & txtAPVRef.Text)

            End If
            TotalDBCR()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub ToolStripButton1_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripButton1.Click
        frmUploader.ModID = "Collection"
        frmUploader.Show()
    End Sub

    Private Sub tsbNext_Click(sender As System.Object, e As System.EventArgs) Handles tsbNext.Click
        If TransNo <> "" Then
            Dim query As String
            query = " SELECT Top 1 TransID FROM tblCollection " & _
                    "   INNER JOIN tblBranch  ON	          " & _
                    "   tblCollection.BranchCode = tblBranch.BranchCode    " & _
                    "     WHERE          " & _
                    " 	( tblCollection.BranchCode IN  " & _
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
                    " AND  TransNo > '" & TransNo & "' AND TransType = '" & TransType & "'  ORDER BY TransNo ASC "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                TransID = SQL.SQLDR("TransID").ToString
                LoadCollection(TransID)
            Else
                Msg("Reached the end of record!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub

    Private Sub tsbPrevious_Click(sender As System.Object, e As System.EventArgs) Handles tsbPrevious.Click
        If TransNo <> "" Then
            Dim query As String
            query = " SELECT Top 1 TransID FROM tblCollection " & _
                    "   INNER JOIN tblBranch  ON	          " & _
                    "   tblCollection.BranchCode = tblBranch.BranchCode    " & _
                    "     WHERE          " & _
                    " 	( tblCollection.BranchCode IN  " & _
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
                    " AND  TransNo < '" & TransNo & "'  AND TransType = '" & TransType & "' ORDER BY TransNo DESC "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                TransID = SQL.SQLDR("TransID").ToString
                LoadCollection(TransID)
            Else
                Msg("Reached the beginning of record!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub


    Private Sub tsbClose_Click(sender As System.Object, e As System.EventArgs) Handles tsbClose.Click
        ' Toolstrip Buttons
        If TransID = "" Then
            ClearText()
            EnableControl(False)
            tsbEdit.Enabled = False
            tsbCancel.Enabled = False
            tsbDelete.Enabled = False
            tsbPrevious.Enabled = False
            tsbNext.Enabled = False
            tsbPrint.Enabled = False
        Else
            LoadCollection(TransID)
            tsbEdit.Enabled = True
            tsbCancel.Enabled = True
            tsbDelete.Enabled = True
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

    Private Sub txtVCEName_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtVCEName.TextChanged

    End Sub

    Private Sub btnSearchVCE_Click(sender As System.Object, e As System.EventArgs) Handles btnSearchVCE.Click
        Dim f As New frmVCE_Search
        f.ShowDialog()
        txtVCECode.Text = f.VCECode
        txtVCEName.Text = f.VCEName
        If TransType = "OR" Then
            If txtVCECode.Text <> "" Then
                f.Dispose()
                Dim r As New frmReceivables
                r.VCECode = txtVCECode.Text
                r.CollectDate = dtpDate.Value.Date
                r.Show()
                'TotalCashAmount()
            End If
        End If
    End Sub

    Private Sub tsbExit_Click(sender As System.Object, e As System.EventArgs) Handles tsbExit.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub tsbDelete_Click(sender As System.Object, e As System.EventArgs) Handles tsbDelete.Click
        If Not AllowAccess(TransType & "_DEL") Then
            msgRestricted()
        Else
            If MsgBox("Are you sure you want to delete this transaction?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Delete") = MsgBoxResult.Yes Then
                Dim deleteSQL As String = "DELETE FROM tblCollection WHERE TransID = '" & TransID & "'"
                SQL.ExecNonQuery(deleteSQL)
                deleteSQL = "DELETE FROM tblJE_Header WHERE RefType = 'OR' AND RefTransID = '" & TransID & "'"
                SQL.ExecNonQuery(deleteSQL)
                MsgBox("Successfully Deleted!", MsgBoxStyle.Information, "Delete")
                tsbNew.PerformClick()
            End If
        End If
    End Sub

    Private Sub TestToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles TestToolStripMenuItem1.Click
        Dim f As New frmReport_Filter
        f.Report = "Collection List"
        f.ShowDialog(TransType)
        f.Dispose()
    End Sub
    Private Sub dgvEntry_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgvEntry.KeyDown
        If e.KeyCode = Keys.Enter Then
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
                dgvEntry.Rows.Add(SQL.SQLDR("AccntCode").ToString, SQL.SQLDR("AccountTitle").ToString, "0.00", CDec(SQL.SQLDR("Net_Purchase")).ToString("N2"),
                                  SQL.SQLDR("VCECode").ToString, SQL.SQLDR("VCEName").ToString, "", "CA:" & SQL.SQLDR("CA_No").ToString)
            End If
            TotalDBCR()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub FromCAToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FromCAToolStripMenuItem.Click
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
End Class