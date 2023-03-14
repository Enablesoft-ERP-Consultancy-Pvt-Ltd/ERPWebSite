<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    EnableEventValidation="false" CodeFile="FrmCommission.aspx.cs" Inherits="Masters_Carpet_FrmCommission" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript">
    </script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function priview() {
            document.getElementById('BtnPreview').click();
            window.open('../../ReportViewer.aspx');
        }
        function New() {
            window.location.href = "FrmCommission.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function validate() {
            var VarCategory = document.getElementById('ddCategoryName').value;
            var VarItem = document.getElementById('ddItemName').value;
            var VarCommission = document.getElementById('TxtCommission').value;
            if (document.getElementById("<%=ddCategoryName.ClientID %>").value == "0") {
                alert("Challan No. Cannot Be Blank");
                document.getElementById("<%=ddCategoryName.ClientID %>").focus();
                return false;
            }
            if (document.getElementById('ddQuality').style.visibility == 'visible') {
                var VarQuality = document.getElementById('ddQuality').value;
            }
            if (VarCategory == "0") {
                alert("Pls Select Category Name");
                var e = document.getElementById('ddCategoryName');
                var strUser = e.options[e.selectedIndex].value;
                document.getElementById('ddCategoryName').value = strUser;
                document.getElementById('ddCategoryName').focus();
                return false;
            }
            else if (VarItem == "0") {
                alert("Pls Select Item Name");
                var e = document.getElementById('ddItemName');
                var strUser = e.options[e.selectedIndex].value;
                document.getElementById('ddItemName').value = strUser;
                document.getElementById('ddItemName').focus();
                return false;
            }
            else if (VarQuality == "0") {
                alert("Pls Select Quality");
                var e = document.getElementById('ddQuality');
                var strUser = e.options[e.selectedIndex].value;
                document.getElementById('ddQuality').value = strUser;
                document.getElementById('ddQuality').focus();
                return false;
            }
            else if (VarCommission == "") {
                alert("Pls Fill Commission");
                var e = document.getElementById('TxtCommission');
                var strUser = e.options[e.selectedIndex].value;
                document.getElementById('TxtCommission').value = strUser;
                document.getElementById('TxtCommission').focus();
                return false;
            }
            else {
                return confirm('Do You Want To Save?')
            }
        }
        function validate1() {
            if (document.getElementById("<%=ddCategoryName.ClientID %>").value == "0") {
                alert("Pls Select Category Name");
                document.getElementById("<%=ddCategoryName.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=ddItemName.ClientID %>").value == "0") {
                alert("Pls Select Item Name");
                document.getElementById("<%=ddItemName.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=ddQuality.ClientID %>").value == "0") {
                alert("Pls Select Quality");
                document.getElementById("<%=ddQuality.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=TxtCommission.ClientID %>").value == "") {
                alert("Pls Fill Commission");
                document.getElementById("<%=TxtCommission.ClientID %>").focus();
                return false;
            }
            else {
                return confirm('Do You Want To Save?')
            }
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td class="tdstyle">
                        <asp:Label ID="lblcategoryname" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddCategoryName" runat="server" OnSelectedIndexChanged="ddcategoryname_SelectedIndexChanged"
                            Width="200px" AutoPostBack="True" TabIndex="2" CssClass="dropdown">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddCategoryName"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="lblitemname" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddItemName" runat="server" OnSelectedIndexChanged="dditemname_SelectedIndexChanged"
                            Width="200px" AutoPostBack="True" TabIndex="4" CssClass="dropdown">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddItemName"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td id="Quality" runat="server" visible="false" class="tdstyle">
                        <asp:Label ID="lblqualityname" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddQuality" runat="server" Width="200px" TabIndex="6" AutoPostBack="True"
                            CssClass="dropdown" OnSelectedIndexChanged="ddQuality_SelectedIndexChanged">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddQuality"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td id="Design" runat="server" visible="false" class="tdstyle">
                        <asp:Label ID="lbldesignname" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddDesign" runat="server" Width="200px" TabIndex="7" AutoPostBack="True"
                            CssClass="dropdown">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender4" runat="server" TargetControlID="ddDesign"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>
                    </td>
                    <td class="tdstyle">
                        <asp:Label ID="LblCommission" runat="server" Text="Commission"></asp:Label>
                        <br />
                        <asp:TextBox ID="TxtCommission" runat="server" Width="100px" CssClass="textb" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="5" align="right">
                        <asp:Label ID="LblErrorMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
                        <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="BtnSave_Click"
                            OnClientClick="return validate1();" />
                        &nbsp;<asp:Button ID="BtnNew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return New()" />
                        &nbsp;<asp:Button ID="BtncClose" runat="server" CssClass="buttonnorm" Text="Close"
                            OnClientClick="return CloseForm();" />
                        &nbsp;<asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" Text="Preview"
                            OnClientClick="return priview();" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <div style="width: 100%; height: 200px; overflow: auto;">
                            <asp:GridView ID="DG" runat="server" DataKeyNames="ID" AutoGenerateColumns="False"
                                OnRowDeleting="DG_RowDeleting" CssClass="grid-views">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <Columns>
                                    <asp:BoundField DataField="Category" HeaderText="Category Name">
                                        <HeaderStyle Width="200px" HorizontalAlign="Left" />
                                        <ItemStyle Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ItemName" HeaderText="Item Name">
                                        <HeaderStyle Width="200px" HorizontalAlign="Left" />
                                        <ItemStyle Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Quality" HeaderText="Quality">
                                        <HeaderStyle Width="150px" HorizontalAlign="Left" />
                                        <ItemStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Design" HeaderText="Design">
                                        <HeaderStyle Width="150px" HorizontalAlign="Left" />
                                        <ItemStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Commission" HeaderText="Commission">
                                        <HeaderStyle Width="100px" HorizontalAlign="Right" />
                                        <ItemStyle Width="100px" HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                Text="Del" OnClientClick="return confirm('Do You Want To Delete Data?')"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="50px" />
                                        <ItemStyle HorizontalAlign="Right" Width="50px" />
                                    </asp:TemplateField>
                                </Columns>
                                <SelectedRowStyle CssClass="SelectedRowStyle" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
