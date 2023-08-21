

//enum ProcessName {
//    Dyeing = 5,
//    DIGITALPRINTING=11,
//    TASSEL = 15,

//}





$(function () {

    const FROM_PATTERN = 'YYYY-MM-DD HH:mm:ss.SSS';
    const TO_PATTERN = 'DD/MM/YYYY';

    var orderlist = null;

    $('#myModal').on('hidden.bs.modal', function (evt) {
        $('.modal .modal-body').empty();
    });



    $.ajax({
        type: "POST",
        url: "Home.aspx/OrderList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: {},
        success: function (data) {

            orderlist = $.parseJSON(data.d);
            //BindChart(orderlist);
            /* alert(orderlist.chartData[0].OrderCount);*/
            let table = $('#tblOrder').DataTable({
                lengthMenu: [[5, 10, 25, 50, -1], [10, 25, 50, "All"]],
                data: orderlist.data,
                columns: [
                    { data: 'CustomerCode' },
                    { data: 'CustomerOrderNo' },
                    { data: 'OrderDate' },
                    { data: 'DispatchDate' },
                    {
                        data: 'DelayDays',
                        render: function (data, type) {
                            if (data > 0) {
                                return data + ' Days';
                            }
                            else {
                                return 'No Delay';
                            }
                        },
                    },
                    {
                        data: 'OrderStatus',

                        render: function (data, type) {
                            if (data === 1) {
                                return '<span class="label label-sm label-success">Close</span>';
                            }
                            else {

                                return '<span class="label label-sm label-danger">Open</span>';
                            }
                        },
                    },

                    {
                        data: null,
                        render: function (data, type, row, meta) {
                            var btnHtml = "<a class='btnOrder mrm' exthref=" + data.OrderId + "><i class='fa fa fa-info-circle text-red mrm mediumtxt'></i>Info</a>";
                            btnHtml += "<a class='btnPurchase mrm' exthref=" + data.OrderId + "><i class='fa fa-shopping-cart text-red mrm mediumtxt'></i>Purchase</a>";
                            btnHtml += "<a class='btnDyeing mrm' exthref=" + data.OrderId + "><i class='fa fa-pencil text-red mrm mediumtxt'></i>Dyeing</a>";
                            btnHtml += "<a class='btnDigiPrint mrm' exthref=" + data.OrderId + "><i class='fa fa-print text-red mrm mediumtxt'></i>DIGITAL PRINTING(MTR)</a>";
                            return btnHtml;
                        },
                    },
                ], 
                columnDefs: [

                    {
                        targets: 2,
                        render: function (data) {
                            return moment(data).format('DD MMM  YYYY');
                        },
                    },
                    {
                        targets: 3,
                        render: function (data) {
                            return moment(data).format('DD MMM YYYY');
                        },
                    },
                ]

            });

       

            table.on('click', 'a.btnOrder', function (e) {
                var elem = $(this);
                var id = elem.attr('exthref');
                var _orderId = parseInt(id);
                OrderDetail(_orderId);
            });

            table.on('click', 'a.btnPurchase', function (e) {
                var elem = $(this);
                var id = elem.attr('exthref');
                var _orderId = parseInt(id);
                PurchaseReport(_orderId);
            });

            table.on('click', 'a.btnDyeing', function (e) {
                var elem = $(this);
                var id = elem.attr('exthref');
                var _orderId = parseInt(id);
                var _processId = 5;
                DyeingReport(_orderId, _processId);
            });


            table.on('click', 'a.btnDigiPrint', function (e) {
                var elem = $(this);
                var id = elem.attr('exthref');
                var _orderId = parseInt(id);
                var _processId = 11;
                DyeingReport(_orderId, _processId);
            });
            




        },
        error: function (xhr) {
            alert("There is some technical Issue contact with admin!");
        }
    });



    var polist = null; var jqxhr = $.getJSON('Home.aspx/GetPOStatusList', function (data) {
        polist = $.parseJSON(data.d);

    });
    jqxhr.done(function () {
        console.log(orderlist);

        $('#tblVendorPO').DataTable({
            lengthMenu: [[5, 10, 25, 50, -1], [10, 25, 50, "All"]],
            data: polist.data,
            columns: [
                { data: 'VendorName' },
                { data: 'VendorPO' },
                { data: 'IssueDate' },
                { data: 'ExpectedDate' },
                { data: 'Quantity' },
                {
                    data: 'DelayDays',
                    render: function (data, type) {
                        if (data > 0) {
                            return data + ' Days';
                        }
                        else {
                            return 'No Delay';
                        }
                    },
                },
                {
                    data: 'OrderStatus',
                    render: function (data, type) {
                        if (data === 1) {
                            return '<span class="label label-sm label-success">Close</span>';
                        }
                        else {

                            return '<span class="label label-sm label-danger">Open</span>';
                        }
                    },
                },
            ],
            columnDefs: [
                {
                    targets: 2,
                    render: function (data) {
                        return moment(data).format('DD MMM  YYYY');
                    },
                },
                {
                    targets: 3,
                    render: function (data) {
                        return moment(data).format('DD MMM YYYY');
                    },
                },
            ]

        });

    });


    var dyeinglist = null;
    var jqxhr = $.getJSON('Home.aspx/GetDyeingStatusList', function (data) {

        dyeinglist = $.parseJSON(data.d);
    });
    jqxhr.done(function () {
        console.log(dyeinglist);

        $('#tblDyeing').DataTable({
            lengthMenu: [[5, 10, 25, 50, -1], [10, 25, 50, "All"]],
            data: dyeinglist.data,
            columns: [

                { data: 'DyerName' },
                { data: 'IndentNo' },
                { data: 'IssueDate' },
                { data: 'ExpectedDate' },
                { data: 'Quantity' },
                {
                    data: 'DelayDays',
                    render: function (data, type) {
                        if (data > 0) {
                            return data + ' Days';
                        }
                        else {
                            return 'No Delay';
                        }
                    },
                },
                {
                    data: 'DyeingStatus',
                    render: function (data, type) {
                        if (data === 1) {
                            return '<span class="label label-sm label-success">Close</span>';
                        }
                        else {

                            return '<span class="label label-sm label-danger">Open</span>';
                        }
                    },
                },
            ],
            columnDefs: [
                {
                    targets: 2,
                    render: function (data) {
                        return moment(data).format('DD MMM  YYYY');
                    },
                },
                {
                    targets: 3,
                    render: function (data) {
                        return moment(data).format('DD MMM YYYY');
                    },
                },
            ]

        });


    });




});

