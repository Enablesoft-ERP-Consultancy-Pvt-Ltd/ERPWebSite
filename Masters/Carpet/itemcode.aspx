<%@ Page Title="SubQuality" Language="C#" AutoEventWireup="true" CodeFile="itemcode.aspx.cs"
    Inherits="Masters_Carpet_itemcode" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>itemcode</title>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function priview() {
            window.open("../../ReportViewer.aspx");
        }
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
        function category() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddItemCategory.aspx', '', 'width=901px,Height=401px');
            }
        }
        function additem() {
            var a1 = document.getElementById('TxtFinishedid').value;
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddItemName.aspx?' + a1, '', 'width=901px,Height=401px');
            }
        }
        function addquality() {

            var a2 = document.getElementById('txtqlt').value;
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                window.open('AddQuality.aspx?' + a2, '', 'width=901px,Height=401px');
            }
        }
 
    
    </script>
    <style type="text/css">
        .buttonnormal
        {
        }
    </style>
</head>
<body>
    <form id="itemcode" runat="server">
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
                <uc2:ucmenu ID="ucmenu1" runat="server" />
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
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <table>
                                            <tr>
                                                <td class="tdstyle">
                                                    <asp:Label ID="lblcategoryname" runat="server" Text="Category" Font-Bold="true"></asp:Label>
                                                    <asp:Button ID="refreshcategory" runat="server" Text="Button" OnClick="refreshcategory_Click"
                                                        BackColor="White" BorderColor="White" BorderWidth="0px" EnableTheming="True"
                                                        ForeColor="White" Height="1px" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList CssClass="dropdown" ID="DDCategory" runat="server" Width="150px"
                                                        OnSelectedIndexChanged="DDCategory_SelectedIndexChanged" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <asp:Button ID="btnaddcategory" runat="server" Text="---" CssClass="buttonsmalls "
                                                        Height="21px" OnClientClick="return category();" Width="24px" />
                                                    <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDCategory"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    <asp:Label ID="lblitemname" runat="server" Text="Item Name" Font-Bold="true"></asp:Label>
                                                    <asp:Button ID="refreshitem" runat="server" Text="Button" BackColor="White" BorderColor="White"
                                                        BorderWidth="1px" ForeColor="White" Height="1px" Width="1px" OnClick="refreshitem_Click" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList CssClass="dropdown" ID="DDItemName" runat="server" AutoPostBack="True"
                                                        Width="150px" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:Button ID="btnadditem" runat="server" OnClientClick="return additem(); " Text="---"
                                                        CssClass="buttonsmalls" Height="21px" Width="24px" />
                                                    <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDItemName"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    <asp:Label ID="lblqualityname" runat="server" Text="Quality" Font-Bold="true"></asp:Label>
                                                    <asp:Button ID="refreshquality" runat="server" Text="Button" BackColor="White" BorderColor="White"
                                                        BorderWidth="1px" ForeColor="White" Height="1px" Width="1px" OnClick="refreshquality_Click" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList CssClass="dropdown" ID="DdQuality" runat="server" Width="150px"
                                                        AutoPostBack="True" OnSelectedIndexChanged="DdQuality_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DdQuality"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                    <asp:Button ID="Button4" runat="server" Text="---" CssClass="buttonsmalls" OnClientClick="return addquality();"
                                                        Height="21px" Width="24px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label1" runat="server" Text="Quantity" Font-Bold="true" />
                                                    <asp:Label ID="subquality" runat="server" Text="Label" Visible="False"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TxtQuantity" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    <asp:Label ID="Label2" runat="server" Text=" Unit" Font-Bold="true" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList CssClass="dropdown" ID="DDUnit" Width="100px" runat="server">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDUnit"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td class="style1">
                                    <div style="height: 200px; width: 500px; overflow: auto;">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="Gvitemdetail" runat="server" AutoGenerateColumns="False" DataKeyNames="Quality_Id"
                                                    Width="458px" CssClass="grid-views" OnRowCreated="Gvitemdetail_RowCreated">
                                                    <HeaderStyle CssClass="gvheaders" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <RowStyle CssClass="gvrow" />
                                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Quality_Id" HeaderText="Sr No" />
                                                        <asp:BoundField DataField="item_name" HeaderText="Item Name">
                                                            <ControlStyle Height="17px" />
                                                            <HeaderStyle Width="200px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SubItemName" HeaderText="SubItem Name" />
                                                        <asp:TemplateField HeaderText="Percentage">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtp_tage" runat="server" Width="70px" AutoPostBack="true"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="80px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                            <tr id="ErrorMessage" runat="server" visible="false">
                                <td colspan="2">
                                    <asp:Label ID="lblErrorMessage" runat="server" Font-Bold="true" blink="true" ForeColor="Red"
                                        Text=""></asp:Label>
                                    <asp:Label ID="qualitycode" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="TxtCode" runat="server" Width="300px" ForeColor="Black" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="TxtFinishedid" runat="server" Height="0px" Width="0px" BorderStyle="None"
                                        BorderColor="White" ForeColor="White"></asp:TextBox>
                                    <asp:TextBox ID="txtqlt" runat="server" Height="0px" Width="0px" BorderStyle="None"
                                        BorderColor="White" ForeColor="White"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:TextBox ID="Txtitemcode" runat="server" Width="500px" Height="30px" ForeColor="Black"
                                        Enabled="false"></asp:TextBox>
                                    <asp:Button ID="TxtCHECK" runat="server" Visible="false" Text="CHECK" Width="55px"
                                        Height="23px" CssClass="buttonnorm" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    <div id="divitemcode" runat="server" style="height: 150px; overflow: auto;">
                                        <asp:GridView ID="Gditemcode" runat="server" Height="175px" OnSelectedIndexChanged="Gditemcode_SelectedIndexChanged"
                                            OnRowDataBound=" Gditemcode_RowDataBound" DataKeyNames="Sr_No" CssClass="grid-views"
                                            OnRowCreated="Gditemcode_RowCreated">
                                            <HeaderStyle CssClass="gvheaders" />
                                            <AlternatingRowStyle CssClass="gvalts" />
                                            <RowStyle CssClass="gvrow" />
                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:Button ID="BtnNew" runat="server" Text="New" CssClass="buttonnorm" />
                                    <asp:Button ID="Btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClientClick="return confirm('Do you want to save data?')"
                                        OnClick="Btnsave_Click" />
                                    <asp:Button ID="Button1" runat="server" Text="Preview" CssClass="buttonnorm preview_width"
                                        OnClick="Button1_Click" />
                                    <asp:Button ID="Btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <div style="background-color: #edf3fe; text-align: center; height: 43px; width: 100%">
                    <span class="labelcopyr">&copy; Enablesoft Erp Consultancy Pvt. Ltd.All rights reserved.</span>
                    ights reserved.
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
