Public Class frmUser_Add
    Public User_ID As Integer
    Public Type As String
    Public password As String
    Dim ModuleID As String = "UAR"

    Public Overloads Function ShowDialog(ByVal User As Integer) As Boolean
        User_ID = User
        MyBase.ShowDialog()
        Return True
    End Function


    Private Sub frmUser_Add_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ClearText()
        LoadUserLevel()
        If Type = "ADD" Then
            EnableControl(True)
            txtOldPass.Visible = False
            lblOldPass.Visible = False
        ElseIf Type = "CHANGE PASS" Then
            If User_ID <> 0 Then
                LoadUser(User_ID)
                txtOldPass.Visible = True
                lblOldPass.Visible = True
                EnableControl(True)
            End If
        Else
            txtOldPass.Visible = False
            lblOldPass.Visible = False
            EnableControl(True)
        End If
    End Sub

    Protected Sub LoadUser(User_ID)

        activityStatus = True
        Dim query As String
        query = " SELECT UserName, LoginName, Password, UserLevel, RefID, EmailAddress, ContactNo " & _
              " FROM tblUser WHERE UserID = '" & User_ID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtName.Text = SQL.SQLDR("UserName").ToString
            txtUsername.Text = SQL.SQLDR("LoginName").ToString
            password = SQL.SQLDR("Password").ToString
            txtOldPass.Text = password
            cbUserLevel.SelectedItem = SQL.SQLDR("UserLevel").ToString
            txtID.Text = SQL.SQLDR("RefID").ToString
            txtEmail.Text = SQL.SQLDR("EmailAddress").ToString
            txtContact.Text = SQL.SQLDR("ContactNo").ToString
        End If

    End Sub

    Private Sub EnableControl(ByVal Value As Boolean)

        If Type = "ADD" Then
            cbUserLevel.Enabled = True
            txtID.Enabled = True
            txtName.Enabled = True
            txtEmail.Enabled = True
            txtContact.Enabled = True
            txtUsername.Enabled = True
            txtPassword.Enabled = True
            txtConfirmPass.Enabled = True
            txtOldPass.Enabled = True
            btnSearchVCE.Enabled = True
        ElseIf Type = "CHANGE PASS" Then
            If User_ID <> 0 Then
                cbUserLevel.Enabled = True
                txtID.Enabled = False
                txtName.Enabled = False
                txtEmail.Enabled = True
                txtContact.Enabled = True
                txtUsername.Enabled = False
                txtPassword.Enabled = True
                txtConfirmPass.Enabled = True
                txtOldPass.Enabled = False
                btnSearchVCE.Enabled = False
            End If

        End If
    End Sub

    Private Sub ClearText()
        cbUserLevel.SelectedIndex = -1
        txtID.Clear()
        txtName.Clear()
        txtEmail.Clear()
        txtContact.Clear()
        txtUsername.Clear()
        txtPassword.Clear()
        txtConfirmPass.Clear()
        txtOldPass.Clear()

    End Sub

    Private Sub LoadUserLevel()
        Dim query As String
        query = " SELECT DISTINCT UserLevel FROM tblUser_Level WHERE UserLevel <> 'Master Admin' "
        SQL.ReadQuery(query)
        cbUserLevel.Items.Clear()
        While SQL.SQLDR.Read
            cbUserLevel.Items.Add(SQL.SQLDR("UserLevel").ToString)
        End While
        If cbUserLevel.Items.Count > 0 Then
            cbUserLevel.SelectedIndex = 0
        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Type = "ADD" Then
                If MessageBox.Show("Are you sure you want to Save this User?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    If txtName.Text = "" Or txtPassword.Text = "" Or txtUsername.Text = "" Then
                        msgRequired()
                    ElseIf txtPassword.Text <> txtConfirmPass.Text Then
                        MsgBox("Password confirmation doesn't match!", MsgBoxStyle.Exclamation)
                    ElseIf UserExist() Then
                        MsgBox("Username already exist! Please choose another.", MsgBoxStyle.Exclamation)
                        txtUsername.Focus()
                    ElseIf txtPassword.TextLength < 6 Then
                        MsgBox("Password with less than 6 character is not acceptable, it is considered a weak Password.", vbInformation)
                    Else
                        SaveUser()
                        msgsave()
                        Me.Close()
                    End If
                End If
            ElseIf Type = "CHANGE PASS" Then
                If MessageBox.Show("Are you sure you want to Update this User?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    If txtPassword.Text = "" Then
                        msgRequired()
                    ElseIf txtPassword.Text <> txtConfirmPass.Text Then
                        MsgBox("Password confirmation doesn't match!", MsgBoxStyle.Exclamation)
                        txtUsername.Focus()
                    ElseIf txtOldPass.Text <> password Then
                        MsgBox("Old password is invalid!", MsgBoxStyle.Exclamation)
                        txtUsername.Focus()
                    ElseIf txtPassword.TextLength < 6 Then
                        MsgBox("Password with less than 6 character is not acceptable, it is considered a weak Password.", vbInformation)
                    Else
                        UpdateUser()
                        msgupdated()
                        Me.Close()
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Protected Sub SaveUser()
        Try
            ' HASH PASSWORD
            Dim hash As String = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text, WorkFactor)
            activityStatus = True
            Dim insertSQL As String
            insertSQL = " INSERT INTO " & _
                  " tblUser (UserName, LoginName, Password, UserLevel, RefID, EmailAddress, ContactNo, WhoCreated) " & _
                  " VALUES (@UserName, @LoginName, @Password, @UserLevel, @RefID, @EmailAddress, @ContactNo, @WhoCreated)"
            SQL.AddParam("@UserName", txtName.Text)
            SQL.AddParam("@LoginName", txtUsername.Text)
            SQL.AddParam("@Password", hash)
            SQL.AddParam("@UserLevel", cbUserLevel.SelectedItem)
            SQL.AddParam("@RefID", txtID.Text)
            SQL.AddParam("@EmailAddress", txtEmail.Text)
            SQL.AddParam("@ContactNo", txtContact.Text)
            SQL.AddParam("@WhoCreated", UserID)
            SQL.ExecNonQuery(insertSQL)
        Catch ex As Exception
            activityStatus = False
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        Finally
            RecordActivity(UserID, ModuleID, Me.Name.ToString, "INSERT", "UserID", User_ID, BusinessType, BranchCode, "", activityStatus)
            SQL.FlushParams()
        End Try

    End Sub

    Protected Sub UpdateUser()
        Try
            activityStatus = True
            Dim hash As String = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text, WorkFactor)
            Dim updateSQL As String
            updateSQL = " UPDATE tblUser " & _
                        " SET    UserName = @UserName, LoginName = @LoginName, Password = @Password, FirstLogin = @FirstLogin, " & _
                        "        UserLevel = @UserLevel, RefID = @RefID, EmailAddress = @EmailAddress, ContactNo = @ContactNo, " & _
                        "        WhoModified = @WhoModified, DateModified = GETDATE() " & _
                        " WHERE  UserID = @UserID "
            SQL.AddParam("@UserID", User_ID)
            SQL.AddParam("@UserName", txtName.Text)
            SQL.AddParam("@LoginName", txtUsername.Text)
            SQL.AddParam("@Password", hash)
            SQL.AddParam("@FirstLogin", True)
            SQL.AddParam("@UserLevel", cbUserLevel.SelectedItem)
            SQL.AddParam("@RefID", txtID.Text)
            SQL.AddParam("@EmailAddress", txtEmail.Text)
            SQL.AddParam("@ContactNo", txtContact.Text)
            SQL.AddParam("@WhoModified", UserID)
            SQL.ExecNonQuery(updateSQL)
        Catch ex As Exception
            activityStatus = False
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
        Finally
            RecordActivity(UserID, ModuleID, Me.Name.ToString, "UPDATE", "UserID", User_ID, BusinessType, BranchCode, "", activityStatus)
            SQL.FlushParams()
        End Try
    End Sub

    Private Sub txtUsername_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUsername.GotFocus
        txtUsername.SelectionStart = 0
        txtUsername.SelectionLength = Len(txtUsername.Text)
    End Sub

    Private Function UserExist() As Boolean
        Dim query As String
        Try
            query = "SELECT LoginName FROM tblUser WHERE LoginName = @LoginName "
            SQL.FlushParams()
            SQL.AddParam("@LoginName", txtUsername.Text)
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            SaveError(ex.Message, ex.StackTrace, Me.Name.ToString, ModuleID)
            Return False
        Finally
            SQL.FlushParams()
        End Try
    End Function

    Private Sub btnSearchVCE_Click(sender As System.Object, e As System.EventArgs) Handles btnSearchVCE.Click
        Dim f As New frmVCE_Search
        f.ShowDialog()
        LoadUserInfo(f.VCECode)
        f.Dispose()
    End Sub

    Private Sub LoadUserInfo(ByVal ID As String)
        Dim query As String
        query = " SELECT    VCECode, VCEName  " & _
                " FROM      viewVCE_Master " & _
                " WHERE     VCECode ='" & ID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtID.Text = SQL.SQLDR("VCECode").ToString
            txtName.Text = SQL.SQLDR("VCEName").ToString
        Else

            txtID.Clear()
            txtName.Clear()
            txtEmail.Clear()
            txtContact.Clear()
        End If
    End Sub

End Class