function BindChart(orderlist) {

    var SeriesList = [];
    orderlist.chartData.forEach(myFunction);
    function myFunction(item, index, arr) {
        var coOrdinate = [];
        coOrdinate.push((item.OrderStatus == 0 ? "Open" : "Close"));
        coOrdinate.push((item.OrderCount / orderlist.totalSum * 100));
        SeriesList.push(coOrdinate);
    }
    //console.log(seriesList);
    $('#pie-chart').highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false
        },
        title: {
            text: 'Orders'
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    color: '#000000',
                    connectorColor: '#000000',
                    format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                }
            }
        },
        series: [{
            type: 'pie',
            name: 'Order share',
            data: SeriesList
        }]
    });
}




function OrderDetail(_orderId) {


    var bodyHtml = "";

    $('#myModal').find('h4.modal-title').empty();


    $('#myModal').find('h4.modal-title').html("Order Info");
    const obj = { OrderId: _orderId };
    $.ajax({
        url: "Home.aspx/GetOrderDetail",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify(obj),
        success: function (data) {

            var result = $.parseJSON(data.d);
            console.log(data.d)

            bodyHtml += "<div class='row'><div class='col-lg-12'><div class='table-responsive'>";
            bodyHtml += " <table class='table table-hover table-bordered table-striped'><thead>";
            bodyHtml += "<tr><th>Due Date</th>";
            bodyHtml += "<th>Technique</th><th>Quality</th><th>Design</th>";
            bodyHtml += "<th>Color</th><th>Shape</th><th>Shade</th><th>Size</th>";
            bodyHtml += "<th>Unit</th><th>Ouantity</th><th>Filler</th></tr></thead><tbody>";
            if (result.data.length > 0) {
                $.each(result.data, function (index, item) {
                    bodyHtml += "<tr><td>" + item.DueDate + "</td>";
                    bodyHtml += "<td>" + item.Technique + "</td><td>" + item.Quality + "</td><td>" + item.Design + "</td>";
                    bodyHtml += "<td>" + item.Color + "</td><td>" + item.Shape + "</td><td>" + item.Shade + "</td><td>" + item.Size + "</td>";
                    bodyHtml += "<td>" + item.Unit + "</td><td>" + item.OrderQty + "</td><td>" + item.Filler + "</td></tr>";
                });
            }
            else {
                bodyHtml += "<tr><td colspan='11'>Data not found</td></tr>";
            }

            bodyHtml += "</tbody></table></div></div></div>"

        },
        error: function (xhr, status, error) {
            var msg = "Response failed with status: " + status + "</br>"
                + " Error: " + error;
            bodyHtml = "<div class='row'><div class='col-lg-12'><h1 class='largetxt text-red mtn'><strong>" + msg + "</strong></h1></div></div>";
        },
        complete: function (xhr, status) {
            $('#myModal').find('div.modal-body').empty();
            $('#myModal').find('div.modal-body').html(bodyHtml);
            $('#myModal').modal('show');
        }
    });


}


