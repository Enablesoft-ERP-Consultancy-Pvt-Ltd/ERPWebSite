<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FrmLoommaster.aspx.cs" Inherits="Masters_Campany_FrmLoommaster"
    Title="ADD LOOM DETAIL" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="head1" runat="server">
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Enter" />
    <meta content="blendTrans(Duration=0.5)" http-equiv="Page-Exit" />
    <title></title>
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function closesLoom() {

            self.close();
        }

    </script>
    <script type="text/javascript">
        function validate() {

            if (document.getElementById('DDitem').selectedIndex == 0) {
                alert("Plz Select item");
                document.getElementById('DDitem').focus();
                return false;
            }
        }

        function getbacktostepone() {
            window.location = "frmloommaster.aspx";
        }

        function onSuccess() {
            ta
            setTimeout(okay, 200);
        }
        function onError() {
            setTimeout(getbacktostepone, 200);
        }
        function okay() {
            window.parent.document.getElementById('btnOkay').click();
        }
        function cancel() {
            window.parent.document.getElementById('btnCancel').click();
        }
        function priview() {
            window.open('../../ReportViewer.aspx', '');

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmanager1" runat="server">
    </asp:ScriptManager>
    <div style="width: 1100px">
        <fieldset>
            <legend><font size="3px">Search Looms</font> </legend>
            <div style="float: left; width: 455px">
                <table style="width: 450px">
                    <tr>
                        <td>
                            <asp:RadioButton ID="RdVillage" runat="server" Text="Village/City" Font-Bold="true"
                                GroupName="m" />
                        </td>
                        <td>
                            <asp:RadioButton ID="RDQuality" runat="server" Text="Quality" Font-Bold="true" GroupName="m" />
                        </td>
                        <td>
                            <asp:Label ID="lblContent" runat="server" Text="Enter Content" CssClass="labelbold"></asp:Label><br />
                            <asp:TextBox ID="txtenterContent" runat="server" CssClass="textb" Width="250px" Height="24px"
                                OnTextChanged="txtenterContent_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="float: right; width: 630px; height: 200px; overflow: auto" id="DivLooMDetails"
                visible="false" runat="server">
                <asp:GridView ID="gdLoomDetails" runat="server" ShowFooter="true" AutoGenerateColumns="false"
                    Width="620px">
                    <HeaderStyle ForeColor="white" BackColor="#0080C0" CssClass="gvheader" Height="20px" />
                    <AlternatingRowStyle CssClass="gvalt" />
                    <RowStyle CssClass="gvrow" Height="20px" HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="Weaver" HeaderText="Weaver/Contractor" />
                        <asp:BoundField DataField="Village" HeaderText="Village/City" />
                        <asp:BoundField DataField="QualityName" HeaderText="Quality" />
                        <asp:BoundField DataField="Looms" HeaderText="No.of Looms" />
                        <asp:BoundField DataField="MaxWidth" HeaderText="Max Width" />
                        <asp:BoundField DataField="MinWidth" HeaderText="Min Width" />
                        <asp:BoundField DataField="MaxLength" HeaderText="Max Length" />
                        <asp:TemplateField>
                            <ItemTemplate>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Right" />
                            <FooterTemplate>
                                <asp:Button ID="btnclose" Width="70px" runat="server" Text="Close" CssClass="buttonnorm"
                                    OnClick="btnclose_click" /></FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </fieldset>
    </div>
    <table>
        <tr>
            <td>
                <div style="width: 1100px; height: 100px; margin-top: 20px">
                    <font size="3px" color="teal"><b>ADD WEAVER LOOM DETAILS</b></font>
                    <asp:GridView ID="Gvloom" runat="server" AutoGenerateColumns="false" OnRowDataBound="Gvloom_RowDataBound">
                        <HeaderStyle ForeColor="white" BackColor="#0080C0" CssClass="gvheader" />
                        <AlternatingRowStyle CssClass="gvalt" />
                        <RowStyle CssClass="gvrow" />
                        <%--<HeaderStyle ForeColor="white" BackColor="#0080C0" />--%>
                        <Columns>
                            <asp:TemplateField HeaderText="ITEM" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:DropDownList ID="DDitem" runat="server" Width="150px" OnSelectedIndexChanged="DDitem_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="QUALITY" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:DropDownList ID="DDquality" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="NUMBER OF LOOMS" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtlooms" runat="server" Text='<%#Eval("looms") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="MIN_WIDTH" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtminwidth" runat="server" Text='<%#Eval("min_width") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="MAX_WIDTH" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtmaxwidth" runat="server" Text='<%#Eval("max_width") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="MAX _<u>LENGTH</u>" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtmaxlength" runat="server" Text='<%#Eval("max_length") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="UNIT</br><u>CM/METER/INCH/FEET</u>" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <%--<asp:TextBox ID="txtunit" runat="server" Text='<%#Eval("unit") %>'></asp:TextBox>--%>
                                    <asp:DropDownList ID="DDunit" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblErr" runat="server" CssClass="labelbold" ForeColor="Red" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="buttonnorm" Width="70px"
                    OnClientClick=" return validate();" OnClick="BtnSave_Click" />
                <asp:Button ID="Btnclose" runat="server" Text="Close" CssClass="buttonnorm" Width="70px"
                    OnClientClick="return closesLoom();" />
            </td>
        </tr>
        <tr>
            <td>
                <div style="width: 1100px; height: 100px; margin-top: 20px" id="DivEdit" runat="server">
                    <font size="3px" color="teal"><b>EDIT WEAVER LOOM DETAIL</b></font>
                    <asp:GridView ID="DGLoomDetils" runat="server" AutoGenerateColumns="False" DataKeyNames="DetailId"
                        OnRowCancelingEdit="DGLoomDetils_RowCancelingEdit" OnRowEditing="DGLoomDetils_RowEditing"
                        OnRowUpdating="DGLoomDetils_RowUpdating" OnRowDeleting="DGLoomDetils_RowDeleting"
                        OnRowDataBound="DGLoomDetils_RowDataBound">
                        <HeaderStyle ForeColor="white" BackColor="#0080C0" CssClass="gvheader" />
                        <AlternatingRowStyle CssClass="gvalt" />
                        <RowStyle CssClass="gvrow" Height="25px" />
                        <HeaderStyle ForeColor="white" BackColor="#0080C0" />
                        <Columns>
                            <asp:TemplateField HeaderText="ITEM" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:DropDownList ID="DDitem" runat="server" Width="150px" OnSelectedIndexChanged="DDitem_SelectedIndexChangedForEdit"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <%--                                <EditItemTemplate>
                                    <asp:DropDownList ID="DDitem" runat="server" Width="150px" Selectedvalue='<%#Bind("ItemId") %>'>
                                    </asp:DropDownList>
                                </EditItemTemplate>--%>
                                <HeaderStyle Width="15%"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="QUALITY" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:DropDownList ID="DDquality" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <%--                                <EditItemTemplate>
                                    <asp:DropDownList ID="DDquality" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </EditItemTemplate>--%>
                                <HeaderStyle Width="15%"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="NUMBER OF LOOMS" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <%#Eval("looms") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtloom" runat="server" Text='<%#Eval("looms") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <HeaderStyle Width="15%"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="MIN_WIDTH" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <%#Eval("min_width") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtminwidth" runat="server" Text='<%#Eval("min_width") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <HeaderStyle Width="15%"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="MAX_WIDTH" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <%#Eval("max_width") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtmaxwidth" runat="server" Text='<%#Eval("max_width") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <HeaderStyle Width="15%"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="MAX _<u>LENGTH</u>" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <%#Eval("max_length") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtmaxlength" runat="server" Text='<%#Eval("max_length") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <HeaderStyle Width="15%"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="UNIT</br><u>CM/METER/INCH/FEET</u>" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:DropDownList ID="DDunit" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <%--<asp:Label ID="lblunit" runat="server" Text='<%#Bind("unitname") %>'></asp:Label>--%>
                                <%-- <EditItemTemplate>
                                    <asp:DropDownList ID="DDunit" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </EditItemTemplate>--%>
                                <HeaderStyle Width="15%"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ItemId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblItemId" runat="server" Text='<%#Bind("ItemId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="QualityId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblQualityId" runat="server" Text='<%#Bind("QualityId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="UnitId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblUnitId" runat="server" Text='<%#Bind("UnitId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="True" />
                            <asp:CommandField ShowDeleteButton="True" />
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
