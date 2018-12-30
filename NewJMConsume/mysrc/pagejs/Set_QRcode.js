var now_page = 1;
dic_head = {
    QR_ID: "序号",
    Company_owner: "所属公司",
    Staff_name: "姓名",
    Staff_phone: "手机号码",
    Profession: "职称",
    Create_time: "创建时间",
    Expiry_time: "有效期"

}
$(document).ready(function () {
    load(true, 1);
})

//加载页面
function load(isfirst, pc) {
    var dates = $("#btime").val();
    var Staff_name = $("#StaffName").val();
    var jsonObj = {
        type: "set_QR_load",
        table: "tab_QR_setting",
        QR_info: {
            Create_time: dates,
            Staff_name: Staff_name

        },
        id:"QR_ID",
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
    tr.setAttribute("name", data["QR_ID"]);
    for (index in dic) {
        var td = document.createElement("td");

        td.innerHTML = data[index];
        tr.appendChild(td);

    }
    var td2 = document.createElement("td");
    td2.innerHTML = checkStatus(data["QR_ID"], data["UUID"]);
    tr.appendChild(td2);

    return tr;
}
//判断操作是否为启用或停用
function checkStatus(isuse, feeid) {
    var str = "";
    str = "<a onclick=\"showQR(this.name)\" name=" + feeid + " >生成</a>";
    //str += "<a onclick=\"deleteRole(this.name)\" name=" + isuse + ">预览</a>";
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

//打印二维码
function myPrint(obj) {
    var newWindow = window.open("打印窗口", "_blank");//打印窗口要换成页面的url
    var docStr = obj.innerHTML;
    newWindow.document.write(docStr);
    newWindow.document.close();
    newWindow.print();
    newWindow.close();
}
//展示二维码
function showQR(id) {
    $("#id_QR").empty();
    var qrcode = new QRCode('id_QR');
        qrcode.makeCode(id);
    $("#myModal03").modal('show');
}

//新增二维码时显示模态框
function showAddQR() {
    $("#myModalLabel").html("单个新增--二维码");
    $("#Company_owner").val("");
    $("#Staff_name").val("");
    $("#Staff_phone").val("");
    $("#Profession").val("");
    $("Expiry_time").val("");
    //$("#changeBtn").attr("onclick", "addQR()");
    $('#myModal').modal('show');

}
//批量添加二维码显示模态框
 function showUpQR() {
    $("#myModal02").modal('show');
}

//添加一张二维码
function addQR() {
    var Company_owner = $("#Company_owner").val();
    var Staff_name = $("#Staff_name").val();
    var Staff_phone = $("#Staff_phone").val();
    var Profession = $("#Profession").val();
    var Expiry_time = $("#Expiry_time").val();
    if (Company_owner != "" && Staff_name != "" && Staff_phone != "" && Profession != "" && Expiry_time != "") {
        var jsonObj = {
            type: "set_QR_add",
            Company_owner: Company_owner,
            Staff_name: Staff_name,
            Staff_phone: Staff_phone,
            Profession: Profession,
            Expiry_time: Expiry_time
        }
        var postStr = JSON.stringify(jsonObj);
        mj_ajax("SettingBack.ashx", "json", postStr, callBack_addQR);
    } else {
        alert("不可为空！");
    }
}
//添加一张二维码的回调
function callBack_addQR(data) {
    alert(data.msg);
    load(true, 1);
}
