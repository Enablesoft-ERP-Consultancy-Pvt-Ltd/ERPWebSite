<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Draft_order.aspx.cs" ViewStateMode="Enabled"
    Inherits="Masters_Order_Draft_order" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>CompCompany</title>
    <link rel="stylesheet" type="text/css" href="~/App_Themes/Default/Style.css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function priview() {
            window.open('../../ReportViewer.aspx');
        }
        function Addcons() {
            window.open('../Carpet/DefineBomAndConsumption.aspx?ZZZ=1', '', 'Height=600px,width=1000px');
        } 
    </script>
    <style type="text/css">
        .style1
        {
            width: 815px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" border="1">
        <tr style="width: 100%" align="center">
            <td height="66px" align="center">
                <%--style="background-image:url(Images/header.jpg)" --%>
                <%--<div><img src="Images/header.jpg" alt="" /></div>--%>
                <asp:Image ID="Image1" ImageUrl="~/Images/header.jpg" runat="server" />
            </td>
            <td style="background-color: #0080C0;" width="100px" valign="bottom">
                <asp:Image ID="imgLogo" align="left" runat="server" Height="66px" Width="111px" />
                <span style="color: Black; margin-left: 30px; font-family: Arial; font-size: xx-large">
                    <strong><em><i><font size="4" face="GEORGIA">
                        <asp:Label ID="LblCompanyName" runat="server" Text=""></asp:Label></font></i></em></strong></span>
                <br />
                <i><font size="2" face="GEORGIA">
                    <asp:Label ID="LblUserName" ForeColor="White" runat="server" Text=""></asp:Label></font></i>
            </td>
        </tr>
        <tr bgcolor="#999999">
            <td class="style1">
                <uc2:ucmenu ID="ucmenu1" runat="server" />
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </td>
            <td width="25%">
                <asp:Button Width="100%" BorderStyle="None" BackColor="#999999" ID="BtnLogout" runat="server"
                    Text="Logout" OnClick="BtnLogout_Click" />
            </td>
        </tr>
        <tr>
            <td align="center" height="inherit" valign="top" class="style1" colspan="2">
                <div id="1" style="height: auto" align="left">
                    <div>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="TxtOrderDetailId" runat="server" Width="0px" Height="0px"></asp:TextBox>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:CheckBox ID="ChkEditOrder" runat="server" CssClass="checkboxbold" Text="Edit Order"
                                                AutoPostBack="True" OnCheckedChanged="ChkEditOrder_CheckedChanged" />
                                        </td>
                                        <td colspan="3" class="tdstyle">
                                            <asp:CheckBox ID="ChkForApprovalOrder" runat="server" CssClass="checkboxbold" Text="Check For Approval Order"
                                                AutoPostBack="True" OnCheckedChanged="ChkForApprovalOrder_CheckedChanged" />
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle">
                                            COMPANY NAME<b style="color: Red">*</b>
                                        </td>
                                        <td class="tdstyle">
                                            CUST CODE<b style="color: Red">*</b>
                                        </td>
                                        <td class="tdstyle">
                                            CUST.ORD. NO
                                        </td>
                                        <td class="tdstyle">
                                            LOCAL ORDER<b style="color: Red">*</b>
                                        </td>
                                        <td class="tdstyle">
                                            ORDER DATE<b style="color: Red">*</b>
                                        </td>
                                        <td class="tdstyle">
                                            DELIVERY DATE<b style="color: Red">*</b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" runat="server" AutoPostBack="True"
                                                Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="dropdown" ID="DDCustomerCode" runat="server" AutoPostBack="True"
                                                Width="250px" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="dropdown" ID="ddorderno" runat="server" Width="90px"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddorderno_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="textb" ID="Textlocalorder" runat="server" Width="100px"></asp:TextBox>
                                            <td>
                                                <asp:TextBox CssClass="textb" ID="TxtOrderDate" runat="server" Width="100px"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                                    TargetControlID="TxtOrderDate">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="textb" ID="TxtDeliveryDate" runat="server" Width="120px"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                                    TargetControlID="TxtDeliveryDate">
                                                </asp:CalendarExtender>
                                            </td>
                                    </tr>
                                </table>
                                <table width="100%" border="1">
                                    <tr>
                                        <td>
                                            <asp:Label ID="LblErrorMessage" runat="server" Font-Bold="true" ForeColor="RED" Text="Some IMportent Fields are Missing......."
                                                Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="tr1" runat="server" style="display: none">
                                        <td valign="top" class="tdstyle">
                                            Remark
                                            <asp:TextBox ID="Txtremark" runat="server" TextMode="MultiLine" Width="300px" Height="100Px"></asp:TextBox>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Save" runat="server" OnClick="BtnSave_Click"
                                                OnClientClick="return confirm('Do you want to save data?')" />
                                        </td>
                                    </tr>
                                    <tr align="center" id="trdrig" runat="server" style="display: none">
                                        <td align="center">
                                            <div style="width: 100%; height: 250px; overflow: scroll">
                                                <asp:GridView ID="DGOrderDetail" Width="100%" runat="server" DataKeyNames="Sr_No"
                                                    AutoGenerateColumns="False" OnRowDataBound="DGOrderDetail_RowDataBound" OnRowCommand="DGOrderDetail_RowCommand"
                                                    CssClass="grid-view" OnRowCreated="DGOrderDetail_RowCreated">
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Sr_No" HeaderText="Sr_No" />
                                                        <asp:BoundField DataField="ourcode" HeaderText="Our Code" />
                                                        <asp:BoundField DataField="buyercode" HeaderText="Buyer Code" />
                                                        <asp:BoundField DataField="CATEGORY" HeaderText="CATEGORY" />
                                                        <asp:BoundField DataField="ITEMNAME" HeaderText="ITEMNAME" />
                                                        <asp:BoundField DataField="Qty" HeaderText="Qty" />
                                                        <asp:BoundField DataField="Area" HeaderText="Area" />
                                                        <asp:BoundField DataField="DESCRIPTION" HeaderText="DESCRIPTION" />
                                                        <asp:TemplateField HeaderText="PHOTO">
                                                            <ItemTemplate>
                                                                <asp:Image ID="Image1" Width="100px" Height="50px" runat="server" ImageUrl='<%# "~/ImageHandler.ashx?ID=" + Eval("Sr_No")+"&img=4"%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="PPInstruction" HeaderText="PP.Instruction" />
                                                        <asp:TemplateField HeaderText="Description">
                                                            <ItemTemplate>
                                                                <asp:Button ID="BTNAdd" CssClass="buttonnorm" runat="server" Text="Add" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                    OnClick="BTNadd_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="consumption">
                                                            <ItemTemplate>
                                                                <asp:Button ID="BTNconsum" CssClass="buttonnorm" runat="server" Text="Show" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                    OnClick="BTNcon_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ADD consumption">
                                                            <ItemTemplate>
                                                                <asp:Button CssClass="buttonnorm" ID="btnconsump" runat="server" Text="Add consumption"
                                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" OnClick="BTNaddcon_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="">
                                                            <ItemTemplate>
                                                                <asp:Label ID="item_finished_id" runat="server" Text='<%# BIND("item_finished_id") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="0" />
                                                            <HeaderStyle Width="0" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="TxtLabel" runat="server" Text="Green Rows Shows That Items Consumption Was Not Defined"
                                                ForeColor="Green" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Button CssClass="buttonnorm" ID="btnreport" Text="Preview" runat="server" OnClientClick="return priview()" />
                                        </td>
                                        <td>
                                            <asp:Button ID="refreshitem" runat="server" Height="0px" OnClick="refreshitem_Click"
                                                Text="." Width="0px" BackColor="White" BorderColor="White" BorderWidth="0px"
                                                ForeColor="White" />
                                        </td>
                                    </tr>
                                    <tr align="center" id="trgridcus" runat="server" style="display: none">
                                        <td align="center">
                                            <div style="width: 100%; height: 200px; overflow: scroll">
                                                <asp:GridView ID="gv_cus" Width="100%" runat="server" AutoGenerateColumns="False"
                                                    CssClass="grid-view" OnRowCreated="gv_cus_RowCreated">
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Item" HeaderText="ITEM" />
                                                        <asp:BoundField DataField="process_name" HeaderText="PROCESS" />
                                                        <asp:BoundField DataField="input_item" HeaderText="INPUT ITEM" />
                                                        <asp:BoundField DataField="INPUT_QTY" HeaderText="INPUT QTY" />
                                                        <asp:BoundField DataField="INPUT_LOSS" HeaderText="INPUT LOSS" />
                                                        <asp:BoundField DataField="INPUT_RATE" HeaderText="INPUT RATE" />
                                                        <asp:BoundField DataField="output_item" HeaderText="OUTPUT ITEM" />
                                                        <asp:BoundField DataField="OUTPUT_QNT" HeaderText="OUTPUT QTY" />
                                                        <asp:BoundField DataField="OUTPUT_RATE" HeaderText="OUTPUT RATE" />
                                                    </Columns>
                                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Button CssClass="buttonnorm" ID="BtnForApprovalOrder" Visible="false" Text="For Approval Order"
                                                runat="server" OnClientClick="return confirm('Do You Want To Approval Order?')"
                                                OnClick="BtnForApprovalOrder_Click" />
                                            <asp:Button CssClass="buttonnorm" ID="BtnOrderDetailWithConsumption" Text="OrderDetail With Consumption"
                                                runat="server" OnClick="BtnOrderDetailWithConsumption_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div style="background-color: #edf3fe; text-align: center; height: 43px; width: 100%">
                    <span class="labelcopyr">&copy; Enablesoft Erp Consultancy Pvt. Ltd.All rights reserved.</span>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
