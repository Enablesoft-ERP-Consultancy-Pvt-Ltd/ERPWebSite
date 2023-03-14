<%@ Page Title="TDSMASTER" Language="C#" AutoEventWireup="true" CodeFile="FrmTDS.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Carpet_FrmTDS" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function addpriview() {
            window.open("../../ReportViewer.aspx");
        }    
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="margin-left: 15%; margin-right: 15%">
                <table>
                    <tr>
                        <td class="tdstyle">
                            <span class="labelbold">Type</span>
                            <br />
                            <asp:DropDownList ID="DDType" runat="server" Width="150px" CssClass="dropdown" OnSelectedIndexChanged="DDType_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">TDS(%)</span>
                            <br />
                            &nbsp;<asp:TextBox CssClass="textb" ID="TxtTDS" Width="80px" runat="server"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">Min Amount</span>
                            <br />
                            &nbsp;<asp:TextBox CssClass="textb" ID="TxtMinimumAmount" Width="80px" runat="server"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">From Date</span>
                            <br />
                            <asp:TextBox CssClass="textb" ID="TxtFromDate" Width="90px" runat="server"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="TxtFromDate" Format="dd-MMM-yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                        </td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="text-align: right">
                            <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="buttonnorm" OnClientClick="return confirm('Do You Want To Save Data?')"
                                OnClick="btnSave_Click" />
                            &nbsp;<asp:Button ID="btnPreview" Text="Preview" runat="server" CssClass="buttonnorm"
                                OnClick="btnPreview_Click" />
                            &nbsp;<asp:Button ID="btnClose" Text="Close" runat="server" CssClass="buttonnorm"
                                Width="53px" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                            <asp:GridView ID="DG" runat="server" DataKeyNames="Sr_No" OnRowDataBound="DG_RowDataBound"
                                OnSelectedIndexChanged="DG_SelectedIndexChanged">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
