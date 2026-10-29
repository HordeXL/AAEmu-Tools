<#
.SYNOPSIS
    AAEmu-Tools 本地化检查脚本。

.DESCRIPTION
    检查中文源码编码、Designer 未翻译文本、MessageBox 英文文案，
    以及被误翻译的 ShowSelectedData 数据库表名。

.PARAMETER WarnOnly
    发现问题时只输出警告，仍返回 0。

.PARAMETER Build
    检查完成后构建 DBViewer 与 DBEditor。

.EXAMPLE
    pwsh -File scripts/check-localization.ps1
    pwsh -File scripts/check-localization.ps1 -WarnOnly
    pwsh -File scripts/check-localization.ps1 -Build
#>
[CmdletBinding()]
param(
    [switch]$WarnOnly,
    [switch]$Build
)

$ErrorActionPreference = 'Stop'
$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
Push-Location $repoRoot
try {
    $files = @(git ls-files)
    if ($LASTEXITCODE -ne 0) {
        throw '当前目录不是 git 仓库。'
    }

    $issues = [System.Collections.Generic.List[string]]::new()
    function Add-Issue([string]$message) {
        $issues.Add($message)
        Write-Host "  [!] $message" -ForegroundColor Yellow
    }

    # 这些是数据库字段名、GM 命令或控件内部名称，允许保留英文。
    $designerAllow = @(
        'ID', 'DBID', 'SKU', 'NPC', 'BUFFs', 'toolStripMenuItem2', 'statusStrip1', 'toolStrip1',
        '/addbuff <id>', '/doodad spawn', '/additem', '/spawn', '/quest', '/item', '0c',
        '-- , -- | 0°N, 0°E', 'AAEmu.Editor  ', 'AAEmu.DBViewer', 'NPC (F6)'
    )

    Write-Host '== 1. UTF-8 BOM / 乱码检查 ==' -ForegroundColor Cyan
    foreach ($f in $files) {
        if ($f -notmatch '\.(cs|resx)$') { continue }
        $path = Join-Path $repoRoot $f
        if (-not (Test-Path -LiteralPath $path)) { continue }
        $bytes = [IO.File]::ReadAllBytes($path)
        $text = [Text.Encoding]::UTF8.GetString($bytes)
        if ($text.Contains([char]0xFFFD)) {
            Add-Issue "${f}: 包含 U+FFFD 替换字符（可能编码损坏）"
        }
        if ($f -match '\.cs$' -and $text -match '[\u4e00-\u9fff]') {
            $hasBom = ($bytes.Length -ge 3 -and $bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF)
            if (-not $hasBom) {
                Add-Issue "${f}: 含中文的 .cs 文件缺少 UTF-8 BOM"
            }
        }
    }

    Write-Host '== 2. Designer 未翻译控件文本 ==' -ForegroundColor Cyan
    foreach ($f in $files) {
        if ($f -notmatch '\.Designer\.cs$') { continue }
        $lines = [IO.File]::ReadAllLines((Join-Path $repoRoot $f))
        for ($i = 0; $i -lt $lines.Length; $i++) {
            $line = $lines[$i].Trim()
            if ($line.StartsWith('//')) { continue }
            $m = [regex]::Match($line, '\.(Text|HeaderText|ToolTipText|Title)\s*=\s*"((?:[^"\\]|\\.)*)"\s*;')
            if (-not $m.Success) { continue }
            $value = $m.Groups[2].Value
            if ($value -notmatch '[A-Za-z]' -or $value -match '[\u4e00-\u9fff]') { continue }
            if ($designerAllow -contains $value) { continue }
            Add-Issue "${f}:$($i + 1): 未翻译控件文本 [$value]"
        }
    }

    Write-Host '== 3. MessageBox 英文文案 ==' -ForegroundColor Cyan
    foreach ($f in $files) {
        if ($f -match '\.Designer\.cs$' -or $f -notmatch '\.cs$') { continue }
        $lines = [IO.File]::ReadAllLines((Join-Path $repoRoot $f))
        for ($i = 0; $i -lt $lines.Length; $i++) {
            if ($lines[$i] -notmatch 'MessageBox\.Show\s*\(') { continue }
            $buffer = ($lines[$i] -replace '//.*$', '')
            $j = $i
            while ($j -lt $lines.Length - 1 -and $j -lt $i + 5 -and
                   (([regex]::Matches($buffer, '\(').Count) -gt ([regex]::Matches($buffer, '\)').Count))) {
                $j++
                $buffer += ' ' + ($lines[$j] -replace '//.*$', '')
            }
            $matches = [regex]::Matches($buffer, '(?:\$@|@\$|@|\$)?"((?:[^"\\]|\\.)*)"')
            foreach ($m in $matches) {
                $value = $m.Groups[1].Value
                if ($value -notmatch '[A-Za-z]' -or $value -match '[\u4e00-\u9fff]') { continue }
                if ($value -match '^(?:\r|\n|\\r|\\n)+$') { continue }
                Add-Issue "${f}:$($i + 1): MessageBox 含英文文案 [$value]"
            }
        }
    }

    Write-Host '== 4. ShowSelectedData 表名 ==' -ForegroundColor Cyan
    foreach ($f in $files) {
        if ($f -notmatch '\.cs$' -or $f -match '\.Designer\.cs$') { continue }
        $lines = [IO.File]::ReadAllLines((Join-Path $repoRoot $f))
        for ($i = 0; $i -lt $lines.Length; $i++) {
            $line = $lines[$i]
            if ($line.Trim().StartsWith('//')) { continue }
            $m = [regex]::Match($line, 'ShowSelectedData\s*\(\s*"([^"]+)"')
            if (-not $m.Success) { continue }
            $table = $m.Groups[1].Value
            if ($table -notmatch '^[a-z][a-z0-9_]*$') {
                Add-Issue "${f}:$($i + 1): ShowSelectedData 表名应为小写数据库表名，实际为 [$table]"
            }
        }
    }

    if ($Build) {
        Write-Host '== 5. 构建检查 ==' -ForegroundColor Cyan
        dotnet build 'AAEmu.DBViewer\AAEmu.DBViewer.csproj' -c Debug --nologo -v m
        if ($LASTEXITCODE -ne 0) { Add-Issue 'AAEmu.DBViewer 构建失败' }
        dotnet build 'AAEmu.DBEditor\AAEmu.DBEditor.csproj' -c Debug --nologo -v m
        if ($LASTEXITCODE -ne 0) { Add-Issue 'AAEmu.DBEditor 构建失败' }
    }

    Write-Host ''
    if ($issues.Count -eq 0) {
        Write-Host '本地化检查通过。' -ForegroundColor Green
        exit 0
    }

    Write-Host "发现 $($issues.Count) 个本地化问题。" -ForegroundColor Red
    if ($WarnOnly) { exit 0 }
    exit 1
}
finally {
    Pop-Location
}
