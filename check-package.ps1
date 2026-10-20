# NuGet 包内容校验：断言 lib 目标框架资产、README、图标与包 ID 齐全
# 打包脚本改动、目标框架增删、csproj 改名都会让发布包悄悄缺东西，这里在推送前拦下。
#
# 用法：
#   pwsh -File check-package.ps1                          # 校验 out 目录下最新的 nupkg
#   pwsh -File check-package.ps1 -Path out -Project Newlife.JT808\NewLife.JT808.csproj
param(
    [String]$Path = "out",
    [String]$Project = "Newlife.JT808\NewLife.JT808.csproj",
    [String]$PackageId = "NewLife.JT808"
)

$ErrorActionPreference = "Stop"

$root = $PSScriptRoot

$projPath = if ([IO.Path]::IsPathRooted($Project)) { $Project } else { Join-Path $root $Project }
if (!(Test-Path -LiteralPath $projPath)) {
    Write-Host "未找到包项目：$projPath" -ForegroundColor Red
    exit 1
}

# 目标框架以 csproj 为唯一事实源，避免脚本与项目各自维护一份列表
$projText = Get-Content -LiteralPath $projPath -Raw -Encoding UTF8
$match = [regex]::Match($projText, '<TargetFrameworks>([^<]+)</TargetFrameworks>')
if (!$match.Success) {
    Write-Host "包项目未声明 TargetFrameworks：$projPath" -ForegroundColor Red
    exit 1
}
$tfms = @($match.Groups[1].Value.Split(';') | ForEach-Object { $_.Trim() } | Where-Object { $_ })

$outDir = if ([IO.Path]::IsPathRooted($Path)) { $Path } else { Join-Path $root $Path }
if (!(Test-Path -LiteralPath $outDir)) {
    Write-Host "未找到打包输出目录：$outDir" -ForegroundColor Red
    exit 1
}

$nupkg = Get-ChildItem -LiteralPath $outDir -Filter *.nupkg | Sort-Object LastWriteTime -Descending | Select-Object -First 1
if (!$nupkg) {
    Write-Host "目录中没有 nupkg：$outDir" -ForegroundColor Red
    exit 1
}

$zip = [IO.Compression.ZipFile]::OpenRead($nupkg.FullName)
try {
    $entries = @($zip.Entries | ForEach-Object { $_.FullName })
}
finally {
    $zip.Dispose()
}

$required = @($tfms | ForEach-Object { "lib/$_/$PackageId.dll"; "lib/$_/$PackageId.xml" }) + @("README.nuget.md", "leaf.png")
$missing = @($required | Where-Object { $entries -notcontains $_ })

# 包 ID 随程序集名变化，csproj 改名或 AssemblyName 被改都会让发布出的包换名字
$nuspec = @($entries | Where-Object { $_ -like '*.nuspec' } | Select-Object -First 1)
if ($nuspec.Count -eq 0) { $missing += "$PackageId.nuspec" }
elseif ($nuspec[0] -ne "$PackageId.nuspec") { $missing += "$PackageId.nuspec（实际 $($nuspec[0])）" }

if ($missing.Count -gt 0) {
    Write-Host "包内容校验失败：$($nupkg.Name) 缺少 $($missing -join ', ')" -ForegroundColor Red
    exit 1
}

Write-Host "包内容校验通过：$($nupkg.Name)，$($tfms.Count) 个目标框架（$($tfms -join '/')）、README 与图标齐全"
