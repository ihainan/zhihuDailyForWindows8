<?php
/**
  > File Name: ZhihuDaily.class.php
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

		/* covert array into XML file and then show the content */
		$root = "titles";
		$xml_data = new SimpleXMLElement("<?xml version=\"1.0\" encoding=\"GB2312\"?><".$root."></".$root.">");
		$this -> arrayToXML($results, $xml_data);
		echo $xml_data -> asXML();
	}

	/* get and show the special news as XML file */
	public function getNewsContent($url = null){
		$root = "news";
		$xml_data = new SimpleXMLElement("<?xml version=\"1.0\" encoding=\"GB2312\"?><".$root."></".$root.">");
		if(!isset($url) || $url == ""){
			$this -> showNoContent($root);
			return;
		}

		/* get data from zhihuDaily API */
		$json_data = json_decode(file_get_contents($url), 1);
		$results = array(
				'body' => $json_data['body'],
				'image_source' => $json_data['image_source'],
				'title' => $json_data['title'],
				'url' => $json_data['url'],
				'image' => $json_data['image'],
				'share_url' => $json_data['share_url'],
				'share_image' => $json_data['share_image'],
				'js' => $json_data['js'],
				'css' => $json_data['css'],
				'thumbnail' => $json_data['thumbnail'],
				);

		/* covert array into XML file and then show the content */
		$this -> arrayToXML($results, $xml_data);
		echo $xml_data -> asXML();
	}

	/* show special content if no content can be shown */
	private function showNoContent($root){
		echo "<?xml version=\"1.0\" encoding=\"GB2312\"?><".$root."></".$root.">";
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
?>
