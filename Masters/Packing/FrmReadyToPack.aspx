<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmReadyToPack.aspx.cs" Inherits="Masters_Packing_FrmReadyToPack"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ready To Pack</title>
    <link rel="Stylesheet" type="text/css" href="../../App_Themes/Default/Style.css" />
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmReadyToPack.aspx";
        }
        function priview() {
            window.open('../../ReportViewer.aspx');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function validate() {
            if (document.getElementById('DDGodown').value <= "0") {
                alert("Please select Godown");
                document.getElementById('DDGodown').focus();
                return false;
            }
            else if (document.getElementById('ddlotno').value <= "") {
                alert("Please Select Lot No");
                document.getElementById('ddlotno.ClientID').focus();
                return false;
            }
            else if (document.getElementById('TxtPackQty').value == "") {
                alert("Pack Qty Cannot  Blank");
                document.getElementById('TxtPackQty.ClientID').focus();
                return false;
            }
            else {
                return confirm('Do You Want To Save?')
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" border="1">
        <tr style="width: 100%" align="center">
            <td height="66px" align="center">
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
    <table width="100%">
        <tr>
            <td height="inherit" valign="top" class="style1" colspan="2">
                <div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td colspan="4" align="right" class="tdstyle">
                                                    <asp:CheckBox ID="ChkEditOrder" runat="server" Text=" EDIT" AutoPostBack="True" OnCheckedChanged="ChkEditOrder_CheckedChanged" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdstyle">
                                                    Company Name
                                                    <br />
                                                    <asp:DropDownList ID="DDCompanyName" runat="server" Width="150px" CssClass="dropdown"
                                                        TabIndex="1">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDCompanyName"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td class="tdstyle">
                                                    Customer Code
                                                    <br />
                                                    <asp:DropDownList ID="DDCustomerCode" runat="server" Width="150px" CssClass="dropdown"
                                                        AutoPostBack="True" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged"
                                                        TabIndex="2">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDCustomerCode"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td class="tdstyle">
                                                    Order No
                                                    <br />
                                                    <asp:DropDownList ID="DDOrderNo" runat="server" Width="150px" CssClass="dropdown"
                                                        AutoPostBack="True" OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged" TabIndex="3">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDOrderNo"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td class="tdstyle">
                                                    Order Unit
                                                    <br />
                                                    <asp:DropDownList ID="DDOrderUnit" runat="server" Width="150px" CssClass="dropdown"
                                                        TabIndex="4">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDOrderUnit"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td class="tdstyle">
                                                    Ready To Pack No<br />
                                                    <asp:TextBox ID="txtpackno" runat="server" Width="75px" CssClass="textb" TabIndex="17"
                                                        ReadOnly="true"></asp:TextBox>
                                                </td>
                                                <td class="tdstyle">
                                                    Date
                                                    <br />
                                                    <asp:TextBox ID="TxtDate" runat="server" Width="90px" CssClass="textb" TabIndex="5"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                                        TargetControlID="TxtDate">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td id="TDProdCode" runat="server" visible="false" class="tdstyle">
                                                    Prod Code<br />
                                                    <asp:TextBox ID="TxtProdCode" runat="server" Width="100px" AutoPostBack="True" CssClass="textb"
                                                        OnTextChanged="TxtProdCode_TextChanged" TabIndex="6"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" EnableCaching="true"
                                                        Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtProdCode"
                                                        UseContextKey="True">
                                                    </cc1:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblCategoryName" class="tdstyle" runat="server" Text="Category Name"></asp:Label>
                                                    &nbsp;<br />
                                                    <asp:DropDownList ID="ddCategoryName" runat="server" Width="150px" CssClass="dropdown"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddCategoryName_SelectedIndexChanged"
                                                        TabIndex="7">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="ddCategoryName"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblItemName" class="tdstyle" runat="server" Text="Item Name"></asp:Label>
                                                    <br />
                                                    <asp:DropDownList ID="ddItemName" runat="server" Width="150px" CssClass="dropdown"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddItemName_SelectedIndexChanged"
                                                        TabIndex="8">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="ddItemName"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td id="TDQuality" runat="server" visible="false">
                                                    <asp:Label ID="lblQualityName" class="tdstyle" runat="server" Text="Quality"></asp:Label>
                                                    &nbsp;<br />
                                                    <asp:DropDownList ID="ddQuality" runat="server" Width="150px" TabIndex="9" CssClass="dropdown"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddQuality_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="ddQuality"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td id="TDDesign" runat="server" visible="false">
                                                    <asp:Label ID="lblDesignName" class="tdstyle" runat="server" Text="Design"></asp:Label>
                                                    <br />
                                                    <asp:DropDownList ID="ddDesign" runat="server" Width="150px" TabIndex="10" CssClass="dropdown"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddDesign_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="ddDesign"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td id="TDColor" runat="server" visible="false">
                                                    <asp:Label ID="lblColorName" class="tdstyle" runat="server" Text="Color"></asp:Label>
                                                    <br />
                                                    <asp:DropDownList ID="ddColor" runat="server" Width="100px" TabIndex="11" CssClass="dropdown"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddColor_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="ddColor"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td id="TDShape" runat="server" visible="false">
                                                    <asp:Label ID="lblShapeName" class="tdstyle" runat="server" Text="Shape"></asp:Label>
                                                    <br />
                                                    <asp:DropDownList ID="ddShape" runat="server" Width="100px" AutoPostBack="True" CssClass="dropdown"
                                                        OnSelectedIndexChanged="ddShape_SelectedIndexChanged" TabIndex="12">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="ddShape"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td id="TDSize" runat="server" visible="false">
                                                    <asp:Label ID="lblSizeName" class="tdstyle" runat="server" Text="Size"></asp:Label>
                                                    <br />
                                                    <asp:DropDownList ID="ddSize" runat="server" Width="100px" TabIndex="13" CssClass="dropdown"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddSize_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender11" runat="server" TargetControlID="ddSize"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td id="TDShade" runat="server" visible="false">
                                                    <asp:Label ID="lblShade" class="tdstyle" runat="server" Text="Shade"></asp:Label>
                                                    <br />
                                                    <asp:DropDownList ID="ddShade" runat="server" Width="100px" CssClass="dropdown" OnSelectedIndexChanged="ddShade_SelectedIndexChanged"
                                                        AutoPostBack="True" TabIndex="14">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender12" runat="server" TargetControlID="ddShade"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="TDFinishType" runat="server" visible="false">
                                                    <asp:Label ID="LblFinishType" class="tdstyle" runat="server" Text="Finish Type"></asp:Label>
                                                    <br />
                                                    <asp:DropDownList ID="DDFinishType" runat="server" Width="150px" CssClass="dropdown"
                                                        AutoPostBack="True" OnSelectedIndexChanged="DDFinishType_SelectedIndexChanged"
                                                        TabIndex="15">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender13" runat="server" TargetControlID="DDFinishType"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td class="tdstyle">
                                                    Godown Name<br />
                                                    <asp:DropDownList ID="DDGodown" runat="server" Width="150px" CssClass="dropdown"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddgodown_SelectedIndexChanged" TabIndex="16">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender14" runat="server" TargetControlID="DDGodown"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td class="tdstyle">
                                                    LOT NO<br />
                                                    <asp:DropDownList ID="ddlotno" runat="server" Width="150px" CssClass="dropdown" AutoPostBack="True"
                                                        TabIndex="16" OnSelectedIndexChanged="ddlotno_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <cc1:ListSearchExtender ID="ListSearchExtender15" runat="server" TargetControlID="ddlotno"
                                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                                    </cc1:ListSearchExtender>
                                                </td>
                                                <td class="tdstyle">
                                                    Total Qty<br />
                                                    <asp:TextBox ID="TxtTotalQty" runat="server" Width="75px" CssClass="textb" TabIndex="17"
                                                        ReadOnly="true"></asp:TextBox>
                                                </td>
                                                <td class="tdstyle">
                                                    Pre Pack Qty<br />
                                                    <asp:TextBox ID="TxtPrePackQty" runat="server" Width="75px" CssClass="textb" TabIndex="18"
                                                        ReadOnly="true"></asp:TextBox>
                                                </td>
                                                <td class="tdstyle">
                                                    Stock Qty<br />
                                                    <asp:TextBox ID="TxtStockQty" runat="server" Width="75px" CssClass="textb" TabIndex="19"
                                                        ReadOnly="true"></asp:TextBox>
                                                </td>
                                                <td class="tdstyle">
                                                    Pack Qty<br />
                                                    <asp:TextBox ID="TxtPackQty" runat="server" Width="75px" AutoPostBack="True" CssClass="textb"
                                                        OnTextChanged="TxtPackQty_TextChanged" TabIndex="20"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <asp:Label ID="LblErrorMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5" align="right">
                                                    <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" OnClientClick="return validate()"
                                                        TabIndex="21" CssClass="buttonnorm" />
                                                    &nbsp;
                                                    <asp:Button ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();"
                                                        CssClass="buttonnorm" />
                                                    &nbsp;
                                                    <asp:Button ID="BTNCLOSE" runat="server" Text="Close" OnClick="BTNCLOSE_Click" CssClass="buttonnorm" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="DGSHOWDATA" runat="server" AutoGenerateColumns="False" AllowPaging="true"
                                                        PageSize="6" OnPageIndexChanging="DGSHOWDATA_PageIndexChanging" CssClass="grid-view"
                                                        OnRowCreated="DGSHOWDATA_RowCreated" OnRowDataBound="DGSHOWDATA_RowDataBound"
                                                        OnSelectedIndexChanged="DGSHOWDATA_SelectedIndexChanged">
                                                        <PagerStyle CssClass="PagerStyle" />
                                                        <Columns>
                                                            <asp:BoundField DataField="PRODUCTCODE" HeaderText="PRODUCTCODE" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div style="width: 100%; height: 200px; overflow: scroll">
                                            <asp:GridView ID="DGOrderDetail" Width="100%" runat="server" DataKeyNames="Sr_No"
                                                AutoGenerateColumns="False" CssClass="grid-view" OnRowCreated="DGOrderDetail_RowCreated"
                                                OnSelectedIndexChanged="DGOrderDetail_SelectedIndexChanged" OnRowDataBound="DGOrderDetail_RowDataBound"
                                                OnRowDeleting="DGOrderDetail_RowDeleting">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <%--<asp:BoundField DataField="Sr_No" HeaderText="Sr_No" />--%>
                                                    <asp:BoundField DataField="CATEGORY" HeaderText="CATEGORY">
                                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ITEMNAME" HeaderText="ITEMNAME">
                                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DESCRIPTION" HeaderText="DESCRIPTION">
                                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="QTY" HeaderText="QTY">
                                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblGodownID" runat="server" Visible="true" Text='<%# Bind("GodownId") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                                Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')" CssClass="linkbutton"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
    </table>
    <table width="100%">
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
