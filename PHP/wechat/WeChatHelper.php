<?php

namespace wechat;

require_once 'HttpHelper.php';
require_once 'JsonHelper.php';

/**
 * wechat api
 */
class WeChatHelper
{
    /*******************  基础支持  ****************************/
    public static function signature($arr)
    {
        sort($arr, SORT_STRING);
        $tmpStr = implode($arr);
        $tmpStr = sha1($tmpStr);
        return $tmpStr;
    }

    /*******************  微信签名验证  ****************************
     * @param $token
     * @param $signature
     * @param $timestamp
     * @param $nonce
     * @return bool
     */
    public static function checkSignature($token, $signature, $timestamp, $nonce)
    {
        $tmpArr = array($token, $timestamp, $nonce);
        sort($tmpArr, SORT_STRING);
        $tmpStr = implode($tmpArr);
        $tmpStr = self::signature($tmpArr);

        if ($tmpStr == $signature) {
            return true;
        } else {
            return false;
        }
    }

    /**
     * 获取Access Token
     * @param $info
     * @return mixed
     */
    public static function getAccessToken($info)
    {
        $url_get = 'https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential';

        $accessTxt = HttpHelper::httpGet($url_get, $info);
        $access = json_decode($accessTxt, true);
        if (empty ($access ['access_token'])) {
            return false;
        }
        return array(
            'data' => $access['access_token'],
            'expires' => $access['expires_in']
        );
    }

    /**
     * 获取JS Api Ticket
     * @return
     */
    public static function getJsApiTicket($accToken){
        $url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket";

        $httpRes = HttpHelper::httpGet($url, array(
            'access_token' => $accToken,
            'type' => 'jsapi'
        ));
        $ticket = json_decode($httpRes, true);
        return array(
            'data' => $ticket['ticket'],
            'expires' => $ticket['expires_in']
        );
    }

    public static function log($msg)
    {
    }
}
