# to run the tests locally we need to run Invoke-Pester directly in the pester.tests folder
# the module itself must be present in c:\temp\ycps
# the following modules must be also installed
# Install-Module Microsoft.PowerShell.SecretManagement, Microsoft.PowerShell.SecretStore
# few secrets must be created in the local PowerShell.SecretStore:
# YandexOAuthToken, YandexTestCloudId, YandexTestFolderId


# Import-Module $modulePath -Verbose
Import-Module c:\temp\ycps -Verbose
Connect-YcAccount -OAuthToken (Get-Secret -Name YandexOAuthToken -AsPlainText)


Describe 'Basic Get-YcCloud tests' {
    It 'No parameters - list clouds' {
        Get-YcCloud
    }
}

Describe 'Basic Get-YcFolder tests' {
    It 'No parameters - list folders of a cloud' {
        $testCloudId = (Get-Secret -Name YandexTestCloudId -AsPlainText)
        
        Get-YcFolder -CloudId $testCloudId
    }

    It 'Pipeline binding - iterate through clouds' {
        Get-YcCloud | Get-YcFolder
    }

    It 'Get single folder by id' {
        $testFolderId = (Get-Secret -Name YandexTestFolderId -AsPlainText)
        
        Get-YcFolder -FolderId $testFolderId
    }
}


Describe 'Basic Organization tests' {
    It 'Get organization - no parameters' {
        Get-YcOrganization
    }
}