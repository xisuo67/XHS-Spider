

try {
    sign(url,t);
} catch (e) { winning.log(e); }
function sign(url,t) {
    var o = window._webmsxyw(url, t);
    return o;
}





//try {
//    getuserinfo(user_id);
//} catch (e) { winning.log(e); }
//function getuserinfo(user_id) {

//    var xsxt = sign(user_id);
//    var xs = xsxt['X-s'];
//    var xt = xsxt['X-t'];
//    var json = JSON.stringify(xsxt);
//    winning.Sign(json)
//    return xsxt;
//    // winning.log(user_id);
//    // winning.log(cookie);

//    // var url="https://edith.xiaohongshu.com/api/sns/web/v1/user_posted?num=30&cursor=&user_id="+user_id;
//    // winning.log(url);
//    // var accept="application/json";
//    // var acceptlanguage="zh-CN,zh;q=0.9";
//    // var referer="https://www.xiaohongshu.com";
//    // var  userAgent ="Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36";
//    // var contenttype="application/json";
//    // var headers ='accept-language:'+acceptlanguage+ '\r\naccept:'+accept+'\r\nReferer: ' +referer + '\r\nUser-Agent: ' + userAgent + '\r\nCookie: ' + cookie+ '\r\nX-t: ' + xt+ '\r\nX-s: ' + xs;
//    // winning.log(headers);
//    // var result = winning.getRequestResultEx(url, headers);
//    // winning.log(result);
//    // winning.getJsResult(result);
//    // return JSON.stringify(result);
//}

//function sign(user_id) {
//    // var url="/api/sns/web/v1/user_posted?num=30&cursor=&user_id="+user_id;
//    var url = user_id;
//    var t;
//    var o = window._webmsxyw(url, t);
//    //var headers = 'X-t: ' + o['X-t'] + '\r\nX-s: ' + o['X-s'];
//    return o;
//}
