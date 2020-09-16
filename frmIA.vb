Public Class frmIA
    Dim ModuleID As String = "IA"
    Dim type As String = ""
    Dim showCost As Boolean = False
    Public DateFrom, DateTo As Date
    Public WHSE As String = "''"
    Public Item As String = "''"
    Public ItemOwner As String = "''"

    Private Sub tsbSearch_Click(sender As System.Object, e As System.EventArgs) Handles tsbSearch.Click

        frmIA_Filter.ShowDialog()
        LoadCount()
    End Sub

    Private Sub LoadCount()
        Dim query As String
        Dim filter As String = ""
        If Item = Nothing Then Item = "''"
        If WHSE = Nothing Then WHSE = "''"
        If ItemOwner = Nothing Then ItemOwner = "''"
        If ItemOwner = "''" Then
            filter = " WHERE '' = ''"
        Else
            filter = " WHERE ItemOwner ='" & ItemOwner & "' "

        End If
        If showCost = True Then
            query = " SELECT ItemDetail.ItemCode, ItemName, ID_UOM AS UOM,  " & _
                    " 		 CAST([BB] / ISNULL(viewItem_UOM.QTY,1) AS decimal(18,2)) AS [BB] ,  " & _
                    " 		 CAST([IN] / ISNULL(viewItem_UOM.QTY,1) AS decimal(18,2)) AS [IN] ,  " & _
                    " 		 CAST([OUT] / ISNULL(viewItem_UOM.QTY,1) AS decimal(18,2)) AS [OUT] ,  " & _
                    " 		 CAST([EB] / ISNULL(viewItem_UOM.QTY,1) AS decimal(18,2)) AS [EB],  " & _
                    " 		 CAST(AverageCost / ISNULL(viewItem_UOM.QTY,1) AS decimal(18,2)) AS AverageCost, ID_SC, ItemOwner " & _
                    " FROM " & _
                    " ( " & _
                    " 	SELECT	ItemCode, ItemName, Barcode, ItemType, ItemCategory, ItemOwner, ItemGroup, ItemUOM, ItemWeight, ID_UOM, ID_SC  " & _
                    " 	FROM tblItem_Master " & _
                    " ) AS ItemDetail " & _
                    " INNER JOIN " & _
                    " ( " & _
                    " SELECT ItemCode, SUM(ISNULL([BB],0)) AS [BB], SUM(ISNULL([IN],0)) AS [IN], SUM(ISNULL([OUT],0)) AS [OUT], " & _
                    " 		  SUM(ISNULL([BB],0)) + SUM(ISNULL([IN],0)) - SUM(ISNULL([OUT],0)) AS [EB] " & _
                    " 	FROm " & _
                    " 	( " & _
                    " 		SELECT ItemCode, MovementType, SUM(TransQTY) AS QTY  " & _
                    " 		FROM tblInventory " & _
                    " 		WHERE  PostDate BETWEEN '" & DateFrom & "' AND '" & DateTo & "' " & _
                    "       AND    WHSE IN (" & WHSE & ") " & _
                    " 		GROUP BY ItemCode, MovementType " & _
                    " 	 UNION ALL " & _
                    " 		SELECT ItemCode, 'BB',  SUM(CASE WHEN MovementType ='IN' THEN TransQTY ELSE TransQTY * -1 END) AS QTY  " & _
                    " 		FROM tblInventory " & _
                    " 		WHERE  PostDate < '" & DateFrom & "'  " & _
                    "       AND    WHSE IN (" & WHSE & ") " & _
                    " 		GROUP BY ItemCode " & _
                    " 	) AS A " & _
                    " 	PIVOT  " & _
                    " 	(  " & _
                    " 		SUM(QTY) " & _
                    " 		FOR [MovementType] IN ([BB],[IN],[OUT]) " & _
                    " 	) AS pvt " & _
                    " 	GROUP BY ItemCode " & _
                    " ) AS ItemCount " & _
                    " ON	ItemDetail.Itemcode = ItemCount.ItemCode " & _
                    " LEFT JOIN  " & _
                    " ( " & _
                    "  SELECT    ItemCode, AverageCost, ROW_NUMBER() OVER (PARTITION BY ItemCode ORDER BY Postdate DESC, DateCreated DESC) AS RowID " & _
                    "  FROM      tblInventory  " & _
                    "  ) AS ItemCost " & _
                    " ON	ItemDetail.Itemcode = ItemCost.ItemCode " & _
                    " AND RowID = 1 " & _
                    " LEFT JOIN viewITEM_UOM " & _
                    " ON ItemDetail.ItemCode = viewItem_UOM.GroupCode " & _
                    " AND ItemDetail.ID_UOM = viewItem_UOM.UnitCode "
        Else
            query = " SELECT ItemDetail.ItemCode, ItemName, ID_UOM AS UOM,  " & _
                    " 		 CAST([BB] / ISNULL(viewItem_UOM.QTY,1) AS decimal(18,2)) AS [BB] ,  " & _
                    " 		 CAST([IN] / ISNULL(viewItem_UOM.QTY,1) AS decimal(18,2)) AS [IN] ,  " & _
                    " 		 CAST([OUT] / ISNULL(viewItem_UOM.QTY,1) AS decimal(18,2)) AS [OUT] ,  " & _
                    " 		 CAST([EB] / ISNULL(viewItem_UOM.QTY,1) AS decimal(18,2)) AS [EB],  " & _
                    " 		 CAST(AverageCost / ISNULL(viewItem_UOM.QTY,1) AS decimal(18,2)) AS AverageCost, ID_SC " & _
                    " FROM " & _
                    " ( " & _
                    " 	SELECT	ItemCode, ItemName, Barcode, ItemType, ItemCategory, ItemOwner, ItemGroup, ItemUOM, ItemWeight, ID_UOM, ID_SC  " & _
                    " 	FROM tblItem_Master " & _
                    " ) AS ItemDetail " & _
                    " INNER JOIN " & _
                    " ( " & _
                    " SELECT ItemCode, SUM(ISNULL([BB],0)) AS [BB], SUM(ISNULL([IN],0)) AS [IN], SUM(ISNULL([OUT],0)) AS [OUT], " & _
                    " 		  SUM(ISNULL([BB],0)) + SUM(ISNULL([IN],0)) - SUM(ISNULL([OUT],0)) AS [EB] " & _
                    " 	FROm " & _
                    " 	( " & _
                    " 		SELECT ItemCode, MovementType, SUM(TransQTY) AS QTY  " & _
                    " 		FROM tblInventory " & _
                    " 		WHERE  PostDate BETWEEN '" & DateFrom & "' AND '" & DateTo & "' " & _
                    "       AND    WHSE IN (" & WHSE & ") " & _
                    " 		GROUP BY ItemCode, MovementType " & _
                    " 	 UNION ALL " & _
                    " 		SELECT ItemCode, 'BB',  SUM(CASE WHEN MovementType ='IN' THEN TransQTY ELSE TransQTY * -1 END) AS QTY  " & _
                    " 		FROM tblInventory " & _
                    " 		WHERE  PostDate < '" & DateFrom & "'  " & _
                    "       AND    WHSE IN (" & WHSE & ") " & _
                    " 		GROUP BY ItemCode " & _
                    " 	) AS A " & _
                    " 	PIVOT  " & _
                    " 	(  " & _
                    " 		SUM(QTY) " & _
                    " 		FOR [MovementType] IN ([BB],[IN],[OUT]) " & _
                    " 	) AS pvt " & _
                    " 	GROUP BY ItemCode " & _
                    " ) AS ItemCount " & _
                    " ON	ItemDetail.Itemcode = ItemCount.ItemCode " & _
                    " LEFT JOIN  " & _
                    " ( " & _
                    "  SELECT    ItemCode, AverageCost, ROW_NUMBER() OVER (PARTITION BY ItemCode ORDER BY Postdate DESC, DateCreated DESC) AS RowID " & _
                    "  FROM      tblInventory  " & _
                    "  ) AS ItemCost " & _
                    " ON	ItemDetail.Itemcode = ItemCost.ItemCode " & _
                    " AND RowID = 1 " & _
                    " LEFT JOIN viewITEM_UOM " & _
                    " ON ItemDetail.ItemCode = viewItem_UOM.GroupCode " & _
                    " AND ItemDetail.ID_UOM = viewItem_UOM.UnitCode " & filter

        End If
        SQL.GetQuery(query)

        If SQL.SQLDS.Tables(0).Rows.Count > 0 Then
            dgvList.DataSource = SQL.SQLDS.Tables(0)
        Else
            dgvList.DataSource = Nothing
        End If
        dgvList.Refresh()
    End Sub


    Private Sub dgvList_CellDoubleClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvList.CellDoubleClick
        If dgvList.SelectedRows.Count = 1 Then
            Dim f As New frmIA_Ledger
            f.ShowDialog(dgvList.SelectedRows(0).Cells(0).Value, showCost)
            f.Dispose()
        End If
    End Sub

    Private Sub frmIA_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        DateFrom = Date.Today.Date
        frmIA_Filter.ShowDialog()

        LoadCount()
    End Sub

    Private Sub tsbRefresh_Click(sender As System.Object, e As System.EventArgs) Handles tsbRefresh.Click
        LoadCount()
    End Sub

    Private Sub tsbExit_Click(sender As System.Object, e As System.EventArgs) Handles tsbExit.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub tsbPrint_Click(sender As System.Object, e As System.EventArgs) Handles tsbPrint.Click
        Dim ItemOwner As String
        ItemOwner = frmIA_Filter.cbItemOwner.SelectedItem
        If frmIA_Filter.cbItemOwner.SelectedItem = "ALL" Then
            Dim f As New frmReport_Display
            f.ShowDialog("IA_Count", DateFrom, DateTo)
            f.Dispose()
        ElseIf frmIA_Filter.cbItemOwner.SelectedItem = ItemOwner Then
            Dim f As New frmReport_Display
            f.ShowDialog("IA_CountR", DateFrom, DateTo, ItemOwner)
            f.Dispose()

        End If
    End Sub
End Class