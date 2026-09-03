CREATE TABLE [User]
(
userId INT PRIMARY KEY IDENTITY(1,1),
userName NVARCHAR(100) NOT NULL,
emailAddress NVARCHAR(255) NOT NULL UNIQUE,
contactNumber NVARCHAR(20) NOT NULL,
passwordHash NVARCHAR(255) NOT NULL,
createdDate DATETIME DEFAULT GETDATE()
);


CREATE TABLE EventOrganiser (
    organiserID INT PRIMARY KEY,
    userID INT NOT NULL UNIQUE,
    FOREIGN KEY (userID) REFERENCES [User](userId) ON DELETE CASCADE
);


CREATE TABLE Participant (
participantID INT PRIMARY KEY,
userID INT NOT NULL UNIQUE,
FOREIGN KEY (userID) REFERENCES [User](userId) ON DELETE CASCADE
);

CREATE TABLE Event (
 eventID INT PRIMARY KEY IDENTITY(1,1),
 organiserID INT NOT NULL,
 eventName NVARCHAR(255) NOT NULL,
 description NVARCHAR(250),
location NVARCHAR(255) NOT NULL,
eventDate DATETIME NOT NULL,
distance DECIMAL(10, 2) NOT NULL CHECK (distance > 0),
eventType NVARCHAR(50) NOT NULL CHECK (eventType IN ('Walk', 'Run', 'Cycle')),
createdDate DATETIME DEFAULT GETDATE(),
FOREIGN KEY (organiserID) REFERENCES EventOrganiser(organiserID) ON DELETE CASCADE
);

CREATE TABLE EventCategories (
categoryID INT PRIMARY KEY IDENTITY(1,1),
eventID INT NOT NULL,
categoryName NVARCHAR(100) NOT NULL,
ageGroup NVARCHAR(50),
distanceType NVARCHAR(50),
createdDate DATETIME DEFAULT GETDATE(),
FOREIGN KEY (eventID) REFERENCES Event(eventID) ON DELETE CASCADE,
 CHECK (ageGroup IS NOT NULL OR distanceType IS NOT NULL)
);

CREATE TABLE Route (
    routeID INT PRIMARY KEY IDENTITY(1,1),
    eventID INT NOT NULL UNIQUE,
    routeDescription NVARCHAR(MAX),
    distance DECIMAL(10, 2) NOT NULL,
    difficulty NVARCHAR(50),
    mapURL NVARCHAR(MAX),
    createdDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (eventID) REFERENCES Event(eventID) ON DELETE CASCADE
);

CREATE TABLE EventEnrolment (
    enrolmentID INT PRIMARY KEY IDENTITY(1,1),
    participantID INT NOT NULL,
    eventID INT NOT NULL,
    categoryID INT NOT NULL,
    enrolmentDate DATETIME DEFAULT GETDATE(),
    status NVARCHAR(50) NOT NULL DEFAULT 'Confirmed' CHECK (status IN ('Confirmed', 'Withdrawn')),
    FOREIGN KEY (participantID) REFERENCES Participant(participantID) ,
    FOREIGN KEY (eventID) REFERENCES Event(eventID) ,
    FOREIGN KEY (categoryID) REFERENCES EventCategories(categoryID) ON DELETE CASCADE,
    UNIQUE (participantID, eventID, categoryID)
);

CREATE TABLE Results (
    resultID INT PRIMARY KEY IDENTITY(1,1),
    enrolmentID INT NOT NULL UNIQUE,
    finishTime TIME NOT NULL,
    position INT NOT NULL CHECK (position > 0),
    dateOfResults DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (enrolmentID) REFERENCES EventEnrolment(enrolmentID) ON DELETE CASCADE
);

INSERT INTO [User] (userName, emailAddress, contactNumber, passwordHash)
VALUES 
    ('John Mthembu', 'john.mthembu@email.com', '0721234567', 'hashed_password_1'),
    ('Sarah Pieterse', 'sarah.pieterse@email.com', '0729876543', 'hashed_password_2'),
    ('David Nkosi', 'david.nkosi@email.com', '0715555555', 'hashed_password_3'),
    ('Emma van der Merwe', 'emma.vdm@email.com', '0716666666', 'hashed_password_4');

    -- Inserting into Event Organisers
INSERT INTO EventOrganiser (organiserID, userID)
VALUES 
    (1, 1),
    (2, 2);

-- Insert Participants
INSERT INTO Participant (participantID, userID)
VALUES 
    (3, 3),
    (4, 4);

-- Insert Events (3 South African road events)
INSERT INTO Event (organiserID, eventName, description, location, eventDate, distance, eventType)
VALUES 
    (1, 'Comrades Marathon', 'The legendary 87km ultra marathon', 'Pietermaritzburg to Durban', '2026-05-24', 87, 'Run'),
    (1, 'Cape Town 10km', 'Popular urban 10km race through the city', 'Cape Town CBD', '2026-06-15', 10, 'Run'),
    (2, 'Soweto Marathon', 'Annual marathon supporting local communities', 'Soweto', '2026-08-10', 42, 'Run');

-- Insert Categories
INSERT INTO EventCategories (eventID, categoryName, ageGroup, distanceType)
VALUES 
    (1, 'Ultra Marathon', 'Senior', '87km'),
    (1, 'Half Marathon', '40-50', '42km'),
    (2, '10km Race', 'Under 30', '10km'),
    (2, '5km Fun Run', 'Open', '5km'),
    (3, 'Full Marathon', 'All ages', '42km'),
    (3, 'Half Marathon', 'All ages', '21km');

-- Insert Routes
INSERT INTO Route (eventID, routeDescription, distance, difficulty, mapURL)
VALUES 
    (1, 'Start at Town Hall Pietermaritzburg, descend through Karkloof Hills, finish at Moses Mabhida Stadium Durban', 87, 'Hard', 'https://maps.example.com/comrades'),
    (2, 'Start at V&A Waterfront, loop through Cape Town CBD, finish at Company Gardens', 10, 'Easy', 'https://maps.example.com/capetown10km'),
    (3, 'Start at Orlando Stadium, loop through Soweto townships, finish at Kliptown', 42, 'Moderate', 'https://maps.example.com/soweto');

-- Insert Event Enrolments
INSERT INTO EventEnrolment (participantID, eventID, categoryID, status)
VALUES 
    (3, 1, 1, 'Confirmed'),
    (3, 2, 3, 'Confirmed'),
    (4, 2, 4, 'Confirmed'),
    (4, 3, 5, 'Confirmed');

-- Insert Results (Some participants finished, some didn't)
INSERT INTO Results (enrolmentID, finishTime, position)
VALUES 
    (1, '05:47:30', 1),
    (2, '00:47:15', 5),
    (4, '04:23:45', 12);
