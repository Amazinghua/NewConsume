using MySqlDB;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace NewJMConsume
{
    /// <summary>
    /// MainBack 的摘要说明
    /// </summary>
    public class MainBack : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string check_str = "";//用来生成查询语句
            string Result = "";
            string types = "";
            bool isExec = false;//是否成功成执行数据库
            int execNum = 0;//执行数据库，受影响行数
            string execStr = "";//执行数据库，返回的统计结果
            DataTable execDt;//执行数据库，返回的datatable
            int recordCount = 0;//数据总量
            string wherestr = "";//where语句
            string sqlStr = "";//sql语句
            JObject resjobj = new JObject();
            try
            {
                string poststr = getpost();
                if (!string.IsNullOrEmpty(poststr))
                {
                    JObject jobj = JObject.Parse(poststr);
                    types = jobject(jobj, "type");//执行哪条命令
                    string pc = jobject(jobj, "pc");//当前页数
                    int page = pc != "" ? int.Parse(pc) : 0;//当前页数
                    string table = jobject(jobj, "table");//数据库表的名字
                    string id = jobject(jobj, "id");//表的主键

                    string btn_type = jobject(jobj, "btn_type");

                    string nowmoney = "";
                    float nm = 0;
                    float addmoney = 0;
                    if (types != "")
                    {
                        switch (types)
                        {

                            #region//删除的通用方法
                            case "delete":
                                object delete_item = jobject(jobj, "delete_item");//数据库表的名字
                                execStr = MysqlHelper.ExecuteNonQuery(sqlStr).ToString();
                                if (execStr == "1")
                                {
                                    resjobj.Add("result", JToken.FromObject("success"));
                                    resjobj.Add("msg", JToken.FromObject("删除成功！"));
                                }
                                else
                                {
                                    resjobj.Add("result", JToken.FromObject("fail"));
                                    resjobj.Add("msg", JToken.FromObject("删除失败！"));
                                }
                                break;
                            #endregion
                            #region//通过选择的公司加载部门
                            case "select_dept":
                                string select_company = jobject(jobj, "select_company");
                                DataTable dt = MysqlHelper.ExecuteDataTable("select Dept_Id,Dept_Name from tbdeptinfo where Company ='" + select_company + "'");
                                dt.Dispose();
                                resjobj.Add("result", JToken.FromObject("ok"));
                                resjobj.Add("dt", JToken.FromObject(dt));
                                break;
                            #endregion
                            #region//加载页面
                            case "load":
                                JObject time = jobject03(jobj, "time");
                                string tiemname = jobject(time, "name");//获取时间字段
                                string btime = jobject(time, "btime");//获取开始字段
                                string etime = jobject(time, "etime");//获取结束时间
                                if (btime != "" && etime != "")
                                {

                                    wherestr += "and " + tiemname + " > '" + btime + "' and " + tiemname + " < '" + etime + "'";
                                }
                                JObject items = jobject03(jobj, "items");
                                foreach (JToken child in items.Children())
                                {
                                    var property1 = child as JProperty;
                                    if (property1.Value.ToString() != "")
                                    {
                                        wherestr += " and " + property1.Name + "=" + "'" + property1.Value + "'";
                                    }
                                }
                                JObject keyword = jobject03(jobj, "keyword");
                                if (keyword != null)
                                {
                                    foreach (JToken child2 in keyword.Children())
                                    {
                                        var property2 = child2 as JProperty;
                                        if (property2.Value.ToString() != "")
                                        {
                                            wherestr += " and " + property2.Name + " like '%" + property2.Value + "%'";
                                        }
                                    }
                                }
                                //if (keyword != null)
                                //{
                                //    wherestr += "and (usr_name like '%" + keyword + "%')";
                                //}
                                if (btn_type != "")
                                {
                                    var titHeader = "序号|账号|年份|月份|充值金额|扣减金额|实充金额|创建时间|创建人|充值途径|备注|admin|卡号";
                                    execDt = MysqlHelper.GetStrTable(table, wherestr, id);
                                    var fileName = "充值记录查询（" + DateTime.Now.ToString("yyyy-MM-ddHHmmss") + "）.xls";
                                    string resultd = DownLoad(fileName, execDt, titHeader);
                                    resjobj.Add("result", JToken.FromObject("ok"));
                                    resjobj.Add("files", JToken.FromObject(fileName));
                                    Result = JsonConvert.SerializeObject(resjobj);
                                    context.Response.Write(Result);
                                    context.Response.End();
                                }


                                execDt = MysqlHelper.getPager(out recordCount, "", table, wherestr, id, page, 10);
                                resjobj.Add("result", JToken.FromObject("ok"));
                                resjobj.Add("execDt", JToken.FromObject(execDt));
                                resjobj.Add("numcount", JToken.FromObject(recordCount));

                                break;
                            #endregion
                            #region//充值记录加载
                            case "TU_detail_load":
                                string detail_btime = jobject(jobj, "btime");
                                string detail_etime = jobject(jobj, "etime");
                                string detail_usr_name = jobject(jobj, "usr_no");
                                string detail_card_no = jobject(jobj, "card_no");
                                string detail_method = jobject(jobj, "method");
                                string detail_creator = jobject(jobj, "creator");
                                if(detail_btime !="" && detail_etime != "")
                                {
                                    wherestr += "and left(t1.create_date,10) between '" + detail_btime + "'and '" + detail_etime + "'";
                                }
                                if(detail_usr_name != "")
                                {
                                    wherestr += "and t2.usr_name like '%"+detail_usr_name+"%'";
                                }
                                if(detail_card_no != "")
                                {
                                    wherestr += "and t1.card_no like '%"+detail_card_no+"%'";
                                }
                                if(detail_method != "")
                                {
                                    wherestr += "and t1,method like '%"+detail_method+"%'";
                                }
                                if(detail_creator != "")
                                {
                                    wherestr += "and t1.creator like '%"+detail_creator+"%'";
                                }
                                if(btn_type != "")
                                {

                                }
                                if(wherestr != "")
                                {
                                    check_str = "select t2.usr_name,t1.usr_no,t1.card_no,t1.add_money,t1.method,t1.create_date,t1.creator,t1.ReMark from tab_add_money t1,tab_user_info t2 where t1.card_no = t2.card_no " + wherestr;
                                    execDt = MysqlHelper.ToDataTablePager(out recordCount, check_str, page, 10);
                                    resjobj.Add("result", JToken.FromObject("ok"));
                                    resjobj.Add("execDt", JToken.FromObject(execDt));
                                    resjobj.Add("numcount", JToken.FromObject(recordCount));
                                }
                                else
                                {
                                    wherestr = "select t2.usr_name,t1.usr_no,t1.card_no,t1.add_money,t1.method,t1.create_date,t1.creator,t1.ReMark from tab_add_money t1,tab_user_info t2 where t1.card_no = t2.card_no";
                                    execDt = MysqlHelper.ToDataTablePager(out recordCount, wherestr, page, 10);
                                    resjobj.Add("result", JToken.FromObject("ok"));
                                    resjobj.Add("execDt", JToken.FromObject(execDt));
                                    resjobj.Add("numcount", JToken.FromObject(recordCount));

                                }
                                break;
                            #endregion
                            #region//加载余额查询页面
                            case "load_Card":
                                string dept_card = jobject(jobj, "dept_card");
                                string name_card = jobject(jobj, "name_card");
                                if (dept_card != "")
                                {
                                    wherestr += "and  Dept_Name  LIKE '%" + dept_card + "%'";
                                }
                                if (name_card != "")
                                {
                                    wherestr += "and usr_name like '%" + name_card + "%'";
                                }
                                if (btn_type != "")
                                {
                                    var titHeader = "姓名|卡号|部门|饭卡余额(元)";
                                    if (wherestr != "")//有条件导出
                                    {
                                        check_str = "select a.usr_name,a.card_no,b.Dept_Name,a.card_money FROM tab_user_info as a,tbdeptinfo b where a.dept_ID = b.Dept_Id" + wherestr;
                                        execDt = MysqlHelper.ExecuteDataTable(check_str);
                                        var fileName = "饭卡余额查询结果（" + DateTime.Now.ToString("yyyy-MM-ddHHmmss") + "）.xls";
                                        string resultd = DownLoad(fileName, execDt, titHeader);
                                        resjobj.Add("result", JToken.FromObject("ok"));
                                        resjobj.Add("files", JToken.FromObject(fileName));
                                        Result = JsonConvert.SerializeObject(resjobj);
                                        context.Response.Write(Result);
                                        context.Response.End();
                                    }
                                    else
                                    {
                                        wherestr = "select a.usr_name,a.card_no,b.Dept_Name,a.card_money FROM tab_user_info as a,tbdeptinfo b where a.dept_ID = b.Dept_Id";
                                        execDt = MysqlHelper.ExecuteDataTable(wherestr);
                                        var fileName = "饭卡余额查询结果（" + DateTime.Now.ToString("yyyy-MM-ddHHmmss") + "）.xls";
                                        string resultd = DownLoad(fileName, execDt, titHeader);
                                        resjobj.Add("result", JToken.FromObject("ok"));
                                        resjobj.Add("files", JToken.FromObject(fileName));
                                        Result = JsonConvert.SerializeObject(resjobj);
                                        context.Response.Write(Result);
                                        context.Response.End();
                                    }
                                }
                                if (wherestr != "")
                                {
                                    check_str = "select * from (select d.usr_name,d.card_no,a.Dept_Id, a.Dept_Name, coalesce(b.Dept_Name, a.Dept_Name) as root,COALESCE(c.Dept_Name,'') as proot,d.card_money from tbdeptinfo a left join tbdeptinfo b on a.Dept_up = b.Dept_Id left JOIN tbdeptinfo c on b.Dept_up = c.Dept_Id  JOIN tab_user_info d on a.dept_Id = d.Dept_Id )as all_tab where 1=1 " + wherestr;
                                    execDt = MysqlHelper.ToDataTablePager(out recordCount, check_str, page, 10);
                                }
                                else
                                {
                                    //wherestr = "select c.usr_name,c.card_no,d.Dept_Name,COALESCE(a.addsum,0)-COALESCE(b.minSum,0) as balance from (select sum(acc_money) as addsum, usr_no from tab_add_money GROUP BY usr_no) as a,(select sum(order_money) as minSum,usr_no from tab_tran_info GROUP BY usr_no) as b, (select * from tab_user_info) as c,(select * from tbdeptinfo) as d where a.usr_no = b.usr_no and b.usr_no = c.usr_no and c.dept_ID = d.dept_Id";
                                    wherestr = "select * from (select d.usr_name,d.card_no,a.Dept_Id, a.Dept_Name, coalesce(b.Dept_Name, a.Dept_Name) as root,COALESCE(c.Dept_Name,'') as proot,d.card_money from tbdeptinfo a left join tbdeptinfo b on a.Dept_up = b.Dept_Id left JOIN tbdeptinfo c on b.Dept_up = c.Dept_Id  JOIN tab_user_info d on a.dept_Id = d.Dept_Id )as all_tab";
                                    execDt = MysqlHelper.ToDataTablePager(out recordCount, wherestr, page, 10);
                                }
                                execDt.Columns.Add("all_root", typeof(string));
                                for (int i = 0; i < execDt.Rows.Count; i++)
                                {
                                    if (execDt.Rows[i]["Dept_Name"].ToString() == execDt.Rows[i]["root"].ToString())
                                    {
                                        execDt.Rows[i]["all_root"] = execDt.Rows[i]["root"].ToString();
                                    }
                                    else
                                    {
                                        execDt.Rows[i]["all_root"] = execDt.Rows[i]["proot"].ToString() + "/" + execDt.Rows[i]["root"].ToString() + "/" + execDt.Rows[i]["Dept_Name"].ToString();
                                    }

                                }
                                resjobj.Add("result", JToken.FromObject("ok"));
                                resjobj.Add("execDt", JToken.FromObject(execDt));
                                resjobj.Add("numcount", JToken.FromObject(recordCount));
                                break;
                            #endregion

                            #region//查找需要充值的部门的所有人员

                            case "select_all_department":
                                string show_checked = jobject(jobj, "user_checked");
                                string show_money = jobject(jobj, "money");
                                //遍历人员充值
                                string[] show_array = show_checked.Split(',');
                                int[] show_iNums;
                                show_iNums = Array.ConvertAll(show_array, int.Parse);
                                List<string> show_list = new List<string>();
                                foreach (int i in show_iNums)
                                {
                                    if (i == show_iNums[0])
                                    {
                                        wherestr += "where t1.dept_ID ='" + i + "'";
                                    }
                                    else
                                    {
                                        wherestr += " or t1.dept_ID ='" + i + "'";
                                    }
                                }
                                check_str = "select t1.ust_ID,t1.usr_name,t2.Dept_Name,t1.card_no,t1.phone_no from tab_user_info t1 LEFT JOIN tbdeptinfo t2 on t1.dept_ID =t2.Dept_Id " + wherestr;
                                DataTable show_dt = MySqlDB.MysqlHelper.ExecuteDataTable(check_str);
                                resjobj.Add("result", JToken.FromObject("success"));
                                resjobj.Add("msg", JToken.FromObject("请确定充值人员"));
                                resjobj.Add("show_dt", JToken.FromObject(show_dt));

                                break;
                            #endregion
                            #region//按部门充值
                            case "department_charge":
                                //string cardno = jobject(jobj, "cardno");
                                //string name = jobject(jobj, "name");
                                string dept_checked = jobject(jobj, "user_checked");
                                string dept_method = jobject(jobj, "method");
                                string dept_money = jobject(jobj, "money");
                                string dept_memo = jobject(jobj, "memo");
                                string dept_usrno = jobject(jobj, "usrno");
                                //string cash_phone = jobject(jobj, "cash_phone");
                                //float dept_nm = 0;
                                //float dept_addmoney = 0;
                                //遍历人员充值
                                string[] dept_array = dept_checked.Split(',');
                                int[] dept_iNums;
                                dept_iNums = Array.ConvertAll(dept_array, int.Parse);
                                List<string> department_charge_list = new List<string>();
                                foreach (int i in dept_iNums)
                                {
                                    DataTable dt0 = MysqlHelper.ExecuteDataTable("select * from tab_user_info where dept_ID ='" + i + "'");
                                    if(dt0.Rows.Count != 0)
                                    {
                                        for(int k = 0; k < dt0.Rows.Count; k++)
                                        {
                                            nowmoney = dt0.Rows[k]["card_money"].ToString();
                                            nm = float.Parse(nowmoney);
                                            addmoney = float.Parse(dept_money) + nm;
                                            int execNum2 = MysqlHelper.ExecuteNonQuery("update tab_user_info set card_money = '" + addmoney + "' where ust_ID ='" + dt0.Rows[k]["ust_ID"].ToString() + "'");
                                            if(execNum2 == 1)
                                            {
                                                execNum = MysqlHelper.ExecuteNonQuery("insert into tab_add_money (usr_no,add_year,add_month,add_money,minus_money,acc_money,create_date,creator,method,ReMark,usr_name,card_no)values('" + dt0.Rows[k]["usr_no"].ToString() + "', year(CURRENT_DATE), month(CURRENT_DATE), '" + dept_money + "', '0', '" + dept_money + "', NOW(), '" + dept_usrno + "', '" + dept_method + "', '" + dept_memo + "', '" + dept_usrno + "', '" + dt0.Rows[k]["card_no"].ToString() + "')");
                                                if (execNum == 1)
                                                {
                                                    department_charge_list.Add(dt0.Rows[k]["ust_ID"].ToString());
                                                    string message = "充值提醒:" + "你好！您的饭卡于 </br> " + DateTime.Now.ToString() + "充值" + dept_money + "元";
                                                    //插入微信推送充值记录
                                                    int a = MysqlHelper.ExecuteNonQuery("insert into WX_add_money_push_info (usr_no,add_money,message,phone_no,create_time)values('" + dt0.Rows[k]["usr_no"].ToString() + "','" + dept_money + "','" + message + "','" + dt0.Rows[k]["phone_no"].ToString() + "',NOW())");

                                                    MySqlDB.QYWeixinHelper.SendText(dt0.Rows[k]["usr_no"].ToString(), message);
                                                }
                                            }
                                        }
                                    }
                                }
                                //遍历充值成功的人员数组
                                string[] strArray = department_charge_list.ToArray();//strArray=[str0,str1,str2]
                                foreach(string iarray in strArray)
                                {
                                    if(iarray == strArray[0])
                                    {
                                        wherestr += "where ust_ID ='" + iarray + "'";
                                    }
                                    else
                                    {
                                        wherestr += " or ust_ID ='" + iarray + "'";
                                    }
                                }
                                if (wherestr != "")
                                {
                                    DataTable department_dt = MySqlDB.MysqlHelper.ExecuteDataTable("select * from tab_user_info " + wherestr);
                                    resjobj.Add("result", JToken.FromObject("success"));
                                    resjobj.Add("msg", JToken.FromObject("充值成功"));
                                    resjobj.Add("department_dt", JToken.FromObject(department_dt));
                                }
                                break;
                            #endregion
                            #region//个人充值
                            case "recharge_self":
                                //string cardno = jobject(jobj, "cardno");
                                //string name = jobject(jobj, "name");
                                string user_checked = jobject(jobj, "user_checked");
                                string method = jobject(jobj, "method");
                                string money = jobject(jobj, "money");
                                string memo = jobject(jobj, "memo");
                                string usrno = jobject(jobj, "usrno");
                                //string cash_phone = jobject(jobj, "cash_phone");
                                #region//遍历人员充值
                                string[] user_array = user_checked.Split(',');
                                int[] iNums;
                                iNums = Array.ConvertAll(user_array, int.Parse);
                                List<string> self_charge_list = new List<string>();
                                foreach (int i in iNums)
                                {
                                    DataTable dt0 = MysqlHelper.ExecuteDataTable("select * from tab_user_info where ust_ID ='" + i + "'");
                                    nowmoney = dt0.Rows[0]["card_money"].ToString();
                                    nm = float.Parse(nowmoney);
                                    addmoney = float.Parse(money) + nm;
                                    int execNum2 = MysqlHelper.ExecuteNonQuery("update tab_user_info set card_money = '" + addmoney + "' where ust_ID ='" + i + "'");
                                    if (execNum2 == 1)
                                    {
                                        execNum = MysqlHelper.ExecuteNonQuery("insert into tab_add_money (usr_no,add_year,add_month,add_money,minus_money,acc_money,create_date,creator,method,ReMark,usr_name,card_no)values('" + dt0.Rows[0]["usr_no"].ToString() + "', year(CURRENT_DATE), month(CURRENT_DATE), '" + money + "', '0', '" + money + "', NOW(), '" + usrno + "', '" + method + "', '" + memo + "', '" + usrno + "', '" + dt0.Rows[0]["card_no"].ToString() + "')");
                                        if (execNum == 1)
                                        {
                                            self_charge_list.Add(i.ToString());
                                            string message = "充值提醒:" + "你好！您的饭卡于 </br> " + DateTime.Now.ToString() + "充值" + money + "元";
                                            //插入微信推送充值记录
                                            int a = MysqlHelper.ExecuteNonQuery("insert into WX_add_money_push_info (usr_no,add_money,message,phone_no,create_time)values('" + dt0.Rows[0]["usr_no"].ToString() + "','" + money + "','" + message + "','" + dt0.Rows[0]["phone_no"].ToString() + "',NOW())");

                                            MySqlDB.QYWeixinHelper.SendText(dt0.Rows[0]["usr_no"].ToString(), message);
                                        }
                                    }
                                }
                                //遍历充值成功的人员数组
                                string[] self_Array = self_charge_list.ToArray();//strArray=[str0,str1,str2]
                                foreach (string iarray in self_Array)
                                {
                                    if (iarray == self_Array[0])
                                    {
                                        wherestr += "where ust_ID ='" + iarray + "'";
                                    }
                                    else
                                    {
                                        wherestr += " or ust_ID ='" + iarray + "'";
                                    }
                                }
                                if (wherestr != "")
                                {
                                    DataTable self_dt = MySqlDB.MysqlHelper.ExecuteDataTable("select * from tab_user_info " + wherestr);
                                    self_dt.Columns.Add("add_money_now", typeof(string));
                                    for(int i = 0; i < self_dt.Rows.Count; i++)
                                    {
                                        self_dt.Rows[i]["add_money_now"] = money.ToString();
                                    }
                                    resjobj.Add("result", JToken.FromObject("success"));
                                    resjobj.Add("msg", JToken.FromObject("充值成功"));
                                    resjobj.Add("self_dt", JToken.FromObject(self_dt));
                                }
                                #endregion

                                break;
                            case "check_usr":
                                string check_key = jobject(jobj, "check_key");
                                string sql = "select * from(select a.ust_ID,a.Company,a.usr_no,a.usr_name,a.card_no,b.Dept_Name,a.phone_no from tab_user_info a,tbdeptinfo b where a.dept_ID =b.Dept_Id ) as tab_all_user where usr_no like '%" + check_key + "%' or card_no like '%" + check_key + "%' or usr_name like '%" + check_key + "%' or phone_no like '%" + check_key + "%'";
                                execDt = MysqlHelper.ToDataTablePager(out recordCount, sql, page, 10);
                                if (execDt.Rows.Count != 0)
                                {
                                    resjobj.Add("result", JToken.FromObject("ok"));
                                    resjobj.Add("execDt", JToken.FromObject(execDt));
                                    resjobj.Add("numcount", JToken.FromObject(recordCount));
                                }
                                else if (execDt.Rows.Count == 0)
                                {
                                    resjobj.Add("result", JToken.FromObject("fail"));
                                    resjobj.Add("msg", JToken.FromObject("暂无相关信息！"));
                                }

                                break;
                            #endregion
                            #region//按部门充值
                            case "add_money_dept":
                                float add_money = 0;
                                string select_company_dept = jobject(jobj, "select_company");
                                string select_dept = jobject(jobj, "select_dept");
                                string add_money_now = jobject(jobj, "add_money");
                                if (select_company_dept != "")
                                {
                                    wherestr += "and Company ='" + select_company_dept + "'";
                                }
                                if (select_dept != "")
                                {
                                    wherestr += "and dept_ID ='" + select_dept + "'";
                                }
                                check_str = "select * from tab_user_info where 1=1 " + wherestr;
                                execDt = MysqlHelper.ExecuteDataTable(check_str);
                                if (execDt.Rows.Count == 0)
                                {
                                    resjobj.Add("result", JToken.FromObject("fail"));
                                    resjobj.Add("msg", JToken.FromObject("充值失败！原因:未查到人员"));
                                }
                                else
                                {
                                    for (int i = 0; i < execDt.Rows.Count; i++)
                                    {
                                        nowmoney = MysqlHelper.ExecuteDataTable("select card_money from tab_user_info where usr_no = '" + execDt.Rows[i]["usr_no"].ToString() + "'").Rows[0]["card_money"].ToString();
                                        nm = float.Parse(nowmoney);
                                        add_money = float.Parse(add_money_now) + nm;
                                        int execNum2 = MysqlHelper.ExecuteNonQuery("update tab_user_info set card_money = '" + add_money + "' where usr_no ='" + execDt.Rows[i]["usr_no"].ToString() + "'");
                                        if (execNum2 == 1)
                                        {
                                            execNum = MysqlHelper.ExecuteNonQuery("insert into tab_add_money (usr_no,add_year,add_month,add_money,minus_money,acc_money,create_date,creator,method,ReMark,usr_name,card_no)values('" + execDt.Rows[i]["usr_no"].ToString() + "', year(CURRENT_DATE), month(CURRENT_DATE), '" + add_money_now + "', '0', '" + add_money_now + "', NOW(), '" + "admin" + "', '" + "部门充值" + "', '" + "无" + "', '" + execDt.Rows[i]["usr_no"].ToString() + "', '" + execDt.Rows[i]["card_no"].ToString() + "')");
                                            if (execNum == 1)
                                            {
                                                string message = "充值提醒:" + "你好!您的饭卡于 </br> " + DateTime.Now.ToString() + "充值" + add_money_now + "元";
                                                //插入微信推送充值记录
                                                int a = MysqlHelper.ExecuteNonQuery("insert into WX_add_money_push_info (usr_no,add_money,message,phone_no,create_time)values('" + execDt.Rows[i]["usr_no"].ToString() + "','" + add_money_now + "','" + message + "','" + execDt.Rows[i]["phone_no"].ToString() + "',NOW())");
                                                MySqlDB.QYWeixinHelper.SendText(execDt.Rows[i]["usr_no"].ToString(), message);
                                            }
                                        }

                                    }
                                    resjobj.Add("result", JToken.FromObject("success"));
                                    resjobj.Add("msg", JToken.FromObject("按照部门充值成功！"));
                                }
                                break;
                            #endregion
                            #region//订餐统计
                            case "BM_Count_load":
                                string date_count = jobject(jobj, "date_count");
                                string company_count = jobject(jobj, "company_count");
                                string place_count = jobject(jobj, "place_count");
                                //var parent_id = HttpContext.Current.Session["parent_id"];
                                if (date_count != "")
                                {
                                    wherestr += "and left(t1.order_date,10) = '" + date_count + "'";
                                }
                                if (company_count != "")
                                {
                                    wherestr += "and t2.Company like '%" + company_count + "%'";
                                }
                                if (place_count != "")
                                {
                                    wherestr += "and t2.FeePlace = '" + place_count + "'";
                                }
                                if (HttpContext.Current.Session["parent_id"] != "" || HttpContext.Current.Session["parent_id"] != null)
                                {
                                    if (HttpContext.Current.Session["parent_id"].ToString() != "1")
                                    {
                                        wherestr += "and t2.Company  like '%" + HttpContext.Current.Session["Company"].ToString() + "%'";
                                    }
                                }

                                if (btn_type != "") //导出excel
                                {
                                    var titHeader = "日期|公司|消费地点|早餐-工作餐|早餐-自助餐|午餐-工作餐|午餐-自助餐|晚餐-工作餐|晚餐-自助餐|小计";
                                    if (wherestr != "") //有条件的导出
                                    {
                                        check_str = "select left(t1.order_date,10) order_date,t2.Company Company,t2.FeePlace FeePlace,count(case when t1.order_Price_Type ='早餐' and order_type='0' then 1 ELSE NULL end) Breakfast_work,count(case when t1.order_Price_Type ='早餐' and order_type='1' then 1 ELSE NULL end) Breakfast_buffet,count(case when t1.order_Price_Type ='午餐' and order_type='0' then 1 ELSE NULL end) Lunch_work,count(case when t1.order_Price_Type ='午餐' and order_type='1' then 1 ELSE NULL end) Lunch_buffet,count(case when t1.order_Price_Type ='晚餐' and order_type='0' then 1 ELSE NULL end) Dinner_work,count(case when t1.order_Price_Type ='晚餐' and order_type='1' then 1 ELSE NULL end) Dinner_buffet,COUNT(t1.order_count) total from tab_order_info t1 left JOIN tab_user_info t2 on t1.card_no = t2.card_no where 1=1 " + wherestr + " GROUP BY t1.order_date,t2.Company,t2.FeePlace ORDER BY t1.order_date DESC";
                                        execDt = MysqlHelper.ExecuteDataTable(check_str);
                                        var fileName = "订餐统计（" + DateTime.Now.ToString("yyyy-MM-ddHHmmss") + "）.xls";
                                        string resultd = DownLoad(fileName, execDt, titHeader);
                                        resjobj.Add("result", JToken.FromObject("ok"));
                                        resjobj.Add("files", JToken.FromObject(fileName));
                                        Result = JsonConvert.SerializeObject(resjobj);
                                        context.Response.Write(Result);
                                        context.Response.End();

                                    }
                                    else
                                    {
                                        wherestr = "select left(t1.order_date,10) order_date,t2.Company Company,t2.FeePlace FeePlace,count(case when t1.order_Price_Type ='早餐' and order_type='0' then 1 ELSE NULL end) Breakfast_work,count(case when t1.order_Price_Type ='早餐' and order_type='1' then 1 ELSE NULL end) Breakfast_buffet,count(case when t1.order_Price_Type ='午餐' and order_type='0' then 1 ELSE NULL end) Lunch_work,count(case when t1.order_Price_Type ='午餐' and order_type='1' then 1 ELSE NULL end) Lunch_buffet,count(case when t1.order_Price_Type ='晚餐' and order_type='0' then 1 ELSE NULL end) Dinner_work,count(case when t1.order_Price_Type ='晚餐' and order_type='1' then 1 ELSE NULL end) Dinner_buffet,COUNT(t1.order_count) total from tab_order_info t1 left JOIN tab_user_info t2 on t1.card_no = t2.card_no GROUP BY t1.order_date,t2.Company,t2.FeePlace ORDER BY t1.order_date DESC";
                                        execDt = MysqlHelper.ExecuteDataTable(wherestr);
                                        var fileName = "订餐统计（" + DateTime.Now.ToString("yyyy-MM-ddHHmmss") + "）.xls";
                                        string resultd = DownLoad(fileName, execDt, titHeader);

                                        resjobj.Add("result", JToken.FromObject("ok"));
                                        resjobj.Add("files", JToken.FromObject(fileName));
                                        Result = JsonConvert.SerializeObject(resjobj);
                                        context.Response.Write(Result);
                                        context.Response.End();
                                    }
                                }
                                if (wherestr != "")
                                {
                                    check_str = "select left(t1.order_date,10) order_date,t2.Company Company,t2.FeePlace FeePlace,count(case when t1.order_Price_Type ='早餐' and order_type='0' then 1 ELSE NULL end) Breakfast_work,count(case when t1.order_Price_Type ='早餐' and order_type='1' then 1 ELSE NULL end) Breakfast_buffet,count(case when t1.order_Price_Type ='午餐' and order_type='0' then 1 ELSE NULL end) Lunch_work,count(case when t1.order_Price_Type ='午餐' and order_type='1' then 1 ELSE NULL end) Lunch_buffet,count(case when t1.order_Price_Type ='晚餐' and order_type='0' then 1 ELSE NULL end) Dinner_work,count(case when t1.order_Price_Type ='晚餐' and order_type='1' then 1 ELSE NULL end) Dinner_buffet,COUNT(t1.order_count) total from tab_order_info t1 left JOIN tab_user_info t2 on t1.card_no = t2.card_no where 1=1 " + wherestr + " GROUP BY t1.order_date,t2.Company,t2.FeePlace ORDER BY t1.order_date DESC";
                                    execDt = MysqlHelper.ToDataTablePager(out recordCount, check_str, page, 10);
                                    resjobj.Add("result", JToken.FromObject("ok"));
                                    resjobj.Add("execDt", JToken.FromObject(execDt));
                                    resjobj.Add("numcount", JToken.FromObject(recordCount));
                                }
                                else
                                {
                                    wherestr = "select left(t1.order_date,10) order_date,t2.Company Company,t2.FeePlace FeePlace,count(case when t1.order_Price_Type ='早餐' and order_type='0' then 1 ELSE NULL end) Breakfast_work,count(case when t1.order_Price_Type ='早餐' and order_type='1' then 1 ELSE NULL end) Breakfast_buffet,count(case when t1.order_Price_Type ='午餐' and order_type='0' then 1 ELSE NULL end) Lunch_work,count(case when t1.order_Price_Type ='午餐' and order_type='1' then 1 ELSE NULL end) Lunch_buffet,count(case when t1.order_Price_Type ='晚餐' and order_type='0' then 1 ELSE NULL end) Dinner_work,count(case when t1.order_Price_Type ='晚餐' and order_type='1' then 1 ELSE NULL end) Dinner_buffet,COUNT(t1.order_count) total from tab_order_info t1 left JOIN tab_user_info t2 on t1.card_no = t2.card_no GROUP BY t1.order_date,t2.Company,t2.FeePlace ORDER BY t1.order_date DESC";
                                    execDt = MysqlHelper.ToDataTablePager(out recordCount, wherestr, page, 10);
                                    resjobj.Add("result", JToken.FromObject("ok"));
                                    resjobj.Add("execDt", JToken.FromObject(execDt));
                                    resjobj.Add("numcount", JToken.FromObject(recordCount));
                                }
                                break;

                            #endregion
                            #region//订餐明细页面加载
                            case "BM_Detail_load":
                                string check_detail = jobject(jobj, "check_detail");
                                string price_type_detail = jobject(jobj, "price_type_detail");
                                string place_detail = jobject(jobj, "place_detail");
                                string name_detail = jobject(jobj, "name_detail");
                                if (check_detail != "")
                                {
                                    wherestr += "and left(t1.order_date,10)= '" + check_detail + "'";
                                }
                                if (price_type_detail != "")
                                {
                                    wherestr += "and t1.order_Price_Type = '" + price_type_detail + "'";
                                }
                                if (place_detail != "")
                                {
                                    wherestr += "and t2.FeePlace like '%" + place_detail + "%'";
                                }
                                if (name_detail != "")
                                {
                                    wherestr += "and t2.usr_name like '%" + name_detail + "%'";
                                }
                                if (btn_type != "")
                                {
                                    var titHeader = "部门|姓名|订餐生成日期|餐次|餐类|消费地点";
                                    if (wherestr != "") //有条件的导出
                                    {
                                        check_str = "select t2.dept_ID,t2.usr_name,left(t1.order_date ,10) order_data,t1.order_Price_Type,(case when t1.order_Price_Type ='早餐' and order_type='0' then '工作餐' when t1.order_Price_Type ='早餐' and order_type ='1' then '自助餐' when t1.order_Price_Type ='午餐' and order_type ='0' then '工作餐' when t1.order_Price_Type ='午餐' and order_type ='1' then '自助餐' when t1.order_Price_Type ='晚餐' and order_type ='0' then '工作餐' when t1.order_Price_Type ='晚餐' and order_type ='1' then '自助餐' ELSE '其他' end) meal_types,t2.FeePlace from tab_order_info t1 JOIN tab_user_info t2 on t1.card_no = t2.card_no where 1=1 " + wherestr;
                                        execDt = MysqlHelper.ExecuteDataTable(check_str);
                                        var fileName = "订餐明细（" + DateTime.Now.ToString("yyyy-MM-ddHHmmss") + "）.xls";
                                        string resultd = DownLoad(fileName, execDt, titHeader);
                                        resjobj.Add("result", JToken.FromObject("ok"));
                                        resjobj.Add("files", JToken.FromObject(fileName));
                                        Result = JsonConvert.SerializeObject(resjobj);
                                        context.Response.Write(Result);
                                        context.Response.End();

                                    }
                                    else
                                    {
                                        wherestr = "select t2.dept_ID,t2.usr_name,left(t1.order_date ,10) order_data,t1.order_Price_Type,(case when t1.order_Price_Type ='早餐' and order_type='0' then '工作餐' when t1.order_Price_Type ='早餐' and order_type ='1' then '自助餐' when t1.order_Price_Type ='午餐' and order_type ='0' then '工作餐' when t1.order_Price_Type ='午餐' and order_type ='1' then '自助餐' when t1.order_Price_Type ='晚餐' and order_type ='0' then '工作餐' when t1.order_Price_Type ='晚餐' and order_type ='1' then '自助餐' ELSE '其他' end) meal_types,t2.FeePlace from tab_order_info t1 JOIN tab_user_info t2 on t1.card_no = t2.card_no ";
                                        execDt = MysqlHelper.ExecuteDataTable(wherestr);
                                        var fileName = "订餐明细（" + DateTime.Now.ToString("yyyy-MM-ddHHmmss") + "）.xls";
                                        string resultd = DownLoad(fileName, execDt, titHeader);

                                        resjobj.Add("result", JToken.FromObject("ok"));
                                        resjobj.Add("files", JToken.FromObject(fileName));
                                        Result = JsonConvert.SerializeObject(resjobj);
                                        context.Response.Write(Result);
                                        context.Response.End();
                                    }
                                }
                                if (wherestr != "")
                                {
                                    check_str = "select t3.Dept_Name,t2.dept_ID,t2.usr_name,left(t1.order_date ,10) order_data,t1.order_Price_Type,(case when t1.order_Price_Type ='早餐' and order_type='0' then '工作餐' when t1.order_Price_Type ='早餐' and order_type ='1' then '自助餐' when t1.order_Price_Type ='午餐' and order_type ='0' then '工作餐' when t1.order_Price_Type ='午餐' and order_type ='1' then '自助餐' when t1.order_Price_Type ='晚餐' and order_type ='0' then '工作餐' when t1.order_Price_Type ='晚餐' and order_type ='1' then '自助餐' ELSE '其他' end) meal_types,t2.FeePlace from tab_order_info t1 JOIN tab_user_info t2 on t1.card_no = t2.card_no LEFT JOIN tbdeptinfo t3 on t2.dept_ID = t3.Dept_Id where 1=1 " + wherestr;
                                    execDt = MysqlHelper.ToDataTablePager(out recordCount, check_str, page, 10);
                                    resjobj.Add("result", JToken.FromObject("ok"));
                                    resjobj.Add("execDt", JToken.FromObject(execDt));
                                    resjobj.Add("numcount", JToken.FromObject(recordCount));
                                }
                                else
                                {
                                    wherestr = "select t3.Dept_Name,t2.dept_ID,t2.usr_name,left(t1.order_date ,10) order_data,t1.order_Price_Type,(case when t1.order_Price_Type ='早餐' and order_type='0' then '工作餐' when t1.order_Price_Type ='早餐' and order_type ='1' then '自助餐' when t1.order_Price_Type ='午餐' and order_type ='0' then '工作餐' when t1.order_Price_Type ='午餐' and order_type ='1' then '自助餐' when t1.order_Price_Type ='晚餐' and order_type ='0' then '工作餐' when t1.order_Price_Type ='晚餐' and order_type ='1' then '自助餐' ELSE '其他' end) meal_types,t2.FeePlace from tab_order_info t1 JOIN tab_user_info t2 on t1.card_no = t2.card_no LEFT JOIN tbdeptinfo t3 on t2.dept_ID = t3.Dept_Id";
                                    execDt = MysqlHelper.ToDataTablePager(out recordCount, wherestr, page, 10);
                                    resjobj.Add("result", JToken.FromObject("ok"));
                                    resjobj.Add("execDt", JToken.FromObject(execDt));
                                    resjobj.Add("numcount", JToken.FromObject(recordCount));
                                }

                                break;
                            #endregion

                            #region//结算管理
                            #region//消费明细查询
                            case "FEE_detail_load":
                                string FEE_btime = jobject(jobj, "FEE_btime");
                                string FEE_etime = jobject(jobj, "FEE_etime");
                                string FEE_type = jobject(jobj, "FEE_type");
                                string FEE_dept = jobject(jobj, "FEE_dept");
                                string FEE_dev_no = jobject(jobj, "FEE_dev_no");
                                string FEE_name = jobject(jobj, "FEE_name");
                                string pay_type = jobject(jobj, "pay_type");
                                string FEE_place = jobject(jobj, "FEE_place");
                                if (FEE_btime != "" && FEE_etime != "")
                                {
                                    wherestr += "and left(t1.order_create_date,10) between '" + FEE_btime + "'and '" + FEE_etime + "'";
                                }
                                if (FEE_type != "")
                                {
                                    wherestr += "and t1.order_Price_Type ='" + FEE_type + "'";
                                }
                                if (FEE_dept != "")
                                {
                                    wherestr += "and t3.Dept_Name like '%" + FEE_dept + "%'";
                                }
                                if (FEE_dev_no != "")
                                {
                                    wherestr += "and t1.dev_no like '%" + FEE_dev_no + "%'";
                                }
                                if (FEE_name != "")
                                {
                                    wherestr += "and t2.usr_name like '%" + FEE_name + "%'";
                                }
                                if (pay_type != "")
                                {
                                    wherestr += "and t1.order_Price_Name like '%" + pay_type + "%'";
                                }
                                if (FEE_place != "")
                                {
                                    wherestr += "and t1.dev_Ip like '%" + FEE_place + "%'";
                                }
                                if (btn_type != "")
                                {
                                    var titHeader = "序号|结算单位|姓名|消费地点|电话号码|部门|类型|餐类|金额|生成时间";
                                    if (wherestr != "")
                                    {
                                        check_str = "select t1.order_id,t1.dev_no,t2.usr_name,t1.dev_Ip,t2.phone_no,t2.dept_ID,t1.order_Price_Name,t1.order_Price_Type,t1.order_money,t1.order_create_date from tab_tran_info t1 LEFT JOIN tab_user_info t2 on t1.card_no = t2.card_no where 1=1 " + wherestr;
                                        execDt = MysqlHelper.ExecuteDataTable(check_str);
                                        var fileName = "消费明细表下载（" + DateTime.Now.ToString("yyyy-MM-ddHHmmss") + "）.xls";
                                        string resultd = DownLoad(fileName, execDt, titHeader);

                                        resjobj.Add("result", JToken.FromObject("ok"));
                                        resjobj.Add("files", JToken.FromObject(fileName));
                                        Result = JsonConvert.SerializeObject(resjobj);
                                        context.Response.Write(Result);
                                        context.Response.End();
                                    }
                                    else
                                    {

                                        wherestr = "select t1.order_id,t1.dev_no,t2.usr_name,t1.dev_Ip,t2.phone_no,t2.dept_ID,t1.order_Price_Name,t1.order_Price_Type,t1.order_money,t1.order_create_date from tab_tran_info t1 LEFT JOIN tab_user_info t2 on t1.card_no = t2.card_no";
                                        execDt = MysqlHelper.ExecuteDataTable(wherestr);
                                        var fileName = "消费明细表下载（" + DateTime.Now.ToString("yyyy-MM-ddHHmmss") + "）.xls";
                                        string resultd = DownLoad(fileName, execDt, titHeader);

                                        resjobj.Add("result", JToken.FromObject("ok"));
                                        resjobj.Add("files", JToken.FromObject(fileName));
                                        Result = JsonConvert.SerializeObject(resjobj);
                                        context.Response.Write(Result);
                                        context.Response.End();
                                    }
                                }
                                if (wherestr != "")
                                {
                                    check_str = "select t1.order_id,t1.dev_no,t2.usr_name,t1.dev_Ip,t2.phone_no,t3.Dept_Name,t2.dept_ID,t1.order_Price_Name,t1.order_Price_Type,t1.order_money,t1.order_create_date from tab_tran_info t1 LEFT JOIN tab_user_info t2 on t1.card_no = t2.card_no LEFT JOIN tbdeptinfo t3  on t2.dept_ID = t3.Dept_Id  where 1=1 " + wherestr;
                                    execDt = MysqlHelper.ToDataTablePager(out recordCount, check_str, page, 10);
                                    resjobj.Add("result", JToken.FromObject("ok"));
                                    resjobj.Add("execDt", JToken.FromObject(execDt));
                                    resjobj.Add("numcount", JToken.FromObject(recordCount));
                                }
                                else
                                {

                                    wherestr = "select t1.order_id,t1.dev_no,t2.usr_name,t1.dev_Ip,t2.phone_no,t3.Dept_Name,t2.dept_ID,t1.order_Price_Name,t1.order_Price_Type,t1.order_money,t1.order_create_date from tab_tran_info t1 LEFT JOIN tab_user_info t2 on t1.card_no = t2.card_no LEFT JOIN tbdeptinfo t3  on t2.dept_ID = t3.Dept_Id ";
                                    execDt = MysqlHelper.ToDataTablePager(out recordCount, wherestr, page, 10);
                                    resjobj.Add("result", JToken.FromObject("ok"));
                                    resjobj.Add("execDt", JToken.FromObject(execDt));
                                    resjobj.Add("numcount", JToken.FromObject(recordCount));
                                }
                                break;
                            #endregion
                            #region 订餐未就餐页面加载
                            case "Order_Not_load":
                                string beg_not = jobject(jobj, "beg_not");
                                string end_not = jobject(jobj, "end_not");
                                string name_not = jobject(jobj, "name_not");
                                string dept_not = jobject(jobj, "dept_not");
                                string price_type_not = jobject(jobj, "price_type_not");
                                string spe_type_not = jobject(jobj, "spe_type_not");
                                string time_str = "";
                                if (beg_not != "" && end_not != "")
                                {
                                    wherestr += "and b.order_date between '" + beg_not + "' and '" + end_not + "'";
                                    time_str = "and order_date between '" + beg_not + "'and '" + end_not + "'";
                                }
                                if (name_not != "")
                                {
                                    wherestr += "and c.usr_name like '%" + name_not + "%'";
                                }
                                if (dept_not != "")
                                {
                                    wherestr += "and t3.Dept_Name like '%" + dept_not + "%'";
                                }
                                if (price_type_not != "")
                                {
                                    wherestr += "and b.order_Price_Type ='" + price_type_not + "'";
                                }
                                if (spe_type_not != "")
                                {
                                    wherestr += "and (case when b.order_Price_Type ='早餐' and order_type='0' then '工作餐' when b.order_Price_Type ='早餐' and order_type ='1' then '自助餐' when b.order_Price_Type ='午餐' and order_type ='0' then '工作餐' when b.order_Price_Type ='午餐' and order_type ='1' then '自助餐' when b.order_Price_Type ='晚餐' and order_type ='0' then '工作餐' when b.order_Price_Type ='晚餐' and order_type ='1' then '自助餐' ELSE '其他' end) ='" + spe_type_not + "'";
                                }
                                if (btn_type != "")//导出订餐未就餐结算表
                                {
                                    var titHeader = "日期|账号|卡号|部门名称|餐次|订餐未就餐数量|餐类";
                                    if (wherestr != "" && beg_not != "" && end_not != "")
                                    {

                                        check_str = "select left(b.order_date,10) order_date, b.usr_no, b.card_no , c.dept_ID , b.order_Price_Type,count(*) order_not_count,(case when b.order_Price_Type ='早餐' and order_type='0' then '工作餐' when b.order_Price_Type ='早餐' and order_type ='1' then '自助餐' when b.order_Price_Type ='午餐' and order_type ='0' then '工作餐' when b.order_Price_Type ='午餐' and order_type ='1' then '自助餐' when b.order_Price_Type ='晚餐' and order_type ='0' then '工作餐' when b.order_Price_Type ='晚餐' and order_type ='1' then '自助餐' ELSE '其他' end) meal_types from tab_order_info b LEFT JOIN  tab_user_info c on b.card_no = c.card_no where 1=1 " + wherestr + " and not EXISTS(select * from tab_tran_info where 1=1 " + time_str + " and order_Price_Name ='定额消费' and b.card_no =card_no and b.order_Price_Type = order_Price_Type and left(b.order_date,10) =left(order_date,10)) GROUP BY b.usr_no,b.card_no,b.order_Price_Type,b.order_date,b.order_type,c.dept_ID";
                                        execDt = MysqlHelper.ExecuteDataTable(check_str);
                                        var fileName = "订餐未就餐结算表（" + DateTime.Now.ToString("yyyy-MM-ddHHmmss") + "）.xls";
                                        string resultd = DownLoad(fileName, execDt, titHeader);

                                        resjobj.Add("result", JToken.FromObject("ok"));
                                        resjobj.Add("files", JToken.FromObject(fileName));
                                        Result = JsonConvert.SerializeObject(resjobj);
                                        context.Response.Write(Result);
                                        context.Response.End();

                                    }
                                    else if (beg_not == "" || end_not == "" && wherestr != "")
                                    {
                                        check_str = "select left(b.order_date,10) order_date, b.usr_no, b.card_no , c.dept_ID , b.order_Price_Type,count(*) order_not_count,(case when b.order_Price_Type ='早餐' and order_type='0' then '工作餐' when b.order_Price_Type ='早餐' and order_type ='1' then '自助餐' when b.order_Price_Type ='午餐' and order_type ='0' then '工作餐' when b.order_Price_Type ='午餐' and order_type ='1' then '自助餐' when b.order_Price_Type ='晚餐' and order_type ='0' then '工作餐' when b.order_Price_Type ='晚餐' and order_type ='1' then '自助餐' ELSE '其他' end) meal_types from tab_order_info b LEFT JOIN  tab_user_info c on b.card_no = c.card_no where 1=1 " + wherestr + " and not EXISTS(select * from tab_tran_info where order_Price_Name ='定额消费' and b.card_no =card_no and b.order_Price_Type = order_Price_Type and left(b.order_date,10) =left(order_date,10)) GROUP BY b.usr_no,b.card_no,b.order_Price_Type,b.order_date,b.order_type,c.dept_ID";
                                        execDt = MysqlHelper.ExecuteDataTable(check_str);
                                        var fileName = "订餐未就餐结算表（" + DateTime.Now.ToString("yyyy-MM-ddHHmmss") + "）.xls";
                                        string resultd = DownLoad(fileName, execDt, titHeader);

                                        resjobj.Add("result", JToken.FromObject("ok"));
                                        resjobj.Add("files", JToken.FromObject(fileName));
                                        Result = JsonConvert.SerializeObject(resjobj);
                                        context.Response.Write(Result);
                                        context.Response.End();

                                    }
                                    else //无条件的
                                    {


                                        wherestr = @"select left(b.order_date,10) order_date, b.usr_no, b.card_no , c.dept_ID , b.order_Price_Type,count(*) order_not_count,(case when b.order_Price_Type ='早餐' and order_type='0' then '工作餐' when b.order_Price_Type ='早餐' and order_type ='1' then '自助餐' when b.order_Price_Type ='午餐' and order_type ='0' then '工作餐' when b.order_Price_Type ='午餐' and order_type ='1' then '自助餐' when b.order_Price_Type ='晚餐' and order_type ='0' then '工作餐' when b.order_Price_Type ='晚餐' and order_type ='1' then '自助餐' ELSE '其他' end) meal_types from tab_order_info b LEFT JOIN  tab_user_info c on b.card_no = c.card_no where not EXISTS(select * from tab_tran_info where order_Price_Name ='定额消费' and b.card_no =card_no and b.order_Price_Type = order_Price_Type and left(b.order_date,10) =left(order_date,10)) GROUP BY b.usr_no,b.card_no,b.order_Price_Type,b.order_date,b.order_type,c.dept_ID";
                                        execDt = MysqlHelper.ExecuteDataTable(wherestr);
                                        var fileName = "订餐未就餐结算表（" + DateTime.Now.ToString("yyyy-MM-ddHHmmss") + "）.xls";
                                        string resultd = DownLoad(fileName, execDt, titHeader);

                                        resjobj.Add("result", JToken.FromObject("ok"));
                                        resjobj.Add("files", JToken.FromObject(fileName));
                                        Result = JsonConvert.SerializeObject(resjobj);
                                        context.Response.Write(Result);
                                        context.Response.End();
                                    }
                                }
                                if (wherestr != "" && beg_not != "" && end_not != "")
                                {
                                    check_str = "select left(b.order_date,10) order_date, b.usr_no, b.card_no ,c.usr_name,c.dept_ID ,t3.Dept_Name, b.order_Price_Type,count(*) order_not_count,(case when b.order_Price_Type ='早餐' and order_type='0' then '工作餐' when b.order_Price_Type ='早餐' and order_type ='1' then '自助餐' when b.order_Price_Type ='午餐' and order_type ='0' then '工作餐' when b.order_Price_Type ='午餐' and order_type ='1' then '自助餐' when b.order_Price_Type ='晚餐' and order_type ='0' then '工作餐' when b.order_Price_Type ='晚餐' and order_type ='1' then '自助餐' ELSE '其他' end) meal_types from tab_order_info b LEFT JOIN  tab_user_info c on b.card_no = c.card_no LEFT JOIN tbdeptinfo t3 on c.dept_ID = t3.Dept_Id where 1=1 " + wherestr + " and not EXISTS(select * from tab_tran_info where 1=1 " + time_str + " and order_Price_Name ='定额消费' and b.card_no =card_no and b.order_Price_Type = order_Price_Type and left(b.order_date,10) =left(order_date,10)) GROUP BY b.usr_no,b.card_no,b.order_Price_Type,b.order_date,b.order_type,c.dept_ID";
                                    execDt = MysqlHelper.ToDataTablePager(out recordCount, check_str, page, 10);
                                    resjobj.Add("result", JToken.FromObject("ok"));
                                    resjobj.Add("execDt", JToken.FromObject(execDt));
                                    resjobj.Add("numcount", JToken.FromObject(recordCount));

                                }
                                if (beg_not == "" || end_not == "" && wherestr != "")
                                {
                                    check_str = "select left(b.order_date,10) order_date, b.usr_no, b.card_no ,c.usr_name,c.dept_ID ,t3.Dept_Name, b.order_Price_Type,count(*) order_not_count,(case when b.order_Price_Type ='早餐' and order_type='0' then '工作餐' when b.order_Price_Type ='早餐' and order_type ='1' then '自助餐' when b.order_Price_Type ='午餐' and order_type ='0' then '工作餐' when b.order_Price_Type ='午餐' and order_type ='1' then '自助餐' when b.order_Price_Type ='晚餐' and order_type ='0' then '工作餐' when b.order_Price_Type ='晚餐' and order_type ='1' then '自助餐' ELSE '其他' end) meal_types from tab_order_info b LEFT JOIN  tab_user_info c on b.card_no = c.card_no LEFT JOIN tbdeptinfo t3 on c.dept_ID = t3.Dept_Id where 1=1 " + wherestr + " and not EXISTS(select * from tab_tran_info where order_Price_Name ='定额消费' and b.card_no =card_no and b.order_Price_Type = order_Price_Type and left(b.order_date,10) =left(order_date,10)) GROUP BY b.usr_no,b.card_no,b.order_Price_Type,b.order_date,b.order_type,c.dept_ID";
                                    execDt = MysqlHelper.ToDataTablePager(out recordCount, check_str, page, 10);
                                    resjobj.Add("result", JToken.FromObject("ok"));
                                    resjobj.Add("execDt", JToken.FromObject(execDt));
                                    resjobj.Add("numcount", JToken.FromObject(recordCount));

                                }
                                else if (wherestr == "")
                                {


                                    wherestr = @"select left(b.order_date,10) order_date, b.usr_no, b.card_no ,c.usr_name,c.dept_ID ,t3.Dept_Name, b.order_Price_Type,count(*) order_not_count,(case when b.order_Price_Type ='早餐' and order_type='0' then '工作餐' when b.order_Price_Type ='早餐' and order_type ='1' then '自助餐' when b.order_Price_Type ='午餐' and order_type ='0' then '工作餐' when b.order_Price_Type ='午餐' and order_type ='1' then '自助餐' when b.order_Price_Type ='晚餐' and order_type ='0' then '工作餐' when b.order_Price_Type ='晚餐' and order_type ='1' then '自助餐' ELSE '其他' end) meal_types from tab_order_info b LEFT JOIN  tab_user_info c on b.card_no = c.card_no LEFT JOIN tbdeptinfo t3 on c.dept_ID = t3.Dept_Id where not EXISTS(select * from tab_tran_info where order_Price_Name ='定额消费' and b.card_no =card_no and b.order_Price_Type = order_Price_Type and left(b.order_date,10) =left(order_date,10)) GROUP BY b.usr_no,b.card_no,b.order_Price_Type,b.order_date,b.order_type,c.dept_ID";
                                    execDt = MysqlHelper.ToDataTablePager(out recordCount, wherestr, page, 10);
                                    resjobj.Add("result", JToken.FromObject("ok"));
                                    resjobj.Add("execDt", JToken.FromObject(execDt));
                                    resjobj.Add("numcount", JToken.FromObject(recordCount));
                                }
                                break;
                            #endregion

                            #region 订餐就餐结算表加载
                            case "Order_load":
                                string beg_order = jobject(jobj, "beg_order");
                                string end_order = jobject(jobj, "end_order");
                                string name_order = jobject(jobj, "name_order");
                                string dept_order = jobject(jobj, "dept_order");
                                if (beg_order != "" && end_order != "")
                                {
                                    wherestr += "and left(t1.order_date,10) between '" + beg_order + "'and '" + end_order + "'";
                                }
                                if (name_order != "")
                                {
                                    wherestr += "and t2.usr_name like '%" + name_order + "%'";
                                }
                                if (dept_order != "")
                                {
                                    wherestr += "and t4.dept_Name like '%" + dept_order + "%'";
                                }
                                if (btn_type != "")
                                {
                                    var titHeader = "日期|姓名|卡号|部门编号|早餐-订餐|早餐-就餐|午餐-订餐|午餐-就餐|晚餐-订餐|晚餐-就餐";
                                    if (wherestr != "")
                                    {
                                        //check_str = "SELECT left(t1.order_date,10) order_date,t1.usr_no,t1.card_no,t2.dept_ID,count(case when t1.order_Price_Type ='早餐'  then 1 ELSE NULL end) bra_order,COUNT(case when t3.order_Price_Type ='早餐' then 1 else Null end) bra_jiu,count(case when t1.order_Price_Type ='午餐'  then 1 ELSE NULL end) lun_order,COUNT(case when t3.order_Price_Type ='午餐' then 1 else Null end) lun_jiu,count(case when t1.order_Price_Type ='晚餐'  then 1 ELSE NULL end) din_order,COUNT(case when t3.order_Price_Type ='晚餐' then 1 else Null end) din_jiu from tab_order_info t1 LEFT JOIN  tab_user_info t2 on t1.card_no = t2.card_no LEFT JOIN tab_tran_info t3 ON t1.card_no = t3.card_no and t1.order_Price_type = t3.order_price_Type and left(t1.order_date,10) = left(t3.order_date,10) where 1=1 " + wherestr + " GROUP BY t1.order_date DESC,t1.usr_no,t1.card_no,t2.dept_ID ";
                                        check_str = "select left(t1.order_date,10) order_date,t2.usr_name,t2.card_no,t2.dept_ID,t1.早餐订餐 as bra_order,t3.早餐就餐 as bra_jiu,t1.午餐订餐 as lun_order,t3.午餐就餐 as lun_jiu,t1.晚餐订餐 din_order,t3.晚餐就餐 as din_jiu from (select count(case when order_Price_Type ='早餐'  then 1 ELSE NULL end) 早餐订餐,count(case when order_Price_Type ='午餐'  then 1 ELSE NULL end) 午餐订餐,count(case when order_Price_Type ='晚餐'  then 1 ELSE NULL end) 晚餐订餐,order_Price_Type,card_no,order_date from tab_order_info) as t1,(select * from tab_user_info) as t2,(select COUNT(case when order_Price_Type ='早餐' then 1 else Null end) 早餐就餐,COUNT(case when order_Price_Type ='午餐' then 1 else Null end) 午餐就餐,COUNT(case when order_Price_Type ='晚餐' then 1 else Null end) 晚餐就餐,order_Price_Type,card_no,order_date from tab_tran_info ) as t3 WHERE t1.card_no = t2.card_no and t2.card_no =t3.card_no and t1.order_Price_type = t3.order_price_Type and left(t1.order_date,10) = left(t3.order_date,10) where 1=1 " + wherestr + " ORDER BY t1.order_date desc";
                                        execDt = MysqlHelper.ExecuteDataTable(check_str);
                                        var fileName = "订餐就餐结算表（" + DateTime.Now.ToString("yyyy-MM-ddHHmmss") + "）.xls";
                                        string resultd = DownLoad(fileName, execDt, titHeader);
                                        resjobj.Add("result", JToken.FromObject("ok"));
                                        resjobj.Add("files", JToken.FromObject(fileName));
                                        Result = JsonConvert.SerializeObject(resjobj);
                                        context.Response.Write(Result);
                                        context.Response.End();
                                    }
                                    else
                                    {
                                        string test = System.Configuration.ConfigurationManager.AppSettings["order_load"].ToString();

                                        wherestr = "SELECT left(t1.order_date,10) order_date,t1.usr_no,t1.card_no,t2.dept_ID,count(case when t1.order_Price_Type ='早餐'  then 1 ELSE NULL end) bra_order,COUNT(case when t3.order_Price_Type ='早餐' then 1 else Null end) bra_jiu,count(case when t1.order_Price_Type ='午餐'  then 1 ELSE NULL end) lun_order,COUNT(case when t3.order_Price_Type ='午餐' then 1 else Null end) lun_jiu,count(case when t1.order_Price_Type ='晚餐'  then 1 ELSE NULL end) din_order,COUNT(case when t3.order_Price_Type ='晚餐' then 1 else Null end) din_jiu from tab_order_info t1 LEFT JOIN  tab_user_info t2 on t1.card_no = t2.card_no LEFT JOIN tab_tran_info t3 ON t1.card_no = t3.card_no and t1.order_Price_type = t3.order_price_Type and left(t1.order_date,10) = left(t3.order_date,10) GROUP BY t1.order_date DESC,t1.usr_no,t1.card_no,t2.dept_ID ";
                                        wherestr = @"select 
left(t1.order_date,10) order_date,t2.usr_name,t2.card_no,t2.dept_ID,t1.早餐订餐 as bra_order,t3.早餐就餐 as bra_jiu,t1.午餐订餐 as lun_order,t3.午餐就餐 as lun_jiu,t1.晚餐订餐 din_order,t3.晚餐就餐 as din_jiu from 
(select
count(case when order_Price_Type ='早餐'  then 1 ELSE NULL end) 早餐订餐,
 count(case when order_Price_Type ='午餐'  then 1 ELSE NULL end) 午餐订餐,
 count(case when order_Price_Type ='晚餐'  then 1 ELSE NULL end) 晚餐订餐,order_Price_Type,card_no,order_date from tab_order_info) as t1,
(select * from tab_user_info) as t2,
(select COUNT(case when order_Price_Type ='早餐' then 1 else Null end) 早餐就餐,
COUNT(case when order_Price_Type ='午餐' then 1 else Null end) 午餐就餐,
COUNT(case when order_Price_Type ='晚餐' then 1 else Null end) 晚餐就餐,order_Price_Type,card_no,order_date from tab_tran_info ) as t3
WHERE t1.card_no = t2.card_no and t2.card_no =t3.card_no and t1.order_Price_type = t3.order_price_Type and left(t1.order_date,10) = left(t3.order_date,10)
ORDER BY t1.order_date desc";
                                        wherestr = test;
                                        execDt = MysqlHelper.ExecuteDataTable(wherestr);
                                        var fileName = "订餐就餐结算表（" + DateTime.Now.ToString("yyyy-MM-ddHHmmss") + "）.xls";
                                        string resultd = DownLoad(fileName, execDt, titHeader);
                                        resjobj.Add("result", JToken.FromObject("ok"));
                                        resjobj.Add("files", JToken.FromObject(fileName));
                                        Result = JsonConvert.SerializeObject(resjobj);
                                        context.Response.Write(Result);
                                        context.Response.End();
                                    }
                                }
                                if (wherestr != "")
                                {
                                    //check_str = "SELECT left(t1.order_date,10) order_date,t1.usr_no,t1.card_no,t2.dept_ID,count(case when t1.order_Price_Type ='早餐'  then 1 ELSE NULL end) bra_order,COUNT(case when t3.order_Price_Type ='早餐' then 1 else Null end) bra_jiu,count(case when t1.order_Price_Type ='午餐'  then 1 ELSE NULL end) lun_order,COUNT(case when t3.order_Price_Type ='午餐' then 1 else Null end) lun_jiu,count(case when t1.order_Price_Type ='晚餐'  then 1 ELSE NULL end) din_order,COUNT(case when t3.order_Price_Type ='晚餐' then 1 else Null end) din_jiu from tab_order_info t1 LEFT JOIN  tab_user_info t2 on t1.card_no = t2.card_no LEFT JOIN tab_tran_info t3 ON t1.card_no = t3.card_no and t1.order_Price_type = t3.order_price_Type and left(t1.order_date,10) = left(t3.order_date,10) where 1=1 " + wherestr + " GROUP BY t1.order_date DESC,t1.usr_no,t1.card_no,t2.dept_ID ";
                                    //check_str = "select left(t1.order_date,10) order_date,t2.usr_name,t2.card_no,t2.dept_ID,t1.早餐订餐 as bra_order,t3.早餐就餐 as bra_jiu,t1.午餐订餐 as lun_order,t3.午餐就餐 as lun_jiu,t1.晚餐订餐 din_order,t3.晚餐就餐 as din_jiu from (select count(case when order_Price_Type ='早餐'  then 1 ELSE NULL end) 早餐订餐,count(case when order_Price_Type ='午餐'  then 1 ELSE NULL end) 午餐订餐,count(case when order_Price_Type ='晚餐'  then 1 ELSE NULL end) 晚餐订餐,order_Price_Type,card_no,order_date from tab_order_info) as t1,(select * from tab_user_info) as t2,(select COUNT(case when order_Price_Type ='早餐' then 1 else Null end) 早餐就餐,COUNT(case when order_Price_Type ='午餐' then 1 else Null end) 午餐就餐,COUNT(case when order_Price_Type ='晚餐' then 1 else Null end) 晚餐就餐,order_Price_Type,card_no,order_date from tab_tran_info ) as t3 WHERE t1.card_no = t2.card_no and t2.card_no =t3.card_no and t1.order_Price_type = t3.order_price_Type and left(t1.order_date,10) = left(t3.order_date,10) where 1=1 " + wherestr + " ORDER BY t1.order_date desc";
                                    check_str = "select left(t1.order_date,10) order_date,t2.usr_name,t2.card_no,t4.dept_Name,t1.早餐订餐 as bra_order,t3.早餐就餐 as bra_jiu,t1.午餐订餐 as lun_order,t3.午餐就餐 as lun_jiu,t1.晚餐订餐 din_order,t3.晚餐就餐 as din_jiu from (select count(case when order_Price_Type = '早餐'  then 1 ELSE NULL end) 早餐订餐,count(case when order_Price_Type = '午餐'  then 1 ELSE NULL end) 午餐订餐,count(case when order_Price_Type = '晚餐'  then 1 ELSE NULL end) 晚餐订餐,order_Price_Type,card_no,order_date from tab_order_info GROUP BY order_Price_Type, card_no, order_date ) as t1,(select * from tab_user_info) as t2,(select COUNT(case when order_Price_Type = '早餐' then 1 else Null end) 早餐就餐,COUNT(case when order_Price_Type = '午餐' then 1 else Null end) 午餐就餐,COUNT(case when order_Price_Type = '晚餐' then 1 else Null end) 晚餐就餐,order_Price_Type,card_no,order_date from tab_tran_info GROUP BY order_Price_Type, card_no, order_date  ) as t3,tbdeptinfo as t4 WHERE 1 = 1 "+wherestr+" and t1.card_no = t2.card_no and t2.card_no = t3.card_no and t2.dept_ID = t4.dept_Id and t1.order_Price_type = t3.order_price_Type and left(t1.order_date,10) = left(t3.order_date, 10) ORDER BY t1.order_date desc ";
                                    execDt = MysqlHelper.ToDataTablePager(out recordCount, check_str, page, 10);
                                    resjobj.Add("result", JToken.FromObject("ok"));
                                    resjobj.Add("execDt", JToken.FromObject(execDt));
                                    resjobj.Add("numcount", JToken.FromObject(recordCount));
                                }
                                else
                                {


                                    //wherestr = "SELECT left(t1.order_date,10) order_date,t1.usr_no,t1.card_no,t2.dept_ID,count(case when t1.order_Price_Type ='早餐'  then 1 ELSE NULL end) bra_order,COUNT(case when t3.order_Price_Type ='早餐' then 1 else Null end) bra_jiu,count(case when t1.order_Price_Type ='午餐'  then 1 ELSE NULL end) lun_order,COUNT(case when t3.order_Price_Type ='午餐' then 1 else Null end) lun_jiu,count(case when t1.order_Price_Type ='晚餐'  then 1 ELSE NULL end) din_order,COUNT(case when t3.order_Price_Type ='晚餐' then 1 else Null end) din_jiu from tab_order_info t1 LEFT JOIN  tab_user_info t2 on t1.card_no = t2.card_no LEFT JOIN tab_tran_info t3 ON t1.card_no = t3.card_no and t1.order_Price_type = t3.order_price_Type and left(t1.order_date,10) = left(t3.order_date,10) GROUP BY t1.order_date DESC,t1.usr_no,t1.card_no,t2.dept_ID ";
                                    //                                    wherestr = @"select 
                                    //left(t1.order_date,10) order_date,t2.usr_name,t2.card_no,t2.dept_ID,t1.早餐订餐 as bra_order,t3.早餐就餐 as bra_jiu,t1.午餐订餐 as lun_order,t3.午餐就餐 as lun_jiu,t1.晚餐订餐 din_order,t3.晚餐就餐 as din_jiu from 
                                    //(select
                                    //count(case when order_Price_Type ='早餐'  then 1 ELSE NULL end) 早餐订餐,
                                    // count(case when order_Price_Type ='午餐'  then 1 ELSE NULL end) 午餐订餐,
                                    // count(case when order_Price_Type ='晚餐'  then 1 ELSE NULL end) 晚餐订餐,order_Price_Type,card_no,order_date from tab_order_info) as t1,
                                    //(select * from tab_user_info) as t2,
                                    //(select COUNT(case when order_Price_Type ='早餐' then 1 else Null end) 早餐就餐,
                                    //COUNT(case when order_Price_Type ='午餐' then 1 else Null end) 午餐就餐,
                                    //COUNT(case when order_Price_Type ='晚餐' then 1 else Null end) 晚餐就餐,order_Price_Type,card_no,order_date from tab_tran_info ) as t3
                                    //WHERE t1.card_no = t2.card_no and t2.card_no =t3.card_no and t1.order_Price_type = t3.order_price_Type and left(t1.order_date,10) = left(t3.order_date,10)
                                    //ORDER BY t1.order_date desc";
                                    string test = System.Configuration.ConfigurationManager.AppSettings["order_load"].ToString();
                                    wherestr = test;
                                    execDt = MysqlHelper.ToDataTablePager(out recordCount, wherestr, page, 10);
                                    resjobj.Add("result", JToken.FromObject("ok"));
                                    resjobj.Add("execDt", JToken.FromObject(execDt));
                                    resjobj.Add("numcount", JToken.FromObject(recordCount));
                                }
                                break;
                            #endregion
                            #endregion


                            default:
                                resjobj.Add("result", JToken.FromObject("error"));
                                resjobj.Add("msg", JToken.FromObject("无权限访问"));
                                break;
                        }

                    }
                    else
                    {

                    }
                }

            }
            catch (Exception e)
            {
                resjobj.Add("result", JToken.FromObject("exception"));
                resjobj.Add("msg", JToken.FromObject("异常：" + e.Message.ToString()));
            }
            Result = JsonConvert.SerializeObject(resjobj);
            context.Response.Write(Result);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        //后台通用方法
        #region
        /// <summary>
        /// 这个也是转换为json不过是用系统的
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string DataTableToJson(DataTable dt)
        {
            string result = "";
            result = JsonConvert.SerializeObject(dt, new DataTableConverter());
            //new DataTableConverter()一般是固定的
            return result;
        }
        /// <summary>     
        /// dataTable转换成Json格式     
        /// </summary>     
        /// <param name="dt"></param>     
        /// <returns></returns>     
        public static string tableToJson(DataTable dt, string tablename)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("\"");
            jsonBuilder.Append(tablename);
            jsonBuilder.Append("\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    jsonBuilder.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        jsonBuilder.Append("\"");
                        jsonBuilder.Append(dt.Columns[j].ColumnName.ToLower());
                        jsonBuilder.Append("\":\"");
                        if (dt.Columns[j].DataType == typeof(DateTime))
                        {
                            String v = dt.Rows[i][j].ToString().Trim();
                            if (v != "")
                            {
                                DateTime d = DateTime.Now;
                                DateTime.TryParse(v, out d);
                                v = d.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            jsonBuilder.Append(v);
                        }
                        else
                        {
                            String v = dt.Rows[i][j].ToString().Trim();
                            v = v.Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "").Replace("\\", "\\\\");
                            jsonBuilder.Append(v);
                        }
                        jsonBuilder.Append("\",");
                    }

                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                    jsonBuilder.Append("},");
                }
            }
            else
            {
                jsonBuilder.Append(",");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);

            jsonBuilder.Append("]");
            //jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        /// <summary>
        /// 获取数据流
        /// </summary>
        /// <returns></returns>
        public string getpost()
        {
            string g = "";
            if (HttpContext.Current.Request.InputStream != null)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(HttpContext.Current.Request.InputStream, System.Text.Encoding.UTF8);
                g = sr.ReadToEnd();
            }
            return g;
        }
        /// <summary>
        /// 从jobject中获取相应的数据 
        /// </summary>
        /// <param name="jobj">jobject对象</param>
        /// <param name="key">要获取的值</param>
        /// <returns></returns>
        public string jobject(JObject jobj, string key)
        {
            string hh = "";
            if (jobj[key] != null)
            {
                hh = jobj[key].ToString().Trim();
            }
            return hh;
        }


        public object jobject02(JObject jobj, string key)
        {
            object hh = "";
            if (jobj[key] != null)
            {
                hh = jobj[key];
            }
            return hh;
        }

        public JObject jobject03(JObject jobj, string key)
        {
            JObject hh;
            hh = (JObject)jobj[key];
            return hh;
        }

        public static string DownLoad(string fileName, DataTable dt, string title)
        {
            var strPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"download\" + fileName);
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\download"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\download");
            }
            string resultd = MySqlDB.DateExcel.DataTableToExcel_Count(strPath, dt, title);
            return resultd;
        }
        #endregion
    }
}