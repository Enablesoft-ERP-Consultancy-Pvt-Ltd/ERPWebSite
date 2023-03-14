<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" Title="Tranfer Data From DataBase" CodeFile="FrmDataFromOneDataBaseToOther.aspx.cs"
    Inherits="FrmDataFromOneDataBaseToOther" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="Scripts/JScript.js" type="text/jscript"></script>
    <script src="Scripts/jquery-1.4.1.js" type="text/jscript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Preview() {
            window.open("../../ReportViewer.aspx", "PurchaseReceive");
        }
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <table>
                <div>
                    <tr align="center" style="width: 75%; margin-left: 10%">
                        <td>
                            <asp:Button ID="BtnDeleteData" runat="server" Text="Delete Data" OnClick="BtnDeleteData_Click"
                                OnClientClick="return confirm('Do you want to delete data?')" CssClass="buttonnorm" />
                            &nbsp;<asp:Button ID="BtnPrepareData" runat="server" Text="Prepare Data" OnClick="BtnPrepareData_Click"
                                OnClientClick="return confirm('Do you want to prepare data?')" CssClass="buttonnorm" />
                            &nbsp;<asp:Button ID="BtnTranferData" runat="server" Text="Tranfer Data" OnClick="BtnTranferData_Click"
                                OnClientClick="return confirm('Do you want to tranfer data?')" CssClass="buttonnorm" />
                        </td>
                    </tr>
                    <tr align="center" style="width: 75%; margin-left: 10%">
                        <td>
                            <asp:Label ID="LblErrorMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </div>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
