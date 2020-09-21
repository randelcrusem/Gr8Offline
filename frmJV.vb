Public Class frmJV
    Dim TransID, RefID, JETransiD As String
    Dim JVNo As String
    Dim disableEvent As Boolean = False
    Dim ModuleID As String = "JV"
    Dim ColumnPK As String = "JV_No"
    Dim DBTable As String = "tblJV"
    Dim TransAuto As Boolean
    Dim MemTransfer As Boolean
    Dim BranchTransfer As String
    Dim AccntCode As String
    Dim accntDR, accntCR, accntVAT As String
    Dim LOAN_ID As Integer
    Public interestgen As Boolean = False
    Public interestBranchCode As String = ""

    Public Overloads Function ShowDialog(ByVal docnumber As String) As Boolean
        TransID = docnumber
        MyBase.ShowDialog()
        Return True
    End Function

    Private Sub frmJV_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try
            TransAuto = GetTransSetup(ModuleID)
            dtpDocDate.Value = Date.Today.Date
            If interestgen = False Then
                If TransID <> "" Then
                    If Not AllowAccess("JV_VIEW") Then
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
                        LoadJV(TransID)
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
            End If
        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        End Try
    End Sub

    Private Sub EnableControl(ByVal Value As Boolean)
        txtVCEName.Enabled = Value
        btnSearchVCE.Enabled = Value
        dgvEntry.AllowUserToAddRows = Value
        dgvEntry.AllowUserToDeleteRows = Value
        If Value = True Then
            dgvEntry.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2
            LoadBranch()
        Else
            dgvEntry.EditMode = DataGridViewEditMode.EditProgrammatically
        End If
        txtRemarks.Enabled = Value
        dtpDocDate.Enabled = Value
        If TransAuto Then
            txtTransNum.Enabled = False
        Else
            txtTransNum.Enabled = Value
        End If
    End Sub

    Dim eColIndex As Integer = 0
    Private Sub dgvEntry_CellBeginEdit(sender As Object, e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles dgvEntry.CellBeginEdit
        eColIndex = e.ColumnIndex
    End Sub

    Private Sub dgvEntry_CellEndEdit(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvEntry.CellEndEdit
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

                'Auto Entry Grid View Cost Center
                If IsNothing(dgvEntry.Item(chCostCenter.Index, e.RowIndex).Value) Then
                    Dim cbvCostCenter As DataGridViewComboBoxCell = LoadCostCenterGridView()
                    cbvCostCenter.Value = strDefaultGridView
                    dgvEntry.Item(chCostCenter.Index, e.RowIndex) = cbvCostCenter

                    Dim dgvCostCenter As String
                    dgvCostCenter = dgvEntry.Rows(e.RowIndex).Cells(chCostCenter.Index).Value
                    LoadCostCenterCode(dgvCostCenter, e.RowIndex, chCostCenter.Index, chCostID.Index)
                End If

                'Auto Entry Grid View Cost CIP
                If IsNothing(dgvEntry.Item(chCIP_Desc.Index, e.RowIndex).Value) Then
                    Dim cbvCIP As DataGridViewComboBoxCell = LoadCIPGridView()
                    cbvCIP.Value = strDefaultGridView
                    dgvEntry.Item(chCIP_Desc.Index, e.RowIndex) = cbvCIP

                    Dim dgvCIP As String
                    dgvCIP = dgvEntry.Rows(e.RowIndex).Cells(chCIP_Desc.Index).Value
                    LoadCIPCode(dgvCIP, e.RowIndex, chCIP_Desc.Index, chCIP_Code.Index)
                End If

                ''Auto Entry RefNo

                If IsNothing(dgvEntry.Item(chVCECode.Index, e.RowIndex).Value) And IsNothing(dgvEntry.Item(chAccntCode.Index, e.RowIndex).Value) Then
                    Dim cbvRefID As DataGridViewComboBoxCell = LoadCIPGridView(e.RowIndex, chCIP_Desc.Index, chCIP_Code.Index)
                    cbvRefID.Value = strDefaultGridView
                    dgvEntry.Item(chRefNo.Index, e.RowIndex) = cbvRefID

                End If

                'Dim strVCECode As String = ""
                'Dim strAccntCode As String = ""
                'strVCECode = txtVCECode.Text
                'strAccntCode = dgvEntry.Item(chAccntCode.Index, e.RowIndex).Value
                'If dgvEntry.Item(chVCECode.Index, e.RowIndex).Value <> "" Then
                '    strVCECode = dgvEntry.Item(chVCECode.Index, e.RowIndex).Value
                'End If
                'dgvEntry.Item(chRefNo.Index, e.RowIndex).Value = GetRefNo(strVCECode, strAccntCode)
                'Dim strRefNo As String = ""
                'Dim strRefNoLoan As String = ""
                'strRefNo = dgvEntry.Item(chRefNo.Index, e.RowIndex).Value
                'strRefNoLoan = GetRefNoLoan(strRefNo)
                'If strRefNoLoan <> "" Then
                '    dgvEntry.Rows.Add("")
                '    dgvEntry.Item(chVCECode.Index, e.RowIndex + 1).Value = GetRefNoVCECode(strRefNo)
                '    dgvEntry.Item(chVCEName.Index, e.RowIndex + 1).Value = GetVCEName(dgvEntry.Item(chVCECode.Index, e.RowIndex + 1).Value)
                '    dgvEntry.Item(chAccntCode.Index, e.RowIndex + 1).Value = strRefNoLoan
                '    dgvEntry.Item(chAccntTitle.Index, e.RowIndex + 1).Value = GetAccntTitle(dgvEntry.Item(chAccntCode.Index, e.RowIndex + 1).Value)
                '    dgvEntry.Item(chRefNo.Index, e.RowIndex + 1).Value = strRefNo
                'End If
            ElseIf e.ColumnIndex = chVCECode.Index Or e.ColumnIndex = chVCEName.Index Then
                Dim f As New frmVCE_Search
                f.txtFilter.Text = dgvEntry.Item(e.ColumnIndex, e.RowIndex).Value.ToString
                f.ShowDialog()
                dgvEntry.Item(chVCECode.Index, e.RowIndex).Value = f.VCECode
                dgvEntry.Item(chVCEName.Index, e.RowIndex).Value = f.VCEName
                f.Dispose()

                'Auto Entry Grid View Cost Center
                If IsNothing(dgvEntry.Item(chCostCenter.Index, e.RowIndex).Value) Then
                    Dim cbvCostCenter As DataGridViewComboBoxCell = LoadCostCenterGridView()
                    cbvCostCenter.Value = strDefaultGridView
                    dgvEntry.Item(chCostCenter.Index, e.RowIndex) = cbvCostCenter

                    Dim dgvCostCenter As String
                    dgvCostCenter = dgvEntry.Rows(e.RowIndex).Cells(chCostCenter.Index).Value
                    LoadCostCenterCode(dgvCostCenter, e.RowIndex, chCostCenter.Index, chCostID.Index)
                End If

                'Auto Entry Grid View Cost CIP
                If IsNothing(dgvEntry.Item(chCIP_Desc.Index, e.RowIndex).Value) Then
                    Dim cbvCIP As DataGridViewComboBoxCell = LoadCIPGridView()
                    cbvCIP.Value = strDefaultGridView
                    dgvEntry.Item(chCIP_Desc.Index, e.RowIndex) = cbvCIP

                    Dim dgvCIP As String
                    dgvCIP = dgvEntry.Rows(e.RowIndex).Cells(chCIP_Desc.Index).Value
                    LoadCIPCode(dgvCIP, e.RowIndex, chCIP_Desc.Index, chCIP_Code.Index)
                End If

                ''Auto Entry RefNo
                'Dim strVCECode As String = ""
                'Dim strAccntCode As String = ""
                'strVCECode = txtVCECode.Text
                'strAccntCode = dgvEntry.Item(chAccntCode.Index, e.RowIndex).Value
                'If dgvEntry.Item(chVCECode.Index, e.RowIndex).Value <> "" Then
                '    strVCECode = dgvEntry.Item(chVCECode.Index, e.RowIndex).Value
                'End If
                'If strAccntCode <> "" Then
                '    dgvEntry.Item(chRefNo.Index, e.RowIndex).Value = GetRefNo(strVCECode, strAccntCode)
                '    Dim strRefNo As String = ""
                '    Dim strRefNoLoan As String = ""
                '    strRefNo = dgvEntry.Item(chRefNo.Index, e.RowIndex).Value
                '    strRefNoLoan = GetRefNoLoan(strRefNo)
                '    If strRefNoLoan <> "" Then
                '        dgvEntry.Rows.Add("")
                '        dgvEntry.Item(chVCECode.Index, e.RowIndex + 1).Value = GetRefNoVCECode(strRefNo)
                '        dgvEntry.Item(chVCEName.Index, e.RowIndex + 1).Value = GetVCEName(dgvEntry.Item(chVCECode.Index, e.RowIndex + 1).Value)
                '        dgvEntry.Item(chAccntCode.Index, e.RowIndex + 1).Value = strRefNoLoan
                '        dgvEntry.Item(chAccntTitle.Index, e.RowIndex + 1).Value = GetAccntTitle(dgvEntry.Item(chAccntCode.Index, e.RowIndex + 1).Value)
                '        dgvEntry.Item(chRefNo.Index, e.RowIndex + 1).Value = strRefNo
                '    End If
                'End If
            ElseIf e.ColumnIndex = chCostCenter.Index Then
                Dim dgvCostCenter As String
                dgvCostCenter = dgvEntry.Rows(e.RowIndex).Cells(chCostCenter.Index).Value
                LoadCostCenterCode(dgvCostCenter, e.RowIndex, chCostCenter.Index, chCostID.Index)


            ElseIf e.ColumnIndex = chCIP_Desc.Index Then
                Dim dgvCIP As String
                dgvCIP = dgvEntry.Rows(e.RowIndex).Cells(chCIP_Desc.Index).Value
                LoadCIPCode(dgvCIP, e.RowIndex, chCIP_Desc.Index, chCIP_Code.Index)
            ElseIf e.ColumnIndex = chRefNo.Index Then
                'Dim strRefNo As String = ""
                'strRefNo = dgvEntry.Item(chRefNo.Index, e.RowIndex).Value
                'If strRefNo <> "" Then
                '    dgvEntry.Item(chVCECode.Index, e.RowIndex).Value = GetRefNoVCECode(strRefNo)
                '    dgvEntry.Item(chVCEName.Index, e.RowIndex).Value = GetVCEName(dgvEntry.Item(chVCECode.Index, e.RowIndex).Value)
                '    dgvEntry.Item(chAccntCode.Index, e.RowIndex).Value = GetRefNoAccntCode(strRefNo)
                '    dgvEntry.Item(chAccntTitle.Index, e.RowIndex).Value = GetAccntTitle(dgvEntry.Item(chAccntCode.Index, e.RowIndex).Value)
                '    Dim strRefNoLoan As String = ""
                '    strRefNoLoan = GetRefNoLoan(strRefNo)
                '    If strRefNoLoan <> "" Then
                '        dgvEntry.Rows.Add("")
                '        dgvEntry.Item(chVCECode.Index, e.RowIndex + 1).Value = GetRefNoVCECode(strRefNo)
                '        dgvEntry.Item(chVCEName.Index, e.RowIndex + 1).Value = GetVCEName(dgvEntry.Item(chVCECode.Index, e.RowIndex + 1).Value)
                '        dgvEntry.Item(chAccntCode.Index, e.RowIndex + 1).Value = strRefNoLoan
                '        dgvEntry.Item(chAccntTitle.Index, e.RowIndex + 1).Value = GetAccntTitle(dgvEntry.Item(chAccntCode.Index, e.RowIndex + 1).Value)
                '        dgvEntry.Item(chRefNo.Index, e.RowIndex + 1).Value = strRefNo
                '    End If
                'End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    'Start of Cost Center insert to Table
    Dim strDefaultGridView As String = ""
    Dim strDefaultGridCode As String = ""
    Public Function LoadCostCenterGridView()

        Dim selectSQL As String = " SELECT Code, Description FROM tblCC"
        SQL.ReadQuery(selectSQL, 2)

        Dim cbvGridviewCell As New DataGridViewComboBoxCell

        Dim count As Integer = 1
        cbvGridviewCell.Items.Add("")

        While SQL.SQLDR2.Read
            If count = 1 Then
                strDefaultGridCode = SQL.SQLDR2("Code").ToString
                strDefaultGridView = SQL.SQLDR2("Description").ToString
            End If
            cbvGridviewCell.Items.Add(SQL.SQLDR2("Description").ToString)
            count += 1
        End While
        strDefaultGridView = ""
        Return cbvGridviewCell

    End Function


    Public Function LoadCIPGridView()

        Dim selectSQL As String = " SELECT CIP_Code, CIP_Desc FROM tblCIP_Maintenance"
        SQL.ReadQuery(selectSQL, 2)

        Dim cbvGridviewCell As New DataGridViewComboBoxCell

        Dim count As Integer = 1
        cbvGridviewCell.Items.Add("")
        While SQL.SQLDR2.Read
            If count = 1 Then
                strDefaultGridCode = SQL.SQLDR2("CIP_Code").ToString
                strDefaultGridView = SQL.SQLDR2("CIP_Desc").ToString
            End If
            cbvGridviewCell.Items.Add(SQL.SQLDR2("CIP_Desc").ToString)
            count += 1
        End While
        strDefaultGridView = ""
        Return cbvGridviewCell

    End Function

    Public Function LoadREFCode(ByVal RowIndex As Integer, ByVal CodeIndex As Integer, ByVal CIPIndex As Integer)


        Dim selectSQL As String = " SELECT CIP_Code, CIP_Desc FROM tblCIP_Maintenance"
        SQL.ReadQuery(selectSQL, 2)

        Dim cbvGridviewCell As New DataGridViewComboBoxCell

        Dim count As Integer = 1
        cbvGridviewCell.Items.Add("")
        While SQL.SQLDR2.Read
            If count = 1 Then
                strDefaultGridCode = SQL.SQLDR2("CIP_Code").ToString
                strDefaultGridView = SQL.SQLDR2("CIP_Desc").ToString
            End If
            cbvGridviewCell.Items.Add(SQL.SQLDR2("CIP_Desc").ToString)
            count += 1
        End While
        strDefaultGridView = ""
        Return cbvGridviewCell

    End Function

    Public Sub LoadCostCenterCode(ByVal CostCenter As String, ByVal RowIndex As Integer, ByVal CodeIndex As Integer, ByVal CostIndex As Integer)

        Dim selectSQL As String
        selectSQL = " SELECT Code, Description FROM tblCC WHERE Description = '" & CostCenter & "'"
        SQL.ReadQuery(selectSQL, 2)

        strDefaultGridView = ""
        strDefaultGridCode = ""

        While SQL.SQLDR2.Read
            strDefaultGridView = SQL.SQLDR2("Description").ToString
            strDefaultGridCode = SQL.SQLDR2("Code").ToString
        End While
        dgvEntry.Rows(RowIndex).Cells(CodeIndex).Value = strDefaultGridView
        dgvEntry.Rows(RowIndex).Cells(CostIndex).Value = strDefaultGridCode

    End Sub

    Public Sub LoadCIPCode(ByVal CIP As String, ByVal RowIndex As Integer, ByVal CodeIndex As Integer, ByVal CIPIndex As Integer)

        Dim selectSQL As String
        selectSQL = " SELECT CIP_Code, CIP_Desc FROM tblCIP_Maintenance WHERE CIP_Desc = '" & CIP & "'"
        SQL.ReadQuery(selectSQL, 2)

        strDefaultGridView = ""
        strDefaultGridCode = ""

        While SQL.SQLDR2.Read
            strDefaultGridView = SQL.SQLDR2("CIP_Desc").ToString
            strDefaultGridCode = SQL.SQLDR2("CIP_Code").ToString
        End While
        dgvEntry.Rows(RowIndex).Cells(CIPIndex).Value = strDefaultGridView
        dgvEntry.Rows(RowIndex).Cells(CIPIndex).Value = strDefaultGridCode

    End Sub

    Public Function GetCIPName(ByVal CCCode As String) As String
        Dim query As String
        query = " SELECT CIP_Desc FROM tblCIP_Maintenance WHERE CIP_Code ='" & CCCode & "' "
        SQL.ReadQuery(query, 2)
        If SQL.SQLDR2.Read Then
            Return SQL.SQLDR2("CIP_Desc").ToString
        Else
            Return ""
        End If
    End Function

    Public Function GetCCName(ByVal CCCode As String) As String
        Dim query As String
        query = " SELECT Description FROM tblCC WHERE Code ='" & CCCode & "' "
        SQL.ReadQuery(query, 2)
        If SQL.SQLDR2.Read Then
            Return SQL.SQLDR2("Description").ToString
        Else
            Return ""
        End If
    End Function

    Public Sub TotalDBCR()
        ' COMPUTE TOTAL DEBIT
        Dim db As Decimal = 0

        For i As Integer = 0 To dgvEntry.Rows.Count - 1
            If Val(dgvEntry.Item(chDebit.Index, i).Value) <> 0 Then
                db = db + Double.Parse(dgvEntry.Item(chDebit.Index, i).Value).ToString("N2")
            End If
        Next
        txtDebit.Text = db.ToString("N2")

        ' COMPUTE TOTAL CREDIT
        Dim b As Decimal = 0

        For i As Integer = 0 To dgvEntry.Rows.Count - 1
            If Val(dgvEntry.Item(chCredit.Index, i).Value) <> 0 Then
                b = b + Double.Parse(dgvEntry.Item(chCredit.Index, i).Value).ToString("N2")
            End If
        Next
        txtCredit.Text = b.ToString("N2")


        txtDifference.Text = CDec(txtDebit.Text - txtCredit.Text).ToString("N2")
    End Sub

    Private Sub dgvEntry_RowsRemoved(sender As System.Object, e As System.Windows.Forms.DataGridViewRowsRemovedEventArgs) Handles dgvEntry.RowsRemoved
        Try
            TotalDBCR()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    Private Sub ClearText()
        txtVCECode.Text = ""
        txtVCEName.Text = ""
        txtCredit.Text = "0.00"
        txtDebit.Text = "0.00"
        txtDifference.Text = "0.00"
        txtTransNum.Clear()
        txtRemarks.Clear()
        txtStatus.Clear()
        dgvEntry.Rows.Clear()
        interestgen = False
        interestBranchCode = ""
        MemTransfer = False
        BranchTransfer = ""
    End Sub


    Private Sub SaveJV()
        Try
            Dim insertSQL As String
            activityStatus = True
            insertSQL = " INSERT INTO " & _
                        " tblJV (TransID, JV_No, VCECode, BranchCode, BusinessCode, DateJV, TotalAmount, Remarks, LN_Ref, TransAuto, WhoCreated, MemTransfer) " & _
                        " VALUES(@TransID, @JV_No, @VCECode, @BranchCode, @BusinessCode, @DateJV, @TotalAmount, @Remarks, @LN_Ref, @TransAuto, @WhoCreated, @MemTransfer)"
            SQL.FlushParams()
            SQL.AddParam("@TransID", TransID)
            SQL.AddParam("@JV_No", JVNo)
            SQL.AddParam("@VCECode", txtVCECode.Text)
            SQL.AddParam("@BranchCode", IIf(interestBranchCode <> "", interestBranchCode, BranchCode))
            SQL.AddParam("@BusinessCode", BusinessType)
            SQL.AddParam("@DateJV", dtpDocDate.Value.Date)
            SQL.AddParam("@LN_Ref", LOAN_ID)
            If IsNumeric(txtDebit.Text) Then
                SQL.AddParam("@TotalAmount", CDec(txtDebit.Text))
            Else
                SQL.AddParam("@TotalAmount", 0)
            End If
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@TransAuto", TransAuto)
            SQL.AddParam("@MemTransfer", MemTransfer)
            SQL.AddParam("@WhoCreated", UserID)
            SQL.ExecNonQuery(insertSQL)

            If LOAN_ID > 0 Then
                Dim updateSQL As String
                updateSQL = " UPDATE tblLoan SET Status ='Released', DateRelease = '" & dtpDocDate.Value.Date & "', RefType = 'JV', RefTransID = '" & TransID & "' WHERE TransID = '" & LOAN_ID & "'"
                SQL.ExecNonQuery(updateSQL)
            End If

          

            insertSQL = " INSERT INTO " & _
                        " tblJE_Header (AppDate, BranchCode, BusinessCode, RefType, RefTransID, Book, TotalDBCR, Remarks, WhoCreated) " & _
                        " VALUES(@AppDate, @BranchCode, @BusinessCode, @RefType, @RefTransID, @Book, @TotalDBCR, @Remarks, @WhoCreated)"
            SQL.FlushParams()
            SQL.AddParam("@AppDate", dtpDocDate.Value.Date)
            SQL.AddParam("@RefType", "JV")
            SQL.AddParam("@RefTransID", TransID)
            SQL.AddParam("@Book", "General Journal")
            SQL.AddParam("@TotalDBCR", CDec(txtDebit.Text))
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@BranchCode", IIf(interestBranchCode <> "", interestBranchCode, BranchCode))
            SQL.AddParam("@BusinessCode", BusinessType)
            SQL.AddParam("@WhoCreated", UserID)
            SQL.ExecNonQuery(insertSQL)

            JETransiD = LoadJE("JV", TransID)

            Dim strRefNo As String = ""
            Dim VCECode As String = ""

            Dim line As Integer = 1
            For Each item As DataGridViewRow In dgvEntry.Rows
                If item.Cells(chAccntCode.Index).Value <> Nothing Then
                    insertSQL = " INSERT INTO " & _
                                " tblJE_Details(JE_No, AccntCode, VCECode, Debit, Credit, Particulars, RefNo, LineNumber, CostCenter, CIP_Code, BranchCode) " & _
                                " VALUES(@JE_No, @AccntCode, @VCECode, @Debit, @Credit, @Particulars, @RefNo, @LineNumber, @CostCenter, @CIP_Code, @BranchCode)"
                    SQL.FlushParams()
                    SQL.AddParam("@JE_No", JETransiD)
                    SQL.AddParam("@AccntCode", item.Cells(chAccntCode.Index).Value.ToString)
                    If item.Cells(chVCECode.Index).Value <> Nothing AndAlso item.Cells(chVCECode.Index).Value <> "" Then
                        SQL.AddParam("@VCECode", item.Cells(chVCECode.Index).Value.ToString)
                        VCECode = item.Cells(chVCECode.Index).Value.ToString
                    Else
                        SQL.AddParam("@VCECode", "")
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
                    If item.Cells(chCostID.Index).Value <> Nothing AndAlso item.Cells(chCostID.Index).Value <> "" Then
                        SQL.AddParam("@CostCenter", item.Cells(chCostID.Index).Value.ToString)
                    Else
                        SQL.AddParam("@CostCenter", "")
                    End If
                    If item.Cells(chCIP_Code.Index).Value <> Nothing AndAlso item.Cells(chCIP_Code.Index).Value <> "" Then
                        SQL.AddParam("@CIP_Code", item.Cells(chCIP_Code.Index).Value.ToString)
                    Else
                        SQL.AddParam("@CIP_Code", "")
                    End If
                    If item.Cells(chRefNo.Index).Value <> Nothing AndAlso item.Cells(chRefNo.Index).Value <> "" Then
                        SQL.AddParam("@RefNo", item.Cells(chRefNo.Index).Value.ToString)
                        If strRefNo.Length = 0 Then
                            strRefNo = item.Cells(chRefNo.Index).Value.ToString
                        Else
                            strRefNo = strRefNo & "-" & item.Cells(chRefNo.Index).Value.ToString
                        End If
                    Else
                        SQL.AddParam("@RefNo", "")
                    End If
                    SQL.AddParam("@LineNumber", line)
                    If item.Cells(chBranchCode.Index).Value <> Nothing AndAlso item.Cells(chBranchCode.Index).Value <> "" Then
                        SQL.AddParam("@BranchCode", item.Cells(chBranchCode.Index).Value.ToString)
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
                        updateSQL = " UPDATE tblLoan SET Status ='Released', DateRelease = '" & dtpDocDate.Value.Date & "', RefType = 'JV', RefTransID = '" & TransID & "' WHERE Loan_No = '" & strRefNo.Split("-")(i) & "'"
                        SQL.ExecNonQuery(updateSQL)
                    End If
                Next
            End If

            If MemTransfer = True Then
                Dim updateSQL As String
                updateSQL = " UPDATE tblMember_Master SET BranchCode ='" & BranchTransfer & "' WHERE Member_ID = '" & VCECode & "'"
                SQL.ExecNonQuery(updateSQL)
            End If
        Catch ex As Exception
            activityStatus = False
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        Finally
            RecordActivity(UserID, ModuleID, Me.Name.ToString, "INSERT", "JV_No", txtTransNum.Text, BusinessType, IIf(interestBranchCode <> "", interestBranchCode, BranchCode), "", activityStatus)
            SQL.FlushParams()
        End Try
    End Sub

    Private Sub UpdateJV()
        Try
            Dim insertSQL, updateSQL, deleteSQL As String
            activityStatus = True
            updateSQL = " UPDATE tblJV " & _
                        " SET    JV_No = @JV_No, BranchCode = @BranchCode, BusinessCode = @BusinessCode,  VCECode = @VCECode, DateJV = @DateJV, " & _
                        "        TotalAmount = @TotalAmount, Remarks = @Remarks,  LN_Ref = @LN_Ref, WhoModified = @WhoModified, DateModified = GETDATE(), MemTransfer = @MemTransfer " & _
                        " WHERE  TransID = @TransID "
            SQL.FlushParams()
            SQL.AddParam("@TransID", TransID)
            SQL.AddParam("@JV_No", JVNo)
            SQL.AddParam("@BranchCode", IIf(interestBranchCode <> "", interestBranchCode, BranchCode))
            SQL.AddParam("@BusinessCode", BusinessType)
            SQL.AddParam("@VCECode", txtVCECode.Text)
            SQL.AddParam("@DateJV", dtpDocDate.Value.Date)
            SQL.AddParam("@LN_Ref", LOAN_ID)
            If IsNumeric(txtDebit.Text) Then
                SQL.AddParam("@TotalAmount", CDec(txtDebit.Text))
            Else
                SQL.AddParam("@TotalAmount", 0)
            End If
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@MemTransfer", MemTransfer)
            SQL.AddParam("@WhoModified", UserID)
            SQL.ExecNonQuery(updateSQL)

            If LOAN_ID > 0 Then
                updateSQL = " UPDATE tblLoan SET Status ='Released', DateRelease = '" & dtpDocDate.Value.Date & "', RefType = 'JV', RefTransID = '" & TransID & "' WHERE TransID = '" & LOAN_ID & "'"
                SQL.ExecNonQuery(updateSQL)
            End If

            JETransiD = LoadJE("JV", TransID)

            If JETransiD = 0 Then
                insertSQL = " INSERT INTO " & _
                            " tblJE_Header (AppDate, BranchCode, BusinessCode, RefType, RefTransID, Book, TotalDBCR, Remarks, WhoCreated) " & _
                            " VALUES(@AppDate, @BranchCode, @BusinessCode, @RefType, @RefTransID, @Book, @TotalDBCR, @Remarks, @WhoCreated)"
                SQL.FlushParams()
                SQL.AddParam("@AppDate", dtpDocDate.Value.Date)
                SQL.AddParam("@RefType", "JV")
                SQL.AddParam("@RefTransID", TransID)
                SQL.AddParam("@Book", "General Journal")
                SQL.AddParam("@TotalDBCR", CDec(txtCredit.Text))
                SQL.AddParam("@Remarks", txtRemarks.Text)
                SQL.AddParam("@BranchCode", IIf(interestBranchCode <> "", interestBranchCode, BranchCode))
                SQL.AddParam("@BusinessCode", BusinessType)
                SQL.AddParam("@WhoCreated", UserID)
                SQL.ExecNonQuery(insertSQL)

                JETransiD = LoadJE("JV", TransID)
            Else
                updateSQL = " UPDATE tblJE_Header " & _
                            " SET    AppDate = @AppDate, BranchCode = @BranchCode, BusinessCode = @BusinessCode, " & _
                            "        RefType = @RefType, RefTransID = @RefTransID, Book = @Book, TotalDBCR = @TotalDBCR, " & _
                            "        Remarks = @Remarks, WhoModified = @WhoModified, DateModified = GETDATE() " & _
                            " WHERE  JE_No = @JE_No "
                SQL.FlushParams()
                SQL.AddParam("@JE_No", JETransiD)
                SQL.AddParam("@AppDate", dtpDocDate.Value.Date)
                SQL.AddParam("@RefType", "JV")
                SQL.AddParam("@RefTransID", TransID)
                SQL.AddParam("@Book", "General Journal")
                SQL.AddParam("@TotalDBCR", CDec(txtCredit.Text))
                SQL.AddParam("@Remarks", txtRemarks.Text)
                SQL.AddParam("@BranchCode", IIf(interestBranchCode <> "", interestBranchCode, BranchCode))
                SQL.AddParam("@BusinessCode", BusinessType)
                SQL.AddParam("@WhoModified", UserID)
                SQL.ExecNonQuery(updateSQL)
            End If

            Dim line As Integer = 1

            ' DELETE ACCOUNTING ENTRIES
            deleteSQL = " DELETE FROM tblJE_Details  WHERE  JE_No = @JE_No "
            SQL.FlushParams()
            SQL.AddParam("@JE_No", JETransiD)
            SQL.ExecNonQuery(deleteSQL)

            Dim strRefNo As String = ""

            ' INSERT NEW ENTRIES
            For Each item As DataGridViewRow In dgvEntry.Rows
                If item.Cells(chAccntCode.Index).Value <> Nothing Then
                    insertSQL = " INSERT INTO " & _
                                " tblJE_Details(JE_No, AccntCode, VCECode, Debit, Credit, Particulars, RefNo, LineNumber, CostCenter, CIP_Code, BranchCode) " & _
                                " VALUES(@JE_No, @AccntCode, @VCECode, @Debit, @Credit, @Particulars, @RefNo, @LineNumber, @CostCenter, @CIP_Code, @BranchCode)"
                    SQL.FlushParams()
                    SQL.AddParam("@JE_No", JETransiD)
                    SQL.AddParam("@AccntCode", item.Cells(chAccntCode.Index).Value.ToString)
                    If item.Cells(chVCECode.Index).Value <> Nothing AndAlso item.Cells(chVCECode.Index).Value <> "" Then
                        SQL.AddParam("@VCECode", item.Cells(chVCECode.Index).Value.ToString)
                    Else
                        SQL.AddParam("@VCECode", "")
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
                    If item.Cells(chCostID.Index).Value <> Nothing AndAlso item.Cells(chCostID.Index).Value <> "" Then
                        SQL.AddParam("@CostCenter", item.Cells(chCostID.Index).Value.ToString)
                    Else
                        SQL.AddParam("@CostCenter", "")
                    End If
                    If item.Cells(chCIP_Code.Index).Value <> Nothing AndAlso item.Cells(chCIP_Code.Index).Value <> "" Then
                        SQL.AddParam("@CIP_Code", item.Cells(chCIP_Code.Index).Value.ToString)
                    Else
                        SQL.AddParam("@CIP_Code", "")
                    End If
                    If item.Cells(chRefNo.Index).Value <> Nothing AndAlso item.Cells(chRefNo.Index).Value <> "" Then
                        SQL.AddParam("@RefNo", item.Cells(chRefNo.Index).Value.ToString)
                        If strRefNo.Length = 0 Then
                            strRefNo = item.Cells(chRefNo.Index).Value.ToString
                        Else
                            strRefNo = strRefNo & "-" & item.Cells(chRefNo.Index).Value.ToString
                        End If
                    Else
                        SQL.AddParam("@RefNo", "")
                    End If
                    SQL.AddParam("@LineNumber", line)
                    If item.Cells(chBranchCode.Index).Value <> Nothing AndAlso item.Cells(chBranchCode.Index).Value <> "" Then
                        SQL.AddParam("@BranchCode", item.Cells(chBranchCode.Index).Value.ToString)
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
                        updateSQL1 = " UPDATE tblLoan SET Status ='Released', DateRelease = '" & dtpDocDate.Value.Date & "', RefType = 'JV', RefTransID = '" & TransID & "' WHERE Loan_No = '" & strRefNo.Split("-")(i) & "'"
                        SQL.ExecNonQuery(updateSQL1)
                    End If
                Next
            End If
        Catch ex As Exception
            activityStatus = False
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        Finally
            RecordActivity(UserID, ModuleID, Me.Name.ToString, "UPDATE", "JV_No", txtTransNum.Text, BusinessType, IIf(interestBranchCode <> "", interestBranchCode, BranchCode), "", activityStatus)
            SQL.FlushParams()
        End Try
    End Sub

    Private Sub LoadJV(ByVal ID As String)
        Dim query As String
        query = " SELECT  TransID, JV_No, tblJV.VCECode, VCEName, DateJV, TotalAmount, Remarks, ISNULL(LN_Ref,0) as LN_Ref, tblJV.Status, MemTransfer  " & _
                " FROM    tblJV LEFT JOIN tblVCE_Master " & _
                " ON      tblJV.VCECode = tblVCE_Master.VCECode " & _
                " WHERE   TransID ='" & ID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            TransID = SQL.SQLDR("TransID").ToString
            JVNo = SQL.SQLDR("JV_No").ToString
            LOAN_ID = SQL.SQLDR("LN_Ref").ToString
            txtTransNum.Text = JVNo
            txtVCECode.Text = SQL.SQLDR("VCECode").ToString
            txtVCEName.Text = SQL.SQLDR("VCEName").ToString
            disableEvent = True
            dtpDocDate.Value = SQL.SQLDR("DateJV")
            disableEvent = False
            txtRemarks.Text = SQL.SQLDR("Remarks").ToString
            txtStatus.Text = SQL.SQLDR("Status").ToString
            MemTransfer = SQL.SQLDR("MemTransfer").ToString
            txtLoanRef.Text = GetLoanNo(LOAN_ID)
            LoadAccountingEntry(TransID)

            tsbCancel.Text = "Cancel"
            ' TOOLSTRIP BUTTONS
            If txtStatus.Text = "Cancelled" Then
                tsbEdit.Enabled = False
                tsbCancel.Text = "Un-Can"
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

    Private Function GetLoanNo(ByVal ID As Integer) As String
        Dim query As String
        query = " SELECT Loan_No FROM tblLoan WHERE TransID = '" & ID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return SQL.SQLDR("Loan_No")
        Else
            Return 0
        End If
    End Function

    Private Sub LoadAccountingEntry(ByVal TransID As Integer)
        Try
            Dim query As String
            query = " SELECT JE_No, VCECode, VCEName, AccntCode, AccntTitle, Particulars, RefNo," & _
                    "        ISNULL(Debit,0) AS Debit, ISNULL(Credit,0) AS Credit, CostCenter, CIP_Code, BranchCode " & _
                    " FROM   View_GL  " & _
                    " WHERE  RefType ='JV' AND RefTransID ='" & TransID & "' "
            SQL.ReadQuery(query)
            dgvEntry.Rows.Clear()
            Dim rowsCount As Integer = 0
            If SQL.SQLDR.HasRows Then

                While SQL.SQLDR.Read

                    JETransiD = SQL.SQLDR("JE_No").ToString
                    dgvEntry.Rows.Add(SQL.SQLDR("AccntCode").ToString)
                    dgvEntry.Rows(rowsCount).Cells(chAccntTitle.Index).Value = SQL.SQLDR("AccntTitle").ToString
                    dgvEntry.Rows(rowsCount).Cells(chDebit.Index).Value = CDec(SQL.SQLDR("Debit")).ToString("N2")
                    dgvEntry.Rows(rowsCount).Cells(chCredit.Index).Value = CDec(SQL.SQLDR("Credit")).ToString("N2")
                    dgvEntry.Rows(rowsCount).Cells(chVCECode.Index).Value = SQL.SQLDR("VCECode").ToString
                    dgvEntry.Rows(rowsCount).Cells(chVCEName.Index).Value = SQL.SQLDR("VCEName").ToString
                    dgvEntry.Rows(rowsCount).Cells(chParticulars.Index).Value = SQL.SQLDR("Particulars").ToString
                    dgvEntry.Rows(rowsCount).Cells(chRefNo.Index).Value = SQL.SQLDR("RefNo").ToString
                    dgvEntry.Rows(rowsCount).Cells(chBranchCode.Index).Value = SQL.SQLDR("BranchCode").ToString

                    'CostCenter
                    Dim strCCCode As String = SQL.SQLDR("CostCenter").ToString
                    Dim cbvCostCenter As DataGridViewComboBoxCell = LoadCostCenterGridView()
                    Dim strCostCenter As String = GetCCName(strCCCode)
                    If cbvCostCenter.Items.Contains(IIf(IsNothing(strCostCenter), "", strCostCenter)) Then
                        cbvCostCenter.Value = strCostCenter
                    End If
                    dgvEntry.Rows(rowsCount).Cells(chCostCenter.Index) = cbvCostCenter
                    dgvEntry.Rows(rowsCount).Cells(chCostID.Index).Value = SQL.SQLDR("CostCenter").ToString

                    'CIP
                    Dim strCIP_Code As String = SQL.SQLDR("CIP_Code").ToString
                    Dim cbvCIP As DataGridViewComboBoxCell = LoadCIPGridView()
                    Dim strCIP As String = GetCIPName(strCIP_Code)
                    If cbvCIP.Items.Contains(IIf(IsNothing(strCIP), "", strCIP)) Then
                        cbvCIP.Value = strCIP
                    End If
                    dgvEntry.Rows(rowsCount).Cells(chCIP_Desc.Index) = cbvCIP
                    dgvEntry.Rows(rowsCount).Cells(chCIP_Code.Index).Value = SQL.SQLDR("CIP_Code").ToString

                    rowsCount += 1
                End While

                LoadBranch()
                TotalDBCR()
            Else
                JETransiD = 0
                dgvEntry.Rows.Clear()
            End If
        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        End Try
    End Sub

    Private Sub tsbSearch_Click(sender As System.Object, e As System.EventArgs) Handles tsbSearch.Click
        If Not AllowAccess("JV_VIEW") Then
            msgRestricted()
        Else
            Dim f As New frmLoadTransactions
            f.ShowDialog("JV")
            If f.transID <> "" Then
                TransID = f.transID
            End If
            LoadJV(TransID)
            f.Dispose()
        End If
    End Sub

    Private Sub tsbNew_Click(sender As System.Object, e As System.EventArgs) Handles tsbNew.Click
        If Not AllowAccess("JV_ADD") Then
            msgRestricted()
        Else
            ClearText()
            TransID = ""
            JVNo = ""

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
            EnableControl(True)

            txtTransNum.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
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
                                " WHERE     " & ColumnPK & " LIKE '" & prefix & "%'   AND TransAuto = 1 AND BranchCode = '" & IIf(interestBranchCode <> "", interestBranchCode, BranchCode) & "'"
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
                                    " WHERE     " & ColumnPK & " = '" & TransNum & "' AND BranchCode = '" & IIf(interestBranchCode <> "", interestBranchCode, BranchCode) & "'"
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
        If Not AllowAccess("JV_EDIT") Then
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
        If txtRemarks.Text = "" Then
            MsgBox("Please enter a remark/short explanation", MsgBoxStyle.Exclamation)
            'ElseIf txtVCECode.Text = "" Then
            '    Msg("Please enter VCECode!", MsgBoxStyle.Exclamation)
        ElseIf CDec(txtDebit.Text) <> CDec(txtCredit.Text) Then
            MsgBox("Please check the Debit and Credit Amount!", MsgBoxStyle.Exclamation)
        ElseIf dgvEntry.Rows.Count = 0 Then
            MsgBox("No entries, Please check!", MsgBoxStyle.Exclamation)
        ElseIf TransAuto = False AndAlso IfExist(txtTransNum.Text) And TransID = "" Then
            MsgBox("JV" & " " & txtTransNum.Text & " already exist!", MsgBoxStyle.Exclamation)
        ElseIf TransID = "" Then
            If MsgBox("Saving New Record, Click Yes to confirm", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "GR8 Message Alert") = MsgBoxResult.Yes Then
                TransID = GenerateTransID("TransID", DBTable)
                If TransAuto Then
                    JVNo = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
                Else
                    JVNo = txtTransNum.Text
                End If
                txtTransNum.Text = JVNo
                SaveJV()
                Msg("Record Saved Succesfully!", MsgBoxStyle.Information)
                LoadJV(TransID)
            End If
        Else
            If MsgBox("Updating Record, Click Yes to confirm", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "GR8 Message Alert") = MsgBoxResult.Yes Then
                If JVNo = txtTransNum.Text Then
                    JVNo = txtTransNum.Text
                    UpdateJV()
                    Msg("Record Updated Succesfully!", MsgBoxStyle.Information)
                    JVNo = txtTransNum.Text
                    LoadJV(TransID)
                Else
                    If Not IfExist(txtTransNum.Text) Then
                        JVNo = txtTransNum.Text
                        UpdateJV()
                        Msg("Record Updated Succesfully!", MsgBoxStyle.Information)
                        JVNo = txtTransNum.Text
                        LoadJV(TransID)
                    Else
                        MsgBox("JV" & " " & txtTransNum.Text & " already exist!", MsgBoxStyle.Exclamation)
                    End If
                End If

            End If
        End If
    End Sub

    Private Function IfExist(ByVal ID As String) As Boolean
        Dim query As String
        query = " SELECT * FROM tblJV WHERE JV_No ='" & ID & "'  "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub tsbCancel_Click(sender As System.Object, e As System.EventArgs) Handles tsbCancel.Click
        If Not AllowAccess("JV_DEL") Then
            msgRestricted()
        Else
            If txtTransNum.Text <> "" Then
                If txtStatus.Text <> "Cancelled" AndAlso MsgBox("Are you sure you want to cancel this record?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "GR8 Message Alert") = MsgBoxResult.Yes Then
                    Try
                        activityStatus = True
                        Dim updateSQL As String
                        updateSQL = " UPDATE  tblJV SET Status ='Cancelled' WHERE TransID = @TransID "
                        SQL.FlushParams()
                        SQL.AddParam("@TransID", TransID)
                        SQL.ExecNonQuery(updateSQL)

                        updateSQL = " UPDATE  tblJE_Header SET Status ='Cancelled' WHERE RefTransID = @RefTransID  AND RefType ='JV' "
                        SQL.FlushParams()
                        SQL.AddParam("@RefTransID", TransID)
                        SQL.ExecNonQuery(updateSQL)

                        Dim insertSQL As String
                        insertSQL = " INSERT INTO tblJE_Details(JE_No, AccntCode, VCECode, Debit, Credit, Particulars, RefNo, LineNumber, OtherRef) " & _
                                    " SELECT JE_No, AccntCode, VCECode, Credit, Debit, Particulars, RefNo, 0 AS LineNumber, OtherRef FROM tblJE_Details " & _
                                    " WHERE JE_No IN (SELECT DISTINCT JE_No FROM tblJE_Header WHERE RefType ='JV' AND RefTransID ='" & TransID & "') "
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
                        tsbCopy.Enabled = False
                        EnableControl(False)

                        JVNo = txtTransNum.Text
                        LoadJV(TransID)
                    Catch ex As Exception
                        activityStatus = True
                        SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
                    Finally
                        RecordActivity(UserID, ModuleID, Me.Name.ToString, "CANCEL", "APV_No", JVNo, BusinessType, IIf(interestBranchCode <> "", interestBranchCode, BranchCode), "", activityStatus)
                        SQL.FlushParams()
                    End Try
                ElseIf txtStatus.Text = "Cancelled" AndAlso MsgBox("Are you sure you want to un-cancel this record?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "GR8 Message Alert") = MsgBoxResult.Yes Then
                    Try
                        activityStatus = True
                        Dim updateSQL As String
                        updateSQL = " UPDATE  tblJV SET Status ='Active' WHERE TransID = @TransID "
                        SQL.FlushParams()
                        SQL.AddParam("@TransID", TransID)
                        SQL.ExecNonQuery(updateSQL)

                        updateSQL = " UPDATE  tblJE_Header SET Status ='Saved' WHERE RefTransID = @RefTransID  AND RefType ='JV' "
                        SQL.FlushParams()
                        SQL.AddParam("@RefTransID", TransID)
                        SQL.ExecNonQuery(updateSQL)

                        Dim insertSQL As String
                        insertSQL = " DELETE FROM tblJE_Details " & _
                                    " WHERE JE_No IN (SELECT DISTINCT JE_No FROM tblJE_Header WHERE RefType ='JV' AND RefTransID ='" & TransID & "') AND LineNumber = 0 "
                        SQL.ExecNonQuery(insertSQL)
                        Msg("Record cancelled successfully", MsgBoxStyle.Information)


                        If LOAN_ID > 0 Then
                            updateSQL = " UPDATE tblLoan SET Status ='Released', DateRelease = '" & dtpDocDate.Value & "', RefType = 'JV', RefTransID = '" & TransID & "' WHERE TransID = '" & LOAN_ID & "'"
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

                        JVNo = txtTransNum.Text
                        LoadJV(TransID)
                    Catch ex As Exception
                        activityStatus = True
                        SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
                    Finally
                        RecordActivity(UserID, ModuleID, Me.Name.ToString, "CANCEL", "APV_No", JVNo, BusinessType, IIf(interestBranchCode <> "", interestBranchCode, BranchCode), "", activityStatus)
                        SQL.FlushParams()
                    End Try
                End If
            End If
        End If
    End Sub

    Private Sub tsbPrint_Click(sender As System.Object, e As System.EventArgs) Handles tsbPrint.Click
        Dim f As New frmReport_Display
        f.ShowDialog("JV", TransID)
        f.Dispose()
    End Sub

    Private Sub tsbPrevious_Click(sender As System.Object, e As System.EventArgs) Handles tsbPrevious.Click
        If JVNo <> "" Then
            Dim query As String
            query = "   SELECT Top 1 TransID FROM tblJV  " & _
                    "   INNER JOIN tblBranch  ON	          " & _
                    "   tblJV.BranchCode = tblBranch.BranchCode    " & _
                    "     WHERE          " & _
                    " 	( tblJV.BranchCode IN  " & _
                    " 	  (SELECT    DISTINCT tblBranch.BranchCode  " & _
                    " 	  FROM      tblBranch   " & _
                    " 	  INNER JOIN tblUser_Access    ON          " & _
                    " 	  tblBranch.BranchCode = tblUser_Access.Code   " & _
                    " 	   AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                    " 	   AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                    " 	   WHERE     UserID ='" & UserID & "' " & _
                    " 	   ) " & _
                    "     )   " & _
                    "    AND JV_No < '" & JVNo & "' ORDER BY JV_No DESC  "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                TransID = SQL.SQLDR("TransID").ToString
                LoadJV(TransID)
            Else
                Msg("Reached the beginning of record!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub

    Private Sub tsbNext_Click(sender As System.Object, e As System.EventArgs) Handles tsbNext.Click
        If JVNo <> "" Then
            Dim query As String
            query = "   SELECT Top 1 TransID FROM tblJV  " & _
                    "   INNER JOIN tblBranch  ON	          " & _
                    "   tblJV.BranchCode = tblBranch.BranchCode    " & _
                    "     WHERE          " & _
                    " 	( tblJV.BranchCode IN  " & _
                    " 	  (SELECT    DISTINCT tblBranch.BranchCode  " & _
                    " 	  FROM      tblBranch   " & _
                    " 	  INNER JOIN tblUser_Access    ON          " & _
                    " 	  tblBranch.BranchCode = tblUser_Access.Code   " & _
                    " 	   AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                    " 	   AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                    " 	   WHERE     UserID ='" & UserID & "' " & _
                    " 	   ) " & _
                    "     )   " & _
                    "    AND JV_No > '" & JVNo & "' ORDER BY JV_No ASC  "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                TransID = SQL.SQLDR("TransID").ToString
                LoadJV(TransID)
            Else
                Msg("Reached the end of record!", MsgBoxStyle.Exclamation)
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
            LoadJV(TransID)
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

    Private Sub tsbExit_Click(sender As System.Object, e As System.EventArgs) Handles tsbExit.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub frmJV_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
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
            ElseIf e.KeyCode = Keys.C Then
                If tsbCopy.Enabled = True Then tsbCopy.ShowDropDown()
            ElseIf e.KeyCode = Keys.Left Then
                If tsbPrevious.Enabled = True Then tsbPrevious.PerformClick()
            ElseIf e.KeyCode = Keys.Right Then
                If tsbNext.Enabled = True Then tsbNext.PerformClick()
            ElseIf e.KeyCode = Keys.P Then
                If tsbPrint.Enabled = True Then tsbPrint.PerformClick()
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

    Private Sub dgvEntry_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvEntry.CellContentClick

    End Sub

    Private Sub dgvEntry_CurrentCellDirtyStateChanged(sender As Object, e As System.EventArgs) Handles dgvEntry.CurrentCellDirtyStateChanged
        If eColIndex = chCostCenter.Index And TypeOf (dgvEntry.CurrentRow.Cells(chCostCenter.Index)) Is DataGridViewComboBoxCell Then
            dgvEntry.EndEdit()
        ElseIf eColIndex = chCIP_Desc.Index And TypeOf (dgvEntry.CurrentRow.Cells(chCIP_Desc.Index)) Is DataGridViewComboBoxCell Then
            dgvEntry.EndEdit()
        End If
    End Sub

    Private Sub btnSearchVCE_Click(sender As System.Object, e As System.EventArgs) Handles btnSearchVCE.Click
        Dim f As New frmVCE_Search
        f.ShowDialog()
        txtVCECode.Text = f.VCECode
        txtVCEName.Text = f.VCEName
    End Sub

    Private Sub txtVCEName_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtVCEName.KeyDown
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

    Private Sub FromLoansToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FromLoansToolStripMenuItem.Click
        If Not AllowAccess("JV_LOAN") Then
            msgRestricted()
        Else
            Dim f As New frmLoadTransactions
            f.cbFilter.SelectedItem = "Status"
            f.txtFilter.Text = "Approve"
            f.txtFilter.Enabled = False
            f.cbFilter.Enabled = False
            f.btnSearch.Enabled = False
            f.ShowDialog("Copy Loan DR")

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
            Dim LoanAccount, IntIncomeAccount, UnearnedAccount, IntRecAccount, Loan_No, VCECode, IntAmortMethod, BranchCode As String
            Dim SetupUnearned As Boolean
            Dim LoanAmount, LoanPayable, IntAmount, cashAmount As Decimal
            Dim query As String
            query = " SELECT	tblLoan.LoanCode, SetupUnearned, CASE WHEN (SELECT Employer_Name FROM tblCompany WHERE Employer_Name LIKE '%lending%') IS NULL THEN LoanAccount ELSE '11702' END, IntIncomeAccount, UnearnedAccount, IntRecAccount, " & _
                    " 		    LoanAmount, IntAmount, tblLoan.IntAmortMethod, Loan_No, VCECode, LoanPayable, tblLoan.BranchCode " & _
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
                    LoanPayable = .Item(11)
                    BranchCode = .Item(12)
                    txtLoanRef.Text = Loan_No
                    If IntAmortMethod = "Less to Proceeds" Then    ' IF INTEREST IS LESS TO PROCEED 
                        ' LOAN RECEIVABLE ENTRY
                        dgvEntry.Rows.Add(LoanAccount, GetAccntTitle(LoanAccount), CDec(LoanPayable).ToString("N2"), "0.00", "", VCECode, GetVCEName(VCECode), "LN:" & Loan_No, "", "", "", "", BranchCode)
                        ' INTEREST INCOME ENTRY
                        dgvEntry.Rows.Add(IntIncomeAccount, GetAccntTitle(IntIncomeAccount), "0.00", CDec(IntAmount).ToString("N2"), "", VCECode, GetVCEName(VCECode), Loan_No, "", "", "", "", BranchCode)
                        cashAmount = LoanAmount - IntAmount
                    ElseIf IntAmortMethod = "Add On" Then    ' IF INTEREST IS Add On
                        ' LOAN RECEIVABLE ENTRY
                        dgvEntry.Rows.Add(LoanAccount, GetAccntTitle(LoanAccount), CDec(LoanPayable).ToString("N2"), "0.00", "", VCECode, GetVCEName(VCECode), "LN:" & Loan_No, "", "", "", "", BranchCode)
                        ' INTEREST INCOME ENTRY
                        dgvEntry.Rows.Add(IntIncomeAccount, GetAccntTitle(IntIncomeAccount), "0.00", CDec(IntAmount).ToString("N2"), "", VCECode, GetVCEName(VCECode), Loan_No, "", "", "", "", BranchCode)
                        cashAmount = LoanAmount - IntAmount
                    ElseIf IntAmortMethod = "Amortize" Then
                        If SetupUnearned = False Then  ' IF WITHOUT SETUP OF UNEARNED INCOME
                            ' LOAN RECEIVABLE ENTRY
                            dgvEntry.Rows.Add(LoanAccount, GetAccntTitle(LoanAccount), CDec(LoanAmount).ToString("N2"), "0.00", "", VCECode, GetVCEName(VCECode), "LN:" & Loan_No, "", "", "", "", BranchCode)
                        ElseIf SetupUnearned = True AndAlso LoanAccount = IntIncomeAccount Then  ' IF WITH SETUP OF UNEARNED INCOME AND INT. REC IS SAME AS LOAN REC. ACCOUNT
                            ' LOAN RECEIVABLE ENTRY
                            dgvEntry.Rows.Add(LoanAccount, GetAccntTitle(LoanAccount), CDec(LoanAmount + IntAmount).ToString("N2"), "0.00", "", VCECode, GetVCEName(VCECode), "LN:" & Loan_No, "", "", "", "", BranchCode)
                            ' UNEARNED INCOME ENTRY
                            dgvEntry.Rows.Add(UnearnedAccount, GetAccntTitle(UnearnedAccount), "0.00", CDec(IntAmount).ToString("N2"), "", VCECode, GetVCEName(VCECode), Loan_No, "", "", "", "", BranchCode)
                        ElseIf SetupUnearned = True AndAlso LoanAccount <> IntIncomeAccount Then  ' IF WITH SETUP OF UNEARNED INCOME AND INT. REC IS SAME AS LOAN REC. ACCOUNT
                            ' LOAN RECEIVABLE ENTRY
                            dgvEntry.Rows.Add(LoanAccount, GetAccntTitle(LoanAccount), CDec(LoanAmount).ToString("N2"), "0.00", "", VCECode, GetVCEName(VCECode), "LN:" & Loan_No, "", "", "", "", BranchCode)
                            ' INTEREST RECEIVABLE ENTRY
                            dgvEntry.Rows.Add(IntIncomeAccount, GetAccntTitle(IntIncomeAccount), CDec(IntAmount).ToString("N2"), "0.00", "", VCECode, GetVCEName(VCECode), Loan_No, "", "", "", "", BranchCode)
                            ' UNEARNED INCOME ENTRY
                            dgvEntry.Rows.Add(UnearnedAccount, GetAccntTitle(UnearnedAccount), "0.00", CDec(IntAmount).ToString("N2"), "", VCECode, GetVCEName(VCECode), Loan_No, "", "", "", "", BranchCode)
                        End If
                        cashAmount = LoanAmount
                    End If
                End With


                query = "    SELECT TransID, AccountCode, Amount, Description, VCECode, " & _
                        "       CASE WHEN SetupCharges = 1  " & _
                        " 	    THEN '' ELSE   RefID END AS RefID    " & _
                        "    FROM tblLoan_Details " & _
                        "    WHERE	    tblLoan_Details.TransID = '" & Loan & "' AND tblLoan_Details.AmortMethod IN ( 'Less to Proceeds' , 'Add On') "

                SQL.GetQuery(query)
                If SQL.SQLDS.Tables(0).Rows.Count > 0 Then
                    For Each row As DataRow In SQL.SQLDS.Tables(0).Rows
                        If row(2) > 0 Then
                            dgvEntry.Rows.Add(row(1).ToString, GetAccntTitle(row(1).ToString), "0.00", CDec(row(2)).ToString("N2"), "", row(4).ToString, GetVCEName(row(4).ToString), row(5).ToString, "", "", "", "", BranchCode)
                            cashAmount -= row(2)
                        End If
                    Next
                End If
            End If
            LoadBranch()
            TotalDBCR()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub tsbDelete_Click(sender As System.Object, e As System.EventArgs) Handles tsbDelete.Click
        If MsgBox("Are you sure you want to delete this transaction?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Delete") = MsgBoxResult.Yes Then
            Dim deleteSQL As String = ""
            deleteSQL = "DELETE FROM tblJE_Header WHERE RefType = 'JV' AND RefTransID = '" & TransID & "'"
            SQL.ExecNonQuery(deleteSQL)
            MsgBox("Successfully Deleted!", MsgBoxStyle.Information, "Delete")
            tsbNew.PerformClick()
        End If
    End Sub

    Private Sub GroupBox1_Enter(sender As System.Object, e As System.EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub FromFundsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FromFundsToolStripMenuItem.Click
        frmFund_Copy.strType = "JV"
        frmFund_Copy.ShowDialog()
    End Sub

    Private Sub dgvEntry_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles dgvEntry.KeyDown
        If e.KeyCode = Keys.Enter Then
            Select Case dgvEntry.SelectedCells(0).ColumnIndex
                Case chRefNo.Index
                    Dim f As New frmSeachSL
                    f.ShowDialog()
                    If f.strVCECode <> "" Then
                        dgvEntry.Rows.Add("")
                        dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chVCECode.Index).Value = f.strVCECode
                        dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chVCEName.Index).Value = f.strVCEName
                        dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chAccntCode.Index).Value = f.strAccntCode
                        dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chAccntTitle.Index).Value = f.strAccntTitle
                        dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chRefNo.Index).Value = f.strRefNo
                        Dim selectSQL As String = " SELECT * FROM tblLoan_Type WHERE LoanAccount = '" & f.strAccntCode & "' "
                        SQL.ReadQuery(selectSQL)
                        If SQL.SQLDR.Read Then
                            dgvEntry.Rows.Add("")
                            dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chVCECode.Index).Value = f.strVCECode
                            dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chVCEName.Index).Value = f.strVCEName
                            dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chAccntCode.Index).Value = SQL.SQLDR("IntIncomeAccount").ToString
                            dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chAccntTitle.Index).Value = GetAccntTitle(SQL.SQLDR("IntIncomeAccount").ToString)
                            dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chRefNo.Index).Value = f.strRefNo
                        End If
                    End If
            End Select
        End If
    End Sub

    Private Sub dtpDocDate_ValueChanged(sender As System.Object, e As System.EventArgs) Handles dtpDocDate.ValueChanged
        If disableEvent = False Then
            If TransID = "" Then
                txtTransNum.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
            End If
        End If
    End Sub

    Private Sub FromSavingsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FromSavingsToolStripMenuItem.Click
        frmFund_Copy.strType = "Savings"
        frmFund_Copy.ShowDialog()
    End Sub

    Private Sub FromMemberToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FromMemberToolStripMenuItem.Click
        Dim f As New frmLoadTransactions
        f.txtFilter.Enabled = True
        f.cbFilter.Enabled = True
        f.btnSearch.Enabled = True

        f.ShowDialog("Copy Member")
        LoadMember(f.transID, f.BranchCode)
        f.Dispose()
    End Sub

    Sub LoadMember(ByVal MemID As String, ByVal BranchCode As String)
        Try
            'Transfer Branch
            Dim query As String
            Dim rowsCount As Integer = 0
            BranchTransfer = BranchCode
            query = " SELECT        dbo.tblJE_Details.VCECode, dbo.viewVCE_Master.VCEName, dbo.tblJE_Details.AccntCode, dbo.tblCOA_Master.AccountTitle, CASE WHEN SUM(Debit - Credit) > 0 THEN SUM(Debit - Credit) ELSE 0 END AS Credit, " & _
                    "                          CASE WHEN SUM(Credit - Debit) > 0 THEN SUM(Credit - Debit) ELSE 0 END AS Debit, ISNULL(dbo.tblJE_Details.BranchCode, dbo.tblJE_Header.BranchCode) AS BranchCode" & _
                    " FROM            dbo.tblJE_Details INNER JOIN" & _
                    "                          dbo.viewVCE_Master ON dbo.tblJE_Details.VCECode = dbo.viewVCE_Master.VCECode INNER JOIN" & _
                    "                         dbo.tblCOA_Master ON dbo.tblJE_Details.AccntCode = dbo.tblCOA_Master.AccountCode  INNER JOIN " & _
                    "                         dbo.tblJE_Header ON dbo.tblJE_Header.JE_No = dbo.tblJE_Details.JE_No " & _
                    " 						 WHERE tblJE_Details.AccntCode IN (SELECT AccntCode FROM tblDefault_TransferAccnt) AND  dbo.tblJE_Details.VCECode = '" & MemID & "' " & _
                     " GROUP BY dbo.tblJE_Details.VCECode, dbo.viewVCE_Master.VCEName, dbo.tblJE_Details.AccntCode, dbo.tblCOA_Master.AccountTitle, ISNULL(dbo.tblJE_Details.BranchCode, dbo.tblJE_Header.BranchCode) "
            SQL.ReadQuery(query)
            dgvEntry.Rows.Clear()
            If SQL.SQLDR.HasRows Then

                While SQL.SQLDR.Read
                    dgvEntry.Rows.Add(SQL.SQLDR("AccntCode").ToString)
                    dgvEntry.Rows(rowsCount).Cells(chAccntTitle.Index).Value = SQL.SQLDR("AccountTitle").ToString
                    dgvEntry.Rows(rowsCount).Cells(chDebit.Index).Value = CDec(SQL.SQLDR("Debit")).ToString("N2")
                    dgvEntry.Rows(rowsCount).Cells(chCredit.Index).Value = CDec(SQL.SQLDR("Credit")).ToString("N2")
                    dgvEntry.Rows(rowsCount).Cells(chVCECode.Index).Value = SQL.SQLDR("VCECode").ToString
                    dgvEntry.Rows(rowsCount).Cells(chVCEName.Index).Value = SQL.SQLDR("VCEName").ToString
                    dgvEntry.Rows(rowsCount).Cells(chParticulars.Index).Value = ""
                    dgvEntry.Rows(rowsCount).Cells(chRefNo.Index).Value = ""
                    dgvEntry.Rows(rowsCount).Cells(chBranchCode.Index).Value = SQL.SQLDR("BranchCode").ToString
                    rowsCount += 1
                End While
            End If

            'Current Branch
            query = " SELECT        dbo.tblJE_Details.VCECode, dbo.viewVCE_Master.VCEName, dbo.tblJE_Details.AccntCode, dbo.tblCOA_Master.AccountTitle, CASE WHEN SUM(Debit - Credit) > 0 THEN SUM(Debit - Credit) ELSE 0 END AS Debit, " & _
                    "                          CASE WHEN SUM(Credit - Debit) > 0 THEN SUM(Credit - Debit) ELSE 0 END AS Credit, ISNULL(dbo.tblJE_Details.BranchCode, dbo.tblJE_Header.BranchCode) AS BranchCode" & _
                    " FROM            dbo.tblJE_Details INNER JOIN" & _
                    "                          dbo.viewVCE_Master ON dbo.tblJE_Details.VCECode = dbo.viewVCE_Master.VCECode INNER JOIN" & _
                    "                         dbo.tblCOA_Master ON dbo.tblJE_Details.AccntCode = dbo.tblCOA_Master.AccountCode INNER JOIN " & _
                    "                         dbo.tblJE_Header ON dbo.tblJE_Header.JE_No = dbo.tblJE_Details.JE_No " & _
                    " 						 WHERE tblJE_Details.AccntCode IN (SELECT AccntCode FROM tblDefault_TransferAccnt) AND  dbo.tblJE_Details.VCECode = '" & MemID & "' " & _
                     " GROUP BY dbo.tblJE_Details.VCECode, dbo.viewVCE_Master.VCEName, dbo.tblJE_Details.AccntCode, dbo.tblCOA_Master.AccountTitle, ISNULL(dbo.tblJE_Details.BranchCode, dbo.tblJE_Header.BranchCode) "
            SQL.ReadQuery(query)
            If SQL.SQLDR.HasRows Then

                While SQL.SQLDR.Read
                    dgvEntry.Rows.Add(SQL.SQLDR("AccntCode").ToString)
                    dgvEntry.Rows(rowsCount).Cells(chAccntTitle.Index).Value = SQL.SQLDR("AccountTitle").ToString
                    dgvEntry.Rows(rowsCount).Cells(chDebit.Index).Value = CDec(SQL.SQLDR("Debit")).ToString("N2")
                    dgvEntry.Rows(rowsCount).Cells(chCredit.Index).Value = CDec(SQL.SQLDR("Credit")).ToString("N2")
                    dgvEntry.Rows(rowsCount).Cells(chVCECode.Index).Value = SQL.SQLDR("VCECode").ToString
                    dgvEntry.Rows(rowsCount).Cells(chVCEName.Index).Value = SQL.SQLDR("VCEName").ToString
                    dgvEntry.Rows(rowsCount).Cells(chParticulars.Index).Value = ""
                    dgvEntry.Rows(rowsCount).Cells(chRefNo.Index).Value = ""
                    dgvEntry.Rows(rowsCount).Cells(chBranchCode.Index).Value = BranchCode
                    rowsCount += 1
                End While
            End If
            TotalDBCR()
            MemTransfer = True
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub LoadBranch()
        Try
            Dim dgvCB As New DataGridViewComboBoxColumn
            dgvCB = dgvEntry.Columns(chBranchCode.Index)
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
End Class