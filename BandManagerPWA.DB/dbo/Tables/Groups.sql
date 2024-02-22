CREATE TABLE [dbo].[Groups] (
    [Id]          UNIQUEIDENTIFIER   NOT NULL,
    [Name]        NVARCHAR (MAX)     NOT NULL,
    [Description] NVARCHAR (MAX)     NULL,
    [CreatedDate] DATETIMEOFFSET (7) NOT NULL,
    [UpdatedDate] DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED ([Id] ASC)
);

