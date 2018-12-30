<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="TU_Detail.aspx.cs" Inherits="NewJMConsume.TU_Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="mysrc/pagejs/TU_Detail.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadcrumbs ace-save-state" id="breadcrumbs">
        <ul class="breadcrumb">
            <li>
                <i class="ace-icon fa fa-home home-icon"></i>
                <a href="#">充值管理</a>
            </li>
            <li class="active">充值记录查询</li>
        </ul>
        <!-- /.breadcrumb -->
    </div>
    <div class="page-content">
        <div class="page-header">
            <h1>充值记录查询
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
                                <input type="text" class="form-control" id="btime" style="width: 100px" />
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
                            <label>结束:</label>
                        </div>
                        <div class="form-group">
                            <div id="sandbox-container02" style="margin-left: 5px">
                                <input type="text" class="form-control" id="etime" style="width: 100px" />
                            </div>
                            <script type="text/javascript">
                                $('#sandbox-container02 input').datetimepicker({
                                    minView: "month",
                                    language: 'zh-CN',
                                    format: "yyyy-mm-dd"
                                })
                            </script>
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-top: 1%">
                    <div class="col-lg-12">
                        <%-- 输入行--%>
                        <div class="row">
                            <div class="col-lg-12 form-inline">
                                <div class="form-group">
                                    <label>部门：</label>
                                    <select class="form-control" id="dept" data-width="auto">
                                        <option value="">全部</option>
                                    </select>
                                </div>
                                <div class="form-group">
                                    <label>姓名:</label>
                                </div>
                                <div class="form-group">
                                    <input type="text" class="form-control" id="usrname" />
                                </div>
                                <div class="form-group">
                                    <label>卡号:</label>
                                </div>
                                <div class="form-group">
                                    <input type="text" class="form-control" id="card" />
                                </div>
                                <div class="form-group">
                                    <label>充值途径</label>
                                    <select class="form-control" id="method" data-width="auto">
                                        <option value="">全部</option>
                                        <option value="个人充值">个人充值</option>
                                        <option value="团体充值">团体充值</option>
                                    </select>
                                </div>
                                <div class="form-group">
                                    <label>充值人员:</label>
                                </div>
                                <div class="form-group">
                                    <input type="text" class="form-control" id="creator" />
                                </div>

                                <div class="form-group">
                                    <input type="button" class="btn btn-primary" value="查询" onclick="load(true, 1)" />
                                </div>
                                                                <div class="form-group">
                                    <input type="button" class="btn btn-primary" value="导 出" onclick="downdetail()"/>
                                </div>
                            </div>
                        </div>
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
