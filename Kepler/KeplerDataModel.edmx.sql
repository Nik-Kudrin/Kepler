
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/28/2015 20:40:35
-- Generated from EDMX file: E:\Repo_Kepler\Kepler\KeplerDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Kepler];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'InfoObjects'
CREATE TABLE [dbo].[InfoObjects] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Status] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'InfoObjects_BuildObject'
CREATE TABLE [dbo].[InfoObjects_BuildObject] (
    [BuildId] nvarchar(max)  NOT NULL,
    [Id] bigint  NOT NULL
);
GO

-- Creating table 'InfoObjects_ScreenShot'
CREATE TABLE [dbo].[InfoObjects_ScreenShot] (
    [ImagePath] nvarchar(max)  NOT NULL,
    [Id] bigint  NOT NULL
);
GO

-- Creating table 'InfoObjects_TestCase'
CREATE TABLE [dbo].[InfoObjects_TestCase] (
    [Id] bigint  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'InfoObjects'
ALTER TABLE [dbo].[InfoObjects]
ADD CONSTRAINT [PK_InfoObjects]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InfoObjects_BuildObject'
ALTER TABLE [dbo].[InfoObjects_BuildObject]
ADD CONSTRAINT [PK_InfoObjects_BuildObject]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InfoObjects_ScreenShot'
ALTER TABLE [dbo].[InfoObjects_ScreenShot]
ADD CONSTRAINT [PK_InfoObjects_ScreenShot]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InfoObjects_TestCase'
ALTER TABLE [dbo].[InfoObjects_TestCase]
ADD CONSTRAINT [PK_InfoObjects_TestCase]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Id] in table 'InfoObjects_BuildObject'
ALTER TABLE [dbo].[InfoObjects_BuildObject]
ADD CONSTRAINT [FK_BuildObject_inherits_InfoObject]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[InfoObjects]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'InfoObjects_ScreenShot'
ALTER TABLE [dbo].[InfoObjects_ScreenShot]
ADD CONSTRAINT [FK_ScreenShot_inherits_BuildObject]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[InfoObjects_BuildObject]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'InfoObjects_TestCase'
ALTER TABLE [dbo].[InfoObjects_TestCase]
ADD CONSTRAINT [FK_TestCase_inherits_BuildObject]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[InfoObjects_BuildObject]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------