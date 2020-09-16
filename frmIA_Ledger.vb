Public Class frmIA_Ledger
    Dim itemCode As String
    Dim showCost As Boolean
    Dim ModuleID As String = "IA"

    Public Overloads Function ShowDialog(ByVal code As String, ByVal withCost As Boolean) As Boolean
        itemCode = code
        showCost = withCost
        MyBase.ShowDialog()
        Return True
    End Function


    Private Sub frmIA_Ledger_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        LoadLedger()
        LoadStock
    End Sub

    Private Sub LoadStock()
        Dim query As String
        query = " SELECT CASE WHEN tblWarehouse.Code IS NULL THEN tblProdWarehouse.Code ELSE tblWarehouse.Code END AS Code,  " & _
                " 	   CASE WHEN tblWarehouse.Code IS NULL THEN tblProdWarehouse.Description ELSE tblWarehouse.Description END AS Description,  " & _
                " 	   CAST(SUM(QTY) AS decimal(18,2)) AS QTY " & _
                " FROM " & _
                " ( " & _
                " 	SELECT  tblInventory.ItemCode,  " & _
                " 			 CAST((CASE WHEN MovementType = 'IN'  " & _
                " 						THEN tblInventory.TransQTY  / ISNULL(viewItem_UOM.QTY,1)  " & _
                " 						ELSE (tblInventory.TransQTY  / ISNULL(viewItem_UOM.QTY,1)) * -1  " & _
                " 				   END) AS decimal(18,2)) AS QTY,  " & _
                " 			 WHSE " & _
                " 	FROM     tblInventory INNER JOIN tblItem_Master " & _
                " 	ON		 tblInventory.ItemCode = tblItem_Master.ItemCode " & _
                " 	LEFT JOIN viewITEM_UOM " & _
                " 	ON		tblItem_Master.ItemCode = viewItem_UOM.GroupCode " & _
                " 	AND		tblItem_Master.ID_UOM = viewItem_UOM.UnitCode " & _
                " 	WHERE   PostDate  <='" & frmIA.DateTo & "' " & _
                "   AND     WHSE IN (" & frmIA.WHSE & ") " & _
                "   AND     tblInventory.ItemCode = @ItemCode " & _
                " ) AS Inv " & _
                " LEFT JOIN tblWarehouse " & _
                " ON inv.WHSE = tblWarehouse.Code " & _
                " LEFT JOIN tblProdWarehouse " & _
                " ON inv.WHSE = tblProdWarehouse.Code " & _
                " GROUP BY ItemCode,  " & _
                " 	   CASE WHEN tblWarehouse.Code IS NULL THEN tblProdWarehouse.Code ELSE tblWarehouse.Code END, " & _
                " 	   CASE WHEN tblWarehouse.Code IS NULL THEN tblProdWarehouse.Description ELSE tblWarehouse.Description END  "
        SQL.FlushParams()
        SQL.AddParam("@ItemCode", ItemCode)
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read
            dgvWHSE.Rows.Add({SQL.SQLDR("Code").ToString, SQL.SQLDR("Description").ToString, SQL.SQLDR("QTY").ToString, 0})
        End While
    End Sub

    Private Sub LoadLedger()
        Try
            Dim query As String
            ' ADD/MODIFY ADDITIONAL FIELDS AFTER TransNo ONLY
            If showCost Then
                query = " SELECT PostDate AS Date, RefID, RefType, TransNo,  MovementType AS Movement, QTY, UOM, UnitCost,  InvQTY, InvUOM, InvValue, ComQTY, ComValue, AverageCost, UnitPrice, TotalPrice, WHSE, DateExpired, DateProduced " & _
                        " FROM viewIA_Ledger " & _
                        " WHERE ItemCode = @ItemCode " & _
                        " AND    PostDate BETWEEN "
            Else
                query = " SELECT '" & frmIA.DateFrom & "' AS Date,  0 AS RefID, 'BB' AS RefType, '' AS TransNo, 'IN' AS Movement,  SUM(InvQTY) AS QTY, InvUOM AS UOM,  SUM(InvQTY) AS InvQTY,InvUOM, SUM(InvQTY) AS ComQTY, '' AS  WHSE, NULL AS  DateExpired, NULL AS  DateProduced " & _
                        " FROM viewIA_Ledger " & _
                        " WHERE ItemCode = @ItemCode AND PostDate < '" & frmIA.DateFrom & "'  " & _
                        " AND    WHSE IN (" & frmIA.WHSE & ") " & _
                        " GROUP BY InvUOM " & _
                    " UNION ALL " & _
                        " SELECT PostDate AS Date,  RefID, RefType, TransNo, MovementType AS Movement, QTY, UOM,  InvQTY,InvUOM, ComQTY, WHSE, DateExpired, DateProduced " & _
                        " FROM viewIA_Ledger " & _
                        " WHERE ItemCode = @ItemCode AND PostDate BETWEEN '" & frmIA.DateFrom & "' AND '" & frmIA.DateTo & "' " & _
                        " AND    WHSE IN (" & frmIA.WHSE & ") "
            End If
            SQL.FlushParams()
            SQL.AddParam("@ItemCode", itemCode)
            SQL.GetQuery(query)
            If SQL.SQLDS.Tables(0).Rows.Count > 0 Then
                dgvLedger.DataSource = SQL.SQLDS.Tables(0)
                dgvLedger.Columns(1).Visible = False
            Else
                dgvLedger.DataSource = Nothing
            End If
            dgvLedger.Refresh()

        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        Finally
            SQL.FlushParams()
        End Try
    End Sub

    Private Sub dgvLedger_DoubleClick(sender As System.Object, e As System.EventArgs) Handles dgvLedger.DoubleClick
        If dgvLedger.SelectedRows.Count = 1 Then
            Dim transID As String = dgvLedger.SelectedRows(0).Cells(1).Value
            Select Case dgvLedger.SelectedRows(0).Cells(2).Value
                Case "RR"
                    frmRR.ShowDialog(transID)
                    frmRR.Dispose()
                Case "DR"
                    frmDR.ShowDialog(transID)
                    frmDR.Dispose()
                Case "GI"
                    frmGI.ShowDialog(transID)
                    frmGI.Dispose()
                Case "GR"
                    frmGR.ShowDialog(transID)
                    frmGR.Dispose()
                Case "IT"
                    frmIT.ShowDialog(transID)
                    frmIT.Dispose()
                Case "BOMC"
                    frmBOMC.ShowDialog(transID)
                    frmBOMC.Dispose()
            End Select
         
        End If
    End Sub

    Private Sub dgvLedger_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvLedger.CellContentClick

    End Sub
End Class