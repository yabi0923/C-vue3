window power shell
$env:ConnectionStrings__DefaultConnection="Server=localhost;Database=SmartCommunityDB;User Id=sa;Password=YourStrongPassword!;TrustServerCertificate=True"
dotnet run --project backend

version: "3.8"
services:
  api:
    image: your-api-image
    build: ./backend
    environment:
      - ConnectionStrings__DefaultConnection=Server=db;Database=SmartCommunityDB;User Id=sa;Password=${SA_PASSWORD};TrustServerCertificate=True
    depends_on:
      - db

  db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - SA_PASSWORD=${SA_PASSWORD}
      - ACCEPT_EULA=Y
    ports:
      - "1433:1433"
