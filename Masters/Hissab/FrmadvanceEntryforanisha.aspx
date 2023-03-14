<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="FrmadvanceEntryforanisha.aspx.cs" Inherits="Masters_Hissab_Frmadvanceforanisha" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 60) {
                alert("Please Enter Only Numeric Value:");
                return false;
            }
            return true;
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Newform() {
            window.location.href = "FrmadvanceEntryforanisha.aspx";
        }
        function validate() {
            if (document.getElementById("<%=ddjobname.ClientID %>").value <= "0") {
                alert("Plz select job Name");
                return false;
            }
            if (document.getElementById('CPH_Form_ddpartyname') != null) {
                if (document.getElementById('CPH_Form_ddpartyname').options.length == 0) {
                    alert("Party Name must have a Name....!");
                    document.getElementById("CPH_Form_ddpartyname").focus();
                    return false;
                }
                else if (document.getElementById('CPH_Form_ddpartyname').options[document.getElementById('CPH_Form_ddpartyname').selectedIndex].value == 0) {
                    alert("Please select party name....!");
                    document.getElementById("CPH_Form_ddpartyname").focus();
                    return false;
                }

            }
            if (document.getElementById("<%=txtamount.ClientID %>").value <= "0") {
                if (document.getElementById("<%=txtamount.ClientID %>").value == "" || document.getElementById("<%=txtamount.ClientID %>").value == "0") {
                    alert("Please fill the Amount");
                    document.getElementById("<%=txtamount.ClientID %>").focus();
                    return false;
                }
            }

        }
        
    </script>
    <asp:UpdatePanel runat="server" ID="updatepanel">
        <ContentTemplate>
            <br />
            <div style="width: 50%; margin: auto">
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <asp:Label ID="lbljobname" Text="Job Name" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddjobname" runat="server" CssClass="dropdown" Width="75%" OnSelectedIndexChanged="ddjobname_SelectedIndexChanged"
                                AutoPostBack="true" />
                            <asp:RequiredFieldValidator ID="rfv" ErrorMessage="Please, select Job Name!" ControlToValidate="ddjobname"
                                runat="server" CssClass="errormsg" InitialValue="0" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblPartyname" runat="server" CssClass="labelbold" Text="Party Name" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddpartyname" runat="server" CssClass="dropdown" Width="75%"
                                AutoPostBack="true" OnSelectedIndexChanged="ddpartyname_SelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblamount" runat="server" CssClass="labelbold" Text="Amount" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtamount" runat="server" CssClass="textb" Width="108" onkeypress="return isNumberKey(event)" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <tr>
                            <td>
                                <asp:Label ID="lbldate" runat="server" CssClass="labelbold" Text="Date" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtdate" runat="server" CssClass="textb" Width="27%" AutoPostBack="True" />
                                <asp:CalendarExtender ID="calenderdate" runat="server" Format="dd-MMM-yyyy HH:mm:ss"
                                    TargetControlID="txtdate">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                    </tr>
                </table>
                <table style="width: 130%; margin: 2%">
                    <tr>
                        <td align="center" colspan="4">
                            <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" OnClick="btnsave_Click"
                                Text="Save" OnClientClick="return validate()" />
                            <asp:Button ID="btnnew" runat="server" CssClass="buttonnorm" Text="New" ValidationGroup="p"
                                OnClientClick="return Newform()" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm()"
                                Text="Close" ValidationGroup="p" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblmsg" runat="server" ForeColor="red" />
                        </td>
                    </tr>
                </table>
                <table style="width: 110%;">
                    <tr>
                        <td colspan="3" align="center">
                            <div style="overflow: auto; height: 320px">
                                <asp:GridView runat="server" ID="gridv" AutoGenerateColumns="False" Width="98%" Height="90px"
                                    DataKeyNames="ID">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No">
                                            <ItemTemplate>
                                                <%# Container.DisplayIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PROCESS_NAME" HeaderText="JobName" />
                                        <asp:BoundField DataField="EmpName" HeaderText="PartyName" />
                                        <asp:BoundField DataField="AdvanceAmt" HeaderText="Amount">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Date" HeaderText="Date" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
