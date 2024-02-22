CREATE TABLE [dbo].[EventGroup] (
    [EventsId] UNIQUEIDENTIFIER NOT NULL,
    [GroupsId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_EventGroup] PRIMARY KEY CLUSTERED ([EventsId] ASC, [GroupsId] ASC),
    CONSTRAINT [FK_EventGroup_Events_EventsId] FOREIGN KEY ([EventsId]) REFERENCES [dbo].[Events] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_EventGroup_Groups_GroupsId] FOREIGN KEY ([GroupsId]) REFERENCES [dbo].[Groups] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_EventGroup_GroupsId]
    ON [dbo].[EventGroup]([GroupsId] ASC);

