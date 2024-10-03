CREATE TABLE [dbo].[Libraries] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Libraries] PRIMARY KEY CLUSTERED ([Id] ASC)
);

