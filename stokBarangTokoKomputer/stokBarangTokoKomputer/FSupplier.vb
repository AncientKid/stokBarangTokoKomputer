Imports System.Threading.Tasks

Public Class FSupplier

    Dim tempID As Integer
    Dim lst As ListViewItem

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim sql As String

        If tempID = 0 Then
            sql = "insert into supplier (nama, alamat, telepon) " & _
            "values ('" & TextBox1.Text.Trim & "' , '" & TextBox2.Text.Trim & "', " & _
            "'" & TextBox4.Text.Trim & "')"
        Else
            sql = "update supplier set nama = '" & TextBox1.Text.Trim & "', " & _
                "alamat = '" & TextBox2.Text.Trim & "', telepon = '" & TextBox4.Text.Trim & "' " & _
                "where id_supplier = " & tempID
        End If

        MProgress.showProgress(ProgressBar1)
        Dim myTask = Task.Factory.StartNew(Sub() MKoneksi.exec(sql))
        Task.WaitAll(myTask) 'menunggu hingga selesai
        MProgress.hideProgress(ProgressBar1)

        kosong()
        Call loadGrid(Nothing)
    End Sub

    Sub kosong()
        tempID = 0
        TextBox1.Text = Nothing
        TextBox2.Text = Nothing
        TextBox4.Text = Nothing
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim sql As String = "delete from supplier where id_supplier = " & tempID

        MProgress.showProgress(ProgressBar1)
        Dim myTask = Task.Factory.StartNew(Sub() MKoneksi.exec(sql))
        Task.WaitAll(myTask) 'menunggu hingga selesai
        MProgress.hideProgress(ProgressBar1)

        kosong()
        Call loadGrid(Nothing)
    End Sub

    Private Sub ListView1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseDoubleClick
        With ListView1
            tempID = .SelectedItems.Item(0).Text
            TextBox1.Text = .SelectedItems.Item(0).SubItems(1).Text
            TextBox2.Text = .SelectedItems.Item(0).SubItems(2).Text
            TextBox4.Text = .SelectedItems.Item(0).SubItems(3).Text
        End With
    End Sub

    Async Function loadGrid(ByVal cari As String) As Task
        MProgress.showProgress(ProgressBar1)

        Dim sql As String

        If cari = Nothing Then
            sql = "select * from supplier"
        Else
            sql = "select * from supplier where nama like '%" & cari & "%' " & _
                    "or alamat like '%" & cari & "%' or telepon like '%" & cari & "%'"
        End If

        Dim dt As DataTable = Await Task(Of DataTable).Factory.StartNew(Function() MKoneksi.getList(sql))

        ListView1.Items.Clear()

        For Each dr As DataRow In dt.Rows
            lst = ListView1.Items.Add(dr(0))
            lst.SubItems.Add(dr(1))
            lst.SubItems.Add(dr(2))
            lst.SubItems.Add(dr(3))
        Next

        tempID = 0
        MProgress.hideProgress(ProgressBar1)
    End Function

    Private Sub FNSupplier_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call loadGrid(Nothing)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Call loadGrid(TextBox3.Text.Trim)
    End Sub
End Class
