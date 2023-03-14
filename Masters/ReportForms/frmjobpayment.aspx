<%@ Page Title="JOB PAYMENT" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmjobpayment.aspx.cs" Inherits="Masters_ReportForms_frmjobpayment" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div style="margin: 2% 20% 0% 20%; height: 100%">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lbljob" Text="Job Name" CssClass="labelbold" runat="server" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDJob" CssClass="dropdown" Width="200px" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" Text="Unit Name" CssClass="labelbold" runat="server" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDUnit" CssClass="dropdown" Width="200px" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" Text="Month" CssClass="labelbold" runat="server" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDMonth" CssClass="dropdown" Width="100px" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" Text="Year" CssClass="labelbold" runat="server" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDYear" CssClass="dropdown" Width="100px" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="btnGenerate" Text="Generate Payment" runat="server" CssClass="buttonnorm"
                                OnClick="btnGenerate_Click" OnClientClick="confirm('Do you want to generate Job payment.')" />
                            <asp:Button ID="btnClose" Text="Close" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblmsg" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
