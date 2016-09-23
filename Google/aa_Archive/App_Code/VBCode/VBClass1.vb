Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Web.HttpContext




Public Class VBCLass1







    Public Shared Function GradientMeter(ByVal meterwidth As Integer, ByVal meterheight As Integer, ByVal meterPct As Integer, ByVal showPct As Integer, ByVal showText As String, ByVal hotTagText As String, ByVal hotTagTitle As String) As String


        Dim gradientFile As String = "http://cyberwiz.com/images/metergradient.gif"
        Dim thisColor As String = "#ffffff"
        Dim thisText As String = "&nbsp;"
        If showPct Then
            If Left(showText, 1) = "+" Then
                thisText = Mid(showText, 2) & FormatNumber(meterPct, 0) & "% "

            Else
                thisText = FormatNumber(meterPct, 0) & "% " & showText

            End If
        End If
        If meterPct < 60 Then
            thisColor = "#003300"
        ElseIf meterPct < 90 Then
            thisColor = "#ff3300"
        Else
            thisColor = "#ffff00"
        End If

        If meterPct < 0 Then
            meterPct = 0
        ElseIf meterPct > 100 Then
            meterPct = 100
        End If


        GradientMeter = "<table border=1 cellspacing=0 cellpadding=0 bordercolordark='#d3d3d3'><tr><td  style=" & Chr(34) & "width:" & meterwidth & "px;height:" & meterheight & "px;background-image: url(" & gradientFile & "); background-position: " & meterPct & "%  0px; font-family:verdana, arial; font-size:11px; font-weight: 600; text-align:center; vertical-align:middle; color:" & thisColor & ";" & Chr(34)

        If hotTagText > "" Then
            GradientMeter &= " OnMouseOver=" & Chr(34) & "this.style.cursor='hand'; hhta('" & hotTagText & "','" & hotTagTitle & "','','','','','','');" & Chr(34) & " OnMouseOut=" & Chr(34) & " htclose();" & Chr(34)
        End If

        GradientMeter &= ">" & thisText & "</td></tr></table>"





    End Function




    '---------------------------------------------- Send email using CDONT
    Public Shared Function Dosend(ByVal snmto As String, ByVal snmfrom As String, ByVal snmsubject As String, ByVal snmbody As String, ByVal AttachFile As String) As String


        Dim objMessage As Object, objConfig As Object, Flds As Object

        '--Const CdoBodyFormatText = 1
        '--Const CdoBodyFormatHTML = 0
        '--Const CdoMailFormatMime = 0
        '--Const CdoMailFormatText = 1



        objMessage = CreateObject("cdo.message")
        objConfig = CreateObject("cdo.configuration")

        ' Setting the SMTP Server
        Flds = objConfig.Fields
        Flds.Item("http://schemas.microsoft.com/cdo/configuration/sendusing") = 2
        Flds.Item("http://schemas.microsoft.com/cdo/configuration/smtpserver") = "localhost"
        Flds.update()


        objMessage.Configuration = objConfig
        objMessage.To = snmto
        objMessage.From = snmfrom
        objMessage.Subject = snmsubject
        objMessage.TextBody = snmbody
        objMessage.HTMLBody = snmbody



        If AttachFile > "" Then
            objMessage.AddAttachment(AttachFile)
            'objMessage.AttachFile("d:\HostedWebsiteTest\Reports\SalesReports\DailySalesReport20100208.xls")

        End If
        'objMail.AttachFile("d:\images\pic.gif")



        Try
            '--Response.Write("<b>To: " & snmto & "</b><br>")
            objMessage.fields.update()
            objMessage.Send()
        Catch ex As Exception

            Return ex.Message

        End Try

        '--Response.write ("Mail sent...")

        objMessage = Nothing
        objConfig = Nothing

        Return String.Empty


    End Function





    '----------------------------------------------------------  Save Session Variables
    Public Shared Sub WriteSessionVariables(ByVal type As String)



        Dim Conn_MsSql As SqlConnection
        Dim SqlCommand As SqlCommand

        Conn_MsSql = New SqlConnection("Server=sql2k513.discountasp.net;Initial Catalog=SQL2005_623673_jet;Persist Security Info=True;User ID=SQL2005_623673_jet_user;Password=sell737ng")


        '--- LogIn LogOut SControl

        Dim activeLogIn As Short = 0
        If type = "LogOut" Then
            activeLogIn = -1
        End If

        Dim item As String

        Dim thisSessionVariables As String = ""
        Dim firstPlcd As Boolean = False
        Dim thisItem, thisCmd As String
        '--if Session("adminKeepAliveTime")  = "" then
        System.Web.HttpContext.Current.Session("adminKeepAliveTime") = 300
        '--end if

        Dim expiresTime = DateAdd("n", System.Web.HttpContext.Current.Session("adminKeepAliveTime"), Now())

        Dim includeThis As Boolean
        Dim incLp As Short
        Dim doNotRecord() As String = {"SessionID_Record_Exists", "SessionID"}


        'Response.Write("WRITE: Session UserKey: " &  System.Web.HttpContext.Current.Session("userKey") & "<br>")
        'Response.Write("Session UserName: " &  System.Web.HttpContext.Current.Session("userName") & "<br>")
        'Response.Write("type: " & type & "<br>")
        'Response.Write("Session(SessionID_Record_Exists): " &  System.Web.HttpContext.Current.Session("SessionID_Record_Exists") & "<br>")
        'Response.Write("Session(SessionID): " &  System.Web.HttpContext.Current.Session("SessionID") & "<br>")
        'Response.Write("expiresTime: " & expiresTime & "<br>")




        thisCmd = ""

        If (type = "SControl" Or type = "Login") And (System.Web.HttpContext.Current.Session("userKey") > 0) Then
            For Each item In System.Web.HttpContext.Current.Session.Contents
                'Response.Write("SCONTROL - Write " & item & "=" &  System.Web.HttpContext.Current.Session(item) & "<br>")
                'Response.Write("Write VarType " & VarType( System.Web.HttpContext.Current.Session(item)) & "<br>")

                includeThis = True
                For incLp = 0 To UBound(doNotRecord)
                    If item = doNotRecord(incLp) Then
                        includeThis = False
                    End If
                Next incLp


                If includeThis Then
                    If firstPlcd Then
                        thisSessionVariables &= "||:||"
                    Else
                        firstPlcd = True
                    End If

                    Select Case VarType(System.Web.HttpContext.Current.Session(item))
                        Case 8
                            thisItem = Replace(System.Web.HttpContext.Current.Session(item), "=", "::eq::")
                            thisSessionVariables &= VarType(System.Web.HttpContext.Current.Session(item)) & "=" & item & "=" & thisItem
                        Case Else
                            thisSessionVariables &= VarType(System.Web.HttpContext.Current.Session(item)) & "=" & item & "=" & System.Web.HttpContext.Current.Session(item)
                    End Select
                End If



            Next

            thisSessionVariables = Replace(thisSessionVariables, "::qt::", Chr(34))
            thisSessionVariables = Replace(thisSessionVariables, "::sqt::", "'")

            activeLogIn = -1

            If System.Web.HttpContext.Current.Session("IdensSessionID_Record_Exists") Then

                thisCmd = "UPDATE ecom_SessionVars SET SessionVariables = '" & thisSessionVariables & "', LogInExpires='" & expiresTime & "', ActiveLogIn = " & activeLogIn & "  WHERE  SessionID = '" & System.Web.HttpContext.Current.Session("SessionID") & ";"

            Else
                thisCmd = "INSERT INTO ecom_SessionVars (SessionID, SessionVariables, LogInExpires, ActiveLogIn) VALUES ('" & System.Web.HttpContext.Current.Session("SessionID") & "', '" & thisSessionVariables & "', '" & expiresTime & "', " & activeLogIn & ");"

            End If
        ElseIf type = "LogOut" Then

            thisCmd = "UPDATE ecom_SessionVars SET  ActiveLogIn = 0  WHERE SessionID = '" & System.Web.HttpContext.Current.Session("SessionID") & "';"
        ElseIf type = "LogIn" Then

            thisCmd = "UPDATE ecom_SessionVars SET LogInExpires='" & expiresTime & "', ActiveLogIn = -1  WHERE  SessionID = '" & System.Web.HttpContext.Current.Session("SessionID") & "';"

        End If

        If thisCmd > "" Then

            'Response.Write("thisCmd: " & thisCmd & "<br>")




            Try

                SqlCommand = New SqlCommand(thisCmd, Conn_MsSql)
                'SqlCommand.ExecuteNonQuery()


            Catch ex As Exception
                'Debug.WriteLine("SQL ERROR: " & ex.ToString & "<br>")
            End Try
        End If





    End Sub

    '---------------------------------------------------------- Read Session Variables
    Public Shared Sub ReadSessionVariables()

        Dim Conn_MsSql As SqlConnection
        '--Dim SqlCommand As SqlCommand


        Conn_MsSql = New SqlConnection("Server=sql2k513.discountasp.net;Initial Catalog=SQL2005_623673_jet;Persist Security Info=True;User ID=SQL2005_623673_jet_user;Password=sell737ng")

        System.Web.HttpContext.Current.Session("SessionID_Record_Exists") = False
        System.Web.HttpContext.Current.Session("SessionID") = ""

        Dim CookieColl As HttpCookieCollection
        Dim Counter1 As Short
        Dim Cookie As HttpCookie


        Dim Keys(), SubKeys() As String


        '------------------- Set the variables from the cookies
        Dim ha As HttpApplication = HttpContext.Current.ApplicationInstance
        CookieColl = ha.Request.Cookies

        Keys = CookieColl.AllKeys

        For Counter1 = 0 To Keys.GetUpperBound(0)
            Cookie = CookieColl(Keys(Counter1))
            If Cookie.Name = "SessionID" Then

                Cookie = CookieColl("SessionID")
                SubKeys = Cookie.Values.AllKeys

                System.Web.HttpContext.Current.Session("SessionID") = Cookie.Values(0)
            End If
        Next Counter1


        '--Response.Write("Session from Cookie: " &  System.Web.HttpContext.Current.Session("IdensSessionID") & "<br>")
        '---------------- New Session
        If System.Web.HttpContext.Current.Session("SessionID") = "" Then
            System.Web.HttpContext.Current.Session("SessionID") = System.Web.HttpContext.Current.Session.SessionID
            'Response.Write("Into New Session Cookie Response<br>")


            Dim myCookie2 As New HttpCookie("SessionID")
            myCookie2.Value = System.Web.HttpContext.Current.Session("SessionID")
            myCookie2.Expires = DateTime.Now.AddDays(600)
            ha.Response.Cookies.Add(myCookie2)

        End If


        '--ShowCookies()



        'If Not ROSetup("UseCSPSessionOverride") Then
        '    Exit Sub
        'End If




        Dim thisCmd As String = "Select * from ecom_SessionVars  Where SessionID = '" & System.Web.HttpContext.Current.Session("SessionID") & "';"
        Dim vFromDB As String = ""
        Dim veCommand As SqlCommand, veReader As SqlDataReader

        '--Response.Write("thisCmd on Read: " & thisCmd & "<br>")


        veCommand = New SqlCommand(thisCmd, Conn_MsSql)
        veReader = veCommand.ExecuteReader()
        Dim LogInExpires As Date
        Dim ActiveLogIn As Boolean
        Dim SessionVariables As String = String.Empty

        While veReader.Read()
            If Not IsDBNull(veReader("SessionID")) Then
                LogInExpires = veReader("LogInExpires")
                ActiveLogIn = veReader("ActiveLogIn")
                SessionVariables = veReader("SessionVariables")
                System.Web.HttpContext.Current.Session("SessionID_Record_Exists") = True
            End If
        End While
        veReader.Close()



        'Response.Write("Read Record Exists: " &  System.Web.HttpContext.Current.Session("IdensSessionID_Record_Exists") & "<br>")
        'Response.Write("Read Acvitve Login " & ActiveLogIn & "<br>")
        'Response.Write("Login Expires " & LogInExpires & "<br>")

        'Response.Write("Time Diff " & DateDiff("n", LogInExpires, Now()) & "<br>")


        If DateDiff("n", LogInExpires, Now()) > 0 And ActiveLogIn Then
            '--Call Logout()
            '--ActiveLogIn = False
            '-- System.Web.HttpContext.Current.Session("userName") = ""

        End If

        If System.Web.HttpContext.Current.Session("SessionID_Record_Exists") And ActiveLogIn Then

            SessionVariables = Replace(SessionVariables, Chr(34), "::qt::")
            SessionVariables = Replace(SessionVariables, "'", "::sqt::")


            Dim temp(), temp2() As String
            Dim thisVarType As Short
            temp = Split(SessionVariables, "||:||")
            Dim lp As Short

            For lp = 0 To UBound(temp)
                temp2 = Split(temp(lp), "=")
                thisVarType = CInt(temp2(0))

                Select Case thisVarType
                    Case 11 '--- boolean
                        Select Case LCase(temp2(2))
                            Case "true"
                                System.Web.HttpContext.Current.Session(temp2(1)) = True
                            Case "false"
                                System.Web.HttpContext.Current.Session(temp2(1)) = False
                        End Select
                    Case 2, 3, 4, 5, 6 '--- Numberic
                        System.Web.HttpContext.Current.Session(temp2(1)) = temp2(2)
                    Case Else
                        Select Case LCase(temp2(2))
                            Case "true"
                                System.Web.HttpContext.Current.Session(temp2(1)) = True
                            Case "false"
                                System.Web.HttpContext.Current.Session(temp2(1)) = False
                            Case Else
                                System.Web.HttpContext.Current.Session(temp2(1)) = Replace(temp2(2), "::eq::", "=")
                        End Select

                End Select

            Next lp

            'Dim item As String


            'For Each item In  System.Web.HttpContext.Current.Session.Contents
            'Response.Write("Read " & item & "=" &  System.Web.HttpContext.Current.Session(item) & "<br>")
            'Next

        End If


    End Sub

    Public Shared Sub ShowCookies()

        Dim Counter1, Counter2 As Integer
        Dim Keys(), SubKeys() As String
        Dim CookieColl As HttpCookieCollection
        Dim Cookie As HttpCookie

        Dim ha As HttpApplication = HttpContext.Current.ApplicationInstance
        CookieColl = ha.Request.Cookies

        Keys = CookieColl.AllKeys

        For Counter1 = 0 To Keys.GetUpperBound(0)
            Cookie = CookieColl(Keys(Counter1))
            ha.Response.Write("Cookie: " & Cookie.Name & "<br/>")
            ha.Response.Write("Expires: " & Cookie.Expires & "<br/>")

            SubKeys = Cookie.Values.AllKeys

            For Counter2 = 0 To SubKeys.GetUpperBound(0)
                ha.Response.Write("---------------Key: " & CStr(Counter2) & ":" & SubKeys(Counter2) & "<br/>")
                ha.Response.Write("---------------Value: " & CStr(Counter2) & ":" & Cookie.Values(Counter2) & "<br/>")
            Next Counter2
            ha.Response.Write("<br/>")
        Next Counter1

    End Sub

    Public Shared Sub showServerVariables()

        Dim ha As HttpApplication = HttpContext.Current.ApplicationInstance
        For Each x In ha.Request.ServerVariables
            System.Diagnostics.Debug.WriteLine(x & "<br />")
        Next

    End Sub

    Public Shared Sub showSessionVariables()

        'How many session variables are there?
        System.Diagnostics.Debug.WriteLine("There are " & System.Web.HttpContext.Current.Session.Contents.Count & _
                  " Session variables<P>")

        Dim strName As String
        Dim iLoop As Integer
        'Use a For Each ... Next to loop through the entire collection
        For Each strName In System.Web.HttpContext.Current.Session.Contents
            'Is this session variable an array?
            If IsArray(System.Web.HttpContext.Current.Session(strName)) Then
                'If it is an array, loop through each element one at a time
                For iLoop = LBound(System.Web.HttpContext.Current.Session(strName)) To UBound(System.Web.HttpContext.Current.Session(strName))
                    System.Diagnostics.Debug.WriteLine(strName & "(" & iLoop & ") - " & _
                         System.Web.HttpContext.Current.Session(strName)(iLoop) & "<BR>")
                Next
            Else
                'We aren't dealing with an array, so just display the variable
                System.Diagnostics.Debug.WriteLine(strName & " - " & System.Web.HttpContext.Current.Session.Contents(strName) & "<BR>")
            End If
        Next

    End Sub






    Public Shared Function Highlighter(ByVal fieldString As String, ByVal highlightString As String) As String

        Dim beginhl As Short
        '---Dim thisHlColor As String = "yellow"
        Dim OpenSpan As String = "<span style='background-color:#ccccff;'>"
        Dim CloseSpan As String = "</span>"


        If LCase(highlightString) > "" Then
            beginhl = InStr(LCase(fieldString), LCase(highlightString))

            If beginhl = 1 Then
                Highlighter = OpenSpan & Left(fieldString, Len(highlightString)) & CloseSpan
                If Len(fieldString) > Len(highlightString) Then
                    Highlighter &= Mid(fieldString, beginhl + Len(highlightString))
                End If
            ElseIf beginhl > 1 Then

                Highlighter = Left(fieldString, beginhl - 1) & OpenSpan & Mid(fieldString, beginhl, Len(highlightString)) & CloseSpan
                If Len(fieldString) > Len(highlightString) + beginhl Then
                    Highlighter &= Mid(fieldString, beginhl + Len(highlightString))
                End If
            Else
                Highlighter = fieldString
            End If
        Else
            Highlighter = fieldString
        End If






    End Function


   



End Class
