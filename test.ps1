$books = Import-Csv "books.csv"

$correct = 0
$total = 0

foreach ($book in $books) {

    if ($book.Holdout -ne "Test") {
        continue
    }

    $result = Invoke-WebRequest -Uri "http://booksack.azurewebsites.net/api/Book" -Method POST -Body (ConvertTo-Json @{synopsis = $book.Synopsis}) -ContentType 'application/json'
    
    if ($book.Subgenre -eq $result.Content) {
        ++$correct
        Write-Host "$($result.Content)" -ForegroundColor Green
    }
    
    else {
        Write-host "-----------------------------------------------------" -ForegroundColor Red
        write-host "    PREDICTED: $($result.Content)          " -ForegroundColor Red
        write-host "    ACTUAL:    $($book.Subgenre)              "
        Write-host "    -----------------------------------------------------" -ForegroundColor Red
    }

    #$r = Invoke-WebRequest -Uri "http://booksack.azurewebsites.net/api/TrainingBook" -Method POST -Body (ConvertTo-Json @{synopsis = $book.Synopsis; subgenre = $book.Subgenre}) -ContentType 'application/json'
    $total++

}

Write-Host "    Results: $correct/$total                  " 

