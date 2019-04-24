﻿Imports System.Data.OleDb
Public Class potongan
    Sub kosong()
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox5.Clear()
        TextBox1.Focus()
    End Sub
    Sub baru()
        TextBox2.Clear()
        TextBox5.Clear()
        TextBox2.Focus()
    End Sub
    Sub ketemu()
        On Error Resume Next
        TextBox2.Text = dr.Item(1)
        TextBox2.Focus()
    End Sub
    Sub tampil()
        da = New OleDbDataAdapter("select * from potongan", conn)
        ds = New DataSet
        da.Fill(ds)
        dgv.DataSource = ds.Tables(0)
        dgv.ReadOnly = True
    End Sub
    Sub carikode()
        cmd = New OleDbCommand("SELECT * FROM potongan where id_potongan='" & TextBox1.Text & "'", conn)
        dr = cmd.ExecuteReader
        dr.Read()
    End Sub
    Sub awal()
        Call kosong()
        Call tampil()
    End Sub
    Private Sub barang_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Call koneksi()
        Call awal()
    End Sub

    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            carikode()
            If dr.HasRows Then
                ketemu()
            Else
                baru()
            End If
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        If TextBox1.Text = "" Or TextBox2.Text = "" Then
            MsgBox("Kode Harus Diisi!")
            TextBox1.Focus()
            Exit Sub
        End If
        Try
            carikode()
            If Not dr.HasRows Then
                Dim simpan As String = "INSERT INTO potongan VALUES ('" & TextBox1.Text & "', '" & TextBox2.Text & "')"
                cmd = New OleDbCommand(simpan, conn)
                cmd.ExecuteNonQuery()
            Else
                Dim edit As String = "UPDATE potongan SET nama_potongan = '" & TextBox2.Text & "' WHERE id_potongan = '" & TextBox1.Text & "'"
                cmd = New OleDbCommand(edit, conn)
                cmd.ExecuteNonQuery()
            End If
            awal()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click
        If TextBox1.Text = "" Then
            MsgBox("Kode Harus Diisi!")
            TextBox1.Focus()
            Exit Sub
        End If
        If MessageBox.Show("Yakin Ingin Dihapus ? ", "PERINGATAN", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            Dim hapus As String = "DELETE * from potongan WHERE id_potongan = '" & TextBox1.Text & "'"
            cmd = New OleDbCommand(hapus, conn)
            cmd.ExecuteNonQuery()
            awal()
        Else
            kosong()
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button3.Click
        Call kosong()
    End Sub

    Private Sub Button4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button4.Click
        Me.Close()
    End Sub

    Private Sub TextBox5_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TextBox5.TextChanged
        da = New OleDbDataAdapter("select * from potongan where nama_potongan like'%" & TextBox5.Text & "%'", conn)
        ds = New DataSet
        da.Fill(ds)
        dgv.DataSource = ds.Tables(0)
        dgv.ReadOnly = True
    End Sub

    Private Sub dgv_CellMouseClick(ByVal sender As Object, ByVal e As DataGridViewCellMouseEventArgs) Handles dgv.CellMouseClick
        On Error Resume Next
        TextBox1.Text = dgv.Rows(e.RowIndex).Cells(0).Value
        carikode()
        If dr.HasRows Then
            ketemu()
        End If
    End Sub
End Class