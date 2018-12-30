$(document).ready(function () {

   // zTreeObj = $.fn.zTree.init($("#treeDemo"), setting, zNodes);
    load_tree(true, 1);
})





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
        zTreeObj = $.fn.zTree.init($("#treeDemo"), setting, jsonData);

    }
}
	//将json串转换成树形结构
function transData(a, idStr, pidStr, chindrenStr) {
    var r = [], hash = {}, id = idStr, pid = pidStr, children = chindrenStr, i = 0, j = 0, len = a.length;
    for (; i < len; i++) {
        hash[a[i][id]] = a[i];
    }
    for (; j < len; j++) {
        var aVal = a[j], hashVP = hash[aVal[pid]];
        if (hashVP) {
            !hashVP[children] && (hashVP[children] = []);
            hashVP[children].push(aVal);
        } else {
            r.push(aVal);
        }
    }
    return r;
}




var setting = {
    callback: {


        onRename: zTreeOnRename,
        beforeRemove: zTreeBeforeRemove,
        onRemove: zTreeOnRemove,
        onClick: zTreeOnClick

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
function update_tree (id, name) {
    var jsonObj = {
        type:"tree_change",
        tree_type: "update_tree",
        tree_id: id,
        tree_name: name
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("SettingBack.ashx", "json", postStr, callBack_update_tree);
    
}
function callBack_update_tree(data) {
   
}
function Delete_tree(id) {
    var jsonObj = {
        type: "tree_change",
        tree_type: "delete_tree",
        tree_id: id
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("SettingBack.ashx", "json", postStr, callBack_delete_tree);

}
function callBack_delete_tree(data) {
    alert(data.msg);
}
function Insert_tree(pid,name) {
    var jsonObj = {
        type: "tree_change",
        tree_type: "insert_tree",
        pid: pid,
        tree_name: name

    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("SettingBack.ashx", "json", postStr, callBack_insert_tree);
}
function callBack_insert_tree(data) {
}


//重命名点击事件
function test() {
    var treeObj = $.fn.zTree.getZTreeObj("treeDemo");

    //得到选中的节点

    var nodes = treeObj.getSelectedNodes();

    for (var nodes_i = 0, nodes_len = nodes.length; nodes_i < nodes_len; nodes_i++) {
        treeObj.editName(nodes[nodes_i]);//指明重命名的节点 这个bug可利用
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
    window.location.reload();
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

