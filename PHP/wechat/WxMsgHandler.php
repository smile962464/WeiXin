<?php

namespace wechat;


class WxMsgHandler {

    var $handlers;

    public function __construct(){
        $this->handlers = array(
            'text' => array()
        );
    }

    public function on($event, $handler){

        if(!array_key_exists($event, $this->handlers)){
            $this->handlers[$event] = array();
        }
        $this->handlers[$event][] = $handler;
    }

    public function handle($msg)
    {
        $data = $msg->getData ();
        $type = $data["MsgType"];

        $handlers = $this->handlers[$type];
        if(is_array($handlers)){
            foreach($handlers as $handler){}
            $handler($msg);
        }
    }
} 