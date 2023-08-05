<%@ Page Title="SAMPLE ORDER" Language="C#" MasterPageFile="~/ERPmaster.master" AutoEventWireup="true"
    CodeFile="frmsampleorderagni.aspx.cs" Inherits="Masters_Process_frmsampleorderagni" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/FixFocus2.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmsampleorder.aspx";
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
        function isNumberKeywithdecimal(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
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
                    selectedindex = $("#<%=DDcustcode.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Customer. !!\n";
                    }
                    selectedindex = $("#<%=DDprocess.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Process. !!\n";
                    }
                    selectedindex = $("#<%=DDvendor.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Employee. !!\n";
                    }
                    selectedindex = $("#<%=DDunit.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please Select Unit. !!\n";
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
                    var txtwidth = document.getElementById('<%=txtwidth.ClientID %>');
                    if (txtwidth.value == "" || txtwidth.value == "0") {
                        Message = Message + "Please Enter Width. !!\n";
                    }
                    var txtlength = document.getElementById('<%=txtlength.ClientID %>');
                    if (txtlength.value == "" || txtlength.value == "0") {
                        Message = Message + "Please Enter Length. !!\n";
                    }
                    var txtarea = document.getElementById('<%=txtarea.ClientID %>');
                    if (txtarea.value == "" || txtarea.value == "0") {
                        Message = Message + "Please Enter Area. !!\n";
                    }

                    var txtpcs = document.getElementById('<%=txtpcs.ClientID %>')
                    if (txtpcs.value == "" || txtpcs.value == "0") {
                        Message = Message + "Please Enter Pcs. !!\n";
                    }
                    if (Message == "") {
                        return true;
                    }
                    else {
                        alert(Message);
                        return false;
                    }
                });
                //on DropDown Selected Index
                $('#' + '<%=DDcustcode.ClientID %>').change(function () {
                    $('#' + '<%=hnorderid.ClientID %>').attr({ 'value': '0' });
                    $('#' + '<%=hnissueorderid.ClientID %>').attr({ 'value': '0' });
                    $('#' + '<%=DDvendor.ClientID %>').attr({ 'selectedIndex': 0 });
                });
                $('#' + '<%=DDvendor.ClientID %>').change(function () {
                    $('#' + '<%=hnorderid.ClientID %>').attr({ 'value': '0' });
                    $('#' + '<%=hnissueorderid.ClientID %>').attr({ 'value': '0' });

                    if ($("#<%=TDChallanNO.ClientID %>").is(':visible')) {
                        $('#' + '<%=DDchallanNo.ClientID %>').attr({ 'selectedIndex': 0 });
                    }

                });
                $('#' + '<%=DDunit.ClientID %>').change(function () {
                    $('#' + '<%=hnorderid.ClientID %>').attr({ 'value': '0' });
                    $('#' + '<%=hnissueorderid.ClientID %>').attr({ 'value': '0' });
                });
                $('#' + '<%=DDcaltype.ClientID %>').change(function () {
                    $('#' + '<%=hnorderid.ClientID %>').attr({ 'value': '0' });
                    $('#' + '<%=hnissueorderid.ClientID %>').attr({ 'value': '0' });
                });
            });
        }
    </script>
    <asp:UpdatePanel ID="Updt1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
                
            </script>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label Text="Master Details" runat="server" CssClass="labelbold" ForeColor="Red" />
                    </legend>
                    <table>
                        <tr id="TDEdit" runat="server" visible="false">
                            <td>
                                <asp:CheckBox ID="chkedit" Text="For Edit" runat="server" AutoPostBack="true" CssClass="checkboxbold"
                                    OnCheckedChanged="chkedit_CheckedChanged" />
                            </td>
                            <td>
                                <asp:CheckBox ID="Chkcomplete" Text="For Complete" runat="server" CssClass="checkboxbold" />
                            </td>
                        </tr>
                        <tr>
                            <td id="TDeditchallanNo" runat="server" visible="false">
                                <asp:Label ID="Label18" Text="Challan No" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtchallanNoedit" CssClass="textb" Width="90px" runat="server" AutoPostBack="true"
                                    OnTextChanged="txtchallanNoedit_TextChanged" />
                            </td>
                            <td>
                                <asp:Label ID="lblcompany" Text="Company Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDcompany" CssClass="dropdown" Width="150px" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label2" Text="Process Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDprocess" CssClass="dropdown" Width="150px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDprocess_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label1" Text="Customer Code" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDcustcode" CssClass="dropdown" Width="150px" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label3" Text="Vendor Name" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDvendor" CssClass="dropdown" Width="150px" runat="server"
                                    AutoPostBack="true" OnSelectedIndexChanged="DDvendor_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDChallanNO" runat="server" visible="false">
                                <asp:Label ID="Label17" Text="Challan No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList ID="DDchallanNo" CssClass="dropdown" Width="130px" AutoPostBack="true"
                                    runat="server" OnSelectedIndexChanged="DDchallanNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label4" Text="Challan No." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtchallanNo" CssClass="textb" runat="server" Width="90px" Enabled="false" />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label5" Text="Assign Date" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtassigndate" CssClass="textb" Width="100px" runat="server" />
                                <asp:CalendarExtender ID="calassigndate" TargetControlID="txtassigndate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="Label6" Text="Required Date" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtreqdate" CssClass="textb" Width="100px" runat="server" />
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtreqdate" Format="dd-MMM-yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="Label8" Text="Unit" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDunit" runat="server" Width="100px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label9" Text=" Cal Type" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:DropDownList CssClass="dropdown" ID="DDcaltype" runat="server" Width="100px">
                                    <asp:ListItem Value="0">Area Wise</asp:ListItem>
                                    <asp:ListItem Value="1">Pcs Wise</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label16" Text="Remarks" runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtremarks" CssClass="textb" Width="800px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div>
                <fieldset>
                    <legend>
                        <asp:Label ID="Label7" Text="Item Details" runat="server" CssClass="labelbold" ForeColor="Red" />
                    </legend>
                    <table>
                        <tr id="Tr3" runat="server">
                            <td id="tdProCode" runat="server" visible="false">
                                <span class="labelbold">ProdCode</span>
                                <br />
                                <asp:TextBox ID="TxtProdCode" CssClass="textb" runat="server" Width="136px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblcategoryname" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddCatagory" runat="server" Width="150px" CssClass="dropdown"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblitemname" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="dditemname" runat="server" Width="150px" CssClass="dropdown"
                                    AutoPostBack="True" OnSelectedIndexChanged="dditemname_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDQuality" runat="server" visible="false">
                                <asp:Label ID="lblqualityname" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="dquality" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td id="TDDesign" runat="server" visible="false" class="style5">
                                <asp:Label ID="lbldesignname" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="dddesign" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td id="TDColor" runat="server" visible="false">
                                <asp:Label ID="lblcolorname" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddcolor" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td id="TDShape" runat="server" visible="false">
                                <asp:Label ID="lblshapename" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="ddshape" runat="server" Width="150px" AutoPostBack="True" CssClass="dropdown"
                                    OnSelectedIndexChanged="ddshape_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDSize" runat="server" visible="false">
                                <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged" Visible="false">
                                </asp:DropDownList>
                                <asp:CheckBox ID="chkexportsize" Text="ExportSize" runat="server" AutoPostBack="true"
                                    CssClass="checkboxbold" OnCheckedChanged="chkexportsize_CheckedChanged" />
                                <br />
                                <asp:DropDownList ID="ddsize" runat="server" Width="150px" CssClass="dropdown" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddsize_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDShade" runat="server" visible="false">
                                <asp:Label ID="lblshadecolor" runat="server" Text="ShadeColor" CssClass="labelbold"></asp:Label>
                                &nbsp;<br />
                                <asp:DropDownList ID="ddlshade" runat="server" Width="150px" CssClass="dropdown">
                                </asp:DropDownList>
                            </td>
                            <td colspan="3">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label13" runat="server" Text="Width" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtwidth" CssClass="textb" runat="server" Width="70px" AutoPostBack="true"
                                                OnTextChanged="txtwidth_TextChanged" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label14" runat="server" Text="Length" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtlength" CssClass="textb" runat="server" Width="70px" AutoPostBack="true"
                                                OnTextChanged="txtlength_TextChanged" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label10" runat="server" Text="Area." CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtarea" CssClass="textb" runat="server" Enabled="false" BackColor="Yellow"
                                                Width="70px" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label12" runat="server" Text="Rate" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtrate" CssClass="textb" runat="server" Width="70px" onkeypress="return isNumberKeywithdecimal(event)" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label15" runat="server" Text="Comm.Rate" CssClass="labelbold"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtcomm" CssClass="textb" runat="server" Width="70px" onkeypress="return isNumberKeywithdecimal(event)" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label11" runat="server" Text="Pcs." CssClass="labelbold"></asp:Label>
                                            &nbsp;<br />
                                            <asp:TextBox ID="txtpcs" CssClass="textb" runat="server" onkeypress="return isNumberKey(event);"
                                                Width="70px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div>
                <table style="width: 100%">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnnew" Text="New" runat="server" CssClass="buttonnorm" OnClientClick="return NewForm()" />
                            <asp:Button ID="btnsave" Text="Save" runat="server" CssClass="buttonnorm" OnClick="btnsave_Click" />
                            <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="buttonnorm" OnClientClick="return CloseForm()" />
                            <asp:Button ID="btnpreview" Text="Preview" runat="server" CssClass="buttonnorm" OnClick="btnpreview_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblmsg" Text="" CssClass="labelbold" ForeColor="Red" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <table>
                <tr>
                    <td>
                        <div style="width: 100%; max-height: 250px; overflow: auto">
                            <asp:GridView ID="DGOrderdetail" runat="server" AutoGenerateColumns="False" DataKeyNames="Sr_No"
                                CssClass="grid-views" OnRowCancelingEdit="DGOrderdetail_RowCancelingEdit" OnRowEditing="DGOrderdetail_RowEditing"
                                OnRowUpdating="DGOrderdetail_RowUpdating" OnRowDeleting="DGOrderdetail_RowDeleting">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Category">
                                        <ItemTemplate>
                                            <asp:Label ID="lblcategory" Text='<%#Bind("Category") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItem" Text='<%#Bind("Item") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" Text='<%#Bind("Description") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Width">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWidth" Text='<%#Bind("Width") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Length">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLength" Text='<%#Bind("Length") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQty" Text='<%#Bind("Qty") %>' runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtqtyedit" Text='<%#Bind("Qty") %>' runat="server" Width="70px" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRate" Text='<%#Bind("Rate") %>' runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtrateedit" Text='<%#Bind("rate") %>' runat="server" Width="70px" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comm.Rate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblcommrate" Text='<%#Bind("comm") %>' runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtcommrateedit" Text='<%#Bind("comm") %>' runat="server" Width="70px" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Area">
                                        <ItemTemplate>
                                            <asp:Label ID="lblArea" Text='<%#Bind("Area") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" Text='<%#Bind("Amount") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblorderid" Text='<%#Bind("orderid") %>' runat="server" />
                                            <asp:Label ID="lblitemfinishedid" Text='<%#Bind("Item_finished_id") %>' runat="server" />
                                            <asp:Label ID="lblissueorderid" Text='<%#Bind("issueorderid") %>' runat="server" />
                                            <asp:Label ID="lblcaltype" Text='<%#Bind("caltype") %>' runat="server" />
                                            <asp:Label ID="lblunitid" Text='<%#Bind("unitid") %>' runat="server" />
                                            <asp:Label ID="lblshapeid" Text='<%#Bind("shapeid") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField DeleteText="" ShowEditButton="True" />
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hnissueorderid" runat="server" Value="0" />
            <asp:HiddenField ID="hnorderid" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
