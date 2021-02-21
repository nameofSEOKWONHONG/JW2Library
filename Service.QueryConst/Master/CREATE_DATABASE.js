var sql = `
    USE MASTER

    DECLARE @DBNAME NVARCHAR(128)
    SET @DBNAME = '${DBNAME}'

    IF (NOT EXISTS(SELECT NAME
                   FROM MASTER.DBO.SYSDATABASES
                   WHERE NAME = @DBNAME))
        BEGIN
            PRINT N'JWLIBRARY DB CREATE'
            CREATE DATABASE JWLIBRARY
        END
    ELSE
        BEGIN
            PRINT N'EXISTS DATABASE JWLIBRARY'
            SELECT 1
        END
`;