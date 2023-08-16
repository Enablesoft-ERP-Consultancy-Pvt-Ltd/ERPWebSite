<%@ Page Title="INDENT RECEIVE" Language="C#" AutoEventWireup="true" CodeFile="IndentRawRecieveNew.aspx.cs"
    MasterPageFile="~/ERPmaster.master" Inherits="Masters_RawMaterial_IndentRawRecieveNew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "IndentRawRecieve.aspx";
        }
        function Preview() {
            window.open('../../ReportViewer.aspx');
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }        
        function isNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }
        function validateDate() {

            var varcompanyNo = document.getElementById('CPH_Form_hncomp').value
            //6 For ArtIndia
            if (varcompanyNo == "6") {

                var dt = new Date();
                dt.setDate(dt.getDate() - 3);
                var recDate = new Date(document.getElementById('CPH_Form_txtdate').value);

                if (recDate < dt) {
                    alert('Date Can not be less than  ' + dt.format('dd-MMM-yyyy'));
                    date = new Date();
                    var month = new Array();
                    month[0] = "Jan";
                    month[1] = "Feb";
                    month[2] = "Mar";
                    month[3] = "Apr";
                    month[4] = "May";
                    month[5] = "Jun";
                    month[6] = "Jul";
                    month[7] = "Aug";
                    month[8] = "Sep";
                    month[9] = "Oct";
                    month[10] = "Nov";
                    month[11] = "Dec";
                    var month1 = month[date.getMonth()];
                    var day = date.getDate();
                    var year = date.getFullYear();
                    document.getElementById('CPH_Form_txtdate').value = day + '-' + month1 + '-' + year;
                }
            }
        }
    </script>
     <script type="text/javascript">
         function Jscriptvalidate() {           
                     var Message = "";
                     var selectedindex = $("#<%=ddCompName.ClientID %>").attr('selectedIndex');
                     if (selectedindex < 0) {
                         Message = Message + "Please select Company Name!!\n";
                     }
                     selectedindex = $("#<%=ddProcessName.ClientID %>").attr('selectedIndex');
                     if (selectedindex <= 0) {
                         Message = Message + "Please Select Process. !!\n";
                     }
                     selectedindex = $("#<%=ddempname.ClientID %>").attr('selectedIndex');
                     if (selectedindex <= 0) {
                         Message = Message + "Please Select Vendor. !!\n";
                     }

                     selectedindex = $("#<%=ddindent.ClientID %>").attr('selectedIndex');
                     if (selectedindex <= 0) {
                         Message = Message + "Please Select Vendor. !!\n";
                     }
                     if ($("#<%=TdPartyChallanNo.ClientID %>").is(':visible')) {
                         var selectedindex = $("#<%=ddChallanNo.ClientID %>").attr('selectedIndex');
                         if (selectedindex <= 0) {
                             Message = Message + "Please select Challan No. !!\n";
                         }
                     }
                     if ($("#<%=TDRECSHADE.ClientID %>").is(':visible')) {
                         var selectedindex = $("#<%=DDRECSHADE.ClientID %>").attr('selectedIndex');
                         if (selectedindex <= 0) {
                             Message = Message + "Please select Shade Color. !!\n";
                         }
                     }
                     var txtrec = document.getElementById('<%=txtrec.ClientID %>');
                     if (txtrec.value == "" || txtrec.value == "0") {
                         Message = Message + "Rec Quantity Cann't be blank or Zero.. !!\n";
                     }
                     var txtrec = document.getElementById('<%=txtrec.ClientID %>');
                     if (txtrec.value == "" || txtrec.value == "0") {
                         Message = Message + "Rec Quantity Cann't be blank or Zero.. !!\n";
                     }
                     selectedindex = $("#<%=ddgodown.ClientID %>").attr('selectedIndex');
                     if (selectedindex <= 0) {
                         Message = Message + "Please Select Godown Name. !!\n";
                     }
                     if ($("#<%=TDBinno.ClientID %>").is(':visible')) {
                         var selectedindex = $("#<%=ddbinno.ClientID %>").attr('selectedIndex');
                         if (selectedindex <= 0) {
                             Message = Message + "Please select Bin No. !!\n";
                         }
                     }
                     selectedindex = $("#<%=ddlunit.ClientID %>").attr('selectedIndex');
                     if (selectedindex <= 0) {
                         Message = Message + "Please Select Unit Name. !!\n";
                     }
                     if ($("#<%=TDLotNo.ClientID %>").is(':visible')) {
                         var selectedindex = $("#<%=DDLotNo.ClientID %>").attr('selectedIndex');
                         if (selectedindex <= 0) {
                             Message = Message + "Please Select Lot No. !!\n";
                         }
                     }
                     if ($("#<%=TDTagNo.ClientID %>").is(':visible')) {
                         var selectedindex = $("#<%=DDTagNo.ClientID %>").attr('selectedIndex');
                         if (selectedindex <= 0) {
                             Message = Message + "Please Select Tag No. !!\n";
                         }
                     }
                     if ($("#<%=TDTxtMoisture.ClientID %>").is(':visible')) {
                         var TxtMoisture = document.getElementById('<%=TxtMoisture.ClientID %>');
                         if (TxtMoisture.value == "" || TxtMoisture.value == "0") {
                             Message = Message + "Moisture Cann't be blank or Zero..  !!\n";
                         }
                     }
                     if ($("#<%=TDIssueQtyOnMachine.ClientID %>").is(':visible')) {
                         var txtIssueQtyOnMachine = document.getElementById('<%=txtIssueQtyOnMachine.ClientID %>');
                         if (txtIssueQtyOnMachine.value == "" || txtIssueQtyOnMachine.value == "0") {
                             Message = Message + "IssueQty OnMachine Cann't be blank or Zero..  !!\n";
                         }
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

    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div style="height: 750px">
                <table>
                </table>
                <fieldset>
                    <legend>
                        <asp:Label Text="Master Details" CssClass="labelbold" ForeColor="Red" runat="server" />
                    </legend>
                    <table>
                        <tr>
                            <td id="TDedit" runat="server" visible="false">
                                <asp:CheckBox ID="ChkEdit" runat="server" Text=" Edit Order" CssClass="checkboxbold"
                                    AutoPostBack="True" OnCheckedChanged="ChkEdit_CheckedChanged" />
                            </td>
                            <td id="TDcomplete" runat="server" visible="false">
                                <asp:CheckBox ID="chkcomplete" runat="server" Text=" For Complete Indent No." CssClass="checkboxbold" />
                            </td>
                            <td>
                                <asp:CheckBox ID="checkforsampleorder" runat="server" Visible="false" Text=" Check for Sample Order"
                                    CssClass="checkboxbold" AutoPostBack="True" />
                            </td>
                        </tr>
                        <tr id="Tr1" runat="server">
                            <td class="tdstyle">
                                <asp:Label ID="lbl" Text="Production" runat="server" CssClass="labelbold" />
                                &nbsp;
                                <asp:Label ID="Label3" Text=" No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtidnt" runat="server" OnTextChanged="txtidnt_TextChanged" AutoPostBack="True"
                                    CssClass="textb" Width="100px"></asp:TextBox>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label4" Text="Company Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="ddCompName" runat="server" Width="150px"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddCompName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label33" runat="server" Text="BranchName" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDBranchName" Width="150" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label5" Text="Process Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="ddProcessName" runat="server" Width="150px"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddProcessName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label6" Text="Emp/Vendor name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="ddempname" runat="server" Width="150px"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddempname_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td runat="server" id="Tdlegalvendor" visible="false">
                                <asp:Label ID="Label46" runat="server" Text="Legal Vendor" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDlegalvendor" Width="200px" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label7" Text="Production No." runat="server" CssClass="labelbold" /><br />
                                <asp:DropDownList CssClass="dropdown" ID="ddindent" Width="120px" runat="server"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddindent_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle" id="tdchal" runat="server">
                                <asp:Label ID="Label9" Text="Challan No." runat="server" CssClass="labelbold" /><br />
                                <asp:DropDownList CssClass="dropdown" ID="ddChallanNo" Width="120px" runat="server"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddChallanNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                             <td id="TDItemDescription" runat="server">
                                <asp:Label ID="Label32" runat="server" Text="Order Description" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDitemdescription" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="370px" OnSelectedIndexChanged="DDitemdescription_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TdPartyChallanNo" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="Label10" Text=" Party Challan No" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="ddPartyChallanNo" Width="120px" runat="server"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddPartyChallanNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr id="Tr2" runat="server">
                            <td class="tdstyle">
                                <asp:Label ID="Label11" Text="Rec Date" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtdate" runat="server" Width="90px" CssClass="textb" onchange="return validateDate();"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                    TargetControlID="txtdate">
                                </asp:CalendarExtender>
                            </td>
                            <td id="TDNoOFHank" runat="server" class="tdstyle">
                                <asp:Label ID="Label12" Text=" No Of Hank" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="TxtNoOFHank" runat="server" CssClass="textb" Width="63px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                            <td class="tdstyle" id="TDGateInNo" runat="server" visible="true">
                                <asp:Label ID="Label13" Text="Gate In No" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="TxtGateInNo" Width="100px" runat="server" CssClass="textb" Visible="true"
                                    onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                            <td class="tdstyle" colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label42" Text="Bill No" runat="server" CssClass="labelbold" />
                                            <br />
                                            <asp:TextBox ID="txtBillNo" Width="100px" runat="server" CssClass="textb" Visible="true"
                                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                        <td class="tdstyle" id="tdchallan" runat="server">
                                            <asp:Label ID="Label14" Text="Party Challan No" runat="server" CssClass="labelbold" />
                                            <br />
                                            <asp:TextBox ID="TxtPartyChallanNo" Width="100px" runat="server" BackColor="Beige"
                                                CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" runat="server" id="TdlnkupdatebillNo" visible="false">
                                            <asp:LinkButton ID="lnkupdatebillNo" Text="Update Bill No./Party Challan No." CssClass="lnkbtnClass"
                                                runat="server" OnClick="lnkupdatebillNo_Click" OnClientClick="return confirm('Do you want to update Bill No./Party Challan No.')" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td id="procode" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="Label15" Text=" Product Code" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtcode" Width="150px" runat="server" OnTextChanged="txtcode_TextChanged"
                                    AutoPostBack="True"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                                    Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="txtcode"
                                    UseContextKey="True">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td class="tdstyle" id="chkrec" runat="server" colspan="2">
                                <asp:CheckBox ID="ChkForReceiveIssItem" runat="server" Text="Check For Receive Issue Item"
                                    CssClass="checkboxbold" AutoPostBack="True" OnCheckedChanged="ChkForReceiveIssItem_CheckedChanged" />
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblsrno" runat="server" CssClass="labelbold" ForeColor="Red" Text="Sr No. Generated."></asp:Label>
                            </td>
                            <td class="tdstyle" id="Td1" runat="server" colspan="2">
                                <asp:CheckBox ID="ChkForReceiveAnyColor" runat="server" Text="Check For Receive Any Color"
                                    CssClass="checkboxbold" AutoPostBack="True" Visible="false" OnCheckedChanged="ChkForReceiveAnyColor_CheckedChanged" />
                            </td>
                            <td>
                                <asp:Label ID="lblcheckedby" CssClass="labelbold" Text="Checked By" runat="server" />
                                <br />
                                <asp:TextBox ID="txtcheckedby" CssClass="textb" Width="200px" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="Label39" CssClass="labelbold" Text="Approved By" runat="server" />
                                <br />
                                <asp:TextBox ID="txtapprovedby" CssClass="textb" Width="200px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label Text="Item Details" CssClass="labelbold" ForeColor="Red" runat="server" />
                    </legend>
                    <table>
                        <tr id="Tr3" runat="server">
                            <td class="tdstyle">
                                <asp:Label ID="LblCategory" runat="server" Text="" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="ddCatagory" runat="server" Width="150px"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="LblItemName" runat="server" Text="Label" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="dditemname" runat="server" Width="150px"
                                    OnSelectedIndexChanged="dditemname_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td id="ql" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="LblQuality" runat="server" Text="Label" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="dquality" runat="server" Width="150px"
                                    OnSelectedIndexChanged="dquality_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td id="dsn" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="LblDesign" runat="server" Text="Label" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="dddesign" runat="server" Width="150px"
                                    AutoPostBack="True" OnSelectedIndexChanged="dddesign_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="clr" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="LblColor" runat="server" Text="Label" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="ddcolor" runat="server" Width="150px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddcolor_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="shp" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="LblShape" runat="server" Text="Label" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="ddshape" runat="server" Width="150px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddshape_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="sz" runat="server" visible="false" class="tdstyle">
                                &nbsp;<asp:Label ID="LblSize" runat="server" Text="Label" CssClass="labelbold"></asp:Label>
                                <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Selected="True">Ft</asp:ListItem>
                                    <asp:ListItem Value="1">MTR</asp:ListItem>
                                    <asp:ListItem Value="2">Inch</asp:ListItem>
                                </asp:DropDownList>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="ddsize" runat="server" Width="150px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddsize_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="shd" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="LblColorShade" runat="server" Text="Label" CssClass="labelbold"></asp:Label>
                                &nbsp;<br />
                                <asp:DropDownList CssClass="dropdown" ID="ddlshade" runat="server" Width="150px"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlshade_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDRECSHADE" runat="server" visible="false" class="tdstyle">
                                <asp:Label ID="lblrecshade" runat="server" Text="SHADE_COLOR" CssClass="labelbold"></asp:Label>
                                &nbsp;<br />
                                <asp:DropDownList CssClass="dropdown" ID="DDRECSHADE" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td class="tdstyle" id="TDDyeingMatch" runat="server" visible="false">
                                <asp:Label ID="lblmatch" runat="server" Text="Dyeing Match" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDDyeingMatch" runat="server" CssClass="dropdown" Enabled="false">
                                    <asp:ListItem Value="Cut">Cut</asp:ListItem>
                                    <asp:ListItem Value="Side">Side</asp:ListItem>
                                    <asp:ListItem Value="Cut/Side">Cut/Side</asp:ListItem>
                                    <asp:ListItem Value="N/A">N/A</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle" id="TDDyeing" runat="server" visible="false">
                                <asp:Label ID="Label8" runat="server" Text="Dyeing" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDDyeing" runat="server" CssClass="dropdown" Enabled="false">
                                    <asp:ListItem Value="Boarder">Boarder</asp:ListItem>
                                    <asp:ListItem Value="Ground">Ground</asp:ListItem>
                                    <asp:ListItem Value="Ascent">Ascent</asp:ListItem>
                                    <asp:ListItem Value="Ground/Boarder">Ground/Boarder</asp:ListItem>
                                    <asp:ListItem Value="N/A">N/A</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle" id="TDDyingType" runat="server" visible="false">
                                <asp:Label ID="Label30" runat="server" Text="Dyeing Type" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDDyingType" runat="server" CssClass="dropdown" Enabled="false">
                                    <asp:ListItem Value="Shaded">Shaded</asp:ListItem>
                                    <asp:ListItem Value="Natural">Natural</asp:ListItem>
                                    <asp:ListItem Value="Plain">Plain</asp:ListItem>
                                    <asp:ListItem Value="Gabbeh">Gabbeh</asp:ListItem>
                                    <asp:ListItem Value="Multi Dyeing">Multi Dyeing</asp:ListItem>
                                    <asp:ListItem Value="N/A">N/A</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label16" Text="Order No" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDOrderNo" Width="150px" runat="server"
                                    AutoPostBack="True" OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label17" Text=" Godown Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="ddgodown" runat="server" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="ddgodown_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDBinno" runat="server" visible="false">
                                <asp:Label ID="Label40" Text="Bin No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="ddbinno" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label18" Text="Unit" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="ddlunit" runat="server" Width="100px">
                                </asp:DropDownList>
                            </td>
                            <td id="TDLotNo" runat="server" class="tdstyle">
                                <asp:Label ID="Label19" Text="LotNo." runat="server" CssClass="labelbold" />
                                <br />
                                <%--<asp:TextBox ID="txtlotno" runat="server" Width="150px" CssClass="textb"></asp:TextBox>--%>
                                <asp:DropDownList CssClass="dropdown" ID="DDLotNo" runat="server" Width="150px" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDLotNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr id="TDTagNo" runat="server" class="tdstyle" visible="false">
                                        <td>
                                            <asp:Label ID="Label31" Text="Tag No." runat="server" CssClass="labelbold" />
                                            <asp:CheckBox ID="chkwithouttag" Visible="false" Text="For Without Tag" CssClass="checkboxbold"
                                                ForeColor="Red" runat="server" />
                                            <br />
                                            <asp:DropDownList CssClass="dropdown" ID="DDTagNo" runat="server" Width="130px" AutoPostBack="true"
                                                OnSelectedIndexChanged="DDTagNo_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label41" Text="Tag No." runat="server" CssClass="labelbold" />
                                            <asp:CheckBox ID="chkchangeTagno" Text="Change Tag No." runat="server" AutoPostBack="true"
                                                ForeColor="Red" CssClass="checkboxbold" OnCheckedChanged="chkchangeTagno_CheckedChanged" />
                                            <br />
                                            <asp:TextBox ID="txttagNo" CssClass="textb" runat="server" Width="150px" Enabled="false"
                                                BackColor="LightGray" />
                                        </td>
                                       
                                         <td id="customlotno" visible="false" style="padding-left:5px" runat="server">
                                            <asp:Label ID="Label47" Text="Lot No." runat="server" CssClass="labelbold" />
                                            <asp:CheckBox ID="chkchangeLotno" Text="Change Lot No." runat="server" AutoPostBack="true"
                                                ForeColor="Red" CssClass="checkboxbold" OnCheckedChanged="chkchangeLotno_CheckedChanged" />
                                            <br />
                                            <asp:TextBox ID="txtlotno" CssClass="textb" runat="server" Width="150px" Enabled="false"
                                                BackColor="LightGray" />
                                        </td>
                                        <td id="TDTagRemark" runat="server" visible="false">
                                            <asp:Label ID="Label43" Text="Tag Remark" runat="server" CssClass="labelbold" />
                                            <br />
                                            <asp:TextBox ID="txtTagRemark" runat="server" Width="300px" CssClass="textb"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label Text="..." CssClass="labelbold" ForeColor="Red" runat="server" /></legend>
                    <table>
                        <tr>
                            <td id="TDIssuedQty" runat="server" class="tdstyle">
                                <asp:Label ID="Label20" Text="  Issued Qty" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="TxtIssueQty" runat="server" Width="80px" Enabled="false" BackColor="LightGray"
                                    CssClass="textb"></asp:TextBox>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label21" Text="  Indent Qty" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="TxtIndentQty" runat="server" Width="80px" Enabled="false" BackColor="LightGray"
                                    CssClass="textb"></asp:TextBox>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label22" Text=" PreRec Qty" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtprerec" runat="server" Width="80px" Enabled="false" BackColor="LightGray"
                                    CssClass="textb"></asp:TextBox>
                            </td>
                            <td id="TDtxtpendingQty" runat="server" class="tdstyle">
                                <asp:Label ID="Label23" Text="  P Qty" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtpending" runat="server" Width="80px" Enabled="false" BackColor="LightGray"
                                    CssClass="textb"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label24" Text=" Penalty/Debit Note" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="TxtPenalty" runat="server" Width="100px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="Label25" Text=" Rec Qty" runat="server" CssClass="labelbold" />
                                <asp:CheckBox ID="ChkForReDyeing" Visible="false" Text="Re Dyeing" CssClass="checkboxbold"
                                    ForeColor="Red" runat="server" AutoPostBack="True" OnCheckedChanged="ChkForReDyeing_CheckedChanged" />
                                <br />
                                <asp:TextBox ID="txtrec" runat="server" Width="100px" CssClass="textb" OnTextChanged="txtrec_TextChanged"
                                    onkeypress="return isNumber(event);" AutoPostBack="True" BackColor="Beige"></asp:TextBox>
                            </td>

                            <td runat="server" id="TDIssueQtyOnMachine" visible="false">
                                <asp:Label ID="Label48" Text="IssQty OnMachine" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtIssueQtyOnMachine" runat="server" Width="70px" CssClass="textb" onkeypress="return isNumber(event);"
                                    BackColor="Beige"></asp:TextBox>
                            </td>

                            <td runat="server" id="TDLShort" visible="false">
                                <asp:Label ID="lblLshort" Text=" L-Short(%)" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtLShort" runat="server" Width="70px" CssClass="textb" onkeypress="return isNumber(event);"
                                    BackColor="Beige"></asp:TextBox>
                            </td>
                            <td runat="server" id="TDshrinkage" visible="false">
                                <asp:Label ID="lblshrinkage" Text=" shrinkage(%)" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtshrinkage" runat="server" Width="75px" CssClass="textb" onkeypress="return isNumber(event);"
                                    BackColor="Beige"></asp:TextBox>
                            </td>
                            
                            <td runat="server" id="TDTxtMoisture" visible="false">
                                <asp:Label ID="Label44" Text="Moisture" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="TxtMoisture" runat="server" Width="75px" CssClass="textb" onkeypress="return isNumber(event);"
                                    BackColor="Beige"></asp:TextBox>
                            </td>                            
                            <td>
                                <asp:Label ID="Label45" Text="Bell Wt" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="TxtBellWeight" runat="server" Width="75px" CssClass="textb" onkeypress="return isNumber(event);"
                                    BackColor="Beige"></asp:TextBox>
                            </td>
                            <td id="TDloss" runat="server">
                                <asp:Label ID="Label26" Text="Loss Qty" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="TxtLoss" runat="server" Width="80px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </td>
                            <td id="TDreturnQty" runat="server">
                                <asp:Label ID="Label27" Text="  Ret Qty" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtretrn" runat="server" Width="100px" CssClass="textb" AutoPostBack="True"
                                    Enabled="false" OnTextChanged="txtrec_TextChanged" onkeypress="return isNumber(event);"></asp:TextBox>
                            </td>
                            <td class="tdstyle" id="TDRate" visible="false" runat="server">
                                <asp:Label ID="Label28" Text=" Rate" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="TxtRate" runat="server" Width="75px" CssClass="textb" onkeypress="return isNumber(event);"
                                    Style="background-color: Beige"></asp:TextBox>
                            </td>
                            <td class="tdstyle" id="TdChkWithoutRate" runat="server" colspan="2">
                                <br />
                                <asp:CheckBox ID="ChkForWithoutRate" runat="server" Text="Check For Receive Without Rate"
                                    CssClass="checkboxbold" />
                            </td>
                            <td class="tdstyle">
                                <asp:Label ID="lbreclqty" runat="server" ForeColor="Red" Text="Qty is greater than pending qty"
                                    Visible="False" Width="124px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <table width="100%">
                    <tr>
                        <td style="width: 55%" id="TDtxtremarks" runat="server">
                            <asp:Label ID="Label29" Text=" Remarks" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtremarks" runat="server" Width="100%" Height="80px" CssClass="textb"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td style="width: 45%;" align="center">
                            <asp:CheckBox ID="ChkForGSTReport" runat="server" Text="Check For GST Report" CssClass="checkboxbold"
                                Visible="false" />
                            <asp:Button ID="btnNew" Width="50px" runat="server" Text="New" OnClientClick="return NewForm();"
                                CssClass="buttonnorm" OnClick="btnNew_Click" />
                       <asp:Button ID="btnsave" Width="50px" runat="server" Text="Save" OnClick="btnsave_Click" UseSubmitBehavior="false"
                                  OnClientClick="if (!Jscriptvalidate()) return; this.disabled=true;this.value = 'wait ...';" CssClass="buttonnorm" />

                                   
                        <%--  <asp:Button ID="btnsave" Width="50px" runat="server" Text="Save" OnClick="btnsave_Click"
                                OnClientClick="return Validate();" CssClass="buttonnorm" />--%>
                                
                                                         

                            <asp:Button ID="btnpreview" runat="server" Text="Preview" CssClass="buttonnorm preview_width"
                                OnClick="btnpreview_Click" />
                            <asp:Button ID="btnqcchkpreview" runat="server" Width="70px" CssClass="buttonnorm"
                                Text="QCReport" OnClick="btnqcchkpreview_Click" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" Width="55px" 
                                CssClass="buttonnorm" OnClick="btnclose_Click" OnClientClick="return CloseForm();"/>
                            <asp:Button ID="BtnUpdateStatus" runat="server" Text="Update Status" Visible="false"
                                Width="90px" CssClass="buttonnorm" OnClick="BtnUpdateStatus_Click" />
                                <asp:Button ID="BtnForComplete" Width="90px" runat="server" Text="For Complete" OnClick="BtnForComplete_Click"
                                CssClass="buttonnorm" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="LblErrorMessage" runat="server" ForeColor="Red" Text="" Visible="false"
                                CssClass="labelbold" Font-Size="Small"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td style="width: 60%">
                            <div style="width: 100%; max-height: 300px; overflow: auto">
                                <asp:GridView ID="gvdetail" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvdetail_RowDataBound"
                                    DataKeyNames="prtid" OnRowDeleting="gvdetail_RowDeleting" CssClass="grid-views"
                                    OnRowEditing="gvdetail_RowEditing" OnRowCancelingEdit="gvdetail_RowCancelingEdit"
                                    OnRowUpdating="gvdetail_RowUpdating" Width="100%">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Category Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgridcategiryname" Text='<%#Bind("Category_Name") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item">
                                            <ItemTemplate>
                                                <asp:Label ID="Label32" Text='<%#Bind("Item_Name") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:Label ID="Label33" Text='<%#Bind("Description") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Lot No">
                                            <ItemTemplate>
                                                <asp:Label ID="Label34" Text='<%#Bind("LotNo") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tag/UCN No">
                                            <ItemTemplate>
                                                <asp:Label ID="Label35" Text='<%#Bind("TagNo") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Godown Name">
                                            <ItemTemplate>
                                                <asp:Label ID="Label36" Text='<%#Bind("GodownName") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="Label37" Text='<%#Bind("RecQuantity") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtRecQty" Text='<%#Bind("RecQuantity") %>' onkeypress="return isNumber(event);"
                                                    Width="80px" runat="server" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="LossQty">
                                            <ItemTemplate>
                                                <asp:Label ID="Label38" Text='<%#Bind("Lossqty") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtgloss" Text='<%#Bind("Lossqty") %>' onkeypress="return isNumber(event);"
                                                    Width="80px" runat="server" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblrate" Text='<%#Bind("Rate") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtrate" Text='<%#Bind("Rate") %>' Width="80px" runat="server" onkeypress="return isNumber(event);" />
                                            </EditItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tag Remark" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTagRemark" Text='<%#Bind("TagRemark") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="Del" OnClientClick="return confirm('Do You Want To Deleted?')"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="30px" />
                                            <ItemStyle HorizontalAlign="Right" Width="30px" />
                                        </asp:TemplateField>
                                        <asp:CommandField DeleteText="" ShowEditButton="True" />
                                        <asp:TemplateField HeaderText="" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFinishedId" Text='<%#Bind("FinishedId") %>' runat="server" />
                                                <asp:Label ID="lblGodownId" Text='<%#Bind("GodownId") %>' runat="server" />
                                                <asp:Label ID="lblCompanyID" Text='<%#Bind("CompanyID") %>' runat="server" />
                                                <asp:Label ID="lblLotNo" Text='<%#Bind("LotNo") %>' runat="server" />
                                                <asp:Label ID="lblTagNo" Text='<%#Bind("TagNo") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                        <td style="width: 10%" id="qulitychk" visible="false" runat="server" valign="top">
                            <asp:GridView ID="grdqualitychk" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                                CssClass="grid-views" Width="100%">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="CheckBox1" runat="server" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBox1" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SrNo">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("SrNo") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("SrNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ParaName">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ParaName") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("ParaName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                        <td style="width: 30%" runat="server" id="tdorder" visible="false" valign="top">
                            <div style="max-height: 300px; width: 100%; overflow: auto;">
                                <asp:GridView ID="dGORDER" runat="server" AutoGenerateColumns="False" CssClass="grid-views"
                                    OnSelectedIndexChanged="dGORDER_SelectedIndexChanged" SelectedRowStyle-BackColor="Highlight"
                                    OnRowDataBound="dGORDER_RowDataBound" AutoGenerateSelectButton="true">
                                    <HeaderStyle CssClass="gvheaders" Width="100%" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="ITEMDESCRIPTION">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="LotNo">
                                            <ItemTemplate>
                                                <asp:Label ID="lbllotno" runat="server" Text='<%# Bind("Lotno") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TagNo">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltagno" runat="server" Text='<%# Bind("Tagno") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="QTY">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("QTY") %>'></asp:Label>
                                                <asp:Label ID="lblcategoryid" runat="server" Text='<%# Bind("CATEGORY_ID") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblitem_id" runat="server" Text='<%# Bind("ITEM_ID") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblQualityid" runat="server" Text='<%# Bind("QualityId") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblColorid" runat="server" Text='<%# Bind("ColorId") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lbldesignid" runat="server" Text='<%# Bind("designId") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblshapeid" runat="server" Text='<%# Bind("ShapeId") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblshadecolorid" runat="server" Text='<%# Bind("ShadecolorId") %>'
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="lblsizeid" runat="server" Text='<%# Bind("SizeId") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblqty" runat="server" Text='<%# Bind("Qty") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblfinished" runat="server" Text='<%# Bind("finishedid") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblindent" runat="server" Text='<%# Bind("indentid") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblunitid" runat="server" Text='<%# Bind("unitid") %>' Visible="false"></asp:Label>
                                                <%--<asp:Label ID="lblorderdet" runat="server" Text='<%# Bind("orderdetailid") %>' Visible="false"></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BAl.Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblrecqty" runat="server" Text='<%# getgiven(DataBinder.Eval(Container.DataItem, "finishedid").ToString(),DataBinder.Eval(Container.DataItem, "indentid").ToString(),DataBinder.Eval(Container.DataItem, "QTY").ToString(),DataBinder.Eval(Container.DataItem, "Lotno").ToString(),DataBinder.Eval(Container.DataItem, "Tagno").ToString()) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hncomp" runat="server" />
                            <asp:HiddenField ID="hnorderwiseflag" runat="server" />
                            <asp:HiddenField ID="hnfinishedid" runat="server" />
                        </td>
                    </tr>
                </table>
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
                            <asp:TextBox ID="txtpwd" runat="server" TextMode="Password" Height="30px" Width="174px"
                                OnTextChanged="txtpwd_TextChanged" AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="right">
                            <input type="button" value="Cancel" class="btnPwd" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:HiddenField ID="HPRMID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
