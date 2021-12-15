CREATE TABLE League (
    leagueID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    name varchar(100) NOT NULL,
    category int NOT NULL
);

CREATE TABLE Pitch (
    pitchID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    capacity int NOT NULL,
    name varchar(100) NOT NULL
);

CREATE TABLE Team (
    teamID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    leagueID int NOT NULL FOREIGN KEY REFERENCES League(leagueID),
    name  varchar(100) NOT NULL,
    rank int NOT NULL,
    covid int NOT NULL,
    quarantinedFrom date,
    INDEX teamLeagueID(leagueID)
);

CREATE TABLE TeamMatch (
    teamMatchID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    firstTeamID int NOT NULL FOREIGN KEY REFERENCES Team(teamID),
    secondTeamID  int NOT NULL FOREIGN KEY REFERENCES Team(teamID),
    pitchID int NOT NULL  FOREIGN KEY REFERENCES Pitch(pitchID),
    firstTeamGoals int NOT NULL,
    secondTeamGoals int NOT NULL,
    postponed int NOT NULL,
    date date NOT NULL,
    INDEX teamMatchPitch(pitchID),
    INDEX firstTeamIndex(firstTeamID)
);

ALTER TABLE TeamMatch
  ADD CONSTRAINT teamsCannotBeTheSame CHECK (firstTeamID <> secondTeamID) ;

ALTER TABLE TeamMatch
  ADD CONSTRAINT uniqueDateCombination UNIQUE(firstTeamID, date);

ALTER TABLE TeamMatch
  ADD CONSTRAINT uniqueDateCombinationSecond UNIQUE(secondTeamID, date);

CREATE TABLE Ticket (
    ticketID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    teamMatchID int NOT NULL FOREIGN KEY REFERENCES TeamMatch(teamMatchID),
    firstName varchar(50) NOT NULL,
    lastName varchar(50) NOT NULL,
    price money NOT NULL,
    storno int NOT NULL,
    email varchar(100) NOT NULL,
    INDEX ticketTeamMatch(teamMatchID)
);

CREATE TABLE Player (
    playerID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    teamID int NOT NULL FOREIGN KEY REFERENCES Team(teamID),
    email varchar(100) NOT NULL,
    dateOfBirth date NOT NULL,
    firstName varchar(50) NOT NULL,
    lastName varchar(50) NOT NULL,
    stick char(1),
    covid int NOT NULL,
    INDEX playerTeamID(teamID)
);

CREATE TABLE Statistic (
    statisticsID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    playerID int NOT NULL FOREIGN KEY REFERENCES Player(playerID),
    teamID int NOT NULL FOREIGN KEY REFERENCES Team(teamID),
    goals int NOT NULL,
    assists int NOT NULL,
    INDEX statisticPlayerID(playerID),
    INDEX statisticTeamID(teamID)
);

CREATE TABLE PlayerTransferHistory (
    playerTransferID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    oldTeamID int NOT NULL FOREIGN KEY REFERENCES Team(teamID),
    newTeamID int NOT NULL FOREIGN KEY REFERENCES Team(teamID),
    playerID int NOT NULL FOREIGN KEY REFERENCES Player(playerID),
    date date NOT NULL,
    INDEX playerTHoldTeamID(oldTeamID),
    INDEX playerTHnewTeamID(newTeamID),
    INDEX playerTHplayerID(playerID),
);

ALTER TABLE PlayerTransferHistory
  ADD CONSTRAINT teamsCannotBeTheSamePTH CHECK (newTeamID <> oldTeamID);

DECLARE @PageNumber AS INT, @RowspPage AS INT
SET @PageNumber = 30000
SET @RowspPage = 20
SELECT *
FROM TeamMatch
ORDER BY teamMatchID
OFFSET ((@PageNumber - 1) * @RowspPage) ROWS
FETCH NEXT @RowspPage ROWS ONLY
GO

DECLARE @PageNumber AS INT, @RowspPage AS INT
SET @PageNumber = 2500
SET @RowspPage = 20
SELECT *
FROM Statistic
ORDER BY statisticsID
OFFSET ((@PageNumber - 1) * @RowspPage) ROWS
FETCH NEXT @RowspPage ROWS ONLY
GO

DECLARE @PageNumber AS INT, @RowspPage AS INT
SET @PageNumber = 2500
SET @RowspPage = 20
SELECT *
FROM PlayerTransferHistory
ORDER BY playerTransferID
OFFSET ((@PageNumber - 1) * @RowspPage) ROWS
FETCH NEXT @RowspPage ROWS ONLY
GO

DECLARE @PageNumber AS INT, @RowspPage AS INT
SET @PageNumber = 5000
SET @RowspPage = 20
SELECT *
FROM Player
ORDER BY playerID
OFFSET ((@PageNumber - 1) * @RowspPage) ROWS
FETCH NEXT @RowspPage ROWS ONLY
GO

DECLARE @PageNumber AS INT, @RowspPage AS INT
SET @PageNumber = 250
SET @RowspPage = 20
SELECT *
FROM Team
ORDER BY teamID
OFFSET ((@PageNumber - 1) * @RowspPage) ROWS
FETCH NEXT @RowspPage ROWS ONLY
GO

DECLARE @PageNumber AS INT, @RowspPage AS INT
SET @PageNumber = 25
SET @RowspPage = 20
SELECT *
FROM League
ORDER BY leagueID
OFFSET ((@PageNumber - 1) * @RowspPage) ROWS
FETCH NEXT @RowspPage ROWS ONLY
GO

DECLARE @PageNumber AS INT, @RowspPage AS INT
SET @PageNumber = 2500
SET @RowspPage = 20
SELECT *
FROM Pitch
ORDER BY pitchID
OFFSET ((@PageNumber - 1) * @RowspPage) ROWS
FETCH NEXT @RowspPage ROWS ONLY
GO

DECLARE @PageNumber AS INT, @RowspPage AS INT
SET @PageNumber = 75000
SET @RowspPage = 20
SELECT *
FROM Ticket
ORDER BY ticketID
OFFSET ((@PageNumber - 1) * @RowspPage) ROWS
FETCH NEXT @RowspPage ROWS ONLY
GO