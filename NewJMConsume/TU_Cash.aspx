<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="TU_Cash.aspx.cs" Inherits="NewJMConsume.TU_Cash" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .table-d table {
            border: 1px solid #F00;
            border-collapse: collapse
        }

            .table-d table td {
                border: 1px solid #F00;
            }
    </style>
    <link href="tree/css/zTreeStyle/zTreeStyle.css" rel="stylesheet" />
    <link href="mysrc/js/fileInput/fileinput.css" rel="stylesheet" />
    <script src="mysrc/pagejs/TU_Cash.js"></script>
    <script src="tree/js/jquery.ztree.core.js"></script>
    <script src="tree/js/jquery.ztree.exedit.js"></script>
    <script src="tree/js/jquery.ztree.excheck.min.js"></script>
    <%--        <script src="tree/js/TreeShow.js"></script>--%>
    <script src="mysrc/js/fileInput/fileinput.js"></script>
    <script src="mysrc/js/fileInput/zh.js"></script>
    <script>
        $(document).ready(function () {
            //load_company();
            load_tree(true, 1);
            $("#fileInput").fileinput({

                uploadUrl: 'Fileup.ashx?type=fileToadd',
                uploadAsync: true, //是否异步上传
                language: 'zh', //语言
                showCaption: true, //是否显示标题
                dropZoneEnabled: false, //是否显示拖拽区域
                showPreview: false, //是否显示预览区域
                enctype: 'multipart/form-data',
                allowedFileExtensions: ['xlsx', 'xls'] //接收的文件后缀
            }).on('filebatchselected',
                function (event, data) {
                    if (data.length < 1) {
                        $('.fileinput-remove-button').click(); //点击移除按钮

                    }

                }).on("fileuploaded", function (event, data) {
                    //if (data.response.MessageType === 'success') {
                    setTimeout(function () { $('.fileinput-remove-button').click(); }, 250);
                    //}
                    alert("上传成功！");
                }).on("fileerror", function (event, data) {
                    alert("出现错误！");
                    //setTimeout(function () { $('.fileinput-remove-button').click(); }, 250);
                });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadcrumbs ace-save-state" id="breadcrumbs">
        <ul class="breadcrumb">
            <li>
                <i class="ace-icon fa fa-home home-icon"></i>
                <a href="#">充值管理</a>
            </li>
            <li class="active">现金充值</li>
        </ul>
        <!-- /.breadcrumb -->
    </div>

    <div class="page-content">
        <div class="page-header">
            <h1>个人充值
            </h1>
        </div>
        <!-- /.page-header -->

        <div class="row">
            <div class="col-lg-12">
                <%-- 输入行--%>
                <div class="row" style="">
                    <div class="col-lg-12 form-inline">
                        <div class="form-group">
                            <label>输入查询信息:</label>
                        </div>
                        <div class="form-group">
                            <input type="text" class="form-control" id="check_key" />
                        </div>
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="查询" onclick="check_user(true, 1)" />
                        </div>
                    </div>
                </div>
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
                <div class="row" style="margin-left: 12px; margin-top: 20px;">
                    <div class="col-lg-12 form-inline">
                        <div class="form-group">
                            <label>充值类型</label>
                            <select class="form-control" id="method" data-width="auto">
                                <option selected="selected" value="个人充值">个人充值</option>
                            </select>
                        </div>
                        <div class="form-group">
                            <label>充值金额:</label>
                        </div>
                        <div class="form-group">
                            <input type="text" class="form-control" id="money" />
                        </div>
                        <div class="form-group">
                            <label>备注:</label>
                        </div>
                        <div class="form-group">
                            <input type="text" class="form-control" id="memo" />
                        </div>
                        <div class="form-group">
                            <input type="button" class="btn btn-primary " value="提交" onclick="showAdd()" />
                        </div>
                        <%--                                                <div class="form-group">
                            <input type="button" class="btn btn-primary " value="test" onclick="getChecked()" />
                        </div>--%>
                    </div>
                </div>
                <%--                <div class="row" style="margin-top: 20px;">
                    <div class="col-lg-12 form-inline">
                    </div>
                </div>--%>
            </div>

        </div>
        <hr />
        <div class="page-header">
            <h1>团体充值
            </h1>
        </div>
        <!-- /.page-header -->

        <div class="row">
            <div class="col-lg-12">
                <%-- 输入行--%>
                <div class="row">
                    <div class="col-lg-12 form-inline">
                        <div class="form-group">
                            <label>充值类型</label>
                            <select class="form-control" id="tutype" data-width="auto">
                                <option selected="selected" value="团体充值">团体充值</option>
                            </select>
                        </div>
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" onclick="showTeam()" value="批量充值" />
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <hr />
        <div class="page-header">
            <h1>按部门充值
            </h1>
        </div>
        <!-- /.page-header -->
        <div class="row">
            <div class="col-lg-12">
                <%-- 输入行--%>
                <div class="row">
                    <div class="col-lg-12 form-inline">
                        <%--                        <div class="form-group">
                            <label>选择公司:</label>
                            <select class="form-control" id="select_company" data-width="auto" onchange="Dispaydept($('#select_company option').filter(':selected').val());">
                                <option selected="selected" value="">请选择</option>
                            </select>
                        </div>
                        <div class="form-group">
                            <label>选择部门:</label>
                            <select class="form-control" id="select_dept" data-width="auto">
                                <option selected="selected" value="">请选择</option>
                            </select>
                        </div>--%>
                        <%--                        <div class="form-group">
                            <label>选择人员:</label>
                            <select class="form-control" id="select_staff" data-width="auto">
                                <option selected="selected" value="">请选择</option>
                            </select>
                        </div>--%>
                        <%--                        <div class="form-group">
                            <label>备注:</label>
                        </div>
                        <div class="form-group">
                            <input type="text" class="form-control" />
                        </div>
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="提交" />
                        </div>--%>
                    </div>
                </div>
                <div class="row" style="margin-top: 20px;">
                    <div class="col-lg-12 form-inline">
                        <div class="form-group">
                            <label>充值金额</label>
                            <input type="text" onkeyup="value=value.replace(/[^\d]/g,'')" onblur="value=value.replace(/[^\d]/g,'')" class="form-control" id="add_money" placeholder="（元）" />
                        </div>
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="提交" onclick="showAll()" />
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-top: 20px;">
                    <div class="col-lg-2">
                        <ul id="treeDemo" class="ztree"></ul>
                    </div>
                    <div class="col-lg-10">
                        <table class="table table-bordered" id="table2">
                        </table>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12">
                        <div style="text-align: center">
                            <ul id="pageLimit2"></ul>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
    <%--模态框：批量新增--%>
    <div class="modal fade bs-example-modal-lg" id="myModal02" tabindex="-1" role="dialog" aria-labelledby="myModelLabel02">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header text-center">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <label class="modal-title" id="myModalLabel02">确认充值</label>
                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div class="row ">
                            <div class="table-d">
                                <table class="table table-bordered" width="1000px" border="1" cellspacing="0" cellpadding="0">
                                    <thead>
                                        <tr style="border: 2px;">
                                            <th>姓名</th>
                                            <th>部门</th>
                                            <th>手机号码</th>
<%--                                            <th>充值金额(元)</th>
                                            <th>充值途径</th>
                                            <th>备注</th>--%>
                                        </tr>
                                    </thead>
                                    <tbody id ="print_body">
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="row">
                          <div id="print_footer" class="col-lg-offset-6">

                          </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>
                    <button type="button" class="btn btn-primary" onclick="test()" data-dismiss="modal">确定充值</button>
                </div>
            </div>
        </div>
    </div>

    <%--模态框：批量新增--%>
    <div class="modal fade" id="myModal03" tabindex="-1" role="dialog" aria-labelledby="myModelLabel03">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header text-center">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <label class="modal-title" id="myModalLabel03">批量充值</label>
                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div class="row form-line">
                            <input id="fileInput" name="fileInput" type="file">
                        </div>
                        <div>
                            <label>下载模板: </label>
                            <a href="uploadAdd/团体充值模板.xls">团体充值模板.xls</a>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>
                    <button type="button" class="btn btn-primary" onclick="load(true,1)" data-dismiss="modal" id="changeBtn02">确定</button>
                </div>
            </div>
        </div>
    </div>
    <%--模态框：充值至模态框--%>
    <div class="modal fade" id="myModal04" tabindex="-1" role="dialog" aria-labelledby="myModelLabel04">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header text-center">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <label class="modal-title" id="myModalLabel04">充值人员确认</label>
                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div class="col-lg-12">
                            <table class="table table-bordered" id="department_table">
                            </table>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <div style="text-align: center">
                                    <ul id="pageLimit4"></ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>
                    <button type="button" class="btn btn-primary" onclick="determin_recharge()" data-dismiss="modal">确定充值</button>
                    <%--<button type="button" class="btn btn-primary" onclick="load(true,1)" data-dismiss="modal">确定</button>--%>
                </div>
            </div>
        </div>
    </div>
        <%--模态框：充值完毕回调--%>
    <div class="modal fade" id="myModal05" tabindex="-1" role="dialog" aria-labelledby="myModelLabel05">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header text-center">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <label class="modal-title" id="myModalLabel05"></label>
                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div id="printing" class="col-lg-12 table-d">
                            <table id="self_table"  class="table table-bordered" width="1000px" border="1" cellspacing="0" cellpadding="0">
                            </table>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <div style="text-align: center">
                                    <ul id="pageLimit5"></ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                         <input type="button" class="btn btn-primary" value="打印凭证" onclick="myPrint(document.getElementById('printing'))" />
                    <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>
<%--                    <button type="button" class="btn btn-primary" onclick="determin_recharge()" data-dismiss="modal">确定充值</button>--%>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
