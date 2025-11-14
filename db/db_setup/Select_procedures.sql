

SET TERM ^ ;

-- selecting author based on his name

create or alter procedure SELECT_AUTHOR (
    NAME_INPUT varchar(64))
returns (
    SURNAME varchar(64),
    ABOUT_AUTHOR blob sub_type 1 segment size 80,
    COUNTRY varchar(64),
    DATE_OF_BIRTH varchar(64),
    NAME varchar(64))
as
begin
    FOR SELECT distinct Authors.Name, authors.surname, Authors.dateOfBirth, Countries.Name, authors.aboutauthor
        from Authors
        Join Countries on Authors.countryId = Countries.Id
        where Authors.Name = :NAME_INPUT
    
    INTO :Name, :surname, :DATE_OF_BIRTH, :Country, :About_Author DO
        SUSPEND;
end;
^
SET TERM ; ^

SET TERM ^ ;

-- selecting authors which are in filtes selected

create or alter procedure SELECT_AUTHORS_WITH_FILTERS (
    COUNTRIES_LIST varchar(800))
returns (
    SURNAME varchar(64),
    NAME varchar(64),
    DATEOFBIRTH date,
    COUNTRY varchar(64))
as
declare variable SQL_TEXT varchar(8000);
BEGIN
    sql_text = 'SELECT authors.name,
                        authors.surname,
                       Authors.dateOfBirth, 
                       Countries.Name
                FROM Authors
                JOIN Countries ON Authors.countryId = Countries.Id
                WHERE 1=1';

        IF (COUNTRIES_LIST IS NOT NULL AND TRIM(COUNTRIES_LIST) <> '') THEN
        sql_text = sql_text || ' AND Countries.Name IN (' || COUNTRIES_LIST || ')';

    FOR EXECUTE STATEMENT sql_text INTO :Name,:surname, :dateOfBirth, :Country DO
    SUSPEND;
end;
^
SET TERM ; ^

SET TERM ^ ;

-- selecting authors which have substring of 'search! in name or surname

create or alter procedure SELECT_AUTHOR_WITH_SEARCH (
    SEARCH varchar(64))
returns (
    SURNAME varchar(64),
    NAME varchar(64),
    DATEOFBIRTH date,
    COUNTRY varchar(64))
as
BEGIN
    FOR SELECT Authors.Name, Authors.surname, Authors.dateOfBirth, Countries.Name
        from Authors
        Join Countries on Authors.countryId = Countries.Id
        where Authors.Name CONTAINING :search OR Authors.Surname CONTAINING :search
    
    INTO :Name, :Surname, :dateOfBirth, :Country DO
        SUSPEND;
end;
^
SET TERM ; ^

SET TERM ^ ;

-- selecting book based on its name

create or alter procedure SELECT_BOOK (
    NAME_OF_BOOK varchar(64) character set UTF8)
returns (
    ID integer,
    LANGUAGE varchar(64),
    RATING blob sub_type 1 segment size 80,
    BOOK_TYPE varchar(64) character set UTF8,
    PUBLISHER varchar(64) character set UTF8,
    GENRE varchar(64) character set UTF8,
    AUTHOR varchar(128) character set UTF8,
    EAN varchar(13) character set UTF8,
    ISBN varchar(13) character set UTF8,
    PHOTO blob sub_type 0 segment size 80,
    LENGTH_OF_BOOK smallint,
    DESCRIPTION blob sub_type 1 segment size 80,
    NAME varchar(64) character set UTF8)
as
begin FOR
       SELECT Books.name, books.description, books.rating, books.LENGTH, books.photo, books.isbn, books.ean, languages.name,
       (authors.name || ' ' || Authors.Surname) as author_name, genres.name as genre_name, publishers.name as publisher_name, Booktypes.name as book_type, books.id as ID
       FROM Books
       JOIN authors on authors.id = books.authorid
       JOIN genres on genres.id = books.genreid
       JOIN languages on languages.id = books.languageid
       JOIN publishers on publishers.id = books.publishinghouseid
       JOIN book_to_type btt on btt.bookid = books.id
       JOIN booktypes on booktypes.id = btt.typeid
       WHERE Books.name = :name_of_book
       INTO :name, :description, :rating, :length_OF_BOOK, :photo, :isbn, :ean, :language, :author, :genre, :publisher, :book_type, :ID DO
  suspend;
