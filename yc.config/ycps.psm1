Get-ChildItem $PSScriptRoot\yc.*.dll | ForEach-Object {
    write-verbose "dot sourcing $($_.FullName)"
    import-module $_.FullName
}