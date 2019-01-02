var now_page = 1;
dic_head = {
    head: '<input type="Checkbox" id="all"/>',
    Company: "公司",
    usr_no: "帐号",
    usr_name: "姓名",
    Dept_Name: "部门",
    phone_no: "号码"
}
dic_head2 = {
    head: '<input type="Checkbox" onclick="allcheck(this)"/>',
    Company: "公司",
    usr_no: "帐号",
    usr_name: "姓名",
    card_no: "卡号",
    Dept_Name: "部门",
    phone_no: "号码"
}

//个人充值
function recharge() {
    //var cardno = $("#cardno").val();
    //var name = $("#name").val();
    var method = $("#method").val();
    var money = $("#money").val();
    var memo = $("#memo").val();
    //var cash_phone = $("#cash_phone").val();
    var usrno = "admin";
    var jsonObj = {
        type: "recharge_self",
        //cardno: cardno,
        //name: name,
        method: method,
        money: money,
        memo: memo,
        usrno: usrno,
        user_checked: getChecked()
        //cash_phone: cash_phone
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("MainBack.ashx", "json", postStr, callBack_addRole);
}
//个人充值回调
function callBack_addRole(data) {
    //$("#cardno").val("");
    //$("#name").val("");
    $("#method").val("");
    $("#money").val("");
    $("#memo").val("");
    //$("#cash_phone").val("");
    alert(data.msg);
    $("#table").empty();
    $("#pageLimit").empty();
    // location.reload();
}

//展示模态框-确认充值
function showAdd() {
    $("#myModalLabel02").html("新增人员");
    //$("#addusrno").val("");
    //$("#addusrname").val("");
    //$("#addphone").val("");
    //$("#addcard").val("");
    //$("#addfeeplace").val("");
    //$("#changeBtn").attr("onclick", "addRole()");
    //var cardno = document.getElementById("cardno").value;
    //var account = document.getElementById("name").value;
    //var phone = document.getElementById("cash_phone").value;
    var type = document.getElementById("method").value;
    var money = document.getElementById("money").value;
    var memo = document.getElementById("memo").value;
    $('#myModal02').modal('show');
    // $('#card').html(cardno);
    //$('#account').html(account);
    //$('#phone').html(phone);
    $('#theMoney').html(money);
    $('#newMoney').html(money);
    $('#remark').html(memo);

}
function test() {
    recharge();
}
function myPrint(obj) {
    var newWindow = window.open("打印窗口", "_blank");//打印窗口要换成页面的url
    var docStr = obj.innerHTML;
    newWindow.document.write(docStr);
    newWindow.document.close();
    newWindow.print();
    newWindow.close();
}

//展示团体充值模态框
function showTeam() {
    $('#myModal03').modal('show');
}
//模糊查询返回具体人员信息
function check_user(isfirst, pc) {
    var check_key = $("#check_key").val();
    var jsonObj = {
        type: "check_usr",
        check_key: check_key,
        pc: pc
    }
    if (isfirst) {
        now_page = 1;
    }
    else {
        now_page = pc;
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("MainBack.ashx", "json", postStr, callBack_check_user);
}
function callBack_check_user(data) {
    if (data.result == "ok") {
        $("#table").empty();
        var table = document.getElementById("table");
        table.appendChild(generateTableThead(dic_head));
        if (data.execDt.length != 0) {
            generateTable(data.execDt, dic_head, table);
        }
        bindpaginator(data.numcount);

    } else {
        alert(data.msg);
        //$("#cardno").val("");
        //$("#name").val("");
        //$("#cash_phone").val("");
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
    tr.setAttribute("name", data["ust_ID"]);
    for (index in dic) {
        var td = document.createElement("td");
        if (index == "head") {
            //td.innerHTML = "<input name='" + data[index] + "'type='checkbox'/>";
            td.innerHTML = '<input type="checkbox"/>';
            //td.setAttribute("value", data[ust_ID]);
        } else {
            td.innerHTML = data[index];
        }
        tr.appendChild(td);

    }
    //var td2 = document.createElement("td");
    //td2.innerHTML = checkStatus(data["ust_ID"]);
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
            check_user(false, page);
        }
    });

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
    var json = obj; //JSON.stringify(obj);
    console.log(json.CheckedCode.toString());
    return json.CheckedCode.toString();
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
                $("#select_company").append(opt);//将opt填充到select中
                $("#select_company").selectpicker('refresh');
            }

        }
    });
}
//按部门充值
function add_money_dept() {
    var select_company = $("#select_company").val();
    var select_dept = $("#select_dept").val();
    var select_staff = $("#select_staff").val();
    var add_money = $("#add_money").val();
    if (add_money == "") {
        alert("请输入充值金额！");
        return;
    }
    var jsonObj = {
        type: "add_money_dept",
        select_company: select_company,
        select_dept: select_dept,
        //select_staff: select_staff,
        add_money: add_money
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("MainBack.ashx", "json", postStr, callBack_money_dept);
}
function callBack_money_dept(data) {
    if (data.result = "success") {
        alert(data.msg);
    } else {
        alert(data.msg);
    }
}

function Dispaydept(x) {
    var select_company = x;
    var jsonObj = {
        type: "select_dept",
        select_company: x
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("MainBack.ashx", "json", postStr, callBack_load_dept);
}

function callBack_load_dept(data) {
    var dept = document.getElementById("select_dept");
    dept.innerHTML = "";
    var d = data.dt;
    if (d.length == 0) {
        var comId = "";//获取到索引为i的
        var comName = "暂无部门";//获取索引为i的名
        var opt = "<option value='" + comId + "'>" + comName + "</option>";
        $("#select_dept").append(opt);//将opt填充到select中
        $("#select_dept").selectpicker('refresh');
    } else {
        var comId0 = "";//获取到索引为i的
        var comName0 = "全部";//获取索引为i的名
        var opt0 = "<option value='" + comId0 + "'>" + comName0 + "</option>";
        $("#select_dept").append(opt0);//将opt填充到select中

        for (var i = 0; i < d.length; i++) {
            var comId = d[i].Dept_Id;//获取到索引为i的
            var comName = d[i].Dept_Name;//获取索引为i的名
            var opt = "<option value='" + comId + "'>" + comName + "</option>";
            $("#select_dept").append(opt);//将opt填充到select中
            $("#select_dept").selectpicker('refresh');
        }
    }
}

function load_tree(isfirst, pc) {
    var jsonObj = {
        type: "load_tree",
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
function callBack_load(data) {
    if (data.result == "ok") {
        //zNodes = JSON.stringify(data.execDt);
        var jsonData = eval(JSON.stringify(data.execDt));
        //var jsonDataTree = transData(jsonData, 'id', 'Dept_up', 'chindren');
        //console.log(jsonDataTree);
        //alert(JSON.stringify(jsonDataTree));
        //document.getElementById("hh").innerHTML = JSON.stringify(jsonDataTree);
        $.fn.zTree.init($("#treeDemo"), setting, jsonData);
        setCheck();

    }
}
function setCheck() {
    var zTree = $.fn.zTree.getZTreeObj("treeDemo"),
        type = { "Y": "ps", "N": "ps" };
    zTree.setting.check.chkboxType = type;

}
var setting = {
    callback: {


        onRename: zTreeOnRename,
        beforeRemove: zTreeBeforeRemove,
        onRemove: zTreeOnRemove,
        onClick: zTreeOnClick,
        onCheck: zTreeOnCheck

    },
    check: {
        enable: true
    },
    data: {
        simpleData: {
            enable: true,
            idKey: "id",
            pIdKey: "pId",
            rootPId: 1
        }
    }
};
function zTreeOnRename(event, treeId, treeNode, isCancel) {
    //可以自定义属性，然后根据这些属性来完成操作
    alert(treeNode.id + ",重命名过后的回调函数 " + treeNode.name);
    update_tree(treeNode.id, treeNode.name);
    load(true, 1);
    //1.ajax更新数据库


}
//勾选事件
function zTreeOnCheck() {

    var zTree = $.fn.zTree.getZTreeObj("treeDemo");
    var changedNodes = zTree.getCheckedNodes();
    for (var i = 0; i < changedNodes.length; i++) {
        var treeNode = changedNodes[i];
        console.log((treeNode ? treeNode.name : "root") + "checked " + (treeNode.checked ? "true" : "false"));
    }
}




//删除点击事件
function del() {
    var treeObj = $.fn.zTree.getZTreeObj("treeDemo");

    //得到选择的节点
    var nodes = treeObj.getSelectedNodes();
    for (var nodes_i = 0, nodes_len = nodes.length; nodes_i < nodes_len; nodes_i++) {
        delNow(nodes[nodes_i]);
    }
}

function delNow(node) {
    var treeObj = $.fn.zTree.getZTreeObj("treeDemo");
    treeObj.removeNode(node, true);//必须要设置为true,否则没法调用事件
}


//删除之前的回调
function zTreeBeforeRemove(treeId, treeNode) {
    alert("删除之前");
    return true; //返回false设置不能删除 返回true设置为删除
}
//执行删除
function zTreeOnRemove(event, treeId, treeNode) {
    alert(treeNode.id + ",删除完成" + treeNode.name);
    Delete_tree(treeNode.id);
    //ajax去执行删除
}


function add() {
    //step1 得到tree
    var treeObj = $.fn.zTree.getZTreeObj("treeDemo");
    var node = treeObj.getSelectedNodes()[0]; //默认为第一个选中的文件夹新建子文件

    var newNode = {
        name: "新建文件夹",

        isNew: true,
        id: "",//真正使用的时候，请设为空。
        open: false  //是否为展开状态
    };
    newNode = treeObj.addNodes(node, newNode, false)[0]; //把自已 添加进去，注意，光标的定位，在回调函数中 自动展开的时候有可能会去获取子节点 在展开之前会执行异步

    //重命名新建的文件 改变数据库
    treeObj.editName(newNode);
    Insert_tree(node.id, newNode.name);
    //window.location.reload();
    //刷新一下树 加载父节点 异步的时候使用。
    treeObj.reAsyncChildNodes(newNode.getParentNode(), "refresh");
}

function beforeEditName(treeId, treeNode) {
    className = (className === "dark" ? "" : "dark");
    showLog("[ " + getTime() + " beforeEditName ]&nbsp;&nbsp;&nbsp;&nbsp; " + treeNode.name);
    var zTree = $.fn.zTree.getZTreeObj("treeDemo");
    zTree.selectNode(treeNode);
    setTimeout(function () {
        if (confirm("进入节点 -- " + treeNode.name + " 的编辑状态吗？")) {
            setTimeout(function () {
                zTree.editName(treeNode);
            }, 0);
        }
    }, 0);
    return false;
}
function zTreeOnClick(event, treeId, treeNode) {
    alert(treeNode.id + ", " + treeNode.name);
    load_user(true, 1, treeNode.id);
};
function load_user(isfirst, pc, id) {
    //var level = $("#level").val();
    var jsonObj = {
        type: "set_dep_load",
        id: id,
        pc: pc
    }
    if (isfirst) {
        now_page = 1;
    }
    else {
        now_page = pc;
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("SettingBack.ashx", "json", postStr, callBack_table);

}
function generateTable2(data, dic) {
    var table = document.getElementById("table2");
    table.appendChild(generateTableThead(dic))
    var tbody = document.createElement("tbody");
    for (var i = 0; i < data.length; i++) {
        tbody.appendChild(generateTableRow(data[i], dic, i));
    }
    table.appendChild(tbody);
}
//加载页面的回调
function callBack_table(data) {
    if (data.result == "ok") {
        $("#table2").empty();
        $("#pageLimit").empty();
        if (data.execDt.length != 0) {
            generateTable2(data.execDt, dic_head2);
            bindpaginator2(data.numcount);
        } else {

        }

    }
}
function bindpaginator2(tatal) {
    var tp = Math.ceil(tatal / 10)
    $('#pageLimit2').bootstrapPaginator({
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
            load_user(false, page);
        }
    });

}
//全选
function allcheck(a) {
    //var userids = this.checked;
    //获取name=box的复选框 遍历输出复选框
    if (a.checked == true) {
        $("input[type=checkbox]").each(function () {
            this.checked = true;

        });
    } else {
        $("input[type=checkbox]").each(function () {
            this.checked = false;

        });
    }
}