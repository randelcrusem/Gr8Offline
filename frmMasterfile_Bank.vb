Public Class frmMasterfile_Bank
    Public a As Integer
    Dim idx As Integer
    Dim moduleID As String = "MB"

    Private Sub frmBanklist_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetDatabase()
        LoadItem()
        tsbNew.Enabled = True
        tsbEdit.Enabled = False
        tsbSave.Enabled = False
        tsbDelete.Enabled = False
        tsbClose.Enabled = False
        tsbExit.Enabled = True
        EnableControl(False)
    End Sub

    Protected Sub LoadItem()
        Dim query As String
        query = " SELECT	BankID, Bank, Branch, tblBank_Master.AccountNo, tblBank_Master.AccountCode, AccountTitle, SeriesFrom, SeriesTo, tblBank_Master.Status  " & _
                " FROM	    tblBank_Master LEFT JOIN tblCOA_Master " & _
                " ON		tblBank_Master.AccountCode = tblCOA_Master.AccountCode " & _
                " WHERE     tblBank_Master.Status = 'Active' "
        SQL.ReadQuery(query)
        lvBank.Items.Clear()
        While SQL.SQLDR.Read
            lvBank.Items.Add(SQL.SQLDR("BankID"))
            lvBank.Items(lvBank.Items.Count - 1).SubItems.Add(SQL.SQLDR("Bank").ToString)
            lvBank.Items(lvBank.Items.Count - 1).SubItems.Add(SQL.SQLDR("Branch").ToString)
            lvBank.Items(lvBank.Items.Count - 1).SubItems.Add(SQL.SQLDR("AccountCode").ToString)
            lvBank.Items(lvBank.Items.Count - 1).SubItems.Add(SQL.SQLDR("AccountTitle").ToString)
            lvBank.Items(lvBank.Items.Count - 1).SubItems.Add(SQL.SQLDR("AccountNo").ToString)
            lvBank.Items(lvBank.Items.Count - 1).SubItems.Add(SQL.SQLDR("SeriesFrom").ToString)
            lvBank.Items(lvBank.Items.Count - 1).SubItems.Add(SQL.SQLDR("SeriesTo").ToString)
        End While


        ' Toolstrip Buttons
        tsbNew.Enabled = True
        tsbEdit.Enabled = True
        tsbSave.Enabled = False
        tsbDelete.Enabled = True
        tsbClose.Enabled = False
        tsbExit.Enabled = True
        EnableControl(False)
    End Sub

    Protected Sub ClearText()
        txtBank.Text = ""
        txtBranch.Text = ""
        txtAccntCode.Text = ""
        txtAccntTitle.Text = ""
        txtBankAccntNo.Text = ""
        txtSeriesFr.Text = ""
        txtSeriesTo.Text = ""
    End Sub

    Protected Sub EnableControl(ByVal Value As Boolean)
        txtBank.ReadOnly = Not Value
        txtBranch.ReadOnly = Not Value
        txtAccntTitle.ReadOnly = Not Value
        txtBankAccntNo.ReadOnly = Not Value
        txtSeriesFr.ReadOnly = Not Value
        txtSeriesTo.ReadOnly = Not Value
    End Sub

    Private Sub lstBank_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvBank.MouseClick
        If lvBank.SelectedItems.Count > 0 Then
            txtBank.Text = lvBank.Items(lvBank.SelectedItems(0).Index).SubItems(chBank.Index).Text()
            txtBranch.Text = lvBank.Items(lvBank.SelectedItems(0).Index).SubItems(chBranch.Index).Text()
            txtAccntCode.Text = lvBank.Items(lvBank.SelectedItems(0).Index).SubItems(chAccntCode.Index).Text()
            txtAccntTitle.Text = lvBank.Items(lvBank.SelectedItems(0).Index).SubItems(chAccntTitle.Index).Text()
            txtBankAccntNo.Text = lvBank.Items(lvBank.SelectedItems(0).Index).SubItems(chBankAccount.Index).Text()
            txtSeriesFr.Text = lvBank.Items(lvBank.SelectedItems(0).Index).SubItems(chCheckSeriesFrom.Index).Text()
            txtSeriesTo.Text = lvBank.Items(lvBank.SelectedItems(0).Index).SubItems(chCheckSeriesTo.Index).Text()
            idx = lvBank.Items(lvBank.SelectedItems(0).Index).SubItems(chIDX.Index).Text()
            EnableControl(True)


            ' Toolstrip Buttons
            tsbNew.Enabled = True
            tsbEdit.Enabled = True
            tsbSave.Enabled = False
            tsbDelete.Enabled = True
            tsbClose.Enabled = False
            tsbExit.Enabled = True
            EnableControl(False)
        End If
    End Sub

    Private Sub UpdateBank()

        Dim updateSQL As String
        updateSQL = " UPDATE   tblBank_Master " & _
                    " SET      Branch = @Branch, " & _
                    "          AccountCode = @AccountCode,  " & _
                    "          Bank = @Bank,  " & _
                    "          AccountNo = @AccountNo, " & _
                    "          SeriesFrom = @SeriesFrom, " & _
                    "          SeriesTo = @SeriesTo " & _
                    " WHERE    BankID = @BankID "
        SQL.AddParam("@Bank", txtBank.Text)
        SQL.AddParam("@Branch", txtBranch.Text)
        SQL.AddParam("@AccountCode", txtAccntCode.Text)
        SQL.AddParam("@AccountNo", txtBankAccntNo.Text)
        SQL.AddParam("@SeriesFrom", txtSeriesFr.Text)
        SQL.AddParam("@SeriesTo", txtSeriesTo.Text)
        SQL.AddParam("@BankID", idx)
        SQL.ExecNonQuery(updateSQL)

    End Sub

    Private Sub SaveBank()

        Dim insertSQL As String
        insertSQL = " INSERT INTO " & _
                    " tblBank_Master(Bank, Branch, AccountCode, AccountNo, SeriesFrom, SeriesTo)" & _
                    " VALUES (@Bank, @Branch, @AccountCode, @AccountNo, @SeriesFrom, @SeriesTo)"
        SQL.AddParam("@Bank", txtBank.Text)
        SQL.AddParam("@Branch", txtBranch.Text)
        SQL.AddParam("@AccountCode", txtAccntCode.Text)
        SQL.AddParam("@AccountNo", txtBankAccntNo.Text)
        SQL.AddParam("@SeriesFrom", txtSeriesFr.Text)
        SQL.AddParam("@SeriesTo", txtSeriesTo.Text)
        SQL.ExecNonQuery(insertSQL)

    End Sub

    Private Sub txtAccntTitle_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles txtAccntTitle.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                Dim f As New frmCOA_Search
                f.ShowDialog("AccntTitle", txtAccntTitle.Text)
                txtAccntCode.Text = f.accountcode
                txtAccntTitle.Text = f.accttile
                f.Dispose()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub txtSeriesFr_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtSeriesFr.KeyPress, txtSeriesTo.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub tsbEdit_Click(sender As System.Object, e As System.EventArgs) Handles tsbEdit.Click
        If Not AllowAccess("MB_EDIT") Then
            msgRestricted()
        Else
            If idx = 0 Then
                Msg("Please Select Bank!", MsgBoxStyle.Information)
            Else
                EnableControl(True)

                ' Toolstrip Buttons
                tsbNew.Enabled = False
                tsbEdit.Enabled = False
                tsbSave.Enabled = True
                tsbDelete.Enabled = False
                tsbClose.Enabled = True
                tsbExit.Enabled = False
            End If
        End If
    End Sub

    Private Sub tsbSave_Click(sender As System.Object, e As System.EventArgs) Handles tsbSave.Click
        If txtBank.Text = "" Then
            MsgBox("Please enter bank name!", MsgBoxStyle.Exclamation)
        ElseIf txtAccntCode.Text = "" Then
            MsgBox("Please enter default account title!", MsgBoxStyle.Exclamation)
        ElseIf idx = 0 Then
            If MsgBox("Saving New Record, Click Yes to confirm", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "GR8 Message Alert") = MsgBoxResult.Yes Then
                SaveBank()
                Msg("Record Saved Succesfully!", MsgBoxStyle.Information)
                LoadItem()
                ClearText()
            End If
        Else
            If MsgBox("Updating Record, Click Yes to confirm", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "GR8 Message Alert") = MsgBoxResult.Yes Then
                UpdateBank()
                Msg("Record Updated Succesfully!", MsgBoxStyle.Information)
                LoadItem()
                ClearText()
            End If
        End If
    End Sub

    Private Sub tsbNew_Click(sender As System.Object, e As System.EventArgs) Handles tsbNew.Click

        If Not AllowAccess("MB_ADD") Then
            msgRestricted()
        Else
            ClearText()
            EnableControl(True)
            txtBank.Enabled = True
            idx = 0

            ' Toolstrip Buttons
            tsbNew.Enabled = False
            tsbEdit.Enabled = False
            tsbSave.Enabled = True
            tsbDelete.Enabled = False
            tsbClose.Enabled = True
            tsbExit.Enabled = False
            EnableControl(True)

        End If
    End Sub

    Private Sub tsbDelete_Click(sender As System.Object, e As System.EventArgs) Handles tsbDelete.Click
        If Not AllowAccess("MB_DEL") Then
            msgRestricted()
        Else
            If idx <> 0 Then
                If MsgBox("Are you sure you want to delete this record?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "GR8 Message Alert") = MsgBoxResult.Yes Then

                    Dim updateSQL As String
                    updateSQL = " UPDATE   tblBank_Master " & _
                                " SET      Status = 'Inactive' " & _
                                " WHERE    BankID = @Bank_ID "
                    SQL.AddParam("@Bank_ID", idx)
                    SQL.ExecNonQuery(updateSQL)
                    MsgBox("Removed Succesfully", MsgBoxStyle.Information)
                    LoadItem()

                    tsbNew.PerformClick()
                    tsbNew.Enabled = True
                    tsbEdit.Enabled = False
                    tsbSave.Enabled = False
                    tsbDelete.Enabled = False
                    tsbClose.Enabled = False
                    tsbExit.Enabled = True
                    EnableControl(False)

                End If
            End If
        End If
    End Sub

    Private Sub tsbClose_Click(sender As System.Object, e As System.EventArgs) Handles tsbClose.Click

        ' Toolstrip Buttons
        If idx = 0 Then
            tsbNew.PerformClick()
            EnableControl(False)
            tsbEdit.Enabled = False
            tsbDelete.Enabled = False
        Else
            idx = 0
            LoadItem()
            ClearText()
            tsbEdit.Enabled = True
            tsbDelete.Enabled = True

        End If
        tsbNew.Enabled = True
        tsbSave.Enabled = False
        tsbClose.Enabled = False
        tsbExit.Enabled = True
    End Sub

    Private Sub lvBank_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lvBank.SelectedIndexChanged

    End Sub

    Private Sub tsbExit_Click(sender As System.Object, e As System.EventArgs) Handles tsbExit.Click
        Me.Close()
        Me.Dispose()
    End Sub
End Class