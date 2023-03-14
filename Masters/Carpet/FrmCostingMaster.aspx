<%@ Page Title="Costing" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="FrmCostingMaster.aspx.cs" Inherits="Masters_Carpet_FrmCostingMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmCostingMaster.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function isnumber(evt) {
            evt = (evt) ? evt : window.event;
            var charcode = (evt.which) ? evt.which : evt.keycode;
            if (charcode > 31 && (charcode < 48 || charcode > 57) && charcode != 46) {
                return false;
            }
            return true;
        }
        function ItemDetailTextChanged() {
            var Qty = document.getElementById('<%=txtQty.ClientID %>');
            var Rate = document.getElementById('<%=txtRate.ClientID %>');
            var Wastage = document.getElementById('<%=TxtWastagePercentage.ClientID %>');
            var ProcessRate = document.getElementById('<%=TxtDyeingRate.ClientID %>');
            var amount = document.getElementById('<%=txtAmount.ClientID %>');
            var Interest = 0;
            if ($("#<%=TDInterestPercentage.ClientID %>").is(':visible')) {
                Interest = document.getElementById('<%=TxtInterestPercentage.ClientID %>');
            }
            amount.value = ((getNum(parseFloat(Qty.value)) + getNum(parseFloat(Qty.value)) * getNum(parseFloat(Wastage.value)) * 0.01) * (getNum(parseFloat(Rate.value)) + getNum(parseFloat(ProcessRate.value)))).toFixed(2);
            amount.value = (getNum(parseFloat(amount.value)) + (amount.value * getNum(parseFloat(Interest.value)) * 0.01)).toFixed(2);
        }
        function ProcessTextChanged() {
            var Rate = document.getElementById('<%=txtProcessRate.ClientID %>');
            var Qty = document.getElementById('<%=txtProcessRate.ClientID %>');
            var amount = document.getElementById('<%=txtProcessAmount.ClientID %>');
            amount.value = (getNum(parseFloat(Rate.value)) * getNum(parseFloat(Qty.value))).toFixed(2);
        }
        function getNum(val) {
            if (isNaN(val)) {
                return 0;
            }
            return val;
        }

        function AddDesign() {
            var answer = confirm("Do you want to ADD?")
            var left = (screen.width / 2) - (601 / 2);
            var top = (screen.height / 2) - (450 / 2);

            if (answer) {
                window.open('../Carpet/AddDesign.aspx', '', 'width=601px,Height=450px,top=' + top + ',left=' + left);
            }
        }
        function AddColor() { 
            var answer = confirm("Do you want to ADD?")
            var left = (screen.width / 2) - (601 / 2);
            var top = (screen.height / 2) - (450 / 2);

            if (answer) {
                window.open('../Carpet/AddColor.aspx', '', 'width=601px,Height=450px,top=' + top + ',left=' + left);
            }
        }
    </script>
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=btnsave.ClientID %>").click(function () {
                    var Message = "";

                    if ($("#<%=DDCategoryname.ClientID %>")) {
                        var selectedindex = $("#<%=DDCategoryname.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Category!!\n";
                            alert(Message);
                            return false;
                        }
                    }
                    if ($("#<%=DDItemName.ClientID %>")) {
                        var selectedindex = $("#<%=DDItemName.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select ItemName!!\n";
                            alert(Message);
                            return false;
                        }
                    }
                    if ($("#<%=TDQuality.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDQuality.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Qualityname!!\n";
                            alert(Message);
                            return false;
                        }
                    }
                    if ($("#<%=TDDesign.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDDesign.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Design!!\n";
                            alert(Message);
                            return false;
                        }
                    }
                    if ($("#<%=TDColor.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDColor.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Color!!\n";
                            alert(Message);
                            return false;
                        }
                    }
                    if ($("#<%=TDShape.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDshape.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Shape!!\n";
                            alert(Message);
                            return false;
                        }
                    }
                    if ($("#<%=TDSize.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDSize.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Size!!\n";
                            alert(Message);
                            return false;
                        }
                    }
                    if ($("#<%=TDShade.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDshade.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select shade color!!\n";
                            alert(Message);
                            return false;
                        }
                    }
                    if ($("#<%=tdDDCustomerCode.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDCustomerCode.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select customer code!!\n";
                            alert(Message);
                            return false;
                        }
                    }
                    if (Session["varcompanyId"].ToString() != "44") {
                        if ($("#<%=TxtDescription.ClientID %>")) {
                            var selectedindex = $("#<%=TxtDescription.ClientID %>").attr('value');
                            if (selectedindex == "") {
                                Message = Message + "Please Enter description .!!\n";
                                alert(Message);
                                return false;
                            }
                        }
                        if ($("#<%=TDWeightWithoutLatex.ClientID %>").is(':visible')) {
                            if ($("#<%=TxtWeightWithoutLatex.ClientID %>")) {
                                var selectedindex = $("#<%=TxtWeightWithoutLatex.ClientID %>").attr('value');
                                if (selectedindex == "") {
                                    Message = Message + "Please Enter Weight Without Latex .!!\n";
                                    alert(Message);
                                    return false;
                                }
                            }
                        }
                        if ($("#<%=TDWeightWithLatex.ClientID %>").is(':visible')) {
                            if ($("#<%=TxtWeightWithLatex.ClientID %>")) {
                                var selectedindex = $("#<%=TxtWeightWithLatex.ClientID %>").attr('value');
                                if (selectedindex == "") {
                                    Message = Message + "Please Enter Weight With Latex .!!\n";
                                    alert(Message);
                                    return false;
                                }
                            }
                        }
                        if ($("#<%=TDUSDINR.ClientID %>").is(':visible')) {
                            if ($("#<%=TxtUSDVsINR.ClientID %>")) {
                                var selectedindex = $("#<%=TxtUSDVsINR.ClientID %>").attr('value');
                                if (selectedindex == "") {
                                    Message = Message + "Please Enter USD vs INR.!!\n";
                                    alert(Message);
                                    return false;
                                }
                            }
                        }
                        if ($("#<%=TDTHCPercentage.ClientID %>").is(':visible')) {
                            if ($("#<%=TxtTHCPercentage.ClientID %>")) {
                                var selectedindex = $("#<%=TxtTHCPercentage.ClientID %>").attr('value');
                                if (selectedindex == "") {
                                    Message = Message + "Please Enter THC %.!!\n";
                                    alert(Message);
                                    return false;
                                }
                            }
                        }
                        if ($("#<%=TDRMUPercentage.ClientID %>").is(':visible')) {
                            if ($("#<%=TxtRMUPercentage.ClientID %>")) {
                                var selectedindex = $("#<%=TxtRMUPercentage.ClientID %>").attr('value');
                                if (selectedindex == "") {
                                    Message = Message + "Please Enter RMU % .!!\n";
                                    alert(Message);
                                    return false;
                                }
                            }
                        }
                    }
                    if ($("#<%=TDlblItemInterestPercentage.ClientID %>").is(':visible')) {
                        if ($("#<%=TxtItemInterestPercentage.ClientID %>")) {
                            var selectedindex = $("#<%=TxtItemInterestPercentage.ClientID %>").attr('value');
                            if (selectedindex == "") {
                                Message = Message + "Please enter ItemInterestPercentage % .!!\n";
                                alert(Message);
                                return false;
                            }
                        }
                    }

                    if ($("#<%=DDProcessRD.ClientID %>")) {
                        var selectedindex = $("#<%=DDProcessRD.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select process!!\n";
                            alert(Message);
                            return false;
                        }
                    }
                    if ($("#<%=DDCategoryRD.ClientID %>")) {
                        var selectedindex = $("#<%=DDCategoryRD.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Category!!\n";
                            alert(Message);
                            return false;
                        }
                    }
                    if ($("#<%=DDItemNameRD.ClientID %>")) {
                        var selectedindex = $("#<%=DDItemNameRD.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select ItemName!!\n";
                            alert(Message);
                            return false;
                        }
                    }
                    if ($("#<%=TDQualityRD.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDQualityRD.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Qualityname!!\n";
                            alert(Message);
                            return false;
                        }
                    }
                    if ($("#<%=TDDesignRD.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDDesignRD.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Design!!\n";
                            alert(Message);
                            return false;
                        }
                    }
                    if ($("#<%=TDColorRD.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDColorRD.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Color!!\n";
                            alert(Message);
                            return false;
                        }
                    }
                    if ($("#<%=TDShapeRD.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDShapeRD.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Shape!!\n";
                            alert(Message);
                            return false;
                        }
                    }
                    if ($("#<%=TDSizeRD.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDSizeRD.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Size!!\n";
                            alert(Message);
                            return false;
                        }
                    }
                    if ($("#<%=TDShadeRD.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDShadeRD.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select shade color!!\n";
                            alert(Message);
                            return false;
                        }
                    }
                    if ($("#<%=txtQty.ClientID %>")) {
                        var selectedindex = $("#<%=txtQty.ClientID %>").attr('value');
                        if (selectedindex == "") {
                            Message = Message + "Please Enter consumption.!!\n";
                            alert(Message);
                            return false;
                        }
                    }
                    if ($("#<%=txtRate.ClientID %>")) {
                        var selectedindex = $("#<%=txtRate.ClientID %>").attr('value');
                        if (selectedindex == "") {
                            Message = Message + "Please Enter rate.!!\n";
                            alert(Message);
                            return false;
                        }
                    }
                    if ($("#<%=TDInterestPercentage.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=TxtInterestPercentage.ClientID %>").attr('value');
                        if (selectedindex == "") {
                            Message = Message + "Please Enter interest %.!!\n";
                            alert(Message);
                            return false;
                        }
                    }
                    if ($("#<%=TxtWastagePercentage.ClientID %>")) {
                        var selectedindex = $("#<%=TxtWastagePercentage.ClientID %>").attr('value');
                        if (selectedindex == "") {
                            Message = Message + "Please Enter wastage %.!!\n";
                            alert(Message);
                            return false;
                        }
                    }
                    if ($("#<%=lblDyeingRate.ClientID %>")) {
                        var selectedindex = $("#<%=TxtDyeingRate.ClientID %>").attr('value');
                        if (selectedindex == "") {
                            Message = Message + "Please Enter dyeing rate!!\n";
                            alert(Message);
                            return false;
                        }
                    }
                    if ($("#<%=DDProcess.ClientID %>")) {
                        var selectedindex = $("#<%=DDProcess.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select process!!\n";
                            alert(Message);
                            return false;
                        }
                    }
                    if ($("#<%=txtProcessRate.ClientID %>")) {
                        var selectedindex = $("#<%=txtProcessRate.ClientID %>").attr('value');
                        if (selectedindex == "") {
                            Message = Message + "Please Enter rate.!!\n";
                            alert(Message);
                            return false;
                        }
                    }

                    if (Message == "") {
                        return true;
                    }
                    else {
                        alert(Message);
                        return false;
                    }
                });
                if ($("#<%=TDSize.ClientID %>").is(':visible')) {
                    var selectedindex = $("#<%=DDSize.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        $("#<%=TDCBM.ClientID%>").hide();
                    }
                    else {
                        $("#<%=TDCBM.ClientID%>").show();
                    }
                }
                $("#<%=DDSize.ClientID %>").change(function () {

                    var selectedindex = $("#<%=DDSize.ClientID %>").attr('selectedIndex');
                    if (selectedindex == 0) {
                        $("#<%=TDCBM.ClientID%>").hide();
                    }
                    else {
                        $("#<%=TDCBM.ClientID%>").show();
                    }
                });
                //link button click
                $("#<%=lnkcbm.ClientID%>").click(function () {
                    if ($("#<%=TDSize.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDSize.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            alert("Please select size!!!.");
                        }
                        else {
                            var Sizeid = $("#<%=DDSize.ClientID %>").val();
                            var size = $('#<%=DDSize.ClientID %> :selected').text();
                            var sizeflag = $('#<%=DDsizetype.ClientID %> :selected').text();
                            var sizeflagId = $('#<%=DDsizetype.ClientID %>').val();
                            var left = (screen.width / 2) - (500 / 2);
                            var top = (screen.height / 2) - (400 / 2);

                            window.open('frmcalculatecbm.aspx?a=' + Sizeid + '&b=' + size + '&c=' + sizeflag + '&d=' + sizeflagId, 'ADD LOOM MASTER', 'width=500px, height=200px, top=' + top + ', left=' + left);
                        }
                    }
                });
                //**********
            });
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>
            <fieldset>
                <legend>
                    <asp:Label ID="lblSearchDetail" runat="server" Text="Item Description" CssClass="labelbold"
                        ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                <asp:Panel ID="pnl1" runat="server">
                    <table>
                        <tr>
                            <td id="TDProdcode" runat="server" visible="false">
                                <asp:Label ID="lblItemCode" runat="server" Text="ItemCode" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtprodcode" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblSampleCodeForEdit" Text="Sample Code" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="TxtSampleCodeForEdit" CssClass="textb" runat="server" Width="100px"
                                    AutoPostBack="true" OnTextChanged="TxtSampleCodeForEdit_TextChanged" />
                            </td>
                            <td>
                                <asp:Label ID="lblddSampleCodeForEdit" runat="server" Text="Sample Code" CssClass="labelbold"
                                    Visible="false"></asp:Label><br />
                                <asp:DropDownList ID="DDSampleCode" runat="server" CssClass="dropdown" Width="130px"
                                    Visible="false" OnSelectedIndexChanged="DDSampleCode_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblCompanyName" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDCompanyName" runat="server" CssClass="dropdown" Width="130px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblcategory" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDCategoryname" runat="server" CssClass="dropdown" Width="130px"
                                    OnSelectedIndexChanged="DDCategoryname_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblIemName" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDItemName" runat="server" CssClass="dropdown" Width="130px"
                                    OnSelectedIndexChanged="DDItemName_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td id="TDQuality" runat="server" visible="false">
                                <asp:Label ID="lblQuality" runat="server" Text="Quality" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDQuality" runat="server" CssClass="dropdown" Width="130px">
                                </asp:DropDownList>
                            </td>
                            <td id="TDDesign" runat="server" visible="false">
                                <asp:Label ID="lblDesign" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                                &nbsp;&nbsp;
                                <asp:Button ID="btnadddesign" runat="server" CssClass="buttonsmalls" OnClientClick="return AddDesign()"
                                    Text=" + " />
                                <br />
                                <asp:DropDownList ID="DDDesign" runat="server" CssClass="dropdown" Width="130px">
                                </asp:DropDownList>
                                <asp:Button ID="refreshdesign" runat="server" Style="display: none" OnClick="refreshdesign_Click" />
                            </td>
                            <td id="TDColor" runat="server" visible="false">
                                <asp:Label ID="lblColor" runat="server" Text="Color" CssClass="labelbold"></asp:Label>&nbsp;&nbsp;
                                <asp:Button ID="btnaddcolor" runat="server" CssClass="buttonsmalls" OnClientClick="return AddColor()"
                                    Text=" + " />
                                <br />
                                <asp:DropDownList ID="DDColor" runat="server" CssClass="dropdown" Width="130px">
                                </asp:DropDownList>
                                <asp:Button CssClass="refreshcolor" ID="refreshcolor" runat="server" Style="display: none"
                                    OnClick="refreshcolor_Click" />
                            </td>
                            <td id="TDShape" runat="server" visible="false">
                                <asp:Label ID="lblShape" runat="server" Text="Shape" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDshape" runat="server" CssClass="dropdown" Width="100px" OnSelectedIndexChanged="DDshape_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td id="TDSize" runat="server" visible="false">
                                <asp:Label ID="lblSize" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                &nbsp;
                                <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged" />
                                <br />
                                <asp:DropDownList ID="DDSize" runat="server" CssClass="dropdown" Width="130px">
                                </asp:DropDownList>
                            </td>
                            <td id="TDShade" runat="server" visible="false">
                                <asp:Label ID="lblshade" runat="server" Text="Shade Color" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDshade" runat="server" CssClass="dropdown" Width="130px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblCosting" Text="Costing For" runat="server" CssClass="labelbold" /><br />
                                <asp:DropDownList ID="DDCostingFor" CssClass="dropdown" runat="server" AutoPostBack="true"
                                    Width="130px" OnSelectedIndexChanged="DDCostingFor_SelectedIndexChanged">
                                    <asp:ListItem Value="0">Sample</asp:ListItem>
                                    <asp:ListItem Value="1">Customer</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td id="tdDDCustomerCode" runat="server" visible="false">
                                <asp:Label ID="LblCustomerCode" Text="Customer Code" runat="server" CssClass="labelbold" /><br />
                                <asp:DropDownList ID="DDCustomerCode" runat="server" CssClass="dropdown" Width="130px">
                                </asp:DropDownList>
                            </td>
                            <td id="TDCostingRemark" runat="server" visible="false">
                                <asp:Label ID="lblCostingRemark" Text="Costing Remark" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="TxtCostingRemark" CssClass="textb" runat="server" Width="200px" />
                            </td>
                            <td>
                                <asp:Label ID="lblSampleCode" Text="Sample Code" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="txtsamplecode" CssClass="textb" runat="server" Width="100px" />
                                <asp:AutoCompleteExtender ID="AutoCompleteExtenderSamplecode" runat="server" BehaviorID="SamplecodeSrchAutoComplete"
                                    CompletionInterval="20" Enabled="True" ServiceMethod="GetCostingsamplecode" EnableCaching="true"
                                    CompletionSetCount="30" ServicePath="~/Autocomplete.asmx" TargetControlID="txtsamplecode"
                                    UseContextKey="true" ContextKey="0#0#0" MinimumPrefixLength="1">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblcostingDate" Text="Costing Date" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="txtcostingDate" runat="server" CssClass="textb" Width="100px" />
                                <asp:CalendarExtender ID="cal1" TargetControlID="txtcostingDate" runat="server" Format="dd-MMM-yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblDescription" Text="Description" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="TxtDescription" runat="server" CssClass="textb" Width="200px" />
                            </td>
                            <td id="TDWeightWithoutLatex" runat="server">
                                <asp:Label ID="lblWeightWithoutLatex" Text="Weight Without Latex" runat="server"
                                    CssClass="labelbold" /><br />
                                <asp:TextBox ID="TxtWeightWithoutLatex" runat="server" CssClass="textb" Width="125px" />
                            </td>
                            <td id="TDWeightWithLatex" runat="server">
                                <asp:Label ID="lblWeightWithLatex" Text="Weight With Latex" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="TxtWeightWithLatex" runat="server" CssClass="textb" Width="100px" />
                            </td>
                            <td id="TDUSDINR" runat="server">
                                <asp:Label ID="lblUSDINR" Text="US$ Vs INR" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="TxtUSDVsINR" runat="server" CssClass="textb" Width="60px" />
                            </td>
                            <td id="TDTHCPercentage" runat="server">
                                <asp:Label ID="lblTHCPercentage" Text="THC %" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="TxtTHCPercentage" runat="server" CssClass="textb" Width="60px" />
                            </td>
                            <td id="TDRMUPercentage" runat="server">
                                <asp:Label ID="lblRMUPercentage" Text="RMU %" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="TxtRMUPercentage" runat="server" CssClass="textb" Width="60px" />
                            </td>
                            <td id="TDlblItemInterestPercentage" runat="server" visible="false">
                                <asp:Label ID="lblItemInterestPercentage" Text="Interest %" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="TxtItemInterestPercentage" runat="server" CssClass="textb" Width="60px" />
                            </td>
                            <td>
                                <br />
                                <asp:Button ID="btnshowdetail" Text="Show Detail" CssClass="buttonnorm" runat="server"
                                    OnClick="btnshowdetail_Click" />
                            </td>
                            <td>
                                <br />
                                <asp:Button ID="btnAddImage" Text="Add Image" CssClass="buttonnorm" runat="server"
                                    Visible="false" OnClick="btnAddImage_Click" />
                            </td>
                            <td>
                                <br />
                                <asp:Button ID="btnGetCostingCode" Text="Get Costing Code" CssClass="buttonnorm"
                                    runat="server" OnClick="btnGetCostingCode_Click" />
                            </td>
                            <td id="TDCBM" runat="server">
                                <asp:LinkButton ID="lnkcbm" Text="Calculate CBM" ForeColor="Blue" runat="server"
                                    Visible="false"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </fieldset>
            <fieldset>
                <legend>
                    <asp:Label ID="Label1" runat="server" Text="Item Details" CssClass="labelbold" ForeColor="Red"
                        Font-Bold="true"></asp:Label></legend>
                <asp:Panel ID="Panel1" runat="server">
                    <table>
                        <tr>
                            <td id="TDItemCodeDetail" runat="server" visible="false">
                                <asp:Label ID="lblItemCodeD" runat="server" Text="ItemCode" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtItemCodeRD" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblProcessRD" Text="Process" runat="server" CssClass="labelbold" /><br />
                                <asp:DropDownList ID="DDProcessRD" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblCategoryRD" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDCategoryRD" runat="server" CssClass="dropdown" Width="150px"
                                    OnSelectedIndexChanged="DDCategoryRD_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblItemRD" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDItemNameRD" runat="server" CssClass="dropdown" Width="150px"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDItemNameRD_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDQualityRD" runat="server" visible="false">
                                <asp:Label ID="lblQualityRD" runat="server" Text="Quality" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDQualityRD" runat="server" CssClass="dropdown" Width="150px"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td id="TDDesignRD" runat="server" visible="false">
                                <asp:Label ID="lblDesignRD" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDDesignRD" runat="server" CssClass="dropdown" Width="130px">
                                </asp:DropDownList>
                            </td>
                            <td id="TDColorRD" runat="server" visible="false">
                                <asp:Label ID="lblColorRD" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDColorRD" runat="server" CssClass="dropdown" Width="130px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td id="TDShapeRD" runat="server" visible="false">
                                <asp:Label ID="lblShapeRD" runat="server" Text="Shape" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDShapeRD" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDShapeRD_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDSizeRD" runat="server" visible="false">
                                <asp:Label ID="Label9" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                &nbsp;
                                <asp:DropDownList CssClass="dropdown" Width="50" ID="DDSizeTypeRD" runat="server"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDSizeTypeRD_SelectedIndexChanged" />
                                <br />
                                <asp:DropDownList ID="DDSizeRD" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td id="TDShadeRD" runat="server" visible="false">
                                <asp:Label ID="Label10" runat="server" Text="Shade Color" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDShadeRD" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblQty" Text="Consumption" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="txtQty" runat="server" CssClass="textb" Width="75px" onkeypress=" return isnumber(event)"
                                    onchange="javascript: ItemDetailTextChanged();" />
                            </td>
                            <td>
                                <asp:Label ID="lblRate" Text="Rate" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="txtRate" runat="server" CssClass="textb" Width="75px" onkeypress=" return isnumber(event)"
                                    onchange="javascript: ItemDetailTextChanged();" />
                            </td>
                            <td id="TDInterestPercentage" runat="server" visible="false">
                                <asp:Label ID="lblInterestPercentage" Text="Interest %" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="TxtInterestPercentage" runat="server" CssClass="textb" Width="75px"
                                    onkeypress=" return isnumber(event)" onchange="javascript: ItemDetailTextChanged();" />
                            </td>
                            <td>
                                <asp:Label ID="lblWastagePercentage" Text="Wastage %" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="TxtWastagePercentage" runat="server" CssClass="textb" Width="75px"
                                    onkeypress=" return isnumber(event)" onchange="javascript: ItemDetailTextChanged();" />
                            </td>
                            <td>
                                <asp:Label ID="lblDyeingRate" Text="Process Rate" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="TxtDyeingRate" runat="server" CssClass="textb" Width="100px" onkeypress=" return isnumber(event)"
                                    onchange="javascript: ItemDetailTextChanged();" />
                            </td>
                            <td class="tdstyle" id="TDTxtDyingType" runat="server">
                                <asp:Label ID="lblDyeingType" runat="server" Text="Process Type" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="TxtProcessType" runat="server" CssClass="textb" Width="150px" />
                            </td>
                            <td class="tdstyle" id="TDDDDyingType" runat="server" visible="false">
                                <asp:Label ID="Label24" runat="server" Text="Dyeing Type" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" Width="100" ID="DDDyingType" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblamount" Text="Amount" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="txtAmount" runat="server" CssClass="textb" Width="100px" onkeypress=" return isnumber(event)" />
                            </td>
                            <td class="tdstyle" id="TDProcessPMD" runat="server" visible="false">
                                <asp:Label ID="lblProcessPMD" Text="Process" runat="server" CssClass="labelbold" /><br />
                                <asp:DropDownList ID="DDProcessPMD" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle" id="TDPUnitPMD" runat="server" visible="false">
                                <asp:Label ID="lblPUnitPMD" runat="server" Text="Unit" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" Width="75" ID="DDProcessUnitPMD" runat="server" />
                            </td>
                            <td class="tdstyle" id="TDPRatePMD" runat="server" visible="false">
                                <asp:Label ID="lblPRatePMD" Text="Rate" runat="server" CssClass="labelbold" /><br />
                                <asp:TextBox ID="txtProcessRatePMD" runat="server" CssClass="textb" Width="60px"
                                    onkeypress=" return isnumber(event)" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </fieldset>
            <fieldset>
                <legend>
                    <asp:Label ID="lblProcessCosting" runat="server" Text="Process Costing" CssClass="labelbold"
                        ForeColor="Red" Font-Bold="true"></asp:Label>
                </legend>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblProcess" Text="Process" runat="server" CssClass="labelbold" /><br />
                            <asp:DropDownList ID="DDProcess" runat="server" CssClass="dropdown" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblPUnit" runat="server" Text="Unit" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" Width="75" ID="DDProcessUnit" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="lblPRate" Text="Rate" runat="server" CssClass="labelbold" /><br />
                            <asp:TextBox ID="txtProcessRate" runat="server" CssClass="textb" Width="60px" onkeypress=" return isnumber(event)"
                                onchange="javascript: ProcessTextChanged();" />
                        </td>
                        <td id="TDProcessAmount" runat="server" visible="false">
                            <asp:Label ID="lblPAmount" Text="Amount" runat="server" CssClass="labelbold" /><br />
                            <asp:TextBox ID="txtProcessAmount" runat="server" CssClass="textb" Width="100px"
                                onkeypress=" return isnumber(event)" />
                        </td>
                        <td>
                            <asp:Label ID="lblremark" Text="Remark" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtprocessremark" runat="server" CssClass="textb" Width="500px" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <table>
                <tr>
                    <td id="TDFOB" runat="server" visible="false">
                        <asp:Label ID="lblFOB" Text="FOB" runat="server" CssClass="labelbold" /><br />
                        <asp:TextBox ID="TxtFOB" runat="server" CssClass="textb" Width="60px" />
                    </td>
                    <td id="TDOverHead" runat="server" visible="false">
                        <asp:Label ID="lblOverHead" Text="Over Head" runat="server" CssClass="labelbold" /><br />
                        <asp:TextBox ID="TxtOverHead" runat="server" CssClass="textb" Width="60px" />
                    </td>
                    <td id="TDSalePrice" runat="server" visible="false">
                        <asp:Label ID="lblSalePrice" Text="Sale Price" runat="server" CssClass="labelbold" /><br />
                        <asp:TextBox ID="TxtSalePrice" runat="server" CssClass="textb" Width="65px" />
                    </td>
                    <td id="TDCurrency" runat="server" visible="false">
                        <asp:Label ID="Label2" runat="server" Text="Currency" CssClass="labelbold"></asp:Label><br />
                        <asp:DropDownList ID="DDCurrency" runat="server" CssClass="dropdown" Width="100px">
                        </asp:DropDownList>
                    </td>
                    <td id="TDExchangeRate" runat="server" visible="false">
                        <asp:Label ID="Label3" Text="Ex Rate" runat="server" CssClass="labelbold" /><br />
                        <asp:TextBox ID="TxtExchangeRate" runat="server" CssClass="textb" Width="65px" />
                    </td>
                    <td id="TDPoNo" runat="server" visible="false">
                        <asp:Label ID="Label4" Text="Po No" runat="server" CssClass="labelbold" /><br />
                        <asp:TextBox ID="TxtPoNo" runat="server" CssClass="textb" Width="65px" />
                    </td>
                    <td id="TDLicensePercentage" runat="server" visible="false">
                        <asp:Label ID="Label5" Text="License %" runat="server" CssClass="labelbold" /><br />
                        <asp:TextBox ID="TxtLicensePercentage" runat="server" CssClass="textb" Width="65px" />
                    </td>
                    <td id="TDDrawbackPercentage" runat="server" visible="false">
                        <asp:Label ID="Label6" Text="Drawback %" runat="server" CssClass="labelbold" /><br />
                        <asp:TextBox ID="TxtDrawbackPercentage" runat="server" CssClass="textb" Width="65px" />
                    </td>
                </tr>
            </table>
            <div>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblmessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                        </td>
                        <td align="right">
                            <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                            <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click" />
                            <asp:Button ID="btnpreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnpreview_Click" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="height: 300px; overflow: auto; width: 100%">
                <table style="width: 100%">
                    <tr>
                        <td>
                            <asp:GridView ID="GVDetail" runat="server" DataKeyNames="CostingItemProcessDetailID"
                                AutoGenerateColumns="False" EmptyDataText="No Records Found..." OnRowDeleting="GVDetail_RowDeleting">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Material No.">
                                        <ItemTemplate>
                                            <itemtemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </itemtemplate>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ProcessName" HeaderText="Process">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ItemDetail" HeaderText="Item Detail">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="250px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Consumption" HeaderText="Qty">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Rate" HeaderText="Rate">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="WastagePercentage" HeaderText="Wast. %">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ProcessRate" HeaderText="P Rate">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ProcessType" HeaderText="P Type">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Amount" HeaderText="Amt">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDel" Text="Delete" runat="server" OnClientClick="return confirm('Do you want to delete this row?');"
                                                CommandName="Delete"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                        <td>
                            <asp:GridView ID="GVProcessDetail" runat="server" DataKeyNames="CostingProcessRateDetailID"
                                AutoGenerateColumns="False" EmptyDataText="No Records Found..." OnRowDeleting="GVProcessDetail_RowDeleting">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:BoundField DataField="ProcessName" HeaderText="Process">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UnitName" HeaderText="Unit">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Rate" HeaderText="Rate">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Amount" HeaderText="Amt">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Remark" HeaderText="Remark">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDel" Text="Delete" runat="server" OnClientClick="return confirm('Do you want to delete this row?');"
                                                CommandName="Delete"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnpreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
