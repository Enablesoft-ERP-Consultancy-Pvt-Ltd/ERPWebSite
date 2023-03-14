<%@ Page Title="Yarn Opening Receive" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmyarnopeningReceiveNew.aspx.cs" Inherits="Masters_YarnOpening_frmyarnopeningReceiveNew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="CPH" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmyarnopeningReceive.aspx";
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
        function CheckBoxClick(objref) {

            var row = objref.parentNode.parentNode;
            if (objref.checked) {
                row.style.backgroundColor = "Orange";
            }
            else {
                row.style.backgroundColor = "White";
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
        function Validatesave() {
            debugger;
            var Message = "";

            var selectedindex = $("#<%=DDcompany.ClientID %>").attr('selectedIndex');
            if (selectedindex < 0) {
                Message = Message + "Please select Company Name!!\n";
            }
            selectedindex = $("#<%=DDvendor.ClientID %>").attr('selectedIndex');
            if (selectedindex < 0) {
                Message = Message + "Please Select Dept. !!\n";
            }
            selectedindex = $("#<%=DDemployee.ClientID %>").attr('selectedIndex');
            if (selectedindex <= 0) {
                Message = Message + "Please Select Employee !!\n";
            }
            if ($("#<%=TDGodown.ClientID %>").is(':visible')) {
                selectedindex = $("#<%=DDGodown.ClientID %>").attr('selectedIndex');
                if (selectedindex <= 0) {
                    Message = Message + "Please select Godown !!\n";
                }
            }
            if ($("#<%=TDBinNo.ClientID %>").is(':visible')) {
                var selectedindex = $("#<%=DDBinNo.ClientID %>").attr('selectedIndex');
                if (selectedindex <= 0) {
                    Message = Message + "Please select Bin No. !!\n";
                }
            }
            if (Message == "") {

                var grid = document.getElementById("<%=DG.ClientID%>");
                if (grid != null) {


                    var inputs = grid.getElementsByTagName("input");
                    for (var i = 0; i < inputs.length; i++) {
                        if (inputs[i].type == "checkbox") {
                            var checkid = inputs[i].id;
                            if (checkid.match(/CPH_Form_DG_Chkboxitem_.*/)) {
                                var checked = inputs[i].checked;
                            }
                        }
                        if (checked) {
                            if (inputs[i].type == "text") {
                                var id = inputs[i].id;
                                if (id.match(/CPH_Form_DG_txtrecqty_.*/)) {
                                    // do something
                                    if (inputs[i].value == "" || inputs[i].value == "0") {
                                        alert("Rec Qty can not be blank or zero.");
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
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
                $("#<%=btnsave.ClientID %>").click(function () {
                    var Message = "";

                    var selectedindex = $("#<%=DDcompany.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Company Name!!\n";
                    }
                    selectedindex = $("#<%=DDvendor.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please Select Dept. !!\n";
                    }
                    selectedindex = $("#<%=DDemployee.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Employee !!\n";
                    }
                    if ($("#<%=TDGodown.ClientID %>").is(':visible')) {
                        selectedindex = $("#<%=DDGodown.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Godown !!\n";
                        }
                    }
                    if ($("#<%=TDBinNo.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDBinNo.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Bin No. !!\n";
                        }
                    }
                    if (Message == "") {

                        var grid = document.getElementById("<%=DG.ClientID%>");
                        var inputs = grid.getElementsByTagName("input");
                        for (var i = 0; i < inputs.length; i++) {
                            if (inputs[i].type == "checkbox") {
                                var checkid = inputs[i].id;
                                if (checkid.match(/CPH_Form_DG_Chkboxitem_.*/)) {
                                    var checked = inputs[i].checked;
                                }
                            }
                            if (checked) {
                                if (inputs[i].type == "text") {
                                    var id = inputs[i].id;
                                    if (id.match(/CPH_Form_DG_txtrecqty_.*/)) {
                                        // do something
                                        if (inputs[i].value == "" || inputs[i].value == "0") {
                                            alert("Rec Qty can not be blank or zero.");
                                            return false;
                                        }
                                    }
                                    //                                    if (id.match(/CPH_Form_DG_txtnoofcone_.*/)) {
                                    //                                        // do something
                                    //                                        if (inputs[i].value == "" || inputs[i].value == "0") {
                                    //                                            alert("No of Cone can not be blank or zero.");
                                    //                                            return false;
                                    //                                        }
                                    //                                    }

                                }
                            }
                        }
                        return true;
                    }
                    else {
                        alert(Message);
                        return false;
                    }
                });

            });
        }
    </script>
    <asp:UpdatePanel ID="Upd1" runat="server">
        <ContentTemplate>
            <%-- <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>--%>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label ID="lblMasterDetail" runat="server" Text="Master Details" CssClass="labelbold"
                            ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="Chkedit" Text="For Edit" CssClass="checkboxbold" AutoPostBack="true"
                                                runat="server" OnCheckedChanged="Chkedit_CheckedChanged" Visible="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkcompleteissueno" Text="For Complete Issue No." CssClass="checkboxbold"
                                                runat="server" Visible="false" AutoPostBack="true" OnCheckedChanged="chkcompleteissueno_CheckedChanged" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td id="TRempcodescan" runat="server" visible="false">
                                <asp:Label ID="Label18" Text="Enter/Scan Emp. Code" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtWeaverIdNoscan" CssClass="textb" runat="server" Width="150px"
                                    Height="20px" AutoPostBack="true" OnTextChanged="txtWeaverIdNoscan_TextChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td id="TDdept" runat="server" visible="false">
                                <asp:DropDownList ID="DDdept" CssClass="dropdown" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DDdept_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="Label9" runat="server" Text="CompanyName" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td valign="top">
                                <asp:Label ID="lblyarnopendept" runat="server" Text="Yarn Opening Dept." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDvendor" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDvendor_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDLotno" runat="server" valign="top">
                                <asp:Label ID="Label10" runat="server" Text="Lot No." CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtLotno" CssClass="textb" runat="server" Width="120px" AutoPostBack="true"
                                    OnTextChanged="txtLotno_TextChanged" />
                            </td>
                            <td id="TDIssueNo" runat="server">
                                <asp:Label ID="Label12" runat="server" Text="Enter Wastage Weight" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtwastageweight" Width="180px" CssClass="textb" runat="server"
                                    onkeypress="return isNumberKey(event);" /><br />
                                <br />
                                <asp:Label ID="Label4" runat="server" Text="Issue No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDIssueno" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDIssueno_SelectedIndexChanged" Visible="false">
                                </asp:DropDownList>
                                <div style="overflow: scroll; max-height: 140px; width: 200px; margin-top: 2%">
                                    <asp:CheckBoxList ID="chkissueno" CssClass="checkboxbold" runat="server">
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                            <td valign="top">
                                <asp:Label ID="Label7" runat="server" Text="Employee Name" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDemployee" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="230px" OnSelectedIndexChanged="DDemployee_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDReceiveNo" runat="server" visible="false" valign="top">
                                <asp:Label ID="Label8" runat="server" Text="Receive No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDReceiveNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDReceiveNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td valign="top">
                                <asp:Label ID="Label2" runat="server" Text="Receive No." CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtreceiveNo" CssClass="textb" Width="90px" runat="server" Enabled="false" />
                            </td>
                            <td valign="top">
                                <asp:Label ID="Label3" runat="server" Text="Receive Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtRecdate" CssClass="textb" Width="80px" runat="server" />
                                <asp:CalendarExtender ID="cal1" TargetControlID="txtRecdate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td runat="server" id="TDGodown">
                                <asp:Label Text="Godown Name" CssClass="labelbold" runat="server" /><br />
                                <asp:DropDownList ID="DDGodown" CssClass="dropdown" Width="150px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDGodown_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDBinNo" runat="server" visible="false">
                                <asp:Label ID="Label11" Text="Bin No." CssClass="labelbold" runat="server" /><br />
                                <asp:DropDownList ID="DDBinNo" CssClass="dropdown" Width="150px" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset runat="server" visible="false">
                    <legend>
                        <asp:Label ID="Label5" runat="server" Text="Issue Details" CssClass="labelbold" ForeColor="Red"
                            Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr>
                            <td>
                                <div id="gride" runat="server" style="max-height: 400px; overflow: auto">
                                    <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                        OnRowDataBound="DG_RowDataBound">
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
                                                    <asp:CheckBox ID="Chkboxitem" runat="server" onclick="return CheckBoxClick(this);" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ItemDescription">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unit">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblunit" Text='<%#Bind("UnitName")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Lot No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLotno" Text='<%#Bind("Lotno") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tag No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTagno" Text='<%#Bind("Tagno") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Godown Name">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDGodown" CssClass="dropdown" Width="150px" AutoPostBack="true"
                                                        runat="server" OnSelectedIndexChanged="DDGodownDG_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Bin No">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDBinNo" CssClass="dropdown" Width="150px" AutoPostBack="true"
                                                        runat="server">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Issued Qty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblisuedQty" Text='<%#Bind("IssueQty") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Received Qty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReceivedQty" Text='<%#Bind("ReceivedQty") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PQty.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblpqty" runat="server" Text='<%# System.Math.Round(Convert.ToDouble(Eval("IssueQty")) -Convert.ToDouble(Eval("ReceivedQty")),3) %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rec. Qty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtrecqty" Width="70px" BackColor="Yellow" runat="server" onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Loss Qty" Visible="false">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtlossqty" Width="50px" BackColor="Yellow" runat="server" onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rec Type">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDRecType" CssClass="dropdown" runat="server">
                                                        <asp:ListItem Text="Cone" />
                                                        <asp:ListItem Text="Hank" />
                                                        <asp:ListItem Text="" />
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="No of Cone">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtnoofcone" Width="70px" BackColor="Yellow" runat="server" onkeypress="return isNumberKey(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cone Type">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="DDconetype" CssClass="dropdown" runat="server">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblrate" Text='<%#Bind("Rate") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblissuemasterid" Text='<%#Bind("Id") %>' runat="server" />
                                                    <asp:Label ID="lblitemfinishedid" Text='<%#Bind("item_finished_id") %>' runat="server" />
                                                    <asp:Label ID="lblunitid" Text='<%#Bind("unitid") %>' runat="server" />
                                                    <asp:Label ID="lblflagsize" Text='<%#Bind("flagsize") %>' runat="server" />
                                                    <asp:Label ID="lblissuemasterdetailid" Text='<%#Bind("Detailid") %>' runat="server" />
                                                    <asp:Label ID="lblrectype" Text='<%#Bind("Rectype") %>' runat="server" />
                                                    <asp:Label ID="lblconetype" Text='<%#Bind("conetype") %>' runat="server" />
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
                                <asp:CheckBox ID="chkcomplete" Text="For Complete" CssClass="checkboxbold" AutoPostBack="true"
                                    runat="server" OnCheckedChanged="chkcomplete_CheckedChanged" />
                                <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click"
                                    UseSubmitBehavior="false" OnClientClick="if (!Validatesave())return; this.disabled=true;this.value = 'wait ...';" />
                                <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <fieldset>
                        <legend>
                            <asp:Label ID="Label6" runat="server" Text="Received Details" CssClass="labelbold"
                                ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                        <table>
                            <tr>
                                <td>
                                    <div id="Div1" runat="server" style="max-height: 300px">
                                        <asp:GridView ID="DGReceivedDetail" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                            OnRowDeleting="DGReceivedDetail_RowDeleting">
                                            <HeaderStyle CssClass="gvheaders" />
                                            <AlternatingRowStyle CssClass="gvalts" />
                                            <RowStyle CssClass="gvrow" />
                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Receive No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblrecno" Text='<%#Bind("ReceiveNo") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ItemDescription">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblunit" Text='<%#Bind("UnitName")%>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Godown">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgodown" Text='<%#Bind("GodownName")%>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Lot No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLotno" Text='<%#Bind("Lotno") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tag No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTagno" Text='<%#Bind("Tagno") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rec Qty">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblrecqty" Text='<%#Bind("ReceiveQty") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Loss Qty" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbllossqty" Text='<%#Bind("Lossqty") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rec Type">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRectype" Text='<%#Bind("Rectype") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="No of Cone">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblnoofcone" Text='<%#Bind("Noofcone") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cone Type">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblconetype" Text='<%#Bind("conetype") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                            Text="Del" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                                    <ItemStyle HorizontalAlign="Center" Width="20px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblissuemasterid" Text='<%#Bind("issuemasterId") %>' runat="server" />
                                                        <asp:Label ID="lblid" Text='<%#Bind("Id") %>' runat="server" />
                                                        <asp:Label ID="lbldetailid" Text='<%#Bind("Detailid") %>' runat="server" />
                                                        <asp:Label ID="lblissuemasterdetailid" Text='<%#Bind("issuemasterdetailid") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div>
                    <asp:Button runat="server" ID="btnModalPopUp" Style="display: none" />
                    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="pnModelPopup"
                        TargetControlID="btnModalPopUp" DropShadow="true" BackgroundCssClass="modalBackground"
                        CancelControlID="btnCancel" PopupDragHandleControlID="pnModelPopup" OnOkScript="onOk()">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="pnModelPopup" runat="server" Style="background-color: White; border: 3px solid #0DA9D0;
                        border-radius: 12px; padding: 0; display: none" Height="250px" Width="510px">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl" runat="server" Text="FOR COMPLETION OF YARN OPENING" ForeColor="#cc3300"
                                        CssClass="labelbold"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label Text="Issue No." CssClass="labelbold" runat="server" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDcompissueno" CssClass="dropdown" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="DDcompissueno_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <div style="overflow: auto; width: 500px; height: 100px; margin-top: 10px; height: 180px">
                            <asp:GridView ID="DGGridForComp" runat="server" Width="479px" Style="margin-left: 10px"
                                ForeColor="#333333" AutoGenerateColumns="False" CssClass="grid-views">
                                <HeaderStyle CssClass="gvheaders" Height="20px" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" HorizontalAlign="Center" Height="20px" />
                                <PagerStyle CssClass="PagerStyle" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:TemplateField HeaderText="ItemDescription">
                                        <ItemTemplate>
                                            <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Issued Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="lblisuedQty" Text='<%#Bind("IssueQty") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Received Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReceivedQty" Text='<%#Bind("ReceivedQty") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Gain Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgainQty" Text='<%#Bind("Gainqty") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Loss Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLossqty" Text='<%#Bind("Lossqty") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemfinishedid" Text='<%#Bind("Item_Finished_id") %>' runat="server" />
                                            <asp:Label ID="lblId" Text='<%#Bind("Id") %>' runat="server" />
                                            <asp:Label ID="lblDetailId" Text='<%#Bind("DetailId") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <table width="400px">
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btncomp" runat="server" Text="Complete" CssClass="buttonnorm" OnClick="btncomp_Click"
                                        CausesValidation="false" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonnorm" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
