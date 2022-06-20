# to run the tests locally we need to run Invoke-Pester directly in the pester.tests folder
# the module itself must be present in c:\temp\ycps
# the following modules must be also installed
# Install-Module Microsoft.PowerShell.SecretManagement, Microsoft.PowerShell.SecretStore
# few secrets must be created in the local PowerShell.SecretStore:
# YandexOAuthToken, YandexTestFolderId, YandexTestVmId

$ModuleManifestName = 'ipmgmt.psd1'
$ModuleManifestPath = "$PSScriptRoot\..\$ModuleManifestName"
$modulePath = "$PSScriptRoot\.."

# Import-Module $modulePath -Verbose
Import-Module c:\temp\ycps -Verbose
Connect-YcAccount -OAuthToken (Get-Secret -Name YandexOAuthToken -AsPlainText)


Describe 'Basic Get-YcVM tests' {
    It 'No parameters - list VMs in a folder' {
        $vm = Get-YcVM -FolderId (Get-Secret -Name YandexTestFolderId -AsPlainText)
        $vm | Should -Not -BeNullOrEmpty
    }

    It 'Get a VM by Id (folder id not needed)' {
        $vm = Get-YcVM -InstanceId (Get-Secret -Name YandexTestVmId -AsPlainText)
        $vm | Should -Not -BeNullOrEmpty
    }

    It 'Simple filtering' {
        $vm = Get-YcVM -FolderId (Get-Secret -Name YandexTestFolderId -AsPlainText) -InstanceName "ya-vpn"
        $vm | Should -Not -BeNullOrEmpty
    }

    It 'Pipeline binding folder objects/list all VMs in a set of folders' {
        $cloudId = Get-Secret -Name YandexTestCloudId -AsPlainText
        $vm = Get-YcCloud -CloudId $cloudId | Get-YcFolder | Get-YcVM
        $vm | Should -Not -BeNullOrEmpty
    }
}

Describe 'New-YcVm tests' {
    It 'Create a simple Linux VM with no data disk' {
        $testFolderId = (Get-Secret -Name YandexTestFolderId -AsPlainText)
        $platformId = "standard-v1"
        $zoneId = "ru-central1-a"
        $subnetId = "e9b7860bfmo8149bqos4"

        $vmSpec = New-YcVmSpecification -Memory 4GB -Cores 4 -CoreFraction 5
        $bootDisk = New-YcDiskSpecification -Name "boot01" -Size (32GB) -TypeId "network-hdd" -BlockSize 8192 -ImageId (Get-YcVmImage -Family "ubuntu-2004-lts").id
        New-YcVm -Name "test-vm-from-pester" -FolderId $testFolderId -ZoneId $zoneId -Platform $platformId -SubnetId $subnetId -ResourceSpec $vmSpec -BootDiskSpec $bootDisk
    }
}