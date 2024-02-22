CREATE TABLE [dbo].[BandManagerLogs] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Message]   NVARCHAR (MAX) NULL,
    [Level]     NVARCHAR (MAX) NULL,
    [LogDate]   DATETIME       NULL,
    [Exception] NVARCHAR (MAX) NULL,
    [LogEvent]  NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_BandManagerLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

