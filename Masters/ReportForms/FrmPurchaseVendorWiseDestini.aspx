<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="FrmPurchaseVendorWiseDestini.aspx.cs"
    Inherits="Masters_ReportForms_FrmPurchaseVendorWiseDestini" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function priview() {
            {
                window.open('../../ReportViewer.aspx', '');
            }
        }
    </script>
    <table>
        <tr>
            <td valign="top" id="tdreporttype" runat="server">
                <div style="width: 300px; height: 400px; float: left; border-style: solid; border-width: thin">
                    <asp:RadioButton ID="RDDespatchDetail" Text="Despatch Detail" runat="server" GroupName="OrderType"
                        CssClass="labelnormalMM" AutoPostBack="true" OnCheckedChanged="RDDespatchDetail_CheckedChanged" /><br />
                    <asp:RadioButton ID="RDDespdetwithpo" Text="Despatch Detail With PO" runat="server"
                        GroupName="OrderType" CssClass="labelnormalMM" AutoPostBack="true" OnCheckedChanged="RDDespdetwithpo_CheckedChanged" /><br />
                    <asp:RadioButton ID="rdpurchasevendor" Text="Purchase Vendor Wise " runat="server"
                        GroupName="OrderType" CssClass="labelnormalMM" AutoPostBack="true" OnCheckedChanged="rdpurchasevendor_CheckedChanged" /><br />
                    <asp:RadioButton ID="RDrawmaterial" Text="Material Issue Report" runat="server" GroupName="OrderType"
                        CssClass="labelnormalMM" AutoPostBack="true" OnCheckedChanged="RDrawmaterial_CheckedChanged" /><br />
                    <asp:RadioButton ID="RDrawmaterialrec" Text="Material Receipt Report" runat="server"
                        GroupName="OrderType" CssClass="labelnormalMM" AutoPostBack="true" OnCheckedChanged="RDrawmaterialrec_CheckedChanged" />
                    <br />
                    <asp:RadioButton ID="RDrawmaterialstock" Text="Materail Stock " runat="server" GroupName="OrderType"
                        CssClass="labelnormalMM" AutoPostBack="true" OnCheckedChanged="RDrawmaterialstock_CheckedChanged" /><br />
                    <asp:RadioButton ID="RDSTICKER" Text="Sticker Report" runat="server" GroupName="OrderType"
                        CssClass="labelnormalMM" AutoPostBack="true" OnCheckedChanged="RDJwissue_CheckedChanged" />
                    <br />
                    <asp:RadioButton ID="RDremovalorder" Text="Removal Of Report" runat="server" GroupName="OrderType"
                        CssClass="labelnormalMM" AutoPostBack="true" OnCheckedChanged="RDremovalorder_CheckedChanged" />
                    <br />
                    <asp:RadioButton ID="Rdpurchasedetail" Text="Purchase Order Status" runat="server"
                        GroupName="OrderType" CssClass="labelnormalMM" AutoPostBack="true" OnCheckedChanged="Rdpurchasedetail_CheckedChanged" /><br />
                    <asp:RadioButton ID="RDremovalorderitem" Text="Removal Of Order Item Wise" runat="server"
                        GroupName="OrderType" CssClass="labelnormalMM" AutoPostBack="true" OnCheckedChanged="RDremovalorderitem_CheckedChanged" /><br />
                    <asp:RadioButton ID="RDGlassstockstmt" Text="Glass Stock Statement" runat="server"
                        GroupName="OrderType" CssClass="labelnormalMM" AutoPostBack="true" OnCheckedChanged="RDGlassstockstmt_CheckedChanged" /><br />
                    <asp:RadioButton ID="RDPurchaseRate" Text="Purchase Rate Summary" runat="server"
                        GroupName="OrderType" CssClass="labelnormalMM" AutoPostBack="true" OnCheckedChanged="RDPurchaseRate_CheckedChanged" /><br />
                    <asp:RadioButton ID="Rdorderstatus" Text="Total Order Status" runat="server" GroupName="OrderType"
                        CssClass="labelnormalMM" AutoPostBack="true" OnCheckedChanged="Rdorderstatus_CheckedChanged" /><br />
                    <asp:RadioButton ID="RdOrderStock" Text="Order Wise Stock" runat="server" GroupName="OrderType"
                        CssClass="labelnormalMM" AutoPostBack="true" OnCheckedChanged="RdOrderStock_CheckedChanged" /><br />
                    <asp:RadioButton ID="RDFinishingRateDetail" Text="Finishing Rate Detail" runat="server"
                        GroupName="OrderType" CssClass="labelnormalMM" AutoPostBack="true" OnCheckedChanged="RDFinishingRateDetail_CheckedChanged" />
                    <br />
                    <asp:RadioButton ID="RDRejMaterial" Text="Reject Material" runat="server" GroupName="OrderType"
                        CssClass="labelnormalMM" AutoPostBack="true" OnCheckedChanged="RDRejMaterial_CheckedChanged" />
                    <br />
                    <asp:RadioButton ID="RDDebitNote" Text="Debit Note" runat="server" GroupName="OrderType"
                        CssClass="labelnormalMM" AutoPostBack="true" OnCheckedChanged="RDDebitNote_CheckedChanged" />
                    <br />
                    <asp:RadioButton ID="RDBuyerOrderDetail" Text="Buyer Order Detail" runat="server"
                        GroupName="OrderType" CssClass="labelnormalMM" AutoPostBack="true" OnCheckedChanged="RDBuyerOrderDetail_CheckedChanged" />
                    <br />
                    <asp:RadioButton ID="RDCostComparision" Text="Cost Comparision" runat="server" GroupName="OrderType"
                        CssClass="labelnormalMM" AutoPostBack="true" OnCheckedChanged="RDCostComparision_CheckedChanged" />
                </div>
            </td>
            <td valign="top">
                <table>
                    <tr id="Trcomp" runat="server">
                        <td id="Tdcomp" runat="server" class="tdstyle">
                            Company Name
                        </td>
                        <td>
                            <asp:DropDownList ID="ddCompName" runat="server" Width="250px" TabIndex="1" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="ddCompName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr id="trcustomercode" runat="server">
                        <td id="Td1" runat="server" class="tdstyle">
                            Customer Name
                        </td>
                        <td>
                            <asp:DropDownList ID="ddcustomername" runat="server" Width="250px" TabIndex="1" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="ddcustomername_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="ddcustomername"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr runat="server" id="trvendor">
                        <td id="tdvendor" runat="server">
                            <asp:Label ID="lblcusomername" class="tdstyle" runat="server" Text="Vendor Name"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddvendor" runat="server" Width="250px" TabIndex="7" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddvendor"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr runat="server" id="trgodown">
                        <td>
                            <asp:Label ID="lblgodown" class="tdstyle" runat="server" Text="Godown Name"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddgodown" runat="server" Width="250px" TabIndex="7" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddgodown"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr runat="server" id="trprocess">
                        <td>
                            <asp:Label ID="lblprocess" class="tdstyle" runat="server" Text="Process Name"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddprocess" runat="server" Width="250px" TabIndex="7" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddprocess"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr id="trcategory" runat="server">
                        <td>
                            <asp:Label ID="lblcategoryname" runat="server" Text="Catagory Name"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddCatagory" runat="server" Width="250px" TabIndex="7" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchddCatagory" runat="server" TargetControlID="ddCatagory"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr id="tritem" runat="server">
                        <td>
                            <asp:Label ID="lblitemname" runat="server" Text="Item Name"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="dditemname" runat="server" Width="250px" OnSelectedIndexChanged="dditemname_SelectedIndexChanged"
                                TabIndex="8" AutoPostBack="True" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchdditemname" runat="server" TargetControlID="dditemname"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr id="trquality" runat="server">
                        <td id="ql" runat="server">
                            <asp:Label ID="lblqualityname" runat="server" Text="Quality"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="dquality" runat="server" Width="250px" TabIndex="12" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchdquality" runat="server" TargetControlID="dquality"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr id="trOrder" runat="server">
                        <td id="TdOrder" runat="server">
                            <asp:Label ID="LblOrderNo" runat="server" Text="Customer OrderNo"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDOrderNo" runat="server" Width="250px" TabIndex="14" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="Listsearchextender8" runat="server" TargetControlID="DDOrderNo"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr id="trProdCode" runat="server">
                        <td id="TdProdCode" runat="server">
                            <asp:Label ID="LblProdCode" runat="server" Text="Product Code"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDProdCode" runat="server" Width="250px" TabIndex="13" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="DDProdCode_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="Listsearchextender7" runat="server" TargetControlID="DDProdCode"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr runat="server" id="trfinishtype">
                        <td>
                            <asp:Label ID="lblfinishtype" class="tdstyle" runat="server" Text="Finished Type"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddfinishtype" runat="server" Width="250px" TabIndex="7" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="ddfinishtype_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="ddfinishtype"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr id="trInvno" runat="server">
                        <td id="TdInvno" runat="server">
                            <asp:Label ID="LblInv" runat="server" Text="Invoice No"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDInvNo" runat="server" Width="250px" TabIndex="15" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="Listsearchextender9" runat="server" TargetControlID="DDInvNo"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr runat="server" id="trfr">
                        <td id="frdate" runat="server">
                            <asp:Label ID="LBLFRDATE" runat="server" class="tdstyle" Text="From Date"></asp:Label>
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
                            <asp:Label ID="Label2" runat="server" class="tdstyle" Text="To Date"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="TxtTODate" runat="server" TabIndex="8"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtTODate">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td id="tdsubmit" runat="server">
                            <asp:Button ID="btnsybmit" runat="server" Text="Submit" TabIndex="22" CssClass="buttonnorm"
                                OnClick="btnsybmit_Click" />
                            &nbsp;
                        </td>
                        <td id="tdExport" runat="server">
                            <asp:Button ID="btnExport" runat="server" Text="Export" TabIndex="22" CssClass="buttonnorm"
                                OnClick="btnExport_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr runat="server" id="trgridconsumption" visible="false">
            <td colspan="5" style="font-weight: bold">
                <div style="height: 300px; overflow: scroll">
                    <asp:GridView ID="DGConsumption" runat="server" Width="100%" OnRowDataBound="gdDesign_RowDataBound"
                        CssClass="grid-view" OnRowCreated="gdDesign_RowCreated">
                        <HeaderStyle CssClass="gvheader" />
                        <AlternatingRowStyle CssClass="gvalt" />
                        <RowStyle CssClass="gvrow" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </div>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
