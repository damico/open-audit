<?php
header("Content-type: text/xml");
$action = @$_GET["action"];
$data = @$_GET["data"];

$xml = NULL;

if(isset($action) && $action == "config"){

	$xml = "<?xml version='1.0' encoding='UTF-8' standalone='yes'?>\n".
			"<openaudit>\n".
				"<config strId='$data' remoteServer='https://eigmercados.com.br/open-audit-proxy/proxy.php' remoteTarget='https://www.google.com'>\n".
				"</config>\n".
			"</openaudit>";

}
echo($xml);
?>