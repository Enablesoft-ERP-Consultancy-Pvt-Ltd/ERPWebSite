<%@ Page Title="PRODUCTION ORDER" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmIssueOrderNoConsumptionExtraPercentage.aspx.cs"
    Inherits="Masters_Loom_FrmIssueOrderNoConsumptionExtraPercentage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function validatesave() {
            var Message = "";
            var selectedindex = $("#<%=DDCompanyName.ClientID %>").attr('selectedIndex');
            if (selectedindex < 0) {
                Message = Message + "Please select Company Name!!\n";
            }
            selectedindex = $("#<%=DDProcessName.ClientID %>").attr('selectedIndex');
            if (selectedindex < 0) {
                Message = Message + "Please Select Process Name !!\n";
            }

            selectedindex = $("#<%=DDEmployeeName.ClientID %>").attr('selectedIndex');
            if (selectedindex <= 0) {
                Message = Message + "Please select Employee Name !!\n";
            }

            selectedindex = $("#<%=DDFolioNo.ClientID %>").attr('selectedIndex');
            if (selectedindex <= 0) {
                Message = Message + "Please select Folio No !!\n";
            }
            var ExtraPercentageQty = document.getElementById('<%=TxtExtraPercentageQty.ClientID %>');
            if (ExtraPercentageQty.value == "") {
                Message = Message + "Please Enter Extra Percentage. !!\n";
            }
            if (Message == "") {
                return true;
            }
            else {
                alert(Message);
                return false;
            }
        }
        function NewForm() {
            window.location.href = "FrmIssueOrderNoConsumptionExtraPercentage.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <div>
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
                <fieldset>
                    <div style="width: 100%; float: left;">
                        <table>
                            <tr>
                                <td id="TDComplete" runat="server">
                                    <asp:CheckBox ID="chkcomplete" Text="For Complete" CssClass="checkboxbold" runat="server"
                                        AutoPostBack="true" OnCheckedChanged="chkcomplete_CheckedChanged" />
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="Folio No." CssClass="labelbold"></asp:Label><br />
                                    <asp:TextBox ID="TxtFolioNo" CssClass="textb" Width="90px" AutoPostBack="true" runat="server"
                                        OnTextChanged="TxtFolioNo_TextChanged" />
                                </td>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                    <br />
                                    <asp:DropDownList ID="DDCompanyName" runat="server" CssClass="dropdown" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label1" Text="Process Name" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:DropDownList ID="DDProcessName" CssClass="dropdown" Width="150px" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="DDProcessName_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblempcode" Text="Employee Name" runat="server" CssClass="labelbold" />
                                    <br />
                                    <asp:DropDownList ID="DDEmployeeName" CssClass="dropdown" Width="150px" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="DDEmployeeName_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Text="Folio No." CssClass="labelbold"></asp:Label><br />
                                    <asp:DropDownList ID="DDFolioNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                        Width="150px" OnSelectedIndexChanged="DDFolioNo_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Extra % Qty" CssClass="labelbold"></asp:Label><br />
                                    <asp:TextBox CssClass="textb" ID="TxtExtraPercentageQty" runat="server" Width="100px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
                <div>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblmessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                            </td>
                            <td align="right">
                                <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click"
                                    UseSubmitBehavior="false" OnClientClick="if (!validatesave())return; this.disabled=true;this.value = 'wait ...';" />
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
