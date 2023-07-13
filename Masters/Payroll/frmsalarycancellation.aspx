<%@ Page Title="Salary Cancellation" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmsalarycancellation.aspx.cs" Inherits="Masters_Payroll_frmsalarycancellation" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function validatesave() {
            var Message = "";
            var selectedindex = $("#<%=DDmonth.ClientID %>").attr('selectedIndex');
            if (selectedindex < 0) {
                Message = Message + "Please select Month!!\n";
            }
            selectedindex = $("#<%=DDyear.ClientID %>").attr('selectedIndex');
            if (selectedindex < 0) {
                Message = Message + "Please Select year !!\n";
            }
            if (Message == "") {
                return confirm('Do you want to Cancel Salary ?');
            }
            else {
                alert(Message);
                return false;
            }
        }
        function NewForm() {
            window.location.href = "frmsalarycancellation.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }

    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div style="margin: 0% 20% 0% 20%">
                <table border="1" cellspacing="2" width="100%">
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label Text="Company" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:DropDownList ID="DDcompany" CssClass="dropdown" Width="95%" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label10" CssClass="labelbold" Text="Branch" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:DropDownList ID="DDBranchName" CssClass="dropdown" Width="95%" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label1" Text="Department" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:DropDownList ID="DDDepartment" CssClass="dropdown" Width="95%" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label2" Text="Month" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:DropDownList ID="DDmonth" CssClass="dropdown" Width="50%" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label3" Text="Year" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:DropDownList ID="DDyear" Width="50%" CssClass="dropdown" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label6" Text="Wages Calculation" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <asp:DropDownList ID="DDwagescalculation" CssClass="dropdown" runat="server">
                                <asp:ListItem Text="Monthly" Value="1" />
                                <asp:ListItem Text="Daily" Value="2" />
                                <asp:ListItem Text="Pcs Wise" Value="3" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label4" Text="Emp. Code" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 80%; border-style: dotted">
                            <table width="100%">
                                <tr>
                                    <td style="width: 100%">
                                        <asp:TextBox ID="txtempcode" CssClass="textboxm" MaxLength="100" Width="95%" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%">
                                        <asp:Label ID="Label5" Text="For multiple emp. Code use commas(,)eg:0001,0002" CssClass="labelbold"
                                            ForeColor="Red" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; border-style: dotted" colspan="2" align="right">
                            <asp:Button ID="btnprocess" Text="Salary Cancellation" runat="server" CssClass="buttonnorm"
                                OnClick="btnprocess_Click" UseSubmitBehavior="false" OnClientClick="if (!validatesave())return; this.disabled=true;this.value = 'Salary Cancelling wait ...';" />
                            <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            <asp:Button ID="btnnew" Text="New" runat="server" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%" colspan="2">
                            <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
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
                            <asp:Label ID="Label7" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
