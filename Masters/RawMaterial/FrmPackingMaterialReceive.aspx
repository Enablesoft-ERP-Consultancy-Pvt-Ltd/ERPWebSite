<%@ Page Title="Packing Material Receive" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="FrmPackingMaterialReceive.aspx.cs" Inherits="Masters_RawMaterial_FrmPackingMaterialReceive" %>

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
            window.location.href = "FrmGateIn.aspx";
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
            if (document.getElementById("<%=TxtRecQty.ClientID %>").value == "") {
                alert("Pls fill rec qty");
                document.getElementById("<%=TxtRecQty.ClientID %>").focus();
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
            var RejectQty = 0;
            if (document.getElementById("<%=TxtIssueQty.ClientID %>")) {
                IssQty = document.getElementById("<%=TxtIssueQty.ClientID %>").value
                if (document.getElementById("<%=TxtRecQty.ClientID %>").value != "") {
                    RecQty = document.getElementById("<%=TxtRecQty.ClientID %>").value
                }
                else {
                    RecQty = 0;
                }                
                if (document.getElementById("<%=TxtRejectQty.ClientID %>").value != "") {
                    RejectQty = document.getElementById("<%=TxtRejectQty.ClientID %>").value
                }
                else {
                    RejectQty = 0;
                }
                if (parseFloat(RecQty) + parseFloat(RejectQty) > parseFloat(IssQty)) {
                    alert("Rec/Reject qty is greater than issue qty")
                    document.getElementById("<%=TxtRecQty.ClientID %>").value = "";
                    document.getElementById("<%=TxtRejectQty.ClientID %>").value = "";
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
                        <td id="TDDept" runat="server" visible="false">
                            <asp:Label ID="Label13" runat="server" Text="Department" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDDept" runat="server" Width="150px" CssClass="dropdown" AutoPostBack="True"
                                OnSelectedIndexChanged="DDDept_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="Td4" class="tdstyle">
                            <span class="labelbold">Party Name</span> &nbsp;
                            <asp:CheckBox ID="ChKForEdit" runat="server" Text=" For Edit" CssClass="checkboxbold"
                                OnCheckedChanged="ChKForEdit_CheckedChanged" AutoPostBack="true" />
                            <br />
                            <asp:DropDownList ID="ddempname" runat="server" Width="200px" AutoPostBack="True"
                                CssClass="dropdown" OnSelectedIndexChanged="ddempname_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="Td3" runat="server" visible="false">
                            <span class="labelbold">Receive No</span>
                            <br />
                            <asp:DropDownList ID="DDGateInNo" runat="server" Width="100px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDGateInNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="Td2" runat="server">
                            <span class="labelbold">Issue No</span>
                            <br />
                            <asp:DropDownList ID="DDGatePassNo" runat="server" Width="100px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDGatePassNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                       
                       
                    </tr>
                </table>
                <table>
                <tr id="TR4" runat="server" class="tdstyle">
                    <td id="TDCustCode" runat="server" visible="true">
                            <asp:Label ID="Label16" runat="server" Text="Cust Code" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDCustCode" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCustCode_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                         <td id="TDCustOrderNo" runat="server" visible="true">
                            <asp:Label ID="Label17" runat="server" Text="Order No" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDCustOrderNo" runat="server" Width="150px" CssClass="dropdown">
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
                            <span class="labelbold">Receive No </span>
                            <br />
                            <asp:TextBox ID="TxtGateInNo" Width="100px" runat="server" CssClass="textb" ReadOnly="true"
                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                       <%-- <td id="Td7" class="tdstyle">
                            <span class="labelbold">Challan No. </span>
                            <br />
                            <asp:TextBox ID="txtchallanNo" Width="100px" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>--%>
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
                
                </tr>
                
                </table>

                <table>
                    <tr id="Tr3" runat="server" class="tdstyle">

                     <td class="tdstyle">
                            <asp:Label ID="lblcategorytype" runat="server" Text="Catagory Type" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddlcatagorytype" runat="server" Width="150px" OnSelectedIndexChanged="ddlcatagorytype_SelectedIndexChanged"
                                AutoPostBack="True" CssClass="dropdown">
                                <asp:ListItem Value="1">Raw Material</asp:ListItem>
                                <%--<asp:ListItem Value="0">Finished Item</asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
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
                            <asp:DropDownList ID="dquality" runat="server" Width="150px" CssClass="dropdown">
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
                        <td class="tdstyle">
                            <span class="labelbold">RecQty</span>
                            <br />
                            <asp:TextBox ID="TxtRecQty" runat="server" CssClass="textb" onkeypress="return isNumber(event);"
                                onchange="return checkqty();" BackColor="Beige" Width="70px"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">Reject Qty</span>
                            <br />
                            <asp:TextBox ID="TxtRejectQty" runat="server" CssClass="textb" onkeypress="return isNumber(event);"
                                BackColor="Beige" Width="70px" onchange="return checkqty();"></asp:TextBox>
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
                                    DataKeyNames="PackingMaterialReceiveDetailId" CssClass="grid-views" OnRowDeleting="gvdetail_RowDeleting">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <Columns>
                                    <asp:TemplateField HeaderText="ItemDescription">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldescription" Text='<%#Bind("description") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Godown Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGodownName" Text='<%#Bind("GodownName") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Lot No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLotNo" Text='<%#Bind("LotNo") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tag No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTagNo" Text='<%#Bind("TagNo") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       <asp:TemplateField HeaderText="Rec Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReceiveQty" Text='<%#Bind("ReceiveQty") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Reject Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRejectQty" Text='<%#Bind("RejectQty") %>' runat="server" />
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
                        <td>
                            <div id="DivDGShowData" runat="server" visible="false" style="width: 300px; height: 300px;
                                overflow: scroll;">
                                <asp:GridView ID="DGShowData" runat="server" AutoGenerateColumns="False" OnRowDataBound="DGShowData_RowDataBound"
                                    DataKeyNames="Finishedid" CssClass="grid-view" OnRowCreated="DGShowData_RowCreated"
                                    OnSelectedIndexChanged="DGShowData_SelectedIndexChanged">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <Columns>
                                        <asp:BoundField DataField="Description" HeaderText="Description">
                                            <HeaderStyle HorizontalAlign="Center" Width="350px" />
                                            <ItemStyle HorizontalAlign="Center" Width="350px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="LotNo">
                                            <ItemTemplate>
                                                <asp:Label ID="lbllotno" Text='<%#Bind("Lotno") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TagNo">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltagno" Text='<%#Bind("TagNo") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Iss Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIssQty" Text='<%#Bind("IssQty") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Rec Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRecQty" Text='<%#Bind("RecQty") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reject Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRejectQty" Text='<%#Bind("RejectQty") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblunitid" Text='<%#Bind("unitid") %>' runat="server"></asp:Label>
                                                <asp:Label ID="lblCustomerId" Text='<%#Bind("CustomerId") %>' runat="server"></asp:Label>
                                                <asp:Label ID="lblOrderId" Text='<%#Bind("OrderId") %>' runat="server"></asp:Label>
                                                <asp:Label ID="lblDepartmentId" Text='<%#Bind("DepartmentId") %>' runat="server"></asp:Label>
                                                <asp:Label ID="lblBranchId" Text='<%#Bind("BranchId") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
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
