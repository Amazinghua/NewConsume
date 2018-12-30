function deleteRole(id) {
    var jsonObj = {
        type: "set_staff_delete",
        id: id
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("SettingBack.ashx", "json", postStr, callBack_deCant);
}
//删除饭堂的回调
function callBack_deCant(data) {
    alert("asda");
    load(true, 1);
}