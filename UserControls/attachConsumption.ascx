<%@ Control Language="C#" AutoEventWireup="true" CodeFile="attachConsumption.ascx.cs"
    Inherits="UserControls_attachConsumption" %>
<%--<link href="../Styles/vijay.css" rel="Stylesheet" type="text/css" />--%>
<script src="../../Scripts/JScript.js" type="text/javascript"></script>
<script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
<script type="text/javascript">
    function jScriptValidate() {
        $(document).ready(function () {
            $("#<%=BtnShowDetail.ClientID %>").click(function () {
                var message = "";
                if ($("#<%=DDCategoryname.ClientID %>")) {
                    var selectedIndex = $("#<%=DDCategoryname.ClientID %>").attr('selectedIndex');
                    if (selectedIndex <= 0) {
                        message = message + "Please select category Name!!!\n";
                    }
                }
                if ($("#<%=DDItemName.ClientID %>")) {
                    var selectedIndex = $("#<%=DDItemName.ClientID %>").attr('selectedIndex');
                    if (selectedIndex <= 0) {
                        message = message + "Please select Item Name!!!\n";
                    }
                }
                if ($("#<%=DDProcess.ClientID %>")) {
                    var selectedIndex = $("#<%=DDProcess.ClientID %>").attr('selectedIndex');
                    if (selectedIndex <= 0) {
                        message = message + "Please select Process Name!!!\n";
                    }
                }
                if (message == "") {
                    return true;
                }
                else {
                    alert(message);
                    return false;
                }
            });
            $("#<%=btnclose.ClientID %>").click(function () {
                window.close();
            });

        });
    }        
</script>
<script type="text/javascript">
    $("[id*=chkAll]").live("click", function () {

        var chkAll = $(this);
        var grid = $(this).closest("table");
        $("input[type=checkbox]", grid).each(function () {
            if (chkAll.is(":checked")) {
                $(this).attr("checked", "checked");
                $("td", $(this).closest("tr:")).addClass("selected");
            }
            else {
                $(this).removeAttr("checked");
                $("td", $(this).closest("tr")).removeClass("selected");
            }
        });
    });   
