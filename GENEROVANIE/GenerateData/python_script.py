import pyodbc
import csv

conn = pyodbc.connect('Driver={SQL Server};'
                      'Server=dbsys.cs.vsb.cz\STUDENT;'
                      'Database=MAR0702;'
                      'UID=MAR0702;'
                      'PWD=;')

cursorPlayers = conn.cursor()
cursorPlayers.execute('select * from Player where playerID <= 100000')

dataPlayers = cursorPlayers.fetchall()

with open('statistics.csv', 'w', newline='') as csv_file:
    writer = csv.writer(csv_file)
    writer.writerow(["playerID", "teamID"])

    for p in dataPlayers:
        writer.writerow([p.playerID, p.teamID])
