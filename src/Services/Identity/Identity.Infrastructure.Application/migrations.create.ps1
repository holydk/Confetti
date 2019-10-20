param(
    [string] $i = $null,
    [string] $p = $null,
    [string] $c = $null
)

""
"##############################################################"
"############ Identity.Infrastructure.Application #############"
"### Add AspNetCore Identity and IdentityServer4 Migrations ###"
"##############################################################"
""

$migrationContexts = @(
    [PSCustomObject]@{
        Name = "AppIdentityDbContext"
        MigrationName = $i
        OutputDirectoryPath = ".\Infrastructure\Migrations\AspNetIdentity"
    }
    [PSCustomObject]@{
        Name = "PersistedGrantDbContext"
        MigrationName = $p
        OutputDirectoryPath = ".\Infrastructure\Migrations\IdentityServer4\PersistedGrantDb"
    }
    [PSCustomObject]@{
        Name = "ConfigurationDbContext"
        MigrationName = $c
        OutputDirectoryPath = ".\Infrastructure\Migrations\IdentityServer4\ConfigurationDb"
    }
)

$projectName = "Identity.Infrastructure.Application"
$project = Get-ChildItem -Path ".\" -File -Filter "$($projectName).csproj" -Recurse

foreach ($context in $migrationContexts) {
    if ($context.MigrationName) {
        dotnet ef migrations add $context.MigrationName --project "$($project.DirectoryName)\$($projectName).csproj" --context $context.Name --output-dir "$($context.OutputDirectoryPath)"
    }
}