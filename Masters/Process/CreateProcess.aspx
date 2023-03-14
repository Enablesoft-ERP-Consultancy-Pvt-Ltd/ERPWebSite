<%@ Page Title="CreateProcess" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="CreateProcess.aspx.cs" Inherits="Masters_Campany_Design"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function ProcessSelected(source, eventArgs) {
            document.getElementById('<%=txtgetvalue.ClientID %>').value = eventArgs.get_value();
        }
        function closeform() {
            window.location.href = "../../main.aspx";
        }
        function checkcheck() {
            var isValid = false;
            var j = 0;
            var CheckList = document.getElementById('CPH_Form_ChkProcessType');
            for (var i = 1; i < CheckList.rows.length; i++) {
                var inputs = CheckList.rows[i].getElementsByTagName('input');
                if (inputs != null) {
                    if (inputs[0].type == "checkbox") {
                        if (inputs[0].checked) {
                            j = j + 1;
                            if (j > 1) {
                                alert("Please Select Only One Item");
                                inputs[0].checked = false;
                                return false;
                            }
                        }
                    }
                }
            }
        }
    </script>
    <div style="margin-left: 10%; margin-right: 15%; width: 711px;">
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <div style="overflow: auto; width: 350px; height: 155px">
                                <table>
                                    <tr>
                                        <td class="tdstyle">
                                            <asp:Label Text="Process Name" runat="server" Font-Bold="true" ID="labjdsj" />
                                        </td>
                                        <td>
                                        <asp:TextBox ID="txtgetvalue" runat="server" Style="display: none"></asp:TextBox>
                                            <asp:TextBox ID="TxtProcessName" runat="server" Width="250px" CssClass="textb" onkeydown="return (event.keyCode!=13);">
                                            </asp:TextBox>
                                             <cc1:AutoCompleteExtender ID="TxtProcessName_AutoCompleteExtender" runat="server" BehaviorID="SrchAutoComplete1"
                                                            CompletionInterval="20" Enabled="True" ServiceMethod="GetProcessName" CompletionSetCount="20"
                                                            OnClientItemSelected="ProcessSelected" ServicePath="~/Autocomplete.asmx" TargetControlID="TxtProcessName"
                                                            UseContextKey="True" ContextKey="0" MinimumPrefixLength="2" DelimiterCharacters="">
                                                        </cc1:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdstyle">
                                            <asp:Label Text="Short Name" runat="server" Font-Bold="true" ID="Label1" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtShortName" runat="server" Width="250px" CssClass="textb" onkeydown="return (event.keyCode!=13);">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="right">
                                            <asp:CheckBox ID="ChkForApproval" class="tdstyle" Text="Check For Process Approval"
                                                runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="right" id="TDAreaeditable" runat="server" visible="false">
                                            <asp:CheckBox ID="Chkareaeditable" class="tdstyle" Text="Area Editable" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="right" id="TDissreconetime" runat="server" visible="false">
                                            <asp:CheckBox ID="chkissreconetime" class="tdstyle" Text="Check For Iss. Rec. One Time"
                                                runat="server" />
                                        </td>
                                    </tr>

                                     <tr>
                                        <td colspan="2" align="right" id="TDSizeTolerance" runat="server" visible="false">
                                            <asp:CheckBox ID="ChkForSizeTolerance" class="tdstyle" Text="Check For Size Tolerance"
                                                runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="right" id="TDWeightTolerance" runat="server" visible="false">
                                            <asp:CheckBox ID="ChkForWeightTolerance" class="tdstyle" Text="Check For Weight Tolerance"
                                                runat="server" />
                                        </td>
                                    </tr>

                                </table>
                            </div>
                        </td>
                        <td rowspan="8" colspan="4" align="justify" style="margin-top: 0px" class="tdstyle">
                            <div style="overflow: auto; height: 125px; width: 168px">
                                <asp:Label Text="User Type" runat="server" Font-Bold="true" ID="Label2" />
                                <br />
                                <asp:CheckBoxList ID="ChkForUserType" runat="server">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                        <td rowspan="8" colspan="4" align="justify" style="margin-top: 0px" class="tdstyle">
                            <div style="overflow: auto; height: 125px; width: 180px">
                                <asp:Label Text=" Process Type" runat="server" Font-Bold="true" ID="Label3" />
                                <br />
                                <asp:RadioButtonList ID="RDProcessType" runat="server">
                                    <asp:ListItem Value="0"> For Material Preparation</asp:ListItem>
                                    <asp:ListItem Value="1"> For Production</asp:ListItem>
                                    <asp:ListItem Value="2"> For Purchase</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td align="right" colspan="4">
                            <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" OnClick="btnsave_Click"
                                OnClientClick="return confirm('Do you want to save data?')" Text="Save" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" OnClientClick="return closeform();"
                                Text="Close" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LblErrer" runat="server" ForeColor="Red" Text="" align="left" Visible="false"></asp:Label>
                            <div id="divgride" runat="server" style="height: 400px; width: 300px; overflow: auto;
                                border-width: medium; border-color: Black;">
                                <asp:GridView ID="DGCreateProcess" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                                    OnRowDataBound="DGProcess_RowDataBound" OnSelectedIndexChanged="DGCreateProcess_SelectedIndexChanged"
                                    CssClass="grid-views">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderText="Sr.No" />
                                        <asp:TemplateField HeaderText="Sr.No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblid" Text='<%#Bind("ID") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PROCESS_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblprocess_name" Text='<%#Bind("PROCESS_NAME") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SHORTNAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblshortname" Text='<%#Bind("SHORTNAME") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SEQ No.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="textseqno" Text='<%#Bind("SeqNo") %>' Width="50px" runat="server"
                                                    onfocus="this.select()" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" id="TDseqNo" runat="server">
                            <asp:LinkButton ID="lnkbutton" Text="Update Seq No." runat="server" CssClass="labelbold"
                                ForeColor="DarkViolet" OnClick="lnkbutton_Click" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
