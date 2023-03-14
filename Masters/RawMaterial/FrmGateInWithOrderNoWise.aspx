<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="FrmGateInWithOrderNoWise.aspx.cs" Inherits="Masters_RawMaterial_FrmGateInWithOrderNoWise" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function ClickNew() {
            window.location.href = "FrmGateInWithOrderNoWise.aspx";
        }
        function Validation() {
            if (document.getElementById("<%=ddCompName.ClientID %>").value <= "0") {
                alert("Pls Select Company Name");
                document.getElementById("<%=ddCompName.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ddCustomerCode.ClientID %>").value <= "0") {
                alert("Pls Select Customer Code");
                document.getElementById("<%=ddCustomerCode.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=DDCustomerOrderNo.ClientID %>").value <= "0") {
                alert("Pls Select OrderNo");
                document.getElementById("<%=DDCustomerOrderNo.ClientID %>").focus();
                return false;
            }

            if (document.getElementById("<%=Td3.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDGateInNo').value <= "0") {
                    alert("Please select gate in no....!");
                    document.getElementById("CPH_Form_DDGateInNo").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=txtdate.ClientID %>").value == "") {
                alert("Pls Select Issue Date");
                document.getElementById("<%=txtdate.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ddCatagory.ClientID %>").value <= "0") {
                alert("Pls Select Category");
                document.getElementById("<%=ddCatagory.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=dditemname.ClientID %>").value <= "0") {
                alert("Pls Select Item Name");
                document.getElementById("<%=dditemname.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ql.ClientID %>")) {
                if (document.getElementById('CPH_Form_dquality').value <= "0") {
                    alert("Please Select Quality....!");
                    document.getElementById("CPH_Form_dquality").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=dsn.ClientID %>")) {
                if (document.getElementById('CPH_Form_dddesign').value <= "0") {
                    alert("Please Select Design....!");
                    document.getElementById("CPH_Form_dddesign").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=clr.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddcolor').value <= "0") {
                    alert("Please Select Colour....!");
                    document.getElementById("CPH_Form_ddcolor").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=shp.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddshape').value <= "0") {
                    alert("Please Select Shape....!");
                    document.getElementById("CPH_Form_ddshape").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=sz.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddsize').value <= "0") {
                    alert("Please Select Size....!");
                    document.getElementById("CPH_Form_ddsize").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=shd.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddlshade').value <= "0") {
                    alert("Please Select Shade Colour....!");
                    document.getElementById("CPH_Form_ddlshade").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TDGodown.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddgodown').value <= "0") {
                    alert("Please Select Godown....!");
                    document.getElementById("CPH_Form_ddgodown").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TDUnit.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDunit').value <= "0") {
                    alert("Please Select Unit....!");
                    document.getElementById("CPH_Form_DDunit").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TxtQty.ClientID %>").value == "") {
                alert("Pls fill rec qty");
                document.getElementById("<%=TxtQty.ClientID %>").focus();
                return false;
            }
            else {
                return confirm('Do you want to save data?')
            }
        }
        function isNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode != 45 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }

            else {
                return true;
            }
        }
        function checkqty() {
            var IssQty = 0;
            var RecQty = 0;
            if (document.getElementById("<%=TxtIssueQty.ClientID %>")) {
                IssQty = document.getElementById("<%=TxtIssueQty.ClientID %>").value
                RecQty = document.getElementById("<%=TxtQty.ClientID %>").value
                if (parseFloat(RecQty) > parseFloat(IssQty)) {
                    alert("Rec qty is greater than issue qty")
                    document.getElementById("<%=TxtQty.ClientID %>").value = "";
                    return false;
                }
                else {
                    return true;
                }
            }
            else {
                return true;
            }
        }
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div>
                <table>
                    <tr id="Tr1" runat="server">
                        <td id="Td1" colspan="2" class="tdstyle">
                            <span class="labelbold">Company Name</span>
                            <br />
                            <asp:DropDownList ID="ddCompName" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label33" runat="server" Text="BranchName" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDBranchName" Width="150" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td id="Td4" class="tdstyle">
                            <span class="labelbold">Customer Code</span> &nbsp;
                            <asp:CheckBox ID="ChKForEdit" runat="server" Text=" For Edit" CssClass="checkboxbold"
                                OnCheckedChanged="ChKForEdit_CheckedChanged" AutoPostBack="true" />
                            <br />
                            <asp:DropDownList ID="ddCustomerCode" runat="server" Width="200px" AutoPostBack="True"
                                CssClass="dropdown" OnSelectedIndexChanged="ddCustomerCode_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label1" runat="server" Text="OrderNo" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDCustomerOrderNo" Width="150" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCustomerOrderNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="Td3" runat="server" visible="false">
                            <span class="labelbold">Gate In No</span>
                            <br />
                            <asp:DropDownList ID="DDGateInNo" runat="server" Width="100px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDGateInNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="Td5" class="tdstyle">
                            <span class="labelbold">Rec Date</span>
                            <br />
                            <asp:TextBox ID="txtdate" runat="server" Width="100px" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtdate">
                            </asp:CalendarExtender>
                        </td>
                        <td id="Td6" class="tdstyle">
                            <span class="labelbold">Gate In No </span>
                            <br />
                            <asp:TextBox ID="TxtGateInNo" Width="100px" runat="server" CssClass="textb" ReadOnly="true"
                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td id="Td7" class="tdstyle">
                            <span class="labelbold">Challan No. </span>
                            <br />
                            <asp:TextBox ID="txtchallanNo" Width="100px" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td id="procode" runat="server" visible="false" class="tdstyle">
                            <span class="labelbold">Product Code</span>
                            <br />
                            <asp:TextBox ID="TxtProdCode" runat="server" OnTextChanged="TxtProdCode_TextChanged"
                                AutoPostBack="True" Width="100px" CssClass="textbox "></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                                Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtProdCode"
                                UseContextKey="True">
                            </cc1:AutoCompleteExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblcategorytype" runat="server" Text="Catagory Type" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddlcatagorytype" runat="server" Width="150px" OnSelectedIndexChanged="ddlcatagorytype_SelectedIndexChanged"
                                AutoPostBack="True" CssClass="dropdown">
                                <asp:ListItem Value="1">Raw Material</asp:ListItem>
                                <asp:ListItem Value="0">Finished Item</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr id="Tr3" runat="server" class="tdstyle">
                        <td>
                            <asp:Label ID="lblcategoryname" runat="server" Text="Catagory Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddCatagory" runat="server" Width="150px" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged"
                                AutoPostBack="True" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblitemname" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="dditemname" runat="server" Width="150px" OnSelectedIndexChanged="dditemname_SelectedIndexChanged"
                                AutoPostBack="True" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="ql" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lblqualityname" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="dquality" runat="server" Width="150px" AutoPostBack="True"
                                CssClass="dropdown" OnSelectedIndexChanged="dquality_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="dsn" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lbldesignname" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="dddesign" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="clr" runat="server" visible="false">
                            <asp:Label ID="lblcolorname" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddcolor" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="shp" runat="server" visible="false">
                            <asp:Label ID="lblshapename" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddshape" runat="server" Width="100px" AutoPostBack="True" CssClass="dropdown"
                                OnSelectedIndexChanged="ddshape_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="sz" runat="server" visible="false">
                            <asp:Label ID="lblSize" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                            <asp:CheckBox ID="ChkForMtr" runat="server" AutoPostBack="True" OnCheckedChanged="ChkForMtr_CheckedChanged"
                                Text="For Mtr" CssClass="checkboxbold" />
                            <br />
                            <asp:DropDownList ID="ddsize" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td id="shd" runat="server" visible="false">
                            <asp:Label ID="lblshadecolor" runat="server" Text="ShadeColor" CssClass="labelbold"></asp:Label>
                            &nbsp;<br />
                            <asp:DropDownList ID="ddlshade" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="TDGodown" runat="server" class="tdstyle">
                            <span class="labelbold">Godown Name</span>
                            <br />
                            <asp:DropDownList ID="ddgodown" runat="server" Width="150px" CssClass="dropdown"
                                OnSelectedIndexChanged="ddgodown_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td id="TDBinNo" runat="server" class="tdstyle" visible="false">
                            <span class="labelbold">Bin No.</span>
                            <br />
                            <asp:DropDownList ID="DDBinNo" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="TDUnit" runat="server" class="tdstyle">
                            <span class="labelbold">Unit</span>
                            <br />
                            <asp:DropDownList ID="DDunit" runat="server" Width="80px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="TDLotNo" runat="server" class="tdstyle">
                            <span class="labelbold">LotNo.</span>
                            <br />
                            <asp:TextBox ID="TxtLotNo" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                        </td>
                        <td id="TDTagNo" runat="server" visible="false">
                            <span class="labelbold">TagNo.</span>
                            <br />
                            <asp:TextBox ID="txtTagno" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                        </td>
                        <td id="TDIssueQty" runat="server" class="tdstyle" visible="false">
                            <span class="labelbold">Iss Qty</span>
                            <br />
                            <asp:TextBox ID="TxtIssueQty" runat="server" Enabled="false" CssClass="textb" Width="70px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label17" runat="server" Text="Cone Type" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDConeType" CssClass="dropdown" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label16" runat="server" Text="No. of Cone" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtNoofCone" runat="server" Width="60px" CssClass="textb"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label19" runat="server" Text="Bell Wt" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtBellWeight" runat="server" Width="60px" CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <span class="labelbold">Qty</span>
                            <br />
                            <asp:TextBox ID="TxtQty" runat="server" CssClass="textb" onkeypress="return isNumber(event);"
                                onchange="return checkqty();" BackColor="Beige" Width="70px"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">Rate</span>
                            <br />
                            <asp:TextBox ID="txtRate" runat="server" CssClass="textb" onkeypress="return isNumber(event);"
                                BackColor="Beige" Width="70px"></asp:TextBox>
                        </td>
                        <td class="tdstyle" id="TDtxtremarks" runat="server" colspan="3">
                            <span class="labelbold">Remarks</span>
                            <br />
                            <asp:TextBox ID="txtremarks" runat="server" Width="250px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="right">
                            <asp:Label ID="LblErrorMessage" runat="server" Text="Label" ForeColor="Red" Visible="false"></asp:Label>
                            <asp:Button ID="btnnew" runat="server" Text="New" OnClientClick="return ClickNew()"
                                CssClass="buttonnorm" Width="70px" />
                            <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" OnClientClick="return Validation();"
                                CssClass="buttonnorm" Width="70px" />
                            <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click"
                                Width="80px" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm(); "
                                CssClass="buttonnorm" Width="70px" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="6" valign="top">
                            <div style="width: 700px; height: 300px; overflow: auto;">
                                <asp:GridView ID="gvdetail" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvdetail_RowDataBound"
                                    DataKeyNames="GateInDetailID" CssClass="grid-views" OnRowDeleting="gvdetail_RowDeleting">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <Columns>
                                        <asp:BoundField DataField="description" HeaderText="Description">
                                            <HeaderStyle HorizontalAlign="Center" Width="350px" />
                                            <ItemStyle HorizontalAlign="Center" Width="350px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="GodownName" HeaderText="Godown Name">
                                            <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="lotno" HeaderText="LotNo">
                                            <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TagNo" HeaderText="TagNo">
                                            <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="qty" HeaderText="Iss Qty">
                                            <HeaderStyle HorizontalAlign="Center" Width="70px" />
                                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Rate" HeaderText="Rate">
                                            <HeaderStyle HorizontalAlign="Center" Width="70px" />
                                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Customer OrderNo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerOrderNo" Text='<%#Bind("CustomerOrderNo") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="Del" OnClientClick="return confirm('Do You Want To Deleted Data ?')"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
