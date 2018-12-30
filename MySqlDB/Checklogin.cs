using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace MySqlDB
{
    public class Checklogin
    {
        //public static string SetUser
        //{
        //    get
        //    {
        //        return HttpContext.Current.Session["User"].ToString();

        //    }
        //    //set
        //    //{
        //    //    HttpContext.Current.Session["User"] = value;
        //    //}
        //}
        public static void CheckLogin()
        {
            
            var the_user = HttpContext.Current.Session["user"];
            try
            {
                if (the_user == "" || the_user == null)
                {
                    System.Web.HttpContext.Current.Response.Redirect("~/Login.html");
                }
            }
            catch (Exception ex)
            {

            }
        }
        public static void Test( string conect)
        {
            var the_user = HttpContext.Current.Session["user"];//用户姓名
            var account = HttpContext.Current.Session["account"];//用户账号
            //var dept_ID = HttpContext.Current.Session["dept_ID"];//用户所属部门
            var role = HttpContext.Current.Session["role"];//用户所属角色
            var company = HttpContext.Current.Session["Company"];//用户所属公司
            try
            {
                if (the_user == "" || the_user == null)
                {
                    System.Web.HttpContext.Current.Response.Redirect("~/Login.html");
                }
                DataTable dt = MysqlHelper.ExecuteDataTable("select count(*) from tab_role_val where RoName ='" + role + "'and Visual ='" + conect + "'");
                if(dt.Rows[0][0].ToString() =="0")
                {
                    dt.Dispose();
                    System.Web.HttpContext.Current.Response.Redirect("~/point.aspx");
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
