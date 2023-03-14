<%@ Page Title="Roll Weight New" Language="C#" MasterPageFile="~/ERPmasterPopUp.master"
    AutoEventWireup="true" CodeFile="frmRollWeightNew.aspx.cs" Inherits="Masters_Packing_frmRollWeightNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function CloseForm() {
            self.close();
        }
        function isNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }
        function CheckBoxClick(objref) {

            var row = objref.parentNode.parentNode;
            if (objref.checked) {
                row.style.backgroundColor = "Orange";
            }
            else {
                row.style.backgroundColor = "White";
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
                        row.style.backgroundColor = "Orange";
                    }
                    else {
                        inputlist[i].checked = false;
                        row.style.backgroundColor = "White";

                    }
                }
            }

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <div id="Div1" runat="server" style="width: 800px; overflow: scroll">
        <asp:Label ID="lbl" Text="Packing List No" runat="server" CssClass="labelbold"></asp:Label>
        <asp:TextBox ID="TxtPKLNo" runat="server" CssClass="labelbold" Enabled="false"></asp:TextBox>
        <asp:GridView ID="GdvRollWeight" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            CssClass="grid-view" OnRowCancelingEdit="GdvRollWeight_RowCancelingEdit" OnRowEditing="GdvRollWeight_RowEditing"
            OnRowUpdating="GdvRollWeight_RowUpdating">
            <HeaderStyle CssClass="gvheader" />
            <AlternatingRowStyle CssClass="gvalt" />
            <RowStyle CssClass="gvrow" />
            <Columns>
                <asp:TemplateField HeaderText="">
                    <HeaderTemplate>
                        <asp:CheckBox ID="ChkAllItem" runat="server" onclick="return CheckAll(this);" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="Chkboxitem" runat="server" onclick="return CheckBoxClick(this);" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="10px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Roll No">
                    <HeaderStyle Width="50px" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="lblRollFrom" runat="server" align="right" Text='<%#Bind ("rollfrom") %>'></asp:Label>-
                        <asp:Label ID="lblRollTo" runat="server" align="right" Text='<%#Bind ("rollTo") %>'></asp:Label>
                    </ItemTemplate>
                    <%-- <EditItemTemplate>
                        <asp:Label ID="lblRollno" runat="server" align="right" Text='<%#Bind ("rollno") %>'></asp:Label>
                    </EditItemTemplate>--%>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Single Pcs Net Weight">
                    <HeaderStyle Width="50px" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <%-- <asp:Label ID="lblNetwt" runat="server" align="right" Text='<%#Bind ("SinglePcsNetWt") %>'></asp:Label>--%>
                        <asp:TextBox ID="TxtNetWt" Width="100px" align="right" runat="server" Text='<%#Bind ("SinglePcsNetWt") %>'
                            onkeypress="return isNumber(event);"></asp:TextBox>
                    </ItemTemplate>
                    <%-- <EditItemTemplate>
                       
                    </EditItemTemplate>--%>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Single Pcs Gross Weight">
                    <HeaderStyle Width="50px" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <%-- <asp:Label ID="lblGrossswt" runat="server" align="right" Text='<%#Bind ("SinglePcsGrossWt") %>'></asp:Label>--%>
                        <asp:TextBox ID="TxtGrossswt" Width="100px" align="right" runat="server" Text='<%#Bind ("SinglePcsGrossWt") %>'
                            onkeypress="return isNumber(event);"></asp:TextBox>
                    </ItemTemplate>
                    <%-- <EditItemTemplate>
                        
                    </EditItemTemplate>--%>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Width">
                    <HeaderStyle Width="50px" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <%--<asp:Label ID="lblWidth" runat="server" align="right" Text='<%#Bind ("BaleWidth") %>'></asp:Label>--%>
                        <asp:TextBox ID="TxtWidth" Width="100px" align="right" runat="server" Text='<%#Bind ("BaleWidth") %>'
                            onkeypress="return isNumber(event);"></asp:TextBox>
                    </ItemTemplate>
                    <%--  <EditItemTemplate>
                       
                    </EditItemTemplate>--%>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Length">
                    <HeaderStyle Width="50px" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <%-- <asp:Label ID="lblLength" runat="server" align="right" Text='<%#Bind ("BaleLength") %>'></asp:Label>--%>
                        <asp:TextBox ID="TxtLength" Width="100px" align="right" runat="server" Text='<%#Bind ("BaleLength") %>'
                            onkeypress="return isNumber(event);"></asp:TextBox>
                    </ItemTemplate>
                    <%-- <EditItemTemplate>
                       
                    </EditItemTemplate>--%>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Height">
                    <HeaderStyle Width="50px" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <%-- <asp:Label ID="lblHeight" runat="server" align="right" Text='<%#Bind ("BaleHeight") %>'></asp:Label>--%>
                        <asp:TextBox ID="TxtHeight" Width="100px" align="right" runat="server" Text='<%#Bind ("BaleHeight") %>'
                            onkeypress="return isNumber(event);"></asp:TextBox>
                    </ItemTemplate>
                    <%--  <EditItemTemplate>
                      
                    </EditItemTemplate>--%>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CBM">
                    <HeaderStyle Width="50px" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="lblCBM" runat="server" align="right" Text='<%#Bind ("BaleCBM") %>'></asp:Label>
                    </ItemTemplate>
                    <%--  <EditItemTemplate>
                        <asp:Label ID="lblArea" runat="server" align="right" Text='<%#Bind ("Area") %>'></asp:Label>
                    </EditItemTemplate>--%>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Label ID="lblRollno" runat="server" align="right" Text='<%#Bind ("rollno") %>'
                            Visible="false"></asp:Label>
                        <asp:Label ID="lblId" runat="server" align="right" Text='<%#Bind ("id") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblPackingId" runat="server" align="right" Text='<%#Bind ("PackingId") %>'
                            Visible="false"></asp:Label>
                        <asp:Label ID="lblNoOfBales" runat="server" align="right" Text='<%#Bind ("PcsFrom") %>'
                            Visible="false"></asp:Label>
                        <asp:Label ID="lblPcs" runat="server" align="right" Text='<%#Bind ("Pcs") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="false" />
            </Columns>
            <EmptyDataRowStyle CssClass="gvemptytext" />
        </asp:GridView>
    </div>
    <div>
        <table>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                   <asp:Button ID="BtnSave" Text="Save" CssClass="buttonnorm" runat="server" OnClick="BtnSave_Click" />
                </td>
                <td>
                    <asp:Button ID="BtnCancel" Text="Close" CssClass="buttonnorm" runat="server" OnClientClick="return CloseForm();" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
