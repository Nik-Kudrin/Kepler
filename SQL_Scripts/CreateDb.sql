USE [master]
GO

DECLARE @DbName varchar(60) = 'Kepler'
DECLARE @COMMAND_TEMPLATE VARCHAR(MAX)
DECLARE @SQL_SCRIPT VARCHAR(MAX)
DECLARE @USE_DB_TEMPLATE varchar(60) 
DECLARE @USE_DB_SCRIPT VARCHAR(60)

set @DbName = QUOTENAME(@DbName)

EXEC('CREATE DATABASE '+ @DbName)


SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET ANSI_NULL_DEFAULT OFF '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)

SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET ANSI_NULLS OFF '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)

SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET ANSI_PADDING OFF '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)
 
SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET ANSI_WARNINGS OFF  '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)
 
SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET ARITHABORT OFF '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)
 
SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET AUTO_CLOSE OFF '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)
 
SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET AUTO_SHRINK OFF '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)
 
SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET AUTO_UPDATE_STATISTICS ON '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)
 
SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET CURSOR_CLOSE_ON_COMMIT OFF '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)
 
SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET CURSOR_DEFAULT  GLOBAL '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)
 
SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET CONCAT_NULL_YIELDS_NULL OFF '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)

SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME}  SET NUMERIC_ROUNDABORT OFF '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)
 
SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET QUOTED_IDENTIFIER OFF '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)
 
SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET RECURSIVE_TRIGGERS OFF '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)
 
SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET  DISABLE_BROKER '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)
 
SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET AUTO_UPDATE_STATISTICS_ASYNC OFF '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)
 
SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET DATE_CORRELATION_OPTIMIZATION OFF '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)

SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME}  SET TRUSTWORTHY OFF '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)
 
SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET ALLOW_SNAPSHOT_ISOLATION OFF '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)
 
SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET PARAMETERIZATION SIMPLE '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)
 
SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET READ_COMMITTED_SNAPSHOT ON '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)

SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME}  SET HONOR_BROKER_PRIORITY OFF '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)

SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME}  SET RECOVERY FULL '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)
 
SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET  MULTI_USER '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)

SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET PAGE_VERIFY CHECKSUM  '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)
 
SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET DB_CHAINING OFF '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)
 
SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)

SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME}  SET TARGET_RECOVERY_TIME = 0 SECONDS '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)
 
SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET DELAYED_DURABILITY = DISABLED '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)


/****** Object:  Table [dbo].[BaseLine]    Script Date: 5/31/2016 8:42:14 PM ******/

SET @COMMAND_TEMPLATE='USE {DBNAME};
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON


CREATE TABLE {DBNAME}.[dbo].[BaseLine](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BranchId] [bigint] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.BaseLine] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]'

SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)

/****** Object:  Table [dbo].[Branch]    Script Date: 6/28/2016 11:02:52 PM ******/
SET @COMMAND_TEMPLATE='USE {DBNAME};
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[Branch](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BaseLineId] [bigint] NULL,
	[LatestBuildId] [bigint] NULL,
	[ProjectId] [bigint] NULL,
	[IsMainBranch] [bit] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Branch] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]'

SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)


/****** Object:  Table [dbo].[Build]    Script Date: 5/31/2016 8:42:14 PM ******/
SET @COMMAND_TEMPLATE='USE {DBNAME};
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON


CREATE TABLE {DBNAME}.[dbo].[Build](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[StartDate] [datetime] NULL,
	[StopDate] [datetime] NULL,
	[Duration] [time](7) NULL,
	[PredictedDuration] [time](7) NULL,
	[BranchId] [bigint] NULL,
	[NumberTestAssemblies] [bigint] NOT NULL,
	[NumberTestSuites] [bigint] NOT NULL,
	[NumberTestCase] [bigint] NOT NULL,
	[NumberScreenshots] [bigint] NOT NULL,
	[NumberFailedScreenshots] [bigint] NOT NULL,
	[BuildId] [bigint] NULL,
	[ParentObjId] [bigint] NULL,
	[Name] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Build] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]'

SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)


