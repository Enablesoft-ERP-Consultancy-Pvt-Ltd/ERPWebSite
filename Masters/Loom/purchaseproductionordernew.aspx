<%@ Page Title="PRODUCTION ORDER" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeFile="purchaseproductionordernew.aspx.cs" Inherits="Masters_Loom_frmproductionorderonLoom" %>

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
        function Loomnoselected(source, eventArgs) {
            document.getElementById('<%=txtloomid.ClientID%>').value = eventArgs.get_value();
            document.getElementById('<%=hnissueorderid.ClientID%>').value = "0";
        }
        function NewForm() {
            if (document.getElementById('CPH_Form_chkEdit') == null) {
                window.location.href = "purchaseproductionorderagni.aspx";
            }
            else if (document.getElementById('CPH_Form_chkEdit').disabled == true && document.getElementById('CPH_Form_chkEdit').checked == true) { //here
                window.location.href = "purchaseproductionorderagni.aspx?ForEdit=1";
            } else {
                window.location.href = "purchaseproductionorderagni.aspx";
            }
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
//            selectedindex = $("#<%=DDProdunit.ClientID %>").attr('selectedIndex');
//            if (selectedindex <= 0) {
//                Message = Message + "Please Select Production Unit. !!\n";
//            }
//            if (document.getElementById("<%=TDLoomNoDropdown.ClientID %>")) {
//                selectedindex = $("#<%=DDLoomNo.ClientID %>").attr('selectedIndex');
//                if (selectedindex <= 0) {
//                    Message = Message + "Please select Loom No. !!\n";
//                }
//            }
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
    <script language="javascript" type="text/javascript">

        function setScroll(val) {

            document.getElementById('<%=scrollPos.ClientID%>').value = val.scrollTop;

        }

        function scrollTo(what) {

            if (what != "0")

                document.getElementById(what).scrollTop = document.getElementById("scrollPos").value;

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
                            <tr id="TRPrefixpostfix" runat="server" visible="false">
                                <td align="center" colspan="2">
                                    <asp:Label ID="lbl" Text="PreFix" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="TxtPrefix" runat="server" CssClass="textb" AutoPostBack="true" OnTextChanged="TxtPrefix_TextChanged"></asp:TextBox>
                                </td>
                                <td align="center" colspan="2">
                                    <asp:Label ID="Label10" Text=" No." runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="TxtPostfix" runat="server" CssClass="textb" AutoPostBack="true"
                                        OnTextChanged="TxtPostfix_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td id="TDEdit" runat="server" visible="false">
                                    <asp:CheckBox ID="chkEdit" Text="For Edit" CssClass="checkboxbold" runat="server"
                                        AutoPostBack="true" OnCheckedChanged="chkEdit_CheckedChanged" />
                                </td>
                                <td id="TDComplete" runat="server" visible="false">
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
                                    <asp:Label CssClass="labelbold" Text="Production Order No." runat="server" />
                                    <br />
                                    <asp:TextBox ID="txtfolionoedit" CssClass="textb" Width="80px" runat="server" AutoPostBack="true"
                                        OnTextChanged="txtfolionoedit_TextChanged" />
                                </td>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="120px">
                                    </asp:DropDownList>
                                </td>
                                <td class="tdstyle">
                                    <asp:Label ID="Label33" runat="server" Text="Branch Name" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList CssClass="dropdown" ID="DDBranchName" Width="120" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                <asp:Label ID="Label19" runat="server" Text="Vendor Name" CssClass="labelbold"></asp:Label><br />
                                  <asp:DropDownList ID="ddempname" runat="server" CssClass="dropdown" AutoPostBack="True"
                                            Width="150px" OnSelectedIndexChanged="ddempname_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    <asp:Label ID="Label1" runat="server" Visible="false" Text="Production Unit" CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="DDProdunit" Visible="false"  runat="server" CssClass="dropdown" AutoPostBack="true"
                                        Width="120px" OnSelectedIndexChanged="DDProdunit_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                 <td class="tdstyle">
                                    <asp:Label ID="lblshipto" runat="server" Text="Ship To" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList CssClass="dropdown" ID="ddlshipto" Width="120" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td id="TDLoomNoDropdown" runat="server" visible="false">
                                                <asp:Label ID="Label15" runat="server" Text="Loom No." CssClass="labelbold"></asp:Label><br />
                                                <asp:DropDownList ID="DDLoomNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                                    Width="150px" OnSelectedIndexChanged="DDLoomNo_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="TDLoomNotextbox" visible="false" runat="server">
                                                <asp:Label ID="Label8" Text=" Loom No." runat="server" CssClass="labelbold" />
                                                <asp:TextBox ID="txtloomid" runat="server" Style="display: none"></asp:TextBox>
                                                <br />
                                                <asp:TextBox ID="txtloomno" CssClass="textb" runat="server" Width="150px" />
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtenderloomno" runat="server" BehaviorID="LoomSrchAutoComplete"
                                                    CompletionInterval="20" Enabled="True" ServiceMethod="GetLoomNo" EnableCaching="true"
                                                    CompletionSetCount="30" OnClientItemSelected="Loomnoselected" ServicePath="~/Autocomplete.asmx"
                                                    TargetControlID="txtloomno" UseContextKey="true" ContextKey="0" MinimumPrefixLength="1">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td id="TDTanaLotNo" runat="server" visible="false">
                                    <asp:Label ID="Label11" Text="Cotton LotNo." runat="server" CssClass="labelbold" /><br />
                                    <asp:TextBox ID="txtTanaLotNo" CssClass="textb" runat="server" Width="150px" />
                                    <br />
                                    <asp:LinkButton ID="BtnUpdateTanaLotNo" Text="Update Cotton LotNo" runat="server"
                                        CssClass="linkbuttonnew" OnClick="BtnUpdateTanaLotNo_Click" Visible="false" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 40%; float: right;visibility:hidden;" >
                        <table>
                            <tr>
                                <td style="vertical-align: top;">
                                    <asp:Label ID="lblempcode" Text="Vendor Name." runat="server" CssClass="labelbold" />
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
                                    <div style="overflow: auto; width: 210px">
                                        <asp:ListBox ID="listWeaverName" runat="server" Width="200px" Height="80px" SelectionMode="Multiple">
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
                                <td colspan="2" align="right" id="TDupdateemp" runat="server" visible="false">
                                    <asp:LinkButton ID="btnupdateemp" Text="Update Folio Employee" runat="server" CssClass="linkbuttonnew"
                                        OnClick="btnupdateemp_Click" />
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
                                <asp:Label ID="Label7" runat="server" Text="Production Order No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDFolioNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDFolioNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Production Order No." CssClass="labelbold"></asp:Label><br />
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
                                <asp:Label ID="LblCalType" CssClass="labelbold" Text="Cal Type" runat="server" />
                                <br />
                                <asp:DropDownList ID="DDCalType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDCalType_SelectedIndexChanged"
                                    CssClass="dropdown">
                                    <asp:ListItem Value="0">Area Wise</asp:ListItem>
                                    <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td id="TDDepartmentName" runat="server" visible="false">
                                <asp:Label ID="Label16" CssClass="labelbold" Text="Department Name" runat="server" />
                                <br />
                                <asp:DropDownList ID="DDDepartmentName" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDDepartmentName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table border="1" cellspacing="7">
                        <tr>
                            <td id="TDcheckpurchasefolio" visible="false" runat="server" style="border-style: none">
                                <asp:CheckBox ID="chkpurchasefolio" runat="server" Text="Purchase Folio" CssClass="checkboxbold" />
                            </td>
                            <td style="display:none" style="border-style: none">
                                <asp:CheckBox ID="ChkForFix" class="tdstyle" runat="server" Text="For Fix" CssClass="checkboxbold" />
                            </td>
                            <td style="display:none" style="text-align: right; border-style: none">
                                <asp:CheckBox ID="chkforRateUpdate" AutoPostBack="true" Text="For Rate Update" runat="server"
                                    CssClass="checkboxbold" OnCheckedChanged="chkforRateUpdate_CheckedChanged" />
                            </td>
                            <td style="display:none" style="border-style: none">
                                <asp:CheckBox ID="chkexportsize" class="tdstyle" runat="server" Text="Export Size"
                                    AutoPostBack="true" CssClass="checkboxbold" OnCheckedChanged="chkexportsize_CheckedChanged" />
                            </td>
                            <td id="TDPcsWise" runat="server" visible="false" style="border-style: none">
                                <asp:CheckBox ID="ChkForPcsWise" class="tdstyle" runat="server" Text="For Pcs Wise"
                                    CssClass="checkboxbold" />
                            </td>
                            <td id="TDChkForStockNoAttachWithoutMaterialIssue" runat="server" visible="false"
                                style="border-style: none">
                                <asp:CheckBox ID="ChkForStockNoAttachWithoutMaterialIssue" class="tdstyle" runat="server"
                                    Text="Without Material" CssClass="checkboxbold" />
                            </td>
                            <td id="TDChkForStockNoAttach" runat="server" visible="false" style="border-style: none">
                                <asp:CheckBox ID="ChkForStockNoAttach" class="tdstyle" runat="server" Text="For StockNo Attach"
                                    CssClass="checkboxbold" />
                            </td>
                            <td id="TDChkForMaterialRate" runat="server" visible="false"
                                style="border-style: none">
                                <asp:CheckBox ID="ChkForMaterialRate" class="tdstyle" runat="server"
                                    Text="Material Rate" CssClass="checkboxbold" />
                            </td>
                        </tr>
                    </table>
                    
                    <div style="width: 60%; float: left;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblbuyer" runat="server" Text="Buyer" CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="DDcustcode" runat="server" CssClass="dropdown" Width="150px"
                                        AutoPostBack="true" OnSelectedIndexChanged="DDcustcode_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDDepartmentIssueNo" runat="server" visible="false">
                                    <asp:Label ID="Label17" CssClass="labelbold" Text="Department Issue No" runat="server" />
                                    <br />
                                    <asp:DropDownList ID="DDDepartmentIssueNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                        Width="150px" OnSelectedIndexChanged="DDDepartmentIssueNo_SelectedIndexChanged">
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
                                <td id="tdprocess" runat="server" visible="false">
                                    <asp:CheckBox ID="chkprocessitem" Text="Cushion" runat="server" />
                                </td>
                                 <td id="tdwithoutorder" runat="server" visible="false">
                                    <asp:CheckBox ID="chkwithoutorder" AutoPostBack="true" OnCheckedChanged="chkwithoutorder_CheckedChanged"  Text="Without Order" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                    
                    <div style="width: 40%; float: right">
                        <table>
                            <tr>
                                <td id="TDTanaCottonLotNo" runat="server" visible="false">
                                    <asp:Label ID="Label14" Text="Available Cotton LotNo." runat="server" CssClass="labelbold" /><br />
                                    <div style="max-height: 100px; overflow: scroll; width: 150px">
                                        <asp:GridView ID="DGTanaCottonLotNo" AutoGenerateColumns="false" CssClass="grid-views"
                                            runat="server" EmptyDataText="No CottonLotNo Data Fetched.">
                                            <HeaderStyle CssClass="gvheaders" />
                                            <AlternatingRowStyle CssClass="gvalts" />
                                            <RowStyle CssClass="gvrow" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="LotNo">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCottonLotNo" Text='<%#Bind("LotNo") %>' runat="server" Width="200px" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStockId" Text='<%#Bind("stockId") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                                  <td id="tdweaverpedningstock" runat="server" visible="false">
                                    <asp:Label ID="Label18" Text="Weaver Pending Stock." runat="server" CssClass="labelbold" />
                                    <div style="height:100px; overflow: scroll; width:278px">
                                        <asp:GridView ID="grdpendingstock" AutoGenerateColumns="false" CssClass="grid-views"
                                            runat="server" EmptyDataText="No Pending Stocks.">
                                            <HeaderStyle CssClass="gvheaders" />
                                            <AlternatingRowStyle CssClass="gvalts" />
                                            <RowStyle CssClass="gvrow" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Design">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblweaverdesign" Text='<%#Bind("DESIGNNAME") %>' runat="server" Width="200px" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Count">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblweaverpendingcount" Text='<%#Bind("PQTY") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>


                            </tr>
                        </table>
                    </div>
                </fieldset>
                <div>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label5" Text="Item Details" runat="server" CssClass="labelbold" ForeColor="Red" />
                    </legend>
                    <table>
                        <tr id="Tr3" runat="server">
                            <td id="tdProCode" runat="server" visible="false">
                                <span class="labelbold">ProdCode</span>
                                <br />
                                <asp:TextBox ID="TxtProdCode" CssClass="textb" runat="server" Width="136px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblcategoryname" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddCatagory" runat="server" Width="150px" CssClass="dropdown"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblitemname" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="dditemname" runat="server" Width="150px" CssClass="dropdown"
                                    AutoPostBack="True" OnSelectedIndexChanged="dditemname_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDQuality" runat="server" visible="false">
                                <asp:Label ID="lblqualityname" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="dquality" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td id="TDDesign" runat="server" visible="false" class="style5">
                                <asp:Label ID="lbldesignname" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="dddesign" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="TDColor" runat="server" visible="false">
                                <asp:Label ID="lblcolorname" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddcolor" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td id="TDShape" runat="server" visible="false">
                                <asp:Label ID="lblshapename" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddshape" runat="server" Width="150px" AutoPostBack="True" CssClass="dropdown"
                                    OnSelectedIndexChanged="ddshape_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDSize" runat="server" visible="false">
                                <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged" Visible="false">
                                </asp:DropDownList>
                                <asp:CheckBox ID="CheckBox1" Text="ExportSize" runat="server" AutoPostBack="true"
                                    CssClass="checkboxbold" OnCheckedChanged="chkexportsize_CheckedChanged" />
                                <br />
                                <asp:DropDownList ID="ddsize" runat="server" Width="150px" CssClass="dropdown" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddsize_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDShade" runat="server" visible="false">
                                <asp:Label ID="lblshadecolor" runat="server" Text="ShadeColor" CssClass="labelbold"></asp:Label>
                                &nbsp;<br />
                                <asp:DropDownList ID="ddlshade" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td colspan="3">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblprodnqty" runat="server" Text="Production Qty" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtprodqty" CssClass="textb" runat="server" Width="70px" 
                                                />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblrate" runat="server" Text="Rate" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtrateo" CssClass="textb" runat="server" Width="70px" AutoPostBack="true"
                                                OnTextChanged="txtrateo_TextChanged" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblamount" runat="server" Text="Amount" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtamount" CssClass="textb" runat="server" Enabled="false" BackColor="Yellow"
                                                Width="70px" />
                                        </td>
                                        
                                        <td>
                                            <asp:Label ID="lblGSTType" runat="server" Text="GST Type" CssClass="labelbold"></asp:Label>
                                            <br />
                                          <asp:DropDownList ID="ddltypegst" CssClass="dropdown" Width="150px" runat="server" OnSelectedIndexChanged="ddltypegst_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                         <asp:ListItem Value="0" Selected="True">---Select----</asp:ListItem>
                                            <asp:ListItem Value="1">CGST/SGST</asp:ListItem>
                                            <asp:ListItem Value="2">IGST</asp:ListItem>
                                                    </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblcgst" runat="server" Text="CGST" CssClass="labelbold"></asp:Label>
                                            &nbsp;<br />
                                             <asp:TextBox ID="TXTCGST" runat="server" Width="70px" Enabled="false" AutoPostBack="true"  onkeypress="return isNumberKey(event)"></asp:TextBox>
                                        </td>
                                         <td>
                                            <asp:Label ID="lbligst" runat="server" Text="CGST" CssClass="labelbold"></asp:Label>
                                            &nbsp;<br />
                                             <asp:TextBox ID="TXTIGST" runat="server" Width="70px" Enabled="false" AutoPostBack="true"  onkeypress="return isNumberKey(event)"></asp:TextBox>
                                        </td>
                                         <td>
                                            <asp:Label ID="lblsgst" runat="server" Text="CGST" CssClass="labelbold"></asp:Label>
                                            &nbsp;<br />
                                             <asp:TextBox ID="TXTSGST" runat="server" Width="70px" Enabled="false" AutoPostBack="true"  onkeypress="return isNumberKey(event)"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
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
                          
            <input id="scrollPos" runat="server" type="hidden" value="0"  />
                                <div id="gride" runat="server" style="max-height: 300px; overflow: auto" onscroll='javascript:setScroll(this);'>
                                    <asp:GridView ID="DG"  AutoGenerateColumns="False" runat="server" CssClass="grid-views"
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
                                            <asp:TemplateField HeaderText="Purchase Qty." Visible="true">
                                                <ItemTemplate>
                                                    <%--<asp:TextBox ID="txtloomqty" Width="70px" BackColor="Yellow" runat="server" onkeypress="return isNumberKey(event);" />--%>
                                                    <asp:TextBox ID="txtloomqty" Width="70px" BackColor="Yellow" runat="server" Text='<%#Convert.ToInt32(Eval("Qtyrequired")) -Convert.ToInt32(Eval("orderedqty")) %>'
                                                        onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rate">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtrate" Width="90px" BackColor="Yellow" runat="server" AutoPostBack="true" OnTextChanged="txtrate_TextChanged"
                                                        Text='<%#Bind("Rate") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false" HeaderText="Comm .Rate">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtcommrate" Width="90px" BackColor="Yellow" runat="server" Enabled="false"
                                                        Text='<%#Bind("Commrate") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false" HeaderText="Bonus">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtBonus" Width="90px" BackColor="Yellow" runat="server" Enabled="false"
                                                        Text='<%#Bind("Bonus") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false" HeaderText="F Rate">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtFinisherRate" Width="90px" BackColor="Yellow" runat="server" Enabled="false"
                                                        Text='<%#Bind("FinisherRate") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblamount" runat="server" Text='<%#(Convert.ToInt32(Eval("Qtyrequired")) -Convert.ToInt32(Eval("orderedqty"))) * Convert.ToInt32(Eval("rate"))%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="CGST %" Visible="true">
                                <ItemTemplate>
                                    <asp:DropDownList ID="gsttype" CssClass="dropdown" Width="150px" runat="server" OnSelectedIndexChanged="gsttype_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                         <asp:ListItem Value="0" Selected="True">---Select----</asp:ListItem>
                                            <asp:ListItem Value="1">CGST/SGST</asp:ListItem>
                                            <asp:ListItem Value="2">IGST</asp:ListItem>
                                                    </asp:DropDownList>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CGST %" Visible="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="TXTCGST" runat="server" Width="70px" Enabled="false" AutoPostBack="true"  onkeypress="return isNumberKey(event)"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SGST %" Visible="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="TXTSGST" runat="server" Width="70px" Enabled="false" AutoPostBack="true"  onkeypress="return isNumberKey(event)"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IGST %" Visible="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="TXTIGST" runat="server" Width="70px"  Enabled="false" AutoPostBack="true" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Total Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbltotalamt" runat="server" Text='<%#(Convert.ToInt32(Eval("Qtyrequired")) -Convert.ToInt32(Eval("orderedqty"))) * Convert.ToInt32(Eval("rate"))%>'></asp:Label>
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
                                                    <asp:Label ID="lblcategoryid" Text='<%#Bind("CATEGORY_ID") %>' runat="server" />
                                                    <asp:Label ID="lblitemid" Text='<%#Bind("item_id") %>' runat="server" />
                                                    <asp:Label ID="lblqualityid" Text='<%#Bind("qualityid") %>' runat="server" />
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
                                <asp:CheckBox ID="ChkForDyeingConsumption" runat="server" Text="Check For Dyeing Consumption"
                                    class="tdstyle" Visible="false" CssClass="checkboxbold" />
                                <asp:CheckBox ID="ChkForWithoutRate" runat="server" Text="Without Rate Print" class="tdstyle"
                                    Visible="false" CssClass="checkboxbold" />
                                <asp:CheckBox ID="ChkForSlipPrint" runat="server" Text="For Slip Print" class="tdstyle"
                                    Visible="false" CssClass="checkboxbold" />
                                <asp:Button ID="BtnOrderProcessToPNM" runat="server" Text="PNM INC." CssClass="buttonnorm"
                                    Visible="false" OnClick="BtnOrderProcessToPNM_Click" />
                                <asp:Button ID="BtnChampoPanipat" runat="server" Text="Champo Panipat" CssClass="buttonnorm"
                                    Visible="false" OnClick="BtnOrderProcessToChampoPanipat_Click" />
                                <asp:Button ID="BtnPanipatPNM1" runat="server" Text="FAB LIVING" CssClass="buttonnorm"
                                    Visible="false" OnClick="BtnOrderProcessToChampoPanipatPNM1_Click" />
                                <asp:Button ID="BtnPanipatPNM2" runat="server" Text="Panipat" CssClass="buttonnorm"
                                    Visible="false" OnClick="BtnOrderProcessToChampoPanipatPNM2_Click" />
                                <asp:Button ID="BtnChampoHome" runat="server" Text="Champo Home" CssClass="buttonnorm"
                                    Visible="false" OnClick="BtnOrderProcessToChampoPanipatPNM3_Click" />
                                <asp:Button ID="BtnStockNoStatus" runat="server" Text="StockNo Status" CssClass="buttonnorm"
                                    Visible="false" OnClick="StockNoStatus_Click" />
                                     <asp:CheckBox ID="chksummary" runat="server"  Text="Summary" />
                                <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                                <asp:Button ID="btnupdateconsmp" runat="server" Text="Update consumption" CssClass="buttonnorm"
                                    Visible="false" OnClientClick="return cancelvalidation();" OnClick="btnupdateconsmp_Click" />
                                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click"
                                    UseSubmitBehavior="false" OnClientClick="if (!validatesave())return; this.disabled=true;this.value = 'wait ...';" />
                                <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                                <asp:Button ID="BtnPreviewConsumption" runat="server" Text="Preview Consumption"
                                    CssClass="buttonnorm" OnClick="BtnPreviewConsumption_Click" Visible="false" />
                                <asp:Button ID="btncancel" runat="server" Text="Cancel Order" CssClass="buttonnorm"
                                    Visible="false" OnClientClick="return cancelvalidation();" OnClick="btncancel_Click" />
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
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtqty" Text='<%#Bind("Qty") %>' Width="50px" runat="server" onkeypress="return isNumberKey(event);" />
                                    </EditItemTemplate>
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
                                <asp:TemplateField Visible="false" HeaderText="Comm.Rate">
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
                                 <asp:TemplateField HeaderText="CGST %" Visible="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="TXTCGST" runat="server" Width="50px" Enabled="false" AutoPostBack="true" Text='<%#Bind("CGST") %>'  onkeypress="return isNumberKey(event)"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SGST %" Visible="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="TXTSGST" runat="server" Width="50px" Enabled="false" AutoPostBack="true" Text='<%#Bind("SGST") %>'  onkeypress="return isNumberKey(event)"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IGST %" Visible="true">
                                <ItemTemplate>
                                    <asp:TextBox ID="TXTIGST" runat="server" Width="50px"  Enabled="false" AutoPostBack="true" Text='<%#Bind("IGST") %>' onkeypress="return isNumberKey(event)"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle Width="80px" />
                            </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lbltotalamount" Text='<%#Bind("totalamount") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bonus">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBonus" Text='<%#Bind("Bonus") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <%--<asp:TemplateField HeaderText="Bonus">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBonus" Text='<%#Bind("stockno") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Finisher Rate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFinisherRate" Text='<%#Bind("FinisherRate") %>' runat="server" />
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
                    <div style="max-height: 400px; overflow: auto; width: 39%; float: right">
                        <asp:GridView ID="DGConsumption" runat="server" AutoGenerateColumns="False" EmptyDataText="No Raw Material Details found.">
                            <HeaderStyle CssClass="gvheaders" />
                            <AlternatingRowStyle CssClass="gvalts" />
                            <RowStyle CssClass="gvrow" />
                            <PagerStyle CssClass="PagerStyle" />
                            <EmptyDataRowStyle CssClass="gvemptytext" />
                            <Columns>
                                <asp:BoundField DataField="ItemDescription" HeaderText="Raw Material Description">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="250px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ConsmpQTY" HeaderText="Consmp Qty">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Issuedqty" HeaderText="Issued Qty">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="UnitName" HeaderText="Unit">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
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

                                <asp:LinkButton ID="BtnUpdateRemark" Text="Update Remarks" runat="server"
                                        CssClass="linkbuttonnew" OnClick="BtnUpdateRemark_Click" Visible="false" />
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
           <Triggers>
           <%-- <asp:PostBackTrigger ControlID="DG"  />--%>
          

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
