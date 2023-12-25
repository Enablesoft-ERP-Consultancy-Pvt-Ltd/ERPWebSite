<%@ Page Title="carton inspection" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmcartoninspectionKaysons.aspx.cs" Inherits="Masters_Inspection_frmcartoninspectionKaysons" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function ClickNew() {
            window.location.href = "frmcartoninspectionKaysons.aspx";
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
                        <asp:Label ID="Label39" Style="width: 100%; text-align: center" runat="server" Text="Production Specification: IOS-P-0010-AA-171373"></asp:Label>
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
                        <asp:Label ID="Label1" Text="Inspected By :" CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 15%; border-style: dotted">
                        <asp:TextBox ID="txtInspectedBy" CssClass="textb" Width="95%" runat="server" />
                    </td>
                    <td style="width: 5%; border-style: dotted">
                        <asp:Label ID="Label6" Text="Order Qty :" CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 15%; border-style: dotted">
                        <asp:TextBox ID="txtOrderQty" CssClass="textb" Width="95%" runat="server" />
                    </td>
                    <td style="width: 10%; border-style: dotted">
                        <asp:Label ID="Label8" Text="Inspected Qty:" CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 15%; border-style: dotted">
                        <asp:TextBox ID="txtInspectedQty" CssClass="textb" Width="95%" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%; border-style: dotted">
                        <asp:Label ID="Label2" Text="Supplier Name :" CssClass="labelbold" runat="server" />
                    </td>
                    <td colspan="3" style="width: 50%; border-style: dotted">
                        <asp:TextBox ID="txtsuppliername" CssClass="textb" Width="98%" runat="server" />
                    </td>
                    <td style="width: 10%; border-style: dotted">
                        <asp:Label ID="Label7" Text="Acceptable Qty :" CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 15%; border-style: dotted">
                        <asp:TextBox ID="txtAcceptableQty" CssClass="textb" Width="98%" runat="server" />
                    </td>
                    <td style="width: 10%; border-style: dotted">
                        <asp:Label ID="Label10" Text="UID No:" CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 15%; border-style: dotted">
                        <asp:TextBox ID="txtUIDNo" CssClass="textb" Width="98%" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 18%; border-style: dotted">
                        <asp:Label ID="Label3" Text="Challan No. & Invoice No." runat="server" CssClass="labelbold" />
                    </td>
                    <td colspan="3" style="width: 42%; border-style: dotted">
                        <asp:TextBox ID="txtchallannodate" CssClass="textb" Width="98%" runat="server" />
                    </td>
                    <td style="width: 12%; border-style: dotted">
                        <asp:Label ID="Label9" Text="Sampling Plan AQL:" CssClass="labelbold" runat="server" />
                    </td>
                    <td style="width: 15%; border-style: dotted">
                        <asp:TextBox ID="txtSamplingPlanAQL" CssClass="textb" Width="98%" runat="server" />
                    </td>
                    
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td style="width: 100%">
                        <div style="width: 100%; overflow: auto; max-height: 300px">
                            <asp:GridView ID="Dgdetail" runat="server" Width="100%" CssClass="grid-views" AutoGenerateColumns="False"
                                ShowFooter="true" OnRowDataBound="Dgdetail_RowDataBound">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr No.">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtsrno" runat="server" Width="99%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtdescription" runat="server" Width="99%" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="300px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Test Report">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTestReport" runat="server" Width="99%" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="300px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Size">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSize" runat="server" Width="99%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Thickness">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtsamplesize" runat="server" Width="99%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PLY">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtnoofply" runat="server" Width="99%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IKEY Code">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtIkeyCode" runat="server" Width="99%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Moisture level">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtmoisture" runat="server" Width="99%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sub-Supplier Date Stamp/SYM">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtburstingstrength" runat="server" Width="99%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Uneven/Rough Edge Damage Flutes">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtweight" runat="server" Width="99%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Foot Size">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtfound" runat="server" Width="99%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Feet Alignment">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtacceptance" runat="server" Width="99%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pasting of the Feet">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPastingOfTheFeet" runat="server" Width="99%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Gap B/W Feet">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGapBtweenFeet" runat="server" Width="99%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="4 Way Entry">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtFourWayEntry" runat="server" Width="99%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Feet Pattern">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtFeetPattern" runat="server" Width="99%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CAPA on observation">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCAPAonObservation" runat="server" Width="99%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Result">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddresult" CssClass="dropdown" runat="server">
                                                <asp:ListItem Text="Pass" />
                                                <asp:ListItem Text="Fail" />
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <FooterStyle HorizontalAlign="Right" />
                                        <FooterTemplate>
                                            <asp:Button ID="ButtonAdd" runat="server" Text="Add New Row" CssClass="buttonnorm"
                                                OnClick="ButtonAdd_Click" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbllotresult" Text='<%#Bind("Lotresult") %>' runat="server" />
                                        </ItemTemplate>
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
                    <td style="width: 70%">
                        <asp:TextBox ID="txtcomments" runat="server" CssClass="textb" Width="95%" TextMode="MultiLine"
                            Height="30px" />
                    </td>
                    <td style="width: 20%; border-style: dotted">
                        <asp:CheckBoxList ID="chkSymbolList" AutoPostBack="True" RepeatDirection="Horizontal" TextAlign="Right" 
                        runat="server" CellSpacing="5" Height="32px">
                            <asp:ListItem Text="<img src='../../Images/SHYM 1.jpg' title='Symbol1' width='32' height='32' />" Value="Symbol1" />
                            <asp:ListItem Text="<img src='../../Images/SHYM 2.jpg' title='Symbol2' width='32' height='32' />" Value="Symbol2" />
                            <asp:ListItem Text="<img src='../../Images/SHYM 3.jpg' title='Symbol3' width='32' height='32' />" Value="Symbol3" />
                        </asp:CheckBoxList>
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td align="center">
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
