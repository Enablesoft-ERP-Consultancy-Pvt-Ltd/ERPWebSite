<%@ Page Title="Shift Schedule" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmshiftschedule.aspx.cs" Inherits="Masters_Payroll_frmshiftschedule" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function ClickNew() {
            window.location.href = "frmshiftschedule.aspx";
        }

        function KeyDownHandlerWeaverIdscan(event) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                window.document.getElementById('<%=btnsearchemp.ClientID %>').click();
            }
        }
        function CheckAll(objref) {
            var gridview = objref.parentNode.parentNode.parentNode;
            var inputlist = gridview.getElementsByTagName("input");
            for (var i = 0; i < inputlist.length; i++) {
                var row = inputlist[i].parentNode.parentNode;
                if (inputlist[i].type == "checkbox" && objref != inputlist[i]) {
                    if (objref.checked) {

                        inputlist[i].checked = true;

                    }
                    else {
                        inputlist[i].checked = false;


                    }
                }
            }

        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div style="width: 100%">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 40%" valign="top">
                            <table border="1" cellspacing="2" width="100%">
                                <tr>
                                    <td class="tdstyle">
                                        <asp:Label ID="Label33" runat="server" Text="Branch Name" CssClass="labelbold"></asp:Label>
                                        <asp:DropDownList CssClass="dropdown" ID="DDBranchName" Width="80%" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-style: dotted">
                                        <asp:Label Text="Department Name" CssClass="labelbold" runat="server" />
                                        <asp:DropDownList ID="DDdepartment" CssClass="dropdown" Width="75%" runat="server"
                                            AutoPostBack="true" OnSelectedIndexChanged="DDdepartment_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%">
                                        <div style="width: 100%; overflow: auto; max-height: 350px">
                                            <asp:GridView ID="Dgemp" CssClass="grid-views" AutoGenerateColumns="false" runat="server"
                                                Width="100%" EmptyDataText="No data fetched..">
                                                <HeaderStyle CssClass="gvheaders" />
                                                <AlternatingRowStyle CssClass="gvalts" />
                                                <RowStyle CssClass="gvrow" />
                                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkitem" Text="" runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkallitem" Text="" runat="server" onclick="return CheckAll(this);" />
                                                        </HeaderTemplate>
                                                        <HeaderStyle Width="20px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sr No.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="40px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Emp. Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblempcode" Text='<%#Bind("Empcode") %>' runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="60px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Emp. Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblempname" Text='<%#Bind("empname") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblempid" Text='<%#Bind("empid") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="border-style: dotted">
                                        <asp:Button ID="btnset" CssClass="buttonnorm" Text="Assign" runat="server" OnClick="btnset_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 60%" valign="top">
                            <table border="1" cellspacing="2" width="100%">
                                <tr>
                                    <td style="width: 40%; border-style: dotted" valign="middle">
                                        <asp:Label ID="Label1" CssClass="labelbold" Text="Enter Emp. Code" runat="server" /><br />
                                        <asp:TextBox ID="txtempcode" CssClass="textboxm" Width="95%" runat="server" onKeypress="KeyDownHandlerWeaverIdscan(event);" />
                                        <asp:Button ID="btnsearchemp" Text="Emp.Search" Style="display: none" runat="server"
                                            OnClick="btnsearchemp_Click" />
                                    </td>
                                    <td style="width: 60%; border-style: dotted" valign="top">
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 100%">
                                                    <div style="overflow: auto; width: 100%">
                                                        <asp:ListBox ID="listWeaverName" runat="server" Style="width: 100%; max-height: 100px"
                                                            SelectionMode="Multiple"></asp:ListBox>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:LinkButton ID="btnDelete" Text="Remove Employee" runat="server" CssClass="linkbuttonnew"
                                                        OnClick="btnDelete_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%" colspan="2">
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 10%">
                                                    <asp:Label ID="Label2" Text="From Date" CssClass="labelbold" runat="server" />
                                                </td>
                                                <td style="width: 15%">
                                                    <asp:TextBox ID="txtfromdate" CssClass="textboxm" Width="95%" runat="server" autocomplete="Off" />
                                                    <asp:CalendarExtender ID="calfrom" TargetControlID="txtfromdate" Format="dd/MM/yyyy"
                                                        runat="server">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td style="width: 10%">
                                                    <asp:Label ID="Label3" Text="To Date" CssClass="labelbold" runat="server" />
                                                </td>
                                                <td style="width: 15%">
                                                    <asp:TextBox ID="txttodate" CssClass="textboxm" Width="95%" runat="server" autocomplete="Off" />
                                                    <asp:CalendarExtender ID="calto" TargetControlID="txttodate" Format="dd/MM/yyyy"
                                                        runat="server">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td style="width: 10%">
                                                    <asp:Button ID="btngetdata" CssClass="buttonnorm" Text="Get Data" runat="server"
                                                        OnClick="btngetdata_Click" />
                                                </td>
                                                <td style="width: 20%">
                                                    <asp:Button ID="btnassignbulk" CssClass="buttonnorm" Text="Assign Shift All" runat="server"
                                                        OnClientClick="if (!confirm('Do you want to Assign?')) return; this.disabled=true;this.value = 'wait ...';"
                                                        UseSubmitBehavior="false" OnClick="btnassignbulk_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%" colspan="2">
                                        <fieldset>
                                            <legend>
                                                <asp:Label ID="Label4" CssClass="labelbold" ForeColor="Red" Text="Data Insert Grid"
                                                    runat="server" />
                                            </legend>
                                            <div style="width: 100%; overflow: auto; max-height: 300px">
                                                <asp:GridView ID="Dgdetail" runat="server" CssClass="grid-views" Width="100%" ShowFooter="true"
                                                    AutoGenerateColumns="false" OnRowDataBound="Dgdetail_RowDataBound">
                                                    <HeaderStyle CssClass="gvheaders" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <RowStyle CssClass="gvrow" />
                                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sr No.">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="40px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Date">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtdate" CssClass="textboxm" runat="server" Width="95%" autocomplete="Off" />
                                                                <asp:CalendarExtender ID="caldategrid" TargetControlID="txtdate" Format="dd/MM/yyyy"
                                                                    runat="server">
                                                                </asp:CalendarExtender>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="120px" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Shift">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddshift" CssClass="dropdown" Width="100%" runat="server">
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Button ID="btnaddnewrow" runat="server" Text="Add New Row" CssClass="buttonnorm"
                                                                    CausesValidation="false" OnClick="btnaddnewrow_Click" />
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblshiftid" Text='<%#Bind("shiftid") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="width: 100%" align="right">
                                        <asp:Button ID="btnsave" CssClass="buttonnorm" Text="Save" runat="server" OnClick="btnsave_Click"
                                            OnClientClick="if (!confirm('Do you want to save Data?')) return; this.disabled=true;this.value = 'wait ...';"
                                            UseSubmitBehavior="false" />
                                        <asp:Button ID="btnclose" CssClass="buttonnorm" Text="Close" runat="server" OnClientClick="return CloseForm();" />
                                        <asp:Button ID="btnnew" CssClass="buttonnorm" Text="New" runat="server" OnClientClick="return ClickNew();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="width: 100%">
                                        <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
