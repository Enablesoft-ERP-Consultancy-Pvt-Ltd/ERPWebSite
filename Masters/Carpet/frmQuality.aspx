<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmQuality.aspx.cs" Inherits="Masters_Campany_Quality"
    MasterPageFile="~/ERPmaster.master" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function QualitySelected(source, eventArgs) {
            document.getElementById('<%=txtgetvalue.ClientID %>').value = eventArgs.get_value();
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function addpriview() {
            window.open("../../ReportViewer.aspx");
        }
    </script>
    <div style="margin-left: 15%; margin-right: 15%;">
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <table style="width: 50;">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td class="tdstyle">
                                        <asp:TextBox CssClass="textb" ID="txtid" runat="server" Width="0px" Visible="False"></asp:TextBox>
                                        <asp:Label ID="lblcategory" runat="server" Text="CATEGORY" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlcategory" runat="server" AutoPostBack="True" CssClass="dropdown"
                                            OnSelectedIndexChanged="ddlcategory_SelectedIndexChanged" Width="165px">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" PromptCssClass="labelbold"
                                            PromptPosition="Bottom" TargetControlID="ddlcategory" ViewStateMode="Disabled">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdstyle">
                                        <asp:Label ID="lblitemname" runat="server" Text="ITEM NAME" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="dropdown" ID="DDMasterQulaty" runat="server" Width="165px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDMasterQulaty_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DDMasterQulaty"
                                            ErrorMessage="*" ValidationGroup="m" ForeColor="Red">*</asp:RequiredFieldValidator>
                                        <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDMasterQulaty"
                                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                                        </cc1:ListSearchExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdstyle">
                                        <asp:Label ID="lblqualityname" runat="server" Text="QUALITY NAME" CssClass="labelbold"></asp:Label>
                                        &nbsp;
                                    </td>
                                    <td>
                                    <asp:TextBox ID="txtgetvalue" runat="server" Style="display: none"></asp:TextBox>
                                        <asp:TextBox CssClass="textb" ID="txtquality" runat="server" Width="159px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter Design Name"
                                            ControlToValidate="txtquality" ValidationGroup="m" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                 <cc1:AutoCompleteExtender ID="txtquality_AutoCompleteExtender" runat="server" BehaviorID="SrchAutoComplete1"
                                                            CompletionInterval="20" Enabled="True" ServiceMethod="GetQualityName" CompletionSetCount="20"
                                                            OnClientItemSelected="QualitySelected" ServicePath="~/Autocomplete.asmx" TargetControlID="txtquality"
                                                            UseContextKey="True" ContextKey="0" MinimumPrefixLength="2" DelimiterCharacters="">
                                                        </cc1:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="TDLossLabel" runat="server" visible="false" class="tdstyle">
                                        <asp:Label ID="Label1" runat="server" Text="LOSS" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td id="TDLossText" runat="server" visible="false">
                                        <asp:TextBox ID="TxtLoss" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"
                                            Width="159px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label Text="HSN CODE" ID="lblss" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txthscode" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"
                                            Width="159px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="LblQUALITYCODE" runat="server" Text="QUALITY CODE" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtQualityCode" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"
                                            Width="159px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="TRRate" runat ="server" visible="false" >
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Material Rate" CssClass="labelbold"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtRate" runat="server" CssClass="textb" onkeydown="return (event.keyCode!=13);"
                                            Width="159px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="Trinstruction" runat="server" visible="false">
                                    <td>
                                        <span class="labelbold">INSTRUCTIONS</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtinstruction" CssClass="textb" TextMode="MultiLine" Width="300px"
                                            Height="100px" runat="server" />
                                    </td>
                                </tr>
                                <tr id="TrQualityRemark" runat="server" visible="false">
                                    <td>
                                        <span class="labelbold">REMARKS</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemark" CssClass="textb" TextMode="MultiLine" Width="300px" Height="100px"
                                            runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td id="TDDGMonthName" runat="server" visible="false">
                            <div id="gride" runat="server" style="max-height: 350px; overflow: auto">
                                <asp:GridView ID="DGMonthName" runat="server" DataKeyNames="MonthID" AutoGenerateColumns="False"
                                    CssClass="grid-views">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Month Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMonthName" Text='<%#Bind("MonthName") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="80px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Loss%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLossPercentage" Text='<%#Bind("LossPercentage") %>' Width="80px"
                                                    runat="server" CssClass="textb" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMonthID" Text='<%#Bind("MonthID") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right">
                            <asp:Button CssClass="buttonnorm" ID="btnsave" runat="server" Text="Save" OnClientClick="return confirm('Do you want to save data?')"
                                OnClick="btnsave_Click" ValidationGroup="m" />
                            &nbsp;<asp:Button CssClass="buttonnorm" ID="btnclose" runat="server" Text="Close"
                                OnClientClick="return CloseForm()" />
                            &nbsp;<asp:Button CssClass="buttonnorm preview_width" ID="BtnPreview" runat="server"
                                Text="Preview" OnClick="BtnPreview_Click" />
                            &nbsp;<asp:Button ID="btndelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"
                                CssClass="buttonnorm" OnClick="btndelete_Click" Visible="False" />
                        </td>
                    </tr>
                </table>
                <table style="margin-left: 80px">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lbl" runat="server" ForeColor="Red"></asp:Label>
                            <div style="overflow: auto; max-height: 400px; width: 500px">
                                <asp:GridView ID="gdQuality" runat="server" AllowPaging="True" PageSize="15" DataKeyNames="Sr_No"
                                    OnPageIndexChanging="gdQuality_PageIndexChanging" OnRowDataBound="gdQuality_RowDataBound"
                                    OnSelectedIndexChanged="gdQuality_SelectedIndexChanged" CaptionAlign="Left" CssClass="grid-views">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <PagerStyle CssClass="PagerStyle" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnsave" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
