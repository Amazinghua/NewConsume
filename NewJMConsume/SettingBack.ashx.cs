using MySql.Data.MySqlClient;
using MySqlDB;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace NewJMConsume
{
    /// <summary>
    /// SettingBack 的摘要说明
    /// </summary>
    public class SettingBack : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
             context.Response.ContentType = "text/plain";
            string Result = "";
            string types = "";
            bool isExec = false;//是否成功成执行数据库
            int execNum = 0;//执行数据库，受影响行数
            string execStr = "";//执行数据库，返回的统计结果
            DataTable execDt;//执行数据库，返回的datatable
            int recordCount = 0;//数据总量
            string wherestr = "";//where语句
            JObject resjobj = new JObject();
            try
            {
                string poststr = getpost();
                if (!string.IsNullOrEmpty(poststr))
                {
                    JObject jobj = JObject.Parse(poststr);
                    types = jobject(jobj, "type");//执行哪条命令

                    string user = jobject(jobj, "user");//账户
                    string password = jobject(jobj, "psw");//密码
                    string role = jobject(jobj, "role");//角色名字
                    string remark = jobject(jobj, "remark");//备注
                    string company = jobject(jobj, "company");//所属公司
                    string keyword = jobject(jobj, "keyword");//关键字
                    string status = jobject(jobj, "status");//是否可用
                    string pc = jobject(jobj, "pc");//当前页数
                    int page = pc != "" ? int.Parse(pc) : 0;//当前页数
                    string table = jobject(jobj, "table");//数据库表的名字
                    string id = jobject(jobj, "id");//id
                    string feeplace = jobject(jobj, "feeplace");//就餐地点
                    string eattype = jobject(jobj, "eattype");//就餐餐次
                    string feeresource = jobject(jobj, "feeresource");//消费策略
                    string level = jobject(jobj, "level");//消费策略
                    string dept = jobject(jobj, "dept");//部门名称
                    string usrno = jobject(jobj, "usrno");//用户帐号
                    string usrname = jobject(jobj, "usrname");//用户姓名
                    string card = jobject(jobj, "card");//员工卡号
                    string phone = jobject(jobj, "phone");//员工号码
                    string usr_state = jobject(jobj, "usr_state");
                    string memo = jobject(jobj, "memo");//备注
                    string card_state = jobject(jobj, "card_state");
                    string usrrole = jobject(jobj, "usrrole");//更新用户角色

                    string btn_type = jobject(jobj, "btn_type");//导出标识
                    string check_str = "";
                    if (types != "")
                    {
                        switch (types)
                        {

                            #region//角色设置的页面
                            case "set_role_addrole"://添加角色
                                #region
                                execStr = MysqlHelper.ExecuteScalar("select count(*) from tbrole where RoName = '" + role +"'").ToString();
                                if (execStr == "0")
                                {
                                    string str_role = "";//循环生成的insert语句
                                    string role_val = jobject(jobj, "role_val");
                                    role_val = role_val.Substring(1, role_val.Length - 1);
                                    role_val = role_val.Remove(role_val.Length - 1, 1);
                                    string[] role_array = role_val.Trim().Split(',');
                                    for (int count =0;count < role_array.Length; count++)
                                    {
                                        str_role += "insert into tab_role_val (Create_time,RoName,Visual) values (now(),'" + role + "'," + role_array[count] + ");";
                                    }
                                    int execNum0 = MysqlHelper.ExecuteNonQuery(str_role);//将insert语句插入权限表中
                                    execNum = MysqlHelper.ExecuteNonQuery("insert into tbrole (RoName,RoRemark,RoIsModify,RoIsValid,RoNo,Company) values ('" + role + "','" + remark + "','0','1','0','" + company + "')");
                                    if (execNum == 1)
                                    {
                                        resjobj.Add("result", JToken.FromObject("success"));
                                        resjobj.Add("msg", JToken.FromObject("添加成功"));
                                    }
                                    else
                                    {
                                        resjobj.Add("result", JToken.FromObject("fail"));
                                        resjobj.Add("msg", JToken.FromObject("添加失败"));
                                    }
                                }
                                else
                                {
                                    resjobj.Add("result", JToken.FromObject("fail"));
                                    resjobj.Add("msg", JToken.FromObject("添加失败，已有相同角色名称！"));
                                }
                                #endregion
                                break;

                            case "set_role_load"://加载页面
                                #region
                                if (status != "")
                                {
                                    wherestr += "and RoIsValid = '" + status + "'";
                                }
                                if (company != "")
                                {
                                    wherestr += "and Company = '" + company + "'";
                                }
                                if (keyword != "")
                                {
                                    wherestr += "and (RoName  like '%" + keyword + "%' or RoRemark like '%" + keyword + "%' or Company like '%" + keyword + "%')";
                                }
                                execDt = MysqlHelper.getPager(out recordCount, "", "tbrole", wherestr, "RoId", page, 10);
                                resjobj.Add("result", JToken.FromObject("ok"));
                                resjobj.Add("execDt", JToken.FromObject(execDt));
                                resjobj.Add("numcount", JToken.FromObject(recordCount));
                                #endregion
                                break;

                            case "set_role_update"://更新状态
                                #region
                                string nowstatus = status == "1" ? "0" : "1";
                                execNum = MysqlHelper.ExecuteNonQuery("update tbrole set RoIsValid = '" + nowstatus + "' where RoId = '" + id + "'");
                                if (execNum == 1)
                                {
                                    resjobj.Add("result", JToken.FromObject("success"));
                                    resjobj.Add("msg", JToken.FromObject("添加成功"));
                                }
                                else
                                {
                                    resjobj.Add("result", JToken.FromObject("fail"));
                                    resjobj.Add("msg", JToken.FromObject("添加失败"));
                                }
                                #endregion
                                break;

                            #endregion

                            #region//饭堂设置的页面
                            case "set_cant_load"://加载页面
                                #region
                                if (feeplace != "")
                                {
                                    wherestr += " and PlaceType = '" + feeplace + "'";
                                }
                                if (id != "")
                                {
                                    wherestr += " and FeeID = '" + id + "'";
                                }
                                execDt = MysqlHelper.getPager(out recordCount, "", "tk_resource_place", wherestr, "FeeID", page, 10);
                                resjobj.Add("result", JToken.FromObject("ok"));
                                resjobj.Add("execDt", JToken.FromObject(execDt));
                                resjobj.Add("numcount", JToken.FromObject(recordCount));
                                #endregion
                                break;

                            case "set_cant_add"://添加食堂
                                #region

                                execStr = MysqlHelper.ExecuteScalar("select count(*) from tk_resource_place where PlaceType = '" + feeplace + "' and Company = '" + company + "'").ToString();
                                if (execStr == "0")
                                {
                                    execNum = MysqlHelper.ExecuteNonQuery("insert into tk_resource_place (FeeResource,ResourceName,order_price_type,PID,PlaceType,Company) values ('" + feeresource + "','" + feeresource + "','" + eattype + "','5','" + feeplace + "','" + company + "')");
                                    if (execNum == 1)
                                    {
                                        resjobj.Add("result", JToken.FromObject("success"));
                                        resjobj.Add("msg", JToken.FromObject("添加成功"));
                                    }
                                    else
                                    {
                                        resjobj.Add("result", JToken.FromObject("fail"));
                                        resjobj.Add("msg", JToken.FromObject("添加失败"));
                                    }
                                }
                                else
                                {
                                    resjobj.Add("result", JToken.FromObject("fail"));
                                    resjobj.Add("msg", JToken.FromObject("添加失败，此消费地点已存在！"));
                                }
                                #endregion
                                break;

                            case "set_cant_delete"://删除食堂
                                #region
                                execStr = MysqlHelper.ExecuteNonQuery("delete from tk_resource_place where FeeID = '" + id + "'").ToString();
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
                                #endregion
                                break;

                            case "set_cant_update"://更新饭堂
                                #region
                                execNum = MysqlHelper.ExecuteNonQuery("update tk_resource_place set order_price_type = '" + eattype + "' ,PlaceType = '" + feeplace + "',ResourceName = '" + feeresource + "' ,Company = '" + company + "'where FeeID = '" + id + "'");
                                if (execNum == 1)
                                {
                                    resjobj.Add("result", JToken.FromObject("success"));
                                    resjobj.Add("msg", JToken.FromObject("更新成功！"));
                                }
                                else
                                {
                                    resjobj.Add("result", JToken.FromObject("fail"));
                                    resjobj.Add("msg", JToken.FromObject("更新失败！"));
                                }
                                #endregion
                                break;
                            #endregion

                            #region//部门设置
                            case "set_dep_load"://加载页面
                                #region
                                //if (level != "")
                                //{
                                //    wherestr += " and Dept_Level = '" + level + "'";
                                //}
                                if (id != "")
                                {
                                    wherestr += " and dept_ID = '" + id + "'";
                                }
                                //check_str = " select b.Dept_Id,b.Dept_Name as parent_Name,b.Dept_Level as parent_Level,a.Dept_Id as sub_id,a.Dept_Name,a.Dept_Level as sub_Level,a.FeeResource,a.PlaceType from TbDeptInfo a,(select * from TbDeptInfo) as b where 1=1 and a.Dept_up =b.Dept_Id "+wherestr;
                                check_str = "select * from(select a.dept_ID,a.ust_ID,a.Company,a.usr_no,a.usr_name,a.card_no,b.Dept_Name,a.phone_no from tab_user_info a,tbdeptinfo b where a.dept_ID =b.Dept_Id ) as tab_all_user where 1 = 1" + wherestr;
                                execDt = MysqlHelper.ToDataTablePager(out recordCount, check_str, page, 10);
                                //execDt = MysqlHelper.getPager(out recordCount, "", "tbdeptinfo", wherestr, "Dept_Id", page, 10);
                                resjobj.Add("result", JToken.FromObject("ok"));
                                resjobj.Add("execDt", JToken.FromObject(execDt));
                                resjobj.Add("numcount", JToken.FromObject(recordCount));
                                resjobj.Add("numid", JToken.FromObject(id));
                                #endregion
                                break;

                            case "set_dep_delete"://删除部门
                                #region
                                execStr = MysqlHelper.ExecuteNonQuery("delete from tbdeptinfo where Dept_Id = '" + id + "'").ToString();
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
                                #endregion
                                break;

                            case "set_dep_add"://添加部门
                                #region
                                execStr = MysqlHelper.ExecuteScalar("select count(*) from tbdeptinfo where Dept_Name = '" + dept + "' and Dept_Id = '" + company + "'").ToString();//这里的company值为获取到的dept_Id值
                                if (execStr == "0")
                                {
                                    execNum = MysqlHelper.ExecuteNonQuery("insert into tbdeptinfo (Dept_Name,Dept_Level,Dept_up,dept_state,FeeResource,PlaceType) values ('" + dept  + "','2','"+company+"','1','" + feeresource + "','" + feeplace +"')");
                                    if (execNum == 1)
                                    {
                                        resjobj.Add("result", JToken.FromObject("success"));
                                        resjobj.Add("msg", JToken.FromObject("添加成功"));
                                    }
                                    else
                                    {
                                        resjobj.Add("result", JToken.FromObject("fail"));
                                        resjobj.Add("msg", JToken.FromObject("添加失败"));
                                    }
                                }
                                else
                                {
                                    resjobj.Add("result", JToken.FromObject("fail"));
                                    resjobj.Add("msg", JToken.FromObject("添加失败，此公司的部门已存在！"));
                                }
                                #endregion
                                break;

                            case "set_dep_update"://更新部门
                                #region
                                execNum = MysqlHelper.ExecuteNonQuery("update tbdeptinfo set FeeResource = '" + feeresource +"',Dept_up = '" + company + "',PlaceType ='" + feeplace + "',dept_state = '1'where Dept_Id = '" + id + "'");
                                if (execNum == 1)
                                {
                                    resjobj.Add("result", JToken.FromObject("success"));
                                    resjobj.Add("msg", JToken.FromObject("更新成功！"));
                                }
                                else
                                {
                                    resjobj.Add("result", JToken.FromObject("fail"));
                                    resjobj.Add("msg", JToken.FromObject("更新失败！"));
                                }
                                #endregion
                                break;
                            #endregion

                            #region//人员设置
                            case "set_staff_load"://加载页面
                                #region
                                if (keyword != "")
                                {
                                    wherestr += "where t1.usr_no like '%" + keyword + "%' or t1.usr_name like '%" + keyword + "%' or t1.card_no like '%" + keyword + "%'  or t1.phone_no like '%" + keyword + "%' or t2.Dept_Name like '%" + keyword + "%'or t1.Company like '%" + keyword + "%'";
                                }
                                if(btn_type != "")
                                {
                                    var titHeader = "公司|账号|姓名|部门|卡号|号码|备注|消费策略|消费地点";
                                    if(wherestr != "")
                                    {
                                        check_str = "select Company,usr_no,usr_name,dept_ID,card_no,phone_no,memo,FeeResource,FeePlace from tab_user_info where 1=1 " + wherestr;
                                        execDt = MysqlHelper.ExecuteDataTable(check_str);
                                        var fileName = "导出人员信息（" + DateTime.Now.ToString("yyyy-MM-ddHHmmss") + "）.xls";
                                        string resultd = MainBack.DownLoad(fileName, execDt, titHeader);
                                        resjobj.Add("result", JToken.FromObject("ok"));
                                        resjobj.Add("files", JToken.FromObject(fileName));
                                        Result = JsonConvert.SerializeObject(resjobj);
                                        context.Response.Write(Result);
                                        context.Response.End();
                                    }
                                    else
                                    {
                                        check_str = "select Company,usr_no,usr_name,dept_ID,card_no,phone_no,memo,FeeResource,FeePlace from tab_user_info";
                                        execDt = MysqlHelper.ExecuteDataTable(check_str);
                                        var fileName = "导出人员信息（" + DateTime.Now.ToString("yyyy-MM-ddHHmmss") + "）.xls";
                                        string resultd = MainBack.DownLoad(fileName, execDt, titHeader);
                                        resjobj.Add("result", JToken.FromObject("ok"));
                                        resjobj.Add("files", JToken.FromObject(fileName));
                                        Result = JsonConvert.SerializeObject(resjobj);
                                        context.Response.Write(Result);
                                        context.Response.End();
                                    }
                                }
                                if(wherestr != "")
                                {
                                    check_str = "select t1.ust_ID,t1.Company,t1.usr_no,t1.usr_name,t2.Dept_Name,t1.card_no,t1.card_state,t1.phone_no,t1.memo,t1.FeeResource,t1.FeePlace from tab_user_info t1 left JOIN tbdeptinfo t2 on t1.dept_ID =t2.Dept_Id  " + wherestr;
                                    execDt = MysqlHelper.ToDataTablePager(out recordCount, check_str, page, 10);
                                    resjobj.Add("result", JToken.FromObject("ok"));
                                    resjobj.Add("execDt", JToken.FromObject(execDt));
                                    resjobj.Add("numcount", JToken.FromObject(recordCount));
                                }
                                else
                                {
                                    wherestr = "select t1.ust_ID,t1.Company,t1.usr_no,t1.usr_name,t2.Dept_Name,t1.card_no,t1.card_state,t1.phone_no,t1.memo,t1.FeeResource,t1.FeePlace from tab_user_info t1 left JOIN tbdeptinfo t2 on t1.dept_ID =t2.Dept_Id ";
                                    execDt = MysqlHelper.ToDataTablePager(out recordCount, wherestr, page, 10);
                                    resjobj.Add("result", JToken.FromObject("ok"));
                                    resjobj.Add("execDt", JToken.FromObject(execDt));
                                    resjobj.Add("numcount", JToken.FromObject(recordCount));
                                }
                                //execDt = MysqlHelper.getPager(out recordCount, "", "tab_user_info", wherestr, "ust_ID", page, 10);
                                #endregion
                                break;
                            case "set_staff_delete"://删除人员
                                #region
                                execStr = MysqlHelper.ExecuteNonQuery("delete from tab_user_info where ust_ID = '" + id + "'").ToString();
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
                                #endregion
                                break;

                            case "set_staff_add"://添加人员
                                #region
                                execStr = MysqlHelper.ExecuteScalar("select count(*) from tab_user_info where usr_no = '" + usrno + "' and Company = '" + company + "'").ToString();
                                if (execStr == "0")
                                {
                                    execNum = MysqlHelper.ExecuteNonQuery("insert into tab_user_info (usr_no,usr_name,usr_RoId,usr_state,pw_password,dept_ID,card_no,card_state,card_money,phone_no,memo,FeeResource,Company,FeePlace) values ('" + usrno + "','" + usrname + "','1','1','123456','" + dept +"','" + card + "','1','0.00','" + phone + "','" + memo + "','" + feeresource + "','" + company + "','" + feeplace + "')");
                                    if (execNum == 1)
                                    {
                                        resjobj.Add("result", JToken.FromObject("success"));
                                        resjobj.Add("msg", JToken.FromObject("添加成功"));
                                    }
                                    else
                                    {
                                        resjobj.Add("result", JToken.FromObject("fail"));
                                        resjobj.Add("msg", JToken.FromObject("添加失败"));
                                    }
                                }
                                else
                                {
                                    resjobj.Add("result", JToken.FromObject("fail"));
                                    resjobj.Add("msg", JToken.FromObject("添加失败，此公司的人员已存在！"));
                                }
                                #endregion
                                break;

                            case "set_staff_update"://更新人员
                                #region
                                execNum = MysqlHelper.ExecuteNonQuery("update tab_user_info set phone_no = '" + phone + "',Company = '" + company + "',dept_ID = '" + dept + "',FeeResource = '" + feeresource + "',FeePlace = '" + feeplace + "',memo = '" + memo + "',card_state ='"+card_state +"' where ust_ID ='"+id+"'");
                                if (execNum == 1)
                                {
                                    resjobj.Add("result", JToken.FromObject("success"));
                                    resjobj.Add("msg", JToken.FromObject("更新成功！"));
                                }
                                else
                                {
                                    resjobj.Add("result", JToken.FromObject("fail"));
                                    resjobj.Add("msg", JToken.FromObject("更新失败！"));
                                }
                                #endregion
                                break;
                            case "set_staff_check"://查询单个人员基本信息
                                wherestr = "select t1.ust_ID,t1.Company,t1.usr_no,t1.usr_name,t2.Dept_Name,t1.card_no,t1.card_state,t1.phone_no,t1.memo,t1.FeeResource,t1.FeePlace from tab_user_info t1 left JOIN tbdeptinfo t2 on t1.dept_ID =t2.Dept_Id where t1.ust_ID ='"+id+"'";
                                execDt = MysqlHelper.ToDataTablePager(out recordCount, wherestr, page, 10);
                                resjobj.Add("result", JToken.FromObject("ok"));
                                resjobj.Add("execDt", JToken.FromObject(execDt));
                                resjobj.Add("numcount", JToken.FromObject(recordCount));
                                break;


                            #endregion


                            #region//消费策略设置页面
                            case "set_res_load"://加载页面
                                #region
                                if (keyword != "")
                                {
                                    wherestr += "and (ResourceName like '%" + keyword + "%' or ReMark like '%" + keyword + "%' or ResourceID like '%" + keyword + "%')";
                                }
                                if (id != "")
                                {
                                    wherestr += " and ust_ID = '" + id + "'";
                                }
                                execDt = MysqlHelper.getPager(out recordCount, "", "tab_resource_info", wherestr, "ID", page, 10);
                                resjobj.Add("result", JToken.FromObject("ok"));
                                resjobj.Add("execDt", JToken.FromObject(execDt));
                                resjobj.Add("numcount", JToken.FromObject(recordCount));
                                #endregion
                                break;

                            case "set_res_delete"://删除策略
                                #region
                                execStr = MysqlHelper.ExecuteNonQuery("delete from tab_resource_info where ID = '" + id + "'").ToString();
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
                                #endregion
                                break;

                            case "set_res_add"://添加策略
                                #region
                                execStr = MysqlHelper.ExecuteScalar("select count(*) from tab_resource_info where ResourceID = '" + usrno + "' and ResourceName = '" + usrname + "'").ToString();
                                if (execStr == "0")
                                {
                                    execNum = MysqlHelper.ExecuteNonQuery("insert into tab_resource_info (ResourceID,ResourceName,ReMark,IsModify,IsUse) values ('" + id + "','" + usrname + "','" + memo + "','1','1')");
                                    if (execNum == 1)
                                    {
                                        resjobj.Add("result", JToken.FromObject("success"));
                                        resjobj.Add("msg", JToken.FromObject("添加成功"));
                                    }
                                    else
                                    {
                                        resjobj.Add("result", JToken.FromObject("fail"));
                                        resjobj.Add("msg", JToken.FromObject("添加失败"));
                                    }
                                }
                                else
                                {
                                    resjobj.Add("result", JToken.FromObject("fail"));
                                    resjobj.Add("msg", JToken.FromObject("添加失败，此消费策略已存在！"));
                                }
                                #endregion
                                break;

                            case "set_res_det_load"://加载策略详情
                                #region
                                if (id != "")
                                {
                                    wherestr += " and ResourceID = '" + id + "'";
                                }
                                execDt = MysqlHelper.getPager(out recordCount, "", "tab_resource_set", wherestr, "ID", page, 10);
                                resjobj.Add("result", JToken.FromObject("ok"));
                                resjobj.Add("execDt", JToken.FromObject(execDt));
                                resjobj.Add("numcount", JToken.FromObject(recordCount));
                                #endregion
                                break;


                            case "set_res_det_add"://更新或添加策略详情
                                #region
                                DataTable info = JsonConvert.DeserializeObject<DataTable>(jobject02(jobj, "info").ToString()); ;
                                execStr = MysqlHelper.ExecuteScalar("select count(*) from tab_resource_set where ResourceID = '" + id + "'").ToString();
                                if (execStr == "0")
                                {
                                    string sqlstr = "";
                                    for (int i = 0; i < info.Rows.Count; i++)
                                    {
                                        sqlstr += "insert into tab_resource_set (ResourceID, BasicAmount,PriceType, ExtraFee, StrongPrice, NotOrderFee, OrderResource2, FeeTimes, StrongFeeTimes, CheckGroupOrderTime) values('" + id + "', '"+ info.Rows[i]["BasicAmount"] +"', '" + info.Rows[i]["PriceType"] + "', '" + info.Rows[i]["ExtraFee"] + "', '" + info.Rows[i]["StrongPrice"] + "', '" + info.Rows[i]["NotOrderFee"] + "','" + info.Rows[i]["OrderResource2"] + "', '" + info.Rows[i]["FeeTimes"] + "', '" + info.Rows[i]["StrongFeeTimes"] + "', '" + info.Rows[i]["CheckGroupOrderTime"] + "'); ";
                                    }
                                    execNum = MysqlHelper.ExecuteNonQuery(sqlstr);
                                    if (execNum == 3)
                                    {
                                        resjobj.Add("result", JToken.FromObject("success"));
                                        resjobj.Add("msg", JToken.FromObject("更新成功！"));
                                    }
                                    else
                                    {
                                        resjobj.Add("result", JToken.FromObject("fail"));
                                        resjobj.Add("msg", JToken.FromObject("更新失败！"));
                                    }
                                }
                                else
                                {

                                    string sqlstr = "";
                                    for (int i = 0; i < info.Rows.Count; i++)
                                    {
                                        sqlstr += "update tab_resource_set set ExtraFee = '" + info.Rows[i]["ExtraFee"] +"',BasicAmount = '"+info.Rows[i]["BasicAmount"]+ "',StrongPrice = '" + info.Rows[i]["StrongPrice"] + "',NotOrderFee = '" + info.Rows[i]["NotOrderFee"] + "',OrderResource2 = '" + info.Rows[i]["OrderResource2"] + "',FeeTimes = '" + info.Rows[i]["FeeTimes"] + "',StrongFeeTimes = '" + info.Rows[i]["StrongFeeTimes"] + "',CheckGroupOrderTime = '" + info.Rows[i]["CheckGroupOrderTime"] + "'  where ResourceID = '" + id +"' and PriceType ='"+info.Rows[i]["PriceType"]+"';";
                                    }
                                    execNum = MysqlHelper.ExecuteNonQuery(sqlstr);
                                    if (execNum == 3)
                                    {
                                        resjobj.Add("result", JToken.FromObject("success"));
                                        resjobj.Add("msg", JToken.FromObject("更新成功！"));
                                    }
                                    else
                                    {
                                        resjobj.Add("result", JToken.FromObject("fail"));
                                        resjobj.Add("msg", JToken.FromObject("更新失败！"));
                                    }

                                }
                                #endregion
                                break;

                            #endregion
                            case "Login":
                                // MySqlConnection sqlCon = new MySqlConnection(MysqlHelper.connectionString);
                                //string sqlstring = "";
                                //string wherestrs = "and the_user = '" + user + "'";
                                //sqlstring = string.Format("select * from tab_account where 1=1 {0} ",wherestrs);
                                //MySqlCommand cmd = new MySqlCommand(sqlstring,sqlCon);
                                //MySqlDataReader dr = MysqlHelper.ExecuteReader("select * from tab_account where the_user='" + user + "'");
                                //var a = MysqlHelper.ExecuteScalar("select count(*) from tab_user_info where usr_no = '" + usrno + "' and Company = '" + company + "'").ToString();
                                // MySqlDataReader dr = MySqlHelper.ExecuteReader(MysqlHelper.connectionString, "select * from tab_account where the_user='" + user + "'");
                                string sql = "select * from tab_admin where usr_account= '" + user.Trim() + "' and usr_psw = '" + password.Trim() + "' ";
                                DataTable dt = MysqlHelper.ExecuteDataTable(sql);
                                if (dt.Rows.Count > 0)
                                {
                                    HttpContext.Current.Session["user"] = dt.Rows[0]["usr_name"].ToString();//用户姓名
                                    HttpContext.Current.Session["account"] = dt.Rows[0]["usr_account"].ToString();//用户账号
                                    //HttpContext.Current.Session["dept_ID"] = dt.Rows[0]["dept_ID"].ToString();//所属部门
                                    HttpContext.Current.Session["role"] = dt.Rows[0]["usr_Role"].ToString();//用户角色
                                    HttpContext.Current.Session["Company"] = dt.Rows[0]["Company"].ToString();//所属公司
                                    string check_company = "select * from tab_company_all where company_name like '%" + dt.Rows[0]["Company"].ToString() + "%'";
                                    DataTable dt0 = MysqlHelper.ExecuteDataTable(check_company);
                                    if(dt0.Rows.Count > 0)
                                    {
                                        HttpContext.Current.Session["parent_id"] = dt0.Rows[0]["parent_id"].ToString();
                                        //var parent_id = HttpContext.Current.Session["parent_id"];
                                    }
                                    string user_back = HttpContext.Current.Session["user"].ToString();
                                    resjobj.Add("result", JToken.FromObject("success"));
                                    resjobj.Add("msg", JToken.FromObject("登陆成功！"));
                                    resjobj.Add("data", JToken.FromObject(user_back));
                                }
                                else
                                {


                                    resjobj.Add("result", JToken.FromObject("fail"));
                                    resjobj.Add("msg", JToken.FromObject("登陆失败:账号或密码错误！"));
                                }
                                break;
                            #region 二维码设置页面
                            case "set_QR_load":
                                JObject QR_info = jobject03(jobj, "QR_info");
                                string Create_time = jobject(QR_info, "Create_time");
                                string Staff_name = jobject(QR_info, "Staff_name");
                                if (QR_info != null)
                                {
                                    if (Create_time != "")
                                    {
                                        wherestr += " and Create_time = '" + Create_time + "'";
                                    }
                                    if (Staff_name != "")
                                    {
                                        wherestr += " and Staff_name = '" + Staff_name + "'";
                                    }
                                }
                                execDt = MysqlHelper.getPager(out recordCount, "", table, wherestr, id, page, 10);
                                resjobj.Add("result", JToken.FromObject("ok"));
                                resjobj.Add("execDt", JToken.FromObject(execDt));
                                resjobj.Add("numcount", JToken.FromObject(recordCount));
                                break;
                            case "set_QR_add":
                                //execStr = MysqlHelper.ExecuteScalar("select count(*) from tab_resource_info where ResourceID = '" + usrno + "' and ResourceName = '" + usrname + "'").ToString();
                                //if (execStr == "0")
                                //{
                                string Company_owner = jobject(jobj, "Company_owner");
                                string Staff_names = jobject(jobj, "Staff_name");
                                string Staff_phone = jobject(jobj, "Staff_phone");
                                string Profession = jobject(jobj, "Profession");
                                string Expiry_time = jobject(jobj, "Expiry_time");
                                
                                execNum = MysqlHelper.ExecuteNonQuery("insert into tab_qr_setting (UUID,Create_time,Company_owner,Staff_name,Staff_phone,Profession,Expiry_time) values (replace(uuid(), '-', ''),now(),'" + Company_owner + "','" + Staff_names + "','" + Staff_phone + "','" + Profession + "','" + Expiry_time +"')");
                                    if (execNum == 1)
                                    {
                                        resjobj.Add("result", JToken.FromObject("success"));
                                        resjobj.Add("msg", JToken.FromObject("添加成功"));
                                    }
                                    else
                                    {
                                        resjobj.Add("result", JToken.FromObject("fail"));
                                        resjobj.Add("msg", JToken.FromObject("添加失败"));
                                    }
                                
                                //else
                                //{
                                //    resjobj.Add("result", JToken.FromObject("fail"));
                                //    resjobj.Add("msg", JToken.FromObject("添加失败，此消费策略已存在！"));
                                //}
                                
                                break;
                            #region//子公司设置
                            case "set_com_load":
                                wherestr = "select * from tab_company_all";
                                execDt = MysqlHelper.ToDataTablePager(out recordCount, wherestr, page, 10);
                                resjobj.Add("result", JToken.FromObject("ok"));
                                resjobj.Add("execDt", JToken.FromObject(execDt));
                                resjobj.Add("numcount", JToken.FromObject(recordCount));

                                break;
                            case "set_com_delete":
                                string com_id = jobject(jobj, "com_id");
                                wherestr = "delete from tab_company_all where id='" + com_id + "'";
                                execNum = MysqlHelper.ExecuteNonQuery(wherestr);
                                if (execNum == 1)
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
                            case "set_com_add":
                                string companylevel = jobject(jobj, "companylevel");
                                string companyname = jobject(jobj, "companyname");
                                wherestr = "select count(*) from tab_company_all where parent_id = 1";
                                execDt = MysqlHelper.ExecuteDataTable(wherestr);
                                if (execDt.Rows[0][0].ToString() == "1")//如果存在父公司
                                {
                                    if(companylevel == "1")
                                    {
                                        wherestr = "update tab_company_all set company_name ='" + companyname + "' where parent_id ='" + companylevel + "'";
                                        execNum = MysqlHelper.ExecuteNonQuery(wherestr);
                                        if (execNum == 1)
                                        {
                                            resjobj.Add("result", JToken.FromObject("success"));
                                            resjobj.Add("msg", JToken.FromObject("更新总公司成功！"));
                                        }
                                        else
                                        {
                                            resjobj.Add("result", JToken.FromObject("fail"));
                                            resjobj.Add("msg", JToken.FromObject("更新总公司失败！"));
                                        }

                                    }
                                    else
                                    {
                                        wherestr = "insert into tab_company_all (company_name,parent_id) values ('" + companyname + "','" + companylevel + "')";
                                        execNum = MysqlHelper.ExecuteNonQuery(wherestr);
                                        if (execNum == 1)
                                        {
                                            resjobj.Add("result", JToken.FromObject("success"));
                                            resjobj.Add("msg", JToken.FromObject("添加分公司成功！"));
                                        }
                                        else
                                        {
                                            resjobj.Add("result", JToken.FromObject("fail"));
                                            resjobj.Add("msg", JToken.FromObject("添加分公司失败！"));
                                        }
                                    }
                                }
                                else
                                {
                                    if(companylevel == "1")
                                    {
                                        wherestr = "insert into tab_company_all (company_name,parent_id) values ('" + companyname + "','" + companylevel + "')";
                                        execNum = MysqlHelper.ExecuteNonQuery(wherestr);
                                        if (execNum == 1)
                                        {
                                            resjobj.Add("result", JToken.FromObject("success"));
                                            resjobj.Add("msg", JToken.FromObject("添加总公司成功！"));
                                        }
                                        else
                                        {
                                            resjobj.Add("result", JToken.FromObject("fail"));
                                            resjobj.Add("msg", JToken.FromObject("添加总公司失败！"));
                                        }
                                    }
                                    else
                                    {
                                        wherestr = "update tab_company_all set company_name ='" + companyname + "' where parent_id ='" + companylevel + "'";
                                        execNum = MysqlHelper.ExecuteNonQuery(wherestr);
                                        if (execNum == 1)
                                        {
                                            resjobj.Add("result", JToken.FromObject("success"));
                                            resjobj.Add("msg", JToken.FromObject("更新分公司成功！"));
                                        }
                                        else
                                        {
                                            resjobj.Add("result", JToken.FromObject("fail"));
                                            resjobj.Add("msg", JToken.FromObject("更新分公司失败！"));
                                        }
                                    }
                                }
                                break;
                            #endregion
                            #endregion
                                //测试部门树
                            case "load_tree":
                                //string sql = "select Dept_Name,Dept_Id,Dept_up from tbdeptinfo";
                                DataTable dt2 = MysqlHelper.ExecuteDataTable("select Dept_Name,Dept_Level,Dept_Id,Dept_up from tbdeptinfo");
                                dt2.Columns.Add("isParent", typeof(object));
                                foreach (DataRow dr in dt2.Rows)
                                {
                                    if(dr["Dept_up"].ToString() == "1")
                                    {
                                        dr["isParent"] = true;
                                    }
                                    
                                }
                                //dt2.Columns.Add("open", typeof(string));
                                //foreach (DataRow dr in dt2.Rows)
                                //{
                                //    dr["open"] = true;
                                //}

                                dt2.Columns["Dept_Name"].ColumnName = "name";
                                dt2.Columns["Dept_Id"].ColumnName = "id";
                                dt2.Columns["Dept_up"].ColumnName = "pId";
                                resjobj.Add("result", JToken.FromObject("ok"));
                                resjobj.Add("execDt", JToken.FromObject(dt2));
                                break;
                            case "tree_change":
                                string tree_type = jobject(jobj, "tree_type");
                                string tree_id = jobject(jobj, "tree_id");
                                string tree_name = jobject(jobj, "tree_name");
                                
                                switch (tree_type)
                                {
                                    case "update_tree":
                                        int update_tree = MysqlHelper.ExecuteNonQuery("update tbdeptinfo set Dept_Name ='" + tree_name + "' where Dept_Id ='" + tree_id + "'");
                                        resjobj.Add("result", JToken.FromObject("ok"));
                                        resjobj.Add("msg", JToken.FromObject("更新部门成功！"));
                                        break;
                                    case "delete_tree":
                                        int delete_tree = MysqlHelper.ExecuteNonQuery("delete from tbdeptinfo where Dept_Id ='" + tree_id + "'");
                                        resjobj.Add("result", JToken.FromObject("ok"));
                                        resjobj.Add("msg", JToken.FromObject("删除部门成功！"));
                                        break;
                                    case "insert_tree":
                                        string pid = jobject(jobj, "pid");
                                        int insert_tree = MysqlHelper.ExecuteNonQuery("insert into tbdeptinfo (Dept_Name,Dept_up,dept_state) values ('" + tree_name + "','" + pid +"',1 )");
                                        resjobj.Add("result", JToken.FromObject("ok"));
                                        resjobj.Add("msg", JToken.FromObject("添加成功！"));
                                        break;
                                }
                                break;



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
        #endregion


    }
}