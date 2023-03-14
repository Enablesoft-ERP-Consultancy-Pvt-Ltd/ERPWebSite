<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmOrderAddOtherItems.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_Order_FrmOrderAddOtherItems"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/jscript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmOrderAddOtherItems.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Preview() {
            window.open('../../ReportViewer.aspx', '');
        }
        function GroupSelectedChange() {
            document.getElementById('CPH_Form_TxtUnit').value = "";
        }
        function CalculateAmount() {
            var VarQty = 0;
            var VarRate = 0;
            var VarArea = 0;

            if (document.getElementById('CPH_Form_TxtQty').value != "") {
                VarQty = document.getElementById('CPH_Form_TxtQty').value;
            }
            if (document.getElementById('CPH_Form_TxtRate').value != "") {
                VarRate = document.getElementById('CPH_Form_TxtRate').value;
            }
            if (document.getElementById('CPH_Form_DDCalType').options[document.getElementById('CPH_Form_DDCalType').selectedIndex].value == 0) {
                if (document.getElementById('CPH_Form_TxtArea').value != "") {
                    VarArea = document.getElementById('CPH_Form_TxtArea').value;
                }
                document.getElementById('CPH_Form_TxtAmount').value = VarQty * VarArea * VarRate;
            }
            else {
                document.getElementById('CPH_Form_TxtAmount').value = VarQty * VarRate;
            }
        }
        function ValidationSave() {
            if (document.getElementById('CPH_Form_DDCompanyName').options[document.getElementById('CPH_Form_DDCompanyName').selectedIndex].value == 0) {
                alert("Please Select Company Name....!");
                document.getElementById("CPH_Form_DDCompanyName").focus();
                return false;
            }
            else if (document.getElementById('CPH_Form_DDCustomerCode').options[document.getElementById('CPH_Form_DDCustomerCode').selectedIndex].value == 0) {
                alert("Please Select Customer Code....!");
                document.getElementById("CPH_Form_DDCustomerCode").focus();
                return false;
            }
            else if (document.getElementById('CPH_Form_DDOrderNo').options[document.getElementById('CPH_Form_DDOrderNo').selectedIndex].value == 0) {
                alert("Please Select Order No....!");
                document.getElementById("CPH_Form_DDOrderNo").focus();
                return false;
            }
            else if (document.getElementById('CPH_Form_DDCurrency').options[document.getElementById('CPH_Form_DDCurrency').selectedIndex].value == 0) {
                alert("Please Select Currency....!");
                document.getElementById("CPH_Form_DDCurrency").focus();
                return false;
            }
            else if (document.getElementById('CPH_Form_ChkForAddDescriptionWithValue').checked == false) {
                if (document.getElementById('CPH_Form_TxtDescription').value == "") {
                    alert("Pls Fill Description....!");
                    document.getElementById('CPH_Form_TxtDescription').focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_TxtUnit').value == "") {
                    alert("Pls Fill Unit....!");
                    document.getElementById('CPH_Form_TxtUnit').focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_DDCalType').options[document.getElementById('CPH_Form_DDCalType').selectedIndex].value == 0) {
                    if (document.getElementById('CPH_Form_TxtArea').value == "") {
                        alert("Pls Fill Area....!");
                        document.getElementById('CPH_Form_TxtArea').focus();
                        return false;
                    }
                }
                else if (document.getElementById('CPH_Form_TxtQty').value == "") {
                    alert("Pls Fill Qty....!");
                    document.getElementById('CPH_Form_TxtQty').focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_TxtRate').value == "") {
                    alert("Pls Fill Rate....!");
                    document.getElementById('CPH_Form_TxtRate').focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_TxtAmount').value == "") {
                    alert("Pls Fill Amount....!");
                    document.getElementById('CPH_Form_TxtAmount').focus();
                    return false;
                }
                else {
                    return confirm('Do You Want To Save?')
                }
            }
            else if (document.getElementById('CPH_Form_ChkForAddDescriptionWithValue').checked == true) {
                if (document.getElementById('CPH_Form_TxtDescriptionText').value == "") {
                    alert("Pls Fill Description Text....!");
                    document.getElementById('CPH_Form_TxtDescriptionText').focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_TxtDescriptionWithValue').value == "") {
                    alert("Pls Fill Description With Value....!");
                    document.getElementById('CPH_Form_TxtDescriptionWithValue').focus();
                    return false;
                }
                else {
                    return confirm('Do You Want To Save?')
                }
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
    </script>
    <asp:UpdatePanel ID="updatepanal" runat="server">
        <ContentTemplate>
            <table width="75%">
                <tr>
                    <td class="tdstyle">
                        Company Name<br />
                        <asp:DropDownList ID="DDCompanyName" runat="server" Width="250px" CssClass="dropdown"
                            TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="tdstyle">
                        Customer Code&nbsp;
                        <asp:CheckBox ID="ChkForAddDescriptionWithValue" Text="For Add Description With Value"
                            Width="200px" CssClass="checkboxbold" runat="server" AutoPostBack="True" OnCheckedChanged="ChkForAddDescriptionWithValue_CheckedChanged" />
                        <br />
                        <asp:DropDownList ID="DDCustomerCode" runat="server" Width="200px" CssClass="dropdown"
                            TabIndex="2" AutoPostBack="True" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="tdstyle">
                        Order No.<br />
                        <asp:DropDownList ID="DDOrderNo" runat="server" Width="150px" CssClass="dropdown"
                            TabIndex="3" AutoPostBack="True" OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="tdstyle">
                        Cal Type<br />
                        <asp:DropDownList ID="DDCalType" runat="server" Width="100px" CssClass="dropdown"
                            TabIndex="4" OnSelectedIndexChanged="DDCalType_SelectedIndexChanged" AutoPostBack="True">
                            <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                            <asp:ListItem Value="0">Area Wise</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="tdstyle">
                        Currency<br />
                        <asp:DropDownList ID="DDCurrency" runat="server" Width="110px" CssClass="dropdown"
                            TabIndex="5">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table width="75%" id="Table1" runat="server">
                <tr id="Tr1" runat="server">
                    <td class="tdstyle">
                        <asp:HiddenField ID="VarID" runat="server" />
                        Group Name<br />
                        <asp:TextBox ID="TxtGroupName" runat="server" Width="200px" onchange="return GroupSelectedChange();"
                            TabIndex="6" CssClass="textboxWihOutUpper"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        Buyer Code<br />
                        <asp:TextBox ID="TxtBuyerCode" runat="server" Width="150px" TabIndex="7" CssClass="textboxWihOutUpper"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        Our Code<br />
                        <asp:TextBox ID="TxtOurCode" runat="server" Width="150px" TabIndex="8" CssClass="textboxWihOutUpper"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        Description
                        <br />
                        <asp:TextBox ID="TxtDescription" runat="server" Width="450px" TabIndex="9" CssClass="textboxWihOutUpper"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table width="50%" id="Table2" runat="server">
                <tr>
                    <td class="tdstyle" id="TDUnit" runat="server">
                        Unit<br />
                        <asp:TextBox ID="TxtUnit" runat="server" CssClass="textboxWihOutUpper" TabIndex="10"
                            Width="150px"></asp:TextBox>
                    </td>
                    <td class="tdstyle" id="TDArea" runat="server" visible="false">
                        Area<br />
                        <asp:TextBox ID="TxtArea" runat="server" CssClass="textboxWihOutUpper" TabIndex="11"
                            Width="100px" AutoPostBack="True" onchange="return CalculateAmount();" onkeypress="return isNumberKey(event);"></asp:TextBox>
                    </td>
                    <td class="tdstyle" id="TDQty" runat="server">
                        Qty<br />
                        <asp:TextBox ID="TxtQty" runat="server" CssClass="textboxWihOutUpper" TabIndex="12"
                            Width="100px" AutoPostBack="True" onchange="return CalculateAmount();" onkeypress="return isNumberKey(event);"></asp:TextBox>
                    </td>
                    <td class="tdstyle" id="TDRate" runat="server">
                        Rate<br />
                        <asp:TextBox ID="TxtRate" runat="server" CssClass="textboxWihOutUpper" TabIndex="13"
                            Width="100px" AutoPostBack="True" onchange="return CalculateAmount();" onkeypress="return isNumberKey(event);"></asp:TextBox>
                    </td>
                    <td class="tdstyle" id="TDAmount" runat="server">
                        Amount<br />
                        <asp:TextBox ID="TxtAmount" runat="server" CssClass="textboxWihOutUpper" TabIndex="14"
                            Width="100px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table width="50%" id="Table3" runat="server">
                <tr>
                    <td class="tdstyle">
                        Add Description Text
                        <br />
                        <asp:TextBox ID="TxtDescriptionText" runat="server" Width="500px" TabIndex="15" CssClass="textboxWihOutUpper"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        Description With Value
                        <br />
                        <asp:TextBox ID="TxtDescriptionWithValue" runat="server" Width="150px" TabIndex="16"
                            CssClass="textboxWihOutUpper"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table width="75%">
                <tr>
                    <td>
                        <asp:Label ID="LblErrorMessage" runat="server" Text="" ForeColor="red" Font-Bold="true"
                            Visible="false"></asp:Label>
                    </td>
                    <td id="Td1" class="tdstyle" align="right" runat="server">
                        <asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();" />
                        &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnSave" Text="Save" runat="server" TabIndex="48"
                            OnClick="BtnSave_Click" OnClientClick="return ValidationSave();" />
                        &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnPreview" runat="server" Text="Preview"
                            OnClientClick="return Preview();" />
                        &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close"
                            OnClientClick="return CloseForm();" />
                    </td>
                </tr>
                <tr id="trgrid1" runat="server">
                    <td colspan="3">
                        <div style="width: 100%; height: 300px; overflow: scroll">
                            <asp:GridView ID="DGOrderDetail" Width="100%" runat="server" DataKeyNames="ID" AutoGenerateColumns="False"
                                OnRowDataBound="DGOrderDetail_RowDataBound" OnSelectedIndexChanged="DGOrderDetail_SelectedIndexChanged"
                                OnRowDeleting="DGOrderDetail_RowDeleting" CssClass="grid-view" OnRowCreated="DGOrderDetail_RowCreated">
                                <HeaderStyle CssClass="gvheader" />
                                <AlternatingRowStyle CssClass="gvalt" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:BoundField DataField="GroupName" HeaderText="GroupName">
                                        <HeaderStyle HorizontalAlign="Center" Width="150px" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BuyerCode" HeaderText="BuyerCode">
                                        <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="OurCode" HeaderText="OurCode">
                                        <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Description" HeaderText="Description">
                                        <HeaderStyle HorizontalAlign="Center" Width="250px" />
                                        <ItemStyle HorizontalAlign="Center" Width="250px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Unit" HeaderText="Unit">
                                        <HeaderStyle HorizontalAlign="Center" Width="75px" />
                                        <ItemStyle HorizontalAlign="Center" Width="75px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Qty" HeaderText="Qty">
                                        <HeaderStyle HorizontalAlign="Center" Width="70px" />
                                        <ItemStyle HorizontalAlign="Center" Width="70px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Rate" HeaderText="Rate">
                                        <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                        <ItemStyle HorizontalAlign="Center" Width="90px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Amount" HeaderText="Amount">
                                        <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:BoundField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                Text="Del" OnClientClick="return confirm('Do You Want To Delete ?')"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
