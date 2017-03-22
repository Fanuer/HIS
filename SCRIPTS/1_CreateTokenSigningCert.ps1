# Creates a new SelfSignedCertificate. Is Valid one Year after Creation
# Source: http://woshub.com/how-to-create-self-signed-certificate-with-powershell/

New-SelfSignedCertificate -DnsName his-auth.azurewebsites.net -CertStoreLocation cert:\LocalMachine\My
$plaincertpw = Read-Host -Prompt 'Input your Certificate Password'
$thumbnail = Read-Host -Prompt 'Input the certificates thumbnail'

$CertPassword = ConvertTo-SecureString -String $plaincertpw -Force -AsPlainText
$certthumbnail = 'cert:\LocalMachine\My\' + $thumbnail
$currentDate = get-date -format dd.MM.yyyy
$outputPath = (Get-Item -Path ".\" -Verbose).FullName + '/cert_'+ $currentDate + '.pfx'
Export-PfxCertificate -Cert $certthumbnail -FilePath $outputPath -Password $CertPassword
Write-Host 'Press any key to continue ...'

$x = $host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown')