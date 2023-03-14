<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddInspectionDate.aspx.cs"
    Inherits="Masters_Carpet_AddInspectionDate" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            window.opener.document.getElementById('refreshdatename').click();
            self.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="txtid" runat="server" Visible="False"></asp:TextBox>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td class="tdstyle">
                            Inspection Date Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtShape" runat="server" CssClass="textb">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter Shape Name"
                                ControlToValidate="txtShape" ForeColor="Red" ValidationGroup="m">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:ValidationSummary ID="ValidationSummary1" ShowMessageBox="false" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="LblError" runat="server" Text="" ForeColor="Red"></asp:Label>
                            <asp:GridView ID="gvShape" runat="server" OnRowDataBound="gvShape_RowDataBound" AllowPaging="True"
                                PageSize="6" OnSelectedIndexChanged="gvShape_SelectedIndexChanged" OnPageIndexChanging="gvShape_PageIndexChanging"
                                DataKeyNames="Sr_No" Width="274px" CssClass="grid-view" OnRowCreated="gvShape_RowCreated">
                                <HeaderStyle CssClass="gvheader" />
                                <AlternatingRowStyle CssClass="gvalt" />
                                <RowStyle CssClass="gvrow" />
                                <PagerStyle CssClass="PagerStyle" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="BtnNew" runat="server" Text="New" OnClick="BtnNew_Click" Width="49px"
                                Visible="false" CssClass="buttonnorm" />
                            <asp:Button ID="BtnSave" runat="server" Text="Save" OnClientClick="return confirm('Do you want to save data?')"
                                OnClick="BtnSave_Click" ValidationGroup="m" CssClass="buttonnorm" />
                            <asp:Button ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                OnClick="BtnClose_Click" CssClass="buttonnorm" />
                            <asp:Button ID="btndelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"
                                CssClass="buttonnorm" OnClick="btndelete_Click" Visible="False" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
