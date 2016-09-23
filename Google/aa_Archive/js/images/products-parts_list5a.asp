<%@ Language=VBSCRIPT %>
 <% Option Explicit %>
   <% if Session("username") = False then
       Session("lasturl") = "products-parts_list2a.asp"
	    Response.Redirect "support_login.asp"
	End IF 
  %>

 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
<head>
	<meta http-equiv="Content-Language" content="en-us">
	<title>Jet Parts Engineering: Parts List</title>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="description" content="data" />
	<meta name="keywords" content="data" />
	<meta name="author" content="Scott Moffat, et al." />
	<link rel="stylesheet" type="text/css" media="screen" href="css/screen.css" />
	<link rel="stylesheet" type="text/css" media="print" href="css/print.css" />
	<style type="text/css">
	
	/*Credits: Dynamic Drive CSS Library */
	/*URL: http://www.dynamicdrive.com/style/ */
	
	.thumbnail {
		position: relative;
		z-index: 0;
		}
	
	.thumbnail:hover {
		background-color: transparent;
		z-index: 50;
		}
	
	.thumbnail span { /*CSS for enlarged image*/
		position: absolute;
		background-color: lightyellow;
		padding: 5px;
		left: -1000px;
		border: 1px dashed gray;
		visibility: hidden;
		color: black;
		text-decoration: none;
		}
	
	.thumbnail span img { /*CSS for enlarged image*/
		border-width: 0;
		padding: 2px;
		}
	
	.thumbnail:hover span { /*CSS for enlarged image on hover*/
		visibility: visible;
		top: 0;
		left: 60px; /*position where enlarged image should offset horizontally */
		}
	
	</style>
<!--
	<link rel="stylesheet" type="text/css" media="screen" href="css/screen-secondary.css" />
-->
	<script type="text/javascript" src="/js/scripts.js"></script>
</head>

<body id="products" class="tertiary">
<!-- DO NOT MOVE! The following AllWebMenus linking code section must always be placed right AFTER the BODY tag-->
<!-- ******** BEGIN ALLWEBMENUS CODE FOR jpe_menu02 ******** -->
<span id='xawmMenuPathImg-jpe_menu02' style='position:absolute;top:-50px;left:0px'><img name='awmMenuPathImg-jpe_menu02' id='awmMenuPathImg-jpe_menu02' src='./awmmenupath.gif' alt=''></span><script type='text/javascript'>var MenuLinkedBy='AllWebMenus [4]', awmBN='626'; awmAltUrl='';</script><script charset='UTF-8' src='./jpe_menu02.js' language='JavaScript1.2' type='text/javascript'></script><script type='text/javascript'>awmBuildMenu();</script>
<!-- ******** END ALLWEBMENUS CODE FOR jpe_menu02 ******** -->
<!--#include file="adovbs.inc"-->
<!--#include file="header_new.inc"-->
	<div id="main">
	
		<table border="0" width="800" id="layout_grid" style="border-collapse: collapse" cellpadding="0">
<!-- ROW 01: -->
			<tr id="menu">
<!-- NAV: GLOBAL -->
				<td id="awmAnchor-jpe_menu02" colspan="7" width="800">
				&nbsp;<br/>
<!--
					<map name="FPMap0">
					<area href="index.htm" shape="rect" coords="6, 2, 145, 29">
					<area href="test_about_us3.stm" shape="rect" coords="150, 2, 301, 30">
					<area href="products.stm" shape="rect" coords="304, 3, 447, 30">
					<area href="support.stm" shape="rect" coords="450, 2, 598, 30">
					<area href="contact.stm" shape="rect" coords="604, 2, 758, 29">
					</map>
					<img src="images/placeholder_menu.gif" width="760" height="31" usemap="#FPMap0"/>
-->
				</td>
			</tr>
			
