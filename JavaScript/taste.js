var Weixin = require("./Weixin.js").Weixin;


var xml = require("node-xml");

console.log('taste of Weixin Framework for javascript');
console.log(Weixin);

var conf = {
    APPID: "wx8980382cb43a6897",
    APPSECRET: "1137740a67c5152a9cd7af37abb4cd28"
};

Weixin.config(conf);

// console.log(xml);

var msg = {
    msg: {
        name: 'tangyu',
        age: '20'
    }
};

var h = new Weixin.MessageHandlers(null);
var msg = h.getXmlFromMsg(msg);
console.log(msg);
var msg_o = h.getMsgFromXml(msg);
console.log(msg_o);


var http = require("http");

var https = require('https');

var options = {
    hostname: 'www.baidu.com',
    port: 80,
    path: '/',
    method: 'GET'
};
console.log("https://api.weixin.qq.com/cgi-bin/tok5en?grant_type=client_credential&appid=" + Weixin.config.APPID + "&secret=" + Weixin.config.APPSECRET + "");
var req = https.get("https://api.weixin.qq.com/cgi-bin/tok5en?grant_type=client_credential&appid=" + Weixin.config.APPID + "&secret=" + Weixin.config.APPSECRET + "", function(res) {
    console.log('STATUS: ' + res.statusCode);
    console.log('HEADERS: ' + JSON.stringify(res.headers));
    res.setEncoding('utf8');
    res.on('data', function(chunk) {
        console.log('BODY: ' + chunk);
    });
});