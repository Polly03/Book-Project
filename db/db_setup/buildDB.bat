@echo off

rem === Nastavení databaze a uzivatele ===
set DB_NAME=BOOK_DB_TEST.FDB
set DB_USER=SYSDBA
set DB_PASS=masterkey
set DB_PATH=%~dp0%DB_NAME%
set ISQL_PATH="%~dp0isql.exe"

rem === Pouzij isql.exe ze stejne slozky ===

rem === Overeni, ze isql.exe existuje ===
if not exist "%ISQL_PATH%" (
    echo [CHYBA] Nenalezen isql.exe ve slozce: %~dp0
    pause
    exit /b
)

rem === Pokud databaze uz existuje, nic nedelame ===
if exist "%DB_PATH%" (
    echo [INFO] Databaze "%DB_NAME%" uz existuje, nic se nebude delat.
    pause
    exit /b
)

echo [INFO] Databaze "%DB_NAME%" neexistuje, vytvarim...

rem === Vytvoreni databaze ===
%ISQL_PATH% -user %DB_USER% -password %DB_PASS% -i "%~dp0create_db.sql"
if errorlevel 1 (
    echo [CHYBA] Vytvoreni databaze selhalo.
    pause
    exit /b
)

echo [INFO] Databaze byla vytvorena. Spoustim dalsi SQL skripty...

rem === Spusteni dalsich SQL souboru pod sebou ===
"%ISQL_PATH%" -q "%DB_PATH%" -user %DB_USER% -password %DB_PASS% -i "%~dp0\create_tables.sql"

echo [HOTOVO] Databaze vytvorena a vsechny skripty byly spusteny.
pause