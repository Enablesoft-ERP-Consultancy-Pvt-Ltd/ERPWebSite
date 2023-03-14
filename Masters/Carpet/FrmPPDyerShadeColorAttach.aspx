<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmPPDyerShadeColorAttach.aspx.cs"
    EnableEventValidation="false" Inherits="Masters_Carpet_FrmPPDyerShadeColorAttach" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmPPDyerShadeColorAttach.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Preview() {
            window.open('../../ReportViewer.aspx');
        }
        function SaveData() {
            var Message = "";
            if (document.getElementById("ddprocessprogram")) {
                selectedindex = document.getElementById('ddprocessprogram').value;
                if (selectedindex <= 0) {
                    Message = Message + "Please select Process Program No. !!\n";
                }
            }
            if (document.getElementById("DDDyerName")) {
                selectedindex = document.getElementById('DDDyerName').value;
                if (selectedindex <= 0) {
                    Message = Message + "Please select dyer name. !!\n";
                }
            }
            if (Message == "") {
                var a = document.getElementById("Note").value;
                var result;
                result = confirm('Do you want to save data?');
                if (result)
                    return true;
                else
                    return false;
            }
            else {
                alert(Message);
                return false;
            }

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" border="1">
        <tr style="width: 100%" align="center">
            <td height="66px" align="center">
                <%--style="background-image:url(Images/header.jpg)" --%>
                <%--<div><img src="Images/header.jpg" alt="" /></div>--%>
                <asp:Image ID="Image1" ImageUrl="~/Images/header.jpg" runat="server" />
            </td>
            <td style="background-color: #0080C0;" width="100px" valign="bottom">
                <asp:Image ID="imgLogo" align="left" runat="server" Height="66px" Width="111px" />
                <span style="color: Black; margin-left: 30px; font-family: Arial; font-size: xx-large">
                    <strong><em><i><font size="4" face="GEORGIA">
                        <asp:Label ID="LblCompanyName" runat="server" Text=""></asp:Label></font></i></em></strong></span>
                <br />
                <i><font size="2" face="GEORGIA">
                    <asp:Label ID="LblUserName" ForeColor="White" runat="server" Text=""></asp:Label></font></i>
            </td>
        </tr>
        <tr bgcolor="#999999">
            <td class="style1">
                <uc1:ucmenu ID="ucmenu1" runat="server" />
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </td>
            <td width="25%">
                <asp:UpdatePanel ID="up" runat="server">
                    <ContentTemplate>
                        <asp:Button Width="100%" BorderStyle="None" BackColor="#999999" ID="BtnLogout" runat="server"
                            Text="Logout" OnClick="BtnLogout_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <table width="75%">
        <tr>
            <td align="center" height="inherit" valign="top" class="style1" colspan="2">
                <div id="1" style="height: auto" align="left">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div>
                                <table>
                                    <tr>
                                        <td class="tdstyle">
                                            <asp:Label ID="lblprocessprogram" runat="server" Text="Process Program" Width="100%"
                                                CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtprocessprogram" runat="server" Width="100px"
                                                CssClass="textb" OnTextChanged="txtprocessprogram_TextChanged" AutoPostBack="True"></asp:TextBox>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="lblcompany" runat="server" Text="Company Name" Width="100%" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="ddcompany" runat="server" AutoPostBack="True" Width="200px"
                                                CssClass="dropdown" OnSelectedIndexChanged="ddcompany_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="lblprocess" runat="server" Text="Process Name" Width="100%" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="ddprocess" runat="server" Width="150px" CssClass="dropdown"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddprocess_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td id="Tdcustcode" runat="server">
                                            <asp:Label ID="lblcustomer" runat="server" Text="Customer Code" Width="100%" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="ddcustomer" runat="server" AutoPostBack="True" Width="150px"
                                                CssClass="dropdown" OnSelectedIndexChanged="ddcustomer_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblprocessprogram1" runat="server" Text="Process Program" Width="100%"
                                                CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="ddprocessprogram" runat="server" CssClass="dropdown" Width="150px"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddprocessprogram_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Dyer Name" Width="100%" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="DDDyerName" runat="server" CssClass="dropdown" Width="150px"
                                                AutoPostBack="True" OnSelectedIndexChanged="DDDyerName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%">
                                    <tr>
                                        <td class="tdstyle" id="TDProcessItemDetail" runat="server">
                                            <div style="height: 400px; width: 50%; overflow: scroll">
                                                <asp:Label ID="LblProcessItemDetail" runat="server" Text="Process Item Detail" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:CheckBoxList ID="ChkBoxListProcessItemDetail" runat="server" Width="400px" CssClass="checkboxnormal">
                                                </asp:CheckBoxList>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblerror" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                                            <asp:Button ID="btnsave" runat="server" Text="Save" OnClientClick="return SaveData();"
                                                OnClick="btnsave_Click" CssClass="buttonnorm" />
                                            <asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                                CssClass="buttonnorm" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div style="background-color: #edf3fe; text-align: center; height: 43px; width: 100%">
                    <span class="labelcopyr">&copy; Enablesoft Erp Consultancy Pvt. Ltd.All rights reserved.</span>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
