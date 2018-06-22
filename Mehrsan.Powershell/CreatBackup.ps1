$svnm = 'L4KD6162\SQLEXPRESS'
$dbname = 'Word'
$dt = get-date -format yyyyMMddHHmmss
$bdir = 'D:\Code\mehran\Mehrsan_School\Mehrsan.Word\Mehrsan.Db\Backups\'
$bfil = "$bdir\$($dbname)_db_$($dt).bak"
Backup-SqlDatabase <<-ServerInstance $svnm -Database $dbname -BackupFile $bfil