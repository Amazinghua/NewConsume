var now_page = 1;
dic_head = {
    ResourceID: "策略编码",
    ResourceName: "策略名称",
    ReMark: "备注",
    IsModify: "是否可修改",
    IsUse: "是否启用"
}
$(document).ready(function () {
    load(true, 1);
})

//加载页面
function load(isfirst, pc) {
    var keyword = $("#keyword").val();
    var jsonObj = {
        type: "set_res_load",
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
    tr.setAttribute("name", data["ID"]);
    for (index in dic) {
        var td = document.createElement("td");

        td.innerHTML = data[index];
        tr.appendChild(td);

    }
    var td2 = document.createElement("td");
    td2.innerHTML = checkStatus(data["ID"], data["ResourceID"]);
    tr.appendChild(td2);

    return tr;
}
//判断操作是否为启用或停用
function checkStatus(isuse,feeid) {
    var str = "";
    str = "<a onclick=\"showCant(this.name)\" name=" + feeid + " >编辑</a>|";
    str += "<a onclick=\"deleteRole(this.name)\" name=" + isuse + ">删除</a>";
    return str;
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

//新增部门时显示模态框
function showAddCant() {
    $("#myModalLabel").html("新增策略");
    $("#addid").val("");
    $("#addname").val("");
    $("#memo").val("");
    $("#changeBtn").attr("onclick", "addRole()");
    $('#myModal').modal('show');

}

//添加策略
function addRole() {
    var id = $("#addid").val();
    var usrname = $("#addname").val();
    var memo = $("#memo").val();
    if (id != "" && usrname != "" && memo != "") {
        var jsonObj = {
            type: "set_res_add",
            id: id,
            usrname: usrname,
            memo: memo
        }
        var postStr = JSON.stringify(jsonObj);
        mj_ajax("SettingBack.ashx", "json", postStr, callBack_addRole);

    }
    else {
        alert("不可为空！");
    }

}
//添加策略回调
function callBack_addRole(data) {
    alert(data.msg);
    load(true, 1);
}

//删除人员
function deleteRole(id) {
    var jsonObj = {
        type: "set_res_delete",
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
    $("#change").attr("name",id)
    var jsonObj = {
        type: "set_res_det_load",
        id: id,
        pc: "1"
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("SettingBack.ashx", "json", postStr, callBack_showCant);

}
//显示更新状态的模态框的回调
function callBack_showCant(data) {
    if (data.result == "ok") {
        var table = data.execDt;
        if (table.length != 0) {
            for (var i = 0; i < table.length; i++) {
                $("#BasicAmount" + i).val(table[i]["BasicAmount"]);
                $("#ExtraFee" + i).val(table[i]["ExtraFee"]);
                $("#StrongPrice" + i).val(table[i]["StrongPrice"]);
                $("#NotOrderFee" + i).val(table[i]["NotOrderFee"]);

                $("#OrderResource2" + i).val(table[i]["OrderResource2"]);
                $('#OrderResource2' + i).selectpicker('refresh');
                $('#OrderResource2' + i).selectpicker('render');

                $("#FeeTimes" + i).val(table[i]["FeeTimes"]);
                $("#StrongFeeTimes" + i).val(table[i]["StrongFeeTimes"]);

                $("#CheckGroupOrderTime" + i).val(table[i]["CheckGroupOrderTime"]);
                $('#CheckGroupOrderTime' + i).selectpicker('refresh');
                $('#CheckGroupOrderTime' + i).selectpicker('render');
            }
        }
        else {
            for (var i = 0; i < 4; i++) {
                $("#BasicAmount" + i).val("0");
                $("#ExtraFee" + i).val("0");
                $("#StrongPrice" + i).val("0");
                $("#NotOrderFee" + i).val("0");

                $("#OrderResource2" + i).val("Default");
                $('#OrderResource2' + i).selectpicker('refresh');
                $('#OrderResource2' + i).selectpicker('render');

                $("#FeeTimes" + i).val("0");
                $("#StrongFeeTimes" + i).val("0");

                $("#CheckGroupOrderTime" + i).val("Default");
                $('#CheckGroupOrderTime' + i).selectpicker('refresh');
                $('#CheckGroupOrderTime' + i).selectpicker('render');
            }

        }

        $('#myModal02').modal('show');
    }
}

//更新字段
function updateCant(id) {
    //早餐
    var BasicAmount0 = $("#BasicAmount0").val();
    var ExtraFee0 = $("#ExtraFee0").val();
    var StrongPrice0 = $("#StrongPrice0").val();
    var NotOrderFee0 = $("#NotOrderFee0").val();
    var OrderResource20 = $("#OrderResource20").val();
    var FeeTimes0 = $("#FeeTimes0").val();
    var StrongFeeTimes0 = $("#StrongFeeTimes0").val();
    var CheckGroupOrderTime0 = $("#CheckGroupOrderTime0").val();
    //午餐
    var BasicAmount1 = $("#BasicAmount1").val();
    var ExtraFee1 = $("#ExtraFee1").val();
    var StrongPrice1= $("#StrongPrice1").val();
    var NotOrderFee1 = $("#NotOrderFee1").val();
    var OrderResource21= $("#OrderResource21").val();
    var FeeTimes1 = $("#FeeTimes1").val();
    var StrongFeeTimes1 = $("#StrongFeeTimes1").val();
    var CheckGroupOrderTime1 = $("#CheckGroupOrderTime1").val();
    //晚餐
    var BasicAmount2 = $("#BasicAmount2").val();
    var ExtraFee2 = $("#ExtraFee2").val();
    var StrongPrice2 = $("#StrongPrice2").val();
    var NotOrderFee2 = $("#NotOrderFee2").val();
    var OrderResource22 = $("#OrderResource22").val();
    var FeeTimes2 = $("#FeeTimes2").val();
    var StrongFeeTimes2 = $("#StrongFeeTimes2").val();
    var CheckGroupOrderTime2 = $("#CheckGroupOrderTime2").val();

    var jsonObj = {
        type: "set_res_det_add",
        info: [
            {
                PriceType: "早餐",
                BasicAmount: BasicAmount0,
                ExtraFee: ExtraFee0,
                StrongPrice: StrongPrice0,
                NotOrderFee: NotOrderFee0,
                OrderResource2: OrderResource20,
                FeeTimes: FeeTimes0,
                StrongFeeTimes: StrongFeeTimes0,
                CheckGroupOrderTime: CheckGroupOrderTime0
            },
            {
                PriceType: "午餐",
                BasicAmount: BasicAmount1,
                ExtraFee: ExtraFee1,
                StrongPrice: StrongPrice1,
                NotOrderFee: NotOrderFee1,
                OrderResource2: OrderResource21,
                FeeTimes: FeeTimes1,
                StrongFeeTimes: StrongFeeTimes1,
                CheckGroupOrderTime: CheckGroupOrderTime1
            },
            {
                PriceType: "晚餐",
                BasicAmount: BasicAmount2,
                ExtraFee: ExtraFee2,
                StrongPrice: StrongPrice2,
                NotOrderFee: NotOrderFee2,
                OrderResource2: OrderResource22,
                FeeTimes: FeeTimes2,
                StrongFeeTimes: StrongFeeTimes2,
                CheckGroupOrderTime: CheckGroupOrderTime2
            },
        ],
        id: id
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("SettingBack.ashx", "json", postStr, callBacl_updateCant);
}
//更新字段的回调
function callBacl_updateCant(data) {
    if (data.result == "success" || data.result == "fail" || data.result == "error") {
        alert(data.msg);
    }
}

