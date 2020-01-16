﻿Public Class Menu
    Private Sub PembelianToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles PembelianToolStripMenuItem1.Click

    End Sub

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

   
    Private Sub Menu_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub PembelianToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles PembelianToolStripMenuItem2.Click
        stok.Show()
        stok.MdiParent = Me
    End Sub
End Class