<!-- ROW 02: -->
			<tr id="focus"> 
				<td id="article_style-01" width="800" style="padding: 2em;">
				<%dim strtemp
		 If Request("siteval")="search" then 
		     strtemp = Request("combo")
		     if strtemp = "JPE_PN" then 
		 		 strtemp = "PMA_PN"
	         End if
		     Response.Write " <b> You Searched for  " &  strtemp & " ----> " & Request("text1") & "</b>"
		  elseif Request("siteval")="abc" then 
		    
		     Response.Write " <b> You Searched for  Chapter: " & Request("ata") & "</b>"
          End if
          
		  set strtemp = Nothing
		
			Dim objfso,objfile
		  	Set objfso = Server.CreateObject("Scripting.FileSystemObject")
			Set objfile = objfso.GetFile(Server.MapPath("/data/jetparts.mdb"))
		    Dim objconn 
			set objconn = Server.CreateObject("ADODB.Connection")
			objconn.ConnectionString = "DRIVER={Microsoft Access Driver (*.mdb)};DBQ=" & objfile.Path
			objconn.Open
			Set objfso = Nothing
			Set objfile = Nothing
			Dim objrs,strfld,strval,strsql,strata,strsite
			If Request("siteval")= "search" then 
				strfld = Request("combo")

				strval = Ucase(Request("text1"))
			elseif Request("siteval")= "abc" then 
			 	strata = Request("ata")
			End If
			
			strsite = Request("siteval")
				Set objrs =  Server.CreateObject("ADODB.Recordset")


            if strsite="search" then
                
     			strsql = "Select * from Item_master1 im,price pc where im.oem_pn = pc.oem_pn AND im."& strfld & " LIKE '%" &trim(strval) &"%' order by im.Oem_pn"
	        elseif strsite="abc" then
	            	
			    strsql = "Select * from Item_master1 im, price pc where im.oem_pn = pc.oem_pn and im.ata = '"& strata & "' order by im.Oem_pn "
		    else
		         strsql = "Select * from Item_master1 im, price pc where im.oem_pn = pc.oem_pn order by im.Oem_pn "
		    end if

			objrs.Open strsql,objconn
            if objrs.eof then 
			   Response.Write "" & _
				
                "<font face=Verdana, Arial, Helvetica, sans-serif size=3 color=green>" & _
				 "<b><i>" & "&nbsp;&nbsp;&nbsp;        0 Result Found" & "</b></i>" & _
			   "</font>" 
             
			 end if
			  %>

					<h2><a name="parts_list">Parts List</a> (View <a href="#supplements_list">Supplements List</a>)</h2>
					<p>For further information on Parts, Pricing &amp; Special discounts on bulk orders, please contact your JPE Sales Representative at (206) 281-0961.</p>
					<table class="parts_list">
						<thead>
							<tr>
<!-- PART NUMBER -->
								<th colspan="1">Part Number</th>
<!-- PRODUCT DETAIL -->
								<th colspan="4" class="border_left_white">Details</th>
<!-- PRICE INFORMATION -->
								<th colspan="1" class="border_left_white">Price</th>
<!-- PRODUCT DETAIL -->
								<th colspan="2" class="border_left_white">Supplement</th>
							</tr>
							<tr>
<!-- PART NUMBER -->
								<th>OEM<br /><span class="achtung">PMA</span></th>
<!-- PART NUMBER -->
								<th class="border_left_white">Descr.</th>
								<th>Assem.</th>
								<th>ATA</th>
								<th>Elig.</th>
<!-- PRICE INFORMATION -->
							
								<th class="border_left_white">JPE</th>
<!-- PRODUCT DETAIL -->
								<th class="border_left_white">Image</th>
								<th>PMA #</th>						
							</tr>
						</thead>
						
						<tfoot>
							<tr>
<!-- PART NUMBER -->
								<td colspan="1">Part Number</td>
<!-- PRODUCT DETAIL -->
								<td colspan="4" class="border_left_white">Details</td>
<!-- PRICE INFORMATION -->
								<td colspan="1" class="border_left_white">Price</td>
<!-- PRODUCT DETAIL -->
								<td colspan="2" class="border_left_white">Supplement</td>
							</tr>
							<tr>
<!-- PART NUMBER -->
								<td>OEM<br /><span class="achtung">PMA</span></td>
