<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="RawMaterial_report1.aspx.cs" Inherits="Masters_RawMaterial_RawMaterial_report1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
        }
        function priview() {
            window.open('../../ReportViewer.aspx', '');
        }
    </script>
    <div>
        <table width="100%">
            <tr id="trshow" align="right" runat="server" visible="false">
                <td>
                    <asp:Label ID="lblcategoryname" runat="server" Text="Catagory Name"></asp:Label>
                    <br />
                    <asp:DropDownList ID="ddCatagory" runat="server" Width="150px" TabIndex="7" CssClass="dropdown"
                        AutoPostBack="True" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchddCatagory" runat="server" TargetControlID="ddCatagory"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td>
                    <asp:Label ID="lblitemname" runat="server" Text="Item Name"></asp:Label>
                    <br />
                    <asp:DropDownList ID="dditemname" runat="server" Width="150px" OnSelectedIndexChanged="dditemname_SelectedIndexChanged"
                        TabIndex="8" AutoPostBack="True" CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchdditemname" runat="server" TargetControlID="dditemname"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td id="ql" runat="server">
                    <asp:Label ID="lblqualityname" runat="server" Text="Quality"></asp:Label>
                    <br />
                    <asp:DropDownList ID="dquality" runat="server" Width="150px" TabIndex="12" CssClass="dropdown">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchdquality" runat="server" TargetControlID="dquality"
                        ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                    </cc1:ListSearchExtender>
                </td>
                <td colspan="2" id="tdexp" runat="server">
                    <asp:Button ID="Btn_Exp" runat="server" Text="Excel Export" TabIndex="22" CssClass="buttonnorm"
                        Width="100px" OnClick="btnexp_Click" />
                </td>
                <td colspan="2">
                    <asp:Button ID="btn_show" runat="server" Text="Show" TabIndex="22" CssClass="buttonnorm"
                        OnClick="btnshow_Click" />
                </td>
            </tr>
            <tr id="trgrid" runat="server" visible="false">
                <td class="style2" colspan="4">
                    <asp:Label ID="Lblerr" runat="server" ForeColor="Red"></asp:Label>
                    <div style="width: 100%; height: 375px; overflow: scroll">
                        <asp:GridView ID="gdDesign" runat="server" Width="100%" OnRowDataBound="gdDesign_RowDataBound"
                            CssClass="grid-view" OnRowCreated="gdDesign_RowCreated">
                            <HeaderStyle CssClass="gvheader" />
                            <AlternatingRowStyle CssClass="gvalt" />
                            <RowStyle CssClass="gvrow" />
                            <EmptyDataRowStyle CssClass="gvemptytext" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <table>
    </table>
</asp:Content>
