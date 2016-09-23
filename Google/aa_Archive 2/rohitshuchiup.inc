<% if Session("Username") <> "" then 
	Response.Write ("<a href=ecom_login.aspx?status=logoff>Log-Off</a>&nbsp;&nbsp;|&nbsp;&nbsp;")	
	Response.Write ("<a href=ecom_accounts.aspx>Manage My Account</a>&nbsp;&nbsp;|&nbsp;&nbsp;")	
else
	Response.Write ("<a href=ecom_login.aspx>Log-In</a>&nbsp;&nbsp;|&nbsp;&nbsp;")
	Response.Write ("<a href=ecom_accounts.aspx>Sign-up for ECommerce</a>&nbsp;&nbsp;|&nbsp;&nbsp;")
end if %>

<% if trim(Session("Username")) = "admin" then	
	Response.Write ("<a href=await.aspx>Administrator</a>&nbsp;&nbsp;|&nbsp;&nbsp;")
end if %>