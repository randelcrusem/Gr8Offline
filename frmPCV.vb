Public Class frmPCV
    Dim TransID, RefID, JETransiD As String
    Dim PCVNo As String
    Dim disableEvent As Boolean = False
    Dim bankEvent As Boolean = False
    Dim ModuleID As String = "PCV"
    Dim ColumnPK As String = "PCV_No"
    Dim ColumnID As String = "TransID"
    Dim DBTable As String = "tblPCV_Entry"
    Dim TransAuto As Boolean
    Dim AccntCode As String
    Dim APV_ID, ADV_ID, RFP_ID, LOAN_ID, PCV_ID As Integer
    Dim bankID As Integer
    Dim tpHidden As New Dictionary(Of String, System.Windows.Forms.TabPage)
    Dim tpOrder As New List(Of String)
    Dim tempBranchCode As String = ""

    Public Overloads Function ShowDialog(ByVal DocNumber As String) As Boolean
        TransID = DocNumber
        MyBase.ShowDialog()
        Return True
    End Function

    Private Sub Disbursement_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.Text = "(" & database & ") - Petty Cash Voucher "
            TransAuto = GetTransSetup(ModuleID)
            LoadBranches()
           
            If TransID <> "" Then
                If Not AllowAccess("PCV_VIEW") Then
                    msgRestricted()
                    dtpDocDate.Value = Date.Today.Date
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
                    EnableControl(False)
                Else
                    LoadPCV(TransID)
                End If
            Else
                dtpDocDate.Value = Date.Today.Date
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
                EnableControl(False)
            End If

        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        End Try
    End Sub

     Private Sub LoadBranches()
        Dim query As String
        query = "  SELECT tblBranch.BranchCode + ' - ' + Description AS BranchCode  " & _
                "  FROM      tblBranch  " & _
                "  LEFT JOIN tblUser_Access  ON         " & _
                "  tblBranch.BranchCode = tblUser_Access.Code   " & _
                "  AND       tblUser_Access.Type = 'BranchCode'  " & _
                "  AND  tblUser_Access.Status ='Active'  AND        " & _
                "  tblUser_Access.UserID = '" & UserID & "'  " & _
                "  WHERE     tblBranch.Status ='Active'   AND isAllowed = 1  "
        SQL.ReadQuery(query)
        cbBranchCode.Items.Clear()
        While SQL.SQLDR.Read
            cbBranchCode.Items.Add(SQL.SQLDR("BranchCode").ToString)
        End While
    End Sub

    Private Sub EnableControl(ByVal Value As Boolean)

        dtpDocDate.Enabled = Value
        txtVCEName.Enabled = Value
        btnSearchVCE.Enabled = Value
        dgvEntry.Enabled = Value
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
        cbBranchCode.Enabled = Value
        cbDisburseType.Enabled = Value
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

    Private Sub LoadPCV(ByVal ID As String)
        If dgvEntry.ColumnCount >= 11 AndAlso dgvEntry.Columns(10).HeaderText = "Balance" Then
            dgvEntry.Columns.Remove("Balance")
        End If
        Dim query, payment_type As String
        query = " SELECT  TransID, PCV_No, PaymentType, tblPCV_Entry.VCECode, VCEName, DatePCV, TotalAmount, Remarks, " & _
                "         ISNULL(APV_Ref,0) as APV_Ref, OR_Ref, ISNULL(LN_Ref,0) as LN_Ref, tblPCV_Entry.Status, " & _
                 " 		   tblPCV_Entry.BranchCode + ' - ' + tblBranch.Description AS BranchCode " & _
                " FROM    tblPCV_Entry LEFT JOIN viewVCE_Master " & _
                " ON      tblPCV_Entry.VCECode = viewVCE_Master.VCECode " & _
                "  LEFT JOIN tblBranch ON " & _
                "  tblBranch.BranchCode = tblPCV_Entry.BranchCode AND tblBranch.Status = 'Active' " & _
                " WHERE   TransID = '" & ID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            dtpDocDate.MinDate = "01-01-1900"
            TransID = SQL.SQLDR("TransID").ToString
            PCVNo = SQL.SQLDR("PCV_No").ToString
            LOAN_ID = SQL.SQLDR("LN_Ref").ToString
            APV_ID = SQL.SQLDR("APV_Ref").ToString
            txtTransNum.Text = PCVNo
            payment_type = SQL.SQLDR("PaymentType").ToString
            txtVCECode.Text = SQL.SQLDR("VCECode").ToString
            txtVCEName.Text = SQL.SQLDR("VCEName").ToString
            txtAmount.Text = CDec(SQL.SQLDR("TotalAmount")).ToString("N2")
            txtRemarks.Text = SQL.SQLDR("Remarks").ToString
            txtORNo.Text = SQL.SQLDR("OR_Ref").ToString
            txtStatus.Text = SQL.SQLDR("Status").ToString
            disableEvent = True
            cbBranchCode.SelectedItem = SQL.SQLDR("BranchCode").ToString
            dtpDocDate.Value = SQL.SQLDR("DatePCV")
            disableEvent = False
            txtLoanRef.Text = GetLoanNo(LOAN_ID)
            txtAPVRef.Text = GetAPVNo(APV_ID)

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

    Private Sub LoadEntry(ByVal PCVNo As Integer)
        Dim query As String
        query = " SELECT ID, JE_No, View_GL.BranchCode, View_GL.AccntCode, AccountTitle, View_GL.VCECode, View_GL.VCEName, Debit, Credit, Particulars, RefNo   " & _
                " FROM   View_GL INNER JOIN tblCOA_Master " & _
                " ON     View_GL.AccntCode = tblCOA_Master.AccountCode " & _
                " WHERE JE_No = (SELECT  JE_No FROM tblJE_Header WHERE RefType = 'PCV' AND RefTransID = " & PCVNo & ") " & _
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
 

    Private Sub cbCategory_SelectedIndexChanged(sender As System.Object, e As System.EventArgs)
        Try
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

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
        'If disableEvent = False Then
        '    txtTransNum.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
        'End If
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
        RFP_ID = 0
        LOAN_ID = 0
        PCV_ID = 0
        txtVCECode.Text = ""
        txtVCEName.Text = ""
        txtAmount.Text = ""
        txtRemarks.Text = ""
        cbBranchCode.SelectedIndex = 0
        txtTransNum.Text = ""
        txtAPVRef.Text = ""
        txtORNo.Text = ""
        txtStatus.Text = ""
        dgvEntry.Rows.Clear()
        txtTotalDebit.Text = "0.00"
        txtTotalCredit.Text = "0.00"
        dtpDocDate.MinDate = GetMaxPEC()
    End Sub

    Private Sub SavePCV()
        Try
            Dim insertSQL As String
            activityStatus = True
            insertSQL = " INSERT INTO " & _
                        " tblPCV_Entry (TransID, PCV_No,  VCECode, DatePCV, TotalAmount, Remarks, APV_Ref, ADV_Ref, PCV_Ref, LN_Ref, OR_Ref, WhoCreated, TransAuto, BranchCode, BusinessCode ) " & _
                        " VALUES (@TransID, @PCV_No,  @VCECode, @DatePCV, @TotalAmount, @Remarks, @APV_Ref, @ADV_Ref, @PCV_Ref, @LN_Ref, @OR_Ref, @WhoCreated, @TransAuto, @BranchCode, @BusinessCode)"
            SQL.FlushParams()
            SQL.AddParam("@TransID", TransID)
            SQL.AddParam("@PCV_No", PCVNo)
            SQL.AddParam("@VCECode", txtVCECode.Text)
            SQL.AddParam("@TotalAmount", CDec(txtAmount.Text))
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@APV_Ref", IIf(txtAPVRef.Text = "", DBNull.Value, APV_ID))
            SQL.AddParam("@ADV_Ref", IIf(txtADVRef.Text = "", DBNull.Value, ADV_ID))
            SQL.AddParam("@PCV_Ref", PCV_ID)
            SQL.AddParam("@LN_Ref", LOAN_ID)
            SQL.AddParam("@OR_Ref", txtORNo.Text)
            SQL.AddParam("@TransAuto", TransAuto)
            SQL.AddParam("@WhoCreated", UserID)
            SQL.AddParam("@BranchCode", tempBranchCode)
            SQL.AddParam("@DatePCV", dtpDocDate.Value.Date)
            SQL.AddParam("@BusinessCode", BusinessType)
            SQL.ExecNonQuery(insertSQL)

            If ADV_ID > 0 Then
                Dim updateSQL As String
                updateSQL = " UPDATE tblADV SET Status ='Closed' WHERE TransID = '" & ADV_ID & "'"
                SQL.ExecNonQuery(updateSQL)
            End If

            If LOAN_ID > 0 Then
                Dim updateSQL As String
                updateSQL = " UPDATE tblLoan SET Status ='Released', DateRelease = '" & dtpDocDate.Value.Date & "', RefType = 'PCV', RefTransID = '" & TransID & "' WHERE TransID = '" & LOAN_ID & "'"
                SQL.ExecNonQuery(updateSQL)
            End If

            If RFP_ID > 0 Then
                Dim updateSQL As String
                updateSQL = " UPDATE tblRFP SET Status ='Closed' WHERE TransID = '" & RFP_ID & "'"
                SQL.ExecNonQuery(updateSQL)
            End If

            insertSQL = " INSERT INTO " & _
                        " tblJE_Header (AppDate, BranchCode, BusinessCode, RefType, RefTransID, Book, TotalDBCR, Remarks, WhoCreated) " & _
                        " VALUES(@AppDate, @BranchCode, @BusinessCode, @RefType, @RefTransID, @Book, @TotalDBCR, @Remarks, @WhoCreated)"
            SQL.FlushParams()
            SQL.AddParam("@AppDate", dtpDocDate.Value.Date)
            SQL.AddParam("@RefType", "PCV")
            SQL.AddParam("@RefTransID", TransID)
            SQL.AddParam("@Book", "Cash Disbursements")
            SQL.AddParam("@TotalDBCR", CDec(txtTotalCredit.Text))
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@BranchCode", tempBranchCode)
            SQL.AddParam("@BusinessCode", BusinessType)
            SQL.AddParam("@WhoCreated", UserID)
            SQL.ExecNonQuery(insertSQL)

            JETransiD = LoadJE("PCV", TransID)

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
                    SQL.AddParam("@BranchCode", tempBranchCode)
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
                        updateSQL = " UPDATE tblLoan SET Status ='Released', DateRelease = '" & dtpDocDate.Value.Date & "', RefType = 'PCV', RefTransID = '" & TransID & "' WHERE Loan_No = '" & strRefNo.Split("-")(i) & "'"
                        SQL.ExecNonQuery(updateSQL)
                    End If
                Next
            End If
        Catch ex As Exception
            activityStatus = False
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        Finally
            RecordActivity(UserID, ModuleID, Me.Name.ToString, "INSERT", "PCV_No", txtTransNum.Text, BusinessType, tempBranchCode, "", activityStatus)
            SQL.FlushParams()
        End Try
    End Sub


    Private Sub UpdateCV()
        Try
            Dim insertSQL, updateSQL, deleteSQL As String
            activityStatus = True
            updateSQL = " UPDATE tblPCV_Entry  " & _
                        " SET    PCV_No = @PCV_No, VCECode = @VCECode, DatePCV = @DatePCV, " & _
                        "        TotalAmount = @TotalAmount, Remarks = @Remarks, APV_Ref = @APV_Ref, ADV_Ref = @ADV_Ref, OR_Ref = @OR_Ref, WhoModified = @WhoModified, DateModified = GETDATE(), " & _
                        "        BranchCode = @BranchCode, BusinessCode = @BusinessCode" & _
                        " WHERE TransID = @TransID "
            SQL.FlushParams()
            SQL.AddParam("@TransID", TransID)
            SQL.AddParam("@PCV_No", PCVNo)
            SQL.AddParam("@VCECode", txtVCECode.Text)
            SQL.AddParam("@DatePCV", dtpDocDate.Value.Date)
            SQL.AddParam("@TotalAmount", CDec(txtAmount.Text))
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@APV_Ref", IIf(txtAPVRef.Text = "", DBNull.Value, APV_ID))
            SQL.AddParam("@ADV_Ref", IIf(txtADVRef.Text = "", DBNull.Value, ADV_ID))
            SQL.AddParam("@OR_Ref", txtORNo.Text)
            SQL.AddParam("@WhoModified", UserID)
            SQL.AddParam("@BranchCode", tempBranchCode)
            SQL.AddParam("@BusinessCode", BusinessType)
            SQL.ExecNonQuery(updateSQL)

            If LOAN_ID > 0 Then
                updateSQL = " UPDATE tblLoan SET Status ='Released', DateRelease = '" & dtpDocDate.Value.Date & "', RefType = 'PCV', RefTransID = '" & TransID & "' WHERE TransID = '" & LOAN_ID & "'"
                SQL.ExecNonQuery(updateSQL)
            End If

           
            JETransiD = LoadJE("PCV", TransID)
            If JETransiD = 0 Then
                insertSQL = " INSERT INTO " & _
                       " tblJE_Header (AppDate, BranchCode, BusinessCode, RefType, RefTransID, Book, TotalDBCR, Remarks, WhoCreated) " & _
                       " VALUES(@AppDate, @BranchCode, @BusinessCode, @RefType, @RefTransID, @Book, @TotalDBCR, @Remarks, @WhoCreated)"
                SQL.FlushParams()
                SQL.AddParam("@AppDate", dtpDocDate.Value.Date)
                SQL.AddParam("@RefType", "PCV")
                SQL.AddParam("@RefTransID", TransID)
                SQL.AddParam("@Book", "Cash Disbursements")
                SQL.AddParam("@TotalDBCR", CDec(txtTotalCredit.Text))
                SQL.AddParam("@Remarks", txtRemarks.Text)
                SQL.AddParam("@BranchCode", tempBranchCode)
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
                SQL.AddParam("@RefType", "PCV")
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
                    SQL.AddParam("@BranchCode", tempBranchCode)
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
                        updateSQL1 = " UPDATE tblLoan SET Status ='Released', DateRelease = '" & dtpDocDate.Value.Date & "', RefType = 'PCV', RefTransID = '" & TransID & "' WHERE Loan_No = '" & strRefNo.Split("-")(i) & "'"
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

    Private Sub cbDisburseType_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbDisburseType.SelectedIndexChanged
        Try
            If disableEvent = False Then
                If cbDisburseType.SelectedIndex <> -1 Then
                    Dim query As String
                    Dim amount As Decimal
                    query = " SELECT  Account_Code, AccntTitle, Amount  " & _
                            " FROM    tblCV_ExpenseType INNER JOIN ChartOfAccount " & _
                            " ON      tblCV_ExpenseType.Account_Code = ChartOfAccount.AccntCode " & _
                            " WHERE   Status ='Active' AND Expense_Description = @Expense_Description "
                    SQL.FlushParams()
                    SQL.AddParam("@Expense_Description", cbDisburseType.SelectedItem)
                    SQL.ReadQuery(query)
                    If SQL.SQLDR.Read Then
                        If txtAmount.Text = "" Then
                            amount = CDec(SQL.SQLDR("Amount"))
                        Else
                            amount = CDec(txtAmount.Text) - IIf(txtTotalDebit.Text = "", 0, txtTotalDebit.Text)
                        End If
                        dgvEntry.Rows.Add(SQL.SQLDR("Account_Code").ToString, SQL.SQLDR("AccntTitle").ToString, CDec(amount).ToString("N2"), "0.00", "", "", cbDisburseType.SelectedItem)
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
                    query = " SELECT  WithBank, Account_Code, AccountTitle " & _
                            " FROM    tblPCV_Entry_PaymentType LEFT JOIN tblCOA_Master " & _
                            " ON      tblPCV_Entry_PaymentType.Account_Code = tblCOA_Master.AccountCode " & _
                            " WHERE   Payment_Type ='" & cbBranchCode.SelectedItem & "' "
                    SQL.ReadQuery(query)
                    If SQL.SQLDR.Read Then
                        If cbBranchCode.SelectedIndex <> -1 Then
                            dgvEntry.Rows.Add(BranchCode, SQL.SQLDR("Account_Code").ToString, SQL.SQLDR("AccountTitle").ToString, "0.00", CDec(CDec(txtTotalDebit.Text) - CDec(txtTotalCredit.Text)).ToString("N2"), "", "", "", "")
                                    LoadBranch()
                                txtAmount.Text = CDec(CDec(txtTotalDebit.Text) - CDec(txtTotalCredit.Text)).ToString("N2")
                        End If
                        TotalDBCR()
                        End If
                    End If
                End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Function GetCashAccount(ByVal BranchCode As String)
        Dim cashAccount As String = ""
        Dim query As String
        query = " SELECT  WithBank, Account_Code, AccountTitle " & _
                           " FROM    tblPCV_Entry_PaymentType LEFT JOIN tblCOA_Master " & _
                           " ON      tblPCV_Entry_PaymentType.Account_Code = tblCOA_Master.AccountCode " & _
                           " WHERE   Payment_Type ='" & BranchCode & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            
                cashAccount = SQL.SQLDR("Account_Code").ToString
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
                Dim strVCECode As String = ""
                Dim strAccntCode As String = ""
                strVCECode = txtVCECode.Text
                strAccntCode = dgvEntry.Item(chAccntCode.Index, e.RowIndex).Value
                If dgvEntry.Item(chVCECode.Index, e.RowIndex).Value <> "" Then
                    strVCECode = dgvEntry.Item(chVCECode.Index, e.RowIndex).Value
                End If
                dgvEntry.Item(chRef.Index, e.RowIndex).Value = GetRefNo(strVCECode, strAccntCode)
                Dim strRefNo As String = ""
                Dim strRefNoLoan As String = ""
                strRefNo = dgvEntry.Item(chRef.Index, e.RowIndex).Value
                strRefNoLoan = GetRefNoLoan(strRefNo)
                If strRefNoLoan <> "" Then
                    dgvEntry.Rows.Add("")
                    dgvEntry.Item(chVCECode.Index, e.RowIndex + 1).Value = GetRefNoVCECode(strRefNo)
                    dgvEntry.Item(chVCEName.Index, e.RowIndex + 1).Value = GetVCEName(dgvEntry.Item(chVCECode.Index, e.RowIndex + 1).Value)
                    dgvEntry.Item(chAccntCode.Index, e.RowIndex + 1).Value = strRefNoLoan
                    dgvEntry.Item(chAccntTitle.Index, e.RowIndex + 1).Value = GetAccntTitle(dgvEntry.Item(chAccntCode.Index, e.RowIndex + 1).Value)
                    dgvEntry.Item(chRef.Index, e.RowIndex + 1).Value = strRefNo
                End If
            End If
        ElseIf e.ColumnIndex = chVCECode.Index Or e.ColumnIndex = chVCEName.Index Then
            Dim f As New frmVCE_Search
            f.txtFilter.Text = dgvEntry.Item(e.ColumnIndex, e.RowIndex).Value.ToString
            f.ShowDialog()
            dgvEntry.Item(chVCECode.Index, e.RowIndex).Value = f.VCECode
            dgvEntry.Item(chVCEName.Index, e.RowIndex).Value = f.VCEName
            f.Dispose()



            'Auto Entry RefNo
            Dim strVCECode As String = ""
            Dim strAccntCode As String = ""
            strVCECode = txtVCECode.Text
            strAccntCode = dgvEntry.Item(chAccntCode.Index, e.RowIndex).Value
            If dgvEntry.Item(chVCECode.Index, e.RowIndex).Value <> "" Then
                strVCECode = dgvEntry.Item(chVCECode.Index, e.RowIndex).Value
            End If
            If strAccntCode <> "" Then
                dgvEntry.Item(chRef.Index, e.RowIndex).Value = GetRefNo(strVCECode, strAccntCode)
                Dim strRefNo As String = ""
                Dim strRefNoLoan As String = ""
                strRefNo = dgvEntry.Item(chRef.Index, e.RowIndex).Value
                strRefNoLoan = GetRefNoLoan(strRefNo)
                If strRefNoLoan <> "" Then
                    dgvEntry.Rows.Add("")
                    dgvEntry.Item(chVCECode.Index, e.RowIndex + 1).Value = GetRefNoVCECode(strRefNo)
                    dgvEntry.Item(chVCEName.Index, e.RowIndex + 1).Value = GetVCEName(dgvEntry.Item(chVCECode.Index, e.RowIndex + 1).Value)
                    dgvEntry.Item(chAccntCode.Index, e.RowIndex + 1).Value = strRefNoLoan
                    dgvEntry.Item(chAccntTitle.Index, e.RowIndex + 1).Value = GetAccntTitle(dgvEntry.Item(chAccntCode.Index, e.RowIndex + 1).Value)
                    dgvEntry.Item(chRef.Index, e.RowIndex + 1).Value = strRefNo
                End If
            End If
        ElseIf e.ColumnIndex = chRef.Index Then
            Dim strRefNo As String = ""
            strRefNo = dgvEntry.Item(chRef.Index, e.RowIndex).Value
            If strRefNo <> "" Then
                dgvEntry.Item(chVCECode.Index, e.RowIndex).Value = GetRefNoVCECode(strRefNo)
                dgvEntry.Item(chVCEName.Index, e.RowIndex).Value = GetVCEName(dgvEntry.Item(chVCECode.Index, e.RowIndex).Value)
                dgvEntry.Item(chAccntCode.Index, e.RowIndex).Value = GetRefNoAccntCode(strRefNo)
                dgvEntry.Item(chAccntTitle.Index, e.RowIndex).Value = GetAccntTitle(dgvEntry.Item(chAccntCode.Index, e.RowIndex).Value)
                Dim strRefNoLoan As String = ""
                strRefNo = dgvEntry.Item(chRef.Index, e.RowIndex).Value
                strRefNoLoan = GetRefNoLoan(strRefNo)
                If strRefNoLoan <> "" Then
                    dgvEntry.Rows.Add("")
                    dgvEntry.Item(chVCECode.Index, e.RowIndex + 1).Value = GetRefNoVCECode(strRefNo)
                    dgvEntry.Item(chVCEName.Index, e.RowIndex + 1).Value = GetVCEName(dgvEntry.Item(chVCECode.Index, e.RowIndex + 1).Value)
                    dgvEntry.Item(chAccntCode.Index, e.RowIndex + 1).Value = strRefNoLoan
                    dgvEntry.Item(chAccntTitle.Index, e.RowIndex + 1).Value = GetAccntTitle(dgvEntry.Item(chAccntCode.Index, e.RowIndex + 1).Value)
                    dgvEntry.Item(chRef.Index, e.RowIndex + 1).Value = strRefNo
                End If
            End If
        End If
    End Sub


    Private Sub LoadBranch()
        Try
            Dim dgvCB As New DataGridViewComboBoxColumn
            dgvCB = dgvEntry.Columns(dgcBranchCode.Index)
            dgvCB.Items.Clear()
            ' ADD ALL BranchCode
            Dim query As String
            query = "  SELECT tblBranch.BranchCode  " & _
                    "  FROM      tblBranch  " & _
                    "  LEFT JOIN tblUser_Access  ON         " & _
                    "  tblBranch.BranchCode = tblUser_Access.Code   " & _
                    "  AND       tblUser_Access.Type = 'BranchCode'  " & _
                    "  AND  tblUser_Access.Status ='Active'  AND        " & _
                    "  tblUser_Access.UserID = '" & UserID & "'  " & _
                    "  WHERE     tblBranch.Status ='Active'   AND isAllowed = 1  "
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


    Private Sub tsbSearch_Click(sender As System.Object, e As System.EventArgs) Handles tsbSearch.Click
        If Not AllowAccess("PCV_VIEW") Then
            msgRestricted()
        Else
            Dim f As New frmLoadTransactions
            f.ShowDialog("PCV")
            TransID = f.transID
            LoadPCV(TransID)
            f.Dispose()
        End If
    End Sub

    Private Sub tsbNew_Click(sender As System.Object, e As System.EventArgs) Handles tsbNew.Click
        If Not AllowAccess("PCV_ADD") Then
            msgRestricted()
        Else
            ClearText()
            TransID = ""
            PCVNo = ""

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
            txtStatus.Text = "Open"
            EnableControl(True)

            txtTransNum.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
            txtVCEName.Select()
        End If
    End Sub

    Private Sub tsbEdit_Click(sender As System.Object, e As System.EventArgs) Handles tsbEdit.Click
        If Not AllowAccess("PCV_EDIT") Then
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
        End If
    End Sub

    Private Sub tsbCancel_Click(sender As System.Object, e As System.EventArgs) Handles tsbCancel.Click
        If Not AllowAccess("PCV_DEL") Then
            msgRestricted()
        Else
            If txtTransNum.Text <> "" Then
                If txtStatus.Text <> "Cancelled" AndAlso MsgBox("Are you sure you want to cancel this record?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "GR8 Message Alert") = MsgBoxResult.Yes Then
                    Try
                        activityStatus = True
                        Dim updateSQL As String
                        updateSQL = " UPDATE  tblPCV_Entry SET Status ='Cancelled' WHERE TransID = @TransID "
                        SQL.FlushParams()
                        PCVNo = txtTransNum.Text
                        SQL.AddParam("@TransID", TransID)
                        SQL.ExecNonQuery(updateSQL)

                        updateSQL = " UPDATE  tblJE_Header SET Status ='Cancelled' WHERE RefTransID = @RefTransID  AND RefType ='PCV' "
                        SQL.FlushParams()
                        SQL.AddParam("@RefTransID", TransID)
                        SQL.ExecNonQuery(updateSQL)


                        Dim insertSQL As String
                        insertSQL = " INSERT INTO tblJE_Details(JE_No, AccntCode, VCECode, Debit, Credit, Particulars, RefNo, LineNumber, OtherRef) " & _
                                    " SELECT JE_No, AccntCode, VCECode, Credit, Debit, Particulars, RefNo, 0 AS LineNumber, OtherRef FROM tblJE_Details " & _
                                    " WHERE JE_No IN (SELECT DISTINCT JE_No FROM tblJE_Header WHERE RefType ='PCV' AND RefTransID ='" & TransID & "') "
                        SQL.ExecNonQuery(insertSQL)
                        Msg("Record cancelled successfully", MsgBoxStyle.Information)


                        If LOAN_ID > 0 Then
                            updateSQL = " UPDATE tblLoan SET Status ='Approved', DateRelease = '', RefType = '', RefTransID = '' WHERE TransID = '" & LOAN_ID & "'"
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
                        EnableControl(False)

                        PCVNo = txtTransNum.Text
                        LoadPCV(TransID)
                    Catch ex As Exception
                        activityStatus = True
                        SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
                    Finally
                        RecordActivity(UserID, ModuleID, Me.Name.ToString, "CANCEL", "PCV_No", PCVNo, BusinessType, BranchCode, "", activityStatus)
                        SQL.FlushParams()
                    End Try
                ElseIf txtStatus.Text = "Cancelled" AndAlso MsgBox("Are you sure you want to un-cancel this record?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "GR8 Message Alert") = MsgBoxResult.Yes Then
                    Try
                        activityStatus = True
                        Dim updateSQL As String
                        updateSQL = " UPDATE  tblPCV_Entry SET Status ='Active' WHERE TransID = @TransID "
                        SQL.FlushParams()
                        PCVNo = txtTransNum.Text
                        SQL.AddParam("@TransID", TransID)
                        SQL.ExecNonQuery(updateSQL)

                        updateSQL = " UPDATE  tblJE_Header SET Status ='Saved' WHERE RefTransID = @RefTransID  AND RefType ='PCV' "
                        SQL.FlushParams()
                        SQL.AddParam("@RefTransID", TransID)
                        SQL.ExecNonQuery(updateSQL)

                        Dim deleteSQL As String
                        deleteSQL = " DELETE FROM tblJE_Details " & _
                                    " WHERE JE_No IN (SELECT DISTINCT JE_No FROM tblJE_Header WHERE RefType ='PCV' AND RefTransID ='" & TransID & "') AND LineNumber = 0 "
                        SQL.ExecNonQuery(deleteSQL)
                        Msg("Record cancelled successfully", MsgBoxStyle.Information)


                        If LOAN_ID > 0 Then
                            updateSQL = " UPDATE tblLoan SET Status ='Released', DateRelease = '" & dtpDocDate.Text & "', RefType = 'PCV', RefTransID = '" & TransID & "' WHERE TransID = '" & LOAN_ID & "'"
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
                        EnableControl(False)

                        PCVNo = txtTransNum.Text
                        LoadPCV(TransID)
                    Catch ex As Exception
                        activityStatus = True
                        SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
                    Finally
                        RecordActivity(UserID, ModuleID, Me.Name.ToString, "CANCEL", "PCV_No", PCVNo, BusinessType, BranchCode, "", activityStatus)
                        SQL.FlushParams()
                    End Try
                End If
            End If
        End If
    End Sub

    Private Sub tsbCopy_Click(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub tsbPrint_Click(sender As System.Object, e As System.EventArgs)
        Dim f As New frmReport_Display
        f.ShowDialog("CV", TransID)
        f.Dispose()
    End Sub

    Private Sub tsbPrevious_Click(sender As System.Object, e As System.EventArgs) Handles tsbPrevious.Click
        If PCVNo <> "" Then
            Dim query As String
            query = " SELECT Top 1 TransID FROM tblPCV_Entry  " & _
                    "   INNER JOIN tblBranch  ON	          " & _
                    "   tblPCV_Entry.BranchCode = tblBranch.BranchCode    " & _
                    "     WHERE          " & _
                    " 	( tblPCV_Entry.BranchCode IN  " & _
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
                    " AND PCV_No < '" & PCVNo & "' ORDER BY PCV_No DESC "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                TransID = SQL.SQLDR("TransID").ToString
                LoadPCV(TransID)
            Else
                Msg("Reached the beginning of record!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub

    Private Sub tsbNext_Click(sender As System.Object, e As System.EventArgs) Handles tsbNext.Click
        If PCVNo <> "" Then
            Dim query As String
            query = " SELECT Top 1 TransID FROM tblPCV_Entry  " & _
                    "   INNER JOIN tblBranch  ON	          " & _
                    "   tblPCV_Entry.BranchCode = tblBranch.BranchCode    " & _
                    "     WHERE          " & _
                    " 	( tblPCV_Entry.BranchCode IN  " & _
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
                    " AND PCV_No > '" & PCVNo & "' ORDER BY PCV_No ASC "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                TransID = SQL.SQLDR("TransID").ToString
                LoadPCV(TransID)
            Else
                Msg("Reached the end of record!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub

    Private Sub tsbClose_Click(sender As System.Object, e As System.EventArgs) Handles tsbClose.Click

        ' Toolstrip Buttons
        If PCVNo = "" Then
            ClearText()
            EnableControl(False)
            tsbEdit.Enabled = False
            tsbCancel.Enabled = False
            tsbDelete.Enabled = False
            tsbPrevious.Enabled = False
            tsbNext.Enabled = False
            tsbPrint.Enabled = False
        Else
            LoadPCV(TransID)
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
        ElseIf TransAuto = False And txtTransNum.Text = "" Then
            MsgBox("Please Enter Voucher Number!", MsgBoxStyle.Exclamation)
        ElseIf TransAuto = False AndAlso IfExist(txtTransNum.Text) And TransID = "" Then
            MsgBox("PCV" & " " & txtTransNum.Text & " already exist!", MsgBoxStyle.Exclamation)
        ElseIf TransID = "" Then
            If MsgBox("Saving New Record, Click Yes to confirm", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "GR8 Message Alert") = MsgBoxResult.Yes Then
                TransID = GenerateTransID(ColumnID, DBTable)
                If TransAuto Then
                    PCVNo = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
                Else
                    PCVNo = txtTransNum.Text
                End If
                txtTransNum.Text = PCVNo
                SavePCV()
                Msg("Record Saved Succesfully!", MsgBoxStyle.Information)
                LoadPCV(TransID)
            End If
        Else
            If MsgBox("Updating Record, Click Yes to confirm", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "GR8 Message Alert") = MsgBoxResult.Yes Then
                If PCVNo = txtTransNum.Text Then
                    PCVNo = txtTransNum.Text
                    UpdateCV()
                    Msg("Record Updated Succesfully!", MsgBoxStyle.Information)
                    PCVNo = txtTransNum.Text
                    LoadPCV(TransID)
                Else
                    If Not IfExist(txtTransNum.Text) Then
                        PCVNo = txtTransNum.Text
                        UpdateCV()
                        Msg("Record Updated Succesfully!", MsgBoxStyle.Information)
                        PCVNo = txtTransNum.Text
                        LoadPCV(TransID)
                    Else
                        MsgBox("PCV" & " " & txtTransNum.Text & " already exist!", MsgBoxStyle.Exclamation)
                    End If
                End If
            End If
        End If
    End Sub

    Private Function IfExist(ByVal ID As String) As Boolean
        Dim query As String
        query = " SELECT * FROM tblPCV_Entry WHERE PCV_No ='" & ID & "' AND BranchCode = '" & tempBranchCode & "' "
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


    Private Sub PrintCVToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PrintCVToolStripMenuItem.Click
        Dim f As New frmReport_Display
        f.ShowDialog("PCV", TransID)
        f.Dispose()
    End Sub

    Private Sub tsbPrint_ButtonClick(sender As System.Object, e As System.EventArgs) Handles tsbPrint.ButtonClick
        
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


    Private Sub gbPayee_Enter(sender As System.Object, e As System.EventArgs) Handles gbPayee.Enter

    End Sub

    Private Sub txtVCEName_MouseCaptureChanged(sender As Object, e As System.EventArgs) Handles txtVCEName.MouseCaptureChanged

    End Sub

    Private Sub txtVCEName_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtVCEName.TextChanged

    End Sub

    Private Sub tsbDelete_Click(sender As System.Object, e As System.EventArgs) Handles tsbDelete.Click
        If Not AllowAccess("PCV_DEL") Then
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

    Private Sub FromFundsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
        frmFund_Copy.strType = "CV"
        frmFund_Copy.ShowDialog()
    End Sub

    Private Sub txtBankRefName_TextChanged(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub FromPCVToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
        Dim f As New frmLoadTransactions
        f.cbFilter.SelectedItem = "Status"
        f.txtFilter.Text = "Active"
        f.txtFilter.Enabled = False
        f.cbFilter.Enabled = False
        f.btnSearch.Enabled = False

        f.ShowDialog("RF-CV")
        LoadRF(f.transID)
        f.Dispose()
    End Sub

    Private Sub LoadRF(ByVal RF_NO As String)
        Dim query As String
        query = " SELECT TransNo, View_GL.AccntCode, AccntTitle, SUM(Credit) as Credit " & _
                " FROM View_GL " & _
                " WHERE JE_No = (SELECT DISTINCT  tblJE_Header.JE_No FROM tblJE_Header INNER JOIN tblJE_Details ON tblJE_Details.JE_No = tblJE_Header.JE_No WHERE RefType = 'RF' AND " & _
                 "tblJE_Details.RefNo LIKE '%" & RF_NO & "%')  " & _
                " AND AccntCode = '20410-0000' " & _
                " GROUP BY TransNo, View_GL.AccntCode, AccntTitle "
        SQL.ReadQuery(query)
        dgvEntry.Rows.Clear()
        While SQL.SQLDR.Read
            dgvEntry.Rows.Add(BranchCode, SQL.SQLDR("AccntCode").ToString, SQL.SQLDR("AccntTitle").ToString, CDec(SQL.SQLDR("Credit")).ToString("N2"), _
                              "0.00", "", "", _
                               "", "RF:" & SQL.SQLDR("TransNo").ToString)
        End While
        TotalDBCR()
    End Sub
    Private Sub cbBranchCode_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbBranchCode.SelectedIndexChanged
        If cbBranchCode.SelectedIndex <> -1 Then
            tempBranchCode = Strings.Left(cbBranchCode.SelectedItem, cbBranchCode.SelectedItem.ToString.IndexOf(" - "))
            'If disableEvent = False Then
            '    txtTransNum.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
            'End If
        End If
    End Sub

    Private Sub txtTransNum_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtTransNum.TextChanged

    End Sub

    Private Sub FromLoanToolStripMenuItem_Click_1(sender As System.Object, e As System.EventArgs) Handles FromLoanToolStripMenuItem.Click
        If Not AllowAccess("PCV_LOAN") Then
            msgRestricted()
        Else
            Dim f As New frmLoadTransactions
            f.cbFilter.SelectedItem = "Status"
            f.txtFilter.Text = "Approved"
            f.txtFilter.Enabled = False
            f.cbFilter.Enabled = False
            f.btnSearch.Enabled = False
            f.ShowDialog("Copy Loan PCV")
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
                    If cbBranchCode.SelectedIndex <> -1 Then
                        cashAccount = GetCashAccount(tempBranchCode)
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
                End If
                LoadBranch()
            End If
            TotalDBCR()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    Private Sub FromLoanToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub FromRFPToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub FromAdvancesToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub tsbCopyAPV_Click(sender As System.Object, e As System.EventArgs)

    End Sub
End Class