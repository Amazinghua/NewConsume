<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="checktest.aspx.cs" Inherits="NewJMConsume.checktest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="assets/js/jquery-2.1.4.min.js"></script>
</head>
<body>
    <div>
        <input type="checkbox" name="z" value="早" disabled="disabled" />
        <input type="checkbox" name="z" value="早" checked="checked"  />
        <input type="checkbox" name="z" value="早"  />
        <input type="checkbox" name="w" value="午"  />
        <input type="checkbox" name="w" value="午"  />
        <input type="checkbox" name="d" value="晚"  />
        <input type="checkbox" name="d" value="晚" />
        <input type="checkbox" name="d" value="晚"  />
        <hr />
        <script>
            var i = 0;
            $(document).ready(function () {
                $("input[type=checkbox]").on("click", function () {
                    test(this);
                });
                var a = $("input[type=checkbox]:checked").length;
                i = a;
                console.log(i);
            });
            function test(a) {
                alert(i);
                console.log(a);
                var all_z = $("input:checkbox[name ='z']:checked");
                var all_w = $("input:checkbox[name ='w']:checked");
                var all_d = $("input:checkbox[name ='d']:checked");
                var res = all_z.length * 5 + all_w.length * 3 + all_d.length * 3;
                if (res > cardmoney) {
                    alert("余额不足");
                    a.checked = false;
                }
            }
            var cardmoney = 10;
            //$("input[type=checkbox]").click(function () {
            //    //var z = 8;
            //    //var w = 4;
            //    //var d = 7;
            //    //let i;
            //    //let q;
            //    //var cardMoney = "10.5";
            //    //var all = new Array();
            //    console.log(this);
            //    var all_z = $("input:checkbox[name ='z']:checked");
            //    var all_w = $("input:checkbox[name ='w']:checked");
            //    var all_d = $("input:checkbox[name ='d']:checked");
            //    var res = all_z.length * 100 + all_w.length * 3 + all_d.length * 4;
            //    if (res > cardmoney) {

            //    }

            //    alert($("input:checkbox[name ='z']:checked").length);

            //    //for (i = 0; i < $("input[type=checkbox]:checked").length; i++) {
            //    //    if ($("input[type=checkbox]:checked")[i].name == "z") {

            //    //        all.push(z);
            //    //    } else if ($("input[type=checkbox]:checked")[i].name == "w") {

            //    //        all.push(w);
            //    //    } else if ($("input[type=checkbox]:checked")[i].name == "d") {

            //    //        all.push(d);
            //    //    }
            //    //}
            //    //var res = 0;
            //    //for (i = 0; i < $("input[type=checkbox]").length; i++) {

            //    //    if ($("input[type=checkbox]")[i].checked == true) {

            //    //        if ($("input[type=checkbox]")[i].name == "z") {
            //    //            //all.push(z);
            //    //            res = res + 8;
            //    //            if (res > cardMoney) {
            //    //                console.log(i);
            //    //                alert("不足");
            //    //                $("input[type=checkbox]")[i].checked = false;
            //    //                continue;

            //    //            }

            //    //        } else if ($("input[type=checkbox]")[i].name == "w") {

            //    //            //all.push(w);
            //    //            res = res + 4;
            //    //            if (res > cardMoney) {
            //    //                console.log(i);
            //    //                alert("不足");
            //    //                $("input[type=checkbox]")[i].checked = false;
            //    //                continue;

            //    //            }

            //    //        } else if ($("input[type=checkbox]")[i].name == "d") {
            //    //            //all.push(d);
            //    //            res = res + 7;
            //    //            if (res > cardMoney) {
            //    //                console.log(i);
            //    //                alert("不足");
            //    //                $("input[type=checkbox]")[i].checked = false;
            //    //                continue;

            //    //            }

            //    //        }
            //    //    }
            //    //}

            //    //$("input[type=checkbox]").each(function () {
            //    //    // console.log($(this)[0].checked);
            //    //    if ($(this)[0].checked == true) {

            //    //        console.log($(this));
            //    //        if ($(this).attr("name") == "z") {

            //    //            all.push(z);
            //    //            res = sum(all);
            //    //            if (res > cardMoney) {

            //    //                alert("不足");
            //    //                $(this)[0].checked = false;

            //    //            }

            //    //        } else if ($(this).attr("name") == "w") {

            //    //            all.push(w);
            //    //            res = sum(all);
            //    //            if (res > cardMoney) {

            //    //                alert("不足");
            //    //                $(this)[0].checked = false;

            //    //            }
            //    //        } else if ($(this).attr("name") == "d") {

            //    //            all.push(d);
            //    //            res = sum(all);
            //    //            if (res > cardMoney) {

            //    //                alert("不足");
            //    //                $(this)[0].checked = false;

            //    //            }
            //    //        }
            //    //    }
            //    //});


            //    //if (res > cardMoney) {

            //    //    alert("不足");

            //    //    console.log();
            //    //    //console.log($("input[type=checkbox]:checked")[i]);
            //    //    //$("input[type=checkbox]:checked")[i].attr("checked", false);
            //    //    //var res = sum(all);
            //    //    //if (res > cardMoney) {
            //    //    //    console.log(i);

            //    //    //    console.log($("input[type=checkbox]").length);
            //    //    //    //$("input[type=checkbox]")[i].checked = false;
            //    //    //    alert("不足");
            //    //    //    $("input[type=checkbox]:checked")[$("input[type=checkbox]:checked").length - 1].checked = false;
            //    //}
            //});
            //function sum(arr) {
            //    return eval(arr.join("+"));
            //};
        </script>
    </div>
</body>
</html>
