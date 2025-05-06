-- ========================================
-- DROP FOREIGN KEYS (in dependency order)
-- ========================================
IF OBJECT_ID('FK_Addresses_ZipCodes', 'F') IS NOT NULL
    ALTER TABLE Addresses DROP CONSTRAINT FK_Addresses_ZipCodes;

IF OBJECT_ID('FK_Events_Addresses', 'F') IS NOT NULL
    ALTER TABLE AllOurEvents DROP CONSTRAINT FK_Events_Addresses;

IF OBJECT_ID('FK_allEventSignups_Events', 'F') IS NOT NULL
    ALTER TABLE allEventSignups DROP CONSTRAINT FK_allEventSignups_Events;

IF OBJECT_ID('FK_allEventSignups_Users', 'F') IS NOT NULL
    ALTER TABLE allEventSignups DROP CONSTRAINT FK_allEventSignups_Users;

-- ========================================
-- DROP TABLES (reverse dependency order)
-- ========================================
IF OBJECT_ID('allEventSignups', 'U') IS NOT NULL DROP TABLE allEventSignups;
DROP TABLE AllOurEvents;
IF OBJECT_ID('Addresses', 'U') IS NOT NULL DROP TABLE Addresses;
IF OBJECT_ID('Users', 'U') IS NOT NULL DROP TABLE Users;
IF OBJECT_ID('OpenHours', 'U') IS NOT NULL DROP TABLE OpenHours;
IF OBJECT_ID('ZipCodes', 'U') IS NOT NULL DROP TABLE ZipCodes;

-- ========================================
-- CREATE TABLES
-- ========================================

-- ZIPCODES
CREATE TABLE ZipCodes (
    postalcode CHAR(4) PRIMARY KEY,
    city NVARCHAR(50) NOT NULL,
    state NVARCHAR(50) NOT NULL,
    country NVARCHAR(50) NOT NULL
);

-- ADDRESSES
CREATE TABLE Addresses (
    address_id INT IDENTITY(1,1) PRIMARY KEY,
    street NVARCHAR(100) NOT NULL,
    address_postalcode CHAR(4) NOT NULL
);

-- EVENTS
CREATE TABLE AllOurEvents (
    event_id INT IDENTITY(1,1) PRIMARY KEY,
    event_name NVARCHAR(50) NOT NULL,
    event_description NVARCHAR(MAX),
    address_id INT NOT NULL,
    event_date DATETIME NOT NULL,
    event_maxAttendance INT,
    event_ticketPrice DECIMAL(6,2),
    event_imageName NVARCHAR(50)
);

-- USERS
CREATE TABLE Users (
    user_id INT IDENTITY(1,1) PRIMARY KEY,
    user_name NVARCHAR(50) NOT NULL,
    user_email NVARCHAR(50) NOT NULL UNIQUE,
    user_password NVARCHAR(50) NOT NULL,
    user_isAdmin BIT NOT NULL,
    user_newsletter BIT NOT NULL
);

-- ALLEVENTSIGNUPS
CREATE TABLE allEventSignups (
    event_id INT NOT NULL,
    user_id INT NOT NULL,
    PRIMARY KEY (event_id, user_id)
);

-- OPENHOURS
CREATE TABLE OpenHours (
    OpenHours_Id INT IDENTITY(1,1) PRIMARY KEY,
    openHours_date DATETIME NOT NULL,
    openHours_start INT NOT NULL,
    openHours_end INT NOT NULL
);

-- ========================================
-- ADD FOREIGN KEY CONSTRAINTS
-- ========================================

-- Addresses → ZipCodes
ALTER TABLE Addresses ADD CONSTRAINT FK_Addresses_ZipCodes
    FOREIGN KEY (address_postalcode) REFERENCES ZipCodes(postalcode);

-- Events → Addresses
ALTER TABLE AllOurEvents ADD CONSTRAINT FK_Events_Addresses
    FOREIGN KEY (address_id) REFERENCES Addresses(address_id);

-- allEventSignups → Events
ALTER TABLE allEventSignups ADD CONSTRAINT FK_allEventSignups_Events
    FOREIGN KEY (event_id) REFERENCES AllOurEvents(event_id);

-- allEventSignups → Users
ALTER TABLE allEventSignups ADD CONSTRAINT FK_allEventSignups_Users
    FOREIGN KEY (user_id) REFERENCES Users(user_id);

ALTER TABLE allEventSignups ADD signup_date DATETIME NOT NULL DEFAULT GETDATE();


-- Insert ZipCodes
INSERT INTO ZipCodes (postalcode, city, state, country) VALUES
('1000', 'Copenhagen', 'Hovedstaden', 'Denmark'),
('2000', 'Frederiksberg', 'Hovedstaden', 'Denmark'),
('3000', 'Helsingør', 'Hovedstaden', 'Denmark');

-- Insert Addresses
INSERT INTO Addresses (street, address_postalcode) VALUES
('Main Street 1', '1000'),
('Central Avenue 5', '2000'),
('Harbor Road 12', '3000');

-- Insert Events
INSERT INTO AllOurEvents(event_name, event_description, address_id, event_date, event_maxAttendance, event_ticketPrice, event_imageName) VALUES
('Food Festival', 'Enjoy local cuisine and drinks.', 1, '2025-07-10 12:00:00', 500, 150.00, 'foodfest.jpg'),
('Tech Conference', 'Future of AI and robotics.', 2, '2025-09-15 09:00:00', 1000, 799.99, 'tech2025.png'),
('Music Gala', 'An evening of classical and jazz.', 3, '2025-11-20 19:30:00', 300, 350.00, 'musicgala.jpeg');

-- Insert Users
INSERT INTO Users (user_name, user_email, user_password, user_isAdmin, user_newsletter) VALUES
('Alice Jensen', 'alice@example.com', 'Passw0rd!', 0, 1),
('Bob Sørensen', 'bob@example.com', 'Secure123!', 1, 1),
('Charlie Hansen', 'charlie@example.com', 'qwerty789', 0, 0);

-- Insert OpenHours
INSERT INTO OpenHours (openHours_date, openHours_start, openHours_end) VALUES
('2025-07-10', 10, 22),
('2025-09-15', 08, 18),
('2025-11-20', 17, 23);

-- Insert AllEventSignups
INSERT INTO allEventSignups (event_id, user_id, signup_date) VALUES
(1, 1, '2025-06-01'),
(1, 2, '2025-06-02'),
(2, 2, '2025-08-01'),
(3, 3, '2025-10-15');


