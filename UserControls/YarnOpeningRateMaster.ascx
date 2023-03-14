<%@ Control Language="C#" AutoEventWireup="true" CodeFile="YarnOpeningRateMaster.ascx.cs"
    Inherits="UserControls_YarnOpeningRateMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<script src="../../Scripts/JScript.js" type="text/javascript"></script>
<script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
<script type="text/javascript">
    function closeForm() {

        var objParent = window.opener;
        if (objParent != null) {
            self.close();
        }
        else {
            window.location.href = "../../main.aspx";
        }
    }
    function Jscriptvalidate() {
        $(document).ready(function () {
            $("#<%=btnsave.ClientID %>").click(function () {
                var Message = "";
                var selectedindex = $("#<%=DDcategoryName.ClientID %>").attr('selectedIndex');
                if (selectedindex <= 0) {
                    Message = Message + "Please select Category Name!!\n";
                }
                var selectedindex = $("#<%=DDitemname.ClientID %>").attr('selectedIndex');
                if (selectedindex <= 0) {
                    Message = Message + "Please select Item Name!!\n";
                }
                var selectedindex = $("#<%=DDquality.ClientID %>").attr('selectedIndex');
                if (selectedindex <= 0) {
                    Message = Message + "Please select Quality Name!!\n";
                }
                var selectedindex = $("#<%=DDjobname.ClientID %>").attr('selectedIndex');
                if (selectedindex <= 0) {
                    Message = Message + "Please select Job Name!!\n";
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
    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        else {
            return true;
        }
    }
</script>
<asp:UpdatePanel ID="upd1" runat="server">
    <ContentTemplate>
        <script type="text/javascript" language="javascript">
            Sys.Application.add_load(Jscriptvalidate);
        </script>
        <div style="margin: 0% 20% 0% 20%">
            <div style="width: 75%">
                <table border="1" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="Label10" Text="Category Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDcategoryName" CssClass="dropdown" runat="server" AutoPostBack="true"
                                Width="200px" OnSelectedIndexChanged="DDcategoryName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblitemName" Text="Item Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDitemname" CssClass="dropdown" runat="server" AutoPostBack="true"
                                Width="200px" OnSelectedIndexChanged="DDitemname_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label1" Text="Quality Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDquality" CssClass="dropdown" runat="server" AutoPostBack="true"
                                Width="200px" OnSelectedIndexChanged="DDquality_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label7" Text="Shade Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDShade" CssClass="dropdown" runat="server" Width="200px" AutoPostBack="true"
                                OnSelectedIndexChanged="DDShade_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label2" Text="Job Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDjobname" CssClass="dropdown" runat="server" AutoPostBack="true"
                                Width="200px" OnSelectedIndexChanged="DDjobname_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label8" Text="Emp Name" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDEmpName" CssClass="dropdown" runat="server" Width="200px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td id="TDconetype" runat="server">
                            <asp:Label ID="Label9" Text="Cone type" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDConetype" CssClass="dropdown" runat="server" Width="200px"
                                AutoPostBack="true" OnSelectedIndexChanged="DDConetype_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label11" Text="Ply Type" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDPly" CssClass="dropdown" runat="server" Width="200px" AutoPostBack="true"
                                OnSelectedIndexChanged="DDPly_SelectedIndexChanged">
                                <asp:ListItem Text="" />
                                <asp:ListItem Text="1 Ply" />
                                <asp:ListItem Text="2 Ply" />
                                <asp:ListItem Text="3 Ply" />
                                <asp:ListItem Text="4 Ply" />
                                <asp:ListItem Text="5 Ply" />
                                <asp:ListItem Text="6 Ply" />
                                <asp:ListItem Text="7 Ply" />
                                <asp:ListItem Text="8 Ply" />
                                <asp:ListItem Text="9 Ply" />
                                <asp:ListItem Text="10 Ply" />
                                <asp:ListItem Text="11 Ply" />
                                <asp:ListItem Text="12 Ply" />
                                <asp:ListItem Text="8-32 Ply" />
                                <asp:ListItem Text="30 Ply" />
                                <asp:ListItem Text="15 Ply" />
                                <asp:ListItem Text="21 Ply" />
                                <asp:ListItem Text="13 Ply" />
                                <asp:ListItem Text="14 Ply" />
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label12" Text="Tranport" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:DropDownList ID="DDTransport" CssClass="dropdown" runat="server" Width="200px"
                                AutoPostBack="true" OnSelectedIndexChanged="DDTransport_SelectedIndexChanged">
                                <asp:ListItem Text="" />
                                <asp:ListItem Text="Self" />
                                <asp:ListItem Text="Company" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label14" Text="Remark" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="TxtRemark" CssClass="textb" Width="200px" runat="server" />
                        </td>
                    </tr>
                </table>
                <fieldset>
                    <legend>
                        <asp:Label ID="lblrrate" CssClass="labelbold" Text="Rate" runat="server" ForeColor="Red" />
                    </legend>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblRate" Text="Rate(Rs.)" CssClass="labelbold" runat="server" />
                                <br />
                                <asp:TextBox ID="txtRate" CssClass="textb" Width="120px" runat="server" onkeypress="return isNumberKey(event);" />
                            </td>
                            <td>
                                <asp:Label ID="Label3" Text="OutSide Rate(Rs.)" CssClass="labelbold" runat="server" />
                                <br />
                                <asp:TextBox ID="TxtORate" CssClass="textb" Width="140px" runat="server" onkeypress="return isNumberKey(event);" />
                            </td>
                            <td>
                                <asp:Label ID="Label4" Text="Effective Date" CssClass="labelbold" runat="server" />
                                <br />
                                <asp:TextBox ID="txteffectivedate" CssClass="textb" Width="120px" runat="server" />
                                <asp:CalendarExtender ID="cal1" runat="server" TargetControlID="txteffectivedate"
                                    Format="dd-MMM-yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <table>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnnew" runat="server" CssClass="buttonnorm" Text="New" OnClick="btnnew_Click" />
                            <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" OnClick="btnsave_Click" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="closeForm();" />
                        </td>
                    </tr>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblmsg" ForeColor="Red" CssClass="labelbold" Text="" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div>
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <div style="overflow: auto; max-height: 500px">
                            <asp:GridView ID="DgDetail" CssClass="grid-views" runat="server" AutoGenerateColumns="false"
                                EmptyDataText="No records found." OnRowDataBound="DgDetail_RowDataBound" AutoGenerateSelectButton="true"
                                OnSelectedIndexChanged="DgDetail_SelectedIndexChanged">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr No.">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblitemName" Text='<%#Bind("Item_name") %>' runat="server" CssClass="labelbold" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quality Name">
                                        <ItemTemplate>
                                            <asp:Label ID="Label5" Text='<%#Bind("Qualityname") %>' runat="server" CssClass="labelbold" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Shade Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblshade" Text='<%#Bind("shadecolorname") %>' runat="server" CssClass="labelbold" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Job Name">
                                        <ItemTemplate>
                                            <asp:Label ID="Label6" Text='<%#Bind("Process_name") %>' runat="server" CssClass="labelbold" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="EmpName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmpName" Text='<%#Bind("EmpName") %>' runat="server" CssClass="labelbold" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblrate" Text='<%#Bind("Rate") %>' runat="server" CssClass="labelbold" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Outside Rate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblorate" Text='<%#Bind("ORate") %>' runat="server" CssClass="labelbold" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cone Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblConeType" Text='<%#Bind("ConeType") %>' runat="server" CssClass="labelbold" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ply Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPlyType" Text='<%#Bind("PlyType") %>' runat="server" CssClass="labelbold" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Transport">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTransportType" Text='<%#Bind("TransportType") %>' runat="server"
                                                CssClass="labelbold" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Effective From">
                                        <ItemTemplate>
                                            <asp:Label ID="lbleffectivefrom" Text='<%#Bind("Effectivedate") %>' runat="server"
                                                CssClass="labelbold" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Effective To">
                                        <ItemTemplate>
                                            <asp:Label ID="Label9" Text='<%#Bind("Todate") %>' runat="server" CssClass="labelbold" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remark">
                                        <ItemTemplate>
                                            <asp:Label ID="LblRemark" Text='<%#Bind("Remark") %>' runat="server" CssClass="labelbold" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblitemid" Text='<%#Bind("Itemid") %>' runat="server" CssClass="labelbold" />
                                            <asp:Label ID="lblqualityid" Text='<%#Bind("Qualityid") %>' runat="server" CssClass="labelbold" />
                                            <asp:Label ID="lblprocessid" Text='<%#Bind("processid") %>' runat="server" CssClass="labelbold" />
                                            <asp:Label ID="lblid" Text='<%#Bind("id") %>' runat="server" CssClass="labelbold" />
                                            <asp:Label ID="lblcategoryid" Text='<%#Bind("categoryid") %>' runat="server" CssClass="labelbold" />
                                            <asp:Label ID="lblshadecolorid" Text='<%#Bind("shadecolorid") %>' runat="server"
                                                CssClass="labelbold" />
                                            <asp:Label ID="lblEmpId" Text='<%#Bind("EmpId") %>' runat="server" CssClass="labelbold" />
                                            <%--<asp:Label ID="lblORate" Text='<%#Bind("ORate") %>' runat="server" CssClass="labelbold" />
                                            <asp:Label ID="lblConeType" Text='<%#Bind("CONETYPE") %>' runat="server" CssClass="labelbold" />
                                            <asp:Label ID="lblPlyType" Text='<%#Bind("PlyType") %>' runat="server" CssClass="labelbold" />
                                            <asp:Label ID="lblTransportType" Text='<%#Bind("TransportType") %>' runat="server"
                                                CssClass="labelbold" />--%>
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
