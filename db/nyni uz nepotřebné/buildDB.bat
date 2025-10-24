@echo off
echo starting
SET FB_PATH=C:\fb
SET DB_USER=SYSDBA
SET DB_PASSWORD=masterkey
SET DB_PATH=%CD%\MyDatabase.FDB

echo creating database
"%FB_PATH%\isql.exe" -user %DB_USER% -password %DB_PASSWORD% -q -i new_database.sql
