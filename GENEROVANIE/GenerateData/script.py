import pyodbc
import random
import csv
import date

conn = pyodbc.connect('Driver={SQL Server};'
                      'Server=dbsys.cs.vsb.cz\STUDENT;'
                      'Database=MAR0702;'
                      'UID=MAR0702;'
                      'PWD=;')

cursorTeams = conn.cursor()
cursorTeams.execute('select * from Team')

dataTeams = cursorTeams.fetchall()
dictionary = {}
pitches = {}
date = datetime.datetime.strptime('2021-01-09','%Y-%d-%m')

dateInStrptime = date
date = date.strftime('%Y-%d-%m')

for d in dataTeams:
   dictionary[d.teamID] = 0

for i in range(1, 100010):
    pitches[i] = i

teamOne = {}
teamTwo = {}
datesDict = {date : []}
dateFinal = {}
firstGoals = {}
secondGoals = {}
postponed = {}

for i in range(1, 1200000):
    wasChange = True
    firstTeamID = random.choice(list(dictionary.keys()))
    secondTeamID = random.choice(list(dictionary.keys()))

    while firstTeamID == secondTeamID:
        secondTeamID = random.choice(list(dictionary.keys()))

    while wasChange:
        wasChange = False

        for d in datesDict[date]:
            while secondTeamID == d[0] or secondTeamID == d[1]:
                wasChange = True
                secondTeamID = random.choice(list(dictionary.keys()))

                while firstTeamID == secondTeamID:
                    secondTeamID = random.choice(list(dictionary.keys()))
                
            while firstTeamID == d[0] or firstTeamID == d[1]:
                wasChange = True
                firstTeamID = random.choice(list(dictionary.keys()))

                while firstTeamID == secondTeamID:
                    secondTeamID = random.choice(list(dictionary.keys()))
        
            if wasChange:
                break

    teamOne[i] = firstTeamID
    teamTwo[i] = secondTeamID
    dateFinal[i] = date
    firstGoals[i] = random.randint(0, 20)
    secondGoals[i] = random.randint(0, 20)
    postponed[i] = random.randint(0, 1)

    dictionary[firstTeamID] += 1
    dictionary[secondTeamID] += 1

    if dictionary[firstTeamID] == 250:
        del dictionary[firstTeamID]

    if dictionary[secondTeamID] == 250:
        del dictionary[secondTeamID]
    
    datesDict[date].append([firstTeamID, secondTeamID])

    if i % 1000 == 0:
        dateInStrptime += datetime.timedelta(days=1)
        date = dateInStrptime.strftime('%Y-%d-%m')
        datesDict = {date : []}

    print(i)

countPitch = 1

with open('everything_new_1.csv', 'w', newline='') as csv_file:
    writer = csv.writer(csv_file)
    writer.writerow(["primary", "first", "second", "pitch", "goals", "secondgoals", "postponed", "date"])

    for i in range(1, 1200000):
        writer.writerow([i + 1, teamOne[i], teamTwo[i], pitches[countPitch], firstGoals[i], secondGoals[i], postponed[i], dateFinal[i]])
        countPitch += 1

        if countPitch == 100010:
            countPitch = 1