<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmParameterMaster.aspx.cs"
    Inherits="Masters_Carpet_frmParameterMaster" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--User controls regertration--%>
<%@ Register Src="~/UserControls/SearchA.ascx" TagName="SearchA" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc2" %>
<%--User controls regertration--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Enter" />
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Exit" />
    <title></title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function getbacktostepone() {
            window.location = "frmBank1.aspx";
        }
        function onSuccess() {
            setTimeout(okay, 200);
        }
        function onError() {
            setTimeout(getbacktostepone, 200);
        }
        function okay() {
            window.parent.document.getElementById('btnOkay').click();
        }
        function cancel() {
            window.parent.document.getElementById('btnCancel').click();
        }
    </script>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmBank1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <%--Page Design--%>
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
        <tr>
            <td width="75%" height="400" rowspan="2">
                <%--Page Design--%>
                <table>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtParameterid" Visible="false" CssClass="textb" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            ParaParameter Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtParameterName" CssClass="textb" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RfParameter" runat="server" ControlToValidate="txtParameterName"
                                ErrorMessage="Please Enter The PArameter Name">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:ValidationSummary ID="ValidationSummary1" ShowMessageBox="false" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="gvParamerter" runat="server" DataKeyNames="PARAMETER_ID" OnPageIndexChanging="gvParamerter_PageIndexChanging"
                                OnRowDataBound="gvParamerter_RowDataBound" OnSelectedIndexChanged="gvParamerter_SelectedIndexChanged"
                                CssClass="grid-view">
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="BtnNew" runat="server" Text="New" Width="49px" OnClick="BtnNew_Click"
                                CssClass="buttonnorm" />
                            <asp:Button ID="BtnSave" runat="server" Text="Save" Width="49px" OnClick="BtnSave_Click"
                                CssClass="buttonnorm" />
                            <asp:Button ID="BtnClose" runat="server" Text="Close" CssClass="buttonnorm" />
                        </td>
                    </tr>
                </table>
                <%--Page Working table Ends--%>
            </td>
            <td width="25%" height="50%" valign="top">
            </td>
        </tr>
        <tr>
            <%--User Spacipic Job Alert--%>
            <td class="tdstyle" width="25%" height="50%" valign="top" id="trAlert" runat="server">
                <span class="style5">--User Spacipic Job Alert ----- -----<br />
                    ----- </span>
            </td>
            <%--User Spacipic Job Alert--%>
        </tr>
    </table>
    <%--Page Design--%>
    </form>
</body>
</html>
