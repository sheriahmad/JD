<% if trim(Session("Username")) = "admin" then 
              Response.Write ("<h2>Administration</h2>")
              Response.write ("<ul>")
			  Response.Write ("<li><a href=insert.xxxasp>Insert/Update<br>PMAs</a></li>")
			  Response.Write ("<br>")
			  Response.Write ("<li><a href=insert_price.xxxxasp>Insert/Change<br>Price</a></li>")
			  Response.Write ("<br>")
			  Response.Write ("<li><a href=await.xxxasp>Awaited Users</a></li>")
			  Response.Write ("<br>")
			  Response.Write ("<li><a href=diapprove.xxxasp>List All Users</a></li>")
			  Response.Write ("<br>")
			  Response.Write ("<li><a href=history.xxxasp>Users History</b></li>")
			  Response.write ("</ul>")
	End if
%>