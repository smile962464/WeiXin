<?php
/*
 * Smarty baseUrl plugin
 */
function smarty_function_baseUrl($params, $template)
{
    $withUri = isset($params['withUri']) ? $params['withUri'] : true;
    $appName = isset($params['appname']) ? $params['appname'] : 'default';

    $req = \Slim\Slim::getInstance($appName)->request();
    $uri = $req->getUrl();

    if ($withUri) {
        $uri .= $req->getRootUri();
    }

    return $uri;
}
