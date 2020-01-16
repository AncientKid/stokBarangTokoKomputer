Imports System.Threading.Tasks

Public Class stok
    Dim dtSupplier As DataTable

    Private Sub FPembelian_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call loadSupplier()
        initPembelian()
    End Sub

    Async Function loadSupplier() As Threading.Tasks.Task
        'ambil data supplier dan tampilkan dalam bentuk key - value 
        'seperti <option value="xxx">yyy</option> pada HTML
        Progress.showProgress(ProgressBar1)

        Dim sql As String = "select id_supplier, nama, alamat, telepon from supplier"

        'ambil data table
        dtSupplier = Await Task(Of DataTable).Factory.StartNew(Function() Koneksi.getList(sql))

        cb_supplier.DataSource = dtSupplier
        cb_supplier.DisplayMember = "nama"
        cb_supplier.ValueMember = "id_supplier"

        cb_supplier_SelectedIndexChanged(Nothing, Nothing)

        Progress.hideProgress(ProgressBar1)
    End Function

    Sub initPembelian()
        With lv_stok
            .View = View.Details
            .GridLines = True
            .FullRowSelect = True
            .Columns.Clear()
            .Columns.Add("IDBarang", 120, HorizontalAlignment.Left)
            .Columns.Add("Nama", 200, HorizontalAlignment.Left)
            .Columns.Add("Qty", 70, HorizontalAlignment.Right)
            .Columns.Add("Harga", 120, HorizontalAlignment.Right)
            .Columns.Add("Jumlah", 120, HorizontalAlignment.Right)
        End With
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        stokBarang.ShowDialog()
    End Sub

    Sub initForm()
        lv_stok.Items.Clear()
        cb_supplier.SelectedIndex = 0
        TextBox2.Text = "0"
        TextBox5.Text = Nothing
        DateTimePicker1.Value = Date.Now
    End Sub
    'Sub hitungTotal()
    '    Dim subtotal, total, dp As Long
    '    For i As Integer = 0 To ListView1.Items.Count - 1
    '        subtotal += Val(ListView1.Items(i).SubItems(4).Text)
    '    Next

    '    dp = Val(TextBox1.Text.Trim)
    '    total = subtotal - dp

    '    TextBox3.Text = subtotal : TextBox4.Text = total
    'End Sub

    'Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
    '    hitungTotal()
    'End Sub

    Private Sub ListView1_MouseDown(sender As Object, e As MouseEventArgs) Handles lv_stok.MouseDown

    End Sub

    Private Sub HapusToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HapusToolStripMenuItem.Click
        lv_stok.Items.Remove(lv_stok.SelectedItems(0))
        'hitungTotal()
    End Sub

    Private Sub UbahToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UbahToolStripMenuItem.Click
        With stokBarang
            .cb_barang.SelectedValue = lv_stok.SelectedItems(0).Text
            .tb_qty.Text = lv_stok.SelectedItems(0).SubItems(3).Text
            .Show()
        End With

        lv_stok.Items.Remove(lv_stok.SelectedItems(0))
        'hitungTotal()
    End Sub

    Private Async Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        If (cb_supplier.Text = Nothing Or
            TextBox2.TextLength = 0 Or
            lv_stok.Items.Count = 0) Then

            MsgBox("Lengkapi data")
            Exit Sub
        End If

        Dim sql As String
        If Button2.Text = "Simpan" Then
            sql = "insert into stok (tanggal, id_supplier, term) values (" &
                "'" & DateTimePicker1.Value & "'," & cb_supplier.SelectedValue.ToString & "," &
                "" & Val(TextBox2.Text) & ", " &
                "'" & TextBox5.Text & "')"

            Progress.showProgress(ProgressBar1)
            Dim myTask = Task.Factory.StartNew(Sub() Koneksi.exec(sql))
            Task.WaitAll(myTask)

            Dim dt As DataTable = Await Task(Of DataTable).Factory.StartNew(
                Function() Koneksi.getLastID("pembelian", "id_beli"))
            Dim id_beli As String = dt.Rows(0).Item(0).ToString

            For i As Integer = 0 To lv_stok.Items.Count - 1
                sql = "insert into detail_pembelian values (" & id_beli & ", " &
                    "" & lv_stok.Items(i).Text & ", " &
                    "" & lv_stok.Items(i).SubItems(3).Text & ", " &
                    "" & lv_stok.Items(i).SubItems(2).Text & ");"
                myTask = Task.Factory.StartNew(Sub() Koneksi.exec(sql))
                Task.WaitAll(myTask)
            Next

            Progress.hideProgress(ProgressBar1)

            MsgBox("Data berhasil disimpan", , "Pesan")
        ElseIf Button2.Text = "Perbarui" Then

        End If
        initForm()
    End Sub

    'Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
    '    FCariPembelian.ShowDialog()
    'End Sub

    Async Function display(ByVal id As Integer) As Task
        sql = "select * stok where id_stok = " & id
        Dim dtPemb As DataTable = Await Task(Of DataTable).Factory.StartNew(
            Function() Koneksi.getList(sql))

        sql = "select * detail_stok where id_stok = " & id
        Dim dtDPemb As DataTable = Await Task(Of DataTable).Factory.StartNew(
            Function() Koneksi.getList(sql))

        'cek apakah datanya ada?
        If (dtPemb.Rows.Count = 0) Then Exit Function
        If (dtDPemb.Rows.Count = 0) Then Exit Function
    End Function

    Private Sub cb_supplier_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cb_supplier.SelectedIndexChanged
        Dim sid As String = cb_supplier.SelectedValue.ToString
        Try
            Dim row As DataRow() = dtSupplier.Select("id_supplier = " & sid)

            TextBox2.Text = row(0)(2) & vbCrLf & row(0)(3)
        Catch ex As Exception
            Console.Write(ex.ToString)
        End Try
    End Sub

    Private Sub lv_stok_MouseDown(sender As Object, e As MouseEventArgs) Handles lv_stok.MouseDown
        If lv_stok.Items.Count = 0 Then Exit Sub

        If e.Button = Windows.Forms.MouseButtons.Right Then
            ContextMenuStrip1.Show(MousePosition.X, MousePosition.Y)
        End If
    End Sub

End Class