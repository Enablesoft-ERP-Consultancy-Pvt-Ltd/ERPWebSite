<%@ Page Title="Shape" Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="Shape.aspx.cs" Inherits="Masters_Carpet_Shape" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
        function AddItum() {
            window.open('AddItemt.aspx', '', 'width=500px,Height=400px');
            // window.open('AddItemt.aspx','popup Form', 'Width=550px, Height=400px') 
        }
    </script>
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
    <div style="margin-left: 15%; margin-right: 15%; height: 430px">
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
                            <asp:Label ID="lblshapeyname" runat="server" Text="Shape Name" Font-Bold="true"></asp:Label>
                            &nbsp; &nbsp;&nbsp;
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
                </table>
                &nbsp;<table>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="LblError" runat="server" Text="" ForeColor="Red"></asp:Label>
                            <asp:GridView ID="gvShape" runat="server" OnRowDataBound="gvShape_RowDataBound" AllowPaging="True"
                                PageSize="6" OnSelectedIndexChanged="gvShape_SelectedIndexChanged" OnPageIndexChanging="gvShape_PageIndexChanging"
                                DataKeyNames="Sr_No" Width="274px" CssClass="grid-views">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
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
                        <td colspan="2" align="right">
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
</asp:Content>
