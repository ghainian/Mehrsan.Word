CREATE TABLE [dbo].[Graph] (
    [Id]        BIGINT IDENTITY (1, 1) NOT NULL,
    [SrcWordId] BIGINT NOT NULL,
    [DstWordId] BIGINT NOT NULL,
    CONSTRAINT [PK_Graph] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Graph_Word] FOREIGN KEY ([SrcWordId]) REFERENCES [dbo].[Word] ([Id]),
    CONSTRAINT [FK_Graph_Word1] FOREIGN KEY ([DstWordId]) REFERENCES [dbo].[Word] ([Id])
);

