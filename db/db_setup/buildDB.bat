@echo off

rem setting of attributes
set DB_NAME=BOOK_DB.FDB
set DB_USER=SYSDBA
set DB_PASS=masterkey

set ISQL_PATH="%~dp0src\isql.exe"

pushd %~dp0
set PATH=%CD%
popd

SET "PFolder="
FOR %%I IN ("%PATH%\..") DO (
    SET "PFolder=%%~fI"
)
set "DB_PATH=%PFolder%\BOOK_DB.FDB"


rem If Db exist, then return

if exist "%PFolder%\BOOK_DB.FDB" (
    echo DB ALREADY EXISTS
   rem del "%PFolder%\BOOK_DB.FDB"
   rem echo DB DELETED
   exit /b
)

echo CREATING DB

rem creating Db
%ISQL_PATH% -user %DB_USER% -password %DB_PASS% -i "%PATH%\create_db.sql"


echo DB CREATED

rem SQL Files
"%ISQL_PATH%" "%DB_PATH%" -user %DB_USER% -password %DB_PASS% -i "%~dp0create_tables.sql"

"%ISQL_PATH%" "%DB_PATH%" -user %DB_USER% -password %DB_PASS% -i "%~dp0insert_countries.sql"
"%ISQL_PATH%" "%DB_PATH%" -user %DB_USER% -password %DB_PASS% -i "%~dp0insert_genres_publishers.sql"
"%ISQL_PATH%" "%DB_PATH%" -user %DB_USER% -password %DB_PASS% -i "%~dp0insert_language.sql"

echo ADDING PROCEDURES
"%ISQL_PATH%" "%DB_PATH%" -user %DB_USER% -password %DB_PASS% -i "%~dp0Select_procedures.sql"
"%ISQL_PATH%" "%DB_PATH%" -user %DB_USER% -password %DB_PASS% -i "%~dp0Insert_procedures.sql"
"%ISQL_PATH%" "%DB_PATH%" -user %DB_USER% -password %DB_PASS% -i "%~dp0Delete_procedures.sql"
"%ISQL_PATH%" "%DB_PATH%" -user %DB_USER% -password %DB_PASS% -i "%~dp0Other_procedures.sql"
echo PROCEDURES ADDED

echo QUERIES COMPLETED