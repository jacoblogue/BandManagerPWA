CREATE TABLE [dbo].[GroupUser] (
    [GroupsId] UNIQUEIDENTIFIER NOT NULL,
    [UsersId]  UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_GroupUser] PRIMARY KEY CLUSTERED ([GroupsId] ASC, [UsersId] ASC),
    CONSTRAINT [FK_GroupUser_Groups_GroupsId] FOREIGN KEY ([GroupsId]) REFERENCES [dbo].[Groups] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_GroupUser_Users_UsersId] FOREIGN KEY ([UsersId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_GroupUser_UsersId]
    ON [dbo].[GroupUser]([UsersId] ASC);

