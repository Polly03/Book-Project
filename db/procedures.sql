-- procedure for selecting books with filter choose by the user

SET TERM ^ ;

CREATE Procedure select_books_with_filters (
    Author_list VARCHAR(400),
    Genre_list VARCHAR(400),
    Language_list VARCHAR(400),
    Publishing_house_list VARCHAR(400)
)
RETURNS (
    Photo BLOB,
    Name VARCHAR(64),
    Author VARCHAR(64),
    Genre VARCHAR(64)
)
AS
DECLARE VARIABLE sql_text VARCHAR(8000);
BEGIN
    sql_text = '
        SELECT Books.Photo, Books.Name, Authors.Name, Genres.Name
        FROM Books
        JOIN Genres ON Books.GenreID = Genres.ID
        JOIN Authors ON Books.AuthorID = Authors.ID
        JOIN Publishers ON Books.PublishingHouseID = Publishers.ID
        JOIN Languages ON Books.LanguageID = Languages.ID
        WHERE 1=1
    ';

    IF (Author_list IS NOT NULL) THEN
        sql_text = sql_text || ' AND Authors.Name IN (' || Author_list || ')';

    IF (Genre_list IS NOT NULL) THEN
        sql_text = sql_text || ' AND Genres.Name IN (' || Genre_list || ')';

    IF (Language_list IS NOT NULL) THEN
        sql_text = sql_text || ' AND Languages.Name IN (' || Language_list || ')';

    IF (Publishing_house_list IS NOT NULL) THEN
        sql_text = sql_text || ' AND Publishers.Name IN (' || Publishing_house_list || ')';

    FOR EXECUTE STATEMENT sql_text INTO :Photo, :Name, :Author, :Genre DO
        SUSPEND;
END;
^

SET TERM ; ^

-- procedure for selecting authors with filter choose by the user

SET TERM ^ ;

CREATE PROCEDURE select_authors_with_filters(
    Name_list VARCHAR(800),
    Countries_list VARCHAR(400)
)
RETURNS(
    Name Varchar(64),
    dateOfBirth date,
    Country Varchar(64)
)
AS
DECLARE VARIABLE sql_text Varchar(8000);
BEGIN
    sql_text = '
        SELECT Authors.Name, Authors.dateOfBirth, Countries.Name
        from Authors
        Join Countries on Authors.countryId = Countries.Id
        where 1=1
    ';

    IF (Name_List IS NOT NULL) THEN
        sql_text = sql_text || ' AND Authors.Name IN (' || Name_list ||')';
    IF (Countries_list IS NOT NULL) THEN
        sql_text = sql_text || ' AND Countries.Name IN (' || Countries_list ||')';

    FOR EXECUTE STATEMENT sql_text INTO :Name, :dateOfBirth, :Country DO
    SUSPEND;
END;
^

SET TERM ; ^

-- procedure for selecting books which have search bar text in name or author name

SET TERM ^ ;

CREATE PROCEDURE select_book_with_search(
    search varchar(64)
)
RETURNS(
    Photo BLOB,
    Name VARCHAR(64),
    Author VARCHAR(64),
    Genre VARCHAR(64)
)
AS
BEGIN
    FOR SELECT Books.Photo, Books.Name, Authors.Name, Genres.Name
        FROM Books
        JOIN Genres ON Books.GenreID = Genres.ID
        JOIN Authors ON Books.AuthorID = Authors.ID
        JOIN Publishers ON Books.PublishingHouseID = Publishers.ID
        JOIN Languages ON Books.LanguageID = Languages.ID
        WHERE Authors.Name CONTAINING :search
        OR Books.Name CONTAINING :search
    
    INTO :Photo, :Name, :Author, :Genre DO
        SUSPEND;
END;
^
    
SET TERM ; ^

-- procedure for selecting author which have search bar text in author name

SET TERM ^ ;

CREATE PROCEDURE select_author_with_search(
    search varchar(64)
)
RETURNS(
    Name Varchar(64),
    dateOfBirth date,
    Country Varchar(64)
)
AS
BEGIN
    FOR SELECT Authors.Name, Authors.dateOfBirth, Countries.Name
        from Authors
        Join Countries on Authors.countryId = Countries.Id
        where Authors.Name CONTAINING :search
    
    INTO :Name, :dateOfBirth, :Country DO
        SUSPEND;
END;
^
    
SET TERM ; ^
   
COMMIT;