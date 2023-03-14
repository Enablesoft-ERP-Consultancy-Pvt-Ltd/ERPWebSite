<%@ Page Title="Size Attached" Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="FrmSizeAttachedWithItem.aspx.cs" Inherits="FrmSizeAttachedWithItem" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <link href="../../Styles/vijay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function priview() {
            window.open("../../ReportViewer.aspx");
        }
        function ClickNew() {
            window.location.href = "FrmSizeAttachedWithItem.aspx";
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="height: 450px">
                <table>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtsize" runat="server" CssClass="textb" Visible="false" Width="83px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtid" runat="server" Visible="false" CssClass="textb" Width="80px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            <asp:Label ID="lblew" Text="Category Name" runat="server" Font-Bold="true" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDCategory" runat="server" OnSelectedIndexChanged="DDCategory_SelectedIndexChanged"
                                AutoPostBack="True" CssClass="dropdown" Width="120px">
                            </asp:DropDownList>                           
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblItemName" runat="server" Text="Item Name" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDItem" runat="server" Width="120px" CssClass="dropdown" AutoPostBack="true" 
                            OnSelectedIndexChanged="DDItem_SelectedIndexChanged">
                                
                            </asp:DropDownList>
                           <%-- <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDItem"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>--%>
                        </td>

                         <td id="TDQuality" runat="server" visible="false" class="tdstyle">
                            <asp:Label ID="Label31" runat="server" Text="Quality" Font-Bold="true"></asp:Label>
                        </td>
                        <td id="TDDDQuality" runat="server" visible="false">
                            <asp:DropDownList ID="DDQuality" runat="server" Width="120px" CssClass="dropdown">
                                
                            </asp:DropDownList>
                           <%-- <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDItem"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>--%>
                        </td>

                        <td class="tdstyle">
                            <asp:Label ID="lblshapeyname" runat="server" Text="Shape" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddshape" runat="server" AutoPostBack="True" Width="80px" CssClass="dropdown"
                                OnSelectedIndexChanged="ddshape_SelectedIndexChanged">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddshape"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label8" runat="server" Text="Size" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDSize" runat="server" Width="120px" CssClass="dropdown" AutoPostBack="true" OnSelectedIndexChanged="DDSize_SelectedIndexChanged">                                
                            </asp:DropDownList>
                           <%-- <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="DDItem"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>--%>
                        </td>
                       
                    </tr>
                    <tr id="Tr4" runat="server" visible="false">
                        <td colspan="3">
                            <asp:Label ID="LblExportFormat" runat="server" Text="Export Format" Font-Bold="True"
                                ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr id="TRExportFt" runat="server" visible="false">
                        <td class="tdstyle">
                            <asp:Label ID="Label1" runat="server" Text=" Ft.Width" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtwidthFt" runat="server" CssClass="textb" OnTextChanged="txtwidthFt_TextChanged"
                                AutoPostBack="True" Width="80px"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label2" runat="server" Text=" Ft.Length" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlengthFt" runat="server" CssClass="textb" OnTextChanged="txtlengthFt_TextChanged"
                                AutoPostBack="True" Width="80px"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label3" runat="server" Text="Ft.Height" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtheightFt" runat="server" CssClass="textb" OnTextChanged="txtheightFt_TextChanged"
                                AutoPostBack="True" Width="80px"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label4" runat="server" Text="Ft.Area" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAreaFt" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label5" runat="server" Text="Ft.Volume" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVolFt" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="TRExportMtr" runat="server" visible="false">
                        <td class="tdstyle">
                            <asp:Label ID="lblmtrwidth" runat="server" Text="Cm. Width" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtwidthMtr" runat="server" CssClass="textb" OnTextChanged="txtwidthMtr_TextChanged"
                                AutoPostBack="True" Width="80px"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblmtrlength" runat="server" Text="Cm. Length" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlengthMtr" runat="server" CssClass="textb" OnTextChanged="txtlengthMtr_TextChanged"
                                AutoPostBack="True" Width="80px"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="lblmtrHeight" runat="server" Text=" Cm. Height" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtheightMtr" runat="server" CssClass="textb" OnTextChanged="txtheightMtr_TextChanged"
                                AutoPostBack="True" Width="80px"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label9" runat="server" Text="  Mtr. Area" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAreaMtr" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label10" runat="server" Text="Mtr. Volume" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVolMtr" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="BtnCalCulate" CssClass="buttonnorm" runat="server" Text="Calculate"
                                OnClick="BtnCalCulate_Click" />
                        </td>
                    </tr>
                    <tr id="TRExportInch" runat="server" visible="false">
                        <td class="tdstyle">
                            <asp:Label ID="Label11" runat="server" Text=" Inch.Width" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtwidthInch" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" OnTextChanged="txtwidthInch_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label12" runat="server" Text="  Inch.Length" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlengthInch" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" OnTextChanged="txtlengthInch_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label13" runat="server" Text="  Inch.Height" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtheightInch" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" OnTextChanged="txtheightInch_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label14" runat="server" Text=" Inch.Area" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAreaInch" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label15" runat="server" Text="Inch.Volume" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVolInch" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="Tr1" runat="server" visible="false">
                        <td colspan="3">
                            <asp:Label ID="LblProductionFormat" runat="server" Text="Production Format" Font-Bold="True"
                                ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr id="Tr2" runat="server" visible="false">
                        <td class="tdstyle">
                            <asp:Label ID="Label16" runat="server" Text="Width Ft" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtWidthProdFt" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" OnTextChanged="TxtWidthProdFt_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label17" runat="server" Text="Length Ft" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtLengthProdFt" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" OnTextChanged="TxtLengthProdFt_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label6" runat="server" Text="Height Ft" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtHeightProdFt" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label18" runat="server" Text=" Area in Sq.Yd." Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtAreaProdSqYD" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="Tr3" runat="server" visible="false">
                        <td class="tdstyle">
                            <asp:Label ID="Label19" runat="server" Text="  Width Cm" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtWidthCm" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" OnTextChanged="TxtWidthCm_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label20" runat="server" Text="Length Cm" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtLengthCm" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" OnTextChanged="TxtLengthCm_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label7" runat="server" Text="Height Cm" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtHeightCM" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label21" runat="server" Text=" Area Sq. Mt." Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtAreaProdSqMtr" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>



                    <tr id="Tr5" runat="server" visible="false">
                        <td colspan="3">
                            <asp:Label ID="Label22" runat="server" Text="Finishing Format" Font-Bold="True"
                                ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr id="Tr6" runat="server" visible="false">
                        <td class="tdstyle">
                            <asp:Label ID="Label23" runat="server" Text="Width Ft" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtWidthFinishingFt" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" OnTextChanged="TxtWidthFinishingFt_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label24" runat="server" Text="Length Ft" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtLengthFinishingFt" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" OnTextChanged="TxtLengthFinishingFt_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label25" runat="server" Text="Height Ft" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtHeightFinishingFt" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label26" runat="server" Text=" Area in Sq.Yd." Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtAreaFinishingSqYD" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="Tr7" runat="server" visible="false">
                        <td class="tdstyle">
                            <asp:Label ID="Label27" runat="server" Text="  Width Cm" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtWidthFinishingCm" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" OnTextChanged="TxtWidthFinishingCm_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label28" runat="server" Text="Length Cm" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtLengthFinishingCm" runat="server" CssClass="textb" AutoPostBack="True"
                                Width="80px" OnTextChanged="TxtLengthFinishingCm_TextChanged"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label29" runat="server" Text="Height Cm" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtHeightFinishingCm" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                        </td>
                        <td class="tdstyle">
                            <asp:Label ID="Label30" runat="server" Text=" Area Sq. Mt." Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtAreaFinishingSqMtr" runat="server" CssClass="textb" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>



                    <tr>
                        <td colspan="10" runat="server" align="right">
                            <asp:Button ID="btnClear0" CssClass="buttonnorm" Text="New" runat="server" Width="56px"
                                OnClientClick="return ClickNew()" />
                            <asp:Button ID="btnSave" CssClass="buttonnorm" runat="server" OnClientClick="return confirm('Do you want to save data?')"
                                OnClick="btnSave_Click" Text="Save" Width="56px" />
                            <asp:Button ID="btnclose0" CssClass="buttonnorm" Text="Close" runat="server" Width="48px"
                                OnClientClick="return CloseForm();" OnClick="btnclose0_Click" />
                            <asp:Button ID="btndelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"
                                CssClass="buttonnorm" Visible="False" OnClick="btndelete_Click" />
                            <asp:Button ID="btnpreview" runat="server" Text="Preview" CssClass="buttonnorm preview_width" OnClick="btnpreview_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblMessage" ForeColor="Red" runat="server" Text=" "></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td id="td1" runat="server" colspan="10">
                            <asp:Label ID="lbl" runat="server" ForeColor="Red"></asp:Label>
                            <div id="Gride" style="overflow: auto; width: 950px; height: 250px">
                                <asp:GridView ID="gdSize" runat="server" OnRowDataBound="gdSize_RowDataBound" OnSelectedIndexChanged="gdSize_SelectedIndexChanged"
                                    DataKeyNames="Sr_No" CssClass="grid-views" AutoGenerateColumns="False">
                                    <HeaderStyle CssClass="gvheaders" />
                                    <AlternatingRowStyle CssClass="gvalts" />
                                    <RowStyle CssClass="gvrow" />
                                   <Columns>
                                   <asp:TemplateField HeaderText="Sr No" Visible="true">
                                       <ItemTemplate>
                                        <asp:Label ID="lblSrNo" runat="server" Text='<%#Bind("Sr_No") %>' Visible="true"></asp:Label>                                        
                                       </ItemTemplate>
                                       <ItemStyle HorizontalAlign="Center" Width="80px"  />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Category Name" Visible="true">
                                       <ItemTemplate>
                                        <asp:Label ID="lblCategoryName" runat="server" Text='<%#Bind("CATEGORY_NAME") %>' Visible="true"></asp:Label>                                        
                                       </ItemTemplate>
                                       <ItemStyle HorizontalAlign="Center" Width="80px"  />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Item Name" Visible="true">
                                       <ItemTemplate>
                                        <asp:Label ID="lblItemName" runat="server" Text='<%#Bind("ITEM_NAME") %>' Visible="true"></asp:Label>                                        
                                       </ItemTemplate>
                                       <ItemStyle HorizontalAlign="Center" Width="80px"  />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Export Size" Visible="true">
                                       <ItemTemplate>
                                        <asp:Label ID="lblExportSizeFt" runat="server" Text='<%#Bind("SizeFt") %>' Visible="true"></asp:Label>                                        
                                       </ItemTemplate>
                                       <ItemStyle HorizontalAlign="Center" Width="80px"  />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Shape" Visible="true">
                                       <ItemTemplate>
                                        <asp:Label ID="lblShapeName" runat="server" Text='<%#Bind("ShapeName") %>' Visible="true"></asp:Label>                                        
                                       </ItemTemplate>
                                       <ItemStyle HorizontalAlign="Center" Width="80px"  />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Prod SizeFt" Visible="true">
                                       <ItemTemplate>
                                        <asp:Label ID="lblItemProdSizeFt" runat="server" Text='<%#Bind("ItemProdSizeFt") %>' Visible="true"></asp:Label>                                        
                                       </ItemTemplate>
                                       <ItemStyle HorizontalAlign="Center" Width="80px"  />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Prod SizeMtr" Visible="true">
                                       <ItemTemplate>
                                        <asp:Label ID="lblItemProdSizemtr" runat="server" Text='<%#Bind("ItemProdSizemtr") %>' Visible="true"></asp:Label>                                        
                                       </ItemTemplate>
                                       <ItemStyle HorizontalAlign="Center" Width="80px"  />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Prod AreaFt" Visible="true">
                                       <ItemTemplate>
                                        <asp:Label ID="ItemProdAreaFt" runat="server" Text='<%#Bind("ItemProdAreaFt") %>' Visible="true"></asp:Label>                                        
                                       </ItemTemplate>
                                       <ItemStyle HorizontalAlign="Center" Width="80px"  />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Prod AreaMtr" Visible="true">
                                       <ItemTemplate>
                                        <asp:Label ID="ItemProdAreaMtr" runat="server" Text='<%#Bind("ItemProdAreaMtr") %>' Visible="true"></asp:Label>                                        
                                       </ItemTemplate>
                                       <ItemStyle HorizontalAlign="Center" Width="80px"  />
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Finishing SizeFt" Visible="false">
                                       <ItemTemplate>
                                        <asp:Label ID="lblItemFinishingSizeFt" runat="server" Text='<%#Bind("ItemFinishingSizeFt") %>' Visible="true"></asp:Label>                                        
                                       </ItemTemplate>
                                       <ItemStyle HorizontalAlign="Center" Width="80px"  />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Finishing SizeMtr" Visible="false">
                                       <ItemTemplate>
                                        <asp:Label ID="lblItemFinishingSizemtr" runat="server" Text='<%#Bind("ItemFinishingSizemtr") %>' Visible="true"></asp:Label>                                        
                                       </ItemTemplate>
                                       <ItemStyle HorizontalAlign="Center" Width="80px"  />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Finishing AreaFt" Visible="false">
                                       <ItemTemplate>
                                        <asp:Label ID="ItemFinishingAreaFt" runat="server" Text='<%#Bind("ItemFinishingAreaFt") %>' Visible="true"></asp:Label>                                        
                                       </ItemTemplate>
                                       <ItemStyle HorizontalAlign="Center" Width="80px"/>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Finishing AreaMtr" Visible="false">
                                       <ItemTemplate>
                                        <asp:Label ID="ItemFinishingAreaMtr" runat="server" Text='<%#Bind("ItemFinishingAreaMtr") %>' Visible="true"></asp:Label>                                        
                                       </ItemTemplate>
                                       <ItemStyle HorizontalAlign="Center" Width="80px"  />
                                    </asp:TemplateField>
                                     
                                     <asp:TemplateField HeaderText="" Visible="false">
                                       <ItemTemplate>
                                        <asp:Label ID="lblSizeId" runat="server" Text='<%#Bind("SizeId") %>' Visible="false"></asp:Label>                                        
                                       </ItemTemplate>
                                       <ItemStyle HorizontalAlign="Center" Width="80px"  />
                                    </asp:TemplateField>   

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hnSizeId" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
