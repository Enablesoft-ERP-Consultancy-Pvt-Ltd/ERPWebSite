<%@ Page Title="Leave Application" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmapplyLeave.aspx.cs" Inherits="Masters_Payroll_frmapplyLeave" %>

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
            var txtdateofapplication = document.getElementById('<%=txtdateofapplication.ClientID %>');
            if (txtdateofapplication.value == "") {
                Message = Message + "Please Enter Date of Application!!\n";
            }
            var hnempid = document.getElementById('<%=hnempid.ClientID %>');
            if (hnempid.value == "0") {
                Message = Message + "Please Enter Valid Emp. Code!!\n";
            }

            var selectedindex = $("#<%=DDleavetype.ClientID %>").attr('selectedIndex');
            if (selectedindex <= 0) {
                Message = Message + "Please select Leave Type!!\n";
            }
            var txtfrom = document.getElementById('<%=txtfrom.ClientID %>');
            var txtto = document.getElementById('<%=txtto.ClientID %>');
            if (txtfrom.value == "") {
                Message = Message + "Please Enter From Date!!\n";
            }
            if (txtto.value == "") {
                Message = Message + "Please Enter To Date!!\n";
            }
            var datefrom = new Date(txtfrom.value);
            var dateto = new Date(txtto.value);
            if (datefrom > dateto) {
                Message = Message + "From Date can not greater than To Date !!\n";
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
                        <fieldset>
                            <legend>
                                <asp:Label Text="Leave Application" CssClass="labelbold" ForeColor="Red" runat="server" />
                            </legend>
                            <table width="100%" border="1">
                                <tr>
                                    <td style="width: 30%; border-style: dotted">
                                        <asp:Label ID="Label3" Text="Date of Application" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 70%; border-style: dotted">
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 50%">
                                                    <asp:TextBox ID="txtdateofapplication" CssClass="textboxm" Width="95%" runat="server"
                                                        autocomplete="off" Enabled="false" />
                                                    <asp:CalendarExtender ID="caldateofapplication" TargetControlID="txtdateofapplication"
                                                        Format="dd-MMM-yyyy" runat="server" PopupButtonID="imgdateofaaplication">
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
                                        <asp:Label Text="Leave Type" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 70%; border-style: dotted">
                                        <asp:DropDownList ID="DDleavetype" CssClass="dropdown" runat="server" Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 30%; border-style: dotted">
                                        <asp:Label ID="Label1" Text="From" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 70%; border-style: dotted">
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 50%">
                                                    <asp:TextBox ID="txtfrom" CssClass="textboxm" Width="95%" runat="server" autocomplete="off"
                                                        Enabled="false" />
                                                    <asp:CalendarExtender ID="calfrom" TargetControlID="txtfrom" Format="dd-MMM-yyyy"
                                                        runat="server" PopupButtonID="imgfrom">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="imgfrom" AlternateText="Click here to display calender" ImageUrl="~/Images/calendar.png"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 20%; border-style: dotted">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 30%; border-style: dotted">
                                        <asp:Label ID="Label2" Text="To" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 70%; border-style: dotted">
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 50%">
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="width: 90%">
                                                                <asp:TextBox ID="txtto" CssClass="textboxm" Width="95%" runat="server" autocomplete="off"
                                                                    OnTextChanged="txtto_TextChanged" AutoPostBack="true" Enabled="false" />
                                                                <asp:CalendarExtender ID="calto" TargetControlID="txtto" Format="dd-MMM-yyyy" runat="server"
                                                                    PopupButtonID="Imgto">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="Imgto" AlternateText="Click here to display calender" ImageUrl="~/Images/calendar.png"
                                                                    runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="width: 30%">
                                                    <asp:Label Text="Duration(in days)" CssClass="labelbold" runat="server" />
                                                </td>
                                                <td style="width: 20%">
                                                    <asp:TextBox ID="txtleavecount" CssClass="textboxm" Width="95%" Enabled="false" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 30%; border-style: dotted">
                                        <asp:Label ID="Label4" Text="Leave Period" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 70%; border-style: dotted">
                                        <asp:RadioButton ID="rdisthalf" Text="Ist Half" CssClass="radiobutton" runat="server"
                                            AutoPostBack="true" GroupName="q" OnCheckedChanged="rdisthalf_CheckedChanged" />
                                        <asp:RadioButton ID="rdiindhalf" Text="2nd Half" CssClass="radiobutton" runat="server"
                                            AutoPostBack="true" GroupName="q" OnCheckedChanged="rdiindhalf_CheckedChanged" />
                                        <asp:RadioButton ID="rdboth" Text="Both" CssClass="radiobutton" runat="server" Checked="true"
                                            AutoPostBack="true" GroupName="q" OnCheckedChanged="rdboth_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 30%; border-style: dotted">
                                        <asp:Label ID="Label5" Text="Reason for Leave" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 70%; border-style: dotted">
                                        <asp:TextBox ID="txtreasonforleave" MaxLength="200" CssClass="textboxm" Width="100%"
                                            TextMode="MultiLine" runat="server" Height="37px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 30%; border-style: dotted">
                                        <asp:Label ID="Label6" Text="Address during Leave" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 70%; border-style: dotted">
                                        <asp:TextBox ID="txtaddressduringleave" MaxLength="100" CssClass="textboxm" Width="100%"
                                            TextMode="MultiLine" runat="server" Height="37px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 30%; border-style: dotted">
                                        <asp:Label ID="Label9" Text="File upload" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 70%; border-style: dotted">
                                        <asp:FileUpload ID="fileupload" runat="server" ViewStateMode="Enabled" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Only .gif,.jpeg or.jpg,.pdf files are allowed!"
                                            ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|BMP|bmp|pdf|PDF)$" ControlToValidate="fileupload"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%; border-style: dotted" colspan="2" align="right">
                                        <asp:Button ID="btnapply" CssClass="buttonnorm" Text="Apply" runat="server" OnClick="btnapply_Click"
                                            OnClientClick="if (!validatesave())return; this.disabled=true;this.value = 'wait ...';"
                                            UseSubmitBehavior="false" />
                                        <asp:Button ID="btnclose" CssClass="buttonnorm" Text="Close" runat="server" OnClientClick="return CloseForm();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%; border-style: dotted" colspan="2">
                                        <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                    <td style="width: 40%" valign="top">
                        <fieldset>
                            <legend>
                                <asp:Label Text="Leave Balance" CssClass="labelbold" ForeColor="Red" runat="server"></asp:Label>
                            </legend>
                            <div style="width: 100%; max-height: 200px; overflow: auto">
                                <asp:GridView ID="DGleavebal" CssClass="grid-views" AutoGenerateColumns="false" runat="server"
                                    Width="100%">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Leave Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblname" Text='<%#Bind("Name") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Carry Forward">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcarryforward" Text='<%#Bind("Carryforward") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Entitlement">
                                            <ItemTemplate>
                                                <asp:Label ID="lblentitlement" Text='<%#Bind("Entitlement") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Availed">
                                            <ItemTemplate>
                                                <asp:Label ID="lblavailed" Text='<%#Bind("Availed") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Approval Pending">
                                            <ItemTemplate>
                                                <asp:Label ID="lblapprovalpending" Text='<%#Bind("Approvalpending") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Balance">
                                            <ItemTemplate>
                                                <asp:Label ID="lblbalance" Text='<%#Bind("Balance") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hnempid" runat="server" Value="0" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnapply" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
