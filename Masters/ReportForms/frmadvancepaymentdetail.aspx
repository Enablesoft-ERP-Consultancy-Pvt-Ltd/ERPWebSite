<%@ Page Title="Advance Payment Detail" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmadvancepaymentdetail.aspx.cs" Inherits="Masters_ReportForms_frmadvancepaymentdetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function validate() {
            if (document.getElementById("<%=DDcompany.ClientID %>").selectedIndex < "0") {
                alert('Plz Select Company...')
                document.getElementById("<%=DDcompany.ClientID %>").focus()
                return false;
            }
            if (document.getElementById("<%=DDjobname.ClientID %>").selectedIndex <= "0") {
                alert('Plz Select Job Name...')
                document.getElementById("<%=DDjobname.ClientID %>").focus()
                return false;
            }
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div style="margin: 1% 20% 0% 30%">
                <table border="0" cellpadding="0" cellspacing="5">
                    <tr>
                        <td>
                            <asp:Label ID="lblcompname" CssClass="labelbold" Text="Company Name" runat="server" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDcompany" CssClass="dropdown" Width="250px" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" CssClass="labelbold" Text="Job Name" runat="server" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDjobname" CssClass="dropdown" Width="250px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDjobname_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label5" CssClass="labelbold" Text="Category Name" runat="server" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDcategory" CssClass="dropdown" Width="250px" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="DDcategory_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label6" CssClass="labelbold" Text="Item Name" runat="server" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDItemname" CssClass="dropdown" Width="250px" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" CssClass="labelbold" Text="EMP. Code" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtempcode" CssClass="textb" Width="250px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label Text="(Type Comma Separated for more than one Emp. Code)" CssClass="labelbold"
                                ForeColor="Red" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" CssClass="labelbold" Text="From Date" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtfromdate" CssClass="textb" Width="100px" runat="server" />
                            <asp:CalendarExtender ID="calfrom" TargetControlID="txtfromdate" runat="server" Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" CssClass="labelbold" Text="To Date" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txttodate" CssClass="textb" Width="100px" runat="server" />
                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txttodate" runat="server"
                                Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="ChkForAdvanceDetail" runat="server" CssClass="checkboxbold" Text=" For Advance Detail" /><br />
                            <asp:CheckBox ID="chkExportForExcel" runat="server" CssClass="checkboxbold" Text=" Export Excel Report"
                                AutoPostBack="true" OnCheckedChanged="chkExportForExcel_CheckedChanged" /><br />
                            <asp:CheckBox ID="chkEmpTransaction" runat="server" CssClass="checkboxbold" Text=" Emp Transaction"
                                Visible="false" AutoPostBack="true" OnCheckedChanged="chkEmpTransaction_CheckedChanged" />
                            <br />
                            <asp:CheckBox ID="chkpaymentdetails" CssClass="checkboxbold" Text="Payment Details"
                                runat="server" Visible="false" AutoPostBack="true" OnCheckedChanged="chkpaymentdetails_CheckedChanged" />
                        </td>
                        <td style="text-align: left">
                            <asp:Button ID="btnpreview" Text="Preview" CssClass="buttonnorm" runat="server" OnClientClick="return validate();"
                                OnClick="btnpreview_Click" />
                            <asp:Button ID="btnPreviewEmpTran" Text="Preview" CssClass="buttonnorm" runat="server"
                                OnClientClick="return validate();" OnClick="btnPreviewEmpTran_Click" Visible="false" />
                            <asp:Button ID="btnclose" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblmsg" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPreviewEmpTran" />
            <asp:PostBackTrigger ControlID="chkExportForExcel" />
        </Triggers>
    </asp:UpdatePanel>
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
                    <asp:TextBox ID="txtpwd" runat="server" TextMode="Password" Height="30px" Width="174px" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td align="right">
                    <asp:Button ID="btnCheck" CommandName="Check" runat="server" Text="Check" CssClass="btnPwd"
                        ValidationGroup="m" OnClick="btnCheck_Click" />
                    <input type="button" value="Cancel" class="btnPwd" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
