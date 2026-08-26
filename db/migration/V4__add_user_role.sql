-- =========================================================
-- Add Role to Users
-- 1 = User
-- 2 = Admin
-- =========================================================

ALTER TABLE dbo.Users
ADD Role INT NOT NULL
    CONSTRAINT DF_Users_Role DEFAULT 1;
GO


-- =========================================================
-- Update User Create Stored Procedure
-- =========================================================

CREATE OR ALTER PROCEDURE dbo.usp_Users_Create
    @Id UNIQUEIDENTIFIER,
    @Username NVARCHAR(100),
    @Email NVARCHAR(255),
    @PasswordHash NVARCHAR(500),
    @Role INT,
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
        Role,
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
        @Role,
        @IsActive,
        @CreatedAt,
        @UpdatedAt
    );
END;
GO


-- =========================================================
-- Update User Get By Email Stored Procedure
-- =========================================================

CREATE OR ALTER PROCEDURE dbo.usp_Users_GetByEmail
    @Email NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Username,
        Email,
        PasswordHash,
        Role,
        IsActive,
        CreatedAt,
        UpdatedAt
    FROM dbo.Users
    WHERE Email = @Email;
END;
GO


-- =========================================================
-- Update User Get By Id Stored Procedure
-- =========================================================

CREATE OR ALTER PROCEDURE dbo.usp_Users_GetById
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Username,
        Email,
        PasswordHash,
        Role,
        IsActive,
        CreatedAt,
        UpdatedAt
    FROM dbo.Users
    WHERE Id = @Id;
END;
GO