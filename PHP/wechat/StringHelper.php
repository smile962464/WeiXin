<?php

namespace wechat;

class StringHelper {

    public static function toQueryString($params = array()){
        if(count($params) > 0){
            $aGet = array();
            foreach ($params as $key => $val) {
                $aGet [] = $key . "=" . $val;
            }
            return join("&", $aGet);
        }
    }
}