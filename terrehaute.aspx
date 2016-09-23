<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Code/Site.Master" CodeBehind="TerreHaute.aspx.cs" Inherits="JetParts.Web.Com.terrehaute" %>

<%@ Register TagPrefix="jpe" Src="~/Code/PageHeader.ascx" TagName="PageHeader" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewPortContent" runat="server">
    <jpe:PageHeader ID="pageHeader" MouseOutImage="images/banner_terrehaute_bw.png" MouseOverImage="images/banner_terrehaute_color.png"
        Alt="Jet Parts Engineering Terre Haute Office" runat="server" />
    <div style="width: 660px;" class="contact">
        <h2>
            Daphne, Alabama Office</h2>
        <p class="address">
            Jet Parts Engineering<br />
            2901 Ohio Blvd, Suite 115<br />
            Terre haute IN, 47803
        </p>
       <p class="address">
           
                   
           <iframe src="https://www.google.com/maps/embed?pb=!1m5!3m3!1m2!1s0x886d6ffaf7ba3afb%3A0x2c2760a677a41e58!2s2901+Ohio+Blvd+%23115%2C+Terre+Haute%2C+IN+47803!5e0!3m2!1sen!2sus!4v1389911210538" width="600" height="450" frameborder="0" style="border:0"></iframe>
            
            
            <br />
            <br />
            
        </p>
        <p class="address">
         
            <dl>
                <dt>Telephone</dt><dd>(812) 642-3411</dd>
                
                <dt>E-mail</dt><dd><a href="mailto:sales@jetpartsengineering.com?subject=Alabama Sales Office">sales@jetpartsengineering.com</a></dd>
            </dl>
        </p>
    </div>
</asp:Content>
