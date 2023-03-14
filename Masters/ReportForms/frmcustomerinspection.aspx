<%@ Page Title="customer inspection" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmcustomerinspection.aspx.cs" Inherits="Masters_ReportForms_frmcustomerinspection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmcustomerinspection.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div style="margin: 0% 20% 0% 20%">
                <table border="1" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="lblcustomer" Text="Customer Code" CssClass="labelbold" runat="server" />
                            <br />
                            <asp:DropDownList ID="DDCustCode" runat="server" Width="250px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDCustCode_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label1" Text="Order No." CssClass="labelbold" runat="server" />
                            <br />
                            <asp:DropDownList ID="DDorderNo" runat="server" Width="250px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDorderNo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Label ID="Label2" Text="Process Name" CssClass="labelbold" runat="server" />
                            <br />
                            <asp:DropDownList ID="DDprocessname" runat="server" Width="250px" CssClass="dropdown"
                                AutoPostBack="True" OnSelectedIndexChanged="DDprocessname_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <fieldset>
                    <legend>
                        <asp:Label Text="Issue Details" runat="server" CssClass="labelbold" ForeColor="Red" />
                    </legend>
                    <table border="1" cellpadding="0" cellspacing="5" width="50%">
                        <tr>
                            <td>
                                <asp:Label Text="Job Start Date :" CssClass="labelbold" ForeColor="DarkOrange" Font-Size="Small"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lbljobissuestartdate" CssClass="labelbold" Text="" runat="server"
                                    Font-Size="Small" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" Text="Job End Date :" CssClass="labelbold" ForeColor="DarkOrange"
                                    Font-Size="Small" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lbljobissueenddate" CssClass="labelbold" Text="" runat="server" Font-Size="Small" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" Text="No of Pcs :" CssClass="labelbold" ForeColor="DarkOrange"
                                    Font-Size="Small" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblissuepcs" CssClass="labelbold" Text="" runat="server" Font-Size="Small" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label5" Text="Issue Pcs as on Date:" CssClass="labelbold" ForeColor="DarkOrange"
                                    Font-Size="Small" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="Label6" CssClass="labelbold" Text="Click to select Date" runat="server"
                                    Font-Size="Small" />
                                <br />
                                <asp:TextBox ID="txtissueasondate" CssClass="textb" Width="110px" runat="server"
                                    OnTextChanged="txtissueasondate_TextChanged" AutoPostBack="true" />
                                <asp:CalendarExtender ID="cal1" TargetControlID="txtissueasondate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" Text="No of Pcs as on Date:" CssClass="labelbold" ForeColor="DarkOrange"
                                    Font-Size="Small" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblnoofpcsissueasondate" Text="" CssClass="labelbold" runat="server"
                                    Font-Size="Small" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label7" Text="Receive Details" runat="server" CssClass="labelbold"
                            ForeColor="Red" />
                    </legend>
                    <table border="1" cellpadding="10" cellspacing="5" width="50%">
                        <tr>
                            <td>
                                <asp:Label ID="Label8" Text="Job Start Date :" CssClass="labelbold" ForeColor="DarkOrange"
                                    Font-Size="Small" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lbljobrecstartdate" CssClass="labelbold" Text="" runat="server" Font-Size="Small" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label10" Text="Job End Date :" CssClass="labelbold" ForeColor="DarkOrange"
                                    Font-Size="Small" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lbljobrecEnddate" CssClass="labelbold" Text="" runat="server" Font-Size="Small" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label12" Text="No of Pcs :" CssClass="labelbold" ForeColor="DarkOrange"
                                    Font-Size="Small" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblRecpcs" CssClass="labelbold" Text="" runat="server" Font-Size="Small" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label14" Text="Receive Pcs as on Date:" CssClass="labelbold" ForeColor="DarkOrange"
                                    Font-Size="Small" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="Label15" CssClass="labelbold" Text="Click to select Date" runat="server"
                                    Font-Size="Small" />
                                <br />
                                <asp:TextBox ID="txtrecasondate" CssClass="textb" Width="110px" runat="server" OnTextChanged="txtrecasondate_TextChanged"
                                    AutoPostBack="true" />
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtrecasondate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label11" Text="No of Pcs as on Date:" CssClass="labelbold" ForeColor="DarkOrange"
                                    Font-Size="Small" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblRecpcsasondate" Text="" CssClass="labelbold" runat="server" Font-Size="Small" />
                            </td>
                        </tr>
                    </table>
                    <table border="0" cellpadding="0" cellspacing="0" width="50%">
                        <tr>
                            <td align="right">
                                <asp:Button ID="btnnew" Text="New" CssClass="buttonnorm" runat="server" OnClientClick="return NewForm();" />
                                <asp:Button ID="btnclose" Text="Close" CssClass="buttonnorm" runat="server" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblmsg" ForeColor="Red" CssClass="labelbold" Text="" runat="server" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
