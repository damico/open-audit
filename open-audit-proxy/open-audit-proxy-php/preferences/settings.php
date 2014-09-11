<?php 


function getDbType(){
	return 'mysql';
}

function getConnStr(){
    
    $server = '127.0.0.1';
    $user   = 'xxxx';
    $password = 'xxxx';
    $database = 'xxxx';
    $port = '3306';

    return "host=$server port=$port dbname=$database user=$user password=$password";
}

?>