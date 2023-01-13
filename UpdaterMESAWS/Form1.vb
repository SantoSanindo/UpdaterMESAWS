Imports System.IO
Imports System.IO.Compression
Imports System.Net
Imports System.Reflection.Emit
Imports System.Xml.XPath
Imports System.Xml
Imports System.Threading

Public Class Form1

    Dim delay_timer As Integer = 0

    Public Sub UnZip()

        'Dim startPath As String = "c:\example\start"
        Dim zipPath As String = Application.StartupPath() & "/main.zip"
        Dim extractPath As String = Application.StartupPath()

        ZipFile.ExtractToDirectory(zipPath, extractPath)

    End Sub
    Public Sub deletingFile()
        Try
            If System.IO.File.Exists(Application.StartupPath() & "/main.zip") = True Then
                Label2.Text = "Deleting !"
                System.IO.File.Delete(Application.StartupPath() & "/main.zip")
                System.IO.Directory.Delete(Application.StartupPath() & "/MESAWSSG_Build-Main", True)
            End If
        Catch ex As Exception

        End Try
    End Sub


    Public Sub download_update()
        Dim wc As New WebClient
        deletingFile()
        AddHandler wc.DownloadProgressChanged, AddressOf downloadProgressChanged
        wc.DownloadFileAsync(New System.Uri("https://codeload.github.com/SantoSanindo/MESAWSSG_Build/zip/refs/heads/main"), Application.StartupPath() & "/main.zip")
    End Sub

    Public Sub downloadProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs)
        Label2.Text = "Downloading !"
        Progress_download.Value = e.ProgressPercentage
        Label1.Text = e.BytesReceived & " / " & e.TotalBytesToReceive
        If e.BytesReceived = e.TotalBytesToReceive Then
            System.Threading.Thread.Sleep(5000)
            Label2.Text = "UnZiping !"
            UnZip()
            Label2.Text = "Copying !"
            CopyDirectory(Application.StartupPath() & "/MESAWSSG_Build-Main", Application.StartupPath())
            Label2.Text = "Deleting !"
            deletingFile()
            Label2.Text = "Finish !"

        End If

    End Sub
    Public Sub CopyDirectory(ByVal sourcePath As String, ByVal destinationPath As String)


        If System.IO.Directory.Exists(sourcePath) Then
            Dim sourceDirectoryInfo As New System.IO.DirectoryInfo(sourcePath)

            ' If the destination folder don't exist then create it
            If Not System.IO.Directory.Exists(destinationPath) Then
                'System.IO.Directory.CreateDirectory(destinationPath)
                MessageBox.Show("Directory doesn't exist !")
                Exit Sub
            End If

            Dim fileSystemInfo As System.IO.FileSystemInfo
            For Each fileSystemInfo In sourceDirectoryInfo.GetFileSystemInfos

                'MessageBox.Show(fileSystemInfo.FullName)

                Dim destinationFileName As String =
                    System.IO.Path.Combine(destinationPath, fileSystemInfo.Name)

                ' Now check whether its a file or a folder and take action accordingly
                If TypeOf fileSystemInfo Is System.IO.FileInfo Then
                    System.IO.File.Copy(fileSystemInfo.FullName, destinationFileName, True)
                Else
                    ' Recursively call the mothod to copy all the neste folders
                    'CopyDirectory(fileSystemInfo.FullName, destinationFileName)
                End If
                Label2.Text = fileSystemInfo.FullName
                Application.DoEvents()
            Next

        Else
            Label2.Text = "Updating Failed !"

        End If

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        download_update()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        delay_timer = delay_timer + 1
        Label3.Text = delay_timer
        If delay_timer = 5 Then Button1_Click(sender, e)
        If delay_timer >= 15 And Label2.Text = "Finish !" Then
            Dim proc As New System.Diagnostics.Process()
            proc = Process.Start("MES APP.exe", "")
            Me.Close()
        End If
    End Sub
End Class
