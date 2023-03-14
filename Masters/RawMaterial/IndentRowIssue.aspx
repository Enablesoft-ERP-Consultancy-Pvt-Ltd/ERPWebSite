<%@ Page Title="Indent Raw Issue" Language="C#" AutoEventWireup="true" CodeFile="IndentRowIssue.aspx.cs"
    MasterPageFile="~/ERPmaster.master" EnableEventValidation="false" Inherits="Masters_process_PRI" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/FixFocus2.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Preview() {
            window.open('../../ReportViewer.aspx', '');
        }
        function ClickNew() {
            window.location.href = "IndentRowIssue.aspx";
        }
        function OrderDetail() {
            var e = document.getElementById("CPH_Form_ddempname");
            var varcode = e.options[e.selectedIndex].value;
            if (varcode > 0) {
                window.open('../order/frmorderdetail.aspx?Vendor=' + varcode + '&Type=JW', '', 'width=950px,Height=500px');
            }
            else {
                alert("Plz select Vendor name");
            }
        }
        function Validation() {
            if (document.getElementById("<%=txtissqty.ClientID %>").value == "" || document.getElementById("<%=txtissqty.ClientID %>").value == "0") {
                alert("Quantity Cann't be blank Or Zero");
                document.getElementById("<%=txtissqty.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=TDMoisture.ClientID %>")) {
                if (document.getElementById("<%=TxtMoisture.ClientID %>").value == "") {
                    alert("Moisture Cannot Be Blank");
                    document.getElementById("<%=TxtMoisture.ClientID %>").focus()
                    return false;
                }
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
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label Text="Master Details" CssClass="labelbold" ForeColor="Red" runat="server" />
                    </legend>
                    <asp:Panel ID="pnl1" runat="server">
                        <table>
                            <tr>
                                <td id="TDForOrderWise" runat="server" align="right" colspan="5" class="tdstyle">
                                    <asp:CheckBox ID="ChKForOrder" runat="server" Text="Check For OrderWise" OnCheckedChanged="ChKForOrder_CheckedChanged"
                                        AutoPostBack="true" CssClass="checkboxbold" />
                                </td>
                            </tr>
                            <tr id="Tr1" runat="server">
                                <td id="Td1" colspan="2" class="tdstyle">
                                    <asp:Label ID="lbl" Text="Company Name" runat="server" CssClass="labelbold" />
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
                                <td id="Td2" class="tdstyle">
                                    <asp:Label ID="Label3" Text=" Process Name" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:DropDownList ID="ddProcessName" runat="server" Width="150px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddProcessName_SelectedIndexChanged" CssClass="dropdown">
                                    </asp:DropDownList>
                                </td>
                                <td id="Td4" class="tdstyle">
                                    <asp:Label ID="Label4" Text="  Party Name" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:DropDownList ID="ddempname" runat="server" Width="150px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddempname_SelectedIndexChanged" CssClass="dropdown">
                                    </asp:DropDownList>
                                </td>
                                <td id="Td3" align="center" class="tdstyle">
                                    <asp:Label ID="lblindent" runat="server" Text="Indent No." CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="ddindentno" runat="server" Width="150px" CssClass="dropdown"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddindentno_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td id="Td5" align="center" class="tdstyle">
                                    <asp:Label ID="Label5" Text=" Issue Date" runat="server" CssClass="labelbold" />
                                    <b style="color: Red">&nbsp; *</b><br />
                                    <asp:TextBox ID="txtdate" runat="server" Width="100px" AutoPostBack="True" CssClass="textb"
                                        OnTextChanged="txtdate_TextChanged" BackColor="Beige"></asp:TextBox>
                                    <asp:RequiredFieldValidator SetFocusOnError="true" ID="RequiredFieldValidator1" runat="server"
                                        ErrorMessage="please Enter Date" ControlToValidate="txtdate" ValidationGroup="f1"
                                        ForeColor="Red">*</asp:RequiredFieldValidator>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                        TargetControlID="txtdate">
                                    </asp:CalendarExtender>
                                </td>
                                <td id="Td6" class="tdstyle">
                                    <asp:Label ID="Label6" Text="Challan No." runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="txtchalanno" Width="100px" runat="server" CssClass="textb"></asp:TextBox>
                                </td>
                                <td id="procode" runat="server" visible="false" class="tdstyle">
                                    <asp:Label ID="Label7" Text=" Product Code" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="TxtProdCode" runat="server" OnTextChanged="TxtProdCode_TextChanged"
                                        AutoPostBack="True" Width="100px"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                                        Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtProdCode"
                                        UseContextKey="True">
                                    </cc1:AutoCompleteExtender>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label Text="Item Details" CssClass="labelbold" ForeColor="Red" runat="server" />
                    </legend>
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
                                <asp:DropDownList ID="ddshape" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="sz" runat="server" visible="false">
                                <asp:Label ID="lblSize" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                <%--<asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox1_CheckedChanged" />Check
                            for Mtr--%>
                                <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Selected="True">Ft</asp:ListItem>
                                    <asp:ListItem Value="1">MTR</asp:ListItem>
                                    <asp:ListItem Value="2">Inch</asp:ListItem>
                                </asp:DropDownList>
                                <br />
                                <asp:DropDownList ID="ddsize" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="shd" runat="server" visible="false">
                                <asp:Label ID="lblshadecolor" runat="server" Text="ShadeColor" CssClass="labelbold"></asp:Label>
                                &nbsp;<br />
                                <asp:DropDownList ID="ddlshade" runat="server" Width="150px" AutoPostBack="true"
                                    CssClass="dropdown" OnSelectedIndexChanged="ddlshade_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label Text="..." CssClass="labelbold" ForeColor="Red" runat="server" />
                    </legend>
                    <table>
                        <tr>
                            <td id="TDGodown" runat="server" class="tdstyle">
                                <asp:Label ID="Label8" Text=" Godown Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="ddgodown" runat="server" Width="150px" CssClass="dropdown"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddgodown_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDIndentQty" runat="server" class="tdstyle">
                                <asp:Label ID="Label9" Text="Issue Qty" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtissue" runat="server" Enabled="false" CssClass="textb" Width="100px"></asp:TextBox>
                            </td>
                            <td id="TDPreIssue" runat="server" class="tdstyle">
                                <asp:Label ID="Label10" Text="   PreIssue Qty" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtpreissue" runat="server" Enabled="false" CssClass="textb" Width="100px"></asp:TextBox>
                            </td>
                            <td id="TDPending" runat="server" class="tdstyle">
                                <asp:Label ID="Label11" Text=" Pending Qty" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtpendingqty" runat="server" Enabled="false" CssClass="textb" Width="72px"></asp:TextBox>
                            </td>
                            <td id="TDLotNo" runat="server" class="tdstyle">
                                <asp:Label ID="Label12" Text="LotNo." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="ddlotno" runat="server" AutoPostBack="True" Width="150px" CssClass="dropdown"
                                    OnSelectedIndexChanged="ddlotno_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDBinNo" runat="server" visible="false">
                                <asp:Label ID="Label18" Text="Bin No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDBinNo" runat="server" Width="150px" CssClass="dropdown" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDBinNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDTagNo" runat="server" visible="false">
                                <asp:Label ID="Label17" Text="Tag No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDTagNo" runat="server" AutoPostBack="True" Width="150px" CssClass="dropdown"
                                    OnSelectedIndexChanged="DDTagNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDStock" runat="server" class="tdstyle">
                                <asp:Label ID="Label13" Text=" Stock" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtstock" runat="server" Enabled="false" CssClass="textb" Width="69px"></asp:TextBox>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label14" Text=" Iss Qty" runat="server" CssClass="labelbold" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtissqty"
                                    ErrorMessage="please Enter qty" ForeColor="Red" SetFocusOnError="true" ValidationGroup="f1">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:TextBox ID="txtissqty" runat="server" CssClass="textb" OnTextChanged="txtissqty_TextChanged"
                                    onkeypress="return isNumber(event);" BackColor="Beige" Width="65px" AutoPostBack="True"></asp:TextBox>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label15" Text=" Unit" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="ddlunit" runat="server" AutoPostBack="true" CssClass="dropdown"
                                    Width="100px">
                                </asp:DropDownList>
                            </td>
                            <%--</caption>--%>
                        </tr>
                        <tr>
                            <td id="TDManualRate" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="Label19" Text=" Manual Rate" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="TxtManualRate" runat="server" CssClass="textb" onkeypress="return isNumber(event);"
                                    BackColor="Beige" Width="65px"></asp:TextBox>
                            </td>
                            <td id="TDGSTType" runat="server" class="tdstyle" visible="false">
                                <asp:Label ID="Label59" runat="server" Text="GST Type" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" Width="100px" ID="DDGSType" runat="server"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDGSType_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Selected="True">---Select----</asp:ListItem>
                                    <asp:ListItem Value="1">CGST/SGST</asp:ListItem>
                                    <asp:ListItem Value="2">IGST</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td id="TDCGST" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="Label20" Text=" CGST%" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:Label ID="lblCGST" runat="server" CssClass="labelbold"></asp:Label>
                            </td>
                            <td id="TDSGST" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="Label21" Text=" SGST%" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:Label ID="lblSGST" runat="server" CssClass="labelbold"></asp:Label>
                            </td>
                            <td id="TDIGST" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="Label22" Text=" IGST%" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:Label ID="lblIGST" runat="server" CssClass="labelbold"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <table width="100%">
                    <tr>                            
                    <td align="left" class="tdstyle" id="TDMoisture" runat="server" visible="false">
                                <asp:Label ID="Label48" runat="server" Text="Moisture" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtMoisture" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                            </td>
                        <td style="width: 60%" id="TDtxtremarks" runat="server">
                            <asp:Label ID="Label16" Text="Remark" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtremarks" runat="server" Width="90%" CssClass="textb" TextMode="MultiLine"
                                Height="50px"></asp:TextBox>
                        </td>
                        <td style="width: 40%; text-align: right">
                            <asp:CheckBox ID="ChkForGSTReport" runat="server" Text="Check For GST Report" CssClass="checkboxbold"
                                Visible="false" />
                            <asp:Button ID="btnstatus" runat="server" Text="Order Complete" Width="150px" Visible="false"
                                CssClass="buttonnorm" OnClick="btnstatus_Click" />
                            <asp:Button ID="btnnew" runat="server" Text="New" OnClientClick="return ClickNew()"
                                CssClass="buttonnorm" Width="50px" />
                            <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" OnClientClick="return Validation();"
                                CssClass="buttonnorm" Width="50px" />
                            <asp:Button ID="btnpriview" runat="server" Text="Preview" CssClass="buttonnorm" Visible="False"
                                OnClick="btnpriview_Click" Width="80px" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" OnClientClick="return CloseForm(); "
                                CssClass="buttonnorm" OnClick="btnclose_Click" Width="50px" />
                            <asp:Button ID="Btnorder" runat="server" Text="Work Load" Visible="false" CssClass="buttonnorm "
                                OnClientClick="return OrderDetail()" Width="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="Label1" runat="server" CssClass="labelbold" Text="Label" ForeColor="Red"
                                Visible="false" Font-Size="Small"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="True" Font-Size="Small"
                                CssClass="labelbold"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%">
                    <tr>
                        <td>
                            <asp:Label ID="lbldate" runat="server" ForeColor="Red" Text="Plz enter the date"
                                Visible="false"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="ProdCode doesnot exist"
                                Visible="false" Width="148px"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Lblfinished" runat="server" ForeColor="Red" Text="Allready Issued Data not save...."
                                Visible="False" Width="124px"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblqty" runat="server" ForeColor="Red" Text="Please Check Qty........."
                                Visible="False" Width="124px"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 20%" id="Td7" runat="server" valign="top">
                            <div id="Div1" runat="server" style="width: 95%; max-height: 300px; overflow: auto">
                                <asp:GridView ID="dgorder" AutoGenerateColumns="false" runat="server" CssClass="grid-views"
                                    OnRowDataBound="dgorder_RowDataBound" Width="100%">
                                    <HeaderStyle Height="20px" CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <asp:BoundField DataField="Description" HeaderText="Order Description" />
                                        <asp:BoundField DataField="Qty" HeaderText="Indent Qty" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                        <td style="width: 20%">
                            <table width="100%">
                                <tr runat="server" id="trorder" visible="false">
                                    <td runat="server" visible="false" valign="top" id="TDInputDesc">
                                        <div style="width: 95%; max-height: 200px; overflow: auto">
                                            <asp:GridView ID="GDInputDescp" runat="server" AutoGenerateColumns="False" CssClass="grid-views"
                                                OnRowDataBound="GDInputDescp_RowDataBound" OnSelectedIndexChanged="GDInputDescp_SelectedIndexChanged"
                                                SelectedRowStyle-BackColor="Highlight" Width="100%">
                                                <HeaderStyle CssClass="gvheaders" />
                                                <AlternatingRowStyle CssClass="gvalts" />
                                                <RowStyle CssClass="gvrow" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="INPUT DESCRIPTION">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("ItemDescription") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblcategoryid" runat="server" Text='<%# Bind("CATEGORY_ID") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblitem_id" runat="server" Text='<%# Bind("ITEM_ID") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblQualityid" runat="server" Text='<%# Bind("QualityId") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblColorid" runat="server" Text='<%# Bind("ColorId") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lbldesignid" runat="server" Text='<%# Bind("designId") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblshapeid" runat="server" Text='<%# Bind("ShapeId") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblshadecolorid" runat="server" Text='<%# Bind("ShadecolorId") %>'
                                                                Visible="false"></asp:Label>
                                                            <asp:Label ID="lblsizeid" runat="server" Text='<%# Bind("SizeId") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblfinished" runat="server" Text='<%# Bind("finishedid") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblindent" runat="server" Text='<%# Bind("indentid") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblIunitid" runat="server" Text='<%# Bind("IUNITID") %>' Visible="false"></asp:Label>
                                                            <%--<asp:Label ID="lblorderdet" runat="server" Text='<%# Bind("orderdetailid") %>' Visible="false"></asp:Label>--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 60%">
                            <div style="width: 100%; max-height: 200px; overflow: auto;">
                                <asp:GridView ID="gvdetail" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvdetail_RowDataBound"
                                    DataKeyNames="PRTid" OnRowDeleting="gvdetail_RowDeleting" Width="100%">
                                    <RowStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <Columns>
                                        <asp:BoundField DataField="PRTid" HeaderText="Sr.No." Visible="False" />
                                        <asp:BoundField DataField="CATEGORY_NAME" HeaderText="Catagory">
                                            <HeaderStyle Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ITEM_NAME" HeaderText="Item Name">
                                            <HeaderStyle Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="GodownName" HeaderText="Godown Name">
                                            <HeaderStyle Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DESCRIPTION" HeaderText="Description">
                                            <HeaderStyle Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LotNo" HeaderText="LotNo" />
                                        <asp:BoundField DataField="TagNo" HeaderText="Tag/UCN No" />
                                        <asp:BoundField DataField="issuequantity" HeaderText="Issue Quantity" />
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="Del" OnClientClick="return confirm('Do You Want To Deleted Data ?')"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="30px" />
                                            <ItemStyle HorizontalAlign="Center" Width="30px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="4">
                            <div style="width: 500px; height: 200px; overflow: auto; margin-top: 20px">
                                <asp:GridView ID="DGShowConsumption" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="DGShowConsumption_SelectedIndexChanged"
                                    OnRowDataBound="DGShowConsumption_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="CATEGORY_NAME" HeaderText="Catagory">
                                            <HeaderStyle Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ITEM_NAME" HeaderText="Item Name">
                                            <HeaderStyle Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DESCRIPTION" HeaderText="Description">
                                            <HeaderStyle Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                        <asp:BoundField DataField="Qty" HeaderText="Qty" />
                                        <asp:TemplateField HeaderText="Issued Qty" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Finishedid" runat="server" Width="0px" Visible="false" Text='<%# Bind("Finishedid") %>'></asp:Label>
                                                <asp:Label ID="OrderId" runat="server" Width="0px" Visible="false" Text='<%# Bind("OrderId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="0px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Issued Qty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtIssueQty" runat="server" Enabled="false" Width="40px" Text='<%# getgiven(DataBinder.Eval(Container.DataItem, "Finishedid").ToString(),DataBinder.Eval(Container.DataItem, "OrderId").ToString()) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="45px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="BalQty" HeaderText="Bal Qty" />
                                    </Columns>
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hnorderid" runat="server" />
                            <asp:HiddenField ID="hnqty" runat="server" />
                            <asp:HiddenField ID="hnissueqty" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
