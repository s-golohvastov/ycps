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
        $ret = Get-YcVpc -FolderId (Get-Secret -Name YandexTestFolderId -AsPlainText)
        $ret | Should -Not -BeNullOrEmpty
    }

    It '2. Get a VPC by Id (folder id not needed)' {
        $ret = Get-YcVpc -NetworkId (Get-Secret -Name YandexTestNetworkId -AsPlainText)
        $ret | Should -Not -BeNullOrEmpty
    }

    It '3. Simple filtering' {
        $ret = Get-YcVpc -FolderId (Get-Secret -Name YandexTestFolderId -AsPlainText) -NetworkName "default"
        $ret | Should -Not -BeNullOrEmpty
    }

    It '4. Pipeline binding folder objects/list all VMs in a set of folders' {
        $cloudId = Get-Secret -Name YandexTestCloudId -AsPlainText
        $ret = Get-YcCloud -CloudId $cloudId | Get-YcFolder | Get-YcVpc -NetworkName "default"
        $ret | Should -Not -BeNullOrEmpty
    }
}

Describe 'Basic Get-YcSubnet tests' {
    It '1. No parameters - list Subnets in a folder' {
        $ret = Get-YcSubnet -FolderId (Get-Secret -Name YandexTestFolderId -AsPlainText)
        $ret | Should -Not -BeNullOrEmpty
    }

    It '2. Get a Subnet by Id (folder id not needed)' {
        $ret = Get-YcSubnet -SubnetId (Get-Secret -Name YandexTestSubnetId -AsPlainText)
        $ret | Should -Not -BeNullOrEmpty
    }

    It '3. Simple filtering' {
        $ret = Get-YcSubnet -FolderId (Get-Secret -Name YandexTestFolderId -AsPlainText) -SubnetName "default-ru-central1-c"
        $ret | Should -Not -BeNullOrEmpty
    }

    It '4. Pipeline binding folder objects/list all VMs in a set of folders' {
        $cloudId = Get-Secret -Name YandexTestCloudId -AsPlainText
        $ret = Get-YcCloud -CloudId $cloudId | Get-YcFolder | Get-YcSubnet -SubnetName "default-ru-central1-b"
        $ret | Should -Not -BeNullOrEmpty
    }
}


Describe 'New-YcVpc Test' {
    It 'Create and remove VPC' {
        $ret = New-YcVpc -FolderId (Get-Secret -Name YandexTestFolderId -AsPlainText) -Name testVpc -Description "cccc"
        $ret | Should -Not -BeNullOrEmpty

        Remove-YcVpc -NetworkId $ret.id
    }
}


Describe 'Get-YcZone' {
    It 'Get list of zones' {
        $ret = Get-YcZone
        $ret | Should -Not -BeNullOrEmpty
    }
}

Describe 'New-YcSubnet' {
    It 'Create VPC and subnets in it' {
        $zones = Get-YcZone
        $zones | Should -Not -BeNullOrEmpty

        $network = New-YcVpc -FolderId (Get-Secret -Name YandexTestFolderId -AsPlainText) -Name testVpc -Description "VPC for subnet testing"

        $i = 0
        $ranges = "192.168.1.0/24", "192.168.2.0/24", "192.168.3.0/24"
        $zones | % {
            New-YcSubnet -FolderId $network.FolderId -NetworkId $network.id -Name "subnet-$i" -ZoneId $_.id -Ipv4Ranges $ranges[$i]
            $i += 1
        }
    }

    It 'Remove VPC along with nested subnets' {
        $vpc = Get-YcVpc -FolderId (Get-Secret -Name YandexTestFolderId -AsPlainText) -NetworkName "testVpc"
        Remove-YcVpc -NetworkId $vpc.id -Force
    }
}