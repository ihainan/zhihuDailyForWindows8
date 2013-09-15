<?php
/**
  > File Name: getTitles.php
  > Function : get the titles of news from ZhihuDaily API
  > Author: ihainan
  > Mail: ihainan72@gmail.com 
  > Created Time: 2013年09月07日 星期六 10时20分06秒
 **/
?>
<?php
header("text/xml");
require_once('ZhihuDaily.class.php');
@$date = $_GET['date'];
$zhihu = new ZhihuDaily();
$zhihu -> showTitles($date);
?>
