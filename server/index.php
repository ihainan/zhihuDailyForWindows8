<?php
/**
  > File Name: index.php
  > Function : A interface for zhihuDaily
  > Author: ihainan
  > Mail: ihainan72@gmail.com 
  > Created Time: 2013年09月05日 星期四 22时52分06秒
 **/
?>
<?php
class ZhihuDaily{
	/* show the news items */
	public function showTities(){
		/* get data from zhihuDaily API */
		$jsonData = json_decode(file_get_contents('http://news.at.zhihu.com/api/1.2/news/latest'), 1);
		$results = array();
		for($i = 0; $i < count($jsonData['news']); $i++){
			// Make a array
			$tmpArray = array(
					'image_source' => $jsonData['news'][$i]['image_source'],
					'title' => $jsonData['news'][$i]['title'],
					'image' => $jsonData['news'][$i]['image'],
					'share_url' => $jsonData['news'][$i]['share_url'],
					'share_image' => $jsonData['news'][$i]['share_image'], 
					'url' => $jsonData['news'][$i]['url'], 
					'thumbnail' => $jsonData['news'][$i]['thumbnail'], 
					);
			array_push( $results, $tmpArray);
		}
		$root = "titles";
		$xml_data = new SimpleXMLElement("<?xml version=\"1.0\" encoding=\"GB2312\"?><".$root."></".$root.">");
		$this -> array_to_xml($results, $xml_data);
		echo $xml_data -> asXML();
	}

	/* corvert array to XML */
	public function array_to_xml($array, $xml_data){
		foreach($array as $key => $value){
			if(is_array($value)){
				if(!is_numeric($key)){
					$subnode = $xml_data -> addChild("$key");
					$this -> array_to_xml($value, $subnode);
				}
				else{
					$subnode = $xml_data -> addChild("item$key");
					$this -> array_to_xml($value, $subnode);
				}
			}
			else{
				$xml_data -> $key =  $value;
			}
		}

	}
}

$zhihu = new ZhihuDaily();
$zhihu -> showTities();

?>
