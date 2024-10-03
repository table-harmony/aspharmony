CREATE TABLE [dbo].[LibraryMemberships] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Role]      NVARCHAR (MAX) NOT NULL,
    [UserId]    NVARCHAR (450) NOT NULL,
    [LibraryId] INT            NOT NULL,
    CONSTRAINT [PK_LibraryMemberships] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LibraryMemberships_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_LibraryMemberships_Libraries_LibraryId] FOREIGN KEY ([LibraryId]) REFERENCES [dbo].[Libraries] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_LibraryMemberships_UserId]
    ON [dbo].[LibraryMemberships]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_LibraryMemberships_LibraryId]
    ON [dbo].[LibraryMemberships]([LibraryId] ASC);

