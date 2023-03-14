<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmpackingForHomefurn.aspx.cs" Inherits="Masters_Packing_frmpackingForHomefurn" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmpackingForHomefurn.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function isnumber(evt) {
            evt = (evt) ? evt : window.event;
            var charcode = (evt.which) ? evt.which : evt.keycode;
            if (charcode > 31 && (charcode < 48 || charcode > 57)) {
                return false;
            }
            return true;
        }
        function CrtnTextChanged() {
            var crtnfrom = document.getElementById('<%=txtcrtnFrom.ClientID %>');
            var crtnTo = document.getElementById('<%=txtcrtnTo.ClientID %>');
            var TotalCrtn = document.getElementById('<%=txttotalcrnt.ClientID %>');
            var PcPerCrtn = document.getElementById('<%=txtpcspercrnt.ClientID %>');
            var TotalPcs = document.getElementById('<%=txttotalpcs.ClientID %>');

            if (getNum(parseInt(crtnTo.value)) > 0) {
                if (getNum(parseInt(crtnfrom.value)) > (getNum(parseInt(crtnTo.value)))) {
                    alert("Crtn From can not be greater than Crtn To..");
                    crtnTo.value = "0";
                    return;
                }
            }
            TotalCrtn.value = getNum(parseInt(crtnTo.value)) - getNum(parseInt(crtnfrom.value)) + 1;
            if (TotalCrtn.value > 0) {
                TotalPcs.value = getNum(parseInt(TotalCrtn.value)) * getNum(parseInt(PcPerCrtn.value));
            }
            TextChanged();
        }
        function WeightTextChanged() {

            var NetWtPerPc = document.getElementById('<%=txtnetwtperpcs.ClientID %>');
            var NetWtFabric = document.getElementById('<%=txtnetwtfabric.ClientID %>');
            var NetWtBeads = document.getElementById('<%=txtnetwtbeads.ClientID %>');
            var TotalNetWtfabric = document.getElementById('<%=txttotalnetwtfabric.ClientID %>');
            var TotalNetWtBeads = document.getElementById('<%=txttotalnetwtbeads.ClientID %>');
            var TotalNetWt = document.getElementById('<%=txttotalnetwt.ClientID %>');
            var TotalGrWt = document.getElementById('<%=txtTgrwt.ClientID %>');
            var TotalPcs = document.getElementById('<%=txttotalpcs.ClientID %>');
            var CrtnWeight = document.getElementById('<%=txtcrtnwt.ClientID %>');
            var TotalCrtn = document.getElementById('<%=txttotalcrnt.ClientID %>');

            if (getNum(parseFloat(NetWtFabric.value)) > 0) {
                NetWtBeads.value = (getNum(parseFloat(NetWtPerPc.value)) - getNum(parseFloat(NetWtFabric.value))).toFixed(3);
            }
            else {
                NetWtBeads.value = 0;
            }
            TotalNetWtfabric.value = (getNum(parseInt(TotalPcs.value)) * getNum(parseFloat(NetWtFabric.value))).toFixed(2);
            TotalNetWtBeads.value = (getNum(parseInt(TotalPcs.value)) * getNum(parseFloat(NetWtBeads.value))).toFixed(2);
            TotalNetWt.value = (getNum(parseInt(TotalPcs.value)) * getNum(parseFloat(NetWtPerPc.value))).toFixed(2);
            TotalGrWt.value = (getNum(parseInt(TotalCrtn.value)) * getNum(parseFloat(CrtnWeight.value)) + parseFloat(TotalNetWt.value)).toFixed(2);
        }
        function getNum(val) {
            if (isNaN(val)) {
                return 0;
            }
            return val;
        }

        function TextChanged() {
            var txtLength = getNum(parseFloat(document.getElementById('<%=txtLength.ClientID %>').value));
            var txtWidth = getNum(parseFloat(document.getElementById('<%=txtWidth.ClientID %>').value));
            var txtHeigth = getNum(parseFloat(document.getElementById('<%=txtHeight.ClientID %>').value));
            var txtpc_Box = getNum(parseFloat(document.getElementById('<%=txtpcspercrnt.ClientID %>').value));
            var txtcbm_pc = document.getElementById('<%=txtcbm.ClientID %>');
            var txttotalcrnt = getNum(parseFloat(document.getElementById('<%=txttotalcrnt.ClientID %>').value));
            var CBM = (getNum(parseFloat(((txtLength * txtWidth * txtHeigth) / 1000000)))).toFixed(4);
            txtcbm_pc.value = CBM * txttotalcrnt;
            //Rate/pc

        }
    </script>
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=btnsave.ClientID %>").click(function () {
                    var Message = "";
                    if ($("#<%=txtInvoiceno.ClientID %>")) {
                        var selectedindex = $("#<%=txtInvoiceno.ClientID %>").attr('value');
                        if (selectedindex == "") {
                            Message = Message + "Please Enter Invoice No.!!\n";
                        }
                    }
                    if ($("#<%=DDCurrency.ClientID %>")) {
                        var selectedindex = $("#<%=DDCurrency.ClientID %>").attr('selectedIndex');
                        if (selectedindex < 0) {
                            Message = Message + "Please select Currency!!\n";
                        }
                    }
                    if ($("#<%=DDunit.ClientID %>")) {
                        var selectedindex = $("#<%=DDunit.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Unit!!\n";
                        }
                    }
                    if ($("#<%=txtcrtnFrom.ClientID %>")) {
                        var selectedindex = $("#<%=txtcrtnFrom.ClientID %>").attr('value');
                        if (selectedindex == "") {
                            Message = Message + "Please Enter Crtn From.!!\n";
                        }
                    }
                    if ($("#<%=txtcrtnTo.ClientID %>")) {
                        var selectedindex = $("#<%=txtcrtnTo.ClientID %>").attr('value');
                        if (selectedindex == "") {
                            Message = Message + "Please Enter Crtn To!!\n";
                        }
                    }
                    if ($("#<%=txtpcspercrnt.ClientID %>")) {
                        var selectedindex = $("#<%=txtpcspercrnt.ClientID %>").attr('value');
                        if (selectedindex == "") {
                            Message = Message + "Please Enter Pcs per Crtn !!\n";
                        }
                    }
                    if ($("#<%=DDCategoryName.ClientID %>")) {
                        var selectedindex = $("#<%=DDCategoryName.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Category!!\n";
                        }
                    }
                    if ($("#<%=DDItemName.ClientID %>")) {
                        var selectedindex = $("#<%=DDItemName.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select ItemName!!\n";
                        }
                    }
                    if ($("#<%=TDQuality.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDQuality.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Qualityname!!\n";
                        }
                    }
                    if ($("#<%=TDDesign.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDDesign.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Design!!\n";
                        }
                    }
                    if ($("#<%=TDColor.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDColor.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Color!!\n";
                        }
                    }
                    if ($("#<%=TDShape.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDShape.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Shape!!\n";
                        }
                    }
                    if ($("#<%=TDSize.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDSize.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Size!!\n";
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

            });
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label ID="lblleg1" runat="server" Text="...." CssClass="labelnormalMM" ForeColor="Red">
                        </asp:Label></legend>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblConsignor" Text="Consignor" CssClass="labelbold" runat="server" /><br />
                                <asp:DropDownList ID="DDConsignor" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblConsignee" Text="Consignee" CssClass="labelbold" runat="server" />
                                <asp:CheckBox ID="Chkedit" Text="For Edit" runat="server" CssClass="checkboxbold"
                                    OnCheckedChanged="Chkedit_CheckedChanged" AutoPostBack="true" /><br />
                                <asp:DropDownList ID="DDConsignee" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDConsignee_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDinvoiceNo" runat="server" visible="false">
                                <asp:Label ID="lblEditInvoice" Text="Invoice No." CssClass="labelbold" runat="server" /><br />
                                <asp:DropDownList ID="DDinvoiceNo" runat="server" CssClass="dropdown" Width="110px"
                                    OnSelectedIndexChanged="DDinvoiceNo_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblInvoiceNo" Text="Invoice No." CssClass="labelbold" runat="server" /><br />
                                <asp:TextBox ID="txtInvoiceno" CssClass="textb" Width="90px" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblInvoiceDate" Text="Invoice Date" CssClass="labelbold" runat="server" /><br />
                                <asp:TextBox ID="txtInvoiceDate" CssClass="textb" Width="90px" runat="server" />
                                <asp:CalendarExtender ID="calinvDate" TargetControlID="txtInvoiceDate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblCurrency" Text="Currency" CssClass="labelbold" runat="server" /><br />
                                <asp:DropDownList ID="DDCurrency" runat="server" CssClass="dropdown" Width="90px">
                                </asp:DropDownList>
                            </td>
                            <td id="TDUnitName" runat="server">
                                <asp:Label ID="lblunit" Text="UnitName" runat="server" CssClass="labelbold" /><br />
                                <asp:DropDownList ID="DDunit" CssClass="dropdown" Width="90px" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <div>
                    <fieldset>
                        <legend>
                            <asp:Label ID="lblleg2" runat="server" Text="Amount Calculation Type" CssClass="labelnormalMM"
                                ForeColor="Red">
                            </asp:Label>
                        </legend>
                        <table>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="RDAreaWise" Text="Area Wise" runat="server" CssClass="radiobuttonnormal" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="RDPcsWise" Text="Pcs Wise" runat="server" CssClass="radiobuttonnormal" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div>
                    <fieldset>
                        <legend>
                            <asp:Label ID="Label1" runat="server" Text="Carton No. and Order No." CssClass="labelnormalMM"
                                ForeColor="Red">
                            </asp:Label>
                        </legend>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblcrtnfrom" Text="Crtn. From" runat="server" CssClass="labelbold" /><br />
                                    <asp:TextBox ID="txtcrtnFrom" runat="server" CssClass="textb" Width="80px" onkeypress="return isnumber(event);"
                                        onchange="javascript: CrtnTextChanged();" />
                                </td>
                                <td>
                                    <asp:Label ID="lblcrtnTo" Text="Crtn. To" runat="server" CssClass="labelbold" /><br />
                                    <asp:TextBox ID="txtcrtnTo" runat="server" CssClass="textb" Width="80px" onkeypress="return isnumber(event);"
                                        onchange="javascript: CrtnTextChanged();" />
                                </td>
                                <td>
                                    <asp:Label ID="lblTotalcrtn" Text="Total Crtn." runat="server" CssClass="labelbold" /><br />
                                    <asp:TextBox ID="txttotalcrnt" runat="server" CssClass="textb" Width="80px" Enabled="false"
                                        BackColor="Yellow" />
                                </td>
                                <td>
                                    <asp:Label ID="lblpcpercrtn" Text="Pcs Per Crtn." runat="server" CssClass="labelbold" /><br />
                                    <asp:TextBox ID="txtpcspercrnt" runat="server" CssClass="textb" Width="80px" onkeypress="return isnumber(event);"
                                        onchange="javascript: CrtnTextChanged();" />
                                </td>
                                <td>
                                    <asp:Label ID="lblTotalpcs" Text="Total Pcs." runat="server" CssClass="labelbold" /><br />
                                    <asp:TextBox ID="txttotalpcs" runat="server" CssClass="textb" Width="80px" Enabled="false"
                                        BackColor="Yellow" />
                                </td>
                                <td>
                                    <asp:Label ID="lblorderno" Text="Order No." runat="server" CssClass="labelbold" />
                                    <asp:CheckBox ID="chkwithoutorder" Text="Check For Without OrderNo." runat="server"
                                        CssClass="checkboxbold" AutoPostBack="true" OnCheckedChanged="chkwithoutorder_CheckedChanged" />
                                    <br />
                                    <asp:DropDownList ID="DDOrderNo" runat="server" CssClass="dropdown" Width="200px"
                                        OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDProdCode" runat="server" visible="false" class="tdstyle">
                                    <asp:Label ID="Label17" class="tdstyle" runat="server" Text="  Prod Code" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:TextBox ID="TxtProdCode" runat="server" Width="100px" CssClass="textb"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div>
                    <fieldset>
                        <legend>
                            <asp:Label ID="Label2" runat="server" Text="Item Description" CssClass="labelnormalMM"
                                ForeColor="Red">
                            </asp:Label>
                        </legend>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblcategory" Text="CategoryName" runat="server" CssClass="labelbold" /><br />
                                    <asp:DropDownList ID="DDCategoryName" CssClass="dropdown" Width="150px" runat="server"
                                        OnSelectedIndexChanged="DDCategoryName_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblItemName" Text="ItemName" runat="server" CssClass="labelbold" /><br />
                                    <asp:DropDownList ID="DDItemName" CssClass="dropdown" Width="150px" runat="server"
                                        OnSelectedIndexChanged="DDItemName_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDQuality" runat="server" visible="false">
                                    <asp:Label ID="lblQuality" Text="Quality" runat="server" CssClass="labelbold" /><br />
                                    <asp:DropDownList ID="DDQuality" CssClass="dropdown" Width="150px" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDDesign" runat="server" visible="false">
                                    <asp:Label ID="lblDesign" Text="Design" runat="server" CssClass="labelbold" /><br />
                                    <asp:DropDownList ID="DDDesign" CssClass="dropdown" Width="150px" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDColor" runat="server" visible="false">
                                    <asp:Label ID="lblColor" Text="Color" runat="server" CssClass="labelbold" /><br />
                                    <asp:DropDownList ID="DDColor" CssClass="dropdown" Width="150px" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td id="TDShape" runat="server" visible="false">
                                    <asp:Label ID="lblshape" Text="Shape" runat="server" CssClass="labelbold" /><br />
                                    <asp:DropDownList ID="DDShape" CssClass="dropdown" Width="150px" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDSize" runat="server" visible="false">
                                    <asp:Label ID="lblSize" Text="Size" runat="server" CssClass="labelbold" />
                                    <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="True"
                                        OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged" />
                                    <br />
                                    <asp:DropDownList ID="DDSize" CssClass="dropdown" Width="150px" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td id="TDShade" runat="server" visible="false">
                                    <asp:Label ID="lblshade" Text="Shade" runat="server" CssClass="labelbold" /><br />
                                    <asp:DropDownList ID="DDShade" CssClass="dropdown" Width="150px" runat="server">
                                    </asp:DropDownList>
                                </td>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div>
                    <fieldset>
                        <legend>
                            <asp:Label ID="Label4" runat="server" Text="...." CssClass="labelnormalMM" ForeColor="Red">
                            </asp:Label>
                        </legend>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="BarCode" CssClass="labelbold">
                                    </asp:Label><br />
                                    <asp:TextBox ID="txtbarcode" CssClass="textb" runat="server" />
                                </td>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="Description" CssClass="labelbold">
                                    </asp:Label><br />
                                    <asp:TextBox ID="txtdescription" CssClass="textb" runat="server" Width="200px" />
                                </td>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Text="L" CssClass="labelbold">
                                    </asp:Label><br />
                                    <asp:TextBox ID="txtLength" CssClass="textb" runat="server" Width="50px" onchange="javascript:TextChanged();" />
                                </td>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Text="W" CssClass="labelbold">
                                    </asp:Label><br />
                                    <asp:TextBox ID="txtWidth" CssClass="textb" runat="server" Width="50px" onchange="javascript:TextChanged();" />
                                </td>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Text="H" CssClass="labelbold">
                                    </asp:Label><br />
                                    <asp:TextBox ID="txtHeight" CssClass="textb" runat="server" Width="50px" onchange="javascript:TextChanged();" />
                                </td>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Text="CBM" CssClass="labelbold">
                                    </asp:Label><br />
                                    <asp:TextBox ID="txtcbm" CssClass="textb" runat="server" Width="100px" Enabled="false" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div>
                    <fieldset>
                        <legend>
                            <asp:Label ID="Label3" runat="server" Text="Weight Detail." CssClass="labelnormalMM"
                                ForeColor="Red">
                            </asp:Label>
                        </legend>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblnetwtperpc" Text="Nt.Wt./pcs." runat="server" CssClass="labelnormalMM" /><br />
                                    <asp:TextBox ID="txtnetwtperpcs" runat="server" CssClass="textb" Width="80px" onchange="javascript: WeightTextChanged();" />
                                </td>
                                <td>
                                    <asp:Label ID="lblnetwtfabric" Text="Nt.Wt.Fabric" runat="server" CssClass="labelnormalMM" /><br />
                                    <asp:TextBox ID="txtnetwtfabric" runat="server" CssClass="textb" Width="80px" onchange="javascript: WeightTextChanged();" />
                                </td>
                                <td>
                                    <asp:Label ID="lblnetwtbeads" Text="Nt.Wt.Beads" runat="server" CssClass="labelnormalMM" /><br />
                                    <asp:TextBox ID="txtnetwtbeads" runat="server" CssClass="textb" Width="80px" onchange="javascript: WeightTextChanged();"
                                        BackColor="MenuBar" />
                                </td>
                                <td>
                                    <asp:Label ID="lblTnetwtfabric" Text="Total Nt.Wt.Fabric" runat="server" CssClass="labelnormalMM" /><br />
                                    <asp:TextBox ID="txttotalnetwtfabric" runat="server" CssClass="textb" Width="100px"
                                        Enabled="false" BackColor="MenuBar" />
                                </td>
                                <td>
                                    <asp:Label ID="lblTnetwtbeads" Text="Total Nt.Wt.Beads" runat="server" CssClass="labelnormalMM" /><br />
                                    <asp:TextBox ID="txttotalnetwtbeads" runat="server" CssClass="textb" Width="100px"
                                        Enabled="false" BackColor="MenuBar" />
                                </td>
                                <td>
                                    <asp:Label ID="lblTnetwt" Text="Total Nt.Wt." runat="server" CssClass="labelnormalMM" /><br />
                                    <asp:TextBox ID="txttotalnetwt" runat="server" CssClass="textb" Width="80px" Enabled="false"
                                        BackColor="MenuBar" />
                                </td>
                                <td>
                                    <asp:Label ID="lblcrtnweight" Text="Crtn.Wt." runat="server" CssClass="labelnormalMM" /><br />
                                    <asp:TextBox ID="txtcrtnwt" runat="server" CssClass="textb" Width="80px" onchange="javascript: WeightTextChanged();" />
                                </td>
                                <td>
                                    <asp:Label ID="lblTgrwt" Text="Total Gr.Wt." runat="server" CssClass="labelnormalMM" /><br />
                                    <asp:TextBox ID="txtTgrwt" runat="server" CssClass="textb" Width="80px" Enabled="false"
                                        BackColor="MenuBar" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblmessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                            </td>
                            <td align="right">
                                <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="height: 300px; overflow: auto; width: 800px">
                    <table>
                        <tr>
                            <td>
                                <asp:GridView ID="GVDetail" runat="server" AutoGenerateColumns="False" EmptyDataText="No Records Found..."
                                    OnRowDeleting="GVDetail_RowDeleting">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:BoundField DataField="RollFrom" HeaderText="Crtn. From">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="90px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RollTo" HeaderText="Crtn. To">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DesignName" HeaderText="Style#">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Description" HeaderText="Description">
                                            <ItemStyle Width="200px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Size" HeaderText="Size">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="120px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ColorName" HeaderText="Color">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TotalPcs" HeaderText="Qty.">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="netwtperpcs" HeaderText="Nt.Wt./Pc">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Netwtfabric" HeaderText="Nt.Wt.Fabric">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NetwtBeads" HeaderText="Nt.Wt.Beads">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Totalnetwtfabric" HeaderText="Total Nt.Wt.Fabric">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Totalnetwtbeads" HeaderText="Total Nt.Wt.Beads">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Totalnetwt" HeaderText="Total Nt.Wt.">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Totalgrosswt" HeaderText="Total Gr.Wt.">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="PackingDetailid" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPackingDetailId" Text='<%#Bind("PackingDetailId") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
