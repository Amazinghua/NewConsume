var now_page = 1;
dic_head = {
    Company: "公司",
    PlaceType: "消费地点",
    order_price_type: "餐次",
    ResourceName: "消费策略",
}
$(document).ready(function () {
    load(true, 1);
    load_company();
    load_feeSource();
})

//加载页面
function load(isfirst, pc) {
    var feeplace = $("#feeplace").val();
    var jsonObj = {
        type: "set_cant_load",
        feeplace: feeplace,
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
        if (data.execDt.length != 0) {
            generateTable(data.execDt, dic_head);
        }
        bindpaginator(data.numcount);
    }
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

//新建表
function generateTable(data, dic) {
    var table = document.getElementById("table");
    table.appendChild(generateTableThead(dic))
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
    tr.setAttribute("name", data["RoId"]);
    for (index in dic) {
        var td = document.createElement("td");

        td.innerHTML = data[index];
        tr.appendChild(td);

    }
    var td2 = document.createElement("td");
    td2.innerHTML = checkStatus(data["FeeID"]);
    tr.appendChild(td2);

    return tr;
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
//判断操作是否为启用或停用
function checkStatus(isuse) {
    var str = "";
    str = "<a onclick=\"showCant(this.name)\" name=" + isuse + " >编辑</a>|";
    str += "<a onclick=\"deleteRole(this.name)\" name=" + isuse + ">删除</a>";
    return str;
}



//添加食堂
function addRole() {
    var company = $("#addcompany").val();
    var eattype = $("#addeattype").val();
    var feeplace = $("#addfeeplace").val();
    var feeresource = $("#addfeeresource").val();
    var jsonObj = {
        type: "set_cant_add",
        company: company,
        eattype: eattype,
        feeplace: feeplace,
        feeresource: feeresource
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("SettingBack.ashx", "json", postStr, callBack_addRole);
}
//添加食堂回调
function callBack_addRole(data) {
    alert(data.msg);
    load(true, 1);
}
//删除饭堂
function deleteRole(id) {
    var jsonObj = {
        type: "set_cant_delete",
        id: id
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("SettingBack.ashx", "json", postStr, callBack_deleteCant);
}
//删除饭堂的回调
function callBack_deleteCant(data) {
    alert(data.msg);
    load(true, 1);
}
//显示更新状态的模态框
function showCant(id) {
    var jsonObj = {
        type: "set_cant_load",
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
        $("#myModalLabel").html("更新饭堂");
        $("#addcompany").val(table["Company"]);
        $("#addfeeplace").val(table["PlaceType"]);
        $("#addeattype").val(table["order_price_type"]);
        $("#addfeeresource").val(table["ResourceName"]);
        $("#changeBtn").attr("name", table["FeeID"]);
        $("#changeBtn").attr("onclick", "updateCant(this.name)");
        $('#myModal').modal('show');
    }
}

//更新字段
function updateCant(id) {
    var company = $("#addcompany").val();
    var eattype = $("#addeattype").val();
    var feeplace = $("#addfeeplace").val();
    var feeresource = $("#addfeeresource").val();
    if (company != "" && eattype != "" && feeplace != "" && feeresource != "") {
        var jsonObj = {
            type: "set_cant_update",
            company: company,
            eattype: eattype,
            feeplace: feeplace,
            feeresource: feeresource,
            id: id
        }
        var postStr = JSON.stringify(jsonObj);
        mj_ajax("SettingBack.ashx", "json", postStr, callBack_addRole);
    }
    else {
        alert("不可为空！")

    }
}
//更新字段的回调
function callBacl_updateCant(data) {
    if (data.result == "success" || data.result == "fail" || data.result == "error") {
        alert(data.msg);
    }

}

//新增食堂时显示模态框
function showAddCant() {
    $("#myModalLabel").html("新增饭堂");
    $("#addfeeplace").val("");
    $("#changeBtn").attr("onclick", "addRole()");
    $('#myModal').modal('show');

}


