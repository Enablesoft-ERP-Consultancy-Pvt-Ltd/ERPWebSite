<%@ Page Title="HOME FURNISHING PRODUCTION ORDER" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmHomeFurnishingProductionOrder.aspx.cs" Inherits="Masters_HomeFurnishing_FrmHomeFurnishingProductionOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    &nbsp;
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CallConfirmBox() {
            if (confirm("Confirm Proceed Further?")) {
                //OK – Do your stuff or call any callback method here..
                alert("You pressed OK!");
            } else {
                //CANCEL – Do your stuff or call any callback method here..
                alert("You pressed Cancel!");
            }
        }
        function EmpSelected(source, eventArgs) {
            document.getElementById('<%=txtgetvalue.ClientID%>').value = eventArgs.get_value();
            document.getElementById('<%=btnSearch.ClientID%>').click();
        }
        function EmpSelectedEdit(source, eventArgs) {
            document.getElementById('<%=txteditempid.ClientID%>').value = eventArgs.get_value();
            document.getElementById('<%=btnsearchedit.ClientID%>').click();
        }
        function Loomnoselected(source, eventArgs) {
            document.getElementById('<%=txtloomid.ClientID%>').value = eventArgs.get_value();
            document.getElementById('<%=hnissueorderid.ClientID%>').value = "0";
        }
        function NewForm() {
            window.location.href = "FrmHomeFurnishingProductionOrder.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
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
        function cancelvalidation() {
            var Message = "";
            var selectedindex = $("#<%=DDFolioNo.ClientID %>").attr('selectedIndex');
            if (selectedindex <= 0) {
                Message = Message + "Please select Folio No!!\n";
            }
            if (Message == "") {
                return true;
            }
            else {
                alert(Message);
                return false;
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

                    }
                    else {
                        inputlist[i].checked = false;
                    }
                }
            }
        }
        function KeyDownHandlerWeaverIdscan(event) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                window.document.getElementById('<%=btnweaveridscan.ClientID %>').click();
            }
        }
    </script>
    <script type="text/javascript">
        function validatesave() {
            var Message = "";
            var selectedindex = $("#<%=DDcompany.ClientID %>").attr('selectedIndex');
            if (selectedindex < 0) {
                Message = Message + "Please select Company Name!!\n";
            }
            selectedindex = $("#<%=DDProdunit.ClientID %>").attr('selectedIndex');
            if (selectedindex <= 0) {
                Message = Message + "Please Select Production Unit. !!\n";
            }
            if (document.getElementById("<%=TDLoomNoDropdown.ClientID %>")) {
                selectedindex = $("#<%=DDLoomNo.ClientID %>").attr('selectedIndex');
                if (selectedindex <= 0) {
                    Message = Message + "Please select Loom No. !!\n";
                }
            }
            if (document.getElementById("<%=TDUNIT.ClientID %>")) {
                selectedindex = $("#<%=ddunit.ClientID %>").attr('selectedIndex');
                if (selectedindex <= 0) {
                    Message = Message + "Please select Unit Name. !!\n";
                }
            }
            if (document.getElementById("<%=TDFolioNo.ClientID %>")) {
                selectedindex = $("#<%=DDFolioNo.ClientID %>").attr('selectedIndex');
                if (selectedindex <= 0) {
                    Message = Message + "Please select Folio No. !!\n";
                }
            }
            var txtissuedate = document.getElementById('<%=txtissuedate.ClientID %>');
            if (txtissuedate.value == "") {
                Message = Message + "Please Enter Issue Date. !!\n";
            }
            var txttargetdate = document.getElementById('<%=txttargetdate.ClientID %>');
            if (txttargetdate.value == "") {
                Message = Message + "Please Enter Target Date. !!\n";
            }
            selectedindex = $("#<%=DDcustcode.ClientID %>").attr('selectedIndex');
            if (selectedindex <= 0) {
                Message = Message + "Please select Buyer. !!\n";
            }
            selectedindex = $("#<%=DDorderNo.ClientID %>").attr('selectedIndex');
            if (selectedindex <= 0) {
                Message = Message + "Please select Order No. !!\n";
            }
            if (Message == "") {
                return true;
            }
            else {
                alert(Message);
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {

                $("#<%=btnactiveemployee.ClientID %>").click(function () {
                    var Message1 = "";
                    var selectedindex = $("#<%=DDFolioNo.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message1 = Message1 + "Please select Folio No!!\n";
                    }
                    if (Message1 == "") {
                        return true;
                    }
                    else {
                        alert(Message1);
                        return false;
                    }
                });

            });
        }
    </script>
    <div>
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
                <script type="text/javascript" language="javascript">
                    Sys.Application.add_load(Jscriptvalidate);
                </script>
                <fieldset>
                    <legend>
                        <asp:Label ID="lblMasterDetail" runat="server" Text="Master Details" CssClass="labelbold"
                            ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                    <div style="width: 60%; float: left;">
                        <table>
                            <tr>
                                <td id="TDEdit" runat="server">
                                    <asp:CheckBox ID="chkEdit" Text="For Edit" CssClass="checkboxbold" runat="server"
                                        AutoPostBack="true" OnCheckedChanged="chkEdit_CheckedChanged" />
                                </td>
                                <td id="TDComplete" runat="server">
                                    <asp:CheckBox ID="chkcomplete" Text="For Complete" CssClass="checkboxbold" runat="server"
                                        AutoPostBack="true" OnCheckedChanged="chkcomplete_CheckedChanged" />
                                </td>
                                <td id="TDLastFolioNo" runat="server" visible="false">
                                    <span style="color: Red; font-weight: bold">
                                        <asp:Label ID="lblLastFolio" runat="server" Text="Last FolioNo:"></asp:Label>&nbsp;<asp:Label
                                            ID="lblLastFolioNo" runat="server"></asp:Label>
                                    </span>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td id="TDEMPEDIT" runat="server" visible="false">
                                    <asp:Label ID="lblempcodeedit" CssClass="labelbold" Text="Emp. Code." runat="server" />
                                    <br />
                                    <asp:TextBox ID="txteditempcode" CssClass="textb" runat="server" Width="80px" />
                                    <asp:TextBox ID="txteditempid" runat="server" Style="display: none"></asp:TextBox>
                                    <asp:Button ID="btnsearchedit" runat="server" Text="Button" OnClick="btnsearchedit_Click"
                                        Style="display: none;" />
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="SrchAutoComplete1"
                                        CompletionInterval="20" Enabled="True" ServiceMethod="GetEmployeeAll" EnableCaching="true"
                                        CompletionSetCount="20" OnClientItemSelected="EmpSelectedEdit" ServicePath="~/Autocomplete.asmx"
                                        TargetControlID="txteditempcode" UseContextKey="True" ContextKey="0#0#0" MinimumPrefixLength="2">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td id="TDFolioNotext" runat="server" visible="false">
                                    <asp:Label CssClass="labelbold" Text="Folio No." runat="server" />
                                    <br />
                                    <asp:TextBox ID="txtfolionoedit" CssClass="textb" Width="80px" runat="server" AutoPostBack="true"
                                        OnTextChanged="txtfolionoedit_TextChanged" />
                                </td>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="130px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Text="Process Name" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="DDProcessName" runat="server" CssClass="dropdown" Width="130px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Production Unit" CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="DDProdunit" runat="server" CssClass="dropdown" AutoPostBack="true"
                                        Width="130px" OnSelectedIndexChanged="DDProdunit_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td id="TDLoomNoDropdown" runat="server" visible="false">
                                                <asp:Label ID="Label15" runat="server" Text="Loom No." CssClass="labelbold"></asp:Label><br />
                                                <asp:DropDownList ID="DDLoomNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                                    Width="130px" OnSelectedIndexChanged="DDLoomNo_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="TDLoomNotextbox" runat="server">
                                                <asp:Label ID="Label8" Text=" Loom No." runat="server" CssClass="labelbold" />
                                                <asp:TextBox ID="txtloomid" runat="server" Style="display: none"></asp:TextBox>
                                                <br />
                                                <asp:TextBox ID="txtloomno" CssClass="textb" runat="server" Width="130px" />
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtenderloomno" runat="server" BehaviorID="LoomSrchAutoComplete"
                                                    CompletionInterval="20" Enabled="True" ServiceMethod="GetLoomNo" EnableCaching="true"
                                                    CompletionSetCount="30" OnClientItemSelected="Loomnoselected" ServicePath="~/Autocomplete.asmx"
                                                    TargetControlID="txtloomno" UseContextKey="true" ContextKey="0" MinimumPrefixLength="1">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 40%; float: right">
                        <table>
                            <tr>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblempcode" Text="Employee Code." runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="txtWeaverIdNo" CssClass="textb" runat="server" Width="130px" Visible="false" />
                                    <asp:TextBox ID="txtWeaverIdNoscan" CssClass="textb" runat="server" Width="130px"
                                        onKeypress="KeyDownHandlerWeaverIdscan(event);" />
                                    <asp:Button ID="btnweaveridscan" runat="server" Text="Button" Style="display: none;"
                                        OnClick="btnweaveridscan_Click" />
                                    <asp:TextBox ID="txtgetvalue" runat="server" Style="display: none"></asp:TextBox>
                                    <asp:Button ID="btnSearch" runat="server" Text="Button" OnClick="btnSearch_Click"
                                        Style="display: none;" />
                                    <asp:AutoCompleteExtender ID="txtWeaverIdNo_AutoCompleteExtender" runat="server"
                                        BehaviorID="SrchAutoComplete" CompletionInterval="20" Enabled="True" ServiceMethod="GetEmployeeAll"
                                        EnableCaching="true" CompletionSetCount="20" OnClientItemSelected="EmpSelected"
                                        ServicePath="~/Autocomplete.asmx" TargetControlID="txtWeaverIdNo" UseContextKey="True"
                                        ContextKey="0#0#0" MinimumPrefixLength="2">
                                    </asp:AutoCompleteExtender>
                                    <asp:DropDownList ID="DDemployee" CssClass="dropdown" Width="130px" runat="server"
                                        Visible="false" AutoPostBack="true" OnSelectedIndexChanged="DDemployee_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <div style="overflow: auto; width: 250px">
                                        <asp:ListBox ID="listWeaverName" runat="server" Width="240px" Height="100px" SelectionMode="Multiple">
                                        </asp:ListBox>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:LinkButton ID="BtnEmpPhoto" Text="Emp Photo" runat="server" CssClass="linkbuttonnew"
                                        OnClick="BtnEmpPhoto_Click" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="btnDelete" Text="Remove Employee" runat="server" CssClass="linkbuttonnew"
                                        OnClick="btnDelete_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right" id="TDactiveemployee" runat="server" visible="false">
                                    <asp:LinkButton ID="btnactiveemployee" Text="De-Active Employee" runat="server" CssClass="linkbuttonnew"
                                        OnClick="btnactiveemployee_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table>
                        <tr>
                            <td id="TDFolioNo" runat="server" visible="false">
                                <asp:Label ID="Label7" runat="server" Text="Folio No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDFolioNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDFolioNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Folio No." CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtfoliono" CssClass="textb" Width="90px" runat="server" Enabled="False" />
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Issue Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtissuedate" CssClass="textb" Width="95px" runat="server" />
                                <asp:CalendarExtender ID="cal1" TargetControlID="txtissuedate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Target Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txttargetdate" CssClass="textb" Width="95px" runat="server" />
                                <asp:CalendarExtender ID="cal2" TargetControlID="txttargetdate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td id="TDUNIT" runat="server">
                                <asp:Label ID="lblunit" CssClass="labelbold" Text="Unit" runat="server" />
                                <br />
                                <asp:DropDownList ID="ddunit" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddunit_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label11" Text=" Cal Type" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDcaltype" runat="server" Width="100px">
                                    <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                    <asp:ListItem Value="0">Area Wise</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <div style="width: 60%; float: left;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Buyer" CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="DDcustcode" runat="server" CssClass="dropdown" Width="150px"
                                        AutoPostBack="true" OnSelectedIndexChanged="DDcustcode_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="Order No." CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="DDorderNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                        Width="150px" OnSelectedIndexChanged="DDorderNo_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDCustomerCode" runat="server" visible="false">
                                    <asp:Label ID="Label12" runat="server" Text="Customer Code" CssClass="labelbold"></asp:Label><br />
                                    <asp:Label ID="lblCustomerCode" runat="server" CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td id="TDCustomerOrderNo" runat="server" visible="false">
                                    <asp:Label ID="Label13" runat="server" Text="Customer OrderNo" CssClass="labelbold"></asp:Label><br />
                                    <asp:Label ID="lblCustomerOrderNo" runat="server" CssClass="labelbold"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="border-style: none">
                                    <asp:CheckBox ID="chkexportsize" class="tdstyle" runat="server" Text="Export Size"
                                        AutoPostBack="true" CssClass="checkboxbold" OnCheckedChanged="chkexportsize_CheckedChanged" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label Text="Order Description" runat="server" CssClass="labelbold" ForeColor="Red" />
                    </legend>
                    <table>
                        <tr>
                            <td>
                                <div id="gride" runat="server" style="max-height: 300px; overflow: auto">
                                    <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                        EmptyDataText="No. Records found." OnRowDataBound="DG_RowDataBound">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
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
                                            <asp:TemplateField HeaderText="Order Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="400px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unit">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblunit" Text='<%#Bind("UnitName")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Width">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtwidth" Text='<%#Bind("Width") %>' Width="80px" runat="server"
                                                        AutoPostBack="true" OnTextChanged="Txtwidthlength_TextChanged" Font-Size="Small"
                                                        Font-Bold="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Length">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtlength" Text='<%#Bind("Length") %>' Width="80px" runat="server"
                                                        AutoPostBack="true" OnTextChanged="Txtwidthlength_TextChanged" Font-Size="Small"
                                                        Font-Bold="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Height">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtheight" Text='<%#Bind("height") %>' Width="80px" runat="server" Font-Size="Small"
                                                        Font-Bold="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Area">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblarea" Text='<%#Bind("area") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qty. Required">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblqtyreq" Text='<%#Bind("Qtyrequired") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ordered Qty.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblorderedQty" Text='<%#Bind("orderedqty") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pending Qty.">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label3" runat="server" Text='<%#Convert.ToInt32(Eval("Qtyrequired")) -Convert.ToInt32(Eval("orderedqty")) %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Loom Qty." Visible="true">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtloomqty" Width="70px" BackColor="Yellow" runat="server" Text='<%#Convert.ToInt32(Eval("Qtyrequired")) -Convert.ToInt32(Eval("orderedqty")) %>'
                                                        onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Production Rate">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtrate" Width="90px" BackColor="Yellow" runat="server" Text='<%#Bind("Rate") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Comm .Rate">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtcommrate" Width="90px" BackColor="Yellow" runat="server" Enabled="false"
                                                        Text='<%#Bind("Commrate") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblorderid" Text='<%#Bind("orderid") %>' runat="server" />
                                                    <asp:Label ID="lblitemfinishedid" Text='<%#Bind("item_finished_id") %>' runat="server" />
                                                    <asp:Label ID="lblunitid" Text='<%#Bind("OrderUnitId") %>' runat="server" />
                                                    <asp:Label ID="lblflagsize" Text='<%#Bind("flagsize") %>' runat="server" />
                                                    <asp:Label ID="lblOrderdetailid" Text='<%#Bind("orderDetailId") %>' runat="server" />
                                                    <asp:Label ID="lblshapeid" Text='<%#Bind("shapeid") %>' runat="server" />
                                                    <asp:Label ID="LblOrderDetailDetailID" Text='<%#Bind("OrderDetailDetailID") %>' runat="server" />
                                                    <asp:Label ID="LblOrderDetailDetail_Item_Finished_Id" Text='<%#Bind("OrderDetailDetail_Item_Finished_Id") %>'
                                                        runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <div>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblmessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                            </td>
                            <td align="right">
                                <asp:Button ID="BtnUpdateConsumption" runat="server" Text="Update Consumption" CssClass="buttonnorm"
                                    Visible="false" OnClick="BtnUpdateConsumption_Click" />
                                <asp:Button ID="BtnOrderProcessToPNM" runat="server" Text="Process To PNM" CssClass="buttonnorm"
                                    Visible="false" OnClick="BtnOrderProcessToPNM_Click" />
                                <asp:Button ID="BtnChampoPanipat" runat="server" Text="Champo Panipat" CssClass="buttonnorm"
                                    Visible="false" OnClick="BtnOrderProcessToChampoPanipat_Click" />
                                <asp:Button ID="BtnPanipatPNM1" runat="server" Text="FAB LIVING" CssClass="buttonnorm"
                                    Visible="false" OnClick="BtnOrderProcessToChampoPanipatPNM1_Click" />
                                <asp:Button ID="BtnPanipatPNM2" runat="server" Text="Panipat" CssClass="buttonnorm"
                                    Visible="false" OnClick="BtnOrderProcessToChampoPanipatPNM2_Click" />
                                <asp:Button ID="BtnChampoHome" runat="server" Text="Champo Home" CssClass="buttonnorm"
                                    Visible="false" OnClick="BtnOrderProcessToChampoPanipatPNM3_Click" />
                                <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click"
                                    UseSubmitBehavior="false" OnClientClick="if (!validatesave())return; this.disabled=true;this.value = 'wait ...';" />
                                <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hnissueorderid" runat="server" />
                </div>
                <div>
                    <div style="width: 100%; max-height: 250px; overflow: auto; float: left">
                        <asp:GridView ID="DGOrderdetail" runat="server" AutoGenerateColumns="False" CssClass="grid-views"
                            EmptyDataText="No Records found." OnRowDataBound="DGOrderdetail_RowDataBound"
                            OnRowEditing="DGOrderdetail_RowEditing" OnRowCancelingEdit="DGOrderdetail_RowCancelingEdit"
                            OnRowUpdating="DGOrderdetail_RowUpdating">
                            <HeaderStyle CssClass="gvheaders" />
                            <AlternatingRowStyle CssClass="gvalts" />
                            <RowStyle CssClass="gvrow" />
                            <Columns>
                                <asp:TemplateField HeaderText="OrderDescription">
                                    <ItemTemplate>
                                        <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="350px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Width">
                                    <ItemTemplate>
                                        <asp:Label ID="lblwidth" Text='<%#Bind("Width") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Length">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLength" Text='<%#Bind("Length") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Height" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblheight" Text='<%#Bind("Height") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qty">
                                    <ItemTemplate>
                                        <asp:Label ID="lblqty" Text='<%#Bind("Qty") %>' runat="server" />
                                    </ItemTemplate>
                                    <%--<EditItemTemplate>
                                        <asp:TextBox ID="txtqty" Text='<%#Bind("Qty") %>' Width="50px" runat="server" onkeypress="return isNumberKey(event);" />
                                    </EditItemTemplate>--%>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblhqty" Text='<%#Bind("Qty") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRate" Text='<%#Bind("Rate") %>' runat="server" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtrategrid" Width="70px" Text='<%#Bind("Rate") %>' runat="server" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Comm.Rate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcommrate" Text='<%#Bind("Comm") %>' runat="server" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtcommrategrid" Width="70px" Text='<%#Bind("comm") %>' runat="server" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Area">
                                    <ItemTemplate>
                                        <asp:Label ID="lblArea" Text='<%#Bind("Area") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblamount" Text='<%#Bind("Amount") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblissueorderid" Text='<%#Bind("Issueorderid") %>' runat="server" />
                                        <asp:Label ID="lblissuedetailid" Text='<%#Bind("IssueDetailId") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkdel" runat="server" Text="Del" OnClientClick="return confirm('Do you Want to delete this row?')"
                                            OnClick="lnkdelClick"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField EditText="Edit" ShowEditButton="True" CausesValidation="false">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:CommandField>
                            </Columns>
                            <EmptyDataRowStyle CssClass="gvemptytext" />
                        </asp:GridView>
                    </div>
                </div>
                <div style="clear: both">
                </div>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label21" Text="Remarks" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="TxtRemarks" runat="server" Width="500px" TextMode="MultiLine" Height="30px"
                                CssClass="textb"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label20" Text=" Instructions" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="TxtInstructions" runat="server" Width="800px" Height="50px" CssClass="textb"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <div>
                    <asp:Button runat="server" ID="btnModalPopUp" Style="display: none" />
                    <asp:ModalPopupExtender ID="ModalpopupextDeactivefolio" runat="server" PopupControlID="pnModelPopup"
                        TargetControlID="btnModalPopUp" BackgroundCssClass="modalBackground" CancelControlID="btnqcclose">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="pnModelPopup" runat="server" Style="background-color: ActiveCaption;
                        display: none">
                        <fieldset>
                            <legend>
                                <asp:Label ID="lblqc" Text="De-Active Folio Employee" runat="server" ForeColor="Red"
                                    CssClass="labelbold" />
                            </legend>
                            <table>
                                <tr>
                                    <td>
                                        <div style="max-height: 500px; overflow: auto">
                                            <asp:GridView ID="GVDetail" CssClass="grid-views" runat="server" AutoGenerateColumns="false"
                                                OnRowDataBound="GVDetail_RowDataBound">
                                                <HeaderStyle CssClass="gvheaders" Font-Size="12px" />
                                                <AlternatingRowStyle CssClass="gvalts" />
                                                <RowStyle CssClass="gvrow" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="Chkboxitem" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Employeee">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblemployee" Text='<%#Bind("Employee") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblissueorderid" Text='<%#Bind("Issueorderid") %>' runat="server" />
                                                            <asp:Label ID="lblactivestatus" Text='<%#Bind("activestatus") %>' runat="server" />
                                                            <asp:Label ID="lblempid" Text='<%#Bind("empid") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btnemployeesave" Text="De-Active" runat="server" CssClass="buttonnorm"
                                            OnClick="btnemployeesave_Click" />
                                        <asp:Button ID="btnqcclose" Text="Close" runat="server" CssClass="buttonnorm" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblpopupmsg" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </asp:Panel>
                </div>
                <asp:HiddenField ID="hnEmpWagescalculation" Value="" runat="server" />
                <asp:HiddenField ID="hnEmployeeType" Value="" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
