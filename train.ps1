clear

Invoke-WebRequest -Uri "http://booksack.azurewebsites.net/api/TrainingBook" -Method Delete

$books = Import-Csv "books.csv"

$count = 0;

foreach ($book in $books) {

    if ($book.Holdout -eq "Train") {

        Write-Host "Completed $count out of $($books.Count)"

        Invoke-WebRequest -Uri "http://booksack.azurewebsites.net/api/TrainingBook" -Method POST -Body (ConvertTo-Json @{synopsis = $book.Synopsis; subgenre = $book.Subgenre}) -ContentType 'application/json'
        
        $count++

    }

    clear

}

$correct = 0;
$total = 0;

foreach ($book in $books) {

    if ($book.Holdout -ne "Test") {
        continue
    }

    $result = Invoke-WebRequest -Uri "http://booksack.azurewebsites.net/api/Book" -Method Post -Body (ConvertTo-Json @{synopsis = $book.Synopsis}) -ContentType 'application/json'
    
    if ($book.Subgenre -eq $result.Content) {

        $correct++
        Write-Host $result.Content -ForegroundColor Green

    }
    
    else {
        
        Write-Host "-----------------------------------------------------" -ForegroundColor Red
        write-Host "PREDICTED: $($result.Content)" -ForegroundColor Red
        write-Host "ACTUAL: $($book.Subgenre)"
        Write-Host "-----------------------------------------------------" -ForegroundColor Red

    }

    $total++

}

Write-Host "Results: $correct/$total" 

#localhost:5000
#booksack.azurewebsites.net