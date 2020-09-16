Public Class frmReport_Generator
    Public Type As String
    Dim branch As String

    Private Sub frmReports_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        cbReport.Items.Clear()
        cbReport.Items.Add("Trial Balance")
        cbReport.Items.Add("Subsidiary Ledger Schedule")
        'cbReport.Items.Add("Subsidiary Ledger Column")
        'cbReport.Items.Add("Insurance Premium Payable")
        cbReport.Items.Add("Account Schedule")
        cbReport.Items.Add("Book of Accounts")
        cbReport.Items.Add("Balance Sheet")
        cbReport.Items.Add("Income Statement")
        'cbReport.Items.Add("Loan Aging Report")
        'cbReport.Items.Add("Loan Aging Report Per Loan Type")
        'cbReport.Items.Add("Loan Release")
        'cbReport.Items.Add("Loan Title")
        cbReport.Items.Add("CV Transaction")
        cbReport.Items.Add("OR Transaction")
        cbReport.Items.Add("Check Issuance")
        'cbReport.Items.Add("Account Balances")
        'cbReport.Items.Add("Loan Balances")
        cbReport.Items.Add("Member List")
        cbReport.Items.Add("Chart of Account")
        'cbReport.Items.Add("Loan Co-Maker List")
        nupYear.Value = Date.Today.Year
        cbMonth.SelectedIndex = Date.Today.Month - 1
        LoadBranches()
    End Sub

    Private Sub LoadBranches()
        Dim query As String
        query = " SELECT    DISTINCT  tblBranch.BranchCode + ' - ' + Description AS BranchCode  " & _
                " FROM      tblBranch    " & _
                " INNER JOIN tblUser_Access    ON   " & _
                " tblBranch.BranchCode = tblUser_Access.Code    " & _
                " AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                " AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                " WHERE     UserID ='" & UserID & "'  "
        SQL.ReadQuery(query)
        cbBranch.Items.Clear()
        cbBranch.Items.Add("ALL - ALL BRANCHES")
        While SQL.SQLDR.Read
            cbBranch.Items.Add(SQL.SQLDR("BranchCode").ToString)
        End While
        cbBranch.SelectedItem = "ALL - ALL BRANCHES"
    End Sub
    Private Sub cbReport_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbReport.SelectedIndexChanged
        If cbReport.SelectedItem = "Trial Balance" Then
            Reset()
            gbType.Enabled = True
            cbMonth.SelectedIndex = Date.Today.Month - 1
            gbPeriodYM.Visible = True
            lblType.Visible = False
            cmbLoanType.Visible = False
        ElseIf cbReport.SelectedItem = "Book of Accounts" Then
            gbType.Enabled = True
            Reset()
            cbPeriod.Enabled = True
            cbPeriod.SelectedIndex = 0
            gbPeriodYM.Visible = True
            lvFilter.Items.Add("General Ledger")
            lvFilter.Items.Add("Cash Receipts")
            lvFilter.Items.Add("Cash Disbursements")
            lvFilter.Items.Add("Sales")
            lvFilter.Items.Add("Purchase Journal")
            lvFilter.Items.Add("General Journal")
            'lvFilter.Items.Add("Inventory")
            lvFilter.Items(0).Checked = True
            lblType.Visible = False
            cmbLoanType.Visible = False
        ElseIf cbReport.SelectedItem = "Balance Sheet" Or cbReport.SelectedItem = "Income Statement" Then
            Reset()
            cbPeriod.Enabled = False
            gbPeriodYM.Visible = True
            gbType.Enabled = False
            lblType.Visible = False
            cmbLoanType.Visible = False
        ElseIf cbReport.SelectedItem = "Subsidiary Ledger Schedule" Then
            Reset()
            cbPeriod.Enabled = False
            gbPeriodFT.Visible = True
            lblTo.Visible = False
            dtpTo.Visible = False
            lblFrom.Text = "As Of :"
            lblType.Visible = False
            cmbLoanType.Visible = False
            gbType.Enabled = False
            LoadSubsidiary()
        ElseIf cbReport.SelectedItem = "Subsidiary Ledger Column" Then
            Reset()
            cbPeriod.Enabled = False
            gbPeriodFT.Visible = True
            lblTo.Visible = False
            dtpTo.Visible = False
            lblFrom.Text = "As Of :"
            lblType.Visible = False
            cmbLoanType.Visible = False
            gbType.Enabled = False
            LoadSubsidiary()
        ElseIf cbReport.SelectedItem = "Insurance Premium Payable" Then
            Reset()
            cbPeriod.Enabled = False
            gbPeriodFT.Visible = True
            lblTo.Visible = False
            dtpTo.Visible = False
            lblFrom.Text = "As Of :"
            lblType.Visible = False
            cmbLoanType.Visible = False
            gbType.Enabled = False
            LoadInsurance()
        ElseIf cbReport.SelectedItem = "Daily Cash Position" Then
            Reset()
            cbPeriod.Enabled = False
            gbPeriodFT.Visible = True
            gbPeriodYM.Visible = False
            lblTo.Visible = False
            dtpTo.Visible = False
            lblFrom.Text = "As Of :"
            lblType.Visible = False
            cmbLoanType.Visible = False
            gbType.Enabled = False
        ElseIf cbReport.SelectedItem = "Account Schedule" Then
            Reset()
            cbPeriod.Enabled = True
            gbPeriodFT.Visible = False
            gbPeriodYM.Visible = True
            lblTo.Visible = False
            dtpTo.Visible = False
            lblFrom.Text = "As Of :"
            lblType.Visible = False
            cmbLoanType.Visible = False
            gbType.Enabled = True
            cbPeriod.SelectedIndex = 0
            LoadAccounts()
        ElseIf cbReport.SelectedItem = "Loan Aging Report" Then
            Reset()
            cbPeriod.Enabled = False
            gbPeriodFT.Visible = True
            gbPeriodYM.Visible = False
            lblTo.Visible = False
            dtpTo.Visible = False
            lblFrom.Text = "As Of :"
            lblType.Visible = False
            cmbLoanType.Visible = False
            gbType.Enabled = False
        ElseIf cbReport.SelectedItem = "Loan Aging Report Per Loan Type" Then
            Reset()
            cbPeriod.Enabled = False
            gbPeriodFT.Visible = True
            gbPeriodYM.Visible = False
            lblTo.Visible = False
            dtpTo.Visible = False
            lblFrom.Text = "As Of :"
            lblType.Visible = True
            lblType.Text = "Loan Type :"
            cmbLoanType.Visible = True
            gbType.Enabled = False
            LoadLoanType()
        ElseIf cbReport.SelectedItem = "BIR Tax 2550" Then
            Reset()
            cbPeriod.Enabled = True
            gbPeriodFT.Visible = False
            gbPeriodYM.Visible = True
            lblTo.Visible = False
            dtpTo.Visible = False
            lblFrom.Text = "From :"
            gbType.Enabled = False
            lblType.Visible = False
            cmbLoanType.Visible = False
            cbPeriod.Items.Clear()
            cbPeriod.Items.Add("Quarterly")
            cbPeriod.Items.Add("Monthly")
        ElseIf cbReport.SelectedItem = "Loan Release" Then
            Reset()
            cbPeriod.Enabled = False
            gbPeriodFT.Visible = False
            gbPeriodYM.Visible = False
            lblTo.Visible = True
            lblFrom.Visible = True
            dtpTo.Visible = True
            dtpFrom.Visible = True
            lblFrom.Text = "From :"
            lblTo.Text = "To :"
            gbType.Enabled = False
            lblType.Visible = False
            cmbLoanType.Visible = False
            cbPeriod.SelectedIndex = 3
        ElseIf cbReport.SelectedItem = "CV Transaction" Then
            Reset()
            cbPeriod.Enabled = False
            gbPeriodFT.Visible = False
            gbPeriodYM.Visible = False
            lblTo.Visible = True
            lblFrom.Visible = True
            dtpTo.Visible = True
            dtpFrom.Visible = True
            lblFrom.Text = "From :"
            lblTo.Text = "To :"
            gbType.Enabled = False
            lblType.Visible = False
            cmbLoanType.Visible = False
            cbPeriod.SelectedIndex = 3
        ElseIf cbReport.SelectedItem = "OR Transaction" Then
            Reset()
            cbPeriod.Enabled = False
            gbPeriodFT.Visible = False
            gbPeriodYM.Visible = False
            lblTo.Visible = True
            lblFrom.Visible = True
            dtpTo.Visible = True
            dtpFrom.Visible = True
            lblFrom.Text = "From :"
            lblTo.Text = "To :"
            gbType.Enabled = False
            lblType.Visible = False
            cmbLoanType.Visible = False
            cbPeriod.SelectedIndex = 3
        ElseIf cbReport.SelectedItem = "Check Issuance" Then
            Reset()
            cbPeriod.Enabled = False
            gbPeriodFT.Visible = False
            gbPeriodYM.Visible = False
            lblTo.Visible = True
            lblFrom.Visible = True
            dtpTo.Visible = True
            dtpFrom.Visible = True
            lblFrom.Text = "From :"
            lblTo.Text = "To :"
            gbType.Enabled = False
            lblType.Visible = False
            cmbLoanType.Visible = False
            cbPeriod.SelectedIndex = 3
        ElseIf cbReport.SelectedItem = "Account Balances" Then
            Reset()
            cbPeriod.Enabled = False
            gbPeriodFT.Visible = False
            gbPeriodYM.Visible = False
            lblTo.Visible = True
            lblFrom.Visible = True
            dtpTo.Visible = True
            dtpFrom.Visible = True
            lblFrom.Text = "From :"
            lblTo.Text = "To :"
            gbType.Enabled = False
            lblType.Text = "Saving Type :"
            lblType.Visible = True
            cmbLoanType.Visible = True
            cbPeriod.SelectedIndex = 3
            LoadSavingsType()
        ElseIf cbReport.SelectedItem = "Loan Balances" Then
            Reset()
            cbPeriod.Enabled = False
            gbPeriodFT.Visible = True
            gbPeriodYM.Visible = False
            lblTo.Visible = False
            dtpTo.Visible = False
            lblFrom.Text = "As Of :"
            lblType.Visible = False
            cmbLoanType.Visible = False
            gbType.Enabled = False
            lblType.Visible = True
            lblType.Text = "Loan Type :"
            cmbLoanType.Visible = True
            LoadLoanAccount()
        ElseIf cbReport.SelectedItem = "Loan Title" Then
            Reset()
            cbPeriod.Enabled = False
            gbPeriodFT.Visible = False
            gbPeriodYM.Visible = False
            lblTo.Visible = True
            lblFrom.Visible = True
            dtpTo.Visible = True
            dtpFrom.Visible = True
            lblFrom.Text = "From :"
            lblTo.Text = "To :"
            gbType.Enabled = False
            lblType.Visible = False
            cmbLoanType.Visible = False
            cbPeriod.SelectedIndex = 3
        ElseIf cbReport.SelectedItem = "Member List" Then
            Reset()
            cbPeriod.Enabled = False
            gbPeriodFT.Visible = False
            gbPeriodYM.Visible = False
            lblTo.Visible = True
            lblFrom.Visible = True
            dtpTo.Visible = True
            dtpFrom.Visible = True
            lblFrom.Text = "From :"
            lblTo.Text = "To :"
            gbType.Enabled = False
            cbPeriod.SelectedIndex = 3
            LoadMembType()
        ElseIf cbReport.SelectedItem = "Chart of Account" Then
            Reset()
        ElseIf cbReport.SelectedItem = "Loan Co-Maker List" Then
            Reset()
        End If
    End Sub

    Private Sub LoadSubsidiary()
        Dim query As String
        query = " SELECT   DISTINCT tblCOA_Master.AccountTitle " & _
                " FROM     tblCOA_Master " & _
                " WHERE    withSubsidiary = 1 AND AccountGroup  = 'SubAccount'" & _
                " ORDER BY AccountTitle "
        SQL.ReadQuery(query)
        lvFilter.Items.Clear()
        While SQL.SQLDR.Read
            lvFilter.Items.Add(SQL.SQLDR("AccountTitle").ToString)
        End While
    End Sub

    Private Sub LoadInsurance()
        Dim query As String
        query = " SELECT   DISTINCT tblCOA_Master.AccountTitle " & _
                " FROM     tblCOA_Master " & _
                " WHERE    withSubsidiary = 1 " & _
                " AND AccountCode = (SELECT AccntCode FROM tblDefaultAccount WHERE Type = 'Insurance') " & _
                " ORDER BY AccountTitle "
        SQL.ReadQuery(query)
        lvFilter.Items.Clear()
        While SQL.SQLDR.Read
            lvFilter.Items.Add(SQL.SQLDR("AccountTitle").ToString)
        End While
    End Sub

    Private Sub LoadPeriod()
        If cbMonth.SelectedIndex <> -1 Then
            Dim period As String = (cbMonth.SelectedIndex + 1).ToString & "-1-" & nupYear.Value.ToString
            If chkYTD.Checked Then
                dtpFrom.Value = Date.Parse("1-1-" & nupYear.Value.ToString)
                dtpTo.Value = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, Date.Parse(period)))
            Else
                dtpFrom.Value = Date.Parse(period)
                dtpTo.Value = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, Date.Parse(period)))
            End If
        End If
    End Sub

    Private Sub Reset()
        cbMonth.Enabled = True
        lvFilter.Items.Clear()
        gbPeriodYM.Visible = True
        gbPeriodFT.Visible = False
        cbPeriod.Enabled = False
        lblFrom.Text = "From :"
        lblTo.Visible = True
        dtpTo.Visible = True
        cbPeriod.Items.Clear()
        cbPeriod.Items.Add("Yearly")
        cbPeriod.Items.Add("Monthly")
        cbPeriod.Items.Add("Daily")
        cbPeriod.Items.Add("Date Range")
        cbPeriod.SelectedIndex = -1

        cbMonth.Items.Clear()
        cbMonth.Items.Add("January")
        cbMonth.Items.Add("February")
        cbMonth.Items.Add("March")
        cbMonth.Items.Add("April")
        cbMonth.Items.Add("May")
        cbMonth.Items.Add("June")
        cbMonth.Items.Add("July")
        cbMonth.Items.Add("August")
        cbMonth.Items.Add("September")
        cbMonth.Items.Add("October")
        cbMonth.Items.Add("November")
        cbMonth.Items.Add("December")
        lblMonth.Text = "Month :"
    End Sub

    Private Sub btnPreview_Click(sender As System.Object, e As System.EventArgs) Handles btnPreview.Click
        If cbBranch.SelectedIndex = -1 Then
            Msg("Select Branch First!", MsgBoxStyle.Exclamation)
        Else

            branch = Strings.Left(cbBranch.SelectedItem, cbBranch.SelectedItem.ToString.IndexOf(" - "))
            If cbReport.SelectedItem = "Trial Balance" Then
                GenerateTB(IIf(rbSummary.Checked = True, "Summary", "Detailed"), dtpFrom.Value.Date, dtpTo.Value.Date)
            ElseIf cbReport.SelectedItem = "Book of Accounts" Then
                Dim index As Integer = -1
                For Each item As ListViewItem In lvFilter.Items
                    If item.Checked = True Then
                        index = item.Index
                    End If
                Next
                If index <> -1 Then
                    If cbPeriod.SelectedItem = "Daily" Then
                        GenerateTB("By Book", dtpFrom.Value.Date, dtpFrom.Value.Date, lvFilter.Items(index).SubItems(0).Text)
                    ElseIf cbPeriod.SelectedItem = "Monthly" Then
                        Dim DateFrom, DateTo As Date
                        DateFrom = CDate((cbMonth.SelectedIndex + 1) & "-1-" & nupYear.Value)
                        DateTo = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate((cbMonth.SelectedIndex + 1) & "-1-" & nupYear.Value)))
                        GenerateTB("By Book", DateFrom, DateTo, lvFilter.Items(index).SubItems(0).Text)
                    ElseIf cbPeriod.SelectedItem = "Yearly" Then
                        GenerateTB("By Book", "01-01-" & nupYear.Value, "12-31-" & nupYear.Value, lvFilter.Items(index).SubItems(0).Text)
                    Else
                        GenerateTB("By Book", dtpFrom.Value.Date, dtpTo.Value.Date, lvFilter.Items(index).SubItems(0).Text)
                    End If
                Else
                    MsgBox("Please select book first!", MsgBoxStyle.Exclamation)
                End If
            ElseIf cbReport.SelectedItem = "Balance Sheet" Then
                GenerateBS()
                Dim f As New frmReport_Display
                f.ShowDialog("FSBS", UserName, dtpTo.Value.Date, cbBranch.SelectedItem)
                f.Dispose()
            ElseIf cbReport.SelectedItem = "Income Statement" Then

                GenerateIS("By Branch")
                Dim f As New frmReport_Display
                f.ShowDialog("FSIS", UserName, dtpTo.Value.Date, cbBranch.SelectedItem)
                f.Dispose()

                'ElseIf cbReport.SelectedItem = "Income Statement" Then
                '    Dim f As New frmReport_Display

                '    f.ShowDialog("FSINCS", "", dtpFrom.Value.Date, dtpTo.Value.Date)
                '    f.Dispose()
            ElseIf cbReport.SelectedItem = "Subsidiary Ledger Schedule" Then
                GenerateSL()
            ElseIf cbReport.SelectedItem = "Subsidiary Ledger Column" Then
                GenerateSLC()
            ElseIf cbReport.SelectedItem = "Insurance Premium Payable" Then
                GenerateSLC()
            ElseIf cbReport.SelectedItem = "Daily Cash Position" Then
                GenerateDCPR("Daily", dtpFrom.Value.Date, dtpFrom.Value.Date)
            ElseIf cbReport.SelectedItem = "Account Schedule" Then
                GenerateGL()
            ElseIf cbReport.SelectedItem = "Loan Aging Report" Then
                'GenerateAging(dtpFrom.Value.Date)
                Dim f As New frmReport_Display
                f.ShowDialog("LN_AGING", "", dtpFrom.Value.Date, IIf(branch = "ALL", "", branch), "")
                f.Dispose()
                'Threading.Thread.Sleep(2000)
                'Dim f As New frmReport_Display
                'f.rpt = "LN_AGING"
                'f.p1 = ""
                'f.p2 = dtpFrom.Value.Date
                'f.p3 = branch
                'f.Show()

            ElseIf cbReport.SelectedItem = "Loan Aging Report Per Loan Type" Then
                If cmbLoanType.SelectedIndex <> -1 Then
                    ' GenerateAging(dtpFrom.Value.Date)
                    Dim f As New frmReport_Display
                    f.ShowDialog("LN_AGING", "", dtpFrom.Value.Date, IIf(branch = "ALL", "", branch), cmbLoanType.Text)
                    'f.ShowDialog("LN_Aging_PerLoanType", "", dtpFrom.Value.Date, cmbLoanType.Text, branch)
                    f.Dispose()
                Else
                    MsgBox("Please select loan type!", MsgBoxStyle.Exclamation)
                End If
            ElseIf cbReport.SelectedItem = "Loan Release" Then
                Dim f As New frmReport_Display
                f.ShowDialog("LN_Release", "Released", dtpFrom.Value.Date, dtpTo.Value.Date, branch)
                f.Dispose()
                ElseIf cbReport.SelectedItem = "CV Transaction" Then
                    Dim f As New frmReport_Display
                    f.ShowDialog("CV_Transaction", dtpFrom.Value.Date, dtpTo.Value.Date)
                    f.Dispose()
                ElseIf cbReport.SelectedItem = "OR Transaction" Then
                    Dim f As New frmReport_Display
                    f.ShowDialog("OR_Transaction", dtpFrom.Value.Date, dtpTo.Value.Date)
                    f.Dispose()
                ElseIf cbReport.SelectedItem = "Check Issuance" Then
                    Dim f As New frmReport_Display
                    f.ShowDialog("Check_Issuance", dtpFrom.Value.Date, dtpTo.Value.Date)
                f.Dispose()
            ElseIf cbReport.SelectedItem = "Account Balances" Then
                Dim f As New frmReport_Display
                f.ShowDialog("Account_Balances", dtpFrom.Value.Date, dtpTo.Value.Date, cmbLoanType.Text)
                f.Dispose()
            ElseIf cbReport.SelectedItem = "Loan Balances" Then
                Dim f As New frmReport_Display
                f.ShowDialog("Loan_Balances", dtpFrom.Value.Date, cmbLoanType.Text)
                f.Dispose()
            ElseIf cbReport.SelectedItem = "Loan Title" Then
                Dim f As New frmReport_Display
                f.ShowDialog("Loan_Title", dtpFrom.Value.Date, dtpTo.Value.Date)
                f.Dispose()

            ElseIf cbReport.SelectedItem = "Member List" Then
                Dim f As New frmReport_Display
                f.ShowDialog("MemberReport", dtpFrom.Value.Date, dtpTo.Value.Date)
                f.Dispose()
            ElseIf cbReport.SelectedItem = "Chart of Account" Then
                Dim f As New frmReport_Display
                f.ShowDialog("COA_Master", Date.Today)
                f.Dispose()
            ElseIf cbReport.SelectedItem = "Loan Co-Maker List" Then
                Dim f As New frmReport_Display
                f.ShowDialog("LN_CoMakerList", branch)
                f.Dispose()
            End If
        End If


    End Sub

    Private Sub GenerateAging(ByVal DateFrom As String)
        Dim insertSQL As String
        insertSQL = " DELETE FROM tblFilter_LNAging " & _
                    " INSERT INTO tblFilter_LNAging (Loan_No) " & _
                    " SELECT Loan_No FROM tblLoan  " & _
                    " WHERE DateLoan <= '" & DateFrom & "' " & _
                    " AND Loan_No IN  " & _
                    " ( " & _
                    " SELECT REPLACE(RefNo, 'LN:', '')  " & _
                    " FROM view_GL " & _
                    " WHERE AccntCode IN (SELECT LoanAccount FROM tblLoan_Type WHERE LoanAccount <> '')  " & _
                    " AND RefNo <> '' AND AppDate <= '" & DateFrom & "'  " & IIf(branch = "ALL", "AND BranchCode in ( SELECT    DISTINCT  tblBranch.BranchCode   " & _
                    " FROM      tblBranch    " & _
                    " INNER JOIN tblUser_Access    ON   " & _
                    " tblBranch.BranchCode = tblUser_Access.Code    " & _
                    " AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                    " AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                    " WHERE     UserID ='" & UserID & "'  )", " AND BranchCode = '" & branch & "' ") & _
                    " GROUP BY REPLACE(RefNo, 'LN:', '')  " & _
                    " HAVING SUM(Debit - Credit) > 0 " & _
                    " ) "
        SQL.ExecNonQuery(insertSQL)

        'insertSQL = " DELETE tblview_LoanLedger " & _
        '    " INSERT INTO tblview_LoanLedger " & _
        '    " SELECT * FROM view_LoanLedger  " & _
        '    " WHERE Loan_NO IN (SELECT * FROM tblFilter_LNAging) "
        'SQL.ExecNonQuery(insertSQL)
    End Sub

    Private Sub GenerateIS(ByVal Type As String)
        Dim deleteSQl As String
        deleteSQl = " DELETE FROM rptBS "
        SQL.ExecNonQuery(deleteSQl)
        Dim dt As New DataTable
        Dim query As String
        Dim desc As String
        Dim AccountCode As String
        Dim groupID As Integer = 0
        Dim value As Decimal = 0
        Dim valueMonth As Decimal = 0
        Dim valueBudget As Decimal = 0
        Dim totalDesc(7) As String
        Dim totalCR(7) As Decimal
        Dim insertSQl As String
        Dim prevID As Integer = 0
        Dim recID As Integer = 1
        Dim incre As Integer = 1
        Dim Filter As String

        Filter = IIf(branch = "ALL", "AND BranchCode in ( SELECT    DISTINCT  tblBranch.BranchCode   " & _
                 " FROM      tblBranch    " & _
                 " INNER JOIN tblUser_Access    ON   " & _
                 " tblBranch.BranchCode = tblUser_Access.Code    " & _
                 " AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                 " AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                 " WHERE     UserID ='" & UserID & "'  )", " AND BranchCode = '" & branch & "' ")



        query = " SELECT  CASE WHEN AccountGroup ='SubAccount' THEN tblCOA_Master.AccountCode  + ' - ' + CASE WHEN ISNULL(ReportAlias,'') ='' THEN AccountTitle ELSE ReportAlias END " & vbCrLf & _
                " 			 ELSE CASE WHEN ReportAlias ='' THEN AccountTitle ELSE ReportAlias END " & vbCrLf & _
                " 		END  AS Descrition," & vbCrLf & _
                "  CASE	WHEN AccountGroup = 'Group' THEN 'G1'		WHEN AccountGroup = 'SubGroup' THEN 'G2' " & vbCrLf & _
                " 		WHEN AccountGroup = 'Category' THEN 'G3'	WHEN AccountGroup = 'SubCategory' THEN 'G4' " & vbCrLf & _
                " 		WHEN AccountGroup = 'CostCenter' THEN 'G5'	WHEN AccountGroup = 'MainAccount' THEN 'G6' " & vbCrLf & _
                " 		WHEN AccountGroup = 'SubAccount' THEN 'G7' " & vbCrLf & _
                "  ELSE ''	END AS AccountGroup, " & vbCrLf & _
                " 	   CASE WHEN AccountNature = 'Debit' THEN SUM(ISNULL(Debit,0) - ISNULL(Credit,0))  " & vbCrLf & _
                " 	        WHEN AccountNature = 'Credit' THEN SUM(ISNULL(Credit,0) - ISNULL(Debit,0)) " & vbCrLf & _
                " 			ELSE 0 " & vbCrLf & _
                " 	   END AS Amount," & vbCrLf & _
                "      showTotal, contraAccount, " & vbCrLf & _
                " 	   CASE WHEN AccountNature = 'Debit' THEN SUM(ISNULL(MonthDebit,0) - ISNULL(MonthCredit,0))  " & vbCrLf & _
                " 	        WHEN AccountNature = 'Credit' THEN SUM(ISNULL(MonthCredit,0) - ISNULL(MonthDebit,0)) " & vbCrLf & _
                " 			ELSE 0 " & vbCrLf & _
                " 	   END AS MonthAmount," & vbCrLf & _
                " 	   tblCOA_Master.AccountCode," & vbCrLf & _
                "      ISNULL(Budget.Amount,0) as BudgetAmount " & vbCrLf & _
                " FROM tblCOA_Master " & vbCrLf & _
                " LEFT JOIN " & vbCrLf & _
                " ( " & vbCrLf & _
                " 	SELECT    AccntCode, AccntTitle, SUM(Debit) AS Debit, SUM(Credit)  AS Credit " & vbCrLf & _
                " 	FROM      view_GL  " & vbCrLf & _
                " 	WHERE     AppDate BETWEEN CAST('01-01-" & dtpFrom.Value.Year & "' AS DATE) AND '" & dtpTo.Value.Date & "' " & Filter & vbCrLf & _
                " 	GROUP BY  AccntCode, AccntTitle " & vbCrLf & _
                " ) AS TB " & vbCrLf & _
                " ON  tblCOA_Master.AccountCode = TB.AccntCode " & vbCrLf & _
                " LEFT JOIN " & vbCrLf & _
                " ( " & vbCrLf & _
                " 	SELECT    AccntCode, AccntTitle, SUM(Debit) AS MonthDebit, SUM(Credit)  AS MonthCredit " & vbCrLf & _
                " 	FROM      view_GL  " & vbCrLf & _
                " 	WHERE     AppDate BETWEEN CAST('" & dtpTo.Value.Month & "-01-" & dtpFrom.Value.Year & "' AS DATE) AND '" & dtpTo.Value.Date & "' " & Filter & vbCrLf & _
                " 	GROUP BY  AccntCode, AccntTitle " & vbCrLf & _
                " ) AS MB " & vbCrLf & _
                " ON  tblCOA_Master.AccountCode = MB.AccntCode " & vbCrLf & _
                " LEFT JOIN  " & vbCrLf & _
                "  (  " & vbCrLf & _
                "  	SELECT    AccountCode, Amount " & vbCrLf & _
                "  	FROM      tblBudget   " & vbCrLf & _
                "  	WHERE     YEAR(AppDate) = '" & dtpFrom.Value.Year & "'  " & vbCrLf & _
                "  ) AS Budget  " & vbCrLf & _
                "  ON  tblCOA_Master.AccountCode = Budget.AccountCode " & vbCrLf & _
                " WHERE  AccountType ='Income Statement' " & vbCrLf & _
                " GROUP BY tblCOA_Master.AccountCode, AccountTitle, ReportAlias, AccountGroup, AccountNature, showTotal, OrderNo, contraAccount, Budget.Amount " & vbCrLf & _
                " HAVING  (AccountGroup <> 'SubAccount' OR " & vbCrLf & _
                "        (AccountGroup = 'SubAccount'  AND " & vbCrLf & _
                "        (CASE WHEN AccountNature = 'Debit' THEN SUM(ISNULL(Debit,0) - ISNULL(Credit,0))  " & vbCrLf & _
                " 	        WHEN AccountNature = 'Credit' THEN SUM(ISNULL(Credit,0) - ISNULL(Debit,0)) " & vbCrLf & _
                " 			ELSE 0 " & vbCrLf & _
                " 	   END) <> 0)) " & vbCrLf & _
                " ORDER BY OrderNo "
        SQL.GetQuery(query)
        If SQL.SQLDS.Tables(0).Rows.Count > 0 Then
            For Each row As DataRow In SQL.SQLDS.Tables(0).Rows
                desc = row(0).ToString
                groupID = CInt(row(1).ToString.Replace("G", ""))
                value = row(2)
                valueMonth = row(5)
                If row(3) = True Then
                    totalDesc(groupID) = desc
                End If
                If row(4) = True Then
                    value = value * -1
                    valueMonth = valueMonth * -1
                End If
                AccountCode = row(6).ToString
                valueBudget = row(7)
                If groupID <> prevID Or groupID = 7 Then

                    If prevID > groupID Then
                        If prevID <> 7 Then
                            For i As Integer = incre - 1 To 0 Step -1
                                deleteSQl = " DELETE FROM rptBS WHERE RecordID = '" & recID - 1 & "' "
                                SQL.ExecNonQuery(deleteSQl)
                                recID -= 1
                            Next

                            incre = 0
                        Else
                            incre = 0
                        End If
                        For i As Integer = 7 To 1 Step -1
                            If groupID <= i Then
                                If Not IsNothing(totalDesc(i)) AndAlso totalDesc(i) <> "" Then
                                    If totalCR(i) <> 0 Then
                                        insertSQl = " INSERT INTO " & _
                                               " rptBS(RecordID, Description, Amount, AmountMonth, GroupID, AccountCode, AmountBudget) " & _
                                               " VALUES(@RecordID, @Description, @Amount, @AmountMonth, @GroupID, @AccountCode, @AmountBudget)"
                                        SQL.FlushParams()
                                        SQL.AddParam("@RecordID", recID)
                                        SQL.AddParam("@Description", "TOTAL " & totalDesc(i))
                                        SQL.AddParam("@Amount", totalCR(i))
                                        SQL.AddParam("@AmountMonth", totalCR(i))
                                        SQL.AddParam("@GroupID", i)
                                        SQL.AddParam("@AccountCode", totalCR(i))
                                        SQL.AddParam("@AmountBudget", totalCR(i))
                                        SQL.ExecNonQuery(insertSQl)
                                        incre = 0
                                        totalDesc(i) = Nothing
                                        totalCR(i) = Nothing
                                        recID += 1
                                    End If

                                End If
                            End If

                        Next
                    End If
                    insertSQl = " INSERT INTO " & _
                        " rptBS(RecordID, Description, Amount, AmountMonth, GroupID, AccountCode, AmountBudget) " & _
                        " VALUES(@RecordID, @Description, @Amount, @AmountMonth, @GroupID, @AccountCode, @AmountBudget)"
                    SQL.FlushParams()
                    SQL.AddParam("@RecordID", recID)
                    SQL.AddParam("@Description", desc)
                    SQL.AddParam("@Amount", value)
                    SQL.AddParam("@AmountMonth", valueMonth)
                    SQL.AddParam("@GroupID", groupID)
                    SQL.AddParam("@AccountCode", AccountCode)
                    SQL.AddParam("@AmountBudget", valueBudget)
                    SQL.ExecNonQuery(insertSQl)
                    prevID = groupID
                    recID += 1
                    incre += 1
                    If value <> 0 Then
                        For i As Integer = 1 To 7
                            If Not IsNothing(totalDesc(i)) AndAlso totalDesc(i) <> "" Then
                                totalCR(i) += value
                            End If
                        Next
                    End If

                End If
            Next
            If prevID <> 7 Then
                For i As Integer = incre - 1 To 0 Step -1
                    deleteSQl = " DELETE FROM rptBS WHERE RecordID = '" & recID - 1 & "' "
                    SQL.ExecNonQuery(deleteSQl)
                    recID -= 1
                Next
            End If
            For i As Integer = 7 To 1 Step -1
                If Not IsNothing(totalDesc(i)) AndAlso totalDesc(i) <> "" Then
                    If totalCR(i) <> 0 Then
                        insertSQl = " INSERT INTO " & _
                               " rptBS(RecordID, Description, Amount, GroupID, AccountCode, AmountBudget) " & _
                               " VALUES(@RecordID, @Description, @Amount, @GroupID, @AccountCode, @AmountBudget)"
                        SQL.FlushParams()
                        SQL.AddParam("@RecordID", recID)
                        SQL.AddParam("@Description", "TOTAL " & totalDesc(i))
                        SQL.AddParam("@Amount", totalCR(i))
                        SQL.AddParam("@GroupID", i)
                        SQL.AddParam("@AccountCode", totalCR(i))
                        SQL.AddParam("@AmountBudget", totalCR(i))
                        SQL.ExecNonQuery(insertSQl)
                        incre = 0
                        totalDesc(i) = Nothing
                        totalCR(i) = Nothing
                        recID += 1
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub GenerateISbyBranch()
        Dim deleteSQl As String
        deleteSQl = " DELETE FROM rptIS_Header "
        SQL.ExecNonQuery(deleteSQl)


        Dim Code As String
        Dim desc As String
        Dim nature As String
        Dim groupID As Integer = 0
        Dim incre As Integer = 1
        Dim prevID As Integer = 0
        Dim prevTotalLabel As String
        Dim prevNature As String
        Dim recID As Integer = 1
        Dim showTitle As Boolean = True
        Dim sortID As Integer = 0
        Dim totalCR(7) As Decimal
        Dim totalDB(7) As Decimal
        Dim totalDBCR(7) As String
        Dim totalDesc(7) As String
        Dim totalCode(7) As String
        Dim value As Decimal = 0
        Dim valueCR As Decimal = 0
        Dim valueDB As Decimal = 0
        Dim dt As DataTable

        Dim insertSQL As String
        insertSQL = "  INSERT INTO  " & _
                "  rptIS_Header(Type, Description, Param1) " & _
                "  SELECT 'G' +  CAST(ROW_NUMBER() OVER (ORDER BY SortID) AS nvarchar) AS Type, Description, BranchCode " & _
                "  FROM   tblBranch " & _
                "  WHERE  BranchCode IN  " & _
                "  (  " & _
                "  	SELECT    BranchCode  " & _
                "  	FROM      view_GL   " & _
                " 	WHERE     AppDate BETWEEN '" & dtpFrom.Value.Date & "' AND '" & dtpTo.Value.Date & "' " & _
                "  )  "
        SQL.ExecNonQuery(insertSQL)

        Dim query As String
        query = " SELECT Type, Description, Param1 FROM rptIS_Header "
        SQL.GetQuery(query)
        dt = SQL.SQLDS.Tables(0)
        If dt.Rows.Count > 0 Then
            For Each row1 As DataRow In dt.Rows
                prevID = 0
                incre = 1
                groupID = 0
                recID = 1
                sortID = 1
                query = " SELECT  CASE WHEN AccountGroup ='SubAccount' THEN AccountCode  + ' - ' + CASE WHEN ISNULL(ReportAlias,'') ='' THEN AccountTitle ELSE ReportAlias END " & _
                         " 			 ELSE CASE WHEN ReportAlias ='' THEN AccountTitle ELSE ReportAlias END " & _
                         " 		END  AS Descrition,  " & _
                         "  CASE	WHEN AccountGroup = 'Group' THEN 'G1'		WHEN AccountGroup = 'SubGroup' THEN 'G2' " & _
                         " 		WHEN AccountGroup = 'Category' THEN 'G3'	WHEN AccountGroup = 'SubCategory' THEN 'G4' " & _
                         " 		WHEN AccountGroup = 'CostCenter' THEN 'G5'	WHEN AccountGroup = 'MainAccount' THEN 'G6' " & _
                         " 		WHEN AccountGroup = 'SubAccount' THEN 'G7' " & _
                         "  ELSE ''	END AS AccountGroup, " & _
                         " 	   CASE WHEN AccountNature = 'Debit' THEN SUM(ISNULL(Debit,0) - ISNULL(Credit,0))  " & _
                         " 	        WHEN AccountNature = 'Credit' THEN SUM(ISNULL(Credit,0) - ISNULL(Debit,0)) " & _
                         " 			ELSE 0 " & _
                         " 	   END AS Amount, showTotal, contraAccount, " & _
                         "       SUM(ISNULL(Debit,0) - ISNULL(Credit,0)) AS TotalDebit, " & _
                         "       SUM(ISNULL(Credit,0) - ISNULL(Debit,0)) AS TotalCredit, " & _
                         "       AccountNature, showTitle, ISNULL(OrderNo,0) AS OrderNo, AccountCode " & _
                         " FROM tblCOA_Master " & _
                         " LEFT JOIN " & _
                         " ( " & _
                         " 	SELECT    AccntCode, AccntTitle, SUM(Debit) AS Debit, SUM(Credit)  AS Credit " & _
                         " 	FROM      view_GL  " & _
                         " 	WHERE     AppDate BETWEEN'" & dtpFrom.Value.Date & "' AND '" & dtpTo.Value.Date & "' " & _
                         "  AND       BranchCode = '" & row1(2).ToString & "' " & _
                         " 	GROUP BY  AccntCode, AccntTitle " & _
                         " ) AS TB " & _
                         " ON  tblCOA_Master.AccountCode = TB.AccntCode " & _
                         " WHERE  AccountType ='Income Statement' " & _
                         " GROUP BY AccountCode, AccountTitle, ReportAlias, AccountGroup, AccountNature, showTotal, OrderNo, contraAccount, showTitle " & _
                         " HAVING  (AccountGroup <> 'SubAccount' OR " & _
                         "        (AccountGroup = 'SubAccount'  AND " & _
                         "        (CASE WHEN AccountNature = 'Debit' THEN SUM(ISNULL(Debit,0) - ISNULL(Credit,0))  " & _
                         " 	        WHEN AccountNature = 'Credit' THEN SUM(ISNULL(Credit,0) - ISNULL(Debit,0)) " & _
                         " 			ELSE 0 " & _
                         " 	   END) <> 0)) " & _
                         " ORDER BY OrderNo "
                SQL.GetQuery(query)
                If SQL.SQLDS.Tables(0).Rows.Count > 0 Then
                    For Each row As DataRow In SQL.SQLDS.Tables(0).Rows

                        desc = row(0).ToString
                        groupID = CInt(row(1).ToString.Replace("G", ""))
                        value = row(2)
                        valueDB = row(5)
                        valueCR = row(6)
                        showTitle = row(8)
                        sortID = row(9)
                        nature = row(7)
                        Code = row(10)

                        If groupID <> prevID Or groupID = 7 Then
                            If prevID > groupID Then
                                If prevID <> 7 Then
                                    For i As Integer = incre - 1 To 0 Step -1
                                        recID -= 1
                                    Next

                                    incre = 0
                                    For i As Integer = groupID + 1 To 7
                                        totalDesc(i) = Nothing
                                    Next
                                Else
                                    incre = 0
                                End If

                                For i As Integer = 6 To 1 Step -1
                                    If groupID <= i Then
                                        If Not IsNothing(totalDesc(i)) AndAlso totalDesc(i) <> "" Then
                                            If totalCR(i) <> 0 Then

                                                insertSQL = " UPDATE rptIS  " & _
                                                              " SET    " & row1(0).ToString & " = @Amount " & _
                                                              " WHERE  RecordID = @RecordID AND Description = @Description AND GroupID = @GroupID "
                                                SQL.FlushParams()
                                                SQL.AddParam("@Description", "TOTAL " & totalDesc(i))
                                                If totalDBCR(i) = "Credit" Then
                                                    SQL.AddParam("@Amount", totalCR(i))
                                                Else
                                                    SQL.AddParam("@Amount", totalDB(i))
                                                End If
                                                SQL.AddParam("@GroupID", i)
                                                SQL.AddParam("@RecordID", totalCode(i))
                                                SQL.ExecNonQuery(insertSQL)
                                                incre = 0
                                                totalDesc(i) = Nothing
                                                totalCR(i) = Nothing
                                                totalDB(i) = Nothing
                                                totalDBCR(i) = Nothing
                                                totalCode(i) = Nothing
                                                If i = groupID Then
                                                    totalDesc(i) = desc
                                                    totalCode(i) = Code
                                                End If
                                                recID += 1

                                                'If groupID = i Then
                                                '    insertSQL = " UPDATE rptIS " & _
                                                '                " SET    " & row1(0).ToString & " = @Amount " & _
                                                '                " WHERE  SortID = @SortID AND Description = @Description AND GroupID = @GroupID "
                                                '    SQL.FlushParams()
                                                '    If IsNothing(prevTotalLabel) Then SQL.AddParam("@Description", "TOTAL " & totalDesc(i)) Else SQL.AddParam("@Description", "TOTAL " & prevTotalLabel)
                                                '    If totalDBCR(i) = "Credit" Then
                                                '        SQL.AddParam("@Amount", totalCR(i))
                                                '    Else
                                                '        SQL.AddParam("@Amount", totalDB(i))
                                                '    End If
                                                '    SQL.AddParam("@GroupID", i)
                                                '    SQL.AddParam("@SortID", sortID)
                                                '    SQL.ExecNonQuery(insertSQL)
                                                '    incre = 0
                                                '    If IsNothing(prevTotalLabel) Then totalDesc(i) = Nothing Else prevTotalLabel = Nothing
                                                '    totalCR(i) = Nothing
                                                '    totalDB(i) = Nothing
                                                '    totalDBCR(i) = Nothing
                                                '    recID += 1
                                                'Else
                                                '    insertSQL = " UPDATE rptIS  " & _
                                                '                " SET    " & row1(0).ToString & " = @Amount " & _
                                                '                " WHERE  SortID = @SortID AND Description = @Description AND GroupID = @GroupID "
                                                '    SQL.FlushParams()
                                                '    SQL.AddParam("@Description", "TOTAL " & totalDesc(i))
                                                '    If totalDBCR(i) = "Credit" Then
                                                '        SQL.AddParam("@Amount", totalCR(i))
                                                '    Else
                                                '        SQL.AddParam("@Amount", totalDB(i))
                                                '    End If
                                                '    SQL.AddParam("@GroupID", i)
                                                '    SQL.AddParam("@SortID", sortID)
                                                '    SQL.ExecNonQuery(insertSQL)
                                                '    incre = 0
                                                '    totalDesc(i) = Nothing
                                                '    totalCR(i) = Nothing
                                                '    totalDB(i) = Nothing
                                                '    totalDBCR(i) = Nothing
                                                '    recID += 1
                                                'End If

                                            End If

                                        End If
                                    End If

                                Next
                            Else
                                If row(3) = True Then
                                    If Not IsNothing(totalDesc(groupID)) AndAlso totalDesc(groupID) <> desc Then
                                        prevTotalLabel = totalDesc(groupID)
                                    End If
                                    totalDesc(groupID) = desc
                                    totalCode(groupID) = Code
                                    If Not IsNothing(totalDBCR(groupID)) AndAlso totalDBCR(groupID) <> nature Then
                                        prevNature = totalDBCR(groupID)
                                    End If
                                    totalDBCR(groupID) = nature
                                End If
                            End If
                        End If
                        If showTitle = True Then
                            If groupID = "7" And value <> 0 Then
                                insertSQL = " UPDATE rptIS  " & _
                                       " SET    " & row1(0).ToString & " = @Amount " & _
                                       " WHERE  RecordID = @RecordID AND Description = @Description AND GroupID = @GroupID "
                                SQL.FlushParams()
                                SQL.AddParam("@Description", desc)
                                SQL.AddParam("@Amount", value)
                                SQL.AddParam("@GroupID", groupID)
                                SQL.AddParam("@RecordID", Code)
                                SQL.ExecNonQuery(insertSQL)
                            End If

                        End If

                        prevID = groupID
                        recID += 1
                        incre += 1
                        If value <> 0 Then
                            For i As Integer = 1 To 7
                                If Not IsNothing(totalDesc(i)) AndAlso totalDesc(i) <> "" Then
                                    totalCR(i) += valueCR
                                    totalDB(i) += valueDB

                                    'If IsNothing(totalDBCR(i)) Then
                                    '    totalDBCR(i) = row(7)
                                    'End If
                                End If
                            Next
                        End If

                    Next
                    If prevID <> 7 Then
                        For i As Integer = incre - 1 To 0 Step -1

                            recID -= 1
                        Next
                    End If

                    For i As Integer = 6 To 1 Step -1
                        If Not IsNothing(totalDesc(i)) AndAlso totalDesc(i) <> "" Then
                            If totalCR(i) <> 0 Then
                                insertSQL = " UPDATE rptIS  " & _
                                            " SET    " & row1(0).ToString & " = @Amount " & _
                                            " WHERE  RecordID = @RecordID AND Description = @Description AND GroupID = @GroupID "
                                SQL.FlushParams()
                                SQL.AddParam("@RecordID", totalCode(i))
                                SQL.AddParam("@Description", "TOTAL " & totalDesc(i))
                                If totalDBCR(i) = "Credit" Then
                                    SQL.AddParam("@Amount", totalCR(i))
                                Else
                                    SQL.AddParam("@Amount", totalDB(i))
                                End If
                                SQL.AddParam("@GroupID", i)
                                SQL.ExecNonQuery(insertSQL)
                                incre = 0
                                totalDesc(i) = Nothing
                                totalCR(i) = Nothing
                                totalDB(i) = Nothing
                                totalDBCR(i) = Nothing
                                recID += 1
                            End If
                        End If
                    Next
                End If

            Next
        End If
    End Sub


    Private Sub GenerateSL()
        Dim i As Integer = 1
        Dim insertSQl As String
        Dim deleteSQL As String
        deleteSQL = " DELETE FROM tblSL_PrintH  "
        SQL.ExecNonQuery(deleteSQL)
        deleteSQL = " DELETE FROM tblSL_Print "
        SQL.ExecNonQuery(deleteSQL)
        insertSQl = " INSERT INTO tblSL_Print(VCECode, RefID) " & _
                    " SELECT VCECode,  REPLACE(RefNo, 'LN:', '') as RefNo" & _
                    " FROM   view_GL INNER JOIN tblCOA_Master " & _
                    " ON     view_GL.AccntCode = tblCOA_Master.AccountCode " & _
                    " WHERE  AppDate BETWEEN CAST('01-01-" & dtpFrom.Value.Year & "' AS DATE) AND '" & dtpTo.Value.Date & "'  " & _
                    " AND    WithSubsidiary = 1 AND VCECode <> '' " & IIf(branch = "ALL", "AND BranchCode in ( SELECT    DISTINCT  tblBranch.BranchCode   " & _
                    " FROM      tblBranch    " & _
                    " INNER JOIN tblUser_Access    ON   " & _
                    " tblBranch.BranchCode = tblUser_Access.Code    " & _
                    " AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                    " AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                    " WHERE     UserID ='" & UserID & "'  )", " AND BranchCode = '" & branch & "' ") & _
                    " GROUP BY VCECode,  REPLACE(RefNo, 'LN:', '')" & _
                    " ORDER BY VCECode "
        SQL.ExecNonQuery(insertSQl)
        For Each item As ListViewItem In lvFilter.Items
            If item.Checked = True And i < 10 Then
                insertSQl = " INSERT INTO " & _
                            " tblSL_PrintH(Type, Description) " & _
                            " VALUES ('C" & i & "',@Description)"
                SQL.FlushParams()
                SQL.AddParam("@Description", item.SubItems(0).Text)
                SQL.ExecNonQuery(insertSQl)
                Dim updateSQL As String
                updateSQL = " UPDATE tblSL_Print " & _
                            " SET   Type = @Description, C" & i & " = ISNULL(Balance,0) " & _
                            " FROM  " & _
                            " ( " & _
                            "      SELECT VCECode, REPLACE(RefNo, 'LN:', '') AS RefNo, SUM(ISNULL(CASE WHEN AccountNature ='Debit' " & _
                            " 		     THEN Debit -Credit                           " & _
                            " 			ELSE Credit - Debit  " & _
                            " 	   END,0)) AS Balance " & _
                            " FROM view_GL INNER JOIN tblCOA_Master " & _
                            " ON view_GL.AccntCode = tblCOA_Master.AccountCode " & _
                            " WHERE AppDate BETWEEN CAST('01-01-" & dtpFrom.Value.Year & "' AS DATE) AND '" & dtpTo.Value.Date & "' " & _
                            " AND WithSubsidiary = 1 AND tblCOA_Master.AccountTitle = @Description AND AccountNature IN ('Debit','Credit') " & IIf(branch = "ALL", "AND BranchCode in ( SELECT    DISTINCT  tblBranch.BranchCode   " & _
                            " FROM      tblBranch    " & _
                            " INNER JOIN tblUser_Access    ON   " & _
                            " tblBranch.BranchCode = tblUser_Access.Code    " & _
                            " AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                            " AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                            " WHERE     UserID ='" & UserID & "'  )", " AND BranchCode = '" & branch & "' ") & _
                            " GROUP BY VCECode, REPLACE(RefNo, 'LN:', '') " & _
                            " ) AS A " & _
                            " WHERE  tblSL_Print.VCECode = A.VCECode AND tblSL_Print.RefID = A.RefNo"
                SQL.FlushParams()
                SQL.AddParam("@Description", item.SubItems(0).Text)
                SQL.ExecNonQuery(updateSQL)
                i += 1
            End If
        Next

        Dim f As New frmReport_Display
        f.ShowDialog("SUBLGRS", UserName, dtpFrom.Value.Date, dtpTo.Value.Date)
        f.Dispose()
    End Sub

    Private Sub GenerateSLC()
        Dim i As Integer = 1
        Dim insertSQl As String
        Dim deleteSQL As String
        deleteSQL = " DELETE FROM tblSL_PrintH  "
        SQL.ExecNonQuery(deleteSQL)
        deleteSQL = " DELETE FROM tblSL_Print "
        SQL.ExecNonQuery(deleteSQL)
        insertSQl = " INSERT INTO tblSL_Print(VCECode) " & _
                    " SELECT DISTINCT VCECode " & _
                    " FROM   view_GL INNER JOIN tblCOA_Master " & _
                    " ON     view_GL.AccntCode = tblCOA_Master.AccountCode " & _
                    " WHERE  AppDate BETWEEN CAST('01-01-" & dtpFrom.Value.Year & "' AS DATE) AND '" & dtpTo.Value.Date & "'  " & _
                    " AND    WithSubsidiary = 1 " & IIf(branch = "ALL", "AND BranchCode in ( SELECT    DISTINCT  tblBranch.BranchCode   " & _
                    " FROM      tblBranch    " & _
                    " INNER JOIN tblUser_Access    ON   " & _
                    " tblBranch.BranchCode = tblUser_Access.Code    " & _
                    " AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                    " AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                    " WHERE     UserID ='" & UserID & "'  )", " AND BranchCode = '" & branch & "' ") & _
                    " ORDER BY VCECode "
        SQL.ExecNonQuery(insertSQl)
        For Each item As ListViewItem In lvFilter.Items
            If item.Checked = True And i < 10 Then
                insertSQl = " INSERT INTO " & _
                            " tblSL_PrintH(Type, Description) " & _
                            " VALUES ('C" & i & "',@Description)"
                SQL.FlushParams()
                SQL.AddParam("@Description", item.SubItems(0).Text)
                SQL.ExecNonQuery(insertSQl)
                Dim updateSQL As String
                updateSQL = " UPDATE tblSL_Print " & _
                            " SET    C" & i & " = ISNULL(Balance,0) " & _
                            " FROM  " & _
                            " ( " & _
                            "      SELECT VCECode, SUM(ISNULL(CASE WHEN AccountNature ='Debit' " & _
                            " 		     THEN Debit -Credit                           " & _
                            " 			ELSE Credit - Debit  " & _
                            " 	   END,0)) AS Balance " & _
                            " FROM view_GL INNER JOIN tblCOA_Master " & _
                            " ON view_GL.AccntCode = tblCOA_Master.AccountCode " & _
                            " WHERE AppDate BETWEEN CAST('01-01-" & dtpFrom.Value.Year & "' AS DATE) AND '" & dtpTo.Value.Date & "' " & IIf(branch = "ALL", "AND BranchCode in ( SELECT    DISTINCT  tblBranch.BranchCode   " & _
                            " FROM      tblBranch    " & _
                            " INNER JOIN tblUser_Access    ON   " & _
                            " tblBranch.BranchCode = tblUser_Access.Code    " & _
                            " AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                            " AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                            " WHERE     UserID ='" & UserID & "'  )", " AND BranchCode = '" & branch & "' ") & _
                            " AND WithSubsidiary = 1 AND tblCOA_Master.AccountTitle = @Description AND AccountNature IN ('Debit','Credit') " & _
                            " GROUP BY VCECode " & _
                            " ) AS A " & _
                            " WHERE  tblSL_Print.VCECode = A.VCECode "
                SQL.FlushParams()
                SQL.AddParam("@Description", item.SubItems(0).Text)
                SQL.ExecNonQuery(updateSQL)
                i += 1
            End If
        Next

        Dim f As New frmReport_Display
        If cbReport.SelectedItem = "Subsidiary Ledger Column" Then
            f.ShowDialog("SUBLGRSC", UserName, dtpFrom.Value.Date, dtpTo.Value.Date)
            f.Dispose()
        Else
            f.ShowDialog("SUBLGRS_Insu", UserName, dtpFrom.Value.Date, dtpTo.Value.Date)
            f.Dispose()
        End If
    End Sub

    Private Sub LoadAccounts()
        Dim query As String
        query = " SELECT DISTINCT AccountTitle FROM tblCOA_Master WHERE AccountGroup = 'SubAccount'  ORDER BY AccountTitle "
        SQL.ReadQuery(query)
        lvFilter.Items.Clear()
        While SQL.SQLDR.Read
            lvFilter.Items.Add(SQL.SQLDR("AccountTitle").ToString)
        End While
    End Sub

    Private Sub LoadMembType()
        lvFilter.Items.Clear()
        lvFilter.Items.Add("All")
        lvFilter.Items.Add("Clients")
        lvFilter.Items.Add("Partners")
        lvFilter.Items.Add("Suppliers")
        lvFilter.Items.Add("Employees")
        lvFilter.Items.Add("Others")
    End Sub

    Private Sub LoadLoanType()
        Dim query As String
        query = " SELECT LoanType FROM tblLoan_Type WHERE Status = 'Active'  ORDER BY LoanType "
        SQL.ReadQuery(query)
        cmbLoanType.Items.Clear()
        While SQL.SQLDR.Read
            cmbLoanType.Items.Add(SQL.SQLDR("LoanType").ToString)
        End While
    End Sub

    Private Sub LoadSavingsType()
        Dim query As String
        query = " SELECT DISTINCT DepositType FROM tblSavings_Account  ORDER BY DepositType "
        SQL.ReadQuery(query)
        cmbLoanType.Items.Clear()
        While SQL.SQLDR.Read
            cmbLoanType.Items.Add(SQL.SQLDR("DepositType").ToString)
        End While
    End Sub

    Private Sub LoadLoanAccount()
        Dim query As String
        query = " SELECT  DISTINCT LoanAccount, AccountTitle from tblLoan_Type " & _
                " INNER JOIN " & _
                " tblCOA_Master ON " & _
                " tblCOA_Master.AccountCode = tblLoan_Type.LoanAccount " & _
                " WHERE tblLoan_Type.Status = 'Active' " & _
                " ORDER BY AccountTitle"
        SQL.ReadQuery(query)
        cmbLoanType.Items.Clear()
        While SQL.SQLDR.Read
            cmbLoanType.Items.Add(SQL.SQLDR("AccountTitle").ToString)
        End While
    End Sub

    Private Sub GenerateGL()
        Dim i As Integer = 1
        Dim insertSQl As String
        Dim deleteSQL As String
        deleteSQL = " DELETE FROM tblPrint_TB  "
        SQL.ExecNonQuery(deleteSQL)
        For Each item As ListViewItem In lvFilter.Items
            If item.Checked = True Then
                insertSQl = " INSERT INTO tblPrint_TB(Code) " & _
                            " VALUES (@Code) "
                SQL.FlushParams()
                SQL.AddParam("@Code", GetAccntCode(item.SubItems(0).Text))
                SQL.ExecNonQuery(insertSQl)
            End If
        Next
        If rbSummary.Checked = True Then
            If cbPeriod.SelectedItem = "Yearly" Then
                Dim f As New frmReport_Display
                f.ShowDialog("GENLGRYS", "", nupYear.Value)
                f.Dispose()
            ElseIf cbPeriod.SelectedItem = "Monthly" Then
                Dim f As New frmReport_Display
                f.ShowDialog("GENLGRMS", "", nupYear.Value, cbMonth.SelectedIndex + 1)
                f.Dispose()
            ElseIf cbPeriod.SelectedItem = "Daily" Then
                Dim f As New frmReport_Display
                f.ShowDialog("GENLGRDS", "", dtpFrom.Value.Date)
                f.Dispose()
            ElseIf cbPeriod.SelectedItem = "Date Range" Then
                Dim f As New frmReport_Display
                f.ShowDialog("GENLGRRS", "", dtpFrom.Value.Date, dtpTo.Value.Date)
                f.Dispose()
            End If
        Else
            If cbPeriod.SelectedItem = "Yearly" Then
                Dim f As New frmReport_Display
                f.ShowDialog("GENLGRYD", "", nupYear.Value)
                f.Dispose()
            ElseIf cbPeriod.SelectedItem = "Monthly" Then
                Dim f As New frmReport_Display
                f.ShowDialog("GENLGRMD", "", nupYear.Value, cbMonth.SelectedIndex + 1)
                f.Dispose()
            ElseIf cbPeriod.SelectedItem = "Daily" Then
                Dim f As New frmReport_Display
                f.ShowDialog("GENLGRDD", "", dtpFrom.Value.Date)
                f.Dispose()
            ElseIf cbPeriod.SelectedItem = "Date Range" Then
                Dim f As New frmReport_Display
                f.ShowDialog("GENLGRRD", "", dtpFrom.Value.Date, dtpTo.Value.Date)
                f.Dispose()
            End If
        End If
    End Sub

    Private Sub GenerateTB(ByVal Type As String, ByVal DateFrom As Date, ByVal DateTo As Date, Optional ByVal Filter As String = "")
        Dim insertSQL, deleteSQL As String
        deleteSQL = " DELETE FROM tblPRint_TB "
        SQL.ExecNonQuery(deleteSQL)
        If Type = "Detailed" Then
            insertSQL = " INSERT INTO tblPRint_TB(Code, Title, BBDR, BBCR, CRDR, CRCR, CDDR, CDCR, SBDR, SBCR, PBDR, PBCR, JVDR, JVCR, IBDR, IBCR, TBDR, TBCR) " & _
                    " SELECT  AccountCode, AccountTitle,  " & _
                    " 		  CASE WHEN SUM(BBDR) > SUM(BBCR) THEN SUM(BBDR) - SUM(BBCR) ELSE 0 END AS BBDR, " & _
                    " 		  CASE WHEN SUM(BBCR) > SUM(BBDR) THEN SUM(BBCR) - SUM(BBDR) ELSE 0 END AS BBDR, " & _
                    " 		  SUM(CRDR) AS CRDR, " & _
                    " 		  SUM(CRCR) AS CRCR, " & _
                    " 		  SUM(CDDR) AS CDDR, " & _
                    " 		  SUM(CDCR) AS CDCR, " & _
                    " 		  SUM(SBDR) AS SBDR, " & _
                    " 		  SUM(SBCR) AS SBCR, " & _
                    " 		  SUM(PBDR) AS PBDR, " & _
                    " 		  SUM(PBCR) AS PBCR, " & _
                    " 		  SUM(JVDR) AS JVDR, " & _
                    " 		  SUM(JVCR) AS JVCR, " & _
                    " 		  SUM(IBDR) AS IBDR, " & _
                    " 		  SUM(IBCR) AS IBCR, " & _
                    " 		  CASE WHEN SUM(TBDR) > SUM(TBCR) THEN SUM(TBDR) - SUM(TBCR) ELSE 0 END AS TBDR, " & _
                    " 		  CASE WHEN SUM(TBCR) > SUM(TBDR) THEN SUM(TBCR) - SUM(TBDR) ELSE 0 END AS TBCR " & _
                    " FROM " & _
                    " ( " & _
                    " 	SELECT tblCOA_Master.AccountCode, tblCOA_Master.AccountTitle,  " & _
                    " 		   CASE WHEN AppDate <'" & DateFrom & "' OR Book ='BB' THEN Debit ELSE 0 END AS BBDR, " & _
                    " 		   CASE WHEN AppDate < '" & DateFrom & "' OR Book ='BB' THEN Credit ELSE 0 END AS BBCR, " & _
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Receipts' THEN Debit ELSE 0 END AS CRDR, " & _
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Receipts' THEN Credit ELSE 0 END AS CRCR, " & _
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Disbursements' THEN Debit ELSE 0 END AS CDDR, " & _
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Disbursements' THEN Credit ELSE 0 END AS CDCR, " & _
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Sales' THEN Debit ELSE 0 END AS SBDR, " & _
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Sales' THEN Credit ELSE 0 END AS SBCR, " & _
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Purchase Journal' THEN Debit ELSE 0 END AS PBDR, " & _
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Purchase Journal' THEN Credit ELSE 0 END AS PBCR, " & _
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='General Journal' THEN Debit ELSE 0 END AS JVDR, " & _
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='General Journal' THEN Credit ELSE 0 END AS JVCR, " & _
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Inventory' THEN Debit ELSE 0 END AS IBDR, " & _
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Inventory' THEN Credit ELSE 0 END AS IBCR, " & _
                    " 		   Debit AS TBDR, " & _
                    " 		   Credit AS TBCR, view_GL.Status   " & _
                    " 	FROM view_GL INNER JOIN tblCOA_Master " & _
                    " 	ON view_GL.AccntCode = tblCOA_Master.AccountCode " & _
                    " 	WHERE AppDate BETWEEN '01-01-" & DateFrom.Year & "' AND '" & DateTo & "' " & IIf(branch = "ALL", "AND BranchCode in ( SELECT    DISTINCT  tblBranch.BranchCode   " & _
                    " FROM      tblBranch    " & _
                    " INNER JOIN tblUser_Access    ON   " & _
                    " tblBranch.BranchCode = tblUser_Access.Code    " & _
                    " AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                    " AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                    " WHERE     UserID ='" & UserID & "'  )", " AND BranchCode = '" & branch & "' ") & _
                    " ) AS A " & _
                    " GROUP BY AccountCode, AccountTitle "

            '" WHERE A.Status ='Saved'  " & _
            SQL.ExecNonQuery(insertSQL)
            Dim f As New frmReport_Display
            f.ShowDialog("FS_TB_Detailed", "", DateTo, branch)
            f.Dispose()
        ElseIf Type = "Summary" Then
            insertSQL = " INSERT INTO tblPRint_TB(Code, Title, BBDR, BBCR, CRDR, CRCR, TBDR, TBCR) " & _
                 " SELECT  AccountCode, AccountTitle,  " & _
                 " 		  CASE WHEN SUM(BBDR) > SUM(BBCR) THEN SUM(BBDR) - SUM(BBCR) ELSE 0 END AS BBDR, " & _
                 " 		  CASE WHEN SUM(BBCR) > SUM(BBDR) THEN SUM(BBCR) - SUM(BBDR) ELSE 0 END AS BBDR, " & _
                 " 		  CASE WHEN SUM(CRDR + CDDR + JVDR + PBDR + SBDR + IBDR) > SUM(CRCR + CDCR + JVCR + PBCR + SBCR + IBCR) THEN SUM(CRDR + CDDR + JVDR + PBDR + SBDR + IBDR) - SUM(CRCR + CDCR + JVCR + PBCR + SBCR + IBCR) ELSE 0 END AS CRDR, " & _
                 " 		  CASE WHEN SUM(CRCR + CDCR + JVCR + PBCR + SBCR + IBCR) > SUM(CRDR + CDDR + JVDR + PBDR + SBDR + IBDR) THEN SUM(CRCR + CDCR + JVCR + PBCR + SBCR + IBCR) - SUM(CRDR + CDDR + JVDR + PBDR + SBDR + IBDR) ELSE 0 END AS CRCR, " & _
                 " 		  CASE WHEN SUM(TBDR) > SUM(TBCR) THEN SUM(TBDR) - SUM(TBCR) ELSE 0 END AS TBDR, " & _
                 " 		  CASE WHEN SUM(TBCR) > SUM(TBDR) THEN SUM(TBCR) - SUM(TBDR) ELSE 0 END AS TBCR " & _
                 " FROM " & _
                 " ( " & _
                 " 	SELECT tblCOA_Master.AccountCode, tblCOA_Master.AccountTitle,  " & _
                 " 		   CASE WHEN AppDate <'" & DateFrom & "' OR Book ='BB' THEN Debit ELSE 0 END AS BBDR, " & _
                 " 		   CASE WHEN AppDate < '" & DateFrom & "' OR Book ='BB' THEN Credit ELSE 0 END AS BBCR, " & _
                 " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Receipts' THEN Debit ELSE 0 END AS CRDR, " & _
                 " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Receipts' THEN Credit ELSE 0 END AS CRCR, " & _
                 " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Disbursements' THEN Debit ELSE 0 END AS CDDR, " & _
                 " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Disbursements' THEN Credit ELSE 0 END AS CDCR, " & _
                 " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='General Journal' THEN Debit ELSE 0 END AS JVDR, " & _
                 " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='General Journal' THEN Credit ELSE 0 END AS JVCR, " & _
                 " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Accounts Payable' THEN Debit ELSE 0 END AS PBDR, " & _
                 " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Accounts Payable' THEN Credit ELSE 0 END AS PBCR, " & _
                 " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Sales' THEN Debit ELSE 0 END AS SBDR, " & _
                 " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Sales' THEN Credit ELSE 0 END AS SBCR, " & _
                 " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Inventory' THEN Debit ELSE 0 END AS IBDR, " & _
                 " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Inventory' THEN Credit ELSE 0 END AS IBCR, " & _
                 " 		   Debit AS TBDR, " & _
                 " 		   Credit AS TBCR, view_GL.Status " & _
                 " 	FROM View_GL JOIN tblCOA_Master " & _
                 " 	ON View_GL.AccntCode = tblCOA_Master.AccountCode  " & _
                 " 	WHERE AppDate BETWEEN '01-01-" & DateFrom.Year & "' AND '" & DateTo & "' " & IIf(branch = "ALL", "AND BranchCode in ( SELECT    DISTINCT  tblBranch.BranchCode   " & _
                    " FROM      tblBranch    " & _
                    " INNER JOIN tblUser_Access    ON   " & _
                    " tblBranch.BranchCode = tblUser_Access.Code    " & _
                    " AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                    " AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                    " WHERE     UserID ='" & UserID & "'  )", " AND BranchCode = '" & branch & "' ") & _
                 " ) AS A " & _
                 " GROUP BY AccountCode, AccountTitle "

            '" WHERE A.Status ='Saved'  " & _
            SQL.ExecNonQuery(insertSQL)
            Dim f As New frmReport_Display
            f.ShowDialog("FS_TB_Summary", "", DateTo, branch)
            f.Dispose()
        ElseIf Type = "By Book" Then
            If rbSummary.Checked = True Then
                insertSQL = " INSERT INTO tblPRint_TB(Code, Title, BBDR, BBCR, CRDR, CRCR, CDDR, CDCR, JVDR, JVCR, PBDR, PBCR, SBDR, SBCR, IBDR, IBCR, TBDR, TBCR) " & _
             " SELECT  AccountCode, AccountTitle,  " & _
             " 		  SUM(BBDR) AS BBDR, " & _
             " 		  SUM(BBDR) AS BBDR, " & _
             " 		  SUM(CRDR) AS CRDR, " & _
             " 		  SUM(CRCR) AS CRCR, " & _
             " 		  SUM(CDDR) AS CDDR, " & _
             " 		  SUM(CDCR) AS CDCR, " & _
             " 		  SUM(JVDR) AS JVDR, " & _
             " 		  SUM(JVCR) AS JVCR, " & _
             " 		  SUM(PBDR) AS PBDR, " & _
             " 		  SUM(PBCR) AS PBCR, " & _
             " 		  SUM(SBDR) AS SBDR, " & _
             " 		  SUM(SBCR) AS SBCR, " & _
             " 		  SUM(IBDR) AS IBDR, " & _
             " 		  SUM(IBCR) AS IBCR, " & _
             " 		  CASE WHEN SUM(TBDR) > SUM(TBCR) THEN SUM(TBDR) - SUM(TBCR) ELSE 0 END AS TBDR, " & _
             " 		  CASE WHEN SUM(TBCR) > SUM(TBDR) THEN SUM(TBCR) - SUM(TBDR) ELSE 0 END AS TBCR " & _
             " FROM " & _
             " ( " & _
             " 	SELECT tblCOA_Master.AccountCode, tblCOA_Master.AccountTitle,  " & _
             " 		   CASE WHEN AppDate <'" & DateFrom & "' OR Book ='BB' THEN Debit ELSE 0 END AS BBDR, " & _
             " 		   CASE WHEN AppDate < '" & DateFrom & "' OR Book ='BB' THEN Credit ELSE 0 END AS BBCR, " & _
                 " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Receipts' THEN Debit ELSE 0 END AS CRDR, " & _
                 " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Receipts' THEN Credit ELSE 0 END AS CRCR, " & _
                 " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Disbursements' THEN Debit ELSE 0 END AS CDDR, " & _
                 " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Disbursements' THEN Credit ELSE 0 END AS CDCR, " & _
                 " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='General Journal' THEN Debit ELSE 0 END AS JVDR, " & _
                 " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='General Journal' THEN Credit ELSE 0 END AS JVCR, " & _
                 " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Accounts Payable' THEN Debit ELSE 0 END AS PBDR, " & _
                 " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Accounts Payable' THEN Credit ELSE 0 END AS PBCR, " & _
                 " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Sales' THEN Debit ELSE 0 END AS SBDR, " & _
                 " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Sales' THEN Credit ELSE 0 END AS SBCR, " & _
                 " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Inventory' THEN Debit ELSE 0 END AS IBDR, " & _
                 " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Inventory' THEN Credit ELSE 0 END AS IBCR, " & _
             " 		   Debit AS TBDR, " & _
             " 		   Credit AS TBCR " & _
             " 	FROM View_GL INNER JOIN tblCOA_Master " & _
             " 	ON View_GL.AccntCode = tblCOA_Master.AccountCode " & _
             " 	WHERE view_GL.Status <>'Cancelled' AND AppDate BETWEEN '" & DateFrom & "' AND '" & DateTo & "' " & IIf(branch = "ALL", "AND BranchCode in ( SELECT    DISTINCT  tblBranch.BranchCode   " & _
                    " FROM      tblBranch    " & _
                    " INNER JOIN tblUser_Access    ON   " & _
                    " tblBranch.BranchCode = tblUser_Access.Code    " & _
                    " AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                    " AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                    " WHERE     UserID ='" & UserID & "'  )", " AND BranchCode = '" & branch & "' ") & _
             " ) AS A " & _
             " GROUP BY AccountCode, AccountTitle "
                SQL.ExecNonQuery(insertSQL)
                If cbPeriod.SelectedItem = "Yearly" Then
                    Dim f As New frmReport_Display
                    f.ShowDialog("BOASUMY", "", nupYear.Value, Filter, branch)
                    f.Dispose()
                ElseIf cbPeriod.SelectedItem = "Monthly" Then
                    Dim f As New frmReport_Display
                    f.ShowDialog("BOASUMM", "", cbMonth.SelectedIndex + 1, nupYear.Value, Filter, branch)
                    f.Dispose()
                ElseIf cbPeriod.SelectedItem = "Daily" Then
                    Dim f As New frmReport_Display
                    f.ShowDialog("BOASUMD", "", dtpFrom.Value.Date, Filter, branch)
                    f.Dispose()
                ElseIf cbPeriod.SelectedItem = "Date Range" Then
                    Dim f As New frmReport_Display
                    f.ShowDialog("BOASUMR", "", dtpFrom.Value.Date, dtpTo.Value.Date, Filter, branch)
                    f.Dispose()
                End If
            ElseIf rbDetailed.Checked = True Then
                If Filter = "General Ledger" Then
                    If cbPeriod.SelectedItem = "Yearly" Then
                        dtpFrom.Value = CDate("1-1-" + nupYear.Value.ToString)
                        dtpTo.Value = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Year, 1, CDate("1-1-" + nupYear.Value.ToString)))
                    ElseIf cbPeriod.SelectedItem = "Monthly" Then
                        dtpFrom.Value = CDate((cbMonth.SelectedIndex + 1).ToString + "-1-" + nupYear.Value.ToString)
                        dtpTo.Value = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate((cbMonth.SelectedIndex + 1).ToString + "-1-" + nupYear.Value.ToString)))
                    ElseIf cbPeriod.SelectedItem = "Daily" Then
                        dtpTo.Value = dtpFrom.Value.Date
                    End If
                    Dim f As New frmReport_Display
                    f.ShowDialog("Book_GL", "", dtpFrom.Value.Date, dtpTo.Value.Date)
                    f.Dispose()
                Else
                    If cbPeriod.SelectedItem = "Yearly" Then
                        Dim f As New frmReport_Display
                        f.ShowDialog("BOADEMR", "", "01-01-" & nupYear.Value, "12-31-" & nupYear.Value, Filter, branch)
                        f.Dispose()
                    ElseIf cbPeriod.SelectedItem = "Monthly" Then
                        Dim f As New frmReport_Display
                        f.ShowDialog("BOADEMR", "", CDate((cbMonth.SelectedIndex + 1) & "-1-" & nupYear.Value), DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, CDate((cbMonth.SelectedIndex + 1) & "-1-" & nupYear.Value))), Filter, branch)
                        f.Dispose()
                    ElseIf cbPeriod.SelectedItem = "Daily" Then
                        Dim f As New frmReport_Display
                        f.ShowDialog("BOADEMR", "", dtpFrom.Value.Date, dtpTo.Value.Date, Filter, branch)
                        f.Dispose()
                    ElseIf cbPeriod.SelectedItem = "Date Range" Then
                        Dim f As New frmReport_Display
                        f.ShowDialog("BOADEMR", "", dtpFrom.Value.Date, dtpTo.Value.Date, Filter, branch)
                        f.Dispose()
                    End If
                End If
            End If

        End If
    End Sub

    Private Sub GenerateBS()
        Dim deleteSQl As String
        deleteSQl = " DELETE FROM rptBS "
        SQL.ExecNonQuery(deleteSQl)
        Dim dt As New DataTable
        Dim query As String
        Dim desc As String
        Dim groupID As Integer = 0
        Dim value As Decimal = 0
        Dim totalDesc(7) As String
        Dim totalCR(7) As Decimal
        Dim insertSQl As String
        Dim prevID As Integer = 0
        Dim recID As Integer = 1
        Dim incre As Integer = 1
        Dim Filter As String


        Filter = IIf(branch = "ALL", "AND BranchCode in ( SELECT    DISTINCT  tblBranch.BranchCode   " & _
                " FROM      tblBranch    " & _
                " INNER JOIN tblUser_Access    ON   " & _
                " tblBranch.BranchCode = tblUser_Access.Code    " & _
                " AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                " AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                " WHERE     UserID ='" & UserID & "'  )", " AND BranchCode = '" & branch & "' ")

        query = " SELECT  CASE WHEN AccountGroup ='SubAccount' THEN AccountCode  + ' - ' + CASE WHEN ISNULL(ReportAlias,'') ='' THEN AccountTitle ELSE ReportAlias END " & vbCrLf & _
                " 			 ELSE CASE WHEN ReportAlias ='' THEN AccountTitle ELSE ReportAlias END " & vbCrLf & _
                " 		END  AS Descrition," & vbCrLf & _
                "  CASE	WHEN AccountGroup = 'Group' THEN 'G1'		WHEN AccountGroup = 'SubGroup' THEN 'G2' " & vbCrLf & _
                " 		WHEN AccountGroup = 'Category' THEN 'G3'	WHEN AccountGroup = 'SubCategory' THEN 'G4' " & vbCrLf & _
                " 		WHEN AccountGroup = 'CostCenter' THEN 'G5'	WHEN AccountGroup = 'MainAccount' THEN 'G6' " & vbCrLf & _
                " 		WHEN AccountGroup = 'SubAccount' THEN 'G7' " & vbCrLf & _
                "  ELSE ''	END AS AccountGroup, " & vbCrLf & _
                " 	   CASE WHEN AccountNature = 'Debit' THEN SUM(ISNULL(Debit,0) - ISNULL(Credit,0))  " & vbCrLf & _
                " 	        WHEN AccountNature = 'Credit' THEN SUM(ISNULL(Credit,0) - ISNULL(Debit,0)) " & vbCrLf & _
                " 			ELSE 0 " & vbCrLf & _
                " 	   END AS Amount, showTotal, contraAccount, OrderNo " & vbCrLf & _
                " FROM tblCOA_Master " & vbCrLf & _
                " LEFT JOIN " & vbCrLf & _
                " ( " & vbCrLf & _
                " 	SELECT    AccntCode, AccntTitle, SUM(Debit) AS Debit, SUM(Credit)  AS Credit " & vbCrLf & _
                " 	FROM      view_GL  " & vbCrLf & _
                " 	WHERE     AppDate BETWEEN CAST('01-01-" & dtpFrom.Value.Year & "' AS DATE) AND '" & dtpTo.Value.Date & "' " & Filter & vbCrLf & _
                " 	GROUP BY  AccntCode, AccntTitle " & vbCrLf & _
                " ) AS TB " & vbCrLf & _
                " ON  tblCOA_Master.AccountCode = TB.AccntCode " & vbCrLf & _
                " WHERE  AccountType ='Balance Sheet' " & vbCrLf & _
                " GROUP BY AccountCode, AccountTitle, ReportAlias, AccountGroup, AccountNature, showTotal, OrderNo, contraAccount " & vbCrLf & _
                " HAVING  (AccountGroup <> 'SubAccount' OR " & vbCrLf & _
                "        (AccountGroup = 'SubAccount'  AND " & vbCrLf & _
                "        (CASE WHEN AccountNature = 'Debit' THEN SUM(ISNULL(Debit,0) - ISNULL(Credit,0))  " & vbCrLf & _
                " 	        WHEN AccountNature = 'Credit' THEN SUM(ISNULL(Credit,0) - ISNULL(Debit,0)) " & vbCrLf & _
                " 			ELSE 0 " & vbCrLf & _
                " 	   END) <> 0)) " & vbCrLf & _
                " UNION ALL " & vbCrLf & _
                " ( " & vbCrLf & _
                "	  SELECT CASE WHEN AccountGroup ='SubAccount' THEN AccountCode  + ' - ' + CASE WHEN ISNULL(ReportAlias,'') ='' THEN AccountTitle ELSE ReportAlias END " & vbCrLf & _
                " 				 ELSE CASE WHEN ReportAlias ='' THEN AccountTitle ELSE ReportAlias END " & vbCrLf & _
                " 			END  AS Descrition," & vbCrLf & _
                "	  CASE	WHEN AccountGroup = 'Group' THEN 'G1'		WHEN AccountGroup = 'SubGroup' THEN 'G2' " & vbCrLf & _
                " 			WHEN AccountGroup = 'Category' THEN 'G3'	WHEN AccountGroup = 'SubCategory' THEN 'G4' " & vbCrLf & _
                " 			WHEN AccountGroup = 'CostCenter' THEN 'G5'	WHEN AccountGroup = 'MainAccount' THEN 'G6' " & vbCrLf & _
                " 			WHEN AccountGroup = 'SubAccount' THEN 'G7' " & vbCrLf & _
                "	  ELSE ''	END AS AccountGroup," & vbCrLf & _
                "        Amount," & vbCrLf & _
                "        showTotal, contraAccount, OrderNo" & vbCrLf & _
                "        FROM tblCOA_Master" & vbCrLf & _
                "        LEFT JOIN" & vbCrLf & _
                "	  (" & vbCrLf & _
                "		SELECT (SELECT AccountCode FROM tblCOA_Master WHERE AccountTitle LIKE 'NET%' AND AccountGroup = 'SubAccount') AS AccntCode, ISNULL(SUM(Credit - Debit),0) AS Amount FROM" & vbCrLf & _
                "		View_GL WHERE  AppDate BETWEEN CAST('01-01-" & dtpFrom.Value.Year & "' AS DATE) AND '" & dtpTo.Value.Date & "' AND AccntCode IN (SELECT AccountCode FROM tblCOA_Master WHERE AccountType = 'Income Statement')" & vbCrLf & _
                "	  ) AS Bal ON AccountCode = AccntCode" & vbCrLf & _
                "	  WHERE AccountTitle LIKE 'NET%' AND AccountGroup = 'SubAccount'" & vbCrLf & _
                " )" & vbCrLf & _
                " ORDER BY OrderNo "

        'query = " SELECT  CASE WHEN AccountGroup ='SubAccount' THEN AccountCode  + ' - ' + CASE WHEN ISNULL(ReportAlias,'') ='' THEN AccountTitle ELSE ReportAlias END " & _
        '        " 			 ELSE CASE WHEN ReportAlias ='' THEN AccountTitle ELSE ReportAlias END " & _
        '        " 		END  AS Descrition," & _
        '        "  CASE	WHEN AccountGroup = 'Group' THEN 'G1'		WHEN AccountGroup = 'SubGroup' THEN 'G2' " & _
        '        " 		WHEN AccountGroup = 'Category' THEN 'G3'	WHEN AccountGroup = 'SubCategory' THEN 'G4' " & _
        '        " 		WHEN AccountGroup = 'CostCenter' THEN 'G5'	WHEN AccountGroup = 'MainAccount' THEN 'G6' " & _
        '        " 		WHEN AccountGroup = 'SubAccount' THEN 'G7' " & _
        '        "  ELSE ''	END AS AccountGroup, " & _
        '        " 	   CASE WHEN AccountNature = 'Debit' THEN SUM(ISNULL(Debit,0) - ISNULL(Credit,0))  " & _
        '        " 	        WHEN AccountNature = 'Credit' THEN SUM(ISNULL(Credit,0) - ISNULL(Debit,0)) " & _
        '        " 			ELSE 0 " & _
        '        " 	   END AS Amount, showTotal, contraAccount " & _
        '        " FROM tblCOA_Master " & _
        '        " LEFT JOIN " & _
        '        " ( " & _
        '        " 	SELECT    AccntCode, AccntTitle, SUM(Debit) AS Debit, SUM(Credit)  AS Credit " & _
        '        " 	FROM      view_GL  " & _
        '        " 	WHERE     AppDate BETWEEN CAST('01-01-" & dtpFrom.Value.Year & "' AS DATE) AND '" & dtpTo.Value.Date & "' " & Filter & _
        '        " 	GROUP BY  AccntCode, AccntTitle " & _
        '        " ) AS TB " & _
        '        " ON  tblCOA_Master.AccountCode = TB.AccntCode " & _
        '        " WHERE  AccountType ='Balance Sheet' " & _
        '        " GROUP BY AccountCode, AccountTitle, ReportAlias, AccountGroup, AccountNature, showTotal, OrderNo, contraAccount " & _
        '        " HAVING  (AccountGroup <> 'SubAccount' OR " & _
        '        "        (AccountGroup = 'SubAccount'  AND " & _
        '        "        (CASE WHEN AccountNature = 'Debit' THEN SUM(ISNULL(Debit,0) - ISNULL(Credit,0))  " & _
        '        " 	        WHEN AccountNature = 'Credit' THEN SUM(ISNULL(Credit,0) - ISNULL(Debit,0)) " & _
        '        " 			ELSE 0 " & _
        '        " 	   END) <> 0)) " & _
        '        " ORDER BY OrderNo "
        SQL.GetQuery(query)
        If SQL.SQLDS.Tables(0).Rows.Count > 0 Then
            For Each row As DataRow In SQL.SQLDS.Tables(0).Rows
                desc = row(0).ToString
                groupID = CInt(row(1).ToString.Replace("G", ""))
                value = row(2)
                If row(3) = True Then
                    totalDesc(groupID) = desc
                End If
                If row(4) = True Then
                    value = value * -1
                End If
                If groupID <> prevID Or groupID = 7 Then

                    If prevID > groupID Then
                        If prevID <> 7 Then
                            For i As Integer = incre - 1 To 0 Step -1
                                deleteSQl = " DELETE FROM rptBS WHERE RecordID = '" & recID - 1 & "' "
                                SQL.ExecNonQuery(deleteSQl)
                                recID -= 1
                            Next

                            incre = 0
                        Else
                            incre = 0
                        End If
                        For i As Integer = 6 To 1 Step -1
                            If groupID <= i Then
                                If Not IsNothing(totalDesc(i)) AndAlso totalDesc(i) <> "" Then
                                    If totalCR(i) <> 0 Then
                                        insertSQl = " INSERT INTO " & _
                                               " rptBS(RecordID, Description, Amount, GroupID) " & _
                                               " VALUES(@RecordID, @Description, @Amount, @GroupID)"
                                        SQL.FlushParams()
                                        SQL.AddParam("@RecordID", recID)
                                        SQL.AddParam("@Description", "TOTAL " & totalDesc(i))
                                        SQL.AddParam("@Amount", totalCR(i))
                                        SQL.AddParam("@GroupID", i)
                                        SQL.ExecNonQuery(insertSQl)
                                        incre = 0
                                        totalDesc(i) = Nothing
                                        totalCR(i) = Nothing
                                        recID += 1
                                    End If

                                End If
                            End If

                        Next
                    End If
                    insertSQl = " INSERT INTO " & _
                        " rptBS(RecordID, Description, Amount, GroupID) " & _
                        " VALUES(@RecordID, @Description, @Amount, @GroupID)"
                    SQL.FlushParams()
                    SQL.AddParam("@RecordID", recID)
                    SQL.AddParam("@Description", desc)
                    SQL.AddParam("@Amount", value)
                    SQL.AddParam("@GroupID", groupID)
                    SQL.ExecNonQuery(insertSQl)
                    prevID = groupID
                    recID += 1
                    incre += 1
                    If value <> 0 Then
                        For i As Integer = 1 To 7
                            If Not IsNothing(totalDesc(i)) AndAlso totalDesc(i) <> "" Then
                                totalCR(i) += value
                            End If
                        Next
                    End If

                End If
            Next
            If prevID <> 7 Then
                For i As Integer = incre - 1 To 0 Step -1
                    deleteSQl = " DELETE FROM rptBS WHERE RecordID = '" & recID - 1 & "' "
                    SQL.ExecNonQuery(deleteSQl)
                    recID -= 1
                Next
            End If
            For i As Integer = 6 To 1 Step -1
                If Not IsNothing(totalDesc(i)) AndAlso totalDesc(i) <> "" Then
                    If totalCR(i) <> 0 Then
                        insertSQl = " INSERT INTO " & _
                               " rptBS(RecordID, Description, Amount, GroupID) " & _
                               " VALUES(@RecordID, @Description, @Amount, @GroupID)"
                        SQL.FlushParams()
                        SQL.AddParam("@RecordID", recID)
                        SQL.AddParam("@Description", "TOTAL " & totalDesc(i))
                        SQL.AddParam("@Amount", totalCR(i))
                        SQL.AddParam("@GroupID", i)
                        SQL.ExecNonQuery(insertSQl)
                        incre = 0
                        totalDesc(i) = Nothing
                        totalCR(i) = Nothing
                        recID += 1
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub GenerateDCPR(ByVal Type As String, ByVal DateFrom As Date, ByVal DateTo As Date, Optional ByVal Filter As String = "")
        Dim insertSQL, deleteSQL As String
        deleteSQL = " DELETE FROM tblPrint_DCPR "
        SQL.ExecNonQuery(deleteSQL)
        If Type = "Daily" Then
            insertSQL = " INSERT INTO tblPRint_DCPR(Code, Title, BB, CR, DEP, CD, JV, Row_ID) " & _
                        "  SELECT  AccountCode, AccountTitle,   		   " & _
                        " 		SUM(BB) AS BB,  		   " & _
                        " 		SUM(CR) AS CR,  		   " & _
                        " 		SUM(DEP) AS DEP,  		   " & _
                        " 		SUM(CD) AS CD,  		   " & _
                        " 		SUM(JV) AS JV,           " & _
                        " 		ROW_NUMBER() OVER (ORDER BY AccountCode) AS Row_ID   " & _
                        " FROM   " & _
                        " (  	 " & _
                        " 		SELECT  AccountCode, AccountTitle,   		    " & _
                        " 				CASE WHEN AppDate <'" & DateFrom & "' OR Book ='BB' THEN Debit - Credit ELSE 0 END AS BB,  		    " & _
                        " 				CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Receipts' AND RefType <> 'DS' THEN Debit - Credit ELSE 0 END AS CR,   " & _
                        " 			    CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Receipts' AND RefType ='DS' THEN Debit - Credit ELSE 0 END AS DEP,   " & _
                        " 				CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Disbursements' THEN Credit - Debit ELSE 0 END AS CD,  		   " & _
                        " 				CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='General Journal' THEN Debit - Credit ELSE 0 END AS JV  	 " & _
                        " 		FROM view_GL RIGHT JOIN tblCOA_Master  	 " & _
                        " 		ON   view_GL.AccntCode = tblCOA_Master.AccountCode  	 " & _
                        " 		WHERE (AppDate BETWEEN '01-01-" & DateFrom.Year & "' AND '" & DateFrom & "'  OR view_GL.AccntCode IS NULL)    " & _
                        " 		AND   (tblCOA_Master.AccountCode IN (SELECT AccountCode FROM tblBank_Master))   " & IIf(branch = "ALL", "AND BranchCode in ( SELECT    DISTINCT  tblBranch.BranchCode   " & _
                        " FROM      tblBranch    " & _
                        " INNER JOIN tblUser_Access    ON   " & _
                        " tblBranch.BranchCode = tblUser_Access.Code    " & _
                        " AND       tblUser_Access.Status ='Active' AND tblBranch.Status ='Active'   " & _
                        " AND       tblUser_Access.Type = 'BranchCode' AND isAllowed = 1  " & _
                        " WHERE     UserID ='" & UserID & "'  )", " AND BranchCode = '" & branch & "' ") & _
                        " ) AS A  GROUP BY AccountCode, AccountTitle "
            SQL.ExecNonQuery(insertSQL)
            Dim f As New frmReport_Display
            f.ShowDialog("DCPR", DateTo)
            f.Dispose()
        End If
    End Sub



    Private Sub nupYear_ValueChanged(sender As System.Object, e As System.EventArgs) Handles nupYear.ValueChanged, cbMonth.SelectedIndexChanged, chkYTD.CheckedChanged
        LoadPeriod()
    End Sub

    Private Sub lvFilter_Click(sender As Object, e As System.EventArgs) Handles lvFilter.Click
        If rbNone.Checked = True Then
            For Each item As ListViewItem In lvFilter.Items
                item.Checked = False
            Next
        End If
    End Sub



    Private Sub lvFilter_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lvFilter.SelectedIndexChanged
        If cbReport.SelectedItem = "Book of Accounts" Then
            For Each item As ListViewItem In lvFilter.Items
                item.Checked = False
            Next
        End If
        If lvFilter.SelectedItems.Count = 1 Then
            If lvFilter.SelectedItems(0).Checked = False Then
                lvFilter.SelectedItems(0).Checked = True
            Else
                lvFilter.SelectedItems(0).Checked = False
            End If
        End If
        If cbReport.SelectedItem <> "Member List" Then
            rbSpecific.Checked = True
        End If
    End Sub

    Private Sub cbPeriod_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbPeriod.SelectedIndexChanged
        If cbPeriod.SelectedItem = "Monthly" Then
            gbPeriodYM.Visible = True
            gbPeriodFT.Visible = False
            chkYTD.Visible = False
            cbMonth.Enabled = True
            cbMonth.Items.Clear()
            cbMonth.Items.Add("January")
            cbMonth.Items.Add("February")
            cbMonth.Items.Add("March")
            cbMonth.Items.Add("April")
            cbMonth.Items.Add("May")
            cbMonth.Items.Add("June")
            cbMonth.Items.Add("July")
            cbMonth.Items.Add("August")
            cbMonth.Items.Add("September")
            cbMonth.Items.Add("October")
            cbMonth.Items.Add("November")
            cbMonth.Items.Add("December")
            lblMonth.Text = "Month :"
        ElseIf cbPeriod.SelectedItem = "Daily" Then
            gbPeriodYM.Visible = False
            gbPeriodFT.Visible = True
            lblTo.Visible = False
            dtpTo.Visible = False
            lblFrom.Text = "Date :"
        ElseIf cbPeriod.SelectedItem = "Date Range" Then
            lblFrom.Text = "From :"
            lblTo.Visible = True
            dtpTo.Visible = True
            gbPeriodYM.Visible = False
            gbPeriodFT.Visible = True '
        ElseIf cbPeriod.SelectedItem = "Yearly" Then
            chkYTD.Checked = True
            LoadPeriod()
            lblFrom.Text = "From :"
            lblTo.Visible = True
            dtpTo.Visible = True
            gbPeriodYM.Visible = True
            gbPeriodFT.Visible = False
            cbMonth.Enabled = False
        ElseIf cbPeriod.SelectedItem = "Quarterly" Then
            chkYTD.Checked = True
            LoadPeriod()
            cbMonth.Items.Clear()
            cbMonth.Items.Add("1st Quarter")
            cbMonth.Items.Add("2nd Quarter")
            cbMonth.Items.Add("3rd Quarter")
            cbMonth.Items.Add("4th Quarter")
            lblMonth.Text = "Quarter :"
            lblFrom.Text = "From :"
            lblTo.Visible = True
            dtpTo.Visible = True
            gbPeriodYM.Visible = True
            gbPeriodFT.Visible = False
            cbMonth.Enabled = True
        End If
    End Sub


    Private Sub rbAll_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbAll.CheckedChanged, rbNone.CheckedChanged, rbSpecific.CheckedChanged
        If rbAll.Checked = True Then
            For Each item As ListViewItem In lvFilter.Items
                item.Checked = True
            Next
        ElseIf rbNone.Checked = True Then
            For Each item As ListViewItem In lvFilter.Items
                item.Checked = False
            Next
        End If
    End Sub

    Private Sub GroupBox6_Enter(sender As System.Object, e As System.EventArgs) Handles GroupBox6.Enter

    End Sub

    Private Sub cbBranch_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbBranch.SelectedIndexChanged

    End Sub
End Class