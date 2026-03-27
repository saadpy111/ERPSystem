$entities = @(
    "GoodsReceipt",
    "GoodsReceiptItem",
    "PurchaseInvoice",
    "PurchaseOrder",
    "PurchaseOrderItem",
    "PurchaseRequisition",
    "Vendor"
)

$configDir = "e:\ERPAnti\ERPSystem\Modules\ProcurementModule\Procurement.Persistence\Configurations"

foreach ($entity in $entities) {
    $filePath = Join-Path $configDir "$($entity)Configuration.cs"

    if (Test-Path $filePath) {
        $content = Get-Content $filePath -Raw
        
        $pattern = "Configure\(\s*EntityTypeBuilder\s*<\s*$entity\s*>\s+(\w+)\s*\)"
        if ($content -match $pattern) {
            $builderName = $matches[1]
            $modified = $false

            if ($content -notmatch "TenantId") {
                $codeToInject = "`r`n            // Multi-tenancy indexes"
                $codeToInject += "`r`n            $builderName.HasIndex(x => x.TenantId);"
                $codeToInject += "`r`n            $builderName.HasIndex(x => new { x.TenantId, x.Id });"

                $lastBraceIndex = $content.LastIndexOf("        }")
                if ($lastBraceIndex -gt -1) {
                    $content = $content.Insert($lastBraceIndex, $codeToInject + "`r`n")
                    $modified = $true
                }
            }
            
            if ($content -notmatch "\.HasKey\(") {
                $keyCode = "`r`n            $builderName.HasKey(x => x.Id);"
                $firstOpenedBrace = $content.IndexOf("{", $content.IndexOf("Configure("))
                if ($firstOpenedBrace -gt -1) {
                    $content = $content.Insert($firstOpenedBrace + 1, $keyCode)
                    $modified = $true
                }
            }

            if ($modified) {
                Set-Content $filePath -Value $content
                Write-Host "Updated $($entity)Configuration.cs"
            }
        }
    }
}
Write-Host "Done."
