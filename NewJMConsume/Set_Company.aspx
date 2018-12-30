<%@ Page Title="" Language="C#" MasterPageFile="~/Setting.Master" AutoEventWireup="true" CodeBehind="Set_Company.aspx.cs" Inherits="NewJMConsume.WebForm6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="mysrc/pagejs/Set_Company.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadcrumbs ace-save-state" id="breadcrumbs">
        <ul class="breadcrumb">
            <li>
                <i class="ace-icon fa fa-home home-icon"></i>
                <a href="#">设置</a>
            </li>
            <li class="active">子公司设置</li>
        </ul>
        <!-- /.breadcrumb -->
    </div>

    <div class="page-content">
        <div class="page-header">
            <h1>子公司设置
            </h1>
        </div>
        <!-- /.page-header -->
        <div class="row">
            <div class="col-lg-12">
                <%-- 输入行--%>
                <div class="row">
                    <div class="col-lg-12 form-inline">
                        <div class="form-group" style="margin-left: 10px;">
                            <label>公司级别:</label>
                            <select class="form-control" id="companylevel">
                                <option value="1">一级总公司</option>
                                <option value="2">二级子公司</option>
                            </select>
                        </div>
                        <div class="form-group">
                                <div id="name-container" style="margin-left: 10px">
                                    <input type="text" style="width:200px" class="form-control" id="companyName" placeholder="请输入公司名称" />
                                </div>
                            </div>
                        <div class="form-group" style="margin-left: 10px;">
                            <input type="button" class="btn btn-primary" onclick="addCompany()" value="添加" />
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
