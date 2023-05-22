<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmasterPopUp.master" AutoEventWireup="true"
    CodeFile="DefineItemCodeOther.aspx.cs" Inherits="Masters_Carpet_DefineItemCodeOther" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../../Scripts/JScript.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>

    <link href="../../Content/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Content/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <script src="../../Content/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>


    <link href="../../App_Themes/Default/Style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">
        function CloseForm() {
            self.close();
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="2" cellspacing="2" width="90%">
                <tr class="RowStyle">
                    <td>HS CODE:
                    </td>
                    <td>
                        <asp:TextBox ID="TxtHsCode" runat="server" CssClass="textb"></asp:TextBox>
                    </td>
                    <td>COMPOSITION:
                    </td>
                    <td>
                        <asp:TextBox ID="TxtDesc" runat="server" Width="220px" CssClass="textb"></asp:TextBox>
                    </td>
                    <td>MINIMUM ORDER QTY:
                    </td>
                    <td>
                        <asp:TextBox ID="TxtMinOrderQty" runat="server" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                    </td>
                </tr>
                <tr class="RowStyle">
                    <td>PRICE/MTR2:
                    </td>
                    <td>
                        <asp:TextBox ID="TxtPrice" runat="server" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                    </td>
                    <td>INNER PACKING:
                    </td>
                    <td>
                        <asp:TextBox ID="TxtInnerPkg" runat="server" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                    </td>
                    <td>MASTER PACKING:
                    </td>
                    <td>
                        <asp:TextBox ID="TxtMasterPkg" runat="server" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                    </td>
                </tr>
                <tr class="RowStyle">
                    <td>GROSS WEIGHT(KGS/PCS):
                    </td>
                    <td>
                        <asp:TextBox ID="TxtIronWt" runat="server" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                    </td>
                    <td>WOOD/OTHER WEIGHT(KGMS):
                    </td>
                    <td>
                        <asp:TextBox ID="TxtWoodWt" runat="server" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                    </td>
                    <td>NET WEIGHT(KGS/PCS):
                    </td>
                    <td>
                        <asp:TextBox ID="TxtNetWt" runat="server" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                    </td>
                </tr>
                <tr class="RowStyle">
                    <td>MASTER PACKING SIZE:
                    </td>
                    <td>
                        <asp:TextBox ID="TxtMasterPkgSize" runat="server" Width="30px" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                        <asp:TextBox ID="TxtMasterPkgSize1" runat="server" Width="30px" CssClass="textb"
                            onkeypress="return isNumber(event);"></asp:TextBox>
                        <asp:TextBox ID="TxtMasterPkgSize2" runat="server" Width="30px" CssClass="textb"
                            onkeypress="return isNumber(event);"></asp:TextBox>
                    </td>
                    <td>VOLUME (CBM):
                    </td>
                    <td>
                        <asp:TextBox ID="TxtVolume" runat="server" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                    </td>
                    <td>DRAWBACK RATE:
                    </td>
                    <td>
                        <asp:TextBox ID="TxtDrawBackRate" runat="server" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="2" cellspacing="2" width="90%">
                <tr class="RowStyle">
                    <td style="width: 380PX">LOADABILITY:20' &nbsp;
                        <asp:TextBox ID="TxtLoad20" runat="server" Width="50px" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                        &nbsp; 40' &nbsp;
                        <asp:TextBox ID="TxtLoad40" runat="server" Width="50px" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                        &nbsp; 40'HQ
                        <asp:TextBox ID="Txtload40HQ" runat="server" Width="50px" CssClass="textb" onkeypress="return isNumber(event);"></asp:TextBox>
                    </td>
                </tr>
                <tr class="RowStyle">
                    <td>MATERIAL REMARKS
                        <br />
                        <asp:TextBox ID="txtMaterialremarks" runat="server" Width="350px" CssClass="textb"
                            TextMode="MultiLine" TabIndex="26"></asp:TextBox>
                    </td>
                    <td>FINISH REMARKS
                        <br />
                        <asp:TextBox ID="TxtFinishRemarks" runat="server" Width="280px" CssClass="textb"
                            TextMode="MultiLine" TabIndex="26"></asp:TextBox>
                    </td>
                    <td>PACKING REMARKS
                        <br />
                        <asp:TextBox ID="TxtPkgRemarks" runat="server" Width="280px" CssClass="textb" TextMode="MultiLine"
                            TabIndex="26"></asp:TextBox>
                    </td>
                </tr>
                <tr class="RowStyle">
                    <td>
                        <asp:Label ID="LblMsg" runat="server" ForeColor="Red" CssClass="labelbold"></asp:Label>
                    </td>
                    <td></td>
                    <td>
                        <asp:Button ID="BTNSave" runat="server" Text="Save" CssClass="buttonnorm" OnClick="BTNSave_Click" />
                        <asp:Button ID="BtnCancel" runat="server" Text="Close" CssClass="buttonnorm" OnClientClick="return CloseForm();" />
                    </td>
                </tr>
            </table>









            <div class="page-content">
                <div class="row">
                    <div class="col-lg-12">
                        <div class="panel panel-violet">
                            <div class="panel-heading">Order services</div>
                            <div class="panel-body pan">
                                <form action="#">
                                    <div class="form-body pal">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <div class="input-icon right"><i class="fa fa-user"></i><input id="inputName" type="text" placeholder="Name" class="form-control"></div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <div class="input-icon right"><i class="fa fa-briefcase"></i><input id="inputCompany" type="text" placeholder="Company" class="form-control"></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <div class="input-icon right"><i class="fa fa-envelope"></i><input id="inputEmail" type="text" placeholder="E-mail" class="form-control"></div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <div class="input-icon right"><i class="fa fa-phone"></i><input id="inputPhone" type="text" placeholder="Phone" class="form-control"></div>
                                                </div>
                                            </div>
                                        </div>
                                        <hr>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group"><select class="form-control">
                                                    <option>Interested in</option>
                                                    <option value="design">Design</option>
                                                    <option value="development">Development</option>
                                                    <option value="illustration">Illustration</option>
                                                    <option value="brading">Branding</option>
                                                    <option value="video">Video</option>
                                                </select></div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group"><select class="form-control">
                                                    <option>Budget</option>
                                                    <option value="0">Less than 5000$</option>
                                                    <option value="1">5000$ - 10000$</option>
                                                    <option value="2">10000$ - 20000$</option>
                                                    <option value="3">More than 20000$</option>
                                                </select></div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <div class="input-icon right"><i class="fa fa-calendar"></i><input id="inputStartDate" type="text" placeholder="Expected start date" class="form-control">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <div class="input-icon right"><i class="fa fa-calendar"></i><input id="inputFinishDate" type="text" placeholder="Expected finish date" class="form-control">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group"><input id="inputIncludeFile" type="file" placeholder="Inlcude some file"></div>
                                        <div class="form-group mbn"><textarea rows="5" placeholder="Tell us about your project" class="form-control"></textarea></div>
                                    </div>
                                    <div class="form-actions text-right pal">
                                        <button type="submit" class="btn btn-primary">Send request</button>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
