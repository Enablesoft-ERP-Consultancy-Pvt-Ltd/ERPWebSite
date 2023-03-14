<%@ Page Title="OnLoom Inspection StockNo Wise" Language="C#" AutoEventWireup="true"
    CodeFile="FrmOnLoomInspectionStockNoWise.aspx.cs" MasterPageFile="~/ERPmaster.master"
    Inherits="Masters_Campany_FrmOnLoomInspectionStockNoWise" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function KeyDownHandler(btn) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                window.document.getElementById(btn).click();
            }
        }
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
    </script>
    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <table width="85%">
                <tr>
                    <%-- <td>
                            <asp:Button ID="btnStockNo" runat="server" BackColor="White" 
                                    BorderColor="White" BorderWidth="0px" ForeColor="White" Height="0px" Width="0px" />                               
                            </td>--%>
                    <td>
                        <asp:Label ID="lblStockNo" runat="server" Text="Stock No" CssClass="labelbold"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtStockNo" runat="server" CssClass="textb" Width="500px" Height="20px"
                            OnTextChanged="txtStockNo_TextChanged" AutoPostBack="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="LblErrorMessage" runat="server" Text="" Font-Bold="false" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <%-- <asp:PostBackTrigger ControlID="btnprintstockrawdetail" />--%>
            <asp:PostBackTrigger ControlID="txtStockNo" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
