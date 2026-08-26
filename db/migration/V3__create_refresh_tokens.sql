CREATE TABLE dbo.RefreshTokens
(
    Id UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_RefreshTokens PRIMARY KEY,

    UserId UNIQUEIDENTIFIER NOT NULL,

    TokenHash NVARCHAR(500) NOT NULL,

    ExpiresAt DATETIME2 NOT NULL,

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_RefreshTokens_CreatedAt DEFAULT SYSUTCDATETIME(),

    RevokedAt DATETIME2 NULL,

    CONSTRAINT FK_RefreshTokens_Users
        FOREIGN KEY (UserId)
        REFERENCES dbo.Users(Id)
);

CREATE UNIQUE INDEX UX_RefreshTokens_TokenHash
    ON dbo.RefreshTokens(TokenHash);

CREATE INDEX IX_RefreshTokens_UserId
    ON dbo.RefreshTokens(UserId);

CREATE INDEX IX_RefreshTokens_ExpiresAt
    ON dbo.RefreshTokens(ExpiresAt);
GO


CREATE OR ALTER PROCEDURE dbo.usp_RefreshTokens_Create
    @Id UNIQUEIDENTIFIER,
    @UserId UNIQUEIDENTIFIER,
    @TokenHash NVARCHAR(500),
    @ExpiresAt DATETIME2,
    @CreatedAt DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.RefreshTokens
    (
        Id,
        UserId,
        TokenHash,
        ExpiresAt,
        CreatedAt
    )
    VALUES
    (
        @Id,
        @UserId,
        @TokenHash,
        @ExpiresAt,
        @CreatedAt
    );
END;
GO


CREATE OR ALTER PROCEDURE dbo.usp_RefreshTokens_GetByTokenHash
    @TokenHash NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        UserId,
        TokenHash,
        ExpiresAt,
        CreatedAt,
        RevokedAt
    FROM dbo.RefreshTokens
    WHERE TokenHash = @TokenHash;
END;
GO


CREATE OR ALTER PROCEDURE dbo.usp_RefreshTokens_Revoke
    @Id UNIQUEIDENTIFIER,
    @RevokedAt DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.RefreshTokens
    SET RevokedAt = @RevokedAt
    WHERE Id = @Id
      AND RevokedAt IS NULL;
END;
GO

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
        IsActive,
        CreatedAt,
        UpdatedAt
    FROM dbo.Users
    WHERE Email = @Email;
END;
GO

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
        IsActive,
        CreatedAt,
        UpdatedAt
    FROM dbo.Users
    WHERE Id = @Id;
END;
GO