var sql = `
DECLARE 
    IF EXISTS (SELECT * FROM [DBO].[USER_BACKUP])
    BEGIN
        CREATE TABLE [DBO].[USER_BACKUP]
        SELECT *
        FROM [DBO].[USER]        
    END
    ELSE
    BEGIN
            
    END


    CREATE TABLE [DBO].[USER]


    DROP TABLE IF EXISTS [DBO].[USER]


`;