<!-- PART NUMBER -->
								<td class="border_left_white">Descr.</td>
								<td>Assem.</td>
								<td>ATA</td>
								<td>Elig.</td>
<!-- PRICE INFORMATION -->
							
								<td class="border_left_white">JPE</td>
<!-- PRODUCT DETAIL -->
								<td class="border_left_white">Image</td>
								<td>PMA #</td>						
							</tr>
						</tfoot>
						
					<% Dim item
			  		
					For each item in objrs.Fields
						if item.Name = trim(Session("category")) then
							Response.write "<td> "
							Response.Write ""
							Response.Write "</td>"
			
							Response.Write "<td> " 
							Response.Write ""
							Response.write "</td>"
		  				exit for
						End if
					next
					%>
			
						
						<tbody>
						<%dim flagno
						   flagno = 0 %>
						       <% Do while Not objrs.EOF %>
						    	<% if flagno = 1 then
						    	      Response.Write "<tr class=odd>"
						    	       flagno = 0
						    	   else
						    	       Response.Write "<tr>"
						    	      flagno = 1
						    	   end if
						    	%>
						    	
						    	 
								<td><% Response.Write objrs("Oem_pn")  %><br /><span class="achtung"><% Response.Write objrs("JPE_PN") %></span></td>
								<td class="border_left"><% Response.Write objrs("DESCRIPTION") %></td>
								<td><% Response.Write objrs("USED_ON_ASSEMBLY") %></td>
								<td><% Response.Write objrs("ATA") %></td>
								<td><% Response.Write objrs("ELIGIBILITY") %></td>
								<td class="border_left"><%For each item in objrs.Fields
						if item.Name = trim(Session("category")) then
		             		'Response.write "<td width=91 valign=top >"
                            'Response.write ""
            				'		
						'	Response.Write "$" & objrs("OEM_PRICE") 
							
							
							  'Response.Write  ""
          					  'Response.Write "</td>"
						
						  		
							   'Response.write "<td class=achtung width=91 valign=top > "
			            	   Response.write ""
							   						
								Response.Write "$" & objrs(item.name) 
									exit for
								end if
							next 
							set item = nothing
			        Response.write ""
         			Response.write "</td>	"
				%> 

								
								
			</td>					
			<td class="border_left">
								<%
			    Set objfso = Server.CreateObject("Scripting.FileSystemObject")
				if objfso.FileExists(Server.MapPath("/products/thumbnails/" &  objrs("JPE_PN") & ".jpg")) then 
          	        'Response.Write ("<a class=thumbnail href=#thumb ><img src= ../products/thumbnails/" &  objrs("JPE_PN") & ".jpg width=100px height=66px border=0 /><span><img src= ../products/thumbnails/" &  objrs("JPE_PN") & ".jpg" >  Picture </span></a>" )
					Response.Write ("<img src=../products/thumbnails/" & objrs("JPE_PN") &".jpg target=_blank Alt=Jet Parts Engineering><br /></a>")
				else
				   Response.Write "-"
				end if
				Set objfso = nothing
             %>
             
            </td>
			<td>
			<%  
		  	Set objfso = Server.CreateObject("Scripting.FileSystemObject")
				if objfso.FileExists(Server.MapPath("/pdf/"& objrs("Supplement"))) then 
            	  Response.Write ("<a href= ../pdf/" &  objrs("Supplement") &  " target=_blank><img src=images/pdficon_small.gif border=none /></a>" ) 
				else
				   Response.Write "-"
				end if
				Set objfso = nothing
			   %>
			</td>
			</tr>
				
<% objrs.movenext
		loop
		objrs.close
		Set objrs = Nothing
		
		objconn.Close
		Set objconn = Nothing
		Set objfile = Nothing
		Set objfso = Nothing
		Set Strsql = Nothing
		Set Strfld = Nothing
		Set Strval = Nothing 
	%>

						</tbody>
					</table>

					<br />

					
					</table>
					<p><strong>Tip  </strong>To save PDFs, right-click the link and then click <strong>Save Target As</strong>.</p>
				</td>
			</tr>
			
		</table>
	</div>
	
<!--#include file="footer.inc"-->


</body>

</html>