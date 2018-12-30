<%@ Page Title="" Language="C#" MasterPageFile="~/Setting.Master" AutoEventWireup="true" CodeBehind="Set_Role.aspx.cs" Inherits="NewJMConsume.Set_Role" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="mysrc/pagejs/Set_Role.js"></script>
    <script type="text/javascript">

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadcrumbs ace-save-state" id="breadcrumbs">
        <ul class="breadcrumb">
            <li>
                <i class="ace-icon fa fa-home home-icon"></i>
                <a href="#">设置</a>
            </li>
            <li class="active">角色设置</li>
        </ul>
        <!-- /.breadcrumb -->
    </div>


    <div class="page-content">
        <div class="page-header">
            <h1>角色设置
            </h1>
        </div>
        <!-- /.page-header -->

        <div class="row">
            <div class="col-lg-12">
                <%-- 输入行--%>
                <div class="row">
                    <div class="col-lg-12 form-inline">
                        <div class="form-group">
                            <label>状态：</label>
                        </div>
                        <div class="form-group">
                            <label class="radio-inline">
                                <input type="radio" name="statusRadio" id="inlineRadio1" value="" checked="checked">
                                全部
                            </label>
                            <label class="radio-inline">
                                <input type="radio" name="statusRadio" id="inlineRadio2" value="1">
                                启用
                            </label>
                            <label class="radio-inline">
                                <input type="radio" name="statusRadio" id="inlineRadio3" value="0">
                                停用
                            </label>
                        </div>
                        <div class="form-group" style="margin-left: 2%">
                            <label>所属公司：</label>
                        </div>
                        <div class="form-group">

                            <select class="form-control" id="company" data-width="auto">
                                <option value="">全部</option>
                            </select>
                        </div>
                        <div class="form-group">
                            <input type="text" class="form-control" placeholder="关键字" id="keyword" />
                        </div>
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="查询" onclick="load(true, 1);" />
                        </div>
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="新增角色" data-toggle="modal" data-target="#myModal" />
                        </div>
                    </div>
                </div>

                <%--表格--%>
                <div class="row" style="margin-top: 2%">
                    <div class="col-lg-12">
                        <table class="table table-bordered" id="table">
                            <%--                            <thead>
                                <tr>
                                    <th>表头1</th>
                                    <th>表头2</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>单元格1</td>
                                    <td>单元格1</td>
                                </tr>
                            </tbody>--%>
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



    <%--模态框--%>
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header text-center">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <label class="modal-title" id="myModalLabel">添加角色</label>
                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div class="row" style="margin-top: 1%">
                            <label class="col-lg-2">所属公司</label>
                            <select class="selectpicker" id="addcompany">
                                <option value="">全部</option>
                                <option value="江门市公司">江门市公司</option>
                                <option value="新会分公司">新会分公司</option>
                                <option value="台山分公司">台山分公司</option>
                                <option value="开平分公司">开平分公司</option>
                                <option value="恩平分公司">恩平分公司</option>
                                <option value="鹤山分公司">鹤山分公司</option>
                            </select>
                        </div>
                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">角色名称</label>
                            <input type="text" class="form-control col-lg-10" id="addrole" />
                        </div>
                        <div class="row" style="margin-top: 1%">
                            <label class="col-lg-2">可见模块</label>
                            <div class="col-lg-10">
                                <p>
                                    <label>订餐管理：</label>
                                    <label class="checkbox-inline">
                                        <input type="checkbox" value="订餐统计" name="isModel" />订餐统计
                                    </label>
                                    <label class="checkbox-inline">
                                        <input type="checkbox" value="订餐明细" name="isModel" />订餐明细
                                    </label>
                                </p>
                                <p>
                                    <label>结算管理：</label>
                                    <label class="checkbox-inline">
                                        <input type="checkbox" value="消费明细" name="isModel" />消费明细
                                    </label>
                                    <label class="checkbox-inline">
                                        <input type="checkbox" value="消费明细(个人)" name="isModel" />消费明细(个人)
                                    </label>
                                    <label class="checkbox-inline">
                                        <input type="checkbox" value="订餐就餐结算" name="isModel" />订餐就餐结算表
                                    </label>
                                    <label class="checkbox-inline">
                                        <input type="checkbox" value="订餐未就餐结算" name="isModel" />订餐未就餐结算表
                                    </label>
                                </p>
                                <p>
                                    <label>充值管理：</label>
                                    <label class="checkbox-inline">
                                        <input type="checkbox" value="现金充值" name="isModel" />现金充值
                                    </label>
                                    <label class="checkbox-inline">
                                        <input type="checkbox" value="充值记录" name="isModel" />充值记录
                                    </label>
                                    <label class="checkbox-inline">
                                        <input type="checkbox" value="饭卡余额" name="isModel" />余额查询
                                    </label>
                                </p>
                            </div>
                        </div>
                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">角色说明</label>
                            <input type="text" class="form-control" id="addremark" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>
                    <button type="button" class="btn btn-primary" onclick="addRole()" data-dismiss="modal">确定</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
