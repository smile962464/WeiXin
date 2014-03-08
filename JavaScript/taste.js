var Weixin = require("./Weixin.js").Weixin;


var xml = require("node-xml");

console.log('taste of Weixin Framework for javascript');
console.log(Weixin);


console.log(xml);

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