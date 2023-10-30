<%@ Page Title="Shift Master" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmshiftmaster.aspx.cs" Inherits="Masters_Payroll_frmshiftmaster" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmshiftmaster.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function AddColon(txt) {
            if (txt.value.length == 2) {
                txt.value += ":";
            }
        }

        function validatetime(id) {
            var time = id.value;
            //alert(time);
            var re = /^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$/;

            if (re.test(time)) {
                //alert(re.test(time));               

            } else {
                id.value = "";
            }
        }
        function validateDec(key, id) {
            //getting key code of pressed key

            var keycode = (key.which) ? key.which : key.keyCode;
            //comparing pressed keycodes
            if (!(keycode == 8 || keycode == 46) && (keycode < 48 || keycode > 57)) {
                return false;
            }
            else {
                var parts = key.srcElement.value.split('.');
                if (parts.length > 1 && keycode == 46) {
                    return false;
                }
                else {
                    if (id.value != "") {


                        var number = id.value.split('.');
                        if (number.length > 1) {
                            var pointval = number[1] + key.key;
                            if (pointval > 59) {
                                alert('After Point value can not greater than 59.');
                                return false;
                            }
                        }
                        var val = id.value + key.key;

                        if (val > 24) {
                            alert('Value can not greater than 24.')
                            return false;
                        }
                    }
                    return true;
                }



            }
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div style="margin: 0% 10% 0% 10%">
                <table border="1" cellspacing="5" width="80%">
                    <tr>
                        <td style="width: 25%; border-style: dotted" runat="server" id="Tdedit" visible="false">
                            <asp:CheckBox ID="chkedit" CssClass="checkboxbold" Text="For Edit" AutoPostBack="true"
                                runat="server" OnCheckedChanged="chkedit_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 25%; border-style: dotted">
                            <asp:Label ID="Label37" Text="Company Name" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="border-style: dotted">
                            <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" AutoPostBack="true"
                                Width="100%" OnSelectedIndexChanged="DDcompany_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr runat="server" id="Trshiftcode" visible="false">
                        <td style="width: 25%; border-style: dotted">
                            <asp:Label Text="Shift Code" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="border-style: dotted">
                            <asp:DropDownList ID="DDshiftcode" runat="server" CssClass="dropdown" AutoPostBack="true"
                                Width="100%" OnSelectedIndexChanged="DDshiftcode_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 25%; border-style: dotted">
                            <asp:Label ID="Label1" Text="Shift Code" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="border-style: dotted">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 40%">
                                        <asp:TextBox ID="txtshiftcode" CssClass="textb" runat="server" Width="60%" MaxLength="5" />
                                    </td>
                                    <td>
                                        <asp:Label Text="Type :" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rdday" Text="Day" runat="server" Checked="true" CssClass="radiobuttonnormal"
                                            GroupName="s" />
                                        <asp:RadioButton ID="rdnight" Text="Night" runat="server" CssClass="radiobuttonnormal"
                                            GroupName="s" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-style: dotted">
                            <asp:Label Text="In Time :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="border-style: dotted">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 40%">
                                        <asp:TextBox ID="txtintime" CssClass="textb" Width="60%" runat="server" MaxLength="5"
                                            onkeypress="return validateDec(event,this)" />
                                    </td>
                                    <td style="width: 20%">
                                        <asp:Label ID="Label2" Text="Out Time :" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 40%">
                                        <asp:TextBox ID="txtouttime" CssClass="textb" Width="60%" runat="server" MaxLength="5"
                                            onkeypress="return validateDec(event,this)" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-style: dotted">
                            <asp:Label ID="Label38" Text="In Time Relaxation(In Minutes) :" CssClass="labelbold"
                                runat="server" />
                        </td>
                        <td style="border-style: dotted">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 40%">
                                        <asp:TextBox ID="txtintimerelaxation" CssClass="textb" Width="60%" runat="server"
                                            MaxLength="5" />
                                    </td>
                                    <td style="width: 60%">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="border-style: dotted">
                            <fieldset>
                                <legend>
                                    <asp:Label Text="Lunch Break" CssClass="labelbold" runat="server" ForeColor="Red" />
                                </legend>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 20%">
                                            <asp:Label Text="Lunch Break :" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="Rdlunchbreakyes" Text="Yes" GroupName="L" runat="server" CssClass="radiobuttonnormal" />
                                            <asp:RadioButton ID="Rdlunchbreakno" Text="No" runat="server" GroupName="L" CssClass="radiobuttonnormal" />
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 20%">
                                            <asp:Label ID="Label3" Text="Break Out :" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtbreakout" CssClass="textb" runat="server" Width="60%" MaxLength="5"
                                                onkeypress="return validateDec(event,this)" />
                                        </td>
                                        <td style="width: 20%">
                                            <asp:Label ID="Label4" Text="Break In :" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtbreakin" CssClass="textb" runat="server" Width="60%" MaxLength="5"
                                                onkeypress="return validateDec(event,this)" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%">
                                            <asp:Label ID="Label5" Text="Break Out Start:" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtbreakoutstart" CssClass="textb" runat="server" Width="60%" MaxLength="5"
                                                onkeypress="return validateDec(event,this)" />
                                        </td>
                                        <td style="width: 20%">
                                            <asp:Label ID="Label6" Text="Break In End:" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtbreakinend" CssClass="textb" runat="server" Width="60%" MaxLength="5"
                                                onkeypress="return validateDec(event,this)" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%">
                                            <asp:Label ID="Label7" Text="Break Deduction:" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="Rdbreakdeductionyes" Text="Yes" GroupName="Bd" runat="server"
                                                CssClass="radiobuttonnormal" />
                                            <asp:RadioButton ID="Rdbreakdeductionno" Text="No" runat="server" GroupName="Bd"
                                                CssClass="radiobuttonnormal" />
                                        </td>
                                        <td style="width: 25%">
                                            <asp:Label ID="Label8" Text="Excess Break Deduction:" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="Rdexcessbreakdeductionyes" Text="Yes" GroupName="EBd" runat="server"
                                                CssClass="radiobuttonnormal" />
                                            <asp:RadioButton ID="Rdexcessbreakdeductionno" Text="No" runat="server" GroupName="EBd"
                                                CssClass="radiobuttonnormal" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="border-style: dotted">
                            <fieldset>
                                <legend>
                                    <asp:Label ID="Label9" Text="Tea Break" CssClass="labelbold" runat="server" ForeColor="Red" />
                                </legend>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 50%">
                                            <table width="100%">
                                                <tr>
                                                    <td style="text-align: center">
                                                        <asp:Label Text="Ist" CssClass="labelbold" ForeColor="Red" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 30%">
                                                                    <asp:Label Text="Start Time :" runat="server" CssClass="labelbold"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtteastartist" CssClass="textb" runat="server" Width="60%" MaxLength="5"
                                                                        onkeypress="return validateDec(event,this)" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 30%">
                                                                    <asp:Label ID="Label11" Text="End Time :" runat="server" CssClass="labelbold"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtteaendist" CssClass="textb" runat="server" Width="60%" MaxLength="5"
                                                                        onkeypress="return validateDec(event,this)" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 50%">
                                            <table width="100%">
                                                <tr>
                                                    <td style="text-align: center">
                                                        <asp:Label ID="Label10" Text="IInd" CssClass="labelbold" ForeColor="Red" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 30%">
                                                                    <asp:Label ID="Label12" Text="Start Time :" runat="server" CssClass="labelbold"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtteastartiind" CssClass="textb" runat="server" Width="60%" MaxLength="5"
                                                                        onkeypress="return validateDec(event,this)" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 30%">
                                                                    <asp:Label ID="Label13" Text="End Time :" runat="server" CssClass="labelbold"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtteaendiind" CssClass="textb" runat="server" Width="60%" MaxLength="5"
                                                                        onkeypress="return validateDec(event,this)" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-style: dotted; width: 10%">
                            <asp:Label ID="Label14" Text="OT Before :" CssClass="labelbold" runat="server" />
                        </td>
                        <td colspan="2" style="border-style: dotted; width: 90%">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 20%">
                                        <asp:RadioButton ID="rdotbeforeyes" Text="Yes" runat="server" GroupName="OTB" CssClass="radiobuttonnormal" />
                                        <asp:RadioButton ID="rdotbeforeno" runat="server" Text="No" GroupName="OTB" CssClass="radiobuttonnormal" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtotbeforehour" CssClass="textb" runat="server" MaxLength="5" onkeypress="return validateDec(event,this)" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-style: dotted; width: 10%">
                            <asp:Label ID="Label15" Text="OT After :" CssClass="labelbold" runat="server" />
                        </td>
                        <td colspan="2" style="border-style: dotted; width: 90%">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 20%">
                                        <asp:RadioButton ID="rdotafteryes" Text="Yes" runat="server" GroupName="OTA" CssClass="radiobuttonnormal" />
                                        <asp:RadioButton ID="rdotafterno" runat="server" Text="No" GroupName="OTA" CssClass="radiobuttonnormal" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtotafterhour" CssClass="textb" runat="server" MaxLength="5" onkeypress="return validateDec(event,this)" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-style: dotted">
                            <asp:Label ID="Label16" Text="Halfday Hrs >= :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="border-style: dotted">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 40%">
                                        <asp:TextBox ID="txthalfdayhrs" CssClass="textb" Width="60%" runat="server" MaxLength="5"
                                            onkeypress="return validateDec(event,this)" />
                                    </td>
                                    <td style="width: 20%">
                                        <asp:Label ID="Label17" Text="Fullday Hrs :" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 40%">
                                        <asp:TextBox ID="txtfulldayhrs" CssClass="textb" Width="60%" runat="server" MaxLength="5"
                                            onkeypress="return validateDec(event,this)" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-style: dotted">
                            <asp:Label ID="Label18" Text="Late Arrival Grace :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="border-style: dotted">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 40%">
                                        <asp:TextBox ID="txtlatearrivalgrace" CssClass="textb" Width="60%" runat="server"
                                            MaxLength="5" onkeypress="return validateDec(event,this)" />
                                    </td>
                                    <td style="width: 30%">
                                        <asp:Label ID="Label19" Text="Arrival Grace Deduction :" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 40%">
                                        <asp:RadioButton ID="rdarrivalgracedeductionyes" runat="server" Text="Yes" CssClass="radiobuttonnormal"
                                            GroupName="agd" /><asp:RadioButton ID="rdarrivalgracedeductionno" runat="server"
                                                Text="No" GroupName="agd" CssClass="radiobuttonnormal" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-style: dotted">
                            <asp:Label ID="Label20" Text="Early Departure Grace :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="border-style: dotted">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 40%">
                                        <asp:TextBox ID="txtearlydeparturegrace" CssClass="textb" Width="60%" runat="server"
                                            MaxLength="5" onkeypress="return validateDec(event,this)" />
                                    </td>
                                    <td style="width: 35%">
                                        <asp:Label ID="Label21" Text="Departure Grace Deduction :" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 40%">
                                        <asp:RadioButton ID="rddeparturegracedeductionyes" runat="server" Text="Yes" CssClass="radiobuttonnormal"
                                            GroupName="dgd" /><asp:RadioButton ID="rddeparturegracedeductionno" runat="server"
                                                Text="No" GroupName="dgd" CssClass="radiobuttonnormal" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-style: dotted">
                            <asp:Label ID="Label22" Text="Full Shift :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="border-style: dotted">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 40%">
                                        <asp:RadioButton ID="rdfullshiftyes" Text="Yes" CssClass="radiobuttonnormal" runat="server"
                                            GroupName="fs" /><asp:RadioButton ID="rdfullshiftno" Text="No" CssClass="radiobuttonnormal"
                                                runat="server" GroupName="fs" />
                                    </td>
                                    <td style="width: 20%">
                                        <asp:Label ID="Label23" Text="Half Shift :" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 40%">
                                        <asp:TextBox ID="txthalfshift" CssClass="textb" runat="server" MaxLength="5" onkeypress="return validateDec(event,this)" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-style: dotted">
                            <asp:Label ID="Label24" Text="In Start :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="border-style: dotted">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 40%">
                                        <asp:TextBox ID="txtinstart" CssClass="textb" Width="60%" runat="server" MaxLength="5"
                                            onkeypress="return validateDec(event,this)" />
                                    </td>
                                    <td style="width: 20%">
                                        <asp:Label ID="Label25" Text="In End :" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 40%">
                                        <asp:TextBox ID="txtinend" CssClass="textb" Width="60%" runat="server" MaxLength="5"
                                            onkeypress="return validateDec(event,this)" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-style: dotted">
                            <asp:Label ID="Label26" Text="Out Start :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="border-style: dotted">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 20%">
                                        <asp:TextBox ID="txtoutstart" CssClass="textb" Width="60%" runat="server" MaxLength="5"
                                            onkeypress="return validateDec(event,this)" />
                                    </td>
                                    <td style="width: 25%">
                                        <asp:Label ID="Label27" Text="Out End :" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 20%">
                                        <asp:TextBox ID="txtoutend" CssClass="textb" Width="60%" runat="server" MaxLength="5"
                                            onkeypress="return validateDec(event,this)" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-style: dotted">
                            <asp:Label ID="Label28" Text="Work Hour :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="border-style: dotted; width: 60%" colspan="2">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtworkhour" CssClass="textb" Width="25%" runat="server" MaxLength="5"
                                            onkeypress="return validateDec(event,this)" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="border-style: dotted">
                            <fieldset>
                                <legend>
                                    <asp:Label ID="Label29" Text="OT Register" CssClass="labelbold" runat="server" ForeColor="Red" />
                                </legend>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 20%">
                                            <asp:Label ID="Label31" Text="Crop Before < (-):" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtcropbefore" CssClass="textb" runat="server" Width="60%" MaxLength="5"
                                                onkeypress="return validateDec(event,this)" />
                                        </td>
                                        <td style="width: 20%">
                                            <asp:Label ID="Label32" Text="Before Rand :" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtbeforerand" CssClass="textb" runat="server" Width="60%" MaxLength="5"
                                                onkeypress="return validateDec(event,this)" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%">
                                            <asp:Label ID="Label33" Text="Crop After > (+):" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtcropafter" CssClass="textb" runat="server" Width="60%" MaxLength="5"
                                                onkeypress="return validateDec(event,this)" />
                                        </td>
                                        <td style="width: 20%">
                                            <asp:Label ID="Label34" Text="After Rand :" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtafterrand" CssClass="textb" runat="server" Width="60%" MaxLength="5"
                                                onkeypress="return validateDec(event,this)" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-style: dotted">
                            <asp:Label ID="Label30" Text="Tea Break Hour After Shift :" CssClass="labelbold"
                                runat="server" />
                        </td>
                        <td style="border-style: dotted; width: 60%" colspan="2">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtteabreakhouraftershift" CssClass="textb" Width="25%" runat="server"
                                            MaxLength="5" onkeypress="return validateDec(event,this)" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-style: dotted">
                            <asp:Label ID="Label35" Text="Dinner Break Deduct after :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="border-style: dotted">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 40%">
                                        <asp:TextBox ID="txtdinnerbreakdeductafter" CssClass="textb" Width="60%" runat="server"
                                            MaxLength="5" onkeypress="return validateDec(event,this)" />
                                    </td>
                                    <td style="width: 20%">
                                        <asp:Label ID="Label36" Text="Hour :" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 40%">
                                        <asp:TextBox ID="txtdinnerbreakdeductafterhour" CssClass="textb" Width="60%" runat="server"
                                            MaxLength="5" onkeypress="return validateDec(event,this)" />
                                    </td>
                                </tr>
                            </table>
                            .
                        </td>
                    </tr>
                    <tr>
                        <td style="border-style: dotted">
                            <asp:Label ID="Label39" Text="Min In time" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="border-style: dotted">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 20%">
                                        <asp:TextBox ID="TxtShiftMinIntime" CssClass="textboxm" Width="95%" runat="server"
                                            placeholder="hh:mm" Style="text-align: center" onblur="validatetime(this);" onkeypress="AddColon(this)" />
                                    </td>
                                    <td style="width: 25%">
                                        <asp:Label ID="Label40" Text="Max Out time :" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 20%">
                                        <asp:TextBox ID="TxtShiftMaxIntime" CssClass="textboxm" Width="95%" runat="server"
                                            placeholder="hh:mm" Style="text-align: center" onblur="validatetime(this);" onkeypress="AddColon(this)" />
                                    </td>
                                    <td style="width: 15%">
                                        <asp:Label ID="Label41" Text="Next Day :" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 20%">
                                        <asp:TextBox ID="TxtNextDay" CssClass="textboxm" Width="95%" runat="server" Style="text-align: center" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table style="width: 80%">
                    <tr>
                        <td style="text-align: right">
                            <asp:Button Text="Save" ID="btnsave" CssClass="buttonnorm" runat="server" OnClick="btnsave_Click" />
                            <asp:Button Text="Close" ID="btnclose" CssClass="buttonnorm" runat="server" OnClientClick="return CloseForm();" />
                            <asp:Button Text="New" ID="btnnew" CssClass="buttonnorm" runat="server" OnClientClick="return NewForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Font-Bold="true" Text=""
                                runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hnshiftid" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
