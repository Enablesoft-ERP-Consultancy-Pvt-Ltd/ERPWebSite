<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="paking_stock_detail.aspx.cs"
    Inherits="Masters_Packing_paking_stock_detail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script src="../../Scripts/JScript.js" type="text/javascript"></script>
<script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
<script type="text/javascript">
    function CloseForm() {
        window.location.href = "../../main.aspx";
    }
    function report() {
        //var varReportName = "Reports/BankReport.rpt";
        //window.open('../../ReportViewer.aspx?ReportName=' + varReportName + '& CommanFn='+""+'', '');
        window.open('../../ReportViewer.aspx', '');
    }
    function YourFunctionName(msg) {
        var txt = msg;
        alert(txt);
    }
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    <table width="100%">
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
                            <td width="75%">
                                <uc1:ucmenu ID="ucmenu1" runat="server" />
                                <asp:ScriptManager ID="ScriptManager1" runat="server">
                                </asp:ScriptManager>
                            </td>
                            <td width="25%">
                                <asp:UpdatePanel ID="up" runat="server">
                                    <ContentTemplate>
                                        <asp:Button Width="100%" BorderStyle="None" BackColor="#999999" ID="BtnLogout" runat="server"
                                            Text="Logout" OnClick="BtnLogout_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table width="500px">
                                <tr id="Tr1" runat="server">
                                    <td id="Td1" class="tdstyle">
                                        Company Name<br />
                                        <asp:DropDownList ID="ddCompName" runat="server" TabIndex="1" CssClass="dropdown">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender14" runat="server" TargetControlID="ddCompName"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="tdcustomer" runat="server" class="tdstyle">
                                        Customer Code<br />
                                        <asp:DropDownList ID="ddcustomercode" runat="server" CssClass="dropdown" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddcustomercode_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddcustomercode"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td id="tdorderno" runat="server" class="tdstyle">
                                        OrderNo.<br />
                                        <asp:DropDownList ID="ddorderno" runat="server" CssClass="dropdown" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddorderno_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddorderno"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="LblErrorMessage" runat="server" Font-Bold="true" ForeColor="RED" Text="ONE OR MORE MANDATORY FIELDS ARE MISSING......."
                                            Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:GridView ID="DGSHOWDATA" runat="server" AutoGenerateColumns="False" DataKeyNames="pdetail"
                                            OnRowDataBound="DGSHOWDATA_RowDataBound" OnRowCommand="DGSHOWDATA_RowCommand"
                                            CssClass="grid-view" OnRowCreated="DGSHOWDATA_RowCreated">
                                            <HeaderStyle CssClass="gvheader" />
                                            <AlternatingRowStyle CssClass="gvalt" />
                                            <RowStyle CssClass="gvrow" />
                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                            <Columns>
                                                <asp:BoundField DataField="PackingType" HeaderText="PackingType" />
                                                <asp:BoundField DataField="Length" HeaderText="Length" />
                                                <asp:BoundField DataField="width" HeaderText="Width" />
                                                <asp:BoundField DataField="Height" HeaderText="Height" />
                                                <asp:BoundField DataField="gsm" HeaderText="GSM" />
                                                <asp:BoundField DataField="gsm2" HeaderText="GSM2" />
                                                <asp:BoundField DataField="ply" HeaderText="Ply" />
                                                <asp:TemplateField HeaderText="Total Qty">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbltqty" runat="server" Visible="true" Text='<%# Bind("qty") %>' />
                                                        <asp:Label ID="lbldetailid" runat="server" Visible="false" Text='<%# Bind("pdetail") %>' />
                                                        <asp:Label ID="lblpid" runat="server" Visible="false" Text='<%# Bind("pid") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="80px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Receive Qty">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblrqty" runat="server" Visible="true" Text='<%# Bind("receiveqty") %>' />
                                                        <%--<asp:TextBox ID="TXTRAte" runat="server" Width="70px"  Text='<%# Bind("Rate") %>' AutoPostBack="true"
                          ontextchanged="Txtrate_TextChanged"></asp:TextBox>--%>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="80px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Balance Qty">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbqty" runat="server" Text='<%# Bind("pqty") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="80px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Show">
                                                    <ItemTemplate>
                                                        <asp:Button ID="BTNdetail" CssClass="buttonnorm" runat="server" Text="Detail" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="right">
                                        <asp:Button ID="btnpriview" runat="server" Text="Preview" Visible="false" OnClick="btnpriview_Click"
                                            CssClass="buttonnorm" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
