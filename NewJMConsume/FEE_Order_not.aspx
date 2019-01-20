<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="FEE_Order_not.aspx.cs" Inherits="NewJMConsume.WebForm3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="mysrc/pagejs/FEE_Order_not.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadcrumbs ace-save-state" id="breadcrumbs">
        <ul class="breadcrumb">
            <li>
                <i class="ace-icon fa fa-home home-icon"></i>
                <a href="#">结算管理</a>
            </li>
            <li class="active">订餐未就餐结算表</li>
        </ul>
        <!-- /.breadcrumb -->
    </div>
    <div class="page-content">
        <div class="page-header">
            <h1>订餐未就餐
            </h1>
        </div>
        <!-- /.page-header -->

        <div class="row">
            <div class="col-lg-12">

                <div class="row">
                    <div class="col-lg-12 form-inline">
                        <div class="form-group">
                            <label>开始:</label>
                        </div>
                        <div class="form-group">
                            <div id="sandbox-container" style="margin-left: 5px">
                                <input type="text" class="form-control" id="beg_not" style="width: 100px" />
                            </div>
                            <script type="text/javascript">
                                $('#sandbox-container input').datetimepicker({
                                    minView: "month",
                                    language: 'zh-CN',
                                    format: "yyyy-mm-dd",
                                    autoclose: true
                                }).on("click", function () {
                                    $("#sandbox-container input").datetimepicker("setEndDate", $("#sandbox-container02 input").val())
                                });
                            </script>
                        </div>

                        <div class="form-group">
                            <label>结束:</label>
                        </div>
                        <div class="form-group">
                            <div id="sandbox-container02" style="margin-left: 5px">
                                <input type="text" class="form-control" id="end_not" style="width: 100px" />
                            </div>
                            <script type="text/javascript">
                                $('#sandbox-container02 input').datetimepicker({
                                    minView: "month",
                                    language: 'zh-CN',
                                    format: "yyyy-mm-dd",
                                    autoclose: true
                                }).on("click", function () {
                                    $("#sandbox-container02 input").datetimepicker("setStartDate", $("#sandbox-container input").val())
                                });
                            </script>
                        </div>
                        <div class="form-group">
                            <label>姓名:</label>
                        </div>
                        <div class="form-group">
                            <input type="text" class="form-control" id="name_not" style="width: 100px" />
                        </div>
                        <div class="form-group">
                            <label>部门:</label>
                        </div>
                        <div class="form-group">
                            <input type="text" class="form-control" id="dept_not" style="width: 100px" />
                        </div>
                        <div class="form-group">
                            <label>餐次:</label>
                        </div>
                        <select class="form-control" id="price_type_not" data-width="auto">
                            <option value="">全部</option>
                            <option value="早餐">早餐</option>
                            <option value="午餐">午餐</option>
                            <option value="晚餐">晚餐</option>
                        </select>
                        <div class="form-group">
                            <label>餐类:</label>
                        </div>
                        <select class="form-control" id="spe_type_not" data-width="auto">
                            <option value="">全部</option>
                            <option value="工作餐">工作餐</option>
                            <option value="自助餐">自助餐</option>
                        </select>
                        <input type="button" class="btn-primary form-control" style="margin-left: 20px;" value="查询" onclick="load(true, 1)" />
                        <div class="form-group">
                            <input type="button" class="form-control btn btn-primary" onclick="downNot()" value="导 出" />
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-top: 1%">
                    <div class="col-lg-12 form-inline">
                        <%-- 输入行--%>
                    </div>

                </div>
                <%--表格--%>
                <div class="row" style="margin-top: 1%">
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
