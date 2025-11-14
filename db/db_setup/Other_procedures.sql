SET TERM ^ ;

-- procedure for ordering books by 'argument' in 'way'

create or alter procedure ORDER_BOOKS (
    WAY varchar(64),
    ARGUMENT varchar(64))
returns (
    GENRE varchar(64),
    FULLNAME varchar(64),
    NAME varchar(64),
    PHOTO blob sub_type 0 segment size 80)
as
declare variable SQL_QUERY varchar(8000);
begin
    sql_query = 
        'SELECT Books.Photo, Books.Name, (Authors.Name || '' '' || Authors.Surname) as Fullname, ' ||
        'Genres.Name ' ||
        'FROM Books ' ||
        'JOIN Genres ON Books.GenreID = Genres.ID ' ||
        'JOIN Authors ON Books.AuthorID = Authors.ID ' ||
        'JOIN Publishers ON Books.PublishingHouseID = Publishers.ID ' ||
        'JOIN Languages ON Books.LanguageID = Languages.ID ' ||
        'ORDER BY ' || argument || ' ' || way;

    FOR EXECUTE STATEMENT sql_query INTO :Photo, :Name, :Fullname, :Genre
    DO
    SUSPEND;
end;
^
SET TERM ; ^

SET TERM ^ ;

-- procedure for ordering books by 'argument' in 'way'

create or alter procedure ORDER_AUTHORS (
    WAY varchar(64),
    ARGUMENT varchar(64))
returns (
    Fullname varchar(128),
    DateOfBirth date,
    Country varchar(64)
    )
as
declare variable SQL_QUERY varchar(8000);
begin
    sql_query = 
        'SELECT (Authors.Name || '' '' || Authors.Surname) as Fullname, Authors.DateOfBirth, Countries.Name ' ||
        'FROM Authors ' ||
        'JOIN Countries ON Authors.CountryId = Countries.ID ' ||
        'ORDER BY ' || argument || ' ' || way;

    FOR EXECUTE STATEMENT sql_query INTO :Fullname, :DateOfBirth, :Country
    DO
    SUSPEND;
end;
^
SET TERM ; ^

SET TERM ^ ;

-- procedure for updating data for book by id 
create or alter procedure UPDATE_BOOK (
    BOOK_ID integer,
    PHOTO blob sub_type 0 segment size 80,
    TYPE_OF_BOOK varchar(64),
    LENGTH_OF_BOOK smallint,
    EAN varchar(13),
    ISBN varchar(13),
    PUBLISHER varchar(64),
    LANGUAGE varchar(64),
    RATING blob sub_type 1 segment size 80,
    DESCRIPTION blob sub_type 1 segment size 80,
    GENRE varchar(64),
    BOOK_NAME varchar(64),
    AUTHOR_NAME varchar(64))
as
declare variable TYPE_ID integer;
declare variable PUBLISH_ID integer;
declare variable LANGUAGE_ID integer;
declare variable GENRE_ID integer;
declare variable AUTHOR_ID integer;
BEGIN
    SELECT ID FROM AUTHORS WHERE NAME || ' ' || SURNAME = :AUTHOR_NAME
    INTO :AUTHOR_ID;

    SELECT ID FROM GENRES WHERE NAME = :GENRE
    INTO :GENRE_ID;

    SELECT ID FROM LANGUAGES WHERE NAME = :LANGUAGE
    INTO :LANGUAGE_ID;

    SELECT ID FROM PUBLISHERS WHERE NAME = :PUBLISHER
    INTO :PUBLISH_ID;

    SELECT ID FROM BOOKTYPES WHERE NAME = :TYPE_OF_BOOK
    INTO :TYPE_ID;
    
      UPDATE BOOKS
    SET
        Photo = :PHOTO,
        Length = :LENGTH_OF_BOOK,
        Ean = :EAN,
        Isbn = :ISBN,
        PublishingHouseId = :PUBLISH_ID,
        LanguageId = :LANGUAGE_ID,
        Rating = :RATING,
        Description = :DESCRIPTION,
        GenreId = :GENRE_ID,
        Name = :BOOK_NAME,
        AuthorId = :AUTHOR_ID
        WHERE Id = :BOOK_ID;

end;
^
SET TERM ; ^

SET TERM ^ ;
-- updating author based on its name

CREATE OR ALTER PROCEDURE UPDATE_AUTHOR (
    AUTHOR_ID INTEGER,
    NAME VARCHAR(64),
    SURNAME VARCHAR(64),
    DATE_OF_BIRTH DATE,
    COUNTRY_NAME VARCHAR(64),
    ABOUT_AUTHOR BLOB SUB_TYPE TEXT
)
AS
DECLARE VARIABLE COUNTRY_ID INTEGER;
BEGIN

    SELECT ID FROM COUNTRIES WHERE NAME = :COUNTRY_NAME
    INTO :COUNTRY_ID;

    UPDATE AUTHORS
    SET
        NAME = :NAME,
        SURNAME = :SURNAME,
        DATEOFBIRTH = :DATE_OF_BIRTH,
        COUNTRYID = :COUNTRY_ID,
        ABOUTAUTHOR = :ABOUT_AUTHOR
    WHERE ID = :AUTHOR_ID;
END
^

SET TERM ; ^



COMMIT;

