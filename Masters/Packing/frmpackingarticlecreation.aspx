<%@ Page Title="PACKING ARTICLE CREATION" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmpackingarticlecreation.aspx.cs" Inherits="Masters_Packing_frmpackingarticlecreation"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function articleselected(source, eventArgs) {
            document.getElementById('<%=btnSearch.ClientID%>').click();
        }
        function NewForm() {
            window.location.href = "frmpackingarticlecreation.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
    </script>
    <script type="text/javascript">
        function Jscriptvalidate() {
            $(document).ready(function () {
                $("#<%=btnsave.ClientID %>").click(function () {
                    var Message = "";
                    var txtarticleno = document.getElementById('<%=txtarticleno.ClientID %>');
                    if (txtarticleno.value == "") {
                        Message = Message + "Please Enter Article No. !!\n";
                    }
                    selectedindex = $("#<%=DDpacktype.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Pack Type. !!\n";
                    }
                    selectedindex = $("#<%=DDitemname.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Item Name. !!\n";
                    }
                    selectedindex = $("#<%=DDquality.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Quality Name. !!\n";
                    }
                    selectedindex = $("#<%=DDdesign.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Design. !!\n";
                    }
                    selectedindex = $("#<%=DDcolor.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Color. !!\n";
                    }
                    selectedindex = $("#<%=DDshape.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Shape. !!\n";
                    }
                    selectedindex = $("#<%=DDSize.ClientID %>").attr('selectedIndex');
                    if (selectedindex <= 0) {
                        Message = Message + "Please Select Size. !!\n";
                    }
                    //                    



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
            <div style="margin: 0% 10% 0% 10%">
                <table border="1px solid grey">
                    <tr>
                        <td>
                            <asp:Label ID="lblartcleno" Text="Article No." runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:Button ID="btnSearch" runat="server" Text="Button" OnClick="btnSearch_Click"
                                Style="display: none;" />
                            <asp:TextBox ID="txtarticleno" CssClass="textb" runat="server" Width="190px" OnTextChanged="txtarticleno_TextChanged1" />
                            <asp:AutoCompleteExtender ID="txtarticleno_AutoCompleteExtender" runat="server" BehaviorID="SrchAutoComplete"
                                CompletionInterval="20" Enabled="True" ServiceMethod="GetArticleno" EnableCaching="true"
                                CompletionSetCount="20" ServicePath="~/Autocomplete.asmx" TargetControlID="txtarticleno"
                                UseContextKey="True" ContextKey="0#0#0" MinimumPrefixLength="2">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:Label ID="lblpacktype" Text="Pack Type" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDpacktype" CssClass="dropdown" runat="server" Width="200px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label13" Text="Category" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDcategory" CssClass="dropdown" runat="server" AutoPostBack="true"
                                Width="200px" OnSelectedIndexChanged="DDcategory_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label1" Text="Item Name" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDitemname" CssClass="dropdown" runat="server" AutoPostBack="true"
                                Width="200px" OnSelectedIndexChanged="DDitemname_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" Text="Quality Name" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDquality" CssClass="dropdown" runat="server" AutoPostBack="true"
                                Width="200px" OnSelectedIndexChanged="DDquality_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label3" Text="Design" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDdesign" CssClass="dropdown" runat="server" AutoPostBack="true"
                                Width="200px" OnSelectedIndexChanged="DDdesign_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" Text="Colour" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDcolor" CssClass="dropdown" runat="server" Width="200px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label5" Text="Shape" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDshape" CssClass="dropdown" runat="server" Width="200px" AutoPostBack="true"
                                OnSelectedIndexChanged="DDshape_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label6" Text="Size" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDSize" CssClass="dropdown" runat="server" Width="200px">
                            </asp:DropDownList>
                        </td>
                        <td id="psize" runat="server" visible="false">
                            <asp:Label ID="Label14" Text="Pallet Size" runat="server" CssClass="labelbold" />
                        </td>
                        <td id="tpsize" runat="server" visible="false">
                            <asp:TextBox ID="txtpalletsize" CssClass="textb" runat="server" Width="200px" >
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label7" Text="Description of Goods" runat="server" CssClass="labelbold" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtdescofgoods" CssClass="textb" runat="server" Width="475px" TextMode="MultiLine"
                                Height="49px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label8" Text="Content" runat="server" CssClass="labelbold" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtcontent" CssClass="textb" runat="server" Width="475px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label9" Text="Weight/Roll" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtwtperroll" CssClass="textb" runat="server" Width="190px" />
                        </td>
                        <td>
                            <asp:Label ID="Label10" Text="Net Weight" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtnetweight" CssClass="textb" runat="server" Width="190px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label11" Text="Volume/Roll" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtvolroll" CssClass="textb" runat="server" Width="190px" />
                        </td>
                        <td>
                            <asp:Label ID="Label12" Text="Pcs/Roll" runat="server" CssClass="labelbold" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtpcsroll" CssClass="textb" runat="server" Width="190px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <div style="max-height: 250px; overflow: auto">
                                <asp:GridView ID="DGrate" runat="server" AutoGenerateColumns="false" CssClass="grid-views"
                                    ShowFooter="true">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr No.">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Eff. Date">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txteffdate" runat="server" CssClass="textb" Width="90px">
                                                </asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtendertxtdate" runat="server" TargetControlID="txteffdate"
                                                    Format="dd-MMM-yyyy">
                                                </asp:CalendarExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate/Pcs">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtratepcs" runat="server" CssClass="textb" Width="70px">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Gross Wt.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtgrosswt" runat="server" CssClass="textb" Width="80px">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Net Wt.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtnetwt" runat="server" CssClass="textb" Width="80px">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vol">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtvol" runat="server" CssClass="textb" Width="80px">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pcs">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtpcs" runat="server" CssClass="textb" Width="80px">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Button ID="ButtonAdd" runat="server" Text="Add New Row" CssClass="buttonnorm"
                                                    OnClick="ButtonAdd_Click" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align: right">
                            <asp:Button ID="btnnew" runat="server" CssClass="buttonnorm" Text="New" OnClientClick="return NewForm();" />
                            <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" OnClick="btnsave_Click" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblmsg" Text="" runat="server" CssClass="labelbold" ForeColor="Red" />
                        </td>
                    </tr>
                </table>

                <table id="TableGridData" runat="server" visible="false" style="width: 100%">
                <tr>
                    <td>
                        <div id="DivDGDetail" style="max-height: 500px; width: 800px; overflow: scroll;">
                            <asp:GridView ID="DGDetail" runat="server"  AutoGenerateColumns="False" AllowPaging="True" PageSize="100" DataKeyNames="ArticleNo"
                            OnPageIndexChanging="DGDetail_PageIndexChanging" OnRowDataBound="DGDetail_RowDataBound" OnSelectedIndexChanged="DGDetail_SelectedIndexChanged">
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <Columns> 
                                    <asp:TemplateField HeaderText="Article No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblArticleNo" Text='<%#Bind("ArticleNo") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Packing Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPackingType" Text='<%#Bind("PackingType") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>                                   
                                    <asp:TemplateField HeaderText="Item Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemName" Text='<%#Bind("Item_Name") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>                                    
                                    <asp:TemplateField HeaderText="Quality">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQualityName" Text='<%#Bind("QualityName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>                                    
                                    <asp:TemplateField HeaderText="Design">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesignName" Text='<%#Bind("DesignName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>                                   
                                    <asp:TemplateField HeaderText="Color" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblColorName" Text='<%#Bind("ColorName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Shape" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblShapeName" Text='<%#Bind("ShapeName") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Size" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSizeName" Text='<%#Bind("SizeMtr") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Weight Roll" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWeightRoll" Text='<%#Bind("Weight_roll") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Net WT" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNetWt" Text='<%#Bind("Netwt") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Volume Roll" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVolumeRoll" Text='<%#Bind("volume_roll") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pcs Roll" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPcsRoll" Text='<%#Bind("Pcs_roll") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   <%-- <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblArticleNo" runat="server" Text='<%#Bind("ArticleNo") %>'></asp:Label>                                            
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                 <tr>
                        <td  style="text-align: right; padding-right:200px;">                            
                            <asp:Button ID="BtnPreview" runat="server" CssClass="buttonnorm" Text="Preview" OnClick="BtnPreview_Click" />                            
                        </td>
                    </tr>

            </table>

            </div>
        </ContentTemplate>
        <Triggers>
                <asp:PostBackTrigger ControlID="BtnPreview" />
            </Triggers>
    </asp:UpdatePanel>
</asp:Content>
