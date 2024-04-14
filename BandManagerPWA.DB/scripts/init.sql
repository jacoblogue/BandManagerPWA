USE [master]
GO
/****** Object:  Database [BandManagerDB]    Script Date: 4/13/2024 5:16:03 PM ******/
CREATE DATABASE [BandManagerDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BandManagerDB', FILENAME = N'/var/opt/mssql/BandManagerDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BandManagerDB_log', FILENAME = N'/var/opt/mssql/BandManagerDB.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [BandManagerDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BandManagerDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BandManagerDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BandManagerDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BandManagerDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BandManagerDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BandManagerDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [BandManagerDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [BandManagerDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BandManagerDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BandManagerDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BandManagerDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BandManagerDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BandManagerDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BandManagerDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BandManagerDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BandManagerDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [BandManagerDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BandManagerDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BandManagerDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BandManagerDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BandManagerDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BandManagerDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BandManagerDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BandManagerDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [BandManagerDB] SET  MULTI_USER 
GO
ALTER DATABASE [BandManagerDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BandManagerDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BandManagerDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BandManagerDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BandManagerDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BandManagerDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [BandManagerDB] SET QUERY_STORE = OFF
GO
USE [BandManagerDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 4/13/2024 5:16:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Artists]    Script Date: 4/13/2024 5:16:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Artists](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[UpdatedDate] [datetimeoffset](7) NULL,
 CONSTRAINT [PK_Artists] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BandManagerLogs]    Script Date: 4/13/2024 5:16:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BandManagerLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](max) NULL,
	[Level] [nvarchar](max) NULL,
	[LogDate] [datetime] NULL,
	[Exception] [nvarchar](max) NULL,
	[LogEvent] [nvarchar](max) NULL,
 CONSTRAINT [PK_BandManagerLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventGroup]    Script Date: 4/13/2024 5:16:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventGroup](
	[EventsId] [uniqueidentifier] NOT NULL,
	[GroupsId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_EventGroup] PRIMARY KEY CLUSTERED 
(
	[EventsId] ASC,
	[GroupsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Events]    Script Date: 4/13/2024 5:16:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Events](
	[Id] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Location] [nvarchar](max) NOT NULL,
	[Date] [datetimeoffset](7) NOT NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[UpdatedDate] [datetimeoffset](7) NULL,
 CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventUser]    Script Date: 4/13/2024 5:16:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventUser](
	[EventsId] [uniqueidentifier] NOT NULL,
	[UsersId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_EventUser] PRIMARY KEY CLUSTERED 
(
	[EventsId] ASC,
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Groups]    Script Date: 4/13/2024 5:16:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Groups](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[UpdatedDate] [datetimeoffset](7) NULL,
 CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GroupUser]    Script Date: 4/13/2024 5:16:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupUser](
	[GroupsId] [uniqueidentifier] NOT NULL,
	[UsersId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_GroupUser] PRIMARY KEY CLUSTERED 
(
	[GroupsId] ASC,
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SCBSongBank]    Script Date: 4/13/2024 5:16:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SCBSongBank](
	[Title] [nvarchar](50) NOT NULL,
	[Artist] [nvarchar](50) NOT NULL,
	[Key] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[IsOriginal] [bit] NULL,
	[Spotify_Link] [nvarchar](1) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Songs]    Script Date: 4/13/2024 5:16:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Songs](
	[Id] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[ArtistId] [uniqueidentifier] NOT NULL,
	[Key] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[UpdatedDate] [datetimeoffset](7) NULL,
 CONSTRAINT [PK_Songs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 4/13/2024 5:16:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](max) NULL,
	[UserName] [nvarchar](max) NULL,
	[FullName] [nvarchar](max) NULL,
	[CreatedDate] [datetimeoffset](7) NOT NULL,
	[UpdatedDate] [datetimeoffset](7) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[Artists] ([Id], [Name], [CreatedDate], [UpdatedDate]) VALUES (N'3d2cceaa-11a8-4e25-bc2e-065456a9e077', N'Eric Church', CAST(N'2024-04-10T00:26:13.1900000+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Artists] ([Id], [Name], [CreatedDate], [UpdatedDate]) VALUES (N'eacfb2e5-f253-473e-9426-158075b38c8d', N'Dierks Bentley', CAST(N'2024-04-10T00:26:13.1900000+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Artists] ([Id], [Name], [CreatedDate], [UpdatedDate]) VALUES (N'ed311ae6-e119-4b47-97ea-17fa939f2f6d', N'Band of Heathens', CAST(N'2024-04-10T00:26:13.1900000+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Artists] ([Id], [Name], [CreatedDate], [UpdatedDate]) VALUES (N'b09f5317-95c6-4699-a172-2b5997d9e32b', N'George Strait', CAST(N'2024-04-10T00:26:13.1900000+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Artists] ([Id], [Name], [CreatedDate], [UpdatedDate]) VALUES (N'bd98f0a1-0e3f-4be9-8363-4855a1cb470c', N'Jake Owen', CAST(N'2024-04-10T00:26:13.1900000+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Artists] ([Id], [Name], [CreatedDate], [UpdatedDate]) VALUES (N'3226fcd5-18a5-4cdb-bd6c-485883e38dbd', N'Chris Stapleton', CAST(N'2024-04-10T00:26:13.1900000+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Artists] ([Id], [Name], [CreatedDate], [UpdatedDate]) VALUES (N'12803d0f-e6d7-4766-9450-543ff679e44a', N'Brooks & Dunn', CAST(N'2024-04-10T00:26:13.1900000+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Artists] ([Id], [Name], [CreatedDate], [UpdatedDate]) VALUES (N'c26f6196-8fce-4923-b84c-553e2271bf8f', N'The Band', CAST(N'2024-04-10T00:26:13.1900000+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Artists] ([Id], [Name], [CreatedDate], [UpdatedDate]) VALUES (N'ef2bad63-d653-4fba-a87e-7158211eb116', N'Kenny Chesney', CAST(N'2024-04-10T00:26:13.1900000+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Artists] ([Id], [Name], [CreatedDate], [UpdatedDate]) VALUES (N'70428ccb-b2be-4f8c-8034-72f8682cff4d', N'Cody Johnson', CAST(N'2024-04-10T00:26:13.1900000+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Artists] ([Id], [Name], [CreatedDate], [UpdatedDate]) VALUES (N'de7f6554-d2fe-4bac-95c0-8ac2fae24d4c', N'Tim McGraw', CAST(N'2024-04-10T00:26:13.1900000+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Artists] ([Id], [Name], [CreatedDate], [UpdatedDate]) VALUES (N'd41cfe73-cce9-4903-8f33-95f71df32d40', N'Jason Aldean', CAST(N'2024-04-10T00:26:13.1900000+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Artists] ([Id], [Name], [CreatedDate], [UpdatedDate]) VALUES (N'857402b7-34d6-4663-b3d4-9b7890d59178', N'Luke Combs', CAST(N'2024-04-10T00:26:13.1900000+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Artists] ([Id], [Name], [CreatedDate], [UpdatedDate]) VALUES (N'813aeb47-8018-4c86-bcbf-b34fcf15d109', N'Brothers Osborne', CAST(N'2024-04-10T00:26:13.1900000+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Artists] ([Id], [Name], [CreatedDate], [UpdatedDate]) VALUES (N'2e352ef5-9b2b-41a7-b1e8-ba38a9af5176', N'David Lee Murphy', CAST(N'2024-04-10T00:26:13.1900000+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Artists] ([Id], [Name], [CreatedDate], [UpdatedDate]) VALUES (N'd1f523e5-d1c0-43ac-863b-bf915a59484a', N'Marshall Tucker Band', CAST(N'2024-04-10T00:26:13.1900000+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Artists] ([Id], [Name], [CreatedDate], [UpdatedDate]) VALUES (N'04bdb359-e821-4335-a3ef-c1140e8b3239', N'Alan Jackson', CAST(N'2024-04-10T00:26:13.1900000+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Artists] ([Id], [Name], [CreatedDate], [UpdatedDate]) VALUES (N'9881eb26-6480-45e9-aa46-ca567fd872c0', N'Randy Houser', CAST(N'2024-04-10T00:26:13.1900000+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Artists] ([Id], [Name], [CreatedDate], [UpdatedDate]) VALUES (N'8ea5f5c2-af3a-4257-a2b2-d142e608d233', N'Riley Green', CAST(N'2024-04-10T00:26:13.1900000+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Artists] ([Id], [Name], [CreatedDate], [UpdatedDate]) VALUES (N'31e2bf59-03e7-4caa-b3ad-e236c785aea2', N'Steven Cali', CAST(N'2024-04-10T00:26:13.1900000+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Artists] ([Id], [Name], [CreatedDate], [UpdatedDate]) VALUES (N'20f42245-edaa-45a0-b4fa-ed59b388533c', N'Keith Anderson', CAST(N'2024-04-10T00:26:13.1900000+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Artists] ([Id], [Name], [CreatedDate], [UpdatedDate]) VALUES (N'c462ba19-2967-4b95-a071-fd7972496b8c', N'Muscadine Bloodline', CAST(N'2024-04-10T00:26:13.1900000+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'0c411fb6-7c97-4541-8fde-07eef63977ba', N'Something Like That', N'de7f6554-d2fe-4bac-95c0-8ac2fae24d4c', N'F', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'0c7c553d-be29-4c54-9100-08e4277d8dc0', N'Over The Moon', N'31e2bf59-03e7-4caa-b3ad-e236c785aea2', N'C#', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'9b0df5a1-e311-47aa-8974-0a12ec19b2a1', N'My Kinda Party', N'd41cfe73-cce9-4903-8f33-95f71df32d40', N'B', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'eb851413-60d1-4ec8-92c6-1f6c1c4d7654', N'How Country Feels', N'9881eb26-6480-45e9-aa46-ca567fd872c0', N'B', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'bf3f9ffd-fa59-4f0f-83b5-35e453bdd846', N'Atlantic City', N'c26f6196-8fce-4923-b84c-553e2271bf8f', N'F#', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'5aaf686a-6d58-4a3e-a539-3d1be425e4c9', N'Drive (For daddy gene)', N'04bdb359-e821-4335-a3ef-c1140e8b3239', N'B', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'2de4e3db-4b5e-402a-beef-518e7d935db5', N'Carolina', N'3d2cceaa-11a8-4e25-bc2e-065456a9e077', N'B', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'3cea501a-1f57-494d-8672-538939566213', N'Can''t You See', N'd1f523e5-d1c0-43ac-863b-bf915a59484a', N'C#', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'd5c03a28-f728-453e-923f-5b16a6e8aacb', N'Same Old Song', N'8ea5f5c2-af3a-4257-a2b2-d142e608d233', N'F#', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'1b837ae1-1e3f-4b03-b287-5fb2c6d8a6e1', N'There Was This Girl', N'8ea5f5c2-af3a-4257-a2b2-d142e608d233', N'C#', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'9f46d06c-a528-45fe-a460-6a1218cb1e95', N'Porch Swing Angel', N'c462ba19-2967-4b95-a071-fd7972496b8c', N'C#', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'db0fc52c-144e-40bf-8ca8-79620c1ca18e', N'Outlaws Like Us', N'8ea5f5c2-af3a-4257-a2b2-d142e608d233', N'F#', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'a15a1f8d-3f7f-40f4-a364-79dc3e9d51f9', N'She Got The Best Of Me', N'857402b7-34d6-4663-b3d4-9b7890d59178', N'Eb', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'99fcb1d3-0573-40ab-83bf-7afff7e4166d', N'Pickin'' Wildflowers', N'20f42245-edaa-45a0-b4fa-ed59b388533c', NULL, CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'1d5e68d0-41fe-4745-809c-8c1c221cce66', N'Right Thing', N'31e2bf59-03e7-4caa-b3ad-e236c785aea2', NULL, CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'94ce993d-182a-49ee-b6e4-934fe5483fec', N'Texas Was You', N'd41cfe73-cce9-4903-8f33-95f71df32d40', N'F#', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'8d6212b5-7407-4dd2-8bd7-9a7e4dc632d9', N'All The Pretty Girls', N'ef2bad63-d653-4fba-a87e-7158211eb116', N'B', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'41191bc6-2978-4ea6-81ae-a0b5c9fb9cea', N'Red Dirt Road', N'12803d0f-e6d7-4766-9450-543ff679e44a', N'F', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'0081bede-df30-404e-b49e-b451d8823497', N'Stay A Little Longer', N'813aeb47-8018-4c86-bcbf-b34fcf15d109', N'F#', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'60009dd5-5b94-4d0f-8cea-b6c807a9ae9e', N'Check Yes Or No', N'b09f5317-95c6-4699-a172-2b5997d9e32b', N'F#', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'14387464-c3bd-4aea-b8bf-b7cea97fb708', N'I Hold On', N'eacfb2e5-f253-473e-9426-158075b38c8d', N'E', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'0076b151-8902-41d6-8a9f-bd15f70c5f84', N'Over The Moon', N'31e2bf59-03e7-4caa-b3ad-e236c785aea2', N'Db', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'822e7ba4-5ec5-4a3c-983f-be9319ca4ad4', N'Hurricane', N'ed311ae6-e119-4b47-97ea-17fa939f2f6d', N'Ab', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'af40f0c2-b88a-4002-bd11-bf3103192c54', N'Anywhere With You', N'bd98f0a1-0e3f-4be9-8363-4855a1cb470c', N'F', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'8362bdd3-9c2a-470f-94cf-c01aa656af37', N'Dust On The Bottle', N'2e352ef5-9b2b-41a7-b1e8-ba38a9af5176', N'F', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'b1da838f-3f8f-4ba6-a668-cb62003ffef1', N'White Horse', N'3226fcd5-18a5-4cdb-bd6c-485883e38dbd', NULL, CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'4968dd18-4133-412a-888b-ccaabd416710', N'Wild As You', N'70428ccb-b2be-4f8c-8034-72f8682cff4d', N'B', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'a63cf9be-d929-4877-95a5-cfd767fdcaf5', N'Nights To Burn', N'31e2bf59-03e7-4caa-b3ad-e236c785aea2', N'Ab', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'3b538d8e-0dbf-48a0-89b0-d1e0d252f1e3', N'That''s How It Starts', N'31e2bf59-03e7-4caa-b3ad-e236c785aea2', N'G', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'a5a53dde-f9c5-4f61-9798-d9b9ef36bd5d', N'Different ''round Here', N'8ea5f5c2-af3a-4257-a2b2-d142e608d233', N'C#', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'cac22ac1-d1c8-4c1a-8209-e0c213f07d83', N'Don''t Let Me Let You In', N'31e2bf59-03e7-4caa-b3ad-e236c785aea2', N'G#m', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'f7e5ceda-a550-4519-94aa-eb9235bee18f', N'When It Rains It Pours', N'857402b7-34d6-4663-b3d4-9b7890d59178', N'F#', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'c4d376e1-c6cb-4bc2-8756-f70edeb5115b', N'Til You Can''t', N'70428ccb-b2be-4f8c-8034-72f8682cff4d', N'C#', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
INSERT [dbo].[Songs] ([Id], [Title], [ArtistId], [Key], [CreatedDate], [UpdatedDate]) VALUES (N'598c9799-ab33-40f1-9078-ff1a1bf9bd3f', N'Girls & Whiskey', N'31e2bf59-03e7-4caa-b3ad-e236c785aea2', N'F#', CAST(N'2024-04-10T00:34:00.9433333+00:00' AS DateTimeOffset), NULL)
GO
ALTER TABLE [dbo].[Artists] ADD  CONSTRAINT [DF_Artists_CreatedDate]  DEFAULT (getutcdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Songs] ADD  CONSTRAINT [DF_Songs_CreatedDate]  DEFAULT (getutcdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Songs]  WITH CHECK ADD  CONSTRAINT [FK_Songs_Artists_ArtistId] FOREIGN KEY([ArtistId])
REFERENCES [dbo].[Artists] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Songs] CHECK CONSTRAINT [FK_Songs_Artists_ArtistId]
GO
