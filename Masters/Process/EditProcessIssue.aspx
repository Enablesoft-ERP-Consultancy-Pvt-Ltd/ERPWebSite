<%@ Page Language="C#" Title="EditProcessIssue" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="EditProcessIssue.aspx.cs" Inherits="Masters_ProcessIssue_ProcessIssue"
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
            window.location.href = "EditProcessIssue.aspx";
        }
        function Preview() {
            window.open('../../reportViewer.aspx', '');
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
    </script>
    <div id="maindiv" style="height: 600px;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table width="70%">
                    <tr>
                        <td class="tdstyle">
                            <%-- <span class="labelbold">P.O No.</span>--%>
                            <asp:Label ID="lblPONo" runat="server" class="labelbold" Text="P.O No."></asp:Label>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">Company Name</span>
                        </td>
                        <td id="Tdlabelcustcode" runat="server">
                            <span class="labelbold">Customer Code</span>
                        </td>
                        <td>
                            <span class="labelbold">Customer Order No.</span>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">Process Name</span>
                        </td>
                        <td class="tdstyle">
                            <span class="labelbold">Order</span> <span class="labelbold">Emp. Name</span>
                        </td>
                        <td id="chked" runat="server" style="width: 160px" class="tdstyle">
                            <asp:CheckBox ID="ChkEditOrder" runat="server" Text="Edit Order" AutoPostBack="True"
                                OnCheckedChanged="ChkEditOrder_CheckedChanged" Enabled="False" CssClass="checkboxbold" />
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
                        <td id="Tddropdowncustcode" runat="server">
                            <asp:DropDownList CssClass="dropdown" ID="DDCustomerCode" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCustomerCode_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList CssClass="dropdown" ID="DDCustomerOrderNumber" Width="150px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCustomerOrderNumber_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
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
                        <td class="tdstyle">
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
                        <td class="tdstyle">
                            <b>
                                <asp:CheckBox ID="ChkForFix" runat="server" Text="Chk For Fix" CssClass="checkboxbold" />
                        </td>
                    </tr>
                    <tr>
                        <td id="TDItemCode1" runat="server">
                            <asp:TextBox ID="TxtItemCode" CssClass="textb" runat="server" Width="90px"></asp:TextBox>
                        </td>
                        <td>
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
                        <td>
                            <asp:CheckBox ID="chkexportsize" class="tdstyle" runat="server" Text="Export Size"
                                AutoPostBack="true" CssClass="checkboxbold" OnCheckedChanged="chkexportsize_CheckedChanged" />
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
                            <asp:TextBox ID="TxtTotalQty" runat="server" Width="80px" CssClass="textb" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <span class="labelbold">PQty</span>
                            <br />
                            <asp:TextBox ID="TxtPreQuantity" runat="server" Width="80px" CssClass="textb" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <span class="labelbold">Width</span>
                            <br />
                            <asp:TextBox ID="TxtWidth" runat="server" Width="80px" CssClass="textb" AutoPostBack="True"
                                OnTextChanged="TxtWidth_TextChanged"></asp:TextBox>
                        </td>
                        <td>
                            <span class="labelbold">Length</span>
                            <br />
                            <asp:TextBox ID="TxtLength" runat="server" Width="80px" CssClass="textb" AutoPostBack="True"
                                OnTextChanged="TxtLength_TextChanged"></asp:TextBox>
                        </td>
                        <td id="TdArea" runat="server">
                            <span class="labelbold">Area</span>
                            <br />
                            <asp:TextBox ID="TxtArea" runat="server" Width="80px" CssClass="textb" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <span class="labelbold">Rate</span>
                            <br />
                            <asp:TextBox ID="TxtRate" runat="server" Width="80px" CssClass="textb" AutoPostBack="True"
                                OnTextChanged="TxtRate_TextChanged"></asp:TextBox>
                        </td>
                        <td>
                            <%--<span class="labelbold">Comm</span>--%>
                            <asp:Label ID="lblComm" Text="Comm" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtCommission" runat="server" Width="80px" CssClass="textb"></asp:TextBox>
                        </td>
                        <td>
                            <span class="labelbold">IssueQty</span>
                            <br />
                            <asp:TextBox ID="TxtReceived" runat="server" Visible="False" Width="0px"></asp:TextBox>
                            <asp:TextBox ID="TxtQtyRequired" runat="server" Width="80px" CssClass="textb" AutoPostBack="True"
                                OnTextChanged="TxtQtyRequired_TextChanged"></asp:TextBox>
                        </td>
                        <td>
                            <span class="labelbold">CancelQty</span>
                            <br />
                            <asp:TextBox ID="TxtCancelQty" runat="server" Width="80px" CssClass="textb" AutoPostBack="True"
                                OnTextChanged="TxtCancelQty_TextChanged"></asp:TextBox>
                        </td>
                        <td id="tdtxtQualityGrmPerMeterMinus" runat="server" visible="false">
                            <span class="labelbold">QualityGrmPerMeter-</span>
                            <br />
                            <asp:TextBox ID="TxtQualityGrmPerMeterMinus" runat="server" Width="100px" CssClass="textb"></asp:TextBox>
                        </td>
                        <td id="tdtxtQualityGrmPerMeterPlus" runat="server" visible="false">
                            <span class="labelbold">QualityGrmPerMeter+</span>
                            <br />
                            <asp:TextBox ID="TxtQualityGrmPerMeterPlus" runat="server" Width="100px" CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table width="950px">
                    <tr>
                        <td colspan="3" align="right">
                            <asp:Button ID="BtnSave" runat="server" Text="Save" UseSubmitBehavior="false" OnClick="BtnSave_Click"
                                OnClientClick="if (!SaveData()) return ;this.disabled=true;this.value = 'wait ...';"
                                CssClass="buttonnorm" />&nbsp;
                            <%--OnClientClick="if (!confirm('Do you want to save Date?')) return; this.disabled=true;this.value = 'wait ...';"--%>
                            <asp:Button ID="btnLoomDetail" runat="server" Visible="false" Text="Add Loom" CssClass="buttonnorm"
                                OnClientClick="return AddLoomDetail();" />
                            &nbsp;<asp:Button ID="BtnUpdate" runat="server" OnClientClick="UpdateData();" Text="Update"
                                CssClass="buttonnorm" OnClick="BtnUpdate_Click" Visible="False" />
                            &nbsp;<asp:Button ID="BtnNew" runat="Server" Text="New" CssClass="buttonnorm" OnClientClick="return reloadPage();" />
                            &nbsp;<asp:Button ID="BtnUpdateEmpName" runat="server" Text="Update EmpName" CssClass="buttonnorm"
                                OnClick="BtnUpdateEmpName_Click" Visible="false" />
                            &nbsp;<asp:Button ID="BtnPreview" runat="server" Text="Preview" CssClass="buttonnorm"
                                OnClick="BtnPreview_Click" Visible="true" />
                            &nbsp;<asp:Button ID="BtnPreviewConsumption" runat="server" Text="Preview Consumption"
                                CssClass="buttonnorm" OnClick="BtnPreviewConsumption_Click" Visible="false" />
                            &nbsp;<asp:Button ID="btncancel" runat="server" Text="Cancel Po No." CssClass="buttonnorm"
                                OnClientClick="return confirm('Do you want to cancel this Po?')" OnClick="btncancel_Click" />
                            &nbsp;<asp:Button ID="btnupdateconsumption" runat="server" Text="Update All Consumption"
                                UseSubmitBehavior="false" CssClass="buttonnorm" OnClick="btnupdateconsumption_Click"
                                OnClientClick="if (!confirm('Do you want update all item consumption?')) return ;this.disabled=true;this.value = 'wait ...';" />

                                <b>
                                        <asp:CheckBox ID="ChkForExport" Visible="false" AutoPostBack="true" Text="Export Excel Report "
                                            runat="server" class="tdstyle" /></b>
                            <b>
                                <asp:CheckBox ID="chkforsummary" Visible="false" AutoPostBack="true" Text="Check For Summary"
                                    runat="server" class="tdstyle" /></b> <b>
                                        <asp:CheckBox ID="chkforProductionOrder" Visible="false" AutoPostBack="true" Text="Check For ProductionOrder"
                                            runat="server" class="tdstyle" /></b> <b>
                                                <asp:CheckBox ID="chkforWeaverRawMaterial" Visible="false" AutoPostBack="true" Text="For RawMaterial"
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
                                    OnRowCancelingEdit="DGOrderdetail_RowCancelingEdit" OnRowEditing="DGOrderdetail_RowEditing"
                                    OnRowUpdating="DGOrderdetail_RowUpdating1">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex+1 %>
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
                                        <asp:BoundField DataField="Description" HeaderText="Description">
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
                                        <asp:BoundField DataField="Qty" HeaderText="Qty">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Rate" HeaderText="Rate">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Area" HeaderText="Area">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Amount" HeaderText="Amount">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:BoundField>
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
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="DEL" OnClientClick="return confirm('Do you want to save data?')"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblorderdid" runat="server" Text='<%# Bind("OrderId") %>'></asp:Label>
                                                <asp:Label ID="lblitemfinishedid" runat="server" Text='<%# Bind("item_finished_id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:CommandField EditText="Add Item Remark" ShowEditButton="True" CausesValidation="false">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="90px" />
                                        </asp:CommandField>
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
                <asp:HiddenField ID="hnpurchasefolio" runat="server" Value="0" />
                <asp:HiddenField ID="hnCustomerId" runat="server" />
            </ContentTemplate>
             <Triggers>
            <asp:PostBackTrigger ControlID="BtnPreview" />
        </Triggers>
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
