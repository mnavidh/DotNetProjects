using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;


namespace ConnectOracle
{
    class Program
    {
        static void Main(string[] args)
        {
            string oradb = "Data Source=(DESCRIPTION="
                + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.128.17.196)(PORT=1521)))"
                + "(CONNECT_DATA=(SERVICE_NAME=CCMPRD)));"
                + "User Id=ccmread;Password=ccmread;";
            
            string errmsg = "";
            DataSet ds = new DataSet();
            string dateVal = DateTime.Today.AddYears(-1).ToString("dd-MMM-yyyy");
            string emailSub = "CCM Data Pull Job Succeeded";

            using (OracleConnection conn = new OracleConnection(oradb))
            {
                try
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        cmd.Connection = conn;                       
                        cmd.CommandText = "SELECT c.callid callid,app.valueid appid,app.ValueName application,mod.valueid modid,mod.Valuename module,event.eventid eventid,event.EVENTname event,summary.problemsummaryid sumid, summary.ProblemSUMMARY summary, C.ERRORMESSAGE description, TO_CHAR(c.OnDate,'mm-dd-yyyy HH24:MI') LoggedDate, TO_CHAR(cte.attenddate,'mm-dd-yyyy HH24:MI') AttendedDate, TO_CHAR(cci.solveddate,'mm-dd-yyyy HH24:MI') Resolutiondate, c.Severity severity, cause.causeid causeid, cause.cause cause, (SELECT COUNT(upf.callid) FROM tbl_uploadedfiles upf WHERE upf.callid=c.callid) attach, cci.assignedtoccid ccid, cci.ASSIGNEDTOCCNAME callcenter, h.hierarchyid hid, h.hierarchy_name hierarchy, C.USERID userid, cci.Substatusid statid, cci.subSTATUSNAME status, priority.HAPRIORITYname hapriority, cci.Solution solution, solution.USERCOMMENTS usercomments, cci.MESSAGE MESSAGE, cci.spuserid supportuserid, cci.spusername supportuser,c.traceid traceid,cci.ASSETID2 assetid,ei.TEMPCUBICLE cubicleno,vn.VENDORNAME VendorName,vn.VENDORID VendorId, vs.VENDORSEVNAME VendorSeverityName,vs.VENDORSEVID VendorSeverityId, c.VENDORTICKETNO VendorTicketNO "
                            + "FROM CALL c, cbr_application app, cbr_module mod, cbr_event event, TBL_PROBLEMSUMMARYMASTER summary, tbl_prb_cause cause, tbl_hierarchy h, tbl_hierarchylevels hl, tbl_call_currentinfo cci, tbl_HAPrioritymaster priority, tbl_solutiontrace solution, tbl_extendedinfo ei, tbl_vendorname vn, tbl_vendor_severity vs, calltrace_ent cte "
                        + "WHERE c.callid = cci.callid AND c.moduleid = mod.valueid AND mod.applicationid = app.valueid AND c.EVENTID = event.EVENTid AND c.PROBLEMSUMMARYID = summary.PROBLEMSUMMARYID AND c.causeid = cause.causeid (+) AND cci.sphierarchylevelid = hl.hierarchylevelid AND hl.hierarchyid = h.hierarchyid AND c.hapriorityid = priority.hapriorityid AND c.callid = solution.callid AND c.traceid = solution.traceid AND ei.TICKETID = c.callid AND c.vendorid = vn.vendorid(+) AND c.vendorsevid = vs.vendorsevid(+) AND c.callid = cte.callid AND cte.attenddate = (SELECT MAX(cte1.attenddate) FROM calltrace_ent cte1 WHERE cte1.callid=c.callid) AND c.ondate BETWEEN '" + dateVal + "' AND sysdate AND app.valuename='Application'";
                      
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 300;

                        using (OracleDataAdapter dA = new OracleDataAdapter(cmd))
                        {
                            dA.Fill(ds);                            
                        }

                        SqlConnection sqlconn = new SqlConnection(ConfigurationManager.AppSettings["ConnectSQLDB"].ToString());

                        SqlParameter param = new SqlParameter();
                        param.ParameterName = "CCMDtl";
                        param.SqlDbType = SqlDbType.Structured;
                        param.Value = ds.Tables[0];
                        param.Direction = ParameterDirection.Input;
                        sqlconn.Open();

                        using (sqlconn)
                        {
                            SqlCommand sqlCmd = new SqlCommand("dbo.SaveCCMDetail");                          
                            sqlCmd.Connection = sqlconn;
                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            sqlCmd.Parameters.Add(param);
                            sqlCmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (OracleException ex) // catches only Oracle errors
                {
                    switch (ex.Number)
                    {
                        case 1:
                           errmsg = "Error attempting to insert duplicate data";
                            break;
                        case 12545:
                            errmsg = "The ORACLE database is unavailable";
                            break;
                        default:
                            errmsg = "Database error: " + ex.Message.ToString();
                            break;
                    }
                }
                catch (Exception ex) 
                {
                    errmsg = "General Exception - " + ex.ToString();

                    if (ex.InnerException != null)
                    {
                        errmsg += " Inner exception - " + ex.InnerException.ToString();
                    }

                    if (!string.IsNullOrEmpty(ex.StackTrace))
                    {
                        errmsg += " Stack Trace - " + ex.StackTrace.ToString();
                    }
                }
                finally
                {
                    if (!string.IsNullOrEmpty(errmsg))
                    {
                        emailSub = "CCM Data Pull Job Failure Notification";
                        SendEmailNotification(emailSub, errmsg);
                    }
                    else
                    {
                        SendEmailNotification(emailSub, "Job succeeded for " + DateTime.Now.ToString());

                    }
                    
                }
            }

        }

        
        public static void SendEmailNotification(string sub,string msg)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(ConfigurationManager.AppSettings["smtpServer"]);
                mail.From = new MailAddress(ConfigurationManager.AppSettings["fromEmail"]);
                mail.To.Add(ConfigurationManager.AppSettings["toEmail"]);
                mail.Subject = sub;
                mail.Body = msg;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to trigger error alert");
                Console.WriteLine(ex.ToString());
                Console.Read();
            }
        }
    }
}
