<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="reportdlk1.aspx.cs" Inherits="Masters_ReportForms_reportdlk1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function Validate() {
            if (document.getElementById("<%=ddCatagory.ClientID %>").value == "0") {
                alert("Pls Select Category Name");
                document.getElementById("<%=ddCatagory.ClientID %>").focus();
                return false;
            }
        }
    </script>
    <table>
        <tr>
            <td valign="top" id="tdreporttype" runat="server">
                <div style="width: 300px; height: 200px; float: left; background-color: #e1efbb;
                    border-width: thin">
                    <asp:RadioButton ID="rdorderRevised" Text="Update Fabric Status" runat="server" GroupName="OrderType"
                        CssClass="labelbold" AutoPostBack="true" /><br />
                    <asp:RadioButton ID="RDProjectionOrder" Text="Pending Projection Report" runat="server"
                        GroupName="OrderType" CssClass="labelbold" AutoPostBack="true" />
                    <br />
                </div>
            </td>
            <td valign="top">
                <table>
                    <td>
                        <tr id="Tr3" runat="server">
                            <td>
                                <asp:Label ID="lblcategoryname" runat="server" class="tdstyle" Text="Department Name"
                                    CssClass="labelbold"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddCatagory" runat="server" Width="150px" CssClass="dropdown"
                                    TabIndex="13" AutoPostBack="True" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender7" runat="server" TargetControlID="ddCatagory"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                        </tr>
                        <tr runat="server" id="trorder">
                            <td id="tdorder" runat="server" class="tdstyle">
                                <span class="labelbold">Order No.</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddOrderno" runat="server" Width="150px" TabIndex="8" CssClass="dropdown">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddOrderno"
                                    ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                </cc1:ListSearchExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnsybmit" runat="server" Text="Submit" TabIndex="22" CssClass="buttonnorm"
                                    OnClientClick="return Validate();" OnClick="btnsybmit_Click" />
                            </td>
                        </tr>
                    </td>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
