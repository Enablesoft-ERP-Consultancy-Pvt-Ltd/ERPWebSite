<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PackingAndOtherCost.aspx.cs"
    EnableEventValidation="false" Inherits="Masters_Carpet_PackingAndOtherCost" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="Stylesheet" type="text/css" href="../../App_Themes/Default/Style.css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function report() {
            window.open('../../ReportViewer.aspx', '');
        }
        function CalculateInner() {
            var varcode = document.getElementById('TxtProdCode').value;
            window.open('PackingCost.aspx?itemcode=' + varcode + '&PackingType=1', '', 'width=750px,Height=400px');
        }
        function CalculateMiddle() {
            var varcode = document.getElementById('TxtProdCode').value;
            window.open('PackingCost.aspx?itemcode=' + varcode + '&PackingType=2', '', 'width=750px,Height=400px');
        }
        function CalculateMaster() {
            var varcode = document.getElementById('TxtProdCode').value;
            window.open('PackingCost.aspx?itemcode=' + varcode + '&PackingType=3', '', 'width=750px,Height=400px');
        }
        function CalculateOther() {
            var varcode = document.getElementById('TxtProdCode').value;
            window.open('PackingCost.aspx?itemcode=' + varcode + '&PackingType=4', '', 'width=750px,Height=400px');
        }
        function CalculateContainer() {
            var varcode = document.getElementById('TxtProdCode').value;
            window.open('ContainerCost.aspx?itemcode=' + varcode + '&PackingType=5', '', 'width=750px,Height=400px');
        }
        function formclose() {
            window.close();
        }
        function validate() {
            var doc, msg;
            doc = document.forms[0];
            msg = "";
            if (doc.ddcomapnyname.value == "0")
            { msg = "Select Company Name "; }
            else if (doc.ddcustomercode.value == "0") {
                msg = "select customer Code";
            }
            else if (doc.ddCategoryName.value == "0") {
                msg = "select CategoryName";
            }
            else if (doc.ddItemName.value == "0") {
                msg = "select Item Name";
            }
            else if (doc.ddchangename.value == "0") {
                msg = "select Charge Name";
            }
            else if (doc.txtpercentage.value == "0") {
                msg = "select Percentage";
            }
            if (msg == "")
            { return true; }
            else {
                alert(msg);
                return false;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <table width="80%">
            <tr>
                <td class="tdstyle">
                    Item Code
                    <br />
                    <asp:TextBox ID="TxtProdCode" runat="server" CssClass="textb" Width="130px" AutoPostBack="True"
                        OnTextChanged="TxtProdCode_TextChanged"></asp:TextBox>
                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender" runat="server" EnableCaching="true"
                        Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtProdCode"
                        UseContextKey="True">
                    </cc1:AutoCompleteExtender>
                </td>
                <td class="tdstyle">
                    <asp:Label ID="lblcategoryname" runat="server" Text="Category Name"></asp:Label>
                    &nbsp;<br />
                    <asp:DropDownList ID="ddCategoryName" runat="server" OnSelectedIndexChanged="ddcategoryname_SelectedIndexChanged"
                        Width="150px" AutoPostBack="True" CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddCategoryName"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td class="tdstyle">
                    <asp:Label ID="lblitemname" runat="server" Text="Item Name"></asp:Label>
                    <br />
                    <asp:DropDownList ID="ddItemName" runat="server" OnSelectedIndexChanged="dditemname_SelectedIndexChanged"
                        Width="150px" AutoPostBack="True" CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddItemName"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="Quality" runat="server" visible="false" class="tdstyle">
                    <asp:Label ID="lblqualityname" runat="server" Text="Quality "></asp:Label>
                    <br />
                    <asp:DropDownList ID="ddQuality" runat="server" Width="130px" TabIndex="12" AutoPostBack="True"
                        CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddQuality"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td>
                    <asp:Button ID="btnInnerPacking" runat="server" BackColor="White" ForeColor="White"
                        Height="1px" Width="1px" BorderColor="White" BorderWidth="0px" EnableTheming="true" />
                    <asp:Button ID="btnMiddlePacking" runat="server" BackColor="White" ForeColor="White"
                        Height="1px" Width="1px" BorderColor="White" BorderWidth="0px" EnableTheming="true" />
                    <asp:Button ID="btnMasterPackingCost" runat="server" BackColor="White" ForeColor="White"
                        Height="1px" Width="1px" BorderColor="White" BorderWidth="0px" EnableTheming="true" />
                    <asp:Button ID="btnOtherPackingCost" runat="server" BackColor="White" ForeColor="White"
                        Height="1px" Width="1px" BorderColor="White" BorderWidth="0px" EnableTheming="true" />
                    <asp:Button ID="btnContainerPackingCost" runat="server" BackColor="White" ForeColor="White"
                        Height="1px" Width="1px" BorderColor="White" BorderWidth="0px" EnableTheming="true" />
                </td>
            </tr>
            <tr>
                <td id="Design" runat="server" visible="false" class="tdstyle">
                    <asp:Label ID="lbldesignname" runat="server" Text="Design"></asp:Label>
                    <br />
                    <asp:DropDownList ID="ddDesign" runat="server" Width="130px" TabIndex="13" AutoPostBack="True"
                        CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="ddDesign"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="Color" runat="server" visible="false" class="tdstyle">
                    <asp:Label ID="lblcolorname" runat="server" Text="Color"></asp:Label>
                    <br />
                    <asp:DropDownList ID="ddColor" runat="server" Width="100px" TabIndex="14" AutoPostBack="True"
                        CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="ddColor"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="Shape" runat="server" visible="false" class="tdstyle">
                    <asp:Label ID="lblshapename" runat="server" Text="Shape"></asp:Label>
                    <br />
                    <asp:DropDownList ID="ddShape" runat="server" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddshape_SelectedIndexChanged"
                        TabIndex="15" CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="ddShape"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="Size" runat="server" visible="false" class="tdstyle">
                    <asp:Label ID="lblsizename" runat="server" Text="Size"></asp:Label>
                    <br />
                    <asp:DropDownList ID="ddSize" runat="server" Width="100px" TabIndex="16" AutoPostBack="True"
                        CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="ddSize"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="Shade" runat="server" visible="false" class="tdstyle">
                    <asp:Label ID="lblshadename" runat="server" Text="Shade"></asp:Label>
                    <br />
                    <asp:DropDownList ID="ddShade" runat="server" Width="100px" CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="ddShade"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td class="tdstyle">
                    CalType<br />
                    <asp:DropDownList ID="ddCalType" runat="server" Width="100px" CssClass="dropdown">
                        <asp:ListItem>PCS WISE</asp:ListItem>
                        <asp:ListItem>AREA WISE</asp:ListItem>
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="ddCalType"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
            </tr>
            <tr>
                <td class="tdstyle">
                    Inner Packing Cost
                    <asp:Button ID="btnInnerpackingMatCost" OnClientClick="return CalculateInner();"
                        runat="server" Text="...." Height="19px" Width="30px" OnClick="btnInnerpackingMatCost_Click"
                        CssClass="buttonsmall" />
                    <br />
                    <asp:TextBox ID="txtInnerPackingCost" runat="server"></asp:TextBox>
                </td>
                <td class="tdstyle">
                    Middle Packing Cost
                    <asp:Button ID="btnMiddlepackingMatCost" OnClientClick="return CalculateMiddle();"
                        runat="server" Text="...." Height="19px" Width="30px" OnClick="btnMiddlepackingMatCost_Click"
                        CssClass="buttonsmall" />
                    <br />
                    <asp:TextBox ID="txtMiddlePackingCost" runat="server"></asp:TextBox>
                </td>
                <td class="tdstyle">
                    Master Packing Cost
                    <asp:Button ID="btnMasterpackingMatCost" OnClientClick="return CalculateMaster();"
                        runat="server" Text="...." Height="19px" Width="30px" OnClick="btnMasterpackingMatCost_Click"
                        CssClass="buttonsmall" />
                    <br />
                    <asp:TextBox ID="txtMasterPackingCost" runat="server"></asp:TextBox>
                </td>
                <td class="tdstyle">
                    PKG Charge
                    <asp:Button ID="btnOtherpackingMatCost" OnClientClick="return CalculateOther();"
                        runat="server" Text="...." Height="19px" Width="30px" OnClick="btnOtherpackingMatCost_Click"
                        CssClass="buttonsmall" />
                    <br />
                    <asp:TextBox ID="txtOtherPackingCost" runat="server"></asp:TextBox>
                </td>
                <td class="tdstyle">
                    FRT Amt
                    <asp:Button ID="btnContainerpackingMatCost" OnClientClick="return CalculateContainer();"
                        runat="server" Text="...." Height="19px" Width="30px" OnClick="btnContainerpackingMatCost_Click"
                        CssClass="buttonsmall" />
                    <br />
                    <asp:TextBox ID="txtContainerPackingCost" runat="server"></asp:TextBox>
                </td>
                <td>
                    <br />
                    <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClientClick="return confirm('Do you want to save data?')"
                        OnClick="btnsave_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
                    <asp:GridView ID="DG" runat="server" DataKeyNames="Sr_No" OnSelectedIndexChanged="DG_SelectedIndexChanged"
                        OnRowDataBound="DG_RowDataBound" AutoGenerateColumns="False" CssClass="grid-view"
                        OnRowCreated="DG_RowCreated">
                        <Columns>
                            <asp:BoundField DataField="Sr_No" HeaderText="Sr_No">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="INNERAMT" HeaderText="INNERAMT" />
                            <asp:BoundField DataField="MIDDLEAMT" HeaderText="MIDDLEAMT" />
                            <asp:BoundField DataField="MASTERAMT" HeaderText="MASTERAMT" />
                            <asp:BoundField DataField="PKG" HeaderText="PKG" />
                            <asp:BoundField DataField="FRT" HeaderText="FRT" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="3" align="left">
                    <asp:Button ID="BtnNew" runat="server" Text="New" CssClass="buttonnorm" OnClick="BtnNew_Click" />
                    <asp:Button ID="bntpriview" runat="server" Text="Preview" CssClass="buttonnorm" OnClientClick="return report()" />
                    <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return formclose()" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