function PurchaseReport(_orderId) {

    var bodyHtml = "";
    $('h4.modal-title').empty();
    $('h4.modal-title').html("Purchase Report");
    const obj = { OrderId: _orderId };
    $.ajax({
        url: "Home.aspx/GetPurchaseList",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify(obj),
        success: function (data) {

            console.log(data.d)

            var result = $.parseJSON(data.d);

            bodyHtml += "<div class='row'><div class='col-lg-12'><div class='table-responsive'>";
            bodyHtml += " <table class='table table-hover table-bordered table-striped'><thead>";
            bodyHtml += " <tr><th>PO No</th><th>PO Status</th><th>PO Date</th><th>Supplier Name</th>";
            bodyHtml += " <th>Item Name</th><th>Rate</th><th>PO Qty</th><th>Delv. Date</th></tr></thead><tbody>";
            if (result.data.length > 0) {
                $.each(result.data, function (index, item) {

                    bodyHtml += "<tr><td>" + item.PONo + "</td><td>" + item.POStatus + "</td><td>" + item.PODate + "</td ><td>" + item.SupplierName + "</td><td>" + item.ItemName + "</td><td>" + item.Rate + "</td><td>" + item.POQty + "</td><td>" + item.DelvDate + "</td></tr>"

                });
            }
            else {
                bodyHtml += "<tr><td colspan='8'>Data not found</td></tr>";
            }

            bodyHtml += "</tbody></table></div></div></div>"

        },
        error: function (xhr, status, error) {
            var msg = "Response failed with status: " + status + "</br>"
                + " Error: " + error;
            bodyHtml = "<div class='row'><div class='col-lg-12'><h1 class='largetxt text-red mtn'><strong>" + msg + "</strong></h1></div></div>";
        },
        complete: function (xhr, status) {
            $('div.modal-body').empty();
            $('div.modal-body').html(bodyHtml);
            $('#myModal').modal('show');
        }
    });


}


function DyeingReport(_orderId, _processId) {

    var bodyHtml = "";

    $('h4.modal-title').empty();
    if (_processId == 11) {
        $('h4.modal-title').html("DIGITAL PRINTING(MTR) Report");
    }
    else {
        $('h4.modal-title').html("Dyeing Report");
    }
 
 




    const obj = { OrderId: _orderId, ProcessId: _processId };
    $.ajax({
        url: "Home.aspx/GetIndentDetail",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify(obj),
        success: function (data) {
            console.log(data.d)
            var result = $.parseJSON(data.d);

            bodyHtml += "<div class='row'><div class='col-lg-12'><div class='table-responsive'>";
            bodyHtml += " <table class='table table-hover table-bordered table-striped'><thead>";
            bodyHtml += "<tr><th>Supplier</th><th>Material Name</th><th>Quality</th><th>Color</th>";
            bodyHtml += "<th>Shade</th><th>Indent No.</th>";
            bodyHtml += "<th>Request Date</th><th>Quantity</th><th>Rec. Qty</th>";
            bodyHtml += "<th>Issue Qty</th><th>Consm. Qty</th><th>Return Qty</th>";
            bodyHtml += "<th>Remarks</th></tr></thead><tbody>";

            if (result.data.length > 0) {

                $.each(result.data, function (index, item) {

                    bodyHtml += "<tr><td>" + item.VendorName + "</td><td>" + item.MaterialName + "</td><td>" + item.QualityName + "</td><td>" + item.ColorName + "</td>";
                    bodyHtml += "<td>" + item.ShadeName + "</td><td>" + item.IndentNo + "</td><td>" + item.ReqDate + "</td>";
                    bodyHtml += "<td>" + item.Quantity + "</td><td>" + item.RecQuantity + "</td><td>" + item.IssueQuantity + "</td><td>" + item.ConsmpQty + "</td>";
                    bodyHtml += "<td>" + item.ReturnQty + "</td><td>" + item.TagRemarks + "</td></tr>";

                });
            }
            else {
                bodyHtml += "<tr><td colspan='17'>Data not found</td></tr></tbody>";
            }

            bodyHtml += "</tbody></table></div></div></div>"

        },
        error: function (xhr, status, error) {
            var msg = "Response failed with status: " + status + "</br>"
                + " Error: " + error;
            bodyHtml = "<div class='row'><div class='col-lg-12'><h1 class='largetxt text-red mtn'><strong>" + msg + "</strong></h1></div></div>";
        },
        complete: function (xhr, status) {
            $('div.modal-body').empty();
            $('div.modal-body').html(bodyHtml);
            $('#myModal').modal('show');
        }
    });


}