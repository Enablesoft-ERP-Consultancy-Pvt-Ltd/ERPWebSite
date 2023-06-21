<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="Test" %>

<%@ Register Src="~/UserControls/ucmenu.ascx" TagName="ucmenu" TagPrefix="uc2" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- The above 3 meta tags *must* come first in the head; any other head content
must come *after* these tags -->
    <title>Bootstrap 101 Template</title>
    <link href="Content/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/font-awesome/css/font-awesome.min.css" rel="stylesheet" />

    <!-- Bootstrap -->
    <style>
        .menuss {
            font-weight: bold;
            width: 100%;
            margin: 0px 0px 0px 0px;
            padding: 6px 6px 4px 6px;
            height: 40px;
            line-height: 100%;
            -webkit-border-radius: 24px;
            -webkit-box-shadow: 2px 2px 3px #666666;
            background: -webkit-gradient(linear, left top, left bottom, from(#FCFAFA), to(#7A7A7A));
            border: solid 1px #6D6D6D;
            position: relative;
            z-index: 1000;
            box-sizing: border-box;
        }
    </style>


</head>
<body>

    <form id="form1" runat="server">
        <!-- Fixed navbar -->
        <nav class="navbar navbar-inverse navbar-fixed-top">
            <div class="container">
                <div class="navbar-header">

                    <a class="navbar-brand" href="#">
                        <asp:Image ID="Image1" ImageUrl="~/Images/header.jpg" runat="server" /></a>

                    <asp:Image ID="imgLogo" align="left" runat="server" ImageUrl="~/Images/Logo/client-logo.png"
                        Height="66px" Width="111px" />
                    <asp:Label ID="LblUserName" ForeColor="White" runat="server"
                        Text="Rakesh"></asp:Label>


                    <asp:Label ID="LblCompanyName" runat="server" Text="Vikram"></asp:Label>
                </div>
                <div id="navbar" class="navbar-collapse collapse">


                    <uc2:ucmenu ID="ucmenu1" runat="server" />



                    <%-- <ul class="nav navbar-nav">
                    <li class="active"><a href="#">Home</a></li>
                    <li><a href="#about">About</a></li>
                    <li><a href="#contact">Contact</a></li>
                    <li class="dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown"
role="button" aria-haspopup="true"
                            aria-expanded="false">Dropdown <span class="caret"></span></a>
<ul class="dropdown-menu">
                            <li><a href="#">Action</a></li>
                            <li><a href="#">Another action</a></li>
                            <li><a href="#">Something else here</a></li>
                            <li role="separator" class="divider"></li>
                            <li class="dropdown-header">Nav header</li>
                            <li><a href="#">Separated link</a></li>
                            <li><a href="#">One more separated link</a></li>
                        </ul>
                    </li>
                </ul>--%>
                </div>
                <!--/.nav-collapse -->
            </div>
        </nav>




        <div class="container theme-showcase" role="main">



            <div class="page-header">


                <div class="page-header pull-left">
                    <div class="page-title">Rolling Stock List</div>
                </div>
            </div>


            <div class="page-content">
                <div class="panel">
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-lg-4 col-xs-12">
                                <h1 class="largetxt text-red mtn"><strong>281 Records Found!!!</strong></h1>
                            </div>
                            <div class="col-lg-4">
                                <i class="fa fa-file-excel-o largetxt text-green mrl"></i><i class="fa fa-file largetxt mrl text-green">
                                </i>
                                <i class="fa fa-download largetxt"></i>&nbsp;&nbsp;
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-8">
                                <div class="panel  light-green-color pdm">
                                    <div class="row">
                                        <div class="col-lg-15">
                                            <div class="form-group">
                                                <label>Manufacturer</label>
                                                <input type="text" class="form-control" />
                                            </div>
                                        </div>
                                        <div class="col-lg-15">
                                            <div class="form-group">
                                                <label>Descripton</label>
                                                <input type="text" class="form-control" />
                                            </div>
                                        </div>
                                        <div class="col-lg-15">
                                            <div class="form-group">
                                                <label>Max Speed</label>
                                                <input type="text" class="form-control" />
                                            </div>
                                        </div>
                                        <div class="col-lg-15">
                                            <div class="form-group">
                                                <label>Voltage</label>
                                                <input type="text" class="form-control" />
                                            </div>
                                        </div>
                                        <div class="col-lg-15">
                                            <div class="form-group">
                                                <label>Power Nominal</label>
                                                <input type="text" class="form-control" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12">
                                            <button type="button" class="btn btn-md btn-orange mrl">Search</button>
                                            <button type="button" class="btn btn-md btn-orange mrl">Clear Search</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="row mtm">
                            <div class="col-lg-4">
                                <div class="input-group image-preview">
                                    <input type="text" class="form-control image-preview-filename" disabled="disabled">
                                    <span class="input-group-btn">
                                        <!-- image-preview-clear button -->
                                        <button type="button" class="btn btn-default btn-sm image-preview-clear" style="display: none;
                                            padding-top: 5px; padding-bottom: 5px;">
                                            <span class="glyphicon glyphicon-remove"></span>Clear
                                        </button>
                                        <!-- image-preview-input -->
                                        <div class="btn btn-default btn-sm image-preview-input" style="padding-top: 5px;
                                            padding-bottom: 5px;">
                                            <span class="glyphicon glyphicon-folder-open"></span>
                                            <span class="image-preview-input-title">Browse</span>
                                            <input type="file" accept="image/png, image/jpeg, image/gif" name="input-file-preview" />
                                            <!-- rename it -->
                                        </div>
                                    </span>
                                </div>
                            </div>
                            <div class="col-lg-4">
                                <button type="button" class="btn btn-md btn-green">Upload</button>
                            </div>
                        </div>
                    </div>
                </div>


                <div class="panel">
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="table-responsive">
                                    <div oncontextmenu="event.preventDefault();$('#context-menu').show();$('#context-menu').offset({'top':mouseY,'left':mouseX})">
                                        <table class="table table-hover table-bordered table-striped">
                                            <thead>
                                                <tr class="bg-table-header">
                                                    <th width="10%">Train ID</th>
                                                    <th width="20%" style="min-width: 300px">Description</th>
                                                    <th width="10%">Manufacturer</th>
                                                    <th width="10%">Composition</th>
                                                    <th width="10%">Max Speed</th>
                                                    <th width="10%">Voltage</th>
                                                    <th width="15%">Power Nominal</th>
                                                    <th width="15%">Operation</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td>1</td>
                                                    <td>Henry</td>
                                                    <td>23</td>
                                                    <td>23</td>
                                                    <td>23</td>
                                                    <td>23</td>
                                                    <td>23</td>
                                                    <td><i class="fa fa-edit text-green mrm mediumtxt"></i><i class="fa fa-trash-o text-red mrm mediumtxt">
                                                    </i>
                                                        <button type="button" class="btn btn-md btn-green pt-2 pb-2" data-target="#clone"
                                                            data-toggle="modal">
                                                            Clone</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>1</td>
                                                    <td>Henry</td>
                                                    <td>23</td>
                                                    <td>23</td>
                                                    <td>23</td>
                                                    <td>23</td>
                                                    <td>23</td>
                                                    <td><i class="fa fa-edit text-green mrm mediumtxt"></i><i class="fa fa-trash-o text-red mrm mediumtxt">
                                                    </i>
                                                        <button type="button" class="btn btn-md btn-green pt-2 pb-2" data-target="#clone"
                                                            data-toggle="modal">
                                                            Clone</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>1</td>
                                                    <td>Henry</td>
                                                    <td>23</td>
                                                    <td>23</td>
                                                    <td>23</td>
                                                    <td>23</td>
                                                    <td>23</td>
                                                    <td><i class="fa fa-edit text-green mrm mediumtxt"></i><i class="fa fa-trash-o text-red mrm mediumtxt">
                                                    </i>
                                                        <button type="button" class="btn btn-md btn-green pt-2 pb-2" data-target="#clone"
                                                            data-toggle="modal">
                                                            Clone</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>1</td>
                                                    <td>Henry</td>
                                                    <td>23</td>
                                                    <td>23</td>
                                                    <td>23</td>
                                                    <td>23</td>
                                                    <td>23</td>
                                                    <td><i class="fa fa-edit text-green mrm mediumtxt"></i><i class="fa fa-trash-o text-red mrm mediumtxt">
                                                    </i>
                                                        <button type="button" class="btn btn-md btn-green pt-2 pb-2" data-target="#clone"
                                                            data-toggle="modal">
                                                            Clone</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td colspan="8" class="bg-black text-center">
                                                        <button type="" class="btn link-white">First</button>
                                                        |
											<button type="" class="btn link-white">Previous</button>
                                                        |
											<button type="" class="btn link-white">Next</button>
                                                        |
											<button type="" class="btn link-white">Last</button>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="context-menu" id="context-menu" style="display: none; position: absolute;
                                        z-index: 99">
                                        <ul>
                                            <li><a href="#"><i class="fa fa-eye"></i>View</a></li>
                                            <li><a href="#"><i class="fa fa-share-alt"></i>Share</a></li>
                                            <li><a href="#"><i class="fa fa-trash"></i>Delete</a></li>
                                            <li><a href="#"><i class="fa fa-share fa-fw"></i>Move</a></li>
                                            <li><a href="#"><i class="fa fa-files-o"></i>Copy</a></li>
                                        </ul>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>








            </div>



        </div>









    </form>

    <script src="Content/jquery/dist/jquery.min.js"></script>
    <script src="Content/bootstrap/js/bootstrap.min.js"></script>
</body>
</html>
