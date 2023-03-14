<%@ Page Title="Penality" Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="frmPenality.aspx.cs" Inherits="Masters_Campany_frmPenality" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
        function isNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <div style="margin-left: 15%; margin-right: 15%; height: 480px">
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="txtid" runat="server" Visible="False"></asp:TextBox>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Penality For" CssClass="labelbold" Font-Bold="true"></asp:Label>
                            <span style="margin-left: 15px">
                                <asp:DropDownList ID="ddPenalityType" runat="server" CssClass="dropdown" Width="200px"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddPenalityType_SelectedIndexChanged">
                                    <asp:ListItem Value="W" Selected="True">Weaver/Contractor</asp:ListItem>
                                    <asp:ListItem Value="F">Finisher</asp:ListItem>
                                </asp:DropDownList>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Penality Type" CssClass="labelbold" Font-Bold="true"></asp:Label>
                            <span style="margin-left: 10px">
                                <asp:DropDownList ID="ddPenality" runat="server" CssClass="dropdown" Width="200px">
                                    <asp:ListItem Value="C" Selected="True">Area Wise</asp:ListItem>
                                    <asp:ListItem Value="A">Pcs Wise</asp:ListItem>
                                </asp:DropDownList>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="LblQuality" runat="server" CssClass="labelbold" Font-Bold="true"></asp:Label>
                            <span style="margin-left: 13px">
                                <asp:DropDownList ID="ddQuality" runat="server" CssClass="dropdown" Width="200px"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddQuality_SelectedIndexChanged">
                                </asp:DropDownList>
                            </span>
                            <%-- <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddQuality"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>--%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="Label2" runat="server" Text="Penality Name" CssClass="labelbold" Font-Bold="true"></asp:Label>
                            <span style="margin-left: 5px">
                                <asp:TextBox ID="txtPenalityName" runat="server" CssClass="textb">
                                </asp:TextBox>
                                <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter PenalityName"
                                ControlToValidate="txtPenalityName" ForeColor="Red">*</asp:RequiredFieldValidator>--%></span>
                        </td>
                        <%--<td class="tdstyle">
                        <asp:Label Text="Penality Type" runat="server" ID="Label4" Font-Bold="true" />
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtPenalityType" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);" >
                            </asp:TextBox>
                        </td>--%>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label3" runat="server" Text="Rate" CssClass="labelbold" Font-Bold="true"></asp:Label>
                            <span style="margin-left: 57px">
                                <asp:TextBox ID="txtRate" runat="server" CssClass="textb" onkeypress="return isNumber(event);"
                                    onkeydown="return (event.keyCode!=13);">
                                </asp:TextBox></span>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right">
                            <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" OnClick="btnsave_Click"
                                OnClientClick="return confirm('Do you want to save data?')" Text="Save" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" OnClick="btnclose_Click1"
                                OnClientClick="return CloseForm();" Text="Close" />
                            <asp:Button ID="btnPreview" runat="server" CssClass="buttonnorm" OnClick="btnPreview_Click"
                                Text="Preview" />
                            <%-- <asp:Button ID="Button1" runat="server" CssClass="buttonnorm preview_width" 
                                 OnClientClick="return priview();" Text="Preview" />--%>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td class="style2" colspan="4" align="center">
                            <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                            <asp:GridView ID="gdpanality" runat="server" Width="600px" AllowPaging="True" CellPadding="4"
                                PageSize="50" ForeColor="#333333" OnPageIndexChanging="gdpanality_PageIndexChanging"
                                OnRowDataBound="gdpanality_RowDataBound" OnSelectedIndexChanged="gdpanality_SelectedIndexChanged"
                                DataKeyNames="SrNo" CssClass="grid-views" OnRowCreated="gdpanality_RowCreated">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <PagerStyle CssClass="PagerStyle" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
