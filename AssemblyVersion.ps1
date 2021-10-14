$file = "./Razor/Properties/AssemblyInfo.cs"
$version = "1.7.2.$Env:GITHUB_RUN_ID"

$fileObject = Get-Item $file

Write-Verbose "Changing version in $($fileObject.FullName) to $version"

$sr = New-Object System.IO.StreamReader($fileObject, [System.Text.Encoding]::GetEncoding("utf-8"))
$content = $sr.ReadToEnd()
$sr.Close()

$content = [Regex]::Replace($content, "(\d+)\.(\d+)\.(\d+)[\.(\d+)]*", $version);

$sw = New-Object System.IO.StreamWriter($fileObject, $false, [System.Text.Encoding]::GetEncoding("utf-8"))
$sw.Write($content)
$sw.Close()