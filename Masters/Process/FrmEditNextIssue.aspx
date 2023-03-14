<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" ViewStateMode="Enabled"
    CodeFile="FrmEditNextIssue.aspx.cs" Inherits="Masters_Process_FrmEditNextIssue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script language="javascript" type="text/javascript">
        function NewForm() {
            window.location.href = "FrmEditNextIssue.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function KeyDownHandler(btn) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                window.document.getElementById(btn).click();
            }
        }
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function validate() {
            if (document.getElementById('DDCompanyName').value <= "0") {
                alert("Pls Select Company Name");
                document.getElementById("DDCompanyName").focus();
                return false;
            }
            if (document.getElementById("DDTOProcess").value <= "0") {
                alert("Pls Select Vendor Name");
                document.getElementById("DDTOProcess").focus();
                return false;
            }
            if (document.getElementById("DDContractor").value <= "0") {
                alert("Pls Select Order No.");
                document.getElementById("DDContractor").focus();
                return false;
            }
            if (document.getElementById("DDChallanNo").value <= "0") {
                alert("Pls Select Challan No.");
                document.getElementById("DDChallanNo").focus();
                return false;
            }
            else {
                return confirm('Do You Want To Change Consumption?')
            }
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            else {
                return true;
            }
        }
        function CheckAll(objref) {
            var gridview = objref.parentNode.parentNode.parentNode;
            var inputlist = gridview.getElementsByTagName("input");
            for (var i = 0; i < inputlist.length; i++) {
                var row = inputlist[i].parentNode.parentNode;
                if (inputlist[i].type == "checkbox" && objref != inputlist[i]) {
                    if (objref.checked) {
                        if (inputlist[i].disabled) {
                        }
                        else {
                            inputlist[i].checked = true;
                        }

                    }
                    else {
                        inputlist[i].checked = false;


                    }
                }
            }

        }
    </script>
    <title></title>
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
                        <asp:Label ID="LblCompanyName" runat="server" Text=""></asp:Label></font></i></em>
                    </strong></span>
                <br />
                <i><font size="2" face="GEORGIA">
                    <asp:Label ID="LblUserName" ForeColor="White" runat="server" Text=""></asp:Label></font>
                </i>
            </td>
        </tr>
        <tr bgcolor="#999999">
            <td class="style1">
                <uc1:ucmenu ID="ucmenu1" runat="server" />
                <asp:ScriptManager ID="ScriptManager2" runat="server">
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
    <asp:UpdatePanel ID="update" runat="server">
        <ContentTemplate>
            <div>
                <table width="80%">
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="LBL" Text=" Challan No" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label1" Text=" CompanyName" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:Label ID="Label2" Text=" ToProcess" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label3" Text=" Emp/Contractor" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label4" Text="  Challan No" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label5" Text=" FromProcess" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label6" Text=" IssueDate" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label7" Text="  ReqDate" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label8" Text=" CalType" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label9" Text="  Unit" runat="server" CssClass="labelbold" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox CssClass="textb" ID="TxtChallanNO" runat="server" Width="90px" OnTextChanged="TxtChallanNO_TextChanged"
                                AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDTOProcess" runat="server" Width="150px"
                                AutoPostBack="True" OnSelectedIndexChanged="DDTOProcess_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDContractor" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDContractor_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDChallanNo" Width="130px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDFromProcess" runat="server" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="TxtIssueDate" runat="server" Width="90px"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtIssueDate">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="TxtReqDate" runat="server" Width="90px"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtReqDate">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDcaltype" runat="server" Width="90px"
                                Enabled="false" OnSelectedIndexChanged="DDcaltype_SelectedIndexChanged" AutoPostBack="True">
                                <asp:ListItem Value="0">Area Wise</asp:ListItem>
                                <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                <asp:ListItem Value="2">W-2 X L-2</asp:ListItem>
                                <asp:ListItem Value="3">W-2</asp:ListItem>
                                <asp:ListItem Value="4">L-2</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDUnit" runat="server" Width="90px" AutoPostBack="True"
                                Enabled="false" OnSelectedIndexChanged="DDUnit_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td id="ProCod1" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="Label10" Text=" Prod Code" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblCategory" runat="server" Text="Category" CssClass="labelbold"></asp:Label>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblItemName" runat="server" Text="ItemName" CssClass="labelbold"></asp:Label>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblQuality" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                        </td>
                        <td id="tddesign1" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblDesign" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                        </td>
                        <td id="tdColor1" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblColor" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                        </td>
                        <td id="tdShape1" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblShape" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                        </td>
                        <td id="tdsize1" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblSize" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td id="ProCod2" runat="server" visible="false">
                            <asp:TextBox CssClass="textb" ID="TxtItemCode" runat="server" Width="100px"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                                Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtItemCode"
                                UseContextKey="True">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDCategory" runat="server" Width="150px"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDItemName" runat="server" Width="150px"
                                AutoPostBack="True" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDQuality" runat="server" Width="150px"
                                AutoPostBack="True" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="tddesign" runat="server" visible="false">
                            <asp:DropDownList CssClass="dropdown" ID="DDDesign" runat="server" Width="150px"
                                AutoPostBack="True" OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="tdcolor" runat="server" visible="false">
                            <asp:DropDownList CssClass="dropdown" ID="DDColor" runat="server" Width="150px" AutoPostBack="True"
                                OnSelectedIndexChanged="DDColor_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="tdshape" runat="server" visible="false">
                            <asp:DropDownList CssClass="dropdown" ID="DDShape" runat="server" Width="150px" AutoPostBack="True"
                                OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="tdsize" runat="server" visible="false">
                            <asp:DropDownList CssClass="dropdown" Width="150px" ID="DDSize" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="DDSize_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td id="TDsrno" runat="server" visible="false">
                            <asp:Label ID="lblsrno" Text="Sr No." CssClass="labelbold" runat="server" />
                            <br />
                            <asp:DropDownList ID="DDsrno" CssClass="dropdown" Width="150px" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <br />
                            <asp:LinkButton ID="lnkgetitemdetail" Text="Get available Item Details" CssClass="labelbold"
                                ForeColor="DarkBlue" runat="server" OnClick="lnkgetitemdetail_Click"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td id="TDRateNew" runat="server" visible="false" class="tdstyle" colspan="2">
                            <asp:Label ID="Label11" Text="  Rate" runat="server" CssClass="labelbold" />
                            &nbsp;<asp:CheckBox ID="ChkForRate" runat="server" Text="Chk For Rate" CssClass="checkboxbold" />
                            <br />
                            <asp:TextBox CssClass="textb" ID="TxtRateNew" onkeypress="return isNumberKey(event);"
                                runat="server" Width="100px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td id="TDAreaNew" runat="server" visible="false" class="tdstyle" colspan="2">
                            <asp:Label ID="Label12" Text=" Area" runat="server" CssClass="labelbold" />
                            &nbsp;
                            <asp:CheckBox ID="ChkForArea" runat="server" Text="Chk For Area" CssClass="checkboxbold" />
                            <br />
                            <asp:TextBox CssClass="textb" ID="TxtAreaNew" onkeypress="return isNumberKey(event);"
                                runat="server" Width="100px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td colspan="4">
                            <asp:Label ID="LblErrorMessage" runat="server" Text="" Font-Bold="false" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%">
                    <tr>
                        <td>
                            <div style="width: 100%">
                                <div style="width: 80%; float: left">
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <div style="max-height: 300px; overflow: auto;">
                                                    <asp:GridView ID="DGItemDetail" runat="server" OnRowDataBound="DGItemDetail_RowDataBound"
                                                        OnRowCommand="DGItemDetail_RowCommand" AutoGenerateColumns="False" DataKeyNames="Item_Finished_Id"
                                                        CssClass="grid-views">
                                                        <HeaderStyle CssClass="gvheaders" />
                                                        <AlternatingRowStyle CssClass="gvalts" />
                                                        <RowStyle CssClass="gvrow" />
                                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Button ID="BtnStockFil" runat="server" CssClass="buttonnorm" CommandName="FillStock"
                                                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text="Show No" OnClick="BtnStockFill_Click"
                                                                        Width="75px" />
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="75px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Item_Name" HeaderText="Item">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Description" HeaderText="Description" ControlStyle-Width="100%">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Issue Qty">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="TxtissueQty" runat="server" Text='<%# Bind("QTY") %>' Width="50px"
                                                                        BackColor="Yellow"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Length">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="TxtLength" runat="server" Text='<%# Bind("Length") %>' Width="50px"
                                                                        Enabled="false"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Width">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="TxtWidth" runat="server" Text='<%# Bind("Width") %>' Width="50px"
                                                                        Enabled="false"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Area">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="TxtArea" runat="server" Text='<%# Bind("Area") %>' Width="70px"
                                                                        Enabled="false"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Rate">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="TxtRate" runat="server" Text='<%# Bind("Rate") %>' Width="50px"
                                                                        Enabled="false"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Button CssClass="buttonnorm" ID="BtnAllQty" Width="50px" runat="server" Text="All"
                                                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" OnClick="BtnAllQty_Click"
                                                                        OnClientClick="if (!confirm('Do you want to save Data?')) return; this.disabled=true;this.value = 'wait ...';"
                                                                        UseSubmitBehavior="false" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Sr No./OC No.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbllocalorderNo" Text='<%#Bind("Localorder") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Buyer Order No.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblbuyerorderno" Text='<%#Bind("customerorderno") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblitemfinishedid" Text='<%#Bind("Item_finished_id") %>' runat="server" />
                                                                    <asp:Label ID="lblorderid" Text='<%#Bind("orderid") %>' runat="server" />
                                                                    <asp:Label ID="lblrdi" Text='<%#Bind("RDI") %>' runat="server" />
                                                                    <asp:Label ID="lbltqty" Text='<%#Bind("Qty") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="width: 20%; float: right" id="divstock" runat="server">
                                    <table>
                                        <tr>
                                            <td class="tdstyle">
                                                <asp:Button ID="btnStockNo" runat="server" BackColor="White" BorderColor="White"
                                                    BorderWidth="0px" ForeColor="White" Height="0px" Width="0px" OnClick="TxtStockNo_TextChanged" />
                                                <asp:Label ID="Label13" Text="  Stock No" runat="server" CssClass="labelbold" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtStockNo" runat="server" Width="200px" CssClass="textb" TabIndex="8"
                                                    onKeyDown="KeyDownHandler('btnStockNo');" AutoPostBack="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div style="max-height: 200px; width: 100%; overflow: auto;">
                                                    <asp:GridView ID="DGStock" runat="server" AutoGenerateColumns="False" DataKeyNames="StockNo"
                                                        OnRowCommand="DGStock_RowCommand" CssClass="grid-view">
                                                        <HeaderStyle CssClass="gvheaders" />
                                                        <AlternatingRowStyle CssClass="gvalts" />
                                                        <RowStyle CssClass="gvrow" />
                                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                                        <Columns>
                                                            <asp:BoundField DataField="StockNo" HeaderText="StockNo" ItemStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="100px">
                                                                <HeaderStyle Width="100px"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TStockNo" HeaderText="CarpetNo" ItemStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="100px">
                                                                <HeaderStyle Width="100px"></HeaderStyle>
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Rate">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="TxtCarpetRate" runat="server" Text='<%# Bind("Rate") %>' Width="70px"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="80px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="">
                                                                <ItemTemplate>
                                                                    <asp:Button CssClass="buttonnorm" ID="BtnSaveCarpet" Width="70px" runat="server"
                                                                        Text="Save" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" OnClick="BtnSaveCarpet_Click" />
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="80px" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td>
                            <div style="max-height: 400px; width: 100%; overflow: auto">
                                <asp:GridView ID="DGDetail" runat="server" AutoGenerateColumns="False" DataKeyNames="Issue_Detail_Id"
                                    OnRowDeleting="DGDetail_RowDeleting" CssClass="grid-views" OnRowDataBound="DGDetail_RowDataBound">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Item">
                                            <ItemTemplate>
                                                <asp:Label ID="lblitem" Text='<%#Bind("Item") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" Text='<%#Bind("Description") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Width">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWidth" Text='<%#Bind("Width") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Length">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLength" Text='<%#Bind("Length") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Size">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSize" Text='<%#Bind("Size") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblqty" Text='<%#Bind("qty") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblrate" Text='<%#Bind("rate") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Area">
                                            <ItemTemplate>
                                                <asp:Label ID="lblarea" Text='<%#Bind("area") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblamount" Text='<%#Bind("amount") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="StockNo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblstockno" Text='<%#Bind("Stockno") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cancelqty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCancelqty" Text='<%#Bind("Cancelqty") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SrNo./Local Order No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lbllocalorder" Text='<%#Bind("localorder") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkall" Text="Delete all" runat="server" onclick="return CheckAll(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkboxitem" Text="" CssClass="checkboxbold" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False" Visible="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Isreceived" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblisreceived" Text='<%#Bind("Isreceived") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="Label14" Text="  Total Pcs" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label15" Text=" Area" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label16" Text=" Amount" runat="server" CssClass="labelbold" />
                        </td>
                        <td class="tdstyle">
                            <asp:CheckBox ID="ChkDetailPrint" runat="server" Text="For Detail Print" CssClass="checkboxbold" />
                            &nbsp;<asp:Button CssClass="buttonnorm preview_width" ID="BtnPreview" runat="server"
                                Text="Preview" OnClick="BtnPreview_Click" />
                            &nbsp;<asp:Button CssClass="buttonnorm " ID="BtnCurrentConsumptionSave" runat="server"
                                Width="150px" Text="Save Consumption" OnClick="BtnCurrentConsumptionSave_Click"
                                OnClientClick="return validate()" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close"
                                OnClick="BtnClose_Click" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="btndelete" runat="server" Text="Delete"
                                OnClick="btndelete_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox CssClass="textb" ID="TxtTotalPcs" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="TxtArea" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox CssClass="textb" ID="TxtAmount" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td id="TDChkReceiveDetail" runat="server" visible="false">
                            <asp:CheckBox ID="ChkReceiveDetail" runat="server" CssClass="checkboxbold" Text="Check For Receive Detail">
                            </asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="Label17" Text=" Remarks" runat="server" CssClass="labelbold" />
                        </td>
                        <td colspan="3" class="tdstyle">
                            <asp:TextBox CssClass="textb" ID="TxtRemarks" runat="server" Width="900px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="Label18" Text=" Instructions" runat="server" CssClass="labelbold" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox CssClass="textb" ID="TxtInsructions" Width="100%" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
