<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmwarpIssue.aspx.cs" Inherits="Masters_WARP_frmwarpIssue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmwarpIssue.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            else {
                return true;
            }
        }   
    </script>
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=btnsave.ClientID %>").click(function () {
                    var Message = "";
                    var selectedindex = $("#<%=DDcompany.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select Company Name!!\n";
                    }
                    selectedindex = $("#<%=DDDept.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Dept. !!\n";
                    }
                    selectedindex = $("#<%=DDProcess.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Process. !!\n";
                    }
                    selectedindex = $("#<%=DDEmp.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Employee. !!\n";
                    }
                    selectedindex = $("#<%=ddCatagory.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Category !!\n";
                    }
                    selectedindex = $("#<%=dditemname.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Item !!\n";
                    }
                    if ($("#<%=TDQuality.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=dquality.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Quality. !!\n";
                        }
                    }
                    if ($("#<%=TDDesign.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=dddesign.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Design. !!\n";
                        }
                    }
                    if ($("#<%=TDColor.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=ddcolor.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Color. !!\n";
                        }
                    }
                    if ($("#<%=TDShape.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=ddshape.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Shape. !!\n";
                        }
                    }
                    if ($("#<%=TDSize.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=ddsize.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Size. !!\n";
                        }
                    }
                    if ($("#<%=TDShade.ClientID %>").is(':visible')) {
                        var selectedindex = $("#<%=ddlshade.ClientID %>").attr('selectedIndex');
                        if (selectedindex <= 0) {
                            Message = Message + "Please select Shade. !!\n";
                        }
                    }
                    var txtpcs = document.getElementById('<%=txtpcs.ClientID %>');
                    if (txtpcs.value == "" || txtpcs.value == "0") {
                        Message = Message + "Please Enter Pcs. !!\n";
                    }
                    var txtarea = document.getElementById('<%=txtarea.ClientID %>');
                    if (txtarea.value == "" || txtarea.value == "0") {
                        Message = Message + "Please Enter Area. !!\n";
                    }
                    var txtbeamreq = document.getElementById('<%=txtbeamreq.ClientID %>')
                    if (txtbeamreq.value == "" || txtbeamreq.value == "0") {
                        Message = Message + "Please Enter No. of Beam Req. !!\n";
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
    <asp:UpdatePanel ID="R" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label ID="lblMasterDetail" runat="server" Text="Master Details" CssClass="labelbold"
                            ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr>
                            <td id="TDEdit" runat="server" visible="false">
                                <asp:CheckBox ID="chkedit" Text="For Edit" CssClass="labelbold" runat="server" AutoPostBack="true"
                                    OnCheckedChanged="chkedit_CheckedChanged" />
                            </td>
                            <td id="TDcomplete" runat="server" visible="false">
                                <asp:CheckBox ID="chkcomplete" Text="For Complete" CssClass="labelbold" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Department" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDDept" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="Process" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDProcess" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDProcess_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label15" runat="server" Text="Employee" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDEmp" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDEmp_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDissueNo" runat="server" visible="false">
                                <asp:Label ID="Label10" runat="server" Text="Issue No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDissueNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDissueNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Issue No." CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtissueno" CssClass="textb" Width="90px" runat="server" Enabled="False" />
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Issue Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txtissuedate" CssClass="textb" Width="90px" runat="server" />
                                <asp:CalendarExtender ID="cal1" TargetControlID="txtissuedate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Target Date" CssClass="labelbold"></asp:Label><br />
                                <asp:TextBox ID="txttargetdate" CssClass="textb" Width="90px" runat="server" />
                                <asp:CalendarExtender ID="cal2" TargetControlID="txttargetdate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label5" runat="server" Text="Item Details" CssClass="labelbold" ForeColor="Red"
                            Font-Bold="true"></asp:Label></legend>
                    <table>
                        <tr id="Tr3" runat="server">
                            <td id="tdProCode" runat="server" visible="false" class="tdstyle">
                                <span class="labelbold">ProdCode</span>
                                <br />
                                <asp:TextBox ID="TxtProdCode" CssClass="textb" runat="server" Width="136px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblcategoryname" runat="server" class="tdstyle" Text="Category Name"
                                    CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddCatagory" runat="server" Width="150px" CssClass="dropdown"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblitemname" class="tdstyle" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="dditemname" runat="server" Width="150px" CssClass="dropdown"
                                    AutoPostBack="True" OnSelectedIndexChanged="dditemname_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDQuality" runat="server" visible="false">
                                <asp:Label ID="lblqualityname" class="tdstyle" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="dquality" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td id="TDDesign" runat="server" visible="false" class="style5">
                                <asp:Label ID="lbldesignname" runat="server" class="tdstyle" Text="Design" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="dddesign" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="TDColor" runat="server" visible="false">
                                <asp:Label ID="lblcolorname" runat="server" class="tdstyle" Text="Color" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddcolor" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td id="TDShape" runat="server" visible="false">
                                <asp:Label ID="lblshapename" class="tdstyle" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddshape" runat="server" Width="150px" AutoPostBack="True" CssClass="dropdown"
                                    OnSelectedIndexChanged="ddshape_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDSize" runat="server" class="tdstyle" visible="false">
                                <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged">
                                </asp:DropDownList>
                                <br />
                                <asp:DropDownList ID="ddsize" runat="server" Width="150px" CssClass="dropdown" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddsize_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDShade" runat="server" visible="false">
                                <asp:Label ID="lblshadecolor" runat="server" class="tdstyle" Text="ShadeColor" CssClass="labelbold"></asp:Label>
                                &nbsp;<br />
                                <asp:DropDownList ID="ddlshade" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label7" runat="server" class="tdstyle" Text="Area." CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtarea" CssClass="textb" runat="server" onkeypress="return isNumberKey(event);"
                                                Width="70px" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" class="tdstyle" Text="Pcs." CssClass="labelbold"></asp:Label>
                                            &nbsp;<br />
                                            <asp:TextBox ID="txtpcs" CssClass="textb" runat="server" onkeypress="return isNumberKey(event);"
                                                Width="70px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" class="tdstyle" Text="No. of Beam Req." CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtbeamreq" CssClass="textb" runat="server" Text="1" onkeypress="return isNumberKey(event);"
                                    Width="90px" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblmessage" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                        </td>
                        <td align="right">
                            <asp:Button ID="btnupdateconsmp" runat="server" Text="Update Current Consumption"
                                CssClass="buttonnorm" Visible="false" OnClick="btnupdateconsmp_Click" />
                            <asp:Button ID="btnnew" runat="server" Text="New" CssClass="buttonnorm" OnClientClick="return NewForm();" />
                            <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="btnsave_Click" />
                            <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="buttonnorm" OnClick="btnPreview_Click" />
                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <div id="gride" runat="server" style="max-height: 300px">
                    <asp:GridView ID="DG" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                        OnRowDeleting="DG_RowDeleting" OnRowCancelingEdit="DG_RowCancelingEdit" OnRowEditing="DG_RowEditing"
                        OnRowUpdating="DG_RowUpdating">
                        <HeaderStyle CssClass="gvheaders" />
                        <AlternatingRowStyle CssClass="gvalts" />
                        <RowStyle CssClass="gvrow" />
                        <EmptyDataRowStyle CssClass="gvemptytext" />
                        <Columns>
                            <asp:TemplateField HeaderText="Sr No.">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ItemDescription">
                                <ItemTemplate>
                                    <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="350px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Pcs. Qty">
                                <ItemTemplate>
                                    <asp:Label ID="lblorderQty" Text='<%#Bind("pcs") %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="No. of Beam Req.">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtnoofbeam" runat="server" Width="70px" Text='<%#Bind("Noofbeamreq") %>'
                                        onkeypress="return isNumberKey(event);"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblnoofbeamreq" Text='<%#Bind("Noofbeamreq") %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Area">
                                <ItemTemplate>
                                    <asp:Label ID="lblarea" Text='<%#Bind("Area") %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblid" Text='<%#Bind("Id") %>' runat="server" />
                                    <asp:Label ID="lbldetailid" Text='<%#Bind("Detailid") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkdel" runat="server" CausesValidation="False" CommandName="Delete"
                                        Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="True" />
                        </Columns>
                    </asp:GridView>
                </div>
                <div style="margin-top: 20px;">
                </div>
                <div style="width: 100%">
                    <div style="width: 50%; float: left">
                        <table>
                            <tr>
                                <td>
                                    <div id="Div1" runat="server" style="max-height: 300px">
                                        <asp:GridView ID="DGRawdetail" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                            EmptyDataText="No records found.">
                                            <HeaderStyle CssClass="gvheaders" />
                                            <AlternatingRowStyle CssClass="gvalts" />
                                            <RowStyle CssClass="gvrow" />
                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr No.">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Raw Material Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Width="250px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblunit" Text='<%#Bind("UnitName") %>' runat="server" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Issue Qty">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblissueqty" Text='<%#Bind("IQty") %>' runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 50%; float: right">
                        <table>
                            <tr>
                                <td>
                                    <div id="Div2" runat="server" style="max-height: 300px">
                                        <asp:GridView ID="DGreceiveDetail" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                            EmptyDataText="No records found.">
                                            <HeaderStyle CssClass="gvheaders" />
                                            <AlternatingRowStyle CssClass="gvalts" />
                                            <RowStyle CssClass="gvrow" />
                                            <EmptyDataRowStyle CssClass="gvemptytext" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr No.">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Beam Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblitemdescription" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle Width="350px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblunit" Text='<%#Bind("Unitname") %>' runat="server" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hnid" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
