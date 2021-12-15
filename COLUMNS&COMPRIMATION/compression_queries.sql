/***** VHODNÉ DOTAZY *******/
SELECT count(statisticsID) FROM Statistic WHERE playerID = 2 and teamID = 22;
SELECT count(statisticsID) FROM StatisticColumn WHERE playerID = 2 and teamID = 22;

SELECT max(rank) FROM Team WHERE leagueID = 1818;
SELECT max(rank) FROM TeamColumn WHERE leagueID = 1818;

/***** NEVHODNÉ DOTAZY *****/
SELECT * FROM Team
SELECT * FROM TeamColumn

SELECT s.goals, s.assists ,t.name, t.rank, t.leagueID
FROM Statistic s
    JOIN Team t on s.teamID = t.teamID

SELECT s.goals, s.assists ,t.name, t.rank, t.leagueID
FROM StatisticColumn s
    JOIN TeamColumn t on s.teamID = t.teamID