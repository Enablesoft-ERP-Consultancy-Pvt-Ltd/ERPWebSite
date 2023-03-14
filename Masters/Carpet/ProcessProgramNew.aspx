<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProcessProgramNew.aspx.cs"
    EnableEventValidation="false" Inherits="Masters_Carpet_ProcessProgramNew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript" src="../../Scripts/Fixfocus2.js"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Preview() {
            window.open('../../ReportViewer.aspx');
        }
        function SaveData() {
            var a = document.getElementById("Note").value;
            var result;
            //            if (a != "") {
            //                result = window.confirm('Consumption Not Defined! Do you want to save data...')
            //            }
            //            else {
            //                result = confirm('Do you want to save data?');
            //            }
            result = confirm('Do you want to save data?');
            if (result)
                return true;
            else
                return false;
        }
    </script>
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
    <table width="75%">
        <tr>
            <td align="center" height="inherit" valign="top" class="style1" colspan="2">
                <div id="1" style="height: auto" align="left">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div>
                                <table width="75%">
                                    <tr>
                                        <td colspan="2" class="tdstyle">
                                            <asp:CheckBox ID="ChkCurrentConnsumption" runat="server" CssClass="checkboxbold"
                                                OnCheckedChanged="ChkCurrentConnsumption_CheckedChanged" Text="Check for Current Connsumption" />
                                        </td>
                                        <td class="tdstyle">
                                            <asp:CheckBox ID="ChekEdit" runat="server" CssClass="checkboxbold" AutoPostBack="True"
                                                Text="Chek For Edit" OnCheckedChanged="ChekEdit_CheckedChanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle">
                                            <asp:Label ID="lblcompany" runat="server" Text="Company Name" Width="100%" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="ddcompany" runat="server" AutoPostBack="True" Width="250px"
                                                CssClass="dropdown" OnSelectedIndexChanged="ddcompany_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddcompany"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="lblprocess" runat="server" Text="Process Name" Width="100%" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="ddprocess" runat="server" Width="150px" CssClass="dropdown"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddprocess_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddprocess"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="lblcustomer" runat="server" Text="Customer Code" Width="100%" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="ddcustomer" runat="server" AutoPostBack="True" Width="250px"
                                                CssClass="dropdown" OnSelectedIndexChanged="ddcustomer_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddcustomer"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="lblprocessprogram1" runat="server" Text="Process Program" Width="100%"
                                                Visible="false" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:DropDownList ID="ddprocessprogram" runat="server" CssClass="dropdown" Width="150px"
                                                AutoPostBack="True" Visible="false" OnSelectedIndexChanged="ddprocessprogram_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="ddprocessprogram"
                                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                            </cc1:ListSearchExtender>
                                        </td>
                                        <td class="tdstyle">
                                            <asp:Label ID="lblprocessprogram" runat="server" Text="Process Program" Width="100%"
                                                CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtprocessprogram" runat="server" ReadOnly="True" Width="150px"
                                                CssClass="textb"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%">
                                    <tr>
                                        <td class="tdstyle">
                                            <div style="height: 150px; width: 100%; overflow: scroll">
                                                <asp:Label ID="lblchekboxlist" runat="server" Text="LocalOrder/OrderNo" CssClass="labelbold"></asp:Label>
                                                <br />
                                                <asp:CheckBoxList ID="chekboxlist" runat="server" AutoPostBack="True" Width="600px"
                                                    CssClass="checkboxnormal" OnSelectedIndexChanged="chekboxlist_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblerror" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnsave" runat="server" Text="Save" OnClientClick="return SaveData();"
                                                OnClick="btnsave_Click" CssClass="buttonnorm" />
                                            &nbsp;<asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                                CssClass="buttonnorm" />
                                            &nbsp;<asp:Button ID="BtnPreview" runat="server" Text="Preview" Visible="False" CssClass="buttonnorm preview_width"
                                                OnClick="BtnPreview_Click" />
                                            <%-- &nbsp;<asp:Button ID="BtnLocalOcReport" runat="server" Text="LocalOC Report" Visible="False"
                                                CssClass="buttonnorm" OnClick="BtnLocalOcReport_Click" />--%>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr id="Note1" runat="server">
                                        <td>
                                            <asp:TextBox ID="txtgreen" BackColor="Green" Width="15px" CssClass="textb" runat="server"></asp:TextBox>
                                            <asp:TextBox ID="Note" runat="server" BorderStyle="None" Width="180px" Text="Consumption Defined"
                                                CssClass="textb" Font-Bold="true"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtRed" BackColor="Red" Width="15px" CssClass="textb" runat="server"></asp:TextBox>
                                            <asp:TextBox ID="NoteRed" runat="server" BorderStyle="None" Width="210px" Text="Consumption Not Defined"
                                                CssClass="textb" Font-Bold="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <div style="width: 700px; height: 150px; overflow: scroll">
                                                <asp:GridView ID="DgOrderConsumption" runat="server" DataKeyNames="OrderDetailId"
                                                    AutoGenerateColumns="False" OnRowDataBound="DgOrderConsumption_RowDataBound"
                                                    CssClass="grid-views">
                                                    <HeaderStyle CssClass="gvheaders" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <RowStyle CssClass="gvrow" />
                                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                                    <Columns>
                                                        <%--<asp:BoundField DataField="OrderDetailId"/>--%>
                                                        <asp:BoundField HeaderText="Description" DataField="Description">
                                                            <HeaderStyle HorizontalAlign="Left" Width="450px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="450px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Quantity" DataField="Qty" NullDisplayText="-------">
                                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ConsmpOrderDetailId" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                        <td colspan="1">
                                            <div style="width: 300px; height: 150px; overflow: scroll">
                                                <asp:GridView ID="Dgprocessprogram" runat="server" DataKeyNames="Sr_No" AutoGenerateColumns="False"
                                                    CssClass="grid-views">
                                                    <HeaderStyle CssClass="gvheaders" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <RowStyle CssClass="gvrow" />
                                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                                    <Columns>
                                                        <%--<asp:BoundField DataField="OrderDetailId"/>--%>
                                                        <asp:BoundField HeaderText="CustomerOrderNo" DataField="CustomerOrderNo">
                                                            <HeaderStyle HorizontalAlign="Left" Width="450px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="450px" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div style="width: 1000px; height: 400px; overflow: scroll">
                                                <asp:GridView ID="DgConsumption" runat="server" DataKeyNames="FINISHEDID" AutoGenerateColumns="False"
                                                    CssClass="grid-views">
                                                    <HeaderStyle CssClass="gvheaders" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <RowStyle CssClass="gvrow" />
                                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                                    <Columns>
                                                        <asp:BoundField DataField="FINISHEDID" HeaderText="SrNo.">
                                                            <HeaderStyle HorizontalAlign="Left" Width="10px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="10px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Description" HeaderText="Description">
                                                            <HeaderStyle HorizontalAlign="Left" Width="450px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="450px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="QTY" HeaderText="QTY">
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ORDERNO" HeaderText="ORDERNO">
                                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                ExtraQty</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TxtExtraQty" runat="server" Width="140px" Text='<%# Bind("ExtraQty") %>'
                                                                    CssClass="textb"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div style="background-color: #edf3fe; text-align: center; height: 43px; width: 100%">
                    <span class="labelcopyr">&copy; Enablesoft Erp Consultancy Pvt. Ltd.All rights reserved.</span>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
