var now_page = 1;
dic_head = {
    usr_no: "帐号",
    card_no:"卡号",
    add_money: "充值金额",
    minus_money: "扣减金额",
    acc_money: "实充金额",
    method: "充值途径",
    create_date: "创建时间",
    creator: "创建人",
    ReMark: "备注"
}
$(document).ready(function () {
    load(true, 1);
})

//加载页面
function load(isfirst, pc) {
    var btime = $("#btime").val();
    var etime = $("#etime").val();
    var usrname = $("#usrname").val();
    var card = $("#card").val();//卡号
    var method = $("#method option:selected").val();//充值途径
    var creator = $("#creator").val();//充值人员
    var jsonObj = {
        type: "load",
        table: "tab_add_money",
        time: {
            name: "create_date",
            btime: btime,
            etime: etime
        },
        items: {
            usr_no: usrname,
            card_no: card,
            method: method,
            creator: creator
        },
        keyword: {
            
        },
        id: "add_id",
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

function downdetail() {
    var btime = $("#btime").val();
    var etime = $("#etime").val();
    var usrname = $("#usrname").val();
    var card = $("#card").val();//卡号
    var method = $("#method option:selected").val();//充值途径
    var creator = $("#creator").val();//充值人员
    var jsonObj = {
        type: "load",
        btn_type:"TU_Detail",
        table: "tab_add_money",
        time: {
            name: "create_date",
            btime: btime,
            etime: etime
        },
        items: {
            usr_no: usrname,
            card_no: card,
            method: method,
            creator: creator
        },
        keyword: {

        },
        id: "add_id"
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("MainBack.ashx", "json", postStr, callBack_TUDetail);
}
function callBack_TUDetail(data) {
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
    thead.appendChild(tr);
    return thead;
}
//新建一行
function generateTableRow(data, dic, i) {
    var tr = document.createElement("tr");
    tr.setAttribute("name", data["ust_ID"]);
    for (index in dic) {
        var td = document.createElement("td");

            td.innerHTML = data[index];
        
       
        tr.appendChild(td);

    }

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