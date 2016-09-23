﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Code/Site.Master" CodeBehind="philanthropy.aspx.cs" Inherits="JetParts.Web.Com.philanthropy" %>

<%@ Register TagPrefix="jpe" Src="~/Code/PageHeader.ascx" TagName="PageHeader" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ViewPortContent" runat="server">
    <jpe:PageHeader ID="pageHeader" MouseOutImage="images/banner_philanthropy_bw.png"
        MouseOverImage="images/banner_philanthropy_color.png" Alt="Philanthropy" runat="server" />
    <div style="width: 660px;">

        <p class="pageBodyPar1">
            At Jet Parts Engineering we believe a successful company must do its part to give
            back to the community. JPE is proud to join forces with charitable organizations,
            such as Water 1st and the Ronald McDonald House Charity, in an effort to better
            the world around us.
        </p>
        <h4 class="paragraphHeaderH4">Water 1st and Carry 5: Walking for Water</h4>
        <img src="images/carry5.jpg" alt="Carry5" style="height: 200; width: 200; margin-right: 10px; float:left;"><br />
        <p class="pageBody">
            Jet Parts Engineering teams with <a href="http://water1st.org/index.html">Water 1st
                International</a> to raise money to provide safe drinking water to poor communities
            around the world. JPE employees participate in the annual <a href="http://carry5.org/">
                Carry 5: Walking for Water</a>. Walking for Water is an interactive fundraising
            activity put on by Water 1st in which participants carry 5 gallons of water over
            a distance of 5 kilometers. This simulates the challenge that many women and children
            around the world face every day in order to provide safe drinking water for their
            families.
            <br />
        </p>
        <div style="clear:both;"></div>
        <h4 class="paragraphHeaderH4">Ronald McDonald House Charity</h4>
        <img src="images/jpe-rmhc200.jpg" alt="JPE at RMHC" style="width:200; height:150; margin-right: 10px; float:left;">
        <p class="pageBody">
            Each year Jet Parts Engineering visits the local <a href="http://www.rmhcseattle.org/">
                Ronald McDonald House Charity</a> in Seattle. The JPE team prepares a warm meal
            for the families staying at the Ronald McDonald House, whose children undergo care
            at nearby Children's Hospital.
            <br />
            <br />
            Pictured to the left is the JPE Team at the Ronald McDonald House in May 2010.
        </p>
        <div style="clear:both;"></div>
    </div>
</asp:Content>