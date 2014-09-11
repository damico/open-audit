<?php
include_once ('commons/helper.php');
header("Content-type: text/xml");
$action = @$_GET["action"];
$data = @$_GET["data"];

$strId = @$_GET["strId"];
$errorcounter = @$_GET["errorcounter"];
$runcounter = @$_GET["runcounter"];

$ip = $_SERVER['REMOTE_ADDR'];

$xml = NULL;

if(isset($action) && $action == "config"){
	
	insertAuditLog($data, 0, 0, $ip, 0);

	$xml = "<?xml version='1.0' encoding='UTF-8' standalone='yes'?>\n".
			"<openaudit>\n".
				"<config strId='$data' remoteServer='https://eigmercados.com.br/open-audit-proxy/proxy.php' remoteTarget='https://www.google.com'>\n".
				"</config>\n".
			"</openaudit>";
	echo($xml);
}

// cfg.remoteServer + "?action=ACK&data=" + data + "&strId=" + cfg.strId + "&errorcounter=" + Constants.STATIC_ERROR_COUNTER + "&runcounter=" + Constants.STATIC_RUN_COUNTER);


if(isset($action) && $action == "ACK"){
	
	insertAuditLog($strId, $errorcounter, $runcounter, $ip, 1);
	$xml = "<?xml version='1.0' encoding='UTF-8' standalone='yes'?>\n".
			"<openaudit>\n".
			"<config />\n".
			"</openaudit>";
	echo($xml);
	
}

?>