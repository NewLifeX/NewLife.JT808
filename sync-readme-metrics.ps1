# README 指标同步与校验
#
# 单一事实源：readme-metrics.json。AOT 冒烟场景数、TFM 数量、内置语言数等
# 会随版本变化的数字，不再散落在 11 份 README 里手工维护。
#
# 用法：
#   pwsh -File sync-readme-metrics.ps1           # 按配置重写指标块
#   pwsh -File sync-readme-metrics.ps1 -Check    # 只校验不写入，不一致退出码 1（CI 使用）
param(
    [Switch]$Check,
    [String]$Root = $PSScriptRoot,
    [String]$Config = "readme-metrics.json"
)

$ErrorActionPreference = "Stop"

$configPath = Join-Path $Root $Config
if (!(Test-Path -LiteralPath $configPath)) { throw "未找到指标配置：$configPath" }

$metrics = Get-Content -LiteralPath $configPath -Raw -Encoding UTF8 | ConvertFrom-Json
$values = $metrics.values

$StartMarker = "<!-- metrics:start -->"
$EndMarker = "<!-- metrics:end -->"
$BlockPattern = "(?s)" + [regex]::Escape($StartMarker) + ".*?" + [regex]::Escape($EndMarker)

$files = @(Get-ChildItem -Path $Root -File | Where-Object {
    $_.Name -match '^(README|Readme)(\.[A-Za-z-]+)?\.md$' -and $_.Name -notmatch '\.(template|nuget)\.'
} | Sort-Object Name)
if ($files.Count -eq 0) { throw "根目录未找到 README 文件" }

# AOT 冒烟徽章里的斜杠需要 URL 编码，否则 shields.io 会把斜杠当路径分隔符
$smokeEncoded = [Uri]::EscapeDataString([String]$values.aotSmoke)

function Get-MetricsBlock([String]$newline) {
    $lines = @(
        $StartMarker
        "[![AOT smoke](https://img.shields.io/badge/AOT%20smoke-$smokeEncoded-brightgreen?logo=dotnet)](AotSmoke/Program.cs)"
        "[![TFM](https://img.shields.io/badge/TFM-$($values.tfmCount)%20%28net45%20%E2%86%92%20net8.0%29-512BD4?logo=dotnet)](Newlife.JT808/NewLife.JT808.csproj)"
        "[![locales](https://img.shields.io/badge/locales-$($values.localeCount)-2ea44f?logo=dotnet)](Doc/多语言本地化.md)"
        $EndMarker
    )
    return $lines -join $newline
}

$errors = 0
$changed = 0
foreach ($file in $files) {
    $text = [IO.File]::ReadAllText($file.FullName)
    $newline = if ($text.Contains("`r`n")) { "`r`n" } else { "`n" }
    $block = Get-MetricsBlock $newline

    if ($text.Contains($StartMarker) -and $text.Contains($EndMarker)) {
        $updated = [regex]::Replace($text, $BlockPattern, [System.Text.RegularExpressions.MatchEvaluator]{
            param($m) return $block
        })
    }
    else {
        # 首次迁移：优先在 CI 测试徽章行之后插入指标块（徽章顺序：许可证 → NuGet → CI → 指标 → 平台 → 贡献者）
        $pattern = "(?m)^.*actions/workflows/test\.yml.*$"
        if (-not [regex]::IsMatch($text, $pattern)) { $pattern = "(?m)^.*nuget/vpre.*$" }
        if (-not [regex]::IsMatch($text, $pattern)) { throw "$($file.Name) 既无指标块标记，也找不到 CI 测试徽章行" }
        $updated = [regex]::Replace($text, $pattern, [System.Text.RegularExpressions.MatchEvaluator]{
            param($m) return $m.Value + $newline + $block
        }, 1)
    }

    if ($updated -eq $text) { continue }

    $changed++
    if ($Check) {
        $errors++
        Write-Host "✗ $($file.Name)：指标块与 $Config 不一致"
    }
    else {
        [IO.File]::WriteAllText($file.FullName, $updated, (New-Object System.Text.UTF8Encoding($false)))
        Write-Host "✓ $($file.Name)"
    }
}

if ($Check) {
    if ($errors -gt 0) {
        Write-Host "`n指标块不一致：$errors / $($files.Count) 个文件。请运行 pwsh -File sync-readme-metrics.ps1 后提交。"
        exit 1
    }
    Write-Host "指标块一致（$($files.Count) 个 README）"
}
elseif ($changed -eq 0) {
    Write-Host "指标块已是最新（$($files.Count) 个 README）"
}
