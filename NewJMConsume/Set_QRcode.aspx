<%@ Page Title="" Language="C#" MasterPageFile="~/Setting.Master" AutoEventWireup="true" CodeBehind="Set_QRcode.aspx.cs" Inherits="NewJMConsume.WebForm2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="mysrc/js/fileInput/fileinput.css" rel="stylesheet" />
    <script src="mysrc/pagejs/Set_QRcode.js"></script>
    <script src="mysrc/commen_js/qrcode.min.js"></script>
    <script src="mysrc/js/fileInput/fileinput.js"></script>
    <script src="mysrc/js/fileInput/zh.js"></script>
    <script>
        $(document).ready(function () {
            $("#fileInput").fileinput({

                uploadUrl: 'Fileup.ashx?type=fileToQR',
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
                    load(true, 1);
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
                <a href="#">设置</a>
            </li>
            <li class="active">二维码设置</li>
        </ul>
        <!-- /.breadcrumb -->
    </div>

    <div class="page-content">
        <div class="page-header">
            <h1>二维码设置
            </h1>
        </div>
        <!-- /.page-header -->
        <div class="row">
            <div class="col-lg-12">
                <%-- 输入行--%>
                <div class="row">
                    <div class="col-lg-12 form-inline">
                        <label>日期:</label>
                        <div class="form-group">
                            <div id="sandbox-container" style="margin-left: 10px">
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
                            <label>姓名:</label>
                            <div class="form-group">
                                <div id="name-container" style="margin-left: 10px">
                                    <input type="text" class="form-control" id="StaffName" style="width: 100px" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group" style="margin-left: 10px;">
                            <input type="button" class="btn btn-primary" onclick="load(true, 1)" value="查  询" />
                        </div>
                        <div class="form-group" style="margin-left: 40px;">
                            <input type="button" class="btn btn-primary" onclick="showAddQR()" value="单个新增" />
                        </div>
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" onclick="showUpQR()" value="批量新增" />
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

    <%--模态框--%>
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header text-center">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <label class="modal-title" id="myModalLabel">新增二维码</label>
                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">所属公司</label>
                            <input type="text" class="form-control col-lg-10" id="Company_owner" />
                        </div>
                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">姓  名</label>
                            <input type="text" class="form-control col-lg-10" id="Staff_name" />
                        </div>
                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">手机号码</label>
                            <input type="text" class="form-control col-lg-10" id="Staff_phone" />
                        </div>
                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">职 称</label>
                            <input type="text" class="form-control col-lg-10" id="Profession" />
                        </div>
                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">有效期至</label>
                            <input type="text" class="form-control col-lg-10" id="Expiry_time" />
                        </div>
                        <script type="text/javascript">
                            $('#Expiry_time').datetimepicker({
                                minView: "month",
                                language: 'zh-CN',
                                format: "yyyy-mm-dd hh:ii:ss"
                            })
                        </script>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>
                    <button type="button" class="btn btn-primary" onclick="addQR()" data-dismiss="modal">确定</button>
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
                    <label class="modal-title" id="myModalLabel02">批量新增-二维码</label>
                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div class="row form-line">
                            <input id="fileInput" name="fileInput" type="file">
                        </div>
                        <div>
                            <label>下载模板: </label>
                            <a href="uploadQR/批量生成二维码模板.xls">批量生成二维码模板.xls</a>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>
                    <button type="button" class="btn btn-primary" onclick="upfileformdata()" data-dismiss="modal" id="changeBtn02">确定</button>
                </div>
            </div>
        </div>
    </div>
    <%--模态框--%>
    <div class="modal fade" id="myModal03" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header text-center">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <label class="modal-title" id="myModalLabel03">二维码-详情</label>
                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div id="printimg">
                            <div id="id_QR" style="margin: 0 auto; height: 300px; width: 270px">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">

                    <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>
                    <input type="button" class="btn btn-primary" value="打 印" onclick="myPrint(document.getElementById('printimg'))" />
                    <button type="button" class="btn btn-primary" data-dismiss="modal">确定</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
