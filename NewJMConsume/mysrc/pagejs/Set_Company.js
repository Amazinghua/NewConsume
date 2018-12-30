var now_page = 1;
dic_head = {
    id: "序号",
    company_name: "公司",
    parent_id:"公司级别"
}
$(document).ready(function () {
    load(true, 1);
})



//添加公司
function addCompany() {
    var level = $("#companylevel").val();
    var companyname = $("#companyName").val();
    var jsonObj = {
        type: "set_com_add",
        companyname: companyname,
        companylevel:level
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("SettingBack.ashx", "json", postStr, callBack_addcom);
}
function callBack_addcom(data) {
    if (data.result == "success") {
        alert(data.msg);
        load(true, 1);
    }
}

//加载页面
function load(isfirst, pc) {
    var jsonObj = {
        type: "set_com_load",
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
    tr.setAttribute("name", data["id"]);
    for (index in dic) {
        var td = document.createElement("td");

        td.innerHTML = data[index];
        tr.appendChild(td);

    }
    var td2 = document.createElement("td");
    td2.innerHTML = checkStatus(data["id"]);
    tr.appendChild(td2);

    return tr;
}
//判断操作
function checkStatus(isuse) {
    var str = "";
    str = "<a onclick=\"deleteCompany(this.name)\" name=" + isuse + ">删除</a>";
    return str;
}

//删除公司
function deleteCompany(id) {
    var jsonObj = {
        type: "set_com_delete",
        com_id: id
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("SettingBack.ashx", "json", postStr, callBack_delete_Company);
}
function callBack_delete_Company(data) {
    if (data.result == "success") {
        alert("删除成功！");
        load(false, 1);
    }
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