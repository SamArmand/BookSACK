$connectionString = "Data Source=h98ohmld2f.database.windows.net;Initial Catalog=BookSACK;Integrated Security=False;User ID=JMSXTech;Password=jmsx!2014;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;"

$books = Import-Csv "books.csv"

foreach ($book in $books) {
    
    if ($book.Holdout -ne "Test") {
        continue
    }

    $params = @{
        synopsis = $book.Synopsis;
    }

    $result = Invoke-WebRequest -Uri "http://booksack.azurewebsites.net/api/Book" -Method POST -Body (ConvertTo-Json $params) -ContentType 'application/json'
    
    $result

}

