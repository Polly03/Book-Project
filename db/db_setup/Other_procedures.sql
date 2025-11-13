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


COMMIT;