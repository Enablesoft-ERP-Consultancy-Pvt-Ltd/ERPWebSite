<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Enter" />
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Exit" />
    <script src="Scripts/JScript.js" type="text/javascript"></script>
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <style>
        /* Basics */
        html, body
        {
            padding: 0;
            margin: 0;
            width: 100%;
            height: 100%;
            font-family: "Helvetica Neue" , Helvetica, sans-serif;
            background: #f7e6e6;
        }
        .logincontent
        {
            position: fixed;
            width: 350px;
            height: 300px;
            top: 50%;
            left: 50%;
            margin-top: -150px;
            margin-left: -175px;
            background: #dea774;
            padding-top: 10px;
        }
        .loginheading
        {
            border-bottom: solid 1px #ECF2F5;
            padding-left: 18px;
            padding-bottom: 10px;
            color: #ffffff;
            font-size: 20px;
            font-weight: bold;
            font-family: sans-serif;
            text-align: center;
        }
        label
        {
            color: #ffffff;
            display: inline-block;
            margin-left: 18px;
            padding-top: 10px;
            font-size: 15px;
        }
        input[type=text], input[type=password]
        {
            font-size: 14px;
            padding-left: 10px;
            margin: 10px;
            margin-top: 12px;
            margin-left: 18px;
            width: 300px;
            height: 35px;
            border: 1px solid #ccc;
            border-radius: 2px;
            box-shadow: inset 0 1.5px 3px rgba(190, 190, 190, .4), 0 0 0 5px #f5f5f5;
            font-size: 14px;
        }
        input[type=checkbox]
        {
            margin-left: 18px;
            margin-top: 30px;
        }
        .loginremember
        {
            background: #ECF2F5;
            height: 70px;
            margin-top: 20px;
        }
        .check
        {
            font-family: sans-serif;
            position: relative;
            top: -2px;
            margin-left: 2px;
            padding: 0px;
            font-size: 12px;
            color: #321;
        }
        .loginbtn
        {
            float: right;
            margin-right: 20px;
            margin-top: 20px;
            padding: 6px 20px;
            font-size: 14px;
            font-weight: bold;
            color: #fff;
            background-color: #dbece4;
            background-image: -webkit-gradient(linear, left top, left bottom, from(#1f5d2c), to(#1f5d2c));
            background-image: -moz-linear-gradient(top left 90deg, #1f5d2c 0%, #07c32f 100%);
            background-image: linear-gradient(top left 90deg, #1f5d2c 0%, #1f5d2c 100%);
            border-radius: 30px;
            border: 1px solid #07A8C3;
            cursor: pointer;
        }
        .loginbtn:hover
        {
            background-image: -webkit-gradient(linear, left top, left bottom, from(#b6e2ff), to(#6ec2e8));
            background-image: -moz-linear-gradient(top left 90deg, #b6e2ff 0%, #6ec2e8 100%);
            background-image: linear-gradient(top left 90deg, #b6e2ff 0%, #6ec2e8 100%);
        }
    </style>
</head>
<body bgcolor="#edf3fe">
    <form id="form1" runat="server">
    <div class="logincontent">
        <div class="loginheading">
            <asp:Label ID="logname" Text="Export-Erp..." runat="server" />
        </div>
        <label for="txtUserName">
            User Name:</label>
        <%--<input type="text" id="txtUserName" name="txtUserName" />--%>
        <asp:TextBox ID="txtUser" CssClass="textboxm" Width="295px" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ErrorMessage="*" ForeColor="Red" ControlToValidate="txtUser" runat="server" />
        <label for="txtPassword">
            Password:</label>
        <asp:TextBox ID="txtPassword" CssClass="textboxm" runat="server" Width="295px" TextMode="Password"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtPassword" runat="server" />
        <div class="loginremember">
            <%--<input type="submit" class="loginbtn" value="Login" id="btnSubmit" />--%>
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="loginbtn" 
                            OnClick="btnLogin_Click" />&nbsp;
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <asp:Label ID="lblErr" runat="server" CssClass="" ForeColor="Red"></asp:Label>
        </div>
    </div>
    </form>
</body>
</html>
