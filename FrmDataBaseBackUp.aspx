<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" Title="Take Data Base Back Up" CodeFile="FrmDataBaseBackUp.aspx.cs"
    Inherits="FrmDataBaseBackUp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="Scripts/JScript.js" type="text/jscript"></script>
    <script src="Scripts/jquery-1.4.1.js" type="text/jscript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Preview() {
            window.open("../../ReportViewer.aspx", "FrmDataBaseBackUp");
        }
    </script>
    <asp:UpdatePanel ID="upnew1" runat="server">
        <ContentTemplate>
            <table width="100%">
                <div>
                    <tr style="height: 50px">
                        <td>
                        </td>
                    </tr>
                    <tr align="center" style="height: 50px">
                        <td>
                            <asp:Button ID="BtnDataBaseBackUp" runat="server" Text="Click for data base backup"
                                OnClick="BtnDataBaseBackUp_Click" OnClientClick="return confirm('Do you want to take data back-up its take some minutes ?')"
                                CssClass="buttonnorm" Width="175px" />
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
