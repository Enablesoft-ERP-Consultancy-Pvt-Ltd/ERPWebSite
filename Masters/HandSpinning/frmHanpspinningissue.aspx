<%@ Page Title="HAND SPINNING ISSUE" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmHanpspinningissue.aspx.cs" Inherits="Masters_HandSpinning_frmHanpspinningissue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/FixFocus2.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmhanpspinningissue.aspx";
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
                    var selectedindex = $("#<%=DDCompanyName.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Company Name!!\n";
                    }
                    selectedindex = $("#<%=DDProcessName.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Process. !!\n";
                    }
                    selectedindex = $("#<%=DDPartyName.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Vendor. !!\n";
                    }
                    if ($("#<%=TDissueno.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDissueno.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Issue No. !!\n";
                        }
                    }
                    selectedindex = $("#<%=DDCategory.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Category !!\n";
                    }
                    selectedindex = $("#<%=DDItem.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Item !!\n";
                    }
                    if ($("#<%=TDIquality.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDQuality.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Quality. !!\n";
                        }
                    }
                    if ($("#<%=TDIdesign.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDDesign.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Design. !!\n";
                        }
                    }
                    if ($("#<%=TDIcolor.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDColor.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Color. !!\n";
                        }
                    }
                    if ($("#<%=TDIShape.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDShape.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Shape. !!\n";
                        }
                    }
                    if ($("#<%=TDISize.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDSize.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Size. !!\n";
                        }
                    }
                    if ($("#<%=TDIColorShade.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDColorShade.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Shade. !!\n";
                        }
                    }
                    selectedindex = $("#<%=DDgodown.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Godown !!\n";
                    }
                    if ($("#<%=TDBinNo.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDBinNo.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select BinNo. !!\n";
                        }
                    }
                    selectedindex = $("#<%=DDLotno.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Lot No !!\n";
                    }
                    if ($("#<%=TDTagno.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDTagNo.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Tag No. !!\n";
                        }
                    }
                    //Receive Item
                    selectedindex = $("#<%=DDRcategory.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Receive Category !!\n";
                    }
                    selectedindex = $("#<%=DDRitemName.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Receive Item !!\n";
                    }
                    if ($("#<%=TDRquality.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDRquality.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Receive Quality. !!\n";
                        }
                    }
                    if ($("#<%=TDRdesign.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDRdesign.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Receive Design. !!\n";
                        }
                    }
                    if ($("#<%=TDRcolor.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDRcolor.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Receive Color. !!\n";
                        }
                    }
                    if ($("#<%=TDRShape.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDRshape.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Receive Shape. !!\n";
                        }
                    }
                    if ($("#<%=TDRSize.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDRsize.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Receive Size. !!\n";
                        }
                    }
                    if ($("#<%=TDRShadecolor.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=DDRShadecolor.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Receive Shade. !!\n";
                        }
                    }
                    var txtrecqty = document.getElementById('<%=txtrecqty.ClientID %>');
                    if (txtrecqty.value == "" || txtrecqty.value == "0") {
                        Message = Message + "Please Enter QTY. !!\n";
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
                <table>
                    <tr>
                        <td id="TREdit" runat="server" visible="false">
                            <asp:CheckBox ID="chkedit" Text="Edit" CssClass="checkboxbold" Font-Size="Small"
                                AutoPostBack="true" runat="server" OnCheckedChanged="chkedit_CheckedChanged" />
                        </td>
                        <td id="TDComplete" runat="server" visible="false">
                            <asp:CheckBox ID="chkcomplete" Text="Fill Complete Issue No." CssClass="checkboxbold"
                                Font-Size="Small" AutoPostBack="true" runat="server" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="2">
                        </td>
                        <td id="TRempcodescan" runat="server" visible="false">
                            <asp:Label ID="Label21" Text="Enter/Scan Emp. Code" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtWeaverIdNoscan" CssClass="textb" runat="server" Width="150px"
                                Height="20px" AutoPostBack="true" OnTextChanged="txtWeaverIdNoscan_TextChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" Width="150px" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Process Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDProcessName" Width="150px" AutoPostBack="true"
                                runat="server" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Vendor Name" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDPartyName" Width="180px" AutoPostBack="true"
                                runat="server" OnSelectedIndexChanged="DDPartyName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TDissueno" runat="server" visible="false">
                            <asp:Label ID="lblindentnoedit" runat="server" Text="Issue No" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDissueno" Width="130px" AutoPostBack="true"
                                runat="server" OnSelectedIndexChanged="DDissueno_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblcustcode" Text="Customer code" CssClass="labelbold" runat="server" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDcustcode" Width="130px" AutoPostBack="true"
                                runat="server" OnSelectedIndexChanged="DDcustcode_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label2" Text="Order No." CssClass="labelbold" runat="server" />
                            <br />
                            <asp:DropDownList CssClass="dropdown" ID="DDorderno" Width="150px" AutoPostBack="true"
                                runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblindentno" runat="server" Text=" Issue No." CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox CssClass="textb" ID="TxtIndentNo" Width="90px" Enabled="false" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="Issue Date" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox CssClass="textb" ID="TxtDate" runat="server" Width="80px" TabIndex="7"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtDate">
                            </asp:CalendarExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label9" runat="server" Text=" ReqDate" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox CssClass="textb" ID="TxtReqDate" runat="server" Width="80px" AutoPostBack="false"
                                BackColor="#7b96bb "></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtReqDate">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label ID="lbliss" Text="Issue Detail" runat="server" CssClass="labelbold" ForeColor="Red" />
                    </legend>
                    <table>
                        <tr>
                            <td id="TDprodcode" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="Label10" runat="server" Text=" ProdCode" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox CssClass="textb" ID="TxtProdCode" Width="60px" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="LblCategory" class="tdstyle" runat="server" Text="ICategory" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDCategory" Width="110px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="LblItemName" class="tdstyle" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDItem" Width="150px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDItem_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDIquality" runat="server" visible="false">
                                <asp:Label ID="LblQuality" runat="server" Text="Quality" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" ID="DDQuality" Width="150px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDIdesign" runat="server" visible="false">
                                <asp:Label ID="LblDesign" runat="server" Text="Design" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" Width="110px" ID="DDDesign" runat="server"
                                    OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDIcolor" runat="server" visible="false">
                                <asp:Label ID="LblColor" runat="server" Text="Color" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" Width="110px" ID="DDColor" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="DDColor_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDIColorShade" runat="server" visible="false">
                                <asp:Label ID="LblColorShade" runat="server" Text="Shadecolor" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" Width="150" ID="DDColorShade" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDColorShade_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td id="TDIShape" runat="server" visible="false">
                                <asp:Label ID="LblShape" runat="server" Text="Shape" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" Width="110px" ID="DDShape" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDISize" runat="server" visible="false">
                                <asp:Label ID="LblSize" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                <asp:DropDownList CssClass="dropdown" Width="50px" ID="DDsizetype" runat="server"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged">
                                </asp:DropDownList>
                                <br />
                                <asp:DropDownList CssClass="dropdown" Width="110px" ID="DDSize" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="Unit" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddUnit" runat="server" CssClass="dropdown" Width="100px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Godown" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDgodown" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDgodown_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Lot No" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDLotno" runat="server" CssClass="dropdown" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DDLotno_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDTagno" runat="server" visible="false">
                                <asp:Label ID="lbltagno" runat="server" Text="Tag No" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDTagNo" runat="server" CssClass="dropdown" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DDTagNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDBinNo" runat="server" visible="false">
                                <asp:Label ID="Label7" runat="server" Text="Bin No." CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDBinNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDBinNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TD1" runat="server" valign="bottom">
                                <asp:Label ID="Label12" runat="server" Text="Stock Qty." CssClass="labelbold" ForeColor="Red"></asp:Label>
                                <asp:Label ID="lblstockqty" runat="server" Text="" CssClass="labelbold"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <asp:CheckBox ID="chkfillsame" Text="For Fill Same Receive Detail" runat="server"
                    CssClass="checkboxbold" Font-Bold="true" Font-Size="Small" />
            </div>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label13" runat="server" Text="Receive Detail" CssClass="labelbold"
                            ForeColor="Red"></asp:Label>
                    </legend>
                    <table>
                        <tr>
                            <td id="TD2" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="Label14" runat="server" Text=" ProdCode" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox CssClass="textb" ID="txtRprodcode" Width="60px" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label15" class="tdstyle" runat="server" Text="RCategory" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDRcategory" Width="110px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDRcategory_SelectedIndexChanged1">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label16" class="tdstyle" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDRitemName" Width="150px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDRitemName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDRquality" runat="server" visible="false">
                                <asp:Label ID="Label17" runat="server" Text="Quality" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" ID="DDRquality" Width="150px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDRquality_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDRdesign" runat="server" visible="false">
                                <asp:Label ID="Label18" runat="server" Text="Design" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" Width="110px" ID="DDRdesign" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td id="TDRcolor" runat="server" visible="false">
                                <asp:Label ID="Label19" runat="server" Text="Color" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" Width="110px" ID="DDRcolor" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td id="TDRShadecolor" runat="server" visible="false">
                                <asp:Label ID="Label20" runat="server" Text="Shadecolor" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" Width="150px" ID="DDRShadecolor" runat="server"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDRShadecolor_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td id="TDRShape" runat="server" visible="false">
                                <asp:Label ID="Label26" runat="server" Text="Shape" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList CssClass="dropdown" Width="110px" ID="DDRshape" runat="server"
                                    AutoPostBack="True" OnSelectedIndexChanged="DDRshape_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDRSize" runat="server" visible="false">
                                <asp:Label ID="Label27" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                <asp:DropDownList CssClass="dropdown" Width="50px" ID="DDRSizetype" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDRSizetype_SelectedIndexChanged">
                                </asp:DropDownList>
                                <br />
                                <asp:DropDownList CssClass="dropdown" Width="110px" ID="DDRsize" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label22" runat="server" Text="Rec. Lot No" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtreclotno" CssClass="textb" runat="server" Width="150px" />
                            </td>
                            <td id="TDrectagno" runat="server" visible="false">
                                <asp:Label ID="Label23" runat="server" Text="Rec. Tag No" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtrectagno" CssClass="textb" runat="server" Width="150px" />
                            </td>
                            <td>
                                <asp:Label ID="Label28" runat="server" Text="Bell Wt" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="TxtBellWeight" CssClass="textb" runat="server" Width="80px" onkeypress="return isNumberKey(event);" />
                            </td>
                            <td>
                                <asp:Label ID="Label24" runat="server" Text="Qty" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtrecqty" CssClass="textb" runat="server" Width="80px" onkeypress="return isNumberKey(event);" />
                            </td>
                            <td>
                                <asp:Label ID="Label25" runat="server" Text="Rate" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtrate" CssClass="textb" runat="server" Width="80px" onkeypress="return isNumberKey(event);"
                                    Enabled="false" />
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
                    </tr>
                </table>
                <table style="width: 100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" runat="server" />
                        </td>
                        <td align="right">
                            <asp:Button ID="btnsave" CssClass="buttonnorm" Text="Save" runat="server" OnClick="btnsave_Click" />
                            <asp:Button ID="btnclose" CssClass="buttonnorm" Text="Close" runat="server" OnClientClick="return CloseForm();" />
                            <asp:Button ID="btnpreview" CssClass="buttonnorm" Text="Preview" runat="server" OnClick="btnpreview_Click" />
                            <asp:Button ID="btnnew" CssClass="buttonnorm" Text="New" runat="server" OnClientClick="return NewForm();" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="gride" runat="server" style="max-height: 500px; overflow: auto">
                <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                    Width="100%" OnRowDataBound="DG_RowDataBound" OnRowEditing="DG_RowEditing" OnRowCancelingEdit="DG_RowCancelingEdit"
                    OnRowDeleting="DG_RowDeleting" OnRowUpdating="DG_RowUpdating">
                    <HeaderStyle CssClass="gvheaders" />
                    <AlternatingRowStyle CssClass="gvalts" />
                    <RowStyle CssClass="gvrow" />
                    <EmptyDataRowStyle CssClass="gvemptytext" />
                    <Columns>
                        <asp:TemplateField HeaderText="Sr No.">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IDescription">
                            <ItemTemplate>
                                <asp:Label ID="lblitemdescription" Text='<%#Bind("Iitemdescription") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="RDescription">
                            <ItemTemplate>
                                <asp:Label ID="lblRitemdescription" Text='<%#Bind("Ritemdescription") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rec Lot No">
                            <ItemTemplate>
                                <asp:Label ID="lblRlotno" Text='<%#Bind("Reclotno") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rec Tag No">
                            <ItemTemplate>
                                <asp:Label ID="lblRTagno" Text='<%#Bind("Rectagno") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qty">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtqty" Width="70px" Text='<%#Bind("Issueqty") %>' runat="server"
                                    BackColor="Yellow" onkeypress="return isNumberKey(event);" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblqty" Text='<%#Bind("issueqty") %>' runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rate">
                            <ItemTemplate>
                                <asp:Label ID="lblrate" Text='<%#Bind("rate") %>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtRate" Width="70px" Text='<%#Bind("rate") %>' runat="server" BackColor="Yellow"
                                    onkeypress="return isNumberKey(event);" />
                            </EditItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblid" Text='<%#Bind("Id") %>' runat="server" />
                                <asp:Label ID="lbldetailid" Text='<%#Bind("Detailid") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkdel" runat="server" CausesValidation="False" CommandName="Delete"
                                    Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowEditButton="True" />
                    </Columns>
                </asp:GridView>
            </div>
            <asp:HiddenField ID="hnid" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
