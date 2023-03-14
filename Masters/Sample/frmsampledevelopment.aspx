<%@ Page Title="Sample Development" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmsampledevelopment.aspx.cs" Inherits="Masters_Process_frmsampledevelopment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="page" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function ClickNew() {
            window.location.href = "frmsampledevelopment.aspx";
        }
        function Samplecodeselected(source, eventArgs) {
            document.getElementById('<%=hnid.ClientID%>').value = eventArgs.get_value();
            document.getElementById('<%=btnSearch.ClientID%>').click();
        }
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <asp:TabContainer ID="tbsample" runat="server" Width="100%" ActiveTabIndex="0">
                <asp:TabPanel ID="tabmasterdetail" HeaderText="Product Detail" runat="server">
                    <ContentTemplate>
                        <div>
                            <div style="width: 60%; float: left">
                                <table>
                                    <tr>
                                        <td id="TDEdit" runat="server" visible="False">
                                            <asp:CheckBox ID="chkedit" Text="For Edit" CssClass="checkboxbold" runat="server"
                                                Font-Size="Medium" AutoPostBack="True" OnCheckedChanged="chkedit_CheckedChanged" />
                                            &nbsp;
                                            <asp:CheckBox ID="ChkForWeavingConsumptionSave" Visible="false" Text="For Save Weaving Consumption"
                                                CssClass="checkboxbold" runat="server" Font-Size="Medium" />
                                            &nbsp;&nbsp;&nbsp;<asp:TextBox ID="TxtSampleCodeNew" CssClass="textboxm" runat="server"
                                                Visible="False" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr id="TRSearch" runat="server" visible="False">
                                        <td id="Td1" runat="server">
                                            <fieldset>
                                                <legend>
                                                    <asp:Label ID="lbltype" Text="Search" CssClass="labelbold" ForeColor="Red" Font-Bold="True"
                                                        runat="server" />
                                                </legend>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label17" Text="Buyer" runat="server" CssClass="labelbold" />
                                                        </td>
                                                        <td id="TD2" runat="server">
                                                            <asp:DropDownList ID="DDEbuyer" CssClass="dropdown" Width="150px" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbledittech" Text="Technique" runat="server" CssClass="labelbold" />
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddEitem" runat="server" Width="150px" CssClass="dropdown" AutoPostBack="True"
                                                                OnSelectedIndexChanged="ddEitem_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td id="Td3" runat="server">
                                                            <asp:Label ID="Label14" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                                                        </td>
                                                        <td id="Td4" runat="server">
                                                            <asp:DropDownList ID="ddEquality" runat="server" CssClass="dropdown" Width="150px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label15" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="dddesign" runat="server" CssClass="dropdown" Width="150px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label16" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddcolor" runat="server" CssClass="dropdown" Width="150px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:LinkButton ID="lnkgetsamplecode" CssClass="labelbold" runat="server" Font-Bold="True"
                                                                Text="Get Sample Code" ForeColor="Red" OnClick="lnkgetsamplecode_Click"></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 80%">
                                    <tr id="TRSamplecode" runat="server" visible="False">
                                        <td id="Td5" runat="server">
                                            <asp:Label ID="Label13" Text="Type Sample Code" CssClass="labelbold" runat="server" />
                                            <asp:Button ID="btnSearch" runat="server" Text="Button" OnClick="btnSearch_Click"
                                                Style="display: none" />
                                        </td>
                                        <td id="Td6" runat="server" visible="False">
                                            <asp:TextBox ID="txttypesamplecode" CssClass="textboxm" runat="server" Width="150px" />
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtendersamplecode" runat="server" BehaviorID="SampleSrchAutoComplete"
                                                CompletionInterval="20" Enabled="True" ServiceMethod="Getsamplecodedevelopment"
                                                CompletionSetCount="30" OnClientItemSelected="Samplecodeselected" ServicePath="~/Autocomplete.asmx"
                                                TargetControlID="txttypesamplecode" UseContextKey="True" ContextKey="0" MinimumPrefixLength="1"
                                                DelimiterCharacters="">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                        <td id="Td7" runat="server">
                                            <asp:DropDownList ID="DDsamplecode" CssClass="dropdown" Width="150px" runat="server"
                                                AutoPostBack="True" OnSelectedIndexChanged="DDsamplecode_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblsamplecode" CssClass="labelbold" Text="Sample Code" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtsamplecode" CssClass="textboxm" Enabled="False" runat="server"
                                                Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label23" CssClass="labelbold" Text="Date" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtDate" CssClass="textboxm" runat="server" Width="150px" />
                                            <asp:CalendarExtender ID="calfrom" runat="server" TargetControlID="TxtDate" Format="dd-MMM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" CssClass="labelbold" Text="Month" runat="server" />
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="DDMonth" CssClass="dropdown" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblyear" CssClass="labelbold" Text="Year" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="DDyear" CssClass="dropdown" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblpurpose" Text="Purpose" runat="server" CssClass="labelbold" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDpurpose" runat="server" CssClass="dropdown" AutoPostBack="True"
                                                Width="150px" OnSelectedIndexChanged="DDpurpose_SelectedIndexChanged">
                                                <asp:ListItem Text="GENERAL" />
                                                <asp:ListItem Text="BUYER" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" Text="Buyer/General" CssClass="labelbold" runat="server" />
                                        </td>
                                        <td id="TDgeneral" runat="server">
                                            <asp:TextBox ID="txtgeneral" CssClass="textboxm" runat="server" Width="150px" />
                                        </td>
                                        <td id="TDbuyer" runat="server" visible="False">
                                            <asp:DropDownList ID="DDbuyer" CssClass="dropdown" Width="150px" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblcategoryname" runat="server" Text="Category" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddCatagory" runat="server" Width="150px" CssClass="dropdown"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblitemname" runat="server" Text="Technique" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="dditemname" runat="server" Width="150px" CssClass="dropdown"
                                                AutoPostBack="True" OnSelectedIndexChanged="dditemname_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TDQuality" runat="server" visible="False">
                                        <td id="Td8" runat="server">
                                            <asp:Label ID="lblqualityname" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td id="Td9" runat="server">
                                            <asp:DropDownList ID="dquality" runat="server" CssClass="dropdown" AutoPostBack="True"
                                                Width="150px" OnSelectedIndexChanged="dquality_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbldesignname" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDDesignName" runat="server" CssClass="dropdown" AutoPostBack="True"
                                                Width="150px" OnSelectedIndexChanged="DDDesignName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblcolorname" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDColorName" runat="server" CssClass="dropdown" Width="150px"
                                                AutoPostBack="True" OnSelectedIndexChanged="DDColorName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TDShape" runat="server" visible="False">
                                        <td id="Td10" runat="server">
                                            <asp:Label ID="lblshapename" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td id="Td11" runat="server">
                                            <asp:DropDownList ID="ddshape" runat="server" Width="150px" AutoPostBack="True" CssClass="dropdown"
                                                OnSelectedIndexChanged="ddshape_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TDSize" runat="server" visible="False">
                                        <td id="Td12" runat="server">
                                            <asp:Label ID="lblsizename" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td id="Td13" runat="server">
                                            <asp:DropDownList CssClass="dropdown" ID="DDsizetype" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="DDsizetype_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <br />
                                            <asp:DropDownList ID="ddsize" runat="server" Width="150px" CssClass="dropdown" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddsize_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="width: 40%; float: right">
                                <table style="width: 100%;">
                                    <tr>
                                        <td>
                                            <asp:Image ID="lblimage" runat="server" Height="150px" Width="170px" />
                                        </td>
                                        <tr>
                                            <td>
                                                <asp:FileUpload ID="PhotoImage" ViewStateMode="Enabled" runat="server" />
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Only .gif,.jpeg or.jpg files are allowed!"
                                                    ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|BMP|bmp)$" ControlToValidate="PhotoImage"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <fieldset style="width: 220px">
                                                    <legend>
                                                        <asp:Label ID="lblcadupload" Text="CAD UPLOAD" CssClass="labelbold" runat="server" />
                                                    </legend>
                                                    <asp:Image ID="imgcadupload" runat="server" Height="150px" Width="170px" />
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:FileUpload ID="Photocadimage" ViewStateMode="Enabled" runat="server" />
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Only .gif,.jpeg or.jpg files are allowed!"
                                                    ValidationExpression="^.*\.(jpg|JPG|gif|GIF|jpeg|JPEG|BMP|bmp)$" ControlToValidate="PhotoImage"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:FileUpload ID="fileupload" runat="server" />
                                                <asp:RegularExpressionValidator ID="regpdf" Text="Please upload only excel files"
                                                    ForeColor="Red" ErrorMessage="Please upload only excel files" ControlToValidate="fileupload"
                                                    ValidationExpression="^.*\.(xls|xlsx)$" runat="server" />
                                            </td>
                                        </tr>
                                        <%--<tr>
                                            <td>
                                                <asp:Button ID="BtnGetCostingFile" runat="server" CssClass="buttonnorm" Text="Save"
                                                    OnClick="BtnGetCostingFile_Click" OnClientClick="return confirm('Do you want to get costing file?');" />
                                            </td>
                                        </tr>--%>
                                </table>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="TBProductSplitDetail" HeaderText="Product Split Detail" runat="server">
                    <HeaderTemplate>
                        Product Split Detail
                    </HeaderTemplate>
                    <ContentTemplate>
                        <div>
                            <div style="width: 50%; float: left">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSplitcategoryname" runat="server" Text="Category" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddSplitCatagory" runat="server" Width="150px" CssClass="dropdown"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddSplitCatagory_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSplitItemname" runat="server" Text="Technique" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddSplitItemname" runat="server" Width="150px" CssClass="dropdown"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddSplitItemname_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRSplitQuality" runat="server" visible="False">
                                        <td runat="server">
                                            <asp:Label ID="lblSplitQualityname" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="DDSplitQuality" runat="server" CssClass="dropdown" Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRSplitDesignName" runat="server">
                                        <td runat="server">
                                            <asp:Label ID="lblSplitDesignName" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="DDSplitDesignName" runat="server" CssClass="dropdown" Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRSplitColorName" runat="server">
                                        <td runat="server">
                                            <asp:Label ID="lblSplitColorName" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="DDSplitColorName" runat="server" CssClass="dropdown" Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRSplitShape" runat="server">
                                        <td runat="server">
                                            <asp:Label ID="lblSplitShapeName" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="DDSplitShape" runat="server" Width="150px" AutoPostBack="True"
                                                CssClass="dropdown" OnSelectedIndexChanged="DDSplitShape_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRSplitSize" runat="server">
                                        <td runat="server">
                                            <asp:Label ID="lblSplitSizeName" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddSplitSize" runat="server" Width="150px" CssClass="dropdown">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TRSplitShadeColor" runat="server">
                                        <td runat="server">
                                            <asp:Label ID="lblSplitShadeColor" runat="server" Text="Shade Color" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td runat="server">
                                            <asp:DropDownList ID="ddSplitShadeColor" runat="server" Width="150px" CssClass="dropdown">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="LblSplitFinishedID" runat="server" Text="" Visible="false" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Button ID="BtnOK" runat="server" CssClass="buttonnorm" Text="OK" OnClick="OK_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="width: 45%; margin-left: 5%; float: right">
                                <div style="max-height: 250px; overflow: auto">
                                    <asp:GridView ID="GDSplitDescription" runat="server" AutoGenerateColumns="False"
                                        AutoGenerateSelectButton="true" OnSelectedIndexChanged="GDSplitDescription_SelectedIndexChanged"
                                        DataKeyNames="FinishedID" OnRowDeleting="GDSplitDescription_RowDeleting">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Item Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblItemName" Text='<%#Bind("ItemName") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quality">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuality" Text='<%#Bind("Quality") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDescription" Text='<%#Bind("Description") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item_Finished_id" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFinishedID" Text='<%#Bind("FinishedID") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:CommandField ShowDeleteButton="True" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tabrawmaterial" HeaderText="Raw Material Description" runat="server">
                    <ContentTemplate>
                        <div>
                            <div style="width: 50%; float: left">
                                <table>
                                    <tr id="Tr1" runat="server" visible="False">
                                        <td id="Td14" runat="server">
                                            <asp:Label ID="Label12" runat="server" Text="Product Code" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td id="Td15" runat="server">
                                            <asp:TextBox ID="TxtProdCode" Width="150px" CssClass="textboxm" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label21" runat="server" Text="Process Name" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDRProcessName" runat="server" Width="150px" CssClass="dropdown"
                                                AutoPostBack="True" OnSelectedIndexChanged="DDRProcessName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label22" runat="server" Text="Cal Type" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList CssClass="dropdown" ID="DDRCalType" runat="server" Width="100px">
                                                <asp:ListItem Value="0">AREA</asp:ListItem>
                                                <asp:ListItem Value="1">PCS</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="Category Name" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDRCategory" runat="server" Width="150px" CssClass="dropdown"
                                                AutoPostBack="True" OnSelectedIndexChanged="DDRCategory_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="Item Name" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDRitemname" runat="server" Width="150px" CssClass="dropdown"
                                                AutoPostBack="True" OnSelectedIndexChanged="DDRitemname_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TDRQuality" runat="server" visible="False">
                                        <td id="Td16" runat="server">
                                            <asp:Label ID="Label5" runat="server" Text="Quality" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td id="Td17" runat="server">
                                            <asp:DropDownList ID="DDRquality" runat="server" CssClass="dropdown" AutoPostBack="True"
                                                Width="150px" OnSelectedIndexChanged="DDRquality_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TDRDesign" runat="server" visible="False">
                                        <td id="Td18" runat="server">
                                            <asp:Label ID="Label6" runat="server" Text="Design" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td id="Td19" runat="server">
                                            <asp:DropDownList ID="ddRdesign" runat="server" Width="150px" CssClass="dropdown">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TDRColor" runat="server" visible="False">
                                        <td id="Td20" runat="server">
                                            <asp:Label ID="Label7" runat="server" Text="Color" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td id="Td21" runat="server">
                                            <asp:DropDownList ID="ddRcolor" runat="server" Width="150px" CssClass="dropdown">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TDRShape" runat="server" visible="False">
                                        <td id="Td22" runat="server">
                                            <asp:Label ID="Label8" runat="server" Text="Shape" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td id="Td23" runat="server">
                                            <asp:DropDownList ID="DDRshape" runat="server" Width="150px" AutoPostBack="True"
                                                CssClass="dropdown" OnSelectedIndexChanged="DDRshape_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="TDRSize" runat="server" visible="False">
                                        <td id="Td24" runat="server">
                                            <asp:Label ID="Label9" runat="server" Text="Size" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td id="Td25" runat="server">
                                            <asp:DropDownList ID="DDRSizetype" runat="server" CssClass="dropdown" AutoPostBack="True"
                                                OnSelectedIndexChanged="DDRSizetype_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <br />
                                            <asp:DropDownList CssClass="dropdown" Width="150px" ID="DDRsize" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="Td26" runat="server">
                                            <asp:Label ID="Label18" runat="server" Text="Lot No." CssClass="labelbold"></asp:Label>
                                            &nbsp;&nbsp;
                                            <asp:CheckBox ID="ChkForAllLotNoofItem" Text="All Lot" CssClass="checkbox" runat="server"
                                                AutoPostBack="True" OnCheckedChanged="ChkForAllLotNoofItem_CheckedChanged" />
                                        </td>
                                        <td id="Td27" runat="server">
                                            <asp:DropDownList ID="DDLotno" runat="server" Width="150px" CssClass="dropdown" OnSelectedIndexChanged="DDLotno_SelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblvendorname" CssClass="labelbold" ForeColor="Red" runat="server" />
                                        </td>
                                    </tr>
                                    <tr id="TDRShade" runat="server" visible="False">
                                        <td id="Td28" runat="server">
                                            <asp:Label ID="lblshadecolor" runat="server" Text="ShadeColor" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td id="Td29" runat="server">
                                            <asp:DropDownList ID="ddRlshade" runat="server" Width="150px" CssClass="dropdown">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="Tr2" runat="server">
                                        <td id="Td30" runat="server">
                                            <asp:Label ID="Label19" runat="server" Text="Dyeing Type" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td id="Td31" runat="server">
                                            <asp:DropDownList ID="DDDyeingtype" runat="server" Width="150px" CssClass="dropdown">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="Tr3" runat="server">
                                        <td id="Td32" runat="server">
                                            <asp:Label ID="Label24" runat="server" Text="Unit" CssClass="labelbold"></asp:Label>
                                        </td>
                                        <td id="Td33" runat="server">
                                            <asp:DropDownList ID="DDUnit" runat="server" Width="150px" CssClass="dropdown">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblantiwt" Text="Development Cons" CssClass="labelbold" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtanticipatedwt" Width="100px" CssClass="textboxm" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label25" Text="Prod Cons" CssClass="labelbold" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtprodwt" Width="100px" CssClass="textboxm" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnaddmaterial" Text="Add Material" runat="server" CssClass="buttonnorm"
                                                OnClick="btnaddmaterial_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <fieldset>
                                    <legend>
                                        <asp:Label ID="lblwt" Text="Total Raw Material Weight" ForeColor="Red" runat="server"
                                            CssClass="labelbold" />
                                    </legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblwtgsm" Text="Weight GSM" runat="server" CssClass="labelbold" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtwtgsm" CssClass="textboxm" Width="100px" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label10" Text="Total Wt" runat="server" CssClass="labelbold" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txttotalgsm" CssClass="textboxm" Width="100px" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                            <div style="width: 45%; margin-left: 5%; float: right">
                                <div style="max-height: 250px; overflow: auto">
                                    <asp:GridView ID="DGraw" runat="server" AutoGenerateColumns="False" OnRowDeleting="DGraw_RowDeleting"
                                        OnDataBound="DGraw_DataBound" OnRowDataBound="DGraw_RowDataBound">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Item Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemdesc" Text='<%#Bind("ItemDescription") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="250px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Lot No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbllotno" Text='<%#Bind("Lotno") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Dyeing Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldyeingtype" Text='<%#Bind("Dyeingtype") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unit">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUnit" Text='<%#Bind("UnitName") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Development Cons">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblanticipatedwt" Enabled="false" Text='<%#Bind("anticipatedwt") %>'
                                                        Width="70px" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="100px" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Production Cons">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtactualwt" Text='<%#Bind("actualwt") %>' Enabled="false" Width="70px"
                                                        runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="100px" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Vendor Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblvendornamegrid" Text='<%#Bind("vendorname") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item_Finished_id" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblitemfinishedid" Text='<%#Bind("Item_Finished_id") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ProcessID" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProcessID" Text='<%#Bind("ProcessID") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CalType" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCalType" Text='<%#Bind("CalType") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="UnitID" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUnitID" Text='<%#Bind("UnitID") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:CommandField ShowDeleteButton="True" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tabvendordetail" HeaderText="Vendor Detail" runat="server">
                    <ContentTemplate>
                        <div>
                            <div style="width: 50%; float: left">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbldept" Text="Department" CssClass="labelbold" runat="server" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDDept" runat="server" Width="200px" CssClass="dropdown" AutoPostBack="True"
                                                OnSelectedIndexChanged="DDDept_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:Label ID="Label11" Text="Vendor" CssClass="labelbold" runat="server" />
                                        </td>
                                        <td>
                                            <div style="overflow: scroll; height: 140px; width: 200PX">
                                                <asp:CheckBoxList ID="chkemp" CssClass="checkboxbold" runat="server">
                                                </asp:CheckBoxList>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnaddvendor" CssClass="buttonnorm" Text="Add Vendor" runat="server"
                                                OnClick="btnaddvendor_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="width: 50%; float: right">
                                <div style="max-height: 300px; overflow: auto">
                                    <asp:GridView ID="DGvendor" runat="server" AutoGenerateColumns="False" OnRowDeleting="DGvendor_RowDeleting"
                                        OnSorting="DGvendor_Sorting">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Department" SortExpression="Department">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldept" Text='<%#Bind("Department") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Vendor" SortExpression="Vendor">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblvendor" Text='<%#Bind("Vendor") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="250px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ID" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldeptid" Text='<%#Bind("Departmentid") %>' runat="server" />
                                                    <asp:Label ID="lblempid" Text='<%#Bind("empid") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:CommandField ShowDeleteButton="True" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
            <div>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblremark" Text="Remark" runat="server" CssClass="labelbold" />
                            <br />
                            <asp:TextBox ID="txtremark" CssClass="textbox" TextMode="MultiLine" Width="479px"
                                runat="server" Height="61px" />
                        </td>
                        <td valign="middle">
                            <asp:Button ID="btnnew" runat="server" CssClass="buttonnorm" Text="New" OnClientClick="return ClickNew();" />
                            <asp:Button ID="btndelete" runat="server" CssClass="buttonnorm" Text="Delete" OnClientClick="return confirm('Do you want to delete this sample code?')"
                                OnClick="btndelete_Click" Visible="false" />
                            <asp:Button ID="btnsave" runat="server" CssClass="buttonnorm" Text="Save" OnClick="btnsave_Click"
                                OnClientClick="return confirm('Do you want to save Data?');" />
                            <asp:Button ID="btnpreview" Text="Preview" runat="server" CssClass="buttonnorm" OnClick="btnpreview_Click" />
                            <asp:Button ID="btnclose" runat="server" CssClass="buttonnorm" Text="Close" OnClientClick="return CloseForm();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" runat="server" />
                            <asp:Label ID="lblDeleteButtonCall" Text="Remark" runat="server" CssClass="labelbold"
                                Visible="false" />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hnid" runat="server" Value="0" />
            <style type="text/css">
                #mask
                {
                    position: fixed;
                    left: 0px;
                    top: 0px;
                    z-index: 4;
                    opacity: 0.4;
                    -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=40)"; /* first!*/
                    filter: alpha(opacity=40); /* second!*/
                    background-color: Gray;
                    display: none;
                    width: 100%;
                    height: 100%;
                }
            </style>
            <script type="text/javascript" language="javascript">
                function ShowPopup() {
                    $('#mask').show();
                    $('#<%=pnlpopup.ClientID %>').show();
                }
                function HidePopup() {
                    $('#mask').hide();
                    $('#<%=pnlpopup.ClientID %>').hide();
                }
                $(".btnPwd").live('click', function () {
                    HidePopup();
                });
            </script>
            <div id="mask">
            </div>
            <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Height="175px" Width="300px"
                Style="z-index: 111; background-color: White; position: absolute; left: 35%;
                top: 40%; border: outset 2px gray; padding: 5px; display: none">
                <table width="100%" style="width: 100%; height: 100%;" cellpadding="0" cellspacing="5">
                    <tr style="background-color: #8B7B8B; height: 1px">
                        <td colspan="2" style="color: White; font-weight: bold; font-size: 1.2em; padding: 3px"
                            align="center">
                            ENTER PASSWORD <a id="closebtn" style="color: white; float: right; text-decoration: none"
                                class="btnPwd" href="#">X</a>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Enter Password:
                        </td>
                        <td>
                            <asp:TextBox ID="txtpwd" runat="server" TextMode="Password" Height="30px" Width="174px"
                                OnTextChanged="txtpwd_TextChanged" AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="right">
                            <input type="button" value="Cancel" class="btnPwd" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="Label20" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnsave" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
