# Set Game Back-End

Het is aan te raden om V4 te gebruiken, deze is up to date. 
Het idee m.b.t. versies was om te laten zien hoe een applicatie incrementeel ontwikkeld zou kunnen worden, echter dit is niet goed gelukt voor de back-end :-(.

De Back-end is gemaakt in ASP.NET Core (NET versie 5) i.c.m. Entity Framework Core (EF Core) als ORM laag. 
Als database wordt MySQL gebruikt. 

## Installatie

Het is aan te raden de volgende producten te installeren:
- MySQL versie >= 8.0.23 
- >= NET5.0 
- Een IDE naar keuze, enkele mogelijkheden:
  - Rider (als student kan je dit gratis gebruiken m.b.v. een Educational Licentie: https://www.jetbrains.com/community/education/#students).
  - Visual Studio.
  - Visual Studio Code (dan moet je nog wel een paar extensie installeren).

In appsettings.json wordt de connectionstring aangemaakt die gebruikt wordt voor de verbinding met MySQL.
Maak een acoount aan met de volgende gegevens (of pas de connectiestring aan voor een bestaand account):
-username: root
-password: Test@1234!

```
CREATE USER 'root'@'localhost' IDENTIFIED BY 'Test@1234!';
GRANT ALL PRIVILEGES ON *.* TO 'root'@'localhost';
FLUSH PRIVILEGES;

-- Aanmaken van database is niet noodzakelijk (opstarten van webserver doet dit ook al, zie Startup.cs --> Configure --> db.Database.EnsureCreated();)
CREATE DATABASE IF NOT EXISTS SetGame;
```

## Opstarten

Als het goed is moet je nu op de play button kunnen drukken in je favoriete IDE (Rider).
Het kan even duren maar als het goed is moet je een webpagina te zien krijgen waarin Swagger alle API-methoden documenteert. 

