$connectionString = "Data Source=h98ohmld2f.database.windows.net;Initial Catalog=BookSACK;Integrated Security=False;User ID=JMSXTech;Password=jmsx!2014;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;"

$connection = New-Object -TypeName System.Data.SqlClient.SqlConnection
$connection.ConnectionString = $connectionString
$command = $connection.CreateCommand()

$query = "DELETE FROM Dictionaries;"

$command.CommandText = $query
$connection.Open()
$command.ExecuteNonQuery()

$connection.close()

$books = Import-Csv "books.csv"

foreach ($book in $books) {
    
    if ($book.Holdout -ne "Train") {
        continue
    }

    $params = @{
        synopsis = $book.Synopsis;
        subcategory = $book.Subcategory;
    }

    Invoke-WebRequest -Uri "http://booksack.azurewebsites.net/api/TrainingBook" -Method POST -Body (ConvertTo-Json $params) -ContentType 'application/json'
    
}

