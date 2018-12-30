<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="TU_Card.aspx.cs" Inherits="NewJMConsume.TU_Card" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="mysrc/pagejs/TU_Card.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadcrumbs ace-save-state" id="breadcrumbs">
        <ul class="breadcrumb">
            <li>
                <i class="ace-icon fa fa-home home-icon"></i>
                <a href="#">充值管理</a>
            </li>
            <li class="active">饭卡余额</li>
        </ul>
        <!-- /.breadcrumb -->
    </div>

    <div class="page-content">
        <div class="page-header">
            <h1>饭卡余额
            </h1>
        </div>
        <!-- /.page-header -->

        <div class="row">
            <div class="col-lg-12">
                <%-- 输入行--%>
                <div class="row">
                    <div class="col-lg-12 form-inline">
                        <div class="form-group">
                            <label>部门：</label>
                            <%--                            <select class="selectpicker" id="dept" data-width="auto">
                                <option value="">全部</option>
                            </select>--%>
                            <input type="text" class="form-control" id="dept" />
                        </div>
                        <div class="form-group">
                            <label>姓名:</label>
                        </div>
                        <div class="form-group">
                            <input type="text" class="form-control" id="keyword" />
                        </div>
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="查询" onclick="load(true, 1)" />
                        </div>
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="导 出" onclick="downTUcard()" />
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
