<?php
namespace wechat;

require_once'WxMsg.php';

class WxMsg
{
    /* ========================发送消息 begin================================== */
    /* send文本消息 */
    public function sendText($content)
    {
        $msg ['Content'] = $content;
        $this->_sendData($msg, 'text');
    }

    /* send图片消息 */
    public function sendImage($media_id)
    {
        $msg ['Image'] ['MediaId'] = $media_id;
        $this->_sendData($msg, 'image');
    }

    /* send语音消息 */
    public function sendVoice($media_id)
    {
        $msg ['Voice'] ['MediaId'] = $media_id;
        $this->_sendData($msg, 'voice');
    }

    /* send视频消息 */
    public function sendVideo($media_id, $title = '', $description = '')
    {
        $msg ['Video'] ['MediaId'] = $media_id;
        $msg ['Video'] ['Title'] = $title;
        $msg ['Video'] ['Description'] = $description;
        $this->_sendData($msg, 'video');
    }

    /* send音乐消息 */
    public function sendMusic($media_id, $title = '', $description = '', $music_url, $HQ_music_url)
    {
        $msg ['Music'] ['ThumbMediaId'] = $media_id;
        $msg ['Music'] ['Title'] = $title;
        $msg ['Music'] ['Description'] = $description;
        $msg ['Music'] ['MusicURL'] = $music_url;
        $msg ['Music'] ['HQMusicUrl'] = $HQ_music_url;
        $this->_sendData($msg, 'music');
    }

    /*
     * send图文消息 articles array 格式如下： array( array('Title'=>'','Description'=>'','PicUrl'=>'','Url'=>''), array('Title'=>'','Description'=>'','PicUrl'=>'','Url'=>'') );
     */
    public function sendNews($articles)
    {
        $msg ['ArticleCount'] = count($articles);
        $msg ['Articles'] = $articles;

        $this->_sendData($msg, 'news');
    }

    protected function _sendData($msg, $msgType)
    {
        echo 'send data!';
    }
}