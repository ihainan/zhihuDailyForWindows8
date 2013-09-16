<?php
/**
  > File Name: getNewsContent.php
  > Function : get the content of special news from ZhihuDaily API
  > Author: ihainan
  > Mail: ihainan72@gmail.com 
  > Created Time: 2013年09月07日 星期六 10时27分06秒
 **/
?>
<?php
header("text/xml");
require_once('ZhihuDaily.class.php');
@$url = $_GET['url'];
$zhihu = new ZhihuDaily();
echo $zhihu -> getNewsContent($url);
?>
