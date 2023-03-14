<%@ Page Title="Bio-Data Form Entry" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmbiodataentry.aspx.cs" Inherits="Masters_Payroll_frmbiodataentry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function Addgroupmaster() {
            window.open('../Payroll/AddGroupMaster.aspx', '', 'Height=350px,width=500px');
        }

        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function ClickNew() {
            window.location.href = "frmbiodataentry.aspx";
        }     
    </script>
    <asp:UpdatePanel runat="server" ID="upd1">
        <ContentTemplate>
            <asp:TabContainer ID="tcbiodata" Width="100%" ActiveTabIndex="0" runat="server">
                <asp:TabPanel ID="tpempinformation" HeaderText="Employee Information" runat="server">
                    <ContentTemplate>
                        <table width="100%">
                            <tr runat="server" id="Tredit" visible="False">
                                <td colspan="2" runat="server">
                                    <table width="100%" border="1" cellspacing="2">
                                        <tr>
                                            <td style="width: 10%; border-style: dotted">
                                                <asp:CheckBox ID="chkedit" Text="For Edit" CssClass="checkboxbold" runat="server"
                                                    AutoPostBack="True" OnCheckedChanged="chkedit_CheckedChanged" />
                                            </td>
                                            <td style="width: 90%; border-style: dotted" runat="server" id="Tdeditcardno" visible="False">
                                                <table width="100%" border="1">
                                                    <tr>
                                                        <td style="width: 10%; border-style: dotted">
                                                            <asp:Label ID="Label99" Text="Card No." CssClass="labelbold" runat="server" />
                                                        </td>
                                                        <td style="width: 30%; border-style: dotted">
                                                            <asp:TextBox ID="txteditcardno" CssClass="textboxm" runat="server" Width="95%" />
                                                        </td>
                                                        <td style="border-style: dotted">
                                                            <asp:Button ID="btngetdetail" CausesValidation="False" Text="Get Detail" CssClass="buttonnorm"
                                                                runat="server" OnClick="btngetdetail_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 50%;" valign="top">
                                    <table border="1" cellspacing="2" style="width: 100%">
                                        <tr>
                                            <td style="width: 20%; border-style: dotted">
                                                <asp:Label ID="Label21" runat="server" CssClass="labelbold" Text="Post Applied For :"></asp:Label>
                                            </td>
                                            <td style="width: 80%; border-style: dotted">
                                                <asp:DropDownList ID="ddpostapplied" runat="server" CssClass="dropdown" Width="95%">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="border-style: none">
                                                <asp:CompareValidator runat="server" ControlToValidate="ddpostapplied" ErrorMessage="*"
                                                    ForeColor="Red" Operator="GreaterThan" SetFocusOnError="True" ValueToCompare="0"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%; border-style: dotted">
                                                <asp:Label ID="Label1" runat="server" CssClass="labelbold" Text="Name"></asp:Label>
                                            </td>
                                            <td style="width: 80%; border-style: dotted">
                                                <asp:TextBox ID="txtname" runat="server" CssClass="textboxm" MaxLength="50" Width="95%"></asp:TextBox>
                                            </td>
                                            <td style="border-style: none">
                                                <asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="txtname"
                                                    ErrorMessage="*" ForeColor="Red" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 30%; border-style: dotted">
                                                <asp:Label ID="lblfather_husbandname" runat="server" CssClass="labelbold" Text="Father's/Husband's Name"></asp:Label>
                                            </td>
                                            <td style="width: 80%; border-style: dotted">
                                                <asp:TextBox ID="txtfather_husbandname" runat="server" CssClass="textboxm" MaxLength="50"
                                                    Width="95%"></asp:TextBox>
                                            </td>
                                            <td style="border-style: none">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtfather_husbandname"
                                                    ErrorMessage="*" ForeColor="Red" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 30%; border-style: dotted">
                                                <asp:Label ID="lblmothername" runat="server" CssClass="labelbold" Text="Mother's Name"></asp:Label>
                                            </td>
                                            <td style="width: 80%; border-style: dotted">
                                                <asp:TextBox ID="txtmothername" runat="server" CssClass="textboxm" Width="95%"></asp:TextBox>
                                            </td>
                                            <td style="border-style: none">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtmothername"
                                                    ErrorMessage="*" ForeColor="Red" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 30%; border-style: dotted">
                                                <asp:Label ID="Label4" runat="server" CssClass="labelbold" Text="Date of Birth"></asp:Label>
                                            </td>
                                            <td style="width: 80%; border-style: dotted">
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 40%">
                                                            <asp:TextBox ID="txtdateofbirth" runat="server" AutoComplete="off" AutoPostBack="True"
                                                                CssClass="textboxm" OnTextChanged="txtdateofbirth_TextChanged" Width="95%"></asp:TextBox><asp:CalendarExtender
                                                                    ID="caldateofbirth" runat="server" Enabled="True" Format="dd-MMM-yyyy" TargetControlID="txtdateofbirth">
                                                                </asp:CalendarExtender>
                                                        </td>
                                                        <td style="border-style: none">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtdateofbirth"
                                                                ErrorMessage="*" ForeColor="Red" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td style="width: 20%">
                                                            <asp:Label ID="lblage" runat="server" CssClass="labelbold" Text="Age :"></asp:Label>
                                                        </td>
                                                        <td style="width: 40%">
                                                            <asp:TextBox ID="txtage" runat="server" CssClass="textboxm" Width="95%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 30%; border-style: dotted">
                                                <asp:Label ID="Label5" runat="server" CssClass="labelbold" Text="Gender"></asp:Label>
                                            </td>
                                            <td style="width: 80%; border-style: dotted">
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 100%">
                                                            <asp:RadioButton ID="rdmale" runat="server" Checked="True" CssClass="radiobutton"
                                                                GroupName="sex" Text="Male" /><asp:RadioButton ID="rdfemale" runat="server" CssClass="radiobutton"
                                                                    GroupName="sex" Text="Female" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 30%; border-style: dotted">
                                                <asp:Label ID="Label6" runat="server" CssClass="labelbold" Text="Marital Status"></asp:Label>
                                            </td>
                                            <td style="width: 80%; border-style: dotted">
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 100%">
                                                            <asp:RadioButton ID="rdmarried" runat="server" CssClass="radiobutton" GroupName="marital"
                                                                Text="Married" /><asp:RadioButton ID="rdunmarried" runat="server" Checked="True"
                                                                    CssClass="radiobutton" GroupName="marital" Text="Unmarried" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="width: 100%">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 15%">
                                                            <asp:Label ID="Label2" runat="server" CssClass="labelbold" Text="Nationality"></asp:Label>
                                                        </td>
                                                        <td style="width: 30%">
                                                            <asp:TextBox ID="txtnationality" runat="server" CssClass="textboxm" MaxLength="20"
                                                                Text="INDIAN" Width="95%"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 15%">
                                                            <asp:Label ID="Label7" runat="server" CssClass="labelbold" Text="Religion"></asp:Label>
                                                        </td>
                                                        <td style="width: 30%">
                                                            <asp:TextBox ID="txtreligion" runat="server" CssClass="textboxm" MaxLength="20" Width="95%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 15%">
                                                            <asp:Label ID="Label8" runat="server" CssClass="labelbold" Text="Caste"></asp:Label>
                                                        </td>
                                                        <td style="width: 30%">
                                                            <asp:TextBox ID="txtcaster" runat="server" CssClass="textboxm" MaxLength="20" Width="95%"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 15%">
                                                            <asp:Label ID="Label9" runat="server" CssClass="labelbold" Text="Region"></asp:Label>
                                                        </td>
                                                        <td style="width: 30%">
                                                            <asp:TextBox ID="txtregion" runat="server" CssClass="textboxm" MaxLength="20" Width="95%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 30%; border-style: dotted">
                                                <asp:Label ID="Label20" runat="server" CssClass="labelbold" Text="Shift Type :"></asp:Label>
                                            </td>
                                            <td style="width: 80%; border-style: dotted">
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 100%">
                                                            <asp:RadioButton ID="rdfixed" runat="server" Checked="True" CssClass="radiobutton"
                                                                GroupName="shifttype" Text="Fixed" /><asp:RadioButton ID="rdrotational" runat="server"
                                                                    CssClass="radiobutton" GroupName="shifttype" Text="Rotational" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 50%;" valign="top">
                                    <table border="1" cellspacing="2" style="width: 100%">
                                        <tr>
                                            <td style="width: 30%; border-style: dotted">
                                                <asp:Label ID="Label18" runat="server" CssClass="labelbold" Text="Pay Code :"></asp:Label>
                                            </td>
                                            <td style="width: 80%; border-style: dotted">
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 40%">
                                                            <asp:TextBox ID="txtpaycode" runat="server" BackColor="LightYellow" CssClass="textboxm"
                                                                Enabled="False" Width="95%"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 20%">
                                                            <asp:Label ID="Label19" runat="server" CssClass="labelbold" Text="Card No. :"></asp:Label>
                                                        </td>
                                                        <td style="width: 40%">
                                                            <asp:TextBox ID="txtcardno" runat="server" BackColor="LightYellow" CssClass="textboxm"
                                                                Enabled="False" Width="95%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%; border-style: dotted">
                                                <asp:Label ID="Label10" runat="server" CssClass="labelbold" Text="Designation"></asp:Label>
                                            </td>
                                            <td style="width: 80%; border-style: dotted">
                                                <asp:DropDownList ID="dddesignation" runat="server" CssClass="dropdown" Width="95%">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="border-style: none">
                                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="dddesignation"
                                                    ErrorMessage="*" ForeColor="Red" Operator="GreaterThan" SetFocusOnError="True"
                                                    ValueToCompare="0"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%; border-style: dotted">
                                                <asp:Label ID="Label11" runat="server" CssClass="labelbold" Text="Department"></asp:Label>
                                            </td>
                                            <td style="width: 80%; border-style: dotted">
                                                <asp:DropDownList ID="dddepartment" runat="server" CssClass="dropdown" Width="95%">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="border-style: none">
                                                <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="dddepartment"
                                                    ErrorMessage="*" ForeColor="Red" Operator="GreaterThan" SetFocusOnError="True"
                                                    ValueToCompare="0"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%; border-style: dotted">
                                                <asp:Label ID="Label12" runat="server" CssClass="labelbold" Text="Sub Dept."></asp:Label>
                                            </td>
                                            <td style="width: 80%; border-style: dotted">
                                                <asp:DropDownList ID="ddsubdept" runat="server" CssClass="dropdown" Width="95%">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="border-style: none">
                                                <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="ddsubdept"
                                                    ErrorMessage="*" ForeColor="Red" Operator="GreaterThan" SetFocusOnError="True"
                                                    ValueToCompare="0"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%; border-style: dotted">
                                                <asp:Label ID="Label13" runat="server" CssClass="labelbold" Text="Division"></asp:Label>
                                            </td>
                                            <td style="width: 80%; border-style: dotted">
                                                <asp:DropDownList ID="dddivision" runat="server" CssClass="dropdown" Width="95%">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="border-style: none">
                                                <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToValidate="dddivision"
                                                    ErrorMessage="*" ForeColor="Red" Operator="GreaterThan" SetFocusOnError="True"
                                                    ValueToCompare="0"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%; border-style: dotted">
                                                <asp:Label ID="Label14" runat="server" CssClass="labelbold" Text="Category"></asp:Label>
                                            </td>
                                            <td style="width: 80%; border-style: dotted">
                                                <asp:DropDownList ID="ddcategory" runat="server" CssClass="dropdown" Width="95%">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="border-style: none">
                                                <asp:CompareValidator ID="CompareValidator5" runat="server" ControlToValidate="ddcategory"
                                                    ErrorMessage="*" ForeColor="Red" Operator="GreaterThan" SetFocusOnError="True"
                                                    ValueToCompare="0"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%; border-style: dotted">
                                                <asp:Label ID="Label15" runat="server" CssClass="labelbold" Text="Cadre"></asp:Label>
                                            </td>
                                            <td style="width: 80%; border-style: dotted">
                                                <asp:DropDownList ID="ddcadre" runat="server" CssClass="dropdown" Width="95%">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="border-style: none">
                                                <asp:CompareValidator ID="CompareValidator6" runat="server" ControlToValidate="ddcadre"
                                                    ErrorMessage="*" ForeColor="Red" Operator="GreaterThan" SetFocusOnError="True"
                                                    ValueToCompare="0"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%; border-style: dotted">
                                                <asp:Label ID="Label16" runat="server" CssClass="labelbold" Text="Company"></asp:Label>
                                            </td>
                                            <td style="width: 80%; border-style: dotted">
                                                <asp:DropDownList ID="ddcompany" runat="server" CssClass="dropdown" Width="95%">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="border-style: none">
                                                <asp:CompareValidator ID="CompareValidator7" runat="server" ControlToValidate="ddcompany"
                                                    ErrorMessage="*" ForeColor="Red" Operator="GreaterThan" SetFocusOnError="True"
                                                    ValueToCompare="0"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%; border-style: dotted">
                                                <asp:Label ID="Label17" runat="server" CssClass="labelbold" Text="Location"></asp:Label>
                                            </td>
                                            <td style="width: 80%; border-style: dotted">
                                                <asp:DropDownList ID="ddlocation" runat="server" CssClass="dropdown" Width="95%">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="border-style: none">
                                                <asp:CompareValidator ID="CompareValidator8" runat="server" ControlToValidate="ddlocation"
                                                    ErrorMessage="*" ForeColor="Red" Operator="GreaterThan" SetFocusOnError="True"
                                                    ValueToCompare="0"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%; border-style: dotted">
                                                <asp:Label ID="Label118" runat="server" CssClass="labelbold" Text="Branch Name"></asp:Label>
                                            </td>
                                            <td style="width: 80%; border-style: dotted">
                                                <asp:DropDownList ID="DDBranchName" runat="server" CssClass="dropdown" Width="95%">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td style="width: 20%">
                                                <asp:Label ID="Label119" runat="server" CssClass="labelbold" Text="Identifaction Mark"></asp:Label>
                                            </td>
                                            <td style="width: 80%">
                                                <asp:TextBox ID="TxtIdentificationMark" runat="server" CssClass="textboxm" Width="95%"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table width="100%" border="1" cellspacing="2">
                            <tr>
                                <td style="width: 10%; border-style: dotted">
                                    <asp:Label ID="Label22" Text="Shift Option" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 25%; border-style: dotted">
                                    <asp:DropDownList ID="ddshiftoption" CssClass="dropdown" Width="95%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="border-style: none">
                                    <asp:CompareValidator ID="CompareValidator9" ErrorMessage="*" ControlToValidate="ddshiftoption"
                                        Operator="GreaterThan" ValueToCompare="0" runat="server" ForeColor="Red" SetFocusOnError="True" />
                                </td>
                                <td style="width: 10%; border-style: dotted">
                                    <asp:Label ID="Label23" Text="Doc Type" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 25%; border-style: dotted">
                                    <asp:DropDownList ID="dddoctype" CssClass="dropdown" Width="95%" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td style="border-style: none">
                                    <asp:CompareValidator ID="CompareValidator10" ErrorMessage="*" ControlToValidate="dddoctype"
                                        Operator="GreaterThan" ValueToCompare="0" runat="server" ForeColor="Red" />
                                </td>
                                <td style="width: 10%; border-style: dotted">
                                    <asp:Label ID="Label24" Text="Doc No." CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 30%; border-style: dotted">
                                    <asp:TextBox ID="txtdocno" CssClass="textboxm" runat="server" Width="95%" MaxLength="50" />
                                </td>
                            </tr>
                        </table>
                        <fieldset>
                            <legend>
                                <asp:Label ID="lbladdresslegend" Text="Address" CssClass="labelbold" ForeColor="Red"
                                    Font-Bold="True" runat="server" /></legend>
                            <table width="100%">
                                <tr>
                                    <td style="width: 100%">
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 45%">
                                                    <table width="100%">
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:Label ID="lblpresentadd" Text="Present Address :" CssClass="labelbold" Font-Underline="True"
                                                                    runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 10%">
                                                                <asp:Label ID="Label25" Text="Vill :" CssClass="labelbold" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtvill_present" CssClass="textboxm" Width="95%" runat="server"
                                                                    MaxLength="50" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 10%">
                                                                <asp:Label ID="Label26" Text="P.O :" CssClass="labelbold" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtpo_present" CssClass="textboxm" Width="95%" runat="server" MaxLength="30" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 20%">
                                                                <asp:Label ID="Label27" Text="Police Station :" CssClass="labelbold" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtpolicestation_present" CssClass="textboxm" Width="95%" runat="server"
                                                                    MaxLength="30" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 20%">
                                                                <asp:Label ID="Label28" Text="Sub Division :" CssClass="labelbold" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtsubdivision_present" CssClass="textboxm" Width="95%" runat="server"
                                                                    MaxLength="50" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 20%">
                                                                <asp:Label ID="Label29" Text="Distt :" CssClass="labelbold" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtdistt_present" CssClass="textboxm" Width="95%" runat="server"
                                                                    MaxLength="30" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 20%">
                                                                <asp:Label ID="Label30" Text="State :" CssClass="labelbold" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtstate_present" CssClass="textboxm" Width="95%" runat="server"
                                                                    MaxLength="20" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 20%">
                                                                <asp:Label ID="Label31" Text="Pin Code :" CssClass="labelbold" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtpincode_present" CssClass="textboxm" Width="95%" runat="server"
                                                                    MaxLength="10" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 20%">
                                                                <asp:Label ID="Label32" Text="Ph. No. :" CssClass="labelbold" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtphno_present" CssClass="textboxm" Width="95%" runat="server"
                                                                    MaxLength="22" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="width: 10%; text-align: center">
                                                    <asp:Button ID="btnsameaddress" Text=">>" CssClass="buttonnorm" ToolTip="Same as Present Address"
                                                        runat="server" CausesValidation="False" OnClick="btnsameaddress_Click" />
                                                </td>
                                                <td style="width: 45%">
                                                    <table width="100%">
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:Label ID="Label33" Text="Permanent Address :" CssClass="labelbold" Font-Underline="True"
                                                                    runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 10%">
                                                                <asp:Label ID="Label34" Text="Vill :" CssClass="labelbold" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtvill_permanent" CssClass="textboxm" Width="95%" runat="server"
                                                                    MaxLength="50" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 10%">
                                                                <asp:Label ID="Label35" Text="P.O :" CssClass="labelbold" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtpo_permanent" CssClass="textboxm" Width="95%" runat="server"
                                                                    MaxLength="30" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 20%">
                                                                <asp:Label ID="Label36" Text="Police Station :" CssClass="labelbold" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtpolicestation_permanent" CssClass="textboxm" Width="95%" runat="server"
                                                                    MaxLength="30" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 20%">
                                                                <asp:Label ID="Label37" Text="Sub Division :" CssClass="labelbold" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtsubdivision_permanent" CssClass="textboxm" Width="95%" runat="server"
                                                                    MaxLength="50" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 20%">
                                                                <asp:Label ID="Label38" Text="Distt :" CssClass="labelbold" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtdistt_permanent" CssClass="textboxm" Width="95%" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 20%">
                                                                <asp:Label ID="Label39" Text="State :" CssClass="labelbold" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtstate_permanent" CssClass="textboxm" Width="95%" runat="server"
                                                                    MaxLength="20" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 20%">
                                                                <asp:Label ID="Label40" Text="Pin Code :" CssClass="labelbold" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtpincode_permanent" CssClass="textboxm" Width="95%" runat="server"
                                                                    MaxLength="10" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 20%">
                                                                <asp:Label ID="Label41" Text="Ph. No. :" CssClass="labelbold" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtphno_permanent" CssClass="textboxm" Width="95%" runat="server"
                                                                    MaxLength="22" />
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
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tpexp_qual" HeaderText="Experience/Qualification" runat="server">
                    <ContentTemplate>
                        <table border="1" cellspacing="2" width="100%">
                            <tr>
                                <td style="width: 10%; border-style: dotted">
                                    <asp:Label Text="Qualification :" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 20%; border-style: dotted">
                                    <asp:TextBox ID="txtqualification" CssClass="textboxm" runat="server" Width="95%"
                                        MaxLength="50" />
                                </td>
                                <td style="width: 15%; border-style: dotted">
                                    <asp:Label ID="Label3" Text="Technical Qualification :" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 20%; border-style: dotted">
                                    <asp:TextBox ID="txttechnicalqualification" CssClass="textboxm" runat="server" Width="95%"
                                        MaxLength="50" />
                                </td>
                                <td style="width: 12%; border-style: dotted">
                                    <asp:Label ID="Label42" Text="Language Known :" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 15%; border-style: dotted">
                                    <asp:TextBox ID="txtlanguageknown" CssClass="textboxm" runat="server" Width="95%"
                                        MaxLength="20" />
                                </td>
                            </tr>
                        </table>
                        <fieldset>
                            <legend>
                                <asp:Label Text="Experience :" CssClass="labelbold" ForeColor="Red" runat="server" /></legend>
                            <table border="1" cellspacing="2" width="100%">
                                <tr>
                                    <td style="width: 15%; border-style: dotted">
                                        <asp:Label ID="Label43" Text="Total Experience :" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 85%; border-style: dotted">
                                        <asp:TextBox ID="txttotalexperience" CssClass="textboxm" runat="server" Width="50%"
                                            MaxLength="20" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%" colspan="2">
                                        <div style="width: 100%; overflow: auto; max-height: 300px">
                                            <asp:GridView ID="DGexperience" runat="server" Width="100%" CssClass="grid-views"
                                                AutoGenerateColumns="False" ShowFooter="True">
                                                <HeaderStyle CssClass="gvheaders" />
                                                <AlternatingRowStyle CssClass="gvalts" />
                                                <RowStyle CssClass="gvrow" />
                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr No.">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtsrno" runat="server" Width="100%" /></ItemTemplate>
                                                        <HeaderStyle Width="20px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Name of employer">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtnameofemployer" runat="server" Width="100%" /></ItemTemplate>
                                                        <HeaderStyle Width="200px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Post Held">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtpostheld" runat="server" Width="100%" /></ItemTemplate>
                                                        <HeaderStyle Width="150px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="DOJ">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtdoj" runat="server" Width="100%" /><asp:CalendarExtender ID="caldoj"
                                                                TargetControlID="txtdoj" Format="dd-MMM-yyyy" runat="server">
                                                            </asp:CalendarExtender>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="DOL">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtdol" runat="server" Width="100%" /><asp:CalendarExtender ID="caldol"
                                                                TargetControlID="txtdol" Format="dd-MMM-yyyy" runat="server">
                                                            </asp:CalendarExtender>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="50px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="From &amp;nbsp&amp;nbsp&amp;nbsp&amp;nbsp&amp;nbsp&amp;nbsp&amp;nbsp&amp;nbsp&amp;nbsp&amp;nbsp&amp;nbsp&amp;nbsp&amp;nbsp&amp;nbsp&amp;nbsp To"
                                                        Visible="false">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtfromto" runat="server" Width="100%" /></ItemTemplate>
                                                        <HeaderStyle Width="150px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Reason for Leaving">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtreasonforleaving" runat="server" Width="100%" /></ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Button ID="ButtonAdd" runat="server" Text="Add New Row" CssClass="buttonnorm"
                                                                OnClick="ButtonAdd_Click" CausesValidation="false" /></FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <HeaderStyle Width="200px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tpfamilymembers" HeaderText="Family Members/Witness" runat="server">
                    <ContentTemplate>
                        <table border="1" cellspacing="2" width="100%">
                            <tr>
                                <td>
                                    <asp:Label Text="FAMILY MEMBERS" CssClass="labelbold" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%">
                                    <div style="width: 100%; overflow: auto; max-height: 300px">
                                        <asp:GridView ID="Dgfamilymembers" runat="server" Width="100%" CssClass="grid-views"
                                            AutoGenerateColumns="False" ShowFooter="True" OnRowDataBound="Dgfamilymembers_RowDataBound">
                                            <HeaderStyle CssClass="gvheaders" />
                                            <AlternatingRowStyle CssClass="gvalts" />
                                            <RowStyle CssClass="gvrow" />
                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr No.">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtsrno" runat="server" Width="100%" /></ItemTemplate>
                                                    <HeaderStyle Width="20px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Name">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtfamilymembername" runat="server" Width="100%" /></ItemTemplate>
                                                    <HeaderStyle Width="200px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Address">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtaddressnominee" runat="server" Width="100%" /></ItemTemplate>
                                                    <HeaderStyle Width="150px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Year of Birth">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtyearofbirth" runat="server" Width="100%" /></ItemTemplate>
                                                    <HeaderStyle Width="150px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Relation">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddrelation" CssClass="dropdown" Width="100%" runat="server">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Button ID="ButtonAddfamilymembers" runat="server" Text="Add New Row" CssClass="buttonnorm"
                                                            OnClick="ButtonAddfamilymembers_Click" CausesValidation="false" /></FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle Width="200px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Set As Nominee">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chksetasnominee" Text="" runat="server" /></ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Share % age">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtshare" runat="server" Width="100%" /></ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblrelationid" Text='<%#Bind("relationid") %>' runat="server" /><asp:Label
                                                            ID="lblsetasnominee" runat="server" /></ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <table border="1" cellspacing="2">
                            <tr>
                                <td style="border-style: dotted">
                                    <asp:Label Text="Do you have any relative working in the company/unit ?" CssClass="labelbold"
                                        runat="server" />
                                </td>
                                <td style="border-style: dotted">
                                    <asp:RadioButton Text="Yes" CssClass="radiobutton" ID="rdrelativeworkyes" GroupName="reltwork"
                                        runat="server" /><asp:RadioButton Text="No" CssClass="radiobutton" ID="rdrelativeworkno"
                                            GroupName="reltwork" runat="server" />
                                </td>
                            </tr>
                        </table>
                        <fieldset>
                            <legend>
                                <asp:Label Text="Witness" CssClass="labelbold" ForeColor="Red" runat="server" /></legend>
                            <table border="1" cellspacing="2" width="100%">
                                <tr>
                                    <td style="width: 10%; border-style: dotted">
                                        <asp:Label Text="Name (1) :" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 20%; border-style: dotted">
                                        <asp:TextBox ID="txtwitnessname_1" CssClass="textboxm" runat="server" Width="95%"
                                            MaxLength="50" />
                                    </td>
                                    <td style="width: 10%; border-style: dotted">
                                        <asp:Label ID="Label44" Text="Relationship :" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 20%; border-style: dotted">
                                        <asp:TextBox ID="txtwitnessrelation_1" CssClass="textboxm" runat="server" Width="95%"
                                            MaxLength="30" />
                                    </td>
                                    <td style="width: 10%; border-style: dotted">
                                        <asp:Label ID="Label45" Text="Address :" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 30%; border-style: dotted">
                                        <asp:TextBox ID="txtwitnessaddress_1" CssClass="textboxm" runat="server" Width="95%"
                                            MaxLength="30" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%; border-style: dotted">
                                        <asp:Label ID="Label46" Text="Name (2) :" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 20%; border-style: dotted">
                                        <asp:TextBox ID="txtwitnessname_2" CssClass="textboxm" runat="server" Width="95%"
                                            MaxLength="50" />
                                    </td>
                                    <td style="width: 10%; border-style: dotted">
                                        <asp:Label ID="Label47" Text="Relationship :" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 20%; border-style: dotted">
                                        <asp:TextBox ID="txtwitnessrelationship_2" CssClass="textboxm" runat="server" Width="95%"
                                            MaxLength="30" />
                                    </td>
                                    <td style="width: 10%; border-style: dotted">
                                        <asp:Label ID="Label48" Text="Address :" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 30%; border-style: dotted">
                                        <asp:TextBox ID="txtwitnessaddress_2" CssClass="textboxm" runat="server" Width="95%"
                                            MaxLength="30" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tpjoiningsalary" HeaderText="Joining/Salary information" runat="server">
                    <ContentTemplate>
                        <table border="1" cellspacing="2" width="100%">
                            <tr>
                                <td style="width: 30%" valign="top">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 40%">
                                                <asp:Label ID="Label115" Text="Employee Group :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 60%">
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 80%">
                                                            <asp:DropDownList ID="ddempgroup" CssClass="dropdown" runat="server" Width="100%">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnaddgroup" runat="server" CssClass="buttonnorm" OnClientClick="Addgroupmaster();"
                                                                Text="+" ToolTip="Add New Group" />
                                                            <asp:Button ID="btnrefreshgrouponBiodata" runat="server" Style="display: none" OnClick="btnrefreshgrouponBiodata_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 40%">
                                                <asp:Label Text="Appointment Date :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 60%">
                                                <asp:TextBox ID="txtappointmentdate" CssClass="textboxm" runat="server" Width="95%" /><asp:CalendarExtender
                                                    ID="calappointment" TargetControlID="txtappointmentdate" runat="server" Format="dd-MMM-yyyy"
                                                    Enabled="True">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 40%">
                                                <asp:Label ID="Label49" Text="Place :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 60%">
                                                <asp:TextBox ID="txtplace" CssClass="textboxm" runat="server" Width="95%" MaxLength="50" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 40%">
                                                <asp:Label ID="Label50" Text="Interview Date :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 60%">
                                                <asp:TextBox ID="txtinterviewdate" CssClass="textboxm" runat="server" Width="95%" /><asp:CalendarExtender
                                                    ID="calinterview" TargetControlID="txtinterviewdate" runat="server" Format="dd-MMM-yyyy"
                                                    Enabled="True">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 40%">
                                                <asp:Label ID="Label51" Text="Interview By :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 60%">
                                                <asp:TextBox ID="txtinterviewby" CssClass="textboxm" runat="server" Width="95%" MaxLength="50" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 40%">
                                                <asp:Label ID="Label52" Text="Date of Joining :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 60%">
                                                <asp:TextBox ID="txtdateofjoining" CssClass="textboxm" runat="server" AutoPostBack="True"
                                                    Width="95%" OnTextChanged="txtdateofjoining_TextChanged" /><asp:CalendarExtender
                                                        ID="caldateofjoining" TargetControlID="txtdateofjoining" runat="server" Format="dd-MMM-yyyy"
                                                        Enabled="True">
                                                    </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 40%">
                                                <asp:Label ID="Label53" Text="Confirm Date :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 60%">
                                                <asp:TextBox ID="txtconfirmdate" CssClass="textboxm" runat="server" Width="95%" /><asp:CalendarExtender
                                                    ID="calconfirm" TargetControlID="txtconfirmdate" runat="server" Format="dd/MM/yyyy"
                                                    Enabled="True">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 40%">
                                                <asp:Label ID="Label100" Text="Minimum Wages Date :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 60%">
                                                <asp:TextBox ID="txtminimumwagesdate" CssClass="textboxm" runat="server" Width="95%" /><asp:CalendarExtender
                                                    ID="CalendarExtender1" TargetControlID="txtminimumwagesdate" runat="server" Format="dd/MM/yyyy"
                                                    Enabled="True">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <fieldset>
                                                    <legend>
                                                        <asp:Label Text="Retirement/Resignation" CssClass="labelbold" runat="server" /></legend>
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="width: 40%">
                                                                <asp:Label ID="Label114" Text="Date of Leaving :" CssClass="labelbold" runat="server" />
                                                            </td>
                                                            <td style="width: 60%">
                                                                <asp:TextBox ID="txtresigndate" CssClass="textboxm" runat="server" Width="95%" /><asp:CalendarExtender
                                                                    ID="CalendarExtender2" TargetControlID="txtresigndate" runat="server" Format="dd/MM/yyyy"
                                                                    Enabled="True">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100%" colspan="2">
                                                                <asp:Label ID="Label116" Text="Remarks :" CssClass="labelbold" runat="server" /><br />
                                                                <asp:TextBox ID="txtreasonremarks" CssClass="textboxm" TextMode="MultiLine" runat="server"
                                                                    Width="95%" Height="35px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 30%" valign="top">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 60%; text-align: right">
                                                <asp:Label ID="Label54" Text="Individual Bio-Data :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 40%">
                                                <asp:DropDownList ID="ddindividualdate" CssClass="dropdown" runat="server">
                                                    <asp:ListItem Text="Yes" />
                                                    <asp:ListItem Text="No" />
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 60%; text-align: right">
                                                <asp:Label ID="Label55" Text="Photo :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 40%">
                                                <asp:DropDownList ID="ddphoto" CssClass="dropdown" runat="server">
                                                    <asp:ListItem Text="Yes" />
                                                    <asp:ListItem Text="No" />
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 60%; text-align: right">
                                                <asp:Label ID="Label56" Text="Application for Employment :" CssClass="labelbold"
                                                    runat="server" />
                                            </td>
                                            <td style="width: 40%">
                                                <asp:DropDownList ID="ddapplicationforemployment" CssClass="dropdown" runat="server">
                                                    <asp:ListItem Text="Yes" />
                                                    <asp:ListItem Text="No" />
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 60%; text-align: right">
                                                <asp:Label ID="Label57" Text="Proof of Age :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 40%">
                                                <asp:DropDownList ID="ddproofofage" CssClass="dropdown" runat="server">
                                                    <asp:ListItem Text="Yes" />
                                                    <asp:ListItem Text="No" />
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 60%; text-align: right">
                                                <asp:Label ID="Label58" Text="Proof Name :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 40%">
                                                <asp:TextBox ID="txtproofname" Width="95%" CssClass="textboxm" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 60%; text-align: right">
                                                <asp:Label ID="Label59" Text="Certificates/Testimonials :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 40%">
                                                <asp:DropDownList ID="ddcertificates_testimonials" CssClass="dropdown" runat="server">
                                                    <asp:ListItem Text="Yes" />
                                                    <asp:ListItem Text="No" />
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 60%; text-align: right">
                                                <asp:Label ID="Label60" Text="Contract of Employment :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 40%">
                                                <asp:DropDownList ID="ddcontractofemployment" CssClass="dropdown" runat="server">
                                                    <asp:ListItem Text="Yes" />
                                                    <asp:ListItem Text="No" />
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 60%; text-align: right">
                                                <asp:Label ID="Label61" Text="Joining Report :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 40%">
                                                <asp:DropDownList ID="ddjoiningreport" CssClass="dropdown" runat="server">
                                                    <asp:ListItem Text="Yes" />
                                                    <asp:ListItem Text="No" />
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 60%; text-align: right">
                                                <asp:Label ID="Label62" Text="Nomination Form :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 40%">
                                                <asp:DropDownList ID="ddnominationform" CssClass="dropdown" runat="server">
                                                    <asp:ListItem Text="Yes" />
                                                    <asp:ListItem Text="No" />
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table width="100%" border="1" cellspacing="2">
                                                    <tr>
                                                        <td style="border-style: dotted">
                                                            <asp:Label Text="Employee Type :" CssClass="labelbold" runat="server" />
                                                        </td>
                                                        <td style="border-style: dotted">
                                                            <asp:RadioButton ID="rdpermanent" Text="Permanent" CssClass="radiobutton" runat="server"
                                                                GroupName="PermanentCasual" />
                                                            <asp:RadioButton ID="rdcasual" Text="Casual" CssClass="radiobutton" runat="server"
                                                                GroupName="PermanentCasual" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="border-style: dotted;">
                                                            <asp:Label ID="Label63" Text="Wages Calculation :" CssClass="labelbold" runat="server" />
                                                        </td>
                                                        <td style="border-style: dotted; width: 60%">
                                                            <asp:RadioButton ID="rdmonthly" Text="Monthly" CssClass="radiobutton" GroupName="wages"
                                                                runat="server" />
                                                            <asp:RadioButton ID="rddaily" Text="Daily" CssClass="radiobutton" runat="server"
                                                                GroupName="wages" />
                                                            <asp:RadioButton ID="rdpcswise" Text="PcsWise" CssClass="radiobutton" runat="server"
                                                                GroupName="wages" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="border-style: dotted">
                                                            <asp:Label ID="Label64" Text="Payment Type :" CssClass="labelbold" runat="server" />
                                                        </td>
                                                        <td style="border-style: dotted">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:RadioButton ID="rdcash" Text="Cash" CssClass="radiobutton" GroupName="paymenttype"
                                                                            runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rdcheque" Text="Cheque" CssClass="radiobutton" runat="server"
                                                                            GroupName="paymenttype" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButton ID="rdbank" Text="Bank" CssClass="radiobutton" runat="server" GroupName="paymenttype" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="border-style: dotted">
                                                            <asp:Label ID="Label65" Text="Overtime :" CssClass="labelbold" runat="server" />
                                                        </td>
                                                        <td style="border-style: dotted">
                                                            <asp:RadioButton ID="rdenableovertime" Text="Enable" CssClass="radiobutton" GroupName="overtime"
                                                                runat="server" /><asp:RadioButton ID="rddisableovertime" Text="Disable" CssClass="radiobutton"
                                                                    runat="server" GroupName="overtime" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="border-style: dotted">
                                                            <asp:Label ID="Label66" Text="Fooding :" CssClass="labelbold" runat="server" />
                                                        </td>
                                                        <td style="border-style: dotted">
                                                            <asp:RadioButton ID="rdenablefooding" Text="Enable" CssClass="radiobutton" GroupName="fooding"
                                                                runat="server" /><asp:RadioButton ID="rddisbalefooding" Text="Disable" CssClass="radiobutton"
                                                                    runat="server" GroupName="fooding" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="border-style: dotted">
                                                            <asp:Label ID="Label67" Text="Sun/HD Pay :" CssClass="labelbold" runat="server" />
                                                        </td>
                                                        <td style="border-style: dotted">
                                                            <asp:RadioButton ID="rdenablesunhdpay" Text="Enable" CssClass="radiobutton" GroupName="sunhdpay"
                                                                runat="server" /><asp:RadioButton ID="rddisablesunhdpay" Text="Disable" CssClass="radiobutton"
                                                                    runat="server" GroupName="sunhdpay" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 40%" valign="top">
                                    <table border="1" cellspacing="2" width="100%">
                                        <tr>
                                            <td style="width: 26%; border-style: dotted">
                                                <asp:Label Text="Minimum Wages :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 10%; border-style: dotted">
                                                <asp:Label ID="Label88" Text="Basic :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 30%; border-style: dotted">
                                                <asp:TextBox ID="txtbasic" CssClass="textboxm" runat="server" Width="95%" />
                                            </td>
                                            <td style="width: 10%; border-style: dotted">
                                                <asp:Label ID="Label89" Text="DA :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 20%; border-style: dotted">
                                                <asp:TextBox ID="txtda" CssClass="textboxm" runat="server" Width="95%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%; border-style: dotted">
                                                <asp:Label ID="Label90" Text="Payroll Type :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td colspan="4">
                                                <asp:DropDownList ID="DDpayrolltype" CssClass="dropdown" Width="95%" runat="server"
                                                    AutoPostBack="True" OnSelectedIndexChanged="DDpayrolltype_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="border-style: dotted">
                                                <asp:Label ID="Label96" Text="Basic_Pay" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td colspan="3" style="border-style: dotted">
                                                <asp:TextBox ID="txtbasicpay" CssClass="textboxm" runat="server" AutoPostBack="True"
                                                    onkeypress="return isNumberKey(event);" OnTextChanged="txtbasicpay_TextChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="1" cellspacing="2" width="100%">
                                        <tr>
                                            <td style="width: 100%">
                                                <fieldset>
                                                    <legend>
                                                        <asp:Label ID="lblallowances" Text="Allowances" CssClass="labelbold" ForeColor="Red"
                                                            runat="server" /></legend>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <div style="max-height: 200px; overflow: auto">
                                                                    <asp:GridView ID="DGallowances" runat="server" AutoGenerateColumns="False" CssClass="grid-views"
                                                                        EmptyDataText="No records found.." OnRowDataBound="DGallowances_RowDataBound">
                                                                        <HeaderStyle CssClass="gvheaders" />
                                                                        <AlternatingRowStyle CssClass="gvalts" />
                                                                        <RowStyle CssClass="gvrow" />
                                                                        <PagerStyle CssClass="PagerStyle" />
                                                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Allowance Title">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblallowancetitle" Text='<%#Bind("ParameterName") %>' runat="server" /></ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Allowance Type">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblallowancetype" Text='<%#Bind("Allowance_deductiontype") %>' runat="server" /></ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Amount">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtallowanceamount" Text='<%#Bind("AMount") %>' Width="70px" runat="server"
                                                                                        AutoPostBack="true" OnTextChanged="txtallowanceamount_TextChanged" /></ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField Visible="False">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblallowanceid" Text='<%#Bind("ParameterId") %>' runat="server" /><asp:Label
                                                                                        ID="lblallowancemaxcapingamt" Text='<%#Bind("Maxcapingamount") %>' runat="server" /><asp:Label
                                                                                            ID="lblallowancemincapingamt" Text='<%#Bind("Mincapingamount") %>' runat="server" /><asp:Label
                                                                                                ID="lblallowance_type" Text='<%#Bind("Allowance_Type") %>' runat="server" /><asp:Label
                                                                                                    ID="lblallowancepercent_amount" Text='<%#Bind("percent_amount") %>' runat="server" /><asp:Label
                                                                                                        ID="lbltaxableallowance" Text='<%#Bind("Taxable") %>' runat="server" /><asp:Label
                                                                                                            ID="lblAllowance_Deduction_Id" Text='<%#Bind("Allowance_Deduction_Id") %>' runat="server" /></ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%">
                                                <fieldset>
                                                    <legend>
                                                        <asp:Label ID="Label94" Text="Deductions" CssClass="labelbold" ForeColor="Red" runat="server" /></legend>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <div style="max-height: 200px; overflow: auto">
                                                                    <asp:GridView ID="DGDeductions" runat="server" AutoGenerateColumns="False" CssClass="grid-views"
                                                                        EmptyDataText="No records found.." OnRowDataBound="DGDeductions_RowDataBound">
                                                                        <HeaderStyle CssClass="gvheaders" />
                                                                        <AlternatingRowStyle CssClass="gvalts" />
                                                                        <RowStyle CssClass="gvrow" />
                                                                        <PagerStyle CssClass="PagerStyle" />
                                                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Deduction Title">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbldeductiontitle" Text='<%#Bind("ParameterName") %>' runat="server" /></ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Deduction Type">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbldeductiontype" Text='<%#Bind("Allowance_deductiontype") %>' runat="server" /></ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Amount">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtdeductionamount" Text='<%#Bind("AMount") %>' Width="70px" runat="server"
                                                                                        AutoPostBack="true" OnTextChanged="txtdeductionamount_TextChanged" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField Visible="False">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbldeductionid" Text='<%#Bind("ParameterId") %>' runat="server" /><asp:Label
                                                                                        ID="lbldeductionmaxcapingamt" Text='<%#Bind("Maxcapingamount") %>' runat="server" /><asp:Label
                                                                                            ID="lbldeductionmincapingamt" Text='<%#Bind("Mincapingamount") %>' runat="server" /><asp:Label
                                                                                                ID="lbldeduction_type" Text='<%#Bind("Allowance_Type") %>' runat="server" /><asp:Label
                                                                                                    ID="lbldeductionpercent_amount" Text='<%#Bind("percent_amount") %>' runat="server" /><asp:Label
                                                                                                        ID="lbldeductiontaxable" Text='<%#Bind("Taxable") %>' runat="server" /><asp:Label
                                                                                                            ID="lbldeductionmastertypeid" Text='<%#Bind("Allowance_Deduction_Id") %>' runat="server" /></ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="1" cellspacing="2" width="100%">
                                        <tr>
                                            <td style="width: 20%">
                                                <asp:Label ID="lblgrosssal" runat="server" CssClass="labelbold" Text="Gross salary"></asp:Label>
                                            </td>
                                            <td style="width: 30%">
                                                <asp:TextBox ID="txtgrosssal" runat="server" BackColor="LightYellow" CssClass="textboxm"
                                                    Enabled="False" Width="95%"></asp:TextBox>
                                            </td>
                                            <td style="width: 20%">
                                                <asp:Label ID="Label95" runat="server" CssClass="labelbold" Text="Net. salary"></asp:Label>
                                            </td>
                                            <td style="width: 30%">
                                                <asp:TextBox ID="txtnetsal" runat="server" BackColor="LightYellow" CssClass="textboxm"
                                                    Enabled="False" Width="95%"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tpstatutory" HeaderText="Statutory" runat="server">
                    <ContentTemplate>
                        <table border="1" cellspacing="2" width="100%">
                            <tr>
                                <td style="width: 50%">
                                    <table width="100%">
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="Label75" Text="FORM 1 - ESI :" CssClass="labelbold" Font-Underline="true"
                                                    runat="server" /><asp:RadioButton ID="rdesiyes" GroupName="form1" Text="Yes" CssClass="radiobutton"
                                                        runat="server" /><asp:RadioButton ID="rdesino" GroupName="form1" Text="No" CssClass="radiobutton"
                                                            runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <asp:Label ID="Label68" Text="ESI Insurance No. :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 75%">
                                                <asp:TextBox ID="txtesiinsuranceno" CssClass="textboxm" runat="server" Width="95%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <asp:Label ID="Label69" Text="ESI Employer Code :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 75%">
                                                <asp:TextBox ID="txtesiemployercode" CssClass="textboxm" runat="server" Width="95%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <asp:Label ID="Label70" Text="Dispensary :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 75%">
                                                <asp:TextBox ID="txtesidispensary" CssClass="textboxm" runat="server" Width="95%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <asp:Label ID="Label71" Text="Local Office :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 75%">
                                                <asp:TextBox ID="txtesilocaloffice" CssClass="textboxm" runat="server" Width="95%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <asp:Label ID="Label72" Text="ESI Nominee for Payment :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 75%">
                                                <asp:TextBox ID="txtesinomineeforpayment" CssClass="textboxm" runat="server" Width="95%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <asp:Label ID="Label73" Text="Particulars of Family :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 75%">
                                                <asp:TextBox ID="txtesiparticularsoffamily" CssClass="textboxm" runat="server" Width="95%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <asp:Label ID="Label74" Text="Family member residing With insured person :" CssClass="labelbold"
                                                    runat="server" />
                                            </td>
                                            <td style="width: 75%">
                                                <asp:TextBox ID="txtesifamilymemberresidingwithinsuredperson" CssClass="textboxm"
                                                    runat="server" Width="95%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 50%" valign="top">
                                    <table width="100%">
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="Label76" Text="FORM 2 - PF :" CssClass="labelbold" Font-Underline="true"
                                                    runat="server" /><asp:RadioButton ID="rdpfyes" GroupName="form2" Text="Yes" CssClass="radiobutton"
                                                        runat="server" /><asp:RadioButton ID="rdpfno" GroupName="form2" Text="No" CssClass="radiobutton"
                                                            runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <asp:Label ID="Label77" Text="PF Account No. :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 75%">
                                                <asp:TextBox ID="txtpfaccountno" CssClass="textboxm" runat="server" Width="95%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <asp:Label ID="Label78" Text="Nominee/Nominees :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 75%">
                                                <asp:TextBox ID="txtpfnominee_nominees" CssClass="textboxm" runat="server" Width="95%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 30%">
                                                <asp:Label ID="Label79" Text="Share %age of Nominee :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 75%">
                                                <asp:TextBox ID="txtpfsharepercentageofnominee" CssClass="textboxm" runat="server"
                                                    Width="95%" Enabled="false" BackColor="LightYellow" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <asp:Label ID="Label80" Text="Children Pension :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 75%">
                                                <asp:DropDownList ID="ddchildrenpension" CssClass="dropdown" runat="server">
                                                    <asp:ListItem Text="No"></asp:ListItem>
                                                    <asp:ListItem Text="Yes"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <asp:Label ID="Label81" Text="Widow Pension :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 75%">
                                                <asp:DropDownList ID="ddwidowpension" CssClass="dropdown" runat="server">
                                                    <asp:ListItem Text="No"></asp:ListItem>
                                                    <asp:ListItem Text="Yes"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table border="1" cellspacing="2" width="100%">
                            <tr>
                                <td style="width: 50%" valign="top">
                                    <table width="100%">
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="Label82" Text="FORM F - Gratuity :" CssClass="labelbold" Font-Underline="true"
                                                    runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <asp:Label ID="Label83" Text="Nominee's for Gratuity :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 75%">
                                                <asp:TextBox ID="txtnomineeforgratuity" Enabled="false" BackColor="LightYellow" CssClass="textboxm"
                                                    runat="server" Width="95%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 30%">
                                                <asp:Label ID="Label93" Text="Share %age of Nominee :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 75%">
                                                <asp:TextBox ID="txtgratuitysharepercentageofnominee" Enabled="false" BackColor="LightYellow"
                                                    CssClass="textboxm" runat="server" Width="95%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 50%" valign="top">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 25%">
                                                <asp:Label ID="Label91" Text="PF UAN No. :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 75%">
                                                <asp:TextBox ID="txtpfuanno" CssClass="textboxm" runat="server" Width="95%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <asp:Label ID="Label92" Text="IFSC Code :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 75%">
                                                <asp:TextBox ID="txtifsccode" CssClass="textboxm" runat="server" Width="95%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <asp:Label ID="Label117" Text="Account holder Name :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 75%">
                                                <asp:TextBox ID="txtaccountholdername" CssClass="textboxm" runat="server" Width="95%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <asp:Label ID="Label84" Text="Bank Name :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 75%">
                                                <asp:TextBox ID="txtbankname" CssClass="textboxm" runat="server" Width="95%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <asp:Label ID="Label85" Text="Branch :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 75%">
                                                <asp:TextBox ID="txtbranch" CssClass="textboxm" runat="server" Width="95%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <asp:Label ID="Label86" Text="Address :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 75%">
                                                <asp:TextBox ID="txtbankaddress" CssClass="textboxm" runat="server" Width="95%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <asp:Label ID="Label87" Text="A/c No. :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 75%">
                                                <asp:TextBox ID="txtacno" CssClass="textboxm" runat="server" Width="95%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tppreviousemployement" HeaderText="Previous Employment Verification"
                    runat="server">
                    <ContentTemplate>
                        <table border="1" cellpadding="2" width="100%">
                            <tr>
                                <td style="border-style: dotted" colspan="2">
                                    <asp:Label ID="lblverified" Text="Verified the above details with the done company employee on telephone :"
                                        CssClass="labelbold" runat="server" ForeColor="#E65A5A" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%; border-style: dotted">
                                    <asp:Label ID="lblname" Text="Name :" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%; border-style: dotted">
                                    <asp:TextBox ID="txtname_verifiedby" BackColor="LightYellow" CssClass="textboxm"
                                        Width="95%" runat="server" MaxLength="50" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%; border-style: dotted">
                                    <asp:Label ID="Label101" Text="Mobile No. :" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%; border-style: dotted">
                                    <asp:TextBox ID="txtmobileno_verifiedby" BackColor="LightYellow" CssClass="textboxm"
                                        Width="95%" runat="server" MaxLength="20" />
                                </td>
                            </tr>
                            <tr>
                                <td style="border-style: dotted" colspan="2">
                                    <asp:Label ID="Label102" Text="Reference Verification :" ForeColor="#E65A5A" CssClass="labelbold"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%; border-style: dotted">
                                    <asp:Label ID="Label103" Text="1.  Name :" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%; border-style: dotted">
                                    <asp:TextBox ID="txtname1_verification" BackColor="LightYellow" CssClass="textboxm"
                                        Width="95%" runat="server" MaxLength="50" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%; border-style: dotted">
                                    <asp:Label ID="Label104" Text="Mobile No. :" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%; border-style: dotted">
                                    <asp:TextBox ID="txtmobileno1_verification" BackColor="LightYellow" CssClass="textboxm"
                                        Width="95%" runat="server" MaxLength="20" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%; border-style: dotted">
                                    <asp:Label ID="Label105" Text="2.  Name :" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%; border-style: dotted">
                                    <asp:TextBox ID="txtname2_verification" BackColor="LightYellow" CssClass="textboxm"
                                        Width="95%" runat="server" MaxLength="50" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%; border-style: dotted">
                                    <asp:Label ID="Label106" Text="Mobile No. :" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%; border-style: dotted">
                                    <asp:TextBox ID="txtmobileno2_verification" BackColor="LightYellow" CssClass="textboxm"
                                        Width="95%" runat="server" MaxLength="20" />
                                </td>
                            </tr>
                            <tr>
                                <td style="border-style: dotted; width: 100%" colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 85%">
                                                <asp:Label ID="Label107" Text="Character / Criminal Background Verification through :"
                                                    ForeColor="#E65A5A" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 15%;">
                                                <table width="100%" border="1" cellpadding="2">
                                                    <tr>
                                                        <td style="text-align: center; border-style: dotted; width: 40%">
                                                            <asp:DropDownList ID="DDcriminalbackground" CssClass="dropdown" Width="100%" runat="server">
                                                                <asp:ListItem Text="OK" />
                                                                <asp:ListItem Text="NOT OK" />
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="border-style: dotted; width: 100%" colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 85%">
                                                <table border="1" cellpadding="2" width="100%">
                                                    <tr>
                                                        <td style="width: 30%; border-style: dotted">
                                                            <asp:Label ID="Label109" Text="1.  Neighbour Name :" CssClass="labelbold" runat="server" />
                                                        </td>
                                                        <td style="width: 70%; border-style: dotted">
                                                            <asp:TextBox ID="txtneighbourname" BackColor="LightYellow" CssClass="textboxm" Width="95%"
                                                                runat="server" MaxLength="50" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 30%; border-style: dotted">
                                                            <asp:Label ID="Label112" Text="Mobile No. :" CssClass="labelbold" runat="server" />
                                                        </td>
                                                        <td style="width: 70%; border-style: dotted">
                                                            <asp:TextBox ID="txtmobileno_neighbour" BackColor="LightYellow" CssClass="textboxm"
                                                                Width="95%" runat="server" MaxLength="20" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 30%; border-style: dotted">
                                                            <asp:Label ID="Label113" Text="2. Graam Pradhan/Member :" CssClass="labelbold" runat="server" />
                                                        </td>
                                                        <td style="width: 70%; border-style: dotted">
                                                            <asp:TextBox ID="txtgraampradhan_memeber" BackColor="LightYellow" CssClass="textboxm"
                                                                Width="95%" runat="server" MaxLength="50" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 30%; border-style: dotted">
                                                            <asp:Label ID="Label108" Text="Verification Done By :" CssClass="labelbold" runat="server" />
                                                        </td>
                                                        <td style="width: 70%; border-style: dotted">
                                                            <asp:TextBox ID="txtverificationdoneby" BackColor="LightYellow" CssClass="textboxm"
                                                                Width="95%" runat="server" MaxLength="50" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 30%; border-style: dotted">
                                                            <asp:Label ID="Label110" Text="Designation :" CssClass="labelbold" runat="server" />
                                                        </td>
                                                        <td style="width: 70%; border-style: dotted">
                                                            <asp:TextBox ID="txtdesignation_verification" BackColor="LightYellow" CssClass="textboxm"
                                                                Width="95%" runat="server" MaxLength="50" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 30%; border-style: dotted">
                                                            <asp:Label ID="Label111" Text="Date :" CssClass="labelbold" runat="server" />
                                                        </td>
                                                        <td style="width: 70%; border-style: dotted">
                                                            <asp:TextBox ID="txtdate_verification" BackColor="LightYellow" CssClass="textboxm"
                                                                Width="20%" runat="server" /><asp:CalendarExtender ID="caldateverifi" TargetControlID="txtdate_verification"
                                                                    runat="server" Format="dd-MMM-yyyy" Enabled="True">
                                                                </asp:CalendarExtender>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tpphoto" HeaderText="Photo upload" runat="server">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td style="width: 50%">
                                    <fieldset>
                                        <legend>
                                            <asp:Label ID="Label97" Text="Signature Upload" CssClass="labelbold" ForeColor="Red"
                                                runat="server" /></legend>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Image ID="lblsignatureimage" runat="server" Height="150px" Width="170px" />
                                                </td>
                                                <td>
                                                    <asp:FileUpload ID="fileuploadsignature" ViewStateMode="Enabled" runat="server" /><asp:RegularExpressionValidator
                                                        ID="RegularExpressionValidator1" runat="server" ErrorMessage="Only .gif,.jpeg or.jpg files are allowed!"
                                                        ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|BMP|bmp)$" ControlToValidate="fileuploadsignature"></asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                                <td style="width: 50%">
                                    <fieldset>
                                        <legend>
                                            <asp:Label ID="Label98" Text="Photo Upload" CssClass="labelbold" ForeColor="Red"
                                                runat="server" /></legend>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Image ID="lblphotoimage" runat="server" Height="150px" Width="170px" />
                                                </td>
                                                <td>
                                                    <asp:FileUpload ID="fileuploadphoto" ViewStateMode="Enabled" runat="server" /><asp:RegularExpressionValidator
                                                        ID="RegularExpressionValidator2" runat="server" ErrorMessage="Only .gif,.jpeg or.jpg files are allowed!"
                                                        ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|BMP|bmp)$" ControlToValidate="fileuploadphoto"></asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
            <table cellspacing="2" width="100%">
                <tr>
                    <td style="text-align: right">
                        <asp:Button ID="btnsave" CssClass="buttonnorm" Text="Save" runat="server" OnClick="btnsave_Click" />
                        <asp:Button ID="btnclose" CssClass="buttonnorm" Text="Close" runat="server" OnClientClick="return CloseForm();" />
                        <asp:Button ID="btnnew" CssClass="buttonnorm" Text="New" runat="server" OnClientClick="return ClickNew();" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hnempid" runat="server" Value="0" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnsave" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
