<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/ERPmaster.master"
    CodeFile="frmSize.aspx.cs" Inherits="Masters_Campany_frmSize" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Form" runat="server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx"
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
                            Unit Name
                        </td>
                        <td>
                            <asp:DropDownList ID="ddunit" runat="server" OnSelectedIndexChanged="ddunit_SelectedIndexChanged"
                                AutoPostBack="True" Width="80px" CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddunit"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                        <td class="tdstyle">
                            Shape
                        </td>
                        <td>
                            <asp:DropDownList ID="ddshape" runat="server" AutoPostBack="True" Width="80px" OnSelectedIndexChanged="ddshape_SelectedIndexChanged"
                                CssClass="dropdown">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddshape"
                                ViewStateMode="Disabled" PromptCssClass="labelbold" PromptPosition="Bottom">
                            </cc1:ListSearchExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            Ft.Width
                        </td>
                        <td class="tdstyle">
                            Ft.Length
                        </td>
                        <td class="tdstyle">
                            Ft.Height
                        </td>
                        <td class="tdstyle">
                            Ft.Area
                        </td>
                        <td class="tdstyle">
                            Ft.Volume
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtwidthFt" runat="server" CssClass="textb" OnTextChanged="txtwidthFt_TextChanged"
                                AutoPostBack="True" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlengthFt" runat="server" CssClass="textb" OnTextChanged="txtlengthFt_TextChanged"
                                AutoPostBack="True" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtheightFt" runat="server" CssClass="textb" OnTextChanged="txtheightFt_TextChanged"
                                AutoPostBack="True" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAreaFt" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVolFt" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="BtnCalCulate" runat="server" Text="Calculate" OnClick="BtnCalCulate_Click"
                                CssClass="buttonnorm" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdstyle">
                            Mtr.Width
                        </td>
                        <td class="tdstyle">
                            Mtr.Length
                        </td>
                        <td class="tdstyle">
                            Mtr.Height
                        </td>
                        <td class="tdstyle">
                            Mtr.Area
                        </td>
                        <td class="tdstyle">
                            Mtr.Volume
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtwidthMtr" runat="server" CssClass="textb" OnTextChanged="txtwidthMtr_TextChanged"
                                AutoPostBack="True" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtlengthMtr" runat="server" CssClass="textb" OnTextChanged="txtlengthMtr_TextChanged"
                                AutoPostBack="True" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtheightMtr" runat="server" CssClass="textb" OnTextChanged="txtheightMtr_TextChanged"
                                AutoPostBack="True" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAreaMtr" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVolMtr" runat="server" CssClass="textb" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align: right">
                            <asp:Button ID="btnClear0" Text="Clear" runat="server" Width="56px" OnClick="btnClear_Click"
                                CssClass="buttonnorm" />
                            <asp:Button ID="btnSave" runat="server" OnClientClick="return confirm('Do you want to save data?')"
                                OnClick="btnSave_Click" Text="Save" Width="48px" CssClass="buttonnorm" />
                            <asp:Button ID="btnclose0" Text="Close" runat="server" Width="48px" OnClientClick="return CloseForm();"
                                CssClass="buttonnorm" />
                            <asp:Button ID="btndelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you want to Delete data?')"
                                CssClass="buttonnorm" Visible="False" OnClick="btndelete_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td id="td1" runat="server" colspan="9">
                            <asp:Label ID="lbl" runat="server" ForeColor="Red"></asp:Label>
                            <div id="Gride" style="overflow: scroll; width: 400; height: 273px">
                                <asp:GridView ID="gdSize" runat="server" OnRowDataBound="gdSize_RowDataBound" OnSelectedIndexChanged="gdSize_SelectedIndexChanged"
                                    DataKeyNames="SizeId" CssClass="grid-view" OnRowCreated="gdSize_RowCreated">
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="font-family: Times New Roman; font-size: 18px">
                            <asp:Label ID="lblMessage" ForeColor="Red" runat="server" Text=" "></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
