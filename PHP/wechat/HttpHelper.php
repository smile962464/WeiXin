<?php

namespace wechat;

class HttpHelper {
    /**
     * GET 请求
     * @param string $url
     * @param array $param
     * @return bool|mixed
     */
    public static function httpGet($url, $param = array())
    {
        if(count($param) > 0){
            $querys = http_build_query($param);
            $url = $url.((!stristr($url, '?') ? '?' : '&').$querys);
        }
        $oCurl = curl_init();
        if (stripos($url, "https://") !== FALSE) {
            curl_setopt($oCurl, CURLOPT_SSL_VERIFYPEER, FALSE);
            curl_setopt($oCurl, CURLOPT_SSL_VERIFYHOST, FALSE);
        }
        curl_setopt($oCurl, CURLOPT_URL, $url);
        curl_setopt($oCurl, CURLOPT_RETURNTRANSFER, 1);
        curl_setopt($oCurl, CURLOPT_USERAGENT, 'Mozilla/5.0 (compatible; MSIE 5.01; Windows NT 5.0)');
        $sContent = curl_exec($oCurl);
        $aStatus = curl_getinfo($oCurl);
        curl_close($oCurl);
        if (intval($aStatus ["http_code"]) == 200) {
            return $sContent;
        } else {
            return false;
        }
    }

    /**
     * POST 请求
     *
     * @param string $url
     * @param array $param
     * @param array $header
     * @return string content
     */
    public static function httpPost($url, $param, $header = array())
    {
        $oCurl = curl_init();
        if (stripos($url, "https://") !== FALSE) {
            curl_setopt($oCurl, CURLOPT_SSL_VERIFYPEER, false);
            curl_setopt($oCurl, CURLOPT_SSL_VERIFYHOST, false);
        }
        curl_setopt($oCurl, CURLOPT_URL, $url);
        curl_setopt($oCurl, CURLOPT_RETURNTRANSFER, true);
        curl_setopt($oCurl, CURLOPT_POST, true);
        curl_setopt($oCurl, CURLOPT_CUSTOMREQUEST, "POST");
        curl_setopt($oCurl, CURLOPT_HTTPHEADER, $header);

        curl_setopt($oCurl, CURLOPT_POSTFIELDS, $param);
        $sContent = curl_exec($oCurl);

        if (curl_errno($oCurl) != 0) {
            return array('errno'=>1, 'errmsg'=>"post data to $url failed: ".curl_error($oCurl), 'data'=>'');
        }

        $aStatus = curl_getinfo($oCurl);
        curl_close($oCurl);
        if (intval($aStatus ["http_code"]) == 200) {
            return $sContent;
        } else {
            return false;
        }
    }

    /* 上传文件 */
    public static function uploadFile($file, $url)
    {
        $post_data ['f'] = '@'.$file;
        return self::httpPost($url, $post_data);
    }

    /* 下载文件 */
    public static function downloadFile($url, $file)
    {
        $temp = self::httpGet($url);
        if(@file_put_contents($file, $temp)) {
            return $file;
        } else {
            return false;
        }
    }
}