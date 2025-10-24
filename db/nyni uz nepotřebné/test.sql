SET COUNT ON;

INSERT INTO Authors (name, dateOfBirth, countryId, aboutAuthor) VALUES ('Stephen King', '1947-09-21', 1, 'Americký spisovatel známý horory a thrillery.');
INSERT INTO Authors (name, dateOfBirth, countryId, aboutAuthor) VALUES ('J.R.R. Tolkien', '1892-01-03', 2, 'Britský filolog a autor Pána prstenů.');
INSERT INTO Authors (name, dateOfBirth, countryId, aboutAuthor) VALUES ('Agatha Christie', '1890-09-15', 3, 'Anglická autorka detektivních románů.');
INSERT INTO Authors (name, dateOfBirth, countryId, aboutAuthor) VALUES ('George Orwell', '1903-06-25', 4, 'Autor známý pro dystopická díla jako 1984.');
INSERT INTO Authors (name, dateOfBirth, countryId, aboutAuthor) VALUES ('Franz Kafka', '1883-07-03', 5, 'Pražský německy píšící spisovatel, autor Procesu.');

INSERT INTO Books (authorId, name, genreId, description, rating, languageId, publishingHouseId, isbn, ean, length, typeId, photo
) VALUES (1, 'The Shining', 1, 'Psychologický horor odehrávající se v opuštěném hotelu.', '9/10', 1, 1, '9780307743657', '9780307743657', 447, 1, NULL);
INSERT INTO Books (authorId, name, genreId, description, rating, languageId, publishingHouseId, isbn, ean, length, typeId, photo
) VALUES (1, 'It', 1, 'Hororový román o entitě, která terorizuje město Derry.', '9/10', 1, 2, '9781501182099', '9781501182099', 1138, 2, NULL);
INSERT INTO Books (authorId, name, genreId, description, rating, languageId, publishingHouseId, isbn, ean, length, typeId, photo
) VALUES (2, 'The Lord of the Rings', 2, 'Epické fantasy dobrodružství o prstenu moci.', '10/10', 2, 3, '9780261102385', '9780261102385', 1178, 3, NULL);
INSERT INTO Books (authorId, name, genreId, description, rating, languageId, publishingHouseId, isbn, ean, length, typeId, photo
) VALUES (3, 'Murder on the Orient Express', 3, 'Detektivní příběh s Herculem Poirotem.', '8/10', 3, 4, '9780007119318', '9780007119318', 256, 4, NULL);
INSERT INTO Books (authorId, name, genreId, description, rating, languageId, publishingHouseId, isbn, ean, length, typeId, photo
) VALUES (4, '1984', 4, 'Dystopický román o totalitním režimu a ztrátě svobody.', '10/10', 4, 5, '9780451524935', '9780451524935', 328, 5, NULL);
INSERT INTO Books (authorId, name, genreId, description, rating, languageId, publishingHouseId, isbn, ean, length, typeId, photo
) VALUES (5, 'The Trial', 5, 'Absurdní příběh Josefa K. obviněného bez důvodu.', '9/10', 5, 1, '9780805209990', '9780805209990', 240, 1, NULL);

SELECT COUNT(*) FROM Countries;
SELECT COUNT(*) FROM Languages;
SELECT COUNT(*) FROM Genres;
SELECT COUNT(*) FROM Publishers;
SELECT COUNT(*) FROM BookTypes;
SELECT COUNT(*) FROM Authors;
SELECT COUNT(*) FROM Books;

COMMIT;