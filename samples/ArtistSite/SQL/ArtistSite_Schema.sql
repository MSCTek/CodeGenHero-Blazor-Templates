CREATE DATABASE [ArtistSite] COLLATE SQL_Latin1_General_CP1_CI_AS;
GO
USE [ArtistSite]
GO
ALTER DATABASE [ArtistSite] SET COMPATIBILITY_LEVEL = 130
GO
ALTER DATABASE [ArtistSite] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ArtistSite] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ArtistSite] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ArtistSite] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ArtistSite] SET ARITHABORT OFF 
GO
ALTER DATABASE [ArtistSite] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ArtistSite] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ArtistSite] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ArtistSite] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ArtistSite] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ArtistSite] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ArtistSite] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ArtistSite] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ArtistSite] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ArtistSite] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ArtistSite] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [ArtistSite] SET  MULTI_USER 
GO
ALTER DATABASE [ArtistSite] SET QUERY_STORE = ON
GO
ALTER DATABASE [ArtistSite] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200)
GO

sp_configure 'contained database authentication', 1;
GO
RECONFIGURE;
GO
ALTER DATABASE [ArtistSite]
SET containment = PARTIAL
GO
CREATE USER [MyAdmin] WITH PASSWORD=N'MySuperSecretPW007&', DEFAULT_SCHEMA=[dbo]
GO
sys.sp_addrolemember @rolename = N'db_owner', @membername = N'MyAdmin'
GO

