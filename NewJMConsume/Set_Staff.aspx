<%@ Page Title="" Language="C#" MasterPageFile="~/Setting.Master" AutoEventWireup="true" CodeBehind="Set_Staff.aspx.cs" Inherits="NewJMConsume.Set_Staff" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="mysrc/js/fileInput/fileinput.css" rel="stylesheet" />
    <script src="mysrc/pagejs/Set_Staff.js"></script>
    <script src="mysrc/pagejs/test.js"></script>
    <script src="mysrc/js/fileInput/fileinput.js"></script>
    <script src="mysrc/js/fileInput/zh.js"></script>
    <script>
        $(document).ready(function () {
            load(true, 1);
            load_deptID();
            load_company();
            load_feeSource();
            load_feeplace();
            $("#fileInput").fileinput({

                uploadUrl: 'Fileup.ashx?type=fileToSql',
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
                    //if (data.response.MessageType === 'success') {
                    //如果直接执行，会出现bug（页面显示取消按钮，选择按钮变为不可点击），所以延时一定时间再执行
                    setTimeout(function () { $('.fileinput-remove-button').click(); }, 250);
                    //}
                    alert("上传成功!");
                }).on("fileerror", function (event, data) {
                    alert("上传人员出现错误！");
                    //setTimeout(function () { $('.fileinput-remove-button').click(); }, 250);
                });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadcrumbs ace-save-state" id="breadcrumbs" style="padding-top: 0 0 0 0">
        <ul class="breadcrumb">
            <li>
                <i class="ace-icon fa fa-home home-icon"></i>
                <a href="#">设置</a>
            </li>
            <li class="active">人员设置</li>
        </ul>
        <!-- /.breadcrumb -->
    </div>


    <div class="page-content">
        <div class="page-header">
            <h1>人员设置
            </h1>
        </div>
        <!-- /.page-header -->

        <div class="row">
            <div class="col-lg-12">
                <%-- 输入行--%>
                <div class="row">
                    <div class="col-lg-12 form-inline">
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="批量新增" onclick="dosave();" />
                        </div>
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="单个新增" onclick="showAddCant()" />
                        </div>
                        <div class="form-group">
                            <input type="text" class="form-control" placeholder="关键字" id="keyword" />
                        </div>
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="查询" onclick="load(true, 1);" />
                        </div>
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="导 出" onclick="downStaff();" />
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


    <%--模态框：单个新增--%>
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header text-center">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <label class="modal-title" id="myModalLabel">新增人员</label>
                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">平台帐号</label>
                            <input type="text" class="form-control col-lg-10" id="addusrno" />
                        </div>
                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">用户姓名</label>
                            <input type="text" class="form-control col-lg-10" id="addusrname" />
                        </div>
                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">手机号码</label>
                            <input type="text" class="form-control col-lg-10" id="addphone" />
                        </div>
                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">员工卡号</label>
                            <input type="text" class="form-control col-lg-10" id="addcard" />
                        </div>
                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">饭卡余额</label>
                            <input type="text" class="form-control col-lg-10" id="addmoney" readonly="readonly" />
                        </div>
                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">员工备注</label>
                            <input type="text" class="form-control col-lg-10" id="memo" />
                        </div>
                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">饭卡状态</label>
                            <label class="checkbox-inline">
                                <input type="radio" value="1" name="isuse" />启用
                            </label>
                            <label class="checkbox-inline">
                                <input type="radio" value="0" name="isuse" />禁用
                            </label>
                        </div>

                        <%--                                                <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">用户角色</label>
                            <select class="selectpicker" id="addusrrole">
                               <option selected="selected">请选择！</option>
                            </select>--%>
                        <%--             <input type="text" class="form-control" id="addusrrole" placeholder="请输入角色名称" />--%>
                        <%-- </div>--%>
                        <div class="row" style="margin-top: 1%">
                            <label class="col-lg-2">所属部门</label>
                            <select class="selectpicker" id="adddept">
                            </select>
                        </div>
                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">消费策略编码</label>
                            <select class="selectpicker" id="addfeeresource">
                            </select>
                        </div>
                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">消费地点</label>
                            <select class="selectpicker" id="fee">
                            </select>
                        </div>
                        <div class="row" style="margin-top: 1%">
                            <label class="col-lg-2">所属公司</label>
                            <select class="selectpicker" id="addcompany">
                            </select>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>
                    <button type="button" class="btn btn-primary" data-dismiss="modal" id="changeBtn">确定</button>
                </div>
            </div>
        </div>
    </div>
    <%--模态框：批量新增--%>
    <div class="modal fade" id="myModal02" tabindex="-1" role="dialog" aria-labelledby="myModelLabel02">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header text-center">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <label class="modal-title" id="myModalLabel02">批量新增-人员</label>
                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div class="row form-line">
                            <input id="fileInput" name="fileInput" type="file">
                        </div>
                        <div>
                            <label>下载模板: </label>
                            <a href="uploadStaff/批量新增_人员_模板_.xls">批量新增-(人员)模板.xls</a>
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
