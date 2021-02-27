
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/27/2021 19:21:13
-- Generated from EDMX file: C:\Users\semah\source\repos\Test\Test\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Testt];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserPost]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PostSet] DROP CONSTRAINT [FK_UserPost];
GO
IF OBJECT_ID(N'[dbo].[FK_PostLike]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LikeSet] DROP CONSTRAINT [FK_PostLike];
GO
IF OBJECT_ID(N'[dbo].[FK_UserLike]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LikeSet] DROP CONSTRAINT [FK_UserLike];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSet];
GO
IF OBJECT_ID(N'[dbo].[PostSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PostSet];
GO
IF OBJECT_ID(N'[dbo].[LikeSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LikeSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserSet'
CREATE TABLE [dbo].[UserSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [DateOfBirth] datetime  NOT NULL,
    [Notes] nvarchar(max)  NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LasName] nvarchar(max)  NOT NULL,
    [ImageUrl] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'PostSet'
CREATE TABLE [dbo].[PostSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [UserId] int  NOT NULL,
    [CreationDate] datetime  NOT NULL
);
GO

-- Creating table 'LikeSet'
CREATE TABLE [dbo].[LikeSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PostId] int  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [PK_UserSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PostSet'
ALTER TABLE [dbo].[PostSet]
ADD CONSTRAINT [PK_PostSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LikeSet'
ALTER TABLE [dbo].[LikeSet]
ADD CONSTRAINT [PK_LikeSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserId] in table 'PostSet'
ALTER TABLE [dbo].[PostSet]
ADD CONSTRAINT [FK_UserPost]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UserSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserPost'
CREATE INDEX [IX_FK_UserPost]
ON [dbo].[PostSet]
    ([UserId]);
GO

-- Creating foreign key on [PostId] in table 'LikeSet'
ALTER TABLE [dbo].[LikeSet]
ADD CONSTRAINT [FK_PostLike]
    FOREIGN KEY ([PostId])
    REFERENCES [dbo].[PostSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PostLike'
CREATE INDEX [IX_FK_PostLike]
ON [dbo].[LikeSet]
    ([PostId]);
GO

-- Creating foreign key on [UserId] in table 'LikeSet'
ALTER TABLE [dbo].[LikeSet]
ADD CONSTRAINT [FK_UserLike]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UserSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserLike'
CREATE INDEX [IX_FK_UserLike]
ON [dbo].[LikeSet]
    ([UserId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------