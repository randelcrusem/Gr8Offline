Public Class frmLoadTransactions
    Dim moduleID As String
    Public transID As String = ""
    Public BranchCode As String = ""
    Public itemCode As String = ""
    Public batch As Boolean = False

    Public Overloads Function ShowDialog(ByVal ModID As String) As Boolean
        moduleID = ModID
        MyBase.ShowDialog()
        Return True
    End Function

    Private Sub frmLoadTransactions_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        LoadData()
        LoadBranches()
        If moduleID = "Copy Member" Then
            cbFilter.Items.Clear()
            cbFilter.Items.Add("Member ID")
            cbFilter.Items.Add("Full Name")
            cbFilter.Items.Add("Status")
            Label2.Visible = True
            cbBranch.Visible = True
        End If
    End Sub

    Private Sub LoadBranches()
        Dim query As String
        query = " SELECT    DISTINCT  tblBranch.BranchCode + ' - ' + Description AS BranchCode  " & _
                " FROM      tblBranch    " 
        SQL.ReadQuery(query)
        cbBranch.Items.Clear()
        While SQL.SQLDR.Read
            cbBranch.Items.Add(SQL.SQLDR("BranchCode").ToString)
        End While
        cbBranch.SelectedIndex = 0
    End Sub

    Private Sub LoadData()
        Try
            Dim filter As String = ""
            Dim query As String = ""

            Select Case moduleID
                Case "JO_GI"

                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE JO_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE Description LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblJO.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT        tblJO.TransID, JO_No AS [JO No.], DateJO AS [JO Date],  VCEName AS Supplier,ItemName, tblJO.QTY, DateNeeded AS [Date Needed], tblJO.Remarks, tblJO.Status  " & _
                            " FROM          tblBOM INNER JOIN tblJO " & _
                            " ON            tblBOM.JO_Ref = tblJO.TransID " & _
                            " LEFT JOIN     tblVCE_Master " & _
                            " ON            tblJO.VCECode = tblVCE_Master.VCECode " & _
                            " LEFT JOIN tblItem_Master ON" & _
                            " tblJO.ItemCode = tblItem_Master.ItemCode " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "DR-SI"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " AND '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " AND DR_No LIKE '%' + @Filter + '%' AND ForECS = 0"
                            Case "VCE Name"
                                filter = " AND VCEName LIKE '%' + @Filter + '%' AND ForECS = 0"
                            Case "Remarks"
                                filter = " AND Remarks LIKE '%' + @Filter + '%' AND ForECS = 0"
                            Case "Status"
                                filter = " AND tblDR.Status LIKE '%' + @Filter + '%' AND ForECS = 0"
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, DR_No AS [DR No.], DateDR AS [Date], VCEName AS [Customer], Remarks, SO_Ref AS [Reference SO], tblDR.Status  " & _
                            " FROM     tblDR LEFT JOIN tblVCE_Master " & _
                            " ON	   tblDR .VCECode = tblVCE_Master.VCECode " & _
                            " WHERE    TransID NOT IN (SELECT DR_Ref FROM tblSI ) " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "DR-ECS"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = "WHERE tblDR.ForECS = 1 AND DR_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = "WHERE tblDR.ForECS = 1 AND VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = "WHERE tblDR.ForECS = 1 AND Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = "WHERE tblDR.ForECS = 1 AND viewDR_Status.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   tblDR.TransID, tblDR.DR_No AS [DR No.], DateDR AS [Date], VCEName AS [Customer], Remarks, SO_Ref AS [Reference SO], viewDR_Status.Status  " & _
                            " FROM     tblDR LEFT JOIN tblVCE_Master " & _
                            " ON	   tblDR.VCECode = tblVCE_Master.VCECode " & _
                            " LEFT JOIN	 viewDR_Status " & _
                            " ON		 tblDR.TransID = viewDR_Status.TransID " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "DR-LN"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = "WHERE  DR_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = "WHERE  VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = "WHERE   Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = "WHERE viewDR_LN_Status.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = "  SELECT   tblDR.TransID, tblDR.DR_No AS [DR No.], DateDR AS [Date], VCEName AS [Customer], Remarks, SO_Ref AS [Reference SO], Unserved, viewDR_LN_Status.Status   " & _
                            "  FROM     tblDR LEFT JOIN tblVCE_Master  " & _
                            "  ON	   tblDR.VCECode = tblVCE_Master.VCECode  " & _
                            "  LEFT JOIN	 viewDR_LN_Status  " & _
                            "  ON		 tblDR.TransID = viewDR_LN_Status.TransID " & _
                            "  LEFT JOIN viewDR_LN_UNserved " & _
                            "  ON viewDR_LN_UNserved.TransID = tblDR.TransID " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)


                Case "SO-PL"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " AND '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " AND tblSO.SO_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " AND VCEName '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " AND Remarks '%' + @Filter + '%' "
                            Case "Status"
                                filter = " AND tblSO.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT     DISTINCT  CAST(tblSO.TransID AS nvarchar)  AS RecordID, CAST(tblSO.SO_No AS nvarchar)  AS [SO No.],  " & _
                            "            DateSO AS [Date], VCEName AS [Customer], Remarks, ReferenceNo, tblSO.Status  " & _
                            " FROM       tblSO LEFT JOIN tblVCE_Master " & _
                            " ON	     tblSO.VCECode = tblVCE_Master.VCECode " & _
                            " INNER JOIN viewSO_Unallocated " & _
                            " ON         tblSO.TransID = viewSO_Unallocated.TransID " & _
                            " WHERE      tblSO.Status <> 'Modified' " & filter & _
                            " ORDER BY   [SO No.]"
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "PL-DR"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " AND '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " AND PL_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " AND Description '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " AND Remarks '%' + @Filter + '%' "
                            Case "Status"
                                filter = " AND tblPL.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT     DISTINCT tblPL.TransID, tblPL.PL_No AS [PL No.], DatePL AS [PL Date], VCEName AS Supplier, Remarks, tblPL.Status  " & _
                            " FROM       tblPL INNER JOIN tblPL_Details " & _
                            " ON	     tblPL.TransID = tblPL_Details.TransID " & _
                            " INNER JOIN viewPL_Unserved " & _
                            " ON        tblPL_Details.ItemCode = viewPL_Unserved.ItemCode " & _
                            " AND        tblPL_Details.LineNum = viewPL_Unserved.LineNum " & _
                            " AND        tblPL_Details.WHSE = viewPL_Unserved.WHSE " & _
                            " LEFT JOIN  tblVCE_Master " & _
                            " ON	     tblPL.VCECode = tblVCE_Master.VCECode " & _
                            " WHERE      tblPL.Status <> 'Modified' " & _
                            " AND        viewPL_Unserved.Unserved > 0 " & _
                            " AND        tblPL_Details.WHSE IN (SELECT    DISTINCT tblWarehouse.Code  " & _
                            "                             FROM      tblWarehouse INNER JOIN tblUser_Access " & _
                            "                             ON        tblWarehouse.Code = tblUser_Access.Code " & _
                            "                             AND       tblUser_Access.Status ='Active' AND tblWarehouse.Status ='Active' " & _
                            "                             AND       tblUser_Access.Type = 'Warehouse' AND isAllowed = 1 " & _
                            "                             WHERE     UserID ='" & UserID & "'" & _
                            " 			UNION ALL " & _
                            " 				SELECT    DISTINCT tblProdWarehouse.Code   " & _
                            "                 FROM      tblProdWarehouse INNER JOIN tblUser_Access  " & _
                            "                 ON        tblProdWarehouse.Code = tblUser_Access.Code  " & _
                            "                 AND       tblUser_Access.Status ='Active' AND tblProdWarehouse.Status ='Active'  " & _
                            "                 AND       tblUser_Access.Type = 'Production' AND isAllowed = 1  " & _
                            "                             WHERE     UserID ='" & UserID & "')" & filter & _
                            " ORDER BY [PL No.]"
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)



                Case "BOMC"

                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " AND '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " AND BOMC_No LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " AND Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " AND tblBOMC.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = "  SELECT     tblBOMC.TransID,BOMC_No AS [BOMC No.],  tblBOMC.WHSE,  " & _
                            "  tblProdWarehouse.Description,  DateBOMC AS [BOMC Date],  " & _
                            "   Remarks, tblBOMC.Status    " & _
                            "  FROM       tblBOMC  " & _
                            "  LEFT JOIN  tblProdWarehouse  ON	      " & _
                            "  tblBOMC.WHSE = tblProdWarehouse.Code   " & _
                            "  WHERE       " & _
                            "  tblBOMC.WHSE IN  " & _
                            "  (SELECT    DISTINCT tblProdWarehouse.Code                                " & _
                            "  FROM      tblProdWarehouse  " & _
                            "  INNER JOIN tblUser_Access  ON         " & _
                            "  tblProdWarehouse.Code = tblUser_Access.Code   AND        " & _
                            "  tblUser_Access.Status ='Active' AND tblProdWarehouse.Status ='Active'   " & _
                            "   AND       tblUser_Access.Type = 'Production' AND isAllowed = 1    WHERE UserID ='" & UserID & "') " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "GI-GR-PWHSE"

                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " AND '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " AND GI_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " AND Description LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " AND Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " AND tblGI.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT        TransID, GI_No AS [GI No.], DateGI AS [GI Date],  Description AS [Issued From], Remarks, tblGI.Status  " & _
                            " FROM          tblGI LEFT JOIN tblWarehouse " & _
                            " ON	        tblGI.WHSE_From = tblWarehouse.Code " & _
                            " WHERE         tblGI.withConfirm = 1 " & _
                            " AND           WHSE_To IN (SELECT    DISTINCT tblProdWarehouse.Code  " & _
                            "                             FROM      tblProdWarehouse INNER JOIN tblUser_Access " & _
                            "                             ON        tblProdWarehouse.Code = tblUser_Access.Code " & _
                            "                             AND       tblUser_Access.Status ='Active' AND tblProdWarehouse.Status ='Active' " & _
                            "                             AND       tblUser_Access.Type = 'Production' AND isAllowed = 1 " & _
                            "                             WHERE     UserID ='" & UserID & "') " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)


                Case "GI-GR-WHSE"

                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " AND '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " AND GI_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " AND Description LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " AND Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " AND tblGI.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT        TransID, GI_No AS [GI No.], DateGI AS [GI Date],  Description AS [Issued From], Remarks, tblGI.Status  " & _
                            " FROM          tblGI LEFT JOIN tblProdWarehouse " & _
                            " ON	        tblGI.WHSE_From = tblProdWarehouse.Code " & _
                            " WHERE         tblGI.withConfirm = 1 " & _
                            " AND           WHSE_To IN (SELECT    DISTINCT tblWarehouse.Code  " & _
                            "                             FROM      tblWarehouse INNER JOIN tblUser_Access " & _
                            "                             ON        tblWarehouse.Code = tblUser_Access.Code " & _
                            "                             AND       tblUser_Access.Status ='Active' AND tblWarehouse.Status ='Active' " & _
                            "                             AND       tblUser_Access.Type = 'Warehouse' AND isAllowed = 1 " & _
                            "                             WHERE     UserID ='" & UserID & "') " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "MR-GI"

                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " AND '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " AND MR_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " AND Description LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " AND Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " AND tblMR.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT     DISTINCT tblMR_Details.TransID, MR_No AS [MR No.], DateMR AS [MR Date],  tblProdWarehouse.Description AS [Request From], Remarks, tblMR.Status   " & _
                            "  FROM       tblMR INNER JOIN tblMR_Details  " & _
                            "  ON			tblMR.TransID = tblMR_Details.TransID " & _
                            "  LEFT JOIN	tblProdWarehouse  " & _
                            "  ON	        tblMR.WHSE_From = tblProdWarehouse.Code  " & _
                            "  WHERE      tblMR_Details.WHSE  " & _
                            "  IN			(	SELECT    DISTINCT tblWarehouse.Code   " & _
                            "                 FROM      tblWarehouse INNER JOIN tblUser_Access  " & _
                            "                 ON        tblWarehouse.Code = tblUser_Access.Code  " & _
                            "                 AND       tblUser_Access.Status ='Active' AND tblWarehouse.Status ='Active'  " & _
                            "                 AND       tblUser_Access.Type = 'Warehouse' AND isAllowed = 1  " & _
                            "                 WHERE     UserID ='" & UserID & "' " & _
                            " 			UNION ALL " & _
                            " 				SELECT    DISTINCT tblProdWarehouse.Code   " & _
                            "                 FROM      tblProdWarehouse INNER JOIN tblUser_Access  " & _
                            "                 ON        tblProdWarehouse.Code = tblUser_Access.Code  " & _
                            "                 AND       tblUser_Access.Status ='Active' AND tblProdWarehouse.Status ='Active'  " & _
                            "                 AND       tblUser_Access.Type = 'Production' AND isAllowed = 1  " & _
                            "                             WHERE     UserID ='" & UserID & "')  AND tblMR.TransID in (SELECT DISTINCT TransID FROM viewMR_Unserved)" & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "PCV-CV"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " AND '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " AND PCV_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " AND VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " AND tblPCV_Entry.Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " AND CASE WHEN View_PCV_Balance.Ref_TransID IS NOT NULL THEN  'Active'  WHEN tblPCV_Entry.Status ='Active' THEN 'Closed' ELSE tblPCV_Entry.Status   END LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID,  tblBranch.Description as Branch, tblPCV_Entry.PCV_No AS [RF No.], DatePCV AS [Date], VCEName AS [Supplier], tblPCV_Entry.Remarks,  " & _
                            "           CASE WHEN View_PCV_Balance.Ref_TransID IS NOT NULL THEN  'Active' " & _
                            "                WHEN tblPCV_Entry.Status ='Active' THEN 'Closed' ELSE tblPCV_Entry.Status   END AS Status" & _
                            " FROM     tblPCV_Entry LEFT JOIN viewVCE_Master " & _
                            " ON	   tblPCV_Entry .VCECode = viewVCE_Master.VCECode " & _
                            " LEFT JOIN View_PCV_Balance " & _
                            " ON        tblPCV_Entry.TransID = View_PCV_Balance.Ref_TransID " & _
                             " LEFT JOIN tblBranch  ON	           " & _
                            " tblPCV_Entry.BranchCode = tblBranch.BranchCode      " & _
                            " WHERE          " & _
                            " 	( tblPCV_Entry.BranchCode IN  " & _
                            " 	  (SELECT    DISTINCT tblBranch.BranchCode  " & _
                            " 	  FROM      tblBranch   " & _
                            " 	  INNER JOIN tblUser_Access    ON          " & _
                            " 	  tblBranch.BranchCode = tblUser_Access.Code   " & _
                            " 	   AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                            " 	   AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                            " 	   WHERE     UserID ='" & UserID & "'" & _
                            " 	   ) " & _
                            "     )   " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "PL" ' PICK LIST

                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " AND '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE PL_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE Description LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblPL.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT        tblPL.TransID, PL_No AS [PL No.], DatePL AS [PL Date],  VCEName AS Supplier, DateDeliver AS [Delivery Date], tblPL.Remarks, tblPL.Status  " & _
                            " FROM          tblPL LEFT JOIN tblVCE_Master " & _
                            " ON            tblPL.VCECode = tblVCE_Master.VCECode " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "PL-DR"

                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " AND '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " AND PL_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " AND Description LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " AND Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " AND tblPL.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT DISTINCT    tblPL.TransID, tblPL_Details.WHSE, tblWarehouse.Description, PL_No AS [PL No.], DatePL AS [PL Date], VCEName AS Supplier, Remarks, tblPL.Status  " & _
                            " FROM       tblPL INNER JOIN tblPL_Details " & _
                            " ON	     tblPL.TransID = tblPL_Details.TransID " & _
                            " LEFT JOIN  tblVCE_Master " & _
                            " ON	     tblPL.VCECode = tblVCE_Master.VCECode " & _
                            " LEFT JOIN  tblWarehouse " & _
                            " ON	     tblPL_Details.WHSE = tblWarehouse.Code " & _
                            " WHERE      tblPL_Details.WHSE IN (SELECT    DISTINCT tblWarehouse.Code  " & _
                            "                             FROM      tblWarehouse INNER JOIN tblUser_Access " & _
                            "                             ON        tblWarehouse.Code = tblUser_Access.Code " & _
                            "                             AND       tblUser_Access.Status ='Active' AND tblWarehouse.Status ='Active' " & _
                            "                             AND       tblUser_Access.Type = 'Warehouse' AND isAllowed = 1 " & _
                            "                             WHERE     UserID ='" & UserID & "') " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "ITI-ITR"

                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " AND '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " AND ITI_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " AND Description LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " AND Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " AND tblITI.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT        TransID, ITI_No AS [ITI No.], DateITI AS [ITI Date],  Description AS [Issued From], Remarks, tblITI.Status  " & _
                            " FROM          tblITI LEFT JOIN tblWarehouse " & _
                            " ON	        tblITI.WHSE_From = tblWarehouse.Code " & _
                            " WHERE         WHSE_To IN (SELECT    DISTINCT tblProdWarehouse.Code  " & _
                            "                             FROM      tblProdWarehouse INNER JOIN tblUser_Access " & _
                            "                             ON        tblProdWarehouse.Code = tblUser_Access.Code " & _
                            "                             AND       tblUser_Access.Status ='Active' AND tblProdWarehouse.Status ='Active' " & _
                            "                             AND       tblUser_Access.Type = 'Production' AND isAllowed = 1 " & _
                            "                             WHERE     UserID ='" & UserID & "') " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)
                Case "MR-ITI"

                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " AND '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " AND MR_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " AND Description LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " AND Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " AND tblMR.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT        TransID, MR_No AS [MR No.], DateMR AS [MR Date],  Description AS [Request From], Remarks, tblMR.Status  " & _
                            " FROM          tblMR LEFT JOIN tblProdWarehouse " & _
                            " ON	        tblMR.WHSE_From = tblProdWarehouse.Code " & _
                            " WHERE        ( WHSE_To IN (SELECT    DISTINCT tblWarehouse.Code  " & _
                            "                             FROM      tblWarehouse INNER JOIN tblUser_Access " & _
                            "                             ON        tblWarehouse.Code = tblUser_Access.Code " & _
                            "                             AND       tblUser_Access.Status ='Active' AND tblWarehouse.Status ='Active' " & _
                            "                             AND       tblUser_Access.Type = 'Warehouse' AND isAllowed = 1 " & _
                            "                             WHERE     UserID ='" & UserID & "') " & _
                            "               OR WHSE_To IN (SELECT    DISTINCT tblProdWarehouse.Code  " & _
                            "                             FROM      tblProdWarehouse INNER JOIN tblUser_Access " & _
                            "                             ON        tblProdWarehouse.Code = tblUser_Access.Code " & _
                            "                             AND       tblUser_Access.Status ='Active' AND tblProdWarehouse.Status ='Active' " & _
                            "                             AND       tblUser_Access.Type = 'Production' AND isAllowed = 1 " & _
                            "                             WHERE     UserID ='" & UserID & "'))" & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "BI"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE BI_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblBI.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, BI_No AS [BI No.], DateBI AS [Date], VCEName AS [Customer], Remarks, SO_Ref AS [Reference SO], tblBI.Status  " & _
                            " FROM     tblBI LEFT JOIN tblVCE_Master " & _
                            " ON	   tblBI .VCECode = tblVCE_Master.VCECode " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "BS"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE BS_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblBStatement.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, BS_No AS [BS No.], DateBS AS [Date], VCEName AS [Customer], Remarks, SO_Ref AS [Reference SO], tblBStatement.Status  " & _
                            " FROM     tblBStatement LEFT JOIN tblVCE_Master " & _
                            " ON	   tblBStatement .VCECode = tblVCE_Master.VCECode " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "MR"

                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " AND '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " AND MR_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " AND Description LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " AND Remarks  LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " AND tblMR.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT        TransID, MR_No AS [MR No.], DateMR AS [MR Date],  Description AS Warehouse, Remarks, tblMR.Status  " & _
                            " FROM          tblMR LEFT JOIN tblWarehouse " & _
                            " ON	        tblMR.WHSE_To = tblWarehouse.Code " & _
                            " WHERE         WHSE_From IN (SELECT    DISTINCT tblProdWarehouse.Code  " & _
                            "                             FROM      tblProdWarehouse INNER JOIN tblUser_Access " & _
                            "                             ON        tblProdWarehouse.Code = tblUser_Access.Code " & _
                            "                             AND       tblUser_Access.Status ='Active' AND tblProdWarehouse.Status ='Active' " & _
                            "                             AND       tblUser_Access.Type = 'Production' AND isAllowed = 1 " & _
                            "                             WHERE     UserID ='" & UserID & "') " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)


                    ' ** LOANS **

                Case "LN"
                    chkBatch.Visible = False
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " AND '' = '' ORDER BY Loan_No"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " AND Loan_No LIKE '%' + @Filter + '%' ORDER BY Loan_No "
                            Case "VCE Name"
                                filter = " AND VCEName LIKE'%' + @Filter + '%' ORDER BY Loan_No "
                            Case "Remarks"
                                filter = " AND Remarks LIKE'%' + @Filter + '%' ORDER BY Loan_No  "
                            Case "Status"
                                filter = " AND tblLoan.Status LIKE '%' + @Filter + '%' ORDER BY Loan_No"
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT        TransID,  Loan_No AS [Loan No.], DateLoan AS [Date of Loan],  VCEName, LoanType, LoanAmount, tblLoan.Terms,  tblLoan.Status  " & _
                            " FROM          tblLoan LEFT JOIN viewVCE_Master " & _
                            " ON	        tblLoan.VCECode = viewVCE_Master.VCECode " & _
                            " INNER JOIN    tblLoan_Type " & _
                            " ON            tblLoan.LoanCode = tblLoan_Type.LoanCode " & _
                             " LEFT JOIN tblBranch  ON	           " & _
                            " tblLoan.BranchCode = tblBranch.BranchCode      " & _
                            " WHERE          " & _
                            " 	( tblLoan.BranchCode IN  " & _
                            " 	  (SELECT    DISTINCT tblBranch.BranchCode  " & _
                            " 	  FROM      tblBranch   " & _
                            " 	  INNER JOIN tblUser_Access    ON          " & _
                            " 	  tblBranch.BranchCode = tblUser_Access.Code   " & _
                            " 	   AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                            " 	   AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                            " 	   WHERE     UserID ='" & UserID & "'" & _
                            " 	   ) " & _
                            "     )   " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "Copy Loan"
                    chkBatch.Visible = True
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE  ForJV = 0 AND  Loan_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE  ForJV = 0 AND VCEName '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE  ForJV = 0 AND Remarks '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE  ForJV = 0 AND tblLoan.Status LIKE '%' + @Filter + '%'  "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT        TransID,  Loan_No AS [Loan No.], DateLoan AS [Date of Loan],  VCEName, LoanType, LoanAmount, tblLoan.Terms,  tblLoan.Status  " & _
                            " FROM          tblLoan LEFT JOIN viewVCE_Master " & _
                            " ON	        tblLoan.VCECode = viewVCE_Master.VCECode " & _
                            " INNER JOIN    tblLoan_Type " & _
                            " ON            tblLoan.LoanCode = tblLoan_Type.LoanCode " & filter
                    '" AND tblLoan.Loan_No NOT IN (SELECT DISTINCT REPLACE(RefNo, 'LN:', '') AS RefNo FROM View_GL WHERE Debit > 0 AND RefType IN ('CV', 'JV')) "
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)


                Case "Copy Loan DR"
                    chkBatch.Visible = True
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE  ForJV = 0 AND  Loan_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE  ForJV = 0 AND VCEName '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE  ForJV = 0 AND Remarks '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE  ForJV = 0 AND tblLoan.Status LIKE '%' + @Filter + '%'  "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT        TransID,  Loan_No AS [Loan No.], DateLoan AS [Date of Loan],  VCEName, LoanType, LoanAmount, tblLoan.Terms,  tblLoan.Status  " & _
                            " FROM          tblLoan LEFT JOIN viewVCE_Master " & _
                            " ON	        tblLoan.VCECode = viewVCE_Master.VCECode " & _
                            " INNER JOIN    tblLoan_Type " & _
                            " ON            tblLoan.LoanCode = tblLoan_Type.LoanCode " & filter
                    '" AND tblLoan.Loan_No NOT IN (SELECT DISTINCT REPLACE(RefNo, 'LN:', '') AS RefNo FROM View_GL WHERE Debit > 0 AND RefType IN ('CV', 'JV')) "
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)
                    'chkBatch.Visible = True
                    '' CONDITION OF QUERY
                    'If cbFilter.SelectedIndex = -1 Then
                    '    filter = " WHERE '' = ''"
                    'Else
                    '    Select Case cbFilter.SelectedItem
                    '        Case "Transaction ID"
                    '            filter = " WHERE (DR_Ref <> 0 OR ForJV = 1) AND Loan_No LIKE '%' + @Filter + '%' "
                    '        Case "VCE Name"
                    '            filter = " WHERE (DR_Ref <> 0 OR ForJV = 1)  AND VCEName '%' + @Filter + '%' "
                    '        Case "Remarks"
                    '            filter = " WHERE (DR_Ref <> 0 OR ForJV = 1)  AND Remarks '%' + @Filter + '%' "
                    '        Case "Status"
                    '            filter = " WHERE (DR_Ref <> 0 OR ForJV = 1)  AND tblLoan.Status IN ('Approved', 'Released') "
                    '    End Select
                    'End If

                    '' QUERY 
                    'query = " SELECT        TransID,  Loan_No AS [Loan No.], DateLoan AS [Date of Loan],  VCEName, LoanType, LoanAmount, tblLoan.Terms,  tblLoan.Status, CAST(0 as bit) AS [Check]  " & vbCrLf & _
                    '        " FROM          tblLoan LEFT JOIN viewVCE_Master " & vbCrLf & _
                    '        " ON	        tblLoan.VCECode = viewVCE_Master.VCECode " & vbCrLf & _
                    '        " INNER JOIN    tblLoan_Type " & vbCrLf & _
                    '        " ON            tblLoan.LoanCode = tblLoan_Type.LoanCode " & filter & vbCrLf & _
                    '        " AND tblLoan.Loan_No NOT IN (SELECT DISTINCT REPLACE(RefNo, 'LN:', '') AS RefNo FROM View_GL WHERE Debit > 0 AND RefType IN ('CV', 'PCV')) "
                    'SQL.FlushParams()
                    'SQL.AddParam("@Filter", txtFilter.Text)


                Case "Copy Loan PCV"
                    chkBatch.Visible = True
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE (DR_Ref <> 0 OR ForJV = 1) AND Loan_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE (DR_Ref <> 0 OR ForJV = 1)  AND VCEName '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE (DR_Ref <> 0 OR ForJV = 1)  AND Remarks '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE (DR_Ref <> 0 OR ForJV = 1)  AND tblLoan.Status IN ('Approved', 'Released') "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT        TransID,  Loan_No AS [Loan No.], DateLoan AS [Date of Loan],  VCEName, LoanType, LoanAmount, tblLoan.Terms,  tblLoan.Status, CAST(0 as bit) AS [Check]  " & vbCrLf & _
                            " FROM          tblLoan LEFT JOIN viewVCE_Master " & vbCrLf & _
                            " ON	        tblLoan.VCECode = viewVCE_Master.VCECode " & vbCrLf & _
                            " INNER JOIN    tblLoan_Type " & vbCrLf & _
                            " ON            tblLoan.LoanCode = tblLoan_Type.LoanCode " & filter & vbCrLf & _
                            " AND tblLoan.Loan_No NOT IN (SELECT DISTINCT REPLACE(RefNo, 'LN:', '') AS RefNo FROM View_GL WHERE Debit > 0 AND RefType IN ('CV', 'PCV')) "
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                    ' ** LOANS END ** 
                    dgvList.EditMode = DataGridViewEditMode.EditOnKeystroke

                Case "BOM-SFG"

                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = '' ORDER BY tblBOM_SFG.BOM_Code"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE BOM_Code LIKE '%' + @Filter + '%' ORDER BY tblBOM_SFG.BOM_Code "
                            Case "VCE Name"
                                filter = " WHERE ItemName LIKE '%' + @Filter + '%' ORDER BY tblBOM_SFG.BOM_Code "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' ORDER BY tblBOM_SFG.BOM_Code "
                            Case "Status"
                                filter = " WHERE tblBOM_SFG.Status LIKE '%' + @Filter + '%' ORDER BY tblBOM_SFG.BOM_Code "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT BOM_Code, BOM_Code AS [SFG Code], tblBOM_SFG.ItemCode, ItemName, tblBOM_SFG.UOM, tblBOM_SFG.QTY " & _
                            " FROM   tblBOM_SFG LEFT JOIN tblItem_Master " & _
                            " ON     tblBOM_SFG.ItemCode = tblItem_Master.ItemCode " & _
                            " AND    tblItem_Master.Status ='Active' " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)
                Case "BOM-FG"

                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = '' ORDER BY tblBOM_FG.BOM_Code"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE BOM_Code LIKE '%' + @Filter + '%' ORDER BY tblBOM_FG.BOM_Code"
                            Case "VCE Name"
                                filter = " WHERE ItemName LIKE '%' + @Filter + '%' ORDER BY tblBOM_FG.BOM_Code "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' ORDER BY tblBOM_FG.BOM_Code"
                            Case "Status"
                                filter = " WHERE tblBOM_FG.Status LIKE '%' + @Filter + '%' ORDER BY tblBOM_FG.BOM_Code "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT BOM_Code, BOM_Code AS [BOM Code], tblBOM_FG.ItemCode, ItemName, tblBOM_FG.UOM, tblBOM_FG.QTY " & _
                            " FROM   tblBOM_FG INNER JOIN tblItem_Master " & _
                            " ON     tblBOM_FG.ItemCode = tblItem_Master.ItemCode " & _
                            " AND    tblItem_Master.Status ='Active' " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)
                Case "JO_BOM"

                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE JO_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblJO.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, JO_No AS [JO No.], DateJO AS [Date], VCEName, ItemCode, Description, QTY, Remarks  " & _
                            " FROM     tblJO LEFT JOIN tblVCE_Master " & _
                            " ON	   tblJO.VCECode = tblVCE_Master.VCECode " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "BR"

                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE BR_No LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblBR.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, BR_No AS [BR No.], DateBR AS [Date], Bank, Branch, AccountNo, Remarks, tblBR.Status  " & _
                            " FROM     tblBR INNER JOIN tblBank_Master " & _
                            " ON       tblBR.BankID = tblBank_Master.BankID " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)
                Case "JO"

                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE JO_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblJO.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, JO_No AS [JO No.], DateJO AS [Date], VCEName, ItemCode, Description, QTY, Remarks,  tblJO.Status  " & _
                            " FROM     tblJO LEFT JOIN tblVCE_Master " & _
                            " ON	   tblJO.VCECode = tblVCE_Master.VCECode " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)
                Case "JO-BOM"
                    ' QUERY 
                    query = " SELECT   TransID, JO_No AS [JO No.], DateJO AS [Date], VCEName, ItemCode, Description, QTY, Remarks,  tblJO.Status  " & _
                            " FROM     tblJO LEFT JOIN tblVCE_Master " & _
                            " ON	   tblJO.VCECode = tblVCE_Master.VCECode " & _
                            " WHERE	   TransID NOT IN (SELECT JO_Ref FROM tblBOM) " & _
                            " AND	   tblJO.Status = 'Active' and tblJO.ForBOM = 1 "

                Case "BOM_PR"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE BOM_No LIKE '%' + @Filter + '%' AND ForPR = 1 "
                            Case "VCE Name"
                                filter = " WHERE VCEName LIKE '%' + @Filter + '%' AND ForPR = 1 "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' AND ForPR = 1 "
                            Case "Status"
                                filter = " WHERE tblBOM.Status LIKE '%' + @Filter + '%' AND ForPR = 1 "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, BOM_No AS [BOM No.], DateBOM AS [Date], VCEName, ItemCode, Description, QTY, Remarks,  tblBOM.Status  " & _
                            " FROM     tblBOM LEFT JOIN tblVCE_Master " & _
                            " ON	   tblBOM.VCECode = tblVCE_Master.VCECode " & filter & " AND TransID NOT IN (SELECT BOM_TransID FROM viewPR_SO_Served) AND tblBOM.Status <> 'Cancelled'"
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "BOM"

                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE BOM_No LIKE '%' + @Filter + '%' AND ForPR = 1 "
                            Case "VCE Name"
                                filter = " WHERE VCEName LIKE '%' + @Filter + '%' AND ForPR = 1 "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' AND ForPR = 1 "
                            Case "Status"
                                filter = " WHERE tblBOM.Status LIKE '%' + @Filter + '%' AND ForPR = 1 "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, BOM_No AS [BOM No.], DateBOM AS [Date], VCEName, ItemCode, Description, QTY, Remarks,  tblBOM.Status  " & _
                            " FROM     tblBOM LEFT JOIN tblVCE_Master " & _
                            " ON	   tblBOM.VCECode = tblVCE_Master.VCECode " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)


                Case "RFP"

                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE RFP_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblRFP.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, RFP_No AS [RFP No.], DateRFP AS [Date], VCEName, Remarks,  tblRFP.Status  " & _
                            " FROM     tblRFP LEFT JOIN tblVCE_Master " & _
                            " ON	   tblRFP.VCECode = tblVCE_Master.VCECode " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)
                Case "PR"

                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE PR_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblPR.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, PR_No AS [PR No.], DatePR AS [Date],  Remarks, DateNeeded, RequestedBy, tblPR.Status  " & _
                            " FROM     tblPR LEFT JOIN tblVCE_Master " & _
                            " ON	   tblPR.VCECode = tblVCE_Master.VCECode " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "PO"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE tblPO.PO_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE viewPO_Status.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   tblPO.TransID, tblPO.PO_No AS [PO No.], DatePO AS [Date], VCEName AS [Supplier], Remarks, NetAmount, viewPO_Status.Status  " & _
                            " FROM     tblPO LEFT JOIN tblVCE_Master " & _
                            " ON	   tblPO.VCECode = tblVCE_Master.VCECode " & _
                            " LEFT JOIN	 viewPO_Status " & _
                            " ON		 tblPO.TransID = viewPO_Status.TransID " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)
                Case "RR"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " AND '' = '' ORDER BY tblRR.TransID"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " AND RR_No LIKE '%' + @Filter + '%' ORDER BY tblRR.TransID "
                            Case "VCE Name"
                                filter = " AND VCEName LIKE '%' + @Filter + '%' ORDER BY tblRR.TransID"
                            Case "Remarks"
                                filter = " AND Remarks LIKE '%' + @Filter + '%' ORDER BY tblRR.TransID"
                            Case "Status"
                                filter = " AND tblRR.Status LIKE '%' + @Filter + '%' ORDER BY tblRR.TransID"
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, RR_No AS [RR No.], DateRR AS [Date], VCEName AS [Supplier], Remarks, PO_Ref AS [Reference PO], tblRR.Status  " & _
                            " FROM     tblRR LEFT JOIN tblVCE_Master " & _
                            " ON	   tblRR .VCECode = tblVCE_Master.VCECode " & _
                            " LEFT JOIN tblWarehouse  ON	         " & _
                            "  tblRR.WHSE = tblWarehouse.Code   " & _
                            "  WHERE         " & _
                            "  ( WHSE IN (SELECT    DISTINCT tblWarehouse.Code " & _
                            "  FROM      tblWarehouse  " & _
                            "  INNER JOIN tblUser_Access    ON         " & _
                            "  tblWarehouse.Code = tblUser_Access.Code  " & _
                            "   AND       tblUser_Access.Status ='Active' AND tblWarehouse.Status ='Active'  " & _
                            "   AND       tblUser_Access.Type = 'Warehouse' AND isAllowed = 1 " & _
                            "   WHERE     UserID ='" & UserID & "'))  " & _
                            "   OR " & _
                            "   WHSE ='MW' " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "RR-APV"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " AND '' = '' "
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " AND RR_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " AND VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " AND Remarks  LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " AND tblRR.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, RR_No AS [RR No.], DateRR AS [Date], VCEName AS [Supplier], Remarks, PO_Ref AS [Reference PO], tblRR.Status  " & _
                            " FROM     tblRR LEFT JOIN tblVCE_Master " & _
                            " ON	   tblRR .VCECode = tblVCE_Master.VCECode " & _
                            "WHERE TransID not in (SELECT RR_Ref from tblAPV WHERE Status ='Active')  " & filter & " ORDER BY tblRR.TransID "
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "APV"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " AND '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " AND APV_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " AND VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " AND tblAPV.Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " AND CASE WHEN View_APV_Balance.Ref_TransID IS NOT NULL THEN  'Active'  WHEN tblAPV.Status ='Active' THEN 'Closed' ELSE tblAPV.Status   END LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, tblBranch.Description as Branch, tblAPV.APV_No AS [APV No.], DateAPV AS [Date], VCEName AS [Supplier], tblAPV.Remarks, PO_Ref AS [Reference PO], " & _
                            "           CASE WHEN View_APV_Balance.Ref_TransID IS NOT NULL THEN  'Active' " & _
                            "                WHEN tblAPV.Status ='Active' THEN 'Closed' ELSE tblAPV.Status   END AS Status" & _
                            " FROM     tblAPV LEFT JOIN viewVCE_Master " & _
                            " ON	   tblAPV .VCECode = viewVCE_Master.VCECode " & _
                             " LEFT JOIN tblBranch  ON	           " & _
                            " tblAPV.BranchCode = tblBranch.BranchCode      " & _
                            " LEFT JOIN View_APV_Balance " & _
                            " ON        tblAPV.TransID = View_APV_Balance.Ref_TransID " & _
                             " WHERE          " & _
                            " 	( tblAPV.BranchCode IN  " & _
                            " 	  (SELECT    DISTINCT tblBranch.BranchCode  " & _
                            " 	  FROM      tblBranch   " & _
                            " 	  INNER JOIN tblUser_Access    ON          " & _
                            " 	  tblBranch.BranchCode = tblUser_Access.Code   " & _
                            " 	   AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                            " 	   AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                            " 	   WHERE     UserID ='" & UserID & "'" & _
                            " 	   ) " & _
                            "     )   " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)
                Case "SQ"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE tblSQ.SQ_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblSQ.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   tblSQ.TransID, tblSQ.SQ_No AS [SQ No.], DateSQ AS [Date], VCEName AS [Customer], Remarks, NetAmount, tblSQ.Status  " & _
                            " FROM     tblSQ LEFT JOIN tblVCE_Master " & _
                            " ON	   tblSQ.VCECode = tblVCE_Master.VCECode " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "SO"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE tblSO.SO_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblSO.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   CAST(tblSO.TransID AS nvarchar)  AS TransID, CAST(tblSO.SO_No AS nvarchar)  AS [SO No.],  " & _
                            "          DateSO AS [Date], VCEName AS [Customer], tblSO.NetAmount, Remarks, ReferenceNo, tblSO.Status  " & _
                            " FROM     tblSO LEFT JOIN tblVCE_Master " & _
                            " ON	   tblSO.VCECode = tblVCE_Master.VCECode "
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "PR_SO"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE tblSO.SO_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblSO.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   CAST(tblSO.TransID AS nvarchar)  AS TransID, CAST(tblSO.SO_No AS nvarchar)  AS [SO No.],  " & _
                            "          DateSO AS [Date], VCEName AS [Customer], tblSO.NetAmount, tblSO.Remarks, ReferenceNo, tblSO.Status  " & _
                            " FROM     tblSO LEFT JOIN tblVCE_Master " & _
                            " ON	   tblSO.VCECode = tblVCE_Master.VCECode " & _
                            " WHERE   tblso.transid IN ( select tblso.transid from tblso " & _
                            " INNER JOIN  tblJO ON  tblSO.TransID = tblJO.SO_Ref  " & _
                            " INNER JOIN  tblBOM ON " & _
                            " tblJO.TransID = tblBOM.JO_Ref " & _
                            " WHERE tblbom.transid NOT IN (SELECT BOM_TransID FROM viewPR_SO_Served)) AND tblSO.Status <> 'Cancelled'"
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "PO_SO"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE tblSO.SO_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblSO.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT CAST(tblSO.TransID AS nvarchar)  AS TransID, CAST(tblSO.SO_No AS nvarchar)  AS [SO No.], " & _
                            " 		DateSO AS [Date], VCEName AS [Customer], tblSO.NetAmount, tblSO.Remarks, ReferenceNo, tblSO.Status  " & _
                            " FROM     tblSO  " & _
                            " LEFT JOIN tblVCE_Master  ON	    " & _
                            " tblSO.VCECode = tblVCE_Master.VCECode " & _
                            " WHERE tblSO.TransID NOT IN (SELECT SO_Ref FROM tblPO WHERE tblPO.Status <> 'Cancelled') " & _
                            "  AND tblSO.Status <> 'Cancelled' "
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)


                Case "SO-JO"
                    query = " SELECT   RefID, SO_No AS [SO No.], " & _
                            " 	       DateSO AS [Date], VCEName AS [Customer], ItemCode, QTY, DateDeliver AS [Date Needed], ReferenceNo, Status   " & _
                            " FROM     viewSO_SKU " & _
                            " WHERE    Status ='Active' " & _
                            " ORDER BY RefID "

                Case "SO-JO-perLine"

                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE SO_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT    CAST(TransID AS nvarchar) + '-' + CAST(RowID AS nvarchar)  AS TransID,  " & _
                            " 	        CAST(SO_No AS nvarchar) + '-' + CAST(RowID AS nvarchar)  AS [SO No.], " & _
                            " 	        DateSO AS [Date], VCEName AS [Customer], ItemCode, QTY, DateDeliver AS [Date Needed], ReferenceNo, Status   " & _
                            " FROM " & _
                            " ( " & _
                            " 	 SELECT		tblSO.TransID , tblSO.SO_No, DateSO, VCEName, ItemCode, SUM(QTY) AS QTY, ReferenceNo, viewSO_Status.Status, " & _
                            " 				ROW_NUMBER() OVER (PARTITION BY tblSO.SO_No ORDER BY tblSO.SO_No) AS RowID, MAX(tblSO_Details.DateDeliver) AS DateDeliver " & _
                            " 	 FROM		tblSO LEFT JOIN tblSO_Details   " & _
                            " 	 ON			tblSO.TransID = tblSO_Details.TransID  " & _
                            " 	 LEFT JOIN	tblVCE_Master  " & _
                            " 	 ON			tblSO.VCECode = tblVCE_Master.VCECode  " & _
                            "   LEFT JOIN viewSO_Status ON " & _
                            "   viewSO_Status.TransID = tblSO.TransID" & _
                            " 	 GROUP BY   tblSO.TransID , tblSO.SO_No, DateSO, VCEName, ItemCode, ReferenceNo, viewSO_Status.Status   " & _
                            " ) AS SO " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "DR"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE DR_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblDR.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, DR_No AS [DR No.], DateDR AS [Date], VCEName AS [Customer], Remarks, SO_Ref AS [Reference SO], tblDR.Status  " & _
                            " FROM     tblDR LEFT JOIN tblVCE_Master " & _
                            " ON	   tblDR .VCECode = tblVCE_Master.VCECode " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "ECS"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE ECS_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblECS.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, tblECS.ECS_No AS [ECS No.], DateECS AS [Date], VCEName AS [Customer], tblECS.Remarks, SO_Ref AS [Reference SO], " & _
                            "       tblECS.Status" & _
                            " FROM     tblECS LEFT JOIN tblVCE_Master " & _
                            " ON	   tblECS.VCECode = tblVCE_Master.VCECode " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "SI"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE SI_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblSI.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, tblSI.SI_No AS [SI No.], DateSI AS [Date], VCEName AS [Customer], tblSI.Remarks, SO_Ref AS [Reference SO], " & _
                            "           CASE WHEN View_SI_Balance.Ref_TransID IS NOT NULL THEN  'Active' " & _
                            "                WHEN tblSI.Status ='Active' THEN 'Closed' ELSE tblSI.Status   END AS Status" & _
                            " FROM     tblSI LEFT JOIN tblVCE_Master " & _
                            " ON	   tblSI.VCECode = tblVCE_Master.VCECode " & _
                            " LEFT JOIN View_SI_Balance " & _
                            " ON        tblSI.TransID = View_SI_Balance.Ref_TransID " & filter

                    'query = " SELECT   TransID, SI_No AS [SI No.], DateSI AS [Date], VCEName AS [Customer], Remarks, SO_Ref AS [Reference SO], tblSI.Status  " & _
                    '        " FROM     tblSI LEFT JOIN tblVCE_Master " & _
                    '        " ON	   tblSI .VCECode = tblVCE_Master.VCECode " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "ADV"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE ADV_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblADV.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, ADV_No AS [ADV No.], DateADV AS [Date], VCEName AS [Supplier], Remarks, PO_Ref AS [Reference PO], tblADV.Status  " & _
                            " FROM     tblADV LEFT JOIN tblVCE_Master " & _
                            " ON	   tblADV .VCECode = tblVCE_Master.VCECode " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "CA"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE CA_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblCA.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, CA_No AS [CA No.], DateCA AS [Date], VCEName AS [Name], Remarks,  tblCA.Status  " & _
                            " FROM     tblCA LEFT JOIN viewVCE_Master " & _
                            " ON	   tblCA .VCECode = viewVCE_Master.VCECode " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "CV"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " AND '' = '' ORDER BY CV_No"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " AND CV_No LIKE '%' + @Filter + '%' ORDER BY CV_No"
                            Case "VCE Name"
                                filter = " AND VCEName LIKE '%' + @Filter + '%' ORDER BY CV_No "
                            Case "Remarks"
                                filter = " AND Remarks LIKE '%' + @Filter + '%' ORDER BY CV_No "
                            Case "Status"
                                filter = " AND tblCV.Status LIKE '%' + @Filter + '%' ORDER BY CV_No "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, tblBranch.Description as Branch, CV_No AS [CV No.], DateCV AS [Date], VCEName AS [Supplier], Remarks, APV_Ref AS [Reference APV], tblCV.Status  " & _
                            " FROM     tblCV LEFT JOIN viewVCE_Master " & _
                            " ON	   tblCV .VCECode = viewVCE_Master.VCECode " & _
                             " LEFT JOIN tblBranch  ON	           " & _
                            " tblCV.BranchCode = tblBranch.BranchCode      " & _
                            " WHERE          " & _
                            " 	( tblCV.BranchCode IN  " & _
                            " 	  (SELECT    DISTINCT tblBranch.BranchCode  " & _
                            " 	  FROM      tblBranch   " & _
                            " 	  INNER JOIN tblUser_Access    ON          " & _
                            " 	  tblBranch.BranchCode = tblUser_Access.Code   " & _
                            " 	   AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                            " 	   AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                            " 	   WHERE     UserID ='" & UserID & "'" & _
                            " 	   ) " & _
                            "     )   " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)


                Case "PCV"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " AND '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " AND tblPCV_Entry.PCV_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " AND VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " AND tblPCV_Entry.Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " AND tblPCV_Entry.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, tblBranch.Description as Branch, tblPCV_Entry.PCV_No AS [PCV No.], DatePCV AS [Date], VCEName AS [Supplier], tblPCV_Entry.Remarks, APV_Ref AS [Reference APV],   " & _
                            "           CASE WHEN View_PCV_Balance.Ref_TransID IS NOT NULL THEN  'Active' " & _
                            "                WHEN tblPCV_Entry.Status ='Active' THEN 'Closed' ELSE tblPCV_Entry.Status   END AS Status" & _
                            " FROM     tblPCV_Entry LEFT JOIN viewVCE_Master " & _
                            " ON	   tblPCV_Entry .VCECode = viewVCE_Master.VCECode " & _
                            " LEFT JOIN tblBranch  ON	           " & _
                            " tblPCV_Entry.BranchCode = tblBranch.BranchCode      " & _
                            " LEFT JOIN View_PCV_Balance " & _
                            " ON        tblPCV_Entry.TransID = View_PCV_Balance.Ref_TransID " & _
                            " WHERE          " & _
                            " 	( tblPCV_Entry.BranchCode IN  " & _
                            " 	  (SELECT    DISTINCT tblBranch.BranchCode  " & _
                            " 	  FROM      tblBranch   " & _
                            " 	  INNER JOIN tblUser_Access    ON          " & _
                            " 	  tblBranch.BranchCode = tblUser_Access.Code   " & _
                            " 	   AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                            " 	   AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                            " 	   WHERE     UserID ='" & UserID & "'" & _
                            " 	   ) " & _
                            "     )   " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "BD"

                    ' QUERY 
                    query = "  SELECT   TransID, tblBranch.Description as Branch,BD_No AS [BD No.], DateBD AS [Date], Remarks, Bank, Branch, tblBD.Status " & _
                            "  FROM     tblBD LEFT JOIN tblBank_Master ON " & _
                            "  tblBank_Master.BankID = tblBD.BankID " & _
                            " LEFT JOIN tblBranch  ON	           " & _
                            " tblBD.BranchCode = tblBranch.BranchCode      " & _
                            " WHERE          " & _
                            " 	( tblBD.BranchCode IN  " & _
                            " 	  (SELECT    DISTINCT tblBranch.BranchCode  " & _
                            " 	  FROM      tblBranch   " & _
                            " 	  INNER JOIN tblUser_Access    ON          " & _
                            " 	  tblBranch.BranchCode = tblUser_Access.Code   " & _
                            " 	   AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                            " 	   AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                            " 	   WHERE     UserID ='" & UserID & "'" & _
                            " 	   ) " & _
                            "     )  "
                    SQL.FlushParams()

                Case "GI"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " AND '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " AND GI_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " AND VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " AND Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " AND tblGI.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = "  SELECT   TransID, GI_No AS [GI No.], DateGI AS [Date], VCEName AS [VCEName], Remarks, tblGI.Type AS [Type], tblGI.Status  " & _
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
                            "  	 WHERE     UserID ='" & UserID & "'))  " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "GR"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE GR_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblGR.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, GR_No AS [GR No.], DateGR AS [Date], VCEName AS [VCEName], Remarks, Type AS [Type], tblGR.Status  " & _
                            " FROM     tblGR LEFT JOIN tblVCE_Master " & _
                            " ON	   tblGR .VCECode = tblVCE_Master.VCECode " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "ITI"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE ITI_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblITI.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, ITI_No AS [ITI No.], DateITI AS [Date], VCEName AS [VCEName], Remarks, Type AS [Type], tblITI.Status  " & _
                            " FROM     tblITI LEFT JOIN tblVCE_Master " & _
                            " ON	   tblITI .VCECode = tblVCE_Master.VCECode " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "ITR"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE ITR_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblITR.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, ITR_No AS [ITR No.], DateITR AS [Date], VCEName AS [VCEName], Remarks, Type AS [Type], tblITR.Status  " & _
                            " FROM     tblITR LEFT JOIN tblVCE_Master " & _
                            " ON	   tblITR .VCECode = tblVCE_Master.VCECode " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "IT"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE IT_No LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblIT.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, IT_No AS [IT No.], DateIT AS [Date], Remarks, Type AS [Type], tblIT.Status  " & _
                            " FROM     tblIT " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "RI"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE RI_No LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblRI.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, RI_No AS [RI No.], DateRI AS [Date], Remarks, Type AS [Type], tblRI.Status  " & _
                            " FROM     tblRI " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "JV"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " AND '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " AND JV_No LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " AND Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " AND tblJV.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, tblBranch.Description as Branch,  JV_No AS [JV No.], DateJV AS [Date], VCEName as [Name], Remarks, Type AS [Type], tblJV.Status  " & _
                            " FROM     tblJV LEFT JOIN viewVCE_Master " & _
                            " ON	   tblJV.VCECode = viewVCE_Master.VCECode " & _
                            " LEFT JOIN tblBranch  ON	           " & _
                            " tblJV.BranchCode = tblBranch.BranchCode      " & _
                            " WHERE          " & _
                            " 	( tblJV.BranchCode IN  " & _
                            " 	  (SELECT    DISTINCT tblBranch.BranchCode  " & _
                            " 	  FROM      tblBranch   " & _
                            " 	  INNER JOIN tblUser_Access    ON          " & _
                            " 	  tblBranch.BranchCode = tblUser_Access.Code   " & _
                            " 	   AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                            " 	   AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                            " 	   WHERE     UserID ='" & UserID & "'" & _
                            " 	   ) " & _
                            "     )   " & filter & _
                            "  ORDER BY tblBranch.Description, JV_No, Date "
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "PJ"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " AND '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " AND PJ_No LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " AND Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " AND tblPJ.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, tblBranch.Description as Branch,  PJ_No AS [PJ No.], DatePJ AS [Date], VCEName as [Name], Remarks, Type AS [Type], tblPJ.Status  " & _
                            " FROM     tblPJ LEFT JOIN viewVCE_Master " & _
                            " ON	   tblPJ.VCECode = viewVCE_Master.VCECode " & _
                            " LEFT JOIN tblBranch  ON	           " & _
                            " tblPJ.BranchCode = tblBranch.BranchCode      " & _
                            " WHERE          " & _
                            " 	( tblPJ.BranchCode IN  " & _
                            " 	  (SELECT    DISTINCT tblBranch.BranchCode  " & _
                            " 	  FROM      tblBranch   " & _
                            " 	  INNER JOIN tblUser_Access    ON          " & _
                            " 	  tblBranch.BranchCode = tblUser_Access.Code   " & _
                            " 	   AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                            " 	   AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                            " 	   WHERE     UserID ='" & UserID & "'" & _
                            " 	   ) " & _
                            "     )   " & filter & _
                            "  ORDER BY tblBranch.Description, PJ_No, Date "
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "SJ"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " AND '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " AND SJ_No LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " AND Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " AND tblSJ.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, tblBranch.Description as Branch,  SJ_No AS [SJ No.], DateSJ AS [Date], VCEName as [Name], Remarks, Type AS [Type], tblSJ.Status  " & _
                            " FROM     tblSJ LEFT JOIN viewVCE_Master " & _
                            " ON	   tblSJ.VCECode = viewVCE_Master.VCECode " & _
                            " LEFT JOIN tblBranch  ON	           " & _
                            " tblSJ.BranchCode = tblBranch.BranchCode      " & _
                            " WHERE          " & _
                            " 	( tblSJ.BranchCode IN  " & _
                            " 	  (SELECT    DISTINCT tblBranch.BranchCode  " & _
                            " 	  FROM      tblBranch   " & _
                            " 	  INNER JOIN tblUser_Access    ON          " & _
                            " 	  tblBranch.BranchCode = tblUser_Access.Code   " & _
                            " 	   AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                            " 	   AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                            " 	   WHERE     UserID ='" & UserID & "'" & _
                            " 	   ) " & _
                            "     )   " & filter & _
                            "  ORDER BY tblBranch.Description, SJ_No, Date "
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "SP"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE SP_No LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblSP.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   TransID, SP_No AS [SP No.], DateSP AS [Date], VCEName, NetAmount,  Remarks, Type AS [Type], tblSP.Status  " & _
                            " FROM     tblSP " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "OR"
                    query = GetQueryCollection(moduleID)
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)
                Case "AR"
                    query = GetQueryCollection(moduleID)
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)
                Case "CR"
                    query = GetQueryCollection(moduleID)
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)
                Case "CSI"
                    query = GetQueryCollection(moduleID)
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "PR"
                    query = GetQueryCollection(moduleID)
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "CF"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE CF_No LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblCF.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   tblCF.TransID, CF_No AS [CF No.], DateCF AS [Date],  TotalAmount,  tblCF.Remarks, DateNeeded, RequestedBy, tblCF.Status  " & _
                            " FROM     tblCF  " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                    'Case "PCV"
                    '    ' CONDITION OF QUERY
                    '    If cbFilter.SelectedIndex = -1 Then
                    '        filter = " WHERE '' = ''"
                    '    Else
                    '        Select Case cbFilter.SelectedItem
                    '            Case "Transaction ID"
                    '                filter = " WHERE PCV_No LIKE '%' + @Filter + '%' "
                    '            Case "VCE Name"
                    '                filter = " WHERE VCEName LIKE '%' + @Filter + '%' "
                    '            Case "Remarks"
                    '                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                    '            Case "Status"
                    '                filter = " WHERE tblPCV.Status LIKE '%' + @Filter + '%' "
                    '        End Select
                    '    End If

                    '    ' QUERY 
                    '    query = " SELECT  TransID, PCV_No AS [PCV No],  tblPCV.VCECode, VCEName, DatePCV, Amount, Remarks,    tblPCV.Status  " & _
                    '    " FROM tblPCV" & _
                    '     " LEFT JOIN tblVCE_Master  ON  " & _
                    '     " tblPCV.VCECode = tblVCE_Master.VCECode " & filter
                    '    SQL.FlushParams()
                    '    SQL.AddParam("@Filter", txtFilter.Text)

                Case "PCV_Load"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE PCV_No LIKE '%' + @Filter + '%' "
                            Case "VCE Name"
                                filter = " WHERE VCEName LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblPCV.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT  TransID, PCV_No AS [PCV No],  tblPCV.VCECode, VCEName, DatePCV, Amount, Remarks,    tblPCV.Status  " & _
                    " FROM tblPCV" & _
                     " LEFT JOIN tblVCE_Master  ON  " & _
                     " tblPCV.VCECode = tblVCE_Master.VCECode " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)


                Case "Sub CF"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE CF_No LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblCF.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = " SELECT   tblCF.TransID, SupplierCode, VCEName, CF_No AS [CF No.], DateCF AS [Date], CFdetials.TotalAmount, [No. of Items],  tblCF.Remarks,  tblCF.Status  " & _
                            " FROM     tblCF INNER JOIN  " & _
                            " ( " & _
                            "     SELECT  TransID, CASE WHEN ApproveSP ='Supplier 1' THEN S1code " & _
                            "                           WHEN ApproveSP ='Supplier 2' THEN S2code " & _
                            "                           WHEN ApproveSP ='Supplier 3' THEN S3code " & _
                            "                           WHEN ApproveSP ='Supplier 4' THEN S4code " & _
                            "                      END AS SupplierCode, " & _
                            "             SUM(TotalAmount) AS TotalAmount, COUNT(TotalAmount) AS [No. of Items] " & _
                            " FROM tblCF_Details " & _
                            " WHERE Status ='Active' " & _
                            " GROUP BY TransID, CASE WHEN ApproveSP ='Supplier 1' THEN S1code " & _
                            "                           WHEN ApproveSP ='Supplier 2' THEN S2code " & _
                            "                           WHEN ApproveSP ='Supplier 3' THEN S3code " & _
                            "                           WHEN ApproveSP ='Supplier 4' THEN S4code " & _
                            "                      END " & _
                            " ) AS CFdetials " & _
                            " ON        tblCF.TransID = CFdetials.TransID " & _
                            " LEFT JOIN tblVCE_Master " & _
                            " ON tblVCE_Master.VCECode = CFdetials.SupplierCode " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "Sub PO"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " WHERE '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Transaction ID"
                                filter = " WHERE PO_No LIKE '%' + @Filter + '%' "
                            Case "Remarks"
                                filter = " WHERE Remarks LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " WHERE tblPO.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = "  SELECT   tblPO.TransID, POdetials.VCECode, VCEName, PO_No AS [PO No.], DatePO AS [Date], POdetials.TotalAmount, [No. of Items],  tblPO.Remarks,  tblPO.Status   " & _
                            "  FROM     tblPO INNER JOIN   " & _
                            "  (  " & _
                            "      SELECT  TransID, VCECode, " & _
                            "              SUM(NetAmount) AS TotalAmount, COUNT(NetAmount) AS [No. of Items]  " & _
                            "  FROM tblPO_Details  " & _
                            "  WHERE Status ='Active'  " & _
                            "  GROUP BY TransID, VCECode " & _
                            "  ) AS POdetials  " & _
                            "  ON        tblPO.TransID = POdetials.TransID  " & _
                            "  LEFT JOIN tblVCE_Master  " & _
                            "  ON tblVCE_Master.VCECode = POdetials.VCECode  " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

                Case "Copy Member"
                    ' CONDITION OF QUERY
                    If cbFilter.SelectedIndex = -1 Then
                        filter = " AND '' = ''"
                    Else
                        Select Case cbFilter.SelectedItem
                            Case "Member ID"
                                filter = " AND Member_ID LIKE '%' + @Filter + '%' "
                            Case "Full Name"
                                filter = " AND Full_Name LIKE '%' + @Filter + '%' "
                            Case "Status"
                                filter = " AND tblMember_Master.Status LIKE '%' + @Filter + '%' "
                        End Select
                    End If

                    ' QUERY 
                    query = "  SELECT Member_ID, Full_Name,Member_Type, tblMember_Master.BranchCode, Description as BranchName from tblMember_Master   " & _
                            "  INNER JOIN tblBranch ON   " & _
                            "  tblBranch.BranchCode = tblMember_Master.BranchCode  " & _
                            " INNER JOIN tblUser_Access    ON   " & _
                            " tblMember_Master.BranchCode = tblUser_Access.Code    " & _
                            " AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                            " AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                            " WHERE     UserID ='" & UserID & "'  " & filter
                    SQL.FlushParams()
                    SQL.AddParam("@Filter", txtFilter.Text)

            End Select
            If query <> "" Then
                SQL.GetQuery(query)
                If SQL.SQLDS.Tables(0).Rows.Count > 0 Then
                    dgvList.DataSource = SQL.SQLDS.Tables(0)
                    dgvList.Columns(0).Visible = False
                End If
                'If moduleID = "PR" Then
                '    dgvList.Columns.Add()
                'End If
            End If

        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, moduleID)
        Finally
            SQL.FlushParams()
        End Try
    End Sub

    Private Function GetQueryCollection(ByVal Type As String) As String
        ' CONDITION OF QUERY
        Dim filter As String = ""
        Dim temp As String = ""
        If cbFilter.SelectedIndex = -1 Then
            filter = " ORDER BY TransNo ASC"
        Else
            Select Case cbFilter.SelectedItem
                Case "Transaction ID"
                    filter = " AND TransNo LIKE '%' + @Filter + '%' ORDER BY TransNo ASC"
                Case "Remarks"
                    filter = " AND Remarks LIKE '%' + @Filter + '%' ORDER BY TransNo ASC"
                Case "Status"
                    filter = " AND tblCollection.Status LIKE '%' + @Filter + '%' ORDER BY TransNo ASC"
                Case "VCE Name"
                    filter = " AND VCEName LIKE '%' + @Filter + '%' ORDER BY TransNo ASC"
            End Select
        End If

        ' QUERY 
        temp = " SELECT   TransID, tblBranch.Description as Branch, TransNo AS [TransNo.], DateTrans AS [Date], VCEName AS [VCEName], Remarks, Amount AS [Amount], tblCollection.Status  " & _
                " FROM     tblCollection LEFT JOIN viewVCE_Master " & _
                " ON       tblCollection.VCECode = viewVCE_Master.VCECode " & _
                 " LEFT JOIN tblBranch  ON	           " & _
                " tblCollection.BranchCode = tblBranch.BranchCode      " & _
                " WHERE          " & _
                " 	( tblCollection.BranchCode IN  " & _
                " 	  (SELECT    DISTINCT tblBranch.BranchCode  " & _
                " 	  FROM      tblBranch   " & _
                " 	  INNER JOIN tblUser_Access    ON          " & _
                " 	  tblBranch.BranchCode = tblUser_Access.Code   " & _
                " 	   AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                " 	   AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                " 	   WHERE     UserID ='" & UserID & "'" & _
                " 	   ) " & _
                "     )   " & _
                " AND    tblCollection.TransType ='" & Type & "'  " & filter
        Return temp
    End Function

    Private Sub dgvList_CellBeginEdit(sender As Object, e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles dgvList.CellBeginEdit

    End Sub

    Private Sub dgvList_DoubleClick(sender As System.Object, e As System.EventArgs) Handles dgvList.DoubleClick
      ChooseRecord
    End Sub


    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        ChooseRecord()
    End Sub

    Private Sub ChooseRecord()
        If dgvList.SelectedRows.Count = 1 Then
            transID = dgvList.SelectedRows(0).Cells(0).Value.ToString
            itemCode = dgvList.SelectedRows(0).Cells(1).Value.ToString
            If cbBranch.Visible = True Then
                BranchCode = Strings.Left(cbBranch.SelectedItem, cbBranch.SelectedItem.ToString.IndexOf(" - "))
            End If
            If moduleID = "PCV" Then
                frmAPV.LoadPCV(transID)
                Me.Close()
            End If
            Me.Close()
        End If
    End Sub

    Private Sub dgvList_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles dgvList.KeyDown
        If e.KeyCode = Keys.Enter Then
            ChooseRecord()
        End If
    End Sub

    Private Sub cbFilter_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbFilter.SelectedIndexChanged

    End Sub

    Private Sub btnSearch_Click(sender As System.Object, e As System.EventArgs) Handles btnSearch.Click
        LoadData()
    End Sub

    Private Sub txtFilter_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtFilter.KeyDown
        If e.KeyCode = Keys.Enter Then
            LoadData()
        End If
    End Sub

    Private Sub frmLoadTransactions_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        ElseIf e.KeyCode = Keys.Down Or e.KeyCode = Keys.Up Then
            ' CHANGE FOCUS TO DATAGRID SELECTION ON WHEN KEY DOWN OR KEY UP IS PRESSED
            Dim RowIndex As Integer = 0
            If dgvList.Focused = False Then
                If dgvList.SelectedRows.Count = 0 Then ' IF THERE ARE NO ROWS SELECTED THEN SELECT A DEFAUL ROW IF THERE ARE EXISTING ROW
                    If dgvList.Rows.Count > 0 Then
                        dgvList.Rows(0).Selected = True
                    End If
                Else
                    If e.KeyCode = Keys.Down Then
                        If dgvList.Rows.Count > dgvList.SelectedRows(0).Index + 1 Then
                            dgvList.Focus()
                            RowIndex = dgvList.SelectedRows(0).Index
                            dgvList.Rows(dgvList.SelectedRows(0).Index).Selected = False
                            dgvList.Rows(RowIndex + 1).Selected = True
                        End If
                    ElseIf e.KeyCode = Keys.Up Then
                        If dgvList.SelectedRows(0).Index > 0 Then
                            dgvList.Rows(dgvList.SelectedRows(0).Index - 1).Selected = True
                        End If
                    End If
                End If
                dgvList.Focus()
            End If
        Else
            txtFilter.Focus()
            txtFilter.SelectionStart = txtFilter.TextLength
        End If
    End Sub


    Private Sub txtFilter_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtFilter.TextChanged
        LoadData()
    End Sub

    Private Sub chkBatch_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkBatch.CheckedChanged
        batch = chkBatch.Checked
    End Sub

    Private Sub dgvList_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvList.CellContentClick
        If e.RowIndex >= 0 Then
            If e.ColumnIndex = dgvList.Columns.Count - 1 Then

            End If
        End If
    End Sub
End Class