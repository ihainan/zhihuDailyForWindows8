<?php
/*********************************************************
	File Name : cron_get_xml.php
	Author: ihainan
	mail: ihainan72@gmail.com
	Created Time: 2013年09月15日 星期日 10时13分39秒
 **************************************************************/
?>

<?php
require_once("../ZhihuDaily.class.php");
$domain = "ihainan";

/* get today's news */
$today = date("Ymd");
$zhihu = new ZhihuDaily();
$xml_date = $zhihu -> showTitles($today);

/* save file(for sinaapp) */
$s = new SaeStorage();
$current = date("Ymdhis");
$file_name = $current.".xml";
echo $file_name;
$s -> write($domain, $file_name, $xml_date -> asXML());
?>
