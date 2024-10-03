CREATE TABLE [dbo].[LibraryBooks] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [BookId]    INT NOT NULL,
    [LibraryId] INT NOT NULL,
    CONSTRAINT [PK_LibraryBooks] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LibraryBooks_Books_BookId] FOREIGN KEY ([BookId]) REFERENCES [dbo].[Books] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_LibraryBooks_Libraries_LibraryId] FOREIGN KEY ([LibraryId]) REFERENCES [dbo].[Libraries] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_LibraryBooks_LibraryId]
    ON [dbo].[LibraryBooks]([LibraryId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_LibraryBooks_BookId]
    ON [dbo].[LibraryBooks]([BookId] ASC);

