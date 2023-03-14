<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmPurchaseReturn.aspx.cs"
    EnableEventValidation="true" Inherits="Masters_Purchase_frmPurchaseReturn" MasterPageFile="~/ERPmaster.master"
    Title="PURCHASE RETURN" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
        function NewForm() {
            window.location.href = "frmPurchaseReturn.aspx"
        }

        function isNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('Please Enter numeric value only');
                return false;
            }
            else {
                return true;
            }
        }
        function ValidationSave(e, txt) {

            if (document.getElementById('CPH_Form_DDCompany') != null) {
                if (document.getElementById('CPH_Form_DDCompany').options.length == 0) {
                    alert("Company name must have a value....!");
                    document.getElementById("CPH_Form_DDCompany").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDCompany').options[document.getElementById('CPH_Form_DDCompany').selectedIndex].value == 0) {
                    alert("Please select company name ....!");
                    document.getElementById("CPH_Form_DDCompany").focus();
                    return false;
                }
            }

            if (document.getElementById('CPH_Form_DDPartyName') != null) {
                if (document.getElementById('CPH_Form_DDPartyName').options.length == 0) {
                    alert("Party name must have a value....!");
                    document.getElementById("CPH_Form_DDPartyName").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDPartyName').options[document.getElementById('CPH_Form_DDPartyName').selectedIndex].value == 0) {
                    alert("Please select Party name ....!");
                    document.getElementById("CPH_Form_DDPartyName").focus();
                    return false;
                }
            }

            if (document.getElementById('CPH_Form_DDChallanNo') != null) {
                if (document.getElementById('CPH_Form_DDChallanNo').options.length == 0) {
                    alert("ChallanNo name must have a value....!");
                    document.getElementById("CPH_Form_DDChallanNo").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDChallanNo').options[document.getElementById('CPH_Form_DDChallanNo').selectedIndex].value == 0) {
                    alert("Please select Challan No ....!");
                    document.getElementById("CPH_Form_DDChallanNo").focus();
                    return false;
                }
                var gvcheck = document.getElementById('CPH_Form_DGItemDetail');
                var rowindex = txt.offsetParent.parentNode.rowIndex;
                var inputs = gvcheck.rows[rowindex].cells[7].children[0].value;
                if (inputs == "0") {
                    alert("Return Quantity can not be zero !");
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_DDGatePass') != null) {
                if (document.getElementById('CPH_Form_DDGatePass').options.length == 0) {
                    alert("Gate Pass No. must have a value....!");
                    document.getElementById("CPH_Form_DDGatePass").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDGatePass').options[document.getElementById('CPH_Form_DDGatePass').selectedIndex].value == 0) {
                    alert("Please select Gate Pass No. ....!");
                    document.getElementById("CPH_Form_DDGatePass").focus();
                    return false;
                }
            }
            return confirm('Do You Want To Save?')
        }
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div id="main" runat="server" style="height: 400px; width: 800px">
                <table>
                    <tr>
                        <td>
                            <asp:CheckBox ID="CHKEdit" runat="server" Text="     Edit" CssClass="checkboxbold"
                                AutoPostBack="true" OnCheckedChanged="CHKEdit_CheckedChanged" />
                        </td>
                        <%--<td id="TDcomplete" runat="server" visible="false">
                                <asp:CheckBox ID="chkcomplete" runat="server" Text="For Complete" CssClass="checkboxbold" 
                                AutoPostBack="true" OnCheckedChanged="chkcomplete_CheckedChanged" />
                            </td>--%>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <span class="labelbold">CompanyName</span>
                            <br />
                            <asp:DropDownList ID="DDCompany" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDCompany"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">PartyName</span>
                            <br />
                            <asp:DropDownList ID="DDPartyName" runat="server" Width="200px" CssClass="dropdown"
                                OnSelectedIndexChanged="DDPartyName_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDPartyName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">ChallanNo</span>
                            <br />
                            <asp:DropDownList ID="DDChallanNo" runat="server" Width="100px" CssClass="dropdown"
                                OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle" runat="server" id="tdGatepassDD" visible="false">
                            <span class="labelbold">GatePassNo</span>
                            <br />
                            <asp:DropDownList ID="DDGatePass" runat="server" Width="100px" CssClass="dropdown"
                                AutoPostBack="true" OnSelectedIndexChanged="DDGatePass_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">GatePassNo</span>
                            <br />
                            <asp:TextBox ID="TxtGatepassno" runat="server" CssClass="textb" Width="75px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">Date</span>
                            <br />
                            <asp:TextBox ID="TxtDate" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                            <asp:CalendarExtender ID="extender1" runat="server" TargetControlID="TxtDate" Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="8">
                            <div style="max-height: 250px; overflow: auto; width: 900">
                                <asp:GridView ID="DGItemDetail" runat="server" Width="100%" AutoGenerateColumns="False"
                                    DataKeyNames="DetailID" CssClass="grid-view" OnRowCommand="DGItemDetail_RowCommand"
                                    OnRowDataBound="DGItemDetail_RowDataBound">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <asp:BoundField DataField="Item_Name" HeaderText="Item">
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ItemDescription" HeaderText="Description" ControlStyle-Width="120%">
                                            <ControlStyle Width="120%"></ControlStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="GodownName" HeaderText="GodownName" ControlStyle-Width="120%">
                                            <ControlStyle Width="120%"></ControlStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LotNo" HeaderText="LotNo" ControlStyle-Width="120%">
                                            <ControlStyle Width="120%"></ControlStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Qty" HeaderText="RecQty" ControlStyle-Width="120%">
                                            <ControlStyle Width="120%"></ControlStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BalQty" HeaderText="Bal.Qty" ControlStyle-Width="120%">
                                            <ControlStyle Width="120%"></ControlStyle>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Rate">
                                            <ItemTemplate>
                                                <asp:TextBox ID="Txtrate" runat="server" Text='<%# Bind("Rate") %>' Width="60px"></asp:TextBox></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ReturnQty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtReturnQty" runat="server" Text='<%# Bind("ReturnQty") %>' Width="60px"
                                                    onkeypress="return isNumber(event);"></asp:TextBox>
                                                <asp:Label ID="lblPurchaseReceiveId" runat="server" Text='<%# Bind("PurchaseReceiveId") %>'
                                                    Width="70px" Visible="false"></asp:Label>
                                                <asp:Label ID="lblPurchaseReceiveDetailId" runat="server" Text='<%# Bind("PurchaseReceiveDetailId") %>'
                                                    Width="70px" Visible="false"></asp:Label>
                                                <asp:Label ID="lblFinishedid" runat="server" Text='<%# Bind("Finishedid") %>' Width="70px"
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="lblGodownid" runat="server" Text='<%# Bind("Godownid") %>' Width="70px"
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="lblPartyId" runat="server" Text='<%# Bind("PartyId") %>' Width="70px"
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="lblunitid" runat="server" Text='<%# Bind("Unitid") %>' Width="70px"
                                                    Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remark">
                                            <ItemTemplate>
                                                <asp:TextBox ID="Txtremark" runat="server" Text='<%# Bind("Remark") %>' Width="200px"></asp:TextBox></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:Button ID="btnSave" CssClass="buttonnorm" Text="Save" runat="server" CommandName="save"
                                                    OnClientClick="return ValidationSave(event,this);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblmsg" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="6">
                            <asp:Button ID="BtnNew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                            <asp:Button ID="BtnPreview" runat="server" Text="Return Gatepass" CssClass="buttonnorm"
                                OnClick="BtnPreview_Click" />
                            <asp:Button ID="BtnClose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hncomp" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
