CREATE OR ALTER PROCEDURE dbo.usp_Users_ExistsByUsername
    @Username NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CASE
            WHEN EXISTS
            (
                SELECT 1
                FROM dbo.Users
                WHERE Username = @Username
            )
            THEN CAST(1 AS BIT)
            ELSE CAST(0 AS BIT)
        END;
END;
GO


CREATE OR ALTER PROCEDURE dbo.usp_Users_ExistsByEmail
    @Email NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CASE
            WHEN EXISTS
            (
                SELECT 1
                FROM dbo.Users
                WHERE Email = @Email
            )
            THEN CAST(1 AS BIT)
            ELSE CAST(0 AS BIT)
        END;
END;
GO


CREATE OR ALTER PROCEDURE dbo.usp_Users_Create
    @Id UNIQUEIDENTIFIER,
    @Username NVARCHAR(100),
    @Email NVARCHAR(255),
    @PasswordHash NVARCHAR(500),
    @IsActive BIT,
    @CreatedAt DATETIME2,
    @UpdatedAt DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Users
    (
        Id,
        Username,
        Email,
        PasswordHash,
        IsActive,
        CreatedAt,
        UpdatedAt
    )
    VALUES
    (
        @Id,
        @Username,
        @Email,
        @PasswordHash,
        @IsActive,
        @CreatedAt,
        @UpdatedAt
    );
END;
GO