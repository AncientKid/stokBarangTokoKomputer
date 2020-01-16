Public Class FMenu

    Private Sub KeluarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles KeluarToolStripMenuItem.Click
        End
    End Sub

    Private Sub SupplierToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SupplierToolStripMenuItem.Click
        FSupplier.Show()
        FSupplier.MdiParent = Me
    End Sub

    Private Sub PenggunaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PenggunaToolStripMenuItem.Click
        Pengguna.Show()
        Pengguna.MdiParent = Me
    End Sub

    Private Sub MinimizeAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MinimizeAllToolStripMenuItem.Click
        For Each form As Form In Me.MdiChildren
            form.WindowState = FormWindowState.Minimized
        Next
    End Sub

    Private Sub CloseAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseAllToolStripMenuItem.Click
        For Each form As Form In Me.MdiChildren
            form.Close()
        Next
    End Sub

    Private Sub NormalAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NormalAllToolStripMenuItem.Click
        For Each form As Form In Me.MdiChildren
            form.WindowState = FormWindowState.Normal
        Next
    End Sub

    Private Sub BarangToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BarangToolStripMenuItem.Click
        Barang.Show()
        Barang.MdiParent = Me
    End Sub

    Private Sub StokToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StokToolStripMenuItem.Click
        stok.Show()
        stok.MdiParent = Me
    End Sub

    Private Sub LaporanToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LaporanToolStripMenuItem.Click
        Laporan.Show()
        Laporan.MdiParent = Me
    End Sub

    Private Sub Menu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        IsMdiContainer = True
    End Sub
End Class