<%@ Page Title="JOB RECEIVE EDIT" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmeditreceiveprocessnext.aspx.cs" Inherits="Masters_Process_frmeditreceiveprocessnext" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script language="javascript" type="text/javascript">
        function EmpSelected(source, eventArgs) {
            document.getElementById('txtgetvalue').value = eventArgs.get_value();
        }
    </script>
    <asp:UpdatePanel runat="server" ID="upd1">
        <ContentTemplate>
            <div style="width: 100%; margin-bottom: 2px; border: 1px Solid; background-color: #DEB887;">
                <table>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" Text="Job Name" CssClass="labelbold" runat="server" />
                                        <br />
                                        <asp:DropDownList CssClass="dropdown" ID="DDTOProcess" runat="server" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="Tdrecfrom" class="tdstyle" runat="server">
                                        <span class="labelbold">Receive From</span>
                                        <br />
                                        <asp:TextBox CssClass="textb" ID="txtfrom" runat="server" Width="100px"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="txtfrom">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td id="Tdrecto" class="tdstyle" runat="server">
                                        <span class="labelbold">Receive To</span>
                                        <br />
                                        <asp:TextBox CssClass="textb" ID="txtto" runat="server" Width="100px"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MMM-yyyy"
                                            TargetControlID="txtto">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <span class="labelbold">Enter ID No.</span>
                                        <br />
                                        <asp:TextBox ID="txtWeaverIdNo" runat="server" Width="112px" Height="20px" CssClass="textb"
                                            AutoPostBack="true" OnTextChanged="txtWeaverIdNo_TextChanged"></asp:TextBox>
                                        <asp:TextBox ID="txtgetvalue" runat="server" Style="display: none"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="txtWeaverIdNo_AutoCompleteExtender" runat="server"
                                            BehaviorID="SrchAutoComplete" CompletionInterval="20" Enabled="True" ServiceMethod="GetEmployeeForJob"
                                            EnableCaching="true" CompletionSetCount="20" OnClientItemSelected="EmpSelected"
                                            ServicePath="~/Autocomplete.asmx" TargetControlID="txtWeaverIdNo" UseContextKey="True"
                                            ContextKey="0#0#0" MinimumPrefixLength="2">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="td1" runat="server">
                                        <asp:Label ID="Label3" runat="server" Text="Category" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList CssClass="dropdown" ID="DDcategory" runat="server" Width="150px"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDcategory_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="tdqualityname1" runat="server">
                                        <asp:Label ID="Label2" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList CssClass="dropdown" ID="DDQuality" runat="server" Width="150px"
                                            AutoPostBack="True" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="tdChallanNo" runat="server">
                                        <asp:Label ID="Label5" runat="server" Text="ChallanNo" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList CssClass="dropdown" ID="DDChallanNo" runat="server" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="tddesign1" runat="server">
                                        <asp:Label ID="lbldesignname" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList CssClass="dropdown" ID="DDDesign" runat="server" Width="150px" AutoPostBack="true"
                                            OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="tdcolor" runat="server">
                                        <asp:Label ID="lblcolorname" CssClass="labelbold" runat="server" Text="Color"></asp:Label><br />
                                        <asp:DropDownList ID="ddcolour" runat="server" CssClass="dropdown" Width="130px"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddcolour_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="tdsize" runat="server">
                                        <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddsize" runat="server" CssClass="dropdown" Width="130px">
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="bottom" id="TDgetstockdetail" runat="server">
                                        <asp:Button ID="btngetdetail" runat="server" CssClass="buttonnorm" Text="Get Detail"
                                            OnClick="btngetdetail_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="justify">
                            <table>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <div style="overflow: auto; width: 200px">
                                                        <asp:ListBox ID="lstWeaverName" runat="server" Width="200px" Height="100px" SelectionMode="Multiple">
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
                    </tr>
                </table>
            </div>
            <div>
                <asp:Label ID="lblmsg" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
            </div>
            <table style="width: 100%">
                <tr>
                    <td style="width: 100%">
                        <div style="max-height: 300px; overflow: auto">
                            <asp:GridView ID="DG" runat="server" AutoGenerateColumns="false" EmptyDataText="No records found for this combination."
                                OnRowDeleting="DG_RowDeleting" OnRowDataBound="DG_RowDataBound" OnRowCancelingEdit="DG_RowCancelingEdit"
                                OnRowEditing="DG_RowEditing" OnRowUpdating="DG_RowUpdating" Width="100%">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Stock No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lbltstockno" Text='<%#Bind("Tstockno") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblitemdesc" Text='<%#Bind("Itemdescription") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Width">
                                        <ItemTemplate>
                                            <asp:Label ID="lblwidth" Text='<%#Bind("Width") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Length">
                                        <ItemTemplate>
                                            <asp:Label ID="lbllen" Text='<%#Bind("Length") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Issue Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblissdate" Text='<%#Bind("OrderDate") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rec Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblrecdate" Text='<%#Bind("ReceiveDate") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblunitname" Text='<%#Bind("Unitname") %>' runat="server" />
                                        </ItemTemplate>
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
                                    <asp:TemplateField HeaderText="Actual Length">
                                        <ItemTemplate>
                                            <asp:Label ID="lblactualL" Text='<%#Bind("ActualLength") %>' runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtgridactualL" runat="server" Text='<%#Bind("ActualLength") %>'
                                                CssClass="textb" Width="80px"></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblprcessid" Text='<%#Bind("Processid") %>' runat="server" />
                                            <asp:Label ID="lblIssueDetailid" Text='<%#Bind("IssueDetailId") %>' runat="server" />
                                            <asp:Label ID="lblrecdetailid" Text='<%#Bind("ReceiveDetailId") %>' runat="server" />
                                            <asp:Label ID="lblStockNo" Text='<%#Bind("stockno") %>' runat="server" />
                                            <asp:Label ID="lblIssueorderId" Text='<%#Bind("issueorderid") %>' runat="server" />
                                            <asp:Label ID="lblprocessrecid" Text='<%#Bind("process_rec_id") %>' runat="server" />
                                            <asp:Label ID="lblpack" Text='<%#Bind("pack") %>' runat="server" />
                                            <asp:Label ID="lblDefectStatus" Text='<%#Bind("DefectStatus") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField DeleteText="" ShowEditButton="True" />
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkRemoveQccheck" runat="server" CausesValidation="False" Text="REMOVE QC"
                                                OnClientClick="return confirm('Do you want to remove QC?')" OnClick="lnkRemoveQccheck_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="Label4" Text="Total Pcs" runat="server" CssClass="labelbold" />
                        <br />
                        <asp:TextBox ID="txttotalpcs" CssClass="textb" runat="server" />
                    </td>
                    <td align="right" id="TDQccheck" runat="server" visible="false">
                        <asp:Button ID="btnqccheck" Text="QC CHECK" runat="server" CssClass="buttonnorm"
                            OnClick="btnqccheck_Click" />
                    </td>
                </tr>
            </table>
            <div>
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
                                        <div style="max-height: 500px; overflow: auto">
                                            <asp:GridView ID="GDQC" CssClass="grid-views" runat="server" OnRowDataBound="GDQC_RowDataBound">
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
                <div>
                    <asp:Button runat="server" ID="btnModalPopUp2" Style="display: none" />
                    <asp:ModalPopupExtender ID="ModalPopuptext2" runat="server" PopupControlID="pnModelPopup2"
                        TargetControlID="btnModalPopUp2" BackgroundCssClass="modalBackground" CancelControlID="BtnRemoveQCClose">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="pnModelPopup2" runat="server" Style="background-color: ActiveCaption;
                        display: none;">
                        <fieldset>
                            <legend>
                                <asp:Label ID="Label33" Text="QC REMOVE DATE" runat="server" ForeColor="Red" CssClass="labelbold" />
                            </legend>
                            <table>
                                <tr>
                                    <td>
                                        <div style="max-height: 300px; overflow: scroll; width: 250px" id="div4">
                                            <asp:TextBox ID="txtRemoveQCDate" CssClass="textb" Width="95px" runat="server" />
                                            <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txtRemoveQCDate" Format="dd-MMM-yyyy"
                                                runat="server">
                                            </asp:CalendarExtender>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="BtnRemoveQCSave" Text="Save" runat="server" CssClass="buttonnorm"
                                            OnClick="BtnRemoveQCSave_Click" />
                                        <asp:Button ID="BtnRemoveQCClose" Text="Close" runat="server" CssClass="buttonnorm" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblRemoveqcmsg" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                                        <asp:Label ID="lblRemoveQCProcessRecId" runat="server" CssClass="labelbold" ForeColor="Red"
                                            Visible="false"></asp:Label>
                                        <asp:Label ID="lblRemoveQCProcessRecDetailId" runat="server" CssClass="labelbold"
                                            ForeColor="Red" Visible="false"></asp:Label>
                                        <asp:Label ID="lblRemoveQCProcessId" runat="server" CssClass="labelbold" ForeColor="Red"
                                            Visible="false"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </asp:Panel>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
