Public Class frmGI
    Dim TransID, RefID As String
    Dim GINo As String
    Dim disableEvent As Boolean = False
    Dim ModuleID As String = "GI"
    Dim ColumnPK As String = "GI_No"
    Dim ColumnID As String = "TransID"
    Dim DBTable As String = "tblGI"
    Dim TransAuto As Boolean
    Dim AccntCode As String
    Dim SysCode As String = "EMERALD"
    Dim MR_ID, JO_ID As Integer
    Dim withConfirm As Boolean = True

    Public Overloads Function ShowDialog(ByVal docnumber As String) As Boolean
        TransID = docnumber
        MyBase.ShowDialog()
        Return True
    End Function

    Private Sub frmGI_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try
            TransAuto = GetTransSetup(ModuleID)
            dtpDocDate.Value = Date.Today.Date
            loadWH()
            If TransID <> "" Then
                LoadGI(TransID)
            End If
            tsbSearch.Enabled = True
            tsbNew.Enabled = True
            tsbEdit.Enabled = False
            tsbSave.Enabled = False
            tsbCancel.Enabled = False
            tsbClose.Enabled = False
            tsbPrevious.Enabled = False
            tsbNext.Enabled = False
            tsbExit.Enabled = True
            tsbPrint.Enabled = False
            tsbCopy.Enabled = False
            EnableControl(False)
        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        End Try
    End Sub

    Private Sub LoadCostCenter()
        Dim query As String
        query = " SELECT Code + '|' + Description AS CostCenter FROM tblCC "
        SQL.ReadQuery(query)
        cbCostCenter.Items.Clear()
        While SQL.SQLDR.Read
            cbCostCenter.Items.Add(SQL.SQLDR("CostCenter").ToString)
        End While
    End Sub


    Private Sub loadWHFrom()
        If cbIssuerFrom.SelectedItem = "Warehouse" Then
            Dim query As String
            query = " SELECT tblWarehouse.Code + ' | ' + Description AS WHSECode " & _
                    " FROM tblWarehouse INNER JOIN tblUser_Access " & _
                    " ON tblWarehouse.Code = tblUser_Access.Code " & _
                    " AND tblUser_Access.Status ='Active' AND tblWarehouse.Status ='Active' " & _
                    " AND Type = 'Warehouse' AND isAllowed = 1 " & _
                    " WHERE UserID ='" & UserID & "' "
            SQL.ReadQuery(query)
            cbWHfrom.Items.Clear()
            Dim ctr As Integer = 0
            While SQL.SQLDR.Read
                cbWHfrom.Items.Add(SQL.SQLDR("WHSECode").ToString)
                ctr += 1
            End While
            If cbWHfrom.Items.Count > 0 Then
                cbWHfrom.SelectedIndex = 0
            End If
        ElseIf cbIssuerFrom.SelectedItem = "Production" Then
            Dim query As String
            query = " SELECT    tblProdWarehouse.Code + ' | ' + Description AS WHSECode " & _
                    " FROM      tblProdWarehouse INNER JOIN tblUser_Access " & _
                    " ON        tblProdWarehouse.Code = tblUser_Access.Code " & _
                    " AND       tblUser_Access.Status ='Active' AND tblProdWarehouse.Status ='Active' " & _
                    " AND       tblUser_Access.Type = 'Production' AND isAllowed = 1 " & _
                    " WHERE     UserID ='" & UserID & "' "
            SQL.ReadQuery(query)
            cbWHfrom.Items.Clear()
            Dim ctr As Integer = 0
            While SQL.SQLDR.Read
                cbWHfrom.Items.Add(SQL.SQLDR("WHSECode").ToString)
                ctr += 1
            End While
            If cbWHfrom.Items.Count > 0 Then
                cbWHfrom.SelectedIndex = 0
            End If
        End If
    End Sub


    Private Sub loadWH()
        If cbIssueTo.SelectedItem = "Warehouse" Then
            Dim query As String
            query = " SELECT tblWarehouse.Code + ' | ' + Description AS Description  FROM tblWarehouse WHERE Status ='Active' "
            SQL.ReadQuery(query)
            cbWHto.Items.Clear()
            While SQL.SQLDR.Read
                cbWHto.Items.Add(SQL.SQLDR("Description"))
            End While
        ElseIf cbIssueTo.SelectedItem = "Production" Then
            Dim query As String
            query = " SELECT tblProdWarehouse.Code + ' | ' + Description AS Description  FROM tblProdWarehouse WHERE Status ='Active' "
            SQL.ReadQuery(query)
            cbWHto.Items.Clear()
            While SQL.SQLDR.Read
                cbWHto.Items.Add(SQL.SQLDR("Description"))
            End While
        End If

    End Sub

    Private Function LoadWH(ByVal Type As String, WHSEtype As String, WHSECode As String) As String
        Dim query As String = ""
        If Type = "From" Then
            If WHSEtype = "Warehouse" Then
                query = " SELECT tblWarehouse.Code + ' | ' + Description AS WHSE " & _
                        " FROM tblWarehouse INNER JOIN tblUser_Access " & _
                        " ON tblWarehouse.Code = tblUser_Access.Code " & _
                        " AND tblUser_Access.Status ='Active' AND tblWarehouse.Status ='Active' " & _
                        " AND Type = 'Warehouse' AND isAllowed = 1 " & _
                        " WHERE UserID ='" & UserID & "' AND tblWarehouse.Code = '" & WHSECode & "'  "
            ElseIf WHSEtype = "Production" Then
                query = " SELECT    tblProdWarehouse.Code + ' | ' + Description AS WHSE " & _
                        " FROM      tblProdWarehouse INNER JOIN tblUser_Access " & _
                        " ON        tblProdWarehouse.Code = tblUser_Access.Code " & _
                        " AND       tblUser_Access.Status ='Active' AND tblProdWarehouse.Status ='Active' " & _
                        " AND       tblUser_Access.Type = 'Production' AND isAllowed = 1 " & _
                        " WHERE     UserID ='" & UserID & "'  AND tblProdWarehouse.Code = '" & WHSECode & "' "
            End If
        ElseIf Type = "To" Then
            If WHSEtype = "Warehouse" Then
                query = " SELECT    tblWarehouse.Code + ' | ' + Description AS WHSE " & _
                        " FROM      tblWarehouse WHERE Status ='Active' " & _
                        " AND       tblWarehouse.Code = '" & WHSECode & "'  "
            ElseIf WHSEtype = "Production" Then
                query = " SELECT    tblProdWarehouse.Code + ' | ' + Description AS WHSE " & _
                        " FROM      tblProdWarehouse WHERE Status ='Active' " & _
                        " AND       tblProdWarehouse.Code = '" & WHSECode & "'  "
            End If
        End If
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return SQL.SQLDR("WHSE").ToString
        Else
            Return ""
        End If
    End Function

    Private Sub EnableControl(ByVal Value As Boolean)
        cbWHto.Enabled = Value
        cbWHfrom.Enabled = Value
        cbIssuerFrom.Enabled = Value
        cbIssueTo.Enabled = Value
        btnSearchVCE.Enabled = Value
        txtVCEName.Enabled = Value
        dgvItemMaster.AllowUserToAddRows = Value
        dgvItemMaster.AllowUserToDeleteRows = Value
        dgvItemMaster.ReadOnly = Not Value
        If Value = True Then
            dgvItemMaster.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2
        Else
            dgvItemMaster.EditMode = DataGridViewEditMode.EditProgrammatically
        End If
        cbTransType.Enabled = Value
        txtRemarks.Enabled = Value
        dtpDocDate.Enabled = Value

        If TransAuto = True Then
            txtTransNum.Enabled = False
        Else
            txtTransNum.Enabled = Value
        End If

        dgcStockQTY.ReadOnly = True

    End Sub

    Private Sub tsbSearch_Click(sender As System.Object, e As System.EventArgs) Handles tsbSearch.Click
        If Not AllowAccess("GI_VIEW") Then
            msgRestricted()
        Else
            Dim f As New frmLoadTransactions
            f.ShowDialog("GI")
            TransID = f.transID
            LoadGI(TransID)
            f.Dispose()
        End If
    End Sub

    Private Sub LoadGI(ByVal ID As String)
        Dim query As String
        Dim WHSEfrom, WHSEto As String
        query = " SELECT   TransID, GI_No, VCECode, DateGI, Type, IssueFrom, IssueTo, WHSE_From, WHSE_To, Remarks, Status, MR_Ref, ISNULL(JO_Ref,0) AS JO_Ref, CostCenter, ISNULL(withConfirm,0) AS withConfirm " & _
                " FROM     tblGI " & _
                " WHERE    TransId = '" & ID & "' " & _
                " ORDER BY TransId "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            TransID = SQL.SQLDR("TransID").ToString
            GINo = SQL.SQLDR("GI_No").ToString
            txtTransNum.Text = GINo
            txtVCECode.Text = SQL.SQLDR("VCECode").ToString
            dtpDocDate.Text = SQL.SQLDR("DateGI").ToString
            txtRemarks.Text = SQL.SQLDR("Remarks").ToString
            txtStatus.Text = SQL.SQLDR("Status").ToString
            disableEvent = True
            cbIssuerFrom.SelectedItem = SQL.SQLDR("IssueFrom").ToString
            cbIssueTo.SelectedItem = SQL.SQLDR("IssueTo").ToString
            WHSEfrom = SQL.SQLDR("WHSE_From").ToString
            WHSEto = SQL.SQLDR("WHSE_To").ToString
            MR_ID = SQL.SQLDR("MR_Ref").ToString
            JO_ID = SQL.SQLDR("JO_Ref").ToString
            withConfirm = SQL.SQLDR("withConfirm").ToString
            cbCostCenter.SelectedItem = SQL.SQLDR("CostCenter").ToString
            cbTransType.SelectedItem = SQL.SQLDR("Type").ToString
            disableEvent = False
            RefreshTranType()
            txtMRRef.Text = LoadMRNo(MR_ID)
            txtJORef.Text = LoadJONo(JO_ID)
            If txtMRRef.Text = "" Then
                dgvItemMaster.Columns(dgcReqQTY.Index).Visible = False
            Else
                dgvItemMaster.Columns(dgcReqQTY.Index).Visible = True
            End If
            loadWHFrom()
            loadWHto()

            If cbIssuerFrom.SelectedItem = "Production" Then
                cbWHfrom.SelectedItem = GetWHSE(WHSEfrom, "tblProdWarehouse")
            Else
                cbWHfrom.SelectedItem = GetWHSE(WHSEfrom, "tblWarehouse")
            End If
            If cbIssueTo.SelectedItem = "Production" Then
                cbWHto.SelectedItem = GetWHSE(WHSEto, "tblProdWarehouse")
            Else
                cbWHto.SelectedItem = GetWHSE(WHSEto, "tblWarehouse")
            End If
            txtVCEName.Text = GetVCEName(txtVCECode.Text)
            LoadGIDetails(TransID)
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
            Cleartext()
        End If
    End Sub

    Private Function LoadMRNo(MR_ID As Integer) As String
        Dim query As String
        query = " SELECT MR_No FROM tblMR WHERE TransID = '" & MR_ID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return SQL.SQLDR("MR_No")
        Else
            Return ""
        End If
    End Function

    Private Function LoadJONo(MR_ID As Integer) As String
        Dim query As String
        query = " SELECT JO_No FROM tblJO WHERE TransID = '" & JO_ID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return SQL.SQLDR("JO_No")
        Else
            Return ""
        End If
    End Function

    Protected Sub LoadGIDetails(ByVal TransID As String)
        Dim ctr As Integer = 0
        Dim query As String
        query = " SELECT	tblGI_Details.ItemCode, tblGI_Details.Description, tblGI_Details.UOM, RequestQTY, StockQTY, IssueQTY, tblGI_Details.WHSE " & _
                " FROM	    tblGI INNER JOIN tblGI_Details " & _
                " ON		tblGI.TransID = tblGI_Details.TransID " & _
                " WHERE     tblGI_Details.TransId = " & TransID & " " & _
                " ORDER BY  tblGI_Details.LineNum "
        disableEvent = True
        dgvItemMaster.Rows.Clear()
        disableEvent = False
        SQL.ReadQuery(query)
        SQL.GetQuery(query)
        If SQL.SQLDS.Tables(0).Rows.Count > 0 Then
            Dim IssuedQTY As Decimal = 0
            Dim ConvertQTY As Decimal = 0
            For Each row As DataRow In SQL.SQLDS.Tables(0).Rows
                IssuedQTY = CDec(row(5))
                ConvertQTY = ConvertToBaseUOM(row(0).ToString, row(2).ToString, IssuedQTY)
                dgvItemMaster.Rows.Add(row(0).ToString, row(1).ToString, row(2).ToString, _
                                     CDec(row(3)).ToString("N2"), CDec(row(4)).ToString("N2"), CDec(IssuedQTY).ToString("N2"), GetWHSEDesc(row(6).ToString), CDec(ConvertQTY).ToString("N2"))
                LoadUOM(row(0).ToString, ctr)
                LoadWHSE(ctr)
                ctr += 1
            Next
        End If
    End Sub

    Private Function ConvertToBaseUOM(ByVal itemCode As String, UOM As String, QTY As Decimal)
        Dim query As String
        Dim ConvertQTY As Decimal = 0
        query = " SELECT ISNULL(QTY,0) as QTY  FROM viewItem_UOM WHERE GroupCode ='" & itemCode & "' AND UnitCode ='" & UOM & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            ConvertQTY = CDec(SQL.SQLDR("QTY")) * QTY
        End If
        Return ConvertQTY
    End Function


    Private Function ConvertToAltUOM(ByVal itemCode As String, UOM As String, QTY As Decimal)
        Dim query As String
        Dim ConvertQTY As Decimal = 0
        query = " SELECT ISNULL(QTY,0) as QTY  FROM viewItem_UOM WHERE GroupCode ='" & itemCode & "' AND UnitCode ='" & UOM & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            ConvertQTY = QTY / CDec(SQL.SQLDR("QTY"))
        End If
        Return ConvertQTY
    End Function

    Private Sub LoadWHSE(Optional ByVal SelectedIndex As Integer = -1)
        Try
            Dim dgvCB As New DataGridViewComboBoxCell
            dgvCB = dgvItemMaster.Item(chWHSE.Index, SelectedIndex)
            dgvCB.Items.Clear()
            ' ADD ALL WHSEc
            Dim query As String
            If cbIssueTo.SelectedItem = "Warehouse" Then
                query = " SELECT Description FROM tblWarehouse WHERE Status ='Active' "
            Else
                query = " SELECT Description FROM tblProdWarehouse WHERE Status ='Active' "
            End If
            SQL.ReadQuery(query)
            dgvCB.Items.Clear()
            While SQL.SQLDR.Read
                dgvCB.Items.Add(SQL.SQLDR("Description").ToString)
            End While
            dgvCB.Items.Add("")
        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        End Try
    End Sub

    Private Sub Cleartext()
        txtVCECode.Clear()
        txtVCEName.Clear()
        txtTransNum.Clear()
        txtMRRef.Clear()
        cbWHto.SelectedIndex = -1
        cbTransType.SelectedIndex = -1
        dgvItemMaster.Rows.Clear()
        txtRemarks.Clear()
        txtStatus.Text = "Open"
        dtpDocDate.Value = Date.Today.Date
    End Sub

    Private Sub tsbNew_Click(sender As System.Object, e As System.EventArgs) Handles tsbNew.Click
        If Not AllowAccess("GI_ADD") Then
            msgRestricted()
        Else
            Cleartext()
            TransID = ""
            GINo = ""


            dgvItemMaster.Columns(dgcReqQTY.Index).Visible = False

            ' Toolstrip Buttons
            tsbSearch.Enabled = False
            tsbNew.Enabled = False
            tsbEdit.Enabled = False
            tsbSave.Enabled = True
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
        If Not AllowAccess("GI_EDIT") Then
            msgRestricted()
        Else
            EnableControl(True)
            LoadStock()
            ' Toolstrip Buttons
            tsbSearch.Enabled = False
            tsbNew.Enabled = False
            tsbEdit.Enabled = False
            tsbSave.Enabled = True
            tsbCancel.Enabled = False
            tsbClose.Enabled = True
            tsbPrevious.Enabled = False
            tsbNext.Enabled = False
            tsbExit.Enabled = False
            tsbPrint.Enabled = False
            tsbCopy.Enabled = False
            RefreshTranType()
        End If
    End Sub

    Private Sub tsbSave_Click(sender As System.Object, e As System.EventArgs) Handles tsbSave.Click
        If DataValidated() Then
            If TransID = "" Then
                If MsgBox("Saving New Record, Click Yes to confirm", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "JADE Message Alert") = MsgBoxResult.Yes Then
                    TransID = GenerateTransID(ColumnID, DBTable)
                    GINo = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
                    txtTransNum.Text = GINo
                    SaveGI()
                    Msg("Record Saved Succesfully!", MsgBoxStyle.Information)
                    GINo = txtTransNum.Text
                    LoadGI(TransID)
                End If
            Else
                If MsgBox("Updating Record, Click Yes to confirm", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "JADE Message Alert") = MsgBoxResult.Yes Then
                    UpdateGI()
                    Msg("Record Updated Succesfully!", MsgBoxStyle.Information)
                    GINo = txtTransNum.Text
                    LoadGI(TransID)
                End If
            End If
        End If

    End Sub

    Private Function DataValidated() As Boolean
        'If cbWHto.SelectedIndex = -1 And withConfirm = True Then
        '    Msg("Please select warehouse!", MsgBoxStyle.Exclamation)
        '    Return False
        'ElseIf dgvItemMaster.Rows.Count <= 1 Then
        '    Msg("There are no items on the list!", MsgBoxStyle.Exclamation)
        '    Return False
        'ElseIf validateDGV() = False Then
        '    Return False
        'Else
        '    Return True
        'End 
        Return True
    End Function

    Private Function validateDGV() As Boolean
        Dim reqQTY, stockQTY, issueQTY As Decimal
        Dim value As Boolean = True
        For Each row As DataGridViewRow In dgvItemMaster.Rows
            If Not IsNumeric(row.Cells(dgcReqQTY.Index).Value) Then reqQTY = 0 Else reqQTY = row.Cells(dgcReqQTY.Index).Value
            If Not IsNumeric(row.Cells(dgcStockQTY.Index).Value) Then stockQTY = 0 Else stockQTY = row.Cells(dgcStockQTY.Index).Value
            If Not IsNumeric(row.Cells(dgcIssueQTY.Index).Value) Then issueQTY = 0 Else issueQTY = row.Cells(dgcIssueQTY.Index).Value

            If issueQTY > stockQTY Then
                Msg("Issue Quantity should not be greater than Stock Quantity", MsgBoxStyle.Exclamation)
                value = False
                Exit For
            End If
        Next
        Return value
    End Function


    Private Sub SaveGI()
        Try
            Dim WHSEfrom, WHSEto, IssueTo, Type, CC As String
            Type = IIf(cbTransType.SelectedIndex = -1, "", cbTransType.SelectedItem)

            WHSEfrom = GetWHSE(cbWHfrom.SelectedItem)

            If cbIssueTo.SelectedIndex = -1 Then IssueTo = "" Else IssueTo = cbIssueTo.SelectedItem
            If cbWHto.SelectedIndex = -1 Then WHSEto = "" Else WHSEto = GetWHSE(cbWHto.SelectedItem)
            If cbCostCenter.SelectedIndex = -1 Then CC = "" Else CC = GetCCCode(cbCostCenter.SelectedItem)

            activityStatus = True
            Dim insertSQL As String
            insertSQL = " INSERT INTO " & _
                        " tblGI(TransID, GI_No, BranchCode, BusinessCode, DateGI, VCECode, IssueFrom, IssueTo, WHSE_From, WHSE_To, Type, Remarks, CostCenter, withConfirm, JO_Ref, MR_Ref, WhoCreated) " & _
                        " VALUES (@TransID, @GI_No, @BranchCode, @BusinessCode, @DateGI, @VCECode, @IssueFrom, @IssueTo, @WHSE_From, @WHSE_To, @Type, @Remarks, @CostCenter, @withConfirm, @JO_Ref, @MR_Ref, @WhoCreated) "
            SQL.FlushParams()
            SQL.AddParam("@TransID", TransID)
            SQL.AddParam("@GI_No", GINo)
            SQL.AddParam("@BranchCode", BranchCode)
            SQL.AddParam("@BusinessCode", BusinessType)
            SQL.AddParam("@DateGI", dtpDocDate.Value.Date)
            SQL.AddParam("@VCECode", txtVCECode.Text)
            SQL.AddParam("@IssueFrom", cbIssuerFrom.SelectedItem)
            SQL.AddParam("@IssueTo", IssueTo)
            SQL.AddParam("@WHSE_From", WHSEfrom)
            SQL.AddParam("@WHSE_To", WHSEto)
            SQL.AddParam("@withConfirm", withConfirm)
            SQL.AddParam("@CostCenter", CC)
            If IsNumeric(MR_ID) Then SQL.AddParam("@MR_Ref", MR_ID) Else SQL.AddParam("@MR_Ref", 0)
            If IsNumeric(JO_ID) Then SQL.AddParam("@JO_Ref", JO_ID) Else SQL.AddParam("@JO_Ref", 0)
            SQL.AddParam("@Type", Type)
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@WhoCreated", UserID)
            SQL.ExecNonQuery(insertSQL)

            Dim line As Integer = 1
            Dim ItemCode, Description, UOM As String
            Dim reqQTY, stockQTY, issueQTY, UnitCost As Decimal
            For Each row As DataGridViewRow In dgvItemMaster.Rows
                If Not row.Cells(dgcIssueQTY.Index).Value Is Nothing AndAlso Not row.Cells(chItemCode.Index).Value Is Nothing Then
                    ItemCode = IIf(row.Cells(chItemCode.Index).Value = Nothing, "", row.Cells(chItemCode.Index).Value)
                    Description = IIf(row.Cells(chItemName.Index).Value = Nothing, "", row.Cells(chItemName.Index).Value)
                    UOM = IIf(row.Cells(chUOM.Index).Value = Nothing, "", row.Cells(chUOM.Index).Value)
                    UnitCost = GetAverageCost(ItemCode)
                    If IsNumeric(row.Cells(dgcReqQTY.Index).Value) Then reqQTY = CDec(row.Cells(dgcReqQTY.Index).Value) Else reqQTY = 1
                    If IsNumeric(row.Cells(dgcStockQTY.Index).Value) Then stockQTY = CDec(row.Cells(dgcStockQTY.Index).Value) Else stockQTY = 1
                    If IsNumeric(row.Cells(dgcIssueQTY.Index).Value) Then issueQTY = CDec(row.Cells(dgcIssueQTY.Index).Value) Else issueQTY = 1
                    insertSQL = " INSERT INTO " & _
                         " tblGI_Details(TransId, ItemCode, Description, UOM, RequestQTY, StockQTY, IssueQTY, WHSE, LineNum, WhoCreated) " & _
                         " VALUES(@TransId, @ItemCode, @Description, @UOM, @RequestQTY, @StockQTY, @IssueQTY, @WHSE, @LineNum, @WhoCreated) "
                    SQL.FlushParams()
                    SQL.AddParam("@TransID", TransID)
                    SQL.AddParam("@ItemCode", ItemCode)
                    SQL.AddParam("@Description", Description)
                    SQL.AddParam("@UOM", UOM)
                    SQL.AddParam("@RequestQTY", reqQTY)
                    SQL.AddParam("@StockQTY", stockQTY)
                    SQL.AddParam("@IssueQTY", issueQTY)
                    SQL.AddParam("@WHSE", WHSEto)
                    SQL.AddParam("@LineNum", line)
                    SQL.AddParam("@WhoCreated", UserID)
                    SQL.ExecNonQuery(insertSQL)
                    line += 1

                    If withConfirm = False Then
                        SaveINVTY("OUT", ModuleID, "GI", TransID, dtpDocDate.Value.Date, ItemCode, WHSEfrom, issueQTY, UOM, UnitCost)
                    End If

                End If
            Next
        Catch ex As Exception
            activityStatus = False
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        Finally
            RecordActivity(UserID, ModuleID, Me.Name.ToString, "INSERT", "GI_No", txtTransNum.Text, BusinessType, BranchCode, "", activityStatus)
            SQL.FlushParams()
        End Try
    End Sub

    Private Sub UpdateGI()
        Try
            Dim WHSEfrom, WHSEto, Type, IssueTo, CC As String
            Type = IIf(cbTransType.SelectedIndex = -1, "", cbTransType.SelectedItem)

            If cbIssueTo.SelectedIndex = -1 Then IssueTo = "" Else IssueTo = cbIssueTo.SelectedItem
            If cbWHto.SelectedIndex = -1 Then WHSEto = "" Else WHSEto = GetWHSE(cbWHto.SelectedItem)
            If cbCostCenter.SelectedIndex = -1 Then CC = "" Else CC = GetCCCode(cbCostCenter.SelectedItem)
            WHSEfrom = GetWHSE(cbWHfrom.SelectedItem)
            activityStatus = True
            Dim insertSQL, updateSQL, deleteSQL As String
            updateSQL = " UPDATE    tblGI " & _
                        " SET       GI_No = @GI_No, BranchCode = @BranchCode, BusinessCode = @BusinessCode, DateGI = @DateGI, VCECode = @VCECode, " & _
                        "           IssueFrom = @IssueFrom, IssueTo = @IssueTo, WHSE_From = @WHSE_From, WHSE_To = @WHSE_To, Type = @Type, Remarks = @Remarks, " & _
                        "           CostCenter = @CostCenter, withConfirm = @withConfirm, JO_Ref = @JO_Ref, MR_Ref = @MR_Ref, WhoModified = @WhoModified, DateModified = GETDATE() " & _
                        " WHERE     TransID = @TransID  "
            SQL.FlushParams()
            SQL.AddParam("@TransID", TransID)
            SQL.AddParam("@GI_No", GINo)
            SQL.AddParam("@BranchCode", BranchCode)
            SQL.AddParam("@BusinessCode", BusinessType)
            SQL.AddParam("@DateGI", dtpDocDate.Value.Date)
            SQL.AddParam("@VCECode", txtVCECode.Text)
            SQL.AddParam("@IssueFrom", cbIssuerFrom.SelectedItem)
            SQL.AddParam("@IssueTo", IssueTo)
            SQL.AddParam("@WHSE_From", WHSEfrom)
            SQL.AddParam("@WHSE_To", WHSEto)
            SQL.AddParam("@withConfirm", withConfirm)
            SQL.AddParam("@CostCenter", CC)
            If IsNumeric(MR_ID) Then SQL.AddParam("@MR_Ref", MR_ID) Else SQL.AddParam("@MR_Ref", 0)
            If IsNumeric(JO_ID) Then SQL.AddParam("@JO_Ref", JO_ID) Else SQL.AddParam("@JO_Ref", 0)
            SQL.AddParam("@Type", Type)
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@WhoModified", UserID)
            SQL.ExecNonQuery(updateSQL)

            deleteSQL = " DELETE FROM tblGI_Details WHERE TransID =@TransID "
            SQL.FlushParams()
            SQL.AddParam("@TransID", TransID)
            SQL.ExecNonQuery(deleteSQL)

            DELINVTY(ModuleID, "GI", TransID)

            Dim line As Integer = 1
            Dim ItemCode, Description, UOM As String
            Dim reqQTY, stockQTY, issueQTY, UnitCost As Decimal
            For Each row As DataGridViewRow In dgvItemMaster.Rows
                If Not row.Cells(dgcIssueQTY.Index).Value Is Nothing AndAlso Not row.Cells(chItemCode.Index).Value Is Nothing Then
                    ItemCode = IIf(row.Cells(chItemCode.Index).Value = Nothing, "", row.Cells(chItemCode.Index).Value)
                    Description = IIf(row.Cells(chItemName.Index).Value = Nothing, "", row.Cells(chItemName.Index).Value)
                    UOM = IIf(row.Cells(chUOM.Index).Value = Nothing, "", row.Cells(chUOM.Index).Value)
                    UnitCost = GetAverageCost(ItemCode)
                    If IsNumeric(row.Cells(dgcReqQTY.Index).Value) Then reqQTY = CDec(row.Cells(dgcReqQTY.Index).Value) Else reqQTY = 1
                    If IsNumeric(row.Cells(dgcStockQTY.Index).Value) Then stockQTY = CDec(row.Cells(dgcStockQTY.Index).Value) Else stockQTY = 1
                    If IsNumeric(row.Cells(dgcIssueQTY.Index).Value) Then issueQTY = CDec(row.Cells(dgcIssueQTY.Index).Value) Else issueQTY = 1
                    insertSQL = " INSERT INTO " & _
                         " tblGI_Details(TransId, ItemCode, Description, UOM, RequestQTY, StockQTY, IssueQTY, WHSE, LineNum, WhoCreated) " & _
                         " VALUES(@TransId, @ItemCode, @Description, @UOM, @RequestQTY, @StockQTY, @IssueQTY, @WHSE, @LineNum, @WhoCreated) "
                    SQL.FlushParams()
                    SQL.AddParam("@TransID", TransID)
                    SQL.AddParam("@ItemCode", ItemCode)
                    SQL.AddParam("@Description", Description)
                    SQL.AddParam("@UOM", UOM)
                    SQL.AddParam("@RequestQTY", reqQTY)
                    SQL.AddParam("@StockQTY", stockQTY)
                    SQL.AddParam("@IssueQTY", issueQTY)
                    SQL.AddParam("@WHSE", WHSEto)
                    SQL.AddParam("@LineNum", line)
                    SQL.AddParam("@WhoCreated", UserID)
                    SQL.ExecNonQuery(insertSQL)
                    line += 1

                    If withConfirm = False Then
                        SaveINVTY("OUT", ModuleID, "GI", TransID, dtpDocDate.Value.Date, ItemCode, WHSEfrom, issueQTY, UOM, UnitCost)
                    End If
                End If
            Next
        Catch ex As Exception
            activityStatus = False
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        Finally
            RecordActivity(UserID, ModuleID, Me.Name.ToString, "UPDATE", "GI_No", txtTransNum.Text, BusinessType, BranchCode, "", activityStatus)
            SQL.FlushParams()
        End Try

    End Sub

    Private Sub tsbCancel_Click(sender As System.Object, e As System.EventArgs) Handles tsbCancel.Click
        If Not AllowAccess("GI_DEL") Then
            msgRestricted()
        Else
            If txtTransNum.Text <> "" Then
                If MsgBox("Are you sure you want to cancel this record?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "JADE Message Alert") = MsgBoxResult.Yes Then
                    Try
                        activityStatus = True
                        Dim deleteSQL As String
                        deleteSQL = " UPDATE  tblGI SET Status ='Cancelled' WHERE GI_No = @GI_No "
                        SQL.FlushParams()
                        GINo = txtTransNum.Text
                        SQL.AddParam("@GI_No", GINo)
                        SQL.ExecNonQuery(deleteSQL)
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

                        Dim WHSEfrom As String
                        WHSEfrom = GetWHSE(cbWHfrom.SelectedItem)

                        Dim line As Integer = 1
                        Dim ItemCode, Description, UOM As String
                        Dim reqQTY, stockQTY, issueQTY, UnitCost As Decimal
                        For Each row As DataGridViewRow In dgvItemMaster.Rows
                            If Not row.Cells(dgcIssueQTY.Index).Value Is Nothing AndAlso Not row.Cells(chItemCode.Index).Value Is Nothing Then
                                ItemCode = IIf(row.Cells(chItemCode.Index).Value = Nothing, "", row.Cells(chItemCode.Index).Value)
                                Description = IIf(row.Cells(chItemName.Index).Value = Nothing, "", row.Cells(chItemName.Index).Value)
                                UOM = IIf(row.Cells(chUOM.Index).Value = Nothing, "", row.Cells(chUOM.Index).Value)
                                UnitCost = GetAverageCost(ItemCode)
                                If IsNumeric(row.Cells(dgcReqQTY.Index).Value) Then reqQTY = CDec(row.Cells(dgcReqQTY.Index).Value) Else reqQTY = 1
                                If IsNumeric(row.Cells(dgcStockQTY.Index).Value) Then stockQTY = CDec(row.Cells(dgcStockQTY.Index).Value) Else stockQTY = 1
                                If IsNumeric(row.Cells(dgcIssueQTY.Index).Value) Then issueQTY = CDec(row.Cells(dgcIssueQTY.Index).Value) Else issueQTY = 1
                                line += 1

                                If withConfirm = False Then
                                    SaveINVTY("IN", ModuleID, "GI", TransID, dtpDocDate.Value.Date, ItemCode, WHSEfrom, issueQTY, UOM, UnitCost)
                                End If

                            End If
                        Next


                        LoadGI(TransID)
                    Catch ex As Exception
                        activityStatus = True
                        SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
                    Finally
                        RecordActivity(UserID, ModuleID, Me.Name.ToString, "CANCEL", "GI_No", GINo, BusinessType, BranchCode, "", activityStatus)
                        SQL.FlushParams()
                    End Try
                End If
            End If
        End If
    End Sub

    Private Sub tsbPrevious_Click(sender As System.Object, e As System.EventArgs) Handles tsbPrevious.Click
        If GINo <> "" Then
            Dim query As String
            query = " SELECT Top 1 TransID " & _
                    "  FROM     tblGI LEFT JOIN tblVCE_Master " & _
                    "  ON	   tblGI .VCECode = tblVCE_Master.VCECode " & _
                    "   LEFT JOIN tblProdWarehouse  ON	          " & _
                    "   tblGI.WHSE_From = tblProdWarehouse.Code    " & _
                    "   WHERE          " & _
                    "   ( WHSE_From IN (SELECT    DISTINCT tblWarehouse.Code  " & _
                    "   FROM      tblWarehouse   " & _
                    "   INNER JOIN tblUser_Access    ON          " & _
                    "   tblWarehouse.Code = tblUser_Access.Code   " & _
                    "    AND       tblUser_Access.Status ='Active' AND tblWarehouse.Status ='Active'   " & _
                    "    AND       tblUser_Access.Type = 'Warehouse' AND isAllowed = 1  " & _
                    "    WHERE     UserID ='" & UserID & "')                  " & _
                    "    OR  " & _
                    "     WHSE_From IN (SELECT    DISTINCT tblProdWarehouse.Code   " & _
                    "     FROM      tblProdWarehouse INNER JOIN tblUser_Access     " & _
                    "      ON        tblProdWarehouse.Code = tblUser_Access.Code    " & _
                    "  	 AND       tblUser_Access.Status ='Active' AND tblProdWarehouse.Status ='Active'    " & _
                    "  	 AND       tblUser_Access.Type = 'Production' AND isAllowed = 1    " & _
                    "  	 WHERE     UserID ='" & UserID & "'))  AND GI_No < '" & GINo & "' ORDER BY GI_No DESC "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                TransID = SQL.SQLDR("TransID").ToString
                LoadGI(TransID)
            Else
                Msg("Reached the beginning of record!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub

    Private Sub tsbNext_Click(sender As System.Object, e As System.EventArgs) Handles tsbNext.Click
        If GINo <> "" Then
            Dim query As String
            query = " SELECT Top 1 TransID " & _
                    "  FROM     tblGI LEFT JOIN tblVCE_Master " & _
                    "  ON	   tblGI .VCECode = tblVCE_Master.VCECode " & _
                    "   LEFT JOIN tblProdWarehouse  ON	          " & _
                    "   tblGI.WHSE_From = tblProdWarehouse.Code    " & _
                    "   WHERE          " & _
                    "   ( WHSE_From IN (SELECT    DISTINCT tblWarehouse.Code  " & _
                    "   FROM      tblWarehouse   " & _
                    "   INNER JOIN tblUser_Access    ON          " & _
                    "   tblWarehouse.Code = tblUser_Access.Code   " & _
                    "    AND       tblUser_Access.Status ='Active' AND tblWarehouse.Status ='Active'   " & _
                    "    AND       tblUser_Access.Type = 'Warehouse' AND isAllowed = 1  " & _
                    "    WHERE     UserID ='" & UserID & "')                  " & _
                    "    OR  " & _
                    "     WHSE_From IN (SELECT    DISTINCT tblProdWarehouse.Code   " & _
                    "     FROM      tblProdWarehouse INNER JOIN tblUser_Access     " & _
                    "      ON        tblProdWarehouse.Code = tblUser_Access.Code    " & _
                    "  	 AND       tblUser_Access.Status ='Active' AND tblProdWarehouse.Status ='Active'    " & _
                    "  	 AND       tblUser_Access.Type = 'Production' AND isAllowed = 1    " & _
                    "  	 WHERE     UserID ='" & UserID & "'))  AND GI_No > '" & GINo & "' ORDER BY GI_No ASC "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                TransID = SQL.SQLDR("TransID").ToString
                LoadGI(TransID)
            Else
                Msg("Reached the end of record!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub

    Private Sub tsbClose_Click(sender As System.Object, e As System.EventArgs) Handles tsbClose.Click


        ' Toolstrip Buttons
        If GINo = "" Then
            Cleartext()
            EnableControl(False)
            tsbEdit.Enabled = False
            tsbCancel.Enabled = False
            tsbPrevious.Enabled = False
            tsbNext.Enabled = False
            tsbPrint.Enabled = False
        Else
            LoadGI(TransID)
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

    Private Sub tsbExit_Click(sender As System.Object, e As System.EventArgs) Handles tsbExit.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub frmGI_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
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

    Private Sub txtVCEName_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtVCEName.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim f As New frmVCE_Search
            f.cbFilter.SelectedItem = "VCEName"
            f.txtFilter.Text = txtVCEName.Text
            f.ShowDialog()
            txtVCECode.Text = f.VCECode
            txtVCEName.Text = f.VCEName
        End If
    End Sub

    Private Sub dgvItemMaster_CellEndEdit(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvItemMaster.CellEndEdit
        Try
            Dim itemCode As String
            Dim rowIndex As Integer = dgvItemMaster.CurrentCell.RowIndex
            Dim colindex As Integer = dgvItemMaster.CurrentCell.ColumnIndex
            Select Case colindex
                Case chItemCode.Index
                    If dgvItemMaster.Item(chItemCode.Index, e.RowIndex).Value <> "" Then
                        itemCode = dgvItemMaster.Item(chItemCode.Index, e.RowIndex).Value
                        Dim f As New frmCopyFrom
                        f.ShowDialog("All Item", itemCode)
                        If f.TransID <> "" Then
                            itemCode = f.TransID
                            LoadItemDetails(itemCode)
                        Else
                            dgvItemMaster.Rows.RemoveAt(e.RowIndex)
                        End If
                        f.Dispose()
                    End If
                Case chItemName.Index
                    If dgvItemMaster.Item(chItemName.Index, e.RowIndex).Value <> "" Then
                        itemCode = dgvItemMaster.Item(chItemName.Index, e.RowIndex).Value
                        Dim f As New frmCopyFrom
                        f.ShowDialog("All Item", itemCode)
                        If f.TransID <> "" Then
                            itemCode = f.TransID
                            LoadItemDetails(itemCode)
                        Else
                            dgvItemMaster.Rows.RemoveAt(e.RowIndex)
                        End If
                        f.Dispose()
                    End If
                Case dgcReqQTY.Index
                    If dgvItemMaster.Item(chItemName.Index, e.RowIndex).Value <> "" Then
                        LoadStock()
                    End If
            End Select
        Catch ex1 As InvalidOperationException

        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        End Try

    End Sub

    Private Sub LoadItemDetails(ByVal ItemCode As String)
        Dim query As String
        query = " SELECT    ItemCode, ItemName, 1 " & _
                " FROM       tblItem_Master " & _
                " WHERE     ItemCode = @ItemCode"
        SQL.FlushParams()
        SQL.AddParam("@ItemCode", IIf(ItemCode = Nothing, "", ItemCode))
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            If dgvItemMaster.SelectedCells.Count > 0 Then

                dgvItemMaster.Item(0, dgvItemMaster.SelectedCells(0).RowIndex).Value = SQL.SQLDR("ItemCode").ToString
                dgvItemMaster.Item(1, dgvItemMaster.SelectedCells(0).RowIndex).Value = SQL.SQLDR("ItemName").ToString
                dgvItemMaster.Item(2, dgvItemMaster.SelectedCells(0).RowIndex).Value = ""
                dgvItemMaster.Item(3, dgvItemMaster.SelectedCells(0).RowIndex).Value = 1
                dgvItemMaster.Item(4, dgvItemMaster.SelectedCells(0).RowIndex).Value = 1
                dgvItemMaster.Item(5, dgvItemMaster.SelectedCells(0).RowIndex).Value = 1
                dgvItemMaster.Item(6, dgvItemMaster.SelectedCells(0).RowIndex).Value = IIf(cbWHto.SelectedIndex = -1, "", cbWHto.SelectedItem)
                LoadWHSE(dgvItemMaster.SelectedCells(0).RowIndex)
                LoadUOM(ItemCode, dgvItemMaster.SelectedCells(0).RowIndex)
                LoadStock()
            End If

        End If
    End Sub


    Private Sub LoadUOM(ItemCode As String, ByVal SelectedIndex As Integer)
        Try
            Dim dgvCB As New DataGridViewComboBoxCell
            dgvCB = dgvItemMaster.Item(chUOM.Index, SelectedIndex)
            dgvCB.Items.Clear()
            ' ADD ALL WHSEc
            Dim query As String

            query = " SELECT DISTINCT UOM.UnitCode FROM tblItem_Master INNER JOIN  " & _
               " ( " & _
               " SELECT GroupCode, UnitCode FROM tblUOM_Group WHERE Status ='Active' " & _
               " UNION ALL " & _
               " SELECT GroupCode, UnitCodeFrom FROM tblUOM_GroupDetails " & _
               " UNION ALL " & _
               " SELECT GroupCode, UnitCodeTo FROM tblUOM_GroupDetails " & _
               " ) AS UOM " & _
               " ON tblItem_Master.ItemUOMGroup = UOM.GroupCode " & _
               " OR  tblItem_Master.ItemCode = UOM.GroupCode " & _
               " WHERE ItemCode ='" & ItemCode & "' "
            SQL.ReadQuery(query)
            dgvCB.Items.Clear()
            While SQL.SQLDR.Read
                dgvCB.Items.Add(SQL.SQLDR("UnitCode").ToString)
            End While
            dgvCB.Items.Add("")
        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        End Try
    End Sub

    Private Sub dgvItemMaster_CellClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvItemMaster.CellClick
        Try
            If e.ColumnIndex = chUOM.Index Then
                If e.RowIndex <> -1 AndAlso dgvItemMaster.Rows.Count > 0 AndAlso dgvItemMaster.Item(chItemCode.Index, e.RowIndex).Value <> Nothing Then

                    LoadUOM(dgvItemMaster.Item(chItemCode.Index, e.RowIndex).Value.ToString, e.RowIndex)
                    Dim dgvCol As New DataGridViewComboBoxColumn
                    dgvCol = dgvItemMaster.Columns.Item(e.ColumnIndex)
                    dgvItemMaster.BeginEdit(True)
                    dgvCol.Selected = True
                    DirectCast(dgvItemMaster.EditingControl, DataGridViewComboBoxEditingControl).DroppedDown = True
                    Dim editingComboBox As ComboBox = TryCast(sender, ComboBox)

                End If
            ElseIf e.ColumnIndex = chWHSE.Index Then
                If e.RowIndex <> -1 AndAlso dgvItemMaster.Rows.Count > 0 AndAlso dgvItemMaster.Item(chItemCode.Index, e.RowIndex).Value <> Nothing Then
                    LoadWHSE(e.RowIndex)
                    Dim dgvCol As DataGridViewComboBoxColumn
                    dgvCol = dgvItemMaster.Columns.Item(e.ColumnIndex)
                    dgvItemMaster.BeginEdit(True)
                    dgvCol.Selected = True
                    DirectCast(dgvItemMaster.EditingControl, DataGridViewComboBoxEditingControl).DroppedDown = True
                    Dim editingComboBox As ComboBox = TryCast(sender, ComboBox)

                End If
            End If

        Catch ex As NullReferenceException
            If dgvItemMaster.ReadOnly = False Then
                SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
            End If
        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        End Try
    End Sub

    Private Sub dgvItemMaster_EditingControlShowing(sender As System.Object, e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgvItemMaster.EditingControlShowing
        ' GET THE EDITING CONTROL
        Dim editingComboBox As ComboBox = TryCast(e.Control, ComboBox)
        If Not editingComboBox Is Nothing Then
            ' REMOVE AN EXISTING EVENT-HANDLER TO AVOID ADDING MULTIPLE HANDLERS WHEN THE EDITING CONTROL IS REUSED
            RemoveHandler editingComboBox.SelectionChangeCommitted, New EventHandler(AddressOf editingComboBox_SelectionChangeCommitted)

            ' ADD THE EVENT HANDLER
            AddHandler editingComboBox.SelectionChangeCommitted, AddressOf editingComboBox_SelectionChangeCommitted

            ' PREVENT THIS HANDLER FROM FIRING TWICE
            RemoveHandler dgvItemMaster.EditingControlShowing, AddressOf dgvItemMaster_EditingControlShowing
        End If
    End Sub

    Private Sub editingComboBox_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim rowIndex, columnIndex As Integer
        'Get the editing control
        Dim editingComboBox As ComboBox = TryCast(sender, ComboBox)
        If editingComboBox Is Nothing Then Exit Sub
        'Show your Message Boxes
        If editingComboBox.SelectedIndex <> -1 Then
            rowIndex = dgvItemMaster.SelectedCells(0).RowIndex
            columnIndex = dgvItemMaster.SelectedCells(0).ColumnIndex
            If dgvItemMaster.SelectedCells.Count > 0 Then
                Dim tempCell As DataGridViewComboBoxCell = dgvItemMaster.Item(columnIndex, rowIndex)
                Dim temp As String = editingComboBox.Text
                dgvItemMaster.Item(columnIndex, rowIndex).Selected = False
                dgvItemMaster.EndEdit(True)
                tempCell.Value = temp
                LoadStock()
            End If
        End If

        'Remove the handle to this event. It will be readded each time a new combobox selection causes the EditingControlShowing Event to fire
        RemoveHandler editingComboBox.SelectionChangeCommitted, AddressOf editingComboBox_SelectionChangeCommitted
        'Re-enable the EditingControlShowing event so the above can take place.
        AddHandler dgvItemMaster.EditingControlShowing, AddressOf dgvItemMaster_EditingControlShowing
    End Sub

    Private Sub dgvItemMaster_DataError(sender As Object, e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles dgvItemMaster.DataError
        Try

        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        End Try
    End Sub

    Private Sub btnSearchVCE_Click(sender As System.Object, e As System.EventArgs) Handles btnSearchVCE.Click
        Dim f As New frmVCE_Search
        f.ShowDialog()
        txtVCECode.Text = f.VCECode
        txtVCEName.Text = f.VCEName
    End Sub

    Private Sub cbIssueTo_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbIssueTo.SelectedIndexChanged
        If disableEvent = False Then
            loadWHto()
        End If
    End Sub

    Private Sub loadWHto()
        If cbIssueTo.SelectedItem = "Warehouse" Then
            txtVCECode.ReadOnly = False
            txtVCEName.ReadOnly = False
            btnSearchVCE.Enabled = True
        ElseIf cbIssueTo.SelectedItem = "Production" Then
            txtVCECode.ReadOnly = False
            txtVCEName.ReadOnly = False
            btnSearchVCE.Enabled = True
        End If
        loadWH()
        For Each row As DataGridViewRow In dgvItemMaster.Rows
            If row.Cells(chItemCode.Index).Value <> Nothing AndAlso row.Cells(chItemCode.Index).Value <> "" Then
                LoadWHSE(row.Index)
            End If
        Next
    End Sub

    Private Sub cbIssuerFrom_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbIssuerFrom.SelectedIndexChanged
        If disableEvent = False Then
            loadWHFrom()
        End If

    End Sub

    Private Sub LoadStock()
        Dim WHSE As String
        If cbWHfrom.SelectedIndex = -1 Then
            WHSE = ""
        Else
            WHSE = GetWHSE(cbWHfrom.SelectedItem)
        End If

        Dim query As String
        Dim itemCode, UOM As String
        Dim StockQTY As Decimal = 0
        Dim IssueQTY As Decimal = 0
        Dim IssuedQTY As Decimal = 0
        Dim ReqQTY As Decimal = 0
        For Each row As DataGridViewRow In dgvItemMaster.Rows
            If Not IsNothing(row.Cells(chItemCode.Index).Value) Then
                itemCode = row.Cells(chItemCode.Index).Value.ToString
                If Not IsNothing(row.Cells(chUOM.Index).Value) Then UOM = row.Cells(chUOM.Index).Value Else UOM = ""
                If Not IsNumeric(row.Cells(dgcReqQTY.Index).Value) Then ReqQTY = 0 Else ReqQTY = CDec(row.Cells(dgcReqQTY.Index).Value)
                If Not IsNumeric(row.Cells(dgcIssuedQTY.Index).Value) Then IssuedQTY = 0 Else IssuedQTY = CDec(row.Cells(dgcIssuedQTY.Index).Value)

                query = "   SELECT	    ISNULL(SUM(QTY),0) AS QTY " & _
                        "   FROM		viewItem_Stock " & _
                        "   WHERE       ItemCode ='" & itemCode & "' " & _
                        "   AND         WHSE = '" & WHSE & "' " & _
                        "   AND         UOM = '" & UOM & "' "
                SQL.ReadQuery(query)
                If SQL.SQLDR.Read Then
                    StockQTY = SQL.SQLDR("QTY") + ConvertToAltUOM(itemCode, UOM, IssuedQTY)
                    If StockQTY >= ReqQTY Then ' IF AVAILABLE STOCK IS GREATER THAN THE BOM REQUIREMENT THEN ISSUE QTY SHOULD BE THE SAME AS BOM QTY
                        IssueQTY = ReqQTY
                    Else ' IF AVAILABLE STOCK IS LESS THAN THE BOM REQUIREMENT THEN ISSUE QTY SHOULD BE ONLY THE STOCK QTY
                        IssueQTY = StockQTY
                    End If
                End If
                row.Cells(dgcStockQTY.Index).Value = CDec(StockQTY).ToString("N2")
                row.Cells(dgcIssueQTY.Index).Value = CDec(IssueQTY).ToString("N2")

            End If

        Next
        dgvItemMaster.Columns(dgcStockQTY.Index).Visible = True
    End Sub

    Private Sub cbWHfrom_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbWHfrom.SelectedIndexChanged
        If MR_ID <> 0 Then
            LoadMRDEtails(MR_ID)
        Else
            LoadStock()
        End If

    End Sub

    Private Sub tsbCopyPR_Click(sender As System.Object, e As System.EventArgs) Handles tsbCopyPR.Click
        Dim f As New frmLoadTransactions
        f.cbFilter.SelectedItem = "Status"
        f.txtFilter.Text = "Active"
        f.txtFilter.Enabled = False
        f.cbFilter.Enabled = False
        f.btnSearch.Enabled = False
        f.ShowDialog("MR-GI")
        LoadMR(f.transID)
        f.Dispose()
    End Sub

    Private Sub LoadMR(ByVal ID As String)
        Try
            Dim WHSEfrom, WHSEto As String
            Dim query As String
            query = " SELECT TransID, MR_No, WHSE_From, WHSE_To, DateMR, Remarks " & _
                    " FROM   tblMR " & _
                    " WHERE  TransID ='" & ID & "' "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                MR_ID = SQL.SQLDR("TransID")
                txtMRRef.Text = SQL.SQLDR("MR_No").ToString
                WHSEfrom = SQL.SQLDR("WHSE_From").ToString
                WHSEto = SQL.SQLDR("WHSE_To").ToString
                cbTransType.SelectedItem = "Material Transfer"
                If Strings.Left(WHSEto, 2) = "PL" Then
                    cbIssuerFrom.SelectedItem = "Production"
                Else
                    cbIssuerFrom.SelectedItem = "Warehouse"
                End If
                cbIssueTo.SelectedItem = "Production"
                loadWHFrom()
                loadWH()
                If Strings.Left(WHSEto, 2) = "PL" Then
                    cbWHfrom.SelectedItem = loadWH("From", "Production", WHSEto)
                Else
                    cbWHfrom.SelectedItem = loadWH("From", "Warehouse", WHSEto)
                End If

                cbWHto.SelectedItem = loadWH("To", "Production", WHSEfrom)

                LoadMRDEtails(MR_ID)
            End If
        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        End Try
    End Sub

    Private Sub LoadMRDEtails(ByVal ID As Integer)
        Dim query, WHSE As String
        WHSE = GetWHSE(cbWHfrom.SelectedItem)

        query = " SELECT tblMR_Details.ItemCode, Description, UOM, Unserved AS QTY, viewMR_Unserved.WHSE " & _
                " FROM   tblMR_Details INNER JOIN viewMR_Unserved " & _
                " ON     tblMR_Details.RecordID = viewMR_Unserved.RecordID " & _
                " WHERE  tblMR_Details.TransID ='" & ID & "' AND tblMR_Details.WHSE = '" & WHSE & "'"
        SQL.GetQuery(query)
        dgvItemMaster.Columns(dgcReqQTY.Index).Visible = True
        dgvItemMaster.Rows.Clear()
        Dim ctr As Integer = 0
        Dim ctrGroup As Integer = 0
        If SQL.SQLDS.Tables(0).Rows.Count > 0 Then
            For Each row As DataRow In SQL.SQLDS.Tables(0).Rows
                dgvItemMaster.Rows.Add(row(0).ToString, row(1).ToString, row(2).ToString, _
                              CDec(row(3)).ToString("N2"), 0, 0, GetWHSEDesc(row(4).ToString))
                LoadWHSE(ctr)
                LoadUOM(row(0).ToString, ctr)
                ctr += 1
            Next

            LoadStock()
        End If
    End Sub

    Private Sub LoadJO(ByVal ID As String)
        Try
            Dim query As String

            cbTransType.SelectedItem = "Issuance to Job"

            query = " SELECT  tblBOM_Details.ItemCode, tblBOM_Details.Description, tblBOM_Details.UOM, tblBOM_Details.GrossQTY  " & _
                    " FROM tblBOM INNER JOIN tblBOM_Details " & _
                    " ON tblBOM.TransID = tblBOM_Details.TransID " & _
                    " WHERE JO_Ref ='" & ID & "' "
            SQL.GetQuery(query)
            dgvItemMaster.Columns(dgcReqQTY.Index).Visible = True
            dgvItemMaster.Rows.Clear()
            Dim ctr As Integer = 0
            Dim ctrGroup As Integer = 0
            If SQL.SQLDS.Tables(0).Rows.Count > 0 Then
                txtJORef.Text = ID
                JO_ID = ID
                For Each row As DataRow In SQL.SQLDS.Tables(0).Rows
                    dgvItemMaster.Rows.Add(row(0).ToString, row(1).ToString, row(2).ToString, _
                              CDec(row(3)).ToString("N2"), 0, 0, "")
                    LoadWHSE(ctr)
                    LoadUOM(row(0).ToString, ctr)
                    ctr += 1
                Next
                LoadStock()
            End If
        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        End Try
    End Sub

    Private Sub cbTransType_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbTransType.SelectedIndexChanged
        RefreshTranType()
    End Sub

    Private Sub RefreshTranType()
        cbIssueTo.Enabled = False
        cbWHto.Enabled = False
        lblJORef.Visible = False
        txtJORef.Visible = False
        cbCostCenter.Visible = False
        lblCostCenter.Visible = False
        cbIssuerFrom.Enabled = False
        cbIssueTo.Enabled = False
        If cbTransType.SelectedItem = "Material Transfer" Then
            cbIssueTo.Enabled = True
            cbWHto.Enabled = True
            cbIssuerFrom.Enabled = True
            cbIssueTo.Enabled = True
            withConfirm = True
        ElseIf cbTransType.SelectedItem = "Issuance to Job" Then
            lblJORef.Visible = True
            txtJORef.Visible = True
            cbIssuerFrom.Enabled = True
            withConfirm = False
        ElseIf cbTransType.SelectedItem = "Disposal" Then
            LoadCostCenter()
            cbCostCenter.Visible = True
            lblCostCenter.Visible = True
            cbIssuerFrom.SelectedItem = "Warehouse"
            withConfirm = False
        ElseIf cbTransType.SelectedItem = "Promotional" Then
            LoadCostCenter()
            cbCostCenter.Visible = True
            lblCostCenter.Visible = True
            cbIssuerFrom.SelectedItem = "Warehouse"
            withConfirm = False
        ElseIf cbTransType.SelectedItem = "Material Return" Then
            cbWHto.Enabled = True
            cbIssuerFrom.SelectedItem = "Production"
            cbIssueTo.SelectedItem = "Warehouse"
            withConfirm = True
        ElseIf cbTransType.SelectedItem = "Issuance to Department" Then
            cbWHto.Enabled = True
            cbIssuerFrom.SelectedItem = "Warehouse"
            cbWHto.Enabled = False
            LoadCostCenter()
            cbCostCenter.Visible = True
            lblCostCenter.Visible = True
            withConfirm = False
        End If
    End Sub

    Private Sub FromJOToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FromJOToolStripMenuItem.Click
        Dim f As New frmLoadTransactions
        f.cbFilter.SelectedItem = "Status"
        f.txtFilter.Text = "Active"
        f.txtFilter.Enabled = False
        f.cbFilter.Enabled = False
        f.btnSearch.Enabled = False
        f.ShowDialog("JO_GI")
        LoadjO(f.transID)
        f.Dispose()
    End Sub

    Private Sub dgvItemMaster_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvItemMaster.CellContentClick

    End Sub

    Private Sub tsbPrint_Click(sender As System.Object, e As System.EventArgs) Handles tsbPrint.Click
        Dim f As New frmReport_Display
        f.ShowDialog("GI", TransID)
        f.Dispose()
    End Sub
End Class