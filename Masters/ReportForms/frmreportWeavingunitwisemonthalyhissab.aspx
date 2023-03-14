<%@ Page Title="WEAVING UNIT WISE MONTHLY HISSAB" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmreportWeavingunitwisemonthalyhissab.aspx.cs"
    Inherits="Masters_ReportForms_frmreportWeavingunitwisemonthalyhissab" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() { window.location.href = "../../main.aspx"; }
    </script>
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=btnPreview.ClientID %>").click(function () {
                    var Message = "";
                    var selectedindex = $("#<%=DDcompany.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Company Name!!\n";
                    }
                    //                    selectedindex = $("#<%=DDProdunit.ClientID %>").attr('selectedIndex');
                    //                    if (selectedindex <= 0) {
                    //                        Message = Message + "Please Select Production Unit. !!\n";
                    //                    }

                    if (Message == "") {
                        return true;
                    }
                    else {
                        alert(Message);
                        return false;
                    }
                });

            });
        }
    </script>
    <script runat="server">
        protected void RadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            Trprocessname.Visible = false;
            TrReportType.Visible = false;
            TrDDDepartmentName.Visible = true;
            Trprocessname.Visible = true;
            TrDDProdunit.Visible = true;
            TrDDLoomNo.Visible = true;
            TrDDCategory.Visible = true;
            TrDDitemName.Visible = true;
            TrReportType.Visible = true;
            chkexport.Visible = true;
            ChkForSaveCurrentData.Visible = false;
            if (RDPAYMENT.Checked == true)
            {
                Trprocessname.Visible = true;
                TrReportType.Visible = true;
            }
            else if (RDPAYMENTSAVE.Checked == true)
            {
                TrDDDepartmentName.Visible = false;
                Trprocessname.Visible = false;
                TrDDProdunit.Visible = false;
                TrDDLoomNo.Visible = false;
                TrDDCategory.Visible = false;
                TrDDitemName.Visible = false;
                TrReportType.Visible = false;
                ChkForSaveCurrentData.Visible = true;
                TrReportType.Visible = true;
            }
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>
            <div style="margin-top: 2%; margin-left: 5%">
                <div style="width: 25%; float: left">
                    <asp:Panel ID="Panel1" Style="width: 100%; border: 1px Solid" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="RDProduction" CssClass="labelbold" Text="PRODUCTION" runat="server"
                                        GroupName="a" Checked="true" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="RDPAYMENT" CssClass="labelbold" GroupName="a" Text="JOBWISE PAYMENT"
                                        runat="server" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="RDPAYMENTSAVE" CssClass="labelbold" Visible="false" GroupName="a"
                                        Text="JOBWISE PAYMENT SAVE" runat="server" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                <div style="width: 70%; float: right;">
                    <table border="0">
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
                                <asp:Label ID="Label8" runat="server" Text="Branch Name" CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDBranchName" runat="server" CssClass="dropdown" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="TrDDDepartmentName" runat="server">
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Department Name" CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDDepartmentName" runat="server" CssClass="dropdown" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="Trprocessname" runat="server" visible="false">
                            <td valign="top">
                                <asp:Label ID="Label6" runat="server" Text="Process Name" CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <div style="overflow: scroll; height: 140px; width: 250PX">
                                                <asp:CheckBoxList ID="chkprocessname" CssClass="checkboxbold" runat="server">
                                                </asp:CheckBoxList>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td runat="server" visible="false">
                                <asp:DropDownList ID="DDProcessName" runat="server" CssClass="dropdown" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="TrDDProdunit" runat="server">
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Production Unit" CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDProdunit" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="200px" OnSelectedIndexChanged="DDProdunit_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="TrDDLoomNo" runat="server">
                            <td>
                                <asp:Label ID="Label15" runat="server" Text="Loom No." CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDLoomNo" runat="server" CssClass="dropdown" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="TrDDCategory" runat="server">
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDCategory" runat="server" CssClass="dropdown" Width="200px"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="TrDDitemName" runat="server">
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDitemName" runat="server" CssClass="dropdown" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="TrReportType" runat="server" visible="false">
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="Report Type" CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDReportType" runat="server" Width="200px" CssClass="dropdown">
                                    <asp:ListItem Text="-- Select --" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Salary Transfer OtherBank Emp" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Salary Transfer SameBank Emp" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Cash Salary" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkdate" Text="For Date" runat="server" CssClass="checkboxbold"
                                    Visible="false" />
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblfrom" Text="From Date" runat="server" CssClass="labelbold" /><br />
                                            <asp:TextBox ID="txtfromdate" CssClass="textb" Width="100px" runat="server" />
                                            <asp:CalendarExtender ID="calfrom" TargetControlID="txtfromdate" runat="server" Format="dd-MMM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" Text="To Date" runat="server" CssClass="labelbold" /><br />
                                            <asp:TextBox ID="txttodate" CssClass="textb" Width="100px" runat="server" />
                                            <asp:CalendarExtender ID="calto" TargetControlID="txttodate" runat="server" Format="dd-MMM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                            <asp:CheckBox ID="ChkForSaveCurrentData" Text="Save For Over Write" Visible ="false"  runat="server" CssClass="checkboxbold" />
                                <asp:CheckBox ID="chkexport" Text="Export" runat="server" CssClass="checkboxbold" />
                                <asp:Button ID="BtnSave" runat="server" Text="Save" Visible ="false"  CssClass="buttonnorm" OnClick="BtnSave_Click" />
                                <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
