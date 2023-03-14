<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" ViewStateMode="Enabled"
    CodeFile="RawMaterial_report.aspx.cs" MasterPageFile="~/ERPmaster.master" Inherits="Masters_RawMaterial_RawMaterial_report" %>

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
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <div>
                <table width="60%">
                    <tr id="trdd" runat="server" visible="false">
                        <td id="code" runat="server" class="tdstyle">
                            ProdCode<br />
                            <asp:TextBox ID="TxtProdCode" runat="server" AutoPostBack="True" OnTextChanged="TxtProdCode_TextChanged"
                                Width="115px" CssClass="textb"></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="TxtProdCode_AutoCompleteExtender" runat="server" EnableCaching="true"
                                Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetQuality" TargetControlID="TxtProdCode"
                                UseContextKey="True">
                            </cc1:AutoCompleteExtender>
                        </td>
                        <td id="TdFINISHED_TYPE" runat="server" class="tdstyle">
                            <asp:Label ID="LblFINISHED_TYPE" runat="server" Text="Finish_Type"></asp:Label>
                            &nbsp;<br />
                            <asp:DropDownList ID="ddFINISHED_TYPE" runat="server" CssClass="dropdown" Width="150px"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td id="TdGodown" runat="server" class="tdstyle">
                            <asp:Label ID="LblGodown" runat="server" Text="Godown"></asp:Label>
                            &nbsp;<br />
                            <asp:DropDownList ID="DDGodown" runat="server" CssClass="dropdown" Width="150px"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td id="frdate" runat="server" visible="false">
                            <asp:Label ID="LBLFRDATE" runat="server" class="tdstyle" Text="From Date"></asp:Label>
                            &nbsp;<br />
                            <asp:TextBox CssClass="textb" ID="TxtFRDate" runat="server" TabIndex="7"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="TxtFRDate">
                            </asp:CalendarExtender>
                        </td>
                        <td runat="server" id="todate" visible="false">
                            <asp:Label ID="Label1" runat="server" class="tdstyle" Text="To Date"></asp:Label>
                            &nbsp;<br />
                            <asp:TextBox CssClass="textb" ID="TxtTODate" runat="server" AutoPostBack="true" TabIndex="8"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                TargetControlID="txtTODate">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr id="tritem" runat="server">
                        <td valign="top">
                            <table>
                                <tr>
                                    <td>
                                        Searching Item:-
                                        <br />
                                        <asp:TextBox ID="txtitem" runat="server" AutoPostBack="true" OnTextChanged="txtitem_TextChanged"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-top: 10px">
                                        <div id="d23" runat="server" style="padding-top: 10px; width: 100%; height: 200px;
                                            overflow: scroll">
                                            <asp:CheckBoxList ID="CHkitem" runat="server" CssClass="dropdown" AutoPostBack="True"
                                                TabIndex="3">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trbtn" align="right" runat="server" visible="false">
                        <td colspan="4">
                            <asp:Button ID="btnpriview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnpriview_Click" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
