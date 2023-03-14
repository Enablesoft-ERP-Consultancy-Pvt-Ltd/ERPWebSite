<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="FrmProductionTNAPLAN.aspx.cs" Inherits="Masters_Order_FrmProductionTNAPLAN" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <%@ register src="~/UserControls/ucmenu.ascx" tagname="ucmenu" tagprefix="uc2" %>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function validate() {
            if (document.getElementById("<%=DDCompanyName.ClientID %>").value <= "0") {
                alert("Pls Select Company Name");
                document.getElementById("<%=DDCompanyName.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=DDCustomerCode.ClientID %>").value <= "0") {
                alert("Pls Select Customer Name");
                document.getElementById("<%=DDCustomerCode.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=ddorderno.ClientID %>").value <= "0") {
                alert("Pls Select Order No");
                document.getElementById("<%=ddorderno.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=Txtyarndate.ClientID %>").value == "") {
                alert("Pls Insert Yarn Approval Date");
                document.getElementById("<%=Txtyarndate.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtpeice.ClientID %>").value == "") {
                alert("Pls Insert Peice Approval Date");
                document.getElementById("<%=txtpeice.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtgoods.ClientID %>").value == "") {
                alert("Pls Insert Goods Approval Date");
                document.getElementById("<%=txtgoods.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtpreprod.ClientID %>").value == "") {
                alert("Pls Insert Pre Production Approval Date");
                document.getElementById("<%=txtpreprod.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtpacking.ClientID %>").value == "") {
                alert("Pls Insert Pre Packing Approval Date");
                document.getElementById("<%=txtpacking.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtlabel.ClientID %>").value == "") {
                alert("Pls Insert Lable Art Work Approval Date");
                document.getElementById("<%=txtlabel.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtlabinhouse.ClientID %>").value == "") {
                alert("Pls Insert Lable In House Approval Date");
                document.getElementById("<%=txtlabinhouse.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txttesting.ClientID %>").value == "") {
                alert("Pls Insert Lable In Testing Approval Date");
                document.getElementById("<%=txttesting.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=chkprocessdate.ClientID %>").checked == true) {
                var isValid = false;
                var gridView = document.getElementById("<%=grdprocessdate.ClientID %>");
                for (var i = 1; i < gridView.rows.length; i++) {
                    var inputs = gridView.rows[i].getElementsByTagName('input');
                    if (inputs != null) {
                        if (inputs[0].type == "checkbox") {
                            if (inputs[0].checked) {
                                isValid = true;
                            }
                        }
                    }
                }
                if (isValid == false) {
                    alert("Please select atleast one Lable");
                    return false;
                }
            }
            {
                var isValid = false;
                var i = 0;
                var gridView = document.getElementById("<%=DGOrderDetail.ClientID %>");
                for (var i = 1; i < gridView.rows.length; i++) {
                    var inputs = gridView.rows[i].getElementsByTagName('input');
                    if (inputs != null) {
                        if (inputs[0].type == "checkbox") {
                            if (inputs[0].checked) {
                                isValid = true;
                                return confirm('Do You Want To Save?')
                            }
                        }
                    }
                }
                alert("Please select atleast one checkbox");
                return false;
            }
        }
        function checkcheck() {
            var isValid = false;
            var j = 0;
            var gridView = document.getElementById("<%=DGOrderDetail.ClientID %>");
            for (var i = 1; i < gridView.rows.length; i++) {
                var inputs = gridView.rows[i].getElementsByTagName('input');
                if (inputs != null) {
                    if (inputs[0].type == "checkbox") {
                        if (inputs[0].checked) {
                            j = j + 1;
                            if (j > 1) {
                                alert("Please Select Only One Item");
                                inputs[0].checked = false;
                                return false;
                            }
                        }
                    }
                }
            }
        }
    </script>
    <table>
        <tr>
            <td class="tdstyle">
                <asp:CheckBox ID="ChkEditOrder" runat="server" Text="Edit Order" AutoPostBack="True"
                    OnCheckedChanged="ChkEditOrder_CheckedChanged" CssClass="checkboxbold" />
            </td>
        </tr>
        <tr>
            <td class="tdstyle">
                <asp:Label ID="lbl" Text="COMPANY NAME" runat="server" CssClass="labelbold" />
                <b style="color: Red">*</b>
            </td>
            <td class="tdstyle">
                <asp:Label ID="Label2" Text="  CUST CODE" runat="server" CssClass="labelbold" />
                <b style="color: Red">*</b>
            </td>
            <td class="tdstyle">
                <asp:Label ID="Label3" Text="CUST.ORD. NO" runat="server" CssClass="labelbold" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" runat="server" Width="200px">
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList CssClass="dropdown" ID="DDCustomerCode" runat="server" AutoPostBack="True"
                    Width="250px" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList CssClass="dropdown" ID="ddorderno" runat="server" Width="90px"
                    AutoPostBack="True" OnSelectedIndexChanged="ddorderno_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr align="center" id="trdrig" runat="server" style="display: none">
            <td align="center" colspan="3">
                <div style="width: 100%; height: 150px; overflow: scroll">
                    <asp:GridView ID="DGOrderDetail" Width="100%" runat="server" DataKeyNames="Sr_No"
                        AutoGenerateColumns="False" CssClass="grid-views" OnRowCreated="DGOrderDetail_RowCreated"
                        OnRowDataBound="DGOrderDetail_RowDataBound">
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts" />
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:CheckBox ID="Chkbox" runat="server" onclick="return checkcheck()" OnCheckedChanged="Chkbox_checked" />
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Sr_No" HeaderText="Sr_No" />
                            <asp:BoundField DataField="CATEGORY" HeaderText="CATEGORY" />
                            <asp:BoundField DataField="ITEMNAME" HeaderText="ITEMNAME" />
                            <asp:BoundField DataField="Qty" HeaderText="Qty" />
                            <asp:BoundField DataField="Area" HeaderText="Area" />
                        </Columns>
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </div>
            </td>
        </tr>
        <tr>
            <td valign="top" runat="server">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" Text=" YARN COLOR APPROVAL" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="Txtyarndate" runat="server" CssClass="textb" Width="90px" TabIndex="39"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="Txtyarndate">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label5" Text=" PIECE APPROVAL" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtpeice" runat="server" CssClass="textb" Width="90px" TabIndex="39"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtpeice">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label6" Text=" GOOD RECEIVAL" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtgoods" runat="server" CssClass="textb" Width="90px" TabIndex="39"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtgoods">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label7" Text=" PRE PRODUCTION APPROVAL" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtpreprod" runat="server" CssClass="textb" Width="90px" TabIndex="39"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtpreprod">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label8" Text="PACKAGING APPROVAL" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtpacking" runat="server" CssClass="textb" Width="90px" TabIndex="39"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender5" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtpacking">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label9" Text=" LABEL ARTWORK APPROVAL" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtlabel" runat="server" CssClass="textb" Width="90px" TabIndex="39"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender6" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtlabel">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label10" Text="LABELLING INHOUSE" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtlabinhouse" runat="server" CssClass="textb" Width="90px" TabIndex="39"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender7" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtlabinhouse">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label11" Text=" TESTING" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txttesting" runat="server" CssClass="textb" Width="90px" TabIndex="39"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender8" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txttesting">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                </table>
            </td>
            <td id="tdprocess" runat="server" visible="false" colspan="2">
                <table>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkprocessdate" Text="Process Dates" runat="server" AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="width: 350px; height: 150px; overflow: scroll">
                                <asp:GridView ID="grdprocessdate" Width="300px" runat="server" DataKeyNames="Sr_No"
                                    AutoGenerateColumns="False" CssClass="grid-views">
                                    <RowStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Chkbox" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="80px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="process" HeaderText="Process Name" />
                                        <asp:TemplateField HeaderText="Approval Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblprocessid" runat="server" Visible="false" Text='<%# bind("Sr_No") %>'>' ></asp:Label>
                                                <asp:TextBox ID="Txtdate" runat="server" CssClass="textb" Width="90px" TabIndex="39"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                                    TargetControlID="Txtdate">
                                                </asp:CalendarExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Approved Date">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TxtApproveddate" runat="server" CssClass="textb" Width="90px" TabIndex="39"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy"
                                                    TargetControlID="TxtApproveddate">
                                                </asp:CalendarExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
            <td id="Td1" valign="top" runat="server">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label12" Text=" YARN COLOR APPROVED" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TXTYARAPPROVED" runat="server" CssClass="textb" Width="90px" TabIndex="39"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender9" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TXTYARAPPROVED">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label13" Text="PIECE APPROVED" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TXTPIECEAPPROVED" runat="server" CssClass="textb" Width="90px" TabIndex="39"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender10" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TXTPIECEAPPROVED">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label14" Text=" GOOD RECEIVED" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TXTGOODRECEIVED" runat="server" CssClass="textb" Width="90px" TabIndex="39"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender11" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TXTGOODRECEIVED">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label15" Text=" PRE PRODUCTION APPROVED" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TXTPREPRODAPPROVED" runat="server" CssClass="textb" Width="90px"
                                TabIndex="39"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender12" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TXTPREPRODAPPROVED">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label16" Text="PACKAGING APPROVED" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TXTPACKINGAPPROVED" runat="server" CssClass="textb" Width="90px"
                                TabIndex="39"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender13" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtpacking">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label17" Text=" LABEL ARTWORK APPROVED" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TXTLABELAPPROVED" runat="server" CssClass="textb" Width="90px" TabIndex="39"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender14" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TXTLABELAPPROVED">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label18" Text=" LABELLING INHOUSE APPROVED" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TXTLABELINHOUSEAPPROVED" runat="server" CssClass="textb" Width="90px"
                                TabIndex="39"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender15" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TXTLABELINHOUSEAPPROVED">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label19" Text="TESTED" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TXTTESTED" runat="server" CssClass="textb" Width="90px" TabIndex="39"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender16" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TXTTESTED">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label20" Text="Inline Inspection Date " runat="server" CssClass="labelbold" />
                <asp:TextBox ID="txtinline" runat="server" CssClass="textb" Width="90px" TabIndex="39"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender17" runat="server" Format="dd-MMM-yyyy"
                    TargetControlID="txtinline">
                </asp:CalendarExtender>
            </td>
            <td>
                <asp:Label ID="Label21" Text="Midline Inspection Date" runat="server" CssClass="labelbold" />
                <asp:TextBox ID="txtmidline" runat="server" CssClass="textb" Width="90px" TabIndex="39"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender18" runat="server" Format="dd-MMM-yyyy"
                    TargetControlID="txtmidline">
                </asp:CalendarExtender>
            </td>
            <td>
                <asp:Label ID="Label22" Text="Finale Inspection Date" runat="server" CssClass="labelbold" />
                <asp:TextBox ID="txtfinal" runat="server" CssClass="textb" Width="90px" TabIndex="39"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender19" runat="server" Format="dd-MMM-yyyy"
                    TargetControlID="txtfinal">
                </asp:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Button CssClass="buttonnorm" ID="BtnSave1" Text="Save" runat="server" OnClientClick="return validate();"
                    OnClick="BtnSave1_Click" />
                &nbsp;<asp:Button CssClass="buttonnorm preview_width" ID="btnreport" Text="Preview"
                    runat="server" OnClick="btnreport_Click" />
            </td>
            <%--<td>
                <asp:Button ID="refreshitem" runat="server" Height="0px" OnClick="refreshitem_Click"
                    Text="." Width="0px" BackColor="White" BorderColor="White" BorderWidth="0px"
                    ForeColor="White" />
            </td>--%>
        </tr>
        <tr id="trgrid" runat="server" visible="false">
            <td class="style2" colspan="4">
                <asp:Label ID="Lblerr" runat="server" ForeColor="Red"></asp:Label>
                <div style="width: 100%; height: 375px; overflow: scroll">
                    <asp:GridView ID="DgExcel" runat="server" Width="100%" CssClass="grid-views">
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts" />
                        <RowStyle CssClass="gvrow" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </div>
            </td>
        </tr>
        <tr id="tr1" runat="server" visible="false">
            <td class="style2" colspan="4">
                <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                <div style="width: 100%; height: 375px; overflow: scroll">
                    <asp:GridView ID="DgExcel1" runat="server" Width="100%" CssClass="grid-views">
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts" />
                        <RowStyle CssClass="gvrow" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                    </asp:GridView>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
