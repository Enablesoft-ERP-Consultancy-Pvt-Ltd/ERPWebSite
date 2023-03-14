<%@ Page Title="Remove QC Defect Process JobWise" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmRemoveQCDefectProcessJobWise.aspx.cs" Inherits="Masters_Process_FrmRemoveQCDefectProcessJobWise" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div>
                <div style="margin: 0% 30% 0% 30%">
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
                                <asp:Label ID="Label3" runat="server" Text="Process Name" CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDprocess" runat="server" CssClass="dropdown" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>

                         <tr id="trCategoryName" runat="server">
                                        <td>
                                            <asp:Label ID="Label8" Text="Category Name" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDCategory" runat="server" CssClass="dropdown" Width="200px"
                                                AutoPostBack="true" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trItemName" runat="server">
                                        <td>
                                            <asp:Label ID="Label44" Text="Item Name" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDQtype" runat="server" CssClass="dropdown" Width="200px" AutoPostBack="true"
                                                OnSelectedIndexChanged="DDQtype_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="Trquality" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label45" Text="Quality" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDQuality" runat="server" CssClass="dropdown" Width="200px"
                                                AutoPostBack="true" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="Trdesign" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label5" Text="Design" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDDesign" runat="server" CssClass="dropdown" Width="200px"
                                                AutoPostBack="true" OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="Trcolor" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label6" Text="Color" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDColor" runat="server" CssClass="dropdown" Width="200px" AutoPostBack="true"
                                                OnSelectedIndexChanged="DDColor_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="Trsize" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label7" Text="Size" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDSize" runat="server" CssClass="dropdown" Width="200px">
                                            </asp:DropDownList>
                                            <asp:CheckBox ID="Chkmtrsize" Text="Mtr Size" runat="server" AutoPostBack="true"
                                                CssClass="checkboxbold" OnCheckedChanged="Chkmtrsize_CheckedChanged" />
                                        </td>
                                    </tr>
                                    <tr id="Trshadecolor" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label10" Text="Shadecolor" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDshade" runat="server" CssClass="dropdown" Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                        <tr>
                            <td>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblfrom" Text="From Date" runat="server" CssClass="labelbold" /><br />
                                            <asp:TextBox ID="txtfromdate" CssClass="textb" Width="100px" runat="server" AutoPostBack="true" OnTextChanged="txtfromdate_TextChanged" />
                                            <asp:CalendarExtender ID="calfrom" TargetControlID="txtfromdate" runat="server" Format="dd-MMM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" Text="To Date" runat="server" CssClass="labelbold" /><br />
                                            <asp:TextBox ID="txttodate" CssClass="textb" Width="100px" runat="server"  />
                                            <asp:CalendarExtender ID="calto" TargetControlID="txttodate" runat="server" Format="dd-MMM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                         <tr>
                            <td>
                            <asp:CheckBox ID="ChkForSelectRemoveDate" runat="server" Text="Select RemoveDate" class="tdstyle"
                                                Visible="true" CssClass="checkboxbold" AutoPostBack="true" OnCheckedChanged="ChkForSelectRemoveDate_CheckedChanged" />
                            </td>
                            <td>
                                <table>
                                    <tr id="TRRemoveDefectDate" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label1" Text="Remove Date" runat="server" CssClass="labelbold" /><br />
                                            <asp:TextBox ID="txtRemoveDefectDate" CssClass="textb" Width="100px" runat="server" />
                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtRemoveDefectDate" runat="server" Format="dd-MMM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2" style="width: 100%">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblmessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="margin-left: 120px">
                                                <asp:Button ID="btnRemoveQCDefect" runat="server" Text="Remove QC Defect" CssClass="buttonnorm"
                                                    OnClick="btnRemoveQCDefect_Click" />
                                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" /></span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnRemoveQCDefect" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
