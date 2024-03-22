<%@ Page Title="Dyed yarn Inspection" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmDyedInspectionKaysons.aspx.cs" Inherits="Masters_Inspection_FrmDyedInspectionKaysons" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function ClickNew() {
            window.location.href = "FrmDyedInspectionKaysons.aspx";
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }
        function getNum(val) {
            if (isNaN(val)) {
                return 0;
            }
            return val;
        }
       
        function EmpSelected(source, eventArgs) {
            document.getElementById('<%=txtgetvalue.ClientID %>').value = eventArgs.get_value();
        }

    </script>
    <asp:UpdatePanel runat="server" ID="upd1">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkedit" Text="FOR EDIT" CssClass="checkboxbold" AutoPostBack="True"
                            runat="server" OnCheckedChanged="chkedit_CheckedChanged" />
                    </td>
                    <td>
                    </td>
                    <td id="TDSupplierSearch" runat="server" visible="false">
                        <asp:TextBox ID="txtsuppliersearch" Placeholder="Type Supplier here to search Doc No."
                            runat="server" Width="235px" CssClass="textb" />
                    </td>
                    <td colspan="2" id="TDcountsearch" runat="server" visible="false">
                        <asp:TextBox ID="txtcountsearch" Placeholder="Type Count here to search Doc No."
                            runat="server" Width="235px" CssClass="textb" />
                    </td>
                    <td colspan="2" id="Tdlotno" runat="server" visible="false">
                        <asp:TextBox ID="txtsearchlotno" Placeholder="Type Int Lot No. here to search Doc No."
                            runat="server" Width="200px" CssClass="textb" />
                        <asp:Button ID="btnsearch" runat="server" Text="Button" OnClick="btnsearch_Click"
                            Style="display: none;" />
                    </td>
                    <td colspan="2" id="TdVenLotNo" runat="server" visible="false">
                        <asp:TextBox ID="TxtSearchVenLotNo" Placeholder="Type Ven Lot No. here to search Doc No."
                            runat="server" Width="200px" CssClass="textb" />
                        <asp:Button ID="BtnSearchVenLotNo" runat="server" Text="Button" OnClick="BtnSearchVenLotNo_Click"
                            Style="display: none;" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblcompany" Text="Company Name" CssClass="labelbold" runat="server" />
                    </td>
                    <td>
                        <asp:DropDownList ID="DDcompanyName" CssClass="dropdown" Width="200px" runat="server"
                            OnSelectedIndexChanged="DDcompanyName_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label20" Text="Branch Name" CssClass="labelbold" runat="server" />
                    </td>
                    <td>
                        <asp:DropDownList ID="DDBranchName" CssClass="dropdown" Width="200px" runat="server"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td id="TDDocno" runat="server" visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label38" Text="Doc No." CssClass="labelbold" runat="server" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDDocNo" CssClass="dropdown" Width="200px" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="DDDocNo_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <asp:Label ID="Label33" Text="System Gen. Doc  No." CssClass="labelbold" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtdocno" CssClass="textb" runat="server" Enabled="false" BackColor="LightGray" />
                    </td>
                </tr>
            </table>
            <table border="1" cellspacing="2" style="width: 100%">
                <tr>
                    <td style="width: 10%; border-style: dotted">
                        <asp:Label Text="Supplier Name" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="width: 40%; border-style: dotted">
                        <asp:TextBox ID="txtgetvalue" runat="server" Style="display: none"></asp:TextBox>
                        <asp:TextBox ID="txtsuppliername" CssClass="textb" Width="95%" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter SupplierName"
                            ControlToValidate="txtsuppliername" Display="Dynamic" ValidationGroup="s" ForeColor="Red">*</asp:RequiredFieldValidator>
                        <asp:AutoCompleteExtender ID="txtsuppliername_AutoCompleteExtender" runat="server"
                            BehaviorID="SrchAutoComplete1" CompletionInterval="20" Enabled="True" ServiceMethod="GetPurchaseVendor"
                            CompletionSetCount="20" OnClientItemSelected="EmpSelected" ServicePath="~/Autocomplete.asmx"
                            TargetControlID="txtsuppliername" UseContextKey="True" ContextKey="0" MinimumPrefixLength="2"
                            DelimiterCharacters="">
                        </asp:AutoCompleteExtender>
                    </td>
                     <td style="width: 8%;">
                                    <asp:Label ID="Label12" Text="Receive Qty" runat="server" CssClass="labelbold" />
                                </td>
                                <td style="width: 19%;">
                                    <asp:TextBox ID="txtReceiveQty" CssClass="textb" Width="95%" runat="server" />
                                </td>
                    <td style="width: 5%; border-style: dotted">
                        <asp:Label ID="Label1" Text="Date :" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="width: 10%; border-style: dotted">
                        <asp:TextBox ID="txtdate" CssClass="textb" Width="95%" runat="server" />
                        <asp:CalendarExtender ID="caldate" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtdate">
                        </asp:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="width: 100%; border-style: dotted">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 12%;">
                                    <asp:Label ID="Label2" Text="Challan No. & Date" runat="server" CssClass="labelbold" />
                                </td>
                                <td style="width: 15%;">
                                    <asp:TextBox ID="txtchallannodate" CssClass="textb" Width="95%" runat="server" />
                                </td>
                                <td style="width: 3%;">
                                    <asp:Label ID="Label9" Text="Color" runat="server" CssClass="labelbold" />
                                </td>
                                <td style="width: 10%;">
                                    <asp:TextBox ID="txtColor" CssClass="textb" Width="95%" runat="server" />
                                </td>
                                <td style="width: 7%;">
                                    <asp:Label ID="Label25" Text="Inwards No" runat="server" CssClass="labelbold" />
                                </td>
                                <td style="width: 10%;">
                                    <asp:TextBox ID="txtInwardsNo" CssClass="textb" Width="95%" runat="server" />
                                </td>
                                <td style="width: 8%;">
                                    <asp:Label ID="Label10" Text="Total OK QTY" runat="server" CssClass="labelbold" />
                                </td>
                                <td style="width: 12%;">
                                    <asp:TextBox ID="txtTotalOkQty" CssClass="textb" Width="95%" runat="server" />
                                </td>
                                <td style="width: 12%;">
                                    <asp:Label ID="Label11" Text="Total Not Ok QTY" runat="server" CssClass="labelbold" />
                                </td>
                                <td style="width: 15%;">
                                    <asp:TextBox ID="txtTotalNotOkQty" CssClass="textb" Width="95%" runat="server" />
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="width: 100%; border-style: dotted">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 6%">
                                    <asp:Label ID="Label3" Text="Yarn Type" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="txtyarntype" CssClass="textb" Width="95%" runat="server" />
                                </td>
                                <td style="width: 6%">
                                    <asp:Label ID="Label6" Text="Total Qty" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="txttotalQty" CssClass="textb" Width="95%" runat="server"  />
                                </td>
                                <td style="width: 6%">
                                    <asp:Label ID="Label4" Text="Count" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="txtcount" CssClass="textb" Width="95%" runat="server" />
                                </td>
                                <td style="width: 6%">
                                    <asp:Label ID="Label7" Text="Sample Size" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="txtsamplesize" CssClass="textb" Width="95%" runat="server" onkeypress="return isNumberKey(event);" />
                                </td>
                                <td style="width: 6%">
                                    <asp:Label ID="Label5" Text="Int Lot No." runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="txtlotno" CssClass="textb" Width="95%" runat="server" />
                                </td>
                                <td style="width: 6%">
                                    <asp:Label ID="Label18" Text="Tag No." runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="txtTagNo" CssClass="textb" Width="95%" runat="server" />
                                </td>
                                <td style="width: 6%">
                                    <asp:Label ID="Label8" Text="No. of Hank" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="txtnoofhank" CssClass="textb" Width="95%" runat="server" onkeypress="return isNumberKey(event);" />
                                </td>
                                <td style="width: 6%">
                                    <asp:Label ID="Label30" Text="Cone" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="txtCone" CssClass="textb" Width="95%" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table id="tblCheckpointDetails" border="1" style="width: 100%;" cellpadding="0">
                   <tr>
                    <td style="padding: 1% 1% 1% 1%; text-align: center">
                        <asp:Label ID="Label17" Text="Sr No" runat="server" CssClass="labelbold" />
                    </td><td >      
                        <asp:Label ID="Label19" Text="Check Point" runat="server" CssClass="labelbold" />
                    </tdl><td>       
                        <asp:Label ID="Label21" Text="Specification" runat="server" CssClass="labelbold" />
                    </td><td>
                        <asp:Label ID="Label22" Text="Finding" runat="server" CssClass="labelbold" />
                    </td><td style="display:none">
                        <asp:Label ID="Label23" Text="2" runat="server" CssClass="labelbold" />
                    </td><td style="display:none">
                        <asp:Label ID="Label24" Text="3" runat="server" CssClass="labelbold" />
                    </td><td style="display:none">
                        <asp:Label ID="Label27" Text="4" runat="server" CssClass="labelbold" />
                    </td><td style="display:none">
                        <asp:Label ID="Label29" Text="5" runat="server" CssClass="labelbold" />
                    </td><td style="text-align:center" >
                        <asp:Label ID="Label31" Text="Remark" runat="server" CssClass="labelbold" />
                    </td></tr>
                    <tr id="Tr1" runat="server">
                    <td style="width: 10%; padding: 1% 1% 1% 1%; text-align: center">
                        <asp:Label ID="lblSrNo1" Text="1" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:Label ID="lblCheckPoint1" Text="Presense Of Ref. Sample" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtSpecification1" CssClass="textb" Width="85%" ReadOnly="true" Text="As Per Approved Sample" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txtVal1_1" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal1_2" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal1_3" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal1_4" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal1_5" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtRemark1" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    </tr>
                    <tr id="Tr2" runat="server">
                    <td style="width: 10%; padding: 1% 1% 1% 1%; text-align: center">
                        <asp:Label ID="lblSrNo2" Text="2" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:Label ID="lblCheckPoint2" Text="Moisture % Age" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtSpecification2" CssClass="textb" Width="85%" runat="server" ReadOnly="true" Text="<12%" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txtVal2_1" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal2_2" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal2_3" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal2_4" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal2_5" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtRemark2" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    </tr>
                    <tr id="Tr3" runat="server">
                    <td style="width: 10%; padding: 1% 1% 1% 1%; text-align: center">
                        <asp:Label ID="lblSrNo3" Text="3" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:Label ID="lblCheckPoint3" Text="Shade Variation" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtSpecification3" CssClass="textb" Width="85%" runat="server" ReadOnly="true" Text="Within Shade Band" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txtVal3_1" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal3_2" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal3_3" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal3_4" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal3_5" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtRemark3" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    </tr>
                    <tr id="Tr4" runat="server">
                    <td style="width: 10%; padding: 1% 1% 1% 1%; text-align: center">
                        <asp:Label ID="lblSrNo4" Text="4" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:Label ID="lblCheckPoint4" Text="Color Fastness to washing" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtSpecification4" CssClass="textb" Width="85%" runat="server" ReadOnly="true" Text="As per Spec. Sheet" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txtVal4_1" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal4_2" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal4_3" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal4_4" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal4_5" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtRemark4" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    </tr>
                    <tr id="Tr5" runat="server">
                    <td style="width: 10%; padding: 1% 1% 1% 1%; text-align: center">
                        <asp:Label ID="lblSrNo5" Text="5" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:Label ID="lblCheckPoint5" Text="Color Fastness to Rubbing (WET)" runat="server" CssClass="labelbold"  />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtSpecification5" CssClass="textb" Width="85%" runat="server" ReadOnly="true" Text="As per Spec. Sheet" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txtVal5_1" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal5_2" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal5_3" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal5_4" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal5_5" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtRemark5" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    </tr>
                    <tr id="Tr6" runat="server">
                    <td style="width: 10%; padding: 1% 1% 1% 1%; text-align: center">
                        <asp:Label ID="lblSrNo6" Text="6" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:Label ID="lblCheckPoint6" Text="Color Fastness to Rubbing (DRY)" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtSpecification6" CssClass="textb" Width="85%" runat="server" ReadOnly="true" Text="As per Spec. Sheet" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txtVal6_1" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal6_2" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal6_3" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal6_4" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal6_5" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtRemark6" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    </tr>
                    <tr id="Tr7" runat="server">
                    <td style="width: 10%; padding: 1% 1% 1% 1%; text-align: center">
                        <asp:Label ID="lblSrNo7" Text="7" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:Label ID="lblCheckPoint7" Text="Result Of PH" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtSpecification7" CssClass="textb" Width="85%" runat="server" ReadOnly="true" Text="As per Spec. Sheet" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txtVal7_1" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal7_2" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal7_3" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal7_4" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal7_5" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtRemark7" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    </tr>
                    <tr id="Tr8" runat="server">
                    <td style="width: 10%; padding: 1% 1% 1% 1%; text-align: center">
                        <asp:Label ID="lblSrNo8" Text="8" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:Label ID="lblCheckPoint8" Text="Transport Condition" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtSpecification8" CssClass="textb" Width="85%" runat="server" ReadOnly="true" Text="OK"   />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txtVal8_1" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal8_2" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal8_3" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal8_4" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal8_5" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtRemark8" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    </tr>
                    <tr id="Tr9" runat="server">
                    <td style="width: 10%; padding: 1% 1% 1% 1%; text-align: center">
                        <asp:Label ID="lblSrNo9" Text="9" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:Label ID="lblCheckPoint9" Text="Other" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtSpecification9" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txtVal9_1" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal9_2" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal9_3" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal9_4" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%;display:none">
                        <asp:TextBox ID="txtVal9_5" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtRemark9" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    </tr>
               </table>
                    
            <table style="width: 100%">
                <tr>
                    <td style="width: 5%">
                        <asp:Label Text="Comments" CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 35%">
                        <asp:TextBox ID="txtcomments" runat="server" CssClass="textb" Width="95%" TextMode="MultiLine"
                            Height="30px" />
                    </td>
                    <td style="width: 10%">
                        <asp:Label ID="Label26" Text="Goods are put in 'Accepted' Area?" CssClass="labelbold"
                            runat="server" />
                    </td>
                    <td style="width: 15%;">
                        <asp:DropDownList ID="DDAcceptedArea" CssClass="dropdown" runat="server" Width="80%">
                            <asp:ListItem Text="Yes" />
                            <asp:ListItem Text="No" />
                            <asp:ListItem Text="NA" />
                        </asp:DropDownList>
                    </td>
                    <td style="width: 10%">
                        <asp:Label ID="Label34" Text="Goods are put in 'Rejected' Area?" CssClass="labelbold"
                            runat="server" />
                    </td>
                    <td style="width: 15%;">
                        <asp:DropDownList ID="DDRejectedArea" CssClass="dropdown" runat="server" Width="80%">
                            <asp:ListItem Text="Yes" />
                            <asp:ListItem Text="No" />
                            <asp:ListItem Text="NA" />
                        </asp:DropDownList>
                    </td>
                    <td style="width: 10%" valign="top">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Label Text="Lot Result" runat="server" CssClass="labelbold" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddresult" CssClass="dropdown" runat="server" Width="100%">
                                        <asp:ListItem Text="Pass" />
                                        <asp:ListItem Text="Fail" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label28" Text="Yarn Photo" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:Image ID="lblphotoimage" runat="server" Height="100px" Width="170px" />
                                </td>
                                <td>
                                    <asp:FileUpload ID="fileuploadphoto" ViewStateMode="Enabled" runat="server" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Only .gif,.jpeg or.jpg files are allowed!"
                                        ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|BMP|bmp)$" ControlToValidate="fileuploadphoto"></asp:RegularExpressionValidator></td></tr></table></td><td style="text-align: center">
                        <asp:Button Text="Save" ID="btnsave" CssClass="buttonnorm" runat="server" OnClick="btnsave_Click" />
                        <asp:Button Text="Preview" ID="btnpreview" CssClass="buttonnorm" runat="server" OnClick="btnpreview_Click" />
                        <asp:Button ID="btndelete" Text="Delete" CssClass="buttonnorm" runat="server" OnClientClick="return confirm('Do you want to delete this Doc No.?')"
                            OnClick="btndelete_Click" />
                        <asp:Button Text="Close" ID="btnclose" CssClass="buttonnorm" runat="server" OnClientClick="return CloseForm();" />
                        <asp:Button Text="New" ID="btnnew" CssClass="buttonnorm" runat="server" OnClientClick="return ClickNew();" />
                        <asp:Button Text="Approve" ID="btnApprove" CssClass="buttonnorm" runat="server" Visible="false"
                            OnClick="btnApprove_Click" OnClientClick="return confirm('Do you want to approve Doc No. ?')" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblmsg" CssClass="labelbold" runat="server" ForeColor="Red" Font-Size="Small" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hndocid" Value="0" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
