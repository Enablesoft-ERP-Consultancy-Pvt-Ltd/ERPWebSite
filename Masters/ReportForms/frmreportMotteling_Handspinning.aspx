<%@ Page Title="MOTTELING_HAND SPINNING REPORT" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmreportMotteling_Handspinning.aspx.cs" Inherits="Masters_ReportForms_frmreportMotteling_Handspinning" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <script runat="server">
        protected void RadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            chkexcelexport.Visible = false;
            TDStatus.Visible = false;
            chkexcelexport.Checked = false;
            if (RDissuerecdetail.Checked == true)
            {
                chkexcelexport.Visible = true;
                TDStatus.Visible = true;
            }
        }
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
                    var selectedindex = $("#<%=DDProcessname.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Process Name!!\n";
                    }

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
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>
            <div>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top">
                            <asp:Panel ID="Panel1" runat="server" Style="border: 1px solid; width: 150px">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton Text="Issue" ID="RDissue" Checked="true" runat="server" CssClass="radiobuttonnormal"
                                                GroupName="a" OnCheckedChanged="RadioButton_CheckedChanged" AutoPostBack="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton Text="Receive" ID="RDReceive" runat="server" CssClass="radiobuttonnormal"
                                                GroupName="a" OnCheckedChanged="RadioButton_CheckedChanged" AutoPostBack="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton Text="Issue_Receive" ID="RDissuerecdetail" runat="server" CssClass="radiobuttonnormal"
                                                GroupName="a" OnCheckedChanged="RadioButton_CheckedChanged" AutoPostBack="true" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td>
                            <div>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblcompany" Text="Company Name" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="250px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label5" Text="Customer Code" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDcustcode" runat="server" CssClass="dropdown" Width="250px"
                                                OnSelectedIndexChanged="DDcustcode_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label6" Text="Order No." runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDorderno" runat="server" CssClass="dropdown" Width="250px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TDStatus" runat="server" visible="false">
                                        <td>
                                            <asp:Label ID="Label7" Text="Status" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDStatus" CssClass="dropdown" runat="server">
                                                <asp:ListItem Text="ALL" />
                                                <asp:ListItem Text="COMPLETE" />
                                                <asp:ListItem Text="PENDING" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" Text="Process Name" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDProcessname" runat="server" CssClass="dropdown" Width="250px"
                                                AutoPostBack="true" OnSelectedIndexChanged="DDProcessname_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" Text="Party Name" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDPartyname" runat="server" CssClass="dropdown" Width="250px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblemplocation" Text="Employee Location" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="Ddemplocation" runat="server" CssClass="dropdown" Width="150px">
                                                <asp:ListItem Text="---Select---" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="InHouse" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="OutSide" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRcategory" runat="server">
                                        <td>
                                            <asp:Label ID="lblcategoryname" runat="server" CssClass="labelbold" Text="Category Name"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDCategory" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="250px" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRddItemName" runat="server">
                                        <td>
                                            <asp:Label ID="lblitemname" runat="server" CssClass="labelbold" Text="Item Name"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="ddItemName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                                Width="250px" OnSelectedIndexChanged="ddItemName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRDDQuality" runat="server">
                                        <td>
                                            <asp:Label ID="lblqualityname" runat="server" CssClass="labelbold" Text="Quality"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDQuality" runat="server" CssClass="dropdown" AutoPostBack="true"
                                                Width="250px" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRshade" runat="server">
                                        <td>
                                            <asp:Label ID="Label4" runat="server" CssClass="labelbold" Text="Shade Color"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="DDShadecolor" runat="server" CssClass="dropdown" Width="250px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkdate" Text="Select Date" runat="server" CssClass="checkboxbold" />
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
                                                        <asp:Label ID="Label1" Text="To Date" runat="server" CssClass="labelbold" /><br />
                                                        <asp:TextBox ID="txttodate" CssClass="textb" Width="100px" runat="server" />
                                                        <asp:CalendarExtender ID="calto" TargetControlID="txttodate" runat="server" Format="dd-MMM-yyyy">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkexcelexport" CssClass="checkboxbold" Text="Check For Excel Export"
                                                runat="server" Visible="false" />
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPreview" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
