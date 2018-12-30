<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="FEE_Detail.aspx.cs" Inherits="NewJMConsume.FEE_Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="mysrc/pagejs/FEE_Detail.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadcrumbs ace-save-state" id="breadcrumbs">
        <ul class="breadcrumb">
            <li>
                <i class="ace-icon fa fa-home home-icon"></i>
                <a href="#">结算管理</a>
            </li>
            <li class="active">消费明细查询</li>
        </ul>
        <!-- /.breadcrumb -->

        <div class="nav-search" id="nav-search">
            <div class="form-search">
                <span class="input-icon">
                    <input type="text" placeholder="Search ..." class="nav-search-input" id="nav-search-input" autocomplete="off" />
                    <i class="ace-icon fa fa-search nav-search-icon"></i>
                </span>
            </div>
        </div>
        <!-- /.nav-search -->
    </div>

    <div class="page-content">
        <div class="page-header">
            <h1>消费明细查询
            </h1>
        </div>
        <!-- /.page-header -->

        <div class="row">
            <div class="col-lg-12">
                <%-- 输入行--%>
                <div class="row">
                    <div class="col-lg-12">
                        <table class="table table-bordered">
                            <tbody>
                                <tr>
                                    <!--开始日期-->
                                    <td class="form-inline">
                                        <div class="form-group">
                                            <label>开始:</label>
                                        </div>
                                        <div class="form-group">
                                            <div id="sandbox-container" style="margin-left: 5px">
                                                <input type="text" class="form-control" id="FEE_btime" style="width: 100px" />
                                            </div>
                                            <script type="text/javascript">
                                                $('#sandbox-container input').datetimepicker({
                                                    minView: "month",
                                                    language: 'zh-CN',
                                                    format: "yyyy-mm-dd"
                                                })
                                            </script>
                                        </div>
                                    </td>
                                    <!--结束日期-->
                                    <td class="form-inline">
                                        <div class="form-group">
                                            <label>结束:</label>
                                        </div>
                                        <div class="form-group">
                                            <div id="sandbox-container02" style="margin-left: 5px">
                                                <input type="text" class="form-control" id="FEE_etime" style="width: 100px" />
                                            </div>
                                            <script type="text/javascript">
                                                $('#sandbox-container02 input').datetimepicker({
                                                    minView: "month",
                                                    language: 'zh-CN',
                                                    format: "yyyy-mm-dd"
                                                })
                                            </script>
                                        </div>
                                    </td>
                                    <!--餐次-->
                                    <td class="form-inline">
                                        <div class="form-group">
                                            <label>餐次</label>
                                            <select class="form-control" id="FEE_type" data-width="auto">
                                                <option value="">全部</option>
                                                <option value="早餐">早餐</option>
                                                <option value="午餐">午餐</option>
                                                <option value="晚餐">晚餐</option>
                                            </select>
                                        </div>
                                    </td>
                                    <!--员工类型-->
                                    <td class="form-inline">
                                        <div class="form-group">
                                            <label>部门</label>
                                            <select class="form-control" id="FEE_dept" data-width="auto">
                                                <option value="">全部</option>
                                                <option value="财务科">财务科</option>
                                                <option value="办公室">办公室</option>
                                            </select>
                                        </div>
                                    </td>
                                    <!--部门-->
                                    <td class="form-inline">
                                        <div class="form-group">
                                            <input type="button" class="btn btn-primary" onclick="load(true,1)" value="查 询" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <!--结算单位-->
                                    <td class="form-inline">
                                        <div class="form-group">
                                            <label>结算单位</label>
                                            <select class="form-control" id="FEE_dev_no" data-width="auto">
                                                <option value="">全部</option>
                                                <option value ="1">1号</option>
                                                <option value="2">2号</option>
                                            </select>
                                        </div>
                                    </td>
                                    <!--付费类型-->
                                    <td class="form-inline">
                                        <div class="form-group">
                                            <label>姓名</label>
                                            <input type="text" id="FEE_name" class="form-control" />
                                        </div>
                                    </td>
                                    <!--姓名-->
                                    <td class="form-inline">
                                        <div class="form-group">
                                            <label>消费类别</label>
                                            <select class="form-control" id="pay_type" data-width="auto">
                                                <option value="">全部</option>
                                                <option value="定额消费">定额消费</option>
                                            </select>
                                        </div>
                                    </td>
                                    <!--消费类别-->
                                    <td class="form-inline">
                                        <div class="form-group">
                                            <label>消费地点</label>
<%--                                            <select class="selectpicker" id="FEE_place" data-width="auto">
                                                <option value="">全部</option>
                                            </select>--%>
                                            <input type="text" id="FEE_place" class="form-control" />
                                        </div>
                                    </td>
                                    <!--消费地点-->
                                    <td class="form-inline">
                                        <div class="form-group">
                                            <input type="button" class="btn btn-primary" value="导 出" onclick="downFeeDetail()" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                </div>
                <%--表格--%>
                <div class="row" style="margin-top: 2%">
                    <div class="col-lg-12">
                        <table class="table table-bordered" id="table">
                        </table>
                    </div>
                </div>


                <%--分页--%>
                <div class="row">
                    <div class="col-lg-12">
                        <div id="example" style="text-align: center">
                            <ul id="pageLimit"></ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </div>

        </div>
    </div>
</asp:Content>
