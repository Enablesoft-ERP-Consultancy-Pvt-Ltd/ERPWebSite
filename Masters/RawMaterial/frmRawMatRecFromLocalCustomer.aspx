<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmRawMatRecFromLocalCustomer.aspx.cs"
    Inherits="Masters_RawMaterial_frmRawMatRecFromLocalCustomer" MasterPageFile="~/ERPmaster.master"
    Title="Raw Material Receive From Local Customer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">

        //        function Call() {
        //            var seconds = 15;
        //            document.getElementById("CPH_Form_lblMessage").style.display = "none";
        //            setTimeout("Call()", seconds * 1000);
        //            document.getElementById("CPH_Form_lblMessagebe").innerHTML = '';
        //        }
        //        window.onload = function () {
        //            Call();
        //        }
        function CloseForm() {

            window.location.href = "../../main.aspx";
        }
        function NewForm() {
            window.location.href = "frmRawMatRecFromLocalCustomer.aspx";
        }
        function Validate() {
            if (document.getElementById("<%=DDCompanyName.ClientID %>").value <= "0") {
                alert("Please select Company Name....!");
                return false;
            }

            if (document.getElementById("<%=DDCustomer.ClientID %>").value <= "0") {
                alert("Please select Customer Name....!");
                return false;
            }
            if (document.getElementById("<%=DDOrderNo.ClientID %>").value <= "0") {
                alert("Please select Order No....!");
                return false;
            }
            if (document.getElementById('CPH_Form_ddCatagory') != null) {
                if (document.getElementById('CPH_Form_ddCatagory').options[document.getElementById('CPH_Form_ddCatagory').selectedIndex].value == 0) {
                    alert("Please select Category....!");
                    document.getElementById("CPH_Form_ddCatagory").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_dditemname') != null) {
                if (document.getElementById('CPH_Form_dditemname').options[document.getElementById('CPH_Form_dditemname').selectedIndex].value == 0) {
                    alert("Please select ItemName....!");
                    document.getElementById("CPH_Form_dditemname").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_dquality') != null) {
                if (document.getElementById('CPH_Form_dquality').options[document.getElementById('CPH_Form_dquality').selectedIndex].value == 0) {
                    alert("Please select Quality....!");
                    document.getElementById("CPH_Form_dquality").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_dddesign') != null) {
                if (document.getElementById('CPH_Form_dddesign').options[document.getElementById('CPH_Form_dddesign').selectedIndex].value == 0) {
                    alert("Please select Design....!");
                    document.getElementById("CPH_Form_dddesign").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_ddcolor') != null) {
                if (document.getElementById('CPH_Form_ddcolor').options[document.getElementById('CPH_Form_ddcolor').selectedIndex].value == 0) {
                    alert("Please select Color....!");
                    document.getElementById("CPH_Form_ddcolor").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_ddlshade') != null) {
                if (document.getElementById('CPH_Form_ddlshade').options[document.getElementById('CPH_Form_ddlshade').selectedIndex].value == 0) {
                    alert("Please select ShadeColor....!");
                    document.getElementById("CPH_Form_ddlshade").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_ddshape') != null) {
                if (document.getElementById('CPH_Form_ddshape').options[document.getElementById('CPH_Form_ddshape').selectedIndex].value == 0) {
                    alert("Please select Shape....!");
                    document.getElementById("CPH_Form_ddshape").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_ddsize') != null) {
                if (document.getElementById('CPH_Form_ddsize').options[document.getElementById('CPH_Form_ddsize').selectedIndex].value == 0) {
                    alert("Please select Size....!");
                    document.getElementById("CPH_Form_ddsize").focus();
                    return false;
                }
            }
            if (document.getElementById('CPH_Form_DDgodown') != null) {
                if (document.getElementById('CPH_Form_DDgodown').options[document.getElementById('CPH_Form_DDgodown').selectedIndex].value == 0) {
                    alert("Please select Godown....!");
                    document.getElementById("CPH_Form_DDgodown").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=txtRecQty.ClientID %>").value == "" || document.getElementById("<%=txtRecQty.ClientID %>").value == "0") {
                alert("Receive Qty  Cannot be blank or Zero..");
                document.getElementById("<%=txtRecQty.ClientID %>").focus();
                return false;
            }
            return confirm("Do you Want to Save?");
        }
        function isNumber(evt) {
            var charcode = (evt.which) ? evt.which : evt.keycode;
            if (charcode != 46 && charcode > 31 && (charcode < 48 || charcode > 57)) {

                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 100%">
                <fieldset>
                    <legend>
                        <asp:Label ID="lblMaser" runat="server" Text="Master Details" CssClass="labelnormalMM"
                            Font-Bold="true" ForeColor="Red"></asp:Label></legend>
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCompany" runat="server" Text="CompanyName" CssClass="labelnormalMM"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="DDCompanyName" runat="server" Width="200px" CssClass="dropdown">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblCustomer" runat="server" Text="CustomerName" CssClass="labelnormalMM"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="DDCustomer" runat="server" Width="200px" CssClass="dropdown"
                                        AutoPostBack="true" OnSelectedIndexChanged="DDCustomer_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblOrderNo" runat="server" Text="OrderNo" CssClass="labelnormalMM"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="DDOrderNo" runat="server" Width="150px" CssClass="dropdown"
                                        AutoPostBack="true" OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblRecDate" runat="server" Text="Receive Date" CssClass="labelnormalMM"></asp:Label>
                                    <br />
                                    <asp:TextBox ID="txtRecDate" runat="server" Width="100px" Height="22px" CssClass="textb"
                                        BackColor="Yellow"></asp:TextBox>
                                    <asp:CalendarExtender ID="cal1" runat="server" TargetControlID="txtRecDate" Format="dd-MMM-yyyy">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lblChallanNo" runat="server" Text="Challan No" CssClass="labelnormalMM"></asp:Label>
                                    <br />
                                    <asp:TextBox ID="txtChallanNo" runat="server" Width="100px" Enabled="false" Height="22px"
                                        CssClass="textb"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label1" runat="server" Text="Item Details" CssClass="labelnormalMM"
                            Font-Bold="true" ForeColor="Red"></asp:Label></legend>
                    <div style="width: 100%">
                        <table>
                            <tr>
                                <td runat="server" visible="false">
                                    <asp:Label ID="lblProdcode" runat="server" class="tdstyle" Text="ProdCode" CssClass="labelnormalMM"></asp:Label>
                                    <br />
                                    <asp:TextBox ID="TxtProdCode" CssClass="textb" runat="server" Width="100px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblcategoryname" runat="server" class="tdstyle" Text="Category Name"
                                        CssClass="labelnormalMM"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="ddCatagory" runat="server" Width="125px" CssClass="dropdown"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblitemname" class="tdstyle" runat="server" Text="Item Name" CssClass="labelnormalMM"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="dditemname" runat="server" Width="150px" CssClass="dropdown"
                                        AutoPostBack="True" OnSelectedIndexChanged="dditemname_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td id="ql" runat="server" visible="false">
                                    <asp:Label ID="lblqualityname" class="tdstyle" runat="server" Text="Quality" CssClass="labelnormalMM"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="dquality" runat="server" CssClass="dropdown" AutoPostBack="True"
                                        Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td id="dsn" runat="server" visible="false" class="style5">
                                    <asp:Label ID="lbldesignname" runat="server" class="tdstyle" Text="Design" CssClass="labelnormalMM"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="dddesign" runat="server" Width="150px" CssClass="dropdown"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td id="clr" runat="server" visible="false">
                                    <asp:Label ID="lblcolorname" runat="server" class="tdstyle" Text="Color" CssClass="labelnormalMM"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="ddcolor" runat="server" Width="130px" CssClass="dropdown" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td id="shd" runat="server" visible="false">
                                    <asp:Label ID="lblshadecolor" runat="server" class="tdstyle" Text="ShadeColor" CssClass="labelnormalMM"></asp:Label>
                                    &nbsp;<br />
                                    <asp:DropDownList ID="ddlshade" runat="server" Width="150px" CssClass="dropdown"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td id="shp" runat="server" visible="false">
                                    <asp:Label ID="lblshapename" class="tdstyle" runat="server" Text="Shape" CssClass="labelnormalMM"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="ddshape" runat="server" Width="125px" AutoPostBack="True" CssClass="dropdown">
                                    </asp:DropDownList>
                                </td>
                                <td id="sz" runat="server" class="tdstyle" visible="false">
                                    <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelnormalMM"></asp:Label>
                                    <%--<asp:CheckBox ID="ChkForMtr" runat="server" AutoPostBack="True" OnCheckedChanged="ChkForMtr_CheckedChanged" />Check
                                    for Mtr--%><asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server"
                                        AutoPostBack="True">
                                        <%--  <asp:ListItem Value="0" Selected="True">Ft</asp:ListItem>
                                        <asp:ListItem Value="1">MTR</asp:ListItem>
                                        <asp:ListItem Value="2">Inch</asp:ListItem>--%>
                                    </asp:DropDownList>
                                    <br />
                                    <asp:DropDownList ID="ddsize" runat="server" Width="125px" CssClass="dropdown" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td id="TdFinish_Type" runat="server" visible="false" class="tdstyle">
                                    <asp:Label ID="LblFinish_Type" runat="server" Text="Finish_Type" CssClass="labelnormalMM"></asp:Label>
                                    &nbsp;<br />
                                    <asp:DropDownList ID="ddFinish_Type" runat="server" Width="150px" CssClass="dropdown"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td class="tdstyle" runat="server" visible="false">
                                    <asp:Label ID="Label16" runat="server" Text="Unit" CssClass="labelnormalMM"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="ddlunit" runat="server" Width="115px" AutoPostBack="true" CssClass="dropdown">
                                    </asp:DropDownList>
                                </td>
                                <td id="Td1" class="tdstyle" runat="server">
                                    <asp:Label ID="lblGodowN" runat="server" Text="Godown" CssClass="labelnormalMM"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="DDgodown" runat="server" Width="125px" AutoPostBack="true"
                                        CssClass="dropdown">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblRecQty" runat="server" Text="Receive Qty" CssClass="labelnormalMM"></asp:Label>
                                    <br />
                                    <asp:TextBox ID="txtRecQty" runat="server" Width="70px" Height="22px" BackColor="Yellow"
                                        CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                                    &nbsp
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonnorm" Width="70px"
                                        OnClientClick="return Validate();" OnClick="btnSave_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
                <div>
                    <div style="width: 800px; margin-left: 0px; height: auto">
                        <div style="width: 680px; background-color: Teal; padding-left: 20px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkTransportInformation" runat="server" Text=" TRANSPORT INFORMATION "
                                            ForeColor="White" CssClass="labelnormalMM" AutoPostBack="true" OnCheckedChanged="chkTransportInformation_CheckedChanged" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div>
                        </div>
                        <div id="DivTransPort" runat="server" style="border: 1px Solid; width: 805px;" visible="false">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblTName" runat="server" Text="NAME OF TRANSPORT" CssClass="labelnormalMM"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTransportName" runat="server" Width="250px" CssClass="textb"
                                            Height="25px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTAddress" runat="server" Text="ADDRESS OF TRANSPORT" CssClass="labelnormalMM"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTransportAddress" runat="server" Width="264px" TextMode="MultiLine"
                                            CssClass="textb" Height="45px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDName" Text="DRIVER NAME" runat="server" CssClass="labelnormalMM"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDriver" runat="server" Width="200px" CssClass="textb" Height="25px"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblTruckNumber" Text="VEHICLE NO." runat="server" CssClass="labelnormalMM"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVehicleNo" runat="server" Width="200px" CssClass="textb" Height="25px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div style="width: 700px; margin-top: 10px" runat="server" id="divNewClose">
                        <table width="690px">
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btnNew" runat="server" Text="New" CssClass="buttonnorm" Width="70px"
                                        OnClientClick="return NewForm();" />
                                    &nbsp;
                                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="buttonnorm" Width="70px"
                                        OnClientClick="return CloseForm();" />
                                    &nbsp;
                                    <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" Width="70px"
                                        OnClick="btnPreview_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="margin-left: 10px">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="labelnormalMM" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 700px; height: 200px; overflow: auto; margin-top: 20px">
                        <asp:GridView ID="gvdetail" runat="server" AutoGenerateColumns="False" DataKeyNames="DetailId"
                            CssClass="grid-view" OnRowCreated="gvdetail_RowCreated" OnRowDeleting="gvdetail_RowDeleting">
                            <Columns>
                                <asp:BoundField DataField="CATEGORY_NAME" HeaderText="Catagory">
                                    <HeaderStyle Width="150px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ITEM_NAME" HeaderText="Item Name">
                                    <HeaderStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="GodownName" HeaderText="Godown Name">
                                    <HeaderStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DESCRIPTION" HeaderText="Description">
                                    <HeaderStyle Width="150px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LotNo" HeaderText="LotNo" />
                                <asp:BoundField DataField="Qty" HeaderText="ReceiveQty" />
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReceiveId" runat="server" Text='<%#Bind("ReceiveId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ShowDeleteButton="True" />
                            </Columns>
                            <HeaderStyle CssClass="gvheader" />
                            <AlternatingRowStyle CssClass="gvalt" />
                            <RowStyle CssClass="gvrow" />
                            <EmptyDataRowStyle CssClass="gvemptytext" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div>
                <asp:HiddenField ID="hnReceiveId" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
