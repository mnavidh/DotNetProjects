using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.SqlTypes;
using System.Data;
using System.Xml;

public partial class _Default : System.Web.UI.Page 
{

    private const string SP_BASICINFO = "DashBoard_GetMaxLoadDate";
    private const string SP_MONTHLYSNAPSHOT = "DashBoard_MonthlySnapshot";
    private const string SP_AGEINGTICKETSINDATERANGE = "DashBoard_AgeingTicketsInDateRange";
    private const string SP_SUPPORTSTAFFQUEUE = "DashBoard_SupportStaffQueue";



    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                UpdateMonthlyGrid();
                UpdateAgeingGrid();
                UpdateSupportQueueGrid();
            }

            UpdateBasicInfo();
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString() + ex.InnerException.ToString());
        }
       

    }

    protected void UpdateBasicInfo()
    {
        headerMaster.InnerText = "Monthly Snapshot (" + DateTime.Today.AddMonths(-1).ToShortDateString() + " - " + DateTime.Today.ToShortDateString() + ")";

        string connString = ConfigurationManager.AppSettings["ConnectDB"].ToString();
        DataSet ds = new DataSet();
        SqlConnection sqlConn = new SqlConnection(connString);

        ds = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, SP_BASICINFO);

        lblMaxLoadDate.Text = "Data loaded on : " + ds.Tables[0].Rows[0].ItemArray[0].ToString();
    }

    protected void UpdateMonthlyGrid()
    {
        string connString = ConfigurationManager.AppSettings["ConnectDB"].ToString();
        DataSet ds = new DataSet();
        SqlConnection sqlConn = new SqlConnection(connString);

        ds = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, SP_MONTHLYSNAPSHOT);

        gridSnapshot.DataSource = ds;

        gridSnapshot.DataBind();

    }

    protected void UpdateAgeingGrid()
    {
        string connString = ConfigurationManager.AppSettings["ConnectDB"].ToString();
        DataSet ds = new DataSet();
        SqlConnection sqlConn = new SqlConnection(connString);

        ds = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, SP_AGEINGTICKETSINDATERANGE);

        gridAgeing.DataSource = ds;

        gridAgeing.DataBind();

    }

    protected void UpdateSupportQueueGrid()
    {
        string connString = ConfigurationManager.AppSettings["ConnectDB"].ToString();
        DataSet ds = new DataSet();
        SqlConnection sqlConn = new SqlConnection(connString);

        ds = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, SP_SUPPORTSTAFFQUEUE);

        gridSupportQueue.DataSource = ds.Tables[0];

        gridSupportQueue.DataBind();

        //Hide the "StaffID" column in the result dataset

        if (gridSupportQueue.Columns.Count > 0)
        {
            gridSupportQueue.Columns[0].Visible = false;
        }
        else
        {
            gridSupportQueue.HeaderRow.Cells[0].Visible = false;
            foreach (GridViewRow gvr in gridSupportQueue.Rows)
            {
                gvr.Cells[0].Visible = false;
            }
        }

    }

    protected void gridSnapshot_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        string hyperLnkPt1 = "<a href=\"#\" onclick='javascript:popUp(\"./DetailsPopUp.aspx";
        string hyperLnkPt2 = "\")'>";
        string hyperLnkPt3 = "</a>";
        string queryStrdata = ""; 

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if(e.Row.Cells[0].Text.ToUpper() == "HA.COM")
            {
                queryStrdata = "?App=1&Sev=";
               
            }
            else if (e.Row.Cells[0].Text.ToUpper() == "HAWAIIANMILES")
            {
                queryStrdata = "?App=2&Sev=";
            }
            else
            {
                queryStrdata = "?App=3&Sev=";
            }

            e.Row.Cells[1].Text = (e.Row.Cells[1].Text == "0") ? "0" : hyperLnkPt1 + queryStrdata + "1" + hyperLnkPt2 + e.Row.Cells[1].Text + hyperLnkPt3;
            e.Row.Cells[2].Text = (e.Row.Cells[2].Text == "0") ? "0" : hyperLnkPt1 + queryStrdata + "2" + hyperLnkPt2 + e.Row.Cells[2].Text + hyperLnkPt3;
            e.Row.Cells[3].Text = (e.Row.Cells[3].Text == "0") ? "0" : hyperLnkPt1 + queryStrdata + "3" + hyperLnkPt2 + e.Row.Cells[3].Text + hyperLnkPt3;

            //Cell formating
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
     
        }

        
    }

    protected void gridAgeing_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        string hyperLnkPt1 = "<a href=\"#\" onclick='javascript:popUp(\"./DetailsPopUp.aspx";
        string hyperLnkPt2 = "\")'>";
        string hyperLnkPt3 = "</a>";
        string queryStrdata = "";

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text.ToUpper().Contains("0 - 5"))
            {
                queryStrdata = "?timePeriod=1&App=";

            }
            else if (e.Row.Cells[0].Text.ToUpper().Contains("6 - 30"))
            {
                queryStrdata = "?timePeriod=2&App=";
            }
            else if (e.Row.Cells[0].Text.ToUpper().Contains("31 - 60"))
            {
                queryStrdata = "?timePeriod=3&App=";
            }
            else if (e.Row.Cells[0].Text.ToUpper().Contains("61 - 365"))
            {
                queryStrdata = "?timePeriod=4&App=";
            }
            else
            {
                queryStrdata = "?timePeriod=5&App=";
            }           

            if (!e.Row.Cells[0].Text.ToUpper().Contains("TOTAL"))
            {
                e.Row.Cells[1].Text = (e.Row.Cells[1].Text == "0") ? "0" : hyperLnkPt1 + queryStrdata + "1" + hyperLnkPt2 + e.Row.Cells[1].Text + hyperLnkPt3;
                e.Row.Cells[2].Text = (e.Row.Cells[2].Text == "0") ? "0" : hyperLnkPt1 + queryStrdata + "2" + hyperLnkPt2 + e.Row.Cells[2].Text + hyperLnkPt3;
                e.Row.Cells[3].Text = (e.Row.Cells[3].Text == "0") ? "0" : hyperLnkPt1 + queryStrdata + "3" + hyperLnkPt2 + e.Row.Cells[3].Text + hyperLnkPt3;
            }

            //Cell formating
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;

        }
    }

    protected void gridSupportQueue_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        string hyperLnkPt1 = "<a href=\"#\" onclick='javascript:popUp(\"./DetailsPopUp.aspx";
        string hyperLnkPt2 = "\")'>";
        string hyperLnkPt3 = "</a>";
        string queryStrdata = "?Staff=";        

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[2].Text = hyperLnkPt1 + queryStrdata + e.Row.Cells[0].Text + hyperLnkPt2 + e.Row.Cells[2].Text + hyperLnkPt3;
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;           
        }
    }

}