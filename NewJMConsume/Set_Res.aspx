<%@ Page Title="" Language="C#" MasterPageFile="~/Setting.Master" AutoEventWireup="true" CodeBehind="Set_Res.aspx.cs" Inherits="NewJMConsume.Set_Res" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="mysrc/pagejs/Set_Res.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadcrumbs ace-save-state" id="breadcrumbs">
        <ul class="breadcrumb">
            <li>
                <i class="ace-icon fa fa-home home-icon"></i>
                <a href="#">设置</a>
            </li>
            <li class="active">消费策略设置</li>
        </ul>
        <!-- /.breadcrumb -->
    </div>

    <div class="page-content">
        <div class="page-header">
            <h1>消费策略设置
            </h1>
        </div>
        <!-- /.page-header -->

        <div class="row">
            <div class="col-lg-12">
                <%-- 输入行--%>
                <div class="row">
                    <div class="col-lg-12 form-inline">
                        <div class="form-group">
                            <input type="text" class="form-control" placeholder="关键字" id="keyword" />
                        </div>

                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="搜索" onclick="load(true, 1)" />
                        </div>
                        <div class="form-group">
                            <input type="button" class="btn btn-primary" value="新增" onclick="showAddCant()" />
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
                    <label class="modal-title" id="myModalLabel">新增策略</label>
                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">策略编码</label>
                            <input type="text" class="form-control col-lg-10" id="addid" />
                        </div>
                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">策略名称</label>
                            <input type="text" class="form-control col-lg-10" id="addname" />
                        </div>
                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">是否启用</label>
                            <input type="text" class="form-control col-lg-10" id="addisuse" value="1" readonly="readonly" />
                        </div>
                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">是否修改</label>
                            <input type="text" class="form-control col-lg-10" id="addismodify" value="1" readonly="readonly" />
                        </div>
                        <div class="row form-inline" style="margin-top: 1%">
                            <label class="col-lg-2">策略说明</label>
                            <input type="text" class="form-control col-lg-10" id="memo" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>
                    <button type="button" class="btn btn-primary" onclick="addRole()"  id="changeBtn">确定</button>
                </div>
            </div>
        </div>
    </div>



    <%--策略详细的模态框--%>
    <div class="modal fade" id="myModal02" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header text-center">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <label class="modal-title" id="myModalLabel02">消费策略设置</label>
                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-lg-12">
                                <table class="table table-bordered">
                                    <thead>
                                        <tr>
                                            <th>餐次</th>
                                            <th>基础金额</th>
                                            <th>附加费</th>
                                            <th>强制消费</th>
                                            <th>订餐未就餐</th>
                                            <th>非订餐附加费策略</th>
                                            <th>消费次数</th>
                                            <th>强制消费次数</th>
                                            <th>付费类型</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>早餐</td>
                                            <td class="form-inline">
                                                <input type="text" class="form-control" style="width: 50px;" id="BasicAmount0" />
                                            </td>
                                            <td class="form-inline">
                                                <input type="text" class="form-control" style="width: 50px" id="ExtraFee0" />
                                            </td>
                                            <td class="form-inline">
                                                <input type="text" class="form-control" style="width: 50px" id="StrongPrice0" />
                                            </td>
                                            <td class="form-inline">
                                                <input type="text" class="form-control" style="width: 50px" id="NotOrderFee0" />
                                            </td>
                                            <td class="form-inline">
                                                <select class="selectpicker" id="OrderResource20" data-width="auto">
                                                    <option value="Default">禁用附加费</option>
                                                    <option value="NotOrderExtra">不订餐消费收附加费</option>
                                                    <option value="NotOrderNoFee">不订餐不给消费</option>
                                                    <option value="Continue">强制收附加费</option>
                                                </select>
                                            </td>
                                            <td class="form-inline">
                                                <input type="text" class="form-control" style="width: 50px" id="FeeTimes0" />

                                            </td>
                                            <td class="form-inline">
                                                <input type="text" class="form-control" style="width: 50px" id="StrongFeeTimes0" />
                                            </td>
                                            <td class="form-inline">
                                                <select class="selectpicker" data-width="auto" id="CheckGroupOrderTime0">
                                                    <option value="1">预付费</option>
                                                    <option value="0">免费</option>
                                                </select>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>午餐</td>
                                            <td class="form-inline">
                                                <input type="text" class="form-control" style="width: 50px;" id="BasicAmount1" />
                                            </td>
                                            <td class="form-inline">
                                                <input type="text" class="form-control" style="width: 50px" id="ExtraFee1" />
                                            </td>
                                            <td class="form-inline">
                                                <input type="text" class="form-control" style="width: 50px" id="StrongPrice1" />
                                            </td>
                                            <td class="form-inline">
                                                <input type="text" class="form-control" style="width: 50px" id="NotOrderFee1" />
                                            </td>
                                            <td class="form-inline">
                                                <select class="selectpicker" id="OrderResource21" data-width="auto">
                                                    <option value="Default">禁用附加费</option>
                                                    <option value="NotOrderExtra">不订餐消费收附加费</option>
                                                    <option value="NotOrderNoFee">不订餐不给消费</option>
                                                    <option value="Continue">强制收附加费</option>
                                                </select>
                                            </td>
                                            <td class="form-inline">
                                                <input type="text" class="form-control" style="width: 50px" id="FeeTimes1" />

                                            </td>
                                            <td class="form-inline">
                                                <input type="text" class="form-control" style="width: 50px" id="StrongFeeTimes1" />
                                            </td>
                                            <td class="form-inline">
                                                <select class="selectpicker" data-width="auto" id="CheckGroupOrderTime1">
                                                    <option value="1">预付费</option>
                                                    <option value="0">免费</option>
                                                </select>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>晚餐</td>
                                            <td class="form-inline">
                                                <input type="text" class="form-control" style="width: 50px;" id="BasicAmount2" />
                                            </td>
                                            <td class="form-inline">
                                                <input type="text" class="form-control" style="width: 50px" id="ExtraFee2" />
                                            </td>
                                            <td class="form-inline">
                                                <input type="text" class="form-control" style="width: 50px" id="StrongPrice2" />
                                            </td>
                                            <td class="form-inline">
                                                <input type="text" class="form-control" style="width: 50px" id="NotOrderFee2" />
                                            </td>
                                            <td class="form-inline">
                                                <select class="selectpicker" id="OrderResource22" data-width="auto">
                                                    <option value="Default">禁用附加费</option>
                                                    <option value="NotOrderExtra">不订餐消费收附加费</option>
                                                    <option value="NotOrderNoFee">不订餐不给消费</option>
                                                    <option value="Continue">强制收附加费</option>
                                                </select>
                                            </td>
                                            <td class="form-inline">
                                                <input type="text" class="form-control" style="width: 50px" id="FeeTimes2" />

                                            </td>
                                            <td class="form-inline">
                                                <input type="text" class="form-control" style="width: 50px" id="StrongFeeTimes2" />
                                            </td>
                                            <td class="form-inline">
                                                <select class="selectpicker" data-width="auto" id="CheckGroupOrderTime2">
                                                    <option value="1">预付费</option>
                                                    <option value="0">免费</option>
                                                </select>
                                            </td>
                                        </tr>
                                                                                <tr>
                                            <td>宵夜</td>
                                            <td class="form-inline">
                                                <input type="text" class="form-control" style="width: 50px;" id="BasicAmount3" />
                                            </td>
                                            <td class="form-inline">
                                                <input type="text" class="form-control" style="width: 50px" id="ExtraFee3" />
                                            </td>
                                            <td class="form-inline">
                                                <input type="text" class="form-control" style="width: 50px" id="StrongPrice3" />
                                            </td>
                                            <td class="form-inline">
                                                <input type="text" class="form-control" style="width: 50px" id="NotOrderFee3" />
                                            </td>
                                            <td class="form-inline">
                                                <select class="selectpicker" id="OrderResource23" data-width="auto">
                                                    <option value="Default">禁用附加费</option>
                                                    <option value="NotOrderExtra">不订餐消费收附加费</option>
                                                    <option value="NotOrderNoFee">不订餐不给消费</option>
                                                    <option value="Continue">强制收附加费</option>
                                                </select>
                                            </td>
                                            <td class="form-inline">
                                                <input type="text" class="form-control" style="width: 50px" id="FeeTimes3" />

                                            </td>
                                            <td class="form-inline">
                                                <input type="text" class="form-control" style="width: 50px" id="StrongFeeTimes3" />
                                            </td>
                                            <td class="form-inline">
                                                <select class="selectpicker" data-width="auto" id="CheckGroupOrderTime3">
                                                    <option value="1">预付费</option>
                                                    <option value="0">免费</option>
                                                </select>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>
                    <button type="button" class="btn btn-primary" onclick="updateCant(this.name)" data-dismiss="modal" id="change">确定</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
