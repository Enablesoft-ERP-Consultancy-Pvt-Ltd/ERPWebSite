<%@ Page Title="Color" Language="C#" AutoEventWireup="true" CodeFile="frmColor.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Carpet_frmColor" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        } function addpriview() {
            window.open("../../ReportViewer.aspx")
        }
    
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="margin-left: 15%; margin-right: 15%">
                <table>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="txtid" runat="server" Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lblcolorname" runat="server" Text="ColorName" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="txtcolor" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="font-family: Times New Roman; font-size: 18px">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: right">
                            <asp:Button ID="Button1" runat="server" Text="New" CssClass="buttonnorm" OnClick="Button1_Click" />
                            <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="buttonnorm" Width="53px"
                                OnClientClick="return confirm('Do you want to save data?')" OnClick="btnSave_Click" />
                            <asp:Button ID="btnClear" Text="Close" runat="server" CssClass="buttonnorm" Width="53px"
                                OnClientClick="return CloseForm();" />
                            <asp:Button ID="btnrpt" Text="Preview" runat="server" CssClass="buttonnorm " OnClick="btnrpt_Click" />
                            <asp:Button ID="btndelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"
                                CssClass="buttonnorm" OnClick="btndelete_Click" Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                            <div style="width: 100%; height: 200px; overflow: auto">
                                <asp:GridView ID="gdcolor" Width="100%" runat="server" DataKeyNames="Sr_No" OnRowDataBound="gdcolor_RowDataBound"
                                    OnSelectedIndexChanged="gdcolor_SelectedIndexChanged">
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <HeaderStyle Font-Bold="true" CssClass="gvheaders" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
