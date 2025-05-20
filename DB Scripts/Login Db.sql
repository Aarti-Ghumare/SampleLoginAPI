CREATE TABLE Logins (
    Id INT PRIMARY KEY IDENTITY,
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    Password NVARCHAR(100)
);

INSERT INTO Logins (Email, Phone, Password)
VALUES 
('aarti@gmail.com', '1234567890', 'Pass@123'),
('tejal@gmail.com', '9876543210', 'Sai@456'),
('pranjal@gmail.com', '1234567890', 'Pranjal@123'),
('om@gmail.com', '9876543210', 'Om@456');

