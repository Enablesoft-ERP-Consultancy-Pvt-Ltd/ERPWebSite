<%@ Page Title="STITCHING ORDER" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmFirstProcessOrder.aspx.cs" Inherits="Masters_HomeFurnishing_FrmFirstProcessOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
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
        function NewForm() {
            window.location.href = "FrmFirstProcessOrder.aspx";
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
        function KeyDownHandler(event) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                window.document.getElementById('<%=btnStockNo.ClientID %>').click();
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
    <div>
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
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
                                <td id="TDComplete" runat="server" visible="false">
                                    <asp:CheckBox ID="chkcomplete" Text="For Complete" CssClass="checkboxbold" runat="server"
                                        AutoPostBack="true" OnCheckedChanged="chkcomplete_CheckedChanged" />
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Process Name" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="DDProcessName" runat="server" CssClass="dropdown" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDFolioNo" runat="server" visible="false">
                                    <asp:Label ID="Label7" runat="server" Text="Folio No." CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="DDFolioNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                        Width="150px" OnSelectedIndexChanged="DDFolioNo_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
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
                                    <asp:Label ID="Label10" CssClass="labelbold" Text="Folio No." runat="server" />
                                    <br />
                                    <asp:TextBox ID="txtfolionoedit" CssClass="textb" Width="80px" runat="server" AutoPostBack="true"
                                        OnTextChanged="txtfolionoedit_TextChanged" />
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
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
                                    <asp:DropDownList ID="ddunit" runat="server" CssClass="dropdown">
                                    </asp:DropDownList>
                                </td>
                                <td id="TD1" runat="server">
                                    <asp:Label ID="Label8" CssClass="labelbold" Text="Cal Type" runat="server" />
                                    <br />
                                    <asp:DropDownList CssClass="dropdown" ID="DDcaltype" runat="server" Width="100px"
                                        AutoPostBack="true" OnSelectedIndexChanged="DDcaltype_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Area Wise</asp:ListItem>
                                        <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
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
                            </tr>
                        </table>
                    </div>
                    <div style="width: 40%; float: right">
                        <table>
                            <tr>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblempcode" Text="Employee Code." runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="txtWeaverIdNo" CssClass="textb" runat="server" Width="150px" Visible="false" />
                                    <asp:TextBox ID="txtWeaverIdNoscan" CssClass="textb" runat="server" Width="150px"
                                        Visible="false" onKeypress="KeyDownHandlerWeaverIdscan(event);" />
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
                                    <asp:DropDownList ID="DDemployee" CssClass="dropdown" Width="150px" runat="server"
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
                                    <asp:LinkButton ID="btnDelete" Text="Remove Employee" runat="server" CssClass="linkbuttonnew"
                                        OnClick="btnDelete_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 60%; float: left;">
                    </div>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label Text="Order Description" runat="server" CssClass="labelbold" ForeColor="Red" />
                    </legend>
                    <table>
                        <tr>
                            <td id="Tdstockno" runat="server" visible="false">
                                <table border="1" cellspacing="5">
                                    <tr>
                                        <td>
                                            <asp:Label Text="Enter Stock No." CssClass="labelbold" Font-Size="Small" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtstockno" CssClass="textb" Height="40px" Width="250px" runat="server"
                                                onKeypress="KeyDownHandler(event);" />
                                            <asp:Button ID="btnStockNo" runat="server" Style="display: none" OnClick="txtstockno_TextChanged" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
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
                                                    <asp:Label ID="lblWidth" Text='<%#Bind("Width")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Length">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLength" Text='<%#Bind("Length")%>' runat="server" />
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
                                                    <%--<asp:TextBox ID="txtloomqty" Width="70px" BackColor="Yellow" runat="server" onkeypress="return isNumberKey(event);" />--%>
                                                    <asp:TextBox ID="txtloomqty" Width="70px" BackColor="Yellow" runat="server" Text='<%#Convert.ToInt32(Eval("Qtyrequired")) -Convert.ToInt32(Eval("orderedqty")) %>'
                                                        onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Production Rate">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtrate" Width="90px" BackColor="Yellow" runat="server" Enabled="false"
                                                        Text='<%#Bind("Rate") %>' />
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
                    <div style="width: 60%; max-height: 250px; overflow: auto; float: left">
                        <asp:GridView ID="DGOrderdetail" runat="server" AutoGenerateColumns="False" CssClass="grid-views"
                            EmptyDataText="No Records found." OnRowEditing="DGOrderdetail_RowEditing" OnRowCancelingEdit="DGOrderdetail_RowCancelingEdit"
                            OnRowUpdating="DGOrderdetail_RowUpdating" OnRowDataBound="DGOrderdetail_RowDataBound">
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
                                <asp:TemplateField HeaderText="Qty">
                                    <ItemTemplate>
                                        <asp:Label ID="lblqty" Text='<%#Bind("Qty") %>' runat="server" />
                                    </ItemTemplate>
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
                                        <asp:Label ID="lblissuedetailid" Text='<%#Bind("issue_Detail_Id") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField EditText="Edit" ShowEditButton="True" CausesValidation="false">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:CommandField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkdel" runat="server" Text="Del" OnClientClick="return confirm('Do you Want to delete this row?')"
                                            OnClick="lnkdelClick"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataRowStyle CssClass="gvemptytext" />
                        </asp:GridView>
                    </div>
                </div>
                <div>
                    <div style="width: 60%; max-height: 250px; overflow: auto; float: left">
                        <asp:GridView ID="DGOrderdetailStockNo" runat="server" AutoGenerateColumns="False"
                            CssClass="grid-views" EmptyDataText="No Records found." OnRowDataBound="DGOrderdetailStockNo_RowDataBound">
                            <HeaderStyle CssClass="gvheaders" />
                            <AlternatingRowStyle CssClass="gvalts" />
                            <RowStyle CssClass="gvrow" />
                            <Columns>
                                <asp:BoundField DataField="ItemDescription" HeaderText="Item Description">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="450px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TStockNo" HeaderText="Stock No">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                </asp:BoundField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="LblStockNo" Text='<%#Bind("StockNo") %>' runat="server" />
                                        <asp:Label ID="lblissueorderid" Text='<%#Bind("Issueorderid") %>' runat="server" />
                                        <asp:Label ID="lblissuedetailid" Text='<%#Bind("issue_Detail_Id") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkdel" runat="server" Text="Del" OnClientClick="return confirm('Do you Want to delete this row?')"
                                            OnClick="lnkStockNodelClick"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataRowStyle CssClass="gvemptytext" />
                        </asp:GridView>
                    </div>
                    <div>
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
                <asp:HiddenField ID="hnordercaltype" Value="" runat="server" />
                <asp:HiddenField ID="hnEmpWagescalculation" Value="" runat="server" />
                <asp:HiddenField ID="hnEmployeeType" Value="" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
