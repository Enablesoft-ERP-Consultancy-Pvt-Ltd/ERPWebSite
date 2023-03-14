<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmEditadvanceamount.aspx.cs"
    Inherits="Masters_Hissab_frmEditadvanceamount" MasterPageFile="~/ERPmaster.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmEditadvanceamount.aspx";
        }
    </script>
    <asp:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <div style="margin: 1% 20% 0% 20%">
                <table style="width: 60%; background: #DEB887; border: 1px groove Teal;">
                    <tr>
                        <td>
                            <span class="labelbold">Enter ID No.</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtIdNo" runat="server" CssClass="textb" Style="width: 150px;" AutoPostBack="true"
                                OnTextChanged="txtIdNo_TextChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblAdvanceAmt" runat="server" Text="Advance Amount" CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAdvanceAmt" runat="server" CssClass="textb" Width="150px" onkeypress="return isnumeric(event);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Adv. Amount Remark" CssClass="labelbold"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtadvremark" CssClass="textb" runat="server" Width="280px" TextMode="MultiLine"
                                Height="44px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblAdvDate" runat="server" Text="Advance Date" CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAdvancedate" runat="server" Width="100px" CssClass="textb" Height="23px"
                                BackColor="Beige"></asp:TextBox>
                            <asp:CalendarExtender ID="calAdvance" runat="server" TargetControlID="txtAdvancedate"
                                Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Voucher No." CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtvoucherno" runat="server" Width="100px" CssClass="textb" BackColor="Beige"></asp:TextBox>
                        </td>
                    </tr>
                    <tr runat="server" visible="false">
                        <td>
                            <asp:Label ID="lblDate" runat="server" Text="Month" CssClass="labelbold"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDMonth" runat="server" CssClass="dropdown">
                                <asp:ListItem Value="1">JAN</asp:ListItem>
                                <asp:ListItem Value="2">FEB</asp:ListItem>
                                <asp:ListItem Value="3">MAR</asp:ListItem>
                                <asp:ListItem Value="4">APR</asp:ListItem>
                                <asp:ListItem Value="5">MAY</asp:ListItem>
                                <asp:ListItem Value="6">JUN</asp:ListItem>
                                <asp:ListItem Value="7">JUL</asp:ListItem>
                                <asp:ListItem Value="8">AUG</asp:ListItem>
                                <asp:ListItem Value="9">SEP</asp:ListItem>
                                <asp:ListItem Value="10">OCT</asp:ListItem>
                                <asp:ListItem Value="11">NOV</asp:ListItem>
                                <asp:ListItem Value="12">DEC</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lblyear" runat="server" Text="Year" CssClass="labelbold"></asp:Label>
                            <asp:DropDownList ID="DDyear" runat="server" CssClass="dropdown" widh="100px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="TRfromtoDate" runat="server">
                        <td>
                            <asp:Label ID="lblfrom" CssClass="labelbold" Text="From Date" runat="server" />
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtfromdate" CssClass="textb" Width="100px" runat="server" />
                                        <asp:CalendarExtender ID="calfrom" TargetControlID="txtfromdate" Format="dd-MMM-yyyy"
                                            runat="server">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" CssClass="labelbold" Text="To Date" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txttodate" CssClass="textb" Width="100px" runat="server" />
                                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txttodate" Format="dd-MMM-yyyy"
                                            runat="server">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblguarantorname" CssClass="labelbold" Text="" runat="server" ForeColor="Red"
                                Font-Size="Small" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblbal" CssClass="labelbold" Text="" runat="server" ForeColor="Red"
                                Font-Size="Small" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button Text="New" ID="btnnew" CssClass="buttonnorm" runat="server" OnClientClick="return NewForm();" />
                            <asp:Button ID="Btnsubmit" Text="Submit" CssClass="buttonnorm" runat="server" OnClick="Btnsubmit_Click" />
                            <asp:Button ID="btnpreview" Text="Preview" CssClass="buttonnorm" runat="server" OnClick="btnpreview_Click" />
                            <asp:Button ID="btnguarantorwise" Text="Guarantor Wise Preview" CssClass="buttonnorm"
                                runat="server" OnClick="btnguarantorwise_Click" />
                        </td>
                    </tr>
                </table>
                <table style="width: 80%">
                    <tr>
                        <td>
                            <div style="overflow: auto; max-height: 300px">
                                <asp:GridView ID="gvadvance" runat="server" AutoGenerateColumns="False" OnRowDeleting="gvadvance_RowDeleting">
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <HeaderStyle CssClass="gvheaders" />
                                    <RowStyle CssClass="gvrow" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="SrNo.">
                                            <ItemTemplate>
                                                <%#Container.DisplayIndex+1 %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DetailId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDetailId" runat="server" Text='<%#Bind("DetailId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="EmpId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpId" runat="server" Text='<%#Bind("EmpId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="EmpCode">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpCode" runat="server" Text='<%#Bind("EmpCode") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="JobId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblJobid" runat="server" Text='<%#Bind("JobId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Job">
                                            <ItemTemplate>
                                                <asp:Label ID="lblJob" runat="server" Text='<%#Bind("Job") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" runat="server" Text='<%#Bind("Date") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%#Bind("Amount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remark">
                                            <ItemTemplate>
                                                <asp:Label ID="lblremark" runat="server" Text='<%#Bind("Remark") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="DEL" OnClientClick="return confirm('Do You Want To Delete Data?')"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label Text="" ID="lblmsg" CssClass="labelbold" ForeColor="Red" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
