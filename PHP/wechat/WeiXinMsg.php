<?php

namespace wechat;

require_once 'WeChatHelper.php';
require_once 'WXBiz/wxBizMsgCrypt.php';

require_once 'WxMsg.php';
/**
 * 微信基础模型
 */
class WeiXinMsg extends WxMsg
{
    var $data = array();
    var $wxCpt, $sReqTimeStamp, $sReqNonce, $sEncryptMsg;
    var $encryptType;
    var $wxInfo;
    var $rawMsg;

    public function __construct($content, $encrypt = null ,$wxInfo = null)
    {
        $this->wxInfo = $wxInfo;
        !empty ($content) || die ('这是微信请求的接口地址，直接在浏览器里无效');

        $this->encryptType = $encrypt['type'];

        if ($this->encryptType == 'aes') {

            $this->sEncryptMsg = $encrypt['signature'];
            $this->sReqTimeStamp = $encrypt['get.timestamp'];
            $this->sReqNonce = $encrypt['nonce'];

            $info = $this->wxInfo;
            $this->wxCpt = new \WXBizMsgCrypt ($info['token'], $info ['encodingaeskey'], $info ['appid']);

            $sMsg = ""; // 解析之后的明文
            $errCode = $this->wxCpt->DecryptMsg($this->sEncryptMsg, $this->sReqTimeStamp, $this->sReqNonce, $content, $sMsg);
            if ($errCode != 0) {
                echo $errCode;
                WeChatHelper::log("DecryptMsg Error: " . $errCode);
                exit ();
            } else {
                // 解密成功，sMsg即为xml格式的明文
                $content = $sMsg;
            }
        }

        $this->rawMsg = $content;
        $data = new \SimpleXMLElement ($content);
        foreach ($data as $key => $value) {
            $this->data [$key] = strval($value);
        }
    }

    /* 获取微信平台请求的信息 */
    public function getData()
    {
        return $this->data;
    }

    /* ========================发送被动响应消息 begin================================== */
    /* 回复文本消息 */
    public function replyText($content)
    {
        $this->sendText($content);
    }

    /* 回复图片消息 */
    public function replyImage($media_id)
    {
        $this->sendImage($media_id);
    }

    /* 回复语音消息 */
    public function replyVoice($media_id)
    {
        $this->sendVoice($media_id);
    }

    /* 回复视频消息 */
    public function replyVideo($media_id, $title = '', $description = '')
    {
        $this->sendVideo($media_id, $title, $description);
    }

    /* 回复音乐消息 */
    public function replyMusic($media_id, $title = '', $description = '', $music_url, $HQ_music_url)
    {
        $this->sendMusic($media_id, $title, $description, $music_url, $HQ_music_url);
    }

    /*
     * 回复图文消息 articles array 格式如下： array( array('Title'=>'','Description'=>'','PicUrl'=>'','Url'=>''), array('Title'=>'','Description'=>'','PicUrl'=>'','Url'=>'') );
     */
    public function replyNews($articles)
    {
        $this->sendNews($articles);
    }

    /* 发送回复消息到微信平台 */
    protected function _sendData($msg, $msgType)
    {
        $msg ['ToUserName'] = $this->data ['FromUserName'];
        $msg ['FromUserName'] = $this->data ['ToUserName'];
        $msg ['CreateTime'] = time();
        $msg ['MsgType'] = $msgType;

        $xml = new \SimpleXMLElement ('<xml></xml>');
        $this->_data2xml($xml, $msg);
        $str = $xml->asXML();

        if ($this->encryptType == 'aes') {
            $sEncryptMsg = ""; // xml格式的密文
            $errCode = $this->wxCpt->EncryptMsg($str, $this->sReqTimeStamp, $this->sReqNonce, $sEncryptMsg);
            if ($errCode == 0) {
                $str = $sEncryptMsg;
            } else {
                WeChatHelper::log($str, "EncryptMsg Error: " . $errCode);
            }
        }

        echo($str);
    }

    /* 组装xml数据 */
    public function _data2xml($xml, $data, $item = 'item')
    {
        foreach ($data as $key => $value) {
            is_numeric($key) && ($key = $item);
            if (is_array($value) || is_object($value)) {
                $child = $xml->addChild($key);
                $this->_data2xml($child, $value, $item);
            } else {
                if (is_numeric($value)) {
                    $child = $xml->addChild($key, $value);
                } else {
                    $child = $xml->addChild($key);
                    $node = dom_import_simplexml($child);
                    $node->appendChild($node->ownerDocument->createCDATASection($value));
                }
            }
        }
    }
    /* ========================发送被动响应消息 end================================== */

}