<%@ Page Title="" Language="C#" MasterPageFile="~/Code/Site.Master" AutoEventWireup="true" CodeBehind="calverton.aspx.cs" Inherits="JetParts.Web.Com.calverton" %>
     

<%@ Register TagPrefix="jpe" Src="~/Code/PageHeader.ascx" TagName="PageHeader" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewPortContent" runat="server">
    <jpe:PageHeader ID="pageHeader" MouseOutImage="images/banner_calverton_bw.png" MouseOverImage="images/banner_calverton_color.png"
        Alt="Jet Parts Engineering Calverton, New York Office" runat="server" />
    <div style="width: 660px;" class="contact">
        <h2>
            Hauppauge, New York Office</h2>
        <p class="address">
            Jet Parts Engineering<br />
            175 Commerce Drive Suite C<br />
            Hauppauge, NY 11788
        </p>
        <p class="address">
         <!--   Map & Directions (Links will open in new window):<br /> -->
            <br />
             <iframe src="https://www.google.com/maps/embed?pb=!1m14!1m8!1m3!1d3019.837752983463!2d-73.2586989!3d40.80955900000001!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x89e8303482e64be5%3A0x30daa29b8449dc23!2s175+Commerce+Dr!5e0!3m2!1sen!2sus!4v1399924350910" width="600" height="450" frameborder="0" style="border:0"></iframe>
        </p>
        <p class="address">
            Contact: Dennis Casale<br />
            <dl>
                <dt>Telephone</dt><dd>(631) 208-0727</dd>
                <dt>E-mail</dt><dd><a href="mailto:sales@jetpartsengineering.com?subject=New York Sales Office">sales@jetpartsengineering.com</a></dd>
            </dl>
        </p>
    </div>
</asp:Content>
