""
"#################################################################"
"############# Identity.Infrastructure.Application ###############"
"### Update AspNetCore Identity and IdentityServer4 Migrations ###"
"#################################################################"
""

$migrationContexts = @(
    [PSCustomObject]@{
        Name = "AppIdentityDbContext"
    }
    [PSCustomObject]@{
        Name = "PersistedGrantDbContext"
    }
    [PSCustomObject]@{
        Name = "ConfigurationDbContext"
    }
)

$projectName = "Identity.Infrastructure.Application"
$project = Get-ChildItem -Path ".\" -File -Filter "$($projectName).csproj" -Recurse

foreach ($context in $migrationContexts) {
    dotnet ef database update --context $context.Name --project "$($project.DirectoryName)\$($projectName).csproj"
}