//加载页面
function LoginIn(isfirst, pc) {
    var name = $("#account").val().trim();
    var psw = $("#password").val().trim();
    var jsonObj = {
        type: "Login",
        user: name,
        psw: psw,
        pc: pc
    }
    if (isfirst) {
        now_page = 1;
    }
    else {
        now_page = pc;
    }
    var postStr = JSON.stringify(jsonObj);
    mj_ajax("SettingBack.ashx", "json", postStr, callBack_login);

}
function callBack_login(data) {
    if (data.result == "success") {
        console.log("asdsas");
        $.cookie('user_name', data.data);
        window.location.href = "BM_Count.aspx";
        console.log("asd");
       // $("#user_info").html("<small>欢迎,</small><br/>玩那个天华");
        console.log("test");
    } else {
        alert(data.msg);
    }
}