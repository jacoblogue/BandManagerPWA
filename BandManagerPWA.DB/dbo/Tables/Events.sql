CREATE TABLE [dbo].[Events] (
    [Id]          UNIQUEIDENTIFIER   NOT NULL,
    [Title]       NVARCHAR (MAX)     NOT NULL,
    [Description] NVARCHAR (MAX)     NOT NULL,
    [Location]    NVARCHAR (MAX)     NOT NULL,
    [Date]        DATETIMEOFFSET (7) NOT NULL,
    [CreatedDate] DATETIMEOFFSET (7) DEFAULT ('0001-01-01T00:00:00.0000000+00:00') NOT NULL,
    [UpdatedDate] DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED ([Id] ASC)
);



