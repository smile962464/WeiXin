/**
 * Created with IntelliJ IDEA.
 * Author: 禹
 * Date: 13-9-7
 * Time: 下午3:07
 * File:
 * Description:
 */

var crypto = require("crypto");
var xml = require("node-xml")

Winxin = {};


Winxin.MessageHandlers = function (stream) {

    this.outStream = stream;

    this.msgHandlers = {
        "text": [],
        "image": [],
        "location": [],
        "link": [],
        "event": [],
        "voice": []
    };

}

Winxin.MessageHandlers.prototype = {
    addHandler: function (type, handler) {
        if (typeof(handler) == "object") {
            this.msgHandlers[type].push(function (msg) {
                return handler.handle();
            });
        }
        if (typeof(handler) == "function") {
            this.msgHandlers[type].push(handler);
        }
    },

    handle: function (msg) {
        if (typeof(msg) == "string") {
            msg = this.getMsgFromXml(msg);
        }

        var type = msg.type;

        try {
            var handlers = this.msgHandlers[type];
            if (handlers) {
                for (var i = 0; i < handlers.length; i++) {
                    var h = handlers[i];
                    var r = h(msg);
                    if (r.handled) {
                        this.outStream.write(this.getMsgFromXml(r.msg));
                    }
                }
            }
        } catch (e) {
            return false;
        }
        return false;
    },

    getMsgFromXml: function (xml) {
        var msg = {};
        return msg;
    },
    getXmlFromMsg: function (msg) {
        var xml = '';
        return xml;
    }

}

Winxin.Utities = {
    getTimeTicks: function (dt) {
        return dt.getTime();
    },
    getTime: function (ticks) {
        return new Date(ticks);
    },
    log: function () {

    },
    getAccessToken: function (appid, appsecret) {

    },
    addMenuFromFile: function (accessToken, path) {

    },
    addMenu: function (accessToken, menu) {

    },
    getMenu: function (accessToken) {

    },
    deleteMenu: function (accessToken) {

    },
    isLegel: function (signature, timestamp, nonce, token) {
        var array = new Array();
        array[0] = timestamp;
        array[1] = nonce;
        array[2] = token;
        array.sort();
        var hasher = crypto.createHash("sha1");
        var msg = array[0] + array[1] + array[2];
        hasher.update(msg);
        var msg = hasher.digest('hex');
        if (msg == signature) {
            return true;
        } else {
            return false;
        }
    }


};


exports.Winxin = Winxin;