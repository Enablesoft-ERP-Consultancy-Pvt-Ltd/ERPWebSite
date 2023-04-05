<%@ Page Title="Yarn opening Issue" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmyarnopeningIssue.aspx.cs" Inherits="Masters_YarnOpening_frmyarnopeningIssue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="CPH" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmyarnopeningissue.aspx";
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
                    var Dept = document.getElementById('<%=DDdept.ClientID %>');
                    if (Dept != null) {
                        selectedindex = $("#<%=DDdept.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Department Name!!\n";
                        }
                    }
                    selectedindex = $("#<%=DDvendor.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please Select Dept. !!\n";
                    }
                    selectedindex = $("#<%=ddCatagory.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Category !!\n";
                    }
                    selectedindex = $("#<%=dditemname.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Item !!\n";
                    }
                    if ($("#<%=TDQuality.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=dquality.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Quality. !!\n";
                        }
                    }
                    if ($("#<%=TDDesign.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=dddesign.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Design. !!\n";
                        }
                    }
                    if ($("#<%=TDColor.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=ddcolor.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Color. !!\n";
                        }
                    }
                    if ($("#<%=TDShape.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=ddshape.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Shape. !!\n";
                        }
                    }
                    if ($("#<%=TDSize.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=ddsize.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Size. !!\n";
                        }
                    }
                    if ($("#<%=TDShade.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=ddlshade.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Shade. !!\n";
                        }
                    }
                    selectedindex = $("#<%=ddlunit.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Unit !!\n";
                    }
                    selectedindex = $("#<%=DDGodown.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Godown !!\n";
                    }
                    selectedindex = $("#<%=DDLotNo.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Lot No. !!\n";
                    }
                    selectedindex = $("#<%=DDTagNo.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Tag No. !!\n";
                    }
                    if ($("#<%=TDBinno.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDbinno.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Bin No. !!\n";
                        }
                    }
                    var txtissqty = document.getElementById('<%=txtissqty.ClientID %>');
                    if (txtissqty.value == "" || txtissqty.value == "0") {
                        Message = Message + "Please Enter Issue Qty. !!\n";
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
    <asp:UpdatePanel ID="R" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label ID="lblMasterDetail" runat="server" Text="Master Details" CssClass="labelbold"
                            ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr>
                            <td>
                                <asp:CheckBox ID="Chkedit" Text="For Edit" CssClass="checkboxbold" AutoPostBack="true"
                                    runat="server" OnCheckedChanged="Chkedit_CheckedChanged" />
                            </td>
                            <td id="TRempcodescan" runat="server" visible="false" colspan="2">
                                <asp:Label ID="Label18" Text="Enter/Scan Emp. Code" runat="server" CssClass="labelbold" />
                                &nbsp;
                                <asp:TextBox ID="txtWeaverIdNoscan" CssClass="textb" runat="server" Width="150px"
                                    Height="20px" AutoPostBack="true" OnTextChanged="txtWeaverIdNoscan_TextChanged" />
                            </td>
                            <td id="TDdept" runat="server" visible="false">
                                <asp:DropDownList ID="DDdept" CssClass="dropdown" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DDdept_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label33" runat="server" Text="BranchName" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDBranchName" Width="150" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblyarnopendept" runat="server" Text="Yarn Opening Employee." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDvendor" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="200px" OnSelectedIndexChanged="DDvendor_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td runat="server" id="Tdcustcode">
                                <asp:Label ID="lblcustcode" Text="Customer code" CssClass="labelbold" runat="server" />
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDcustcode" Width="130px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDcustcode_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="Tdorderno" runat="server">
                                <asp:Label ID="Label20" Text="Order No." CssClass="labelbold" runat="server" />
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDorderno" Width="150px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDorderno_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDIssuedNo" runat="server" visible="false">
                                <asp:Label ID="Label14" runat="server" Text="Issued No." CssClass="labelbold"></asp:Label>
                                <asp:CheckBox ID="chkforComp" Text="For Complete" CssClass="checkboxbold" AutoPostBack="true"
                                    runat="server" OnCheckedChanged="chkforComp_CheckedChanged" />
                                <br />
                                <asp:DropDownList ID="DDissuedNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDissuedNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Issue No." CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtissueno" CssClass="textb" Width="100px" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Issue Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtissuedate" CssClass="textb" Width="100px" runat="server" />
                                <asp:CalendarExtender ID="cal1" TargetControlID="txtissuedate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Target Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txttargetdate" CssClass="textb" Width="100px" runat="server" />
                                <asp:CalendarExtender ID="cal2" TargetControlID="txttargetdate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label5" runat="server" Text="Item Details" CssClass="labelbold" ForeColor="Red"
                            Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr id="Tr3" runat="server">
                            <td id="tdProCode" runat="server" visible="false" class="tdstyle">
                                <span class="labelbold">ProdCode</span>
                                <br />
                                <asp:TextBox ID="TxtProdCode" CssClass="textb" runat="server" Width="100px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblcategoryname" runat="server" class="tdstyle" Text="Category Name"
                                    CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddCatagory" runat="server" Width="150px" CssClass="dropdown"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblitemname" class="tdstyle" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="dditemname" runat="server" Width="150px" CssClass="dropdown"
                                    AutoPostBack="True" OnSelectedIndexChanged="dditemname_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDQuality" runat="server" visible="false">
                                <asp:Label ID="lblqualityname" class="tdstyle" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="dquality" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td id="TDDesign" runat="server" visible="false" class="style5">
                                <asp:Label ID="lbldesignname" runat="server" class="tdstyle" Text="Design" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="dddesign" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="TDColor" runat="server" visible="false">
                                <asp:Label ID="lblcolorname" runat="server" class="tdstyle" Text="Color" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddcolor" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td id="TDShape" runat="server" visible="false">
                                <asp:Label ID="lblshapename" class="tdstyle" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddshape" runat="server" Width="150px" AutoPostBack="True" CssClass="dropdown"
                                    OnSelectedIndexChanged="ddshape_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDSize" runat="server" class="tdstyle" visible="false">
                                <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged">
                                </asp:DropDownList>
                                <br />
                                <asp:DropDownList ID="ddsize" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="TDShade" runat="server" visible="false">
                                <asp:Label ID="lblshadecolor" runat="server" class="tdstyle" Text="ShadeColor" CssClass="labelbold"></asp:Label>
                                &nbsp;<br />
                                <asp:DropDownList ID="ddlshade" runat="server" Width="150px" CssClass="dropdown"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlshade_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label16" runat="server" Text="Unit" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddlunit" runat="server" Width="115px" AutoPostBack="true" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="GodownName" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDGodown" runat="server" Width="150px" AutoPostBack="true"
                                    CssClass="dropdown" OnSelectedIndexChanged="DDGodown_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="Lot No." CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDLotNo" runat="server" Width="150px" AutoPostBack="true" CssClass="dropdown"
                                    OnSelectedIndexChanged="DDLotNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="Tag No." CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDTagNo" runat="server" Width="150px" AutoPostBack="true" CssClass="dropdown"
                                    OnSelectedIndexChanged="DDTagNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDBinno" runat="server" visible="false">
                                <asp:Label ID="Label17" runat="server" Text="Bin No." CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDbinno" runat="server" Width="150px" AutoPostBack="true" CssClass="dropdown"
                                    OnSelectedIndexChanged="DDbinno_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="Stock Qty." CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtstockqty" runat="server" Width="60px" CssClass="textb" Enabled="false"></asp:TextBox>
                            </td>
                            <td id="Tdrate" runat="server">
                                <asp:Label ID="Label15" runat="server" Text="Rate" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtrate" CssClass="textb" Width="70px" runat="server" onkeypress="return isNumberKey(event);" />
                            </td>
                            <td>
                                <asp:Label ID="Label19" runat="server" Text="Issue Qty." CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtissqty" runat="server" Width="60px" CssClass="textb" onkeypress="return isNumberKey(event);"
                                    BackColor="Yellow"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="Rec. Type" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDrectype" runat="server" CssClass="dropdown">
                                    <asp:ListItem Text="Cone" Value="Cone" />
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="No. of Cone" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtnoofcone" runat="server" Width="60px" CssClass="textb"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label12" runat="server" Text="Cone Type" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDconetype" CssClass="dropdown" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label30" Text="Ply Type" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDPly" CssClass="dropdown" runat="server">
                                    <asp:ListItem Text="" />
                                    <asp:ListItem Text="1 Ply" />
                                    <asp:ListItem Text="2 Ply" />
                                    <asp:ListItem Text="3 Ply" />
                                    <asp:ListItem Text="4 Ply" />
                                    <asp:ListItem Text="5 Ply" />
                                    <asp:ListItem Text="6 Ply" />
                                    <asp:ListItem Text="7 Ply" />
                                    <asp:ListItem Text="8 Ply" />
                                    <asp:ListItem Text="9 Ply" />
                                    <asp:ListItem Text="10 Ply" />
                                    <asp:ListItem Text="11 Ply" />
                                    <asp:ListItem Text="12 Ply" />
                                    <asp:ListItem Text="8-32 Ply" />
                                    <asp:ListItem Text="30 Ply" />
                                    <asp:ListItem Text="15 Ply" />
                                    <asp:ListItem Text="21 Ply" />                                   
                                    <asp:ListItem Text="13 Ply" />
                                    <asp:ListItem Text="14 Ply" />
                                    <asp:ListItem Text="20 Ply" />
                                    <asp:ListItem Text="28 Ply" />
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label31" Text="Tranport" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDTransport" CssClass="dropdown" runat="server">
                                    <asp:ListItem Text="" />
                                    <asp:ListItem Text="Self" />
                                    <asp:ListItem Text="Company" />
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Bell Wt" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtBellWt" runat="server" Width="60px" CssClass="textb"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label21" runat="server" Text="Moisture" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtMoisture" runat="server" Width="80px" CssClass="textb"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblremark" Text="Remark" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtremark" CssClass="textb" TextMode="MultiLine" runat="server"
                                Height="55px" Width="706px" />
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkremark" Text="Update Remark" CssClass="lnkbtnClass" Visible="false"
                                runat="server" OnClick="lnkremark_Click"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
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
                            <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                            <asp:Button ID="BtnComplete" runat="server" Text="Complete" CssClass="buttonnorm"
                                Visible="false" OnClick="BtnComplete_Click" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <div id="gride" runat="server" style="height: 300px; overflow: auto">
                    <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                        OnRowCancelingEdit="DG_RowCancelingEdit" OnRowEditing="DG_RowEditing" OnRowUpdating="DG_RowUpdating"
                        OnRowDeleting="DG_RowDeleting">
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts" />
                        <RowStyle CssClass="gvrow" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                        <Columns>
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
                                    <asp:Label ID="lblGodown" Text='<%#Bind("GodownName") %>' runat="server" />
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
                            <asp:TemplateField HeaderText="Issue Qty">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtqty" Width="60px" align="right" runat="server" Text='<%# Bind("IssueQty") %>'
                                        BackColor="#FFFF66" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblQty" Text='<%#Bind("IssueQty") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rec Type">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtrectype" Width="50px" runat="server" Text='<%# Bind("Rectype") %>'
                                        BackColor="#FFFF66"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRecType" Text='<%#Bind("Rectype") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="No of Cone">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtnoofcone" Width="50px" align="right" runat="server" Text='<%# Bind("Noofcone") %>'
                                        BackColor="#FFFF66" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblnoofcone" Text='<%#Bind("noofcone") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cone Type">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtconetype" Width="50px" runat="server" Text='<%# Bind("conetype") %>'
                                        BackColor="#FFFF66"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblconetype" Text='<%#Bind("conetype") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rate">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtRate" Width="50px" runat="server" Text='<%# Bind("Rate") %>'
                                        BackColor="#FFFF66"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRate" Text='<%#Bind("Rate") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" Text='<%#Bind("Id") %>' runat="server" />
                                    <asp:Label ID="lbldetailid" Text='<%#Bind("Detailid") %>' runat="server" />
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
                            <asp:CommandField ShowEditButton="True" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
