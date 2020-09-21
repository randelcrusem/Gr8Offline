﻿Imports System.Windows.Forms
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Security
Imports System.Security.Principal
Imports System.Net.NetworkInformation

Public Class Main_JADE
    Public Overloads Function ShowDialog(ByVal DTBSE As String, ByVal IPADD As String) As Boolean
        TxtDatabase.Text = DTBSE
        MyBase.ShowDialog()
        Return True
    End Function

    Private Sub OpenFile(ByVal sender As Object, ByVal e As EventArgs)
        Dim OpenFileDialog As New OpenFileDialog
        OpenFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        OpenFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
        If (OpenFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            Dim FileName As String = OpenFileDialog.FileName
            ' TODO: Add code here to open the file.
        End If
    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim SaveFileDialog As New SaveFileDialog
        SaveFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        SaveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"

        If (SaveFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            Dim FileName As String = SaveFileDialog.FileName
            ' TODO: Add code here to save the current contents of the form to a file.
        End If
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles btnMasterfile.Click
        mtcMenu.SelectedTab = MetroTabPage1
        btnMasterfile.BackColor = Color.Blue
        btnPurchasing.BackColor = Color.Transparent
        btnCollection.BackColor = Color.Transparent
        btnSales.BackColor = Color.Transparent
        btnInventory.BackColor = Color.Transparent
        btnGeneralJournal.BackColor = Color.Transparent
        btnDisbursement.BackColor = Color.Transparent
        btnPosting.BackColor = Color.Transparent
        btnReports.BackColor = Color.Transparent
        btnOtherModules.BackColor = Color.Transparent
        btnProduction.BackColor = Color.Transparent
        btnCoop.BackColor = Color.Transparent
    End Sub

    Private Sub btnPurchasing_Click(sender As System.Object, e As System.EventArgs) Handles btnPurchasing.Click
        mtcMenu.SelectedTab = MetroTabPage2
        btnMasterfile.BackColor = Color.Transparent
        btnPurchasing.BackColor = Color.Blue
        btnCollection.BackColor = Color.Transparent
        btnSales.BackColor = Color.Transparent
        btnInventory.BackColor = Color.Transparent
        btnGeneralJournal.BackColor = Color.Transparent
        btnDisbursement.BackColor = Color.Transparent
        btnPosting.BackColor = Color.Transparent
        btnReports.BackColor = Color.Transparent
        btnOtherModules.BackColor = Color.Transparent
        btnProduction.BackColor = Color.Transparent
        btnCoop.BackColor = Color.Transparent
    End Sub

    Private Sub btnCollection_Click(sender As System.Object, e As System.EventArgs) Handles btnCollection.Click
        mtcMenu.SelectedTab = MetroTabPage3
        btnMasterfile.BackColor = Color.Transparent
        btnPurchasing.BackColor = Color.Transparent
        btnCollection.BackColor = Color.Blue
        btnSales.BackColor = Color.Transparent
        btnInventory.BackColor = Color.Transparent
        btnGeneralJournal.BackColor = Color.Transparent
        btnDisbursement.BackColor = Color.Transparent
        btnPosting.BackColor = Color.Transparent
        btnReports.BackColor = Color.Transparent
        btnOtherModules.BackColor = Color.Transparent
        btnProduction.BackColor = Color.Transparent
        btnCoop.BackColor = Color.Transparent
    End Sub

    Private Sub btnSales_Click(sender As System.Object, e As System.EventArgs) Handles btnSales.Click
        mtcMenu.SelectedTab = MetroTabPage4
        btnMasterfile.BackColor = Color.Transparent
        btnPurchasing.BackColor = Color.Transparent
        btnCollection.BackColor = Color.Transparent
        btnSales.BackColor = Color.Blue
        btnInventory.BackColor = Color.Transparent
        btnGeneralJournal.BackColor = Color.Transparent
        btnDisbursement.BackColor = Color.Transparent
        btnPosting.BackColor = Color.Transparent
        btnReports.BackColor = Color.Transparent
        btnOtherModules.BackColor = Color.Transparent
        btnProduction.BackColor = Color.Transparent
        btnCoop.BackColor = Color.Transparent
    End Sub

    Private Sub btnInventory_Click(sender As System.Object, e As System.EventArgs) Handles btnInventory.Click
        mtcMenu.SelectedTab = MetroTabPage5
        btnMasterfile.BackColor = Color.Transparent
        btnPurchasing.BackColor = Color.Transparent
        btnCollection.BackColor = Color.Transparent
        btnSales.BackColor = Color.Transparent
        btnInventory.BackColor = Color.Blue
        btnGeneralJournal.BackColor = Color.Transparent
        btnDisbursement.BackColor = Color.Transparent
        btnPosting.BackColor = Color.Transparent
        btnReports.BackColor = Color.Transparent
        btnOtherModules.BackColor = Color.Transparent
        btnProduction.BackColor = Color.Transparent
        btnCoop.BackColor = Color.Transparent
    End Sub

    Private Sub btnGeneralJournal_Click(sender As System.Object, e As System.EventArgs) Handles btnGeneralJournal.Click
        mtcMenu.SelectedTab = MetroTabPage6
        btnMasterfile.BackColor = Color.Transparent
        btnPurchasing.BackColor = Color.Transparent
        btnCollection.BackColor = Color.Transparent
        btnSales.BackColor = Color.Transparent
        btnInventory.BackColor = Color.Transparent
        btnGeneralJournal.BackColor = Color.Blue
        btnDisbursement.BackColor = Color.Transparent
        btnPosting.BackColor = Color.Transparent
        btnReports.BackColor = Color.Transparent
        btnOtherModules.BackColor = Color.Transparent
        btnProduction.BackColor = Color.Transparent
        btnCoop.BackColor = Color.Transparent
    End Sub

    Private Sub btnDisbursement_Click(sender As System.Object, e As System.EventArgs) Handles btnDisbursement.Click
        mtcMenu.SelectedTab = MetroTabPage7
        btnMasterfile.BackColor = Color.Transparent
        btnPurchasing.BackColor = Color.Transparent
        btnCollection.BackColor = Color.Transparent
        btnSales.BackColor = Color.Transparent
        btnInventory.BackColor = Color.Transparent
        btnGeneralJournal.BackColor = Color.Transparent
        btnDisbursement.BackColor = Color.Blue
        btnPosting.BackColor = Color.Transparent
        btnReports.BackColor = Color.Transparent
        btnOtherModules.BackColor = Color.Transparent
        btnProduction.BackColor = Color.Transparent
        btnCoop.BackColor = Color.Transparent
    End Sub

    Private Sub btnPosting_Click(sender As System.Object, e As System.EventArgs) Handles btnPosting.Click
        mtcMenu.SelectedTab = MetroTabPage8
        btnMasterfile.BackColor = Color.Transparent
        btnPurchasing.BackColor = Color.Transparent
        btnCollection.BackColor = Color.Transparent
        btnSales.BackColor = Color.Transparent
        btnInventory.BackColor = Color.Transparent
        btnGeneralJournal.BackColor = Color.Transparent
        btnDisbursement.BackColor = Color.Transparent
        btnPosting.BackColor = Color.Blue
        btnReports.BackColor = Color.Transparent
        btnOtherModules.BackColor = Color.Transparent
        btnProduction.BackColor = Color.Transparent
        btnCoop.BackColor = Color.Transparent
    End Sub

    Private Sub btnReports_Click(sender As System.Object, e As System.EventArgs) Handles btnReports.Click
        mtcMenu.SelectedTab = MetroTabPage9
        btnMasterfile.BackColor = Color.Transparent
        btnPurchasing.BackColor = Color.Transparent
        btnCollection.BackColor = Color.Transparent
        btnSales.BackColor = Color.Transparent
        btnInventory.BackColor = Color.Transparent
        btnGeneralJournal.BackColor = Color.Transparent
        btnDisbursement.BackColor = Color.Transparent
        btnPosting.BackColor = Color.Transparent
        btnReports.BackColor = Color.Blue
        btnOtherModules.BackColor = Color.Transparent
        btnProduction.BackColor = Color.Transparent
        btnCoop.BackColor = Color.Transparent
    End Sub

    Private Sub btnOtherModules_Click(sender As System.Object, e As System.EventArgs) Handles btnOtherModules.Click
        mtcMenu.SelectedTab = MetroTabPage10
        btnMasterfile.BackColor = Color.Transparent
        btnPurchasing.BackColor = Color.Transparent
        btnCollection.BackColor = Color.Transparent
        btnSales.BackColor = Color.Transparent
        btnInventory.BackColor = Color.Transparent
        btnGeneralJournal.BackColor = Color.Transparent
        btnDisbursement.BackColor = Color.Transparent
        btnPosting.BackColor = Color.Transparent
        btnReports.BackColor = Color.Transparent
        btnOtherModules.BackColor = Color.Blue
        btnProduction.BackColor = Color.Transparent
        btnCoop.BackColor = Color.Transparent
    End Sub

    Private Sub MetroTile9_Click(sender As System.Object, e As System.EventArgs) Handles tilePurchaseOrder.Click
        frmPO.Show()
    End Sub

    Private Sub MetroTile1_Click(sender As System.Object, e As System.EventArgs) Handles tileVCEMaster.Click
        frmVCE_Master.Show()
    End Sub

    Private Sub tileItemMaster_Click(sender As System.Object, e As System.EventArgs) Handles tileItemMaster.Click
        frmItem_Master.Show()
    End Sub

    Private Sub tileChartofAccount_Click(sender As System.Object, e As System.EventArgs) Handles tileChartofAccount.Click
        frmCOA.Show()
    End Sub

    Private Sub tileBankList_Click(sender As System.Object, e As System.EventArgs) Handles tileBankList.Click
        frmMasterfile_Bank.Show()
    End Sub

    Private Sub tileUserMaster_Click(sender As System.Object, e As System.EventArgs) Handles tileUserMaster.Click
        If Not AllowAccess("UAR_VIEW") Then
            msgRestrictedMod()
        Else
            frmUser_Master.Show()
        End If

    End Sub

    Private Sub tileWarehouseMaster_Click(sender As System.Object, e As System.EventArgs) Handles tileWarehouseMaster.Click
        frmWH.Show()
    End Sub

    Private Sub tileBillsofMaterials_Click(sender As System.Object, e As System.EventArgs)
        frmBOM_FG.Show()
    End Sub

    Private Sub MainForm1_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        'To avoid borderless form cover the taskbar
        Me.WindowState = FormWindowState.Normal
        Me.StartPosition = FormStartPosition.Manual
        With Screen.PrimaryScreen.WorkingArea
            Me.SetBounds(.Left, .Top, .Width, .Height)
        End With

        mtcMenu.SelectedTab = MetroHomeTab

    End Sub

    Private Sub MetroTile8_Click(sender As System.Object, e As System.EventArgs) Handles tilePurchaseRequisition.Click
        frmPR.Show()
    End Sub

    Private Sub MetroTile10_Click(sender As System.Object, e As System.EventArgs) Handles tileAccountsPayableVoucher.Click
        frmAPV.Show()
    End Sub

    Private Sub tileOR_Click(sender As System.Object, e As System.EventArgs) Handles tileOfficialReceipt.Click
        frmCollection.TransType = "OR"
        frmCollection.Book = "Cash Receipts"
        frmCollection.Show()
    End Sub

    Private Sub tileCollectionReceipt_Click(sender As System.Object, e As System.EventArgs) Handles tileCollectionReceipt.Click
        frmCollection.TransType = "CR"
        frmCollection.Book = "Cash Receipts"
        frmCollection.Show()
    End Sub

    Private Sub tileBankDeposit_Click(sender As System.Object, e As System.EventArgs) Handles tileBankDeposit.Click
        frmBD.Show()
    End Sub

    Private Sub tileAcknowledgementReceipt_Click(sender As System.Object, e As System.EventArgs) Handles tileAcknowledgementReceipt.Click
        frmCollection.TransType = "AR"
        frmCollection.Book = "Cash Receipts"
        frmCollection.Show()
    End Sub

    Private Sub tileBankRecon_Click(sender As System.Object, e As System.EventArgs) Handles tileBankRecon.Click
        FrmBR.Show()
    End Sub

    Private Sub tileSalesOrder_Click(sender As System.Object, e As System.EventArgs) Handles tileSalesOrder.Click
        frmSO.Show()
    End Sub

    Private Sub tileSalesInvoice_Click(sender As System.Object, e As System.EventArgs) Handles tileSalesInvoice.Click
        frmSI.Show()
    End Sub

    Private Sub tileReceivingReport_Click(sender As System.Object, e As System.EventArgs) Handles tileRR.Click
        frmRR.Show()
    End Sub

    Private Sub tileDeliveryReceipt_Click(sender As System.Object, e As System.EventArgs) Handles tileDR.Click
        frmDR.Show()
    End Sub

    Private Sub tileInventoryTransfer_Click(sender As System.Object, e As System.EventArgs) Handles tileIT.Click
        frmIT.Show()
    End Sub

    Private Sub tileJournalVoucher_Click(sender As System.Object, e As System.EventArgs) Handles tileJournalVoucher.Click
        frmJV.Show()
    End Sub

    Private Sub tileCashDisbursement_Click(sender As System.Object, e As System.EventArgs) Handles tileCashDisbursement.Click
        frmCV.Show()
    End Sub

    Private Sub tileAdvances_Click(sender As System.Object, e As System.EventArgs) Handles tileAdvances.Click
        frmAdvances.Show()
    End Sub

    Private Sub tileBatchPosting_Click(sender As System.Object, e As System.EventArgs) Handles tileBatchPosting.Click
        frmPosting_Main.Show()
    End Sub

    Private Sub picLogout_Click(sender As System.Object, e As System.EventArgs) Handles picLogout.Click
        Dim myForms As FormCollection = Application.OpenForms
        Dim listBox As New ListBox
        For Each frmName As Form In myForms
            listBox.Items.Add(frmName.Name.ToString)
        Next
        If listBox.Items.Count > 3 Then
            If MsgBox("There are still opened forms, Are you sure you want to logout?", MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                For Each item In listBox.Items
                    If item.ToString <> "" AndAlso item.ToString <> "MainForm" AndAlso item.ToString <> "LoadingScreen" Then
                        Dim myForm As Form = Application.OpenForms(item.ToString)
                        myForm.Close()
                    End If
                Next
                frmUserLogin.Show()
                Me.Close()
            End If
        Else
            frmUserLogin.Show()
            Me.Close()
        End If
        SQL.CloseCon()
    End Sub

    Private Sub picLogout_MouseEnter(sender As Object, e As System.EventArgs) Handles picLogout.MouseHover
        picLogout.BackColor = Color.Blue
    End Sub

    Private Sub picLogout_MouseLeave(sender As Object, e As System.EventArgs) Handles picLogout.MouseLeave
        picLogout.BackColor = Color.Transparent
    End Sub

    Private Sub picSettings_MouseEnter(sender As Object, e As System.EventArgs) Handles picSettings.MouseEnter
        picSettings.BackColor = Color.Blue
    End Sub

    Private Sub picSettings_MouseLeave(sender As Object, e As System.EventArgs) Handles picSettings.MouseLeave
        picSettings.BackColor = Color.Transparent
    End Sub

    Private Sub MetroTile2_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile2.Click
        frmGLFinacialReportGenerator.Show()
    End Sub

    Private Sub MetroTile1_Click_1(sender As System.Object, e As System.EventArgs) Handles MetroTile1.Click
        frmReport_Generator.Show()
    End Sub

    Private Sub picSettings_Click(sender As System.Object, e As System.EventArgs) Handles picSettings.Click
        frmSettings.Show()
    End Sub

    Private Sub MetroTile4_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile4.Click
        frmBS_Software.Show()
    End Sub

    Private Sub MetroTile3_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile3.Click
        frmBilling_Manpower.Show()
    End Sub

    Private Sub MetroTile5_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile5.Click
        frmSP.Show()
    End Sub

    Private Sub MetroTile6_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile6.Click
        frmSC.Show()
    End Sub

    Private Sub MetroTile7_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile7.Click
        frmUploader.Show()
    End Sub

    Private Sub MetroTile8_Click_1(sender As System.Object, e As System.EventArgs) Handles MetroTile8.Click
        frmUploadHistory.Show()
    End Sub

    Private Sub MetroTile9_Click_1(sender As System.Object, e As System.EventArgs) Handles MetroTile9.Click
        frmRFP.Show()
    End Sub

    Private Sub MetroTile10_Click_1(sender As System.Object, e As System.EventArgs) Handles mtCC.Click
        frmCC.Show()
    End Sub

    Private Sub Button2_Click_1(sender As System.Object, e As System.EventArgs) Handles btnProduction.Click
        mtcMenu.SelectedTab = mtpProd
        btnMasterfile.BackColor = Color.Transparent
        btnPurchasing.BackColor = Color.Transparent
        btnCollection.BackColor = Color.Transparent
        btnSales.BackColor = Color.Transparent
        btnInventory.BackColor = Color.Transparent
        btnGeneralJournal.BackColor = Color.Transparent
        btnDisbursement.BackColor = Color.Transparent
        btnPosting.BackColor = Color.Transparent
        btnReports.BackColor = Color.Transparent
        btnOtherModules.BackColor = Color.Transparent
        btnProduction.BackColor = Color.Blue
        btnCoop.BackColor = Color.Transparent
    End Sub

    Private Sub frmJO_Click(sender As System.Object, e As System.EventArgs) Handles tileJO.Click
        frmJO.Show()
    End Sub

    Private Sub frmBOM_Click(sender As System.Object, e As System.EventArgs) Handles tileBOMFG.Click
        frmBOM_FG.Show()
    End Sub

    Private Sub MetroTile12_Click(sender As System.Object, e As System.EventArgs)
        frmBOM_FG.Show()
    End Sub

    Private Sub MetroTile14_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile14.Click
        frmCF.Show()
    End Sub

    Private Sub mtPC_Click(sender As System.Object, e As System.EventArgs) Handles mtPC.Click
        frmPC.Show()
    End Sub

    Private Sub MetroTile10_Click_2(sender As System.Object, e As System.EventArgs) Handles MetroTile10.Click
        frmCompanyProfile.Show()
    End Sub

    Private Sub MetroTile15_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile15.Click
        frmPWH.Show()
    End Sub

    Private Sub btnCoop_Click(sender As System.Object, e As System.EventArgs) Handles btnCoop.Click
        mtcMenu.SelectedTab = mtpCoop
        btnMasterfile.BackColor = Color.Transparent
        btnPurchasing.BackColor = Color.Transparent
        btnCollection.BackColor = Color.Transparent
        btnSales.BackColor = Color.Transparent
        btnInventory.BackColor = Color.Transparent
        btnGeneralJournal.BackColor = Color.Transparent
        btnDisbursement.BackColor = Color.Transparent
        btnPosting.BackColor = Color.Transparent
        btnReports.BackColor = Color.Transparent
        btnOtherModules.BackColor = Color.Transparent
        btnProduction.BackColor = Color.Transparent
        btnCoop.BackColor = Color.Blue
    End Sub

    Private Sub MetroTile16_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile16.Click
        frmMember_Master.Show()
    End Sub

    Private Sub MetroTile18_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile18.Click
        frmLoan_Setup.Show()
    End Sub

    Private Sub MetroTile19_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile19.Click
        frmLoan_Window.Show()
    End Sub

    Private Sub tilePL_Click(sender As System.Object, e As System.EventArgs) Handles tilePL.Click
        frmPL.Show()
    End Sub

    Private Sub tileBOMSFG_Click(sender As System.Object, e As System.EventArgs) Handles tileBOMSFG.Click
        frmBOM_SFG.Show()
    End Sub

    Private Sub tileBOMconv_Click(sender As System.Object, e As System.EventArgs) Handles tileBOMconv.Click
        frmBOMC.Show()
    End Sub

    Private Sub tileBOMExp_Click(sender As System.Object, e As System.EventArgs) Handles tileBOMExp.Click
        frmBOME.Show()
    End Sub

    Private Sub MetroTile11_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile11.Click
        frmMR.Show()
    End Sub

    Private Sub tilePettyCashFund_Click(sender As System.Object, e As System.EventArgs) Handles tilePettyCashFund.Click
        frmPCV.Show()
    End Sub

    Private Sub MetroTile12_Click_1(sender As System.Object, e As System.EventArgs) Handles MetroTile12.Click
        frmCIP.Show()
    End Sub

    Private Sub MetroTile20_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile20.Click
        frmBS.Show()
    End Sub

    Private Sub MetroTile13_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile13.Click
        frmBI.Show()
    End Sub

    Private Sub MetroTile21_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile21.Click
        frmIA.Show()
    End Sub

    Private Sub Panel3_Paint(sender As System.Object, e As System.Windows.Forms.PaintEventArgs) Handles Panel3.Paint
        Dim query As String

        query = "SELECT Company_Logo, Employer_Name FROM tblcompany"
        SQL.FlushParams()
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Label4.Text = SQL.SQLDR("Employer_Name").ToString
            If Not IsDBNull(SQL.SQLDR("Company_Logo")) Then
                pbPicture.Image = New Bitmap(byteArrayToImage(SQL.SQLDR("Company_Logo")))
                pbPicture.SizeMode = PictureBoxSizeMode.StretchImage
            Else
                LoadDefaultImage()
            End If
        End If
    End Sub
    Private Sub LoadDefaultImage()
        Dim App_Path As String
        App_Path = New System.IO.FileInfo(Application.ExecutablePath).DirectoryName
        pbPicture.ImageLocation = App_Path & "\Images\DefaultLogo.jpg"
        pbPicture.SizeMode = PictureBoxSizeMode.StretchImage
    End Sub


    Private Sub MetroTile25_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile25.Click
        frmGI.Show()
    End Sub

    Private Sub MetroTile26_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile26.Click
        frmGR.Show()
    End Sub

    Private Sub Label4_Click(sender As System.Object, e As System.EventArgs) Handles Label4.Click

    End Sub

    Private Sub MetroTile22_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile22.Click
        frmECS.Show()
    End Sub

    Private Sub MetroTabPage10_Click(sender As System.Object, e As System.EventArgs) Handles MetroTabPage10.Click

    End Sub

    Private Sub MetroTile23_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile23.Click
        frmVerifier.Show()
    End Sub

    Private Sub MetroTile24_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile24.Click
        frmSavings_Maintenance.Show()
    End Sub

    Private Sub MetroTile27_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile27.Click
        frmSavings_Account.Show()
    End Sub

    Private Sub MetroTile28_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile28.Click
        frmSavings_Interest.Show()
    End Sub

    Private Sub MetroTile29_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile29.Click
        frmFund_Maintenance.Show()
        frmFund_Maintenance.Select()
    End Sub

    Private Sub MetroTile30_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile30.Click
        frmCollection.TransType = "CSI"
        frmCollection.Book = "Sales"
        frmCollection.Show()
    End Sub

    Private Sub MetroTile31_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile31.Click
        frmMember_Master.Show()
    End Sub

    Private Sub tileCashAdvance_Click(sender As System.Object, e As System.EventArgs) Handles tileCashAdvance.Click
        frmCashAdvance.Show()
    End Sub

    Private Sub MetroTile32_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile32.Click
        frmPJ.Show()
    End Sub

    Private Sub MetroTile33_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile33.Click
        frmSJ.Show()
    End Sub

    Private Sub MetroTile34_Click(sender As System.Object, e As System.EventArgs) Handles MetroTile34.Click
        frmAuditTrail.Show()
    End Sub
End Class