Public Class frmVerifier_DetailOthers

    Dim SQL As New SQLControl
    Public strType As String = ""
    Public strAccntCode As String = ""
    Public strVCECode As String = ""
    Private Sub frmVerifier_DetailOthers_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        LoadMember()
        LoadAccount()
        LoadTransactions()
        LoadBalance()
    End Sub

    Private Sub LoadMember()
        Dim selectSQL As String = "SELECT VCEName FROM viewVCE_Master WHERE VCECode = '" & strVCECode & "'"
        SQL.ReadQuery(selectSQL)
        If SQL.SQLDR.Read Then
            txtVCECode.Text = strVCECode
            txtVCEName.Text = SQL.SQLDR("VCEName").ToString
        End If
    End Sub

    Private Sub LoadAccount()
        Dim selectSQL As String = "SELECT AccountTitle FROM tblCOA_Master WHERE AccountCode = '" & strAccntCode & "'"
        SQL.ReadQuery(selectSQL)
        If SQL.SQLDR.Read Then
            txtAccount.Text = SQL.SQLDR("AccountTitle").ToString
        End If
    End Sub

    Private Sub LoadTransactions()
        Dim selectSQL As String = ""
        selectSQL = "SELECT RefType, TransNo, AppDate, Credit, Debit, Balance FROM view_Ledger WHERE AccntCode = '" & strAccntCode & "' AND VCECode = '" & strVCECode & "' "
        SQL.ReadQuery(selectSQL)
        dgvLedger.Rows.Clear()
        Dim count As Integer = 1
        While SQL.SQLDR.Read
            If strAccntCode.Substring(0, 1) = "2" Or strAccntCode.Substring(0, 1) = "3" Or strAccntCode.Substring(0, 1) = "5" Then
                dgvLedger.Columns(colDeposit.Index).HeaderText = "Deposit"
                dgvLedger.Columns(colWithdrawal.Index).HeaderText = "Withdrawal"
                dgvLedger.Rows.Add(count, _
                                   SQL.SQLDR("RefType").ToString, _
                                   SQL.SQLDR("TransNo").ToString, _
                                   CDate(SQL.SQLDR("AppDate").ToString).ToShortDateString, _
                                   CDec(SQL.SQLDR("Credit").ToString).ToString("N2"), _
                                   CDec(SQL.SQLDR("Debit").ToString).ToString("N2"), _
                                   CDec(SQL.SQLDR("Balance").ToString).ToString("N2"))
            Else
                dgvLedger.Columns(colDeposit.Index).HeaderText = "Loan"
                dgvLedger.Columns(colWithdrawal.Index).HeaderText = "Payment"
                dgvLedger.Rows.Add(count, _
                                   SQL.SQLDR("RefType").ToString, _
                                   SQL.SQLDR("TransNo").ToString, _
                                   CDate(SQL.SQLDR("AppDate").ToString).ToShortDateString, _
                                   CDec(SQL.SQLDR("Debit").ToString).ToString("N2"), _
                                   CDec(SQL.SQLDR("Credit").ToString).ToString("N2"), _
                                   CDec(SQL.SQLDR("Balance").ToString).ToString("N2"))
            End If

            count += 1
        End While

    End Sub

    Private Sub LoadBalance()
        Dim selectSQL As String = " SELECT TOP 1 Balance FROM view_Ledger WHERE AccntCode = '" & strAccntCode & "' AND VCECode = '" & strVCECode & "' ORDER BY AppDate DESC "
        SQL.ReadQuery(selectSQL)
        While SQL.SQLDR.Read
            lblBalance.Text = CDec(SQL.SQLDR("Balance").ToString).ToString("N2")
        End While
    End Sub

End Class