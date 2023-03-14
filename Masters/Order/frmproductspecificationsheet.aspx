<%@ Page Title="Product Specification Sheet" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmproductspecificationsheet.aspx.cs" Inherits="Masters_Order_frmproductspecificationsheet" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function ClickNew() {
            window.location.href = "frmproductspecificationsheet.aspx";
        }
        function DesignSearch(source, eventArgs) {
            document.getElementById('<%=btnsearch.ClientID%>').click();
        }
       
    </script>
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div>
                <table>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkedit" Text="FOR EDIT" CssClass="checkboxbold" AutoPostBack="True"
                                runat="server" OnCheckedChanged="chkedit_CheckedChanged" />
                        </td>
                        <td>
                        </td>
                        <td id="TdDesignsearch" runat="server" visible="false">
                            <asp:TextBox ID="txtdesignsearch" Placeholder="Type Design here to search Doc No."
                                runat="server" Width="235px" CssClass="textb" />
                            <asp:Button ID="btnsearch" runat="server" Text="Button" OnClick="btnsearch_Click"
                                Style="display: none;" />
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="SrchAutoComplete1"
                                CompletionInterval="20" Enabled="True" ServiceMethod="GetProductionSpecDesignName"
                                EnableCaching="true" CompletionSetCount="20" OnClientItemSelected="DesignSearch"
                                ServicePath="~/Autocomplete.asmx" TargetControlID="txtdesignsearch" UseContextKey="True"
                                ContextKey="0#0#0" MinimumPrefixLength="2">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td colspan="2" id="TDbuyersearch" runat="server" visible="false">
                            <asp:TextBox ID="txtbuyersearch" Placeholder="Type Buyer Name here to search Doc No."
                                runat="server" Width="235px" CssClass="textb" />
                            <asp:Button ID="btnsearchbuyer" runat="server" Text="Button" Style="display: none;"
                                OnClick="btnsearchbuyer_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblcompany" Text="Company Name" CssClass="labelbold" runat="server" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDcompanyName" CssClass="dropdown" Width="200px" runat="server"
                                OnSelectedIndexChanged="DDcompanyName_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td id="TDDocno" runat="server" visible="false">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label38" Text="Doc No." CssClass="labelbold" runat="server" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDDocNo" CssClass="dropdown" Width="200px" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="DDDocNo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <asp:Label Text="System Gen. Doc  No." CssClass="labelbold" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtdocno" CssClass="textb" runat="server" Enabled="false" BackColor="LightGray" />
                        </td>
                    </tr>
                </table>
                <table border="1" cellpadding="10" cellspacing="0" style="width: 100%">
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label Text="Date :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 40%; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtdate" CssClass="textb" runat="server" Width="90%" />
                        </td>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label1" Text="Product Developer :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 40%; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtproductdeveloper" CssClass="textb" runat="server" Width="90%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label2" Text="Merchandiser :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 40%; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtmerchandiser" CssClass="textb" runat="server" Width="90%" />
                        </td>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label3" Text="Buyer Name :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 40%; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtbuyerName" CssClass="textb" runat="server" Width="90%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label4" Text="Type of Product :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 40%; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txttypeofproduct" CssClass="textb" runat="server" Width="90%" />
                        </td>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label5" Text="Product Image :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 40%; padding: 2px 2px 2px 2px">
                        </td>
                    </tr>
                </table>
                <div style="width: 100%">
                    <div style="width: 50%; float: left">
                        <table border="1" cellpadding="10" cellspacing="0" style="width: 100%">
                            <tr>
                                <td style="width: 10%; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="Label6" Text="Style/Design name :" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 40%; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtstyle_designname" CssClass="textb" runat="server" Width="90%" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="Label7" Text="Colors :" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 40%; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtcolors" CssClass="textb" runat="server" Width="90%" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="Label8" Text="Weight of Pcs :" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 40%; padding: 2px 2px 2px 2px">
                                    <table style="width: 100%" border="1" cellpadding="10" cellspacing="0">
                                        <tr>
                                            <td style="width: 10%; padding: 2px 2px 2px 2px">
                                                <asp:Label ID="Label9" Text="Raw :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 90%; padding: 2px 2px 2px 2px">
                                                <asp:TextBox ID="txtrawweight" CssClass="textb" Width="90%" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%; padding: 2px 2px 2px 2px">
                                                <asp:Label ID="Label10" Text="Finish :" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 90%; padding: 2px 2px 2px 2px">
                                                <asp:TextBox ID="txtfinishweight" CssClass="textb" Width="90%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="Label11" Text="Wool :" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 40%; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtwool" CssClass="textb" runat="server" Width="90%" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="Label12" Text="Cotton :" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 40%; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtcotton" CssClass="textb" runat="server" Width="90%" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="Label13" Text="Viscose :" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 40%; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtviscose" CssClass="textb" runat="server" Width="90%" />
                                </td>
                            </tr>
                            <tr>
                                <%--                                <td style="width: 10%; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="Label57" Text="Rawmaterial 4 :" CssClass="labelbold" runat="server" />
                                </td>--%>
                                <td style="width: 40%; padding: 2px 2px 2px 2px" colspan="2">
                                    <asp:TextBox ID="TxtRawMaterial4" CssClass="textb" runat="server" Width="90%" />
                                </td>
                            </tr>
                            <tr>
                                <%--<td style="width: 10%; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="Label58" Text="Rawmaterial 5 :" CssClass="labelbold" runat="server" />
                                </td>--%>
                                <td style="width: 40%; padding: 2px 2px 2px 2px" colspan="2">
                                    <asp:TextBox ID="TxtRawMaterial5" CssClass="textb" runat="server" Width="90%" />
                                </td>
                            </tr>
                            <tr>
                                <%--<td style="width: 10%; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="Label59" Text="Rawmaterial 6 :" CssClass="labelbold" runat="server" />
                                </td>--%>
                                <td style="width: 40%; padding: 2px 2px 2px 2px" colspan="2">
                                    <asp:TextBox ID="TxtRawMaterial6" CssClass="textb" runat="server" Width="90%" />
                                </td>
                            </tr>
                            <tr runat="server" visible="false">
                                <td style="width: 10%; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="Label60" Text="Rawmaterial 7 :" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 40%; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="TxtRawMaterial7" CssClass="textb" runat="server" Width="90%" />
                                </td>
                            </tr>
                            <tr id="Tr1" runat="server" visible="false">
                                <td style="width: 10%; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="Label61" Text="Rawmaterial 8 :" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 40%; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="TxtRawMaterial8" CssClass="textb" runat="server" Width="90%" />
                                </td>
                            </tr>
                            <tr id="Tr2" runat="server" visible="false">
                                <td style="width: 10%; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="Label62" Text="Rawmaterial 9 :" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 40%; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="TxtRawMaterial9" CssClass="textb" runat="server" Width="90%" />
                                </td>
                            </tr>
                            <tr id="Tr3" runat="server" visible="false">
                                <td style="width: 11%; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="Label63" Text="Rawmaterial 10 :" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 40%; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="TxtRawMaterial10" CssClass="textb" runat="server" Width="90%" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="Label14" Text="Tufting Cloth :" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 40%; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txttuftingcloth" CssClass="textb" runat="server" Width="90%" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="Label15" Text="3rd Backing Cloth :" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 40%; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtthirdbackingcloth" CssClass="textb" runat="server" Width="90%" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="Label16" Text="Niwar Width :" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 40%; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtniwarwidth" CssClass="textb" runat="server" Width="90%" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="width: 40%; float: right">
                        <table>
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
                            </tr>
                        </table>
                    </div>
                </div>
                <table border="1" cellpadding="10" cellspacing="0" style="width: 100%">
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label17" Text="Type of Dyeing :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txttypeofdyeing" CssClass="textb" runat="server" Width="95%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label18" Text="Warp :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtwarp" CssClass="textb" runat="server" Width="95%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label19" Text="Weft :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtweft" CssClass="textb" runat="server" Width="95%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label20" Text="Reed :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtreeds" CssClass="textb" runat="server" Width="95%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label21" Text="Picks :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtpicks" CssClass="textb" runat="server" Width="95%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label22" Text="Tuft density :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txttuftdensity" CssClass="textb" runat="server" Width="95%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label23" Text="Weaving Pattern :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtweavingpattern" CssClass="textb" runat="server" Width="95%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label24" Text="Pile height :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 40%; padding: 2px 2px 2px 2px">
                            <table style="width: 100%" border="1" cellpadding="10" cellspacing="0">
                                <tr>
                                    <td style="width: 10%; padding: 2px 2px 2px 2px">
                                        <asp:Label ID="Label25" Text="Raw Loop:" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 90%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtrawpileheightLoop" CssClass="textb" Width="95%" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%; padding: 2px 2px 2px 2px">
                                        <asp:Label ID="Label39" Text="Raw Cut:" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 90%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtRawPileHeightCut" CssClass="textb" Width="95%" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%; padding: 2px 2px 2px 2px">
                                        <asp:Label ID="Label26" Text="Finish Loop:" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 90%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtfinishpileheightLoop" CssClass="textb" Width="95%" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%; padding: 2px 2px 2px 2px">
                                        <asp:Label ID="Label40" Text="Finish Cut:" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 90%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtFinishPileHeightCut" CssClass="textb" Width="95%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="width: 100%; padding: 2px 2px 2px 2px; text-align: center">
                            <asp:Label ID="Label41" Text="Size Tolerence" CssClass="labelbold" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label42" Text="Actual Size :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <table style="width: 100%" border="1" cellpadding="10" cellspacing="0">
                                <tr>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtSizeTolerenceActualSize1" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtSizeTolerenceActualSize2" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtSizeTolerenceActualSize3" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtSizeTolerenceActualSize4" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtSizeTolerenceActualSize5" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label43" Text="Length :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <table style="width: 100%" border="1" cellpadding="10" cellspacing="0">
                                <tr>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtSizeTolerenceLength1" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtSizeTolerenceLength2" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtSizeTolerenceLength3" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtSizeTolerenceLength4" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtSizeTolerenceLength5" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label44" Text="Breadth :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <table style="width: 100%" border="1" cellpadding="10" cellspacing="0">
                                <tr>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtSizeTolerenceBreadth1" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtSizeTolerenceBreadth2" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtSizeTolerenceBreadth3" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtSizeTolerenceBreadth4" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtSizeTolerenceBreadth5" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="width: 100%; padding: 2px 2px 2px 2px; text-align: center">
                            <asp:Label ID="Label45" Text="Weight Tolerence" CssClass="labelbold" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label46" Text="Raw Actual Weight :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <table style="width: 100%" border="1" cellpadding="10" cellspacing="0">
                                <tr>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceActualWt1" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceActualWt2" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceActualWt3" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceActualWt4" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceActualWt5" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label47" Text="Raw Weight Min :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <table style="width: 100%" border="1" cellpadding="10" cellspacing="0">
                                <tr>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceRawWtMin1" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceRawWtMin2" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceRawWtMin3" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceRawWtMin4" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceRawWtMin5" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label48" Text="Raw Weight Max :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <table style="width: 100%" border="1" cellpadding="10" cellspacing="0">
                                <tr>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceRawWtMax1" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceRawWtMax2" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceRawWtMax3" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceRawWtMax4" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceRawWtMax5" CssClass="textb" runat="server" Width="95%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label49" Text="Finish Actual Weight :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <table style="width: 100%" border="1" cellpadding="10" cellspacing="0">
                                <tr>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceFinishActualWt1" CssClass="textb" runat="server"
                                            Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceFinishActualWt2" CssClass="textb" runat="server"
                                            Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceFinishActualWt3" CssClass="textb" runat="server"
                                            Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceFinishActualWt4" CssClass="textb" runat="server"
                                            Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceFinishActualWt5" CssClass="textb" runat="server"
                                            Width="95%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label50" Text="Finished Weight Min :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <table style="width: 100%" border="1" cellpadding="10" cellspacing="0">
                                <tr>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceFinishWtMin1" CssClass="textb" runat="server"
                                            Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceFinishWtMin2" CssClass="textb" runat="server"
                                            Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceFinishWtMin3" CssClass="textb" runat="server"
                                            Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceFinishWtMin4" CssClass="textb" runat="server"
                                            Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceFinishWtMin5" CssClass="textb" runat="server"
                                            Width="95%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label51" Text="Finished Weight Max :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <table style="width: 100%" border="1" cellpadding="10" cellspacing="0">
                                <tr>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceFinishWtMax1" CssClass="textb" runat="server"
                                            Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceFinishWtMax2" CssClass="textb" runat="server"
                                            Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceFinishWtMax3" CssClass="textb" runat="server"
                                            Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceFinishWtMax4" CssClass="textb" runat="server"
                                            Width="95%" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtWeightTolerenceFinishWtMax5" CssClass="textb" runat="server"
                                            Width="95%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="width: 100%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label29" Text="Binding Details(Rugs) :" CssClass="labelbold" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label30" Text="Type of yarn :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txttypeofyarn" CssClass="textb" runat="server" Width="95%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label31" Text="Ply of yarn :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtplyofyarn" CssClass="textb" runat="server" Width="95%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label32" Text="Stitch per inch :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtstitchperinch" CssClass="textb" runat="server" Width="95%" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="width: 100%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label52" Text="Stitching :" CssClass="labelbold" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label33" Text="SPI :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <table border="1" cellpadding="10" cellspacing="0" style="width: 100%">
                                <tr>
                                    <td style="width: 33%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtStitchingSPI" CssClass="textb" Width="80%" runat="server" />
                                    </td>
                                    <td style="width: 10%; padding: 2px 2px 2px 2px">
                                        <asp:Label ID="Label34" Text="Needle No :" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 56%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtStitchingNeedleNo" CssClass="textb" Width="80%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label35" Text="Sewing Thread :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <table border="1" cellpadding="10" cellspacing="0" style="width: 100%">
                                <tr>
                                    <td style="width: 33%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtStitchingSewingThread" CssClass="textb" Width="80%" runat="server" />
                                    </td>
                                    <td style="width: 10%; padding: 2px 2px 2px 2px">
                                        <asp:Label ID="Label36" Text="Filler Weight :" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 56%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtStitchingFillerWeight" CssClass="textb" Width="80%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label37" Text="Process Flow :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtprocessflow" CssClass="textb" runat="server" Width="95%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label28" Text="Latex Recipe :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtlatexrecipe" CssClass="textb" runat="server" Width="95%" TextMode="MultiLine"
                                Height="50px" />
                        </td>
                    </tr>
                    <tr id="TRTolerenceLimit" runat="server" visible="false">
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label53" Text="Tolerence Limit :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <table border="1" cellpadding="10" cellspacing="0" style="width: 100%">
                                <tr>
                                    <td style="width: 10%; padding: 2px 2px 2px 2px">
                                        <asp:Label ID="Label54" Text="Length :" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtlengthtolerence" CssClass="textb" Width="80%" runat="server" />
                                    </td>
                                    <td style="width: 10%; padding: 2px 2px 2px 2px">
                                        <asp:Label ID="Label55" Text="Width :" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtwidthtolerence" CssClass="textb" Width="80%" runat="server" />
                                    </td>
                                    <td style="width: 10%; padding: 2px 2px 2px 2px">
                                        <asp:Label ID="Label56" Text="Weight :" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:TextBox ID="txtweighttolerence" CssClass="textb" Width="78%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="TRWashing" runat="server" visible="false">
                        <td style="width: 10%; padding: 2px 2px 2px 2px">
                            <asp:Label ID="Label27" Text="Washing :" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <asp:TextBox ID="txtwashing" CssClass="textb" runat="server" Width="95%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <asp:Label ID="lblEnteredBy" CssClass="labelbold" runat="server" />
                        </td>
                        <td style="width: 90%; padding: 2px 2px 2px 2px">
                            <table border="1" cellpadding="10" cellspacing="0" style="width: 100%">
                                <tr>
                                    <td style="width: 25%; padding: 2px 2px 2px 2px">
                                        <asp:Label ID="lblEnteredByText" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td align="right" style="width: 50%; padding: 2px 2px 2px 2px">
                                        <asp:Button ID="btnnew" Text="New" CssClass="buttonnorm" runat="server" OnClientClick="return ClickNew();" />
                                        <asp:Button ID="btnsave" Text="Save" CssClass="buttonnorm" runat="server" OnClick="btnsave_Click" />
                                        <asp:Button ID="btndelete" Text="Delete" CssClass="buttonnorm" runat="server" OnClientClick="return confirm('Do you want to delete this Doc No.?')"
                                            OnClick="btndelete_Click" />
                                        <asp:Button ID="btnpreview" Text="Preview" CssClass="buttonnorm" runat="server" OnClick="btnpreview_Click" />
                                        <asp:Button ID="btnclose" Text="Close" CssClass="buttonnorm" runat="server" OnClientClick="return CloseForm();" />
                                        <asp:Button Text="Approve" ID="btnApprove" CssClass="buttonnorm" runat="server" Visible="false"
                                            OnClick="btnApprove_Click" OnClientClick="return confirm('Do you want to approve Doc No. ?')" />
                                    </td>
                                    <td style="width: 10%; padding: 2px 2px 2px 2px">
                                        <asp:Label ID="lblApprovedBy" CssClass="labelbold" runat="server" />
                                    </td>
                                    <td style="width: 20%; padding: 2px 2px 2px 2px">
                                        <asp:Label ID="lblApprovedByText" CssClass="labelbold" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="width: 100%">
                            <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Font-Size="Small" Text=""
                                runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hndocid" Value="0" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnsave" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
