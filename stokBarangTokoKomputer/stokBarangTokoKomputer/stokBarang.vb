Imports System.Threading.Tasks

Public Class stokBarang

    Dim dtBarang As DataTable

    Private Sub FBarangPembelian_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        clear()
        Call loadBarang()
    End Sub
    Sub clear()
        tb_qty.Text = ""
    End Sub
    Async Function loadBarang() As Threading.Tasks.Task
        Dim sql As String = "select id_barang, nama from barang"

        MProgress.showProgress(ProgressBar1)

        'ambil data table
        dtBarang = Await Task(Of DataTable).Factory.StartNew(Function() MKoneksi.getList(sql))

        cb_barang.DataSource = dtBarang
        cb_barang.DisplayMember = "nama"
        cb_barang.ValueMember = "id_barang"

        ComboBox1_SelectedIndexChanged(Nothing, Nothing)

        MProgress.hideProgress(ProgressBar1)
    End Function

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub

    Private Sub cb_barang_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cb_barang.SelectedIndexChanged

    End Sub

    Private Sub bt_ok_Click(sender As Object, e As EventArgs) Handles bt_ok.Click
        Dim lst As ListViewItem
        With stok.lv_stok
            lst = .Items.Add(cb_barang.SelectedValue.ToString)
            lst.SubItems.Add(cb_barang.Text)
            lst.SubItems.Add(tb_qty.Text)

        End With
        stok.hitungTotal()
        Close()
    End Sub
End Class