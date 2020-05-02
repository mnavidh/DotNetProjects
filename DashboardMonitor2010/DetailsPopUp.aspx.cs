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

public partial class DetailsPopUp : System.Web.UI.Page
{
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    private const string SP_MONTHLYDETAILS = "DashBoard_MonthlySnapshotDetails";
    private const string SP_AGEINGDETAILS = "DashBoard_AgeingTicketsDetails";
    public const string SP_STAFFTICKETS = "DashBoard_SupportStaffQueueDetails";
    private string varApp = "";
    private string varSev = "";
    private string varTimePeriod = "";
    private string varStaffId = "";
    private string sortExpression = "";

    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection"];
        }
        set
        {
            ViewState["sortDirection"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        DetermineQueryStringData();

        if (!Page.IsPostBack)
        {           
            UpdateDetailsGrid();
        }

    }

    private void DetermineQueryStringData()
    {
        if(!string.IsNullOrEmpty(Request.QueryString["App"]))
        {
            varApp = Request.QueryString["App"];
        }       
        
        if (!string.IsNullOrEmpty(Request.QueryString["Sev"]))
        {
            varSev = Request.QueryString["Sev"];
        }
        
        if (!string.IsNullOrEmpty(Request.QueryString["timePeriod"]))
        {
            varTimePeriod = Request.QueryString["timePeriod"];
        }

        if (!string.IsNullOrEmpty(Request.QueryString["Staff"]))
        {
            varStaffId = Request.QueryString["Staff"];
        }

    }

    protected void UpdateDetailsGrid()
    {
        string connString = ConfigurationManager.AppSettings["ConnectDB"].ToString();
        DataSet ds = new DataSet();
        SqlConnection sqlConn = new SqlConnection(connString);


        if (!string.IsNullOrEmpty(varApp) && !string.IsNullOrEmpty(varSev))
        {
            SqlParameter[] inputParams = new SqlParameter[2];

            inputParams[0] = new SqlParameter("@app", varApp);
            inputParams[1] = new SqlParameter("@sev", varSev);
            ds = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, SP_MONTHLYDETAILS, inputParams);
            gridDetails.DataSource = ds.Tables[0];
            gridDetails.DataBind();

        }

        else if (!string.IsNullOrEmpty(varApp) && !string.IsNullOrEmpty(varTimePeriod))
        {
            SqlParameter[] inputParams = new SqlParameter[2];

            inputParams[0] = new SqlParameter("@timePeriod", varTimePeriod);
            inputParams[1] = new SqlParameter("@app", varApp);
            ds = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, SP_AGEINGDETAILS, inputParams);
            gridDetails.DataSource = ds.Tables[0];
            gridDetails.DataBind();
        }
        else if (!string.IsNullOrEmpty(varStaffId))
        {
            SqlParameter[] inputParams = new SqlParameter[1];

            inputParams[0] = new SqlParameter("@staffID", varStaffId);
            ds = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, SP_STAFFTICKETS, inputParams);
            gridDetails.DataSource = ds.Tables[0];
            gridDetails.DataBind();
        }

    }

    protected void gridDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridDetails.PageIndex = e.NewPageIndex;
        UpdateDetailsGrid();
    }

    protected void gridDetails_Sorting(object sender, GridViewSortEventArgs e)
    {
        sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING);
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING);
        }
    }

    protected void gridDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string imgAsc = @" <img src='images/asc.jpeg' title='Ascending' style='width: 12px;height:12px'/>";
        string imgDes = @" <img src='images/desc.jpg' title='Descendng' style='width: 12px;height:12px'/>";

        if (e.Row != null && e.Row.RowType == DataControlRowType.Header)
        {
            foreach (TableCell cell in e.Row.Cells)
            {
                LinkButton lbSort = (LinkButton)cell.Controls[0];
                if (lbSort.Text == sortExpression)
                {
                    if (GridViewSortDirection == SortDirection.Ascending)
                        lbSort.Text += imgAsc;
                    else
                        lbSort.Text += imgDes;
                }
            }
        }
    }

    private void SortGridView(string sortExpression, string direction)
    {
        //To Do - Cache the DataTable for better performance
        UpdateDetailsGrid();
        DataTable dt = gridDetails.DataSource as DataTable;
        DataView dv = new DataView(dt);
        dv.Sort = sortExpression + direction;

        gridDetails.DataSource = dv;
        gridDetails.DataBind();

    }
   
}