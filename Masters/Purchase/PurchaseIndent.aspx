<%@ Page Title="Purchase Indent" Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="PurchaseIndent.aspx.cs" Inherits="Masters_Purchase_PurchaseIndent"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Mainpage" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "PurchaseIndent.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function AddSize() {
            window.open('../Carpet/AddSize.aspx', '', 'Height=500px,width=1000px');
        }
        function Preveiw() {
            window.open("../../ReportViewer.aspx", "PurchaseIndent");
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
        function Validation() {
            if (document.getElementById('CPH_Form_DDCompanyName').value <= "0") {
                alert("Please Select Company Name....!");
                document.getElementById("CPH_Form_DDCompanyName").focus();
                return false;
            }
            if (document.getElementById("<%=tdindno.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddindentno').value <= "0") {
                    alert("Please Select IndentNo....!");
                    document.getElementById("CPH_Form_ddindentno").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_DDDepartment').value <= "0") {
                alert("Please Select Department Name....!");
                document.getElementById("CPH_Form_DDDepartment").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDPartyName').value <= "0") {
                alert("Please Select Vendor Name....!");
                document.getElementById("CPH_Form_DDPartyName").focus();
                return false;
            }
            if (document.getElementById("CPH_Form_tdemp")) {
                if (document.getElementById('CPH_Form_DDEmp').value <= "0") {
                    alert("Please Select Employee Name....!");
                    document.getElementById("CPH_Form_DDEmp").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=tdcust.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddcustomer').value <= "0") {
                    alert("Please Select Customer Code....!");
                    document.getElementById("CPH_Form_ddcustomer").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=tdorder.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddorder').value <= "0") {
                    alert("Please Select Order No....!");
                    document.getElementById("CPH_Form_ddorder").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_TxtDate').value == "") {
                alert("Please Select Date....!");
                document.getElementById("CPH_Form_TxtDate").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDCategory').value <= "0") {
                alert("Please Select Category....!");
                document.getElementById("CPH_Form_DDCategory").focus();
                return false;
            }
            if (document.getElementById('CPH_Form_DDItem').value <= "0") {
                alert("Please Select Item....!");
                document.getElementById("CPH_Form_DDItem").focus();
                return false;
            }
            if (document.getElementById("<%=TdQuality.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDQuality').value <= "0") {
                    alert("Please Select Quality Name....!");
                    document.getElementById("CPH_Form_DDQuality").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TdDesign.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDDesign').options[document.getElementById('CPH_Form_DDDesign').selectedIndex].value <= 0) {
                    alert("Please Select Design Name....!");
                    document.getElementById("CPH_Form_DDDesign").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TdColor.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDColor').options[document.getElementById('CPH_Form_DDColor').selectedIndex].value <= 0) {
                    alert("Please Select Colour Name....!");
                    document.getElementById("CPH_Form_DDColor").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TdColorShade.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDColorShade').options[document.getElementById('CPH_Form_DDColorShade').selectedIndex].value <= 0) {
                    alert("Please Select Shade Color Name....!");
                    document.getElementById("CPH_Form_DDColorShade").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TdShape.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDShape').options[document.getElementById('CPH_Form_DDShape').selectedIndex].value <= 0) {
                    alert("Please Select Shape Name....!");
                    document.getElementById("CPH_Form_DDShape").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TdSize.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDSize').options[document.getElementById('CPH_Form_DDSize').selectedIndex].value <= 0) {
                    alert("Please Select Size Name....!");
                    document.getElementById("CPH_Form_DDSize").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TdSize.ClientID %>")) {
                if (document.getElementById('CPH_Form_DDSize').options[document.getElementById('CPH_Form_DDSize').selectedIndex].value <= 0) {
                    alert("Please Select Size Name....!");
                    document.getElementById("CPH_Form_DDSize").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_TxtQty').value == "" || document.getElementById('CPH_Form_TxtQty').value == "0") {
                alert("Please Fill Qty....!");
                document.getElementById("CPH_Form_TxtQty").focus();
                return false;
            }
            else {
                return confirm('Do You Want To Save?')
            }

        }
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                    </td>
                    <td class="tdstyle">
                        <asp:CheckBox ID="ChkEditOrder" runat="server" Text="EDIT ORDER" AutoPostBack="True"
                            OnCheckedChanged="ChkEditOrder_CheckedChanged" CssClass="checkboxbold" />
                    </td>
                    <td id="TDForOrderWise" runat="server" align="right" colspan="4" class="tdstyle">
                        <asp:CheckBox ID="ChKForOrder" runat="server" Text="Check For OrderWise" Font-Bold="true"
                            OnCheckedChanged="ChKForOrder_CheckedChanged" AutoPostBack="true" CssClass="checkboxbold" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td class="tdstyle">
                        <span class="labelbold">CompanyName</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DDCompanyName"
                            ErrorMessage="please fill Company......." ForeColor="Red" SetFocusOnError="true"
                            ValidationGroup="f1">*</asp:RequiredFieldValidator>
                        <br />
                        <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" runat="server" Width="150px"
                            OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDCompanyName"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td id="tdindno" runat="server" visible="false">
                        <span class="labelbold">Indent No</span>
                        <br />
                        <asp:DropDownList CssClass="dropdown" ID="ddindentno" runat="server" OnSelectedIndexChanged="ddindentno_SelectedIndexChanged"
                            AutoPostBack="True" Width="150px">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddindentno"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td class="tdstyle">
                        <span class="labelbold">Department</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="DDCompanyName"
                            ErrorMessage="please fill Company......." ForeColor="Red" SetFocusOnError="true"
                            ValidationGroup="f1">*</asp:RequiredFieldValidator>
                        <br />
                        <asp:DropDownList CssClass="dropdown" ID="DDDepartment" runat="server" OnSelectedIndexChanged="DDDepartment_SelectedIndexChanged"
                            AutoPostBack="True" Width="200px">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDDepartment"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="lblvend" runat="server" Text="Vendor Name" CssClass="labelbold"></asp:Label>
                        <br />
                        <asp:DropDownList CssClass="dropdown" ID="DDPartyName" runat="server" OnSelectedIndexChanged="DDPartyName_SelectedIndexChanged"
                            AutoPostBack="True" Width="200px">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="DDPartyName"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td class="tdstyle" id="tdemp" runat="server">
                        <span class="labelbold">Employee Name</span>
                        <br />
                        <asp:DropDownList CssClass="dropdown" ID="DDEmp" runat="server" Width="200px">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDEmp"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td class="tdstyle" id="tdcust" runat="server" visible="false">
                        <asp:Label ID="lblcustcode" runat="server" Text="Customer Code"></asp:Label>
                        <br />
                        <asp:DropDownList CssClass="dropdown" ID="ddcustomer" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddcustomer_SelectedIndexChanged" Width="150px">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="ddcustomer"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td class="tdstyle" id="tdorder" runat="server" visible="false">
                        <span class="labelbold">Order No</span>
                        <br />
                        <asp:DropDownList CssClass="dropdown" ID="ddorder" runat="server" OnSelectedIndexChanged="ddorder_SelectedIndexChanged"
                            AutoPostBack="True" Width="150px">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="ddorder"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td class="tdstyle">
                        <span class="labelbold">IndentNo</span>
                        <br />
                        <asp:TextBox ID="TxtIndentNo" CssClass="textb" Enabled="false" runat="server" Width="75px"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <span class="labelbold">Date</span>
                        <br />
                        <asp:TextBox ID="TxtDate" CssClass="textb" runat="server" Width="80px"></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                            TargetControlID="TxtDate">
                        </cc1:CalendarExtender>
                    </td>
                    <td id="itmcod" runat="server" class="tdstyle">
                        <span class="labelbold">ProductCode</span>
                        <br />
                        <asp:TextBox ID="TxtItemCode" CssClass="textb" runat="server" OnTextChanged="TxtItemCode_TextChanged"
                            AutoPostBack="True" Width="80px"></asp:TextBox>
                        <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                            Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtItemCode"
                            UseContextKey="True">
                        </cc1:AutoCompleteExtender>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="LblCategory" class="tdstyle" runat="server" AutoPostBack="true" Text=""
                            CssClass="labelbold"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="DDCategory"
                            ErrorMessage="please Select Category" ForeColor="Red" SetFocusOnError="true"
                            ValidationGroup="f1">*</asp:RequiredFieldValidator>
                        <br />
                        <asp:DropDownList CssClass="dropdown" ID="DDCategory" Width="150" AutoPostBack="True"
                            runat="server" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="DDCategory"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td>
                        <asp:Label ID="LblItemName" class="tdstyle" runat="server" AutoPostBack="true" Text="Label"
                            CssClass="labelbold"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="DDItem"
                            ErrorMessage="please Select Item" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                        <br />
                        <asp:DropDownList CssClass="dropdown" ID="DDItem" Width="150" AutoPostBack="True"
                            runat="server" OnSelectedIndexChanged="DDItem_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="DDItem"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td id="TdQuality" runat="server" visible="false">
                        <asp:Label ID="LblQuality" class="tdstyle" runat="server" Text="Label" CssClass="labelbold"></asp:Label><br />
                        <asp:DropDownList CssClass="dropdown" ID="DDQuality" Width="150" runat="server">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="DDQuality"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                </tr>
                <tr>
                    <td id="TdDesign" runat="server" visible="false">
                        <asp:Label ID="LblDesign" class="tdstyle" runat="server" Text="Label" CssClass="labelbold"></asp:Label><br />
                        <asp:DropDownList CssClass="dropdown" Width="150" ID="DDDesign" runat="server">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender11" runat="server" TargetControlID="DDDesign"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td id="TdColor" runat="server" visible="false">
                        <asp:Label ID="LblColor" class="tdstyle" runat="server" Text="Label" CssClass="labelbold"></asp:Label><br />
                        <asp:DropDownList CssClass="dropdown" Width="150" ID="DDColor" runat="server">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender12" runat="server" TargetControlID="DDColor"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td id="TdColorShade" runat="server" visible="false">
                        <asp:Label ID="LblColorShade" class="tdstyle" runat="server" Text="Label" CssClass="labelbold"></asp:Label><br />
                        <asp:DropDownList CssClass="dropdown" Width="150" ID="DDColorShade" runat="server">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender13" runat="server" TargetControlID="DDColorShade"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td id="TdShape" runat="server" visible="false">
                        <asp:Label ID="LblShape" runat="server" class="tdstyle" Text="Label" CssClass="labelbold"></asp:Label><br />
                        <asp:DropDownList CssClass="dropdown" Width="150" ID="DDShape" runat="server" OnSelectedIndexChanged="DDShape_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender14" runat="server" TargetControlID="DDShape"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td id="TdSize" runat="server" visible="false">
                        <asp:Label ID="LblSize" runat="server" class="tdstyle" Text="Label" CssClass="labelbold"></asp:Label>
                        <asp:DropDownList CssClass="dropdown" Width="100" ID="DDsizetype" runat="server"
                            AutoPostBack="True" OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged">
                            <%-- <asp:ListItem Value="0" Selected="True">Ft</asp:ListItem>
                            <asp:ListItem Value="1">MTR</asp:ListItem>
                            <asp:ListItem Value="2">Inch</asp:ListItem>--%>
                        </asp:DropDownList>
                        <asp:Button CssClass="buttonsmalls" ID="btnaddsize" runat="server" Text="ADD" OnClientClick="return AddSize()"
                            Height="22px" TabIndex="33" />
                        <asp:Button CssClass="buttonnorm" ID="refreshsize2" runat="server" Text="" BorderWidth="0px"
                            Height="1px" Width="1px" BackColor="White" BorderColor="White" BorderStyle="None"
                            ForeColor="White" OnClick="refreshsize_Click" />
                        <br />
                        <asp:DropDownList CssClass="dropdown" Width="150" ID="DDSize" runat="server" AutoPostBack="True">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender15" runat="server" TargetControlID="DDSize"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td class="tdstyle">
                        <span class="labelbold">Qty</span>
                        <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtQty"
                            ErrorMessage="please fill float value in Qty.........." ForeColor="Red" SetFocusOnError="true"
                            ValidationGroup="f1">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="RegularExpressionValidator"
                            ForeColor="Red" ValidationGroup="f1" ControlToValidate="TxtQty" ValidationExpression="^\d*[0-9](\.\d*[0-9])?$"></asp:RegularExpressionValidator>--%>
                        <br />
                        <asp:TextBox ID="TxtQty" runat="server" onkeypress="return isNumber(event);" AutoPostBack="true"
                            Width="75px" CssClass="textb"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <span class="labelbold">Unit</span>
                        <br />
                        <asp:DropDownList CssClass="dropdown" Width="110" ID="DDUnit" runat="server">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender16" runat="server" TargetControlID="DDUnit"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td class="tdstyle">
                        <span class="labelbold">Req Date</span>
                        <br />
                        <asp:TextBox ID="txtreqdate" CssClass="textb" runat="server" Width="80px"></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                            TargetControlID="txtreqdate">
                        </cc1:CalendarExtender>
                    </td>
                    <td class="tdstyle">
                        <span class="labelbold">Rate</span>
                        <br />
                        <asp:TextBox ID="txtrate" runat="server" onkeypress="return isNumber(event);" AutoPostBack="true"
                            Width="80px" CssClass="textb"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td class="tdstyle">
                        <span class="labelbold">Order Remark</span>
                        <br />
                        <asp:TextBox ID="txtremarks" runat="server" Width="300px" CssClass="textboxremark"
                            TextMode="MultiLine"></asp:TextBox>
                    </td>
                    <td class="tdstyle">
                        <span class="labelbold">Item Remark</span>
                        <br />
                        <asp:TextBox ID="txtitemremark" runat="server" Width="300px" CssClass="textboxremark"
                            TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:DropDownList ID="DDPreviewType" runat="server" CssClass="dropdown" AutoPostBack="True">
                            <asp:ListItem Text="Report Without Image" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Report With Image" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;<asp:Button ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();"
                            CssClass="buttonnorm" Width="70px" />
                        <asp:Button ID="BtnSave" runat="server" Text="Save" OnClientClick="return Validation();"
                            OnClick="BtnSave_Click" ValidationGroup="f1" CssClass="buttonnorm" Width="70px" />
                        <asp:Button ID="BtnPreview" runat="server" Text="Preview" OnClick="BtnPreview_Click"
                            Visible="true" CssClass="buttonnorm preview_width" Width="70px" />
                        <asp:Button ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                            OnClick="BtnClose_Click" CssClass="buttonnorm" Width="70px" />
                    </td>
                    <td id="TDGridShow" valign="top" colspan="3" align="center" visible="false" runat="server">
                        <div style="height: 150px; width: 80%; overflow: auto;">
                            <asp:GridView ID="DGShowConsumption" runat="server" AutoGenerateColumns="False" CssClass="grid-views"
                                OnSelectedIndexChanged="DGShowConsumption_SelectedIndexChanged" DataKeyNames="finishedid"
                                OnRowDataBound="DGShowConsumption_RowDataBound">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <Columns>
                                    <asp:BoundField DataField="DESCRIPTION" HeaderText="Description">
                                        <HeaderStyle Width="250px" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Qty" HeaderText="Qty"></asp:BoundField>
                                    <asp:TemplateField HeaderText="Ordered Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="lblfinishedid" runat="server" Text='<%# Bind("finishedid") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblcategoryid" runat="server" Text='<%# Bind("category_id") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblitem_id" runat="server" Text='<%# Bind("item_id") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblQualityid" runat="server" Text='<%# Bind("Qualityid") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblColorid" runat="server" Text='<%# Bind("Colorid") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lbldesignid" runat="server" Text='<%# Bind("designid") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblshapeid" runat="server" Text='<%# Bind("shapeid") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblshadecolorid" runat="server" Text='<%# Bind("shadecolorid") %>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblsizeid" runat="server" Text='<%# Bind("sizeid") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblsizeflag" runat="server" Text='<%# Bind("Isizeflag") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblqty" runat="server" Text='<%# Bind("Qty") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblorderedqty" runat="server" ForeColor="Black" Visible="true" Text='<%# getgiven(DataBinder.Eval(Container.DataItem, "finishedid").ToString()) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Lblmessage" runat="server" ForeColor="Red" Text=""></asp:Label>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="f1" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td colspan="5">
                        <div id="gride" runat="server" style="max-height: 400px; overflow: auto">
                            <asp:GridView ID="DGPIndentDetail" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                                CellPadding="4" CssClass="grid-view" DataKeyNames="PIndentDetailId" OnPageIndexChanging="DGPIndentDetail_PageIndexChanging"
                                OnRowDataBound="DGPIndentDetail_RowDataBound" OnRowDeleting="DGPIndentDetail_RowDeleting"
                                OnSelectedIndexChanged="DGPIndentDetail_SelectedIndexChanged" PageSize="10">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <PagerStyle CssClass="PagerStyle" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:BoundField DataField="PIndentDetailId" HeaderText="PIndentDetailId" Visible="false" />
                                    <asp:BoundField DataField="PIndentNo" HeaderText="PIndentNo" />
                                    <asp:BoundField DataField="DepartmentName" HeaderText="Department" />
                                    <asp:BoundField DataField="PartyName" HeaderText="PartyName" />
                                    <asp:BoundField DataField="ItemDescription" HeaderText="Item Description" />
                                    <asp:BoundField DataField="Qty" HeaderText="Qty" />
                                    <asp:BoundField DataField="UnitName" HeaderText="Unit" />
                                    <asp:BoundField DataField="Remark" HeaderText="Remark" />
                                    <asp:BoundField DataField="Rate" HeaderText="Rate" />
                                    <asp:TemplateField HeaderText="Net Amount" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblpindentid" runat="server" Text='<%# Bind("PIndentId") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblflagsize" runat="server" Text='<%# Bind("flagsize") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblfinishedid" runat="server" Text='<%# Bind("finishedid") %>' Visible="false"></asp:Label>
                                            <span class="labelbold">orderremark</span>
                                            <asp:Label ID="lblPIndentDetailId" runat="server" Text='<%# Bind("PIndentDetailId") %>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblitemremark" runat="server" Text='<%# Bind("Remark") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblqty" runat="server" Text='<%# Bind("Qty") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblrate" runat="server" Text='<%# Bind("Rate") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="BtnAddImage" runat="server" Text="Add Image" CommandArgument="BtnAddImage"
                                                OnClick="BtnAddImage_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="Btndel" runat="server" CausesValidation="False" CommandName="Delete"
                                                Text="Delete"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
