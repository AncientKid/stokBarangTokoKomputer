Imports System.Threading.Tasks
Imports System.Security.Cryptography
Imports System.Text

Public Class Pengguna
    Dim tempUsername As String
    Dim lst As ListViewItem

    Private Sub Pengguna_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call loadGrid(Nothing)
    End Sub

    Private Sub kosong()
        tempUsername = Nothing
        TextBox1.Text = Nothing
        TextBox2.Text = Nothing
        ComboBox1.SelectedIndex = 0
        CheckBox1.Checked = False
    End Sub

    Async Function loadGrid(ByVal cari As String) As Task
        Progress.showProgress(ProgressBar1)

        Dim sql As String

        If cari = Nothing Then
            sql = "select username, tipe, aktif from pengguna"
        Else
            sql = "select username, tipe, aktif from pengguna " & _
                    "where username like '%" & cari & "%'"
        End If

        Dim dt As DataTable = Await Task(Of DataTable).Factory.StartNew(Function() Koneksi.getList(sql))

        ListView1.Items.Clear()
        For Each dr As DataRow In dt.Rows
            lst = ListView1.Items.Add(dr(0))
            lst.SubItems.Add(dr(1))
            lst.SubItems.Add(dr(2))
        Next

        tempUsername = Nothing
        Progress.hideProgress(ProgressBar1)
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim sql, pass As String

        If TextBox2.Text.Trim.Length > 0 Then
            Using md5Hash As MD5 = MD5.Create()
                pass = Hash.GetMd5Hash(md5Hash, TextBox2.Text.Trim)

                If Hash.VerifyMd5Hash(md5Hash, TextBox2.Text.Trim, pass) Then
                    Console.WriteLine("Hash sama")
                Else
                    Console.WriteLine("Hash berbeda")
                End If
            End Using
        Else
            pass = Nothing
        End If

        If (tempUsername <> Nothing) Then
            If pass = Nothing Then
                sql = "update pengguna set tipe = '" & ComboBox1.Text() & "',  " & _
                        "aktif = '" & CheckBox1.CheckState & "' " & _
                        "where username = '" & tempUsername & "'"
            Else
                sql = "update pengguna set tipe = '" & ComboBox1.Text() & "',  " & _
                         "pwd = '" & pass & "', " & _
                         "aktif = '" & CheckBox1.CheckState & "' " & _
                         "where username = '" & tempUsername & "'"
            End If
        Else
            sql = "insert into pengguna (username, pwd, tipe, aktif) " & _
                "values ('" & TextBox1.Text().Trim & "' , '" & pass & "', " & _
                "'" & ComboBox1.Text() & "', '" & CheckBox1.CheckState & "')"
        End If

        Progress.showProgress(ProgressBar1)
        Dim myTask = Task.Factory.StartNew(Sub() Koneksi.exec(sql))
        Task.WaitAll(myTask)
        Progress.hideProgress(ProgressBar1)

        kosong()
        Call loadGrid(Nothing)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If MsgBox("Hapus data " & tempUsername & "?", vbYesNo, "Attention") = MsgBoxResult.No Then Exit Sub

        If (tempUsername = Nothing) Then
            MsgBox("Tak ada data yang akan dihapus", vbOKOnly, "Attention")
            Exit Sub
        End If

        sql = "delete from pengguna where username = '" & TextBox1.Text().Trim & "'"

        Progress.showProgress(ProgressBar1)
        Dim myTask = Task.Factory.StartNew(Sub() Koneksi.exec(sql))
        Task.WaitAll(myTask)
        Progress.hideProgress(ProgressBar1)

        kosong()
        Call loadGrid(Nothing)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Call loadGrid(TextBox3.Text.Trim)
    End Sub

    Private Sub ListView1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListView1.MouseDoubleClick
        With ListView1
            tempUsername = .SelectedItems.Item(0).Text
            TextBox1.Text = tempUsername : TextBox1.Enabled = False
            TextBox2.Text = Nothing
            ComboBox1.Text = .SelectedItems.Item(0).SubItems(1).Text
            CheckBox1.Checked = .SelectedItems.Item(0).SubItems(2).Text
        End With

        MsgBox("Passwordmu telah terenkripsi dengan baik",
               vbOKOnly + vbInformation,
               "Jangan mengisi text password, jika tak ingin mengubah")
    End Sub
End Class