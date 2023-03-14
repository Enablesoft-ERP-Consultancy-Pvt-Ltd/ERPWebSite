<%@ Page Title="SAMPLE MATERIAL ISSUE" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmsamplerawissue.aspx.cs" Inherits="Masters_RawMaterial_frmsamplerawissue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/FixFocus2.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmsamplerawissue.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }

        function isNumberKeywithdecimal(evt) {
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
                    selectedindex = $("#<%=DDprocess.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Process. !!\n";
                    }
                    selectedindex = $("#<%=DDvendor.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Employee. !!\n";
                    }
                    selectedindex = $("#<%=DDissueNo.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Issue No. !!\n";
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
                    selectedindex = $("#<%=DDunit.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please Select Unit. !!\n";
                    }
                    selectedindex = $("#<%=DDGodown.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Godown. !!\n";
                    }
                    if ($("#<%=TDBinNo.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDBinNo.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Bin No. !!\n";
                        }
                    }
                    selectedindex = $("#<%=DDLotno.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Lot No. !!\n";
                    }
                    if ($("#<%=TDTagNo.ClientID %>").is(':visible')) {
                        selectedindex = $("#<%=DDTagNo.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please Select Tag No. !!\n";
                        }
                    }
                    var txtissueqty = document.getElementById('<%=txtissueqty.ClientID %>')
                    if (txtissueqty.value == "" || txtissueqty.value == "0") {
                        Message = Message + "Please Enter Issue qty. !!\n";
                    }
                    if (Message == "") {
                        return true;
                    }
                    else {
                        alert(Message);
                        return false;
                    }
                });
                //on DropDown Selected Index             
                $('#' + '<%=DDvendor.ClientID %>').change(function () {
                    $('#' + '<%=DDissueNo.ClientID %>').attr({ 'selectedIndex': 0 });
                });
                $('#' + '<%=dquality.ClientID %>').change(function () {
                    $('#' + '<%=dddesign.ClientID %>').attr({ 'selectedIndex': 0 });
                });
                $('#' + '<%=dddesign.ClientID %>').change(function () {
                    $('#' + '<%=ddcolor.ClientID %>').attr({ 'selectedIndex': 0 });
                });
                $('#' + '<%=ddlshade.ClientID %>').change(function () {
                    var godownid = $('#' + '<%=hnmodulegodownid.ClientID %>').val();
                    $('#' + '<%=DDGodown.ClientID %>').val(godownid);
                    $('#' + '<%=DDGodown.ClientID %>').change();
                });
                //                $('#' + '<%=DDGodown.ClientID %>').change(function () {
                //                    $('#' + '<%=DDLotno.ClientID %>').attr({ 'selectedIndex': 0 });
                //                    $('#' + '<%=DDLotno.ClientID %>').change();
                //                });
                //                $('#' + '<%=DDLotno.ClientID %>').change(function () {
                //                    $('#' + '<%=DDTagNo.ClientID %>').attr({ 'selectedIndex': 0 });
                //                    $('#' + '<%=DDTagNo.ClientID %>').change();
                //                });

            });
        }
    </script>
    <asp:UpdatePanel runat="server" ID="upd1">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label ID="lblmaster" Text="Master Details" runat="server" CssClass="labelbold"
                            ForeColor="Red" />
                    </legend>
                    <table>
                        <tr>
                            <td id="TDCheckedit" runat="server" visible="false">
                                <asp:CheckBox ID="chkedit" Text="For Edit" runat="server" AutoPostBack="true" CssClass="checkboxbold"
                                    OnCheckedChanged="chkedit_CheckedChanged" />
                            </td>
                            <td id="TDComplete" runat="server" visible="false">
                                <asp:CheckBox ID="chkforcomplete" runat="server" CssClass="checkboxbold" AutoPostBack="true"
                                    Text="For Complete" OnCheckedChanged="chkforcomplete_CheckedChanged" />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr id="TRempcodescan" runat="server" visible="false">
                            <td colspan="2">
                            </td>
                            <td>
                                <asp:Label Text="Enter/Scan Emp. Code" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtWeaverIdNoscan" CssClass="textb" runat="server" Width="150px"
                                    Height="20px" AutoPostBack="true" OnTextChanged="txtWeaverIdNoscan_TextChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblcompany" Text="Company Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDcompany" CssClass="dropdown" Width="150px" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" Text="Process Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDprocess" CssClass="dropdown" Width="150px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDprocess_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label3" Text="Vendor Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDvendor" CssClass="dropdown" Width="150px" runat="server"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDvendor_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label17" Text="Issue No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDissueNo" CssClass="dropdown" Width="130px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDissueNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDChallanNo" runat="server" visible="false">
                                <asp:Label ID="Label10" Text="Challan No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDChallanNo" CssClass="dropdown" Width="130px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDChallanNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label4" Text="Challan No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtchallanNo" CssClass="textb" runat="server" Width="90px" Enabled="false" />
                            </td>
                            <td>
                                <asp:Label ID="Label1" Text="Issue Date" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtissuedate" CssClass="textb" runat="server" Width="90px" />
                                <asp:CalendarExtender ID="cal1" runat="server" TargetControlID="txtissuedate" Format="dd-MMM-yyyy">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label Text="Item Details" CssClass="labelbold" ForeColor="Red" runat="server" />
                    </legend>
                    <table>
                        <tr id="Tr3" runat="server">
                            <td id="tdProCode" runat="server" visible="false">
                                <span class="labelbold">ProdCode</span>
                                <br />
                                <asp:TextBox ID="TxtProdCode" CssClass="textb" runat="server" Width="60px"></asp:TextBox>
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
                                <asp:DropDownList ID="dddesign" runat="server" Width="130px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="TDColor" runat="server" visible="false">
                                <asp:Label ID="lblcolorname" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddcolor" runat="server" Width="130px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="TDShape" runat="server" visible="false">
                                <asp:Label ID="lblshapename" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddshape" runat="server" Width="90px" AutoPostBack="True" CssClass="dropdown"
                                    OnSelectedIndexChanged="ddshape_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDSize" runat="server" visible="false">
                                <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged" Visible="false">
                                </asp:DropDownList>
                                <br />
                                <asp:DropDownList ID="ddsize" runat="server" Width="100px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="TDShade" runat="server" visible="false">
                                <asp:Label ID="lblshadecolor" runat="server" Text="ShadeColor" CssClass="labelbold"></asp:Label>
                                &nbsp;<br />
                                <asp:DropDownList ID="ddlshade" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblunit" Text="Unit" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDunit" CssClass="dropdown" Width="80px" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label5" Text="Godown Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDGodown" CssClass="dropdown" Width="150px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDGodown_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label6" Text="Lot No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDLotno" CssClass="dropdown" Width="150px" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="DDLotno_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDTagNo" runat="server" visible="false">
                                <asp:Label ID="Label7" Text="Tag No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDTagNo" CssClass="dropdown" Width="150px" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="DDTagNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDBinNo" runat="server" visible="false">
                                <asp:Label ID="Label11" Text="Bin No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDBinNo" CssClass="dropdown" Width="150px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDBinNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label8" Text="Stock Qty." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtstockqty" runat="server" CssClass="textb" Width="80px" Enabled="false"
                                    BackColor="Yellow" />
                            </td>
                            <td>
                                <asp:Label ID="Label9" Text="Issue Qty." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtissueqty" runat="server" CssClass="textb" Width="80px" onkeypress="return isNumberKeywithdecimal(event)" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblremark" Text="Remark" runat="server" CssClass="labelbold" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtremark" CssClass="textb" TextMode="MultiLine" runat="server"
                            Height="55px" Width="706px" />
                    </td>
                </tr>
            </table>
            <div>
                <table style="width: 100%">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnnew" Text="New" runat="server" CssClass="buttonnorm" OnClientClick="return NewForm()" />
                            <asp:Button ID="btnsave" Text="Save" runat="server" CssClass="buttonnorm" OnClick="btnsave_Click" />
                            <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm()" />
                            <asp:Button ID="btnpreview" Text="Preview" runat="server" CssClass="buttonnorm" OnClick="btnpreview_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <table>
                <tr>
                    <td>
                        <div style="max-height: 500px; overflow: auto;">
                            <asp:GridView ID="gvdetail" runat="server" AutoGenerateColumns="False" DataKeyNames="prtid"
                                OnRowEditing="gvdetail_RowEditing" OnRowCancelingEdit="gvdetail_RowCancelingEdit"
                                OnRowDeleting="gvdetail_RowDeleting" OnRowUpdating="gvdetail_RowUpdating">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Item Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblitemname" Text='<%#Bind("Item_Name") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldesc" Text='<%#Bind("Description") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="lblqty" Text='<%#Bind("qty") %>' runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtqtyedit" Text='<%#Bind("qty") %>' runat="server" Width="70px"
                                                BackColor="Yellow" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Lot No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lbllotno" Text='<%#Bind("lotno") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tag No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lbltagno" Text='<%#Bind("Tagno") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Godown Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgodown" Text='<%#Bind("Godownname") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DEL" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                Text="DEL" OnClientClick="return confirm('Do You Want To Delete?')"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField DeleteText="" ShowEditButton="True" />
                                </Columns>
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hnprmid" runat="server" Value="0" />
            <asp:HiddenField ID="hnmodulegodownid" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
