<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="Purchase_Matarial_report1.aspx.cs" Inherits="Masters_Purchase_Purchase_Matarial_report1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function priview() {
            window.open('../../ReportViewer.aspx', '');
        }
        function Validate() {
            if (document.getElementById("CPH_Form_rdrawmaterial")) {
                if (document.getElementById("CPH_Form_rdrawmaterial").checked == true) {
                    if (document.getElementById("<%=ddcustomer.ClientID %>").value == "0") {
                        alert("Pls Select Customer Code");
                        document.getElementById("<%=ddcustomer.ClientID %>").focus();
                        return false;
                    }
                    if (document.getElementById("<%=ddOrderno.ClientID %>").value == "0") {
                        alert("Pls Select Customer Order NO.");
                        document.getElementById("<%=ddOrderno.ClientID %>").focus();
                        return false;
                    }
                }
            }
        }
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <table style="height: 311px;">
                <tr>
                    <td>
                        <div style="width: 250px; height: 336px; float: left; border-style: solid; border-width: thin">
                            <asp:RadioButton ID="rdrawmaterial" Text="Raw Material Purchase Report Buyer Code & Order"
                                runat="server" GroupName="OrderType" CssClass="radiobuttonnormal" AutoPostBack="true"
                                OnCheckedChanged="RDRawMaterial_CheckedChanged" />
                            <br />
                            <asp:RadioButton ID="RDSupplyorder" Text="Purchase Order Vendor Wise" runat="server"
                                GroupName="OrderType" CssClass="radiobuttonnormal" AutoPostBack="true" OnCheckedChanged="RDSupplyorder_CheckedChanged" />
                            <br />
                            <asp:RadioButton ID="RDPurchaseReceive" Text="Purchase Receive Detail" runat="server"
                                GroupName="OrderType" CssClass="radiobuttonnormal" AutoPostBack="true" OnCheckedChanged="RDPurchaseReceive_CheckedChanged" />
                            <br />
                            <asp:RadioButton ID="RDpurdelivRpt" Text="Purchase Delivery Status Report" runat="server"
                                GroupName="OrderType" CssClass="radiobuttonnormal" AutoPostBack="true" OnCheckedChanged="RDpurdelivRpt_CheckedChanged" />
                            <br />
                            <asp:RadioButton ID="RDindent" Text="Indent Register" runat="server" GroupName="OrderType"
                                CssClass="radiobuttonnormal" AutoPostBack="true" OnCheckedChanged="RDindent_CheckedChanged" />
                            <br />
                            <asp:RadioButton ID="RDpurchasedetail" Text="Purchase Detail List" runat="server"
                                GroupName="OrderType" CssClass="radiobuttonnormal" AutoPostBack="true" OnCheckedChanged="RDpurchasedetail_CheckedChanged" />
                            <br />
                            <asp:RadioButton ID="RdSupply" Text="Purchase Supply Ledger" runat="server" GroupName="OrderType"
                                CssClass="radiobuttonnormal" AutoPostBack="true" OnCheckedChanged="RDpurchasesupply_CheckedChanged" />
                            <br />
                            
                            <asp:RadioButton ID="RDDebitNote" Text="Debit Note Details" runat="server" GroupName="OrderType"
                                CssClass="radiobuttonnormal" AutoPostBack="true" OnCheckedChanged="RDDebitNote_CheckedChanged" />
                            <br />
                            
                            <asp:RadioButton ID="RDPurchaseMaterialReceive" Text="Purchase Material Receive"
                                runat="server" GroupName="OrderType" CssClass="radiobuttonnormal" AutoPostBack="true"
                                OnCheckedChanged="RDPurchaseMaterialReceive_CheckedChanged" Visible="false" />
                            <br />
                            <asp:RadioButton ID="RDPurchaseMaterialRecPending" Text="Purchase Material Rec Pending as On Date"
                                runat="server" GroupName="OrderType" CssClass="radiobuttonnormal" AutoPostBack="true"
                                OnCheckedChanged="RDPurchaseMaterialRecPending_CheckedChanged" Visible="false" />
                            <br />
                            <asp:RadioButton ID="RDPurchaseOrderReceiveBuyerCode" Text="Purchase Order Receive Report Buyer Code & Order"
                                runat="server" GroupName="OrderType" CssClass="radiobuttonnormal" AutoPostBack="true"
                                OnCheckedChanged="RDPurchaseOrderReceiveBuyerCode_CheckedChanged" Visible="false" />
                            <br />
                            <asp:RadioButton ID="RDPurchaseMaterialIssueReceive" Text="Purchase Material Issue Receive"
                                runat="server" GroupName="OrderType" CssClass="radiobuttonnormal" AutoPostBack="true"
                                OnCheckedChanged="RDPurchaseMaterialIssueReceive_CheckedChanged" Visible="false" />
                            <br />
                            <asp:RadioButton ID="RDCustomerOrderWisePODetail" Text="Customer Order Wise PO Detail"
                                runat="server" GroupName="OrderType" CssClass="radiobuttonnormal" AutoPostBack="true"
                                OnCheckedChanged="RDCustomerOrderWisePODetail_CheckedChanged" Visible="false" />
                            <br />
                            <asp:RadioButton ID="RDPurchaseOrderRecPendingDetail" Text="Purchase Order Rec Pending Detail"
                                runat="server" GroupName="OrderType" CssClass="radiobuttonnormal" AutoPostBack="true"
                                OnCheckedChanged="RDPurchaseOrderRecPendingDetail_CheckedChanged" Visible="true" />
                            <br />

                        </div>
                        <div style="float: left; width: 350px; height: 280px;">
                            <table>
                                <tr id="Trcomp" runat="server">
                                    <td id="Tdcomp" runat="server" class="tdstyle">
                                        <span class="labelbold">Company Name</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddCompName" runat="server" Width="250px" TabIndex="1" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr runat="server" id="trcustomer" visible="false">
                                    <td id="tdcustomer" runat="server">
                                        <asp:Label ID="lblcusomername" class="tdstyle" runat="server" Text="Customer Code"
                                            CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddcustomer" runat="server" Width="250px" OnSelectedIndexChanged="ddcustomer_SelectedIndexChanged"
                                            AutoPostBack="True" TabIndex="7" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr runat="server" id="trorder" visible="false">
                                    <td id="tdorder" runat="server" class="tdstyle">
                                        <span class="labelbold">Order No.</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddOrderno" runat="server" Width="250px" TabIndex="8" AutoPostBack="True"
                                            CssClass="dropdown" OnSelectedIndexChanged="ddOrderno_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr runat="server" id="trsupply" visible="false">
                                    <td id="tdsupply" runat="server">
                                        <asp:Label ID="lblemp" runat="server" class="tdstyle" Text="SUPPLIER NAME" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="dsuppl" runat="server" Width="250px" TabIndex="9" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="dsuppl_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr runat="server" id="trPurchaseIndentChallanNo" visible="false">
                                    <td id="td1" runat="server">
                                        <asp:Label ID="lblPoNo" runat="server" class="tdstyle" Text="P.O.No" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDPONo" runat="server" Width="250px" TabIndex="9" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDPONo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Tr3" runat="server">
                                    <td>
                                        <asp:Label ID="lblcategoryname" runat="server" class="tdstyle" Text="Category Name"
                                            CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddCatagory" runat="server" Width="150px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged"
                                            TabIndex="13">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TrItemName" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblitemname" class="tdstyle" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="dditemname" runat="server" Width="150px" CssClass="dropdown"
                                            AutoPostBack="True" OnSelectedIndexChanged="dditemname_SelectedIndexChanged"
                                            TabIndex="14">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="ql" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblqualityname" class="tdstyle" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="dquality" runat="server" TabIndex="15" Width="150px" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="dsn" runat="server" visible="false" class="style5">
                                    <td>
                                        <asp:Label ID="lbldesignname" runat="server" class="tdstyle" Text="Design" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="dddesign" runat="server" Width="150px" TabIndex="16" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="clr" runat="server" visible="false" class="style5">
                                    <td>
                                        <asp:Label ID="lblcolorname" runat="server" class="tdstyle" Text="Color" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddcolor" runat="server" Width="150px" TabIndex="17" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="shp" runat="server" visible="false" class="style5">
                                    <td>
                                        <asp:Label ID="lblshapename" class="tdstyle" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddshape" runat="server" Width="150px" TabIndex="18" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="sz" runat="server" visible="false" class="style5">
                                    <td>
                                        <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddsize" runat="server" Width="150px" TabIndex="19" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="shd" runat="server" visible="false" class="style5">
                                    <td>
                                        <asp:Label ID="lblshadecolor" runat="server" class="tdstyle" Text="ShadeColor" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlshade" runat="server" Width="150px" CssClass="dropdown"
                                            TabIndex="20">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="Trgodown" runat="server" visible="false" class="style5">
                                    <td>
                                        <asp:Label ID="Label4" runat="server" class="tdstyle" Text="Godown Name" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDgodown" runat="server" Width="150px" CssClass="dropdown"
                                            TabIndex="20">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TrStatus" runat="server" visible="false" class="style5">
                                    <td>
                                        <asp:Label ID="Label5" runat="server" class="tdstyle" Text="Status" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDStatus" runat="server" CssClass="dropdown">
                                            <asp:ListItem Value="0">All</asp:ListItem>
                                            <asp:ListItem Value="1">Complete</asp:ListItem>
                                            <asp:ListItem Value="2">Pending</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRRecChallanNo" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label3" runat="server" class="tdstyle" Text="Receive Bill No" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="textb" ID="txtRecChallanNo" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr runat="server" id="trChkForDate" visible="false">
                                    <td colspan="2" align="left">
                                        <asp:CheckBox ID="ChkForDate" runat="server" Text="Check For Date" CssClass="checkboxbold"
                                            AutoPostBack="True" OnCheckedChanged="ChkForDate_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr runat="server" id="trfr">
                                    <td id="frdate" runat="server">
                                        <asp:Label ID="LBLFRDATE" runat="server" class="tdstyle" Text="From Date" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="textb" ID="TxtFRDate" runat="server" TabIndex="7"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="TxtFRDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr id="trto" runat="server">
                                    <td runat="server" id="todate">
                                        <asp:Label ID="Label2" runat="server" class="tdstyle" Text="To Date" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="textb" ID="TxtTODate" runat="server" TabIndex="8"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="txtTODate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr runat="server" id="TRASOnDate" visible="false">
                                    <td id="Td2" runat="server">
                                        <asp:Label ID="Label6" runat="server" class="tdstyle" Text="From Date" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="textb" ID="txtAsOnDate" runat="server" TabIndex="7"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="txtAsOnDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr runat="server" id="TRPurchaseSumm" visible="false">
                                    <td colspan="2">
                                        <asp:CheckBox ID="chkpurchasesumm" Text="For Summary" CssClass="checkboxbold" runat="server" />
                                    </td>
                                </tr>
                                <tr runat="server" id="TRPurchaseDetailByChallan" visible="false">
                                    <td colspan="2">
                                        <asp:CheckBox ID="chkpurchasedetailbychallan" Text="By Receive Bill No" CssClass="checkboxbold"
                                            runat="server" />
                                    </td>
                                </tr>
                                <tr runat="server" id="TRLotBillDetail" visible="false">
                                    <td colspan="2">
                                        <asp:CheckBox ID="chkpurchaseordervendorwiselotbillno" Text="By Lot & BillNo" CssClass="checkboxbold"
                                            runat="server" />
                                    </td>
                                </tr>
                                <tr runat="server" id="TRFinalAbbaReport" visible="false">
                                    <td colspan="2">
                                        <asp:CheckBox ID="ChkFinalAbbaReport" Text="Final Report" CssClass="checkboxbold"
                                            runat="server" />
                                    </td>
                                </tr>
                                <tr id="Tr7">
                                    <td id="TDExcelExport" runat="server" visible="false">
                                        <asp:CheckBox ID="chkexcelexport" CssClass="checkboxbold" Text="Excel Export" runat="server" />
                                    </td>
                                    <td colspan="3" align="right">
                                        <asp:Label ID="Label1" runat="server" Text="Label" ForeColor="Red" Visible="false"
                                            CssClass="labelbold"></asp:Label>
                                        <asp:Button ID="btnpreview" runat="server" Text="Preview" TabIndex="22" CssClass="buttonnorm"
                                            OnClick="btnpreview_Click" OnClientClick=" return Validate();" />
                                        &nbsp;<asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm(); "
                                            TabIndex="23" CssClass="buttonnorm" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnpreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
