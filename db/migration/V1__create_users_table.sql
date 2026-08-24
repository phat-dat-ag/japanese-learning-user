CREATE TABLE dbo.Users
(
    Id UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT PK_Users PRIMARY KEY,

    Username NVARCHAR(100) NOT NULL,

    Email NVARCHAR(255) NOT NULL,

    PasswordHash NVARCHAR(500) NOT NULL,

    IsActive BIT NOT NULL
        CONSTRAINT DF_Users_IsActive DEFAULT 1,

    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Users_CreatedAt DEFAULT SYSUTCDATETIME(),

    UpdatedAt DATETIME2 NULL
);

CREATE UNIQUE INDEX UX_Users_Username
    ON dbo.Users(Username);

CREATE UNIQUE INDEX UX_Users_Email
    ON dbo.Users(Email);