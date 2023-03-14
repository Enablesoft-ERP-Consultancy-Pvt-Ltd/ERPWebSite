<%@ Page Title="Loom Master" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmLoomMaster.aspx.cs" Inherits="Masters_Loom_frmLoomMaster" %>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script>
        function NewForm() {
            window.location.href = "frmLoomMaster.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=BtnSave.ClientID %>").click(function () {
                    var Message = "";
                    var selectedindex = $("#<%=DDcompany.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Company Name!!\n";
                    }
                    selectedindex = $("#<%=DDunitname.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Unit Name. !!\n";
                    }
                    selectedindex = $("#<%=DDitem.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Technique !!\n";
                    }
                    selectedindex = $("#<%=DDshape.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Shape !!\n";
                    }
                    selectedindex = $("#<%=DDSize.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Size !!\n";
                    }
                    var txtLoomNo = document.getElementById('<%=txtLoomNo.ClientID %>');
                    if (txtLoomNo.value == "" || txtLoomNo.value == "0") {
                        Message = Message + "Please Enter Loom No. !!\n";
                    }

                    if (Message == "") {
                        return true;
                    }
                    else {
                        alert(Message);
                        return false;
                    }
                });

            });
        }
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>
            <div style="margin: 0% 10% 0% 20%">
                <fieldset>
                    <legend>
                        <asp:Label ID="lbl" CssClass="labelbold" ForeColor="Red" Text="Loom Detail" runat="server" />
                    </legend>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" CssClass="labelbold" Text="Company Name" runat="server" />
                                <br />
                                <asp:DropDownList ID="DDcompany" CssClass="dropdown" Width="150px" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" CssClass="labelbold" Text="Unit Name" runat="server" />
                                <br />
                                <asp:DropDownList ID="DDunitname" CssClass="dropdown" Width="150px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDunitname_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label3" CssClass="labelbold" Text="Technique" runat="server" />
                                <br />
                                <asp:DropDownList ID="DDitem" CssClass="dropdown" Width="150px" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="DDitem_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" CssClass="labelbold" Text="Shape" runat="server" />
                                <br />
                                <asp:DropDownList ID="DDshape" CssClass="dropdown" Width="150px" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="DDshape_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDSize" runat="server" class="tdstyle">
                                <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged">
                                </asp:DropDownList>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDSize" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DDSize_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Loom No." CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtLoomNo" Width="140px" CssClass="textb" runat="server" />
                            </td>
                            <td id="TDSuperVisorName" runat="server" visible="false">
                                <asp:Label ID="Label7" runat="server" Text="Supervisor Name." CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDSupervisorName" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Loom Type" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDLoomType" runat="server" CssClass="dropdown">
                                    <asp:ListItem Text="New" />
                                    <asp:ListItem Text="Old" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <table style="width: 100%">
                    <tr>
                        <td colspan="8" align="right">
                            <asp:Button CssClass="buttonnorm" ID="btnnew" runat="server" Text="New" OnClientClick="return NewForm();" />
                            <asp:Button CssClass="buttonnorm" ID="BtnSave" runat="server" Text="Save" ValidationGroup="f1"
                                OnClientClick="return Validation();" Width="50px" OnClick="BtnSave_Click" />
                            <%-- <asp:Button CssClass="buttonnorm" ID="BtnPreview" runat="server" Text="Preview" OnClick="BtnPreview_Click" />--%>
                            <asp:Button CssClass="buttonnorm" ID="BtnClose" OnClientClick="return CloseForm();"
                                runat="server" Text="Close" Width="50px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" runat="server" />
                        </td>
                    </tr>
                </table>
                <div>
                    <table width="100%">
                        <tr>
                            <td>
                                <div style="max-height: 300px; overflow: auto">
                                    <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                        EmptyDataText="No records found." OnRowDataBound="DG_RowDataBound">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Unit Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUnitName" Text='<%#Bind("Unitname") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Technique">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemname" Text='<%#Bind("Item_name") %>' runat="server" Width="200px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Size">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSize" Text='<%#Bind("Size") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Loom No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLoomNo" Text='<%#Bind("LoomNo") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Loom Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLoomType" Text='<%#Bind("LoomType") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Supervisor Name" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSupervisorName" Text='<%#Bind("SupervisorName") %>' runat="server" />
                                                    <asp:Label ID="lblSupervisorId" Text='<%#Bind("SupervisorId") %>' runat="server"
                                                        Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbluid" Text='<%#Bind("uid") %>' runat="server" />
                                                    <asp:Label ID="lblcompanyid" Text='<%#Bind("companyid") %>' runat="server" />
                                                    <asp:Label ID="lblunitid" Text='<%#Bind("unitid") %>' runat="server" />
                                                    <asp:Label ID="lblitemid" Text='<%#Bind("itemid") %>' runat="server" />
                                                    <asp:Label ID="lblshapeid" Text='<%#Bind("shapeid") %>' runat="server" />
                                                    <asp:Label ID="lblsizeid" Text='<%#Bind("sizeid") %>' runat="server" />
                                                    <asp:Label ID="lblsizetype" Text='<%#Bind("flagsize") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbEdit" runat="server" OnClick="lbEdit_Click" ToolTip="Edit"
                                                        CausesValidation="False">Edit</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lblDel" runat="server" OnClick="lbDelete_Click" ToolTip="Delete"
                                                        OnClientClick="return confirm('Do you want to delete this row?');" CausesValidation="False">Delete</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Enable/Disable">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkLoomMaster_ED" Text='<%#Bind("Status") %>' runat="server" OnClick="lnkLoomMaster_ED"
                                                        OnClientClick="return confirm('Do you want to Enable_Disable Quality')" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLoomMasterEnable_Disable" Text='<%#Bind("EnableDisableStatus") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:HiddenField ID="hnuid" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
