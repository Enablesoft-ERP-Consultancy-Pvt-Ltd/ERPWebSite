using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ItemProcessModel
/// </summary>
public class ItemProcessModel
{
    public int SeqNo { get; set; }
    public int ProcessId { get; set; }
    public short ProcessType { get; set; }
    public string ProcessName { get; set; }
}


public class ProcessModel
{
    public int ProcessId { get; set; }
    public string ProcessName { get; set; }
    public string ShortName { get; set; }
    
}