<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="FrmSizeNew.aspx.cs" Inherits="Masters_Carpet_FrmSizeNew" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../../Scripts/FixFocus2.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function priview() {
            window.open("../../ReportViewer.aspx");
        }
        function doConfirm() {
            var r = confirm("Do you want to delete ?");
            document.getElementById("CPH_Form_hnsst").value = r;
            if (r == true) {
                x = "You pressed OK!";
                return true;
            }
            else {
                x = "You pressed Cancel!";
                return false;
            }
            //alert(x);
        }
        function validate() {
            if (document.getElementById("<%=ddunittype.ClientID %>").value <= "0") {
                alert("Pls Select Unit Name");
                document.getElementById("<%=ddunittype.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=tdUnit1.ClientID %>")) {
                if (document.getElementById('CPH_Form_ddunit').value <= "0") {
                    alert("Please Select Unit Name....!");
                    document.getElementById("CPH_Form_ddunit").focus();
                    return false;
                }
            }
            else {
                alert("Please Select Unit Name Area....!");
                return false;
            }

            if (document.getElementById("<%=ddshape.ClientID %>")) {
                if (document.getElementById("<%=ddshape.ClientID %>").value <= "0") {
                    alert("plz Select Shape");
                    document.getElementById("<%=ddshape.ClientID %>").focus();
                    return false;
                }
            }
            else {
                alert("Please Select Unit Name Area....!");
                return false;
            }
            if (document.getElementById("<%=txtAreaFt.ClientID %>").value == "") {
                alert("Area  Cann't be blank");
                document.getElementById("<%=txtAreaFt.ClientID %>").focus();
                return false;
            }
            else {
                return confirm('Do You Want To Save?')
            }
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            else {
                return true;
            }
        }
        function Newbtn() {
            document.getElementById("<%=txtVolFt.ClientID %>").value = "";
            document.getElementById("<%=txtwidthFt.ClientID %>").value == ""
            document.getElementById("<%=txtlengthFt.ClientID %>").value == ""
            document.getElementById("<%=txtheightFt.ClientID %>").value == ""
            document.getElementById("<%=txtAreaFt.ClientID %>").value == ""
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="height: 450px">
                <table>
                    <tr>
                        <td class="tdstyle">
                            Unit Type
                        </td>
                        <td>
                            <asp:DropDownList ID="ddunittype" runat="server" OnSelectedIndexChanged="ddunit_SelectedIndexChanged"
                                AutoPostBack="True" CssClass="dropdown" Width="100px" TabIndex="1">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender3" runat="server" TargetControlID="ddunittype"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle" id="tdUnit" runat="server">
                            Unit Name
                        </td>
                        <td id="tdUnit1" runat="server">
                            <asp:DropDownList ID="ddunit" runat="server" AutoPostBack="True" CssClass="dropdown"
                                Width="100px" TabIndex="1" OnSelectedIndexChanged="ddunit_SelectedIndexChanged1">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddunit"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblshapeyname" runat="server" Text="Shape"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddshape" runat="server" AutoPostBack="True" Width="100px" CssClass="dropdown"
                                TabIndex="2" OnSelectedIndexChanged="ddshape_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddshape"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr id="trsize" runat="server">
                        <td class="tdstyle">
                            Width
                        </td>
                        <td>
                            <asp:TextBox ID="txtwidthFt" runat="server" CssClass="textb" OnTextChanged="txtwidthFt_TextChanged"
                                onkeypress="return isNumberKey(event);" AutoPostBack="True" Width="80px" TabIndex="3"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Length
                        </td>
                        <td>
                            <asp:TextBox ID="txtlengthFt" runat="server" CssClass="textb" OnTextChanged="txtlengthFt_TextChanged"
                                onkeypress="return isNumberKey(event);" AutoPostBack="True" Width="80px" TabIndex="4"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Height
                        </td>
                        <td>
                            <asp:TextBox ID="txtheightFt" runat="server" CssClass="textb" OnTextChanged="txtheightFt_TextChanged"
                                onkeypress="return isNumberKey(event);" AutoPostBack="True" Width="80px" TabIndex="5"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Area
                        </td>
                        <td>
                            <asp:TextBox ID="txtAreaFt" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Volume
                        </td>
                        <td>
                            <asp:TextBox ID="txtVolFt" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            Size Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtsizename" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblMessage" ForeColor="Red" runat="server" Text=""></asp:Label>
                        </td>
                        <td id="Td1" colspan="10" runat="server" align="right">
                            <asp:Button ID="BtnNew" CssClass="buttonnorm" Text="New" runat="server" OnClientClick="return Newbtn();"
                                TabIndex="13" />
                            <asp:Button ID="btnSave" CssClass="buttonnorm" runat="server" OnClientClick="return validate();"
                                OnClick="btnSave_Click" Text="Save" TabIndex="13" />
                            <asp:Button ID="btnpreview" runat="server" Text="Preview" OnClientClick="return Preview();"
                                CssClass="buttonnorm" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm();"
                                TabIndex="14" Text="Close" />
                            <asp:HiddenField ID="hnsst" runat="server" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td id="td2" runat="server" colspan="10">
                            <asp:Label ID="lbl" runat="server" ForeColor="Red"></asp:Label>
                            <div id="Gride" style="overflow: scroll; width: 900px; height: 250px">
                                <asp:GridView ID="gdSize" runat="server" OnRowDataBound="gdSize_RowDataBound" OnSelectedIndexChanged="gdSize_SelectedIndexChanged"
                                    DataKeyNames="SIZEID" CssClass="grid-view" OnRowCreated="gdSize_RowCreated" AutoGenerateColumns="false"
                                    OnRowDeleting="gdSize_RowDeleting1">
                                    <Columns>
                                        <asp:BoundField DataField="SizeName" HeaderText="SizeName">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Width" HeaderText="Width">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Length" HeaderText="Length">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Area" HeaderText="Area">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Height" HeaderText="Heigth">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Volume" HeaderText="Volume">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UnitName" HeaderText="Unit">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UnitType" HeaderText="UnitType">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:BoundField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    OnClientClick="return doConfirm();" Text="Del"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
