using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for OrderDetailModel
/// </summary>
public class OrderDetailModel
{
    public int OrderId { get; set; }
    public int OrderDetailId { get; set; }
    public string ShortName { get; set; }
    public string CustomerCode { get; set; }
    public string CustomerOrderNo { get; set; }
    public string OrderDate { get; set; }
    public string DispatchDate { get; set; }
    public string DueDate { get; set; }
    public string LocalOrder { get; set; }
    public int FinishedId { get; set; }
    public string Technique { get; set; }
    public string Quality { get; set; }
    public string Design { get; set; }
    public string Color { get; set; }
    public string Shape { get; set; }
    public string Shade { get; set; }
    public string Size { get; set; }
    public string Unit { get; set; }
    public string OrderQty { get; set; }
    public string ExtraQty { get; set; }
    public string HoldQty { get; set; }
    public string CancelQty { get; set; }
    public string Filler { get; set; }
    public string Remarks { get; set; }
    public int OrderStatus { get; set; }
}