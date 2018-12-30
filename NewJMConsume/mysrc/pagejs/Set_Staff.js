var now_page = 1;
dic_head = {
    Company: "公司",
    usr_no: "帐号",
    usr_name: "姓名",
    dept_ID: "部门",
    card_no: "卡号",
    card_state: "饭卡状态",
    phone_no: "号码",
    memo: "备注",
    FeeResource: "消费策略",
    FeePlace: "消费地点"
}
//$(document).ready(function () {
//    load(true, 1);

//})

//加载页面
function load(isfirst, pc) {
    var keyword = $("#keyword").val();
    var jsonObj = {
        type: "set_staff_load",
        keyword: keyword,
        pc: pc
    }
    if (isfirst) {
        now_page = 1;
    }
    else {
        now_page = pc;
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("SettingBack.ashx", "json", postStr, callBack_load);

}
//加载页面的回调
function callBack_load(data) {
    if (data.result == "ok") {
        $("#table").empty();
        var table = document.getElementById("table");
        table.appendChild(generateTableThead(dic_head))
        if (data.execDt.length != 0) {
            generateTable(data.execDt, dic_head, table);
        }
        bindpaginator(data.numcount);
    }
}
function downStaff() {
    var keyword = $("#keyword").val();
    var jsonObj = {
        type: "set_staff_load",
        keyword: keyword,
        btn_type:"Set_Staff"
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("SettingBack.ashx", "json", postStr, callBack_staff);
}
function callBack_staff(data) {
    var urlArray = window.location.href.split("/");
    var fileUrl = urlArray[0] + "//" + urlArray[2] + "/download/" + data.files;
    window.open(fileUrl);
}
function loadRole() {
    $.ajax({
        type: "Get",
        url: "Fileup.ashx?type=load_role",

        data: {},
        success: function (data) {
           
            
            var data = JSON.parse(data);
            var d = data.dt;
            for (var i = 0; i < d.length; i++) {
                var roId = d[i].RoId;//获取到索引为i的
                var roName = d[i].RoName;//获取索引为i的名
                var opt = "<option value='" + roId + "'>" + roName + "</option>";
                $("#adddept").append(opt);//将opt填充到select中
                $("#adddept").selectpicker('refresh');
            }

        }
    });
}
function load_deptID() {
    $.ajax({
        type: "Get",
        url: "Fileup.ashx?type=load_deptID",

        data: {},
        success: function (data) {


            var data = JSON.parse(data);
            var d = data.dt;
            for (var i = 0; i < d.length; i++) {
                var deptId = d[i].Dept_Id;//获取到索引为i的
                var deptName = d[i].Dept_Name;//获取索引为i的名
                var opt = "<option value='" + deptId + "'>" + deptName + "</option>";
                $("#adddept").append(opt);//将opt填充到select中
                $("#adddept").selectpicker('refresh');
            }

        }
    });
}
function load_company() {
    $.ajax({
        type: "Get",
        url: "Fileup.ashx?type=load_company",

        data: {},
        success: function (data) {


            var data = JSON.parse(data);
            var d = data.dt;
            for (var i = 0; i < d.length; i++) {
                var comId = d[i].id;//获取到索引为i的
                var comName = d[i].company_name;//获取索引为i的名
                var opt = "<option value='" + comName + "'>" + comName + "</option>";
                $("#addcompany").append(opt);//将opt填充到select中
                $("#addcompany").selectpicker('refresh');
            }

        }
    });
}
function load_feeSource() {
    $.ajax({
        type: "Get",
        url: "Fileup.ashx?type=load_feeSource",

        data: {},
        success: function (data) {


            var data = JSON.parse(data);
            var d = data.dt;
            for (var i = 0; i < d.length; i++) {
                var resval = d[i].ResourceID;
                var ResName = d[i].ResourceName;//获取索引为i的名
                var opt = "<option value='" + resval + "'>" + ResName + "</option>";
                $("#addfeeresource").append(opt);//将opt填充到select中
                $("#addfeeresource").selectpicker('refresh');
            }

        }
    });
}
function load_feeplace() {
    $.ajax({
        type: "Get",
        url: "Fileup.ashx?type=load_feeplace",

        data: {},
        success: function (data) {


            var data = JSON.parse(data);
            var d = data.dt;
            for (var i = 0; i < d.length; i++) {
                var placeval = d[i].FeePlace;
                var PlaceName = d[i].PlaceType;//获取索引为i的名
                var opt = "<option value='" + placeval + "'>" + PlaceName + "</option>";
                $("#fee").append(opt);//将opt填充到select中
                $("#fee").selectpicker('refresh');
            }

        }
    });
}

//新建表
function generateTable(data, dic, table) {
    var tbody = document.createElement("tbody");
    for (var i = 0; i < data.length; i++) {
        tbody.appendChild(generateTableRow(data[i], dic, i));
    }
    table.appendChild(tbody);
}
//制作表头
function generateTableThead(dic) {
    var thead = document.createElement("thead");
    var tr = document.createElement("tr");
    for (index in dic) {
        var th = document.createElement("th");
        th.innerHTML = dic[index];
        tr.appendChild(th);
    }
    var th2 = document.createElement("th");
    th2.innerHTML = "操作";
    tr.appendChild(th2);
    thead.appendChild(tr);
    return thead;
}
//新建一行
function generateTableRow(data, dic, i) {
    var tr = document.createElement("tr");
    tr.setAttribute("name", data["ust_ID"]);
    for (index in dic) {
        var td = document.createElement("td");
        if (index == "card_state") {
            console.log(index);
            if (data[index] == 1) {
                td.innerHTML = "启用";
            } else if (data[index] == 0) {
                td.innerHTML = "禁用";
            }
        } else {
            td.innerHTML = data[index];
        }
        tr.appendChild(td);

    }
    var td2 = document.createElement("td");
    td2.innerHTML = checkStatus(data["ust_ID"]);
    tr.appendChild(td2);

    return tr;
}
//判断操作是否为启用或停用
function checkStatus(isuse) {
    var str = "";
    str = "<a onclick=\"showCant(this.name)\" name=" + isuse + " >编辑</a>|";
    str += "<a onclick=\"deleteRole(this.name)\" name=" + isuse + " >删除</a>";
    return str;
}
function getNextElement(node) {
    if (node.nextSibling.nodeType === 1) { //判断下一个节点类型为1则已反馈“元素”节点
        return node.nextSibling;
    }
    if (node.nextSibling.nodeType === 3) { //判断下一个节点类型为3则已反馈“文本”节点  ，回调自身函数
        return getNextElement(node.nextSibling);
    }
    return null;
}
function getChecked() {
    var obj = new Object();
    var arr = new Array();
    $("td input[type='checkBox']").each(function () {
        if ($(this).is(':checked')) {
            //bootstrap会在checkbox的input外层生成一个div
            var row = $(this).parent("td").parent("tr");//获取选中行
            //console.log(row);
            //parent函数返回的已反馈数组，所以需要用索引指定
            //var nextNode = getNextElement(checkBoxTdNode[0]);

            var CheckedCode = row.attr("name");
            arr.push(CheckedCode);
        }
    });
    obj.CheckedCode = arr;
    var json = JSON.stringify(obj);
    console.log(json);
    return json;
}
//分页插件
function bindpaginator(tatal) {
    var tp = Math.ceil(tatal / 10)
    $('#pageLimit').bootstrapPaginator({
        currentPage: now_page,//当前的请求页面。
        totalPages: tp,//一共多少页。
        size: "normal",//应该是页眉的大小。
        bootstrapMajorVersion: 3,//bootstrap的版本要求。
        alignment: "right",
        numberOfPages: 10,//一页列出多少数据。
        itemTexts: function (type, page, current) {//如下的代码是将页眉显示的中文显示我们自定义的中文。
            switch (type) {
                case "first": return "首页";
                case "prev": return "上一页";
                case "next": return "下一页";
                case "last": return "末页";
                case "page": return page;
            }
        },
        onPageClicked: function (event, originalEvent, type, page) {
            load(false, page);
        }
    });

}
//批量新增时显示模态框
dosave = function () {
    $("#myModal02").modal('show');
}

//批量导入人员excel表-存入数据库
function upfileformdata() {
    var fileM = document.querySelector("#fileUp");

    //获取文件对象，files是文件选取控件的属性，存储的是文件选取控件选取的文件对象，类型是一个数组
    var fileObj = fileM.files[0];
    //创建formdata对象，formData用来存储表单的数据，表单数据时以键值对形式存储的。
    var formData = new FormData();
    formData.append('file', fileObj);
    formData.append('type', 'fileToSql');
    $.ajax({
        type: "post",
        url: "SettingBack.ashx",
        data: formData,
        async: false,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            alert(data);

        }
    });
}

//新增部门时显示模态框

function showAddCant() {
    $("#myModalLabel").html("新增人员");
    $("#addusrno").val("");
    $("#addusrname").val("");
    $("#addphone").val("");
    $("#addcard").val("");
    $("#addfeeplace").val("");
    $("#changeBtn").attr("onclick", "addRole()");
    $('#myModal').modal('show');

}

//添加人员
function addRole() {
    var usrno = $("#addusrno").val();
    var usrname = $("#addusrname").val();
    var phone = $("#addphone").val();
    var card = $("#addcard").val();
    var dept = $("#adddept").val();
    var card_state = $("input[name='isuse']:checked").val();
    var usrrole = $("#addusrrole").val();
    var dept = $("#adddept").val();
    var feeresource = $("#addfeeresource").val();
    var fee = $("#fee").val();
    var memo = $("#memo").val();
    var company = $("#addcompany").val();
    var jsonObj = {
        type: "set_staff_add",
        company: company,
        usrno: usrno,
        usrname: usrname,
        phone: phone,
        card: card,
        dept: dept,
        card_state: card_state,
        usrrole: usrrole,
        feeresource: feeresource,
        feeplace: fee,
        memo: memo
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("SettingBack.ashx", "json", postStr, callBack_addRole);
}
//添加食堂回调
function callBack_addRole(data) {
    alert(data.msg);
    load(true, 1);
}

//删除人员
//function deleteRole(id) {
//    var jsonObj = {
//        type: "set_staff_delete",
//        id: id
//    }
//    var postStr = JSON.stringify(jsonObj);
//    mj_ajax("SettingBack.ashx", "json", postStr, callBack_deCant);
//}
////删除饭堂的回调
//function callBack_deCant(data) {
//    alert("asda");
//    load(true, 1);
//}

//显示更新状态的模态框
function showCant(id) {
    var jsonObj = {
        type: "set_staff_load",
        id: id,
        pc: "1"
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("SettingBack.ashx", "json", postStr, callBack_showCant);
}
//显示更新状态的模态框的回调
function callBack_showCant(data) {
    if (data.result == "ok") {
        var table = data.execDt[0];
        $("#myModalLabel").html("更新人员");
        $("#addusrno").val(table["usr_no"]);
        $("#addusrno").attr("readonly", "readonly");

        $("#addusrname").val(table["usr_name"]);
        $("#addusrname").attr("readonly", "readonly");

        $("#addphone").val(table["phone_no"]);

        $("#addcard").val(table["card_no"]);
        $("#addcard").attr("readonly", "readonly");

        $("#memo").val(table["memo"]);

        $("#addmoney").val(table["card_money"]);
        $("#addmoney").attr("readonly", "readonly");

        $("#addusrrole").val(table["usr_RoId"]);

        $("#addfeeresource").val(table["FeeResource"]);
        $("#addfeeresource").attr("readonly", "readonly");
        $("#addcompany").val(table["Company"]);
        // 刷新，公司下拉框
        $('#addcompany').selectpicker('refresh');
        $('#addcompany').selectpicker('render');

        $("#changeBtn").attr("name", table["ust_ID"]);
        $("#changeBtn").attr("onclick", "updateCant(this.name)");


        $('#myModal').modal('show');
    }
}

//更新字段
function updateCant(id) {
    var phone = $("#addphone").val();
    var dept = $("#adddept").val();
    var card_state = $("input[name='isuse']:checked").val();
    //var usrrole = $("#addusrrole").val();
    var dept = $("#adddept").val();
    var feeresource = $("#addfeeresource").val();
    var fee = $("#fee").val();
    var memo = $("#memo").val();
    var company = $("#addcompany").val();
    var usr_state = $('input:radio[name="isuse"]:checked').val();
    var jsonObj = {
        type: "set_staff_update",
        company: company,
        phone: phone,
        dept: dept,
        card_state: card_state,
        feeresource: feeresource,
        feeplace: fee,
        memo: memo,
        id: id
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("SettingBack.ashx", "json", postStr, callBacl_updateCant);
}
//更新字段的回调
function callBacl_updateCant(data) {
    if (data.result == "success" || data.result == "fail" || data.result == "error") {
        alert(data.msg);
        load(true, 1);
    }
}

