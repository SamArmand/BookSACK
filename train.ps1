clear

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

$count = 0;

foreach ($book in $books) {

    if ($book.Holdout -eq "Train") {

        Write-Host "Completed $count out of $($books.Count)"

        Invoke-WebRequest -Uri "http://localhost:5000/api/TrainingBook" -Method POST -Body (ConvertTo-Json @{synopsis = $book.Synopsis; subgenre = $book.Subgenre}) -ContentType 'application/json'
        
        $count++

    }

    clear

}

#localhost:5000
#booksack.azurewebsites.net