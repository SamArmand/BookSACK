$books = Import-Csv "books.csv"

$correct = 0;
$total = 0;

foreach ($book in $books) {

    if ($book.Holdout -ne "Test") {
        continue
    }

    $params = 

    $result = Invoke-WebRequest -Uri "http://booksack.azurewebsites.net/api/Book" -Method POST -Body (ConvertTo-Json @{synopsis = $book.Synopsis}) -ContentType 'application/json'
    
    if ($book.Subgenre -eq $result.Content) {

        $correct++
        write-host $result.Content -ForegroundColor "green"

    }
    
    else {
        
        Write-host "-----------------------------------------------------" -foregroundcolor "red"
        write-host "PREDICTED: $($result.Content)" -foregroundcolor "red"
        write-host "ACTUAL: $($book.Subgenre)"
        Write-host "-----------------------------------------------------" -foregroundcolor "red"

    }

    $total++

}

Write-Host "Results: $correct/$total" 

