<?php
namespace wechat;

require_once'WxMsg.php';
require_once'WeChat.php';

/**
 * Class WxCSMsg ... WeiXin Customer Service Message Model
 * @package wechat
 */
class WxCSMsg extends WxMsg
{
    var $wxInfo;
    var $toUser;

    public function __construct($wxInfo, $to)
    {
        $this->wxInfo = $wxInfo;
        $this->toUser = $to;
    }

    public function sendNews($articles)
    {
        $msg ['news']['articles'] = $articles;
        $this->_sendData($msg, 'news');
    }

    protected function _sendData($msg, $msgType)
    {
        $msg ['ToUser'] = $this->toUser;
        $msg ['MsgType'] = $msgType;

        $weChat = new WeChat($this->wxInfo);

        $jArray = $this->_data2jArray($msg);
        $rawMsg = JsonHelper::json_encode_cn($jArray);
        $weChat->sendCustomerMessage($rawMsg);
    }

    private function getKey($key)
    {
        $k = strtolower($key);
        if ($k == 'mediaid') {
            $k = 'media_id';
        }
        if ($k == 'thumbmediaid') {
            $k = 'thumb_media_id';
        }
        return $k;
    }

    private function _data2jArray($data)
    {
        $jArray = array();
        foreach ($data as $key => $value) {
            if (is_array($value) || is_object($value)) {
                $jArray[$this->getKey($key)] = $this->_data2jArray($value);
            } else {
                $jArray[$this->getKey($key)] = $value;
            }
        }
        return $jArray;
    }
}