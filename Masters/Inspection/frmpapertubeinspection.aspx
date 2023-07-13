<%@ Page Title="Paper tube inspection" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmpapertubeinspection.aspx.cs" Inherits="Masters_Inspection_frmpapertubeinspection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function ClickNew() {
            window.location.href = "frmpapertubeinspection.aspx";
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkedit" Text="FOR EDIT" CssClass="checkboxbold" AutoPostBack="True"
                            runat="server" OnCheckedChanged="chkedit_CheckedChanged" />
                    </td>
                    <td>
                    </td>
                    <td id="TDSupplierSearch" runat="server" visible="false">
                        <asp:TextBox ID="txtsuppliersearch" Placeholder="Type Supplier here to search Doc No."
                            runat="server" Width="235px" CssClass="textb" />
                        <asp:Button ID="btnsearch" runat="server" Text="Button" Style="display: none;" OnClick="btnsearch_Click" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblcompany" Text="Company Name" CssClass="labelbold" runat="server" />
                    </td>
                    <td>
                        <asp:DropDownList ID="DDcompanyName" CssClass="dropdown" Width="200px" runat="server"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label5" Text="Branch Name" CssClass="labelbold" runat="server" />
                    </td>
                    <td>
                        <asp:DropDownList ID="DDBranchName" CssClass="dropdown" Width="200px" runat="server"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td id="TDDocno" runat="server" visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label38" Text="Doc No." CssClass="labelbold" runat="server" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDDocNo" CssClass="dropdown" Width="200px" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="DDDocNo_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <asp:Label ID="Label33" Text="System Gen. Doc  No." CssClass="labelbold" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtdocno" CssClass="textb" runat="server" Enabled="false" BackColor="LightGray" />
                    </td>
                </tr>
            </table>
            <table border="1" cellspacing="2" style="width: 100%">
                <tr>
                    <td style="width: 5%; border-style: dotted">
                        <asp:Label Text="Date :" CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 10%; border-style: dotted">
                        <asp:TextBox ID="txtdate" CssClass="textb" Width="95%" runat="server" />
                        <asp:CalendarExtender ID="caldate" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtdate">
                        </asp:CalendarExtender>
                    </td>
                    <td style="width: 10%; border-style: dotted">
                        <asp:Label ID="Label1" Text="Standared :" CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 50%; border-style: dotted">
                        <asp:TextBox ID="txtstandared" CssClass="textb" Width="95%" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%; border-style: dotted">
                        <asp:Label ID="Label2" Text="Supplier Name :" CssClass="labelbold" runat="server" />
                    </td>
                    <td colspan="3" style="width: 50%; border-style: dotted">
                        <asp:TextBox ID="txtsuppliername" CssClass="textb" Width="95%" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%; border-style: dotted">
                        <asp:Label ID="Label3" Text="Challan No. & Date" runat="server" CssClass="labelbold" />
                    </td>
                    <td style="width: 40%; border-style: dotted">
                        <asp:TextBox ID="txtchallannodate" CssClass="textb" Width="95%" runat="server" />
                    </td>
                   

                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td style="width: 100%">
                        <div style="width: 100%; overflow: auto; max-height: 300px">
                            <asp:GridView ID="Dgdetail" runat="server" Width="100%" CssClass="grid-views" AutoGenerateColumns="False"
                                ShowFooter="true">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr No.">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtsrno" runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtdescription" runat="server" Width="100%" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="300px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Qty">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txttotalqty" runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   
                                    <asp:TemplateField HeaderText="Sample Plan">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtsamplesize" runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Length">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtlength" runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Thickness">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtthickness" runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Inner Dia">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtinnerdia" runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Moisture">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtmoisture" runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Weight">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtweight" runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Found">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtfound" runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acceptance">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtacceptance" runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Lot Result">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtlotresult" runat="server" Width="100%" />
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <FooterTemplate>
                                            <asp:Button ID="ButtonAdd" runat="server" Text="Add New Row" CssClass="buttonnorm"
                                                OnClick="ButtonAdd_Click" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td style="width: 10%">
                        <asp:Label ID="Label4" Text="Comments" CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 80%">
                        <asp:TextBox ID="txtcomments" runat="server" CssClass="textb" Width="95%" TextMode="MultiLine"
                            Height="30px" />
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td style="text-align: center">
                        <asp:Button Text="Save" ID="btnsave" CssClass="buttonnorm" runat="server" OnClick="btnsave_Click" />
                        <asp:Button Text="Preview" ID="btnpreview" CssClass="buttonnorm" runat="server" OnClick="btnpreview_Click" />
                        <asp:Button ID="btndelete" Text="Delete" CssClass="buttonnorm" runat="server" OnClientClick="return confirm('Do you want to delete this Doc No.?')"
                            OnClick="btndelete_Click" />
                        <asp:Button Text="Close" ID="btnclose" CssClass="buttonnorm" runat="server" OnClientClick="return CloseForm();" />
                        <asp:Button Text="New" ID="btnnew" CssClass="buttonnorm" runat="server" OnClientClick="return ClickNew();" />
                        <asp:Button Text="Approve" ID="btnApprove" CssClass="buttonnorm" runat="server" Visible="false"
                            OnClick="btnApprove_Click" OnClientClick="return confirm('Do you want to approve Doc No. ?')" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblmsg" CssClass="labelbold" runat="server" ForeColor="Red" Font-Size="Small" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hndocid" Value="0" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
