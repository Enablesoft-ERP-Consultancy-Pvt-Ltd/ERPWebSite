<%@ Page Language="C#" Title="User Authentication" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="UserRights.aspx.cs" Inherits="UserRigets" %>

<asp:Content ID="Page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function closeform() {
            window.location.href = "main.aspx";
        }
        function NewForm() {
            window.location.href = "UserRights.aspx";
        }
        function AllCompany() {

            if (document.getElementById('CPH_Form_ChkForAllCompany').checked == true) {
                var gvcheck = document.getElementById('CPH_Form_CHKCompany');
                var i;
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
            else {
                var gvcheck = document.getElementById('CPH_Form_CHKCompany');
                var i;
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
        }
        function AllProcess() {
            if (document.getElementById('CPH_Form_ChkForAllProcess').checked == true) {
                var gvcheck = document.getElementById('CPH_Form_ChLProcess');
                var i;
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
            else {
                var gvcheck = document.getElementById('CPH_Form_ChLProcess');
                var i;
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
        }
        function AllCategory() {
            if (document.getElementById('CPH_Form_ChkForAllCategory').checked == true) {
                var gvcheck = document.getElementById('CPH_Form_ChlCategory');
                var i;
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
            else {
                var gvcheck = document.getElementById('CPH_Form_ChlCategory');
                var i;
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
        }
        function AllUnits() {

            if (document.getElementById('CPH_Form_chkallunits').checked == true) {
                var gvcheck = document.getElementById('CPH_Form_chkunits');
                var i;
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
            else {
                var gvcheck = document.getElementById('CPH_Form_chkunits');
                var i;
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
        }
        function AllCustomerCode() {
            if (document.getElementById('CPH_Form_ChkAllCustomer').checked == true) {
                var gvcheck = document.getElementById('CPH_Form_ChkForCustomerCode');
                var i;
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
            else {
                var gvcheck = document.getElementById('CPH_Form_ChkForCustomerCode');
                var i;
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
        }
        function AllBranch() {
            if (document.getElementById('CPH_Form_ChkAllBranch').checked == true) {
                var gvcheck = document.getElementById('CPH_Form_ChkForBranch');
                var i;
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
            else {
                var gvcheck = document.getElementById('CPH_Form_ChkForBranch');
                var i;
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
        }
        function AllGodown() {
            if (document.getElementById('CPH_Form_ChkAllGodown').checked == true) {
                var gvcheck = document.getElementById('CPH_Form_ChkGodown');
                var i;
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
            else {
                var gvcheck = document.getElementById('CPH_Form_ChkGodown');
                var i;
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
        }
    </script>
    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <div>
                <table>
                    <tr>
                        <td>
                            <b>
                                <asp:Label Text="Company Name" runat="server" ID="lbl" CssClass="labelbold" />
                            </b>
                            <br />
                            <asp:DropDownList ID="DDlCompanyName" runat="server" AutoPostBack="true" CssClass="dropdown"
                                OnSelectedIndexChanged="DDlCompanyName_SelectedIndexChanged" Width="350px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <b>
                                <asp:Label Text="User Name" runat="server" ID="Label1" CssClass="labelbold" />
                            </b>
                            <br />
                            <asp:DropDownList ID="DDUserName" runat="server" AutoPostBack="true" CssClass="dropdown"
                                OnSelectedIndexChanged="DDUserName_SelectedIndexChanged" Width="250px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="LblErr" runat="server" ForeColor="Red" Text="" CssClass="dropdown"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td id="Td1" runat="server" align="right" colspan="2">
                            <asp:Button ID="BtnNew" runat="server" CssClass="buttonnorm" OnClientClick="return NewForm();"
                                Text="New" />
                            <asp:Button ID="BtnSave" runat="server" CssClass="buttonnorm" OnClick="BtnSave_Click"
                                OnClientClick="return confirm('Do you want to save data?')" Text="Save" />
                            <asp:Button ID="BtnClose" runat="server" CssClass="buttonnorm" OnClientClick="return closeform();"
                                Text="Close" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td valign="top">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="ChkForAllCompany" runat="server" Text=" Chk For All Company" ForeColor="Red"
                                            onclick="return AllCompany();" CssClass="checkboxbold" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="overflow: scroll; height: 50px; width: 250PX">
                                            <asp:CheckBoxList ID="CHKCompany" CssClass="checkboxbold" runat="server">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="ChkForAllProcess" runat="server" Text=" Chk For All Process" ForeColor="Red"
                                            onclick="return AllProcess();" CssClass="checkboxbold" />
                                    </td>
                                </tr>
                                <tr id="TRChLProcess" runat="server">
                                    <td rowspan="8" valign="top" style="margin-top: 0px">
                                        <div style="overflow: scroll; height: 150px; width: 250PX">
                                            <asp:CheckBoxList ID="ChLProcess" CssClass="checkboxbold" runat="server">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="ChkForAllCategory" runat="server" Text=" Chk For All Category"
                                            ForeColor="Red" onclick="return AllCategory();" CssClass="checkboxbold" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="overflow: auto; height: 150px; width: 250PX">
                                            <asp:CheckBoxList ID="ChlCategory" runat="server" CssClass="checkboxbold">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkallunits" runat="server" Text=" Chk For All Units" ForeColor="Red"
                                            CssClass="checkboxbold" onclick="return AllUnits();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="overflow: scroll; height: 140px; width: 250PX">
                                            <asp:CheckBoxList ID="chkunits" CssClass="checkboxbold" runat="server">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td id="tdunits" runat="server" visible="false" valign="top">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="ChkAllGodown" runat="server" Text=" Chk For All Godown" ForeColor="Red"
                                            onclick="return AllGodown();" CssClass="checkboxbold" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="overflow: scroll; height: 150px; width: 250PX">
                                            <asp:CheckBoxList ID="ChkGodown" CssClass="checkboxbold" runat="server">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="ChkAllBranch" runat="server" Text=" Chk For All Branch" ForeColor="Red"
                                            onclick="return AllBranch();" CssClass="checkboxbold" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="overflow: scroll; height: 150px; width: 250PX">
                                            <asp:CheckBoxList ID="ChkForBranch" CssClass="checkboxbold" runat="server">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="ChkAllCustomer" runat="server" Text=" Chk For All Customer" ForeColor="Red"
                                            CssClass="checkboxbold" onclick="return AllCustomerCode();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="overflow: scroll; height: 140px; width: 250PX">
                                            <asp:CheckBoxList ID="ChkForCustomerCode" CssClass="checkboxbold" runat="server">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <div id="DivMenu" runat="server" style="height: 410px; overflow: scroll; width: 350px"
                                            visible="false">
                                            <asp:TreeView ID="TVMenues" runat="server" ShowCheckBoxes="All" EnableTheming="True"
                                                ShowLines="true" ExpandDepth="1">
                                                <Nodes>
                                                    <asp:TreeNode SelectAction="Expand"></asp:TreeNode>
                                                </Nodes>
                                            </asp:TreeView>
                                        </div>
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
