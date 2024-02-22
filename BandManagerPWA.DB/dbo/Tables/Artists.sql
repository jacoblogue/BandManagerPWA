CREATE TABLE [dbo].[Artists] (
    [Id]          UNIQUEIDENTIFIER   NOT NULL,
    [Name]        NVARCHAR (MAX)     NOT NULL,
    [CreatedDate] DATETIMEOFFSET (7) NOT NULL,
    [UpdatedDate] DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Artists] PRIMARY KEY CLUSTERED ([Id] ASC)
);

