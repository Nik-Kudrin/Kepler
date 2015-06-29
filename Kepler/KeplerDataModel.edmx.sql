
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/29/2015 22:43:39
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

IF OBJECT_ID(N'[dbo].[FK_BaseLineScreenShot]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InfoObjects_ScreenShot] DROP CONSTRAINT [FK_BaseLineScreenShot];
GO
IF OBJECT_ID(N'[dbo].[FK_ProjectBuild]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InfoObjects_Build] DROP CONSTRAINT [FK_ProjectBuild];
GO
IF OBJECT_ID(N'[dbo].[FK_ProjectBaseLine]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InfoObjects_Project] DROP CONSTRAINT [FK_ProjectBaseLine];
GO
IF OBJECT_ID(N'[dbo].[FK_BuildTestAssembly]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InfoObjects_TestAssembly] DROP CONSTRAINT [FK_BuildTestAssembly];
GO
IF OBJECT_ID(N'[dbo].[FK_TestAssemblyTestSuite]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InfoObjects_TestSuite] DROP CONSTRAINT [FK_TestAssemblyTestSuite];
GO
IF OBJECT_ID(N'[dbo].[FK_TestSuiteTestCase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InfoObjects_TestCase] DROP CONSTRAINT [FK_TestSuiteTestCase];
GO
IF OBJECT_ID(N'[dbo].[FK_TestCaseScreenShot]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InfoObjects_ScreenShot] DROP CONSTRAINT [FK_TestCaseScreenShot];
GO
IF OBJECT_ID(N'[dbo].[FK_BaseLine_inherits_InfoObject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InfoObjects_BaseLine] DROP CONSTRAINT [FK_BaseLine_inherits_InfoObject];
GO
IF OBJECT_ID(N'[dbo].[FK_BuildObject_inherits_InfoObject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InfoObjects_BuildObject] DROP CONSTRAINT [FK_BuildObject_inherits_InfoObject];
GO
IF OBJECT_ID(N'[dbo].[FK_ScreenShot_inherits_BuildObject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InfoObjects_ScreenShot] DROP CONSTRAINT [FK_ScreenShot_inherits_BuildObject];
GO
IF OBJECT_ID(N'[dbo].[FK_Project_inherits_InfoObject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InfoObjects_Project] DROP CONSTRAINT [FK_Project_inherits_InfoObject];
GO
IF OBJECT_ID(N'[dbo].[FK_Build_inherits_InfoObject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InfoObjects_Build] DROP CONSTRAINT [FK_Build_inherits_InfoObject];
GO
IF OBJECT_ID(N'[dbo].[FK_TestAssembly_inherits_BuildObject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InfoObjects_TestAssembly] DROP CONSTRAINT [FK_TestAssembly_inherits_BuildObject];
GO
IF OBJECT_ID(N'[dbo].[FK_TestSuite_inherits_BuildObject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InfoObjects_TestSuite] DROP CONSTRAINT [FK_TestSuite_inherits_BuildObject];
GO
IF OBJECT_ID(N'[dbo].[FK_TestCase_inherits_BuildObject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InfoObjects_TestCase] DROP CONSTRAINT [FK_TestCase_inherits_BuildObject];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[InfoObjects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InfoObjects];
GO
IF OBJECT_ID(N'[dbo].[InfoObjects_BaseLine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InfoObjects_BaseLine];
GO
IF OBJECT_ID(N'[dbo].[InfoObjects_BuildObject]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InfoObjects_BuildObject];
GO
IF OBJECT_ID(N'[dbo].[InfoObjects_ScreenShot]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InfoObjects_ScreenShot];
GO
IF OBJECT_ID(N'[dbo].[InfoObjects_Project]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InfoObjects_Project];
GO
IF OBJECT_ID(N'[dbo].[InfoObjects_Build]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InfoObjects_Build];
GO
IF OBJECT_ID(N'[dbo].[InfoObjects_TestAssembly]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InfoObjects_TestAssembly];
GO
IF OBJECT_ID(N'[dbo].[InfoObjects_TestSuite]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InfoObjects_TestSuite];
GO
IF OBJECT_ID(N'[dbo].[InfoObjects_TestCase]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InfoObjects_TestCase];
GO

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

-- Creating table 'InfoObjects_BaseLine'
CREATE TABLE [dbo].[InfoObjects_BaseLine] (
    [Id] bigint  NOT NULL
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
    [BaseLineId] bigint  NOT NULL,
    [TestCaseId] bigint  NOT NULL,
    [Id] bigint  NOT NULL
);
GO

-- Creating table 'InfoObjects_Project'
CREATE TABLE [dbo].[InfoObjects_Project] (
    [Id] bigint  NOT NULL
);
GO

-- Creating table 'InfoObjects_Build'
CREATE TABLE [dbo].[InfoObjects_Build] (
    [ProjectId] bigint  NOT NULL,
    [Config] nvarchar(max)  NOT NULL,
    [Id] bigint  NOT NULL
);
GO

-- Creating table 'InfoObjects_TestAssembly'
CREATE TABLE [dbo].[InfoObjects_TestAssembly] (
    [BuildId1] bigint  NOT NULL,
    [Id] bigint  NOT NULL
);
GO

-- Creating table 'InfoObjects_TestSuite'
CREATE TABLE [dbo].[InfoObjects_TestSuite] (
    [TestAssemblyId] bigint  NOT NULL,
    [Id] bigint  NOT NULL
);
GO

-- Creating table 'InfoObjects_TestCase'
CREATE TABLE [dbo].[InfoObjects_TestCase] (
    [TestSuiteId] bigint  NOT NULL,
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

-- Creating primary key on [Id] in table 'InfoObjects_BaseLine'
ALTER TABLE [dbo].[InfoObjects_BaseLine]
ADD CONSTRAINT [PK_InfoObjects_BaseLine]
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

-- Creating primary key on [Id] in table 'InfoObjects_Project'
ALTER TABLE [dbo].[InfoObjects_Project]
ADD CONSTRAINT [PK_InfoObjects_Project]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InfoObjects_Build'
ALTER TABLE [dbo].[InfoObjects_Build]
ADD CONSTRAINT [PK_InfoObjects_Build]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InfoObjects_TestAssembly'
ALTER TABLE [dbo].[InfoObjects_TestAssembly]
ADD CONSTRAINT [PK_InfoObjects_TestAssembly]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InfoObjects_TestSuite'
ALTER TABLE [dbo].[InfoObjects_TestSuite]
ADD CONSTRAINT [PK_InfoObjects_TestSuite]
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

-- Creating foreign key on [BaseLineId] in table 'InfoObjects_ScreenShot'
ALTER TABLE [dbo].[InfoObjects_ScreenShot]
ADD CONSTRAINT [FK_BaseLineScreenShot]
    FOREIGN KEY ([BaseLineId])
    REFERENCES [dbo].[InfoObjects_BaseLine]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BaseLineScreenShot'
CREATE INDEX [IX_FK_BaseLineScreenShot]
ON [dbo].[InfoObjects_ScreenShot]
    ([BaseLineId]);
GO

-- Creating foreign key on [ProjectId] in table 'InfoObjects_Build'
ALTER TABLE [dbo].[InfoObjects_Build]
ADD CONSTRAINT [FK_ProjectBuild]
    FOREIGN KEY ([ProjectId])
    REFERENCES [dbo].[InfoObjects_Project]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProjectBuild'
CREATE INDEX [IX_FK_ProjectBuild]
ON [dbo].[InfoObjects_Build]
    ([ProjectId]);
GO

-- Creating foreign key on [BuildId1] in table 'InfoObjects_TestAssembly'
ALTER TABLE [dbo].[InfoObjects_TestAssembly]
ADD CONSTRAINT [FK_BuildTestAssembly]
    FOREIGN KEY ([BuildId1])
    REFERENCES [dbo].[InfoObjects_Build]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BuildTestAssembly'
CREATE INDEX [IX_FK_BuildTestAssembly]
ON [dbo].[InfoObjects_TestAssembly]
    ([BuildId1]);
GO

-- Creating foreign key on [TestAssemblyId] in table 'InfoObjects_TestSuite'
ALTER TABLE [dbo].[InfoObjects_TestSuite]
ADD CONSTRAINT [FK_TestAssemblyTestSuite]
    FOREIGN KEY ([TestAssemblyId])
    REFERENCES [dbo].[InfoObjects_TestAssembly]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TestAssemblyTestSuite'
CREATE INDEX [IX_FK_TestAssemblyTestSuite]
ON [dbo].[InfoObjects_TestSuite]
    ([TestAssemblyId]);
GO

-- Creating foreign key on [TestSuiteId] in table 'InfoObjects_TestCase'
ALTER TABLE [dbo].[InfoObjects_TestCase]
ADD CONSTRAINT [FK_TestSuiteTestCase]
    FOREIGN KEY ([TestSuiteId])
    REFERENCES [dbo].[InfoObjects_TestSuite]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TestSuiteTestCase'
CREATE INDEX [IX_FK_TestSuiteTestCase]
ON [dbo].[InfoObjects_TestCase]
    ([TestSuiteId]);
GO

-- Creating foreign key on [TestCaseId] in table 'InfoObjects_ScreenShot'
ALTER TABLE [dbo].[InfoObjects_ScreenShot]
ADD CONSTRAINT [FK_TestCaseScreenShot]
    FOREIGN KEY ([TestCaseId])
    REFERENCES [dbo].[InfoObjects_TestCase]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TestCaseScreenShot'
CREATE INDEX [IX_FK_TestCaseScreenShot]
ON [dbo].[InfoObjects_ScreenShot]
    ([TestCaseId]);
GO

-- Creating foreign key on [Id] in table 'InfoObjects_BaseLine'
ALTER TABLE [dbo].[InfoObjects_BaseLine]
ADD CONSTRAINT [FK_BaseLine_inherits_InfoObject]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[InfoObjects]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

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

-- Creating foreign key on [Id] in table 'InfoObjects_Project'
ALTER TABLE [dbo].[InfoObjects_Project]
ADD CONSTRAINT [FK_Project_inherits_InfoObject]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[InfoObjects]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'InfoObjects_Build'
ALTER TABLE [dbo].[InfoObjects_Build]
ADD CONSTRAINT [FK_Build_inherits_InfoObject]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[InfoObjects]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'InfoObjects_TestAssembly'
ALTER TABLE [dbo].[InfoObjects_TestAssembly]
ADD CONSTRAINT [FK_TestAssembly_inherits_BuildObject]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[InfoObjects_BuildObject]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'InfoObjects_TestSuite'
ALTER TABLE [dbo].[InfoObjects_TestSuite]
ADD CONSTRAINT [FK_TestSuite_inherits_BuildObject]
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