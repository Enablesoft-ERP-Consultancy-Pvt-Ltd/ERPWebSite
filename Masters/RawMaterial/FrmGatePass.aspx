<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="FrmGatePass.aspx.cs" Inherits="Masters_RawMaterial_FrmGatePass" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/FixFocus2.js"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function ClickNew() {
            window.location.href = "FrmGatePass.aspx";
        }
        function Validation() {
            if (document.getElementById("<%=ddCompName.ClientID %>").value <= "0") {
                alert("Pls Select Company Name");
                document.getElementById("<%=ddCompName.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=TDDept.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDDept').value <= "0") {
                    alert("Please select Dept....!");
                    document.getElementById("CPH_Form_DDDept").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=ddempname.ClientID %>").value <= "0") {
                alert("Pls Select Employee Name");
                document.getElementById("<%=ddempname.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=Td3.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDGatePassNo').value <= "0") {
                    alert("Please select gate pass no....!");
                    document.getElementById("CPH_Form_DDGatePassNo").focus();
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
            if (document.getElementById("<%=TDLotNo.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddlotno').value <= "0") {
                    alert("Please Select LotNo....!");
                    document.getElementById("CPH_Form_ddlotno").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TDTagNo.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDTagNo').value <= "0") {
                    alert("Please Select TagNo....!");
                    document.getElementById("CPH_Form_DDTagNo").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TDBinNo.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDBinNo').value <= "0") {
                    alert("Please Select Bin No....!");
                    document.getElementById("CPH_Form_DDBinNo").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TDUnitId.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDunit').value <= "0") {
                    alert("Please Select Unit....!");
                    document.getElementById("CPH_Form_DDunit").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=txtissqty.ClientID %>").value == "" || document.getElementById("<%=txtissqty.ClientID %>").value == "0") {
                alert("Quantity Cann't be blank Or Zero");
                document.getElementById("<%=txtissqty.ClientID %>").focus();
                return false;
            }
            else {
                return confirm('Do you want to save data?')
            }
        }
        function isNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }

            else {
                return true;
            }
        }
        function checkqty() {
            var issueqty = document.getElementById("<%=txtissqty.ClientID %>").value
            var stock = document.getElementById("<%=txtstock.ClientID %>").value
            if (parseFloat(issueqty) > parseFloat(stock)) {
                alert("Issue Qty is greater than Stock Qty")
                document.getElementById("<%=txtissqty.ClientID %>").value = "";
                return false;
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
                    <tr id="TRempcodescan" runat="server" visible="false">
                        <td colspan="2">
                        </td>
                        <td>
                            <asp:Label ID="Label18" Text="Enter/Scan Emp. Code" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtWeaverIdNoscan" CssClass="textb" runat="server" Width="150px"
                                Height="20px" AutoPostBack="true" OnTextChanged="txtWeaverIdNoscan_TextChanged" />
                        </td>
                    </tr>
                    <tr id="Tr1" runat="server">
                        <td id="Td1" colspan="2" class="tdstyle">
                            <asp:Label ID="Label1" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddCompName" runat="server" Width="200px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label33" runat="server" Text="BranchName" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDBranchName" Width="150" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td id="TDDept" runat="server" visible="false">
                            <asp:Label ID="Label13" runat="server" Text="Department" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDDept" runat="server" Width="150px" CssClass="dropdown" AutoPostBack="True"
                                OnSelectedIndexChanged="DDDept_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="Td4" class="tdstyle">
                            <asp:Label ID="Label2" runat="server" Text=" Party Name" CssClass="labelbold"></asp:Label>
                            &nbsp;
                            <asp:CheckBox ID="ChKForEdit" runat="server" Text=" For Edit" CssClass="checkboxbold"
                                OnCheckedChanged="ChKForEdit_CheckedChanged" AutoPostBack="true" />
                            <br />
                            <asp:DropDownList ID="ddempname" runat="server" Width="200px" AutoPostBack="True"
                                CssClass="dropdown" OnSelectedIndexChanged="ddempname_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="Td3" runat="server" visible="false">
                            <asp:Label ID="Label3" runat="server" Text="Gate Pass No" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDGatePassNo" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDGatePassNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="Td5" class="tdstyle">
                            <asp:Label ID="Label4" runat="server" Text="Issue Date" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtdate" runat="server" Width="100px" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtdate">
                            </asp:CalendarExtender>
                        </td>
                        <td id="Td6" class="tdstyle">
                            <asp:Label ID="Label5" runat="server" Text=" Issue No." CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtchalanno" Width="100px" runat="server" CssClass="textb" ReadOnly="true"
                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td id="procode" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="Label6" runat="server" Text="Product Code" CssClass="labelbold"></asp:Label>
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
                            <asp:DropDownList ID="dquality" runat="server" Width="150px" CssClass="dropdown"
                                OnSelectedIndexChanged="dquality_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td id="dsn" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="lbldesignname" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="dddesign" runat="server" Width="150px" CssClass="dropdown"
                                OnSelectedIndexChanged="dddesign_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td id="clr" runat="server" visible="false">
                            <asp:Label ID="lblcolorname" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddcolor" runat="server" Width="150px" CssClass="dropdown" OnSelectedIndexChanged="ddcolor_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td id="shp" runat="server" visible="false">
                            <asp:Label ID="lblshapename" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddshape" runat="server" Width="100px" AutoPostBack="True" CssClass="dropdown"
                                OnSelectedIndexChanged="ddshape_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="sz" runat="server" visible="false">
                            <asp:Label ID="lblSize" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                            <asp:CheckBox ID="ChkForMtr" Visible="false" runat="server" AutoPostBack="True" OnCheckedChanged="ChkForMtr_CheckedChanged"
                                Text="For Mtr" />
                            <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged">
                            </asp:DropDownList>
                            <br />
                            <asp:DropDownList ID="ddsize" runat="server" Width="150px" CssClass="dropdown" OnSelectedIndexChanged="ddsize_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td id="shd" runat="server" visible="false">
                            <asp:Label ID="lblshadecolor" runat="server" Text="ShadeColor" CssClass="labelbold"></asp:Label>
                            &nbsp;<br />
                            <asp:DropDownList ID="ddlshade" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlshade_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TDGodown" runat="server" class="tdstyle">
                            <asp:Label ID="Label7" runat="server" Text="Godown Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddgodown" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="ddgodown_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TDLotNo" runat="server" class="tdstyle">
                            <asp:Label ID="Label8" runat="server" Text="LotNo." CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddlotno" runat="server" AutoPostBack="True" Width="150px" CssClass="dropdown"
                                OnSelectedIndexChanged="ddlotno_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TDTagNo" runat="server" visible="false">
                            <asp:Label ID="Label12" runat="server" Text="TagNo." CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDTagNo" runat="server" AutoPostBack="True" Width="150px" CssClass="dropdown"
                                OnSelectedIndexChanged="DDTagNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TDBinNo" runat="server" visible="false">
                            <asp:Label ID="Label14" runat="server" Text="Bin No." CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDBinNo" runat="server" AutoPostBack="True" Width="150px" CssClass="dropdown"
                                OnSelectedIndexChanged="DDBinNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td id="TDUnitId" runat="server" class="tdstyle">
                            <asp:Label ID="lblunitid" runat="server" Text="Unit" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDunit" runat="server" AutoPostBack="True" Width="80px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="TDStock" runat="server" class="tdstyle">
                            <asp:Label ID="Label9" runat="server" Text=" Stock" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtstock" runat="server" Enabled="false" CssClass="textb" Width="100px"></asp:TextBox>
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
                        <td class="tdstyle">
                            <asp:Label ID="Label10" runat="server" Text="Iss Qty" CssClass="labelbold"></asp:Label>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtissqty"
                                ErrorMessage="please Enter qty" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:TextBox ID="txtissqty" runat="server" CssClass="textb" onkeypress="return isNumber(event);"
                                onchange="return checkqty();" BackColor="#7b96bb" Width="100px" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <td class="tdstyle" id="TDtxtremarks" runat="server" colspan="3">
                            <asp:Label ID="Label11" runat="server" Text=" Remarks" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtremarks" runat="server" Width="250px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                        <td class="tdstyle" id="TD2" runat="server" colspan="3">
                            <asp:Label ID="Label15" runat="server" Text="EWay BillNo" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtEWayBillNo" runat="server" Width="250px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table style="width: 80%;">
                    <tr>
                        <td colspan="8" align="right">
                            <asp:Label ID="LblErrorMessage" runat="server" Text="Label" ForeColor="Red" Visible="false"></asp:Label>
                            <asp:Button ID="btnnew" runat="server" Text="New" OnClientClick="return ClickNew()"
                                CssClass="buttonnorm" />
                            <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" OnClientClick="return Validation();"
                                CssClass="buttonnorm" />
                            <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm preview_width"
                                OnClick="btnPreview_Click" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm(); "
                                CssClass="buttonnorm" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="6" align="center" valign="top">
                            <div style="width: auto; height: auto; overflow: scroll;">
                                <asp:GridView ID="gvdetail" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvdetail_RowDataBound"
                                    DataKeyNames="GATEOUTDETAILID" OnRowDeleting="gvdetail_RowDeleting" OnSelectedIndexChanged="gvdetail_SelectedIndexChanged">
                                    <RowStyle CssClass="gvrow" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <HeaderStyle CssClass="gvheaders" />
                                    <Columns>
                                        <asp:BoundField DataField="description" HeaderText="Description">
                                            <HeaderStyle HorizontalAlign="Center" Width="400px" />
                                            <ItemStyle HorizontalAlign="Center" Width="400px" />
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
                                            <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Remark">
                                            <ItemTemplate>
                                                <asp:Label ID="Remark" runat="server" Text='<%# Bind("Remark") %>' />
                                                <asp:Label ID="CATEGORY_ID" runat="server" Visible="false" Text='<%# Bind("CATEGORY_ID") %>' />
                                                <asp:Label ID="ITEM_ID" runat="server" Visible="false" Text='<%# Bind("ITEM_ID") %>' />
                                                <asp:Label ID="QualityId" runat="server" Visible="false" Text='<%# Bind("QualityId") %>' />
                                                <asp:Label ID="ColorId" runat="server" Visible="false" Text='<%# Bind("ColorId") %>' />
                                                <asp:Label ID="designId" runat="server" Visible="false" Text='<%# Bind("designId") %>' />
                                                <asp:Label ID="SizeId" runat="server" Visible="false" Text='<%# Bind("SizeId") %>' />
                                                <asp:Label ID="ShapeId" runat="server" Visible="false" Text='<%# Bind("ShapeId") %>' />
                                                <asp:Label ID="ShadecolorId" runat="server" Visible="false" Text='<%# Bind("ShadecolorId") %>' />
                                                <asp:Label ID="GoDownID" runat="server" Visible="false" Text='<%# Bind("GoDownID") %>' />
                                                <asp:Label ID="GATEOUTDETAILID" runat="server" Visible="false" Text='<%# Bind("GATEOUTDETAILID") %>' />
                                                <asp:Label ID="GATEOUTID" runat="server" Visible="false" Text='<%# Bind("GATEOUTID") %>' />
                                                <asp:Label ID="lotno" runat="server" Visible="false" Text='<%# Bind("lotno") %>' />
                                                <asp:Label ID="tagno" runat="server" Visible="false" Text='<%# Bind("tagno") %>' />
                                                <asp:Label ID="CategoryType" runat="server" Visible="false" Text='<%# Bind("CategoryType") %>' />
                                                <asp:Label ID="qty" runat="server" Visible="false" Text='<%# Bind("qty") %>' />
                                                <asp:Label ID="ITEM_FINISHED_ID" runat="server" Visible="false" Text='<%# Bind("ITEM_FINISHED_ID") %>' />
                                                <asp:Label ID="ISSUENO" runat="server" Visible="false" Text='<%# Bind("ISSUENO") %>' />
                                                <asp:Label ID="ISSUEDATE" runat="server" Visible="false" Text='<%# Bind("ISSUEDATE") %>' />
                                                <asp:Label ID="ProductCode" runat="server" Visible="false" Text='<%# Bind("ProductCode") %>' />
                                                <asp:Label ID="lblflagsize" runat="server" Visible="false" Text='<%# Bind("flagsize") %>' />
                                                <asp:Label ID="lblunitid" runat="server" Visible="false" Text='<%# Bind("Unitid") %>' />
                                                <asp:Label ID="lblbinno" runat="server" Visible="false" Text='<%# Bind("BinNo") %>' />
                                                <asp:Label ID="lblEWayBillNo" runat="server" Visible="false" Text='<%# Bind("EWayBillNo") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="250px" />
                                            <ItemStyle HorizontalAlign="Center" Width="250px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="Del" OnClientClick="return confirm('Do You Want To Delete Row ?')"></asp:LinkButton>
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
