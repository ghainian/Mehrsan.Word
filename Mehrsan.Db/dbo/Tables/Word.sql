CREATE TABLE [dbo].[Word] (
    [Id]                BIGINT         IDENTITY (1, 1) NOT NULL,
    [TargetWord]        NVARCHAR (400) NOT NULL,
    [Meaning]           NVARCHAR (MAX) NOT NULL,
    [TargetLanguageId]  BIGINT         NOT NULL,
    [MeaningLanguageId] BIGINT         NOT NULL,
    CONSTRAINT [PK_Word] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Word_LanguageMean] FOREIGN KEY ([MeaningLanguageId]) REFERENCES [dbo].[Language] ([Id]),
    CONSTRAINT [FK_Word_LanguageWord] FOREIGN KEY ([TargetLanguageId]) REFERENCES [dbo].[Language] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Word]
    ON [dbo].[Word]([TargetWord] ASC);

