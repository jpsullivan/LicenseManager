[CmdletBinding()]
param(
  )

  Set-StrictMode -Version Latest
  $ErrorActionPreference = 'Stop'
  $PSScriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition

  Write-Output $PSScriptRoot

  #$FluentMigratorExePath = Join-Path $PSScriptRoot \ssl.pfx
  $FluentMigratorExePath = (dir $$PSScriptRoot\..\..\packages\FluentMigrator.Tools.*\tools\AnyCPU\40\Migrate.exe | select -first 1 -expand FullName)
  $LicenseManagerDLLPath = (dir $$PSScriptRoot\..\..\LicenseManager\bin\LicenseManager.dll | select -first 1 -expand FullName)

  Write-Output $FluentMigratorExePath
  Write-Output $LicenseManagerDLLPath

  $steps = Read-Host "How many steps be rolled back?"

  & $FluentMigratorExePath /connection "Data Source=(local);Initial Catalog=LicenseManager;Integrated Security=SSPI;" /provider sqlserver2012 /assembly $LicenseManagerDLLPath --task rollback --steps $steps
  
