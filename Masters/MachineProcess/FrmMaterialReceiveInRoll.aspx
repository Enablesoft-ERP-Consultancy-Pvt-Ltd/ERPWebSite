<%@ Page Title="Production Receive In Roll" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmMaterialReceiveInRoll.aspx.cs" Inherits="Masters_MachineProcess_FrmMaterialReceiveInRoll" %>

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
            window.location.href = "FrmMaterialReceiveInRoll.aspx";
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
        function Addsize() {
            var answer = confirm("Do you want to ADD?")
            if (answer) {
                var Shapeid = 0;
                if (document.getElementById('CPH_Form_ddshape')) {
                    var e = document.getElementById("CPH_Form_ddshape");
                    if (e.options.length > 0) {
                        var Shapeid = e.options[e.selectedIndex].value;
                    }
                }
                e = document.getElementById("CPH_Form_DDUnit");
                var VarUnitID = e.options[e.selectedIndex].value;
                window.open('../Carpet/AddSize.aspx?ShapeID=' + Shapeid + '&UnitID=' + VarUnitID + '', '', 'width=1000px,Height=501px');
            }
        }

        function Validation() {
            if (document.getElementById("<%=ddCompName.ClientID %>").value <= "0") {
                alert("Pls Select Company Name");
                document.getElementById("<%=ddCompName.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=DDProcessName.ClientID %>").value <= "0") {
                alert("Pls Select Process Name");
                document.getElementById("<%=DDProcessName.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=DDProductionUnit.ClientID %>").value <= "0") {
                alert("Pls Select production unit");
                document.getElementById("<%=DDProductionUnit.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=DDMachineNo.ClientID %>").value <= "0") {
                alert("Pls Select machine no");
                document.getElementById("<%=DDMachineNo.ClientID %>").focus();
                return false;
            }

            if (document.getElementById("<%=Td3.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDReceiveNo').value <= "0") {
                    alert("Please select receive no....!");
                    document.getElementById("CPH_Form_DDReceiveNo").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=txtReceiveDate.ClientID %>").value == "") {
                alert("Pls Select Receive Date");
                document.getElementById("<%=txtReceiveDate.ClientID %>").focus();
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
            if (document.getElementById("<%=DDUnit.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDUnit').value <= "0") {
                    alert("Please Select Unit....!");
                    document.getElementById("CPH_Form_DDUnit").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=DDIssueNo.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDIssueNo').value <= "0") {
                    alert("Please Select Issue No....!");
                    document.getElementById("CPH_Form_DDIssueNo").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=DDCustomerCode.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDCustomerCode').value <= "0") {
                    alert("Please Select customer code....!");
                    document.getElementById("CPH_Form_DDCustomerCode").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=DDCustomerOrderNumber.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDCustomerOrderNumber').value <= "0") {
                    alert("Please Select order No....!");
                    document.getElementById("CPH_Form_DDCustomerOrderNumber").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=DDOrderDescription.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDOrderDescription').value <= "0") {
                    alert("Please Select Order Description....!");
                    document.getElementById("CPH_Form_DDOrderDescription").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TxtRecQty.ClientID %>").value == "" || document.getElementById("<%=TxtRecQty.ClientID %>").value == "0") {
                alert("Receive qty Can not be blank Or Zero");
                document.getElementById("<%=TxtRecQty.ClientID %>").focus();
                return false;
            }
            else {
                return confirm('Do you want to save data?')
            }
        }
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div>
                <table>
                    <tr id="Tr1" runat="server">
                        <td id="Td1" colspan="2" class="tdstyle">
                            <asp:Label ID="Label1" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddCompName" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="Td4" class="tdstyle">
                            <asp:Label ID="Label2" runat="server" Text=" Process Name" CssClass="labelbold"></asp:Label>
                            &nbsp;
                            <asp:CheckBox ID="ChKForEdit" runat="server" Text=" For Edit" CssClass="checkboxbold"
                                OnCheckedChanged="ChKForEdit_CheckedChanged" AutoPostBack="true" />
                            <br />
                            <asp:DropDownList ID="DDProcessName" runat="server" Width="150px" AutoPostBack="True"
                                CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="Td7" class="tdstyle">
                            <asp:Label ID="Label13" runat="server" Text=" Production Unit" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDProductionUnit" runat="server" Width="150px" AutoPostBack="True"
                                CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td id="Td8" class="tdstyle">
                            <asp:Label ID="Label16" runat="server" Text=" Machine No" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDMachineNo" runat="server" Width="150px" AutoPostBack="True"
                                CssClass="dropdown" OnSelectedIndexChanged="DDMachineNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="Td3" runat="server" visible="false">
                            <asp:Label ID="Label17" runat="server" Text="Receive No" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDReceiveNo" runat="server" Width="150px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDReceiveNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="Td5" class="tdstyle">
                            <asp:Label ID="Label4" runat="server" Text="Receive Date" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtReceiveDate" runat="server" Width="100px" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtReceiveDate">
                            </asp:CalendarExtender>
                        </td>
                        <td id="Td6" class="tdstyle">
                            <asp:Label ID="Label5" runat="server" Text=" Receive No." CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="txtReceiveno" Width="100px" runat="server" CssClass="textb" ReadOnly="true"
                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
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
                        <td id="shp" runat="server" visible="false">
                            <asp:Label ID="lblshapename" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="ddshape" runat="server" Width="100px" CssClass="dropdown" AutoPostBack="True"
                                OnSelectedIndexChanged="ddshape_SelectedIndexChanged">
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
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Unit" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDUnit" runat="server" Width="100px" CssClass="dropdown" OnSelectedIndexChanged="DDUnit_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td id="sz" runat="server" class="tdstyle">
                            <asp:Label ID="Label7" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                            <asp:Button ID="BtnRefreshSize" runat="server" Style="display: none" OnClick="BtnRefreshSize_Click" />
                            <br />
                            <asp:DropDownList ID="ddsize" runat="server" Width="120px" TabIndex="7" CssClass="dropdown"
                                OnSelectedIndexChanged="ddsize_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                            <asp:Button ID="btnaddsize" runat="server" Width="20px" CssClass="dropdown" OnClientClick="return Addsize();"
                                Text="&#43;" />
                        </td>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="Weight" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtMainRollWeight" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Issue No" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList ID="DDIssueNo" runat="server" Width="150px" CssClass="dropdown">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label12" runat="server" Text="Buyer" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDCustomerCode" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label14" runat="server" Text="OrderNo" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDCustomerOrderNumber" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCustomerOrderNumber_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label18" Text="Split Width" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtSplitWidth" runat="server" Width="80px" CssClass="textb"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label19" Text="Split Length" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtSplitLength" runat="server" Width="80px" CssClass="textb"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="Order Description" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDOrderDescription" Width="250px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDOrderDescription_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label22" runat="server" Text="No of Roll" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtNoofRoll" runat="server" Width="80px" CssClass="textb"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label20" runat="server" Text="Order Width" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtOrderWidth" runat="server" Width="80px" CssClass="textb" AutoPostBack="true"
                                OnTextChanged="TxtOrderWidth_TextChanged"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label21" runat="server" Text="Order Length" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtOrderLength" runat="server" Width="80px" CssClass="textb" AutoPostBack="true"
                                OnTextChanged="TxtOrderLength_TextChanged"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label10" runat="server" Text="Order Qty" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtOrderQty" runat="server" Enabled="false" Width="80px" CssClass="textb"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label11" runat="server" Text="Pend Qty" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtPendingQty" runat="server" Enabled="false" Width="80px" CssClass="textb"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label15" runat="server" Text="Rec Qty" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtRecQty" runat="server" Width="80px" CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table style="width: 80%;">
                    <tr>
                        <td>
                            <asp:Label ID="lblmessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                        </td>
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
                                <asp:GridView ID="gvdetail" runat="server" AutoGenerateColumns="False" EmptyDataText="No records found.."
                                    OnRowDataBound="gvdetail_RowDataBound" OnRowDeleting="gvdetail_RowDeleting">
                                    <RowStyle CssClass="gvrow" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <HeaderStyle CssClass="gvheaders" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Roll No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRollNo" Text='<%#Bind("MaterialReceiveInPcsID") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sub Roll No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubRollNo" Text='<%#Bind("MaterialReceiveInPcsDetailID") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderNo" Text='<%#Bind("OrderNo") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQty" Text='<%#Bind("Qty") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMaterialReceiveInPcsID" Text='<%#Bind("MaterialReceiveInPcsID") %>'
                                                    runat="server" />
                                                <asp:Label ID="lblMaterialReceiveInPcsDetailID" Text='<%#Bind("MaterialReceiveInPcsDetailID") %>'
                                                    runat="server" />
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
            <asp:HiddenField ID="HnMaterialReceiveID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
