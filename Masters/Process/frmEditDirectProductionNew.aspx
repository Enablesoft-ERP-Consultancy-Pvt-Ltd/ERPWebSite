<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmEditDirectProductionNew.aspx.cs"
    Inherits="Masters_Process_frmEditDirectProductionNew" MasterPageFile="~/ERPmaster.master"
    Title="Edit Production Order" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script language="javascript" type="text/javascript"></script>
    <script type="text/javascript">
        function checkforItem() {
            //  alert('hi')
            //            if (document.getElementById('CPH_Form_chkchangeWeaver').checked) {
            //                document.getElementById('btnchangeWeaver').visible = true;
            //            }
            //            else {
            //                document.getElementById('btnchangeWeaver').visible = false;
            //            }
        }
        function reloadPage() {
            window.location.href = "frmEditDirectProductionNew.aspx";
        }
        function Preview() {
            window.open('../../reportViewer1.aspx', '');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function validate_RequiredDate() {
            var required_date = document.getElementById('TxtRequiredDate').Value;
            var assign_date = document.getElementById('TxtAssignDate').value;
            if (assign_date < required_date) {
                alert("Required Date Must Be Greater Then Assign Date");
            }
        }
        function Validation() {
            //          
            if (document.getElementById("<%=DDunit.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDunit').value <= "0") {
                    alert("Please select Unit....!");
                    document.getElementById("CPH_Form_DDunit").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=DDcaltype.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDcaltype').value <= "") {
                    alert("Please select CalType....!");
                    document.getElementById("CPH_Form_DDcaltype").focus();
                    return false;
                }
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
            if (document.getElementById("<%=TDQuality.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddquality').value <= "0") {
                    alert("Please Select Quality....!");
                    document.getElementById("CPH_Form_ddquality").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TDDesign.ClientID %>")) {
                if (document.getElementById('CPH_Form_dddesign').value <= "0") {
                    alert("Please Select Design....!");
                    document.getElementById("CPH_Form_dddesign").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TDColor.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddcolor').value <= "0") {
                    alert("Please Select Colour....!");
                    document.getElementById("CPH_Form_ddcolor").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TDShape.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddshape').value <= "0") {
                    alert("Please Select Shape....!");
                    document.getElementById("CPH_Form_ddshape").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TdSize.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddsize').value <= "0") {
                    alert("Please Select Size....!");
                    document.getElementById("CPH_Form_ddsize").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TDShade.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddlshade').value <= "0") {
                    alert("Please Select Shade Colour....!");
                    document.getElementById("CPH_Form_ddlshade").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TxtQtyRequired.ClientID %>").value == "") {
                alert("Please fill Order qty");
                document.getElementById("<%=TxtQtyRequired.ClientID %>").focus();
                return false;
            }
            else {
                return confirm('Do you want to save data?')
            }
        }
        function isNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }

            else {
                return true;
            }
        }
        function isNumberWith(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }

    </script>
    <script type="text/javascript">
        function jScriptValidate() {
            $("#CPH_Form_BtnSave").click(function () {
                var Message = "";
                if ($("#CPH_Form_ddUnits")) {
                    var selectedIndex = $('#CPH_Form_ddUnits').attr('selectedIndex');
                    if (selectedIndex < 0) {
                        Message = Message + "Please,Select Unit Name!!!\n";
                    }
                }

                if ($("#CPH_Form_dditemname")) {
                    var selectedIndex = $('#CPH_Form_dditemname').attr('selectedIndex');
                    if (selectedIndex <= 0) {
                        Message = Message + "Please,select Article Name !!!\n";
                    }
                }
                if ($("#CPH_Form_ddcolor")) {
                    var selectedIndex = $('#CPH_Form_ddcolor').attr('selectedIndex');
                    if (selectedIndex <= 0) {
                        Message = Message + "Please,select Colour Name !!!\n";
                    }
                }
                if ($("#CPH_Form_ddshape").length) {
                    var selectedIndex = $('#CPH_Form_ddshape').attr('selectedIndex');
                    if (selectedIndex <= 0) {
                        Message = Message + "Please,select Shape Name !!!\n";
                    }
                }
                if ($("#CPH_Form_ddsize").length) {
                    var selectedIndex = $('#CPH_Form_ddsize').attr('selectedIndex');
                    if (selectedIndex <= 0) {
                        Message = Message + "Please,select Size !!!\n";
                    }
                }
                if ($("#CPH_Form_TxtRate").val() == "" || $("#CPH_Form_TxtRate").val() == "0") {
                    Message = Message + "Rate can not be blank and Zero !!!\n";
                }
                if ($("#CPH_Form_TxtQtyRequired").val() == "" || $("#CPH_Form_TxtQtyRequired").val() == "0") {
                    Message = Message + "Order Qty. can not be blank and Zero !!!\n";
                }
                if (Message == "") {
                    return true;
                }
                else {
                    alert(Message);
                    return false;
                }
            });
            //now use keypress event for Pincode and Mobile No
            $("#CPH_Form_txtrate").keypress(function (event) {

                if (event.which >= 46 && event.which <= 58) {
                    return true;
                }
                else {
                    return false;
                }

            });
            //on DropDown Selected Index
            $("#CPH_Form_dditemname").change(function () {
                $("#CPH_Form_ddcolor").attr('selectedIndex', 0);
                $("#CPH_Form_ddsize").attr('selectedIndex', 0);
            });
            $("#CPH_Form_ddcolor").change(function () {

                $("#CPH_Form_ddsize").attr('selectedIndex', 0);
            });
            // ENd
        }
    </script>
    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(jScriptValidate);
            </script>
            <div style="height: auto">
                <div style="width: 900px">
                    <div style="width: 800px; margin: auto; height: 155px; border-style: groove; background-color: #DEB887;">
                        <div style="float: left; margin-left: 150px; margin-top: 45px; width: 280px;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPONo" runat="server" CssClass="labelbold" Text="Enter Folio No."></asp:Label>
                                        <asp:TextBox ID="txtPoNo" runat="server" Height="30px" Width="120px" OnTextChanged="txtPoNo_TextChanged"
                                            AutoPostBack="true" CssClass="textb"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblWeaverIdNo" runat="server" CssClass="labelbold" Text="Enter Weaver ID No."></asp:Label>
                                        <asp:TextBox ID="txtWeaverIdNo" runat="server" AutoPostBack="true" Height="30px"
                                            CssClass="textb" OnTextChanged="txtWeaverIdNo_TextChanged" Width="120px"></asp:TextBox>
                                    </td>
                                    <div>
                                    </div>
                                </tr>
                            </table>
                        </div>
                        <div style="float: right; margin-right: 160px; margin-top: 20px">
                            <table>
                                <tr>
                                    <td>
                                        <div style="overflow: auto; width: 200px">
                                            <asp:ListBox ID="listWeaverName" runat="server" Width="200px" Height="100px" SelectionMode="Multiple">
                                            </asp:ListBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btnDelete" Text="Delete" runat="server"  OnClick="btnDelete_Click"
                                            CssClass="buttonnorm" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="width: 405px; margin-top: 40px">
                            <table style="padding-top: 30px">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkchangeWeaver" runat="server" Text="Check for Change Weaver ID Only."
                                            Font-Bold="true" ForeColor="Red" onclick="return Checkforitem();" AutoPostBack="true" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnchangeWeaver" runat="server" Text="Click to Change" CssClass="buttonnorm"
                                            Width="150px" OnClick="btnchangeWeaver_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div style="width: 850px; height: 30px; margin-top: 10px">
                    <asp:Panel ID="panelMaster" runat="server" BorderStyle="Solid" BorderWidth="1px"
                        BackColor="#DEB887" Width="850px">
                        <table width="600px">
                            <tr>
                                <td>
                                    <asp:Label ID="lblUnits" runat="server" Text="Units" CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddUnits" runat="server" Width="100px" CssClass="dropdown" TabIndex="1">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblAssigndate" runat="server" Text="AssignDate" CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtAssignDate" runat="server" Width="90px" CssClass="textb" BackColor="beige"
                                        TabIndex="2"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                        TargetControlID="TxtAssignDate">
                                    </asp:CalendarExtender>
                                </td>
                                <td id="TdlblRequiredate" runat="server" visible="false">
                                    <asp:Label ID="lblRequiredate" runat="server" Text="RequireDate" CssClass="labelbold"></asp:Label>
                                </td>
                                <td id="TdtxtRequiredate" runat="server" visible="false">
                                    <asp:TextBox ID="TxtRequiredDate" runat="server" AutoPostBack="true" Width="90px"
                                        CssClass="textb" BackColor="beige" TabIndex="3"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                        TargetControlID="TxtRequiredDate">
                                    </asp:CalendarExtender>
                                </td>
                                <td id="Tdlblunit" runat="server" visible="false">
                                    <asp:Label ID="lblUnit" runat="server" Text="Unit" CssClass="labelbold"></asp:Label>
                                </td>
                                <td id="TdDDUnit" runat="server" visible="false">
                                    <asp:DropDownList CssClass="dropdown" ID="DDunit" runat="server" Width="100px" TabIndex="4">
                                    </asp:DropDownList>
                                    <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="DDunit"
                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                    </cc1:ListSearchExtender>
                                </td>
                                <td id="Td1" runat="server" visible="false">
                                    <asp:Label ID="lblCalType" runat="server" Text="CalType" CssClass="labelbold"></asp:Label>
                                </td>
                                <td id="Td2" runat="server" visible="false">
                                    <asp:DropDownList CssClass="dropdown" ID="DDcaltype" runat="server" Width="100px"
                                        TabIndex="5">
                                        <%--<asp:ListItem Value="0">Area Wise</asp:ListItem>--%>
                                        <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblChallanno" runat="server" Text="FolioNo." CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtChallanNo" runat="server" Width="90px" CssClass="textb" ReadOnly="True"
                                        TabIndex="6"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                <div>
                    <fieldset>
                        <legend>
                            <asp:Label ID="lblItemDetail" Text="ItemDetail" runat="server" CssClass="labelbold"></asp:Label></legend>
                        <table>
                            <tr>
                                <td id="TDProductCode" runat="server" visible="false">
                                    <span class="labelbold">Product Code</span>
                                    <br />
                                    <asp:TextBox ID="TxtProductCode" runat="server" TabIndex="8" Width="80px" CssClass="textb"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                                        Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtProductCode"
                                        UseContextKey="True">
                                    </cc1:AutoCompleteExtender>
                                </td>
                                <td id="Td3" runat="server" visible="false">
                                    <asp:Label ID="lblCategory" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="ddCatagory" runat="server" Width="150px" AutoPostBack="True"
                                        TabIndex="9" CssClass="dropdown" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <cc1:ListSearchExtender ID="ListSearchddCatagory" runat="server" TargetControlID="ddCatagory"
                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                    </cc1:ListSearchExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lblItemName" runat="server" Text="Articles" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="dditemname" runat="server" Width="150px" TabIndex="10" AutoPostBack="True"
                                        CssClass="dropdown">
                                    </asp:DropDownList>
                                    <cc1:ListSearchExtender ID="ListSearchdditemname" runat="server" TargetControlID="dditemname"
                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                    </cc1:ListSearchExtender>
                                </td>
                                <td id="TDQuality" runat="server" visible="false">
                                    <asp:Label ID="lblqualityname" runat="server" Text="Quality"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="ddquality" runat="server" Width="150px" TabIndex="11" CssClass="dropdown"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                    <cc1:ListSearchExtender ID="ListSearchdquality" runat="server" TargetControlID="ddquality"
                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                    </cc1:ListSearchExtender>
                                </td>
                                <td id="TDDesign" runat="server" visible="false">
                                    <asp:Label ID="lbldesignname" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="dddesign" runat="server" Width="150px" TabIndex="12" CssClass="dropdown">
                                    </asp:DropDownList>
                                    <cc1:ListSearchExtender ID="ListSearchdddesign" runat="server" TargetControlID="dddesign"
                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                    </cc1:ListSearchExtender>
                                </td>
                                <td id="TDColor" runat="server" visible="false">
                                    <asp:Label ID="lblcolorname" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="ddcolor" runat="server" Width="150px" TabIndex="13" CssClass="dropdown">
                                    </asp:DropDownList>
                                    <cc1:ListSearchExtender ID="ListSearchddcolor" runat="server" TargetControlID="ddcolor"
                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                    </cc1:ListSearchExtender>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td id="TDShape" runat="server" visible="false">
                                    <asp:Label ID="lblshapename" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="ddshape" runat="server" Width="150px" AutoPostBack="True" TabIndex="14"
                                        CssClass="dropdown" OnSelectedIndexChanged="ddshape_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <cc1:ListSearchExtender ID="ListSearchddshape" runat="server" TargetControlID="ddshape"
                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                    </cc1:ListSearchExtender>
                                </td>
                                <td id="TdSize" runat="server" visible="false">
                                    <asp:Label ID="lblsize" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                    <asp:CheckBox ID="ChkForMtr" runat="server" AutoPostBack="True" Text="Check For Mtr."
                                        Visible="false" CssClass="checkboxbold" /><br />
                                    <asp:DropDownList ID="ddsize" runat="server" Width="150px" TabIndex="15" CssClass="dropdown"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddsize_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <cc1:ListSearchExtender ID="ListSearchddsize" runat="server" TargetControlID="ddsize"
                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                    </cc1:ListSearchExtender>
                                </td>
                                <td id="TDShade" runat="server" visible="false">
                                    <asp:Label ID="lblshadecolor" runat="server" Text="ShadeColor" CssClass="labelbold"></asp:Label>
                                    &nbsp;<br />
                                    <asp:DropDownList ID="ddlshade" runat="server" Width="150px" CssClass="dropdown"
                                        TabIndex="16" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <cc1:ListSearchExtender ID="ListSearchddlshade" runat="server" TargetControlID="ddlshade"
                                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                    </cc1:ListSearchExtender>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td id="TdWidth" runat="server" visible="false">
                                    <span class="labelbold">Width</span>
                                    <br />
                                    <asp:TextBox ID="TxtWidth" runat="server" Width="90px" Enabled="false" CssClass="textb"
                                        TabIndex="17"></asp:TextBox>
                                </td>
                                <td id="TdLength" runat="server" visible="false">
                                    <span class="labelbold">Length</span>
                                    <br />
                                    <asp:TextBox ID="TxtLength" runat="server" Width="90px" Enabled="false" CssClass="textb"
                                        TabIndex="18"></asp:TextBox>
                                </td>
                                <td id="TdArea" runat="server" visible="false">
                                    <span class="labelbold">Area</span>
                                    <br />
                                    <asp:TextBox ID="TxtArea" runat="server" Width="70px" Enabled="false" CssClass="textb"
                                        TabIndex="19"></asp:TextBox><br />
                                </td>
                                <td>
                                    <span class="labelbold">Rate</span>
                                    <br />
                                    <asp:TextBox ID="TxtRate" runat="server" Width="70px" AutoPostBack="True" CssClass="textb"
                                        TabIndex="20" onkeypress="return isNumber(event);" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td id="Td4" runat="server" visible="false">
                                    <span class="labelbold">Commission</span>
                                    <br />
                                    <asp:TextBox ID="TxtCommission" runat="server" Width="70px" CssClass="textb" TabIndex="21"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="labelbold">OrderQty</span>
                                    <br />
                                    <asp:TextBox ID="TxtQtyRequired" runat="server" Width="70px" CssClass="textb" BackColor="beige"
                                        TabIndex="22" onkeypress=" return isNumberWith(event);"></asp:TextBox>
                                </td>
                                <td align="right">
                                    <br />
                                    <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="BtnSave_Click"
                                        TabIndex="23" Width="70px" />
                                    &nbsp;<asp:Button ID="BtnNew" runat="Server" Text="New" OnClientClick="return reloadPage();"
                                        CssClass="buttonnorm" TabIndex="24" Width="70px" />
                                    &nbsp;<asp:Button ID="BtnUpdate" runat="server" Text="Update" Visible="False" OnClientClick="return confirm('Do you want to Update?')"
                                        CssClass="buttonnorm" TabIndex="25" Width="70px" />
                                    &nbsp;<asp:Button ID="BtnPreview" runat="server" Text="Preview" Visible="true" CssClass="buttonnorm"
                                        TabIndex="26" OnClick="BtnPreview_Click" Width="70px" />
                                    &nbsp;<asp:Button ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                                        CssClass="buttonnorm" TabIndex="27" Width="70px" />
                                </td>
                            </tr>
                        </table>
                        <table width="800px">
                            <tr id="Tr1" runat="server" visible="false">
                                <td colspan="2">
                                    <span class="labelbold">Remarks</span>
                                    <asp:TextBox ID="TxtRemarks" runat="server" CssClass="textb" Width="618px" TabIndex="28"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="Tr2" runat="server" visible="false">
                                <td>
                                    <span class="labelbold">Instruction</span>
                                    <br />
                                    <asp:TextBox ID="TxtInstructions" runat="server" Width="485px" Height="50px" CssClass="textb"
                                        TextMode="MultiLine" TabIndex="29"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hncomp" runat="server" />
                                    <asp:HiddenField ID="hdArea" runat="server" />
                                </td>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="LblErrorMessage" runat="server" Text="" Font-Bold="true" ForeColor="RED"></asp:Label>
                                    </td>
                                </tr>
                        </table>
                    </fieldset>
                </div>
                <table width="100%">
                    <tr>
                        <td colspan="3">
                            <div style="width: 798px; height: 89px; overflow: auto">
                                <asp:GridView ID="DGOrderdetail" runat="server" AutoGenerateColumns="False" DataKeyNames="IssueDetailId"
                                     Width="500px" OnRowEditing="DGOrderdetail_RowEditing"
                                    OnRowCancelingEdit="DGOrderdetail_RowCancelingEdit" OnRowUpdating="DGOrderdetail_RowUpdating"
                                    OnRowDeleting="DGOrderdetail_RowDeleting">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:BoundField DataField="Category" HeaderText="CATEGORY" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="125px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Articles" HeaderText="ARTICLES">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Colour" HeaderText="COLOUR">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Size" HeaderText="SIZE">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Length" Visible="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtlength" runat="server" Text='<%#Bind("Length") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Width" Visible="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWidth" runat="server" Text='<%#Bind("Width") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <%#Eval("Qty") %>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtQty" runat="server" BackColor="Yellow" Width="75px" Text='<%#Eval("Qty") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate" Visible="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRate" runat="server" Text='<%#Bind("Rate") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Area" Visible="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtArea" runat="server" Text='<%#Bind("Area") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%#Bind("AMount") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OrderId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderId" runat="server" Text='<%#Bind("OrderId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ItemFInishedid" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemFinishedId" runat="server" Text='<%#Bind("Item_Finished_Id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="IssueOrderId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIssueOrderId" runat="server" Text='<%#Bind("IssueOrderId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:CommandField ShowEditButton="True" />
                                        <asp:CommandField ShowDeleteButton="True" />
                                    </Columns>
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
