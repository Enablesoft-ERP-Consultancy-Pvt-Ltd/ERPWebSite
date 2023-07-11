<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frm_receive_process_nextForAnisa.aspx.cs"
    Inherits="Masters_Process_frm_receive_process_nextForAnisa" MasterPageFile="~/ERPmaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/FixFocus2.js"></script>
    <script type="text/javascript">
        function HandleTextKeypress(e) {
            var key = e.keycode || e.which;
            if (key == 13) {
                var txtControl = document.getElementById('<%= TxtStockNo.ClientID %>');
                e.preventDefault();
                txtControl.focus();
            }
        }
        function EmpSelected(source, eventArgs) {
            document.getElementById('<%=txtgetvalue.ClientID %>').value = eventArgs.get_value();
        }
        function NewForm() {
            window.location.href = "frm_receive_process_nextforanisa.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function KeyDownHandler(event) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                window.document.getElementById('<%=btnStockNo.ClientID %>').click();
            }
        }
        function Preview() {
            window.open('../../reportViewer.aspx', '');
        }
        function YourFunctionName(msg) {
            var txt = msg;
            alert(txt);
        }
        function CheckAll(objref) {
            var gridview = objref.parentNode.parentNode.parentNode;
            var inputlist = gridview.getElementsByTagName("input");
            for (var i = 0; i < inputlist.length; i++) {
                var row = inputlist[i].parentNode.parentNode;
                if (inputlist[i].type == "checkbox" && objref != inputlist[i]) {
                    if (objref.checked) {

                        inputlist[i].checked = true;

                    }
                    else {
                        inputlist[i].checked = false;


                    }
                }
            }

        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            else {
                return true;
            }
        }

    </script>
    <div>
        <table width="100%">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div style="width: 1000px">
                                <div style="width: 85%; float: left;">
                                    <div style="max-height: 300px; border-style: groove; background-color: #DEB887;">
                                        <table>
                                            <tr valign="top">
                                                <td runat="server" id="TDQDCS">
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblunitname" Text="Unit Name" runat="server" CssClass="labelbold" />
                                                                <br />
                                                                <asp:DropDownList ID="ddUnits" runat="server" Width="130px" CssClass="dropdown" OnSelectedIndexChanged="ddUnits_SelectedIndexChanged"
                                                                    AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label3" Text="Job Name" runat="server" CssClass="labelbold" /><br />
                                                                <asp:DropDownList ID="ddprocess" runat="server" CssClass="dropdown" Width="130px"
                                                                    OnSelectedIndexChanged="ddprocess_SelectedIndexChanged1" AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td id="TDcategory" runat="server">
                                                                <asp:Label ID="lblcategoryname" class="tdstyle" runat="server" Text="Item Category"
                                                                    CssClass="labelbold"></asp:Label><br />
                                                                <asp:DropDownList ID="ddcattype" runat="server" Width="130px" CssClass="dropdown"
                                                                    AutoPostBack="True" OnSelectedIndexChanged="ddcattype_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td runat="server" id="tdquality">
                                                                <asp:Label ID="lblqualityname" CssClass="labelbold" runat="server" Text="Quality"></asp:Label><br />
                                                                <asp:DropDownList ID="ddquality" runat="server" CssClass="dropdown" Width="130px"
                                                                    AutoPostBack="True" OnSelectedIndexChanged="ddquality_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td runat="server" id="tddesign">
                                                                <asp:Label ID="lbldesignname" CssClass="labelbold" runat="server" Text="Design"></asp:Label><br />
                                                                <asp:DropDownList ID="ddldesig" runat="server" CssClass="dropdown" Width="130px"
                                                                    AutoPostBack="True" OnSelectedIndexChanged="ddldesig_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td id="tdcolor" runat="server">
                                                                <asp:Label ID="lblcolorname" CssClass="labelbold" runat="server" Text="Color"></asp:Label><br />
                                                                <asp:DropDownList ID="ddcolour" runat="server" CssClass="dropdown" Width="130px"
                                                                    AutoPostBack="True" OnSelectedIndexChanged="ddcolour_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td id="tdsize" runat="server">
                                                                <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                                                <br />
                                                                <asp:DropDownList ID="ddsize" runat="server" CssClass="dropdown" Width="130px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td id="TDcheckedby" runat="server">
                                                                <asp:Label ID="Label1" runat="server" Text="Checked By" CssClass="labelbold"></asp:Label>
                                                                <br />
                                                                <asp:TextBox ID="txtcheckedby" CssClass="textb" Width="80px" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label Text="Actual Width" CssClass="labelbold" runat="server" />
                                                                <br />
                                                                <asp:TextBox ID="txtactualwidth" CssClass="textb" Width="80px" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label2" Text="Actual Length" CssClass="labelbold" runat="server" />
                                                                <br />
                                                                <asp:TextBox ID="txtactuallength" CssClass="textb" Width="80px" runat="server" />
                                                            </td>
                                                            <td id="TDBulkReceiveQty" runat="server" visible="false">
                                                                <asp:Label ID="lblBulkReceiveQty" runat="server" Text="Rec Qty" CssClass="labelbold"></asp:Label><br />
                                                                <asp:TextBox ID="TxtBulkReceiveQty" CssClass="textb" Width="50px" runat="server"
                                                                    Enabled="False" AutoPostBack="true" OnTextChanged="TxtBulkReceiveQty_TextChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td id="TDCottonMoisture" runat="server" visible="false">
                                                                <asp:Label ID="Label12" runat="server" Text="Cotton Moisture(%)" CssClass="labelbold"></asp:Label>
                                                                <br />
                                                                <asp:TextBox ID="txtReceiveCottonMoisture" CssClass="textb" Width="120px" runat="server" AutoPostBack="true" OnTextChanged="txtReceiveCottonMoisture_TextChanged" />
                                                            </td>
                                                            <td id="TDWoolMoisture" runat="server" visible="false">
                                                                <asp:Label ID="Label13" runat="server" Text="Wool Moisture(%)" CssClass="labelbold"></asp:Label>
                                                                <br />
                                                                <asp:TextBox ID="txtReceiveWoolMoisture" CssClass="textb" Width="120px" runat="server" AutoPostBack="true" OnTextChanged="txtReceiveWoolMoisture_TextChanged" />
                                                            </td>
                                                            <td id="TDFinishingDateStamp" runat="server" visible="false">
                                                                <asp:Label ID="Label9" runat="server" Text="Date Stamp" CssClass="labelbold"></asp:Label>
                                                                <br />
                                                                <asp:TextBox ID="txtFinishingReceiveDateStamp" CssClass="textb" Width="100px" runat="server" 
                                                                onkeypress="return isNumberKey(this);" AutoPostBack="true" OnTextChanged="txtFinishingReceiveDateStamp_TextChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr id="TRStockNoRemark" runat="server" visible="false">
                                                            <td colspan="3">
                                                                <asp:Label ID="Label5" Text="StockNo Remark" CssClass="labelbold" runat="server" />
                                                                <br />
                                                                <asp:TextBox CssClass="textb" ID="txtstocknoremarks" runat="server" Width="100%"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <span class="labelbold">Enter ID No.</span>
                                                                <br />
                                                                <asp:TextBox ID="txtWeaverIdNo" runat="server" Width="100px" Height="20px" CssClass="textb"
                                                                    AutoPostBack="true" OnTextChanged="txtWeaverIdNo_TextChanged"></asp:TextBox>
                                                                <asp:TextBox ID="txtgetvalue" runat="server" Style="display: none"></asp:TextBox>
                                                                <asp:AutoCompleteExtender ID="txtWeaverIdNo_AutoCompleteExtender" runat="server"
                                                                    BehaviorID="SrchAutoComplete" CompletionInterval="20" Enabled="True" ServiceMethod="GetEmployeeForJob"
                                                                    EnableCaching="true" CompletionSetCount="20" OnClientItemSelected="EmpSelected"
                                                                    ServicePath="~/Autocomplete.asmx" TargetControlID="txtWeaverIdNo" UseContextKey="True"
                                                                    ContextKey="0#0#0" MinimumPrefixLength="2">
                                                                </asp:AutoCompleteExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td id="Tdrecdate" class="tdstyle" runat="server">
                                                                <span class="labelbold">Rec.Date</span><br />
                                                                <asp:TextBox CssClass="textb" ID="TxtreceiveDate" runat="server" Width="90px" AutoPostBack="true"
                                                                    OnTextChanged="TxtreceiveDate_TextChanged"></asp:TextBox>
                                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                                                    TargetControlID="TxtreceiveDate">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                        <tr id="TRWeight" runat="server" visible="false">
                                                            <td>
                                                                <asp:Label ID="lblweight" Text="Weight" CssClass="labelbold" runat="server"></asp:Label>
                                                                <br />
                                                                <asp:TextBox ID="txtweight" CssClass="textb" Width="100px" runat="server" onkeypress="HandleTextKeypress(event);" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="Label14" Text="Hold/Rejected" CssClass="labelbold" runat="server" /><br />
                                                                            <asp:DropDownList ID="ddStockQualityType" runat="server" CssClass="dropdown">
                                                                                <asp:ListItem Value="1">Finished</asp:ListItem>
                                                                                <asp:ListItem Value="2">Hold</asp:ListItem>
                                                                                <asp:ListItem Value="3">Rejected</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <br />
                                                    <asp:Button ID="btnShowDetail" runat="server" Text="Get Stock No." CssClass="buttonnorm"
                                                        Width="100px" OnClick="btnShowDetail_Click" />
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div style="overflow: auto; width: 250px">
                                                                    <asp:ListBox ID="lstWeaverName" runat="server" Width="240px" Height="142px" SelectionMode="Multiple">
                                                                    </asp:ListBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <asp:LinkButton ID="btnDeleteName" Text="Remove Employee" CssClass="labelbold" runat="server"
                                                                    OnClick="btnDeleteName_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div style="margin-top: 10px">
                                        <asp:Panel ID="panelMaster" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                            BackColor="#DEB887">
                                            <table>
                                                <tr id="Tr1" runat="server">
                                                    <td>
                                                        <span class="labelbold">Enter Stock No</span>
                                                        <asp:TextBox ID="TxtStockNo" runat="server" Width="200px" CssClass="textb" TabIndex="8"
                                                            onKeypress="KeyDownHandler(event);" Height="30px"></asp:TextBox>
                                                    </td>
                                                    <td class="tdstyle">
                                                        <asp:Button ID="btnStockNo" runat="server" Style="display: none" OnClick="TxtStockNo_TextChanged" />
                                                    </td>
                                                    <td id="TDChallanNo" runat="server" visible="false" class="tdstyle">
                                                        Challan No<br />
                                                        <asp:TextBox CssClass="textb" ID="TxtEditChallanNo" runat="server" Width="100px"
                                                            AutoPostBack="True" OnTextChanged="TxtEditChallanNo_TextChanged"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="LblErrorMessage" runat="server" Text="" Font-Bold="false" ForeColor="Red"></asp:Label>
                                                    </td>
                                                    <td id="Td1" class="tdstyle" runat="server" visible="false">
                                                        Company Name&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b style="color: Red">*</b>
                                                        <br />
                                                        <asp:DropDownList ID="ddCompName" runat="server" TabIndex="1" CssClass="dropdown"
                                                            Width="100px" OnSelectedIndexChanged="ddCompName_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="Td2" class="tdstyle" runat="server" visible="false">
                                                        Emp/Contractor&nbsp;&nbsp; <b style="color: Red">*</b>&nbsp;&nbsp;
                                                        <asp:CheckBox ID="ChkForEdit" runat="server" CssClass="checkboxnormal" Text="For Edit"
                                                            OnCheckedChanged="ChkForEdit_CheckedChanged" AutoPostBack="True" />
                                                        <br />
                                                        <asp:DropDownList ID="ddemp" runat="server" CssClass="dropdown" Width="100px" OnSelectedIndexChanged="ddemp_SelectedIndexChanged"
                                                            AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table>
                                                <tr>
                                                    <td id="TDDDChallanNo" runat="server" visible="false" class="tdstyle">
                                                        Challan No&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b style="color: Red">*</b><br />
                                                        <asp:DropDownList ID="DDChallanNo" runat="server" Width="100px" CssClass="dropdown"
                                                            AutoPostBack="True" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="Td3" class="tdstyle" runat="server" visible="false">
                                                        P OrderNo&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b style="color: Red">*</b><br />
                                                        <asp:DropDownList ID="ddorderno" runat="server" CssClass="dropdown" Width="100px"
                                                            AutoPostBack="True" OnSelectedIndexChanged="ddorderno_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="Td4" class="tdstyle" runat="server" visible="false">
                                                        Cal Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b style="color: Red">*</b><br />
                                                        <asp:DropDownList CssClass="dropdown" ID="DDcaltype" runat="server" Width="100px"
                                                            Enabled="false">
                                                            <asp:ListItem Value="0">Area Wise</asp:ListItem>
                                                            <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                                            <asp:ListItem Value="2">W-2 X L-2</asp:ListItem>
                                                            <asp:ListItem Value="3">W-2</asp:ListItem>
                                                            <asp:ListItem Value="4">L-2</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="Td5" runat="server" visible="false">
                                                        Unit
                                                        <br />
                                                        <asp:DropDownList CssClass="dropdown" ID="DDUnit" runat="server" Width="100px" AutoPostBack="True"
                                                            Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="Td6" class="tdstyle" runat="server" visible="false">
                                                        Challan No<br />
                                                        <asp:TextBox CssClass="textb" ReadOnly="true" ID="TxtChallanNo" runat="server" Width="90px"
                                                            AutoPostBack="true"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="80%">
                                                <tr id="Tr2" runat="server" visible="false">
                                                    <td>
                                                        <asp:Label ID="lblitemname" class="tdstyle" runat="server" Text="Item Name"></asp:Label><br />
                                                        <asp:DropDownList ID="dditem" runat="server" CssClass="dropdown" Width="150px" AutoPostBack="True"
                                                            OnSelectedIndexChanged="dditem_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td runat="server" id="tdshape" visible="false">
                                                        <asp:Label ID="lblshapename" class="tdstyle" runat="server" Text="Shape"></asp:Label><br />
                                                        <asp:DropDownList ID="ddshape" runat="server" CssClass="dropdown" Width="100px" OnSelectedIndexChanged="ddshape_SelectedIndexChanged"
                                                            AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="tdshadecolor" runat="server" visible="false">
                                                        <asp:Label ID="lblshadecolor" runat="server" Text="Shade Color"></asp:Label>
                                                        &nbsp;<br />
                                                        <asp:DropDownList ID="ddlshade" runat="server" Width="100px" CssClass="dropdown"
                                                            OnSelectedIndexChanged="ddlshade_SelectedIndexChanged" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <%-- <tr>
                                        <td colspan="6">
                                            <asp:Label ID="LblErrorMessage" runat="server" Text="" Font-Bold="false" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>--%>
                                            </table>
                                        </asp:Panel>
                                    </div>
                                    <table>
                                        <tr>
                                            <td>
                                                <div style="max-height: 300px; background-color: Gray; overflow: scroll; width: 800px">
                                                    <asp:GridView ID="DGDetail" runat="server" AutoGenerateColumns="False" DataKeyNames="process_rec_Detail_Id"
                                                        OnRowDeleting="DGDetail_RowDeleting" CssClass="grid-view" Width="860px">
                                                        <HeaderStyle CssClass="gvheaders" />
                                                        <AlternatingRowStyle CssClass="gvalts" />
                                                        <RowStyle CssClass="gvrow" />
                                                        <Columns>
                                                            <asp:BoundField DataField="Category" HeaderText="Category" Visible="false"></asp:BoundField>
                                                            <asp:BoundField DataField="Item" HeaderText="Item"></asp:BoundField>
                                                            <asp:BoundField DataField="Description" HeaderText="Description"></asp:BoundField>
                                                            <asp:BoundField DataField="Width" HeaderText="Width"></asp:BoundField>
                                                            <asp:BoundField DataField="Length" HeaderText="Length"></asp:BoundField>
                                                            <asp:BoundField DataField="Size" HeaderText="Size"></asp:BoundField>
                                                            <asp:BoundField DataField="Qty" HeaderText="Qty" Visible="false"></asp:BoundField>
                                                            <asp:BoundField DataField="Rate" HeaderText="Rate" Visible="false"></asp:BoundField>
                                                            <asp:BoundField DataField="Area" HeaderText="Area" Visible="false"></asp:BoundField>
                                                            <asp:BoundField DataField="Amount" HeaderText="Amount" Visible="false"></asp:BoundField>
                                                            <asp:TemplateField HeaderText="StockNo">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblstockno" Text='<%#Bind("StockNo") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ShowHeader="False">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                                        Text="DEL" OnClientClick="return confirm('Do you want to delete data?')"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ShowHeader="False" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="BtnPenality" runat="server" Text="Penality" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                        CommandName="Penality"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="BtnStockF" runat="server" CssClass="buttonnorm" CommandName="update"
                                                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text="QualityChk"
                                                                        Width="50px" />
                                                                </ItemTemplate>
                                                                <ItemStyle ForeColor="Black" HorizontalAlign="Left" Width="50px" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ShowHeader="False" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProcessId" runat="server" Text='<%#Bind("ProcessId") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ShowHeader="False" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIssueOrderId" runat="server" Text='<%#Bind("IssueOrderId") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ShowHeader="False" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIssueDetailId" runat="server" Text='<%#Bind("Issue_Detail_Id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:CheckBox ID="ChkDetailPrint" class="tdstyle" runat="server" Text="For Detail Print"
                                                    Visible="false" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:Button CssClass="buttonnorm" ID="BtnPreview" runat="server" Text="Preview" OnClick="BtnPreview_Click"
                                                    Visible="false" />
                                                &nbsp;<asp:Button CssClass="buttonnorm" ID="btnQcPreview" runat="server" Text="QC CHECK"
                                                    OnClick="btnQcPreview_Click" Visible="false" />
                                                &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnClose" runat="server" Text="Close"
                                                    OnClientClick=" return CloseForm(); " />
                                                &nbsp;<asp:Button CssClass="buttonnorm" ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr id="Tr3" runat="server">
                                            <td class="tdstyle">
                                                <span class="labelbold">Total Pcs</span>
                                                <asp:TextBox CssClass="textb" ID="TxtTotalPcs" runat="server"></asp:TextBox>
                                            </td>
                                            <td id="Td7" class="tdstyle" runat="server" visible="false">
                                                Total Area
                                                <asp:TextBox CssClass="textb" ID="TxtArea" runat="server"></asp:TextBox>
                                            </td>
                                            <td id="Td8" class="tdstyle" runat="server" visible="false">
                                                Total Amount
                                                <asp:TextBox CssClass="textb" ID="TxtAmount" runat="server"></asp:TextBox>
                                            </td>
                                            <td id="Td9" runat="server" visible="false">
                                                <asp:Button ID="btnShowdata" runat="server" Text="Click For Data Show" CssClass="buttonnorm"
                                                    OnClick="btnShowdata_Click" />
                                            </td>
                                        </tr>
                                        <tr id="Tr4" runat="server" visible="false">
                                            <td colspan="3">
                                                <asp:Label ID="Label4" Text="Remarks" CssClass="labelbold" runat="server" />
                                                <br />
                                                <asp:TextBox CssClass="textb" ID="TxtRemarks" runat="server" Width="100%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:HiddenField ID="hn_finished" runat="server" />
                                                <asp:HiddenField ID="hnstockno" runat="server" />
                                                <asp:HiddenField ID="hnrate1" runat="server" />
                                                <asp:HiddenField ID="hnorderid" runat="server" />
                                                <asp:HiddenField ID="hn_recieve_id" runat="server" />
                                                <asp:HiddenField ID="Hn_Qty" runat="server" />
                                                <asp:HiddenField ID="Hn_ProcessId" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="width: 20%; float: right">
                                    <table>
                                        <tr>
                                            <td align="justify">
                                                <div style="max-width: 250px; max-height: 500px; overflow: auto;">
                                                    <asp:GridView ID="DGStockDetail" runat="server" AutoGenerateColumns="False" AllowPaging="true"
                                                        PageSize="50" OnPageIndexChanging="DGStockDetail_PageIndexChanging" OnRowDataBound="DGStockDetail_RowDataBound">
                                                        <HeaderStyle CssClass="gvheaders" />
                                                        <AlternatingRowStyle CssClass="gvalts" />
                                                        <RowStyle CssClass="gvrow" Height="20px" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="">
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="Chkboxitem" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="PROCESS_NAME" HeaderText="Process Name">
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="150px" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="StockNo.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbltstockno" Text='<%#Bind("Tstockno") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="TDButtonsavegrid" runat="server">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbltpcs" Text="Total Pcs" CssClass="labelbold" runat="server" />
                                                            <br />
                                                            <asp:TextBox ID="txttotalpcsgrid" runat="server" CssClass="textb" Width="90px" />
                                                        </td>
                                                        <td valign="bottom">
                                                            <asp:Button ID="btnsavegrid" Text="Save" runat="server" CssClass="buttonnorm" OnClick="btnsavegrid_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div>
                                <asp:Button runat="server" ID="btnModalPopUp" Style="display: none" />
                                <asp:ModalPopupExtender ID="Modalpopupextqc" runat="server" PopupControlID="pnModelPopup"
                                    TargetControlID="btnModalPopUp" BackgroundCssClass="modalBackground" CancelControlID="btnqcclose">
                                </asp:ModalPopupExtender>
                                <asp:Panel ID="pnModelPopup" runat="server" Style="background-color: ActiveCaption;
                                    display: none">
                                    <fieldset>
                                        <legend>
                                            <asp:Label ID="lblqc" Text="QC PARAMETER" runat="server" ForeColor="Red" CssClass="labelbold" />
                                        </legend>
                                        <table>
                                            <tr>
                                                <td>
                                                    <div style="max-height: 500px; overflow: auto">
                                                        <asp:GridView ID="GDQC" CssClass="grid-views" runat="server" OnRowDataBound="GDQC_RowDataBound">
                                                            <HeaderStyle CssClass="gvheaders" Font-Size="12px" />
                                                            <AlternatingRowStyle CssClass="gvalts" />
                                                            <RowStyle CssClass="gvrow" />
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnqcsavenew" Text="Save" runat="server" CssClass="buttonnorm" OnClick="btnqcsavenew_Click" />
                                                    <asp:Button ID="btnqcclose" Text="Close" runat="server" CssClass="buttonnorm" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblqcmsg" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </asp:Panel>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
