<%@ Page Title="Raw yarn Inspection" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmyarninspection.aspx.cs" Inherits="Masters_Inspection_frmyarninspection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function ClickNew() {
            window.location.href = "frmyarninspection.aspx";
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
        function keypress(Srno, id) {

            var one = getNum(parseFloat((document.getElementById("CPH_Form_txt" + Srno + "_" + id)).value));
            var two = getNum(parseFloat((document.getElementById("CPH_Form_txt" + Srno + "_" + (id + 1))).value));
            var three = getNum(parseFloat((document.getElementById("CPH_Form_txt" + Srno + "_" + (id + 2))).value));
            var four = getNum(parseFloat((document.getElementById("CPH_Form_txt" + Srno + "_" + (id + 3))).value));
            var five = getNum(parseFloat((document.getElementById("CPH_Form_txt" + Srno + "_" + (id + 4))).value));
            var avg = (one + two + three + four + five) / 5;
            avg = avg == "Infinity" ? 0 : avg;

            document.getElementById("CPH_Form_txtavgvalue_" + Srno + "").value = (getNum(parseFloat(avg)).toFixed(2));

        }
        function keypresspet_other(Srno, id, pet_other) {

            debugger;
            var one = getNum(parseFloat((document.getElementById("CPH_Form_txt" + pet_other + "" + Srno + "_" + id)).value));
            var two = getNum(parseFloat((document.getElementById("CPH_Form_txt" + pet_other + "" + Srno + "_" + (id + 1))).value));
            var three = getNum(parseFloat((document.getElementById("CPH_Form_txt" + pet_other + "" + Srno + "_" + (id + 2))).value));
            var four = getNum(parseFloat((document.getElementById("CPH_Form_txt" + pet_other + "" + Srno + "_" + (id + 3))).value));
            var five = getNum(parseFloat((document.getElementById("CPH_Form_txt" + pet_other + "" + Srno + "_" + (id + 4))).value));
            var avg = (one + two + three + four + five) / 5;
            avg = avg == "Infinity" ? 0 : avg;

            document.getElementById("CPH_Form_txtavgvalue" + pet_other + "_" + Srno + "").value = (getNum(parseFloat(avg)).toFixed(2));

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
                        <asp:Label Text="Supplier Name" runat="server" CssClass="labelbold"  />
                    </td>
                    <td style="width: 50%; border-style: dotted">
                        <asp:TextBox ID="txtsuppliername" CssClass="textb" Width="95%" runat="server" />                       
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
                    <td style="width: 15%; border-style: dotted">
                        <asp:Label ID="Label2" Text="Challan No. & Date" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="border-style: dotted">
                        <asp:TextBox ID="txtchallannodate" CssClass="textb" Width="95%" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="width: 100%; border-style: dotted">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 14%">
                                    <asp:Label ID="Label3" Text="Yarn Type" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="txtyarntype" CssClass="textb" Width="95%" runat="server" />
                                </td>
                                <td style="width: 14%">
                                    <asp:Label ID="Label6" Text="Total Bale" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="txttotalbale" CssClass="textb" Width="95%" runat="server" onkeypress="return isNumberKey(event);" />
                                </td>
                                <td style="width: 14%">
                                    <asp:Label ID="Label4" Text="Count" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="txtcount" CssClass="textb" Width="95%" runat="server" />
                                </td>
                                <td style="width: 14%">
                                    <asp:Label ID="Label7" Text="Sample Size" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="txtsamplesize" CssClass="textb" Width="95%" runat="server" onkeypress="return isNumberKey(event);" />
                                </td>
                                <td style="width: 14%">
                                    <asp:Label ID="Label5" Text="Int Lot No." runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="txtlotno" CssClass="textb" Width="95%" runat="server" />
                                </td>
                                <td style="width: 14%">
                                    <asp:Label ID="Label18" Text="Ven Lot No." runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="TxtVenderLotNo" CssClass="textb" Width="95%" runat="server" />
                                </td>
                                <td style="width: 14%">
                                    <asp:Label ID="Label8" Text="No. of Hank" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:TextBox ID="txtnoofhank" CssClass="textb" Width="95%" runat="server" onkeypress="return isNumberKey(event);" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table border="1" style="width: 100%;" cellspacing="0">
                <tr>
                    <td style="width: 10%; padding: 1% 1% 1% 1%; text-align: center">
                        <asp:Label Text="Sr No" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:Label ID="Label9" Text="Check Point" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:Label ID="Label10" Text="Specification" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:Label ID="Label11" Text="1" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:Label ID="Label12" Text="2" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:Label ID="Label13" Text="3" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:Label ID="Label14" Text="4" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:Label ID="Label15" Text="5" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:Label ID="Label16" Text="Average Value" runat="server" CssClass="labelbold" />
                    </td>
                </tr>
                <tr id="Tr1" runat="server">
                    <td style="width: 10%; padding: 1% 1% 1% 1%; text-align: center">
                        <asp:Label ID="Label17" Text="1" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:Label ID="lblcheckpoint_1" Text="T.P.I" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtspecification_1" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt1_1" CssClass="textb" Width="85%" runat="server" onchange="return keypress(1,1);" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt1_2" CssClass="textb" Width="85%" runat="server" onchange="return keypress(1,1);" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt1_3" CssClass="textb" Width="85%" runat="server" onchange="return keypress(1,1);" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt1_4" CssClass="textb" Width="85%" runat="server" onchange="return keypress(1,1);" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt1_5" CssClass="textb" Width="85%" runat="server" onchange="return keypress(1,1);" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtavgvalue_1" CssClass="textb" Width="85%" runat="server" />
                    </td>
                </tr>
                <tr id="Tr2" runat="server">
                    <td style="width: 10%; padding: 1% 1% 1% 1%; text-align: center">
                        <asp:Label ID="Label19" Text="2" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:Label ID="lblcheckpoint_2" Text="COUNT" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtspecification_2" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt2_1" CssClass="textb" Width="85%" runat="server" onchange="return keypress(2,1);" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt2_2" CssClass="textb" Width="85%" runat="server" onchange="return keypress(2,1);" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt2_3" CssClass="textb" Width="85%" runat="server" onchange="return keypress(2,1);" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt2_4" CssClass="textb" Width="85%" runat="server" onchange="return keypress(2,1);" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt2_5" CssClass="textb" Width="85%" runat="server" onchange="return keypress(2,1);" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtavgvalue_2" CssClass="textb" Width="85%" runat="server" />
                    </td>
                </tr>
                <tr id="Tr3" runat="server">
                    <td style="width: 10%; padding: 1% 1% 1% 1%; text-align: center">
                        <asp:Label ID="Label21" Text="3" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:Label ID="lblcheckpoint_3" Text="FIBRE LENGTH" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtspecification_3" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt3_1" CssClass="textb" Width="85%" runat="server" onchange="return keypress(3,1);" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt3_2" CssClass="textb" Width="85%" runat="server" onchange="return keypress(3,1);" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt3_3" CssClass="textb" Width="85%" runat="server" onchange="return keypress(3,1);" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt3_4" CssClass="textb" Width="85%" runat="server" onchange="return keypress(3,1);" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt3_5" CssClass="textb" Width="85%" runat="server" onchange="return keypress(3,1);" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtavgvalue_3" CssClass="textb" Width="85%" runat="server" />
                    </td>
                </tr>
                <tr id="Tr4" runat="server">
                    <td style="width: 10%; padding: 1% 1% 1% 1%; text-align: center">
                        <asp:Label ID="Label23" Text="4" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Label ID="Label24" Text="FIBRE CONTENT" runat="server" CssClass="labelbold" />
                                </td>
                                <td style="width: 60%">
                                    <table style="width: 100%" border="1" cellspacing="0">
                                        <tr>
                                            <td style="padding: 2%">
                                                <%--<asp:Label ID="lblcheckpointpet_4" Text="Pet" runat="server" CssClass="labelbold" />--%>
                                                <asp:TextBox ID="lblcheckpointpet_4" Text="Pet" runat="server" CssClass="textb" Width="95%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 2%">
                                                <%--<asp:Label ID="lblcheckpointother_4" Text="Other" runat="server" CssClass="labelbold" />--%>
                                                <asp:TextBox ID="lblcheckpointother_4" Text="Other" runat="server" CssClass="textb"
                                                    Width="95%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="text-align: center;">
                        <table style="width: 100%;">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtspecificationpet_4" CssClass="textb" Width="85%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtspecificationother_4" CssClass="textb" Width="85%" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td colspan="5" style="width: 50%; border-style: none; text-align: center">
                        <table style="width: 100%;">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtpet4_1" CssClass="textb" Width="85%" runat="server" onchange="return keypresspet_other(4,1,'pet');" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpet4_2" CssClass="textb" Width="85%" runat="server" onchange="return keypresspet_other(4,1,'pet');" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpet4_3" CssClass="textb" Width="85%" runat="server" onchange="return keypresspet_other(4,1,'pet');" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpet4_4" CssClass="textb" Width="85%" runat="server" onchange="return keypresspet_other(4,1,'pet');" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtpet4_5" CssClass="textb" Width="85%" runat="server" onchange="return keypresspet_other(4,1,'pet');" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtother4_1" CssClass="textb" Width="85%" runat="server" onchange="return keypresspet_other(4,1,'other');" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtother4_2" CssClass="textb" Width="85%" runat="server" onchange="return keypresspet_other(4,1,'other');" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtother4_3" CssClass="textb" Width="85%" runat="server" onchange="return keypresspet_other(4,1,'other');" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtother4_4" CssClass="textb" Width="85%" runat="server" onchange="return keypresspet_other(4,1,'other');" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtother4_5" CssClass="textb" Width="85%" runat="server" onchange="return keypresspet_other(4,1,'other');" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="text-align: center">
                        <table style="width: 100%;">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtavgvaluepet_4" CssClass="textb" Width="85%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtavgvalueother_4" CssClass="textb" Width="85%" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="Tr5" runat="server">
                    <td style="width: 10%; padding: 1% 1% 1% 1%; text-align: center">
                        <asp:Label ID="Label27" Text="5" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:Label ID="lblcheckpoint_5" Text="MOISTURE CONTENT" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtspecification_5" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt5_1" CssClass="textb" Width="85%" runat="server" onchange="return keypress(5,1);" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt5_2" CssClass="textb" Width="85%" runat="server" onchange="return keypress(5,1);" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt5_3" CssClass="textb" Width="85%" runat="server" onchange="return keypress(5,1);" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt5_4" CssClass="textb" Width="85%" runat="server" onchange="return keypress(5,1);" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt5_5" CssClass="textb" Width="85%" runat="server" onchange="return keypress(5,1);" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtavgvalue_5" CssClass="textb" Width="85%" runat="server" />
                    </td>
                </tr>
                <tr id="Tr6" runat="server">
                    <td style="width: 10%; padding: 1% 1% 1% 1%; text-align: center">
                        <asp:Label ID="Label29" Text="6" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:Label ID="lblcheckpoint_6" Text="pH VALUE" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtspecification_6" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt6_1" CssClass="textb" Width="85%" runat="server" onchange="return keypress(6,1);" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt6_2" CssClass="textb" Width="85%" runat="server" onchange="return keypress(6,1);" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt6_3" CssClass="textb" Width="85%" runat="server" onchange="return keypress(6,1);" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt6_4" CssClass="textb" Width="85%" runat="server" onchange="return keypress(6,1);" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt6_5" CssClass="textb" Width="85%" runat="server" onchange="return keypress(6,1);" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtavgvalue_6" CssClass="textb" Width="85%" runat="server" />
                    </td>
                </tr>
                <tr id="Tr7" runat="server">
                    <td style="width: 10%; padding: 1% 1% 1% 1%; text-align: center">
                        <asp:Label ID="Label31" Text="7" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center; width: 15%">
                        <asp:Label ID="lblcheckpoint_7" Text="SHADE(GENERAL APPEARANCE)" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtspecification_7" CssClass="textb" Width="85%" runat="server" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt7_1" CssClass="textb" Width="85%" runat="server" onchange="return keypress(7,1);" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt7_2" CssClass="textb" Width="85%" runat="server" onchange="return keypress(7,1);" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt7_3" CssClass="textb" Width="85%" runat="server" onchange="return keypress(7,1);" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt7_4" CssClass="textb" Width="85%" runat="server" onchange="return keypress(7,1);" />
                    </td>
                    <td style="text-align: center; width: 8%">
                        <asp:TextBox ID="txt7_5" CssClass="textb" Width="85%" runat="server" onchange="return keypress(7,1);" />
                    </td>
                    <td style="text-align: center">
                        <asp:TextBox ID="txtavgvalue_7" CssClass="textb" Width="85%" runat="server" />
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td style="width: 10%">
                        <asp:Label Text="Comments" CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 80%">
                        <asp:TextBox ID="txtcomments" runat="server" CssClass="textb" Width="95%" TextMode="MultiLine"
                            Height="30px" />
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
                    <td style="text-align: center">
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
    </asp:UpdatePanel>
</asp:Content>
