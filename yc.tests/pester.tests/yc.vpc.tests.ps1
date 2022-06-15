# to run the tests locally we need to run Invoke-Pester directly in the pester.tests folder
# the module itself must be present in c:\temp\ycps
# the following modules must be also installed
# Install-Module Microsoft.PowerShell.SecretManagement, Microsoft.PowerShell.SecretStore
# few secrets must be created in the local PowerShell.SecretStore:
# YandexOAuthToken, YandexTestFolderId, YandexTestNetworkId, YandexTestSubnetId

$ModuleManifestName = 'ipmgmt.psd1'
$ModuleManifestPath = "$PSScriptRoot\..\$ModuleManifestName"
$modulePath = "$PSScriptRoot\.."

# Import-Module $modulePath -Verbose
Import-Module c:\temp\ycps -Verbose
Connect-YcAccount -OAuthToken (Get-Secret -Name YandexOAuthToken -AsPlainText)


Describe 'Basic Get-YcVpc tests' {
    It '1. No parameters - list VPC in a folder' {
        $vpc = Get-YcVpc -FolderId (Get-Secret -Name YandexTestFolderId -AsPlainText)
        $vpc | Should -Not -BeNullOrEmpty
    }

    It '2. Get a VPC by Id (folder id not needed)' {
        $vpc = Get-YcVpc -NetworkId (Get-Secret -Name YandexTestNetworkId -AsPlainText)
        $vpc | Should -Not -BeNullOrEmpty
    }

    It '3. Simple filtering' {
        $vpc = Get-YcVpc -FolderId (Get-Secret -Name YandexTestFolderId -AsPlainText) -NetworkName "default"
        $vpc | Should -Not -BeNullOrEmpty
    }

    It '4. Pipeline binding folder objects/list all VMs in a set of folders' {
        $cloudId = Get-Secret -Name YandexTestCloudId -AsPlainText
        $vpc = Get-YcCloud -CloudId $cloudId | Get-YcFolder | Get-YcVpc -NetworkName "default"
        $vpc | Should -Not -BeNullOrEmpty
    }
}

Describe 'Basic Get-YcSubnet tests' {
    It '1. No parameters - list Subsnts in a folder' {
        $vpc = Get-YcSubnet -FolderId (Get-Secret -Name YandexTestFolderId -AsPlainText)
        $vpc | Should -Not -BeNullOrEmpty
    }

    It '2. Get a Subnet by Id (folder id not needed)' {
        $vpc = Get-YcSubnet -SubnetId (Get-Secret -Name YandexTestSubnetId -AsPlainText)
        $vpc | Should -Not -BeNullOrEmpty
    }

    It '3. Simple filtering' {
        $vpc = Get-YcSubnet -FolderId (Get-Secret -Name YandexTestFolderId -AsPlainText) -SubnetName "default-ru-central1-c"
        $vpc | Should -Not -BeNullOrEmpty
    }

    It '4. Pipeline binding folder objects/list all VMs in a set of folders' {
        $cloudId = Get-Secret -Name YandexTestCloudId -AsPlainText
        $vpc = Get-YcCloud -CloudId $cloudId | Get-YcFolder | Get-YcSubnet -SubnetName "default-ru-central1-b"
        $vpc | Should -Not -BeNullOrEmpty
    }
}