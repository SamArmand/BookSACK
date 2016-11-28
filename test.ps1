$connectionString = "Data Source=h98ohmld2f.database.windows.net;Initial Catalog=BookSACK;Integrated Security=False;User ID=JMSXTech;Password=jmsx!2014;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;"

$books = Import-Csv "books.csv"

$correct = 0;
$total = 0;

foreach ($book in $books) {

    if ($book.Holdout -ne "Test") {
        continue
    }

    $params = @{
        synopsis = $book.Synopsis;
    }

    $result = Invoke-WebRequest -Uri "http://booksack.azurewebsites.net/api/Book" -Method POST -Body (ConvertTo-Json $params) -ContentType 'application/json'
    

    if ($book.Subcategory -eq $result.Content) {

        $correct++
        write-host $result.Content -ForegroundColor "green"

    }
    
    else {

        
        Write-host "-----------------------------------------------------" -foregroundcolor "red"
        write-host "PREDICTED: $($result.Content)" -foregroundcolor "red"
        write-host "ACTUAL: $($book.Subcategory)"
        Write-host "-----------------------------------------------------" -foregroundcolor "red"

    }


    $total++


}

$tally = "$correct/$total"

Write-Host $tally 

