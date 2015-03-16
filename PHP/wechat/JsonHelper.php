<?php

namespace wechat;

class JsonHelper {

    public static function json_encode_cn($data)
    {
        $data = json_encode($data);
        return preg_replace("/\\\u([0-9a-f]{4})/ie", "iconv('UCS-2BE', 'UTF-8', pack('H*', '$1'));", $data);
    }
}