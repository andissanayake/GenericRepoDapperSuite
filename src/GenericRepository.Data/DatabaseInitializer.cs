using Dapper;
using System.Data;
namespace GenericRepository.Data
{
    public static class DatabaseInitializer
    {
        public static void Migrate(IDbConnection dbConnection)
        {
            dbConnection.Open();

            var initScript = @"

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'YourEntity1')
                BEGIN
                    CREATE TABLE [dbo].[YourEntity1] (
                        [Id]          INT      IDENTITY (1, 1) NOT NULL,
                        [Prop1]       TEXT     NOT NULL,
                        [Prop2]       TEXT     NOT NULL,
                        [CreatedDate] DATETIME NOT NULL,
                        [UpdatedDate] DATETIME NULL,
                        PRIMARY KEY CLUSTERED ([Id] ASC)
                    );
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'YourEntity2')
                BEGIN
                    CREATE TABLE [dbo].[YourEntity2] (
                        [Id]          INT      IDENTITY (1, 1) NOT NULL,
                        [Prop1]       TEXT     NOT NULL,
                        [CreatedDate] DATETIME NOT NULL,
                        [UpdatedDate] DATETIME NULL,
                        PRIMARY KEY CLUSTERED ([Id] ASC)
                    );
                END
            ";

            // Execute SQL to create tables and other database objects
            dbConnection.Execute(initScript);
            dbConnection.Close();
        }
    }
}
