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
	private $titles_api_url_ori = "http://news.at.zhihu.com/api/1.2/news/before/";
	private $titles_api_uri = "http://news.at.zhihu.com/api/1.2/news/before/";
	private $domain = "zhdaily";

	/* get the news items from local storage */
	public function getTitles($date = null){
		$s = new SaeStorage();
		$root = "titles";
		/* get file from SaeStorage*/
		if($date == null)
			$date = date("Ymd");
		$file_name = $date.".xml";
		if($date == date("Ymd")){
			// today
			if($s -> fileExists($this -> domain, $file_name) == true){
				$yesterday = date ("Ymd", strtotime("-1 day", strtotime($date)));
				$yesterday_file_name = $yesterday.".xml";
				$content = $s -> read($this -> domain, $file_name);
				if($s -> fileExists($this -> domain, $yesterday_file_name)){
					$yesterday_content = $s -> read($this -> domain, $yesterday_file_name);
					if($yesterday_content == $content){
						return $this -> showNoContent($root);
					}
				}

			}
			else{
				return $this -> showNoContent($root);
			}
		}
		else{
			if($s -> fileExists($this -> domain, $file_name)){
				$content = $s -> read($domain, $file_name);
			}
			else{
				return $this -> showNoContent($root);
			}
		}
		return $content;
	}

	/* get the news items from Zhihu */
	public function getTitlesFromZhihuAPI($date){
		if($date != null)
			$this -> titles_api_url = $this -> titles_api_url_ori.($date + 1);
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
		return $xml_data;
	}

	/* get and show the special news as XML file */
	public function getNewsContent($url = null){
		$root = "news";
		$xml_data = new SimpleXMLElement("<?xml version=\"1.0\" encoding=\"GB2312\"?><".$root."></".$root.">");
		if(!isset($url) || $url == ""){
			return $this -> showNoContent($root);
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

		/* handle the html code */
		$results["body"] = $this -> htmlHandler($results["body"], $results["css"], $results["js"]);

		/*covert array into XML file and then show the content */
		$this -> arrayToXML($results, $xml_data);
		return $xml_data -> asXML();
	}

	/* HTML Handler */
	private function htmlHandler($html_code, $css_urls = null, $js_urls = null){
		/* load html */
		$dom = new DOMDocument("1.0", "utf-8");
		$meta = '<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>';
		@$dom->loadHTML($meta . $html_code);
		$dom -> formatOutput = true;

		/* remove headline */
		$divs = $dom -> getElementsByTagName("div");
		for($i = 0; $i < $divs -> length; ++$i){
			$node = $divs -> item($i);
			if($node -> attributes -> item(0) -> value == "headline"){
				$node -> parentNode -> removeChild($node);
				break;
			}
		}

		/* head */
		$head_node = $dom -> getElementsByTagName("head") -> item(0);

		/* css */
		foreach($css_urls as $css_url){
			$css_node = $dom -> createElement("link");
			$elm_type_attr = $dom -> createAttribute('type');
			$elm_type_attr->value = 'text/css';
			$css_node -> appendChild($elm_type_attr);	
			$elm_type_attr = $dom -> createAttribute('rel');
			$elm_type_attr->value = 'stylesheet';
			$css_node -> appendChild($elm_type_attr);	
			$elm_type_attr = $dom -> createAttribute('href');
			$elm_type_attr->value = $css_url;
			$css_node -> appendChild($elm_type_attr);	
			$head_node -> appendChild($css_node);
		}
		foreach($js_urls as $js_url){
			$js_node = $dom -> createElement("script");
			$elm_type_attr = $dom -> createAttribute('src');
			$elm_type_attr->value = $js_url;
			$js_node -> appendChild($elm_type_attr);	
			$head_node -> appendChild($js_node);
		}

		return $dom -> saveHTML();
	}

	/* show special content if no content can be shown */
	private function showNoContent($root){
		return "<?xml version=\"1.0\" encoding=\"GB2312\"?><".$root."></".$root.">";
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
