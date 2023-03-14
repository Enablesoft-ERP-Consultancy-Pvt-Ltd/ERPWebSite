<%@ Page Title="YARN OPENING" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmreportyarnopening.aspx.cs" Inherits="Masters_ReportForms_frmreportyarnopening" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script runat="server">
        protected void RadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            TDsummaryForPayment.Visible = false;
            TDsummary.Visible = false;
            chkdate.Enabled = true;
            chkdate.Checked = false;
            btnPreview.Text = "Preview";
            chkexcelexport.Checked = false;
            TDexcelexport.Visible = false;
            TDStatus.Visible = false;
            if (RDreceive.Checked == true)
            {
                TDsummary.Visible = true;
                TDexcelexport.Visible = true;
            }
            else if (RDissuerecdetail.Checked == true)
            {
                TDsummary.Visible = true;
                TDsummaryForPayment.Visible = true;
                TDStatus.Visible = true;
            }
            else if (RDLoss.Checked == true)
            {
                chkdate.Checked = true;
                chkdate.Enabled = false;
                btnPreview.Text = "Export to Excel";
            }
        }
    </script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
            return false;
        }
        function CheckAll(HObj, Obj) {

            if (document.getElementById(HObj).checked == true) {
                var gvcheck = document.getElementById(Obj);
                var i;
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
            else {
                var gvcheck = document.getElementById(Obj);
                var i;
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top">
                            <asp:Panel runat="server" Style="border: 1px solid; width: 150px">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton Text="Receive" ID="RDreceive" runat="server" CssClass="radiobuttonnormal"
                                                GroupName="a" OnCheckedChanged="RadioButton_CheckedChanged" AutoPostBack="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton Text="Loss" ID="RDLoss" runat="server" CssClass="radiobuttonnormal"
                                                GroupName="a" OnCheckedChanged="RadioButton_CheckedChanged" AutoPostBack="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton Text="Issue_Receive" ID="RDissuerecdetail" runat="server" CssClass="radiobuttonnormal"
                                                GroupName="a" OnCheckedChanged="RadioButton_CheckedChanged" AutoPostBack="true" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td>
                            <div>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblcompany" Text="Company Name" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="250px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" Text="Customer Code" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDcustcode" runat="server" CssClass="dropdown" Width="250px"
                                                OnSelectedIndexChanged="DDcustcode_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" Text="Order No." runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDorderno" runat="server" CssClass="dropdown" Width="250px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="Trdeptname" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label4" Text="Dept. Name" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDdeptname" runat="server" CssClass="dropdown" AutoPostBack="true"
                                                Width="250px" OnSelectedIndexChanged="DDdeptname_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    
                                    <tr id="TDStatus" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label7" Text="Status" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDStatus" CssClass="dropdown" runat="server">
                                                <asp:ListItem Text="ALL" />
                                                <asp:ListItem Text="COMPLETE" />
                                                <asp:ListItem Text="PENDING" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:Label ID="lblemp" Text="Employee Name" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkallemp" runat="server" Text=" Chk For All" ForeColor="Red" CssClass="checkboxbold"
                                                            onclick="return CheckAll('CPH_Form_chkallemp','CPH_Form_chkemp');" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style="overflow: scroll; height: 150px; width: 250px">
                                                            <asp:CheckBoxList ID="chkemp" runat="server" CssClass="checkboxbold">
                                                            </asp:CheckBoxList>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblemplocation" Text="Employee Location" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="Ddemplocation" runat="server" CssClass="dropdown" Width="150px">
                                                <asp:ListItem Text="---Select---" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="InHouse" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="OutSide" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkdate" Text="For Date" runat="server" CssClass="checkboxbold" />
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblfrom" Text="From Date" runat="server" CssClass="labelbold" /><br />
                                                        <asp:TextBox ID="txtfromdate" CssClass="textb" Width="100px" runat="server" />
                                                        <asp:CalendarExtender ID="calfrom" TargetControlID="txtfromdate" runat="server" Format="dd-MMM-yyyy">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label1" Text="To Date" runat="server" CssClass="labelbold" /><br />
                                                        <asp:TextBox ID="txttodate" CssClass="textb" Width="100px" runat="server" />
                                                        <asp:CalendarExtender ID="calto" TargetControlID="txttodate" runat="server" Format="dd-MMM-yyyy">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbllotno1" Text="Lot No" CssClass="labelbold" runat="server" /><br />
                                                        <asp:TextBox ID="txtLotno" Width="100px" runat="server" />
                                                        <asp:AutoCompleteExtender ID="txtLotno_AutoCompleteExtender" runat="server" BehaviorID="SrchAutoComplete"
                                                            CompletionInterval="20" Enabled="True" ServiceMethod="GetLotNo" EnableCaching="true"
                                                            CompletionSetCount="20" ServicePath="~/Autocomplete.asmx" TargetControlID="txtLotno"
                                                            UseContextKey="True" ContextKey="0#0#0" MinimumPrefixLength="2">
                                                        </asp:AutoCompleteExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblTagNo" Text="Tag No" CssClass="labelbold" runat="server" /><br />
                                                        <asp:TextBox ID="txtTagno" Width="100px" runat="server" />
                                                        <asp:AutoCompleteExtender ID="txtTagno_AutoCompleteExtender" runat="server" BehaviorID="SrchAutoComplete1"
                                                            CompletionInterval="20" Enabled="True" ServiceMethod="GetTagNo" EnableCaching="true"
                                                            CompletionSetCount="20" ServicePath="~/Autocomplete.asmx" TargetControlID="txtTagno"
                                                            UseContextKey="True" ContextKey="0#0#0" MinimumPrefixLength="2">
                                                        </asp:AutoCompleteExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="TDsummary" runat="server">
                                            <asp:CheckBox ID="chksummary" CssClass="checkboxbold" Text="For Summary" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="TDsummaryForPayment" runat="server">
                                            <asp:CheckBox ID="ChkSummaryForPayment" CssClass="checkboxbold" Text="Summary For Payment"
                                                runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="TDexcelexport" runat="server" visible="false">
                                            <asp:CheckBox ID="chkexcelexport" CssClass="checkboxbold" Text="Excel Export" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="2">
                                            <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
