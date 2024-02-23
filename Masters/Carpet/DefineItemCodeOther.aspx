<%@ Page Title="" Language="C#" MasterPageFile="~/ERPmasterPopUp.master" AutoEventWireup="true"
    CodeFile="DefineItemCodeOther.aspx.cs" Inherits="Masters_Carpet_DefineItemCodeOther" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="<%= ResolveUrl("~/") %>lib/jquery/dist/jquery.min.js"></script>
    <script src="<%= ResolveUrl("~/") %>lib/jquery-validation/dist/jquery.validate.min.js"></script>

    <script src="<%= ResolveUrl("~/") %>lib/bootstrap/dist/js/bootstrap.min.js"></script>
    <link href="<%= ResolveUrl("~/") %>lib/font-awesome/css/font-awesome.min.css"
        rel="stylesheet" />
    <link href="<%= ResolveUrl("~/") %>lib/bootstrap/dist/css/bootstrap.min.css"
        rel="stylesheet" />
    <script src="<%= ResolveUrl("~/") %>Content/JavaScript/Product.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Form" runat="Server">

    <div class="page-content">

        <div class="card">
            <div class="card-header ptx pbn pls normal-font-txt">
                <strong>Additional Parameter</strong>
            </div>
            <div class="card-body">

                <div class="row">
                    <div class="col-xl-6">
                        <asp:UpdatePanel ID="up1" runat="server">
                            <ContentTemplate>
                                <div class="card">
                                    <div class="card-header ptx pbn pls normal-font-txt"></div>
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-xl-12">

                                                <div class="row">
                                                    <div class="col-xl-4">
                                                        <div class="mb-3">
                                                            HS CODE:
                                   <asp:TextBox ID="TxtHsCode" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="col-xl-4">
                                                        <div class="mb-3">
                                                            COMPOSITION/Detail:
                                 <asp:TextBox ID="TxtDesc" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-xl-4">
                                                        <div class="mb-3">
                                                            MINIMUM ORDER QTY:
                                 <asp:TextBox ID="TxtMinOrderQty" runat="server" CssClass="form-control" onkeypress="return isNumber(event);"></asp:TextBox>
                                                        </div>
                                                    </div>


                                                </div>
                                                <div class="row">
                                                    <div class="col-xl-4">
                                                        <div class="mb-3">
                                                            PRICE/MTR2:
              
         
                           <asp:TextBox ID="TxtPrice" runat="server" CssClass="form-control" onkeypress="return isNumber(event);"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-xl-4">
                                                        <div class="mb-3">
                                                            INNER PACKING:
               
                           <asp:TextBox ID="TxtInnerPkg" runat="server" CssClass="form-control" onkeypress="return isNumber(event);"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-xl-4">
                                                        <div class="mb-3">
                                                            MASTER PACKING:
               
                             <asp:TextBox ID="TxtMasterPkg" runat="server" CssClass="form-control" onkeypress="return isNumber(event);"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-xl-4">
                                                        <div class="mb-3">
                                                            GROSS WEIGHT(KGS/PCS):
              
                           <asp:TextBox ID="TxtIronWt" runat="server" CssClass="form-control" onkeypress="return isNumber(event);"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-xl-4">
                                                        <div class="mb-3">
                                                            WOOD/OTHER WEIGHT(KGMS):
        
                             <asp:TextBox ID="TxtWoodWt" runat="server" CssClass="form-control" onkeypress="return isNumber(event);"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-xl-4">
                                                        <div class="mb-3">
                                                            NET WEIGHT(KGS/PCS):
          
                             <asp:TextBox ID="TxtNetWt" runat="server" CssClass="form-control" onkeypress="return isNumber(event);"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-xl-4">
                                                        <div class="mb-3">
                                                            MASTER PACKING SIZE:
                                             <div class="row">
                                                 <div class="col-xl-4">
                                                     <asp:TextBox ID="TxtMasterPkgSize" runat="server" CssClass="form-control"
                                                         onkeypress="return isNumber(event);"></asp:TextBox>
                                                 </div>
                                                 <div class="col-xl-4">
                                                     <asp:TextBox ID="TxtMasterPkgSize1" runat="server" CssClass="form-control"
                                                         onkeypress="return isNumber(event);"></asp:TextBox>
                                                 </div>
                                                 <div class="col-xl-4">
                                                     <asp:TextBox ID="TxtMasterPkgSize2" runat="server" CssClass="form-control"
                                                         onkeypress="return isNumber(event);"></asp:TextBox>
                                                 </div>
                                             </div>




                                                        </div>
                                                    </div>
                                                    <div class="col-xl-4">
                                                        <div class="mb-3">
                                                            VOLUME (CBM):
              
                             <asp:TextBox ID="TxtVolume" runat="server" CssClass="form-control" onkeypress="return isNumber(event);"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-xl-4">
                                                        <div class="mb-3">
                                                            DRAWBACK RATE:
             
                             <asp:TextBox ID="TxtDrawBackRate" runat="server" CssClass="form-control" onkeypress="return isNumber(event);"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-xl-12">

                                                        <div class="col-xl-4">
                                                            <div class="mb-3">
                                                                LOADABILITY:20
                                                <asp:TextBox ID="TxtLoad20" runat="server" CssClass="form-control"
                                                    onkeypress="return isNumber(event);"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-xl-4">


                                                            <div class="mb-3">
                                                                40' 
                        <asp:TextBox ID="TxtLoad40" runat="server" CssClass="form-control" onkeypress="return isNumber(event);"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-xl-4">
                                                            <div class="mb-3">
                                                                40'HQ
                        <asp:TextBox ID="Txtload40HQ" runat="server" CssClass="form-control" onkeypress="return isNumber(event);"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-xl-4">
                                                        <div class="mb-3">
                                                            MATERIAL REMARKS
             
                                <asp:TextBox ID="txtMaterialremarks" runat="server" CssClass="form-control"
                                    TextMode="MultiLine" TabIndex="26"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-xl-4">
                                                        <div class="mb-3">
                                                            FINISH REMARKS
                
                             <asp:TextBox ID="TxtFinishRemarks" runat="server" CssClass="form-control"
                                 TextMode="MultiLine" TabIndex="26"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-xl-4">
                                                        <div class="mb-3">
                                                            PACKING REMARKS
                 
                             <asp:TextBox ID="TxtPkgRemarks" runat="server" CssClass="form-control"
                                 TextMode="MultiLine"
                                 TabIndex="26"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-xl-6">
                                                        <div class="mb-3">
                                                            <asp:Label ID="LblMsg" runat="server" ForeColor="Red" CssClass="labelbold"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="col-xl-6">
                                                        <div class="mb-3">
                                                            <asp:Button ID="BTNSave" runat="server" Text="Save" CssClass="btn btn-primary"
                                                                OnClick="BTNSave_Click" />
                                                            <asp:Button ID="BtnCancel" runat="server" Text="Close" CssClass="btn btn-primary"
                                                                OnClientClick="return CloseForm();" />
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>












                                        </div>


                                    </div>
                                </div>


                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="col-xl-6">


                        <div class="card bg-default border-none mbn">
                            <div class="card-header ptx pbn pls normal-font-txt">
                                <strong>Costing </strong>
                            </div>
                            <div class="card-body pdn">
                                <div class="row">



                                         <asp:HiddenField ID="hdnCostingId" runat="server"  />

                                    <div class="col-lg-3">
                                        <div class="mb-3">
                                            Price:
                                   <asp:TextBox ID="txtItemPrice" runat="server" CssClass="form-control"></asp:TextBox>

                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator2" ValidationGroup="Costing" ControlToValidate="txtItemPrice"
                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>


                                    <div class="col-lg-3">
                                        <div class="mb-3">
                                            Discount(in %):
                                   <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-lg-3">
                                        <div class="mb-3">
                                            New Arrival:
                                                            <asp:CheckBox ID="chkArrival" runat="server" />


                                        </div>
                                    </div>

                                    <div class="col-lg-3">
                                        <div class="mb-3">
                                            On Call:
                                                                 <asp:CheckBox ID="chkCall" runat="server" />



                                        </div>
                                    </div>










                                </div>


                                <div class="col-xl-6">
                                    <div class="mb-3">
                                        <asp:Button OnClick="btnCosting_Click" ValidationGroup="Costing" ID="btnCosting"
                                            runat="server" Text="Save "
                                            CssClass="btn btn-primary" />
                                    </div>
                                </div>

                            </div>
                        </div>

















                        <div class="card bg-default border-none mbn">
                            <div class="card-header ptx pbn pls normal-font-txt">
                                <strong>Sales App Parameter</strong>
                            </div>
                            <div class="card-body pdn">
                                <div class="row">
                                    <div class="col-xl-12">
                                        <div class="table-responsive">
                                            <table id="tblProperty" class="table table-bordered mbn small-inputbox">
                                                <thead class="bg-table-header">
                                                    <tr>
                                                        <th width="40%">
                                                            <select id="ddlAttributeId" class="form-control">
                                                            </select></th>
                                                        <th width="50%">
                                                            <textarea rows="2" id="txtAttributeValue" class="form-control"></textarea>
                                                        </th>
                                                        <th width="10%">
                                                            <a title="Add" href="javascript:;" class="btn btn-primary mrx btnAdd">
                                                                <i class="fa fa-plus text-green mediumtxt"></i>
                                                            </a>
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                </tbody>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="card bg-default border-none mbn">
                            <div class="card-header ptx pbn pls normal-font-txt">
                                <strong>Uplaod Image</strong>
                            </div>
                            <div class="card-body pdn">
                                <div class="row">









                                    <div class="col-xl-12">
                                        <div class="table-responsive">
                                            <table id="tblPhoto" class="table table-bordered mbn small-inputbox">
                                                <thead class="bg-table-header">
                                                    <tr>

                                                        <th width="60%">
                                                            <asp:FileUpload ID="PhotoImage" AllowMultiple="true" runat="server" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="PhotoImage"
                                                                ValidationGroup="upload" runat="server" ErrorMessage="Choose Image"></asp:RequiredFieldValidator>

                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationGroup="upload"
                                                                runat="server" ErrorMessage="Only .png,.jpeg or.jpg files are allowed!"
                                                                ValidationExpression="^.*\.(jpg|JPG|png|GIF|jpeg|JPEG|BMP|bmp)$" ControlToValidate="PhotoImage"></asp:RegularExpressionValidator>
                                                        </th>
                                                        <th width="10%">
                                                            <asp:RadioButtonList ID="rdPrime" runat="server">
                                                                <asp:ListItem Value="true">Prime</asp:ListItem>
                                                                <asp:ListItem Value="false">Non Prime</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </th>
                                                        <th width="30%">
                                                            <asp:Button OnClick="btnUpload_Click" ID="btnUpload" runat="server" Text="Save Image"
                                                                ValidationGroup="upload" CssClass="btn btn-primary" />
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>

                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Repeater ID="rptPhotoList" runat="server">
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td>

                                                                            <asp:Image Height="60px" Width="60px" ID="ImgPhoto" runat="server" ImageUrl='<%# this.GetImage(Eval("PhotoName").ToString())  %>' />
                                                                            <asp:HiddenField ID="lblPhotoId" runat="server" Value='<%# Eval("PhotoId") %>' />
                                                                            <asp:HiddenField ID="hdnPhoto" runat="server" Value='<%# Eval("PhotoName") %>' />
                                                                        </td>
                                                                        <td>
                                                                            <%# ((bool)Eval("IsPrime") == true) ? "Prime":" Non Prime" %>
                                                                          

                                                                        </td>

                                                                        <td>
                                                                            <asp:LinkButton ID="lnkDelete" CssClass="btn btn-primary" Text="Delete" runat="server"
                                                                                OnClientClick="return confirm('Do you want to delete this Photo?');"
                                                                                OnClick="DeletePhoto" />
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>

                                                        </ContentTemplate>

                                                    </asp:UpdatePanel>

                                                </tbody>

                                            </table>
                                        </div>
                                    </div>











                                </div>
                            </div>
                        </div>

                    </div>
                </div>

            </div>
        </div>































    </div>


    <textarea style="display: none" id="propertyTemplate">
        <tr>
                                                <td class='text-center'>
                                                    <input class="AttributeId" type="hidden" id="hdnPropertyList_{0}_AttributeId" name="PropertyList[{0}].AttributeId"
                                                        value="{1}">

                                                    <label>{2}</label>
                                                </td>
                                                <td>
                                                    <input class="AttributeValue" type="hidden" id="hdnPropertyList_{0}_AttributeValue" name="PropertyList[{0}].AttributeValue"
                                                        value="{3}">

                                                    <label>{3}</label>
                                                    
                                                    
                                                 
                                                </td>

                                                <td class="text-left">

                                                    <a title="Delete" href="javascript:;" class="btn btn-primary  mrx btnDel">
                                                        <i class="fa fa-times text-red mediumtxt"></i>
                                                    </a>


                                                </td>
                                            </tr>
        </textarea>
</asp:Content>