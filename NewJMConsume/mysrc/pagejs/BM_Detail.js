var now_page = 1;
dic_head = {
    dept_ID: "部门",
    usr_name: "姓名",
    order_data: "订餐生成日期",
    order_Price_Type: "餐次",
    meal_types: "餐类",
    FeePlace: "消费地点"


}
$(document).ready(function () {
    //load(true, 1);
})

//加载页面
function load(isfirst, pc) {
    var check_detail = $("#check_detail").val();
    var price_type_detail = $("#price_type_detail").val();
    var place_detail = $("#place_detail").val();
    var name_detail = $("#name_detail").val();
    var jsonObj = {
        type: "BM_Detail_load",
        check_detail: check_detail,
        price_type_detail: price_type_detail,
        place_detail: place_detail,
        name_detail: name_detail,
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

//订餐明细导出excel

function downdetail() {
    var check_detail = $("#check_detail").val();
    var price_type_detail = $("#price_type_detail").val();
    var place_detail = $("#place_detail").val();
    var name_detail = $("#name_detail").val();
    var jsonObj = {
        type: "BM_Detail_load",
        check_detail: check_detail,
        price_type_detail: price_type_detail,
        place_detail: place_detail,
        name_detail: name_detail,
        btn_type:"BM_Detail"
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("MainBack.ashx", "json", postStr, callBack_down);
}
function callBack_down(data) {
    var urlArray = window.location.href.split("/");
    console.log(data);
    var fileUrl = urlArray[0] + "//" + urlArray[2] + "/download/" + data.files;
    window.open(fileUrl);
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