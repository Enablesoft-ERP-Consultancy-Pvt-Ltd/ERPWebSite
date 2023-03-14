<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmCustomerOrderReportWithProduction.aspx.cs"
    Inherits="Masters_ReportForms_FrmCustomerOrderReportWithProduction" MasterPageFile="~/ERPmaster.master"
    Title="Order Status Report" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="updatepenl1" runat="server">
        <ContentTemplate>
            <table width="85%">
                <tr>
                    <td style="width: 300px" valign="top" align="left">
                        <div style="width: 287px; padding-top: 5px; height: 300px; float: left; border-style: solid;
                            border-width: thin">
                            &nbsp;&nbsp;
                            <br />
                            &nbsp;&nbsp;
                            <asp:RadioButton ID="RDOrderProductionStatus" runat="server" Text="Order Production Status"
                                GroupName="OrderType" CssClass="labelbold" AutoPostBack="True" OnCheckedChanged="RDOrderProductionStatus_CheckedChanged" />
                            <br />
                            <%--  &nbsp;&nbsp;
                                 <asp:RadioButton ID="RDFinishingReceiveDetail" runat="server" Text="Finishing Receive Detail"
                                    GroupName="OrderType" CssClass="labelbold" AutoPostBack="True" OnCheckedChanged="RDFinishingReceiveDetail_CheckedChanged" />                               
                                &nbsp;&nbsp; 
                            --%>
                        </div>
                    </td>
                    <td>
                        <div>
                            <div style="float: left; width: 450px; vertical-align: top;">
                                <%--<div style="float: left; width: 450px; height: 500px;">--%>
                                <%--<asp:Panel runat="server" ID="panel1" Style="border: 1px groove Teal; border: 1px solid black;"
                        Width="100%">--%>
                                <div style="padding: 0px 0px 0px 20px">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" CssClass="labelbold" Text="CompanyName"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DDCompany" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="300px" OnSelectedIndexChanged="DDCompany_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDCompany"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                        </tr>
                                        <tr id="TRcustcode" runat="server" visible="true">
                                            <td>
                                                <asp:Label ID="Label10" runat="server" CssClass="labelbold" Text="Customer code"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DDcustcode" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="300px" OnSelectedIndexChanged="DDcustcode_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="TRCategory" runat="server" visible="true">
                                            <td>
                                                <asp:Label ID="lblcategoryname" runat="server" CssClass="labelbold" Text="Category Name"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DDCategory" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="300px">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDCategory"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                        </tr>
                                        <tr id="TRddItemName" runat="server">
                                            <td>
                                                <asp:Label ID="lblitemname" runat="server" CssClass="labelbold" Text="Item Name"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="ddItemName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="300px" OnSelectedIndexChanged="ddItemName_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="ddItemName"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                        </tr>
                                        <tr id="TRDDQuality" runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="lblqualityname" runat="server" CssClass="labelbold" Text="Quality"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DDQuality" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="300px" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="DDQuality"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                        </tr>
                                        <tr id="TRDDDesign" runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="lbldesignname" runat="server" CssClass="labelbold" Text="Design"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DDDesign" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="300px">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="DDDesign"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                        </tr>
                                        <tr id="TRDDColor" runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="lblcolorname" runat="server" CssClass="labelbold" Text="Color"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DDColor" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="300px">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="DDColor"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                        </tr>
                                        <tr id="TRDDShape" runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="lblshapename" runat="server" CssClass="labelbold" Text="Shape"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DDShape" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="300px">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="DDShape"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                        </tr>
                                        <tr id="TRDDSize" runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="lblsizename" runat="server" CssClass="labelbold" Text="Size"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DDSize" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="250px">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender11" runat="server" TargetControlID="DDSize"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                                <%--  <asp:CheckBox ID="chkmtr" runat="server" Text="For Mtr." Font-Bold="true" OnCheckedChanged="chkmtr_CheckedChanged"
                                                AutoPostBack="true" />--%>
                                            </td>
                                        </tr>
                                        <tr id="TRDDShadeColor" runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="lblshadename" runat="server" CssClass="labelbold" Text="Shade Color"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="DDShadeColor" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                    Width="300px">
                                                </asp:DropDownList>
                                                <cc1:ListSearchExtender ID="ListSearchExtender12" runat="server" TargetControlID="DDShadeColor"
                                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                </cc1:ListSearchExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="left">
                                                <asp:CheckBox ID="ChkForDate" runat="server" Text="Check For Date" CssClass="checkboxbold"
                                                    AutoPostBack="True" OnCheckedChanged="ChkForDate_CheckedChanged" Visible="false"
                                                    Checked="true" />
                                            </td>
                                        </tr>
                                        <tr id="trDates" runat="server" visible="true">
                                            <td>
                                                <asp:Label ID="Label5" runat="server" CssClass="labelbold" Text="From Date"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtFromDate" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                                    TargetControlID="TxtFromDate">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="Label6" runat="server" CssClass="labelbold" Text="To Date" Width="80px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtToDate" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                                    TargetControlID="TxtToDate">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblMessage" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <%--   <tr>
                                        <td colspan="4">
                                            <asp:CheckBox ID="ChkForPendingStockNo" runat="server" Text="Detail For Pending StockNo"
                                                Visible="false" CssClass="checkboxbold" AutoPostBack="True" OnCheckedChanged="ChkForPendingStockNo_CheckedChanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:CheckBox ID="ChkForFinisherNillDetail" runat="server" Text="Finisher Nill Detail" Visible="false"
                                                CssClass="checkboxbold" AutoPostBack="True" OnCheckedChanged="ChkForFinisherNillDetail_CheckedChanged" />
                                        </td>
                                    </tr>
                                        --%>
                                        <tr>
                                            <td align="right" colspan="4">
                                                &nbsp;<asp:Button ID="BtnShowData" runat="server" CssClass="buttonnorm" OnClick="BtnShowData_Click"
                                                    OnClientClick="return Validate();" Text="Show Data" />
                                                &nbsp;<asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" OnClick="BtnClose_Click"
                                                    Text="Close" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div id="Div1" runat="server" style="width: 600px;">
                                <table id="Table1" runat="server">
                                    <tr id="Tr1" runat="server">
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="height: 300px; width: 700px; overflow: auto">
                                                <asp:GridView ID="GDOrderDetail" runat="server" DataKeyNames="OrderId" AutoGenerateColumns="False"
                                                    EmptyDataText="No. Records found.">
                                                    <HeaderStyle CssClass="gvheaders" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <Columns>
                                                        <%--   <asp:BoundField DataField="OrderId" HeaderText="OrderId">
                                                            <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                                            <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                        </asp:BoundField>--%>
                                                        <asp:TemplateField HeaderText="OrderId" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOrderId" Text='<%#Bind("OrderId") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="PONo" Visible="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustomerOrderNo" Text='<%#Bind("CustomerOrderNo") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="PODate" Visible="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOrderDate" Text='<%#Bind("OrderDate") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="BuyerCode" Visible="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustomerCode" Text='<%#Bind("CustomerCode") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="OrderQty" Visible="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOrderQty" Text='<%#Bind("OrderQty") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="PendingOrderQty" Visible="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPendingOrderQty" Text='<%#Bind("PendingOrderQty") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="WOQty" Visible="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWOQty" Text='<%#Bind("WOQty") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="WPQty" Visible="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWPQty" Text='<%#Bind("WPQty") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="FinisherPQty" Visible="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFinisherPQty" Text='<%#Bind("FinisherPQty") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="GodownIssQty" Visible="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGodownIssueQty" Text='<%#Bind("GodownIssueQty") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="WeaverShow">
                                                            <ItemTemplate>
                                                                <asp:Button ID="BTNShowWeaver" runat="server" Text="WeaverPreview" CssClass="dropdown"
                                                                    OnClick="BTNShowWeaver_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="FinisherShow">
                                                            <ItemTemplate>
                                                                <%--<asp:Button ID="BTNShowAmt" runat="server" Text="Preview" CssClass="dropdown" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"                                                               
                                                                    OnClick="BTNShowAmt_Click" />--%>
                                                                <asp:Button ID="BTNShowFinisher" runat="server" Text="FinisherPreview" CssClass="dropdown"
                                                                    OnClick="BTNShowFinisher_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnShowData" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
