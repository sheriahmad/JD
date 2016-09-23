<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Code/Site.Master" CodeBehind="supplements.aspx.cs" Inherits="JetParts.Web.Com.supplements" %>
<%@ Register TagPrefix="jpe" Src="~/Code/PageHeader.ascx" TagName="PageHeader" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewPortContent" runat="server">
    <jpe:PageHeader ID="pageHeader" MouseOutImage="images/banner_supplements_bw.png" MouseOverImage="images/banner_supplements_color.png" Alt="Jet Parts Engineering Supplements" runat="server" />
    <div style="width: 660px;">
        <asp:Repeater ID="SupplementArticles" runat="server">
            <ItemTemplate>
                <!-- STORY <%# Eval("StoryNumber").ToString() %>-->
                <a name="story_<%# Eval("StoryNumber").ToString() %>"></a>
                <h4 class="newsHeadH4">
                    <%# Eval("Title").ToString()%></h4>
                <p class="newsStory">
                    <%# Eval("StoryBody").ToString()%></p>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
