Option Strict On
Imports System.IO
Imports System.Text

Public Class Form1

    'загрузка из CSV
    Sub loadt(ByRef s As String)
        Dim myStream As Stream = Nothing
        Dim openFileDialog1 As New OpenFileDialog()
        Dim Rf As StreamReader

        openFileDialog1.InitialDirectory = "\"
        openFileDialog1.Filter = "Таблицы (*.csv)|*.csv|Все файлы (*.*)|*.*"
        openFileDialog1.FilterIndex = 1
        openFileDialog1.RestoreDirectory = True

        If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Try
                myStream = openFileDialog1.OpenFile()
                If (myStream IsNot Nothing) Then
                    Rf = New StreamReader(myStream)
                    s = Rf.ReadToEnd()
                End If
            Catch Ex As Exception
                MessageBox.Show("Ошибка при чтении файла: " & Ex.Message)
            Finally
                If (myStream IsNot Nothing) Then
                    myStream.Close()
                End If
            End Try
        End If
    End Sub

    'загрузка из файла
    Sub loadf(ByRef s As String)
        Dim myStream As Stream = Nothing
        Dim openFileDialog1 As New OpenFileDialog()
        Dim Rf As StreamReader

        openFileDialog1.InitialDirectory = "\"
        openFileDialog1.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*"
        openFileDialog1.FilterIndex = 1
        openFileDialog1.RestoreDirectory = True

        If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Try
                myStream = openFileDialog1.OpenFile()
                If (myStream IsNot Nothing) Then
                    Rf = New StreamReader(myStream)
                    s = Rf.ReadToEnd()
                End If
            Catch Ex As Exception
                MessageBox.Show("Ошибка при чтении файла: " & Ex.Message)
            Finally
                If (myStream IsNot Nothing) Then
                    myStream.Close()
                End If
            End Try
        End If
    End Sub

    Function vvod(ByVal TB As TextBox) As String
        Return TB.Text
    End Function

    'вывод CSV
    Sub vivodcsv(ByVal S As String, ByRef DGV As DataGridView)
        Dim str(), h() As String, i, max As Integer
        str = Split(S, vbNewLine)
        max = 0
        For i = 0 To UBound(str) - 1
            h = Split(str(i), ";")
            If UBound(h) > max Then max = UBound(h)
        Next
        For i = 1 To max
        DGV.Columns.Add(CStr(i), CStr(i))
        Next
        For i = 0 To UBound(str) - 1
            h = Split(str(i), ";")
            DGV.Rows.Add(h)
        Next
    End Sub

    'ключ-таблица
    Sub mkey(ByVal S As String, ByRef SKEY() As String)
        Dim i, j As Integer
        SKEY = Split(S, vbNewLine)
        While i < UBound(SKEY) - 1
            delsym(SKEY(i))
            If SKEY(i) = "" Then
                For j = i To UBound(SKEY) - 1
                    SKEY(j) = SKEY(j + 1)
                Next j
                Array.Resize(SKEY, UBound(SKEY))
            Else
                i = i + 1
            End If
        End While
    End Sub

    'Процедура удаления символов из строки
    Sub delsym(ByRef s As String)
        Dim i, C As Integer
        While i < s.Length
            C = Asc(s.Substring(i, 1))
            Select Case C
                Case 65 To 90, 97 To 122, 192 To 255, 168, 184
                    i = i + 1
                Case Else
                    s = s.Remove(i, 1)
            End Select
        End While
    End Sub

    Sub decrypt(ByVal s1 As String, ByVal skey() As String, ByRef s2 As String)
        Dim str(), h(), indexs() As String, i, j As Integer
        str = Split(s1, vbNewLine)
        For i = 0 To UBound(str)
            If str(i) <> "" Then
                h = Split(str(i), ";")
                For j = 0 To UBound(h) - 1
                    If h(j) <> "," And h(j) <> "." And h(j) <> "-" And h(j) <> " " And h(j) <> "" And h(j) <> "?" And h(j) <> "!" And h(j) <> "…" And h(j) <> ":" Then
                        indexs = Split(h(j), ",")
                        s2 = s2 + skey(CInt(indexs(0)) - 1).Substring(CInt(indexs(1)) - 1, 1)
                    Else
                        s2 = s2 + h(j)
                    End If
                Next
                s2 = s2 + vbNewLine
            Else
                If i + 1 <= UBound(str) Then
                    If str(i + 1) = "" Then
                        s2 = s2.Substring(0, s2.Length - 1)
                        i = UBound(str)
                    Else
                        i = i + 1
                    End If
                End If
            End If
        Next
    End Sub

    'Процедура вывода в TextBox
    Overloads Sub vivod(ByVal S As String, ByVal T As TextBox)
        T.Text = S
    End Sub

    'Процедура вывода в TextBox
    Overloads Sub vivod(ByVal S() As String, ByVal T As TextBox)
        Dim i As Integer
        For i = 0 To UBound(S) - 1
            T.Text = T.Text + S(i) & vbNewLine
        Next
    End Sub

    'запись в файл
    Sub savef(ByRef s2 As String)
        Dim saveFileDialog1 As New SaveFileDialog()

        saveFileDialog1.InitialDirectory = "\"
        saveFileDialog1.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*"
        saveFileDialog1.FilterIndex = 1
        saveFileDialog1.RestoreDirectory = True
        saveFileDialog1.Title = "Сохранить полученный файл"
        saveFileDialog1.ShowDialog()

        If saveFileDialog1.FileName <> "" Then
            Try
                My.Computer.FileSystem.WriteAllText(saveFileDialog1.FileName, s2, False)
            Catch ex As Exception
                MsgBox("Ошибка при сохранении (создании) файла")
            End Try
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim s1, ks, s2, skey() As String
        s1 = "" : s2 = "" : ks = ""

        s1 = vvod(TextBox1)
        loadf(ks)
        mkey(ks, skey)
        decrypt(s1, skey, s2)


        vivod(s2, TextBox2)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        End
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim s2 As String
        s2 = vvod(TextBox2)
        savef(s2)
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.Hide()
        Form2.Show()
        Form2.BringToFront()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim s1 As String
        loadt(s1)
        vivod(s1, TextBox1)
    End Sub

    Private Sub Form1_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Form2.Close()
        End
    End Sub
End Class
