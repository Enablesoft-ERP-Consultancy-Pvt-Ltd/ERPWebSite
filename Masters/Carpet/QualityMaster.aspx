<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QualityMaster.aspx.cs" Inherits="Masters_Carpet_QualityMaster"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Enter" />
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Exit" />
    <title></title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function priview() {
            window.open('../../ReportViewer.aspx', '');

        }
    </script>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmanager1" runat="server">
    </asp:ScriptManager>
    <table width="100%" border="1" cellspacing="1" cellpadding="1" align="center">
        <asp:Panel runat="server" ID="hs">
            <tr>
                <td width="75%" height="45">
                    <h1 align="center" class="style2">
                        Header</h1>
                </td>
                <td width="25%" valign="bottom">
                    <span class="style4">Welcome User--------------Logout </span>
                </td>
            </tr>
            <tr bgcolor="#999999">
                <td width="75%">
                    <uc2:ucmenu ID="ucmenu1" runat="server" />
                </td>
                <td width="25%">
                    <span class="style5">Home</span>
                </td>
            </tr>
        </asp:Panel>
        <div class="ToolBar">
            <asp:Button ID="Button1" runat="server" Text="New Expanse" Visible="false" CssClass="buttonnorm" />
            <div class="popup_Buttons" style="display: none">
                <input id="btnOkay" value="Done" runat="server" type="button" onclick="okay();" />
                <input id="btnCancel" value="Cancel" type="button" />
            </div>
            <div id="Panel1" style="display: none; vertical-align: top" class="modalPopup">
                <iframe id="frameeditexpanse" frameborder="0" src="UnitMaster.aspx?id=1" height="550"
                    width="950" scrolling="no"></iframe>
            </div>
        </div>
        <div style="margin-left: 15%; margin-right: 15%">
            <table>
                <tr>
                    <td class="tdstyle">
                        Header
                        <asp:TextBox ID="txtid" runat="server" Visible="False"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <asp:UpdatePanel ID="updatepanel1" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td class="tdstyle">
                                Quality Name
                            </td>
                            <td>
                                <asp:TextBox ID="txtQualityName" runat="server" CssClass="textb">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator" runat="server" ValidationGroup="f1"
                                    ErrorMessage="Please Enter  Quality Name" ControlToValidate="txtQualityName">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdstyle">
                                Quality Code
                            </td>
                            <td>
                                <asp:TextBox ID="txtQualityCode" runat="server" CssClass="textb"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="f1"
                                    ErrorMessage="Please Enter  Quality Code" ControlToValidate="txtQualityCode">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2" colspan="2">
                                <asp:GridView ID="gdQuality" runat="server" Width="280px" AllowPaging="True" CellPadding="4"
                                    PageSize="6" ForeColor="#333333" GridLines="None" DataKeyNames="MasterQualityid"
                                    OnPageIndexChanging="gdQuality_PageIndexChanging" OnRowDataBound="gdQuality_RowDataBound"
                                    OnSelectedIndexChanged="gdQuality_SelectedIndexChanged" CssClass="grid-view"
                                    OnRowCreated="gdQuality_RowCreated">
                                    <HeaderStyle CssClass="gvheader" />
                                    <AlternatingRowStyle CssClass="gvalt" />
                                    <RowStyle CssClass="gvrow" />
                                    <PagerStyle CssClass="PagerStyle" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" OnClick="btnsave_Click"
                                    ValidationGroup="f1" />
                                <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" />
                                <asp:Button ID="btnrpt" runat="server" CssClass="buttonnorm" Text="Preview" OnClientClick="priview();" />
                                <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
