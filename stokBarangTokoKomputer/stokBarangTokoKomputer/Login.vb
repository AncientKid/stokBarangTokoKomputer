Imports System.Threading.Tasks
Imports System.Security.Cryptography
Imports System.Text

Public Class Login

    Async Sub bt_login_Click(sender As Object, e As EventArgs) Handles bt_login.Click
        If tb_username.Text.Length = 0 Or tb_password.Text.Length = 0 Then Exit Sub

        Dim pass As String
        If tb_password.Text.Trim.Length > 0 Then
            Using md5Hash As MD5 = MD5.Create()
                pass = Hash.GetMd5Hash(md5Hash, tb_password.Text.Trim)

                If Hash.VerifyMd5Hash(md5Hash, tb_password.Text.Trim, pass) Then
                    Console.WriteLine("Hash sama")
                Else
                    Console.WriteLine("Hash berbeda")
                End If
            End Using
        Else
            pass = Nothing
        End If

        Sql = "select * from pengguna where username = '" & tb_username.Text.Trim & "' and " &
                "pwd = '" & pass & "' and aktif = true"

        Progress.showProgress(ProgressBar1)
        Dim dt As DataTable = Await Task(Of DataTable).Factory.StartNew(Function() Koneksi.getList(sql))
        Progress.hideProgress(ProgressBar1)

        If dt.Rows.Count > 0 Then
            FMenu.Show()
            Me.Hide()
        Else
            MsgBox("Data tak ditemukan", vbOKOnly + vbExclamation, "Pesan")
        End If
    End Sub

    Private Sub bt_clear_Click(sender As Object, e As EventArgs) Handles bt_clear.Click
        tb_username.Text = "" : tb_password.Text = ""
    End Sub
End Class
