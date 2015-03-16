<?php
namespace wechat;

require_once 'WeChatHelper.php';

class WeChat
{
    var $wxInfo;

    public function __construct($wxInfo)
    {
        $this->wxInfo = $wxInfo;
    }

    public function getAccessToken()
    {
        return \AccTokenHelper::getAccessToken($this->wxInfo);
        // return WeChatHelper::getAccessToken($this->wxInfo);
    }

    public function getJsApiTicket()
    {
        return \AccTokenHelper::getAccessToken($this->wxInfo, 'jsticket');
    }

    /**
     * 获取微信服务器ip列表
     * @return
     */
    public function getIpList()
    {
        $accToken = $this->getAccessToken();
        $url = 'https://api.weixin.qq.com/cgi-bin/getcallbackip';

        $httpRes = HttpHelper::httpGet($url, array(
            'access_token' => $accToken
        ));
        $ips = json_decode($httpRes, true);
        return $ips['ip_list'];
    }

    /** 上传多媒体文件
     * @param $file
     * @param string $type
     * @return mixed
     */
    public function uploadMediaFile($file, $type = 'image')
    {
        $access = $this->getAccessToken();
        $accessToken = $access;
        $url = "http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token=$accessToken&type=$type";
        return HttpHelper::uploadFile($file, $url);
    }

    /** 下载多媒体文件
     * @param $path
     * @param $media_id
     */
    public function downloadMediaFile($path, $media_id)
    {
        $access = $this->getAccessToken();
        $accessToken = $access;
        $url = "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=$accessToken&media_id=$media_id";
        HttpHelper::downloadFile($url, $path);
    }


    /*************************** 菜单操作  ***********************************/
    public function getMenus(){
        $access = $this->getAccessToken();
        $accToken = $access;
        return json_decode( HttpHelper::httpGet("https://api.weixin.qq.com/cgi-bin/menu/get?access_token=$accToken"), true);
    }

    /**
     */
    public function deleteMenus()
    {
        $access = $this->getAccessToken();
        $accToken = $access;
        HttpHelper::httpGet("https://api.weixin.qq.com/cgi-bin/menu/delete?access_token=$accToken");
    }

    /**
     * 发送菜单到微信
     * @param $menu
     * @return mixed|string
     */
    public function addMenus($menu)
    {
        $menuTree = is_string($menu) ? $menu : JsonHelper::json_encode_cn($menu);
        $access = $this->getAccessToken();
        $this->deleteMenus($access);

        $url = 'https://api.weixin.qq.com/cgi-bin/menu/create?access_token=' . $access;
        $header [] = "content-type: application/x-www-form-urlencoded; charset=UTF-8";

        $res = HttpHelper::httpPost($url, $menuTree, $header);
        $res = json_decode($res, true);
        return $res;
    }


    /****************** user manage **********************
     */
    public function getUserList(){
        $access = $this->getAccessToken();
        $accToken = $access;
        $tmp = array(
            'next_openid' => ''
        );
        $users = array();
        $count = 0;
        do {
            $next = $tmp['next_openid'];
            $url = "https://api.weixin.qq.com/cgi-bin/user/get?access_token=$accToken&next_openid=$next";
            $tmp = json_decode(HttpHelper::httpGet($url), true);
            $count += $tmp['count'];
            $userList = (array)($tmp['data']['openid']);
            $users = array_merge($users, $userList);

        }while($tmp['total'] < $count);

        $result = array(
            'total' => $count,
            'users' => $users
        );
        return $result;
    }

    public function getUserInfo($uid){
        $access = $this->getAccessToken();
        $accToken = $access;
        $url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=$accToken&openid=$uid&lang=zh_CN";

        return json_decode(HttpHelper::httpGet($url), true);
    }


    public function getGroups(){
        $access = $this->getAccessToken();
        $accToken = $access;
        $url = "https://api.weixin.qq.com/cgi-bin/groups/get?access_token=$accToken";

        return json_decode(HttpHelper::httpGet($url), true);
    }

    public function addGroup($gname){
        $access = $this->getAccessToken();
        $accToken = $access;
        $url = "https://api.weixin.qq.com/cgi-bin/groups/create?access_token=$accToken";
        $group = array(
            'group' => array(
                'name' => $gname
            )
        );
        return json_decode(HttpHelper::httpPost($url, JsonHelper::json_encode_cn($group)), true);
    }

    public function modifyGroup($id, $newname){
        $access = $this->getAccessToken();
        $accToken = $access;
        $url = "https://api.weixin.qq.com/cgi-bin/groups/update?access_token=$accToken";
        $group = array(
            'group' => array(
                'id' => $id,
                'name' => $newname
            )
        );
        return json_decode(HttpHelper::httpPost($url, JsonHelper::json_encode_cn($group)), true);
    }

    public function getUserInGroup($uid){
        $access = $this->getAccessToken();
        $accToken = $access;
        $url = "https://api.weixin.qq.com/cgi-bin/groups/getid?access_token=$accToken";
        $user = array(
            'openid' => $uid
        );
        return json_decode(HttpHelper::httpPost($url, JsonHelper::json_encode_cn($user)), true);
    }

    public function moveUerToGroup($uid, $gid){
        $access = $this->getAccessToken();
        $accToken = $access;
        $url = "https://api.weixin.qq.com/cgi-bin/groups/members/update?access_token=$accToken";
        $data = array(
            'openid' => $uid,
            'to_groupid' => $gid
        );
        return json_decode(HttpHelper::httpPost($url, JsonHelper::json_encode_cn($data)), true);
    }


    /****************** send messages **********************
     * @param $jsonMsg
     * @return mixed|string
     */
    public function sendCustomerMessage($jsonMsg)
    {
        $access = $this->getAccessToken();
        $accessToken = $access;
        $post = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=$accessToken";

        $res = HttpHelper::httpPost($post, $jsonMsg);
        $res = json_decode($res, true);
        return $res;
    }
}