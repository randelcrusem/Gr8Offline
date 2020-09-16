Public Class frmDR
    Dim TransID, RefID, JETransID As String
    Dim DRNo As String
    Dim disableEvent As Boolean = False
    Dim ModuleID As String = "DR"
    Dim ColumnPK As String = "DR_No"
    Dim DBTable As String = "tblDR"
    Dim ColumnID As String = "TransID"
    Dim TransAuto As Boolean
    Dim SO_ID, PL_ID, PO_ID As Integer


    Public Overloads Function ShowDialog(ByVal docnumber As String) As Boolean
        TransID = docnumber
        MyBase.ShowDialog()
        Return True
    End Function

    Private Sub frmDR_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try
            TransAuto = GetTransSetup(ModuleID)
            dtpDocDate.Value = Date.Today.Date
            dtpActual.Value = Date.Today.Date
            If TransID <> "" Then
                LoadDR(TransID)
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

    Private Sub EnableControl(ByVal Value As Boolean)
        txtVCEName.Enabled = Value
        btnSearchVCE.Enabled = Value
        dgvItemList.AllowUserToAddRows = Value
        dgvItemList.AllowUserToDeleteRows = Value
        dgvItemList.ReadOnly = Not Value
        If Value = True Then
            dgvItemList.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2
        Else
            dgvItemList.EditMode = DataGridViewEditMode.EditProgrammatically
        End If
        txtRemarks.Enabled = Value
        chkForECS.Enabled = Value
        dtpDocDate.Enabled = Value
        dtpActual.Enabled = Value
        If TransAuto Then
            txtTransNum.Enabled = False
        Else
            txtTransNum.Enabled = Value
        End If
    End Sub


    Private Sub LoadDR(ByVal ID As String)
        Dim query As String
        query = " SELECT   TransID, DR_No, VCECode, DateDR, DateDeliver, Remarks, Status, SO_Ref, PL_Ref, PO_Ref, ForECS " & _
                " FROM     tblDR " & _
                " WHERE    TransId = '" & ID & "' " & _
                " ORDER BY TransId "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            TransID = SQL.SQLDR("TransID").ToString
            DRNo = SQL.SQLDR("DR_No").ToString
            txtTransNum.Text = DRNo
            txtVCECode.Text = SQL.SQLDR("VCECode").ToString
            dtpDocDate.Text = SQL.SQLDR("DateDR").ToString
            dtpActual.Value = IIf(IsDate(SQL.SQLDR("DateDeliver")), SQL.SQLDR("DateDeliver"), Date.Today)
            txtRemarks.Text = SQL.SQLDR("Remarks").ToString
            txtStatus.Text = SQL.SQLDR("Status").ToString
            chkForECS.Checked = SQL.SQLDR("ForECS").ToString
            SO_ID = SQL.SQLDR("SO_Ref").ToString
            PL_ID = SQL.SQLDR("PL_Ref").ToString
            PO_ID = SQL.SQLDR("PO_Ref").ToString
            txtVCEName.Text = GetVCEName(txtVCECode.Text)
            txtSORef.Text = LoadSONo(SO_ID)
            txtPLRef.Text = LoadPLNo(PL_ID)
            txtPORef.Text = LoadPONo(PO_ID)
            LoadDRDetails(TransID)
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

    Private Sub LoadAccountingEntry(ByVal TransID As Integer)
        Try
            Dim query As String
            query = " SELECT JE_No, VCECode, ISNULL(VCEName,'') AS VCEName, AccntCode, AccntTitle, Particulars, " & _
                    "        ISNULL(Debit,0) AS Debit, ISNULL(Credit,0) AS Credit " & _
                    " FROM   View_GL  " & _
                    " WHERE  RefType ='DR' AND RefTransID ='" & TransID & "' "
            SQL.ReadQuery(query)
            dgvEntry.Rows.Clear()
            If SQL.SQLDR.HasRows Then
                While SQL.SQLDR.Read
                    JETransiD = SQL.SQLDR("JE_No").ToString
                    dgvEntry.Rows.Add(SQL.SQLDR("AccntCode").ToString, SQL.SQLDR("AccntTitle").ToString, _
                                      IIf(CDec(SQL.SQLDR("Debit")) = 0, "", CDec(SQL.SQLDR("Debit")).ToString("N2")), If(CDec(SQL.SQLDR("Credit")) = 0, "", CDec(SQL.SQLDR("Credit")).ToString("N2")), _
                                      SQL.SQLDR("Particulars").ToString, SQL.SQLDR("VCECode").ToString, SQL.SQLDR("VCEName").ToString)
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

    Private Function LoadSONo(SO_ID As Integer) As String
        Dim query As String
        query = " SELECT SO_No FROM tblSO WHERE TransID = '" & SO_ID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return SQL.SQLDR("SO_No")
        Else
            Return ""
        End If
    End Function

    Private Function LoadPLNo(PL_ID As Integer) As String
        Dim query As String
        query = " SELECT PL_No FROM tblPL WHERE TransID = '" & PL_ID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return SQL.SQLDR("PL_No")
        Else
            Return ""
        End If
    End Function

    Private Function LoadPONo(PO_ID As Integer) As String
        Dim query As String
        query = " SELECT PO_No FROM tblPO WHERE TransID = '" & PO_ID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return SQL.SQLDR("PO_No")
        Else
            Return ""
        End If
    End Function

    Protected Sub LoadDRDetails(ByVal TransID As String)
        dgvItemList.Rows.Clear()
        Dim ctr As Integer = 0
        Dim query As String
        'query = " SELECT	tblDR_Details.ItemCode, tblDR_Details.Description, tblDR_Details.UOM,  tblDR_Details.QTY AS QTY, " & _
        '        "           tblDR_Details.WHSE, ISNULL(tblSO_Details.UnitPrice,0) AS UnitPrice, ISNULL(Ref_SO_RecID,0) AS Ref_SO_RecID " & _
        '        " FROM	    tblDR INNER JOIN tblDR_Details " & _
        '        " ON		tblDR.TransID = tblDR_Details.TransID " & _
        '        " LEFT JOIN tblSO " & _
        '        " ON		tblSO.TransID = tblDR.SO_Ref  " & _
        '        " LEFT JOIN tblSO_Details " & _
        '        " ON		tblSO.TransID = tblSO_Details.TransID " & _
        '        " AND		tblSO_Details.ItemCode = tblDR_Details.ItemCode " & _
        '        " AND		tblSO_Details.RecordID = tblDR_Details.Ref_SO_RecID " & _
        '        " WHERE     tblDR_Details.TransId = " & TransID & " " & _
        '        " ORDER BY  tblDR_Details.LineNum "
        query = " SELECT	tblDR_Details.ItemCode, tblDR_Details.Description, tblDR_Details.UOM,  tblDR_Details.QTY AS QTY, " & _
               "           tblDR_Details.WHSE, ISNULL(tblDR_Details.UnitPrice,0) AS UnitPrice, ISNULL(tblDR_Details.GrossAmount,0) AS GrossAmount,ISNULL(tblDR_Details.NetAmount,0) AS NetAmount, ISNULL(Ref_SO_RecID,0) AS Ref_SO_RecID, tblDR_Details.AccntCode, " & _
               "           ISNULL(tblDR_Details.DRRate,0) AS DRRate, ISNULL(tblDR_Details.DRPrice,0) AS DRPrice" & _
               " FROM	    tblDR INNER JOIN tblDR_Details " & _
               " ON		tblDR.TransID = tblDR_Details.TransID " & _
               " LEFT JOIN tblSO " & _
               " ON		tblSO.TransID = tblDR.SO_Ref  " & _
               " LEFT JOIN tblSO_Details " & _
               " ON		tblSO.TransID = tblSO_Details.TransID " & _
               " AND		tblSO_Details.ItemCode = tblDR_Details.ItemCode " & _
               " AND		tblSO_Details.RecordID = tblDR_Details.Ref_SO_RecID " & _
               " WHERE     tblDR_Details.TransId = " & TransID & " " & _
               " ORDER BY  tblDR_Details.LineNum "
        dgvItemList.Rows.Clear()
        SQL.GetQuery(query)
        If SQL.SQLDS.Tables(0).Rows.Count Then
            For Each row As DataRow In SQL.SQLDS.Tables(0).Rows
                dgvItemList.Rows.Add(row(0).ToString, row(1).ToString, row(2).ToString, _
                                                row(3).ToString, GetWHSEDesc(row(4).ToString), row(5).ToString, row(6).ToString, row(7).ToString, row(8).ToString, row(9).ToString, row(10).ToString, row(11).ToString)
                LoadWHSE(ctr)
                ctr += 1
            Next
        End If
    End Sub

    Private Sub txtVCEName_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtVCEName.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim f As New frmVCE_Search
            f.cbFilter.SelectedItem = "VCEName"
            f.txtFilter.Text = txtVCEName.Text
            f.Type = "Member"
            f.ShowDialog()
            txtVCECode.Text = f.VCECode
            txtVCEName.Text = f.VCEName
        End If
    End Sub

    Private Sub ClearText()
        PL_ID = 0
        SO_ID = 0
        PO_ID = 0
        txtTransNum.Clear()
        txtVCECode.Clear()
        txtVCEName.Clear()
        chkForECS.Checked = False
        dgvItemList.Rows.Clear()
        txtRemarks.Clear()
        txtSORef.Clear()
        txtPLRef.Clear()
        txtPORef.Clear()
        txtStatus.Text = "Open"
        dtpActual.Value = Date.Today.Date
        dtpDocDate.Value = Date.Today.Date
    End Sub


    Private Sub dgvItemList_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvItemList.CellEndEdit
        Try
            Dim itemCode As String
            Dim rowIndex As Integer = dgvItemList.CurrentCell.RowIndex
            Dim colindex As Integer = dgvItemList.CurrentCell.ColumnIndex
            Select Case colindex
                Case chItemCode.Index
                    If dgvItemList.Item(chItemCode.Index, e.RowIndex).Value <> "" Then
                        itemCode = dgvItemList.Item(chItemCode.Index, e.RowIndex).Value
                        Dim f As New frmCopyFrom
                        f.ShowDialog("All Item", itemCode)
                        If f.TransID <> "" Then
                            itemCode = f.TransID
                            LoadItem(itemCode)
                        End If
                        f.Dispose()
                    End If
                Case chItemDesc.Index
                    If dgvItemList.Item(chItemDesc.Index, e.RowIndex).Value <> "" Then
                        itemCode = dgvItemList.Item(chItemDesc.Index, e.RowIndex).Value
                        Dim f As New frmCopyFrom
                        f.ShowDialog("All Item", itemCode)
                        If f.TransID <> "" Then
                            itemCode = f.TransID
                            LoadItem(itemCode)
                        End If
                        dgvItemList.Rows.RemoveAt(e.RowIndex)
                        f.Dispose()
                    End If
                Case chQTY.Index
                    If dgvItemList.Item(e.ColumnIndex, e.RowIndex).Value = "" OrElse IsNothing(dgvItemList.Item(e.ColumnIndex, e.RowIndex).Value) Then
                        dgvItemList.Item(e.ColumnIndex, e.RowIndex).Value = 0
                    ElseIf IsNumeric(dgvItemList.Item(chUnitPrice.Index, e.RowIndex).Value) AndAlso IsNumeric(dgvItemList.Item(chQTY.Index, e.RowIndex).Value) Then
                        Recompute(e.RowIndex, e.ColumnIndex)
                        ComputeTotal()
                    End If
                Case chUnitPrice.Index
                    If dgvItemList.Item(e.ColumnIndex, e.RowIndex).Value = "" OrElse IsNothing(dgvItemList.Item(e.ColumnIndex, e.RowIndex).Value) Then
                        dgvItemList.Item(e.ColumnIndex, e.RowIndex).Value = 0
                    ElseIf IsNumeric(dgvItemList.Item(chUnitPrice.Index, e.RowIndex).Value) AndAlso IsNumeric(dgvItemList.Item(chQTY.Index, e.RowIndex).Value) Then
                        Recompute(e.RowIndex, e.ColumnIndex)
                        ComputeTotal()
                        dgvItemList.Item(chUnitPrice.Index, e.RowIndex).Value = CDec(dgvItemList.Item(chUnitPrice.Index, e.RowIndex).Value).ToString("N2")
                    End If
                Case chDRRate.Index
                    If dgvItemList.Item(e.ColumnIndex, e.RowIndex).Value = "" OrElse IsNothing(dgvItemList.Item(e.ColumnIndex, e.RowIndex).Value) Then
                        dgvItemList.Item(e.ColumnIndex, e.RowIndex).Value = 0
                    ElseIf IsNumeric(dgvItemList.Item(chDRRate.Index, e.RowIndex).Value) AndAlso IsNumeric(dgvItemList.Item(chQTY.Index, e.RowIndex).Value) Then
                        Recompute(e.RowIndex, e.ColumnIndex)
                        ComputeTotal()
                    End If
            End Select
        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        End Try
    End Sub

    Private Sub Recompute(ByVal RowID As Integer, ByVal ColID As Integer)
        Dim gross, VAT, discount, net, baseVAT, DRPrice As Decimal
        If RowID <> -1 Then
            If IsNumeric(dgvItemList.Item(chGross.Index, RowID).Value) Then
                ' GET GROSS AMOUNT (VAT INCLUSIVE)
                gross = CDec(dgvItemList.Item(chUnitPrice.Index, RowID).Value) * CDec(dgvItemList.Item(chQTY.Index, RowID).Value)
                DRPrice = CDec(dgvItemList.Item(chDRRate.Index, RowID).Value) * CDec(dgvItemList.Item(chQTY.Index, RowID).Value)
                baseVAT = gross

                net = baseVAT - discount + VAT + DRPrice
                dgvItemList.Item(chDRPrice.Index, RowID).Value = Format(DRPrice, "#,###,###,###.00").ToString()
                dgvItemList.Item(chGross.Index, RowID).Value = Format(gross, "#,###,###,###.00").ToString()
                dgvItemList.Item(chNetAmount.Index, RowID).Value = Format(net, "#,###,###,###.00").ToString()
                ComputeTotal()

            End If
        End If

    End Sub

    Private Sub ComputeTotal()
        If dgvItemList.Rows.Count > 0 Then
            Dim b As Decimal = 0
            Dim a As Decimal = 0
            Dim c As Decimal = 0
            Dim d As Decimal = 0
            ' COMPUTE GROSS
            For i As Integer = 0 To dgvItemList.Rows.Count - 1
                If Val(dgvItemList.Item(chGross.Index, i).Value) <> 0 Then
                    If IsNumeric(dgvItemList.Item(chGross.Index, i).Value) Then
                        a = a + Double.Parse(dgvItemList.Item(chGross.Index, i).Value).ToString
                    End If
                End If
            Next

            ' COMPUTE NET
            For i As Integer = 0 To dgvItemList.Rows.Count - 1
                If Val(dgvItemList.Item(chNetAmount.Index, i).Value) <> 0 Then
                    If IsNumeric(dgvItemList.Item(chNetAmount.Index, i).Value) Then
                        d = d + Double.Parse(dgvItemList.Item(chNetAmount.Index, i).Value).ToString
                    End If
                End If
            Next
        End If



    End Sub
    

    Public Sub LoadItem(ByVal ItemCode As String, Optional UOM As String = "", Optional QTY As Integer = 1)
        Try
            'Dim query As String
            'Dim netPrice As Decimal
            'query = " SELECT  ItemCode,  ItemName, UOM,  " & _
            '        "         CASE WHEN VATable = 1 " & _
            '        "              THEN (CASE WHEN VATInclusive = 1 THEN UnitPrice ELSE UnitPrice*1.12 END)  " & _
            '        "              ELSE UnitPrice " & _
            '        "         END AS Net_Price, WHSE " & _
            '        " FROM    viewItem_Price " & _
            '        " WHERE   ItemCode = @ItemCode "
            'SQL.FlushParams()
            'SQL.AddParam("@ItemCode", ItemCode)
            'SQL.ReadQuery(query)
            'If SQL.SQLDR.Read Then
            '    If UOM = "" Then
            '        UOM = SQL.SQLDR("UOM").ToString
            '    End If
            '    netPrice = SQL.SQLDR("Net_Price")
            '    dgvItemList.Rows.Add(New String() {SQL.SQLDR("ItemCode").ToString, _
            '                                  SQL.SQLDR("ItemName").ToString, _
            '                                 UOM, _
            '                                  QTY, _
            '                                  GetWHSEDesc(SQL.SQLDR("WHSE").ToString), _
            '                                  Format(netPrice, "#,###,###,###.00").ToString})
            '    LoadWHSE(dgvItemList.Rows.Count - 2)
            'End If

            Dim query, AccntCode As String
            Dim netPrice As Decimal
            query = " SELECT  ItemCode,  ItemName, ItemUOM,  " & _
                    "         ISNULL(ID_SC,0) AS ID_SC, ID_Warehouse, AD_Cos " & _
                    " FROM    tblItem_Master " & _
                    " WHERE   ItemCode = @ItemCode "
            SQL.FlushParams()
            SQL.AddParam("@ItemCode", ItemCode)
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                If UOM = "" Then
                    UOM = SQL.SQLDR("ItemUOM").ToString
                End If
                netPrice = SQL.SQLDR("ID_SC")
                AccntCode = SQL.SQLDR("AD_Cos").ToString
                dgvItemList.Rows.Add(New String() {SQL.SQLDR("ItemCode").ToString, _
                                              SQL.SQLDR("ItemName").ToString, _
                                             UOM, _
                                              QTY, _
                                              GetWHSEDesc(SQL.SQLDR("ID_Warehouse").ToString), _
                                              Format(netPrice, "#,###,###,###.00").ToString, _
                                                   Format(netPrice, "#,###,###,###.00").ToString, _
                                                Format(netPrice, "#,###,###,###.00").ToString, _
                                                   "", _
                                                  AccntCode, _
                                                   "0.00", _
                                                   "0.00"})
                LoadWHSE(dgvItemList.Rows.Count - 2)
            End If
        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        Finally
            SQL.FlushParams()
        End Try
    End Sub


    Private Sub SaveDR()
        Try
            activityStatus = True
            Dim insertSQL As String
            insertSQL = " INSERT INTO " & _
                                " tblDR(TransID, DR_No, BranchCode, BusinessCode, VCECode, DateDR, DateDeliver, Remarks, SO_Ref, PL_Ref, PO_Ref, CusSONo, ForECS,  PlateNumber, DriverName, WhoCreated) " & _
                                " VALUES (@TransID, @DR_No, @BranchCode, @BusinessCode, @VCECode,  @DateDR, @DateDeliver, @Remarks, @SO_Ref, @PL_Ref, @PO_Ref, @CusSONo, @ForECS, @PlateNumber, @DriverName, @WhoCreated) "
            SQL.FlushParams()
            SQL.AddParam("@TransID", TransID)
            SQL.AddParam("@DR_No", DRNo)
            SQL.AddParam("@BranchCode", BranchCode)
            SQL.AddParam("@BusinessCode", BusinessType)
            SQL.AddParam("@VCECode", txtVCECode.Text)
            SQL.AddParam("@DateDR", dtpDocDate.Value.Date)
            SQL.AddParam("@DateDeliver", dtpActual.Value.Date)
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@SO_Ref", SO_ID)
            SQL.AddParam("@PL_Ref", PL_ID)
            SQL.AddParam("@PO_Ref", PO_ID)
            SQL.AddParam("@CusSONo", txtCustSO.Text)
            SQL.AddParam("@PlateNumber", txtPlateNumber.Text)
            SQL.AddParam("@DriverName", txtDriverName.Text)
            SQL.AddParam("@ForECS", chkForECS.Checked)
            SQL.AddParam("@WhoCreated", UserID)
            SQL.ExecNonQuery(insertSQL)

            Dim line As Integer = 1
            Dim ItemCode, Description, UOM, WHSE, Ref_SO_RecID, AccntCode As String
            Dim QTY, UnitCost, UnitPrice, GrossAmount, NetAmount, DRRate, DRPrice As Decimal
            For Each row As DataGridViewRow In dgvItemList.Rows
                If Not row.Cells(chQTY.Index).Value Is Nothing AndAlso Not row.Cells(chItemCode.Index).Value Is Nothing Then
                    ItemCode = IIf(row.Cells(chItemCode.Index).Value = Nothing, "", row.Cells(chItemCode.Index).Value)
                    Description = IIf(row.Cells(chItemDesc.Index).Value = Nothing, "", row.Cells(chItemDesc.Index).Value)
                    UOM = IIf(row.Cells(chUOM.Index).Value = Nothing, "", row.Cells(chUOM.Index).Value)
                    Ref_SO_RecID = IIf(row.Cells(chRefRecID.Index).Value = Nothing, "", row.Cells(chRefRecID.Index).Value)
                    UnitPrice = IIf(row.Cells(chUnitPrice.Index).Value = Nothing, "0.00", CDec(row.Cells(chUnitPrice.Index).Value).ToString("N2"))
                    GrossAmount = IIf(row.Cells(chGross.Index).Value = Nothing, "0.00", CDec(row.Cells(chGross.Index).Value).ToString("N2"))
                    NetAmount = IIf(row.Cells(chNetAmount.Index).Value = Nothing, "0.00", CDec(row.Cells(chNetAmount.Index).Value).ToString("N2"))
                    AccntCode = IIf(row.Cells(chAccnt.Index).Value = Nothing, "", row.Cells(chAccnt.Index).Value)
                    DRRate = IIf(row.Cells(chDRRate.Index).Value = Nothing, "0.00", CDec(row.Cells(chDRRate.Index).Value).ToString("N2"))
                    DRPrice = IIf(row.Cells(chDRPrice.Index).Value = Nothing, "0.00", CDec(row.Cells(chDRPrice.Index).Value).ToString("N2"))
                    UnitCost = GetAverageCost(ItemCode)
                    If IsNumeric(row.Cells(chQTY.Index).Value) Then
                        QTY = CDec(row.Cells(chQTY.Index).Value)
                    Else
                        QTY = 1
                    End If
                    WHSE = IIf(row.Cells(chWHSE.Index).Value = Nothing, "", GetWHSECode(row.Cells(chWHSE.Index).Value))
                    insertSQL = " INSERT INTO " & _
                         " tblDR_Details(TransId, ItemCode, Description, UOM, QTY, UnitPrice, GrossAmount, NetAmount, WHSE, Ref_SO_RecID, AccntCode, DRRate, DRPrice, LineNum, WhoCreated) " & _
                         " VALUES(@TransId, @ItemCode, @Description, @UOM, @QTY, @UnitPrice, @GrossAmount, @NetAmount, @WHSE, @Ref_SO_RecID, @AccntCode,  @DRRate, @DRPrice, @LineNum, @WhoCreated) "
                    SQL.FlushParams()
                    SQL.AddParam("@TransID", TransID)
                    SQL.AddParam("@ItemCode", ItemCode)
                    SQL.AddParam("@Description", Description)
                    SQL.AddParam("@UOM", UOM)
                    SQL.AddParam("@QTY", QTY)
                    SQL.AddParam("@UnitPrice", UnitPrice)
                    SQL.AddParam("@GrossAmount", GrossAmount)
                    SQL.AddParam("@NetAmount", NetAmount)
                    SQL.AddParam("@WHSE", WHSE)
                    SQL.AddParam("@Ref_SO_RecID", Ref_SO_RecID)
                    SQL.AddParam("@AccntCode", AccntCode)
                    SQL.AddParam("@DRRate", DRRate)
                    SQL.AddParam("@DRPrice", DRPrice)
                    SQL.AddParam("@LineNum", line)
                    SQL.AddParam("@WhoCreated", UserID)
                    SQL.ExecNonQuery(insertSQL)
                    line += 1

                    'SaveINVTY("OUT", ModuleID, "DR", TransID, dtpDocDate.Value.Date, ItemCode, WHSE, QTY, UOM, UnitCost)
                End If
            Next
            UpdatePO(PO_ID, txtVCECode.Text)  ' UPDATE PO DETAILS STATUS
            'ComputeWAUC("DR", TransID)

            'insertSQL = " INSERT INTO " & _
            '            " tblJE_Header (AppDate, BranchCode, BusinessCode, RefType, RefTransID, Book, TotalDBCR, Remarks, WhoCreated) " & _
            '            " VALUES(@AppDate, @BranchCode, @BusinessCode, @RefType, @RefTransID, @Book, @TotalDBCR, @Remarks, @WhoCreated)"
            'SQL.FlushParams()
            'SQL.AddParam("@AppDate", dtpDocDate.Value.Date)
            'SQL.AddParam("@RefType", "DR")
            'SQL.AddParam("@RefTransID", TransID)
            'SQL.AddParam("@Book", "Inventory")
            'SQL.AddParam("@TotalDBCR", CDec(txtTotalCredit.Text))
            'SQL.AddParam("@Remarks", txtRemarks.Text)
            'SQL.AddParam("@BranchCode", BranchCode)
            'SQL.AddParam("@BusinessCode", BusinessType)
            'SQL.AddParam("@WhoCreated", UserID)
            'SQL.ExecNonQuery(insertSQL)

            'JETransID = LoadJE("DR", TransID)
            'line = 1
            'For Each item As DataGridViewRow In dgvEntry.Rows
            '    If item.Cells(chAccntCode.Index).Value <> Nothing Then
            '        insertSQL = " INSERT INTO " & _
            '                    " tblJE_Details(JE_No, AccntCode, VCECode, Debit, Credit, Particulars, RefNo, LineNumber) " & _
            '                    " VALUES(@JE_No, @AccntCode, @VCECode, @Debit, @Credit, @Particulars, @RefNo, @LineNumber)"
            '        SQL.FlushParams()
            '        SQL.AddParam("@JE_No", JETransID)
            '        SQL.AddParam("@AccntCode", item.Cells(chAccntCode.Index).Value.ToString)
            '        If item.Cells(chVCECode.Index).Value <> Nothing AndAlso item.Cells(chVCECode.Index).Value <> "" Then
            '            SQL.AddParam("@VCECode", item.Cells(chVCECode.Index).Value.ToString)
            '        Else
            '            SQL.AddParam("@VCECode", txtVCECode.Text)
            '        End If
            '        If item.Cells(chDebit.Index).Value AndAlso IsNumeric(item.Cells(chDebit.Index).Value) Then
            '            SQL.AddParam("@Debit", CDec(item.Cells(chDebit.Index).Value))
            '        Else
            '            SQL.AddParam("@Debit", 0)
            '        End If
            '        If item.Cells(chCredit.Index).Value <> Nothing AndAlso IsNumeric(item.Cells(chCredit.Index).Value) Then
            '            SQL.AddParam("@Credit", CDec(item.Cells(chCredit.Index).Value))
            '        Else
            '            SQL.AddParam("@Credit", 0)
            '        End If
            '        If item.Cells(chParticulars.Index).Value <> Nothing AndAlso item.Cells(chParticulars.Index).Value <> "" Then
            '            SQL.AddParam("@Particulars", item.Cells(chParticulars.Index).Value.ToString)
            '        Else
            '            SQL.AddParam("@Particulars", "")
            '        End If
            '        If item.Cells(chParticulars.Index).Value <> Nothing AndAlso item.Cells(chParticulars.Index).Value <> "" Then
            '            SQL.AddParam("@RefNo", item.Cells(chParticulars.Index).Value.ToString)
            '        Else
            '            SQL.AddParam("@RefNo", "")
            '        End If
            '        SQL.AddParam("@LineNumber", line)
            '        SQL.ExecNonQuery(insertSQL)
            '        line += 1
            '    End If
            'Next

        Catch ex As Exception
            activityStatus = False
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        Finally
            RecordActivity(UserID, ModuleID, Me.Name.ToString, "INSERT", "DR_No", txtTransNum.Text, BusinessType, BranchCode, "", activityStatus)
            SQL.FlushParams()
        End Try
    End Sub

    Private Sub GenerateEntry()
        Dim dataEntry As New DataTable
        dgvEntry.Rows.Clear()
        Dim query As String
        For Each row As DataGridViewRow In dgvItemList.Rows
            If row.Cells(chItemCode.Index).Value <> Nothing AndAlso row.Cells(chNetAmount.Index).Value > 0 Then
                query = " SELECT AD_COS, AccountTitle " & _
                        " FROM   tblItem_Master INNER JOIN tblCOA_Master " & _
                        " ON     tblItem_Master.AD_COS = tblCOA_Master.AccountCode " & _
                        " WHERE  ItemCode ='" & row.Cells(chItemCode.Index).Value & "' "
                SQL.ReadQuery(query)
                If SQL.SQLDR.Read() Then
                    dgvEntry.Rows.Add({SQL.SQLDR("AD_COS").ToString, GetAccntTitle(SQL.SQLDR("AD_COS").ToString), CDec(row.Cells(chNetAmount.Index).Value), "0.00", "DR:" & txtTransNum.Text})
                End If
            End If
        Next
        TotalDBCR()

        If txtTotalDebit.Text <> 0 Then
            query = " SELECT AccntCode, AccountTitle FROM tblDefaultAccount " & _
               " INNER JOIN " & _
               " tblCOA_Master ON " & _
               " tblCOA_Master.AccountCode = tblDefaultAccount.AccntCode " & _
               " WHERE ModuleID = '" & ModuleID & "' AND Type = 'Credit' " & _
               " ORDER BY AccountTitle "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read() Then
                dgvEntry.Rows.Add({SQL.SQLDR("AccntCode").ToString, SQL.SQLDR("AccountTitle").ToString, "0.00", CDec(txtTotalDebit.Text).ToString("N2"), ""})
            End If
        End If

        TotalDBCR()
    End Sub

    Private Sub TotalDBCR()
        ' COMPUTE TOTAL DEBIT
        Dim db As Decimal = 0

        For i As Integer = 0 To dgvEntry.Rows.Count - 1
            If Val(dgvEntry.Item(2, i).Value) <> 0 Then
                db = db + Double.Parse(dgvEntry.Item(2, i).Value).ToString("N2")
            End If
        Next
        txtTotalDebit.Text = db.ToString("N2")

        ' COMPUTE TOTAL CREDIT
        Dim b As Decimal = 0

        For i As Integer = 0 To dgvEntry.Rows.Count - 1
            If Val(dgvEntry.Item(3, i).Value) <> 0 Then
                b = b + Double.Parse(dgvEntry.Item(3, i).Value).ToString("N2")
            End If
        Next
        txtTotalCredit.Text = b.ToString("N2")
    End Sub

    Private Sub UpdateDR()
        Try

            activityStatus = True
            Dim insertSQL, updateSQL, deleteSQL As String
            updateSQL = " UPDATE    tblDR " & _
                        " SET       DR_No = @DR_No, BranchCode = @BranchCode, BusinessCode = @BusinessCode, DateDR = @DateDR, " & _
                        "           VCECode = @VCECode, DateDeliver = @DateDeliver, Remarks = @Remarks, ForECS = @ForECS, " & _
                        "           SO_Ref = @SO_Ref, PL_Ref = @PL_Ref, PO_Ref = @PO_Ref, CusSONo = @CusSONo, PlateNumber = @PlateNumber, DriverName = @DriverName, WhoModified = @WhoModified, DateModified = GETDATE() " & _
                        " WHERE     TransID = @TransID  "
            SQL.FlushParams()
            SQL.AddParam("@TransID", TransID)
            SQL.AddParam("@DR_No", DRNo)
            SQL.AddParam("@BranchCode", BranchCode)
            SQL.AddParam("@BusinessCode", BusinessType)
            SQL.AddParam("@DateDR", dtpDocDate.Value.Date)
            SQL.AddParam("@VCECode", txtVCECode.Text)
            SQL.AddParam("@DateDeliver", dtpActual.Value.Date)
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@SO_Ref", SO_ID)
            SQL.AddParam("@PL_Ref", PL_ID)
            SQL.AddParam("@PO_Ref", PO_ID)
            SQL.AddParam("@CusSONo", txtCustSO.Text)
            SQL.AddParam("@PlateNumber", txtPlateNumber.Text)
            SQL.AddParam("@DriverName", txtDriverName.Text)
            SQL.AddParam("@ForECS", chkForECS.Checked)
            SQL.AddParam("@WhoModified", UserID)
            SQL.ExecNonQuery(updateSQL)

            deleteSQL = " DELETE FROM tblDR_Details WHERE TransID =@TransID "
            SQL.FlushParams()
            SQL.AddParam("@TransID", TransID)
            SQL.ExecNonQuery(deleteSQL)

            Dim line As Integer = 1
            Dim ItemCode, Description, UOM, WHSE, Ref_SO_RecID, AccntCode As String
            Dim QTY, UnitCost, UnitPrice, GrossAmount, NetAmount, DRRate, DRPrice As Decimal
            For Each row As DataGridViewRow In dgvItemList.Rows
                If Not row.Cells(chQTY.Index).Value Is Nothing AndAlso Not row.Cells(chItemCode.Index).Value Is Nothing Then
                    ItemCode = IIf(row.Cells(chItemCode.Index).Value = Nothing, "", row.Cells(chItemCode.Index).Value)
                    Description = IIf(row.Cells(chItemDesc.Index).Value = Nothing, "", row.Cells(chItemDesc.Index).Value)
                    UOM = IIf(row.Cells(chUOM.Index).Value = Nothing, "", row.Cells(chUOM.Index).Value)
                    Ref_SO_RecID = IIf(row.Cells(chRefRecID.Index).Value = Nothing, "", row.Cells(chRefRecID.Index).Value)
                    UnitPrice = IIf(row.Cells(chUnitPrice.Index).Value = Nothing, "0.00", CDec(row.Cells(chUnitPrice.Index).Value).ToString("N2"))
                    GrossAmount = IIf(row.Cells(chGross.Index).Value = Nothing, "0.00", CDec(row.Cells(chGross.Index).Value).ToString("N2"))
                    NetAmount = IIf(row.Cells(chNetAmount.Index).Value = Nothing, "0.00", CDec(row.Cells(chNetAmount.Index).Value).ToString("N2"))
                    AccntCode = IIf(row.Cells(chAccnt.Index).Value = Nothing, "", row.Cells(chAccnt.Index).Value)
                    DRRate = IIf(row.Cells(chDRRate.Index).Value = Nothing, "0.00", CDec(row.Cells(chDRRate.Index).Value).ToString("N2"))
                    DRPrice = IIf(row.Cells(chDRPrice.Index).Value = Nothing, "0.00", CDec(row.Cells(chDRPrice.Index).Value).ToString("N2"))
                    UnitCost = GetAverageCost(ItemCode)
                    If IsNumeric(row.Cells(chQTY.Index).Value) Then
                        QTY = CDec(row.Cells(chQTY.Index).Value)
                    Else
                        QTY = 1
                    End If
                    WHSE = IIf(row.Cells(chWHSE.Index).Value = Nothing, "", GetWHSECode(row.Cells(chWHSE.Index).Value))
                    insertSQL = " INSERT INTO " & _
                         " tblDR_Details(TransId, ItemCode, Description, UOM, QTY, UnitPrice, GrossAmount, NetAmount, WHSE, Ref_SO_RecID, AccntCode, DRRate, DRPrice, LineNum, WhoCreated) " & _
                         " VALUES(@TransId, @ItemCode, @Description, @UOM, @QTY, @UnitPrice, @GrossAmount, @NetAmount, @WHSE, @Ref_SO_RecID, @AccntCode,  @DRRate, @DRPrice, @LineNum, @WhoCreated) "
                    SQL.FlushParams()
                    SQL.AddParam("@TransID", TransID)
                    SQL.AddParam("@ItemCode", ItemCode)
                    SQL.AddParam("@Description", Description)
                    SQL.AddParam("@UOM", UOM)
                    SQL.AddParam("@QTY", QTY)
                    SQL.AddParam("@UnitPrice", UnitPrice)
                    SQL.AddParam("@GrossAmount", GrossAmount)
                    SQL.AddParam("@NetAmount", NetAmount)
                    SQL.AddParam("@WHSE", WHSE)
                    SQL.AddParam("@Ref_SO_RecID", Ref_SO_RecID)
                    SQL.AddParam("@AccntCode", AccntCode)
                    SQL.AddParam("@DRRate", DRRate)
                    SQL.AddParam("@DRPrice", DRPrice)
                    SQL.AddParam("@LineNum", line)
                    SQL.AddParam("@WhoCreated", UserID)
                    SQL.ExecNonQuery(insertSQL)
                    line += 1

                    'SaveINVTY("OUT", ModuleID, "DR", TransID, dtpDocDate.Value.Date, ItemCode, WHSE, QTY, UOM, UnitCost)
                End If
            Next


            'JETransID = LoadJE("DR", TransID)

            'If JETransID = 0 Then
            '    insertSQL = " INSERT INTO " & _
            '                " tblJE_Header (AppDate, BranchCode, BusinessCode, RefType, RefTransID, Book, TotalDBCR, Remarks, WhoCreated) " & _
            '                " VALUES(@AppDate, @BranchCode, @BusinessCode, @RefType, @RefTransID, @Book, @TotalDBCR, @Remarks, @WhoCreated)"
            '    SQL.FlushParams()
            '    SQL.AddParam("@AppDate", dtpDocDate.Value.Date)
            '    SQL.AddParam("@RefType", "DR")
            '    SQL.AddParam("@RefTransID", TransID)
            '    SQL.AddParam("@Book", "Inventory")
            '    SQL.AddParam("@TotalDBCR", CDec(txtTotalCredit.Text))
            '    SQL.AddParam("@Remarks", txtRemarks.Text)
            '    SQL.AddParam("@BranchCode", BranchCode)
            '    SQL.AddParam("@BusinessCode", BusinessType)
            '    SQL.AddParam("@WhoCreated", UserID)
            '    SQL.ExecNonQuery(insertSQL)

            '    JETransID = LoadJE("DR", TransID)
            'Else
            '    updateSQL = " UPDATE tblJE_Header " & _
            '                " SET    AppDate = @AppDate, BranchCode = @BranchCode, BusinessCode = @BusinessCode, " & _
            '                "        RefType = @RefType, RefTransID = @RefTransID, Book = @Book, TotalDBCR = @TotalDBCR, " & _
            '                "        Remarks = @Remarks, WhoModified = @WhoModified, DateModified = GETDATE() " & _
            '                " WHERE  JE_No = @JE_No "
            '    SQL.FlushParams()
            '    SQL.AddParam("@JE_No", JETransID)
            '    SQL.AddParam("@AppDate", dtpDocDate.Value.Date)
            '    SQL.AddParam("@RefType", "DR")
            '    SQL.AddParam("@RefTransID", TransID)
            '    SQL.AddParam("@Book", "Inventory")
            '    SQL.AddParam("@TotalDBCR", CDec(txtTotalCredit.Text))
            '    SQL.AddParam("@Remarks", txtRemarks.Text)
            '    SQL.AddParam("@BranchCode", BranchCode)
            '    SQL.AddParam("@BusinessCode", BusinessType)
            '    SQL.AddParam("@WhoModified", UserID)
            '    SQL.ExecNonQuery(updateSQL)
            'End If

            'line = 1

            '' DELETE ACCOUNTING ENTRIES
            'deleteSQL = " DELETE FROM tblJE_Details  WHERE  JE_No = @JE_No "
            'SQL.FlushParams()
            'SQL.AddParam("@JE_No", JETransID)
            'SQL.ExecNonQuery(deleteSQL)

            '' INSERT NEW ENTRIES
            'For Each item As DataGridViewRow In dgvEntry.Rows
            '    If item.Cells(chAccntCode.Index).Value <> Nothing Then
            '        insertSQL = " INSERT INTO " & _
            '                    " tblJE_Details(JE_No, AccntCode, VCECode, Debit, Credit, Particulars, RefNo, LineNumber) " & _
            '                    " VALUES(@JE_No, @AccntCode, @VCECode, @Debit, @Credit, @Particulars, @RefNo, @LineNumber)"
            '        SQL.FlushParams()
            '        SQL.AddParam("@JE_No", JETransID)
            '        SQL.AddParam("@AccntCode", item.Cells(chAccntCode.Index).Value.ToString)
            '        If item.Cells(chVCECode.Index).Value <> Nothing AndAlso item.Cells(chVCECode.Index).Value <> "" Then
            '            SQL.AddParam("@VCECode", item.Cells(chVCECode.Index).Value.ToString)
            '        Else
            '            SQL.AddParam("@VCECode", txtVCECode.Text)
            '        End If
            '        If item.Cells(chDebit.Index).Value AndAlso IsNumeric(item.Cells(chDebit.Index).Value) Then
            '            SQL.AddParam("@Debit", CDec(item.Cells(chDebit.Index).Value))
            '        Else
            '            SQL.AddParam("@Debit", 0)
            '        End If
            '        If item.Cells(chCredit.Index).Value <> Nothing AndAlso IsNumeric(item.Cells(chCredit.Index).Value) Then
            '            SQL.AddParam("@Credit", CDec(item.Cells(chCredit.Index).Value))
            '        Else
            '            SQL.AddParam("@Credit", 0)
            '        End If
            '        If item.Cells(chParticulars.Index).Value <> Nothing AndAlso item.Cells(chParticulars.Index).Value <> "" Then
            '            SQL.AddParam("@Particulars", item.Cells(chParticulars.Index).Value.ToString)
            '        Else
            '            SQL.AddParam("@Particulars", "")
            '        End If
            '        If item.Cells(chParticulars.Index).Value <> Nothing AndAlso item.Cells(chParticulars.Index).Value <> "" Then
            '            SQL.AddParam("@RefNo", item.Cells(chParticulars.Index).Value.ToString)
            '        Else
            '            SQL.AddParam("@RefNo", "")
            '        End If
            '        SQL.AddParam("@LineNumber", line)
            '        SQL.ExecNonQuery(insertSQL)
            '        line += 1
            '    End If
            'Next
        Catch ex As Exception
            activityStatus = False
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        Finally
            RecordActivity(UserID, ModuleID, Me.Name.ToString, "UPDATE", "DR_No", txtTransNum.Text, BusinessType, BranchCode, "", activityStatus)
            SQL.FlushParams()
        End Try

    End Sub

    Private Sub btnSearchVCE_Click(sender As System.Object, e As System.EventArgs) Handles btnSearchVCE.Click
        Dim f As New frmVCE_Search
        f.ShowDialog()
        txtVCECode.Text = f.VCECode
        txtVCEName.Text = f.VCEName
    End Sub

    Private Sub LoadSO(ByVal SO As String)
        ClearText()
        Dim query As String
        query = " SELECT  TransID, SO_No, DateSO AS Date, tblSO.VCECode, VCEName, Remarks " & _
                " FROM   tblSO INNER JOIN tblVCE_Master " & _
                " ON     tblSO.VCECode = tblVCE_Master.VCECode " & _
                " WHERE  tblSO.Status ='Active' AND TransID ='" & SO & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            SO_ID = SQL.SQLDR("TransID")
            txtSORef.Text = SQL.SQLDR("SO_No")
            txtVCECode.Text = SQL.SQLDR("VCECode").ToString
            txtVCEName.Text = SQL.SQLDR("VCEName").ToString
            txtRemarks.Text = SQL.SQLDR("Remarks").ToString


            query = " SELECT tblSO_Details.ItemCode, Description, UOM, Unserved AS QTY , ISNULL(viewSO_Unserved.UnitPrice,0) AS UnitPrice, viewSO_Unserved.WHSE, tblSO_Details.RecordID  " & _
                    " FROM   tblSO_Details INNER JOIN viewSO_Unserved " & _
                    " ON     tblSO_Details.TransID = viewSO_Unserved.TransID " & _
                    " AND    tblSO_Details.ItemCode = viewSO_Unserved.ItemCode " & _
                    " AND    tblSO_Details.RecordID = viewSO_Unserved.RecordID " & _
                    " WHERE  tblSO_Details.TransID ='" & SO & "' "
            dgvItemList.Rows.Clear()
            SQL.GetQuery(query)
            Dim ctr As Integer = 0
            If SQL.SQLDS.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In SQL.SQLDS.Tables(0).Rows
                    dgvItemList.Rows.Add(row(0).ToString, row(1).ToString, row(2).ToString, _
                                         CDec(row(3)).ToString("N2"), GetWHSEDesc(row(5).ToString), row(4).ToString, row(6).ToString)
                    LoadWHSE(ctr)
                    ctr += 1
                Next
            End If
        Else
            ClearText()
        End If
    End Sub


    Private Sub LoadPL(ByVal ID As String, ByVal WHSE As String)
        Dim query As String
        query = " SELECT    TransID, PL_No, SO_Ref, DatePL AS Date, tblPL.VCECode, VCEName, Remarks " & _
                " FROM      tblPL INNER JOIN tblVCE_Master " & _
                " ON        tblPL.VCECode = tblVCE_Master.VCECode " & _
                " WHERE     tblPL.Status ='Active' AND TransID ='" & ID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            PL_ID = SQL.SQLDR("TransID")
            SO_ID = SQL.SQLDR("SO_Ref")
            txtPLRef.Text = SQL.SQLDR("PL_No")
            txtVCECode.Text = SQL.SQLDR("VCECode").ToString
            txtVCEName.Text = SQL.SQLDR("VCEName").ToString
            txtRemarks.Text = SQL.SQLDR("Remarks").ToString
            txtSORef.Text = LoadSONo(SO_ID)

            query = " SELECT tblPL_Details.ItemCode, Description, UOM, tblPL_Details.PickQTY AS QTY , " & _
                    " ISNULL(ID_SC,0) AS UnitPrice, tblPL_Details.WHSE, viewSO_Unserved.RecordID, AD_COS  " & _
                    " FROM   tblPL_Details " & _
                    " INNER JOIN viewSO_Unserved " & _
                    " ON     viewSO_Unserved.TransID = '" & SO_ID & "'" & _
                    " AND    tblPL_Details.ItemCode = viewSO_Unserved.ItemCode " & _
                    " INNER JOIN " & _
                    " tblItem_Master ON " & _
                    " tblItem_Master.ItemCode = tblPL_Details.ItemCode " & _
                    " WHERE  tblPL_Details.TransID ='" & ID & "' "
            dgvItemList.Rows.Clear()
            SQL.GetQuery(query)
            Dim ctr As Integer = 0
            Dim netamount As Decimal
            If SQL.SQLDS.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In SQL.SQLDS.Tables(0).Rows
                    netamount = row(3) * row(4)
                    dgvItemList.Rows.Add(row(0).ToString, row(1).ToString, row(2).ToString, _
                                         CDec(row(3)).ToString("N2"), GetWHSEDesc(row(5).ToString), row(4).ToString, row(4).ToString, CDec(netamount).ToString("N2"), row(6).ToString, row(7).ToString)
                    LoadWHSE(ctr)
                    ctr += 1
                Next
            End If
        Else
            ClearText()
        End If
    End Sub

    Private Sub tsbSearch_Click(sender As System.Object, e As System.EventArgs) Handles tsbSearch.Click
        If Not AllowAccess("DR_VIEW") Then
            msgRestricted()
        Else
            Dim f As New frmLoadTransactions
            f.ShowDialog("DR")
            TransID = f.transID
            LoadDR(TransID)
            f.Dispose()
        End If
    End Sub

    Private Sub tsbNew_Click(sender As System.Object, e As System.EventArgs) Handles tsbNew.Click
        If Not AllowAccess("DR_ADD") Then
            msgRestricted()
        Else
            ClearText()
            TransID = ""
            DRNo = ""

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

    Private Sub dgvItemList_CellClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvItemList.CellClick
        If e.ColumnIndex = chWHSE.Index Then
            If e.RowIndex <> -1 AndAlso dgvItemList.Rows.Count > 0 Then
                Try
                    LoadWHSE(e.RowIndex)
                    Dim dgvCol As DataGridViewComboBoxColumn
                    dgvCol = dgvItemList.Columns.Item(e.ColumnIndex)
                    dgvItemList.BeginEdit(True)
                    dgvCol.Selected = True
                    DirectCast(dgvItemList.EditingControl, DataGridViewComboBoxEditingControl).DroppedDown = True
                    Dim editingComboBox As ComboBox = TryCast(sender, ComboBox)
                Catch ex As NullReferenceException
                Catch ex As Exception
                    SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
                End Try

            End If
        End If
    End Sub

    Private Sub dgvItemList_EditingControlShowing(sender As System.Object, e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dgvItemList.EditingControlShowing
        ' GET THE EDITING CONTROL
        Dim editingComboBox As ComboBox = TryCast(e.Control, ComboBox)
        If Not editingComboBox Is Nothing Then
            ' REMOVE AN EXISTING EVENT-HANDLER TO AVOID ADDING MULTIPLE HANDLERS WHEN THE EDITING CONTROL IS REUSED
            RemoveHandler editingComboBox.SelectionChangeCommitted, New EventHandler(AddressOf editingComboBox_SelectionChangeCommitted)

            ' ADD THE EVENT HANDLER
            AddHandler editingComboBox.SelectionChangeCommitted, AddressOf editingComboBox_SelectionChangeCommitted

            ' PREVENT THIS HANDLER FROM FIRING TWICE
            RemoveHandler dgvItemList.EditingControlShowing, AddressOf dgvItemList_EditingControlShowing
        End If
    End Sub

    Private Sub editingComboBox_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim rowIndex, columnIndex As Integer
        'Get the editing control
        Dim editingComboBox As ComboBox = TryCast(sender, ComboBox)
        If editingComboBox Is Nothing Then Exit Sub
        'Show your Message Boxes
        If editingComboBox.SelectedIndex <> -1 Then
            rowIndex = dgvItemList.SelectedCells(0).RowIndex
            columnIndex = dgvItemList.SelectedCells(0).ColumnIndex
            If dgvItemList.SelectedCells.Count > 0 Then
                Dim tempCell As DataGridViewComboBoxCell = dgvItemList.Item(columnIndex, rowIndex)
                Dim temp As String = editingComboBox.Text
                dgvItemList.Item(columnIndex, rowIndex).Selected = False
                dgvItemList.EndEdit(True)
                tempCell.Value = temp
            End If
        End If

        'Remove the handle to this event. It will be readded each time a new combobox selection causes the EditingControlShowing Event to fire
        RemoveHandler editingComboBox.SelectionChangeCommitted, AddressOf editingComboBox_SelectionChangeCommitted
        'Re-enable the EditingControlShowing event so the above can take place.
        AddHandler dgvItemList.EditingControlShowing, AddressOf dgvItemList_EditingControlShowing
    End Sub

    Private Sub LoadWHSE(Optional ByVal SelectedIndex As Integer = -1)
        Try
            Dim dgvCB As New DataGridViewComboBoxCell
            dgvCB = dgvItemList.Item(chWHSE.Index, SelectedIndex)
            dgvCB.Items.Clear()
            ' ADD ALL WHSEc
            Dim query As String
            query = " SELECT Description FROM tblWarehouse WHERE Status ='Active' "
            SQL.ReadQuery(query)
            dgvCB.Items.Clear()
            While SQL.SQLDR.Read
                dgvCB.Items.Add(SQL.SQLDR("Description").ToString)
            End While
            If dgvCB.Value <> Nothing AndAlso Not dgvCB.Items.Contains(dgvCB.Value) Then
                dgvCB.Items.Add(dgvCB.Value)
            End If
        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        End Try
    End Sub

    Private Sub tsbEdit_Click(sender As System.Object, e As System.EventArgs) Handles tsbEdit.Click
        If Not AllowAccess("DR_EDIT") Then
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

    Private Sub tsbPrevious_Click(sender As System.Object, e As System.EventArgs) Handles tsbPrevious.Click
        If DRNo <> "" Then
            Dim query As String
            query = " SELECT Top 1 TransID FROM tblDR  WHERE DR_No < '" & DRNo & "' ORDER BY DR_No DESC "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                TransID = SQL.SQLDR("TransID").ToString
                LoadDR(TransID)
            Else
                Msg("Reached the beginning of record!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub

    Private Sub tsbNext_Click(sender As System.Object, e As System.EventArgs) Handles tsbNext.Click
        If DRNo <> "" Then
            Dim query As String
            query = " SELECT Top 1 TransID FROM tblDR  WHERE DR_No > '" & DRNo & "' ORDER BY DR_No ASC "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                TransID = SQL.SQLDR("TransID").ToString
                LoadDR(TransID)
            Else
                Msg("Reached the end of record!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub


    Private Sub tsbClose_Click(sender As System.Object, e As System.EventArgs) Handles tsbClose.Click


        ' Toolstrip Buttons
        If DRNo = "" Then
            ClearText()
            EnableControl(False)
            tsbEdit.Enabled = False
            tsbCancel.Enabled = False
            tsbPrevious.Enabled = False
            tsbNext.Enabled = False
            tsbPrint.Enabled = False
        Else
            LoadDR(TransID)
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

    Private Sub tsbCopyPO_Click(sender As System.Object, e As System.EventArgs) Handles tsbCopySO.Click

        Dim f As New frmLoadTransactions
        f.cbFilter.SelectedItem = "Status"
        f.txtFilter.Text = "Active"
        f.txtFilter.Enabled = False
        f.cbFilter.Enabled = False
        f.btnSearch.Enabled = False
        f.ShowDialog("SO")
        LoadSO(f.transID)
        f.Dispose()
    End Sub

    Private Sub tsbCancel_Click(sender As System.Object, e As System.EventArgs) Handles tsbCancel.Click
        If Not AllowAccess("DR_DEL") Then
            msgRestricted()
        Else
            If txtTransNum.Text <> "" Then
                If MsgBox("Are you sure you want to cancel this record?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "JADE Message Alert") = MsgBoxResult.Yes Then
                    Try
                        activityStatus = True
                        Dim deleteSQL As String
                        deleteSQL = " UPDATE  tblDR SET Status ='Cancelled' WHERE TransID = @TransID "
                        SQL.FlushParams()
                        DRNo = txtTransNum.Text
                        SQL.AddParam("@TransID", TransID)
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

                        DRNo = txtTransNum.Text
                        LoadDR(TransID)
                    Catch ex As Exception
                        activityStatus = True
                        SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
                    Finally
                        RecordActivity(UserID, ModuleID, Me.Name.ToString, "CANCEL", "DR_No", DRNo, BusinessType, BranchCode, "", activityStatus)
                        SQL.FlushParams()
                    End Try
                End If
            End If
        End If
    End Sub

    Private Sub frmDR_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
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

    Private Sub tsbSave_Click(sender As System.Object, e As System.EventArgs) Handles tsbSave.Click
        If txtVCECode.Text = "" Then
            Msg("Please enter VCECode!", MsgBoxStyle.Exclamation)
        ElseIf TransID = "" Then
            If MsgBox("Saving New Record, Click Yes to confirm", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "JADE Message Alert") = MsgBoxResult.Yes Then
                TransID = GenerateTransID(ColumnID, DBTable)
                DRNo = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
                txtTransNum.Text = DRNo
                'GenerateEntry()
                SaveDR()
                Msg("Record Saved Succesfully!", MsgBoxStyle.Information)
                DRNo = txtTransNum.Text
                LoadDR(TransID)
            End If
        Else
            If MsgBox("Updating Record, Click Yes to confirm", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "JADE Message Alert") = MsgBoxResult.Yes Then
                'GenerateEntry()
                UpdateDR()
                Msg("Record Updated Succesfully!", MsgBoxStyle.Information)
                DRNo = txtTransNum.Text
                LoadDR(TransID)
            End If
        End If
    End Sub

    Private Sub tsbPrint_Click(sender As System.Object, e As System.EventArgs) Handles tsbPrint.Click
        Dim f As New frmReport_Display
        f.ShowDialog("DR", TransID)
        f.Dispose()
    End Sub

    Private Sub dgvItemList_DataError(sender As System.Object, e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles dgvItemList.DataError
        Try

        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        End Try
    End Sub

    Private Sub FromPickListToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FromPickListToolStripMenuItem.Click
        Dim f As New frmLoadTransactions
        f.cbFilter.SelectedItem = "Status"
        f.txtFilter.Text = "Active"
        f.txtFilter.Enabled = False
        f.cbFilter.Enabled = False
        f.btnSearch.Enabled = False
        f.ShowDialog("PL-DR")
        LoadPL(f.transID, f.itemCode)
        f.Dispose()
    End Sub


    Private Sub dgvEntry_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvEntry.CellContentClick

    End Sub

    Private Sub dgvItemList_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvItemList.CellContentClick

    End Sub

    Private Sub FromPOToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FromPOToolStripMenuItem.Click
        Dim f As New frmLoadTransactions
        f.cbFilter.SelectedItem = "Status"
        f.txtFilter.Text = "Active"
        f.txtFilter.Enabled = False
        f.cbFilter.Enabled = False
        f.btnSearch.Enabled = False
        f.ShowDialog("Sub PO")
        LoadPO(f.transID, f.itemCode)
        f.Dispose()
    End Sub

    Private Sub LoadPO(ByVal TransNum As String, ByVal SupplierCode As String)
        Dim query As String
        query = "  SELECT   tblPO.TransID, PO_No, PurchaseType, DateDeliver, tblPO.Remarks,  tblPO.Status  " & _
                " FROM     tblPO " & _
                " WHERE     TransID = @TransID  "
        SQL.FlushParams()
        SQL.AddParam("@TransID", TransNum)
        SQL.ReadQuery(query)
        SQL.FlushParams()
        If SQL.SQLDR.Read Then
            PO_ID = SQL.SQLDR("TransID")
            txtPORef.Text = SQL.SQLDR("PO_No").ToString
            txtRemarks.Text = SQL.SQLDR("Remarks").ToString
            txtStatus.Text = "Open"
            query = "  SELECT TransID, POdetails.ItemCode, Description, UOM, QTY, POdetails.VCECode, UnitPrice, POdetails.VATable, VATInc, GrossAmount, VATAmount, NetAmount, AD_Inv , RecordID " & _
                    "  FROM  " & _
                    "  (  " & _
                    "    SELECT    TransID, ItemCode, Description, UOM, QTY,  " & _
                    " 			VCECode,  UnitPrice, VATable, VATInc, GrossAmount, VATAmount, NetAmount, RecordID  " & _
                    "    FROM tblPO_Details  " & _
                    "  ) AS POdetails  " & _
                    "  LEFT JOIN tblVCE_Master  " & _
                    "  ON        tblVCE_Master.VCECode = POdetails.VCECode  " & _
                    "  LEFT JOIN tblItem_Master  " & _
                    "  ON        tblItem_Master.ItemCode = POdetails.ItemCode  " & _
                    "  WHERE     POdetails.TransID = @TransID AND POdetails.VCECode = @SupplierCode AND tblVCE_Master.Status ='Active'  "
            dgvItemList.Rows.Clear()
            SQL.FlushParams()
            SQL.AddParam("@TransID", TransNum)
            SQL.AddParam("@SupplierCode", SupplierCode)
            SQL.ReadQuery(query)

            While SQL.SQLDR.Read
                txtVCECode.Text = SQL.SQLDR("VCECode").ToString
                dgvItemList.Rows.Add(New String() {SQL.SQLDR("ItemCode").ToString, SQL.SQLDR("Description").ToString, _
                                                            SQL.SQLDR("UOM").ToString, SQL.SQLDR("QTY").ToString, "", _
                                                             CDec(SQL.SQLDR("UnitPrice")).ToString("N2"), CDec(SQL.SQLDR("GrossAmount")).ToString("N2"), _
                                                        CDec(SQL.SQLDR("NetAmount")).ToString("N2"), SQL.SQLDR("RecordID").ToString, SQL.SQLDR("AD_Inv").ToString, "0.00", "0.00"})
            End While
            txtVCEName.Text = GetVCEName(txtVCECode.Text)
            SQL.FlushParams()
            ComputeTotal()
        Else
            ClearText()
        End If
    End Sub

    Private Sub UpdatePO(ByVal ID As Integer, ByVal Code As String)
        If ID <> 0 Then
            Dim updateSQL As String
            updateSQL = " UPDATE tblPO_Details SET Status = 'Closed'  " & _
                        " WHERE TransID = '" & ID & "' " & _
                        " AND     VCECode  ='" & Code & "' "
            SQL.ExecNonQuery(updateSQL)
        End If
    End Sub

    Private Sub txtVCEName_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtVCEName.TextChanged

    End Sub
End Class