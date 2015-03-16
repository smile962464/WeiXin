<?php

namespace Slim\Views;

class SmartyView extends \Slim\View
{
    static public $AppRoot = null;

    public function render($template, $data = null)
    {
        $parser = SmartyView::getSmarty();
        $parser->assign($this->all());

        return $parser->fetch($template, $data);
    }

    public static function getSmarty(){

        $root = SmartyView::$AppRoot;

        $smarty = new \Smarty;

        $sae = function_exists('sae_debug');
        $debugging = !$sae;

        $smarty->template_dir = $root.'/views/';

        if($sae){
            $smarty->compile_dir = 'saemc://smartytpl/';
            $smarty->cache_dir = $smarty->compile_dir.'/cache';
            $smarty->compile_locking =  false;                      // 防止调用touch,saemc会自动更新时间，不需要touch
        }else{
            $smarty->compile_dir = $root.'/smartytpl/';
            $smarty->cache_dir = $smarty->compile_dir.'/cache';
            $smarty->compile_locking =  false;
        }

        $smarty->debugging = false; // $debugging;
        $smarty->caching = true;
        $smarty->cache_lifetime = 120;

        $smarty->addPluginsDir(__DIR__.'/SmartyPlugins/');

        return $smarty;
    }
}