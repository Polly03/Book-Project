-- Procedures for inserting data

SET TERM ^ ;

-- Inserting Author
create or alter procedure INSERT_AUTHOR (
    ABOUTAUTHOR blob sub_type 1 segment size 80,
    COUNTRY varchar(64),
    DATEOFBIRTH date,
    SURNAME varchar(64),
    NAME varchar(64))
as
declare variable ABOUTBLOB blob sub_type 1 segment size 80;
declare variable COUNTRY_ID integer;
begin
  SELECT ID FROM COUNTRIES WHERE NAME = :country
  into :country_id;

  insert into Authors (name, surname, DateOfBirth, countryid, ABOUTAUTHOR)
  values (:name, :surname, :dateofbirth, :country_id, :aboutauthor);
end;
^
SET TERM ; ^

SET TERM ^ ;

-- Inserting database with name of author too
create or alter procedure INSERT_BOOK_OLD_AUTHOR (
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
declare variable BOOK_ID integer;
declare variable TYPE_ID integer;
declare variable PUBLISH_ID integer;
declare variable LANGUAGE_ID integer;
declare variable GENRE_ID integer;
declare variable AUTHOR_ID integer;
BEGIN
    SELECT ID FROM AUTHORS WHERE NAME = :AUTHOR_NAME
    INTO :AUTHOR_ID;

    SELECT ID FROM GENRES WHERE NAME = :GENRE
    INTO :GENRE_ID;

    SELECT ID FROM LANGUAGES WHERE NAME = :LANGUAGE
    INTO :LANGUAGE_ID;

    SELECT ID FROM PUBLISHERS WHERE NAME = :PUBLISHER
    INTO :PUBLISH_ID;

    SELECT ID FROM BOOKTYPES WHERE NAME = :TYPE_OF_BOOK
    INTO :TYPE_ID;

    INSERT INTO BOOKS (AUTHORID, NAME, GENREID, DESCRIPTION, RATING, LANGUAGEID, PUBLISHINGHOUSEID, ISBN, EAN, LENGTH, PHOTO)
    VALUES (:AUTHOR_ID, :BOOK_NAME, :GENRE_ID, :DESCRIPTION, :RATING, :LANGUAGE_ID, :PUBLISH_ID, :ISBN, :EAN, :LENGTH_OF_BOOK, :PHOTO)
    Returning ID INTO :BOOK_ID;

    Insert into book_to_type(bookID, TypeID)
    values (:BOOK_ID, :TYPE_ID);

end;
^
SET TERM ; ^

COMMIT;