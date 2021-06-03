Imports System.Data.Odbc
Public Class Form_NilaiMahasiswa
    Dim con As OdbcConnection
    Dim dr As OdbcDataReader
    Dim da As OdbcDataAdapter
    Dim ds As DataSet
    Dim dt As DataTable
    Dim cmd As OdbcCommand

    Sub koneksi()
        con = New OdbcConnection
        con.ConnectionString = "dsn=db_tugas_03"
        con.Open()
    End Sub

    Sub simpan()
        koneksi()
        Dim sql As String = "insert into tabel_nilai values('" & Tkode_nilai.Text & "','" & CbNim.Text & "','" & CbMatkul.Text & "','" & Tteori.Text & "','" & Tpraktek.Text &
        "','" & Trata.Text & "')"
        cmd = New OdbcCommand(sql, con)
        cmd.ExecuteNonQuery()
        Try
            MsgBox("Menyimpan data BERHASIL", vbInformation, "INFORMASI")
        Catch ex As Exception
            MsgBox("Menyimpan data GAGAL", vbInformation, "PERINGATAN")
        End Try
    End Sub

    Sub tampil()
        DataGridView1.Rows.Clear()
        Try
            koneksi()
            da = New OdbcDataAdapter("select *from tabel_nilai order by kode_nilai asc", con)
            dt = New DataTable
            da.Fill(dt)
            For Each row In dt.Rows
                DataGridView1.Rows.Add(row(0), row(1), row(2), row(3),
                row(4), row(5))
            Next
            dt.Rows.Clear()
        Catch ex As Exception
            MsgBox("Menampilkan data GAGAL")
        End Try
    End Sub

    Sub tampilNIM()
        cmd = New OdbcCommand("select nim from tabel_mahasiswa", con)
        dr = cmd.ExecuteReader
        CbNim.Items.Clear()
        Do While dr.Read
            CbNim.Items.Add(dr.Item("nim"))
        Loop
    End Sub

    Sub tampilmatkul()
        cmd = New OdbcCommand("select kode from tabel_matkul", con)
        dr = cmd.ExecuteReader
        CbMatkul.Items.Clear()
        Do While dr.Read
            CbMatkul.Items.Add(dr.Item("kode"))
        Loop
    End Sub

    Private Sub Form_NilaiMahasiswa_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tampil()
        tampilmatkul()
        tampilNIM()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        simpan()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        tampil()
    End Sub

    Private Sub Cb1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbNim.SelectedIndexChanged
        cmd = New OdbcCommand("select * from tabel_mahasiswa where nim='" & CbNim.Text & "'", con)
        dr = cmd.ExecuteReader
        dr.Read()
        If dr.HasRows Then
            Tnama.Text = dr.Item("nama_mhs")
            Tkelas.Text = dr.Item("kelas")
        Else
            MsgBox("No.Induk Mahasiswa tidak ada")
        End If
    End Sub

    Private Sub Cb2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbMatkul.SelectedIndexChanged
        cmd = New OdbcCommand("select * from tabel_matkul where kode='" & CbMatkul.Text & "'", con)
        dr = cmd.ExecuteReader
        dr.Read()
        If dr.HasRows Then
            Tmatkul.Text = dr.Item("matakuliah")
            Tdosen.Text = dr.Item("dosen_pengampu")
            Tsks.Text = dr.Item("sks")
        Else
            MsgBox("Kode Matkul tidak ada")
        End If
        Tkode_nilai.Focus()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        CbNim.Text = "-pilih-"
        Tnama.Text = ""
        Tkelas.Text = ""
        CbMatkul.Text = "-pilih"
        Tmatkul.Text = ""
        Tdosen.Text = ""
        Tsks.Text = ""
        Tkode_nilai.Text = ""
        Tteori.Text = "0"
        Tpraktek.Text = "0"
        Trata.Text = "0"
        CbNim.Focus()
    End Sub



    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim a As String = DataGridView1.Item(0, DataGridView1.CurrentRow.Index).Value
        If a = "" Then
            MsgBox("Data Nilai yang dihapus belum DIPILIH")
        Else
            If (MessageBox.Show("Anda yakin menghapus data dengan kode_nilai=" & a & "...?", "Delete", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning) = Windows.Forms.DialogResult.OK) Then
                koneksi()
                cmd = New OdbcCommand("delete from tabel_nilai where kode_nilai='" & a & "'", con)
                cmd.ExecuteNonQuery()
                MsgBox("Menghapus data nilai BERHASIL", vbInformation, "INFORMASI")
                con.Close()
                tampil()
            End If
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        End
    End Sub

    Private Sub Tpraktek_TextChanged(sender As Object, e As EventArgs) Handles Tpraktek.TextChanged
        Dim teori As Integer
        Dim praktek As Integer
        Dim rata As Single
        teori = CInt(Tteori.Text)
        praktek = CInt(Tpraktek.Text)
        rata = CSng((teori + praktek) / 2)
        Trata.Text = rata
    End Sub
End Class