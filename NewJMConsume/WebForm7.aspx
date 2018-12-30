<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm7.aspx.cs" Inherits="NewJMConsume.WebForm7" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

</head>
<body>
    <div>
                <input type="checkbox" name="z" value="早" disabled="disabled" />
        <input type="checkbox" name="z" value="早" />
        <input type="checkbox" name="z" value="早" />
        <input type="checkbox" name="w" value="午" />
        <input type="checkbox" name="w" value="午" />
        <input type="checkbox" name="d" value="晚" />
        <input type="checkbox" name="d" value="晚" />
        <input type="checkbox" name="d" value="晚" />
        <hr />
        <script src="assets/js/jquery-2.1.4.min.js"></script>
        <script>
            function sum(arr) {
                return eval(arr.join("+"));
            };

            $("input[type=checkbox]").click(function () {
                var z = 8;
                var w = 3;
                var d = 3;
                var cardMoney = "10";
                var all = new Array();
                var res;
                $("input").each(function () {
                    if ($(this).attr("checked") == true) {
                        if ($(this).attr("name") == "z") {

                            all.push(z);
                            res = sum(all);

                        } else if ($(this).attr("name") == "w") {

                            all.push(w);
                            res = sum(all);

                        } else if ($(this).attr("name") == "d") {

                            all.push(d);
                            res = sum(all);

                        }
                    }
                    else {

                    }
                });
            });
        </script>
    </div>
</body>
</html>
