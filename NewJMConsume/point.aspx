<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="point.aspx.cs" Inherits="NewJMConsume.WebForm5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="breadcrumbs ace-save-state" id="breadcrumbs">
        <ul class="breadcrumb">
            <li>
                <i class="ace-icon fa fa-home home-icon"></i>
                <a href="#">提示</a>
            </li>
            <li class="active">系统提示</li>
        </ul>
        <!-- /.breadcrumb -->
    </div>
        <div class="page-content">
        <div class="page-header">
            <h1>您暂无权限访问此页面！
            </h1>
        </div>
            <div class="row" style="margin-left:20px;">
                <a href="FEE_Mine.aspx">返回个人消费明细查询</a>
            </div>
            </div>
</asp:Content>
