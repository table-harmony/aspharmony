CREATE TABLE [dbo].[BookLoans] (
    [Id]                  INT           IDENTITY (1, 1) NOT NULL,
    [LibraryBookId]       INT           NOT NULL,
    [LibraryMembershipId] INT           NOT NULL,
    [LoanDate]            DATETIME2 (7) NOT NULL,
    [DueDate]             DATETIME2 (7) NOT NULL,
    [ReturnDate]          DATETIME2 (7) NULL,
    CONSTRAINT [PK_BookLoans] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BookLoans_LibraryBooks_LibraryBookId] FOREIGN KEY ([LibraryBookId]) REFERENCES [dbo].[LibraryBooks] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_BookLoans_LibraryMemberships_LibraryMembershipId] FOREIGN KEY ([LibraryMembershipId]) REFERENCES [dbo].[LibraryMemberships] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_BookLoans_LibraryMembershipId]
    ON [dbo].[BookLoans]([LibraryMembershipId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BookLoans_LibraryBookId]
    ON [dbo].[BookLoans]([LibraryBookId] ASC);

