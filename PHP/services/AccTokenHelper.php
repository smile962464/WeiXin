<?php

require_once './services/medoo.php';

class AccTokenHelper{

    public static function getAccessToken($info, $type = 'acc_token'){

        $token = $info['token'];
        $database = new \medoo();
        $toks = $database->select("acc_token", array(
            "token",
            "acc_token",
            "valid_time"
        ), array(
            "AND" => array(
                "type" => $type,
                "token" => $token
            )
        ));
        if(count($toks) == 0){
            $database->insert('acc_token', array(
                'token' => $token,
                'acc_token' => '',
                'type' => $type,
                'valid_time' => date('Y-m-d H:i:s', time() - 1000)
            ));
        }
        $toks = $database->select("acc_token", array(
            "token",
            "acc_token",
            "valid_time"
        ), array(
            "AND" => array(
                "type" => $type,
                "token" => $token
            )
        ));
        // echo $database->last_query();
        $tok = $toks[0];
        $time = date('Y-m-d H:i:s', time());
        // invalid
        if($tok["valid_time"] < $time){

            $tok_ = array();
            if($type == 'acc_token'){
                $tok_ = \wechat\WeChatHelper::getAccessToken($info);
            }else{
                $acc_token = self::getAccessToken($info);
                $tok_ = \wechat\WeChatHelper::getJsApiTicket($acc_token);
            }
            $database->update('acc_token', array(
                'acc_token' => $tok_['data'],
                'valid_time' => date('Y-m-d H:i:s', time() + $tok_['expires'])
            ), array(
                "AND" => array(
                    "type" => $type,
                    "token" => $token
                )
            ));
            $tok['acc_token'] = $tok_['data'];
        }
        // echo '<br >'. $tok['acc_token'] . '<br />';
        return $tok['acc_token'];
    }
}