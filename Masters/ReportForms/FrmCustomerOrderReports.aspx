<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmCustomerOrderReports.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_ReportForms_FrmCustomerOrderReports"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Validate() {
            if (document.getElementById("CPH_Form_RDDetailDeliveryStatusMaterialBal")) {
                if (document.getElementById("CPH_Form_RDDetailDeliveryStatusMaterialBal").checked == true) {
                    if (document.getElementById("<%=DDCustCode.ClientID %>").value == "0") {
                        alert("Pls Select Customer Code");
                        document.getElementById("<%=DDCustCode.ClientID %>").focus();
                        return false;
                    }
                }
            }
            if (document.getElementById("CPH_Form_RDRawMaterialBalanceAgainstOrder")) {
                if (document.getElementById("CPH_Form_RDRawMaterialBalanceAgainstOrder").checked == true) {
                    if (document.getElementById("<%=DDCustCode.ClientID %>").value == "0") {
                        alert("Pls Select Customer Code");
                        document.getElementById("<%=DDCustCode.ClientID %>").focus();
                        return false;
                    }

                }
            }
            if (document.getElementById("CPH_Form_RDComm")) {
                if (document.getElementById("CPH_Form_RDComm").checked == true) {
                    if (document.getElementById("<%=DDCustCode.ClientID %>").value == "0") {
                        alert("Pls Select Customer Code");
                        document.getElementById("<%=DDCustCode.ClientID %>").focus();
                        return false;
                    }
                    if (document.getElementById("<%=DDOrderNo.ClientID %>").value == "0") {
                        alert("Pls Select Order No.");
                        document.getElementById("<%=DDOrderNo.ClientID %>").focus();
                        return false;
                    }
                }
            }
            if (document.getElementById("CPH_Form_RDStockAssign")) {
                if (document.getElementById("CPH_Form_RDStockAssign").checked == true) {
                    if (document.getElementById("<%=DDCustCode.ClientID %>").value == "0") {
                        alert("Pls Select Customer Code");
                        document.getElementById("<%=DDCustCode.ClientID %>").focus();
                        return false;
                    }
                    if (document.getElementById("<%=DDOrderNo.ClientID %>").value == "0") {
                        alert("Pls Select Order No.");
                        document.getElementById("<%=DDOrderNo.ClientID %>").focus();
                        return false;
                    }
                }
            }
            //            else if (document.getElementById("<%=DDOrderNo.ClientID %>").value == "0") {
            //                alert("Pls Select Order No");
            //                document.getElementById("<%=DDOrderNo.ClientID %>").focus();
            //                return false;
            //            }
            if (document.getElementById("<%=TxtFromDate.ClientID %>").value == "") {
                alert("Pls Select From Date");
                document.getElementById("<%=TxtFromDate.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=TxtToDate.ClientID %>").value == "") {
                alert("Pls Select To Date");
                document.getElementById("<%=TxtToDate.ClientID %>").focus();
                return false;
            }
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 90%; height: 311px;">
                <tr style="width: 100%">
                    <td style="width: 80px">
                    </td>
                    <td>
                        <div style="width: 250px; height: 336px; float: left; border-style: solid; border-width: thin">
                            <asp:RadioButton ID="RDDetailDeliveryStatusMaterialBal" Text="Detail Delivery Status & Material Bal Against Order"
                                runat="server" Font-Bold="true" GroupName="OrderType" />
                            <br />
                            <br />
                            <asp:RadioButton ID="RDBriefOrdersInHand" Text="Brief Orders In Hand" runat="server"
                                Font-Bold="true" GroupName="OrderType" />
                            <br />
                            <br />
                            <asp:RadioButton ID="RDAllOrdersDetailDateToDate" Text="All Orders Detail Date To Date"
                                runat="server" Font-Bold="true" GroupName="OrderType" />
                            <br />
                            <br />
                            <asp:RadioButton ID="RDComm" Text="OrderWise Weaver Comm." runat="server" Font-Bold="true"
                                GroupName="OrderType" />
                            <br />
                            <br />
                            <asp:RadioButton ID="RDStockAssign" Text="OrderWise StockAssign" runat="server" Font-Bold="true"
                                GroupName="OrderType" />
                            <br />
                            <br />
                            <asp:RadioButton ID="RDOrderConsumption" Text="Order Consumption Detail" runat="server"
                                Font-Bold="true" GroupName="OrderType" />
                            <br />
                            <br />
                            <asp:RadioButton ID="RD1" Text="StockReport" runat="server" Font-Bold="true" GroupName="OrderType"
                                Visible="False" />
                            <br />
                            <br />
                            <asp:RadioButton ID="RDRawMaterialBalanceAgainstOrder" Text="Raw Material Balance Against Order"
                                runat="server" Font-Bold="true" GroupName="OrderType" Visible="false" />
                            <br />
                            <asp:RadioButton ID="RDOrderDeail" Text="Order Detail" runat="server" Font-Bold="true"
                                GroupName="OrderType" Visible="false" />
                            <br />
                            <asp:RadioButton ID="RDOrderSummary" Text="Order Summary" runat="server" Font-Bold="true"
                                GroupName="OrderType" Visible="false" />
                        </div>
                        <div style="float: left; width: 350px; height: 280px;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="CompanyName" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDCompany" runat="server" Width="250px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDCompany_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender12" runat="server" TargetControlID="DDCompany"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="CustomerCode" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDCustCode" runat="server" Width="250px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDCustCode_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDCustCode"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="OrderNo" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDOrderNo" runat="server" Width="250px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDOrderNo"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr id="TrFromDate" runat="server">
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="From Date" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtFromDate" runat="server" Width="150px" CssClass="textb"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="TxtFromDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr id="TrTodate" runat="server">
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="To Date" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtToDate" runat="server" Width="150px" CssClass="textb"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="TxtToDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:CheckBox ID="ChkForFt" runat="server" Text="For Ft Format" Visible="false" CssClass="checkboxbold" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:Button ID="BtnPreview" runat="server" Text="Preview" OnClick="BtnPreview_Click"
                                            OnClientClick="return Validate();" CssClass="buttonnorm" />
                                        <asp:Button ID="BtnClose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" CssClass="labelbold"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