end;
^
SET TERM ; ^

SET TERM ^ ;

-- selecting books which are in filters

create or alter procedure SELECT_BOOKS_WITH_FILTERS (
    AUTHOR_LIST varchar(400),
    GENRE_LIST varchar(400),
    LANGUAGE_LIST varchar(400),
    PUBLISHING_HOUSE_LIST varchar(400))
returns (
    PHOTO blob sub_type 0 segment size 80,
    NAME varchar(64),
    FULLNAME varchar(64),
    GENRE varchar(64))
as
declare variable SQL_TEXT varchar(8000);
BEGIN
    sql_text = '
        SELECT Books.Photo, Books.Name, 
        (Authors.Name || '' '' || Authors.Surname) as Fullname,
        Genres.Name
        FROM Books
        JOIN Genres ON Books.GenreID = Genres.ID
        JOIN Authors ON Books.AuthorID = Authors.ID
        JOIN Publishers ON Books.PublishingHouseID = Publishers.ID
        JOIN Languages ON Books.LanguageID = Languages.ID
        WHERE 1=1
    ';

    IF (AUTHOR_LIST IS NOT NULL AND TRIM(AUTHOR_LIST) <> '') THEN
        SQL_TEXT = SQL_TEXT || ' AND (Authors.Name || '' '' || Authors.Surname) IN (' || AUTHOR_LIST || ')';

    IF (GENRE_LIST IS NOT NULL AND TRIM(GENRE_LIST) <> '') THEN
        SQL_TEXT = SQL_TEXT || ' AND Genres.Name IN (' || GENRE_LIST || ')';

    IF (LANGUAGE_LIST IS NOT NULL AND TRIM(LANGUAGE_LIST) <> '') THEN
        SQL_TEXT = SQL_TEXT || ' AND Languages.Name IN (' || LANGUAGE_LIST || ')';

    IF (PUBLISHING_HOUSE_LIST IS NOT NULL AND TRIM(PUBLISHING_HOUSE_LIST) <> '') THEN
        SQL_TEXT = SQL_TEXT || ' AND Publishers.Name IN (' || PUBLISHING_HOUSE_LIST || ')';

    FOR EXECUTE STATEMENT sql_text INTO :Photo, :Name, :Fullname, :Genre DO
        SUSPEND;
end;
^
SET TERM ; ^


SET TERM ^ ;

-- Selecting book which have substring of search in name or author name

create or alter procedure SELECT_BOOK_WITH_SEARCH (
    SEARCH varchar(64))
returns (
    PHOTO blob sub_type 0 segment size 80,
    NAME varchar(64),
    FULLNAME varchar(64),
    GENRE varchar(64))
as
BEGIN
    FOR SELECT Books.Photo, Books.Name, (Authors.Name || ' '  || Authors.surname) AS Fullname, Genres.Name
        FROM Books
        JOIN Genres ON Books.GenreID = Genres.ID
        JOIN Authors On Books.AuthorID = Authors.ID
        JOIN Publishers ON Books.PublishingHouseID = Publishers.ID
        JOIN Languages ON Books.LanguageID = Languages.ID
        WHERE Authors.Name CONTAINING :search
        OR Books.Name CONTAINING :search
    
    INTO :Photo, :Name, :Fullname, :Genre DO
        SUSPEND;
end;
^
SET TERM ; ^


SET TERM ^ ;





-- selecting column "Name" from table by Name of table

create or alter procedure SELECT_NAME_BY_TABLE_NAME (
    TABLE_NAME varchar(64))
returns (
    Name varchar(128))
as
declare variable SQL_QUERY varchar(8000);
BEGIN
    if (TABLE_NAME = 'Authors') then
    BEGIN
        SQL_QUERY = 'SELECT (Authors.Name || '' ''  || Authors.surname) AS Name FROM ' || TABLE_NAME;
    END

    else
    BEGIN
    SQL_QUERY = 'SELECT Name FROM ' || TABLE_NAME;
    END

    FOR EXECUTE STATEMENT sql_query INTO :Name
    DO
    SUSPEND;
END;
    
^
SET TERM ; ^




COMMIT;