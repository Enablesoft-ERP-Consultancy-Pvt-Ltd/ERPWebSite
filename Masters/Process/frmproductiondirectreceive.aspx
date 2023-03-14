<%@ Page Title="Production Receive" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmproductiondirectreceive.aspx.cs" Inherits="Masters_Process_frmproductiondirectreceive" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/FixFocus2.js" type="text/javascript"></script>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "frmproductiondirectreceive.aspx";
        }

        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function EmpSelected(source, eventArgs) {
            document.getElementById('<%=txtgetvalue.ClientID%>').value = eventArgs.get_value();
            document.getElementById('<%=btnSearch.ClientID%>').click();
        }
        function Loomnoselected(source, eventArgs) {
            document.getElementById('<%=txtloomid.ClientID%>').value = eventArgs.get_value();
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
                    selectedindex = $("#<%=DDProdunit.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Production unit !!\n";
                    }
                    selectedindex = $("#<%=DDunit.ClientID %>").attr('selectedIndex');
                    if (selectedindex < 0) {
                        Message = Message + "Please select  unit !!\n";
                    }
                    selectedindex = $("#<%=dditemname.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please select Type !!\n";
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

                    var txtqty = document.getElementById('<%=txtqty.ClientID %>');
                    if (txtqty.value == "" || txtqty.value == "0") {
                        Message = Message + "Please Enter Receive Qty. !!\n";
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
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(Jscriptvalidate);
            </script>
            <fieldset>
                <legend>
                    <asp:Label ID="lblMasterDetail" runat="server" Text="Master Details" CssClass="labelbold"
                        ForeColor="Red" Font-Bold="true"></asp:Label></legend>
                <table border="1" style="width: 100%">
                    <tr>
                        <td id="TDEdit" runat="server" visible="false">
                            <table border="1" style="width: 30%">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkEdit" Text="For Edit" CssClass="labelbold" runat="server" AutoPostBack="true"
                                            OnCheckedChanged="chkEdit_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblfromdate" Text="Data From" runat="server" CssClass="labelbold" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfromdate" CssClass="textb" runat="server" Width="100px" AutoPostBack="true"
                                            OnTextChanged="txtfromdate_TextChanged" />
                                        <asp:CalendarExtender ID="caltxtfromdate" runat="server" TargetControlID="txtfromdate"
                                            Format="dd-MMM-yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <div style="width: 60%; float: left;">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Company Name" CssClass="labelbold"></asp:Label>
                                <br />
                                <asp:DropDownList ID="DDcompany" runat="server" CssClass="dropdown" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Production Unit" CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDProdunit" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDProdunit_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDreceiveno" runat="server" visible="false">
                                <asp:Label ID="Label16" runat="server" Text="Receive No." CssClass="labelbold"></asp:Label><br />
                                <asp:DropDownList ID="DDreceiveNo" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    Width="150px" OnSelectedIndexChanged="DDreceiveNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td id="TDloomno" runat="server" visible="false">
                                <table>
                                    <tr>
                                        <td id="TDLoomNoDropdown" runat="server">
                                            <asp:Label ID="Label15" runat="server" Text="Loom No." CssClass="labelbold"></asp:Label><br />
                                            <asp:DropDownList ID="DDLoomNo" runat="server" CssClass="dropdown" Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="TDLoomNotextbox" runat="server" visible="false">
                                            <asp:Label ID="Label8" Text=" Loom No." runat="server" CssClass="labelbold" />
                                            <asp:TextBox ID="txtloomid" runat="server" Style="display: none"></asp:TextBox>
                                            <br />
                                            <asp:TextBox ID="txtloomno" CssClass="textb" runat="server" Width="150px" />
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtenderloomno" runat="server" BehaviorID="LoomSrchAutoComplete"
                                                CompletionInterval="20" Enabled="True" ServiceMethod="GetLoomNo" EnableCaching="true"
                                                CompletionSetCount="30" OnClientItemSelected="Loomnoselected" ServicePath="~/Autocomplete.asmx"
                                                TargetControlID="txtloomno" UseContextKey="true" ContextKey="0" MinimumPrefixLength="1">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:Label ID="lblunitname" Text="Unit" CssClass="labelbold" runat="server" />
                                <br />
                                <asp:DropDownList ID="DDunit" CssClass="dropdown" Width="90px" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td id="TDcaltype" runat="server" visible="false">
                                <asp:Label ID="Label2" Text="Cal Type" CssClass="labelbold" runat="server" />
                                <br />
                                <asp:DropDownList ID="DDcaltype" CssClass="dropdown" Width="150px" runat="server">
                                    <asp:ListItem Value="0" Text="Area Wise"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Pc Wise"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblrecdate" Text="Receive Date" CssClass="labelbold" runat="server" />
                                <br />
                                <asp:TextBox ID="txtrecdate" CssClass="textb" Width="100px" runat="server" />
                                <asp:CalendarExtender ID="calrecdate" Format="dd-MMM-yyyy" runat="server" TargetControlID="txtrecdate">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblrecno" Text="" CssClass="labelbold" ForeColor="Red" Font-Size="Small"
                                    runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="width: 40%; float: right">
                    <table>
                        <tr>
                            <td style="vertical-align: top">
                                <asp:Label ID="lblempcode" Text="Employee Code." runat="server" CssClass="labelbold" />
                                <br />
                                <asp:TextBox ID="txtWeaverIdNo" CssClass="textb" runat="server" Width="150px" />
                                <asp:TextBox ID="txtgetvalue" runat="server" Style="display: none"></asp:TextBox>
                                <asp:Button ID="btnSearch" runat="server" Text="Button" OnClick="btnSearch_Click"
                                    Style="display: none;" />
                                <asp:AutoCompleteExtender ID="txtWeaverIdNo_AutoCompleteExtender" runat="server"
                                    BehaviorID="SrchAutoComplete" CompletionInterval="20" Enabled="True" ServiceMethod="GetEmployeeAll"
                                    EnableCaching="true" CompletionSetCount="20" OnClientItemSelected="EmpSelected"
                                    ServicePath="~/Autocomplete.asmx" TargetControlID="txtWeaverIdNo" UseContextKey="True"
                                    ContextKey="0#0#0" MinimumPrefixLength="2">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <div style="overflow: auto; width: 200px">
                                    <asp:ListBox ID="listWeaverName" runat="server" Width="200px" Height="100px" SelectionMode="Multiple">
                                    </asp:ListBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:LinkButton ID="btnDelete" Text="Remove Employee" runat="server" CssClass="labelbold"
                                    OnClick="btnDelete_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </fieldset>
            <fieldset>
                <legend>
                    <asp:Label Text="Item Details" ForeColor="Red" CssClass="labelbold" runat="server" />
                </legend>
                <table>
                    <tr id="Tr3" runat="server">
                        <td id="tdProCode" runat="server" visible="false" class="tdstyle">
                            <span class="labelbold">ProdCode</span>
                            <br />
                            <asp:TextBox ID="TxtProdCode" CssClass="textb" runat="server" Width="100px"></asp:TextBox>
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
                            <asp:Label ID="lblitemname" class="tdstyle" runat="server" Text="Type" CssClass="labelbold"></asp:Label>
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
                        <td id="TdWidth" runat="server" visible="false">
                            <asp:Label ID="Label4" runat="server" class="tdstyle" Text="Width" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtWidth" runat="server" Width="90px" Enabled="false" CssClass="textb"
                                TabIndex="17"></asp:TextBox>
                        </td>
                        <td id="TdLength" runat="server" visible="false">
                            <asp:Label ID="Label5" runat="server" class="tdstyle" Text="Length" CssClass="labelbold"></asp:Label>
                            <br />
                            <asp:TextBox ID="TxtLength" runat="server" Width="90px" Enabled="false" CssClass="textb"
                                TabIndex="18"></asp:TextBox>
                        </td>
                        <td id="TDarea" runat="server" visible="false">
                            <asp:Label ID="Label3" CssClass="labelbold" Text="Area" runat="server" />
                            <br />
                            <asp:TextBox ID="TxtArea" CssClass="textb" Width="90px" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="lblqty" CssClass="labelbold" Text="Qty Received" runat="server" />
                            <br />
                            <asp:TextBox ID="txtqty" CssClass="textb" Width="90px" runat="server" />
                        </td>
                        <td>
                            <asp:Button ID="btnsave" CssClass="buttonnorm" Text="Save" runat="server" OnClick="btnsave_Click" />
                            <asp:Button ID="btnclose" CssClass="buttonnorm" Text="Close" runat="server" OnClientClick="return CloseForm();" />
                            <asp:Button ID="btnnew" CssClass="buttonnorm" Text="New" runat="server" OnClientClick="return NewForm();" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Label ID="lblmsg" Text="" CssClass="labelbold" Font-Size="Small" ForeColor="Red"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="max-height: 300px; overflow: auto">
                            <div id="Div1" runat="server" style="max-height: 300px; overflow: auto">
                                <asp:GridView ID="DGRecDetail" AutoGenerateColumns="False" runat="server" CssClass="grid-views"
                                    EmptyDataText="No. Records found." DataKeyNames="Sr_No" OnRowDeleting="DGRecDetail_RowDeleting">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                    <EmptyDataRowStyle CssClass="gvemptytext" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="SrNo.">
                                            <ItemTemplate>
                                                <%#Container.DisplayIndex+1 %>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblitemgrid" Text='<%#Bind("Item") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quality">
                                            <ItemTemplate>
                                                <asp:Label ID="Label6" Text='<%#Bind("Qualityname") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Design">
                                            <ItemTemplate>
                                                <asp:Label ID="Label13" Text='<%#Bind("Designname") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Color">
                                            <ItemTemplate>
                                                <asp:Label ID="Label14" Text='<%#Bind("Colorname") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Shape">
                                            <ItemTemplate>
                                                <asp:Label ID="Label15" Text='<%#Bind("Shapename") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Width">
                                            <ItemTemplate>
                                                <asp:Label ID="Label7" Text='<%#Bind("Width") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Length">
                                            <ItemTemplate>
                                                <asp:Label ID="Label10" Text='<%#Bind("Length") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rec Qty.">
                                            <ItemTemplate>
                                                <asp:Label ID="Label11" Text='<%#Bind("Qty") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="StockNo">
                                            <ItemTemplate>
                                                <asp:Label ID="Label12" Text='<%#Bind("Tstockno") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblissue_detail_id" Text='<%#Bind("Issue_detail_id") %>' runat="server" />
                                                <asp:Label ID="lblissueorderid" Text='<%#Bind("issueorderid") %>' runat="server" />
                                                <asp:Label ID="lblprocess_rec_id" Text='<%#Bind("process_rec_id") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hnissueorderid" runat="server" Value="0" />
            <asp:HiddenField ID="hnprocessrecid" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
