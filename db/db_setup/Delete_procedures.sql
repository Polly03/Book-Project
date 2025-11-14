-- DELETE PROCEDURES

-- deleting author based on his name, only if he didnt wrote any book
SET TERM ^ ;
CREATE OR ALTER PROCEDURE DELETE_AUTHOR (AUTHOR_NAME VARCHAR(64))
RETURNS (
	STATUS INTEGER
)
AS
declare variable BOOK_COUNT integer;
declare variable AUTHOR_ID integer;
begin
        SELECT ID FROM AUTHORS WHERE NAME = :author_name
        INTO :AUTHOR_ID;

        SELECT COUNT(*) FROM BOOKS WHERE AUTHORID = :author_ID
        INTO :BOOK_COUNT;

        if (BOOK_COUNT > 0) then
        begin
            status = 1;
            suspend;
        end
        else
        begin
            delete from authors where id = :author_id;
            status = 0;
            suspend;
        end
end;
^
SET TERM ; ^


-- deleting book base on its name
SET TERM ^ ;

create or alter procedure DELETE_BOOK (
    NAME_OF_BOOK varchar(64),
    ID_OF_BOOK integer)
as
begin
    DELETE FROM books
    where name = :name_of_book AND ID = :ID_OF_BOOk;
end;
^



SET TERM ; ^

COMMIT;