<%@ Page Title="JOB RECEIVE EDIT" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmjobReceiveeditforOther.aspx.cs" Inherits="Masters_Process_frmjobissueeditforOther" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">

        function EmpSelected(source, eventArgs) {
            document.getElementById("<%=txtgetvalue.ClientID %>").value = eventArgs.get_value();
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function NewForm() {
            window.location.href = "frmjobReceiveeditforOther.aspx";
        }
    </script>
    <asp:UpdatePanel runat="server" ID="upd1">
        <ContentTemplate>
            <div style="background-color: #DEB887;">
                <table>
                    <tr>
                        <td valign="top">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblcomp" Text="Company Name" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:DropDownList CssClass="dropdown" ID="DDCompanyName" Width="150px" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblunit" Text="Unit Name" CssClass="labelbold" runat="server" />
                                        <br />
                                        <asp:DropDownList ID="ddUnits" runat="server" Width="150px" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" Text="Job Name" CssClass="labelbold" runat="server" />
                                        <br />
                                        <asp:DropDownList CssClass="dropdown" ID="DDTOProcess" runat="server" Width="150px"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDTOProcess_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" Text="Enter Receive No." CssClass="labelbold" runat="server" />
                                        <br />
                                        <asp:TextBox ID="txtissueno" CssClass="textb" Width="150px" runat="server" AutoPostBack="true"
                                            OnTextChanged="txtissueno_TextChanged" />
                                    </td>
                                </tr>
                                <tr id="TRTxtWeight" runat="server" visible="false">
                                    <td>
                                        <asp:Label ID="Label3" Text="Weight" CssClass="labelbold" runat="server" />
                                        <br />
                                        <asp:TextBox ID="TxtWeight" CssClass="textb" Width="150px" runat="server" />
                                    </td>
                                    <td>
                                    <br />
                                    <asp:Button ID="BtnUpdateWeight" Text="Update Weight" runat="server" CssClass="buttonnorm" OnClick="BtnUpdateWeight_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="justify">
                            <table>
                                <tr valign="top">
                                    <td>
                                        <span class="labelbold">Enter ID No.</span>
                                        <asp:LinkButton ID="lnkgetissueno" runat="server" Text="Get Challan No." ForeColor="Red"
                                            Font-Bold="true" Font-Size="Medium" OnClick="lnkgetissueno_Click"></asp:LinkButton>
                                        <br />
                                        <asp:TextBox ID="txtWeaverIdNo" runat="server" Width="250px" Height="20px" CssClass="textb"
                                            AutoPostBack="true" OnTextChanged="txtWeaverIdNo_TextChanged"></asp:TextBox>
                                        <asp:TextBox ID="txtgetvalue" runat="server" Style="display: none"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="txtWeaverIdNo_AutoCompleteExtender" runat="server"
                                            BehaviorID="SrchAutoComplete" CompletionInterval="20" Enabled="True" ServiceMethod="GetEmployeeForJobNew"
                                            EnableCaching="true" CompletionSetCount="20" OnClientItemSelected="EmpSelected"
                                            ServicePath="~/Autocomplete.asmx" TargetControlID="txtWeaverIdNo" UseContextKey="True"
                                            ContextKey="0" MinimumPrefixLength="2">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <div style="overflow: auto; width: 250px">
                                                        <asp:ListBox ID="lstWeaverName" runat="server" Width="250px" Height="100px" SelectionMode="Multiple">
                                                        </asp:ListBox>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:LinkButton ID="btnDeleteName" Text="Remove Employee" CssClass="labelbold" runat="server"
                                                        OnClick="btnDeleteName_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <br />
                            <asp:Label ID="lblissueNo" Text="Receive No." runat="server" CssClass="labelbold" />
                            <asp:CheckBox ID="chkcomplete" Text="For Complete" runat="server" AutoPostBack="true"
                                CssClass="checkboxbold" OnCheckedChanged="chkcomplete_CheckedChanged" />
                            <br />
                            <asp:DropDownList ID="DDissueno" CssClass="dropdown" Width="200px" runat="server"
                                AutoPostBack="true" OnSelectedIndexChanged="DDissueno_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblmsg" Text="" runat="server" CssClass="labelbold" ForeColor="Red"
                                Font-Bold="true" Font-Size="Small" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblalert" Text="* Red color in Grid shows Stock is packed." ForeColor="Green"
                                Font-Bold="true" Font-Size="XX-Small" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="max-height: 500px; background-color: Gray; overflow: auto">
                <asp:GridView ID="DGDetail" runat="server" AutoGenerateColumns="False" OnRowDeleting="DGDetail_RowDeleting"
                    EmptyDataText="No Records found....." OnRowDataBound="DGDetail_RowDataBound"
                    OnRowEditing="DGDetail_RowEditing" OnRowCancelingEdit="DGDetail_RowCancelingEdit"
                    OnRowUpdating="DGDetail_RowUpdating">
                    <HeaderStyle CssClass="gvheaders" />
                    <AlternatingRowStyle CssClass="gvalts" />
                    <RowStyle CssClass="gvrow" />
                    <Columns>
                        <asp:TemplateField HeaderText="Sr No.">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Item Name">
                            <ItemTemplate>
                                <asp:Label ID="lblitemname" Text='<%#Bind("Item_Name") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quality">
                            <ItemTemplate>
                                <asp:Label ID="lblquality" Text='<%#Bind("QualityName") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Design">
                            <ItemTemplate>
                                <asp:Label ID="lblDesign" Text='<%#Bind("Designname") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Color">
                            <ItemTemplate>
                                <asp:Label ID="lblcolor" Text='<%#Bind("Colorname") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Shape">
                            <ItemTemplate>
                                <asp:Label ID="lblshape" Text='<%#Bind("shapename") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Size">
                            <ItemTemplate>
                                <asp:Label ID="lblSize" Text='<%#Bind("Size") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actual Length">
                            <ItemTemplate>
                                <asp:Label ID="lblactualL" Text='<%#Bind("ActualLength") %>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtgridactualL" runat="server" Text='<%#Bind("ActualLength") %>'
                                    CssClass="textb" Width="80px"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actual Width">
                            <ItemTemplate>
                                <asp:Label ID="lblactualW" Text='<%#Bind("ActualWidth") %>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtgridactualW" runat="server" Text='<%#Bind("ActualWidth") %>'
                                    CssClass="textb" Width="80px"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Area">
                            <ItemTemplate>
                                <asp:Label ID="lblarea" Text='<%#Bind("Area") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rate">
                            <ItemTemplate>
                                <asp:Label ID="lblrate" Text='<%#Bind("Rate") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblamount" Text='<%#Bind("Amount") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Penality Amt">
                            <ItemTemplate>
                                <asp:Label ID="lblpenamount" Text='<%#Bind("penality") %>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtpenamount" Text='<%#Bind("penality") %>' Width="70px" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Penality Remark">
                            <ItemTemplate>
                                <asp:Label ID="lblpenremark" Text='<%#Bind("premarks") %>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtpenremarks" Text='<%#Bind("Premarks") %>' MaxLength="50" Width="150px"
                                    runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Stock No.">
                            <ItemTemplate>
                                <asp:Label ID="lblTStockNo" Text='<%#Bind("Tstockno") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Bonus" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBonus" Text='<%#Bind("Bonus") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblprcessid" runat="server" Text='<%#Bind("ToProcessid") %>'></asp:Label>
                                <asp:Label ID="lblStockNo" runat="server" Text='<%#Bind("StockNo") %>'></asp:Label>
                                <asp:Label ID="lblrecdetailid" Text='<%#Bind("ReceiveDetailId") %>' runat="server" />
                                <asp:Label ID="lblIssueorderId" runat="server" Text='<%#Bind("issueorderid") %>'></asp:Label>
                                <asp:Label ID="lblIssueDetailid" runat="server" Text='<%#Bind("IssueDetailid") %>'></asp:Label>
                                <asp:Label ID="lblprocessrecid" Text='<%#Bind("process_rec_id") %>' runat="server" />
                                <asp:Label ID="lblpack" Text='<%#Bind("pack") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkdel" runat="server" CausesValidation="False" CommandName="Delete"
                                    Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField DeleteText="" ShowEditButton="True" />
                    </Columns>
                    <EmptyDataRowStyle CssClass="gvemptytext" />
                </asp:GridView>
            </div>
            <table>
                <tr>
                    <td id="Td8">
                        <span class="labelbold">Total Pcs</span>
                        <asp:TextBox CssClass="textb" ID="TxtTotalPcs" runat="server" Width="100px"></asp:TextBox>
                    </td>
                    <td id="Td5">
                        <span class="labelbold">Area</span>
                        <asp:TextBox CssClass="textb" ID="TxtArea" runat="server" Width="100px"></asp:TextBox>
                    </td>
                    <td id="Td6">
                        <span class="labelbold">Amount</span>
                        <asp:TextBox CssClass="textb" ID="TxtAmount" runat="server" Width="100px"></asp:TextBox>
                    </td>
                    <td align="right">
                        <asp:CheckBox ID="ChkForSummary" Text="For Print Summary" runat="server" CssClass="checkboxbold" />
                        <asp:CheckBox ID="ChkForActualSize" Text="For Actual Size" runat="server" CssClass="checkboxbold" />
                        <asp:Button ID="btnpreview" Text="Preview" runat="server" CssClass="buttonnorm" OnClick="btnpreview_Click" />
                        <asp:Button ID="btnqcreport" Text="QcReport" runat="server" CssClass="buttonnorm"
                            OnClick="btnqcreport_Click" Visible="false" />
                        <asp:Button CssClass="buttonnorm" ID="btnQcPreview" runat="server" Text="QC CHECK"
                            OnClick="btnQcPreview_Click" Visible="false" />
                        <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                        <asp:Button ID="Btnnew" Text="New" runat="server" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hnprocessrecid" runat="server" Value="0" />
            <div>
                <asp:Button runat="server" ID="btnModalPopUp" Style="display: none" />
                <asp:ModalPopupExtender ID="Modalpopupextqc" runat="server" PopupControlID="pnModelPopup"
                    TargetControlID="btnModalPopUp" BackgroundCssClass="modalBackground" CancelControlID="btnqcclose">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnModelPopup" runat="server" Style="background-color: ActiveCaption;
                    display: none">
                    <fieldset>
                        <legend>
                            <asp:Label ID="lblqc" Text="QC PARAMETER" runat="server" ForeColor="Red" CssClass="labelbold" />
                        </legend>
                        <table>
                            <tr>
                                <td>
                                    <div style="max-height: 500px; overflow: auto; width: 800px">
                                        <asp:GridView ID="GDQC" CssClass="grid-views" runat="server" OnRowDataBound="GDQC_RowDataBound"
                                            OnRowCreated="GDQC_RowCreated">
                                            <HeaderStyle CssClass="gvheaders" Font-Size="12px" />
                                            <AlternatingRowStyle CssClass="gvalts" />
                                            <RowStyle CssClass="gvrow" />
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btnqcsavenew" Text="Save" runat="server" CssClass="buttonnorm" OnClick="btnqcsavenew_Click" />
                                    <asp:Button ID="btnqcclose" Text="Close" runat="server" CssClass="buttonnorm" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblqcmsg" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
