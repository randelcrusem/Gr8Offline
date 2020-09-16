Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class frmReport_Display
    Public docnum As String
    Public docutype As String
    Public BillingPeriod As String
    Public company As String
    Public Accnttitle As String
    'Dim rpt, p1, p2, p3, p4, p5 As String
    Public rpt, p1, p2, p3, p4, p5 As String
    Dim crtableLogoninfos As New TableLogOnInfos()
    Dim crtableLogoninfo As New TableLogOnInfo()
    Dim crConnectionInfo As New ConnectionInfo()
    Dim CrTables As Tables
    Dim CrTable As Table
    Dim crReportDocument As New ReportDocument()

    Public Overloads Function ShowDialog(ByVal reportType As String, ByVal param1 As String, Optional ByVal param2 As String = "", _
                                         Optional param3 As String = "", Optional param4 As String = "", Optional param5 As String = "") As Boolean
        rpt = reportType
        p1 = param1
        p2 = param2
        p3 = param3
        p4 = param4
        p5 = param5
        MyBase.ShowDialog()
        Return True
    End Function

    Private Sub frmCRDisplay_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub frmCRDisplay_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.Dispose()
    End Sub


    Private Sub frmCRDisplay_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim Report_Path As String
        Report_Path = App_Path & "\CR_Reports\" & database
        Dim CR As New ReportDocument
        Dim CR1 As New ReportDocument
        Dim CR2 As New ReportDocument
        Dim Username As String
        Username = GetUserName(UserID)
        Try
            If Strings.Left(rpt, 5) = "TRANS" Then
                Report_Path = Report_Path & "\" & rpt & ".rpt"
                CR.Load(Report_Path)
                CR.SetDatabaseLogon("sa", "eVoSolution1")
                CR.SetParameterValue("TransID", p1)
            ElseIf Strings.Right(rpt, 5) = "_List" Then
                Report_Path = Report_Path & "\" & rpt & ".rpt"
                CR.Load(Report_Path)
                CR.SetDatabaseLogon("sa", "eVoSolution1")
                CR.SetParameterValue("User", p1)
                CR.SetParameterValue("Type", p2)
                CR.SetParameterValue("DateFrom", p3)
                CR.SetParameterValue("DateTo", p4)
                CR.SetParameterValue("Status", p5)
            Else
                Select Case rpt
                    Case "COA_Master"
                        Report_Path = Report_Path & "\COA_Master.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("Date", p1)
                        CR.SetParameterValue("User", Username)
                    Case "LN_Schedule"
                        Report_Path = Report_Path & "\" & rpt & ".rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("TransID", p1)
                    Case "LN_ScheduleHistory"
                        Report_Path = Report_Path & "\" & rpt & ".rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("TransID", p1)
                        CR.SetParameterValue("Date", p2)
                    Case "LN_PN"
                        Report_Path = Report_Path & "\" & rpt & ".rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("TransID", p1)
                        CR.SetParameterValue("Company", p2)
                    Case "LN_Disclosure"
                        Report_Path = Report_Path & "\" & rpt & ".rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("TransID", p1)
                    Case "LN_AGING"
                        Report_Path = Report_Path & "\LN_Aging.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@AsOfDate", p2)
                        CR.SetParameterValue("@BranchCode", p3)
                        CR.SetParameterValue("@LoanType", p4)
                        CR.SetParameterValue("User", Username)
                        'CR.SetParameterValue("AppDate", p2)
                        'CR.SetParameterValue("Filter", p3)
                    Case "LN_Aging_PerLoanType"
                        Report_Path = Report_Path & "\LN_Aging_PerLoanType.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("AppDate", p2)
                        CR.SetParameterValue("LoanType", p3)
                        CR.SetParameterValue("Filter", p4)
                    Case "LN_Release"
                        Report_Path = Report_Path & "\LN_Release.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("User", frmUserLogin.txtUsername.Text)
                        CR.SetParameterValue("Status", p1)
                        CR.SetParameterValue("DateFrom", p2)
                        CR.SetParameterValue("DateTo", p3)
                        CR.SetParameterValue("Branch", p4)
                    Case "Loan_Title"
                        Report_Path = Report_Path & "\LN_TitleList.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("DateFrom", p1)
                        CR.SetParameterValue("DateTo", p2)
                    Case "LN_CoMakerList"
                        Report_Path = Report_Path & "\LN_CoMakerList.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("Filter", p1)
                    Case "CV_Transaction"
                        Report_Path = Report_Path & "\CV_Transaction.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("DateFrom", p1)
                        CR.SetParameterValue("DateTo", p2)
                    Case "Check_Issuance"
                        Report_Path = Report_Path & "\Check_Issuance.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("DateFrom", p1)
                        CR.SetParameterValue("DateTo", p2)
                    Case "Account_Balances"
                        Report_Path = Report_Path & "\AccountBalances.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("DateFrom", p1)
                        CR.SetParameterValue("DateTo", p2)
                        CR.SetParameterValue("AccntTitle", p3)
                    Case "Loan_Balances"
                        Report_Path = Report_Path & "\LoanBalances.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("DateTo", p1)
                        CR.SetParameterValue("AccountTitle", p2)
                    Case "OR_Transaction"
                        Report_Path = Report_Path & "\OR_Transaction.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("DateFrom", p1)
                        CR.SetParameterValue("DateTo", p2)
                    Case "SOA_Detailed"
                        Report_Path = Report_Path & "\SOA_Detailed.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("VCECode", p1)
                        CR.SetParameterValue("Date", CDate(p2).ToShortDateString)
                        CR.SetParameterValue("DateFrom", CDate(p3).ToShortDateString)
                    Case "SOA_Loan_Detailed"
                        Report_Path = Report_Path & "\SOA_Loan_Detailed.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("VCECode", p1)
                        CR.SetParameterValue("Date", CDate(p2).ToShortDateString)
                        CR.SetParameterValue("DateFrom", CDate(p3).ToShortDateString)
                    Case "SOA_Credit_Detailed"
                        Report_Path = Report_Path & "\SOA_Credit_Detailed.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("VCECode", p1)
                        CR.SetParameterValue("Date", CDate(p2).ToShortDateString)
                        CR.SetParameterValue("DateFrom", CDate(p3).ToShortDateString)
                    Case "SP_AnnexA"
                        Report_Path = Report_Path & "\" & rpt & ".rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("TransID", p1)
                    Case "SP_AnnexC"
                        Report_Path = Report_Path & "\" & rpt & ".rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("TransID", p1)
                    Case "SP_AnnexD"
                        Report_Path = Report_Path & "\" & rpt & ".rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("TransID", p1)
                    Case "SP_AnnexMOA"
                        Report_Path = Report_Path & "\" & rpt & ".rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("TransID", p1)
                    Case "PO_Unserved"
                        Report_Path = Report_Path & "\" & rpt & ".rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("User", p1)
                        CR.SetParameterValue("Type", p2)
                    Case "PR_WithoutPO"
                        Report_Path = Report_Path & "\" & rpt & ".rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("User", p1)
                        CR.SetParameterValue("Type", p2)
                        'Book of Accounts Summary
                    Case "BOASUMY"
                        Report_Path = Report_Path & "\BOASUMY.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("User", p1)
                        CR.SetParameterValue("Year", p2)
                        CR.SetParameterValue("Filter", p3)
                        CR.SetParameterValue("Branch", p4)
                    Case "BOASUMM"
                        Report_Path = Report_Path & "\BOASUMM.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("User", p1)
                        CR.SetParameterValue("Month", p2)
                        CR.SetParameterValue("Year", p3)
                        CR.SetParameterValue("Filter", p4)
                        CR.SetParameterValue("Branch", p5)
                    Case "BOASUMD"
                        Report_Path = Report_Path & "\BOASUMD.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("User", p1)
                        CR.SetParameterValue("Date", p2)
                        CR.SetParameterValue("Filter", p3)
                        CR.SetParameterValue("Branch", p4)
                    Case "BOASUMR"
                        Report_Path = Report_Path & "\BOASUMR.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("User", p1)
                        CR.SetParameterValue("DateFrom", p2)
                        CR.SetParameterValue("DateTo", p3)
                        CR.SetParameterValue("Filter", p4)
                        CR.SetParameterValue("Branch", p5)
                        'Detailed
                    Case "BOADEMR"
                        Report_Path = Report_Path & "\BOADEMR.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("User", Username)
                        CR.SetParameterValue("DateFrom", p2)
                        CR.SetParameterValue("DateTo", p3)
                        CR.SetParameterValue("Filter", p4)
                        CR.SetParameterValue("Branch", p5)

                        'SUMMARY
                    Case "GENLGRDS"
                        Report_Path = Report_Path & "\GENLGRDS.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("User", p1)
                        CR.SetParameterValue("Date", p2)
                    Case "GENLGRMS"
                        Report_Path = Report_Path & "\GENLGRMS.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("User", p1)
                        CR.SetParameterValue("Year", p2)
                        CR.SetParameterValue("Month", p3)
                    Case "GENLGRYS"
                        Report_Path = Report_Path & "\GENLGRYS.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("User", p1)
                        CR.SetParameterValue("Year", p2)
                    Case "GENLGRRS"
                        Report_Path = Report_Path & "\GENLGRRS.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("User", p1)
                        CR.SetParameterValue("DateFrom", p2)
                        CR.SetParameterValue("DateTo", p3)

                        'DETAILED
                    Case "GENLGRDD"
                        Report_Path = Report_Path & "\GENLGRDD.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("User", p1)
                        CR.SetParameterValue("Date", p2)
                    Case "GENLGRMD"
                        Report_Path = Report_Path & "\GENLGRMD.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("User", p1)
                        CR.SetParameterValue("Year", p2)
                        CR.SetParameterValue("Month", p3)
                    Case "GENLGRYD"
                        Report_Path = Report_Path & "\GENLGRYD.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("User", p1)
                        CR.SetParameterValue("Year", p2)
                    Case "GENLGRRD"
                        Report_Path = Report_Path & "\GENLGRRD.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("User", p1)
                        CR.SetParameterValue("DateFrom", p2)
                        CR.SetParameterValue("DateTo", p3)
                    Case "SUBLGRS"
                        Report_Path = Report_Path & "\SUBLGRS.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("User", p1)
                        CR.SetParameterValue("DateFrom", p2)
                        CR.SetParameterValue("DateTo", p3)
                    Case "SUBLGRS_Insu"
                        Report_Path = Report_Path & "\SUBLGRS_Insu.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("User", p1)
                        CR.SetParameterValue("DateFrom", p2)
                        CR.SetParameterValue("DateTo", p3)
                    Case "SUBLGRSC"
                        Report_Path = Report_Path & "\SUBLGRSC.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("User", p1)
                        CR.SetParameterValue("DateFrom", p2)
                        CR.SetParameterValue("DateTo", p3)
                    Case "FSBS"
                        Report_Path = Report_Path & "\FSBS.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("User", p1)
                        CR.SetParameterValue("DateTo", p2)
                    Case "FSINCS"
                        Report_Path = Report_Path & "\FSINCS.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("User", p1)
                        CR.SetParameterValue("DateFrom", p2)
                        CR.SetParameterValue("DateTo", p3)
                    Case "FS_TB_Summary"
                        Report_Path = Report_Path & "\FS_TB_Summary.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("USER", p1)
                        CR.SetParameterValue("DateFrom", p2)
                        CR.SetParameterValue("Branch", p3)
                    Case "FS_TB_Detailed"
                        Report_Path = Report_Path & "\FS_TB_Detailed.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("USER", p1)
                        CR.SetParameterValue("DateFrom", p2)
                        CR.SetParameterValue("Branch", p3)
                    Case "CV Summary"
                        Report_Path = Report_Path & "\CV_Summary.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@FromDate", p1)
                        CR.SetParameterValue("@ToDate", p2)
                    Case "BIR_2307"
                        Report_Path = Report_Path & "\CV_2307.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("TransID", p1)
                    Case ("Billing-Reimburse")
                        Report_Path = Report_Path & "\Billing_Reimbursement.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("TransID", p1)
                    Case "AR"
                        Report_Path = Report_Path & "\AR_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("TransID", p1)
                    Case "OR"
                        Report_Path = Report_Path & "\OR_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("TransID", p1)
                    Case "Billing-Service"
                        Report_Path = Report_Path & "\Billing_Services.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("TransID", p1)
                    Case "CV"
                        Report_Path = Report_Path & "\CV_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                        CR.SetParameterValue("@Username", Username)
                    Case "PCV"
                        Report_Path = Report_Path & "\PCV_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                        CR.SetParameterValue("@Username", Username)
                    Case "Check"
                        Report_Path = Report_Path & "\Check.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                        CR.SetParameterValue("@Name", p2)
                        CR.SetParameterValue("@Date", p3)
                        CR.SetParameterValue("@Amount", p4)
                    Case "Aging"
                        Report_Path = Report_Path & "\Aging_Report.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("AccntTitle", Accnttitle)
                        CR.SetParameterValue("RefDate", BillingPeriod)
                    Case "Aging1"
                        Report_Path = Report_Path & "\Aging_Report_Water.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("AccntTitle", Accnttitle)
                        CR.SetParameterValue("RefDate", BillingPeriod)
                    Case "Aging2"
                        Report_Path = Report_Path & "\Aging_Report_EMU.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("AccntTitle", Accnttitle)
                        CR.SetParameterValue("RefDate", BillingPeriod)

                    Case "Bank Recon"
                        Report_Path = Report_Path & "\BankRecon.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("TransID", docnum)

                    Case "Billing Rental"
                        Report_Path = Report_Path & "\Billing_Rental.rpt"
                        '  MsgBox(Report_Path, vbInformation)
                        CR.Load(Report_Path)  ' CR.Load(Report_Path & "\CR_Reports\Billing Statement072015.rpt")
                        'CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@BSNO", docnum)
                    Case "Billing PEZA"
                        Report_Path = Report_Path & "\Billing_PEZA.rpt"
                        '  MsgBox(Report_Path, vbInformation)
                        CR.Load(Report_Path)  ' CR.Load(Report_Path & "\CR_Reports\Billing Statement072015.rpt")
                        'CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@BSNO", docnum)
                        ' INVENTORY REPORTS
                    Case "IA_Count"
                        Report_Path = Report_Path & "\" & rpt & ".rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("DateFrom", p1)
                        CR.SetParameterValue("DateTo", p2)
                    Case "DepositSlip"
                        Report_Path = Report_Path & "\DepositSlipver2.rpt"
                        '  MsgBox(Report_Path, vbInformation)
                        CR.Load(Report_Path)  ' CR.Load(Report_Path & "\CR_Reports\Billing Statement072015.rpt")
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", docnum)
                    Case "Daily Sales Tally Report"
                        Report_Path = Report_Path & "\Daily Sales Tally Report.rpt"
                        '  MsgBox(Report_Path, vbInformation)
                        CR.Load(Report_Path)  ' CR.Load(Report_Path & "\CR_Reports\Billing Statement072015.rpt")
                        CR.SetDatabaseLogon("sa", "Evosolution")
                        ' CR.SetParameterValue("DepositDate", docnum)
                        '     CR.SetParameterValue("Billing_Period", "July 2015")
                    Case "Sales_Summary_Report"
                        Report_Path = Report_Path & "\Sales_Summary_Report.rpt"
                        '  MsgBox(Report_Path, vbInformation)
                        CR.Load(Report_Path)  ' CR.Load(Report_Path & "\CR_Reports\Billing Statement072015.rpt")
                        CR.SetDatabaseLogon("sa", "Evosolution")
                        Dim datedocnum As Date
                        datedocnum = docnum
                    Case "IDBack"
                        Report_Path = Report_Path & "\ID_BACK.rpt"
                        '  MsgBox(Report_Path, vbInformation)
                        CR.Load(Report_Path)  ' CR.Load(Report_Path & "\CR_Reports\Billing Statement072015.rpt")
                        CR.SetDatabaseLogon("sa", "Evosolution")
                        CR.SetParameterValue("@MemberID2", docnum)
                    Case "IDFront"
                        Report_Path = Report_Path & "\ID_Front.rpt"
                        '  MsgBox(Report_Path, vbInformation)
                        CR.Load(Report_Path)  ' CR.Load(Report_Path & "\CR_Reports\Billing Statement072015.rpt")
                        CR.SetDatabaseLogon("sa", "Evosolution")
                        CR.SetParameterValue("@MemberID2", docnum)
                    Case "Journal Entry"
                        Report_Path = Report_Path & "\JournalEntry.rpt"
                        '  MsgBox(Report_Path, vbInformation)
                        CR.Load(Report_Path)  ' CR.Load(Report_Path & "\CR_Reports\Billing Statement072015.rpt")
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", docnum)
                    Case "Period End Adjusment"
                        Report_Path = Report_Path & "\JournalVoucherPeriodEndAdjustment.rpt"
                        '  MsgBox(Report_Path, vbInformation)
                        CR.Load(Report_Path)  ' CR.Load(Report_Path & "\CR_Reports\Billing Statement072015.rpt")
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", docnum)
                    Case "JV"
                        Report_Path = Report_Path & "\JV_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                    Case "PJ"
                        Report_Path = Report_Path & "\PJ_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                    Case "SJ"
                        Report_Path = Report_Path & "\SJ_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                    Case "BI"
                        Report_Path = Report_Path & "\BI_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                    Case "JO"
                        Report_Path = Report_Path & "\JO_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                        CR.SetParameterValue("@Username", Username)
                    Case "Disbursement"
                        Report_Path = Report_Path & "\CheckVoucher.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", docnum)
                    Case "2307"
                        Report_Path = Report_Path & "\Tax2307.rpt"
                        '  MsgBox(Report_Path, vbInformation)
                        CR.Load(Report_Path)  ' CR.Load(Report_Path & "\CR_Reports\Billing Statement072015.rpt")
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("TransID", docnum)
                    Case "BillingStatement"
                        'CR.Load("C:\perpetual\evo\HCG\HCG\bin\Debug\CR_Reports\Billing_Statement073015abc.rpt")  ' CR.Load(Report_Path & "\CR_Reports\Billing Statement072015.rpt")
                        '   CR.Load("C:\perpetual\evo\HCG\HCG\bin\Debug\CR_Reports\Billing_Statement073015abc.rpt")  ' CR.Load(Report_Path & "\CR_Reports\Billing Statement072015.rpt")
                        Report_Path = Report_Path & "\BillingStatementV2ConsoBP.rpt"
                        '  MsgBox(Report_Path, vbInformation)
                        CR.Load(Report_Path)  ' CR.Load(Report_Path & "\CR_Reports\Billing Statement072015.rpt")
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@BSNO", docnum)
                        '   CR.SetParameterValue("@BillingPeriod", BillingPeriod)
                    Case "Goods Return"
                        CR.Load(Report_Path & "\Billing Statement073015a.rpt")
                        CR.SetDatabaseLogon("sa", "hochengtest")
                        CR.SetParameterValue("ConsolidatingBP", docnum)
                        '  CR.SetParameterValue("DocNumber", docnum)
                    Case "Deposit"
                        'CR.Load("C:\perpetual\evo\HCG\HCG\bin\Debug\CR_Reports\Billing_Statement073015abc.rpt")  ' CR.Load(Report_Path & "\CR_Reports\Billing Statement072015.rpt")
                        '   CR.Load("C:\perpetual\evo\HCG\HCG\bin\Debug\CR_Reports\Billing_Statement073015abc.rpt")  ' CR.Load(Report_Path & "\CR_Reports\Billing Statement072015.rpt")
                        Report_Path = Report_Path & "\Deposit_Report.rpt"
                        '  MsgBox(Report_Path, vbInformation)
                        CR.Load(Report_Path)  ' CR.Load(Report_Path & "\CR_Reports\Billing Statement072015.rpt")
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@DepositDate", docnum)
                        '     CR.SetParameterValue("Billing_Period", "July 2015")
                    Case "Deposit Slip"
                        'CR.Load("C:\perpetual\evo\HCG\HCG\bin\Debug\CR_Reports\Billing_Statement073015abc.rpt")  ' CR.Load(Report_Path & "\CR_Reports\Billing Statement072015.rpt")
                        '   CR.Load("C:\perpetual\evo\HCG\HCG\bin\Debug\CR_Reports\Billing_Statement073015abc.rpt")  ' CR.Load(Report_Path & "\CR_Reports\Billing Statement072015.rpt")
                        Report_Path = Report_Path & "\ORReport.rpt"
                        '  MsgBox(Report_Path, vbInformation)
                        CR.Load(Report_Path)  ' CR.Load(Report_Path & "\CR_Reports\Billing Statement072015.rpt")
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TRNASID", docnum)
                    Case "PO"
                        Report_Path = Report_Path & "\PO_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                        CR.SetParameterValue("@Username", Username)
                    Case "BSStorage"
                        Report_Path = Report_Path & "\BSStorage_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                        CR.SetParameterValue("@Username", Username)
                    Case "PB_Savings"
                        Report_Path = Report_Path & "\PB_Savings.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("VCECode", p1)
                        CR.SetParameterValue("LineNo", p2)
                        CR.SetParameterValue("Page", p3)
                        CR.SetParameterValue("RefNo", p4)
                    Case "PR"
                        Report_Path = Report_Path & "\PR_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                        CR.SetParameterValue("@Username", Username)
                    Case "PR_Detailed"
                        Report_Path = Report_Path & "\PR_Detailed_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                        CR.SetParameterValue("@Username", Username)
                    Case "SO"
                        Report_Path = Report_Path & "\SO_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                        CR.SetParameterValue("@Username", Username)
                    Case "DR"
                        Report_Path = Report_Path & "\DR_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                    Case "APV"
                        Report_Path = Report_Path & "\APV_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                        CR.SetParameterValue("@Username", Username)
                    Case "RR"
                        Report_Path = Report_Path & "\RR_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                        CR.SetParameterValue("@Username", Username)
                    Case "PL"
                        Report_Path = Report_Path & "\PL_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                        CR.SetParameterValue("@Username", Username)
                    Case "GI"
                        Report_Path = Report_Path & "\GI_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                        CR.SetParameterValue("@Username", Username)
                    Case "GR"
                        Report_Path = Report_Path & "\GR_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                        CR.SetParameterValue("@Username", Username)
                    Case "MR"
                        Report_Path = Report_Path & "\MR_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                        CR.SetParameterValue("@Username", Username)
                    Case "ITI"
                        Report_Path = Report_Path & "\ITI_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                    Case "ITR"
                        Report_Path = Report_Path & "\ITR_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                    Case "GR"
                        Report_Path = Report_Path & "\GR_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                    Case "SQ"
                        Report_Path = Report_Path & "\SQ_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                    Case "SI"
                        Report_Path = Report_Path & "\SI_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                    Case "ECS"
                        Report_Path = Report_Path & "\ECS_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                        CR.SetParameterValue("@Username", Username)
                    Case "SIBS"
                        Report_Path = Report_Path & "\SIBS_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                        CR.SetParameterValue("@Username", Username)
                    Case "SIF"
                        Report_Path = Report_Path & "\SI_F_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                        CR.SetParameterValue("@Username", Username)
                    Case "BOM"
                        Report_Path = Report_Path & "\BOM_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                        CR.SetParameterValue("@Username", Username)
                    Case "CF-C"
                        Report_Path = Report_Path & "\CF_Comparison_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                        CR.SetParameterValue("@Username", Username)
                    Case "CF-A"
                        Report_Path = Report_Path & "\CF_Approved_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                        CR.SetParameterValue("@Username", Username)
                    Case "DS"
                        Report_Path = Report_Path & "\DS_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                    Case "DS"
                        Report_Path = Report_Path & "\BR_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                    Case "RFP"
                        Report_Path = Report_Path & "\RFP.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                    Case "RFP_VAT"
                        Report_Path = Report_Path & "\RFP_VAT.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                    Case "RFP_Summary"
                        Report_Path = Report_Path & "\RFP_Summary.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("@TransID", p1)
                    Case "DCPR"
                        Report_Path = Report_Path & "\DCPR.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("DateTo", p1)
                    Case "Book_GL"
                        Report_Path = Report_Path & "\Book_GL.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("User", p1)
                        CR.SetParameterValue("FromDate", p2)
                        CR.SetParameterValue("ToDate", p3)
                    Case "MemberReport"
                        Report_Path = Report_Path & "\MemberList.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("DateFrom", p1)
                        CR.SetParameterValue("DateTo", p2)
                        'Case "FSBS"
                        '    Report_Path = Report_Path & "\FSBS.rpt"
                        '    CR.Load(Report_Path)
                        '    CR.SetDatabaseLogon("sa", "eVoSolution1")
                        '    CR.SetParameterValue("User", p1)
                        '    CR.SetParameterValue("DateTo", p2)
                        '    CR.SetParameterValue("Branch", p3)
                    Case "FSIS"
                        'Report_Path = Report_Path & "\FSIS2.rpt"
                        'CR.Load(Report_Path)
                        'CR.SetDatabaseLogon("sa", "eVoSolution1")
                        'CR.SetParameterValue("User", p1)
                        'CR.SetParameterValue("DateTo", p2)
                        'CR.SetParameterValue("Branch", p3)
                        'CR.SetParameterValue("YTD", p4)
                        Report_Path = Report_Path & "\FSIS.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("User", p1)
                        CR.SetParameterValue("DateTo", p2)
                    Case "BR"
                        Report_Path = Report_Path & "\BR_Printout.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("TransID", p1)
                    Case "Collection_Daily"
                        Report_Path = Report_Path & "\Collection_List.rpt"
                        CR.Load(Report_Path)
                        CR.SetDatabaseLogon("sa", "eVoSolution1")
                        CR.SetParameterValue("DateFrom", p1)
                        CR.SetParameterValue("DateTo", p2)
                        CR.SetParameterValue("Filter", p3)
                        CR.SetParameterValue("TransType", p4)
                        CR.SetParameterValue("Username", Username)

                End Select
            End If


            'CrTables = CR.Database.Tables


            'For Each CrTable In CrTables

            '    crtableLogoninfo = CrTable.LogOnInfo
            '    crConnectionInfo.ServerName = "(local)"
            '    crConnectionInfo.DatabaseName = "CAS_GA"
            '    crConnectionInfo.UserID = "eVoSolution"
            '    crConnectionInfo.Password = "eVoSolutiontest"
            '    crtableLogoninfo.ConnectionInfo = crConnectionInfo
            '    CrTable.ApplyLogOnInfo(crtableLogoninfo)
            '    CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1)

            'Next

            ''    crReportDocument.ReportOptions.EnableSaveDataWithReport = False
            ''Refresh the ReportViewer Object
            'CrystalReportViewer1.RefreshReport()
            'Bind the ReportDocument to ReportViewer Object
            ' CrystalReportViewer1.ReportSource = CR



            CrystalReportViewer1.ReportSource = CR



        Catch exs As SqlException
            MessageBox.Show(exs.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
        End Try

    End Sub

End Class