/****** Object:  Table [dbo].[ErrorMessage]    Script Date: 5/31/2016 8:42:14 PM ******/
SET @COMMAND_TEMPLATE='USE {DBNAME};
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE {DBNAME}.[dbo].[ErrorMessage](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Time] [datetime] NULL,
	[Code] [int] NOT NULL,
	[IsLastViewed] [bit] NOT NULL,
	[ExceptionMessage] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.ErrorMessage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]'

SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)


/****** Object:  Table [dbo].[ImageWorker]    Script Date: 5/31/2016 8:42:14 PM ******/
SET @COMMAND_TEMPLATE='USE {DBNAME};
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE {DBNAME}.[dbo].[ImageWorker](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[WorkerServiceUrl] [nvarchar](600) NULL,
	[WorkerStatus] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ImageWorker] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]'

SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)


/****** Object:  Table [dbo].[KeplerSystemConfig]    Script Date: 5/31/2016 8:42:14 PM ******/
SET @COMMAND_TEMPLATE='USE {DBNAME};
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE {DBNAME}.[dbo].[KeplerSystemConfig](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.KeplerSystemConfig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]'

SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)



/****** Object:  Table [dbo].[Project]    Script Date: 5/31/2016 8:42:14 PM ******/
SET @COMMAND_TEMPLATE='USE {DBNAME};
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE {DBNAME}.[dbo].[Project](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[MainBranchId] [bigint] NULL,
	[Name] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Project] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]'

SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)



/****** Object:  Table [dbo].[ScreenShot]    Script Date: 5/31/2016 8:42:14 PM ******/
SET @COMMAND_TEMPLATE='USE {DBNAME};
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE {DBNAME}.[dbo].[ScreenShot](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ImagePath] [nvarchar](max) NULL,
	[ImagePathUrl] [nvarchar](max) NULL,
	[PreviewImagePath] [nvarchar](max) NULL,
	[PreviewImagePathUrl] [nvarchar](max) NULL,
	[DiffImagePath] [nvarchar](max) NULL,
	[DiffImagePathUrl] [nvarchar](max) NULL,
	[DiffPreviewPath] [nvarchar](max) NULL,
	[DiffPreviewPathUrl] [nvarchar](max) NULL,
	[BaseLineImagePath] [nvarchar](max) NULL,
	[BaseLineImagePathUrl] [nvarchar](max) NULL,
	[BaseLinePreviewPath] [nvarchar](max) NULL,
	[BaseLinePreviewPathUrl] [nvarchar](max) NULL,
	[Url] [nvarchar](max) NULL,
	[BaseLineId] [bigint] NOT NULL,
	[IsLastPassed] [bit] NOT NULL,
	[ErrorMessage] [nvarchar](max) NULL,
	[BuildId] [bigint] NULL,
	[ParentObjId] [bigint] NULL,
	[Name] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ScreenShot] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]'

SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)



/****** Object:  Table [dbo].[TestAssembly]    Script Date: 5/31/2016 8:42:14 PM ******/
SET @COMMAND_TEMPLATE='USE {DBNAME};
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE {DBNAME}.[dbo].[TestAssembly](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BuildId] [bigint] NULL,
	[ParentObjId] [bigint] NULL,
	[Name] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.TestAssembly] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]'

SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)



/****** Object:  Table [dbo].[TestCase]    Script Date: 5/31/2016 8:42:14 PM ******/
SET @COMMAND_TEMPLATE='USE {DBNAME};
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE {DBNAME}.[dbo].[TestCase](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BuildId] [bigint] NULL,
	[ParentObjId] [bigint] NULL,
	[Name] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.TestCase] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]'

SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)



/****** Object:  Table [dbo].[TestSuite]    Script Date: 5/31/2016 8:42:14 PM ******/
SET @COMMAND_TEMPLATE='USE {DBNAME};
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE {DBNAME}.[dbo].[TestSuite](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BuildId] [bigint] NULL,
	[ParentObjId] [bigint] NULL,
	[Name] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.TestSuite] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]'

SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)



USE [master]

SET @COMMAND_TEMPLATE='ALTER DATABASE {DBNAME} SET  READ_WRITE '
SET @SQL_SCRIPT = REPLACE(@COMMAND_TEMPLATE, '{DBNAME}', @DbName)
EXECUTE (@SQL_SCRIPT)

GO