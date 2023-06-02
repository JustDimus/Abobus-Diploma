param(
    [string]
        $file
)

$selfPath = Split-Path $PSCommandPath -Parent

$resultFile = Join-Path $selfPath "result.txt"

if (Test-Path $resultFile) {
    Remove-Item $resultFile -Force | Out-Null
}

$routeSource = Get-Content $file | ConvertFrom-Json

$target = $routeSource.routes.legs

$pointId = 1

foreach ($leg in $target){
    foreach ($step in $leg.steps) {
        $text = "new RoutePointModel()
    {
        Order = $pointId,
        IsDestination = false,
        Coordinate = new RouteCoordinateModel()
        {
            Latitude = $($step.end_location.lat),
            Longitude = $($step.end_location.lng),
        }
    },"
        Add-Content $resultFile -Value $text
        $pointId = $pointId + 1
    }
}