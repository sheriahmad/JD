<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageHeader.ascx.cs"
    Inherits="JetParts.Web.Com.Code.PageHeader" %>
<div class="pageHeaderDiv" onmouseover="JPEImageSwap('pageHeaderImage', '<%= MouseOverImage %>');"
    onmouseout="JPEImageSwap('pageHeaderImage', '<%= MouseOutImage %>');">
    <img src="<%= MouseOutImage %>" id="pageHeaderImage" name="pageHeaderImage" alt="<%= Alt %>"
        border="0" width="660" /></div>
