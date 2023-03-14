<%@ Page Title="FINISHING HISSAB REPORT" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmFinishingProcessHissabReport.aspx.cs" Inherits="Masters_Hissab_FrmFinishingProcessHissabReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=btnprint.ClientID %>").click(function () {
                    var Message = "";
                    var selectedindex = $("#<%=DDCompany.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Company Name!!\n";
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
    <asp:UpdatePanel ID="upda1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>
            <div style="margin: 1% 20% 0% 20%">
                <table style="width: 100%" border="1" cellspacing="0">
                    <tr>
                        <td style="width: 30%; border-style: dotted">
                            <asp:Label ID="Label3" runat="server" CssClass="labelbold" Text="CompanyName"></asp:Label>
                        </td>
                        <td style="width: 50%; border-style: dotted">
                            <asp:DropDownList ID="DDCompany" runat="server" AutoPostBack="True" CssClass="dropdown"
                                Width="300px">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="DDCompany"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%; border-style: dotted">
                            <asp:Label ID="Label1" Text="Process Name" runat="server" CssClass="labelbold" />
                        </td>
                        <td style="width: 50%; border-style: dotted">
                            <asp:DropDownList ID="DDProcessName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged" Width="300px">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDProcessName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%; border-style: dotted">
                            <asp:Label ID="Label2" runat="server" CssClass="labelbold" Text="Customer code"></asp:Label>
                        </td>
                        <td style="width: 50%; border-style: dotted">
                            <asp:DropDownList ID="DDcustcode" runat="server" AutoPostBack="True" CssClass="dropdown"
                                Width="300px" OnSelectedIndexChanged="DDcustcode_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%; border-style: dotted">
                            <asp:Label ID="Label11" runat="server" CssClass="labelbold" Text="Order No."></asp:Label>
                        </td>
                        <td style="width: 50%; border-style: dotted">
                            <asp:DropDownList ID="DDorderno" runat="server" CssClass="dropdown" Width="300px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%; border-style: dotted">
                            <asp:Label ID="Label5" runat="server" CssClass="labelbold" Text="Emp Name"></asp:Label>
                        </td>
                        <td style="width: 50%; border-style: dotted">
                            <asp:DropDownList ID="DDEmpName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                OnSelectedIndexChanged="DDEmpName_SelectedIndexChanged" Width="300px">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="DDEmpName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%; border-style: dotted">
                            <asp:Label ID="lblcategoryname" runat="server" CssClass="labelbold" Text="Category Name"></asp:Label>
                        </td>
                        <td style="width: 50%; border-style: dotted">
                            <asp:DropDownList ID="DDCategory" runat="server" AutoPostBack="True" CssClass="dropdown"
                                Width="300px" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDCategory"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr id="TRddItemName" runat="server">
                        <td style="width: 30%; border-style: dotted">
                            <asp:Label ID="lblitemname" runat="server" CssClass="labelbold" Text="Item Name"></asp:Label>
                        </td>
                        <td style="width: 50%; border-style: dotted">
                            <asp:DropDownList ID="ddItemName" runat="server" AutoPostBack="True" CssClass="dropdown"
                                Width="300px" OnSelectedIndexChanged="ddItemName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender6" runat="server" TargetControlID="ddItemName"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr id="TRDDQuality" runat="server" visible="false">
                        <td style="width: 30%; border-style: dotted">
                            <asp:Label ID="lblqualityname" runat="server" CssClass="labelbold" Text="Quality"></asp:Label>
                        </td>
                        <td style="width: 50%; border-style: dotted">
                            <asp:DropDownList ID="DDQuality" runat="server" AutoPostBack="True" CssClass="dropdown"
                                Width="300px" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="DDQuality"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr id="TRDDDesign" runat="server" visible="false">
                        <td style="width: 30%; border-style: dotted">
                            <asp:Label ID="lbldesignname" runat="server" CssClass="labelbold" Text="Design"></asp:Label>
                        </td>
                        <td style="width: 50%; border-style: dotted">
                            <asp:DropDownList ID="DDDesign" runat="server" AutoPostBack="True" CssClass="dropdown"
                                Width="300px">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender8" runat="server" TargetControlID="DDDesign"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr id="TRDDColor" runat="server" visible="false">
                        <td style="width: 30%; border-style: dotted">
                            <asp:Label ID="lblcolorname" runat="server" CssClass="labelbold" Text="Color"></asp:Label>
                        </td>
                        <td style="width: 50%; border-style: dotted">
                            <asp:DropDownList ID="DDColor" runat="server" AutoPostBack="True" CssClass="dropdown"
                                Width="300px">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender9" runat="server" TargetControlID="DDColor"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr id="TRDDShape" runat="server" visible="false">
                        <td style="width: 30%; border-style: dotted">
                            <asp:Label ID="lblshapename" runat="server" CssClass="labelbold" Text="Shape"></asp:Label>
                        </td>
                        <td style="width: 50%; border-style: dotted">
                            <asp:DropDownList ID="DDShape" runat="server" AutoPostBack="True" CssClass="dropdown"
                                Width="300px" OnSelectedIndexChanged="DDShape_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender10" runat="server" TargetControlID="DDShape"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr id="TRDDSize" runat="server" visible="false">
                        <td style="width: 30%; border-style: dotted">
                            <asp:Label ID="lblsizename" runat="server" CssClass="labelbold" Text="Size"></asp:Label>
                        </td>
                        <td style="width: 50%; border-style: dotted">
                            <asp:DropDownList ID="DDSize" runat="server" AutoPostBack="True" CssClass="dropdown"
                                Width="250px">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender11" runat="server" TargetControlID="DDSize"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                            <asp:CheckBox ID="chkmtr" runat="server" Text="For Mtr." Font-Bold="true" OnCheckedChanged="chkmtr_CheckedChanged"
                                AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr id="TRDDShadeColor" runat="server" visible="false">
                        <td style="width: 30%; border-style: dotted">
                            <asp:Label ID="lblshadename" runat="server" CssClass="labelbold" Text="Shade Color"></asp:Label>
                        </td>
                        <td style="width: 50%; border-style: dotted">
                            <asp:DropDownList ID="DDShadeColor" runat="server" AutoPostBack="True" CssClass="dropdown"
                                Width="300px">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender12" runat="server" TargetControlID="DDShadeColor"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label7" runat="server" CssClass="labelbold" Text="Local Order No"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtLocalOrderNo" runat="server" CssClass="textb" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left">
                            <asp:CheckBox ID="ChkForDate" runat="server" Text="Check For Date" CssClass="checkboxbold" />
                        </td>
                    </tr>
                    <tr id="trDates" runat="server">
                        <td>
                            <asp:Label ID="Label6" runat="server" CssClass="labelbold" Text="From Date"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtFromDate" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtFromDate">
                            </asp:CalendarExtender>
                            <asp:Label ID="Label4" runat="server" CssClass="labelbold" Text="To Date" Width="80px"></asp:Label>
                            <asp:TextBox ID="TxtToDate" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtToDate">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <%--  <tr>
                        <td style="width: 20%; border-style: dotted">
                            <asp:Label ID="Label10" runat="server" CssClass="labelbold" Text="Customer code"></asp:Label>
                        </td>
                        <td style="width: 20%; border-style: dotted">
                           

                        </td>
                        <td style="width: 50%; border-style: dotted">
                            <asp:CheckBox Text="Final Folio" ID="chkfinalfolio" CssClass="checkboxbold" AutoPostBack="true"
                                runat="server" oncheckedchanged="chkfinalfolio_CheckedChanged" />
                        </td>
                    </tr>--%>
                    <tr>
                        <td>
                            &nbsp;
                            <%-- &nbsp;<asp:CheckBox Text="Hindi Format Folio" ID="ChkHindiFormat" CssClass="checkboxbold" runat="server" Visible="false" 
                           AutoPostBack="true" OnCheckedChanged="ChkHindiFormat_CheckedChanged" /><br />
                           &nbsp;<asp:CheckBox Text="Folio Material Detail" ID="ChkFolioMaterialDetail" CssClass="checkboxbold" runat="server" Visible="false" 
                           AutoPostBack="true" OnCheckedChanged="ChkFolioMaterialDetail_CheckedChanged" />--%>
                        </td>
                        <td align="right">
                            <asp:Button ID="btnprint" Text="Preview" CssClass="buttonnorm" runat="server" OnClick="btnprint_Click" />
                            <asp:Button ID="btnclose" Text="Close" CssClass="buttonnorm" runat="server" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnprint" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
