CREATE USER 'root'@'localhost' IDENTIFIED BY 'Test@1234!';
GRANT ALL PRIVILEGES ON *.* TO 'root'@'localhost';
FLUSH PRIVILEGES;

-- Aanmaken van database is niet noodzakelijk (opstarten van webserver doet dit ook al, zie Startup.cs --> Configure --> db.Database.EnsureCreated();)
CREATE DATABASE IF NOT EXISTS SetGame;