<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="BM_Count.aspx.cs" Inherits="NewJMConsume.BM_Count" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="mysrc/commen_js/jquery.cookie.min.js"></script>
    <script src="mysrc/pagejs/BM_Count.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadcrumbs ace-save-state" id="breadcrumbs">
        <ul class="breadcrumb">
            <li>
                <i class="ace-icon fa fa-home home-icon"></i>
                <a href="#">订餐管理</a>
            </li>
            <li class="active">订餐统计</li>
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
            <h1>订餐统计
            </h1>
        </div>
        <!-- /.page-header -->

        <div class="row">
            <div class="col-lg-12">
                <%-- 输入行--%>
                <div class="row">
                    <div class="col-lg-12 form-inline">
                        <div class="form-group">
                            <label>日期:</label>
                        </div>
                        <div class="form-group">
                            <div id="sandbox-container" style="margin-left: 5px">
                                <input type="text" class="form-control" id="check_time" style="width: 100px" />
                            </div>
                            <script type="text/javascript">
                                $('#sandbox-container input').datetimepicker({
                                    minView: "month",
                                    language: 'zh-CN',
                                    format: "yyyy-mm-dd"
                                })
                            </script>
                        </div>
                        <div class="form-group">
                            <label>公司</label>
                            <%--                            <select class="selectpicker" id="eattype" data-width="auto">
                                <option value="">全部</option>
                                <option value="早餐">早餐</option>
                                <option value="午餐">午餐</option>
                                <option value="晚餐">晚餐</option>
                            </select>--%>
                            <input type="text" class="form-control" id="Company" style="width: 100px" />
                        </div>
                        <div class="form-group">
                            <label>消费地点</label>
                            <%--                            <select class="selectpicker" id="" data-width="auto">
                                <option value="">全部</option>
                            </select>--%>
                            <input type="text" class="form-control" id="Feeplace" style="width: 100px" />
                        </div>

                        <div class="form-group">
                            <input type="button" class="btn btn-primary" onclick="load(true, 1)" value="查询" />
                        </div>
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" onclick="downCount()" value="导 出" />
                        </div>
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

</asp:Content>
