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

);

CREATE TABLE TeamMatch (
    teamMatchID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    firstTeamID int NOT NULL FOREIGN KEY REFERENCES Team(teamID),
    secondTeamID  int NOT NULL FOREIGN KEY REFERENCES Team(teamID),
    pitchID int NOT NULL  FOREIGN KEY REFERENCES Pitch(pitchID),
    firstTeamGoals int NOT NULL,
    secondTeamGoals int NOT NULL,
    postponed int NOT NULL,
    date date NULL
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
    email varchar(100) NOT NULL
);

CREATE TABLE Player (
    playerID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    teamID int NOT NULL FOREIGN KEY REFERENCES Team(teamID),
    email varchar(100) NOT NULL,
    dateOfBirth date NOT NULL,
    firstName varchar(50) NOT NULL,
    lastName varchar(50) NOT NULL,
    stick char(1),
    covid int NOT NULL
);

CREATE TABLE Statistic (
    statisticsID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    playerID int NOT NULL FOREIGN KEY REFERENCES Player(playerID),
    teamID int NOT NULL FOREIGN KEY REFERENCES Team(teamID),
    goals int NOT NULL,
    assists int NOT NULL
);

CREATE TABLE PlayerTransferHistory (
    playerTransferID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    oldTeamID int NOT NULL FOREIGN KEY REFERENCES Team(teamID),
    newTeamID int NOT NULL FOREIGN KEY REFERENCES Team(teamID),
    playerID int NOT NULL FOREIGN KEY REFERENCES Player(playerID),
    date date NOT NULL
);

ALTER TABLE PlayerTransferHistory
  ADD CONSTRAINT teamsCannotBeTheSamePTH CHECK (newTeamID <> oldTeamID) ;
