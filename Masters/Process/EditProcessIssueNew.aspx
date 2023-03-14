<%@ Page Language="C#" Title="EditProcessIssue" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="EditProcessIssueNew.aspx.cs" Inherits="Masters_ProcessIssue_ProcessIssueNew"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/Fixfocus2.js"></script>
    <script type="text/javascript">

        function SaveData() {

            var id = document.getElementById('<%=hncomp.ClientID %>').value;

            if (id != "20") {
                var answer = confirm("Do you want to Save?")

                if ((answer)) {
                    return true;
                }
                else {
                    return false;
                }

            }
            return true;
        }

        function UpdateData() {

            var id = document.getElementById('CPH_Form_hncomp').value;
            if (id != "20") {
                alert('Do you want to Update?');
                return false;
            }
        }      
    
    </script>
    <script type="text/javascript">
        //        var g_CurrentTextBox;
        //        Sys.Application.add_load(applicationLoadHandler);
        //        function applicationLoadHandler() {
        //            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
        //        }
        //        function endRequestHandler() {

        //            if (g_CurrentTextBox != null) {
        //                $get(g_CurrentTextBox).focus();
        //                $get(g_CurrentTextBox).select();
        //            }
        //        }
        //        function onTextFocus() {
        //            g_CurrentTextBox = event.srcElement.id;
        //        }
        function reloadPage() {
            window.location.href = "EditProcessIssueNew.aspx";
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
        function AddLoomDetail() {
            var answer = confirm("Do you want to ADD?")

            if (answer) {

                var a = document.getElementById('CPH_Form_hnIssueOrderId').value;
                var b = document.getElementById('CPH_Form_DDProcessName');
                var processId = b.options[b.selectedIndex].value;

                if (a == "" || a == "0") {
                    alert('Plz fill Order Detail first');
                    return false;
                }
                var left = (screen.width / 2) - (650 / 2);
                var top = (screen.height / 2) - (300 / 2);

                //window.open('FrmLoommaster.aspx?a=' + a, '', 'width=1125px,Height=200px');
                window.open('frmWeaverLoomDetail.aspx?a=' + a + '&b=' + processId + '', 'Loom Detail', 'width=650px, height=400px, top=' + top + ', left=' + left);
            }
        }
        function isnumber(event, pointflag) {
            var keycode = event.which;
            if ((keycode > 47 && keycode <= 58) || (keycode == 46 && pointflag == true)) {
                return true;
            }
            else {
                return false;
            }
        }

        function CheckBoxClick(objref) {

            var row = objref.parentNode.parentNode;
            if (objref.checked) {
                row.style.backgroundColor = "Orange";
            }
            else {
                row.style.backgroundColor = "White";
            }
        }

        function CheckAll(objref) {
            var gridview = objref.parentNode.parentNode.parentNode;
            var inputlist = gridview.getElementsByTagName("input");
            for (var i = 0; i < inputlist.length; i++) {
                var row = inputlist[i].parentNode.parentNode;
                if (inputlist[i].type == "checkbox" && objref != inputlist[i]) {
                    if (objref.checked) {

                        inputlist[i].checked = true;
                        row.style.backgroundColor = "Orange";
                    }
                    else {
                        inputlist[i].checked = false;
                        row.style.backgroundColor = "White";

                    }
                }
            }
        }
    </script>
    <div id="maindiv" style="height: 600px;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table width="70%">
                    <tr>
                        <td class="tdstyle">
                            <span class="labelbold">P.O No.</span>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">Company Name</span>
                        </td>
                        <td class="tdstyle" id="tdLProcessName" runat="server" visible="false">
                            <span class="labelbold">Process Name </span>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">Order</span> <span class="labelbold">Emp. Name</span>
                        </td>
                        <td id="chked" runat="server" style="width: 160px" class="tdstyle">
                            <asp:CheckBox ID="ChkEditOrder" runat="server" Text="Edit Order" AutoPostBack="True"
                                OnCheckedChanged="ChkEditOrder_CheckedChanged" Enabled="False" CssClass="checkboxbold" />
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">Customer Code</span>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">Customer Order No.</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="TxtOrderNo" runat="server" Width="90px" AutoPostBack="True" CssClass="textb"
                                OnTextChanged="TxtOrderNo_TextChanged"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCompanyName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="tdProcessName" runat="server" visible="false">
                            <asp:DropDownList CssClass="dropdown" ID="DDProcessName" runat="server" AutoPostBack="True"
                                Width="150px" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged1">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDEmployeeName" Width="150px" runat="server"
                                OnSelectedIndexChanged="DDEmployeeName_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDPOrderNo" runat="server" AutoPostBack="True"
                                Width="150px" Visible="False" OnSelectedIndexChanged="DDPOrderNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDCustomerCode" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDCustomerOrderNumber" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCustomerOrderNumber_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td class="tdstyle">
                            <span class="labelbold">Assign Date</span>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">Required Date</span>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">Unit</span>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">Cal Type</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="TxtAssignDate" runat="server" Width="90px" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtAssignDate">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtRequiredDate" runat="server" OnTextChanged="TxtRequiredDate_TextChanged"
                                AutoPostBack="true" Width="90px" CssClass="textb"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtRequiredDate">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDunit" runat="server" Width="100px" OnSelectedIndexChanged="DDunit_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDcaltype" runat="server" Width="100px">
                                <asp:ListItem Value="0">Area Wise</asp:ListItem>
                                <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                <asp:ListItem Value="2">W-2 X L-2</asp:ListItem>
                                <asp:ListItem Value="3">W-2</asp:ListItem>
                                <asp:ListItem Value="4">L-2</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td id="TDCHKFORCURRENTCONSUMPTION" runat="server" class="tdstyle" colspan="2">
                            <b>
                                <asp:CheckBox ID="CKHCURRENTCONSUMPTION" runat="server" Text="Current Consumption"
                                    AutoPostBack="True" CssClass="checkboxbold" />
                                <asp:CheckBox ID="CKHUPDATEWEAVERNAME" runat="server" Text="Update Weaver Name For This Order"
                                    AutoPostBack="True" CssClass="checkboxbold" />
                                &nbsp;<asp:CheckBox ID="chkboxRoundFullArea" runat="server" Text="Check For Full Area When Round"
                                    CssClass="checkboxbold" />
                                <%--&nbsp;<asp:Label ID="Label38" runat="server" Text="Check For Full Area When Round" CssClass="labelbold"></asp:Label>--%>
                        </td>
                        <td colspan="2" id="DetailNo" runat="server" visible="false" class="tdstyle">
                            <span class="labelbold">IssueDetailNo:</span>
                            <asp:Label ID="lblIssuedetailNo" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td id="TDItemCode2" runat="server" class="tdstyle">
                            <span class="labelbold">ItemCode</span>
                        </td>
                        <td class="tdstyle" id="tdLCategoryName" runat="server" visible="true">
                            <asp:Label ID="lblcategoryname" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label>
                            &nbsp;
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblitemname" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                            &nbsp;
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">Description</span>
                        </td>
                        <td>
                        </td>
                        <td class="tdstyle" id="tdChkforFix" runat="server" visible="false">
                            <b>
                                <asp:CheckBox ID="ChkForFix" runat="server" Text="Chk For Fix" />
                        </td>
                    </tr>
                    <tr>
                        <td id="TDItemCode1" runat="server">
                            <asp:TextBox ID="TxtItemCode" CssClass="textb" runat="server" Width="90px"></asp:TextBox>
                        </td>
                        <td id="tdCategoryName" runat="server" visible="true">
                            <asp:DropDownList CssClass="dropdown" ID="DDCategoryName" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCategoryName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDItemName" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td colspan="3">
                            <asp:DropDownList CssClass="dropdown" ID="DDDescription" runat="server" Width="500px"
                                AutoPostBack="True" OnSelectedIndexChanged="DDDescription_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <%--         <tr>
                       <td id="LblArea" runat="server" visible="false">
                        <td class="tdstyle">
                            T.O.Qty
                        </td>
                        <td class="tdstyle">
                            PQty
                        </td>
                        <td class="tdstyle">
                            Width
                        </td>
                        <td class="tdstyle">
                            Length
                        </td>
                        <td id="LblArea" runat="server" visible="false" class="tdstyle">
                            Area
                        </td>
                        <td class="tdstyle">
                            Rate
                        </td>
                        <td class="tdstyle">
                            Comm
                        </td>
                        <td class="tdstyle">
                            IssueQty&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="TxtReceived" runat="server" Visible="False" Width="0px"></asp:TextBox>
                        </td>
                    </tr>--%>
                    <tr>
                        <td>
                            <span class="labelbold">T.O.Qty</span>
                            <br />
                            <asp:TextBox ID="TxtTotalQty" runat="server" Width="80px" CssClass="textb1" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <span class="labelbold">PQty</span>
                            <br />
                            <asp:TextBox ID="TxtPreQuantity" runat="server" Width="80px" CssClass="textb1" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <span class="labelbold">IssueQty</span>
                            <br />
                            <asp:TextBox ID="TxtReceived" runat="server" Visible="False" Width="0px"></asp:TextBox>
                            <asp:TextBox ID="TxtQtyRequired" runat="server" Width="80px" CssClass="textb" AutoPostBack="True"
                                OnTextChanged="TxtQtyRequired_TextChanged"></asp:TextBox>
                        </td>
                        <td>
                            <span class="labelbold">Width</span>
                            <br />
                            <asp:TextBox ID="TxtWidth" runat="server" Width="80px" CssClass="textb1" AutoPostBack="True"
                                OnTextChanged="TxtWidth_TextChanged" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <span class="labelbold">Length</span>
                            <br />
                            <asp:TextBox ID="TxtLength" runat="server" Width="80px" CssClass="textb1" AutoPostBack="True"
                                OnTextChanged="TxtLength_TextChanged" Enabled="false"></asp:TextBox>
                        </td>
                        <td id="TdArea" runat="server" visible="false">
                            <span class="labelbold">Area</span>
                            <br />
                            <asp:TextBox ID="TxtArea" runat="server" Width="80px" CssClass="textb1" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <span class="labelbold">Rate</span>
                            <br />
                            <asp:TextBox ID="TxtRate" runat="server" Width="80px" CssClass="textb1" AutoPostBack="True"
                                Enabled="false" OnTextChanged="TxtRate_TextChanged"></asp:TextBox>
                        </td>
                        <td>
                            <span class="labelbold">Comm</span>
                            <br />
                            <asp:TextBox ID="TxtCommission" runat="server" Width="80px" CssClass="textb1" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <span class="labelbold">Khap</span>
                            <br />
                            <asp:TextBox ID="TxtKhap" runat="server" Width="90px" CssClass="textb1" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <span class="labelbold">Consump</span>
                            <br />
                            <asp:TextBox ID="TxtConsump" runat="server" Width="90px" CssClass="textb1" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <span class="labelbold">CancelQty</span>
                            <br />
                            <asp:TextBox ID="TxtCancelQty" runat="server" Width="80px" CssClass="textb" AutoPostBack="True"
                                OnTextChanged="TxtCancelQty_TextChanged"></asp:TextBox>
                        </td>
                        <td id="tdAfterKhapSize" runat="server" visible="false">
                            <span class="labelbold">AfterKhapSize</span>
                            <br />
                            <asp:TextBox ID="txtAfterKhapSizeOrder" runat="server" Width="80px" CssClass="textb"
                                Enabled="false" Visible="true"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table width="950px">
                    <tr>
                        <td colspan="3" align="right">
                            <asp:Button ID="BtnSave" runat="server" Text="Save" UseSubmitBehavior="false" OnClick="BtnSave_Click"
                                OnClientClick="if (!SaveData()) return ;this.disabled=true;this.value = 'wait ...';"
                                CssClass="buttonnorm" />
                            <%--OnClientClick="if (!confirm('Do you want to save Date?')) return; this.disabled=true;this.value = 'wait ...';"--%>
                            <asp:Button ID="btnLoomDetail" runat="server" Visible="false" Text="Add Loom" CssClass="buttonnorm"
                                OnClientClick="return AddLoomDetail();" />
                            <asp:Button ID="BtnUpdate" runat="server" OnClientClick="UpdateData();" Text="Update"
                                CssClass="buttonnorm" OnClick="BtnUpdate_Click" Visible="False" />
                            <asp:Button ID="BtnNew" runat="Server" Text="New" CssClass="buttonnorm" OnClientClick="return reloadPage();" />
                            <asp:Button ID="BtnConsump" runat="server" Text="Update Consump" UseSubmitBehavior="false"
                                OnClick="BtnConsump_Click" OnClientClick="if (!SaveData()) return ;this.disabled=true;this.value = 'wait ...';"
                                CssClass="buttonnorm" Visible="false" />
                            <asp:Button ID="BtnUpdateRate" runat="server" Text="Update Rate" UseSubmitBehavior="false"
                                OnClick="BtnUpdateRate_Click" OnClientClick="if (!SaveData()) return ;this.disabled=true;this.value = 'wait ...';"
                                CssClass="buttonnorm" Visible="false" />
                            <asp:Button ID="BtnCancelOrder" runat="server" Text="Cancel Order" UseSubmitBehavior="false"
                                OnClick="BtnCancelOrder_Click" OnClientClick="if (!SaveData()) return ;this.disabled=true;this.value = 'wait ...';"
                                CssClass="buttonnorm" Visible="false" />
                            &nbsp;<asp:Button ID="BtnPreview" runat="server" Text="Preview" CssClass="buttonnorm"
                                OnClick="BtnPreview_Click" Visible="true" />
                            <b>
                                <asp:CheckBox ID="chkforsummary" Visible="false" AutoPostBack="true" Text="Check For Summary"
                                    runat="server" class="tdstyle" /></b> &nbsp;<asp:Button ID="BtnClose" runat="server"
                                        Text="Close" OnClientClick="return CloseForm();" CssClass="buttonnorm" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="LblErrorMessage" runat="server" Text="" Font-Bold="true" ForeColor="RED"
                                Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <div style="width: 900px; max-height: 250px; overflow: auto">
                                <asp:GridView ID="DGOrderdetail" runat="server" AutoGenerateColumns="False" DataKeyNames="Sr_No"
                                    OnSelectedIndexChanged="DGOrderdetail_SelectedIndexChanged" OnRowDataBound="DGOrderdetail_RowDataBound"
                                    OnRowDeleting="DGOrderdetail_RowDeleting" CssClass="grid-view" Width="922px"
                                    ShowFooter="true" OnRowCancelingEdit="DGOrderdetail_RowCancelingEdit" OnRowEditing="DGOrderdetail_RowEditing"
                                    OnRowUpdating="DGOrderdetail_RowUpdating1">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <FooterStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Chkboxitem" runat="server" onclick="return CheckBoxClick(this);" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="10px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="DEL" OnClientClick="return confirm('Do you want to delete data?')"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Category" HeaderText="Category">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="125px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Item" HeaderText="Item">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Quality" HeaderText="Quality">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="350px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Design" HeaderText="Design">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="350px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Color" HeaderText="Color">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="350px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Shape" HeaderText="Shape">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="350px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Size" HeaderText="Size">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="350px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Length" HeaderText="Length">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Width" HeaderText="Width">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Khap" HeaderText="Khap">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Consump" HeaderText="Consump">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Area">
                                            <ItemTemplate>
                                                <asp:Label ID="lblArea" runat="server" Text='<%#Bind("Area") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Rate" HeaderText="Rate">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Comm">
                                            <ItemTemplate>
                                                <asp:Label ID="lblComm" runat="server" Text='<%#Bind("Comm") %>' />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total" Font-Bold="true" />
                                            </FooterTemplate>
                                            <FooterStyle BackColor="Gray" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQty" runat="server" Text='<%#Bind("Qty") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandTQty" runat="server" Font-Bold="true" />
                                            </FooterTemplate>
                                            <FooterStyle BackColor="Gray" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TArea">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTArea" runat="server" Text='<%#Bind("TArea") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandTArea" runat="server" Font-Bold="true" />
                                            </FooterTemplate>
                                            <FooterStyle BackColor="Gray" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="WAmount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWAmount" runat="server" Text='<%#Bind("Amount") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandWAmount" runat="server" Font-Bold="true" />
                                            </FooterTemplate>
                                            <FooterStyle BackColor="Gray" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CommAmt">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCommAmt" runat="server" Text='<%#Bind("CommAmt") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandCommAmt" runat="server" Font-Bold="true" />
                                            </FooterTemplate>
                                            <FooterStyle BackColor="Gray" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="GTotal">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGTotal" runat="server" Text='<%#Bind("GTotal") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandGTotal" runat="server" Font-Bold="true" />
                                            </FooterTemplate>
                                            <FooterStyle BackColor="Gray" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CancelQty" HeaderText="CancelQty">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Item Remark">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtItemRemark" Text='<%#Bind("ItemRemark") %>' Width="250px" Height="50px"
                                                    CssClass="textb" TextMode="MultiLine" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblorderdid" runat="server" Text='<%# Bind("OrderId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:CommandField EditText="Add Item Remark" ShowEditButton="True" CausesValidation="false">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="90px" />
                                        </asp:CommandField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemFinishedId" Text='<%#Bind("Item_Finished_id") %>' runat="server" />
                                                <asp:Label ID="lblQualityid" Text='<%#Bind("Qualityid") %>' runat="server" />
                                                <asp:Label ID="lblDesignid" Text='<%#Bind("Designid") %>' runat="server" />
                                                <asp:Label ID="lblIssueDetailId" Text='<%#Bind("Sr_No") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <span class="labelbold">HSCode</span>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtHSCode" runat="server" Width="200px" CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="labelbold">Remarks</span>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtRemarks" runat="server" Width="500px" TextMode="MultiLine" CssClass="textb"
                                Height="52px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="labelbold">Instructions</span>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtInstructions" runat="server" Width="800px" Height="50px" CssClass="textb"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hncomp" runat="server" />
                <asp:HiddenField ID="hnIssueOrderId" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <style type="text/css">
        #mask
        {
            position: fixed;
            left: 0px;
            top: 0px;
            z-index: 4;
            opacity: 0.4;
            -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=40)"; /* first!*/
            filter: alpha(opacity=40); /* second!*/
            background-color: Gray;
            display: none;
            width: 100%;
            height: 100%;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function ShowPopup() {
            $('#mask').show();
            $('#<%=pnlpopup.ClientID %>').show();
        }
        function HidePopup() {
            $('#mask').hide();
            $('#<%=pnlpopup.ClientID %>').hide();
        }
        $(".btnPwd").live('click', function () {
            HidePopup();
        });
    </script>
    <div id="mask">
    </div>
    <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Height="175px" Width="300px"
        Style="z-index: 111; background-color: White; position: absolute; left: 35%;
        top: 40%; border: outset 2px gray; padding: 5px; display: none">
        <table width="100%" style="width: 100%; height: 100%;" cellpadding="0" cellspacing="5">
            <tr style="background-color: #8B7B8B; height: 1px">
                <td colspan="2" style="color: White; font-weight: bold; font-size: 1.2em; padding: 3px"
                    align="center">
                    ENTER PASSWORD <a id="closebtn" style="color: white; float: right; text-decoration: none"
                        class="btnPwd" href="#">X</a>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Enter Password:
                </td>
                <td>
                    <asp:TextBox ID="txtpwd" runat="server" TextMode="Password" Height="30px" Width="174px" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td align="right">
                    <asp:Button ID="btnCheck" CommandName="Check" runat="server" Text="Check" CssClass="btnPwd"
                        ValidationGroup="m" OnClick="btnCheck_Click" />
                    <input type="button" value="Cancel" class="btnPwd" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
