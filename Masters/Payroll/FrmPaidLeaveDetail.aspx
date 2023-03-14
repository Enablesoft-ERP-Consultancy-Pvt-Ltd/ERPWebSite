<%@ Page Title="Leave Application" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmPaidLeaveDetail.aspx.cs" Inherits="Masters_Payroll_FrmPaidLeaveDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        function KeyDownHandlerWeaverIdscan(event) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                window.document.getElementById('<%=btnsearchemp.ClientID %>').click();
            }
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }

    </script>
    <script type="text/javascript">
        function validatesave() {
            var Message = "";
            var txtdate = document.getElementById('<%=txtdate.ClientID %>');
            if (txtdate.value == "") {
                Message = Message + "Please Enter Date !!\n";
            }
            var hnempid = document.getElementById('<%=hnempid.ClientID %>');
            if (hnempid.value == "0") {
                Message = Message + "Please Enter Valid Emp. Code!!\n";
            }

            var selectedindex = $("#<%=DDleavetype.ClientID %>").attr('selectedIndex');
            if (selectedindex <= 0) {
                Message = Message + "Please select Leave Type!!\n";
            }

            var TxtNoofLeave = document.getElementById('<%=TxtNoofLeave.ClientID %>');

            if (TxtNoofLeave.value == "") {
                Message = Message + "Please Enter no of leave!!\n";
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
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td style="width: 60%;" valign="top">
                        <table width="50%" border="1">
                            <tr>
                                <td style="width: 30%; border-style: dotted">
                                    <asp:Label ID="Label7" Text="Emp. Code" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%; border-style: dotted">
                                    <asp:TextBox ID="txtempcode" placeholder="Type Emp. Code here" CssClass="textboxm"
                                        Width="50%" runat="server" onKeypress="KeyDownHandlerWeaverIdscan(event);" />
                                    <asp:Button ID="btnsearchemp" Text="Emp.Search" Style="display: none" runat="server"
                                        OnClick="btnsearchemp_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%; border-style: dotted">
                                    <asp:Label ID="Label8" Text="Emp. Name" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%; border-style: dotted">
                                    <asp:TextBox ID="txtempname" CssClass="textboxm" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%; border-style: dotted">
                                    <asp:Label ID="Label1" Text="Leave Type" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%; border-style: dotted">
                                    <asp:DropDownList ID="DDleavetype" CssClass="dropdown" runat="server" Width="100%">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%; border-style: dotted">
                                    <asp:Label ID="Label3" Text="Date" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%; border-style: dotted">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 50%">
                                                <asp:TextBox ID="txtdate" CssClass="textboxm" Width="95%" runat="server" autocomplete="off"
                                                    Enabled="false" />
                                                <asp:CalendarExtender ID="caldateofapplication" TargetControlID="txtdate" Format="dd-MMM-yyyy"
                                                    runat="server" PopupButtonID="imgdateofaaplication">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="imgdateofaaplication" AlternateText="Click here to display calender"
                                                    ImageUrl="~/Images/calendar.png" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%; border-style: dotted">
                                    <asp:Label ID="Label2" Text="No of Leave" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%; border-style: dotted">
                                    <asp:TextBox ID="TxtNoofLeave" CssClass="textboxm" Width="50%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%; border-style: dotted">
                                    <asp:Label ID="Label4" Text="Remark" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 70%; border-style: dotted">
                                    <asp:TextBox ID="TxtRemark" CssClass="textboxm" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%; border-style: dotted" colspan="2" align="right">
                                    <asp:Button ID="BtnSave" CssClass="buttonnorm" Text="Save" runat="server" OnClick="BtnSave_Click"
                                        OnClientClick="if (!validatesave())return; this.disabled=true;this.value = 'wait ...';"
                                        UseSubmitBehavior="false" />
                                    <asp:Button ID="BtnPreview" CssClass="buttonnorm" Text="Preview" runat="server" OnClick="BtnPreview_Click" />
                                    <asp:Button ID="btnclose" CssClass="buttonnorm" Text="Close" runat="server" OnClientClick="return CloseForm();" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%; border-style: dotted" colspan="2">
                                    <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hnempid" runat="server" Value="0" />
        </ContentTemplate>
          <Triggers>
            <asp:PostBackTrigger ControlID="BtnPreview" />
        </Triggers>

    </asp:UpdatePanel>
</asp:Content>
