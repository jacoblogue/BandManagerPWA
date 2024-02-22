CREATE TABLE [dbo].[Users] (
    [Id]          UNIQUEIDENTIFIER   NOT NULL,
    [Email]       NVARCHAR (MAX)     NULL,
    [UserName]    NVARCHAR (MAX)     NULL,
    [FullName]    NVARCHAR (MAX)     NULL,
    [CreatedDate] DATETIMEOFFSET (7) NOT NULL,
    [UpdatedDate] DATETIMEOFFSET (7) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);

