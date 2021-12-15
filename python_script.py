import pyodbc
import csv
import random
import datetime

conn = pyodbc.connect('Driver={SQL Server};'
                      'Server=dbsys.cs.vsb.cz\STUDENT;'
                      'Database=MAR0702;'
                      'UID=MAR0702;'
                      'PWD=tq71ge9681Iw9JRA;')

cursorTeams = conn.cursor()
cursorTeams.execute('select * from Team')

dataTeams = cursorTeams.fetchall()
dictionary = {}
date = datetime.datetime.strptime('01/09/2021','%d/%m/%Y').date()

for d in dataTeams:
   dictionary[d.teamID] = 0

teamOne = {}
teamTwo = {}
datesDict = {date.strftime('%d/%m/%Y') : []}
dateFinal = {}

for i in range(1, 100):
    wasChange = True
    firstTeamID = random.choice(list(dictionary.keys()))
    secondTeamID = random.choice(list(dictionary.keys()))

    while firstTeamID == secondTeamID:
        secondTeamID = random.choice(list(dictionary.keys()))

    while wasChange:
        wasChange = False

        for d in datesDict:
            while secondTeamID == d[0] or secondTeamID == d[1]:
                wasChange = True
                secondTeamID = random.choice(list(dictionary.keys()))
                
            while firstTeamID == d[0] or firstTeamID == d[1]:
                wasChange = True
                firstTeamID = random.choice(list(dictionary.keys()))
            
            if wasChange:
                break

    teamOne[i] = firstTeamID
    teamTwo[i] = secondTeamID
    dateFinal[i] = date

    dictionary[firstTeamID] += 1
    dictionary[secondTeamID] += 1

    if dictionary[firstTeamID] == 100:
        del dictionary[firstTeamID]

    if dictionary[secondTeamID] == 100:
        del dictionary[secondTeamID]
    
    datesDict[date.strftime('%d/%m/%Y')].append([firstTeamID, secondTeamID])

print(teamOne)
#with open('zapasy.csv', 'w', newline='') as csv_file:
#   writer = csv.writer(csv_file)
#   writer.writerow([firstTeamID, secondTeamID, 1, date])

    