/****** Object:  UserDefinedFunction [dbo].[getCategoryPath]    Script Date: 11/19/2021 8:35:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[getCategoryPath](@categoryID int)
    RETURNS nvarchar(4000)
AS
BEGIN
    DECLARE @categorypath nvarchar(4000), @catID int, @catName nvarchar(400)
    SET @categorypath  = ''
    
    SELECT @catID = ParentCategoryID, @catName = Name From dbo.category where CategoryID = @categoryID 
    WHILE @@rowcount > 0 BEGIN
        SET @categorypath = '\' + @catName + @categorypath 
        SELECT @catID = ParentCategoryID, @catName = Name From dbo.category where CategoryID = @catID 
    END
    
    RETURN @categorypath 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetIndexColumnOrder]    Script Date: 11/19/2021 8:35:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetIndexColumnOrder] 
( 
    @object_id INT, 
    @index_id TINYINT, 
    @column_id TINYINT 
) 
RETURNS NVARCHAR(5) 
AS 
BEGIN 
    DECLARE @r NVARCHAR(5) 
    SELECT @r = CASE INDEXKEY_PROPERTY 
    ( 
        @object_id, 
        @index_id, 
        @column_id, 
        'IsDescending' 
    ) 
        WHEN 1 THEN N' DESC' 
        ELSE N'' 
    END 
    RETURN @r 
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetIndexColumns]    Script Date: 11/19/2021 8:35:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetIndexColumns] 
( 
    @table_name SYSNAME, 
    @object_id INT, 
    @index_id TINYINT 
) 
RETURNS NVARCHAR(4000) 
AS 
BEGIN 
    DECLARE 
        @colnames NVARCHAR(4000),  
        @thisColID INT, 
        @thisColName SYSNAME 
         
    SET @colnames = INDEX_COL(@table_name, @index_id, 1) 
        + dbo.GetIndexColumnOrder(@object_id, @index_id, 1) 
 
    SET @thisColID = 2 
    SET @thisColName = INDEX_COL(@table_name, @index_id, @thisColID) 
        + dbo.GetIndexColumnOrder(@object_id, @index_id, @thisColID) 
 
    WHILE (@thisColName IS NOT NULL) 
    BEGIN 
        SET @thisColID = @thisColID + 1 
        SET @colnames = @colnames + ', ' + @thisColName 
 
        SET @thisColName = INDEX_COL(@table_name, @index_id, @thisColID) 
            + dbo.GetIndexColumnOrder(@object_id, @index_id, @thisColID) 
    END 
    RETURN @colNames 
END
GO
/****** Object:  UserDefinedFunction [dbo].[getSectionPath]    Script Date: 11/19/2021 8:35:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [dbo].[getSectionPath](@sectionID int)
    RETURNS nvarchar(4000)
AS
BEGIN
    DECLARE @Sectionpath nvarchar(4000), @SecID int, @SectionName nvarchar(400)
    SET @Sectionpath  = ''
    
    SELECT @SecID = ParentSectionID, @SectionName = Name From dbo.Section where SectionID = @sectionID 
    WHILE @@rowcount > 0 BEGIN
        SET @Sectionpath = '\' + @SectionName + @Sectionpath 
        SELECT @SecID = ParentSectionID, @SectionName = Name From dbo.Section where SectionID = @SecID 
    END
    
    RETURN @Sectionpath 
END
GO
/****** Object:  UserDefinedFunction [dbo].[MakeSEName]    Script Date: 11/19/2021 8:35:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [dbo].[MakeSEName](@string varchar(8000))
RETURNS varchar(4000)
AS
BEGIN
    declare @charindex int, @newstring varchar(8000)

    set @string = replace(replace(replace(@string, ' ', '-'), '---', '-'), '--', '-')
        
    set @newstring = ''

    select @charindex = PATINDEX('%[^a-z0-9_-]%', @string)

    IF @charindex = len(@string)
        select @newstring = left(@string, @charindex-1)
        
    ELSE BEGIN
        select @newstring = @newstring + left(@string, @charindex-1), @string = substring(@string, @charindex+1, len(@string)-@charindex+1)
        WHILE PATINDEX('%[^a-z0-9_-]%', @string) > 0 BEGIN
            select @charindex = PATINDEX('%[^a-z0-9_-]%', @string)
            IF @charindex = len(@string)
                select @newstring = @newstring + left(@string, @charindex-1), @string = ''
            ELSE
                select @newstring = @newstring + left(@string, @charindex-1), @string = substring(@string, @charindex+1, len(@string)-@charindex+1)
        END
    END
    RETURN lower(@newstring)

END
GO
/****** Object:  UserDefinedFunction [dbo].[WordCount]    Script Date: 11/19/2021 8:35:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[WordCount] 
(
	@Word Varchar(15),
	@Phrase Varchar(1000)
)	
RETURNS SMALLINT
AS
BEGIN

IF @Word IS NULL OR @Phrase IS NULL RETURN 0

DECLARE @BiggerWord Varchar(21)
SELECT @BiggerWord = @Word + 'x'

DECLARE @BiggerPhrase Varchar(2000)
SELECT @BiggerPhrase = REPLACE (@Phrase, @Word, @BiggerWord)

RETURN LEN(@BiggerPhrase) - LEN(@Phrase)
END
GO
/****** Object:  UserDefinedFunction [dbo].[ZeroFloor]    Script Date: 11/19/2021 8:35:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ZeroFloor](@value money)
RETURNS money
AS BEGIN
    IF @value < 0
        SET @value = 0
    RETURN @value 
END
GO
/****** Object:  Table [dbo].[Artist]    Script Date: 11/19/2021 8:35:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Artist](
	[ArtistId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Code] [nvarchar](20) NOT NULL,
	[Email] [nvarchar](50) NULL,
	[AboutBlurb] [nvarchar](4000) NULL,
	[IsActive] [bit] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[AppBarImgSrcSmall] [nvarchar](500) NULL,
	[AppBarImgSrcLarge] [nvarchar](500) NULL,
	[HeaderCssClass] [nvarchar](50) NULL,
	[MainContentCssClass] [nvarchar](50) NULL,
	[FooterCssClass] [nvarchar](50) NULL,
 CONSTRAINT [PK_Artist] PRIMARY KEY CLUSTERED 
(
	[ArtistId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Artwork]    Script Date: 11/19/2021 8:35:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Artwork](
	[ArtworkId] [int] IDENTITY(1,1) NOT NULL,
	[ArtistId] [int] NOT NULL,
	[Name] [nvarchar](400) NOT NULL,
	[Description] [nvarchar](2000) NULL,
	[DisplayOrder] [int] NOT NULL,
	[LayoutPageName] [nvarchar](50) NULL,
	[CssClass] [nvarchar](50) NULL,
	[IconUri] [nvarchar](50) NOT NULL,
	[ImageUri] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Artwork] PRIMARY KEY CLUSTERED 
(
	[ArtworkId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ArtworkCategory_Xref]    Script Date: 11/19/2021 8:35:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArtworkCategory_Xref](
	[ArtworkId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_ArtworkCategory] PRIMARY KEY CLUSTERED 
(
	[ArtworkId] ASC,
	[CategoryId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ArtworkMedium_Xref]    Script Date: 11/19/2021 8:35:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArtworkMedium_Xref](
	[ArtworkId] [int] NOT NULL,
	[MediumId] [int] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_ArtworkMedium] PRIMARY KEY CLUSTERED 
(
	[ArtworkId] ASC,
	[MediumId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 11/19/2021 8:35:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[ParentCategoryId] [int] NULL,
	[ArtistId] [int] NOT NULL,
	[Name] [nvarchar](400) NOT NULL,
	[CssClass] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Medium]    Script Date: 11/19/2021 8:35:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Medium](
	[MediumID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](400) NOT NULL,
	[Code] [nvarchar](20) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_Medium] PRIMARY KEY CLUSTERED 
(
	[MediumID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NewsItem]    Script Date: 11/19/2021 8:35:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NewsItem](
	[NewsItemId] [int] IDENTITY(1,1) NOT NULL,
	[Headline] [nvarchar](2000) NULL,
	[NewsCopy] [nvarchar](max) NULL,
 CONSTRAINT [PK_News] PRIMARY KEY CLUSTERED 
(
	[NewsItemId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Index [UC_Artist]    Script Date: 11/19/2021 8:35:27 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UC_Artist] ON [dbo].[Artist]
(
	[Code] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ArtworkCategory_Xref] ADD  CONSTRAINT [DF_ArtworkCategory_DisplayOrder]  DEFAULT ((1)) FOR [DisplayOrder]
GO
ALTER TABLE [dbo].[ArtworkMedium_Xref] ADD  CONSTRAINT [DF_ArtworkMedium_DisplayOrder]  DEFAULT ((1)) FOR [DisplayOrder]
GO
ALTER TABLE [dbo].[Medium] ADD  CONSTRAINT [DF_Medium_DisplayOrder]  DEFAULT ((1)) FOR [DisplayOrder]
GO
ALTER TABLE [dbo].[Artwork]  WITH CHECK ADD  CONSTRAINT [FK_Artwork_Artist] FOREIGN KEY([ArtistId])
REFERENCES [dbo].[Artist] ([ArtistId])
GO
ALTER TABLE [dbo].[Artwork] CHECK CONSTRAINT [FK_Artwork_Artist]
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [FK_Category_Artist] FOREIGN KEY([ArtistId])
REFERENCES [dbo].[Artist] ([ArtistId])
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [FK_Category_Artist]
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [FK_Category_Category] FOREIGN KEY([ParentCategoryId])
REFERENCES [dbo].[Category] ([CategoryId])
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [FK_Category_Category]
GO

ALTER DATABASE [ArtistSite] SET  READ_WRITE 
GO
