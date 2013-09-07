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
	private $titles_api_url = "http://news.at.zhihu.com/api/1.2/news/before/";
	/* show the news items */
	public function showTities($date = null){
		if($date != null)
			$this -> titles_api_url = $this -> titles_api_url.($date + 1);
		else
			$this -> titles_api_url = 'http://news.at.zhihu.com/api/1.2/news/latest';
		/* get data from zhihuDaily API */
		$json_data = json_decode(file_get_contents($this -> titles_api_url), 1);
		$results = array();
		for($i = 0; $i < count($json_data['news']); $i++){
			// Make an array
			$tmp_array = array(
					'image_source' => $json_data['news'][$i]['image_source'],
					'title' => $json_data['news'][$i]['title'],
					'image' => $json_data['news'][$i]['image'],
					'share_url' => $json_data['news'][$i]['share_url'],
					'share_image' => $json_data['news'][$i]['share_image'], 
					'url' => $json_data['news'][$i]['url'], 
					'thumbnail' => $json_data['news'][$i]['thumbnail'], 
					);
			array_push( $results, $tmp_array);
		}
		$root = "titles";
		$xml_data = new SimpleXMLElement("<?xml version=\"1.0\" encoding=\"GB2312\"?><".$root."></".$root.">");
		$this -> arrayToXML($results, $xml_data);
		echo $xml_data -> asXML();
	}

	/* corvert array to XML recursively*/
	public function arrayToXML($array, $xml_data){
		foreach($array as $key => $value){
			if(is_array($value)){
				if(!is_numeric($key)){
					$subnode = $xml_data -> addChild("$key");
					$this -> arrayToXML($value, $subnode);
				}
				else{
					$subnode = $xml_data -> addChild("item$key");
					$this -> arrayToXML($value, $subnode);
				}
			}
			else{
				$xml_data -> $key =  $value;
			}
		}

	}
}

@$date = $_GET['date'];
$zhihu = new ZhihuDaily();
$zhihu -> showTities($date);

?>
