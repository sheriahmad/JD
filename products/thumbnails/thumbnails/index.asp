<%@ Language=VBSCRIPT %>
 <% Option Explicit %> 
  <% if Session("username") = False then
       Session("lasturl") = "../products/"
	    'Session("lasturl") = True
	   Response.Redirect "../login/"
	   
	End IF 
  %>
<html>
<head>
<title>Jet Parts Engineering</title>
   <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
    <link href="/images/favicon.ico" rel="SHORTCUT ICON">
		<meta http-equiv="Cache-Control" content="no-cache" />

	<meta name="KEYWORDS" content="jet parts engineering, engineering, pma parts, pma, aircraft parts, airplanes, airlines, engine parts, 
	pma supplements, jet parts, commercial aircraft parts, faa, oem parts, replacement aircraft parts, replacement aircraft components, 
	spare aircraft parts, faa pma parts, reverse engineering, part supplier, aircraft part supplier, aircraft part manufacturer, 
	pma part supplier, pma part manufacturer, jpe, jet, parts, airline parts, www.jetpartsengineering.com.">
	
<meta name="description" content="Jet Parts Engineering, headquartered in Seattle, Washington, has forged relationships with the FAA manufacturers, suppliers, DER's and other
involved in the PMA process that enables us to deliver quality products at a low price than other competitors."> 


