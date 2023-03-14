<%@ Page Title="" Language="C#" MasterPageFile="~/Erplogin.master" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="ForgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

   
<div align="center">
<table style="width:500px; margin-left:50px;"><tr><td style="height:30px" 
        colspan="2">
         &nbsp;</td></tr>
         

    <tr><td style="height:30px" colspan="2">
         &nbsp;</td></tr>
         

    <tr><td style="height:30px">
         Email Id</td><td style="height:30px; text-align: left;">
         <asp:TextBox ID="txtEmail" runat="server" Width="200px"></asp:TextBox>
        </td></tr>
         

    <tr><td style="height:30px">
        Type the Below Code</td><td style="height:30px; text-align: left;">
    <asp:TextBox ID="txtimgcode" runat="server" style="text-align: left" 
            Width="200px"></asp:TextBox>
    </td></tr>
    <tr><td style="height:30px">
    
    </td><td style="height:30px">
    <asp:Image ID="Image1" runat="server" ImageUrl="~/CImage.aspx"/>        
            </td></tr>
    <tr><td style="height:30px">
        &nbsp;</td><td style="height:30px; text-align: left;">
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Submit" 
            BackColor="#FFFF66" style="text-align: left" />
    </td></tr></table>
    </div>    
</asp:Content>

