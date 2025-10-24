@echo off
echo starting
SET FB_PATH=C:\fb
SET DB_USER=SYSDBA
SET DB_PASSWORD=masterkey
SET DB_PATH=%CD%\MyDatabase.FDB

echo creating database
"%FB_PATH%\isql.exe" -user %DB_USER% -password %DB_PASSWORD% -q -i create_db.sql

echo tables and inputs
"%FB_PATH%\isql.exe" "%DB_PATH%" -user %DB_USER% -password %DB_PASSWORD% -i create_tables.sql
"%FB_PATH%\isql.exe" "%DB_PATH%" -user %DB_USER% -password %DB_PASSWORD% -i insert_countries.sql
"%FB_PATH%\isql.exe" "%DB_PATH%" -user %DB_USER% -password %DB_PASSWORD% -i insert_language.sql
"%FB_PATH%\isql.exe" "%DB_PATH%" -user %DB_USER% -password %DB_PASSWORD% -i insert_genres_publishers.sql

echo TESTS:
"%FB_PATH%\isql.exe" "%DB_PATH%" -user %DB_USER% -password %DB_PASSWORD% -i test.sql
"%FB_PATH%\isql.exe" "%DB_PATH%" -user %DB_USER% -password %DB_PASSWORD% -i procedures.sql
"%FB_PATH%\isql.exe" "%DB_PATH%" -user %DB_USER% -password %DB_PASSWORD% -i procedures2.sql
echo zaznamy v tabulce authors a books jsou testovaci


echo finish
pause





