<%@ Page Title="Packing/Dispatched" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmpacking_Dispatched.aspx.cs" Inherits="Masters_ReportForms_frmpacking_Dispatched" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <script runat="server">
        protected void Radiobutton_Checkedchanged(object sender, System.EventArgs e)
        {
            TRbatchno.Visible = false;
            lblfrmdate.Text = "From Date";
            TDtodate.Visible = true;
            ChkForWithStockNo.Visible = false;        
            
            
            if (RDpacking.Checked == true)
            {
                TRbatchno.Visible = true;

                if (Session["varCompanyNo"].ToString() == "21" || Session["varCompanyNo"].ToString() == "14")
                {
                    ChkForWithStockNo.Visible = true;
                }
                
            }
            if (RDdispatch.Checked == true)
            {
                if (Session["varCompanyNo"].ToString() == "21" || Session["varCompanyNo"].ToString() == "14")
                {
                    ChkForWithStockNo.Visible = true;
                }
            }
            if (Rdtobedispatch.Checked == true)
            {
                lblfrmdate.Text = "Up to";
                TDtodate.Visible = false;
                TRbatchno.Visible = true;
            }
            if (RDDispatchWithRawDetail.Checked == true)
            {
                TRbatchno.Visible = true;
            }
            if (RDDispatchWithFinishingProcessRawDetail.Checked == true)
            {
                TRbatchno.Visible = true;
            }
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div>
                <div style="width: 20%; float: left">
                    <asp:Panel runat="server" Style="border: 1px solid">
                        <table>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="RDpacking" CssClass="labelbold" Text="Packing" runat="server"
                                        Checked="true" OnCheckedChanged="Radiobutton_Checkedchanged" GroupName="a" AutoPostBack="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="RDdispatch" CssClass="labelbold" Text="Dispatched" runat="server"
                                        OnCheckedChanged="Radiobutton_Checkedchanged" AutoPostBack="true" GroupName="a" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="Rdtobedispatch" CssClass="labelbold" Text="To be Dispatch" runat="server"
                                        OnCheckedChanged="Radiobutton_Checkedchanged" AutoPostBack="true" GroupName="a" />
                                </td>
                            </tr>
                             <tr id="TRDispatchWithRawDetail" runat="server" visible="false">
                                <td>
                                    <asp:RadioButton ID="RDDispatchWithRawDetail" CssClass="labelbold" Text="Dispatched With Raw Detail" runat="server"
                                        OnCheckedChanged="Radiobutton_Checkedchanged" AutoPostBack="true" GroupName="a" />
                                </td>
                            </tr>
                             <tr id="TRDispatchWithFinishingProcessRawDetail" runat="server" visible="false">
                                <td>
                                    <asp:RadioButton ID="RDDispatchWithFinishingProcessRawDetail" CssClass="labelbold" Text="Dispatched With Finishing Raw Detail" runat="server"
                                        OnCheckedChanged="Radiobutton_Checkedchanged" AutoPostBack="true" GroupName="a" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                <div style="width: 79%; margin-left: 1%; float: right">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblcategoryname" runat="server" CssClass="labelbold" Text="Category Name"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDCategory" runat="server" AutoPostBack="True" CssClass="dropdown"
                                    Width="200px" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="TRddItemName" runat="server">
                            <td>
                                <asp:Label ID="lblitemname" runat="server" CssClass="labelbold" Text="Item Name"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddItemName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                    Width="200px" OnSelectedIndexChanged="ddItemName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="TRDDQuality" runat="server" visible="false">
                            <td>
                                <asp:Label ID="lblqualityname" runat="server" CssClass="labelbold" Text="Quality"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="DDQuality" runat="server" AutoPostBack="True" CssClass="dropdown"
                                    Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="TRDDDesign" runat="server" visible="false">
                            <td>
                                <asp:Label ID="lbldesignname" runat="server" CssClass="labelbold" Text="Design"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="DDDesign" runat="server" AutoPostBack="True" CssClass="dropdown"
                                    Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="TRDDColor" runat="server" visible="false">
                            <td>
                                <asp:Label ID="lblcolorname" runat="server" CssClass="labelbold" Text="Color"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="DDColor" runat="server" CssClass="dropdown" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="TRDDShape" runat="server" visible="false">
                            <td>
                                <asp:Label ID="lblshapename" runat="server" CssClass="labelbold" Text="Shape"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDShape" runat="server" AutoPostBack="True" CssClass="dropdown"
                                    Width="200px" OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="TRDDSize" runat="server" visible="false">
                            <td>
                                <asp:Label ID="lblsizename" runat="server" CssClass="labelbold" Text="Size"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged" />
                                <br />
                                <asp:DropDownList ID="DDSize" runat="server" AutoPostBack="True" CssClass="dropdown"
                                    Width="200px" OnSelectedIndexChanged="DDSize_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="Trpacktype" runat="server">
                            <td>
                                <asp:Label ID="Label2" runat="server" CssClass="labelbold" Text="Pack Type"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDpacktype" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="200px" OnSelectedIndexChanged="DDpacktype_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="Trarticleno" runat="server">
                            <td>
                                <asp:Label ID="Label1" runat="server" CssClass="labelbold" Text="Article No"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDarticleno" runat="server" CssClass="dropdown" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td id="TDfromdate" runat="server">
                                            <asp:Label ID="lblfrmdate" Text="From Date" runat="server" CssClass="labelbold" />
                                            <br />
                                            <asp:TextBox ID="txtfromdate" runat="server" CssClass="textb" Width="90px" />
                                            <asp:CalendarExtender ID="calrom" TargetControlID="txtfromdate" Format="dd-MMM-yyyy"
                                                runat="server">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td id="TDtodate" runat="server">
                                            <asp:Label ID="lbltodate" Text="To Date" runat="server" CssClass="labelbold" />
                                            <br />
                                            <asp:TextBox ID="txttodate" runat="server" CssClass="textb" Width="90px" />
                                            <asp:CalendarExtender ID="Calto" TargetControlID="txttodate" Format="dd-MMM-yyyy"
                                                runat="server">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="TRbatchno" runat="server">
                            <td>
                                <asp:Label ID="Label3" Text="Batch No." runat="server" CssClass="labelbold" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtbatchno" CssClass="textb" Width="190px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            <asp:CheckBox ID="ChkForWithStockNo" runat="server" Text="With StockNo" Visible="false" /></td>
                            <td colspan="1" align="right">
                                <asp:Button ID="btnPreview" runat="server" Text="Export" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblmsg" Text="" CssClass="labelbold" ForeColor="Red" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
