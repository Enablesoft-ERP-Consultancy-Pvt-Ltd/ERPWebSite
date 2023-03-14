<%@ Page Title="Roll Weight" Language="C#" MasterPageFile="~/ERPmasterPopUp.master"
    AutoEventWireup="true" CodeFile="frmRollWeight.aspx.cs" Inherits="Masters_Packing_frmRollWeight" %>

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
        function Validate() {
            if (document.getElementById("<%=DDRollFrom.ClientID %>").value == "0") {
                alert("Please Select Roll From!");
                document.getElementById("<%=DDRollFrom.ClientID %>").value = "0";
                document.getElementById("<%=DDRollFrom.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=DDRollTo.ClientID %>").value == "0") {
                alert("Please Select Roll To! ");
                document.getElementById("<%=DDRollTo.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=TxtNetWeightB.ClientID %>").value == "") {
                alert("Net weight can not be blank! ");
                document.getElementById("<%=TxtNetWeightB.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=TxtLessGrossWt.ClientID %>").value == "") {
                alert("Less Gross weight can not be blank! ");
                document.getElementById("<%=TxtLessGrossWt.ClientID %>").focus();
                return false;
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <div id="Div1" runat="server" style="width: 800px; overflow: scroll">
        <asp:Label ID="lbl" Text="Packing List No" runat="server" CssClass="labelbold"></asp:Label>
        <asp:TextBox ID="TxtPKLNo" runat="server" CssClass="labelbold" Enabled="false"></asp:TextBox>
        <asp:GridView ID="GdvRollWeight" runat="server" AutoGenerateColumns="False" DataKeyNames="rollno"
            CssClass="grid-view" OnRowCancelingEdit="GdvRollWeight_RowCancelingEdit" OnRowEditing="GdvRollWeight_RowEditing"
            OnRowUpdating="GdvRollWeight_RowUpdating">
            <HeaderStyle CssClass="gvheader" />
            <AlternatingRowStyle CssClass="gvalt" />
            <RowStyle CssClass="gvrow" />
            <Columns>
                <asp:TemplateField HeaderText="Roll No">
                    <HeaderStyle Width="50px" HorizontalAlign="Left" />
                    <ItemTemplate>
                    <asp:Label ID="lblRollno" runat="server" align="right" Text='<%#Bind ("rollno") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblRollFrom" runat="server" align="right" Text='<%#Bind ("rollfrom") %>'></asp:Label>- <asp:Label ID="lblRollTo" runat="server" align="right" Text='<%#Bind ("rollTo") %>'></asp:Label>
                    </ItemTemplate>
                   <%-- <EditItemTemplate>
                        <asp:Label ID="lblRollno" runat="server" align="right" Text='<%#Bind ("rollno") %>'></asp:Label>
                    </EditItemTemplate>--%>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Net Weight">
                    <HeaderStyle Width="50px" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="lblNetwt" runat="server" align="right" Text='<%#Bind ("NetWeight") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TxtNetWt" Width="100px" align="right" runat="server" Text='<%#Bind ("NetWeight") %>'
                            onkeypress="return isNumber(event);"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Gross Weight">
                    <HeaderStyle Width="50px" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="lblGrossswt" runat="server" align="right" Text='<%#Bind ("GrossWeight") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TxtGrossswt" Width="100px" align="right" runat="server" Text='<%#Bind ("GrossWeight") %>'
                            onkeypress="return isNumber(event);"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Width">
                    <HeaderStyle Width="50px" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="lblWidth" runat="server" align="right" Text='<%#Bind ("Width") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TxtWidth" Width="100px" align="right" runat="server" Text='<%#Bind ("Width") %>'
                            onkeypress="return isNumber(event);"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Length">
                    <HeaderStyle Width="50px" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="lblLength" runat="server" align="right" Text='<%#Bind ("Length") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TxtLength" Width="100px" align="right" runat="server" Text='<%#Bind ("Length") %>'
                            onkeypress="return isNumber(event);"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Height">
                    <HeaderStyle Width="50px" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="lblHeight" runat="server" align="right" Text='<%#Bind ("Height") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TxtHeight" Width="100px" align="right" runat="server" Text='<%#Bind ("Height") %>'
                            onkeypress="return isNumber(event);"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CBM">
                    <HeaderStyle Width="50px" HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="lblArea" runat="server" align="right" Text='<%#Bind ("Area") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label ID="lblArea" runat="server" align="right" Text='<%#Bind ("Area") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="True" />
            </Columns>
            <EmptyDataRowStyle CssClass="gvemptytext" />
        </asp:GridView>
    </div>
    <div>
        <table>
            <tr>
                <td>
                    Net Weight
                </td>
                <td>
                    <asp:TextBox ID="TxtNetWeight" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                </td>
                <td>
                    Gross Weight
                </td>
                <td>
                    <asp:TextBox ID="TxtGrosWeight" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                </td>
                <td>
                    Area
                </td>
                <td>
                    <asp:TextBox ID="txtArea" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Roll From
                </td>
                <td>
                    Roll To
                </td>
                <td>
                    Gross Weight
                </td>
                <td>
                    Less From GrossWeight
                </td>
                <td>
                    Total Rolls
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="DDRollFrom" runat="server" CssClass="dropdown">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="DDRollTo" runat="server" CssClass="dropdown" OnSelectedIndexChanged="DDRollTo_SelectedIndexChanged"
                        AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox ID="TxtNetWeightB" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="TxtLessGrossWt" runat="server" CssClass="textb" Width="100px"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="TxtTotalRolls" runat="server" CssClass="textb" Width="90px"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="BtnApply" Text="Apply" CssClass="buttonnorm" runat="server" OnClick="BtnApply_Click"
                        OnClientClick="return Validate();" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                    <asp:Button ID="BtnSave" Text="Save" CssClass="buttonnorm" runat="server" OnClick="BtnSave_Click" />
                </td>
                <td>
                    <asp:Button ID="BtnCancel" Text="Cancel" CssClass="buttonnorm" runat="server" OnClientClick="return CloseForm();" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
