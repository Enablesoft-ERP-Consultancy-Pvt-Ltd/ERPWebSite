<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmrptforwarpmaterialPending.aspx.cs" Inherits="Masters_WARP_frmrptforwarpmaterialPending" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
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
    <div>
        <div style="margin: 0% 20% 0% 20%">
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
                                    <div style="overflow: scroll; height: 200px; width: 250px">
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
                        <asp:CheckBox ID="chksummary" CssClass="checkboxbold" Text="For Summary" runat="server"
                            Visible="false" />
                    </td>
                    <td align="right">
                        <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                        <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
