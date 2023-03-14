<%@ Page Title="DESTINATION MASTER" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmdestinationmaster.aspx.cs" Inherits="Masters_Campany_frmdestinationmaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function ClickNew() {
            window.location.href = "frmdestinationmaster.aspx";
        }      
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div>
                <div style="margin: 0% 5% 0% 5%">
                    <table style="width: 100%;" border="1px Solid Grey">
                        <tr>
                            <td valign="top">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbleffdate" Text="EffectiveDate" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtefftdate" runat="server" CssClass="textb" Width="110px" />
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                                TargetControlID="txtefftdate">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbldestcode" Text="Dest.Code" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtdestcode" runat="server" CssClass="textb" Width="90px" AutoPostBack="true"
                                                OnTextChanged="txtdestcode_TextChanged" />
                                            <asp:AutoCompleteExtender ID="txtdestcode_AutoCompleteExtender" runat="server" BehaviorID="SrchAutoComplete"
                                                CompletionInterval="20" Enabled="True" ServiceMethod="GetDestcode" EnableCaching="true"
                                                CompletionSetCount="20" ServicePath="~/Autocomplete.asmx" TargetControlID="txtdestcode"
                                                UseContextKey="True" ContextKey="0#0#0" MinimumPrefixLength="2">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" Text="BuyerName" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td colspan="3" valign="top">
                                            <asp:TextBox ID="txtbuyername" runat="server" CssClass="textb" Width="290px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:Label ID="Label3" Text="Address" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td colspan="3" valign="top">
                                            <asp:TextBox ID="txtbuyeraddress" runat="server" CssClass="textb" Width="290px" TextMode="MultiLine"
                                                Height="75px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label4" Text="Consignee" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtconsignee" runat="server" CssClass="textb" Width="170px" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label5" Text="DT" runat="server" CssClass="labelbold" />
                                            <asp:TextBox ID="txtconsigneeDt" runat="server" CssClass="textb" Width="90px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:Label ID="Label6" Text="Address" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td colspan="3" valign="top">
                                            <asp:TextBox ID="txtconsigneeaddress" runat="server" CssClass="textb" Width="290px"
                                                TextMode="MultiLine" Height="75px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label7" Text="Notify Party" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtnotifyparty" runat="server" CssClass="textb" Width="170px" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label8" Text="DT" runat="server" CssClass="labelbold" />
                                            <asp:TextBox ID="txtnotifydt" runat="server" CssClass="textb" Width="90px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:Label ID="Label9" Text="Address" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td colspan="3" valign="top">
                                            <asp:TextBox ID="txtnotifyaddress" runat="server" CssClass="textb" Width="290px"
                                                TextMode="MultiLine" Height="75px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label10" Text="NotifyParty2" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtnotifyparty2" runat="server" CssClass="textb" Width="170px" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label11" Text="DT" runat="server" CssClass="labelbold" />
                                            <asp:TextBox ID="txtnotifyparty2dt" runat="server" CssClass="textb" Width="90px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:Label ID="Label12" Text="Address" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td colspan="3" valign="top">
                                            <asp:TextBox ID="txtnotifyparty2address" runat="server" CssClass="textb" Width="290px"
                                                TextMode="MultiLine" Height="75px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="justify" valign="top">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblreceiver" Text="Receiver" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtreceiver" runat="server" CssClass="textb" Width="250px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:Label ID="Label2" Text="Address" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtreceiveraddress" runat="server" CssClass="textb" Width="250px"
                                                TextMode="MultiLine" Height="97px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label13" Text="PayingAgent" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpayingagent" runat="server" CssClass="textb" Width="250px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:Label ID="Label14" Text="Address" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td colspan="3" valign="top">
                                            <asp:TextBox ID="txtpayingagentaddress" runat="server" CssClass="textb" Width="250px"
                                                TextMode="MultiLine" Height="75px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label15" Text="Buyer" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtbuyerotherthan" runat="server" CssClass="textb" Width="250px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:Label ID="Label16" Text="(If Other than Consignee)" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td colspan="3" valign="top">
                                            <asp:TextBox ID="txtbuyerotherthanconsgadd" runat="server" CssClass="textb" Width="250px"
                                                TextMode="MultiLine" Height="75px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label17" Text="Final Destination" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtcountry" runat="server" CssClass="textb" Width="250px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label18" Text="Port_of_Disch." runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtportofdisch" runat="server" CssClass="textb" Width="250px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top" align="justify">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblgstin" Text="GSTIN" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtreceivegstin" runat="server" CssClass="textb" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label19" Text="State" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtreceiverstate" runat="server" CssClass="textb" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label20" Text="State Code" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtrecstatecode" runat="server" CssClass="textb" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label21" Text="Pan No." runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtrecpanno" runat="server" CssClass="textb" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label22" Text="CIN No." runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtreccinNo" runat="server" CssClass="textb" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label23" Text="Invoice Receiver" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtinvoice_receiver" runat="server" CssClass="textb" Width="250px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:Label ID="Label24" Text="Address" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtinvoicereceive_address" runat="server" CssClass="textb" Width="250px"
                                                TextMode="MultiLine" Height="97px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return ClickNew()" />
                                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblmsg" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
