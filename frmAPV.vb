Imports Excel = Microsoft.Office.Interop.Excel

Public Class frmAPV
    Dim TransID, RefID, RR_RefID, JETransiD As String
    Dim Adv_Amount As Decimal
    Dim APVNo As String
    Dim disableEvent As Boolean = False
    Dim ModuleID As String = "APV"
    Dim ColumnID As String = "TransID"
    Dim ColumnPK As String = "APV_No"
    Dim DBTable As String = "tblAPV"
    Dim TransAuto As Boolean
    Dim AccntCode As String
    Dim PO_ID As Integer
    Dim PCV As String
    Dim accntDR, accntCR, accntVAT, accntAdvance As String


    ' SETUP VARIABLES
    Dim pendingAPsetup As Boolean
    Dim accntInputVAT, accntAPpending As String

    Public Overloads Function ShowDialog(ByVal DocID As String) As Boolean
        TransID = DocID
        MyBase.ShowDialog()
        Return True
    End Function

    Private Sub APV_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            TransAuto = GetTransSetup(ModuleID)
            dtpDocDate.Value = Date.Today.Date
            LoadSetup()
            LoadChartOfAccount()
            If TransID <> "" Then
                LoadAPV(TransID)
            End If
            tsbSearch.Enabled = True
            tsbNew.Enabled = True
            tsbEdit.Enabled = False
            tsbSave.Enabled = False
            tsbCancel.Enabled = False
            tsbClose.Enabled = False
            tsbUpload.Enabled = False
            tsbPrevious.Enabled = False
            tsbNext.Enabled = False
            tsbExit.Enabled = True
            tsbPrint.Enabled = False
            tsbCopy.Enabled = False
            If dtpDocDate.Value < GetMaxPEC() Then
                tsbEdit.Enabled = False
                tsbCancel.Enabled = False
            End If
            EnableControl(False)
        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        End Try
    End Sub

    Private Sub LoadSetup()
        Dim query As String
        query = " SELECT  ISNULL(AP_SetupPendingAP,0) AS AP_SetupPendingAP, AP_Pending, AP_InputVAT  FROM tblSystemSetup "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            pendingAPsetup = SQL.SQLDR("AP_SetupPendingAP")
            accntAPpending = SQL.SQLDR("AP_Pending").ToString
            accntInputVAT = SQL.SQLDR("AP_InputVAT").ToString
        End If
    End Sub


    Private Sub LoadChartOfAccount()
        Dim query As String
        query = " SELECT  AccountCode, AccountTitle " & _
                " FROM    tblCOA_Master " & _
                " WHERE   AccountNature = 'Credit' " & _
                " ORDER BY AccountTitle "
        SQL.ReadQuery(query)
        cbCreditAccount.Items.Clear()
        While SQL.SQLDR.Read
            cbCreditAccount.Items.Add(SQL.SQLDR("AccountTitle"))
        End While
    End Sub

    Private Sub EnableControl(ByVal Value As Boolean)
        dtpDocDate.Enabled = Value
        txtVCEName.Enabled = Value
        btnSearchVCE.Enabled = Value
        dgvEntry.AllowUserToAddRows = Value
        dgvEntry.AllowUserToDeleteRows = Value
        If Value = True Then
            dgvEntry.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2
        Else
            dgvEntry.EditMode = DataGridViewEditMode.EditProgrammatically
        End If
        txtRemarks.Enabled = Value
        dtpDocDate.Enabled = Value
        txtPORef.Enabled = Value
        txtSINo.Enabled = Value
        txtAmount.Enabled = Value
        txtDiscount.Enabled = Value
        txtVAT.Enabled = Value
        txtNet.Enabled = Value
        If TransAuto Then
            txtTransNum.Enabled = False
        Else
            txtTransNum.Enabled = Value
        End If
    End Sub

    Private Sub LoadAPV(ID As String)
        Dim query As String
        query = " SELECT  TransID, APV_No, tblAPV.VCECOde, VCEName, DateAPV, ISNULL(Amount,0) AS Amount, ISNULL(Discount,0) AS Discount, " & _
                "         ISNULL(InputVAT,0) AS InputVAT, ISNULL(NetPurchase,0) AS NetPurchase, Remarks, PO_Ref, SI_Ref, RR_Ref, tblAPV.Status, AccntCode  " & _
                " FROM    tblAPV INNER JOIN tblVCE_Master " & _
                " ON      tblAPV.VCECode = tblVCE_Master.VCECode " & _
                " WHERE   TransID ='" & ID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            dtpDocDate.MinDate = "01-01-1900"
            TransID = SQL.SQLDR("TransID").ToString
            APVNo = SQL.SQLDR("APV_No").ToString
            txtTransNum.Text = APVNo
            txtVCECode.Text = SQL.SQLDR("VCECode").ToString
            txtVCEName.Text = SQL.SQLDR("VCEName").ToString
            dtpDocDate.Value = SQL.SQLDR("DateAPV")
            txtAmount.Text = CDec(SQL.SQLDR("Amount").ToString).ToString("N2")
            txtDiscount.Text = CDec(SQL.SQLDR("Discount")).ToString("N2")
            txtVAT.Text = CDec(SQL.SQLDR("InputVAT")).ToString("N2")
            txtNet.Text = CDec(SQL.SQLDR("NetPurchase")).ToString("N2")
            txtRemarks.Text = SQL.SQLDR("Remarks").ToString
            txtPORef.Text = SQL.SQLDR("PO_Ref").ToString
            txtRRRef.Text = SQL.SQLDR("RR_Ref").ToString
            txtSINo.Text = SQL.SQLDR("SI_Ref").ToString
            txtStatus.Text = SQL.SQLDR("Status").ToString
            cbCreditAccount.SelectedItem = GetAccntTitle(SQL.SQLDR("AccntCode").ToString)
            LoadAccountingEntry(TransID)

            ' TOOLSTRIP BUTTONS
            If txtStatus.Text = "Cancelled" Then
                tsbEdit.Enabled = False
                tsbCancel.Enabled = False
            Else
                tsbEdit.Enabled = True
                tsbCancel.Enabled = True
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

    Private Function LoadPONo(PO_ID As Integer) As String
        Dim query As String
        query = " SELECT PO_No FROM tblPO WHERE TransID = '" & PO_ID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return SQL.SQLDR("PO_No")
        Else
            Return 0
        End If
    End Function

    Private Sub LoadAccountingEntry(ByVal TransID As Integer)
        Try
            Dim query As String
            query = " SELECT ID, JE_No, View_GL.BranchCode, View_GL.AccntCode, AccntTitle, View_GL.VCECode, View_GL.VCEName, Particulars, " & _
                    "        ISNULL(Debit,0) AS Debit, ISNULL(Credit,0) AS Credit, RefNo, CostCenter, SL_Code, CIP_Code   " & _
                    " FROM   View_GL  " & _
                    " WHERE  RefType ='APV' AND RefTransID ='" & TransID & "' " & _
                    " ORDER BY LineNumber "
            SQL.ReadQuery(query)
            dgvEntry.Rows.Clear()
            Dim rowsCount As Integer = 0
            If SQL.SQLDR.HasRows Then
                While SQL.SQLDR.Read

                    JETransiD = SQL.SQLDR("JE_No").ToString
                    dgvEntry.Rows.Add(SQL.SQLDR("AccntCode").ToString)
                    dgvEntry.Rows(rowsCount).Cells(dgcAccntTitle.Index).Value = SQL.SQLDR("AccntTitle").ToString
                    dgvEntry.Rows(rowsCount).Cells(dgcDebit.Index).Value = CDec(SQL.SQLDR("Debit")).ToString("N2")
                    dgvEntry.Rows(rowsCount).Cells(dgcCredit.Index).Value = CDec(SQL.SQLDR("Credit")).ToString("N2")
                    dgvEntry.Rows(rowsCount).Cells(dgcVCECode.Index).Value = SQL.SQLDR("VCECode").ToString
                    dgvEntry.Rows(rowsCount).Cells(dgcVCEName.Index).Value = SQL.SQLDR("VCEName").ToString
                    dgvEntry.Rows(rowsCount).Cells(dgcParticulars.Index).Value = SQL.SQLDR("Particulars").ToString
                    dgvEntry.Rows(rowsCount).Cells(dgcRefNo.Index).Value = SQL.SQLDR("RefNo").ToString




                    'CostCenter
                    Dim strCCCode As String = SQL.SQLDR("CostCenter").ToString
                    Dim cbvCostCenter As DataGridViewComboBoxCell = LoadCostCenterGridView()
                    Dim strCostCenter As String = GetCCName(strCCCode)
                    If cbvCostCenter.Items.Contains(IIf(IsNothing(strCostCenter), "", strCostCenter)) Then
                        cbvCostCenter.Value = strCostCenter
                    End If
                    dgvEntry.Rows(rowsCount).Cells(chCostCenter.Index) = cbvCostCenter
                    dgvEntry.Rows(rowsCount).Cells(dgcCC.Index).Value = SQL.SQLDR("CostCenter").ToString

                    'CIP
                    Dim strCIP_Code As String = SQL.SQLDR("CIP_Code").ToString
                    Dim cbvCIP As DataGridViewComboBoxCell = LoadCIPGridView()
                    Dim strCIP As String = GetCIPName(strCIP_Code)
                    If cbvCIP.Items.Contains(IIf(IsNothing(strCIP), "", strCIP)) Then
                        cbvCIP.Value = strCIP
                    End If
                    dgvEntry.Rows(rowsCount).Cells(chCIP_Desc.Index) = cbvCIP
                    dgvEntry.Rows(rowsCount).Cells(dgcCIP.Index).Value = SQL.SQLDR("CIP_Code").ToString


                    rowsCount += 1
                End While
                TotalDBCR()
            Else
                JETransiD = 0
                dgvEntry.Rows.Clear()
            End If
        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        End Try
    End Sub

    Private Sub TotalDBCR()
        ' COMPUTE TOTAL DEBIT
        Dim db As Decimal = 0

        For i As Integer = 0 To dgvEntry.Rows.Count - 1
            If Val(dgvEntry.Item(dgcDebit.Index, i).Value) <> 0 Then
                db = db + Double.Parse(dgvEntry.Item(dgcDebit.Index, i).Value).ToString("N2")
            End If
        Next
        txtTotalDebit.Text = db.ToString("N2")

        ' COMPUTE TOTAL CREDIT
        Dim b As Decimal = 0

        For i As Integer = 0 To dgvEntry.Rows.Count - 1
            If Val(dgvEntry.Item(dgcCredit.Index, i).Value) <> 0 Then
                b = b + Double.Parse(dgvEntry.Item(dgcCredit.Index, i).Value).ToString("N2")
            End If
        Next
        txtTotalCredit.Text = b.ToString("N2")
    End Sub

    Private Sub txtVCEName_KeyDown_1(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtVCEName.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim f As New frmVCE_Search
            f.cbFilter.SelectedItem = "VCEName"
            f.txtFilter.Text = txtVCEName.Text
            f.ShowDialog()
            txtVCECode.Text = f.VCECode
            txtVCEName.Text = f.VCEName
        End If
    End Sub

    Private Sub ClearText()
        txtTransNum.Clear()
        txtVCECode.Clear()
        txtVCEName.Clear()
        dgvEntry.Rows.Clear()
        txtRemarks.Clear()
        txtPORef.Clear()
        txtRRRef.Clear()
        txtStatus.Text = "Open"
        dtpDocDate.Value = Date.Today.Date
        txtAmount.Text = "0.00"
        txtDiscount.Text = "0.00"
        txtVAT.Text = "0.00"
        txtNet.Text = "0.00"
        txtSINo.Clear()
        dgvEntry.Rows.Clear()
        txtTotalCredit.Clear()
        txtTotalDebit.Clear()
        dtpDocDate.MinDate = GetMaxPEC()
    End Sub

    Private Sub LoadPO(ByVal ID As String)

        Dim query As String
        query = " SELECT    tblPO.TransID, tblPO.PO_No, DatePO AS Date, tblPO.VCECode, VCEName,  " & _
                " 		    ISNULL(ADV_Amount,0) AS Advances, GrossAmount AS Amount, " & _
                " 		    VATAmount AS VAT, " & _
                " 		    NetAmount AS NetPurchase, " & _
                " 		    Remarks, tblPO.AccntCode, ADV.AccntCode  AS ADVAccount " & _
                " FROM      tblPO INNER JOIN tblVCE_Master " & _
                " ON        tblPO.VCECode = tblVCE_Master.VCECode " & _
                " INNER JOIN viewPO_Status " & _
                " ON        tblPO.TransID = viewPO_Status.TransID " & _
                " LEFT JOIN " & _
                " (  SELECT PO_Ref, AccntCode, SUM(ADV_Amount) AS ADV_Amount  FROM tblADV WHERE Status ='Closed' GROUP BY PO_Ref, AccntCode) AS ADV " & _
                " ON        tblPO.TransID  = ADV.PO_Ref " & _
                " WHERE     viewPO_Status.Status ='Closed' AND tblPO.TransID ='" & ID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            RefID = SQL.SQLDR("TransID")
            txtPORef.Text = SQL.SQLDR("PO_No").ToString
            txtVCECode.Text = SQL.SQLDR("VCECode").ToString
            txtVCEName.Text = SQL.SQLDR("VCEName").ToString
            txtAmount.Text = CDec(SQL.SQLDR("Amount")).ToString("N2")
            txtDiscount.Text = "0.00"
            txtVAT.Text = CDec(SQL.SQLDR("VAT")).ToString("N2")
            txtNet.Text = CDec(SQL.SQLDR("NetPurchase")).ToString("N2")
            txtRemarks.Text = SQL.SQLDR("Remarks").ToString
            accntDR = SQL.SQLDR("AccntCode").ToString
            Adv_Amount = SQL.SQLDR("Advances").ToString
            accntAdvance = SQL.SQLDR("ADVAccount").ToString
            GenerateEntry(ID)
        Else
            ClearText()
        End If
    End Sub

    Private Sub GenerateEntry(ByVal PO_ID As Integer)
        Dim query As String
        Dim amount As Decimal = 0
        If pendingAPsetup = False Then
            Dim baseAmt As Decimal = 0
            query = " SELECT BaseAmount, AD_Inv FROM viewAPV_InvEntry WHERE TransID = '" & PO_ID & "' "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                accntDR = SQL.SQLDR("AD_Inv").ToString
                baseAmt = SQL.SQLDR("BaseAmount").ToString
            End If
            accntCR = GetAccntCode(cbCreditAccount.SelectedItem)
            dgvEntry.Rows.Clear()
            dgvEntry.Rows.Add({accntDR, GetAccntTitle(accntDR), CDec(txtNet.Text) - CDec(txtVAT.Text).ToString("N2"), "", ""})

            If CDec(txtVAT.Text) <> 0 Then
                dgvEntry.Rows.Add({accntInputVAT, GetAccntTitle(accntInputVAT), CDec(txtVAT.Text).ToString("N2"), "", ""})

                Dim cbvCostCenter1 As DataGridViewComboBoxCell = LoadCostCenterGridView()
                cbvCostCenter1.Value = strDefaultGridView
                dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCostCenter.Index) = cbvCostCenter1

                Dim cbvCIP1 As DataGridViewComboBoxCell = LoadCIPGridView()
                cbvCIP1.Value = strDefaultGridView
                dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCIP_Desc.Index) = cbvCIP1
            End If

            If Adv_Amount <> 0 Then
                dgvEntry.Rows.Add({accntAdvance, GetAccntTitle(accntAdvance), "", Adv_Amount.ToString("N2"), ""})

                Dim cbvCostCenter1 As DataGridViewComboBoxCell = LoadCostCenterGridView()
                cbvCostCenter1.Value = strDefaultGridView
                dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCostCenter.Index) = cbvCostCenter1

                Dim cbvCIP1 As DataGridViewComboBoxCell = LoadCIPGridView()
                cbvCIP1.Value = strDefaultGridView
                dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCIP_Desc.Index) = cbvCIP1
            End If

            If CDec(txtNet.Text) - Adv_Amount <> 0 Then
                dgvEntry.Rows.Add({accntCR, GetAccntTitle(accntCR), "", (CDec(txtNet.Text) - Adv_Amount).ToString("N2"), ""})

                Dim cbvCostCenter1 As DataGridViewComboBoxCell = LoadCostCenterGridView()
                cbvCostCenter1.Value = strDefaultGridView
                dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCostCenter.Index) = cbvCostCenter1

                Dim cbvCIP1 As DataGridViewComboBoxCell = LoadCIPGridView()
                cbvCIP1.Value = strDefaultGridView
                dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCIP_Desc.Index) = cbvCIP1
            End If

            TotalDBCR()
        Else
            accntCR = GetAccntCode(cbCreditAccount.SelectedItem)
            query = " SELECT RefNo, SUM(Credit-Debit) AS Debit, VCECode, VCEName  " & _
                    " FROM View_GL WHERE RefType='RR' AND RefTransID IN (SELECT TransID FROM tblRR WHERE PO_Ref = '" & PO_ID & "') AND AccntCode ='" & accntAPpending & "' " & _
                    " GROUP BY RefNo, VCECode, VCEName  "
            SQL.GetQuery(query)
            dgvEntry.Rows.Clear()
            If SQL.SQLDS.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In SQL.SQLDS.Tables(0).Rows
                    dgvEntry.Rows.Add({accntAPpending, GetAccntTitle(accntAPpending), CDec(row.Item(1)).ToString("N2"), "", "", txtVCECode.Text, txtVCEName.Text, row.Item(0).ToString, "", ""})
                    amount += row.Item(1)
                Next
                dgvEntry.Rows.Add({accntCR, GetAccntTitle(accntCR), "", CDec(amount).ToString("N2"), "", txtVCECode.Text, txtVCEName.Text, "APV:" & txtTransNum.Text, "", ""})
            End If
        End If

    End Sub



    Private Sub dgvCVRR_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvEntry.CellEndEdit
        Dim rowIndex As Integer = dgvEntry.CurrentCell.RowIndex
        Dim colIndex As Integer = dgvEntry.CurrentCell.ColumnIndex
        Dim Accountcode As String
        Dim COA_Group As String
        Try
            Select Case colIndex
                Case dgcAccntCode.Index
                    'dgvEntry.Item(dgcAccntTitle.Index, e.RowIndex).Value = GetAccntTitle((dgvEntry.Item(dgcAccntCode.Index, e.RowIndex).Value))
                    Accountcode = dgvEntry.Item(dgcAccntCode.Index, e.RowIndex).Value
                    Dim f As New frmCOA_Search
                    f.accttile = Accountcode
                    f.ShowDialog("AccntCode", Accountcode)
                    dgvEntry.Item(dgcAccntCode.Index, e.RowIndex).Value = f.accountcode
                    dgvEntry.Item(dgcAccntTitle.Index, e.RowIndex).Value = f.accttile
                    COA_Group = f.COA_Group
                    f.Dispose()
                    dgvEntry.Item(dgcParticulars.Index, e.RowIndex).Value = txtRemarks.Text
                    dgvEntry.Item(dgcVCECode.Index, e.RowIndex).Value = txtVCECode.Text
                    dgvEntry.Item(dgcVCEName.Index, e.RowIndex).Value = txtVCEName.Text

                    If COA_Group = "Operating Expenses" Then
                        Msg("Please Select Cost Center!", MsgBoxStyle.Information)
                        'Auto Entry Grid View Cost Center
                        If IsNothing(dgvEntry.Item(chCostCenter.Index, e.RowIndex).Value) Then
                            Dim cbvCostCenter As DataGridViewComboBoxCell = LoadCostCenterGridView()
                            cbvCostCenter.Value = strDefaultGridView
                            dgvEntry.Item(chCostCenter.Index, e.RowIndex) = cbvCostCenter

                            Dim dgvCostCenter As String
                            dgvCostCenter = dgvEntry.Rows(e.RowIndex).Cells(chCostCenter.Index).Value
                            LoadCostCenterCode(dgvCostCenter, e.RowIndex, chCostCenter.Index, dgcCC.Index)
                        End If
                    End If

                    'Auto Entry Grid View Cost CIP
                    If IsNothing(dgvEntry.Item(chCIP_Desc.Index, e.RowIndex).Value) Then
                        Dim cbvCIP As DataGridViewComboBoxCell = LoadCIPGridView()
                        cbvCIP.Value = strDefaultGridView
                        dgvEntry.Item(chCIP_Desc.Index, e.RowIndex) = cbvCIP

                        Dim dgvCIP As String
                        dgvCIP = dgvEntry.Rows(e.RowIndex).Cells(chCIP_Desc.Index).Value
                        LoadCIPCode(dgvCIP, e.RowIndex, chCIP_Desc.Index, dgcCIP.Index)
                    End If

                Case dgcAccntTitle.Index
                    Accountcode = dgvEntry.Item(dgcAccntTitle.Index, e.RowIndex).Value
                    Dim f As New frmCOA_Search
                    f.accttile = Accountcode
                    f.ShowDialog("AccntTitle", Accountcode)
                    dgvEntry.Item(dgcAccntCode.Index, e.RowIndex).Value = f.accountcode
                    dgvEntry.Item(dgcAccntTitle.Index, e.RowIndex).Value = f.accttile
                    COA_Group = f.COA_Group
                    f.Dispose()
                    dgvEntry.Item(dgcParticulars.Index, e.RowIndex).Value = txtRemarks.Text
                    dgvEntry.Item(dgcVCECode.Index, e.RowIndex).Value = txtVCECode.Text
                    dgvEntry.Item(dgcVCEName.Index, e.RowIndex).Value = txtVCEName.Text

                    If COA_Group = "Operating Expenses" Then
                        Msg("Please Select Cost Center!", MsgBoxStyle.Information)
                        'Auto Entry Grid View Cost Center
                        If IsNothing(dgvEntry.Item(chCostCenter.Index, e.RowIndex).Value) Then
                            Dim cbvCostCenter As DataGridViewComboBoxCell = LoadCostCenterGridView()
                            cbvCostCenter.Value = strDefaultGridView
                            dgvEntry.Item(chCostCenter.Index, e.RowIndex) = cbvCostCenter

                            Dim dgvCostCenter As String
                            dgvCostCenter = dgvEntry.Rows(e.RowIndex).Cells(chCostCenter.Index).Value
                            LoadCostCenterCode(dgvCostCenter, e.RowIndex, chCostCenter.Index, dgcCC.Index)
                        End If
                    End If

                    'Auto Entry Grid View Cost CIP
                    If IsNothing(dgvEntry.Item(chCIP_Desc.Index, e.RowIndex).Value) Then
                        Dim cbvCIP As DataGridViewComboBoxCell = LoadCIPGridView()
                        cbvCIP.Value = strDefaultGridView
                        dgvEntry.Item(chCIP_Desc.Index, e.RowIndex) = cbvCIP

                        Dim dgvCIP As String
                        dgvCIP = dgvEntry.Rows(e.RowIndex).Cells(chCIP_Desc.Index).Value
                        LoadCIPCode(dgvCIP, e.RowIndex, chCIP_Desc.Index, dgcCIP.Index)
                    End If


                Case dgcVCEName.Index
                    Dim f As New frmVCE_Search
                    f.txtFilter.Text = dgvEntry.Item(e.ColumnIndex, e.RowIndex).Value.ToString
                    f.ShowDialog()
                    dgvEntry.Item(dgcVCECode.Index, e.RowIndex).Value = f.VCECode
                    dgvEntry.Item(dgcVCEName.Index, e.RowIndex).Value = f.VCEName
                    f.Dispose()


                Case chCostCenter.Index
                    Dim dgvCostCenter As String
                    dgvCostCenter = dgvEntry.Rows(e.RowIndex).Cells(chCostCenter.Index).Value
                    LoadCostCenterCode(dgvCostCenter, e.RowIndex, chCostCenter.Index, dgcCC.Index)


                Case chCIP_Desc.Index
                    Dim dgvCIP As String
                    dgvCIP = dgvEntry.Rows(e.RowIndex).Cells(chCIP_Desc.Index).Value
                    LoadCIPCode(dgvCIP, e.RowIndex, chCIP_Desc.Index, dgcCIP.Index)
            End Select
            TotalDBCR()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Function GetTransID() As Integer
        Dim query As String
        query = " SELECT MAX(TransID) + 1 AS TransID FROM tblAPV"
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read() And Not IsDBNull(SQL.SQLDR("TransID")) Then
            Return SQL.SQLDR("TransID")
        Else
            Return 1
        End If
    End Function

    Private Sub SaveAPV()

        Try
            If cbCreditAccount.SelectedIndex = -1 Then
                AccntCode = ""
            Else
                AccntCode = GetAccntCode(cbCreditAccount.SelectedItem)
            End If
            Dim insertSQL As String
            activityStatus = True
            insertSQL = " INSERT INTO " & _
                    " tblAPV    (TransID, APV_No, BranchCode, BusinessCode, VCECode, DateAPV, SI_Ref, AccntCode, Amount, Discount, InputVAT, NetPurchase, Remarks, PO_Ref, WhoCreated, RR_Ref) " & _
                    " VALUES (@TransID, @APV_No, @BranchCode, @BusinessCode, @VCECode, @DateAPV, @SI_Ref, @AccntCode, @Amount, @Discount, @InputVAT, @NetPurchase, @Remarks, @PO_Ref, @WhoCreated, @RR_Ref) "
            SQL.FlushParams()
            SQL.AddParam("@TransID", TransID)
            SQL.AddParam("@APV_No", APVNo)
            SQL.AddParam("@BranchCode", BranchCode)
            SQL.AddParam("@BusinessCode", BusinessType)
            SQL.AddParam("@VCECode", txtVCECode.Text)
            SQL.AddParam("@DateAPV", dtpDocDate.Value.Date)
            SQL.AddParam("@AccntCode", AccntCode)
            SQL.AddParam("@Amount", IIf(txtAmount.Text = "", 0, CDec(txtAmount.Text)))
            SQL.AddParam("@Discount", IIf(txtDiscount.Text = "", 0, CDec(txtDiscount.Text)))
            SQL.AddParam("@InputVAT", IIf(txtVAT.Text = "", 0, CDec(txtVAT.Text)))
            SQL.AddParam("@NetPurchase", IIf(txtNet.Text = "", 0, CDec(txtNet.Text)))
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@PO_Ref", IIf(RefID = Nothing, 0, RefID))
            SQL.AddParam("@RR_Ref", IIf(RR_RefID = Nothing, 0, RR_RefID))
            SQL.AddParam("@SI_Ref", txtSINo.Text)
            SQL.AddParam("@WhoCreated", UserID)
            SQL.ExecNonQuery(insertSQL)

            Dim updateSQl As String
            updateSQl = " UPDATE tblPO SET Status ='Posted' WHERE TransID ='" & RefID & "' "
            SQL.ExecNonQuery(updateSQl)

            insertSQL = " INSERT INTO " & _
                        " tblJE_Header (AppDate, BranchCode, BusinessCode, RefType, RefTransID, Book, TotalDBCR, Remarks, WhoCreated) " & _
                        " VALUES(@AppDate, @BranchCode, @BusinessCode, @RefType, @RefTransID, @Book, @TotalDBCR, @Remarks, @WhoCreated)"
            SQL.FlushParams()
            SQL.AddParam("@AppDate", dtpDocDate.Value.Date)
            SQL.AddParam("@RefType", "APV")
            SQL.AddParam("@RefTransID", TransID)
            SQL.AddParam("@Book", "Accounts Payable")
            SQL.AddParam("@TotalDBCR", CDec(txtTotalCredit.Text))
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@BranchCode", BranchCode)
            SQL.AddParam("@BusinessCode", BusinessType)
            SQL.AddParam("@WhoCreated", UserID)
            SQL.ExecNonQuery(insertSQL)

            JETransiD = LoadJE("APV", TransID)

            Dim line As Integer = 1
            For Each item As DataGridViewRow In dgvEntry.Rows
                If item.Cells(dgcAccntCode.Index).Value <> Nothing Then
                    insertSQL = " INSERT INTO " & _
                                " tblJE_Details(JE_No, AccntCode, VCECode, Debit, Credit, Particulars, RefNo, LineNumber, CIP_Code, CostCenter) " & _
                                " VALUES(@JE_No, @AccntCode, @VCECode, @Debit, @Credit, @Particulars, @RefNo, @LineNumber, @CIP_Code, @CostCenter)"
                    SQL.FlushParams()
                    SQL.AddParam("@JE_No", JETransiD)
                    SQL.AddParam("@AccntCode", item.Cells(dgcAccntCode.Index).Value.ToString)
                    If item.Cells(dgcVCECode.Index).Value <> Nothing AndAlso item.Cells(dgcVCECode.Index).Value <> "" Then
                        SQL.AddParam("@VCECode", item.Cells(dgcVCECode.Index).Value.ToString)
                    Else
                        SQL.AddParam("@VCECode", txtVCECode.Text)
                    End If
                    If item.Cells(dgcDebit.Index).Value <> Nothing AndAlso IsNumeric(item.Cells(dgcDebit.Index).Value) Then
                        SQL.AddParam("@Debit", CDec(item.Cells(dgcDebit.Index).Value))
                    Else
                        SQL.AddParam("@Debit", 0)
                    End If
                    If item.Cells(dgcCredit.Index).Value <> Nothing AndAlso IsNumeric(item.Cells(dgcCredit.Index).Value) Then
                        SQL.AddParam("@Credit", CDec(item.Cells(dgcCredit.Index).Value))
                    Else
                        SQL.AddParam("@Credit", 0)
                    End If
                    If item.Cells(dgcParticulars.Index).Value <> Nothing AndAlso item.Cells(dgcParticulars.Index).Value <> "" Then
                        SQL.AddParam("@Particulars", item.Cells(dgcParticulars.Index).Value.ToString)
                    Else
                        SQL.AddParam("@Particulars", "")
                    End If
                    If item.Cells(dgcCC.Index).Value <> Nothing AndAlso item.Cells(dgcCC.Index).Value <> "" Then
                        SQL.AddParam("@CostCenter", item.Cells(dgcCC.Index).Value.ToString)
                    Else
                        SQL.AddParam("@CostCenter", "")
                    End If
                    If item.Cells(dgcCIP.Index).Value <> Nothing AndAlso item.Cells(dgcCIP.Index).Value <> "" Then
                        SQL.AddParam("@CIP_Code", item.Cells(dgcCIP.Index).Value.ToString)
                    Else
                        SQL.AddParam("@CIP_Code", "")
                    End If
                    If item.Cells(dgcRefNo.Index).Value <> Nothing AndAlso item.Cells(dgcRefNo.Index).Value <> "" Then
                        SQL.AddParam("@RefNo", item.Cells(dgcRefNo.Index).Value.ToString)
                    Else
                        SQL.AddParam("@RefNo", "")
                    End If
                    SQL.AddParam("@LineNumber", line)
                    SQL.ExecNonQuery(insertSQL)

                    Dim update As String
                    If item.Cells(chRecordID.Index).Value <> Nothing Then
                        update = " UPDATE tblPCV_Details Set Status = 'Closed' " & _
                                 " WHERE RecordID = '" & item.Cells(chRecordID.Index).Value & "'"
                        SQL.ExecNonQuery(update)
                    End If

                    If item.Cells(dgcRefNo.Index).Value <> Nothing Then
                        PCV = dgvEntry.Item(dgcRefNo.Index, line).Value.ToString.Replace("PCV:", "")
                        update = " UPDATE tblPCV SET Status ='Closed' WHERE PCV_No = '" & PCV & "'"
                        SQL.ExecNonQuery(update)
                    End If
                    line += 1
                End If
            Next
        Catch ex As Exception
            activityStatus = False
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        Finally
            RecordActivity(UserID, ModuleID, Me.Name.ToString, "INSERT", "APV_No", txtTransNum.Text, BusinessType, BranchCode, "", activityStatus)
            SQL.FlushParams()
        End Try
    End Sub

    Private Sub UpdateAPV()
        Try
            If cbCreditAccount.SelectedIndex = -1 Then
                AccntCode = ""
            Else
                AccntCode = GetAccntCode(cbCreditAccount.SelectedItem)
            End If
            Dim insertSQL, updateSQL, deleteSQL As String
            activityStatus = True
            updateSQL = " UPDATE tblAPV " & _
                        " SET    APV_No = @APV_No, BranchCode = @BranchCode, BusinessCode = @BusinessCode, " & _
                        "        VCECode = @VCECode, DateAPV = @DateAPV, SI_Ref = @SI_Ref, AccntCode = @AccntCode, " & _
                        "        Amount = @Amount, Discount = @Discount, InputVAT = @InputVAT, NetPurchase = @NetPurchase, " & _
                        "        Remarks = @Remarks, PO_Ref = @PO_Ref, RR_Ref = @RR_Ref, WhoModified = @WhoModified, DateModified = GETDATE() " & _
                        " WHERE  TransID = @TransID "
            SQL.FlushParams()
            SQL.AddParam("@TransID", TransID)
            SQL.AddParam("@APV_No", APVNo)
            SQL.AddParam("@BranchCode", BranchCode)
            SQL.AddParam("@BusinessCode", BusinessType)
            SQL.AddParam("@VCECode", txtVCECode.Text)
            SQL.AddParam("@DateAPV", dtpDocDate.Value.Date)
            SQL.AddParam("@AccntCode", AccntCode)
            SQL.AddParam("@Amount", IIf(txtAmount.Text = "", 0, CDec(txtAmount.Text)))
            SQL.AddParam("@Discount", IIf(txtDiscount.Text = "", 0, CDec(txtDiscount.Text)))
            SQL.AddParam("@InputVAT", IIf(txtVAT.Text = "", 0, CDec(txtVAT.Text)))
            SQL.AddParam("@NetPurchase", IIf(txtNet.Text = "", 0, CDec(txtNet.Text)))
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@PO_Ref", txtPORef.Text)
            SQL.AddParam("@RR_Ref", txtRRRef.Text)
            SQL.AddParam("@SI_Ref", txtSINo.Text)
            SQL.AddParam("@WhoModified", UserID)
            SQL.ExecNonQuery(updateSQL)

            JETransiD = LoadJE("APV", TransID)

            If JETransiD = 0 Then
                insertSQL = " INSERT INTO " & _
                            " tblJE_Header (AppDate, BranchCode, BusinessCode, RefType, RefTransID, Book, TotalDBCR, Remarks, WhoCreated) " & _
                            " VALUES(@AppDate, @BranchCode, @BusinessCode, @RefType, @RefTransID, @Book, @TotalDBCR, @Remarks, @WhoCreated)"
                SQL.FlushParams()
                SQL.AddParam("@AppDate", dtpDocDate.Value.Date)
                SQL.AddParam("@RefType", "APV")
                SQL.AddParam("@RefTransID", TransID)
                SQL.AddParam("@Book", "Accounts Payable")
                SQL.AddParam("@TotalDBCR", CDec(txtTotalCredit.Text))
                SQL.AddParam("@Remarks", txtRemarks.Text)
                SQL.AddParam("@BranchCode", BranchCode)
                SQL.AddParam("@BusinessCode", BusinessType)
                SQL.AddParam("@WhoCreated", UserID)
                SQL.ExecNonQuery(insertSQL)

                JETransiD = LoadJE("APV", TransID)
            Else
                updateSQL = " UPDATE tblJE_Header " & _
                            " SET    AppDate = @AppDate, BranchCode = @BranchCode, BusinessCode = @BusinessCode, " & _
                            "        RefType = @RefType, RefTransID = @RefTransID, Book = @Book, TotalDBCR = @TotalDBCR, " & _
                            "        Remarks = @Remarks, WhoModified = @WhoModified, DateModified = GETDATE() " & _
                            " WHERE  JE_No = @JE_No "
                SQL.FlushParams()
                SQL.AddParam("@JE_No", JETransiD)
                SQL.AddParam("@AppDate", dtpDocDate.Value.Date)
                SQL.AddParam("@RefType", "APV")
                SQL.AddParam("@RefTransID", TransID)
                SQL.AddParam("@Book", "Accounts Payable")
                SQL.AddParam("@TotalDBCR", CDec(txtTotalCredit.Text))
                SQL.AddParam("@Remarks", txtRemarks.Text)
                SQL.AddParam("@BranchCode", BranchCode)
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

            ' INSERT NEW ENTRIES
            For Each item As DataGridViewRow In dgvEntry.Rows
                If item.Cells(dgcAccntCode.Index).Value <> Nothing Then
                    insertSQL = " INSERT INTO " & _
                                " tblJE_Details(JE_No, AccntCode, VCECode, Debit, Credit, Particulars, RefNo, LineNumber, CostCenter, CIP_Code) " & _
                                " VALUES(@JE_No, @AccntCode, @VCECode, @Debit, @Credit, @Particulars, @RefNo, @LineNumber, @CostCenter, @CIP_Code)"
                    SQL.FlushParams()
                    SQL.AddParam("@JE_No", JETransiD)
                    SQL.AddParam("@AccntCode", item.Cells(dgcAccntCode.Index).Value.ToString)
                    If item.Cells(dgcVCECode.Index).Value <> Nothing AndAlso item.Cells(dgcVCECode.Index).Value <> "" Then
                        SQL.AddParam("@VCECode", item.Cells(dgcVCECode.Index).Value.ToString)
                    Else
                        SQL.AddParam("@VCECode", txtVCECode.Text)
                    End If
                    If item.Cells(dgcDebit.Index).Value AndAlso IsNumeric(item.Cells(dgcDebit.Index).Value) Then
                        SQL.AddParam("@Debit", CDec(item.Cells(dgcDebit.Index).Value))
                    Else
                        SQL.AddParam("@Debit", 0)
                    End If
                    If item.Cells(dgcCredit.Index).Value <> Nothing AndAlso IsNumeric(item.Cells(dgcCredit.Index).Value) Then
                        SQL.AddParam("@Credit", CDec(item.Cells(dgcCredit.Index).Value))
                    Else
                        SQL.AddParam("@Credit", 0)
                    End If
                    If item.Cells(dgcParticulars.Index).Value <> Nothing AndAlso item.Cells(dgcParticulars.Index).Value <> "" Then
                        SQL.AddParam("@Particulars", item.Cells(dgcParticulars.Index).Value.ToString)
                    Else
                        SQL.AddParam("@Particulars", "")
                    End If
                    If item.Cells(dgcCC.Index).Value <> Nothing AndAlso item.Cells(dgcCC.Index).Value <> "" Then
                        SQL.AddParam("@CostCenter", item.Cells(dgcCC.Index).Value.ToString)
                    Else
                        SQL.AddParam("@CostCenter", "")
                    End If
                    If item.Cells(dgcCIP.Index).Value <> Nothing AndAlso item.Cells(dgcCIP.Index).Value <> "" Then
                        SQL.AddParam("@CIP_Code", item.Cells(dgcCIP.Index).Value.ToString)
                    Else
                        SQL.AddParam("@CIP_Code", "")
                    End If
                    If item.Cells(dgcRefNo.Index).Value <> Nothing AndAlso item.Cells(dgcRefNo.Index).Value <> "" Then
                        SQL.AddParam("@RefNo", item.Cells(dgcRefNo.Index).Value.ToString)
                    Else
                        SQL.AddParam("@RefNo", "")
                    End If
                    SQL.AddParam("@LineNumber", line)
                    SQL.ExecNonQuery(insertSQL)


                    Dim update As String
                    If item.Cells(chRecordID.Index).Value <> Nothing Then
                        update = " UPDATE tblPCV_Details Set Status = 'Closed' " & _
                                 " WHERE RecordID = '" & item.Cells(chRecordID.Index).Value & "'"
                        SQL.ExecNonQuery(update)
                    End If

                    'If item.Cells(dgcRefNo.Index).Value <> Nothing Then
                    '    PCV = dgvEntry.Item(dgcRefNo.Index, line).Value.ToString.Replace("PCV:", "")
                    '    update = " UPDATE tblPCV SET Status ='Closed' WHERE PCV_No = '" & PCV & "'"
                    '    SQL.ExecNonQuery(update)
                    'End If
                    line += 1
                End If
            Next
        Catch ex As Exception
            activityStatus = False
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        Finally
            RecordActivity(UserID, ModuleID, Me.Name.ToString, "UPDATE", "APV_No", txtTransNum.Text, BusinessType, BranchCode, "", activityStatus)
            SQL.FlushParams()
        End Try
    End Sub

    Private Sub txtAmount_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtAmount.KeyDown, txtVAT.KeyDown, txtDiscount.KeyDown, txtNet.KeyDown
        Try
            If e.KeyValue <> 8 And Not (e.KeyValue >= 48 And e.KeyValue <= 57) Then
                e.SuppressKeyPress = True
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub dgvEntry_RowsRemoved(sender As System.Object, e As System.Windows.Forms.DataGridViewRowsRemovedEventArgs) Handles dgvEntry.RowsRemoved
        TotalDBCR()
    End Sub

    Private Sub tsbSearch_Click(sender As System.Object, e As System.EventArgs) Handles tsbSearch.Click
        If Not AllowAccess("APV_VIEW") Then
            msgRestricted()
        Else
            Dim f As New frmLoadTransactions
            f.ShowDialog("APV")
            If f.transID <> "" Then
                TransID = f.transID
            End If
            LoadAPV(TransID)
            f.Dispose()
        End If
    End Sub

    Private Sub tsbNew_Click(sender As System.Object, e As System.EventArgs) Handles tsbNew.Click
        If Not AllowAccess("APV_ADD") Then
            msgRestricted()
        Else
            ClearText()
            TransID = ""
            APVNo = ""

            ' Toolstrip Buttons
            tsbSearch.Enabled = False
            tsbNew.Enabled = False
            tsbEdit.Enabled = False
            tsbSave.Enabled = True
            tsbUpload.Enabled = True
            tsbCancel.Enabled = False
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


    Private Sub tsbEdit_Click(sender As System.Object, e As System.EventArgs) Handles tsbEdit.Click
        If Not AllowAccess("APV_EDIT") Then
            msgRestricted()
        Else
            EnableControl(True)

            ' Toolstrip Buttons
            tsbSearch.Enabled = False
            tsbNew.Enabled = False
            tsbEdit.Enabled = False
            tsbSave.Enabled = True
            tsbUpload.Enabled = False
            tsbCancel.Enabled = False
            tsbClose.Enabled = True
            tsbPrevious.Enabled = False
            tsbNext.Enabled = False
            tsbExit.Enabled = False
            tsbPrint.Enabled = False
            tsbCopy.Enabled = False
        End If
    End Sub



    Private Sub tsbSave_Click(sender As System.Object, e As System.EventArgs) Handles tsbSave.Click
        If DataValidated() Then
            If txtVCECode.Text = "" Then
                Msg("Please enter VCECode!", MsgBoxStyle.Exclamation)
            ElseIf cbCreditAccount.SelectedIndex = -1 Then
                Msg("Please select default Credit account first!", MsgBoxStyle.Exclamation)
            ElseIf txtTotalDebit.Text <> txtTotalCredit.Text Then
                MsgBox("Please check the Debit and Credit Amount!", MsgBoxStyle.Exclamation)
            ElseIf txtAmount.Text = "" Or txtNet.Text = "" Then
                MsgBox("Please check amount!", MsgBoxStyle.Exclamation)
            ElseIf TransID = "" Then
                If MsgBox("Saving New Record, Click Yes to confirm", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "JADE Message Alert") = MsgBoxResult.Yes Then
                    TransID = GenerateTransID(ColumnID, DBTable)
                    If TransAuto Then
                        APVNo = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
                    Else
                        APVNo = txtTransNum.Text
                    End If
                    txtTransNum.Text = APVNo
                    SaveAPV()
                    Msg("Record Saved Succesfully!", MsgBoxStyle.Information)
                    LoadAPV(TransID)
                End If
            Else
                If MsgBox("Updating Record, Click Yes to confirm", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "JADE Message Alert") = MsgBoxResult.Yes Then

                    If APVNo = txtTransNum.Text Then
                        APVNo = txtTransNum.Text
                        UpdateAPV()
                        Msg("Record Updated Succesfully!", MsgBoxStyle.Information)
                        APVNo = txtTransNum.Text
                        LoadAPV(TransID)
                    Else
                        If Not IfExist(txtTransNum.Text) Then
                            APVNo = txtTransNum.Text
                            UpdateAPV()
                            Msg("Record Updated Succesfully!", MsgBoxStyle.Information)
                            APVNo = txtTransNum.Text
                            LoadAPV(TransID)
                        Else
                            MsgBox("PCV" & " " & txtTransNum.Text & " already exist!", MsgBoxStyle.Exclamation)
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Function IfExist(ByVal ID As String) As Boolean
        Dim query As String
        query = " SELECT * FROM tblAPV WHERE APV_No ='" & ID & "'  "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function DataValidated() As Boolean
        If  validateDGV() = False Then
            Return False
        Else
            Return True
        End If
    End Function

    Private Function validateDGV() As Boolean
        Dim AccntCode, query, costcenter As String
        Dim value As Boolean = True
        For Each row As DataGridViewRow In dgvEntry.Rows
            Dim COA_Group As String
            AccntCode = row.Cells(dgcAccntCode.Index).Value
            query = "SELECT COA_Group FROM tblCOA_Master WHERE AccountCode = '" & AccntCode & "'"
            SQL.ReadQuery(query)

            If SQL.SQLDR.Read Then
                COA_Group = SQL.SQLDR("COA_Group").ToString
                If COA_Group = "Operating Expenses" Then
                    If row.Cells(chCostCenter.Index).Value = "" Then costcenter = "" Else costcenter = row.Cells(chCostCenter.Index).Value

                    If costcenter = "" Then
                        Msg("There are line entry without cost center, please check.", MsgBoxStyle.Exclamation)
                        value = False
                        Exit For
                    End If
                End If
            End If
        Next
        Return value
    End Function

    Private Function isUsed(ByVal strTransNo As String) As Boolean
        Dim query As String = " SELECT * FROM tblAPV WHERE APV_No = '" & strTransNo & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub tsbPrint_Click(sender As System.Object, e As System.EventArgs) Handles tsbPrint.Click
        Dim f As New frmReport_Display
        f.ShowDialog("APV", TransID)
        f.Dispose()
    End Sub

    Private Sub tsbCancel_Click(sender As System.Object, e As System.EventArgs) Handles tsbCancel.Click
        If Not AllowAccess("APV_DEL") Then
            msgRestricted()
        Else
            If txtTransNum.Text <> "" Then
                If MsgBox("Are you sure you want to cancel this record?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "JADE Message Alert") = MsgBoxResult.Yes Then
                    Try
                        activityStatus = True
                        Dim updateSQL As String
                        updateSQL = " UPDATE  tblAPV SET Status ='Cancelled' WHERE TransID = @TransID "
                        SQL.FlushParams()
                        SQL.AddParam("@TransID", TransID)
                        SQL.ExecNonQuery(updateSQL)

                        updateSQL = " UPDATE  tblJE_Header SET Status ='Cancelled' WHERE RefTransID = @RefTransID  AND RefType ='APV' "
                        SQL.FlushParams()
                        SQL.AddParam("@RefTransID", TransID)
                        SQL.ExecNonQuery(updateSQL)

                        Dim insertSQL As String
                        insertSQL = " INSERT INTO tblJE_Details(JE_No, AccntCode, VCECode, Debit, Credit, Particulars, RefNo, LineNumber, OtherRef) " & _
                                    " SELECT JE_No, AccntCode, VCECode, Credit, Debit, Particulars, RefNo, LineNumber, OtherRef FROM tblJE_Details " & _
                                    " WHERE JE_No IN (SELECT DISTINCT JE_No FROM tblJE_Header WHERE RefType ='APV' AND RefTransID ='" & TransID & "') "
                        SQL.ExecNonQuery(insertSQL)
                        Msg("Record cancelled successfully", MsgBoxStyle.Information)

                        tsbSearch.Enabled = True
                        tsbNew.Enabled = True
                        tsbEdit.Enabled = False
                        tsbSave.Enabled = False
                        tsbCancel.Enabled = False
                        tsbClose.Enabled = False
                        tsbPrevious.Enabled = False
                        tsbNext.Enabled = False
                        tsbExit.Enabled = True
                        tsbPrint.Enabled = True
                        tsbCopy.Enabled = False
                        EnableControl(False)

                        APVNo = txtTransNum.Text
                        LoadAPV(TransID)
                    Catch ex As Exception
                        activityStatus = True
                        SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
                    Finally
                        RecordActivity(UserID, ModuleID, Me.Name.ToString, "CANCEL", "APV_No", APVNo, BusinessType, BranchCode, "", activityStatus)
                        SQL.FlushParams()
                    End Try
                End If
            End If
        End If
    End Sub

    Private Sub tsbPrevious_Click(sender As System.Object, e As System.EventArgs) Handles tsbPrevious.Click
        If APVNo <> "" Then
            Dim query As String
            query = " SELECT Top 1 TransID FROM tblAPV  WHERE APV_No < '" & APVNo & "' ORDER BY APV_No DESC "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                TransID = SQL.SQLDR("TransID").ToString
                LoadAPV(TransID)
            Else
                Msg("Reached the beginning of record!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub

    Private Sub tsbNext_Click(sender As System.Object, e As System.EventArgs) Handles tsbNext.Click
        If APVNo <> "" Then
            Dim query As String
            query = " SELECT Top 1 TransID FROM tblAPV  WHERE APV_No > '" & APVNo & "' ORDER BY APV_No ASC "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                TransID = SQL.SQLDR("TransID").ToString
                LoadAPV(TransID)
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
            RR_RefID = 0
            RefID = 0
            tsbEdit.Enabled = False
            tsbCancel.Enabled = False
            tsbPrevious.Enabled = False
            tsbNext.Enabled = False
            tsbPrint.Enabled = False
        Else
            LoadAPV(TransID)
            tsbEdit.Enabled = True
            tsbCancel.Enabled = True
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

    Private Sub btnSearchVCE_Click(sender As System.Object, e As System.EventArgs) Handles btnSearchVCE.Click
        Dim f As New frmVCE_Search
        f.ShowDialog()
        txtVCECode.Text = f.VCECode
        txtVCEName.Text = f.VCEName
    End Sub

    Private Sub tsbExit_Click(sender As System.Object, e As System.EventArgs) Handles tsbExit.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub frmAPV_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
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

    Private Sub tsbCopyPO_Click(sender As System.Object, e As System.EventArgs) Handles tsbCopyPO.Click
        If cbCreditAccount.SelectedIndex = -1 Then
            Msg("Please select default Credit account first!", MsgBoxStyle.Exclamation)
        Else
            Dim f As New frmLoadTransactions
            f.cbFilter.SelectedItem = "Status"
            f.txtFilter.Text = "Closed"
            f.txtFilter.Enabled = False
            f.cbFilter.Enabled = False
            f.btnSearch.Enabled = False
            f.ShowDialog("PO")
            LoadPO(f.transID)
            f.Dispose()
        End If
    End Sub

    Private Sub TestToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles TestToolStripMenuItem1.Click
        Dim f As New frmReport_Filter
        f.Report = "APV List"
        f.ShowDialog()
        f.Dispose()
    End Sub

    Private Sub cbCreditAccount_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cbCreditAccount.KeyPress
        e.Handled = True
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

    Dim eColIndex As Integer = 0
    Private Sub dgvEntry_CellBeginEdit(sender As Object, e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles dgvEntry.CellBeginEdit
        eColIndex = e.ColumnIndex
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
        dgvEntry.Rows(RowIndex).Cells(CodeIndex).Value = strDefaultGridCode

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

    Private Sub FromPCVToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FromPCVToolStripMenuItem.Click
        If cbCreditAccount.SelectedIndex = -1 Then
            Msg("Please select default Credit account first!", MsgBoxStyle.Exclamation)
        Else
            Dim f As New frmLoadTransactions
            f.cbFilter.SelectedItem = "Status"
            f.txtFilter.Text = "Active"
            f.txtFilter.Enabled = False
            f.cbFilter.Enabled = False
            f.btnSearch.Enabled = False
            f.ShowDialog("PCV")
            f.Dispose()
        End If
    End Sub

    Public Sub LoadPCV(ByVal PCVID As String)
        accntCR = GetAccntCode(cbCreditAccount.SelectedItem)
        Try
            Dim query As String
            query = " SELECT    TransID, PCV_No, BranchCode, tblPCV.VCECode, VCEName " & _
                    " FROM      tblPCV LEFT JOIN tblVCE_Master " & _
                    " ON		tblPCV.VCECode = tblVCE_Master.VCECode " & _
                    " WHERE     TransID ='" & PCVID & "' "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                txtVCECode.Text = SQL.SQLDR("VCECode").ToString
                txtVCEName.Text = SQL.SQLDR("VCEName").ToString
                PCV = SQL.SQLDR("PCV_No")

                query = " SELECT		tblPCV.TransID, tblPCV.BranchCode, AccountCode, AccountTitle, CodePayee, RecordPayee, tblPCV_Details.Type, tblPCV_Details.BaseAmount, tblPCV_Details.InputVAT, CostCenter, tblPCV_Details.Status, tblPCV_Details.RecordID, CIP_Code " & _
                          " FROM		tblPCV INNER JOIN tblPCV_Details " & _
                          " ON			tblPCV.TransID = tblPCV_Details.TransID " & _
                          " LEFT JOIN	tblRFP_Type " & _
                          " ON		 	tblPCV_Details.Type = tblRFP_Type.Type " & _
                          " LEFT JOIN	tblCOA_Master " & _
                          " ON			tblCOA_Master.AccountCode = tblRFP_Type.DefaultAccount " & _
                          " WHERE tblPCV.TransID = '" & PCVID & "' AND tblPCV_Details.Status ='Active' "
                SQL.ReadQuery(query)

                Dim rowsCount As Integer = dgvEntry.Rows.Count - 1
                While SQL.SQLDR.Read


                    dgvEntry.Rows.Add("")
                    dgvEntry.Rows(rowsCount).Cells(dgcAccntCode.Index).Value = SQL.SQLDR("AccountCode").ToString
                    dgvEntry.Rows(rowsCount).Cells(dgcAccntTitle.Index).Value = SQL.SQLDR("AccountTitle").ToString
                    dgvEntry.Rows(rowsCount).Cells(dgcDebit.Index).Value = CDec(SQL.SQLDR("BaseAmount")).ToString("N2")
                    dgvEntry.Rows(rowsCount).Cells(dgcCredit.Index).Value = "0.00"
                    dgvEntry.Rows(rowsCount).Cells(dgcVCECode.Index).Value = SQL.SQLDR("CodePayee").ToString
                    dgvEntry.Rows(rowsCount).Cells(dgcVCEName.Index).Value = SQL.SQLDR("RecordPayee").ToString
                    dgvEntry.Rows(rowsCount).Cells(dgcParticulars.Index).Value = SQL.SQLDR("Type").ToString
                    dgvEntry.Rows(rowsCount).Cells(dgcRefNo.Index).Value = "PCV:" & PCV

                    'CostCenter
                    Dim strCCCode As String = SQL.SQLDR("CostCenter").ToString
                    Dim cbvCostCenter As DataGridViewComboBoxCell = LoadCostCenterGridView()
                    Dim strCostCenter As String = GetCCName(strCCCode)
                    If cbvCostCenter.Items.Contains(IIf(IsNothing(strCostCenter), "", strCostCenter)) Then
                        cbvCostCenter.Value = strCostCenter
                    End If
                    dgvEntry.Rows(rowsCount).Cells(chCostCenter.Index) = cbvCostCenter
                    dgvEntry.Rows(rowsCount).Cells(dgcCC.Index).Value = SQL.SQLDR("CostCenter").ToString

                    'CIP
                    Dim strCIP_Code As String = SQL.SQLDR("CIP_Code").ToString
                    Dim cbvCIP As DataGridViewComboBoxCell = LoadCIPGridView()
                    Dim strCIP As String = GetCIPName(strCIP_Code)
                    If cbvCIP.Items.Contains(IIf(IsNothing(strCIP), "", strCIP)) Then
                        cbvCIP.Value = strCIP
                    End If
                    dgvEntry.Rows(rowsCount).Cells(chCIP_Desc.Index) = cbvCIP
                    dgvEntry.Rows(rowsCount).Cells(dgcCIP.Index).Value = SQL.SQLDR("CIP_Code").ToString





                    dgvEntry.Rows(rowsCount).Cells(chRecordID.Index).Value = SQL.SQLDR("RecordID").ToString
                    txtAmount.Text = CDec(txtAmount.Text) + IIf(CDec(SQL.SQLDR("BaseAmount")) = 0, 0, CDec(SQL.SQLDR("BaseAmount")))

                    If SQL.SQLDR("InputVAT") <> 0 Then
                        rowsCount += 1
                        dgvEntry.Rows.Add("")
                        dgvEntry.Rows(rowsCount).Cells(dgcAccntCode.Index).Value = "1550-1030"
                        dgvEntry.Rows(rowsCount).Cells(dgcAccntTitle.Index).Value = "Value Added Tax - Input"
                        dgvEntry.Rows(rowsCount).Cells(dgcDebit.Index).Value = CDec(SQL.SQLDR("InputVAT")).ToString("N2")
                        dgvEntry.Rows(rowsCount).Cells(dgcCredit.Index).Value = "0.00"
                        dgvEntry.Rows(rowsCount).Cells(dgcVCECode.Index).Value = SQL.SQLDR("CodePayee").ToString
                        dgvEntry.Rows(rowsCount).Cells(dgcVCEName.Index).Value = SQL.SQLDR("RecordPayee").ToString
                        dgvEntry.Rows(rowsCount).Cells(dgcParticulars.Index).Value = SQL.SQLDR("Type").ToString
                        dgvEntry.Rows(rowsCount).Cells(dgcRefNo.Index).Value = "PCV:" & PCV

                        Dim strCostCenter1 As String = SQL.SQLDR("CostCenter").ToString
                        Dim cbvCostCenter1 As DataGridViewComboBoxCell = LoadCostCenterGridView()
                        If cbvCostCenter1.Items.Contains(IIf(IsNothing(strCostCenter1), "", strCostCenter1)) Then
                            cbvCostCenter1.Value = strCostCenter1
                        End If
                        dgvEntry.Rows(rowsCount).Cells(chCostCenter.Index) = cbvCostCenter1
                        LoadCostCenterCode(strCostCenter1, rowsCount, chCostCenter.Index, dgcCC.Index)


                        Dim strCIP1 As String = SQL.SQLDR("CIP_Code").ToString
                        Dim cbvCIP1 As DataGridViewComboBoxCell = LoadCIPGridView()
                        If cbvCIP1.Items.Contains(IIf(IsNothing(strCIP1), "", strCIP1)) Then
                            cbvCIP1.Value = strCIP1
                        End If
                        dgvEntry.Rows(rowsCount).Cells(chCIP_Desc.Index) = cbvCIP1
                        LoadCIPCode(strCIP1, rowsCount, chCIP_Desc.Index, dgcCIP.Index)

                        dgvEntry.Rows(rowsCount).Cells(chRecordID.Index).Value = SQL.SQLDR("RecordID").ToString
                        txtVAT.Text = CDec(txtVAT.Text) + IIf(CDec(SQL.SQLDR("InputVAT")) = 0, 0, CDec(SQL.SQLDR("InputVAT")))
                    End If

                    rowsCount += 1
                End While
            End If

            TotalDBCR()

            If CDec(txtTotalDebit.Text) <> 0 Then
                Dim CreditAmnt As Decimal = 0.0
                CreditAmnt = CDec(txtTotalDebit.Text).ToString("N2")

                dgvEntry.Rows(dgvEntry.Rows.Count - 1).Cells(dgcAccntCode.Index).Value = accntCR
                dgvEntry.Rows(dgvEntry.Rows.Count - 1).Cells(dgcAccntTitle.Index).Value = GetAccntTitle(accntCR)
                dgvEntry.Rows(dgvEntry.Rows.Count - 1).Cells(dgcDebit.Index).Value = "0.00"
                dgvEntry.Rows(dgvEntry.Rows.Count - 1).Cells(dgcCredit.Index).Value = CreditAmnt
                dgvEntry.Rows(dgvEntry.Rows.Count - 1).Cells(dgcVCECode.Index).Value = ""
                dgvEntry.Rows(dgvEntry.Rows.Count - 1).Cells(dgcVCEName.Index).Value = ""
                dgvEntry.Rows(dgvEntry.Rows.Count - 1).Cells(dgcParticulars.Index).Value = txtRemarks.Text
                dgvEntry.Rows(dgvEntry.Rows.Count - 1).Cells(dgcRefNo.Index).Value = ""

                'Auto Entry Grid View Cost Center
                If IsNothing(dgvEntry.Item(chCostCenter.Index, dgvEntry.Rows.Count - 1).Value) Then
                    Dim cbvCostCenter As DataGridViewComboBoxCell = LoadCostCenterGridView()
                    cbvCostCenter.Value = strDefaultGridView
                    dgvEntry.Item(chCostCenter.Index, dgvEntry.Rows.Count - 1) = cbvCostCenter

                    Dim dgvCostCenter As String
                    dgvCostCenter = dgvEntry.Rows(dgvEntry.Rows.Count - 1).Cells(chCostCenter.Index).Value
                    LoadCostCenterCode(dgvCostCenter, dgvEntry.Rows.Count - 1, chCostCenter.Index, dgcCC.Index)
                End If


                'Auto Entry Grid View CIP
                If IsNothing(dgvEntry.Item(chCIP_Desc.Index, dgvEntry.Rows.Count - 1).Value) Then
                    Dim cbvCIP As DataGridViewComboBoxCell = LoadCIPGridView()
                    cbvCIP.Value = strDefaultGridView
                    dgvEntry.Item(chCIP_Desc.Index, dgvEntry.Rows.Count - 1) = cbvCIP

                    Dim dgvCIP As String
                    dgvCIP = dgvEntry.Rows(dgvEntry.Rows.Count - 1).Cells(chCIP_Desc.Index).Value
                    LoadCIPCode(dgvCIP, dgvEntry.Rows.Count - 1, chCIP_Desc.Index, dgcCIP.Index)
                End If

                TotalDBCR()
            End If

            txtNet.Text = CDec(txtAmount.Text - txtDiscount.Text) + CDec(txtVAT.Text)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub FromRRToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FromRRToolStripMenuItem.Click
        If cbCreditAccount.SelectedIndex = -1 Then
            Msg("Please select default Credit account first!", MsgBoxStyle.Exclamation)
        Else
            Dim f As New frmLoadTransactions
            f.cbFilter.SelectedItem = "Status"
            f.txtFilter.Text = "Active"
            f.txtFilter.Enabled = False
            f.cbFilter.Enabled = False
            f.btnSearch.Enabled = False
            f.ShowDialog("RR-APV")
            LoadRR(f.transID)
            f.Dispose()
        End If
    End Sub

    Private Sub LoadRR(ByVal ID As String)
        Dim selectquery, RRRef As String
        selectquery = " SELECT RR_No FROM tblRR WHERE TransID ='" & ID & "' "
        SQL.ReadQuery(selectquery)
        If SQL.SQLDR.Read Then
            RRRef = SQL.SQLDR("RR_No").ToString
        End If
        Dim query As String
        query = "  SELECT    tblRR.TransID, tblRR.RR_No, DateRR AS Date, tblRR.VCECode, tblVCE_Master.VCEName,  " & _
                " ISNULL(ADV_Amount,0) AS Advances,  Credit AS NetPurchase,  " & _
                " tblRR.Remarks,  ADV.AccntCode  AS ADVAccount, View_GL.AccntCode as APPendingAccount  " & _
                " FROM tblRR " & _
                " INNER JOIN tblVCE_Master  ON  " & _
                " tblRR.VCECode = tblVCE_Master.VCECode " & _
                " INNER JOIN View_GL  ON " & _
                " tblRR.TransID = View_GL.RefTransID " & _
                " LEFT JOIN " & _
                " (  SELECT PO_Ref, AccntCode, SUM(ADV_Amount) AS ADV_Amount " & _
                "  FROM tblADV " & _
                " WHERE Status ='Closed' GROUP BY PO_Ref, AccntCode) AS ADV " & _
                " ON        tblRR.PO_Ref  = ADV.PO_Ref " & _
                " WHERE      tblRR.TransID ='" & ID & "' and RefNo = 'RR:" & RRRef & "'"
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
            RR_RefID = SQL.SQLDR("TransID")
            txtRRRef.Text = SQL.SQLDR("RR_No").ToString
                txtVCECode.Text = SQL.SQLDR("VCECode").ToString
                txtVCEName.Text = SQL.SQLDR("VCEName").ToString
            txtAmount.Text = CDec(SQL.SQLDR("NetPurchase")).ToString("N2")
                txtDiscount.Text = "0.00"
            txtVAT.Text = "0.00"
                txtNet.Text = CDec(SQL.SQLDR("NetPurchase")).ToString("N2")
                txtRemarks.Text = SQL.SQLDR("Remarks").ToString
            accntDR = SQL.SQLDR("APPendingAccount").ToString
                Adv_Amount = SQL.SQLDR("Advances").ToString
                accntAdvance = SQL.SQLDR("ADVAccount").ToString
            GenerateEntryRR(ID)
            Else
                ClearText()
            End If
    End Sub


    Private Sub GenerateEntryRR(ByVal PO_ID As Integer)
        Dim query As String
        Dim amount As Decimal = 0
        If pendingAPsetup = False Then
            Dim baseAmt As Decimal = 0
            query = " SELECT BaseAmount, AD_Inv FROM viewAPV_InvEntry WHERE TransID = '" & PO_ID & "' "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                accntDR = SQL.SQLDR("AD_Inv").ToString
                baseAmt = SQL.SQLDR("BaseAmount").ToString
            End If
            accntCR = GetAccntCode(cbCreditAccount.SelectedItem)
            dgvEntry.Rows.Clear()
            dgvEntry.Rows.Add({accntDR, GetAccntTitle(accntDR), CDec(txtNet.Text) - CDec(txtVAT.Text).ToString("N2"), "", ""})

            Dim cbvCostCenter2 As DataGridViewComboBoxCell = LoadCostCenterGridView()
            cbvCostCenter2.Value = strDefaultGridView
            dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCostCenter.Index) = cbvCostCenter2

            Dim dgvCostCenter2 As String
            dgvCostCenter2 = dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCostCenter.Index).Value
            LoadCostCenterCode(dgvCostCenter2, dgvEntry.Rows.Count - 2, chCostCenter.Index, dgcCC.Index)

            Dim cbvCIP2 As DataGridViewComboBoxCell = LoadCIPGridView()
            cbvCIP2.Value = strDefaultGridView
            dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCIP_Desc.Index) = cbvCIP2

            Dim dgvCIP As String
            dgvCIP = dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCIP_Desc.Index).Value
            LoadCIPCode(dgvCIP, dgvEntry.Rows.Count - 2, chCIP_Desc.Index, dgcCIP.Index)

            If CDec(txtVAT.Text) <> 0 Then
                dgvEntry.Rows.Add({accntInputVAT, GetAccntTitle(accntInputVAT), CDec(txtVAT.Text).ToString("N2"), "", ""})

                Dim cbvCostCenter1 As DataGridViewComboBoxCell = LoadCostCenterGridView()
                cbvCostCenter1.Value = strDefaultGridView
                dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCostCenter.Index) = cbvCostCenter1

                Dim dgvCostCenter1 As String
                dgvCostCenter1 = dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCostCenter.Index).Value
                LoadCostCenterCode(dgvCostCenter1, dgvEntry.Rows.Count - 2, chCostCenter.Index, dgcCC.Index)

                Dim cbvCIP1 As DataGridViewComboBoxCell = LoadCIPGridView()
                cbvCIP1.Value = strDefaultGridView
                dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCIP_Desc.Index) = cbvCIP1

                Dim dgvCIP1 As String
                dgvCIP1 = dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCIP_Desc.Index).Value
                LoadCIPCode(dgvCIP1, dgvEntry.Rows.Count - 2, chCIP_Desc.Index, dgcCIP.Index)
            End If

            If Adv_Amount <> 0 Then
                dgvEntry.Rows.Add({accntAdvance, GetAccntTitle(accntAdvance), "", Adv_Amount.ToString("N2"), ""})

                Dim cbvCostCenter1 As DataGridViewComboBoxCell = LoadCostCenterGridView()
                cbvCostCenter1.Value = strDefaultGridView
                dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCostCenter.Index) = cbvCostCenter1

                Dim dgvCostCenter1 As String
                dgvCostCenter1 = dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCostCenter.Index).Value
                LoadCostCenterCode(dgvCostCenter1, dgvEntry.Rows.Count - 2, chCostCenter.Index, dgcCC.Index)

                Dim cbvCIP1 As DataGridViewComboBoxCell = LoadCIPGridView()
                cbvCIP1.Value = strDefaultGridView
                dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCIP_Desc.Index) = cbvCIP1

                Dim dgvCIP1 As String
                dgvCIP1 = dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCIP_Desc.Index).Value
                LoadCIPCode(dgvCIP1, dgvEntry.Rows.Count - 2, chCIP_Desc.Index, dgcCIP.Index)
            End If

            If CDec(txtNet.Text) - Adv_Amount <> 0 Then
                dgvEntry.Rows.Add({accntCR, GetAccntTitle(accntCR), "", (CDec(txtNet.Text) - Adv_Amount).ToString("N2"), ""})

                Dim cbvCostCenter1 As DataGridViewComboBoxCell = LoadCostCenterGridView()
                cbvCostCenter1.Value = strDefaultGridView
                dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCostCenter.Index) = cbvCostCenter1

                Dim dgvCostCenter1 As String
                dgvCostCenter1 = dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCostCenter.Index).Value
                LoadCostCenterCode(dgvCostCenter1, dgvEntry.Rows.Count - 2, chCostCenter.Index, dgcCC.Index)

                Dim cbvCIP1 As DataGridViewComboBoxCell = LoadCIPGridView()
                cbvCIP1.Value = strDefaultGridView
                dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCIP_Desc.Index) = cbvCIP1

                Dim dgvCIP1 As String
                dgvCIP1 = dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCIP_Desc.Index).Value
                LoadCIPCode(dgvCIP1, dgvEntry.Rows.Count - 2, chCIP_Desc.Index, dgcCIP.Index)
            End If

            TotalDBCR()
        Else
            accntCR = GetAccntCode(cbCreditAccount.SelectedItem)
            query = " SELECT RefNo, SUM(Credit-Debit) AS Debit, VCECode, VCEName  " & _
                    " FROM View_GL WHERE RefType='RR' AND RefTransID IN (SELECT TransID FROM tblRR WHERE TransID = '" & PO_ID & "') AND AccntCode ='" & accntAPpending & "' " & _
                    " GROUP BY RefNo, VCECode, VCEName  "
            SQL.GetQuery(query)
            dgvEntry.Rows.Clear()
            If SQL.SQLDS.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In SQL.SQLDS.Tables(0).Rows
                    dgvEntry.Rows.Add({accntAPpending, GetAccntTitle(accntAPpending), CDec(row.Item(1)).ToString("N2"), "", "", txtVCECode.Text, txtVCEName.Text, row.Item(0).ToString, "", ""})
                    amount += row.Item(1)

                    Dim cbvCostCenter3 As DataGridViewComboBoxCell = LoadCostCenterGridView()
                    cbvCostCenter3.Value = strDefaultGridView
                    dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCostCenter.Index) = cbvCostCenter3

                    Dim dgvCostCenter3 As String
                    dgvCostCenter3 = dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCostCenter.Index).Value
                    LoadCostCenterCode(dgvCostCenter3, dgvEntry.Rows.Count - 2, chCostCenter.Index, dgcCC.Index)

                    Dim cbvCIP3 As DataGridViewComboBoxCell = LoadCIPGridView()
                    cbvCIP3.Value = strDefaultGridView
                    dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCIP_Desc.Index) = cbvCIP3

                    Dim dgvCIP3 As String
                    dgvCIP3 = dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCIP_Desc.Index).Value
                    LoadCIPCode(dgvCIP3, dgvEntry.Rows.Count - 2, chCIP_Desc.Index, dgcCIP.Index)
                Next
                dgvEntry.Rows.Add({accntCR, GetAccntTitle(accntCR), "", CDec(amount).ToString("N2"), "", txtVCECode.Text, txtVCEName.Text, "APV:" & txtTransNum.Text, "", ""})
                Dim cbvCostCenter2 As DataGridViewComboBoxCell = LoadCostCenterGridView()
                cbvCostCenter2.Value = strDefaultGridView
                dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCostCenter.Index) = cbvCostCenter2

                Dim dgvCostCenter2 As String
                dgvCostCenter2 = dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCostCenter.Index).Value
                LoadCostCenterCode(dgvCostCenter2, dgvEntry.Rows.Count - 2, chCostCenter.Index, dgcCC.Index)

                Dim cbvCIP2 As DataGridViewComboBoxCell = LoadCIPGridView()
                cbvCIP2.Value = strDefaultGridView
                dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCIP_Desc.Index) = cbvCIP2

                Dim dgvCIP2 As String
                dgvCIP2 = dgvEntry.Rows(dgvEntry.Rows.Count - 2).Cells(chCIP_Desc.Index).Value
                LoadCIPCode(dgvCIP2, dgvEntry.Rows.Count - 2, chCIP_Desc.Index, dgcCIP.Index)
            End If
        End If

    End Sub
End Class