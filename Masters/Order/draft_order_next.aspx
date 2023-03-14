<%@ Page Language="C#" AutoEventWireup="true" CodeFile="draft_order_next.aspx.cs"
    Inherits="Masters_Order_draft_order_next" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Company</title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function priview() {
            window.open('../../ReportViewer.aspx');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="1">
            <tr style="width: 100%" align="center">
                <td height="66px" align="center">
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
                                            <td class="tdstyle">
                                                COMPANY NAME<b style="color: Red">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; *</b>
                                            </td>
                                            <td class="tdstyle">
                                                CUST CODE<b style="color: Red">&nbsp;&nbsp;&nbsp;&nbsp; *</b>
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
                                                    Width="300px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList CssClass="dropdown" ID="DDCustomerCode" runat="server" AutoPostBack="True"
                                                    Width="250px" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList CssClass="dropdown" ID="ddorderno" runat="server" Width="150px"
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
                                        <tr>
                                            <td>
                                                <asp:Label ID="LblErrorMessage" runat="server" Font-Bold="true" ForeColor="RED" Text="Some IMportent Fields are Missing......."
                                                    Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtOrderDetailId" runat="server" Width="0px" Height="0px"></asp:TextBox>
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
                                            <td align="center" colspan="5">
                                                <div style="width: 100%; height: 200px; overflow: scroll">
                                                    <asp:GridView ID="DGOrderDetail" Width="100%" runat="server" DataKeyNames="Sr_No"
                                                        AutoGenerateColumns="False" OnRowDataBound="DGOrderDetail_RowDataBound" OnRowCommand="DGOrderDetail_RowCommand"
                                                        OnSelectedIndexChanged="DGOrderDetail_SelectedIndexChanged" CssClass="grid-view"
                                                        OnRowCreated="DGOrderDetail_RowCreated">
                                                        <AlternatingRowStyle BackColor="White" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chkbox" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                        OnCheckedChanged="ChkEditOrder_CheckedChanged" />
                                                                    <%-- <asp:CheckBox ID="CheckBox1" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />--%>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="80px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Sr_No" HeaderText="Sr_No" />
                                                            <asp:BoundField DataField="ourcode" HeaderText="Our Code" />
                                                            <asp:BoundField DataField="buyercode" HeaderText="Buyer Code" />
                                                            <asp:BoundField DataField="CATEGORY" HeaderText="CATEGORY" />
                                                            <asp:BoundField DataField="ITEMNAME" HeaderText="ITEMNAME" />
                                                            <asp:BoundField DataField="Qty" HeaderText="Qty" />
                                                            <asp:BoundField DataField="Area" HeaderText="Area" />
                                                            <asp:BoundField DataField="DESCRIPTION" HeaderText="DESCRIPTION" />
                                                            <asp:BoundField DataField="Remark" HeaderText="PP.Instruction" />
                                                            <asp:TemplateField HeaderText="Description">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="BTNAdd" runat="server" Text="Show" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                        CssClass="buttonnorm" OnClientClick="return priview()" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="BTNAddNew" CssClass="buttonnorm" runat="server" Text="Add" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                        OnClick="BTNaddNew_Click" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblgreen" runat="server" Font-Bold="true" ForeColor="Green" Text="Green Rows Shows that items consumption was modified...."
                                                    Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <asp:HiddenField ID="hncode" runat="server" Visible="false" />
                    <asp:HiddenField ID="hntot" runat="server" Visible="false" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
