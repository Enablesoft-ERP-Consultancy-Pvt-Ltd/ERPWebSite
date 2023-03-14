<%@ Page Title="Pre Production Meeting" Language="C#" MasterPageFile="~/ERPmaster.master"
    AutoEventWireup="true" CodeFile="frmpreproductionmeeting.aspx.cs" Inherits="Masters_Order_frmpreproductionmeeting" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <script type="text/javascript">
        function CloseForm() {
            window.location.href = "../../main.aspx";
        }
        function ClickNew() {
            window.location.href = "frmpreproductionmeeting.aspx";
        }
       
    </script>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <asp:TabContainer ID="tbppm" runat="server" Width="100%" ActiveTabIndex="0">
                <asp:TabPanel ID="tbmasterdetail" HeaderText="MASTER DETAIL" runat="server">
                    <ContentTemplate>
                        <div style="width: 100%">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkedit" Text="FOR EDIT" CssClass="checkboxbold" AutoPostBack="True"
                                            runat="server" OnCheckedChanged="chkedit_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td>
                                        <asp:Label ID="lblCompName" Text="CompanyName" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:DropDownList ID="DDCompanyName" runat="server" Width="150px" CssClass="dropdown">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblcustomer" Text="Customer Name" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:DropDownList ID="DDCustomer" Width="200px" CssClass="dropdown" runat="server"
                                            OnSelectedIndexChanged="DDCustomer_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td id="Tddocno" runat="server" visible="False">
                                        <asp:Label Text="Doc No." CssClass="labelbold" runat="server" />
                                        <br />
                                        <asp:DropDownList ID="DDDocNo" CssClass="dropdown" Width="150px" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="DDDocNo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblOrderNO" Text="P.O.#" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:DropDownList ID="DDOrderNo" Width="150px" CssClass="dropdown" runat="server"
                                            OnSelectedIndexChanged="DDOrderNo_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <br />
                                        <asp:LinkButton ID="lbladdpono" CssClass="labelbold" Text="Add PO No." runat="server"
                                            Font-Size="Small" ForeColor="Red" OnClick="lbladdpono_Click" />
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <div style="overflow: auto; max-width: 300px">
                                                        <asp:ListBox ID="listpono" runat="server" Style="max-width: 300px" Height="100px"
                                                            SelectionMode="Multiple"></asp:ListBox>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:LinkButton ID="btnDelete" Text="Remove PO No." runat="server" CssClass="labelbold"
                                                        OnClick="btnDelete_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr valign="top">
                                    <td>
                                        <asp:Label ID="Label1" Text="Item Description" runat="server" CssClass="labelbold" />
                                        <br />
                                        <asp:DropDownList ID="DDItemdescription" Width="500px" CssClass="dropdown" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <br />
                                        <asp:LinkButton ID="lnkitemdescription" CssClass="labelbold" Text="Add Item Description"
                                            runat="server" Font-Size="Small" ForeColor="Red" OnClick="lnkitemdescription_Click" />
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <div style="overflow: auto; max-width: 500px">
                                                        <asp:ListBox ID="listitemdesc" runat="server" Style="max-width: 500px" Height="100px"
                                                            SelectionMode="Multiple"></asp:ListBox>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:LinkButton ID="lnkdelitem" Text="Remove ItemDescription" runat="server" CssClass="labelbold"
                                                        OnClick="lnkdelitem_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Label CssClass="labelbold" Text="PPM Meeting Date" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtppmmeetingdt" CssClass="textboxm" Width="100px" runat="server" />
                                        <asp:CalendarExtender ID="cal1" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtppmmeetingdt"
                                            Enabled="True">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
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
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" Text="System gen. Doc No." CssClass="labelbold" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtdocno" CssClass="textboxm" Width="150px" Enabled="False" BackColor="LightGray"
                                            runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tbCONSTRUCTION" runat="server" HeaderText="CONSTRUCTION">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <div style="overflow: auto; max-height: 500px">
                                        <asp:GridView ID="DGConstructionyarndetail" runat="server" ShowFooter="true" AutoGenerateColumns="false">
                                            <HeaderStyle CssClass="gvheaders" />
                                            <AlternatingRowStyle CssClass="gvalts" />
                                            <RowStyle CssClass="gvrow" HorizontalAlign="Center" />
                                            <Columns>
                                                <asp:BoundField DataField="RowNumber" HeaderText="Sr No." />
                                                <asp:TemplateField HeaderText="TYPE OF YARN">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox1" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="COUNT">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox2" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PLY">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox3" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <FooterTemplate>
                                                        <asp:Button ID="ButtonAdd" runat="server" Text="Add New Row" OnClick="ButtonAddconstruction_Click" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <table border="1" style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Label Text="BASE CLOTH" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtbasecloth" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" Text="BACKING CLOTH" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtbackingcloth" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" Text="PILE HEIGHT" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 10%">
                                                <asp:Label ID="Label5" Text="CUT" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 40%">
                                                <asp:TextBox ID="txtpileheightcut" Width="90%" runat="server" />
                                            </td>
                                            <td style="width: 10%">
                                                <asp:Label ID="Label6" Text="LOOP" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 40%">
                                                <asp:TextBox ID="txtpileheightLoop" Width="90%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label7" Text="QUALITY" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 10%">
                                                <asp:Label ID="Label8" Text="RAW WT" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 40%">
                                                <asp:TextBox ID="txtqualityrawwt" Width="90%" runat="server" />
                                            </td>
                                            <td style="width: 10%">
                                                <asp:Label ID="Label9" Text="FINISH WT" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 40%">
                                                <asp:TextBox ID="txtqualityFinishwt" Width="90%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label10" Text="CONSTRUCTION TECHNIQUE" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtconstructiontechnique" Width="100%" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tbTestingrequirement" runat="server" HeaderText="TESTING REQUIREMENT">
                    <ContentTemplate>
                        <table border="1" style="width: 100%">
                            <tr>
                                <td colspan="2" style="width: 100%">
                                    <span class="labelbold"><u>RAW MATERIAL</u></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label11" Text="T.P.I" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txttpi" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label12" Text="COUNT" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txttestreq_count" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="width: 100%">
                                    <span class="labelbold"><u>DYED YARN</u></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label20" Text="COLOUR FOR LIGHT" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtcolourforlight" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label13" Text="CROCKING" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 10%">
                                                <asp:Label ID="Label14" Text="WET" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 40%">
                                                <asp:TextBox ID="txtcrockingwet" Width="90%" runat="server" />
                                            </td>
                                            <td style="width: 10%">
                                                <asp:Label ID="Label15" Text="DRY" CssClass="labelbold" runat="server" />
                                            </td>
                                            <td style="width: 40%">
                                                <asp:TextBox ID="txtcrockingdry" Width="90%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label19" Text="COLOUR MATCHING" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtcolourmatching" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label16" Text="BUYER REQUIREMENTS" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtbuyerrequirements" Width="100%" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tbConstruction_Finishing" runat="server" HeaderText="CONSTRUCTION & FINISHING">
                    <ContentTemplate>
                        <div>
                            <div style="width: 40%; float: left">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbldept" Text="JOB" CssClass="labelbold" runat="server" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDDept" runat="server" Width="200px" CssClass="dropdown">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DDjobvalue" CssClass="dropdown" runat="server">
                                                <asp:ListItem />
                                                <asp:ListItem Text="Yes" />
                                                <asp:ListItem Text="No" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label Text="Comments" CssClass="labelbold" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtjobcomments" Width="250px" TextMode="MultiLine" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnaddjobdetail" CssClass="buttonnorm" Text="Add Job Detail" runat="server"
                                                OnClick="btnaddjobdetail_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="width: 60%; float: right">
                                <div style="max-height: 300px; overflow: auto">
                                    <asp:GridView ID="DGjobdetail" runat="server" AutoGenerateColumns="False" OnRowDeleting="DGvendor_RowDeleting">
                                        <HeaderStyle CssClass="gvheaders" />
                                        <AlternatingRowStyle CssClass="gvalts" />
                                        <RowStyle CssClass="gvrow" />
                                        <EmptyDataRowStyle CssClass="gvemptytext" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Job">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbljob" Text='<%#Bind("Jobname") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblvalue" Text='<%#Bind("jobvalue") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Comments">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblcomments" Text='<%#Bind("comments") %>' runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="250px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ID" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbljobid" Text='<%#Bind("Jobid") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:CommandField ShowDeleteButton="True" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both">
                            <fieldset>
                                <asp:Label Text="TOLERENCE" CssClass="labelbold" ForeColor="Red" runat="server" />
                                </legend>
                                <table>
                                    <tr>
                                        <td>
                                            <div style="overflow: auto; max-height: 500px">
                                                <asp:GridView ID="DGTolerence" runat="server" ShowFooter="True" AutoGenerateColumns="False">
                                                    <HeaderStyle CssClass="gvheaders" />
                                                    <AlternatingRowStyle CssClass="gvalts" />
                                                    <RowStyle CssClass="gvrow" HorizontalAlign="Center" />
                                                    <Columns>
                                                        <asp:BoundField DataField="RowNumber" HeaderText="Sr No." />
                                                        <asp:TemplateField HeaderText="TYPE">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TextBox1" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="VALUE">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TextBox2" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Button ID="ButtonAddTolerence" runat="server" Text="Add New Tolerence" OnClick="ButtonAddTolerence_Click" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tbPacking" runat="server" HeaderText="PACKING">
                    <ContentTemplate>
                        <table border="1" style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Label ID="Label17" Text="FOLDING INSTRUCTION" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtfoldinginstruction" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label38" Text="WARNING LABEL" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtwarninglabel" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label18" Text="WASH CARE/SEW IN LABEL" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtwashcare_sewinlabel" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label40" Text="KALEEN LABEL" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtkaleenlabel" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label21" Text="U'CARDS" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtucards" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label22" Text="INSERTS" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtinserts" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label23" Text="TAG" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txttag" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label24" Text="UPC" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtUpc" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label25" Text="PRICE TICKET" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtpriceticket" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label26" Text="POLY BAG" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtpolybag" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label27" Text="PCS PER PLOY BAG" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtpcsperpolybag" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label28" Text="PIECES PER CARTON/BALE" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtpcspercarton_bale" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label39" Text="CARTON PLY" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtcartonply" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label29" Text="WHITE FABRIC" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtwhitefabric" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label30" Text="MARKING" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtmarkings" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label31" Text="CARTON DIMENSIONS" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtcartondimensions" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label32" Text="MAX CARTON WEIGHT" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtmaxcartonweight" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label33" Text="CARTON/BALE STRRAPING" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtcarton_balstrapping" Width="100%" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tbdefects" runat="server" HeaderText="DEFECTS">
                    <ContentTemplate>
                        <table border="1" style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Label ID="Label34" Text="COMMON" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtcommondefects" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label35" Text="CRITICAL" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtcriticaldefects" Width="100%" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tbPrevetiveaction" runat="server" HeaderText="PREVENTIVE ACTION">
                    <ContentTemplate>
                        <table border="1" style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Label ID="Label36" Text="COMMON" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtcommonpreventive" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label37" Text="CRITICAL" CssClass="labelbold" runat="server" />
                                </td>
                                <td style="width: 80%">
                                    <asp:TextBox ID="txtcriticalpreventive" Width="100%" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
            <table>
                <tr>
                    <td>
                        <asp:Label Text="Remarks" CssClass="labelbold" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtremarks" CssClass="textb" TextMode="MultiLine" Width="500px"
                            runat="server" Height="45px" />
                    </td>
                    <td>
                        <asp:Button ID="btnnew" CssClass="buttonnorm" Text="New" runat="server" OnClientClick="return ClickNew();" />
                        <asp:Button ID="btnsave" CssClass="buttonnorm" Text="Save" runat="server" OnClick="btnsave_Click" />
                        <asp:Button ID="btnpreview" CssClass="buttonnorm" Text="Preview" runat="server" OnClick="btnpreview_Click" />
                        <asp:Button ID="btnclose" CssClass="buttonnorm" Text="Close" runat="server" OnClientClick="return CloseForm();" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblmsg" CssClass="labelbold" ForeColor="Red" Text="" runat="server" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hndocid" Value="0" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnsave" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
