CREATE TABLE [dbo].[EventUser] (
    [EventsId] UNIQUEIDENTIFIER NOT NULL,
    [UsersId]  UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_EventUser] PRIMARY KEY CLUSTERED ([EventsId] ASC, [UsersId] ASC),
    CONSTRAINT [FK_EventUser_Events_EventsId] FOREIGN KEY ([EventsId]) REFERENCES [dbo].[Events] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_EventUser_Users_UsersId] FOREIGN KEY ([UsersId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_EventUser_UsersId]
    ON [dbo].[EventUser]([UsersId] ASC);

