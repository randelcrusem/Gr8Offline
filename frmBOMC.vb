Public Class frmBOMC
    Dim TransID, RefID As String
    Dim BOMCNo As String
    Dim disableEvent As Boolean = False
    Dim ModuleID As String = "BOMC"
    Dim ColumnPK As String = "BOMC_No"
    Dim ColumnID As String = "TransID"
    Dim DBTable As String = "tblBOMC"
    Dim TransAuto As Boolean
    Dim AccntCode As String

    Public Overloads Function ShowDialog(ByVal docnumber As String) As Boolean
        TransID = docnumber
        MyBase.ShowDialog()
        Return True
    End Function

    Private Sub frmBOMC_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try
            TransAuto = GetTransSetup(ModuleID)
            dtpDocDate.Value = Date.Today.Date
            loadWH()
            'LoadBOM()
            If TransID <> "" Then
                LoadBOMC(TransID)
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

    Private Sub LoadBOM()
        Dim query As String
        query = " SELECT BOM_Code FROM tblBOM_SFG WHERE Status ='Active' " & _
                " UNION ALL " & _
                " SELECT BOM_Code FROM tblBOM_FG WHERE Status ='Active' "
        SQL.ReadQuery(query)
        cbBOM.Items.Clear()
        While SQL.SQLDR.Read
            cbBOM.Items.Add(SQL.SQLDR("BOM_Code").ToString)
        End While
    End Sub

    Private Sub loadWHFrom()
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
    End Sub

    Private Sub loadWH()
        Dim query As String
        query = " SELECT tblProdWarehouse.Code + ' | ' + Description AS Description  FROM tblProdWarehouse " & _
                " INNER JOIN tblUser_Access " & _
                " ON tblProdWarehouse.Code = tblUser_Access.Code " & _
                " AND tblUser_Access.Status ='Active' AND tblProdWarehouse.Status ='Active' " & _
                " AND tblUser_Access.Type = 'Production' AND isAllowed = 1 " & _
                " WHERE UserID ='" & UserID & "' "
        SQL.ReadQuery(query)
        cbWHfrom.Items.Clear()
        While SQL.SQLDR.Read
            cbWHfrom.Items.Add(SQL.SQLDR("Description"))
        End While
    End Sub

    Private Sub EnableControl(ByVal Value As Boolean)
        cbWHfrom.Enabled = Value
        dgvItemMaster.AllowUserToAddRows = Value
        dgvItemMaster.AllowUserToDeleteRows = Value
        dgvItemMaster.ReadOnly = Not Value
        nupQty.Enabled = Value
        btnAddMats.Enabled = Value
        If Value = True Then
            dgvItemMaster.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2
        Else
            dgvItemMaster.EditMode = DataGridViewEditMode.EditProgrammatically
        End If
        txtItemName.Enabled = Value
        txtActualQTY.Enabled = Value
        txtRemarks.Enabled = Value
        dtpDocDate.Enabled = Value
        cbBOM.Enabled = Value
        If TransAuto Then
            txtTransNum.Enabled = False
        Else
            txtTransNum.Enabled = Value
        End If
        dgcBOMQTY.ReadOnly = True
        chStock.ReadOnly = True
    End Sub

    Private Sub tsbSearch_Click(sender As System.Object, e As System.EventArgs) Handles tsbSearch.Click
        If Not AllowAccess("BOMC_VIEW") Then
            msgRestricted()
        Else
            Dim f As New frmLoadTransactions
            f.ShowDialog("BOMC")
            TransID = f.transID
            LoadBOMC(TransID)
            f.Dispose()
        End If
    End Sub

    Private Sub LoadBOMC(ByVal ID As String)
        Dim query As String
        Dim WHSE, BOM_Code, ItemCode, UOM, ItemName As String
        Dim BatchQTY, StandardQTY, ActualQTY As Decimal
        query = " SELECT   TransID, BOMC_No, DateBOMC, WHSE, Remarks, Status, " & _
                " BOM_Code, ISNULL(BatchQTY,0) AS BatchQTY, BOM_ItemCode, ISNULL(StandardQTY,0) AS StandardQTY, UOM, ISNULL(ActualQTY,0) AS ActualQTY " & _
                " FROM     tblBOMC " & _
                " WHERE    TransId = '" & ID & "' " & _
                " ORDER BY TransId "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            TransID = SQL.SQLDR("TransID").ToString
            BOMCNo = SQL.SQLDR("BOMC_No").ToString
            txtTransNum.Text = BOMCNo
            dtpDocDate.Text = SQL.SQLDR("DateBOMC").ToString
            txtRemarks.Text = SQL.SQLDR("Remarks").ToString
            txtStatus.Text = SQL.SQLDR("Status").ToString
            ActualQTY = SQL.SQLDR("ActualQTY").ToString
            ItemCode = SQL.SQLDR("BOM_ItemCode").ToString
            UOM = SQL.SQLDR("UOM").ToString
            StandardQTY = SQL.SQLDR("StandardQTY").ToString
            BatchQTY = SQL.SQLDR("BatchQTY").ToString
            WHSE = SQL.SQLDR("WHSE").ToString
            BOM_Code = SQL.SQLDR("BOM_Code").ToString
            cbWHfrom.SelectedItem = GetWHSE(WHSE, "tblProdWarehouse")
            LoadBOMType(ItemCode)
            ItemName = GetItemName(ItemCode)
            cbBOM.SelectedItem = GetBOMCode(BOM_Code)
            txtUOM.Text = UOM
            txtItemCode.Text = ItemCode
            txtItemName.Text = ItemName
            nupQty.Value = BatchQTY
            txtActualQTY.Text = ActualQTY
            txtQTY.Text = StandardQTY
            LoadBOMCDetails(TransID)
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

    Protected Sub LoadBOMCDetails(ByVal TransID As String)
        Dim ctr As Integer = 0
        Dim query As String
        query = " SELECT	tblBOMC_Details.ItemCode, tblBOMC_Details.Description, tblBOMC_Details.UOM, tblBOMC_Details.BOM_QTY  AS BOM_QTY,  tblBOMC_Details.WHSE, ISNULL(tblBOMC_Details.AvailableStock,0) as AvailableStock, ISNULL(tblBOMC_Details.QTY,0) as QTY " & _
                " FROM	    tblBOMC INNER JOIN tblBOMC_Details " & _
                " ON		tblBOMC.TransID = tblBOMC_Details.TransID " & _
                " WHERE     tblBOMC_Details.TransId = " & TransID & " " & _
                " ORDER BY  tblBOMC_Details.LineNum "
        disableEvent = True
        dgvItemMaster.Rows.Clear()
        disableEvent = False
        SQL.ReadQuery(query)
        SQL.GetQuery(query)
        If SQL.SQLDS.Tables(0).Rows.Count > 0 Then
            For Each row As DataRow In SQL.SQLDS.Tables(0).Rows
                dgvItemMaster.Rows.Add(row(0).ToString, row(1).ToString, row(2).ToString, _
                                     CDec(row(3).ToString), CDec(row(5).ToString), GetWHSEDesc(row(4).ToString), CDec(row(6).ToString))
                LoadUOM(row(0).ToString, ctr)
                LoadWHSE(ctr)
                ctr += 1
            Next
        End If
    End Sub

    Private Function GetBOMCode(ByVal Code As String) As String
        Try
            Dim query As String
            query = " SELECT * FROM ( " & _
                    " SELECT BOM_Code  + ' | ' + ISNULL(Remarks,'') AS Description, ItemCode, BOM_Code  FROm tblBOM_FG " & _
                    " UNION ALL " & _
                    " SELECT BOM_Code  + ' | ' + ISNULL(Remarks,'') AS Description, ItemCode, BOM_Code  FROm tblBOM_SFG ) AS A " & _
                    "  WHERE BOM_Code ='" & Code & "' "
            SQL.FlushParams()
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                Return SQL.SQLDR("Description").ToString
            Else
                Return ""
            End If
        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, "modGen", "modGen")
            Return ""
        Finally
            SQL.FlushParams()
        End Try

    End Function


    Private Sub LoadWHSE(Optional ByVal SelectedIndex As Integer = -1)
        Try
            Dim dgvCB As New DataGridViewComboBoxCell
            dgvCB = dgvItemMaster.Item(chWHSE.Index, SelectedIndex)
            dgvCB.Items.Clear()
            ' ADD ALL WHSEc
            Dim query As String
            query = " SELECT Description FROM tblProdWarehouse WHERE Status ='Active' "
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
        txtTransNum.Clear()
        txtItemCode.Clear()
        txtItemName.Clear()
        txtActualQTY.Clear()
        txtQTY.Clear()
        txtUOM.Clear()
        nupQty.Value = 1
        cbBOM.Items.Clear()
        dgvItemMaster.Rows.Clear()
        txtRemarks.Clear()
        txtStatus.Text = "Open"
        dtpDocDate.Value = Date.Today.Date
    End Sub

    Private Sub tsbNew_Click(sender As System.Object, e As System.EventArgs) Handles tsbNew.Click
        If Not AllowAccess("BOMC_ADD") Then
            msgRestricted()
        Else
            Cleartext()
            TransID = ""
            BOMCNo = ""

            dgvItemMaster.Columns(dgcBOMQTY.Index).Visible = True
            dgvItemMaster.Columns(chStock.Index).Visible = True

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
        If Not AllowAccess("BOMC_EDIT") Then
            msgRestricted()
        Else
            EnableControl(True)

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
        End If
    End Sub

    Private Sub tsbSave_Click(sender As System.Object, e As System.EventArgs) Handles tsbSave.Click
        If cbWHfrom.SelectedIndex = -1 Then
            Msg("Please select warehouse!", MsgBoxStyle.Exclamation)
        ElseIf txtActualQTY.Text = "" Then
            Msg("Please input actual usage!", MsgBoxStyle.Exclamation)
        ElseIf Not LoadStock() Then
            Msg("Invalid request quantity!" & vbNewLine & "Cannot request more than the available stock!", MsgBoxStyle.Exclamation)
        ElseIf TransID = "" Then
            If MsgBox("Saving New Record, Click Yes to confirm", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "JADE Message Alert") = MsgBoxResult.Yes Then
                TransID = GenerateTransID(ColumnID, DBTable)
                BOMCNo = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
                txtTransNum.Text = BOMCNo
                SaveBOMC()
                Msg("Record Saved Succesfully!", MsgBoxStyle.Information)
                LoadBOMC(TransID)
            End If
        Else
            If MsgBox("Updating Record, Click Yes to confirm", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "JADE Message Alert") = MsgBoxResult.Yes Then
                UpdateBOMC()
                Msg("Record Updated Succesfully!", MsgBoxStyle.Information)
                LoadBOMC(TransID)
            End If
        End If
    End Sub


    Private Sub SaveBOMC()
        Try
            Dim WHSE, BOM_Code As String
            WHSE = GetWHSE(cbWHfrom.SelectedItem)
            BOM_Code = GetBOM(cbBOM.SelectedItem)

            activityStatus = True
            Dim insertSQL As String
            insertSQL = " INSERT INTO " & _
                        " tblBOMC(TransID, BOMC_No, BranchCode, BusinessCode, DateBOMC, WHSE, BOM_Code, BatchQTY, BOM_ItemCode, StandardQTY, UOM, ActualQTY, Remarks, WhoCreated) " & _
                        " VALUES (@TransID, @BOMC_No, @BranchCode, @BusinessCode, @DateBOMC, @WHSE, @BOM_Code, @BatchQTY, @BOM_ItemCode, @StandardQTY, @UOM, @ActualQTY, @Remarks, @WhoCreated) "
            SQL.FlushParams()
            SQL.AddParam("@TransID", TransID)
            SQL.AddParam("@BOMC_No", BOMCNo)
            SQL.AddParam("@BranchCode", BranchCode)
            SQL.AddParam("@BusinessCode", BusinessType)
            SQL.AddParam("@DateBOMC", dtpDocDate.Value.Date)
            SQL.AddParam("@WHSE", WHSE)
            SQL.AddParam("@BOM_Code", BOM_Code)
            SQL.AddParam("@BatchQTY", nupQty.Value)
            SQL.AddParam("@BOM_ItemCode", txtItemCode.Text)
            SQL.AddParam("@StandardQTY", txtQTY.Text)
            SQL.AddParam("@UOM", txtUOM.Text)
            SQL.AddParam("@ActualQTY", CDec(txtActualQTY.Text))
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@WhoCreated", UserID)
            SQL.ExecNonQuery(insertSQL)

            Dim line As Integer = 1
            Dim ItemCode, Description, UOM As String
            Dim QTY, UnitCost, TotalCost, BOM_QTY, AvailableStock As Decimal
            For Each row As DataGridViewRow In dgvItemMaster.Rows
                If Not row.Cells(chItemCode.Index).Value Is Nothing Then
                    ItemCode = IIf(row.Cells(chItemCode.Index).Value = Nothing, "", row.Cells(chItemCode.Index).Value)
                    Description = IIf(row.Cells(chItemName.Index).Value = Nothing, "", row.Cells(chItemName.Index).Value)
                    UOM = IIf(row.Cells(chUOM.Index).Value = Nothing, "", row.Cells(chUOM.Index).Value)
                    AvailableStock = IIf(row.Cells(chStock.Index).Value = Nothing, 0, row.Cells(chStock.Index).Value)
                    QTY = IIf(row.Cells(chActualQTY.Index).Value = Nothing, 0, row.Cells(chActualQTY.Index).Value)
                    BOM_QTY = IIf(row.Cells(dgcBOMQTY.Index).Value = Nothing, 0, row.Cells(dgcBOMQTY.Index).Value)
                    UnitCost = GetAverageCost(ItemCode)
                    insertSQL = " INSERT INTO " & _
                         " tblBOMC_Details(TransId, ItemCode, Description, UOM, QTY, BOM_QTY, AvailableStock, WHSE,  LineNum, WhoCreated) " & _
                         " VALUES(@TransId, @ItemCode, @Description, @UOM, @QTY, @BOM_QTY, @AvailableStock, @WHSE, @LineNum, @WhoCreated) "
                    SQL.FlushParams()
                    SQL.AddParam("@TransID", TransID)
                    SQL.AddParam("@ItemCode", ItemCode)
                    SQL.AddParam("@Description", Description)
                    SQL.AddParam("@UOM", UOM)
                    SQL.AddParam("@QTY", QTY)
                    SQL.AddParam("@BOM_QTY", BOM_QTY)
                    SQL.AddParam("@AvailableStock", AvailableStock)
                    SQL.AddParam("@WHSE", WHSE)
                    SQL.AddParam("@LineNum", line)
                    SQL.AddParam("@WhoCreated", UserID)
                    SQL.ExecNonQuery(insertSQL)
                    line += 1


                    SaveINVTY("OUT", ModuleID, "BOMC", TransID, dtpDocDate.Value.Date, ItemCode, WHSE, QTY, UOM, UnitCost)
                    If txtItemCode.Text = "" Then
                        SaveINVTY("IN", ModuleID, "BOMC", TransID, dtpDocDate.Value.Date, ItemCode, WHSE, QTY, UOM, UnitCost)
                    End If
                    TotalCost += UnitCost
                End If
            Next
            If txtItemCode.Text <> "" Then
                SaveINVTY("IN", ModuleID, "BOMC", TransID, dtpDocDate.Value.Date, txtItemCode.Text, WHSE, txtActualQTY.Text, txtUOM.Text, UnitCost)
            End If
            ComputeWAUC("BOMC", TransID)
        Catch ex As Exception
            activityStatus = False
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        Finally
            RecordActivity(UserID, ModuleID, Me.Name.ToString, "INSERT", "BOMC_No", txtTransNum.Text, BusinessType, BranchCode, "", activityStatus)
            SQL.FlushParams()
        End Try
    End Sub

    Private Sub UpdateBOMC()
        Try
            Dim WHSE, BOM_Code As String

            WHSE = GetWHSE(cbWHfrom.SelectedItem)
            BOM_Code = GetBOM(cbBOM.SelectedItem)

            activityStatus = True
            Dim insertSQL, updateSQL, deleteSQL As String
            updateSQL = " UPDATE    tblBOMC " & _
                        " SET       BOMC_No = @BOMC_No, BranchCode = @BranchCode, BusinessCode = @BusinessCode, DateBOMC = @DateBOMC, " & _
                        "           WHSE = @WHSE,  Remarks = @Remarks, BOM_Code = @BOM_Code, BatchQTY = @BatchQTY, " & _
                        "           BOM_ItemCode = @BOM_ItemCode, StandardQTY = @StandardQTY, UOM = @UOM, ActualQTY = @ActualQTY, " & _
                        "           WhoModified = @WhoModified, DateModified = GETDATE() " & _
                        " WHERE     TransID = @TransID  "
            SQL.FlushParams()
            SQL.AddParam("@TransID", TransID)
            SQL.AddParam("@BOMC_No", BOMCNo)
            SQL.AddParam("@BranchCode", BranchCode)
            SQL.AddParam("@BusinessCode", BusinessType)
            SQL.AddParam("@DateBOMC", dtpDocDate.Value.Date)
            SQL.AddParam("@WHSE", WHSE)
            SQL.AddParam("@BOM_Code", BOM_Code)
            SQL.AddParam("@BatchQTY", nupQty.Value)
            SQL.AddParam("@BOM_ItemCode", txtItemCode.Text)
            SQL.AddParam("@StandardQTY", txtQTY.Text)
            SQL.AddParam("@UOM", txtUOM.Text)
            SQL.AddParam("@ActualQTY", CDec(txtActualQTY.Text))
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@WhoModified", UserID)
            SQL.ExecNonQuery(updateSQL)

            deleteSQL = " DELETE FROM tblBOMC_Details WHERE TransID =@TransID "
            SQL.FlushParams()
            SQL.AddParam("@TransID", TransID)
            SQL.ExecNonQuery(deleteSQL)


            DELINVTY(ModuleID, "BOMC", TransID)

            Dim line As Integer = 1
            Dim ItemCode, Description, UOM As String
            Dim QTY, UnitCost, BOM_QTY, AvailableStock, TotalCost As Decimal
            For Each row As DataGridViewRow In dgvItemMaster.Rows
                If Not row.Cells(chItemCode.Index).Value Is Nothing Then
                    ItemCode = IIf(row.Cells(chItemCode.Index).Value = Nothing, "", row.Cells(chItemCode.Index).Value)
                    Description = IIf(row.Cells(chItemName.Index).Value = Nothing, "", row.Cells(chItemName.Index).Value)
                    QTY = IIf(row.Cells(chActualQTY.Index).Value = Nothing, 0, row.Cells(chActualQTY.Index).Value)
                    BOM_QTY = IIf(row.Cells(dgcBOMQTY.Index).Value = Nothing, 0, row.Cells(dgcBOMQTY.Index).Value)
                    AvailableStock = IIf(row.Cells(chStock.Index).Value = Nothing, 0, row.Cells(chStock.Index).Value)
                    UOM = IIf(row.Cells(chUOM.Index).Value = Nothing, "", row.Cells(chUOM.Index).Value)
                    UnitCost = GetAverageCost(ItemCode)
                    insertSQL = " INSERT INTO " & _
                         " tblBOMC_Details(TransId, ItemCode, Description, UOM, QTY, BOM_QTY, AvailableStock, WHSE, LineNum, WhoCreated) " & _
                         " VALUES(@TransId, @ItemCode, @Description, @UOM, @QTY, @BOM_QTY, @AvailableStock, @WHSE, @LineNum, @WhoCreated) "
                    SQL.FlushParams()
                    SQL.AddParam("@TransID", TransID)
                    SQL.AddParam("@ItemCode", ItemCode)
                    SQL.AddParam("@Description", Description)
                    SQL.AddParam("@UOM", UOM)
                    SQL.AddParam("@QTY", QTY)
                    SQL.AddParam("@BOM_QTY", BOM_QTY)
                    SQL.AddParam("@AvailableStock", AvailableStock)
                    SQL.AddParam("@WHSE", WHSE)
                    SQL.AddParam("@LineNum", line)
                    SQL.AddParam("@WhoCreated", UserID)
                    SQL.ExecNonQuery(insertSQL)
                    line += 1
                    '' NEGATIVE QUANTITY FOR OUTGOING ITEMS
                    'QTY = QTY * -1
                    'Dim baseQTY As Decimal = QTY * ConvertUOM(UOM, ItemCode)
                    'Dim baseValue As Decimal = QTY * UnitCost

                    'UpdateINVTY("OUT", ModuleID, "BOMC", TransID, dtpDocDate.Value.Date, ItemCode, WHSE, QTY, UOM, UnitCost)

                    SaveINVTY("OUT", ModuleID, "BOMC", TransID, dtpDocDate.Value.Date, ItemCode, WHSE, QTY, UOM, UnitCost)
                    If txtItemCode.Text = "" Then
                        SaveINVTY("IN", ModuleID, "BOMC", TransID, dtpDocDate.Value.Date, ItemCode, WHSE, QTY, UOM, UnitCost)
                    End If
                    TotalCost += UnitCost
                End If
            Next
            If txtItemCode.Text <> "" Then
                SaveINVTY("IN", ModuleID, "BOMC", TransID, dtpDocDate.Value.Date, txtItemCode.Text, WHSE, txtActualQTY.Text, txtUOM.Text, UnitCost)
            End If
            ComputeWAUC("BOMC", TransID)

        Catch ex As Exception
            activityStatus = False
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        Finally
            RecordActivity(UserID, ModuleID, Me.Name.ToString, "UPDATE", "BOMC_No", txtTransNum.Text, BusinessType, BranchCode, "", activityStatus)
            SQL.FlushParams()
        End Try

    End Sub

    Private Sub tsbCancel_Click(sender As System.Object, e As System.EventArgs) Handles tsbCancel.Click
        If Not AllowAccess("BOMC_DEL") Then
            msgRestricted()
        Else
            If txtTransNum.Text <> "" Then
                If MsgBox("Are you sure you want to cancel this record?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "JADE Message Alert") = MsgBoxResult.Yes Then
                    Try
                        activityStatus = True
                        Dim deleteSQL As String
                        deleteSQL = " UPDATE  tblBOMC SET Status ='Cancelled' WHERE BOMC_No = @BOMC_No "
                        SQL.FlushParams()
                        BOMCNo = txtTransNum.Text
                        SQL.AddParam("@BOMC_No", BOMCNo)
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


                        Dim insertSQL As String
                        Dim line As Integer = 1
                        Dim ItemCode, Description, UOM, WHSE As String
                        Dim QTY, UnitCost, TotalCost, BOM_QTY, AvailableStock As Decimal
                        WHSE = GetWHSE(cbWHfrom.SelectedItem)
                        For Each row As DataGridViewRow In dgvItemMaster.Rows
                            If Not row.Cells(chItemCode.Index).Value Is Nothing Then
                                ItemCode = IIf(row.Cells(chItemCode.Index).Value = Nothing, "", row.Cells(chItemCode.Index).Value)
                                Description = IIf(row.Cells(chItemName.Index).Value = Nothing, "", row.Cells(chItemName.Index).Value)
                                UOM = IIf(row.Cells(chUOM.Index).Value = Nothing, "", row.Cells(chUOM.Index).Value)
                                AvailableStock = IIf(row.Cells(chStock.Index).Value = Nothing, 0, row.Cells(chStock.Index).Value)
                                QTY = IIf(row.Cells(chActualQTY.Index).Value = Nothing, 0, row.Cells(chActualQTY.Index).Value)
                                BOM_QTY = IIf(row.Cells(dgcBOMQTY.Index).Value = Nothing, 0, row.Cells(dgcBOMQTY.Index).Value)
                                UnitCost = GetAverageCost(ItemCode)
                                insertSQL = " INSERT INTO " & _
                                     " tblBOMC_Details(TransId, ItemCode, Description, UOM, QTY, BOM_QTY, AvailableStock, WHSE,  LineNum, WhoCreated) " & _
                                     " VALUES(@TransId, @ItemCode, @Description, @UOM, @QTY, @BOM_QTY, @AvailableStock, @WHSE, @LineNum, @WhoCreated) "
                                SQL.FlushParams()
                                SQL.AddParam("@TransID", TransID)
                                SQL.AddParam("@ItemCode", ItemCode)
                                SQL.AddParam("@Description", Description)
                                SQL.AddParam("@UOM", UOM)
                                SQL.AddParam("@QTY", QTY)
                                SQL.AddParam("@BOM_QTY", BOM_QTY)
                                SQL.AddParam("@AvailableStock", AvailableStock)
                                SQL.AddParam("@WHSE", WHSE)
                                SQL.AddParam("@LineNum", line)
                                SQL.AddParam("@WhoCreated", UserID)
                                SQL.ExecNonQuery(insertSQL)
                                line += 1


                                SaveINVTY("IN", ModuleID, "BOMC", TransID, dtpDocDate.Value.Date, ItemCode, WHSE, QTY, UOM, UnitCost)
                                If txtItemCode.Text = "" Then
                                    SaveINVTY("OUT", ModuleID, "BOMC", TransID, dtpDocDate.Value.Date, ItemCode, WHSE, QTY, UOM, UnitCost)
                                End If
                                TotalCost += UnitCost
                            End If
                        Next
                        If txtItemCode.Text <> "" Then
                            SaveINVTY("OUT", ModuleID, "BOMC", TransID, dtpDocDate.Value.Date, txtItemCode.Text, WHSE, txtActualQTY.Text, txtUOM.Text, UnitCost)
                        End If
                        ComputeWAUC("BOMC", TransID)


                        LoadBOMC(TransID)
                    Catch ex As Exception
                        activityStatus = True
                        SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
                    Finally
                        RecordActivity(UserID, ModuleID, Me.Name.ToString, "CANCEL", "BOMC_No", BOMCNo, BusinessType, BranchCode, "", activityStatus)
                        SQL.FlushParams()
                    End Try
                End If
            End If
        End If
    End Sub

    Private Sub tsbPrevious_Click(sender As System.Object, e As System.EventArgs) Handles tsbPrevious.Click
        If BOMCNo <> "" Then
            Dim query As String
            query = " SELECT Top 1 TransID FROM tblBOMC  " & _
                    "  LEFT JOIN  tblProdWarehouse  ON	      " & _
                    "  tblBOMC.WHSE = tblProdWarehouse.Code   " & _
                    "  WHERE       " & _
                    "  tblBOMC.WHSE IN  " & _
                    "  (SELECT    DISTINCT tblProdWarehouse.Code                                " & _
                    "  FROM      tblProdWarehouse  " & _
                    "  INNER JOIN tblUser_Access  ON         " & _
                    "  tblProdWarehouse.Code = tblUser_Access.Code   AND        " & _
                    "  tblUser_Access.Status ='Active' AND tblProdWarehouse.Status ='Active'   " & _
                    "   AND       tblUser_Access.Type = 'Production' AND isAllowed = 1    WHERE UserID ='" & UserID & "') AND BOMC_No < '" & BOMCNo & "' ORDER BY BOMC_No DESC "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                TransID = SQL.SQLDR("TransID").ToString
                LoadBOMC(TransID)
            Else
                Msg("Reached the beginning of record!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub

    Private Sub tsbNext_Click(sender As System.Object, e As System.EventArgs) Handles tsbNext.Click
        If BOMCNo <> "" Then
            Dim query As String
            query = " SELECT Top 1 TransID FROM tblBOMC  " & _
                    "  LEFT JOIN  tblProdWarehouse  ON	      " & _
                    "  tblBOMC.WHSE = tblProdWarehouse.Code   " & _
                    "  WHERE       " & _
                    "  tblBOMC.WHSE IN  " & _
                    "  (SELECT    DISTINCT tblProdWarehouse.Code                                " & _
                    "  FROM      tblProdWarehouse  " & _
                    "  INNER JOIN tblUser_Access  ON         " & _
                    "  tblProdWarehouse.Code = tblUser_Access.Code   AND        " & _
                    "  tblUser_Access.Status ='Active' AND tblProdWarehouse.Status ='Active'   " & _
                    "   AND       tblUser_Access.Type = 'Production' AND isAllowed = 1    WHERE UserID ='" & UserID & "') AND BOMC_No > '" & BOMCNo & "' ORDER BY BOMC_No ASC "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                TransID = SQL.SQLDR("TransID").ToString
                LoadBOMC(TransID)
            Else
                Msg("Reached the end of record!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub

    Private Sub tsbClose_Click(sender As System.Object, e As System.EventArgs) Handles tsbClose.Click


        ' Toolstrip Buttons
        If BOMCNo = "" Then
            Cleartext()
            EnableControl(False)
            tsbEdit.Enabled = False
            tsbCancel.Enabled = False
            tsbPrevious.Enabled = False
            tsbNext.Enabled = False
            tsbPrint.Enabled = False
        Else
            LoadBOMC(TransID)
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

    Private Sub cbBOM_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbBOM.SelectedIndexChanged
        If disableEvent = False Then
            LoadBOMData(GetBOM(cbBOM.SelectedItem))
        End If

    End Sub

    Private Function LoadStock() As Boolean
        Dim available As Boolean = True
        If cbWHfrom.SelectedIndex <> -1 Then
            Dim WHSE As String
            WHSE = GetWHSE(cbWHfrom.SelectedItem)
            Dim query As String
            Dim itemCode As String
            Dim StockQTY As Decimal = 0
            Dim ReqQTY As Decimal = 0
            For Each row As DataGridViewRow In dgvItemMaster.Rows
                If Not IsNothing(row.Cells(chItemCode.Index).Value) Then
                    itemCode = row.Cells(chItemCode.Index).Value.ToString
                    If Not IsNumeric(row.Cells(chActualQTY.Index).Value) Then ReqQTY = 0 Else ReqQTY = CDec(row.Cells(chActualQTY.Index).Value)

                    query = "   SELECT	    ISNULL(SUM(QTY),0) AS QTY " & _
                            "   FROM		viewItem_Stock " & _
                            "   WHERE       ItemCode ='" & itemCode & "' " & _
                            "   AND         WHSE = '" & WHSE & "' "
                    SQL.ReadQuery(query)
                    If SQL.SQLDR.Read Then
                        StockQTY = SQL.SQLDR("QTY")
                        If StockQTY >= ReqQTY Then ' IF AVAILABLE STOCK IS GREATER THAN THE BOM REQUIREMENT THEN ISSUE QTY SHOULD BE THE SAME AS BOM QTY
                            row.DefaultCellStyle.BackColor = Color.White
                        Else ' IF AVAILABLE STOCK IS LESS THAN THE BOM REQUIREMENT THEN ISSUE QTY SHOULD BE ONLY THE STOCK QTY
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128)
                            available = False
                        End If
                    End If
                    row.Cells(chStock.Index).Value = CDec(StockQTY).ToString("N2")

                End If

            Next
        End If
        Return available
    End Function

    Private Sub LoadBOMData(ByVal BOM_Code As String)
        If cbBOM.SelectedIndex <> -1 Then
            Dim query As String
            query = " SELECT    BOM_Code, tblBOM_SFG.ItemCode, ItemName, UOM, QTY  " & _
                    " FROM      tblBOM_SFG INNER JOIn tblItem_Master " & _
                    " ON        tblBOM_SFG.ItemCode = tblItem_Master.ItemCode " & _
                    " WHERE     tblBOM_SFG.Status ='Active' " & _
                    " AND       tblItem_Master.Status ='Active' " & _
                    " AND       BOM_Code ='" & BOM_Code & "' "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                txtItemCode.Text = SQL.SQLDR("ItemCode").ToString
                txtItemName.Text = SQL.SQLDR("ItemName").ToString
                txtUOM.Text = SQL.SQLDR("UOM").ToString
                txtQTY.Text = SQL.SQLDR("QTY").ToString * nupQty.Value
            Else
                query = " SELECT    BOM_Code, tblBOM_FG.ItemCode, ItemName, UOM, QTY  " & _
                 " FROM      tblBOM_FG INNER JOIn tblItem_Master " & _
                 " ON        tblBOM_FG.ItemCode = tblItem_Master.ItemCode " & _
                 " WHERE     tblBOM_FG.Status ='Active' " & _
                 " AND       tblItem_Master.Status ='Active' " & _
                 " AND       BOM_Code ='" & BOM_Code & "' "
                SQL.ReadQuery(query)
                If SQL.SQLDR.Read Then
                    txtItemCode.Text = SQL.SQLDR("ItemCode").ToString
                    txtItemName.Text = SQL.SQLDR("ItemName").ToString
                    txtUOM.Text = SQL.SQLDR("UOM").ToString
                    txtQTY.Text = SQL.SQLDR("QTY").ToString * nupQty.Value
                Else
                    txtItemCode.Text = ""
                    txtItemName.Text = ""
                    txtUOM.Text = ""
                    txtQTY.Text = 1
                End If
            End If
        End If
    End Sub

    Private Sub nupQty_ValueChanged(sender As System.Object, e As System.EventArgs) Handles nupQty.ValueChanged
        If disableEvent = False Then
            If cbBOM.SelectedIndex <> -1 Then
                LoadBOMData(GetBOM(cbBOM.SelectedItem))
            End If
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
                Case chActualQTY.Index
                    If IsNumeric(dgvItemMaster.Item(chActualQTY.Index, e.RowIndex).Value) AndAlso IsNumeric(dgvItemMaster.Item(chStock.Index, e.RowIndex).Value) Then
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
                dgvItemMaster.Item(4, dgvItemMaster.SelectedCells(0).RowIndex).Value = IIf(cbWHfrom.SelectedIndex = -1, "", cbWHfrom.SelectedItem)
                LoadWHSE(dgvItemMaster.SelectedCells(0).RowIndex)
                LoadUOM(ItemCode, dgvItemMaster.SelectedCells(0).RowIndex)
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
            End If
        End If

        'Remove the handle to this event. It will be readded each time a new combobox selection causes the EditingControlShowing Event to fire
        RemoveHandler editingComboBox.SelectionChangeCommitted, AddressOf editingComboBox_SelectionChangeCommitted
        'Re-enable the EditingControlShowing event so the above can take place.
        AddHandler dgvItemMaster.EditingControlShowing, AddressOf dgvItemMaster_EditingControlShowing
    End Sub

    Private Sub btnAddMats_Click(sender As System.Object, e As System.EventArgs) Handles btnAddMats.Click
        If cbWHfrom.SelectedIndex = -1 Then
            Msg("Please select warehouse first!", MsgBoxStyle.Exclamation)
        ElseIf cbBOM.SelectedIndex = -1 Then
            Msg("Please select BOM first!", MsgBoxStyle.Exclamation)
        Else
            dgvItemMaster.Rows.Clear()
            Dim ctr As Integer = dgvItemMaster.Rows.Count - 1
            Dim query As String
            Dim BOM_Code As String
            BOM_Code = GetBOM(cbBOM.SelectedItem)
            query = " SELECT    tblBOM_SFG_Details.MaterialCode, ItemName, tblBOM_SFG_Details.UOM, tblBOM_SFG_Details.QTY  " & _
                    " FROM      tblBOM_SFG INNER JOIN tblBOM_SFG_Details " & _
                    " ON        tblBOM_SFG.BOM_Code = tblBOM_SFG_Details.BOM_Code " & _
                    " INNER JOIn tblItem_Master " & _
                    " ON        tblBOM_SFG_Details.MaterialCode = tblItem_Master.ItemCode " & _
                    " WHERE     tblBOM_SFG.Status ='Active' " & _
                    " AND       tblItem_Master.Status ='Active' " & _
                    " AND       tblBOM_SFG.BOM_Code = '" & BOM_Code & "' " & _
                    " ORDER BY  LineNumber "
            SQL.GetQuery(query)
            If SQL.SQLDS.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In SQL.SQLDS.Tables(0).Rows
                    dgvItemMaster.Rows.Add({row(0).ToString, row(1).ToString, row(2).ToString, row(3) * nupQty.Value, 0, 0, row(3) * nupQty.Value, cbWHfrom.SelectedItem})
                    LoadUOM(row(0).ToString, ctr)
                    LoadWHSE(ctr)
                    ctr += 1
                Next
            Else
                query = " SELECT    tblBOM_FG_Details.MaterialCode, ItemName, tblBOM_FG_Details.UOM, tblBOM_FG_Details.QTY  " & _
                        " FROM      tblBOM_FG INNER JOIN tblBOM_FG_Details " & _
                        " ON        tblBOM_FG.BOM_Code = tblBOM_FG_Details.BOM_Code " & _
                        " INNER JOIn tblItem_Master " & _
                        " ON        tblBOM_FG_Details.MaterialCode = tblItem_Master.ItemCode " & _
                        " WHERE     tblBOM_FG.Status ='Active' " & _
                        " AND       tblItem_Master.Status ='Active' " & _
                        " AND       tblBOM_FG.BOM_Code = '" & BOM_Code & "' " & _
                        " ORDER BY  LineNumber "
                SQL.GetQuery(query)
                If SQL.SQLDS.Tables(0).Rows.Count > 0 Then
                    For Each row As DataRow In SQL.SQLDS.Tables(0).Rows
                        dgvItemMaster.Rows.Add({row(0).ToString, row(1).ToString, row(2).ToString, row(3) * nupQty.Value, 0, 0, row(3) * nupQty.Value, cbWHfrom.SelectedItem})
                        LoadUOM(row(0).ToString, ctr)
                        LoadWHSE(ctr)
                        ctr += 1
                    Next
                End If
            End If
            LoadStock()
        End If
    End Sub

    Private Sub dgvItemMaster_DataError(sender As Object, e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles dgvItemMaster.DataError
        Try

        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        End Try
    End Sub

    'Private Sub LoadStock()
    '    Dim WHSE As String
    '    WHSE = GetWHSE(cbWHfrom.SelectedItem)
    '    Dim query As String
    '    Dim itemCode As String
    '    Dim StockQTY As Decimal = 0
    '    Dim IssueQTY As Decimal = 0
    '    Dim BOMQTY As Decimal = 0
    '    For Each row As DataGridViewRow In dgvItemMaster.Rows
    '        If Not IsNothing(row.Cells(chItemCode.Index).Value) Then
    '            itemCode = row.Cells(chItemCode.Index).Value.ToString
    '            If Not IsNumeric(row.Cells(dgcBOMQTY.Index).Value) Then BOMQTY = 0 Else BOMQTY = CDec(row.Cells(dgcBOMQTY.Index).Value)

    '            query = "   SELECT	    ISNULL(SUM(QTY),0) AS QTY " & _
    '                    "   FROM		viewItem_Stock " & _
    '                    "   WHERE       ItemCode ='" & itemCode & "' " & _
    '                    "   AND         WHSE = '" & WHSE & "' "
    '            SQL.ReadQuery(query)
    '            If SQL.SQLDR.Read Then
    '                StockQTY = SQL.SQLDR("QTY")
    '                If StockQTY >= BOMQTY Then ' IF AVAILABLE STOCK IS GREATER THAN THE BOM REQUIREMENT THEN ISSUE QTY SHOULD BE THE SAME AS BOM QTY
    '                    IssueQTY = BOMQTY
    '                Else ' IF AVAILABLE STOCK IS LESS THAN THE BOM REQUIREMENT THEN ISSUE QTY SHOULD BE ONLY THE STOCK QTY
    '                    IssueQTY = StockQTY
    '                End If
    '            End If
    '            row.Cells(chStock.Index).Value = CDec(StockQTY).ToString("N2")

    '        End If

    '    Next
    '    dgvItemMaster.Columns(dgcBOMQTY.Index).Visible = True
    '    dgvItemMaster.Columns(chStock.Index).Visible = True
    'End Sub

    Private Sub cbWHfrom_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbWHfrom.SelectedIndexChanged
        LoadStock()
    End Sub

    Private Sub tsbCopyPR_Click(sender As System.Object, e As System.EventArgs) Handles tsbCopyPR.Click
        Dim f As New frmLoadTransactions
        f.cbFilter.SelectedItem = "Status"
        f.txtFilter.Text = "Active"
        f.txtFilter.Enabled = False
        f.cbFilter.Enabled = False
        f.btnSearch.Enabled = False
        f.ShowDialog("JO-BOM")
        LoadJO(f.transID)
        f.Dispose()
    End Sub


    Private Sub LoadJO(ByVal ID As String)
        Dim query As String
        query = " SELECT    TransID, JO_No, DateJO, VCECode, ItemCode, Description, UOM, QTY, Remarks, DateNeeded, ProdLine, SO_Ref, SO_Ref_LineNum, Status " & _
                 " FROM       tblJO " & _
                 " WHERE      TransId = '" & ID & "' "
        SQL.FlushParams()
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtJORef.Text = SQL.SQLDR("JO_No").ToString
            'Else
            '    ClearText()
        End If
    End Sub


    Private Sub txtItemName_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtItemName.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtUOM.Clear()
            txtQTY.Clear()
            cbBOM.Items.Clear()
            nupQty.Value = 1
            dgvItemMaster.Rows.Clear()
            Dim itemCode As String
            itemCode = txtItemName.Text
            Dim f As New frmCopyFrom
            f.ShowDialog("All Item", itemCode)
            If f.TransID <> "" Then
                itemCode = f.TransID
                LoadItem(itemCode)
                LoadBOMType(txtItemCode.Text)
            Else
                txtItemName.Clear()
            End If
            f.Dispose()
        End If
    End Sub

    Private Sub LoadItem(ByVal itemCode As String)
        Dim query As String
        query = "  SELECT ItemCode, ItemName, PD_UOM, PD_UnitCost " & _
                "  FROM   tblItem_Master     " & _
                "  WHERE  ItemCode = '" & itemCode & "'  AND Status ='Active' "
        SQL.FlushParams()
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtItemCode.Text = itemCode
            txtItemName.Text = SQL.SQLDR("ItemName").ToString
        Else
            MsgBox("No Record found!", MsgBoxStyle.Information)
            Me.Close()
        End If
    End Sub

    Private Sub LoadBOMType(ByVal ItemCode As String)
        Dim query As String
        query = " SELECT * FROM ( " & _
                " SELECT BOM_Code  + ' | ' + ISNULL(Remarks,'') AS Description, ItemCode  FROm tblBOM_FG " & _
                " UNION ALL " & _
                " SELECT BOM_Code  + ' | ' + ISNULL(Remarks,'') AS Description, ItemCode  FROm tblBOM_SFG ) AS A " & _
                "  WHERE ItemCode ='" & ItemCode & "' "
        SQL.ReadQuery(query)
        cbBOM.Items.Clear()
        If SQL.SQLDR.HasRows Then
            While SQL.SQLDR.Read
                cbBOM.Items.Add(SQL.SQLDR("Description").ToString)
            End While
        Else
            MsgBox("No BOM for this Item!", MsgBoxStyle.Information)
        End If
    End Sub


    Private Sub txtItemName_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtItemName.TextChanged

    End Sub

    Private Sub Label3_Click(sender As System.Object, e As System.EventArgs) Handles Label3.Click

    End Sub

    Private Sub tsbPrint_Click(sender As System.Object, e As System.EventArgs) Handles tsbPrint.Click

    End Sub

    Private Sub dgvItemMaster_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvItemMaster.CellContentClick

    End Sub
End Class