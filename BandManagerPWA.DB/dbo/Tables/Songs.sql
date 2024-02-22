CREATE TABLE [dbo].[Songs] (
    [Id]          UNIQUEIDENTIFIER   NOT NULL,
    [Title]       NVARCHAR (MAX)     NOT NULL,
    [ArtistId]    UNIQUEIDENTIFIER   NOT NULL,
    [Key]         NVARCHAR (MAX)     NULL,
    [CreatedDate] DATETIMEOFFSET (7) NOT NULL,
    [UpdatedDate] DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Songs] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Songs_Artists_ArtistId] FOREIGN KEY ([ArtistId]) REFERENCES [dbo].[Artists] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Songs_ArtistId]
    ON [dbo].[Songs]([ArtistId] ASC);

