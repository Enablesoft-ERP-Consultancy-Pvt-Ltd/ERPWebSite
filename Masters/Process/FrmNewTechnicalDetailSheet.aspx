<%@ Page Title="New Technical Detail Sheet" Language="C#" AutoEventWireup="true"
    MasterPageFile="~/ERPmaster.master" CodeFile="FrmNewTechnicalDetailSheet.aspx.cs"
    Inherits="Masters_Process_FrmNewTechnicalDetailSheet" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Mainpage" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="../../Scripts/Fixfocus.js"></script>--%>
    <script type="text/javascript">
        function NewForm() {
            window.location.href = "FrmNewTechnicalDetailSheet.aspx";
        }
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function Preveiw() {
            window.open("../../ReportViewer.aspx", "TechnicalDetailSheet");
        }
        function isNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert('numeric only');
                return false;
            }
            else {
                return true;
            }
        }

        function SaveData() {

            var id = document.getElementById('<%=hncomp.ClientID %>').value;

            if (id != "20") {
                var answer = confirm("Do you want to Save?")

                if ((answer)) {
                    return true;
                }
                else {
                    return false;
                }

            }
            return true;
        }

    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                    </td>
                    <%-- <td>
                        <asp:CheckBox ID="ChkEditOrder" runat="server" Text="EDIT DETAIL" AutoPostBack="True"
                            OnCheckedChanged="ChkEditOrder_CheckedChanged" Font-Bold="true" />
                    </td>--%>
                    <%--<td id="TDForOrderWise" runat="server" align="right" colspan="4" class="tdstyle">
                        <asp:CheckBox ID="ChKForOrder" runat="server" Text="Check For OrderWise" Font-Bold="true" OnCheckedChanged="ChKForOrder_CheckedChanged"
                            AutoPostBack="true" />
                    </td>--%>
                </tr>
            </table>
            <table>
                <tr>
                    <%-- <td id="tdindno" runat="server" visible="false">
                    <span class="labelbold">FileNo</span>
                        
                        <br />
                        <asp:DropDownList CssClass="dropdown" ID="DDFileNo" runat="server"  Width="150px" AutoPostBack="True"
                           OnSelectedIndexChanged="DDFileNo_SelectedIndexChanged">
                        </asp:DropDownList>                       
                    </td>--%>
                    <td>
                        <span class="labelbold">VersionNo</span><br />
                        <asp:TextBox ID="txtFileVersion" CssClass="textb" runat="server" Width="90px" Enabled="false"></asp:TextBox>
                    </td>
                    <td id="td1" runat="server">
                        <span class="labelbold">FileNo</span><br />
                        <asp:TextBox ID="txtFileNo" CssClass="textb" runat="server" Width="90px" AutoPostBack="true"
                            OnTextChanged="txtFileNo_TextChanged"></asp:TextBox>
                    </td>
                    <td>
                        <span class="labelbold">Item Name</span><br />
                        <asp:DropDownList CssClass="dropdown" ID="DDItemName" runat="server" Width="120px"
                            AutoPostBack="True" OnSelectedIndexChanged="DDItemName_SelectedIndexChanged">
                        </asp:DropDownList>
                        <%--<cc1:ListSearchExtender ID="ListSearchExtender5" runat="server" TargetControlID="DDItemName"
                            ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                        </cc1:ListSearchExtender>--%>
                    </td>
                    <td>
                        <span class="labelbold">Quality</span>
                        <br />
                        <asp:DropDownList CssClass="dropdown" ID="DDQuality" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span class="labelbold">Design</span><br />
                        <asp:DropDownList CssClass="dropdown" ID="DDDesign" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <span class="labelbold">Color</span>
                        <br />
                        <asp:DropDownList CssClass="dropdown" ID="DDColor" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td id="tdemp" runat="server">
                        <span class="labelbold">Kanghi</span>
                        <br />
                        <asp:TextBox ID="txtKanghi" CssClass="textb" runat="server" Width="90px"></asp:TextBox>
                    </td>
                    <td id="td2" runat="server">
                        <span class="labelbold">Bharti</span>
                        <br />
                        <asp:TextBox ID="txtBharti" CssClass="textb" runat="server" Width="90px"></asp:TextBox>
                    </td>
                    <td id="td3" runat="server">
                        <span class="labelbold">Pick</span><br />
                        <asp:TextBox ID="txtPick" CssClass="textb" runat="server" Width="90px"></asp:TextBox>
                    </td>
                    <td id="td4" runat="server">
                        <span class="labelbold">Tana</span><br />
                        <asp:TextBox ID="txtTana" CssClass="textb" runat="server" Width="90px"></asp:TextBox>
                    </td>
                    <%-- <td id="td5" runat="server">
                    <span class="labelbold">Bana Color</span><br />
                         <asp:TextBox ID="txtBanaColor" CssClass="textb" runat="server" Width="90px"></asp:TextBox>
                       
                    </td>
                    <td id="td6" runat="server">
                    <span class="labelbold">Bana Ply</span><br />
                         <asp:TextBox ID="txtBanaPly" CssClass="textb" runat="server" Width="90px"></asp:TextBox>
                       
                    </td>--%>
                    <td id="td7" runat="server">
                        <span class="labelbold">Khati Ply</span>
                        <br />
                        <asp:TextBox ID="txtKhatiPly" CssClass="textb" runat="server" Width="90px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td id="td8" runat="server">
                        <span class="labelbold">Khati</span><br />
                        <asp:TextBox ID="txtKhati" CssClass="textb" runat="server" Width="90px"></asp:TextBox>
                    </td>
                    <td id="td9" runat="server">
                        <span class="labelbold">Tarika</span>
                        <br />
                        <asp:TextBox ID="txtTarika" CssClass="textb" runat="server" Width="90px"></asp:TextBox>
                    </td>
                    <td id="td10" runat="server">
                        <span class="labelbold">FrenzesInch</span>
                        <br />
                        <asp:TextBox ID="txtFrenzes" CssClass="textb" runat="server" Width="90px"></asp:TextBox>
                    </td>
                    <td>
                        <span class="labelbold">SR No</span>
                        <br />
                        <asp:TextBox ID="txtSRNo" CssClass="textb" runat="server" Width="90px"></asp:TextBox>
                    </td>
                    <td>
                        <span class="labelbold">GSM</span>
                        <br />
                        <asp:TextBox ID="txtGSM" CssClass="textb" runat="server" Width="90px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <div style="width: 500px;">
                <%--<asp:CheckBox ID="ChkWarp_WeftPrint" Text="Check For Print" runat="server" CssClass="checkboxbold" />--%>
                <asp:GridView Width="500px" ID="Gv1" runat="server" ShowFooter="true" AutoGenerateColumns="false">
                    <HeaderStyle CssClass="gvheader" />
                    <AlternatingRowStyle CssClass="gvalt" />
                    <RowStyle CssClass="gvrow" HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="RowNumber" HeaderText="Sr No." />
                        <asp:TemplateField HeaderText="BANA COLOR">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BANA PLY">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Right" />
                            <FooterTemplate>
                                <asp:Button ID="ButtonAdd" runat="server" Text="Add New Row" OnClick="ButtonAdd_Click" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">Remove</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <table>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="2" align="right">
                        <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" UseSubmitBehavior="false"
                            OnClientClick="if (!SaveData()) return; this.disabled=true;this.value = 'wait ...';"
                            ValidationGroup="f1" CssClass="buttonnorm" />
                        <%-- <asp:Button ID="BtnSave" runat="server" Text="Save" OnClientClick="return Validation();"
                            OnClick="BtnSave_Click" ValidationGroup="f1" CssClass="buttonnorm" Width="70px" />--%>
                        <asp:Button ID="BtnPreview" runat="server" Text="Preview" OnClick="BtnPreview_Click"
                            Visible="true" CssClass="buttonnorm preview_width" Width="70px" />
                        &nbsp;<asp:Button ID="BtnNew" runat="server" Text="New" OnClientClick="return NewForm();"
                            CssClass="buttonnorm" Width="70px" />
                        <asp:Button ID="BtnClose" runat="server" Text="Close" OnClientClick="return CloseForm();"
                            OnClick="BtnClose_Click" CssClass="buttonnorm" Width="70px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Lblmessage" runat="server" ForeColor="Red" Text=""></asp:Label>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="f1" />
                        <asp:HiddenField ID="hncomp" runat="server" />
                    </td>
                </tr>
            </table>
            <%--<table>
                <tr>
                    <td colspan="7">
                        <div id="gride" runat="server" style="max-height: 400px; overflow:auto">
                            <asp:GridView ID="DGTechnicalDetailSheet" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                                CellPadding="4" CssClass="grid-view" DataKeyNames="Id" OnPageIndexChanging="DGTechnicalDetailSheet_PageIndexChanging"
                                 OnRowDataBound="DGTechnicalDetailSheet_RowDataBound"
                                OnRowDeleting="DGTechnicalDetailSheet_RowDeleting"
                                OnSelectedIndexChanged="DGTechnicalDetailSheet_SelectedIndexChanged" 
                                PageSize="50" >
                                <HeaderStyle CssClass="gvheaders" />
                                <AlternatingRowStyle CssClass="gvalts" />
                                <RowStyle CssClass="gvrow" />
                                <PagerStyle CssClass="PagerStyle" />
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="Id" Visible="false" />
                                    <asp:TemplateField HeaderText="FileNo" >
                                   <ItemTemplate>
                                        <asp:Label id="lblFileNo" runat="server" text='<%#Bind("FileNo") %>'></asp:Label>                                   
                                   </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" Width="100px" />
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="ItemName" >
                                   <ItemTemplate>
                                        <asp:Label id="lblItemName" runat="server" text='<%#Bind("ITEM_NAME") %>'></asp:Label>                                   
                                   </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" Width="100px" />
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Quality" >
                                   <ItemTemplate>
                                        <asp:Label id="lblQuality" runat="server" text='<%#Bind("QualityName") %>'></asp:Label>                                   
                                   </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" Width="100px" />
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Design" >
                                   <ItemTemplate>
                                        <asp:Label id="lblDesign" runat="server" text='<%#Bind("DesignName") %>'></asp:Label>                                   
                                   </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" Width="100px" />
                                   </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Color" >
                                   <ItemTemplate>
                                        <asp:Label id="lblColor" runat="server" text='<%#Bind("ColorName") %>'></asp:Label>                                   
                                   </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" Width="100px" />
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Kanghi" >
                                   <ItemTemplate>
                                        <asp:Label id="lblKanghi" runat="server" text='<%#Bind("Kanghi") %>'></asp:Label>                                   
                                   </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" Width="100px" />
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Bharti" >
                                   <ItemTemplate>
                                        <asp:Label id="lblBharti" runat="server" text='<%#Bind("Bharti") %>'></asp:Label>                                   
                                   </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" Width="100px" />
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Pick" >
                                   <ItemTemplate>
                                        <asp:Label id="lblPick" runat="server" text='<%#Bind("Pick") %>'></asp:Label>                                   
                                   </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" Width="100px" />
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Tana" >
                                   <ItemTemplate>
                                        <asp:Label id="lblTana" runat="server" text='<%#Bind("Tana") %>'></asp:Label>                                   
                                   </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" Width="100px" />
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Bana Color" >
                                   <ItemTemplate>
                                        <asp:Label id="lblBanaColor" runat="server" text='<%#Bind("BanaColor") %>'></asp:Label>                                   
                                   </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" Width="100px" />
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Bana Ply" >
                                   <ItemTemplate>
                                        <asp:Label id="lblBanaPly" runat="server" text='<%#Bind("BanaPly") %>'></asp:Label>                                   
                                   </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" Width="100px" />
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Khati Ply" >
                                   <ItemTemplate>
                                        <asp:Label id="lblKhatiPly" runat="server" text='<%#Bind("KhatiPly") %>'></asp:Label>                                   
                                   </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" Width="100px" />
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Khati" >
                                   <ItemTemplate>
                                        <asp:Label id="lblKhati" runat="server" text='<%#Bind("Khati") %>'></asp:Label>                                   
                                   </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" Width="100px" />
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Tarika" >
                                   <ItemTemplate>
                                        <asp:Label id="lblTarika" runat="server" text='<%#Bind("Tarika") %>'></asp:Label>                                   
                                   </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" Width="100px" />
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Frenzes" >
                                   <ItemTemplate>
                                        <asp:Label id="lblFrenzes" runat="server" text='<%#Bind("Frenzes") %>'></asp:Label>                                   
                                   </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" Width="100px" />
                                   </asp:TemplateField>                                   
                                   <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="DEL" OnClientClick="return confirm('Do you want to Delete data?')"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                    
                                </Columns>
                                <EmptyDataRowStyle CssClass="gvemptytext" />
                            </asp:GridView>
                        </div>
                    </td>
                    <asp:HiddenField ID="hncomp" runat="server" />
                </tr>
            </table>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
