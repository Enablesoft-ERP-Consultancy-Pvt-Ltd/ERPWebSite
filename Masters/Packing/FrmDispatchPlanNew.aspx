<%@ Page Title="DISPATCH PLAN NEW" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="FrmDispatchPlanNew.aspx.cs" Inherits="Masters_Packing_FrmDispatchPlanNew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmDispatchPlanNew.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
      
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div>
                <table border="1px solid grey">
                    <tr>
                        <td>
                            <asp:Label ID="lblbatchno" Text="Batch No." runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:Button ID="btnSearch" runat="server" Text="Button" OnClick="btnSearch_Click"
                                Style="display: none;" />
                            <asp:TextBox ID="txtbatchno" CssClass="textb" Width="200px" runat="server" OnTextChanged="txtbatchno_TextChanged" />
                            <asp:AutoCompleteExtender ID="txtbatchno_AutoCompleteExtender" runat="server" BehaviorID="SrchAutoComplete"
                                CompletionInterval="20" Enabled="True" ServiceMethod="GetDispatchBatchno" EnableCaching="true"
                                CompletionSetCount="20" ServicePath="~/Autocomplete.asmx" TargetControlID="txtbatchno"
                                UseContextKey="True" ContextKey="0#0#0" MinimumPrefixLength="2">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label1" Text="Start Date" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtstartdate" CssClass="textb" Width="100px" runat="server" BackColor="Yellow" />
                            <asp:CalendarExtender ID="calstart" runat="server" TargetControlID="txtstartdate"
                                Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label2" Text="Comp. Date" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtcompdate" CssClass="textb" Width="100px" runat="server" BackColor="Yellow" />
                            <asp:CalendarExtender ID="calcompdate" runat="server" TargetControlID="txtcompdate"
                                Format="dd-MMM-yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="lblfilename" Text="File Name" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:FileUpload ID="fileupload" runat="server" />
                        </td>
                        <td>
                            <asp:Button ID="btnimport" runat="server" CssClass="buttonnorm" Text="Import Data"
                                OnClick="btnimport_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="max-height: 500px; overflow: auto;">
                <asp:GridView ID="DG" runat="server" AutoGenerateColumns="false">
                    <HeaderStyle CssClass="gvheaders" />
                    <AlternatingRowStyle CssClass="gvalts" />
                    <RowStyle CssClass="gvrow" />
                    <PagerStyle CssClass="PagerStyle" />
                    <EmptyDataRowStyle CssClass="gvemptytext" />
                    <Columns>
                        <asp:TemplateField HeaderText="Sr No.">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="QUALITY">
                            <ItemTemplate>
                                <asp:Label ID="lblquality" runat="server" Text='<%#Bind("Quality") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="COLOUR">
                            <ItemTemplate>
                                <asp:Label ID="lblcolour" runat="server" Text='<%#Bind("COLOUR") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SIZE">
                            <ItemTemplate>
                                <asp:Label ID="lblsize" runat="server" Text='<%#Bind("Size") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PACK TYPE">
                            <ItemTemplate>
                                <asp:Label ID="lblpacktype" runat="server" Text='<%#Bind("PackType") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ARTICLE NO.">
                            <ItemTemplate>
                                <asp:Label ID="lblarticleno" runat="server" Text='<%#Bind("Articleno") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PLAN PCS">
                            <ItemTemplate>
                                <asp:Label ID="lblplanpcs" runat="server" Text='<%#Bind("Planpcs") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PACK PCS">
                            <ItemTemplate>
                                <asp:Label ID="lblpackpcs" runat="server" Text='<%#Bind("Packpcs") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="WIP PCS">
                            <ItemTemplate>
                                <asp:Label ID="lblwippcs" runat="server" Text='<%#Bind("Wippcs") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ECIS NO">
                            <ItemTemplate>
                                <asp:Label ID="lblecisno" runat="server" Text='<%#Bind("ECISno") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DEST">
                            <ItemTemplate>
                                <asp:Label ID="lbldest" runat="server" Text='<%#Bind("Dest") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PO DATE">
                            <ItemTemplate>
                                <asp:Label ID="lblpodate" runat="server" Text='<%#Bind("Podate") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PO NO">
                            <ItemTemplate>
                                <asp:Label ID="lblpono" runat="server" Text='<%#Bind("Pono") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DT STAMP">
                            <ItemTemplate>
                                <asp:Label ID="lbldtstamp" runat="server" Text='<%#Bind("dtstamp") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="RATE DATE">
                            <ItemTemplate>
                                <asp:Label ID="lblratedate" runat="server" Text='<%#Bind("ratedate") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="SHIP ID">
                            <ItemTemplate>
                                <asp:Label ID="lblShipId" runat="server" Text='<%#Bind("ShipId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblid" Text='<%#Bind("planid") %>' runat="server"></asp:Label>
                                <asp:Label ID="lbldetailid" runat="server" Text='<%#Bind("PlanDetailid") %>'></asp:Label>
                                <asp:Label ID="lblitemid" runat="server" Text='<%#Bind("itemid") %>'></asp:Label>
                                <asp:Label ID="lblqualityid" runat="server" Text='<%#Bind("qualityid") %>'></asp:Label>
                                <asp:Label ID="lbldesignid" runat="server" Text='<%#Bind("designid") %>'></asp:Label>
                                <asp:Label ID="lblcolorid" runat="server" Text='<%#Bind("colorid") %>'></asp:Label>
                                <asp:Label ID="lblshapeid" runat="server" Text='<%#Bind("shapeid") %>'></asp:Label>
                                <asp:Label ID="lblsizeid" runat="server" Text='<%#Bind("sizeid") %>'></asp:Label>
                                <asp:Label ID="lblpackingtypeid" runat="server" Text='<%#Bind("packingtypeid") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <table style="width: 100%" id="TBSave" runat="server" visible="false">
                <tr>
                    <td align="right">
                        <asp:Button ID="btnnew" Text="New" runat="server" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                        <asp:Button ID="btnallocate" Text="Allocate Carpet" runat="server" CssClass="buttonnorm"
                            OnClick="btnallocate_Click" />
                        <asp:Button ID="btnsave" Text="Save" runat="server" CssClass="buttonnorm" OnClick="btnsave_Click" />
                        <asp:Button ID="btndelete" Text="Delete" runat="server" CssClass="buttonnorm" OnClick="btndelete_Click"
                            OnClientClick="return confirm('Do you want to delete this Dispatch Plan?');" />
                        <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="buttonnorm" OnClientClick="CloseForm();" />
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblmsg" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hnplanid" Value="0" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnimport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
