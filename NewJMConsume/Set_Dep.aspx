<%@ Page Title="" Language="C#" MasterPageFile="~/Setting.Master" AutoEventWireup="true" CodeBehind="Set_Dep.aspx.cs" Inherits="NewJMConsume.Set_Dep" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="mysrc/js/fileInput/fileinput.css" rel="stylesheet" />
    <script src="mysrc/pagejs/Set_Dep.js"></script>
       <%-- <link href="tree/css/demo.css" rel="stylesheet" />--%>
    <link href="tree/css/zTreeStyle/zTreeStyle.css" rel="stylesheet" />
<%--    <link href="tree/css/metroStyle/metroStyle.css" rel="stylesheet" />--%>
        <script src="mysrc/commen_js/MJ_JSComment.js"></script>
    <script src="tree/js/jquery.ztree.core.js"></script>
    <script src="tree/js/jquery.ztree.exedit.js"></script>
    <script src="tree/js/TreeShow.js"></script>
    <script src="mysrc/js/fileInput/fileinput.js"></script>
    <script src="mysrc/js/fileInput/zh.js"></script>
    <script>
        $(document).ready(function () {

            $("#fileInput").fileinput({

                uploadUrl: 'Fileup.ashx?type=fileToDept',
                uploadAsync: true, //是否异步上传
                language: 'zh', //语言
                showCaption: true, //是否显示标题
                dropZoneEnabled: false, //是否显示拖拽区域
                showPreview: false, //是否显示预览区域
                enctype: 'multipart/form-data',
                //uploadExtraData: function (previewId, index) {
                //    var data = {
                //        type = "fileToSql"
                //    };
                //    return data;
                //},
                allowedFileExtensions: ['xlsx', 'xls'] //接收的文件后缀
            }).on('filebatchselected',
                function (event, data) {
                    if (data.length < 1) {
                        $('.fileinput-remove-button').click(); //点击移除按钮

                    }

                }).on("fileuploaded", function (event, data) {
                    alert("上传成功！")
                    //如果直接执行，会出现bug（页面显示取消按钮，选择按钮变为不可点击），所以延时一定时间再执行
                    setTimeout(function () { $('.fileinput-remove-button').click(); }, 250);
                    //}
                }).on("fileerror", function (event, data) {
                    alert("上传部门出现错误！");
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
                <a href="#">设置</a>
            </li>
            <li class="active">部门设置</li>
        </ul>
        <!-- /.breadcrumb -->
    </div>

    <div class="page-content">
        <div class="page-header">
            <h1>部门设置
            </h1>
        </div>
        <!-- /.page-header -->

        <div class="row">
            <div class="col-lg-12">
                <%-- 输入行--%>
                <div class="row">
                    <div class="col-lg-12 form-inline">
<%--                        <div class="form-group">
                            <label>部门类别：</label>
                        </div>
                        <div class="form-group">

                            <select class="form-control" id="level" data-width="auto">
                                <option value="">全部</option>
                                <option value="1">1级</option>
                                <option value="2">2级</option>
                                <option value="3">3级</option>
                            </select>
                        </div>
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="搜索" onclick="load(true, 1)";/>
                        </div>
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="单个新增" onclick="showAddCant()"/>
                        </div>--%>
<%--                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="批量新增" onclick="dosave()" />
                        </div>--%>
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="修改名称" onclick="test()" />
                        </div>
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="删除" onclick="del()" />
                        </div>
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="添加部门" onclick="add()" />
                        </div>
                    </div>
                </div>

                <%--表格--%>
                <div class="row" style="margin-top: 2%">
                    <div class="col-lg-2">
<%--                        <table class="table table-bordered" id="table">
                        </table>--%>
                        <ul id="treeDemo" class="ztree"></ul>
                    </div>
                    <div class="col-lg-10">
                        <table class="table table-bordered" id="table">

                        </table>
                    </div>
                </div>


                <%--分页--%>
                <div class="row">
                    <div class="col-lg-12">
                         <div id="example" style="text-align: center"> <ul id="pageLimit"></ul> </div>
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
                    <label class="modal-title" id="myModalLabel">新增部门</label>
                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div class="row" style="margin-top: 1%">
                            <label class="col-lg-2">上级公司</label>
                            <select class="selectpicker" id="addcompany">
                            </select>
                        </div>
                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">部门名称</label>
                            <input type="text" class="form-control col-lg-10" id="adddept"/>
                        </div>
<%--                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">部门层级</label>
                            <select class="selectpicker" id="addlevel">
                                <option value="1">1</option>
                                <option value="2">2</option>
                                <option value="3">3</option>
                            </select>
                        </div>--%>
                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">消费策略</label>
                            <select class="selectpicker" id="addfeeresource">
                            </select>
                        </div>
                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">消费地点</label>
                            <select class="selectpicker" id="fee">
                            </select>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>
                    <button type="button" class="btn btn-primary" onclick="addRole()" data-dismiss="modal" id="changeBtn">确定</button>
                </div>
            </div>
        </div>
    </div>
        <%--模态框：批量新增 部门设置--%>
    <div class="modal fade" id="myModal02" tabindex="-1" role="dialog" aria-labelledby="myModelLabel02">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header text-center">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <label class="modal-title" id="myModalLabel02">批量新增-部门</label>
                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div class="row form-line">
                            <input id="fileInput" name="fileInput" type="file">
                        </div>
                        <div>
                            <label>下载模板:</label>
                            <a href="uploadDept/批量上传_部门（模板）.xls">批量上传-部门(模板)</a>
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
</asp:Content>
