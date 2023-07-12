$(function () {

    const FROM_PATTERN = 'YYYY-MM-DD HH:mm:ss.SSS';
    const TO_PATTERN = 'DD/MM/YYYY';

    var orderlist = null;
    var jqxhr = $.getJSON('/Home/OrderList', function (data) {
        orderlist = data;
    });
    jqxhr.done(function () {
        console.log(orderlist);
        /* alert(orderlist.chartData[0].OrderCount);*/
        $('#tblOrder').DataTable({
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

        var SeriesList = [];
        orderlist.chartData.forEach(myFunction);

        function myFunction(item, index, arr) {
            var coOrdinate = [];
            coOrdinate.push((item.OrderStatus == 0 ? "Open" : "Close"));
            coOrdinate.push((item.OrderCount / orderlist.totalSum * 100));
            SeriesList.push(coOrdinate);
        }

        console.log(SeriesList);
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

    });


    var polist = null;
    var jqxhr = $.getJSON('/Home/GetPOStatusList', function (data) {
        polist = data;
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
    var jqxhr = $.getJSON('/Home/GetDyeingStatusList', function (data) {
        dyeinglist = data;
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

















    /* Pie Chart */
    //$('#pie-chart').highcharts({
    //    chart: {
    //        plotBackgroundColor: null,
    //        plotBorderWidth: null,
    //        plotShadow: false
    //    },
    //    title: {
    //        text: 'Orders'
    //    },
    //    tooltip: {
    //        pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
    //    },
    //    plotOptions: {
    //        pie: {
    //            allowPointSelect: true,
    //            cursor: 'pointer',
    //            dataLabels: {
    //                enabled: true,
    //                color: '#000000',
    //                connectorColor: '#000000',
    //                format: '<b>{point.name}</b>: {point.percentage:.1f} %'
    //            }
    //        }
    //    },
    //    series: [{
    //        type: 'pie',
    //        name: 'Order share',
    //        data: [
    //            ['Open', 45.0],
    //            ['Close', 26.8],
    //        ]
    //    }]
    //});
    /* Pie with gradient fill */
    // Radialize the colors
    //Highcharts.getOptions().colors = Highcharts.map(Highcharts.getOptions().colors, function (color) {
    //    return {
    //        radialGradient: { cx: 0.5, cy: 0.3, r: 0.7 },
    //        stops: [
    //            [0, color],
    //            [1, Highcharts.Color(color).brighten(-0.3).get('rgb')] // darken
    //        ]
    //    };
    //});


});