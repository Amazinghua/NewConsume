var now_page = 1;
dic_head = {
    order_id: "序号",
    dev_no: "结算单位",
    usr_name: "姓名",
    dev_Ip: "消费地点",
    phone_no: "电话号码",
    Dept_Name: "部门",
    order_Price_Name: "类型",
    order_Price_Type:"餐类",
    order_money: "金额",
    order_create_date: "生成时间"


}
$(document).ready(function () {
    //load(true, 1);
})

//消费明细查询导出excel
function downFeeDetail() {
    var FEE_btime = $("#FEE_btime").val();
    var FEE_etime = $("#FEE_etime").val();
    var FEE_type = $("#FEE_type").val();
    var FEE_dept = $("#FEE_dept").val();
    var FEE_dev_no = $("#FEE_dev_no").val();
    var FEE_name = $("#FEE_name").val();
    var pay_type = $("#pay_type").val();
    var FEE_place = $("#FEE_place").val();

    var jsonObj = {
        type:"FEE_detail_load",
        btn_type: "FEE_detail",
        FEE_btime: FEE_btime,
        FEE_etime: FEE_etime,
        FEE_type: FEE_type,
        FEE_dept: FEE_dept,
        FEE_dev_no: FEE_dev_no,
        FEE_name: FEE_name,
        pay_type: pay_type,
        FEE_place: FEE_place
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("MainBack.ashx", "json", postStr, callBack_down_load);
}
function callBack_down_load(data) {
    var urlArray = window.location.href.split("/");
    var fileUrl = urlArray[0] + "//" + urlArray[2] + "/download/" + data.files;
    window.open(fileUrl);
}

//加载页面
function load(isfirst, pc) {
    var FEE_btime = $("#FEE_btime").val();
    var FEE_etime = $("#FEE_etime").val();
    if (FEE_btime != "" && FEE_etime == "") {
        alert("请输入结束时间!");
        return;
    }
    if (FEE_btime == "" && FEE_etime != "") {
        alert("请输入起始时间!");
        return;
    }
    var FEE_type = $("#FEE_type").val();
    var FEE_dept = $("#FEE_dept").val();
    var FEE_dev_no = $("#FEE_dev_no").val();
    var FEE_name = $("#FEE_name").val();
    var pay_type = $("#pay_type").val();
    var FEE_place = $("#FEE_place").val();

    var jsonObj = {
        type: "FEE_detail_load",
        FEE_btime: FEE_btime,
        FEE_etime: FEE_etime,
        FEE_type: FEE_type,
        FEE_dept: FEE_dept,
        FEE_dev_no: FEE_dev_no,
        FEE_name: FEE_name,
        pay_type: pay_type,
        FEE_place: FEE_place,
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