<%@ Page Title="User Enable/Disable" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmuserenable.aspx.cs" Inherits="frmuserenable" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
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
            <div style="margin-left: 30%; margin-right: 30%">
                <table style="width: 100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblusertype" CssClass="labelbold" Text="User Type" runat="server" /><br />
                            <asp:DropDownList ID="DDusertype" CssClass="dropdown" Width="170px" AutoPostBack="true"
                                runat="server" OnSelectedIndexChanged="DDusertype_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="max-height: 300px; overflow: auto">
                                <asp:GridView ID="GDView" runat="server" AutoGenerateColumns="False" OnRowDataBound="GDView_RowDataBound">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkheader"  Text="" runat="server" onclick="return CheckAll(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkitem" Text="" CssClass="checkboxbold" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="User Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblusername" Text='<%#Bind("UserName") %>' runat="server" Width="150px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Designation">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldesignation" Text='<%#Bind("Designation") %>' runat="server" Width="150px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbluserid" Text='<%#Bind("userid")%>' runat="server" />
                                                <asp:Label ID="lblloginflag" Text='<%#Bind("loginflag")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                        </td>
            </div>
            </tr>
            <tr>
                <td align="right">
                    <asp:Button ID="btnenableDisable" Text="Enable/Disable" CssClass="buttonnorm" 
                        runat="server" onclick="btnenableDisable_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button1" runat="server" Text="" BackColor="Green" Width="24px" Enabled="false" />
                    Enable
                    <asp:Button ID="Button2" runat="server" Text="" BackColor="Red" Width="24px" Enabled="false" />Disable
                </td>
            </tr>
            </table> </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
