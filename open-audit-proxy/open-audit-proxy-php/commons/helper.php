<?php
include_once ('adodb/adodb.inc.php');
include_once ('preferences/settings.php');

function insertAuditLog($strId, $errorcounter, $runcounter, $ip, $action) {
	
	    $db = NewADOConnection(getDbType());
	    $db->Connect("localhost", "xxxxx", "xxxxx", "xxxxxx");
		$sql = "INSERT INTO open_audit_log_t  (oac_errorn, oac_runn, oac_user, oac_ip, oac_action)VALUES($errorcounter, $runcounter, '$strId', '$ip', $action)";
		$db->Execute ( $sql );
		$db->Close ();
	

	return 0;
}

?>