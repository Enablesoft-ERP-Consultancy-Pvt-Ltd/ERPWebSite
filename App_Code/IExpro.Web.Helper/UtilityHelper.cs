using IExpro.Core.Models;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace IExpro.Web.Helper
{
    public static class UtilityHelper
    {
        public static void BindList(this ListBox list, IEnumerable<SelectList> lstItem)
        {
            try
            {
                list.DataSource = lstItem;
                list.DataTextField = "ItemName";
                list.DataValueField = "ItemId";
                list.DataBind();
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("UtilityModule|ConditionalComboFill|" + ex.Message);
            }
        }

        public static void BindList(this DropDownList list, IEnumerable<SelectList> lstItem)
        {
            try
            {
                list.DataSource = lstItem;
                list.DataTextField = "ItemName";
                list.DataValueField = "ItemId";
                list.DataBind();
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("UtilityModule|DropDownList|" + ex.Message);
            }
        }


    }
}