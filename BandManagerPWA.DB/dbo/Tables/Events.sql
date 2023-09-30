CREATE TABLE [dbo].[Events] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Title]       NVARCHAR (MAX)   NOT NULL,
    [Description] NVARCHAR (MAX)   NOT NULL,
    [Location]    NVARCHAR (MAX)   NOT NULL,
    [Date]        DATETIME2 (7)    NOT NULL,
    CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED ([Id] ASC)
);