</script>
<asp:ScriptManager runat="server" ID="scr1">
</asp:ScriptManager>
<asp:UpdatePanel runat="server" ID="up1">
    <ContentTemplate>
        <script type="text/javascript" language="javascript">
            Sys.Application.add_load(jScriptValidate);
        </script>
        <fieldset>
            <legend>
                <asp:Label ID="lblSearchDetail" runat="server" Text="Search Data" CssClass="labelbold"
                    ForeColor="Red" Font-Bold="true"></asp:Label></legend>
            <asp:Panel ID="pnl1" runat="server">
                <table>
                    <tr>
                        <td id="TDProdcode" runat="server" visible="false">
                            <asp:Label ID="lblItemCode" runat="server" Text="ItemCode" CssClass="labelbold"></asp:Label><br />
                            <asp:TextBox ID="txtprodcode" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblcategory" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label><br />
                            <asp:DropDownList ID="DDCategoryname" runat="server" CssClass="dropdown" Width="150px"
                                OnSelectedIndexChanged="DDCategoryname_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblIemName" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label><br />
                            <asp:DropDownList ID="DDItemName" runat="server" CssClass="dropdown" Width="150px"
                                OnSelectedIndexChanged="DDItemName_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td id="TDQuality" runat="server" visible="false">
                            <asp:Label ID="lblQuality" runat="server" Text="Quality" CssClass="labelbold"></asp:Label><br />
                            <asp:DropDownList ID="DDQuality" runat="server" CssClass="dropdown" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="DDQuality_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TDDesign" runat="server" visible="false">
                            <asp:Label ID="lblDesign" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                            <asp:CheckBox ID="CHKFORALLDESIGN" runat="server" Text="For All" AutoPostBack="true"
                                CssClass="checkboxbold" OnCheckedChanged="CHKFORALLDESIGN_CheckedChanged" />
                            <br />
                            <asp:DropDownList ID="DDDesign" runat="server" CssClass="dropdown" Width="130px" AutoPostBack="true" OnSelectedIndexChanged="DDDesign_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TDColor" runat="server" visible="false">
                            <asp:Label ID="lblColor" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                            <asp:CheckBox ID="CHKFORALLCOLOR" runat="server" Text="For All" AutoPostBack="true"
                                CssClass="checkboxbold" OnCheckedChanged="CHKFORALLCOLOR_CheckedChanged" />
                            <br />
                            <asp:DropDownList ID="DDColor" runat="server" CssClass="dropdown" Width="130px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td id="TDShape" runat="server" visible="false">
                            <asp:Label ID="lblShape" runat="server" Text="Shape" CssClass="labelbold"></asp:Label><br />
                            <asp:DropDownList ID="DDshape" runat="server" CssClass="dropdown" Width="150px" OnSelectedIndexChanged="DDshape_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td id="TDSize" runat="server" visible="false">
                            <asp:Label ID="lblSize" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                            <asp:CheckBox ID="CHKFORALLSIZE" runat="server" Text="For All" AutoPostBack="true"
                                CssClass="checkboxbold" OnCheckedChanged="CHKFORALLSIZE_CheckedChanged" />&nbsp;
                            <asp:DropDownList CssClass="dropdown" Width="50" ID="DDsizetype" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged" />
                            <br />
                            <asp:DropDownList ID="DDSize" runat="server" CssClass="dropdown" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td id="TDShade" runat="server" visible="false">
                            <asp:Label ID="lblshade" runat="server" Text="Shade Color" CssClass="labelbold"></asp:Label><br />
                            <asp:DropDownList ID="DDshade" runat="server" CssClass="dropdown" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblProcess" runat="server" Text="Process" CssClass="labelbold"></asp:Label><br />
                            <asp:DropDownList ID="DDProcess" runat="server" CssClass="dropdown" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <br />
                            <asp:Button ID="BtnShowDetail" Text="Show Detail" runat="server" CssClass="buttonnorm"
                                OnClick="BtnShowDetail_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </fieldset>
        <fieldset>
            <legend>
                <asp:Label ID="lblFindData" runat="server" Text="Find Data" CssClass="labelbold"
                    ForeColor="Red" Font-Bold="true"></asp:Label></legend>
            <div style="width: 78%; height: auto; max-height: 200px; overflow: auto;">
                <asp:GridView ID="DG" runat="server" DataKeyNames="PCMDID" AutoGenerateColumns="False"
                    CssClass="grid-views" EmptyDataText="No Records Found!!!">
                    <HeaderStyle CssClass="gvheaders" />
                    <AlternatingRowStyle CssClass="gvalts" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkAll" runat="server" CssClass="checkboxbold" ToolTip="Click to select All" /></HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="ChkItem" runat="server" CssClass="checkboxbold" /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SrNo.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="PROCESS_NAME" HeaderText="PROCESS_NAME">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle Width="120px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="INPUTITEM" HeaderText="INPUT_ITEM">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle Width="250px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="IQTY">
                            <ItemTemplate>
                                <asp:TextBox ID="txtIQty" runat="server" Style="width: 40px; background-color: Yellow;
                                    text-align: center" Text='<%#Bind("IQTY") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ILOSS">
                            <ItemTemplate>
                                <asp:TextBox ID="txtILoss" runat="server" Style="width: 40px; background-color: Yellow;
                                    text-align: center" Text='<%#Bind("ILoss") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IRATE">
                            <ItemTemplate>
                                <asp:TextBox ID="txtIRate" runat="server" Style="width: 40px; background-color: Yellow;
                                    text-align: center" Text='<%#Bind("IRATE") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="IUNIT" HeaderText="I_UNIT">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="OUTPUTITEM" HeaderText="OUTPUT_ITEM">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle Width="250px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="OQTY">
                            <ItemTemplate>
                                <asp:TextBox ID="txtOQty" runat="server" Style="width: 40px; background-color: Yellow;
                                    text-align: center" Text='<%#Bind("OQTY") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ORATE">
                            <ItemTemplate>
                                <asp:TextBox ID="txtORATE" runat="server" Style="width: 40px; background-color: Yellow;
                                    text-align: center" Text='<%#Bind("ORATE") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="OUNIT" HeaderText="O_UNIT">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DyingMatch" HeaderText="DyeingMatch">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Dyeing" HeaderText="Dyeing">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DyingType" HeaderText="DyeingType">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblPCMID" runat="server" Text='<%#Bind("PCMID") %>'></asp:Label>
                                <asp:Label ID="lblFinishedid" runat="server" Text='<%#Bind("Finishedid") %>'></asp:Label>
                                <asp:Label ID="lblIFinishedid" runat="server" Text='<%#Bind("IFInishedid") %>'></asp:Label>
                                <asp:Label ID="lblOFinishedid" runat="server" Text='<%#Bind("OFInishedid") %>'></asp:Label>
                                <asp:Label ID="lblProcessid" runat="server" Text='<%#Bind("Process_name_id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <SelectedRowStyle CssClass="SelectedRowStyle" />
                </asp:GridView>
            </div>
        </fieldset>
        <fieldset>
            <legend>
                <asp:Label ID="Label1" runat="server" Text="Attach Item" CssClass="labelbold" ForeColor="Red"
                    Font-Bold="true"></asp:Label></legend>
            <asp:Panel ID="Panel1" runat="server">
                <table>
                    <tr>
                        <td id="TDAProdCode" runat="server" visible="false">
                            <asp:Label ID="lblAProdCode" runat="server" Text="ItemCode" CssClass="labelbold"></asp:Label><br />
                            <asp:TextBox ID="txtAProdCode" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblACategory" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label><br />
                            <asp:DropDownList ID="DDACategory" runat="server" CssClass="dropdown" AutoPostBack="true"
                                Width="150px" OnSelectedIndexChanged="DDACategory_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblAItem" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label><br />
                            <asp:DropDownList ID="DDAItem" runat="server" CssClass="dropdown" Width="150px" AutoPostBack="true"
                                OnSelectedIndexChanged="DDAItem_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TDAQuality" runat="server" visible="false">
                            <asp:Label ID="lblAQuality" runat="server" Text="Quality" CssClass="labelbold"></asp:Label><br />
                            <asp:DropDownList ID="DDAQuality" runat="server" CssClass="dropdown" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td id="TDADesign" runat="server" visible="false">
                            <asp:Label ID="lblADesign" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                            <asp:CheckBox ID="chkAForAllDesign" runat="server" Text="For All" AutoPostBack="true"
                                CssClass="checkboxbold" OnCheckedChanged="chkAForAllDesign_CheckedChanged" />
                            <br />
                            <asp:DropDownList ID="DDADesign" runat="server" CssClass="dropdown" Width="130px">
                            </asp:DropDownList>
                        </td>
                        <td id="TDAColor" runat="server" visible="false">
                            <asp:Label ID="lblAColor" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                            <asp:CheckBox ID="chkAForallColor" runat="server" Text="For All" AutoPostBack="true"
                                CssClass="checkboxbold" OnCheckedChanged="chkAForallColor_CheckedChanged" />
                            <br />
                            <asp:DropDownList ID="DDAColor" runat="server" CssClass="dropdown" Width="130px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td id="TDAShape" runat="server" visible="false">
                            <asp:Label ID="lblAShape" runat="server" Text="Shape" CssClass="labelbold"></asp:Label><br />
                            <asp:DropDownList ID="DDAShape" runat="server" CssClass="dropdown" Width="150px"
                                AutoPostBack="true" OnSelectedIndexChanged="DDAShape_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="TDASize" runat="server" visible="false">
                            <asp:Label ID="Label8" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                            <asp:CheckBox ID="chkAForallSize" runat="server" Text="For All" AutoPostBack="true"
                                CssClass="checkboxbold" OnCheckedChanged="chkAForallSize_CheckedChanged" />&nbsp;
                            <asp:DropDownList CssClass="dropdown" Width="50" ID="DDASizeType" runat="server"
                                AutoPostBack="true" OnSelectedIndexChanged="DDASizeType_SelectedIndexChanged" />
                            <br />
                            <asp:DropDownList ID="DDASize" runat="server" CssClass="dropdown" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td id="TDAShade" runat="server" visible="false">
                            <asp:Label ID="lblAshade" runat="server" Text="Shade Color" CssClass="labelbold"></asp:Label><br />
                            <asp:DropDownList ID="DDAshade" runat="server" CssClass="dropdown" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblAProcess" runat="server" Text="Process" CssClass="labelbold"></asp:Label><br />
                            <asp:DropDownList ID="DDAProcess" runat="server" CssClass="dropdown" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <br />
                            <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="buttonnorm" OnClick="btnSave_Click" />
                            <asp:Button ID="btnclose" Text="CLose" runat="server" CssClass="buttonnorm" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblerrormsg" runat="server" CssClass="labelbold" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </fieldset>
    </ContentTemplate>
</asp:UpdatePanel>
