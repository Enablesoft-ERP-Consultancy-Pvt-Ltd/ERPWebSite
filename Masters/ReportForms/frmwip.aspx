<%@ Page Title="WIP" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmwip.aspx.cs" Inherits="Masters_ReportForms_frmwip" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
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
    <script runat="server">    
        protected void RadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            if (RDWIP.Checked == true)
            {
                TDProcess.Visible = true;
            }
            else
            {
                TDProcess.Visible = false;
            }
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div style="width: 100%">
                <div style="width: 30%; float: left; height: 300px">
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:Panel Style="width: 100%; border: 1px Solid" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="RDWIP" CssClass="labelbold" Text="WIP" runat="server" GroupName="a"
                                                    Checked="true" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="RDBOMDetail" CssClass="labelbold" GroupName="a" Text="BOM" runat="server"
                                                    AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="RDTNA" CssClass="labelbold" GroupName="a" Text="TNA" runat="server"
                                                    AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="RDWipsummary" CssClass="labelbold" GroupName="a" Text="WIP SUMMARY"
                                                    AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td id="TDProcess" runat="server">
                                <table style="width: 100%">
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="ChkForAllProcess" runat="server" Text=" Chk For All Process" ForeColor="Red"
                                                onclick="return CheckAll('CPH_Form_ChkForAllProcess','CPH_Form_ChLProcess');"
                                                CssClass="checkboxbold" />
                                        </td>
                                    </tr>
                                    <tr id="TRChLProcess" runat="server">
                                        <td rowspan="8" valign="top" style="margin-top: 0px">
                                            <div style="overflow: scroll; height: 190px; width: 100%">
                                                <asp:CheckBoxList ID="ChLProcess" CssClass="checkboxbold" runat="server">
                                                </asp:CheckBoxList>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="width: 70%; float: right">
                    <div style="float: left; width: 100%">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" Text="Company Name" CssClass="labelbold" runat="server" /><br />
                                    <asp:DropDownList ID="DDcompanyName" Width="250px" CssClass="dropdown" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkallcustomer" runat="server" Text=" Chk For All Buyer" ForeColor="Red"
                                        CssClass="checkboxbold" onclick="return CheckAll('CPH_Form_chkallcustomer','CPH_Form_chkcustomer');" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkallOrder" runat="server" Text=" Chk For All Order No" ForeColor="Red"
                                        CssClass="checkboxbold" onclick="return CheckAll('CPH_Form_ChkallOrder','CPH_Form_chkorderno');" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="overflow: scroll; height: 250px; width: 250px">
                                        <asp:CheckBoxList ID="chkcustomer" runat="server" CssClass="checkboxbold">
                                        </asp:CheckBoxList>
                                    </div>
                                </td>
                                <td>
                                    <div style="overflow: scroll; height: 250px; width: 250px">
                                        <asp:CheckBoxList ID="chkorderno" runat="server" CssClass="checkboxbold">
                                        </asp:CheckBoxList>
                                </td>
                    </div>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button Text="Get Orders" ID="btngetorders" CssClass="buttonnorm" runat="server"
                                OnClick="btngetorders_Click" />
                        </td>
                        <td align="right">
                            <asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" Text="Preview" OnClick="BtnPreview_Click" />
                            <asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblmsg" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                        </td>
                    </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
