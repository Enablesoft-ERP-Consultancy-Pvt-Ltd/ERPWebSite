using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class MyEventArgs : EventArgs
{
     #region Private Data Members
        int item;
        int quality;
        int design;
        int color;
        int shape;
        int size;
        bool mIsCritical;
    #endregion

    #region Constructor
    /// 
    /// Class constructor, all parameters are required all the time!
    /// 
    /// PARAM name="ex">The SQL Exception raised by the application
    /// The section of the process that caused the error
    /// An indicator if the error was a critical error
    public MyEventArgs(int Item,int Quality, int Design,int Color,int Shape,int Size, bool critical)
    {
        this.item = Item;
        this.quality = Quality;
        this.design = Design;
        this.color = Color;
        this.shape = Shape;
        this.size = Size;
        this.mIsCritical = critical;
    }
    #endregion

    #region Public Properties
    
    public int ItemID
    {
        get { return item; }
    }
    public int QualityID
    {
        get { return quality; }
    }
    public int DesignID
    {
        get { return design; }
    }
    public int ColorID
    {
        get { return color; }
    }
    public int ShapeID
    {
        get { return shape; }
    }
    public int SizeID
    {
        get { return size; }
    }
    /// 
    /// Returns an indicator designating if the failure was critical
    /// 
    public bool IsCritical
    {
        get { return mIsCritical; }
    }
    
    #endregion
    //#region Private Data Members
    //    string id;
    //    bool mIsCritical;
    //#endregion

    //#region Constructor
    ///// 
    ///// Class constructor, all parameters are required all the time!
    ///// 
    ///// PARAM name="ex">The SQL Exception raised by the application
    ///// The section of the process that caused the error
    ///// An indicator if the error was a critical error
    //public MyEventArgs(string selectedid, bool critical)
    //{
    //    this.id = selectedid;
    //    this.mIsCritical = critical;
    //}
    //#endregion

    //#region Public Properties
    ///// 
    ///// Returns the passed SQL Exception detailing the error
    ///// 

    //public string SelectedID
    //{
    //    get { return id; }
    //}
    ///// 
    ///// Returns an indicator designating if the failure was critical
    ///// 
    //public bool IsCritical
    //{
    //    get { return mIsCritical; }
    //}
   
    //#endregion
} 

