Imports System.Threading.Tasks
Imports System.Security.Cryptography
Imports System.Text

Public Class Barang
    Dim tempID As Integer
    Dim lst As ListViewItem

    Private Sub Barang_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call loadGrid(Nothing)
    End Sub

    Async Function loadGrid(ByVal cari As String) As Task
        Progress.showProgress(ProgressBar1)

        Dim sql As String

        If cari = Nothing Then
            sql = "select * from barang"
        Else
            sql = "select id_barang, nama_barang, stok, rak from barang " & _
                    "where id_barang like '%" & cari & "%'"
        End If

        Dim dt As DataTable = Await Task(Of DataTable).Factory.StartNew(Function() Koneksi.getList(sql))

        ListView1.Items.Clear()
        For Each dr As DataRow In dt.Rows
            lst = ListView1.Items.Add(dr(0))
            lst.SubItems.Add(dr(1))
            lst.SubItems.Add(dr(2))
            lst.SubItems.Add(dr(3))
        Next

        tempID = Nothing
        Progress.hideProgress(ProgressBar1)
    End Function

    Private Sub kosong()
        tempID = 0
        TextBox1.Text = Nothing
        TextBox2.Text = Nothing
        ComboBox1.Text = Nothing
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim sql As String

        If tempID = 0 Then
            sql = "insert into barang (nama_barang, stok, rak) " & _
                "values ('" & TextBox1.Text.Trim & "' , '" & TextBox2.Text.Trim & "', " & _
                "'" & ComboBox1.Text.Trim & "')"
        Else
            sql = "update barang set nama_barang = '" & TextBox1.Text.Trim & "', " & _
                "stok = '" & TextBox2.Text.Trim & "', rak = '" & ComboBox1.Text.Trim & "' " & _
                "where id_barang = " & tempID
        End If

        Progress.showProgress(ProgressBar1)
        Dim myTask = Task.Factory.StartNew(Sub() Koneksi.exec(sql))
        Task.WaitAll(myTask)
        Progress.hideProgress(ProgressBar1)

        kosong()
        Call loadGrid(Nothing)
    End Sub

    Private Sub ListView1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListView1.MouseDoubleClick
        With ListView1
            tempID = .SelectedItems.Item(0).Text
            TextBox1.Text = .SelectedItems.Item(0).SubItems(1).Text
            TextBox2.Text = .SelectedItems.Item(0).SubItems(2).Text
            ComboBox1.Text = .SelectedItems.Item(0).SubItems(3).Text
        End With
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim sql As String = "delete from barang where id_barang = " & tempID

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
End Class