<!--	begin:	hyperlink style -->
<STYLE>
A:link, A:visited { text-decoration: none }
A:hover { text-decoration: underline }
font {  font-family: Verdana; font-size: 12px}
</STYLE>
<body TOPMARGIN=0 LEFTMARGIN=0 MARGINWIDTH=0 MARGINHEIGHT=0 bgcolor="#CCCCCC">
<table border=0 cellpadding=0 cellspacing=0>
  <tr> 
    <td align=left valign=top width="179" height="279"> 
      <div align="center">
        <p><font face="Verdana"> <a href="/"> <img src="/images/jpe.jpg" border=0 Alt="Jet Parts Engineering"><br>
          </a></font></p>
       <% if Session("Username") <> "" then 
				Response.Write "<p><a href=../Login/index.asp?status=logoff> <b><font face=Verdana size=1 color=blue> <u>Log-Off</u> </font></b></a> &nbsp;&nbsp;</p>"	
				Response.Write "<p><a href=../Changepwd> <b><font face=Verdana size=1 color=blue> <u>Change Password</u> </font></b></a> &nbsp;&nbsp;</p>"	
			else
				Response.Write "<p><a href=../Login> <b><font face=Verdana size=1 color=blue> <u>Log-In</u> </font></b></a> &nbsp;&nbsp;</p>"
			   Response.Write "<p><a href=../Register><b><font face=Verdana size=1 color=blue><u> Register</u></font></b></a> &nbsp;&nbsp;</p> "
		   end if
		  		  if Session("Username") = "admin" then	 
			  Response.Write "<p><a href=../admin/index.asp> <b><font face=Verdana size=1 color=blue> <u>Awaited Users</u> </font></b></a> &nbsp;&nbsp;</p>"
			  Response.Write "<p><a href=../admin/diapprove.asp> <b><font face=Verdana size=1 color=blue> <u>List All Users</u> </font></b></a> &nbsp;&nbsp;</p>"
			  Response.Write "<p><a href=../admin/history.asp> <b><font face=Verdana size=1 color=blue> <u>Users History</u> </font></b></a> &nbsp;&nbsp;</p>"
			  Response.Write "<p><a href=../admin/insert.asp> <b><font face=Verdana size=1 color=blue> <u>Insert/Update PMAs</u> </font></b></a> &nbsp;&nbsp;</p>"
			   Response.Write "<p><a href=../admin/insert_price.asp> <b><font face=Verdana size=1 color=blue> <u>Insert/Change Price</u> </font></b></a> &nbsp;&nbsp;</p>"
		  End if

		  
		 %>
		 <br>
      <img src="/images/products.jpg" align="middle"> 
      </div></td>
    <td align=left valign=top bgcolor="#000000" width=1><font face="Verdana"><img src="/images/clear-pix.gif" border=0 width=1 height=1></font></td>
    <td align=left valign=top width=6><font face="Verdana"><img src="/images/clear-pix.gif" border=0 width=5 height=1></font></td>
    <td align=left valign=bottom width=790> 
      <table border=0 cellpadding=0 cellspacing=0 width="608">
        <tr> 
          <td align=left valign=bottom> <b><font face=Verdana size=1 color="#000000"> 
                                    <a href="/about"><br>About Us</a>
			 &nbsp;&nbsp; 
			 <a href="../products">PMA Supplements</a>
			 &nbsp;&nbsp; 
			<a href="../products">PMA Parts List</a>
			 &nbsp;&nbsp; 
            <a href="../quote">Quote Request</a>
			&nbsp;&nbsp; 
			<a href="../Search">Search</a> 
			&nbsp;&nbsp; 
			<a href="/">Home</a></font></b></td>
        </tr>
        <tr> 
          <td align=left valign=top><font face="Verdana"><img src="/images/clear-pix.gif" border=0 width=1 height=2></font></td>
        </tr>
        <tr> 
          <td align=left valign=top bgcolor=#000000><font face="Verdana"><img src="/images/clear-pix.gif" border=0 width=1 height=1></font></td>
        </tr>
        <tr> 
          <td align=left valign=top><font face="Verdana"><img src="/images/clear-pix.gif" border=0 width=1 height=2></font></td>
        </tr>
      </table><br>
	          <b><font face="Verdana" size="2">PMA Parts List</font></b><BR>
      <p><font face="Verdana, Arial, Helvetica, sans-serif"> <font size="2"> 
        <% Dim strname
	  		strname = Session("username")
			Response.Write "Welcome  " & strname 
			session("lasturl") = ""
	   %>
        </font></font></p>
      <p><font size="3" face="Verdana, Arial, Helvetica, sans-serif"><a href="http://www.jetpartsengineering.com/pdf/supplement1.pdf" target="_new">Download 
        PMA Supplement (Adobe Acrobat PDF Format)</a> <br>

      <a href="http://www.jetpartsengineering.com/pdf/Supplement4.pdf" target="_new">Supplement1B</a><br>
        <a href="http://www.jetpartsengineering.com/pdf/Supplement2AL.pdf" target="_new"> Supplement2AL</a><br>
        <a href="http://www.jetpartsengineering.com/pdf/Supplement3AL.pdf" target="_new"> Supplement3AL</a><br>
        <a href="http://www.jetpartsengineering.com/pdf/Supplement4AL.pdf" target="_new"> Supplement4AL</a><br>
        <a href="http://www.jetpartsengineering.com/pdf/Supplement5AL.pdf" target="_new"> Supplement5AL</a><br>
        <a href="http://www.jetpartsengineering.com/pdf/Supplement6AL.pdf" target="_new"> Supplement6AL</a><br>
        <a href="http://www.jetpartsengineering.com/pdf/supplementavs.pdf" target="_new"> Supplementavs</a><br></font> <!--<p><font size="3" face="Verdana, Arial, Helvetica, sans-serif">If you need 
        to download Adobe Acrobat, click here</font></p>--> </p>
      <p><font face="Verdana, Arial, Helvetica, sans-serif"> <font size="2"> <a href= ../products/excel/JPE_Parts_list.xls > 
        <U> View This List in Excel </U></a></font></font></p>
          
      <p><font face="Verdana, Arial, Helvetica, sans-serif" size="2"><i><b><font color="#330066">For 
        further information on Parts, Pricing &amp; Special discounts on bulk 
        orders, please contact Sales at 206-281-0961.</font></b></i></font></p>
				 
      <table border="1"  width="767">
        <!-- --------------- with database -->
       <!--- #include file = "adovbs.inc" -->
        <% Dim objconn 
			Dim objfso,objfile
		  	Set objfso = Server.CreateObject("Scripting.FileSystemObject")
			set objconn = Server.CreateObject("ADODB.Connection")
			Set objfile = objfso.GetFile(Server.MapPath("/data/jetparts.mdb"))
		    objconn.ConnectionString = "DRIVER={Microsoft Access Driver (*.mdb)};DBQ=" & objfile.Path
			objconn.Open
			set objfso = Nothing
			set objfile = Nothing	
			Dim objrs
			Set objrs =  Server.CreateObject("ADODB.Recordset")
			Dim strsql
			strsql = "Select * from Item_master1 im, price pc where im.oem_pn = pc.oem_pn order by im.Oem_pn"
			objrs.Open strsql,objconn
			%>
			 
        <tr> 
          <td width="83" valign="top"> 
            <div align="center"><b><font face="Verdana, Arial, Helvetica, sans-serif" size="-2">OEM 
              P/N</font></b></div>
          </td>
          <td width="94" valign="top" > 
            <div align="center"><font size="-1" face="Verdana, Arial, Helvetica, sans-serif"><b>PMA 
              P/N</b></font></div>
          </td>
          <td width="113" valign="top" > 
            <div align="center"><font size="-1" face="Verdana, Arial, Helvetica, sans-serif"><b>Description</b></font></div>
          </td>
          <td width="30" valign="top" > 
            <div align="center"><font size="-1" face="Verdana, Arial, Helvetica, sans-serif"><b>ATA 
              </b></font></div>
          </td>
          <td width="119" valign="top" > 
            <div align="center"><font size="-1" face="Verdana, Arial, Helvetica, sans-serif"><b>Eligibility</b></font></div>
          </td>
		    <% Dim item
			  		
					For each item in objrs.Fields
						if item.Name = trim(Session("category")) then
							Response.write "<td width=91 valign=top > "
							Response.Write "<div align=center><font size=-1 face=Verdana, Arial, Helvetica, sans-serif><b>OEM Price</b></font></div>"
							Response.Write "</td>"
			
							Response.Write "<td width=91 valign=top > " 
							Response.Write "<div align=center><font size=-1 face=Verdana, Arial, Helvetica, sans-serif><b>JPE Price</b></font></div>"
							Response.write "</td>"
		  				exit for
						End if
					next
			%>		

          <td width="89" valign="top" > 
            <div align="center"><font size="-1" face="Verdana, Arial, Helvetica, sans-serif"><b>Part 
              Family </b>(PDF Format)</font></div>
          </td>
          <td width="106" valign="top" > 
            <div align="center"><font size="-1" face="Verdana, Arial, Helvetica, sans-serif"><b>Images 
              of Parts</b></font></div>
          </td>
        </tr>
		
		 <% Do while Not objrs.EOF  %>
        <tr> 
          <td width="83" valign="top" > 
            <div align="center"><b><font face="Verdana, Arial, Helvetica, sans-serif" size="2"> 
              <% Response.Write  objrs("Oem_pn")  %>
              </font></b></div>
          </td>
          <td width="94" valign="top" > 
            <div align="center"><b><font face="Verdana, Arial, Helvetica, sans-serif" size="2"> 
              <% Response.Write "<b>" & objrs("JPE_PN") & "</b>" %>
              </font></b></div>
          </td>
          <td width="113" valign="top" > 
            <div align="center"><b><font face="Verdana, Arial, Helvetica, sans-serif" size="2"> 
              <% Response.Write objrs("DESCRIPTION") %>
              </font></b></div>
          </td>
          <td width="30" valign="top" > 
            <div align="center"><b><font face="Verdana, Arial, Helvetica, sans-serif" size="2"> 
              <% Response.Write objrs("ATA") %>
              </font></b></div>
          </td>
          <td width="119" valign="top" > 
            <div align="center"><b><font face="Verdana, Arial, Helvetica, sans-serif" size="2"> 
              <% Response.Write objrs("ELIGIBILITY") %>
              </font></b></div>
          </td>
		  
		     <% 
			  		For each item in objrs.Fields
						if item.Name = trim(Session("category")) then
		             		Response.write "<td width=91 valign=top >"
                            Response.write "<div align=center><b><font face=Verdana, Arial, Helvetica, sans-serif size=2> "
            						
							Response.Write "$" & objrs("OEM_PRICE") 
							
							  Response.Write  "</font></b></div>"
          					  Response.Write "</td>"
						
			  		
				   Response.write "<td width=91 valign=top > "
            	   Response.write "<div align=center><b><font face=Verdana, Arial, Helvetica, sans-serif size=2> "
				
			
						
									Response.Write "$" & objrs(item.name) 
									exit for
								end if
							next 
							set item = nothing
			        Response.write " </font></b></div>"
         			Response.write "</td>	"
				%>              
			

          <td width="89" valign="top" > 
            <div align="center"><b><font face="Verdana, Arial, Helvetica, sans-serif" size="2"> 
              <%
			    Set objfso = Server.CreateObject("Scripting.FileSystemObject")
				if objfso.FileExists(Server.MapPath("/pdf/" &  objrs("Part_Family") & ".pdf")) then 
          	        Response.Write ("<a href= ../pdf/" &  objrs("Part_Family") & ".pdf target=_blank >  Part Family </a>" ) 
					else
				   Response.Write "-"
				end if
				Set objfso = nothing
             %>
              </font></b></div>
          </td>
          <td valign="top"  width="106"> 
            <div align="center"><b><font size="2" face="Verdana, Arial, Helvetica, sans-serif"> 
              <%
			    Set objfso = Server.CreateObject("Scripting.FileSystemObject")
				if objfso.FileExists(Server.MapPath("/products/thumbnails/" &  objrs("JPE_PN") & ".jpg")) then 
          	        'Response.Write ("<a href= ../products/thumbnails/" &  objrs("JPE_PN") & ".jpg >  Picture </a>" )
					Response.Write ("<img src=../products/thumbnails/" & objrs("JPE_PN") &".jpg target=_blank Alt=Jet Parts Engineering><br></a>")
				else
				   Response.Write "-"
				end if
				Set objfso = nothing
             %>
              </font></b></div>
          </td>
        </tr>
        <% objrs.movenext
		loop
		objrs.close
		Set objrs = Nothing
		Set strsql = Nothing
		objconn.Close
		Set objconn = Nothing
		Set objfile = Nothing
		Set objfso = Nothing
		%>
      </table>
      <form name="form1" method="post" action="">
        <hr>			       
	  	  <p ALIGN="left">       
           <b><font face=Verdana size=1 color="#000000">
		           	<a href="../order_parts">Order Parts</a>
			&nbsp;&nbsp; 
			<a href="../Search">Search Parts</a>
			&nbsp;&nbsp; 
			<a href="../Capabilities">Capabilities</a>
			&nbsp;&nbsp; 
            <a href="../contact">Contact Us</a>  
			
			
			</font></b><br><br>			         
      <P ALIGN="left"><font face="Verdana, Arial, Helvetica, sans-serif" size="1">&copy; 
        2004,
	 Jet Parts Engineering, Inc. All rights reserved.</font></P>
		<p><br></p>
        <p>&nbsp;</p>
      </form>
    </table>

</body>
</html>
