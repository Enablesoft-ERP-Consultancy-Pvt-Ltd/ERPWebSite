<%@ Page Title="Stock Detail" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="FrmCmpRawMaterialStock.aspx.cs" Inherits="Masters_ReportForms_FrmCmpRawMaterialStock" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function Preview() {
            window.open('../../FrmCmpRawMaterialStock.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <script runat="server">    
        protected void RadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            TRProcessName.Visible = false;
            TDFinishedStockWithoutstockNo.Visible = false;
            trwithvalue.Visible = false;
            TDForIssueRegister.Visible = false;
            RDDyed.Checked = false;
            RDundyed.Checked = false;
            TRDDGodow.Visible = true;
            TRDDLotNo.Visible = true;
            chkformtr.Checked = false;
            TDMtr.Visible = false;
            TRLotNoTagNo.Visible = false;
            chkLotwiseTagwise.Visible = false;
            TRcategory.Visible = true;
            TRddItemName.Visible = true;
            TDstocksummary.Visible = false;
            trforfromandtodate.Visible = false;
            TDundyed_dyed.Visible = false;
            chkstocksummary.Checked = false;
            TRDDCustName.Visible = false;
            TRDDOrder.Visible = false;
            lblfromdate.Text = "From Date";
            lblTodate.Visible = true;
            txttodate.Visible = true;
            TDstockupto.Visible = false;
            txtTagno.Text = "";
            txtLotno.Text = "";
            Trwithpurchasedetail.Visible = false;
            chkwithpurchasedetail.Checked = false;
            TDallstockno.Visible = false;
            TRCheckStockDetail.Visible = false;
            TdPurchasesummary.Visible = false;
            //DDCompany.SelectedValue = "1";
            DDCategory.SelectedIndex = 0;
            TDExportExcel.Visible = false;
            TDForLedgerDetail.Visible = false;
            TDForTotalStock.Visible = false;
            TDWithBuyerOrderNo.Visible = false;

            if (RDFinishedStock.Checked == true)
            {
                TRProcessName.Visible = false;
                TDFinishedStockWithoutstockNo.Visible = true;
                trwithvalue.Visible = false;
                TDForTotalStock.Visible = false;
                TDForIssueRegister.Visible = false;
                TDExportExcel.Visible = false;
                TDallstockno.Visible = true;
                TRDDGodow.Visible = true;
                TRDDCompany.Visible = true;
                TRDDCustName.Visible = true;
                TRDate.Visible = false;
                TRDDOrder.Visible = true;
                TRDDLotNo.Visible = true;
                TRCheckStockDetail.Visible = false;
                TDMtr.Visible = true;
                if (Session["varcompanyId"].ToString() == "36" || Session["varcompanyId"].ToString() == "38")
                {
                    TDExportExcel.Visible = true;
                }
                else
                {
                    TDExportExcel.Visible = false;
                }

                if (Session["varcompanyId"].ToString() == "38")
                {
                    TDWithBuyerOrderNo.Visible = true;
                }
                else
                {
                    TDWithBuyerOrderNo.Visible = false;
                }
            }
            else if (RDRawMaterialStock.Checked == true)
            {
                TRProcessName.Visible = false;
                TDFinishedStockWithoutstockNo.Visible = false;
                trwithvalue.Visible = false;
                TDForIssueRegister.Visible = false;
                TDExportExcel.Visible = false;
                TRDDGodow.Visible = true;
                TRDDCompany.Visible = true;

                TRDate.Visible = false;

                TRDDLotNo.Visible = false;
                TRCheckStockDetail.Visible = false;
                TDstocksummary.Visible = true;
                TRLotNoTagNo.Visible = true;
                Trwithpurchasedetail.Visible = true;
                //TDundyed_dyed.Visible = true;
                TDWithBuyerOrderNo.Visible = false;
                
                if (Session["varcompanyId"].ToString() != "14")
                {
                    TDundyed_dyed.Visible = true;
                    trwithvalue.Visible = false;
                }
                else
                {
                    RDundyed.Checked = true;
                    trwithvalue.Visible = true;
                }

                if (Session["VarCompanyId"].ToString() == "22" && Session["usertype"].ToString() == "1")
                {
                    TDForTotalStock.Visible = true;
                }
                else
                {
                    TDForTotalStock.Visible = false;
                }
            }
            else if (RDTotalStock.Checked == true)
            {

                TRProcessName.Visible = false;
                TDFinishedStockWithoutstockNo.Visible = false;
                trwithvalue.Visible = false;
                TDForTotalStock.Visible = false;
                TDForIssueRegister.Visible = false;
                TDExportExcel.Visible = false;
                TRDDGodow.Visible = true;
                TRDDCompany.Visible = true;
                TRDDCustName.Visible = true;
                TRDate.Visible = false;
                TRDDOrder.Visible = false;
                TRDDLotNo.Visible = true;
                TRCheckStockDetail.Visible = false;
                TDWithBuyerOrderNo.Visible = false;
            }
            else if (RDOneDayReport.Checked == true)
            {

                TRProcessName.Visible = false;
                TDFinishedStockWithoutstockNo.Visible = false;
                trwithvalue.Visible = false;
                TDForTotalStock.Visible = false;
                TDForIssueRegister.Visible = false;
                TDExportExcel.Visible = false;
                TRDate.Visible = true;
                TRDDCompany.Visible = false;
                TRDDGodow.Visible = false;
                TRDDCustName.Visible = false;
                TRDDOrder.Visible = false;
                TRDDLotNo.Visible = false;
                TRCheckStockDetail.Visible = false;
                TDWithBuyerOrderNo.Visible = false;
            }
            else if (RDrawmaterialopeningreport.Checked == true)
            {
                TRProcessName.Visible = false;
                TDFinishedStockWithoutstockNo.Visible = false;
                trwithvalue.Visible = false;
                TDForTotalStock.Visible = false;
                TDForIssueRegister.Visible = false;
                TDExportExcel.Visible = false;
                TRDDGodow.Visible = true;
                TRDDCompany.Visible = true;
                TRDDCustName.Visible = false;
                TRDate.Visible = false;
                TRDDOrder.Visible = false;
                TRDDLotNo.Visible = false;
                trforfromandtodate.Visible = true;
                TRCheckStockDetail.Visible = true;
                TRLotNoTagNo.Visible = true;
                chkLotwiseTagwise.Visible = true;
                if (Session["varcompanyId"].ToString() != "14")
                {
                    TDundyed_dyed.Visible = true;
                }
                TDExportExcel.Visible = true;
                TDWithBuyerOrderNo.Visible = false;
            }
            else if (RDMonthlystock.Checked == true)
            {
                TRProcessName.Visible = false;
                TDFinishedStockWithoutstockNo.Visible = false;
                trwithvalue.Visible = false;
                TDForTotalStock.Visible = false;
                TDForIssueRegister.Visible = false;
                TDExportExcel.Visible = false;
                TRLotNoTagNo.Visible = false;
                TRCheckStockDetail.Visible = false;
                chkLotwiseTagwise.Visible = false;
                TRDDGodow.Visible = false;
                TRcategory.Visible = false;
                TRDDCustName.Visible = false;
                TRDDOrder.Visible = false;
                TRDDLotNo.Visible = false;
                TRddItemName.Visible = false;
                trforfromandtodate.Visible = true;
                TRDDCompany.Visible = true;
                TDWithBuyerOrderNo.Visible = false;
            }
            else if (RDpurchaseissue_receivedetail.Checked == true)
            {
                TRProcessName.Visible = false;
                TDFinishedStockWithoutstockNo.Visible = false;
                trwithvalue.Visible = false;
                TDForTotalStock.Visible = false;
                TDForIssueRegister.Visible = false;
                TDExportExcel.Visible = false;
                TRDDGodow.Visible = true;
                TRDDCompany.Visible = true;
                TRDDCustName.Visible = false;
                TRDate.Visible = false;
                TRDDOrder.Visible = false;
                TRDDLotNo.Visible = false;
                trforfromandtodate.Visible = true;
                //TRCheckStockDetail.Visible = true;
                TRLotNoTagNo.Visible = true;
                TdPurchasesummary.Visible = true;
                //chkLotwiseTagwise.Visible = true;
                TDWithBuyerOrderNo.Visible = false;
            }
            else if (RDPurchaseMaterialasondate.Checked == true)
            {
                TRProcessName.Visible = false;
                TDFinishedStockWithoutstockNo.Visible = false;
                trwithvalue.Visible = false;
                TDForTotalStock.Visible = false;
                TDForIssueRegister.Visible = false;
                TDExportExcel.Visible = false;
                trforfromandtodate.Visible = true;
                lblfromdate.Text = "As on Date";
                lblTodate.Visible = false;
                txttodate.Visible = false;
                TDunconfirmcarpet.Visible = false;
                TDWithBuyerOrderNo.Visible = false;

                if (Session["varcompanyId"].ToString() == "21")
                {
                    TDExportExcel.Visible = true;
                }
                else
                {
                    TDExportExcel.Visible = false;
                }

            }
            else if (RDfinishedstockasondate.Checked == true)
            {
                TRProcessName.Visible = false;
                TDFinishedStockWithoutstockNo.Visible = false;
                trwithvalue.Visible = false;
                TDForTotalStock.Visible = false;
                TDForIssueRegister.Visible = false;
                TDExportExcel.Visible = false;
                TRLotNoTagNo.Visible = false;
                TRDDGodow.Visible = false;
                TDstockupto.Visible = true;
                TRDDCustName.Visible = true;
                TRDDOrder.Visible = true;
                TRDDLotNo.Visible = false;
                TDWithBuyerOrderNo.Visible = false;
            }
            else if (RdShadewisestock.Checked == true)
            {
                if (Session["varcompanyId"].ToString() == "22")
                {
                    TDForIssueRegister.Visible = true;
                }
                else
                {
                    TDForIssueRegister.Visible = false;
                }
                TRProcessName.Visible = false;
                TDFinishedStockWithoutstockNo.Visible = false;
                trwithvalue.Visible = false;
                TDExportExcel.Visible = false;
                TRDDLotNo.Visible = false;
                trforfromandtodate.Visible = true;
                TDForTotalStock.Visible = false;
                TDWithBuyerOrderNo.Visible = false;
            }
            else if (RDStockTransferDetail.Checked == true)
            {
                TRProcessName.Visible = false;
                TDFinishedStockWithoutstockNo.Visible = false;
                trwithvalue.Visible = false;
                TDForTotalStock.Visible = false;
                TDForIssueRegister.Visible = false;
                TDExportExcel.Visible = false;
                TRLotNoTagNo.Visible = false;
                TRCheckStockDetail.Visible = false;
                chkLotwiseTagwise.Visible = false;
                TRDDGodow.Visible = false;
                TRcategory.Visible = false;
                TRDDCustName.Visible = false;
                TRDDOrder.Visible = false;
                TRDDLotNo.Visible = false;
                TRddItemName.Visible = false;
                trforfromandtodate.Visible = true;
                TRDDCompany.Visible = true;
                TDWithBuyerOrderNo.Visible = false;
            }
            else if (RDRawMaterialStockGodownWise.Checked == true)
            {
                TRProcessName.Visible = false;
                TDFinishedStockWithoutstockNo.Visible = false;
                trwithvalue.Visible = false;
                TDForTotalStock.Visible = false;
                TDForIssueRegister.Visible = false;
                TDExportExcel.Visible = false;
                TRDDGodow.Visible = true;
                TRDDCompany.Visible = true;

                TRDate.Visible = true;

                TRDDLotNo.Visible = false;
                TRCheckStockDetail.Visible = false;
                TDstocksummary.Visible = true;
                TRLotNoTagNo.Visible = true;
                Trwithpurchasedetail.Visible = false;
                //TDundyed_dyed.Visible = true;
                TDWithBuyerOrderNo.Visible = false;
                
                if (Session["varcompanyId"].ToString() != "14")
                {
                    TDundyed_dyed.Visible = true;
                }
                else
                {
                    RDundyed.Checked = true;
                }
            }
            else if (RDRawMaterialStockAsOnDate.Checked == true)
            {
                TRProcessName.Visible = false;
                TDFinishedStockWithoutstockNo.Visible = false;
                trwithvalue.Visible = false;
                TDForIssueRegister.Visible = false;
                TDExportExcel.Visible = false;
                TRDDGodow.Visible = true;
                TRDDCompany.Visible = true;
                TRDate.Visible = false;
                TRDDLotNo.Visible = false;
                TRCheckStockDetail.Visible = false;
                TDstocksummary.Visible = false;
                TRLotNoTagNo.Visible = true;
                Trwithpurchasedetail.Visible = false;
                TDstockupto.Visible = true;
                TDunconfirmcarpet.Visible = false;
                TDundyed_dyed.Visible = false;
                TDWithBuyerOrderNo.Visible = false;
                
                if (Session["VarCompanyNo"].ToString() == "39")
                {
                    Trwithpurchasedetail.Visible = true;
                }
                else
                {
                    Trwithpurchasedetail.Visible = false;
                }

                if (Session["VarCompanyNo"].ToString() == "14")
                {
                    TDExportExcel.Visible = true;
                }
                else
                {
                    TDExportExcel.Visible = false;
                }
            }
            else if (RDRawMaterialOrderAssignIssueStock.Checked == true)
            {
                TRProcessName.Visible = false;
                TDFinishedStockWithoutstockNo.Visible = false;
                trwithvalue.Visible = false;
                TRDDCompany.Visible = true;
                TRDDCustName.Visible = true;
                TRDDOrder.Visible = true;
                TRDDGodow.Visible = false;
                TDunconfirmcarpet.Visible = false;
                TDWithBuyerOrderNo.Visible = false;
            }
            else if (RDFinishedStockProcessWise.Checked == true)
            {
                TRProcessName.Visible = true;
                TDFinishedStockWithoutstockNo.Visible = false;
                trwithvalue.Visible = false;
                TDForTotalStock.Visible = false;
                TDForIssueRegister.Visible = false;
                TDExportExcel.Visible = false;
                TDallstockno.Visible = false;
                TRDDGodow.Visible = true;
                TRDDCompany.Visible = true;
                TRDDCustName.Visible = true;
                TRDate.Visible = false;
                TRDDOrder.Visible = true;
                TRDDLotNo.Visible = false;
                TRCheckStockDetail.Visible = false;
                TDMtr.Visible = false;
                TDExportExcel.Visible = false;
                TDWithBuyerOrderNo.Visible = false;             
              
            }
        }
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div style="width: 100%;">
                <table style="width: 100%">
                    <td style="width: 30%" valign="top">
                        <div style="width: 90%; max-height: 400px; float: left; border-style: solid; border-width: thin">
                            <asp:RadioButton ID="RDFinishedStock" Text="Finished Stock" runat="server" GroupName="OrderType"
                                CssClass="radiobuttonnormal" AutoPostBack="True" OnCheckedChanged="RadioButton_CheckedChanged" />
                            <br />
                            <asp:RadioButton ID="RDRawMaterialStock" Text="Raw Material Stock" runat="server"
                                GroupName="OrderType" OnCheckedChanged="RadioButton_CheckedChanged" AutoPostBack="True"
                                CssClass="radiobuttonnormal" />
                            <br />
                            <asp:RadioButton ID="RDRawMaterialStockAsOnDate" Text="Raw Material Stock As OnDate"
                                runat="server" GroupName="OrderType" OnCheckedChanged="RadioButton_CheckedChanged"
                                AutoPostBack="True" CssClass="radiobuttonnormal" />
                            <br />
                            <asp:RadioButton ID="RdShadewisestock" Text="Shade Wise Stock" runat="server" GroupName="OrderType"
                                AutoPostBack="true" CssClass="radiobuttonnormal" OnCheckedChanged="RadioButton_CheckedChanged" />
                            <br />
                            <asp:RadioButton ID="RDStockTransferDetail" Text="Stock Transfer Detail" runat="server"
                                GroupName="OrderType" AutoPostBack="true" CssClass="radiobuttonnormal" OnCheckedChanged="RadioButton_CheckedChanged" />
                            <br />
                            <asp:RadioButton ID="RDTotalStock" Text="Total Stock" runat="server" GroupName="OrderType"
                                AutoPostBack="True" CssClass="radiobuttonnormal" OnCheckedChanged="RadioButton_CheckedChanged" />
                            <br />
                            
                            <asp:RadioButton ID="RDOneDayReport" Text="One Day Report" runat="server" GroupName="OrderType"
                                AutoPostBack="True" CssClass="radiobuttonnormal" OnCheckedChanged="RadioButton_CheckedChanged" />
                            <br />
                            <asp:RadioButton ID="RDrawmaterialopeningreport" Text="Raw Material Opening & Closing Report"
                                runat="server" GroupName="OrderType" CssClass="radiobuttonnormal" AutoPostBack="true"
                                OnCheckedChanged="RadioButton_CheckedChanged" />
                            <br />
                            <asp:RadioButton ID="RDMonthlystock" Text="Monthly Stock flow" runat="server" GroupName="OrderType"
                                AutoPostBack="true" CssClass="radiobuttonnormal" OnCheckedChanged="RadioButton_CheckedChanged" />
                            <br />
                            <asp:RadioButton ID="RDpurchaseissue_receivedetail" Text="Purchase Issue_Receive Detail"
                                runat="server" GroupName="OrderType" CssClass="radiobuttonnormal" AutoPostBack="true"
                                OnCheckedChanged="RadioButton_CheckedChanged" />
                            <br />
                            <asp:RadioButton ID="RDPurchaseMaterialasondate" Text="Purchase Material as on Date"
                                runat="server" CssClass="radiobuttonnormal" GroupName="OrderType" AutoPostBack="true"
                                OnCheckedChanged="RadioButton_CheckedChanged" />
                            <br />
                            <asp:RadioButton ID="RDfinishedstockasondate" Text="Finished Stock As on Date" runat="server"
                                GroupName="OrderType" CssClass="radiobuttonnormal" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                            <br />
                            
                            
                            <asp:RadioButton ID="RDRawMaterialStockGodownWise" Text="Raw Material Stock Godown Wise"
                                runat="server" GroupName="OrderType" OnCheckedChanged="RDRawMaterialStockGodownWise_CheckedChanged"
                                AutoPostBack="True" CssClass="radiobuttonnormal" />
                            <br />
                            
                            <asp:RadioButton ID="RDRawMaterialOrderAssignIssueStock" Text="Raw Material Order Assign Issue"
                                runat="server" GroupName="OrderType" OnCheckedChanged="RadioButton_CheckedChanged"
                                AutoPostBack="True" Visible="false" CssClass="radiobuttonnormal" />
                            <br />
                            <asp:RadioButton ID="RDFinishedStockProcessWise" Text="Finished Stock ProcessWise"
                                runat="server" GroupName="OrderType" OnCheckedChanged="RadioButton_CheckedChanged"
                                AutoPostBack="True" Visible="false" CssClass="radiobuttonnormal" />
                            <br />
                        </div>
                    </td>
                    <td style="width: 70%" valign="top">
                        <div style="float: left; width: 90%; max-height: 600px;">
                            <table border="1" cellspacing="2">
                                <tr id="TRDDCompany" runat="server">
                                    <td style="border-style: dotted">
                                        <asp:Label ID="Label1" runat="server" CssClass="labelbold" Text="CompanyName"></asp:Label>
                                    </td>
                                    <td colspan="3" style="border-style: dotted">
                                        <asp:DropDownList ID="DDCompany" runat="server" CssClass="dropdown" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                               <%-- <tr id="TR1" runat="server">
                                        <td class="style2" colspan="3" style="border-style: dotted">
                                            <asp:DropDownList ID="DDBranchName" runat="server" CssClass="dropdown" Width="250px">
                                            </asp:DropDownList>
                                        </td>
                                        </tr> --%>
                                <tr id="TRDDCustName" runat="server" visible="true">
                                    <td style="border-style: dotted">
                                        <asp:Label ID="lblCustname" runat="server" CssClass="labelbold" Text="Customer"></asp:Label>
                                    </td>
                                    <td colspan="3" class="style2" style="border-style: dotted">
                                        <asp:DropDownList ID="DDcustomer" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="250px" OnSelectedIndexChanged="DDcustomer_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRDDOrder" runat="server" visible="true">
                                    <td style="border-style: dotted">
                                        <asp:Label ID="lblOrder" runat="server" CssClass="labelbold" Text="Order"></asp:Label>
                                    </td>
                                    <td colspan="3" class="style2" style="border-style: dotted">
                                        <asp:DropDownList ID="DDOrder" runat="server" CssClass="dropdown" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr id="TRProcessName" runat="server" visible="false">
                                    <td style="border-style: dotted">
                                        <asp:Label ID="Label2" runat="server" CssClass="labelbold" Text="Process Name"></asp:Label>
                                    </td>
                                    <td colspan="3" class="style2" style="border-style: dotted">
                                        <asp:DropDownList ID="DDProcessName" runat="server" CssClass="dropdown" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr id="TRcategory" runat="server">
                                    <td style="border-style: dotted">
                                        <asp:Label ID="lblcategoryname" runat="server" CssClass="labelbold" Text="Category Name"></asp:Label>
                                    </td>
                                    <td colspan="3" style="border-style: dotted">
                                        <asp:DropDownList ID="DDCategory" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="250px" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRddItemName" runat="server">
                                    <td style="border-style: dotted">
                                        <asp:Label ID="lblitemname" runat="server" CssClass="labelbold" Text="Item Name"></asp:Label>
                                    </td>
                                    <td colspan="3" style="border-style: dotted">
                                        <asp:DropDownList ID="ddItemName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="250px" OnSelectedIndexChanged="ddItemName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="ddItemName"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr id="TRDDQuality" runat="server" visible="false">
                                    <td style="border-style: dotted">
                                        <asp:Label ID="lblqualityname" runat="server" CssClass="labelbold" Text="Quality"></asp:Label>
                                    </td>
                                    <td colspan="3" style="border-style: dotted">
                                        <asp:DropDownList ID="DDQuality" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="250px" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRDDDesign" runat="server" visible="false">
                                    <td style="border-style: dotted">
                                        <asp:Label ID="lbldesignname" runat="server" CssClass="labelbold" Text="Design"></asp:Label>
                                    </td>
                                    <td colspan="3" style="border-style: dotted">
                                        <asp:DropDownList ID="DDDesign" runat="server" CssClass="dropdown" Width="250px">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="DDDesign"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr id="TRDDColor" runat="server" visible="false">
                                    <td style="border-style: dotted">
                                        <asp:Label ID="lblcolorname" runat="server" CssClass="labelbold" Text="Color"></asp:Label>
                                    </td>
                                    <td colspan="3" style="border-style: dotted">
                                        <asp:DropDownList ID="DDColor" runat="server" CssClass="dropdown" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRDDShadeColor" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="lblshadename" runat="server" CssClass="labelbold" Text="Shade Color"></asp:Label>
                                    </td>
                                    <td colspan="3" style="border-style: dotted">
                                        <asp:DropDownList ID="DDShadeColor" runat="server" CssClass="dropdown" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRDDShape" runat="server" visible="false">
                                    <td style="border-style: dotted">
                                        <asp:Label ID="lblshapename" runat="server" CssClass="labelbold" Text="Shape"></asp:Label>
                                    </td>
                                    <td colspan="3" style="border-style: dotted">
                                        <asp:DropDownList ID="DDShape" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            Width="250px" OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRDDSize" runat="server" visible="false">
                                    <td style="border-style: dotted">
                                        <asp:Label ID="lblsizename" runat="server" CssClass="labelbold" Text="Size"></asp:Label>
                                    </td>
                                    <td colspan="3" style="border-style: dotted">
                                        <asp:DropDownList ID="DDSize" runat="server" CssClass="dropdown" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRDDLotNo" runat="server" visible="true">
                                    <td style="border-style: dotted">
                                        <asp:Label ID="LblLotNo" runat="server" CssClass="labelbold" Text="Lot No"></asp:Label>
                                    </td>
                                    <td colspan="3" class="style2" style="border-style: dotted">
                                        <asp:DropDownList ID="DDLotNo" runat="server" CssClass="dropdown" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRDDGodow" runat="server">
                                    <td style="border-style: dotted">
                                        <asp:Label ID="lblGudownname" runat="server" CssClass="labelbold" Width="90px" Text="Godown Name"></asp:Label>
                                    </td>
                                    <td colspan="3" class="style2" style="border-style: dotted">
                                        <asp:DropDownList ID="DDGudown" runat="server" CssClass="dropdown" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="TRLotNoTagNo" runat="server" visible="false">
                                    <td>
                                    </td>
                                    <td>
                                        <table border="1">
                                            <tr>
                                                <td style="border-style: dotted">
                                                    <asp:Label ID="lbllotno1" Text="Lot No" CssClass="labelbold" runat="server" /><br />
                                                    <asp:TextBox ID="txtLotno" Width="120px" runat="server" AutoPostBack="true" OnTextChanged="txtLotno_TextChanged" />
                                                    <asp:AutoCompleteExtender ID="txtLotno_AutoCompleteExtender" runat="server" BehaviorID="SrchAutoComplete"
                                                        CompletionInterval="20" Enabled="True" ServiceMethod="GetLotNo" EnableCaching="true"
                                                        CompletionSetCount="20" ServicePath="~/Autocomplete.asmx" TargetControlID="txtLotno"
                                                        UseContextKey="True" ContextKey="0#0#0" MinimumPrefixLength="2">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td style="border-style: dotted">
                                                    <asp:Label ID="lblTagNo" Text="Tag No" CssClass="labelbold" runat="server" /><br />
                                                    <asp:TextBox ID="txtTagno" Width="120px" runat="server" />
                                                    <asp:AutoCompleteExtender ID="txtTagno_AutoCompleteExtender" runat="server" BehaviorID="SrchAutoComplete1"
                                                        CompletionInterval="20" Enabled="True" ServiceMethod="GetTagNo" EnableCaching="true"
                                                        CompletionSetCount="20" ServicePath="~/Autocomplete.asmx" TargetControlID="txtTagno"
                                                        UseContextKey="True" ContextKey="0#0#0" MinimumPrefixLength="2">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trforfromandtodate" runat="server" visible="false">
                                    <td style="border-style: dotted">
                                        <asp:Label ID="lblfromdate" Text="From Date" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td style="border-style: dotted">
                                        <asp:TextBox ID="txtfromdate" runat="server" Width="100px" CssClass="textb"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="txtfromdate">
                                        </asp:CalendarExtender>
                                        <asp:Label ID="lblTodate" Text="To Date" runat="server" CssClass="labelbold" />
                                        <asp:TextBox ID="txttodate" runat="server" Width="100px" CssClass="textb"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="txttodate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr id="TRDate" runat="server" visible="false">
                                    <td style="border-style: dotted">
                                        <asp:Label ID="Label5" runat="server" CssClass="labelbold" Text="Date"></asp:Label>
                                    </td>
                                    <td style="border-style: dotted">
                                        <asp:TextBox ID="TxtDate" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="TxtDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr id="TRCheckStockDetail" runat="server" visible="false">
                                    <td>
                                        <td colspan="2" style="border-style: dotted">
                                            <asp:CheckBox ID="chkStkDetail" Text="Stock Tran Detail" runat="server" CssClass="checkboxbold" />
                                            <asp:CheckBox ID="chkLotwiseTagwise" Text="Lot Tag Wise" runat="server" CssClass="checkboxbold" />
                                        </td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="border-style: dotted">
                                        <asp:Label ID="lblMessage" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDMtr" runat="server" visible="false" style="border-style: dotted">
                                        <asp:CheckBox ID="chkformtr" Text="For Mtr." CssClass="checkboxbold" runat="server" />
                                    </td>
                                    <td style="border-style: dotted">
                                        <table width="100%">
                                            <tr>
                                                <td id="TDallstockno" runat="server" visible="false">
                                                    <asp:CheckBox ID="chkallstockno" CssClass="checkboxbold" Text="For All Stock No."
                                                        runat="server" />
                                                </td>
                                                <td id="TDunconfirmcarpet" runat="server">
                                                    <asp:CheckBox ID="chkunconfirmcarpet" CssClass="checkboxbold" Text="For Unconfirm Stock No."
                                                        runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right" colspan="2" style="border-style: dotted">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" OnClick="BtnPreview_Click"
                                                        Text="Preview" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" OnClick="BtnClose_Click"
                                                        Text="Close" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDstocksummary" runat="server" visible="false">
                                        <asp:CheckBox ID="chkstocksummary" Text="For Summary" runat="server" AutoPostBack="true"
                                            CssClass="labelbold" OnCheckedChanged="chkstocksummary_CheckedChanged" />
                                    </td>
                                    <td id="TDundyed_dyed" runat="server" visible="false">
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="RDundyed" Text="Undyed" GroupName="a" CssClass="radiobuttonnormal"
                                                        runat="server" />
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="RDDyed" Text="Dyed" GroupName="a" CssClass="radiobuttonnormal"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="Trwithpurchasedetail" runat="server" visible="false">
                                        <asp:CheckBox ID="chkwithpurchasedetail" Text="With Purchase Detail" CssClass="checkboxbold"
                                            runat="server" />
                                    </td>
                                    <td id="TdPurchasesummary" runat="server" visible="false" align="left" style="border-style: dotted">
                                        <asp:CheckBox ID="chkpurchaseissuerecsummary" Text="Purchase Issue_Receive Summary"
                                            CssClass="checkboxbold" runat="server" />
                                    </td>
                                     <td align="left" colspan="2" id="trwithvalue"  runat="server" visible="false">
                                 <asp:CheckBox ID="chkwithval" Text="With Value" CssClass="checkboxbold" runat="server" />
                                 </td>
                                    <td id="TDExportExcel" runat="server" visible="false" align="left" style="border-style: dotted">
                                        <asp:CheckBox ID="ChkExportExcel" Text="Export Excel" CssClass="checkboxbold" runat="server" />
                                    </td>
                                    <td id="TDForLedgerDetail" runat="server" visible="false" align="left" style="border-style: dotted">
                                        <asp:CheckBox ID="ChkForLedgerTransaction" Text="For Ledger Trans" AutoPostBack="true"
                                            OnCheckedChanged="ChkForLedgerTransaction_CheckedChanged" CssClass="checkboxbold"
                                            runat="server" />
                                    </td>
                                    <td id="TDForTotalStock" runat="server" visible="false" align="left" style="border-style: dotted">
                                        <asp:CheckBox ID="ChkForTotalStock" Text="For Total Stock" CssClass="checkboxbold"
                                            runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDstockupto" runat="server" visible="false">
                                        <asp:Label ID="lblstockupto" Text="Stock up to" CssClass="labelbold" runat="server" /><br />
                                        <asp:TextBox ID="txtstockupto" CssClass="textb" Width="100px" runat="server" />
                                        <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="txtstockupto">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td id="TDForIssueRegister" runat="server" visible="false">
                                        <asp:CheckBox ID="ChkForIssueRegister" Text="For Issue Register" CssClass="checkboxbold"
                                            runat="server" />
                                    </td>
                                     <td id="TDWithBuyerOrderNo" runat="server" visible="false">
                                        <asp:CheckBox ID="ChkWithBuyerOrderNo" Text="With BuyerOrder No" CssClass="checkboxbold"
                                            runat="server" />
                                    </td>
                                     <td id="TDFinishedStockWithoutstockNo" runat="server" visible="false">
                                        <asp:CheckBox ID="ChkWithoutStockNo" Text="Without StockNo" CssClass="checkboxbold"
                                            runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnPreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
