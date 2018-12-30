var now_page = 1;
dic_head = {
    order_date: "日期",
    usr_no: "账号",
    card_no: "卡号",
    dept_ID: "部门名称",
    order_Price_Type: "餐次",
    order_not_count: "订餐未就餐数量",
    meal_types:"餐类"


}
$(document).ready(function () {
    //load(true, 1);
})

//加载页面
function load(isfirst, pc) {
    var beg_not = $("#beg_not").val();
    var end_not = $("#end_not").val();
    var name_not = $("#name_not").val();
    var dept_not = $("#dept_not").val();
    var price_type_not = $("#price_type_not").val();
    var spe_type_not = $("#spe_type_not").val();
    var jsonObj = {
        type: "Order_Not_load",
        beg_not: beg_not,
        end_not: end_not,
        name_not: name_not,
        dept_not: dept_not,
        price_type_not: price_type_not,
        spe_type_not: spe_type_not,
        pc: pc
    }
    if (isfirst) {
        now_page = 1;
    }
    else {
        now_page = pc;
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("MainBack.ashx", "json", postStr, callBack_load);

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
//导出订餐未就餐
function downNot() {
    var beg_not = $("#beg_not").val();
    var end_not = $("#end_not").val();
    var name_not = $("#name_not").val();
    var dept_not = $("#dept_not").val();
    var price_type_not = $("#price_type_not").val();
    var spe_type_not = $("#spe_type_not").val();
    var jsonObj = {
        type: "Order_Not_load",
        beg_not: beg_not,
        end_not: end_not,
        name_not: name_not,
        dept_not: dept_not,
        price_type_not: price_type_not,
        spe_type_not: spe_type_not,
        btn_type:"btn_Not"
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("MainBack.ashx", "json", postStr, callBack_Not);
}

function callBack_Not(data) {
    var urlArray = window.location.href.split("/");
    var fileUrl = urlArray[0] + "//" + urlArray[2] + "/download/" + data.files;
    window.open(fileUrl);
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
    //var th2 = document.createElement("th");
    //th2.innerHTML = "操作";
    //tr.appendChild(th2);
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
    //var td2 = document.createElement("td");
    //td2.innerHTML = checkStatus(data["ID"], data["ResourceID"]);
    //tr.appendChild(td2);

    return tr;
}
//判断操作是否为启用或停用
function checkStatus(isuse, feeid) {
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