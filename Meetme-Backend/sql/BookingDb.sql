USE [master]
GO
/****** Object:  Database [BookingDb]    Script Date: 8/25/2023 2:02:44 PM ******/
CREATE DATABASE [BookingDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BookingDb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\BookingDb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BookingDb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\BookingDb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [BookingDb] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BookingDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BookingDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BookingDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BookingDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BookingDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BookingDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [BookingDb] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [BookingDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BookingDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BookingDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BookingDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BookingDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BookingDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BookingDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BookingDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BookingDb] SET  ENABLE_BROKER 
GO
ALTER DATABASE [BookingDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BookingDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BookingDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BookingDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BookingDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BookingDb] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [BookingDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BookingDb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [BookingDb] SET  MULTI_USER 
GO
ALTER DATABASE [BookingDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BookingDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BookingDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BookingDb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BookingDb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BookingDb] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [BookingDb] SET QUERY_STORE = OFF
GO
USE [BookingDb]
GO
/****** Object:  Table [dbo].[Appointment]    Script Date: 8/25/2023 2:02:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appointment](
	[Id] [uniqueidentifier] NOT NULL,
	[EventTypeId] [uniqueidentifier] NOT NULL,
	[InviteeName] [varchar](50) NOT NULL,
	[InviteeEmail] [varchar](255) NOT NULL,
	[GuestEmails] [varchar](255) NULL,
	[StartTimeUTC] [datetime] NOT NULL,
	[EndTimeUTC] [datetime] NOT NULL,
	[Status] [varchar](30) NOT NULL,
	[Note] [varchar](max) NULL,
 CONSTRAINT [PK_CalendarAppointment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Availability]    Script Date: 8/25/2023 2:02:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Availability](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[TimeZone] [varchar](255) NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_ScheduleRules] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AvailabilityDetail]    Script Date: 8/25/2023 2:02:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AvailabilityDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AvailabilityId] [uniqueidentifier] NOT NULL,
	[DayType] [nvarchar](max) NULL,
	[Value] [nvarchar](max) NULL,
	[StepId] [smallint] NOT NULL,
	[From] [float] NOT NULL,
	[To] [float] NOT NULL,
 CONSTRAINT [PK_ScheduleRuleAttributes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventType]    Script Date: 8/25/2023 2:02:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventType](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[Location] [nvarchar](max) NULL,
	[Slug] [nvarchar](max) NOT NULL,
	[EventColor] [nvarchar](max) NOT NULL,
	[ActiveYN] [bit] NOT NULL,
	[TimeZone] [varchar](255) NULL,
	[AvailabilityId] [uniqueidentifier] NULL,
	[DateForwardKind] [nvarchar](max) NOT NULL,
	[ForwardDuration] [int] NULL,
	[DateFrom] [datetime2](7) NULL,
	[DateTo] [datetime2](7) NULL,
	[Duration] [int] NOT NULL,
	[BufferTimeBefore] [int] NOT NULL,
	[BufferTimeAfter] [int] NOT NULL,
	[CustomAvailability] [varchar](max) NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedBy] [uniqueidentifier] NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_EventTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventTypeAvailabilityDetail]    Script Date: 8/25/2023 2:02:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventTypeAvailabilityDetail](
	[Id] [uniqueidentifier] NOT NULL,
	[EventTypeId] [uniqueidentifier] NULL,
	[DayType] [nvarchar](max) NOT NULL,
	[Value] [nvarchar](max) NULL,
	[StepId] [smallint] NOT NULL,
	[From] [float] NOT NULL,
	[To] [float] NOT NULL,
 CONSTRAINT [PK_EventTypeAvailabilityDetail_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventTypeQuestion]    Script Date: 8/25/2023 2:02:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventTypeQuestion](
	[Id] [uniqueidentifier] NOT NULL,
	[EventTypeId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[QuestionType] [varchar](50) NOT NULL,
	[Options] [varchar](max) NULL,
	[OtherOptionYN] [bit] NULL,
	[RequiredYN] [bit] NOT NULL,
	[ActiveYN] [bit] NOT NULL,
	[DisplayOrder] [smallint] NOT NULL,
	[SystemDefinedYN] [bit] NOT NULL,
 CONSTRAINT [PK_EventTypeQuestion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TimeZoneData]    Script Date: 8/25/2023 2:02:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TimeZoneData](
	[Id] [varchar](255) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Offset] [varchar](50) NOT NULL,
	[OffsetMinutes] [int] NOT NULL,
 CONSTRAINT [PK_TimeZoneData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 8/25/2023 2:02:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[UserID] [varchar](30) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[BaseURI] [varchar](100) NOT NULL,
	[timeZone] [varchar](max) NOT NULL,
	[UserName] [varchar](255) NOT NULL,
	[WelcomeText] [varchar](max) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[Appointment] ([Id], [EventTypeId], [InviteeName], [InviteeEmail], [GuestEmails], [StartTimeUTC], [EndTimeUTC], [Status], [Note]) VALUES (N'6e1658cf-d800-45aa-96a9-4c38edb457a6', N'481f6333-52ca-4d8d-84fe-4b8c2e48dc0e', N'Shahinur', N'shahin@yahoo.com', N'', CAST(N'2023-07-24T03:00:00.000' AS DateTime), CAST(N'2023-07-24T03:30:00.000' AS DateTime), N'active', N'')
GO
INSERT [dbo].[Appointment] ([Id], [EventTypeId], [InviteeName], [InviteeEmail], [GuestEmails], [StartTimeUTC], [EndTimeUTC], [Status], [Note]) VALUES (N'e5821c9e-4222-4052-85a2-d0c933d137d3', N'481f6333-52ca-4d8d-84fe-4b8c2e48dc0e', N'shahinur2', N'test@email.com', N'', CAST(N'2023-07-24T06:30:00.000' AS DateTime), CAST(N'2023-07-24T07:00:00.000' AS DateTime), N'active', N'test appointment ')
GO
INSERT [dbo].[Appointment] ([Id], [EventTypeId], [InviteeName], [InviteeEmail], [GuestEmails], [StartTimeUTC], [EndTimeUTC], [Status], [Note]) VALUES (N'dfaca9da-b9e9-42a5-b102-fb7a67d23678', N'dc423f5f-45f7-4aa8-8297-0ff38fd93103', N'Shahin', N'shahin@yahoo.ocm', N'', CAST(N'2023-08-21T05:00:00.000' AS DateTime), CAST(N'2023-08-21T05:30:00.000' AS DateTime), N'active', N'notes')
GO
INSERT [dbo].[Availability] ([Id], [Name], [OwnerId], [TimeZone], [IsDefault], [IsDeleted]) VALUES (N'6d828e9f-f34a-49ae-83b2-207dcac36a7f', N'ABC Schedule [ Clone ]', N'aab1a433-ad84-4e37-b370-3c0f162524e1', N'Asia/Dhaka', 0, 1)
GO
INSERT [dbo].[Availability] ([Id], [Name], [OwnerId], [TimeZone], [IsDefault], [IsDeleted]) VALUES (N'4ce4d961-3d41-4e48-9adc-2446b00a3c28', N'XYZ Schedule', N'aab1a433-ad84-4e37-b370-3c0f162524e1', N'Asia/Dhaka', 0, 0)
GO
INSERT [dbo].[Availability] ([Id], [Name], [OwnerId], [TimeZone], [IsDefault], [IsDeleted]) VALUES (N'a23c46ff-ed36-405a-b22d-359692925dff', N'abc', N'aab1a433-ad84-4e37-b370-3c0f162524e1', N'Asia/Dhaka', 0, 1)
GO
INSERT [dbo].[Availability] ([Id], [Name], [OwnerId], [TimeZone], [IsDefault], [IsDeleted]) VALUES (N'aa5159a3-ce64-465c-9a6b-3d5190ad78c5', N'ABC Schedule [ Clone ]', N'aab1a433-ad84-4e37-b370-3c0f162524e1', N'Asia/Dhaka', 0, 1)
GO
INSERT [dbo].[Availability] ([Id], [Name], [OwnerId], [TimeZone], [IsDefault], [IsDeleted]) VALUES (N'08ac6c80-1a70-471a-9f56-60a1b9547b39', N'ABC Schedule', N'aab1a433-ad84-4e37-b370-3c0f162524e1', N'Asia/Dhaka', 1, 0)
GO
INSERT [dbo].[Availability] ([Id], [Name], [OwnerId], [TimeZone], [IsDefault], [IsDeleted]) VALUES (N'bdb6fbd8-63d3-4bbd-844c-93aab2accb1c', N'bbb', N'aab1a433-ad84-4e37-b370-3c0f162524e1', N'Asia/Dhaka', 0, 1)
GO
INSERT [dbo].[Availability] ([Id], [Name], [OwnerId], [TimeZone], [IsDefault], [IsDeleted]) VALUES (N'0bfe3c37-dec0-4a74-a9af-94a7eb47eea5', N'bb', N'aab1a433-ad84-4e37-b370-3c0f162524e1', N'Asia/Dhaka', 0, 1)
GO
SET IDENTITY_INSERT [dbo].[AvailabilityDetail] ON 
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3886, N'6d828e9f-f34a-49ae-83b2-207dcac36a7f', N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3887, N'6d828e9f-f34a-49ae-83b2-207dcac36a7f', N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3888, N'6d828e9f-f34a-49ae-83b2-207dcac36a7f', N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3889, N'6d828e9f-f34a-49ae-83b2-207dcac36a7f', N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3890, N'6d828e9f-f34a-49ae-83b2-207dcac36a7f', N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3891, N'6d828e9f-f34a-49ae-83b2-207dcac36a7f', N'date', N'2023-Jul-15', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3892, N'6d828e9f-f34a-49ae-83b2-207dcac36a7f', N'date', N'2023-Jul-15', 0, 1080, 1140)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3893, N'6d828e9f-f34a-49ae-83b2-207dcac36a7f', N'date', N'2023-Jul-15', 0, 1200, 1260)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3894, N'6d828e9f-f34a-49ae-83b2-207dcac36a7f', N'date', N'2023-Jul-21', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3917, N'08ac6c80-1a70-471a-9f56-60a1b9547b39', N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3918, N'08ac6c80-1a70-471a-9f56-60a1b9547b39', N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3919, N'08ac6c80-1a70-471a-9f56-60a1b9547b39', N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3920, N'08ac6c80-1a70-471a-9f56-60a1b9547b39', N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3921, N'08ac6c80-1a70-471a-9f56-60a1b9547b39', N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3922, N'08ac6c80-1a70-471a-9f56-60a1b9547b39', N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3923, N'08ac6c80-1a70-471a-9f56-60a1b9547b39', N'weekday', N'Saturday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3924, N'08ac6c80-1a70-471a-9f56-60a1b9547b39', N'date', N'2023-Jul-15', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3925, N'08ac6c80-1a70-471a-9f56-60a1b9547b39', N'date', N'2023-Jul-15', 0, 1080, 1140)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3926, N'08ac6c80-1a70-471a-9f56-60a1b9547b39', N'date', N'2023-Jul-15', 0, 1200, 1260)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3927, N'08ac6c80-1a70-471a-9f56-60a1b9547b39', N'date', N'2023-Jul-21', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3942, N'aa5159a3-ce64-465c-9a6b-3d5190ad78c5', N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3943, N'aa5159a3-ce64-465c-9a6b-3d5190ad78c5', N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3944, N'aa5159a3-ce64-465c-9a6b-3d5190ad78c5', N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3945, N'aa5159a3-ce64-465c-9a6b-3d5190ad78c5', N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3946, N'aa5159a3-ce64-465c-9a6b-3d5190ad78c5', N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3947, N'aa5159a3-ce64-465c-9a6b-3d5190ad78c5', N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3948, N'aa5159a3-ce64-465c-9a6b-3d5190ad78c5', N'weekday', N'Saturday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3949, N'aa5159a3-ce64-465c-9a6b-3d5190ad78c5', N'date', N'2023-Jul-15', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3950, N'aa5159a3-ce64-465c-9a6b-3d5190ad78c5', N'date', N'2023-Jul-15', 0, 1080, 1140)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3951, N'aa5159a3-ce64-465c-9a6b-3d5190ad78c5', N'date', N'2023-Jul-15', 0, 1200, 1260)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3952, N'aa5159a3-ce64-465c-9a6b-3d5190ad78c5', N'date', N'2023-Jul-21', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3953, N'bdb6fbd8-63d3-4bbd-844c-93aab2accb1c', N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3954, N'bdb6fbd8-63d3-4bbd-844c-93aab2accb1c', N'weekday', N'Monday', 1, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3955, N'bdb6fbd8-63d3-4bbd-844c-93aab2accb1c', N'weekday', N'Tuesday', 2, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3956, N'bdb6fbd8-63d3-4bbd-844c-93aab2accb1c', N'weekday', N'Wednesday', 3, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3957, N'bdb6fbd8-63d3-4bbd-844c-93aab2accb1c', N'weekday', N'Thursday', 4, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3958, N'bdb6fbd8-63d3-4bbd-844c-93aab2accb1c', N'weekday', N'Friday', 5, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3959, N'bdb6fbd8-63d3-4bbd-844c-93aab2accb1c', N'weekday', N'Saturday', 6, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3960, N'0bfe3c37-dec0-4a74-a9af-94a7eb47eea5', N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3961, N'0bfe3c37-dec0-4a74-a9af-94a7eb47eea5', N'weekday', N'Monday', 1, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3962, N'0bfe3c37-dec0-4a74-a9af-94a7eb47eea5', N'weekday', N'Tuesday', 2, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3963, N'0bfe3c37-dec0-4a74-a9af-94a7eb47eea5', N'weekday', N'Wednesday', 3, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3964, N'0bfe3c37-dec0-4a74-a9af-94a7eb47eea5', N'weekday', N'Thursday', 4, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3965, N'0bfe3c37-dec0-4a74-a9af-94a7eb47eea5', N'weekday', N'Friday', 5, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3966, N'0bfe3c37-dec0-4a74-a9af-94a7eb47eea5', N'weekday', N'Saturday', 6, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3967, N'a23c46ff-ed36-405a-b22d-359692925dff', N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3968, N'a23c46ff-ed36-405a-b22d-359692925dff', N'weekday', N'Monday', 1, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3969, N'a23c46ff-ed36-405a-b22d-359692925dff', N'weekday', N'Tuesday', 2, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3970, N'a23c46ff-ed36-405a-b22d-359692925dff', N'weekday', N'Wednesday', 3, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3971, N'a23c46ff-ed36-405a-b22d-359692925dff', N'weekday', N'Thursday', 4, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3972, N'a23c46ff-ed36-405a-b22d-359692925dff', N'weekday', N'Friday', 5, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3973, N'a23c46ff-ed36-405a-b22d-359692925dff', N'weekday', N'Saturday', 6, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3995, N'4ce4d961-3d41-4e48-9adc-2446b00a3c28', N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3996, N'4ce4d961-3d41-4e48-9adc-2446b00a3c28', N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3997, N'4ce4d961-3d41-4e48-9adc-2446b00a3c28', N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3998, N'4ce4d961-3d41-4e48-9adc-2446b00a3c28', N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (3999, N'4ce4d961-3d41-4e48-9adc-2446b00a3c28', N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (4000, N'4ce4d961-3d41-4e48-9adc-2446b00a3c28', N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[AvailabilityDetail] ([Id], [AvailabilityId], [DayType], [Value], [StepId], [From], [To]) VALUES (4001, N'4ce4d961-3d41-4e48-9adc-2446b00a3c28', N'weekday', N'Saturday', 0, 540, 1020)
GO
SET IDENTITY_INSERT [dbo].[AvailabilityDetail] OFF
GO
INSERT [dbo].[EventType] ([Id], [Name], [Description], [OwnerId], [Location], [Slug], [EventColor], [ActiveYN], [TimeZone], [AvailabilityId], [DateForwardKind], [ForwardDuration], [DateFrom], [DateTo], [Duration], [BufferTimeBefore], [BufferTimeAfter], [CustomAvailability], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (N'dc423f5f-45f7-4aa8-8297-0ff38fd93103', N'EF Core Presentation 2023 (clone)', N'There will be a session held on 5th July at Office. This session will be conducted by industry renowned expert who has experience more than a decade. ', N'aab1a433-ad84-4e37-b370-3c0f162524e1', NULL, N'EFCore-2023-clone', N'orange', 1, N'Asia/Dhaka', N'08ac6c80-1a70-471a-9f56-60a1b9547b39', N'moving', 43200, NULL, NULL, 30, 0, 0, NULL, N'aab1a433-ad84-4e37-b370-3c0f162524e1', CAST(N'2023-07-08T00:28:01.2748755' AS DateTime2), NULL, CAST(N'2023-07-07T18:28:01.2752505' AS DateTime2), 0)
GO
INSERT [dbo].[EventType] ([Id], [Name], [Description], [OwnerId], [Location], [Slug], [EventColor], [ActiveYN], [TimeZone], [AvailabilityId], [DateForwardKind], [ForwardDuration], [DateFrom], [DateTo], [Duration], [BufferTimeBefore], [BufferTimeAfter], [CustomAvailability], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (N'4755c2d3-c5c2-423c-9811-3695fbda88ab', N'EF Core Presentation 2023 (clone)', N'There will be a session held on 5th July at Office. This session will be conducted by industry renowned expert who has experience more than a decade. ', N'aab1a433-ad84-4e37-b370-3c0f162524e1', NULL, N'EFCore-2023-clone', N'orange', 0, N'Asia/Dhaka', NULL, N'moving', 86400, NULL, NULL, 30, 0, 0, NULL, N'aab1a433-ad84-4e37-b370-3c0f162524e1', CAST(N'2023-07-03T00:36:51.2231209' AS DateTime2), NULL, CAST(N'2023-07-02T18:36:51.2234694' AS DateTime2), 1)
GO
INSERT [dbo].[EventType] ([Id], [Name], [Description], [OwnerId], [Location], [Slug], [EventColor], [ActiveYN], [TimeZone], [AvailabilityId], [DateForwardKind], [ForwardDuration], [DateFrom], [DateTo], [Duration], [BufferTimeBefore], [BufferTimeAfter], [CustomAvailability], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (N'481f6333-52ca-4d8d-84fe-4b8c2e48dc0e', N'EF Core Presentation 2023', N'There will be a session held on 5th July at Office. This session will be conducted by industry renowned expert who has experience more than a decade. ', N'aab1a433-ad84-4e37-b370-3c0f162524e1', NULL, N'EFCore-2023', N'purple', 1, N'Asia/Dhaka', NULL, N'moving', 43200, NULL, NULL, 30, 0, 0, NULL, N'aab1a433-ad84-4e37-b370-3c0f162524e1', CAST(N'2023-07-02T18:30:48.0678265' AS DateTime2), NULL, NULL, 0)
GO
INSERT [dbo].[EventType] ([Id], [Name], [Description], [OwnerId], [Location], [Slug], [EventColor], [ActiveYN], [TimeZone], [AvailabilityId], [DateForwardKind], [ForwardDuration], [DateFrom], [DateTo], [Duration], [BufferTimeBefore], [BufferTimeAfter], [CustomAvailability], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (N'c36fab43-6809-4c49-bb55-7efce3a77437', N'EF Core Presentation 2023 (clone)', N'There will be a session held on 5th July at Office. This session will be conducted by industry renowned expert who has experience more than a decade. ', N'aab1a433-ad84-4e37-b370-3c0f162524e1', NULL, N'EFCore-2023-clone-1', N'orange', 1, N'Asia/Dhaka', N'08ac6c80-1a70-471a-9f56-60a1b9547b39', N'moving', 43200, NULL, NULL, 30, 0, 0, NULL, N'aab1a433-ad84-4e37-b370-3c0f162524e1', CAST(N'2023-07-04T21:03:38.9419660' AS DateTime2), NULL, CAST(N'2023-07-04T15:03:38.9419673' AS DateTime2), 1)
GO
INSERT [dbo].[EventType] ([Id], [Name], [Description], [OwnerId], [Location], [Slug], [EventColor], [ActiveYN], [TimeZone], [AvailabilityId], [DateForwardKind], [ForwardDuration], [DateFrom], [DateTo], [Duration], [BufferTimeBefore], [BufferTimeAfter], [CustomAvailability], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (N'9f5239de-ffe9-49a0-923b-88e91f109221', N'EF Core Presentation 2023 (clone) (clone) (clone)', N'There will be a session held on 5th July at Office. This session will be conducted by industry renowned expert who has experience more than a decade. ', N'aab1a433-ad84-4e37-b370-3c0f162524e1', NULL, N'EFCore-2023-clone-clone-clone', N'orange', 1, N'Asia/Dhaka', N'08ac6c80-1a70-471a-9f56-60a1b9547b39', N'moving', 43200, NULL, NULL, 30, 0, 0, NULL, N'aab1a433-ad84-4e37-b370-3c0f162524e1', CAST(N'2023-07-04T21:03:19.1397558' AS DateTime2), NULL, CAST(N'2023-07-04T15:03:19.1397744' AS DateTime2), 1)
GO
INSERT [dbo].[EventType] ([Id], [Name], [Description], [OwnerId], [Location], [Slug], [EventColor], [ActiveYN], [TimeZone], [AvailabilityId], [DateForwardKind], [ForwardDuration], [DateFrom], [DateTo], [Duration], [BufferTimeBefore], [BufferTimeAfter], [CustomAvailability], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (N'27b92d02-40f3-4a94-af18-b932ec358412', N'EF Core Presentation 2023 (clone)', N'There will be a session held on 5th July at Office. This session will be conducted by industry renowned expert who has experience more than a decade. ', N'aab1a433-ad84-4e37-b370-3c0f162524e1', NULL, N'EFCore-2023-clone', N'orange', 1, N'Asia/Dhaka', N'08ac6c80-1a70-471a-9f56-60a1b9547b39', N'moving', 43200, NULL, NULL, 30, 0, 0, NULL, N'aab1a433-ad84-4e37-b370-3c0f162524e1', CAST(N'2023-07-04T15:34:24.1666096' AS DateTime2), NULL, CAST(N'2023-07-04T09:34:24.1668682' AS DateTime2), 1)
GO
INSERT [dbo].[EventType] ([Id], [Name], [Description], [OwnerId], [Location], [Slug], [EventColor], [ActiveYN], [TimeZone], [AvailabilityId], [DateForwardKind], [ForwardDuration], [DateFrom], [DateTo], [Duration], [BufferTimeBefore], [BufferTimeAfter], [CustomAvailability], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (N'38de7907-33ad-41e7-943a-bb8e7279ff43', N'EF Core Presentation 2023 (clone) (clone)', N'There will be a session held on 5th July at Office. This session will be conducted by industry renowned expert who has experience more than a decade. ', N'aab1a433-ad84-4e37-b370-3c0f162524e1', NULL, N'EFCore-2023-clone-clone', N'orange', 1, N'Asia/Dhaka', N'08ac6c80-1a70-471a-9f56-60a1b9547b39', N'moving', 43200, NULL, NULL, 30, 0, 0, NULL, N'aab1a433-ad84-4e37-b370-3c0f162524e1', CAST(N'2023-07-04T21:03:13.9962224' AS DateTime2), NULL, CAST(N'2023-07-04T15:03:13.9965603' AS DateTime2), 1)
GO
INSERT [dbo].[EventType] ([Id], [Name], [Description], [OwnerId], [Location], [Slug], [EventColor], [ActiveYN], [TimeZone], [AvailabilityId], [DateForwardKind], [ForwardDuration], [DateFrom], [DateTo], [Duration], [BufferTimeBefore], [BufferTimeAfter], [CustomAvailability], [CreatedBy], [CreatedAt], [UpdatedBy], [UpdatedAt], [IsDeleted]) VALUES (N'7e767902-dbfb-4bed-a8dc-eb0f14c517c3', N'EF Core Presentation 2023 (clone)', N'There will be a session held on 5th July at Office. This session will be conducted by industry renowned expert who has experience more than a decade. ', N'aab1a433-ad84-4e37-b370-3c0f162524e1', NULL, N'EFCore-2023-clone-1', N'orange', 1, N'Asia/Dhaka', N'08ac6c80-1a70-471a-9f56-60a1b9547b39', N'moving', 43200, NULL, NULL, 30, 0, 0, NULL, N'aab1a433-ad84-4e37-b370-3c0f162524e1', CAST(N'2023-07-04T21:03:26.4184484' AS DateTime2), NULL, CAST(N'2023-07-04T15:03:26.4184496' AS DateTime2), 1)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'f89abf64-f26a-4a2b-a78e-0307c7f9c31e', NULL, N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'f7242c24-b2e4-4cdf-ca1f-08db7c71da81', NULL, N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'85a9ff20-f7df-4a74-ca20-08db7c71da81', NULL, N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'b1c655da-3c2f-4587-ca21-08db7c71da81', NULL, N'date', N'2023-Jul-15', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'04dfd571-ec55-4af0-ca22-08db7c71da81', NULL, N'date', N'2023-Jul-21', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'9fc93451-b13d-466d-ca23-08db7c71da81', NULL, N'date', N'2023-Jul-15', 0, 1200, 1260)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'3e95553c-51ec-4736-ca24-08db7c71da81', NULL, N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'67ee177d-b78e-445c-ca25-08db7c71da81', NULL, N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'1a7dcb4f-504e-471e-ca26-08db7c71da81', NULL, N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'edabc4e8-3faf-4fe0-ca27-08db7c71da81', NULL, N'date', N'2023-Jul-15', 0, 1080, 1140)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'1dc597d9-b027-4dfa-ca28-08db7c71da81', NULL, N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'02cf9c4e-a279-4211-ca29-08db7c71da81', NULL, N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'8a4aa54a-68fb-412b-ca2a-08db7c71da81', NULL, N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'ff118ff9-17df-45fe-ca2b-08db7c71da81', NULL, N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'047b9ce3-f8da-4f36-ca2c-08db7c71da81', NULL, N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'd1953cdc-0d98-4830-ca2d-08db7c71da81', NULL, N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'7edf6338-83cd-4b09-ca2e-08db7c71da81', NULL, N'weekday', N'Saturday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'bac08cf6-14fe-43a5-ca2f-08db7c71da81', NULL, N'date', N'2023-Jul-15', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'bee8cedf-b307-4465-ca30-08db7c71da81', NULL, N'date', N'2023-Jul-15', 0, 1080, 1140)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'd366546d-fe09-4512-ca31-08db7c71da81', NULL, N'date', N'2023-Jul-15', 0, 1200, 1260)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'9ae052d1-a1bb-47fe-ca32-08db7c71da81', NULL, N'date', N'2023-Jul-21', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'6322f32c-36d9-4f95-ca33-08db7c71da81', NULL, N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'e6394a9c-fe30-41b5-ca34-08db7c71da81', NULL, N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'014084d8-6882-4d34-ca35-08db7c71da81', NULL, N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'4a7d5bdc-ffe4-4c76-ca36-08db7c71da81', NULL, N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'8edf4d73-db18-4f6b-ca37-08db7c71da81', NULL, N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'b9eb2a65-c66c-4391-ca38-08db7c71da81', NULL, N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'00b090f5-a82f-41a6-ca39-08db7c71da81', NULL, N'weekday', N'Saturday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'052e73bd-949f-468d-ca3a-08db7c71da81', NULL, N'date', N'2023-Jul-15', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'f083b228-c1fe-4bf8-ca3b-08db7c71da81', NULL, N'date', N'2023-Jul-15', 0, 1080, 1140)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'd2ce0183-6d9d-4a7e-ca3c-08db7c71da81', NULL, N'date', N'2023-Jul-15', 0, 1200, 1260)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'c98d1ad6-f73b-4530-ca3d-08db7c71da81', NULL, N'date', N'2023-Jul-21', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'84a6124f-1dac-4bae-c516-08db7c9fca65', NULL, N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'492df81b-f786-4039-c517-08db7c9fca65', NULL, N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'9747a3cd-7397-4b3f-c518-08db7c9fca65', NULL, N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'62cf13b5-8563-41e3-c519-08db7c9fca65', NULL, N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'326628ed-703a-4ccf-c51a-08db7c9fca65', NULL, N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'a452ff16-2c4a-430a-c51b-08db7c9fca65', NULL, N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'83233b2f-579e-477f-c51c-08db7c9fca65', NULL, N'weekday', N'Saturday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'1da73ff6-8794-4481-c51d-08db7c9fca65', NULL, N'date', N'2023-Jul-15', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'45c5d00c-9d56-469b-c51e-08db7c9fca65', NULL, N'date', N'2023-Jul-15', 0, 1080, 1140)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'd07c0404-ce23-4662-c51f-08db7c9fca65', NULL, N'date', N'2023-Jul-15', 0, 1200, 1260)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'ce279436-3e28-4f70-c520-08db7c9fca65', NULL, N'date', N'2023-Jul-21', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'f5a1a303-890e-4ecf-c521-08db7c9fca65', NULL, N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'2a50ec5b-d519-4e35-c522-08db7c9fca65', NULL, N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'7e860248-955c-4838-c523-08db7c9fca65', NULL, N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'd32156d7-2ec4-464e-c524-08db7c9fca65', NULL, N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'6da8c022-5873-46ec-c525-08db7c9fca65', NULL, N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'c3056e81-8c09-4d3e-c526-08db7c9fca65', NULL, N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'7c786835-b0bf-4077-c527-08db7c9fca65', NULL, N'weekday', N'Saturday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'42c96f19-0ccf-4863-c528-08db7c9fca65', NULL, N'date', N'2023-Jul-15', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'010b3abb-ef23-47b5-c529-08db7c9fca65', NULL, N'date', N'2023-Jul-15', 0, 1080, 1140)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'5a4be0c3-b298-4b1c-c52a-08db7c9fca65', NULL, N'date', N'2023-Jul-15', 0, 1200, 1260)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'e985900f-a759-4ceb-c52b-08db7c9fca65', NULL, N'date', N'2023-Jul-21', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'a49a0c6f-8d73-4a5d-c52c-08db7c9fca65', NULL, N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'c36acb9c-f676-4c66-c52d-08db7c9fca65', NULL, N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'863deee5-c12d-497c-c52e-08db7c9fca65', NULL, N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'36fdb9c9-ae4e-4399-c52f-08db7c9fca65', NULL, N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'5af8ca1f-fc0f-4e1a-c530-08db7c9fca65', NULL, N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'a09a32f7-6bd3-4658-c531-08db7c9fca65', NULL, N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'a84ed5ce-0ec7-4652-c532-08db7c9fca65', NULL, N'weekday', N'Saturday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'bae91d29-b078-4c65-c533-08db7c9fca65', NULL, N'date', N'2023-Jul-15', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'9c1f93a0-62cf-4260-c534-08db7c9fca65', NULL, N'date', N'2023-Jul-15', 0, 1080, 1140)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'017aa7a2-e4b3-4d04-c535-08db7c9fca65', NULL, N'date', N'2023-Jul-15', 0, 1200, 1260)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'939ef029-c77d-4cc8-c536-08db7c9fca65', NULL, N'date', N'2023-Jul-21', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'c98798a8-3401-4d57-c537-08db7c9fca65', NULL, N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'6369f490-056c-4003-c538-08db7c9fca65', NULL, N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'589ab836-1abc-454e-c539-08db7c9fca65', NULL, N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'06ba43c1-3e65-4804-c53a-08db7c9fca65', NULL, N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'9b5bd397-321e-4725-c53b-08db7c9fca65', NULL, N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'f7b05a6b-7976-49a8-c53c-08db7c9fca65', NULL, N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'd2cb87ab-f763-4660-c53d-08db7c9fca65', NULL, N'weekday', N'Saturday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'e0238571-2e9d-43fc-c53e-08db7c9fca65', NULL, N'date', N'2023-Jul-15', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'073d4a24-8e05-43d3-c53f-08db7c9fca65', NULL, N'date', N'2023-Jul-15', 0, 1080, 1140)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'c37bef8e-14d1-47ff-c540-08db7c9fca65', NULL, N'date', N'2023-Jul-15', 0, 1200, 1260)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'35abd1e1-a980-42fb-c541-08db7c9fca65', NULL, N'date', N'2023-Jul-21', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'0bdf9623-fc20-4fa0-6821-08db7cb392de', NULL, N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'89ad0e9a-4c56-45b5-6822-08db7cb392de', NULL, N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'3c2a76d6-2639-424d-6823-08db7cb392de', NULL, N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'ee24aa7b-3ff0-4640-6824-08db7cb392de', NULL, N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'86270360-613f-43dd-6825-08db7cb392de', NULL, N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'6f28082b-de66-4d22-6826-08db7cb392de', NULL, N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'b8796334-a6d2-4ca6-6827-08db7cb392de', NULL, N'weekday', N'Saturday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'ff1d00d4-9b69-4b16-6828-08db7cb392de', NULL, N'date', N'2023-Jul-15', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'813ab064-fa12-498a-6829-08db7cb392de', NULL, N'date', N'2023-Jul-15', 0, 1080, 1140)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'c24f5259-bb14-47f5-682a-08db7cb392de', NULL, N'date', N'2023-Jul-15', 0, 1200, 1260)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'af38fad8-015e-4e33-682b-08db7cb392de', NULL, N'date', N'2023-Jul-21', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'e861a197-6213-46c2-682c-08db7cb392de', NULL, N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'4071244f-6ca9-46ce-682d-08db7cb392de', NULL, N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'ea46bf9b-fcfb-49a7-682e-08db7cb392de', NULL, N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'cf88744b-3283-4ed5-682f-08db7cb392de', NULL, N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'55ce456c-d429-4e4a-6830-08db7cb392de', NULL, N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'95c04f61-5dcd-4583-6831-08db7cb392de', NULL, N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'e54f6478-1cb8-4143-6832-08db7cb392de', NULL, N'weekday', N'Saturday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'1bd187ae-69a0-4270-6833-08db7cb392de', NULL, N'date', N'2023-Jul-15', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'aa04f68b-2916-4e12-6834-08db7cb392de', NULL, N'date', N'2023-Jul-15', 0, 1080, 1140)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'6ec59c49-a225-4436-6835-08db7cb392de', NULL, N'date', N'2023-Jul-15', 0, 1200, 1260)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'1ffe2c63-9dce-44d9-6836-08db7cb392de', NULL, N'date', N'2023-Jul-21', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'a745d2c6-761a-4710-6837-08db7cb392de', NULL, N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'8de37b8a-22c6-4227-6838-08db7cb392de', NULL, N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'94962bf7-d094-4547-6839-08db7cb392de', NULL, N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'c01b8fa5-e17a-49d4-683a-08db7cb392de', NULL, N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'a8cbf516-e030-41e2-683b-08db7cb392de', NULL, N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'582b9087-f29a-4a05-683c-08db7cb392de', NULL, N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'287d0fa9-5a0a-4a0a-683d-08db7cb392de', NULL, N'weekday', N'Saturday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'3b4b1ac1-2a3e-45a7-683e-08db7cb392de', NULL, N'date', N'2023-Jul-15', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'9026c427-c9f9-4d10-683f-08db7cb392de', NULL, N'date', N'2023-Jul-15', 0, 1080, 1140)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'03e14f83-2d0b-496a-6840-08db7cb392de', NULL, N'date', N'2023-Jul-15', 0, 1200, 1260)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'064db15c-18d0-4269-6841-08db7cb392de', NULL, N'date', N'2023-Jul-21', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'86881c42-1cce-4bc7-6842-08db7cb392de', NULL, N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'fc375e84-7302-436a-6843-08db7cb392de', NULL, N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'0cf9c600-5ab6-422f-6844-08db7cb392de', NULL, N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'391d3719-8979-4321-6845-08db7cb392de', NULL, N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'0c7f9f29-2c83-4673-6846-08db7cb392de', NULL, N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'dbc58ffc-dea8-4aec-6847-08db7cb392de', NULL, N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'a0e8d62e-2f00-476e-6848-08db7cb392de', NULL, N'weekday', N'Saturday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'282c7ee8-6261-4915-6849-08db7cb392de', NULL, N'date', N'2023-Jul-15', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'f128f50e-d5cc-4e61-684a-08db7cb392de', NULL, N'date', N'2023-Jul-15', 0, 1080, 1140)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'c6941756-6690-487c-684b-08db7cb392de', NULL, N'date', N'2023-Jul-15', 0, 1200, 1260)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'd5b60f1a-d9d4-44e9-684c-08db7cb392de', NULL, N'date', N'2023-Jul-21', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'2bdb6414-efb3-474c-684d-08db7cb392de', NULL, N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'4adc6ce7-8d9e-402a-684e-08db7cb392de', NULL, N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'ede2fefc-3e43-45c2-684f-08db7cb392de', NULL, N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'cad23108-962d-490b-6850-08db7cb392de', NULL, N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'80321640-de25-449a-6851-08db7cb392de', NULL, N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'61b3cbfc-a387-4550-6852-08db7cb392de', NULL, N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'54826fd3-9cd8-4f9a-6853-08db7cb392de', NULL, N'weekday', N'Saturday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'469504fd-b44a-4c25-6854-08db7cb392de', NULL, N'date', N'2023-Jul-15', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'b581924c-4ff1-4ea6-6855-08db7cb392de', NULL, N'date', N'2023-Jul-15', 0, 1080, 1140)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'5be68711-1a10-427f-6856-08db7cb392de', NULL, N'date', N'2023-Jul-15', 0, 1200, 1260)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'58d05261-ff63-4e26-6857-08db7cb392de', NULL, N'date', N'2023-Jul-21', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'8bb659ac-3dfe-4629-6858-08db7cb392de', NULL, N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'53400c6f-93d4-4494-6859-08db7cb392de', NULL, N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'4e472a8a-47af-454d-685a-08db7cb392de', NULL, N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'cfa31f3e-2311-4015-685b-08db7cb392de', NULL, N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'2783cacd-b57a-42fb-685c-08db7cb392de', NULL, N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'c216884c-f5eb-4e76-685d-08db7cb392de', NULL, N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'66116fd1-a4fa-49c6-685e-08db7cb392de', NULL, N'weekday', N'Saturday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'898588b3-1bff-4c3c-685f-08db7cb392de', NULL, N'date', N'2023-Jul-15', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'1c65bab2-7e28-4a6a-6860-08db7cb392de', NULL, N'date', N'2023-Jul-15', 0, 1080, 1140)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'422fd96f-cd17-4159-6861-08db7cb392de', NULL, N'date', N'2023-Jul-15', 0, 1200, 1260)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'b2029ca6-ce43-4683-6862-08db7cb392de', NULL, N'date', N'2023-Jul-21', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'b1fe090e-068a-43f4-f5cd-08db7e4e70c4', N'c36fab43-6809-4c49-bb55-7efce3a77437', N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'3ae39fe4-d3f3-4451-f5ce-08db7e4e70c4', N'c36fab43-6809-4c49-bb55-7efce3a77437', N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'ac3e8b86-37c5-4488-f5cf-08db7e4e70c4', N'c36fab43-6809-4c49-bb55-7efce3a77437', N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'52a49e8f-b67a-49f2-f5d0-08db7e4e70c4', N'c36fab43-6809-4c49-bb55-7efce3a77437', N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'a6662283-2fce-4672-f5d1-08db7e4e70c4', N'c36fab43-6809-4c49-bb55-7efce3a77437', N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'b3a34e0c-4976-4db4-f5d2-08db7e4e70c4', N'c36fab43-6809-4c49-bb55-7efce3a77437', N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'9a64cfbf-75fd-46eb-f5d3-08db7e4e70c4', N'c36fab43-6809-4c49-bb55-7efce3a77437', N'weekday', N'Saturday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'8a526c3e-d09f-4d33-f5d4-08db7e4e70c4', N'c36fab43-6809-4c49-bb55-7efce3a77437', N'date', N'2023-Jul-15', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'8889d94d-0518-46f5-f5d5-08db7e4e70c4', N'c36fab43-6809-4c49-bb55-7efce3a77437', N'date', N'2023-Jul-15', 0, 1080, 1140)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'be68d8a4-e850-464b-f5d6-08db7e4e70c4', N'c36fab43-6809-4c49-bb55-7efce3a77437', N'date', N'2023-Jul-15', 0, 1200, 1260)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'5df9943f-f7af-4114-f5d7-08db7e4e70c4', N'c36fab43-6809-4c49-bb55-7efce3a77437', N'date', N'2023-Jul-21', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'62ed539f-1ffc-4a21-f5d8-08db7e4e70c4', N'9f5239de-ffe9-49a0-923b-88e91f109221', N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'82f2e024-69da-47c1-f5d9-08db7e4e70c4', N'9f5239de-ffe9-49a0-923b-88e91f109221', N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'3781e50d-7f69-49fa-f5da-08db7e4e70c4', N'9f5239de-ffe9-49a0-923b-88e91f109221', N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'70329ae5-48e4-408c-f5db-08db7e4e70c4', N'9f5239de-ffe9-49a0-923b-88e91f109221', N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'23029245-74a5-400c-f5dc-08db7e4e70c4', N'9f5239de-ffe9-49a0-923b-88e91f109221', N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'6d97adb0-ecdc-4287-f5dd-08db7e4e70c4', N'9f5239de-ffe9-49a0-923b-88e91f109221', N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'a4758fa0-0f90-4078-f5de-08db7e4e70c4', N'9f5239de-ffe9-49a0-923b-88e91f109221', N'weekday', N'Saturday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'ec814f47-686c-4ce7-f5df-08db7e4e70c4', N'9f5239de-ffe9-49a0-923b-88e91f109221', N'date', N'2023-Jul-15', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'38e51644-7325-43f2-f5e0-08db7e4e70c4', N'9f5239de-ffe9-49a0-923b-88e91f109221', N'date', N'2023-Jul-15', 0, 1080, 1140)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'aadecf5e-d42d-40c8-f5e1-08db7e4e70c4', N'9f5239de-ffe9-49a0-923b-88e91f109221', N'date', N'2023-Jul-15', 0, 1200, 1260)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'b1c3e1ed-96c1-4c7d-f5e2-08db7e4e70c4', N'9f5239de-ffe9-49a0-923b-88e91f109221', N'date', N'2023-Jul-21', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'0d0fcc32-917a-4528-f5e3-08db7e4e70c4', N'27b92d02-40f3-4a94-af18-b932ec358412', N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'260a8eb8-def2-4061-f5e4-08db7e4e70c4', N'27b92d02-40f3-4a94-af18-b932ec358412', N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'ece4e812-fb4b-40e3-f5e5-08db7e4e70c4', N'27b92d02-40f3-4a94-af18-b932ec358412', N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'1e44bbf3-699c-449e-f5e6-08db7e4e70c4', N'27b92d02-40f3-4a94-af18-b932ec358412', N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'0168dcb1-9ad5-4e31-f5e7-08db7e4e70c4', N'27b92d02-40f3-4a94-af18-b932ec358412', N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'4940b6bd-b51d-4de1-f5e8-08db7e4e70c4', N'27b92d02-40f3-4a94-af18-b932ec358412', N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'58553e75-8d3d-4727-f5e9-08db7e4e70c4', N'27b92d02-40f3-4a94-af18-b932ec358412', N'weekday', N'Saturday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'f0312aab-27a1-4a5f-f5ea-08db7e4e70c4', N'27b92d02-40f3-4a94-af18-b932ec358412', N'date', N'2023-Jul-15', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'b0a99f7f-aed2-4512-f5eb-08db7e4e70c4', N'27b92d02-40f3-4a94-af18-b932ec358412', N'date', N'2023-Jul-15', 0, 1080, 1140)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'efd90dc6-4099-423d-f5ec-08db7e4e70c4', N'27b92d02-40f3-4a94-af18-b932ec358412', N'date', N'2023-Jul-15', 0, 1200, 1260)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'c9c0792c-cc9b-4ef4-f5ed-08db7e4e70c4', N'27b92d02-40f3-4a94-af18-b932ec358412', N'date', N'2023-Jul-21', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'86e00808-3e75-47e8-f5ee-08db7e4e70c4', N'38de7907-33ad-41e7-943a-bb8e7279ff43', N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'83f6b55d-22d9-4d4b-f5ef-08db7e4e70c4', N'38de7907-33ad-41e7-943a-bb8e7279ff43', N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'873ae792-3a12-467f-f5f0-08db7e4e70c4', N'38de7907-33ad-41e7-943a-bb8e7279ff43', N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'430668e5-56c9-4636-f5f1-08db7e4e70c4', N'38de7907-33ad-41e7-943a-bb8e7279ff43', N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'39c68d99-2cc6-4680-f5f2-08db7e4e70c4', N'38de7907-33ad-41e7-943a-bb8e7279ff43', N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'4d47a4f2-5045-41cd-f5f3-08db7e4e70c4', N'38de7907-33ad-41e7-943a-bb8e7279ff43', N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'9a04b73d-c225-4f88-f5f4-08db7e4e70c4', N'38de7907-33ad-41e7-943a-bb8e7279ff43', N'weekday', N'Saturday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'e38065ea-1d67-46a2-f5f5-08db7e4e70c4', N'38de7907-33ad-41e7-943a-bb8e7279ff43', N'date', N'2023-Jul-15', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'c376fa24-6d3f-4ab5-f5f6-08db7e4e70c4', N'38de7907-33ad-41e7-943a-bb8e7279ff43', N'date', N'2023-Jul-15', 0, 1080, 1140)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'697fd191-89c3-4ea4-f5f7-08db7e4e70c4', N'38de7907-33ad-41e7-943a-bb8e7279ff43', N'date', N'2023-Jul-15', 0, 1200, 1260)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'b75062d4-eadf-47e6-f5f8-08db7e4e70c4', N'38de7907-33ad-41e7-943a-bb8e7279ff43', N'date', N'2023-Jul-21', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'96874ef6-1c82-43d4-f5f9-08db7e4e70c4', N'7e767902-dbfb-4bed-a8dc-eb0f14c517c3', N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'65221a27-bc9d-46e4-f5fa-08db7e4e70c4', N'7e767902-dbfb-4bed-a8dc-eb0f14c517c3', N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'06626fb0-4035-4a65-f5fb-08db7e4e70c4', N'7e767902-dbfb-4bed-a8dc-eb0f14c517c3', N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'12a4899b-d612-44bb-f5fc-08db7e4e70c4', N'7e767902-dbfb-4bed-a8dc-eb0f14c517c3', N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'2f63fc58-67af-4e23-f5fd-08db7e4e70c4', N'7e767902-dbfb-4bed-a8dc-eb0f14c517c3', N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'c291f270-0493-48d9-f5fe-08db7e4e70c4', N'7e767902-dbfb-4bed-a8dc-eb0f14c517c3', N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'46b235fb-65c7-4d20-f5ff-08db7e4e70c4', N'7e767902-dbfb-4bed-a8dc-eb0f14c517c3', N'weekday', N'Saturday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'70cb759e-9165-4f8f-f600-08db7e4e70c4', N'7e767902-dbfb-4bed-a8dc-eb0f14c517c3', N'date', N'2023-Jul-15', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'1096f408-511f-4362-f601-08db7e4e70c4', N'7e767902-dbfb-4bed-a8dc-eb0f14c517c3', N'date', N'2023-Jul-15', 0, 1080, 1140)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'43b77533-a5e8-4fd2-f602-08db7e4e70c4', N'7e767902-dbfb-4bed-a8dc-eb0f14c517c3', N'date', N'2023-Jul-15', 0, 1200, 1260)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'7add62b7-29e1-4a1e-f603-08db7e4e70c4', N'7e767902-dbfb-4bed-a8dc-eb0f14c517c3', N'date', N'2023-Jul-21', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'9c4cf39b-e176-4fe3-a4c2-1acbe1a4553b', NULL, N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'8cc56074-5f20-4e6f-b02e-2976cd6ee6ce', N'dc423f5f-45f7-4aa8-8297-0ff38fd93103', N'weekday', N'Saturday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'6fca9b5f-a405-4992-ad05-2c3acf8c2c26', N'dc423f5f-45f7-4aa8-8297-0ff38fd93103', N'date', N'2023-Jul-21', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'6c96d4fd-b9fe-4966-b0a7-308a7c32c852', N'481f6333-52ca-4d8d-84fe-4b8c2e48dc0e', N'date', N'2023-Aug-24', 0, 540, 1410)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'5e1d3903-f4a9-4453-9220-35e99ca1f6ba', N'dc423f5f-45f7-4aa8-8297-0ff38fd93103', N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'aec32823-0997-4fe4-bac9-3db8fa315771', N'4755c2d3-c5c2-423c-9811-3695fbda88ab', N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'd4c1f79d-d6e2-43eb-960a-3dfb86fa62ec', N'dc423f5f-45f7-4aa8-8297-0ff38fd93103', N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'd7dd47d9-7645-4e58-bb23-3efbc2cdaa52', N'4755c2d3-c5c2-423c-9811-3695fbda88ab', N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'f0e99f33-a744-47bf-826e-4ae3d529f409', N'481f6333-52ca-4d8d-84fe-4b8c2e48dc0e', N'date', N'2023-Jul-15', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'3073d1aa-3b98-46d9-85d7-56b2d8263ceb', NULL, N'date', N'2023-Jul-15', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'd7ee57fd-e95c-4b2b-a0a8-79603c7aeb48', NULL, N'date', N'2023-Jul-21', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'f95de2a6-f25a-4f8f-9647-94b66c832495', N'dc423f5f-45f7-4aa8-8297-0ff38fd93103', N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'707cc542-b35b-4e8d-8b3c-98143e7d220a', N'4755c2d3-c5c2-423c-9811-3695fbda88ab', N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'6f48527f-7f95-4e6f-8b22-9964469fcf3f', NULL, N'date', N'2023-Jul-15', 0, 1200, 1260)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'078e2e02-3e00-4bb2-8e87-9a73483cdfea', N'dc423f5f-45f7-4aa8-8297-0ff38fd93103', N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'8cb4b414-fed5-4ecf-89c7-a0f6700a29c0', NULL, N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'4c6ea34d-f864-4738-bfbe-a2116b52df55', N'481f6333-52ca-4d8d-84fe-4b8c2e48dc0e', N'date', N'2023-Jul-15', 0, 1200, 1260)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'b5da5d72-24e3-42a0-8f2e-a2abc7339bea', N'481f6333-52ca-4d8d-84fe-4b8c2e48dc0e', N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'8f3e2dbd-25f4-437e-80bb-aba688e0dece', N'dc423f5f-45f7-4aa8-8297-0ff38fd93103', N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'3b28fb55-0100-403f-acc3-b284919092a6', NULL, N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'3fde0a8e-aafe-4931-9af5-b7d7c87e5b8c', N'4755c2d3-c5c2-423c-9811-3695fbda88ab', N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'66443738-a0cf-4067-b97e-b88fe1df57ff', N'dc423f5f-45f7-4aa8-8297-0ff38fd93103', N'date', N'2023-Jul-15', 0, 1200, 1260)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'c54df86d-b3ef-48e4-8f5c-bcbb489e0120', N'481f6333-52ca-4d8d-84fe-4b8c2e48dc0e', N'date', N'2023-Jul-21', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'4e700ee5-7ac9-477a-9db8-c011d395bf1b', NULL, N'weekday', N'Monday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'df2a4578-354e-4966-be98-c022b09a67d3', N'dc423f5f-45f7-4aa8-8297-0ff38fd93103', N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'9d69197f-5522-4e5f-867d-c255909e38ca', N'481f6333-52ca-4d8d-84fe-4b8c2e48dc0e', N'weekday', N'Wednesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'33cbdabe-c5f4-4a3b-a11e-cea707607034', N'481f6333-52ca-4d8d-84fe-4b8c2e48dc0e', N'weekday', N'Sunday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'23a1cbab-45bb-442a-b9a6-d19156435882', N'dc423f5f-45f7-4aa8-8297-0ff38fd93103', N'date', N'2023-Jul-15', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'1f4cf74b-b4bf-4cca-89df-d7f1e5c33453', N'481f6333-52ca-4d8d-84fe-4b8c2e48dc0e', N'weekday', N'Saturday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'b547d053-43b8-4d8b-9222-e98c973f9447', N'481f6333-52ca-4d8d-84fe-4b8c2e48dc0e', N'weekday', N'Thursday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'b52ecef4-f68c-477c-94bd-f576ccf13237', NULL, N'date', N'2023-Jul-15', 0, 1080, 1140)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'4f3b6489-9fb4-4ea1-9ccb-f58dbef3ff34', N'481f6333-52ca-4d8d-84fe-4b8c2e48dc0e', N'weekday', N'Tuesday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'4dce4a40-85e1-44ca-ad85-f5f8b3e6571e', N'481f6333-52ca-4d8d-84fe-4b8c2e48dc0e', N'weekday', N'Friday', 0, 540, 1020)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'c6ac06e4-1a83-4964-98a7-fe83fa3be376', N'481f6333-52ca-4d8d-84fe-4b8c2e48dc0e', N'date', N'2023-Jul-15', 0, 1080, 1140)
GO
INSERT [dbo].[EventTypeAvailabilityDetail] ([Id], [EventTypeId], [DayType], [Value], [StepId], [From], [To]) VALUES (N'f033d027-a8f1-49d7-8561-ffeb5d64c500', N'dc423f5f-45f7-4aa8-8297-0ff38fd93103', N'date', N'2023-Jul-15', 0, 1080, 1140)
GO
INSERT [dbo].[EventTypeQuestion] ([Id], [EventTypeId], [Name], [QuestionType], [Options], [OtherOptionYN], [RequiredYN], [ActiveYN], [DisplayOrder], [SystemDefinedYN]) VALUES (N'81cc98d5-bc50-4d99-ea7e-08db7b2a74de', N'481f6333-52ca-4d8d-84fe-4b8c2e48dc0e', N'Name', N'Text', NULL, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[EventTypeQuestion] ([Id], [EventTypeId], [Name], [QuestionType], [Options], [OtherOptionYN], [RequiredYN], [ActiveYN], [DisplayOrder], [SystemDefinedYN]) VALUES (N'a537a21e-f538-4404-ea7f-08db7b2a74de', N'481f6333-52ca-4d8d-84fe-4b8c2e48dc0e', N'Email', N'Text', NULL, 0, 1, 1, 2, 1)
GO
INSERT [dbo].[EventTypeQuestion] ([Id], [EventTypeId], [Name], [QuestionType], [Options], [OtherOptionYN], [RequiredYN], [ActiveYN], [DisplayOrder], [SystemDefinedYN]) VALUES (N'788d5678-1785-465e-954b-26414da6e1f8', N'38de7907-33ad-41e7-943a-bb8e7279ff43', N'Name', N'Text', NULL, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[EventTypeQuestion] ([Id], [EventTypeId], [Name], [QuestionType], [Options], [OtherOptionYN], [RequiredYN], [ActiveYN], [DisplayOrder], [SystemDefinedYN]) VALUES (N'895422d1-7356-482e-9bdd-2695611d2925', N'7e767902-dbfb-4bed-a8dc-eb0f14c517c3', N'Email', N'Text', NULL, 0, 1, 1, 2, 1)
GO
INSERT [dbo].[EventTypeQuestion] ([Id], [EventTypeId], [Name], [QuestionType], [Options], [OtherOptionYN], [RequiredYN], [ActiveYN], [DisplayOrder], [SystemDefinedYN]) VALUES (N'589e5b25-86e6-4448-8cc4-2be27f7b4edb', N'dc423f5f-45f7-4aa8-8297-0ff38fd93103', N'Name', N'Text', NULL, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[EventTypeQuestion] ([Id], [EventTypeId], [Name], [QuestionType], [Options], [OtherOptionYN], [RequiredYN], [ActiveYN], [DisplayOrder], [SystemDefinedYN]) VALUES (N'3318828f-346a-42f8-9ee1-2ea582a3b810', N'4755c2d3-c5c2-423c-9811-3695fbda88ab', N'Email', N'Text', NULL, 0, 1, 1, 2, 1)
GO
INSERT [dbo].[EventTypeQuestion] ([Id], [EventTypeId], [Name], [QuestionType], [Options], [OtherOptionYN], [RequiredYN], [ActiveYN], [DisplayOrder], [SystemDefinedYN]) VALUES (N'114a8799-a609-4d1e-8512-3c6e44628dcd', N'c36fab43-6809-4c49-bb55-7efce3a77437', N'Gender', N'RadioButtons', N'Male~~Female', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[EventTypeQuestion] ([Id], [EventTypeId], [Name], [QuestionType], [Options], [OtherOptionYN], [RequiredYN], [ActiveYN], [DisplayOrder], [SystemDefinedYN]) VALUES (N'c73bd55b-9d85-4e71-9973-46feea05637b', N'38de7907-33ad-41e7-943a-bb8e7279ff43', N'Email', N'Text', NULL, 0, 1, 1, 2, 1)
GO
INSERT [dbo].[EventTypeQuestion] ([Id], [EventTypeId], [Name], [QuestionType], [Options], [OtherOptionYN], [RequiredYN], [ActiveYN], [DisplayOrder], [SystemDefinedYN]) VALUES (N'29ac71b2-b6be-4aef-9611-8c515d2473f9', N'27b92d02-40f3-4a94-af18-b932ec358412', N'Email', N'Text', NULL, 0, 1, 1, 2, 1)
GO
INSERT [dbo].[EventTypeQuestion] ([Id], [EventTypeId], [Name], [QuestionType], [Options], [OtherOptionYN], [RequiredYN], [ActiveYN], [DisplayOrder], [SystemDefinedYN]) VALUES (N'76e642b2-1c6f-4d73-b2db-8d40c44f9bc2', N'c36fab43-6809-4c49-bb55-7efce3a77437', N'Name', N'Text', NULL, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[EventTypeQuestion] ([Id], [EventTypeId], [Name], [QuestionType], [Options], [OtherOptionYN], [RequiredYN], [ActiveYN], [DisplayOrder], [SystemDefinedYN]) VALUES (N'fe99229b-4b2e-4e21-a34b-94537caa35b9', N'9f5239de-ffe9-49a0-923b-88e91f109221', N'Gender', N'RadioButtons', N'Male~~Female', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[EventTypeQuestion] ([Id], [EventTypeId], [Name], [QuestionType], [Options], [OtherOptionYN], [RequiredYN], [ActiveYN], [DisplayOrder], [SystemDefinedYN]) VALUES (N'5c46ab38-0f07-46dd-9727-9b2c38c394dd', N'481f6333-52ca-4d8d-84fe-4b8c2e48dc0e', N'Please Specify Gender.', N'RadioButtons', N'Male~~Female~~NA', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[EventTypeQuestion] ([Id], [EventTypeId], [Name], [QuestionType], [Options], [OtherOptionYN], [RequiredYN], [ActiveYN], [DisplayOrder], [SystemDefinedYN]) VALUES (N'02eb45f8-d558-43d2-b055-a6b32b5762ab', N'27b92d02-40f3-4a94-af18-b932ec358412', N'Gender', N'RadioButtons', N'Male~~Female', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[EventTypeQuestion] ([Id], [EventTypeId], [Name], [QuestionType], [Options], [OtherOptionYN], [RequiredYN], [ActiveYN], [DisplayOrder], [SystemDefinedYN]) VALUES (N'272db9a9-e276-44f6-aae7-b37df557318a', N'9f5239de-ffe9-49a0-923b-88e91f109221', N'Name', N'Text', NULL, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[EventTypeQuestion] ([Id], [EventTypeId], [Name], [QuestionType], [Options], [OtherOptionYN], [RequiredYN], [ActiveYN], [DisplayOrder], [SystemDefinedYN]) VALUES (N'137ef575-2f8d-455d-9670-b6f3a26f42c0', N'4755c2d3-c5c2-423c-9811-3695fbda88ab', N'Name', N'Text', NULL, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[EventTypeQuestion] ([Id], [EventTypeId], [Name], [QuestionType], [Options], [OtherOptionYN], [RequiredYN], [ActiveYN], [DisplayOrder], [SystemDefinedYN]) VALUES (N'8b456d84-e1b3-474a-a592-bc2b430648f5', N'7e767902-dbfb-4bed-a8dc-eb0f14c517c3', N'Name', N'Text', NULL, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[EventTypeQuestion] ([Id], [EventTypeId], [Name], [QuestionType], [Options], [OtherOptionYN], [RequiredYN], [ActiveYN], [DisplayOrder], [SystemDefinedYN]) VALUES (N'4c8e25a0-551d-401b-b121-c3e608de4528', N'7e767902-dbfb-4bed-a8dc-eb0f14c517c3', N'Gender', N'RadioButtons', N'Male~~Female', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[EventTypeQuestion] ([Id], [EventTypeId], [Name], [QuestionType], [Options], [OtherOptionYN], [RequiredYN], [ActiveYN], [DisplayOrder], [SystemDefinedYN]) VALUES (N'c8392dea-ad18-4826-86d9-c55e35825449', N'c36fab43-6809-4c49-bb55-7efce3a77437', N'Email', N'Text', NULL, 0, 1, 1, 2, 1)
GO
INSERT [dbo].[EventTypeQuestion] ([Id], [EventTypeId], [Name], [QuestionType], [Options], [OtherOptionYN], [RequiredYN], [ActiveYN], [DisplayOrder], [SystemDefinedYN]) VALUES (N'27bc5da8-64d0-4e7e-a4da-c9a8db071ade', N'dc423f5f-45f7-4aa8-8297-0ff38fd93103', N'Gender', N'RadioButtons', N'Male~~Female', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[EventTypeQuestion] ([Id], [EventTypeId], [Name], [QuestionType], [Options], [OtherOptionYN], [RequiredYN], [ActiveYN], [DisplayOrder], [SystemDefinedYN]) VALUES (N'a1059636-fa2d-4363-9755-f52d4c12ebf1', N'dc423f5f-45f7-4aa8-8297-0ff38fd93103', N'Email', N'Text', NULL, 0, 1, 1, 2, 1)
GO
INSERT [dbo].[EventTypeQuestion] ([Id], [EventTypeId], [Name], [QuestionType], [Options], [OtherOptionYN], [RequiredYN], [ActiveYN], [DisplayOrder], [SystemDefinedYN]) VALUES (N'56923635-6aaf-47f4-9b85-f8d89e79dc9b', N'9f5239de-ffe9-49a0-923b-88e91f109221', N'Email', N'Text', NULL, 0, 1, 1, 2, 1)
GO
INSERT [dbo].[EventTypeQuestion] ([Id], [EventTypeId], [Name], [QuestionType], [Options], [OtherOptionYN], [RequiredYN], [ActiveYN], [DisplayOrder], [SystemDefinedYN]) VALUES (N'15ed082f-c763-4917-bca0-fc507c0a58c3', N'38de7907-33ad-41e7-943a-bb8e7279ff43', N'Gender', N'RadioButtons', N'Male~~Female', 1, 1, 0, 0, 0)
GO
INSERT [dbo].[EventTypeQuestion] ([Id], [EventTypeId], [Name], [QuestionType], [Options], [OtherOptionYN], [RequiredYN], [ActiveYN], [DisplayOrder], [SystemDefinedYN]) VALUES (N'f35be564-b404-4452-8dce-fe9feffbc83b', N'27b92d02-40f3-4a94-af18-b932ec358412', N'Name', N'Text', NULL, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'1', N'Dateline Standard Time', N'-12:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'10', N'Pacific Standard Time', N'-07:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'100', N'SE Asia Standard Time', N'+07:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'101', N'Altai Standard Time', N'+07:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'102', N'W. Mongolia Standard Time', N'+07:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'103', N'North Asia Standard Time', N'+07:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'104', N'N. Central Asia Standard Time', N'+07:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'105', N'Tomsk Standard Time', N'+07:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'106', N'China Standard Time', N'+08:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'107', N'North Asia East Standard Time', N'+08:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'108', N'Singapore Standard Time', N'+08:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'109', N'W. Australia Standard Time', N'+08:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'11', N'US Mountain Standard Time', N'-07:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'110', N'Taipei Standard Time', N'+08:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'111', N'Ulaanbaatar Standard Time', N'+08:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'112', N'Aus Central W. Standard Time', N'+08:45', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'113', N'Transbaikal Standard Time', N'+09:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'114', N'Tokyo Standard Time', N'+09:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'115', N'North Korea Standard Time', N'+09:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'116', N'Korea Standard Time', N'+09:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'117', N'Yakutsk Standard Time', N'+09:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'118', N'Cen. Australia Standard Time', N'+09:30', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'119', N'AUS Central Standard Time', N'+09:30', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'12', N'Mountain Standard Time (Mexico)', N'-07:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'120', N'E. Australia Standard Time', N'+10:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'121', N'AUS Eastern Standard Time', N'+10:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'122', N'West Pacific Standard Time', N'+10:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'123', N'Tasmania Standard Time', N'+10:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'124', N'Vladivostok Standard Time', N'+10:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'125', N'Lord Howe Standard Time', N'+10:30', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'126', N'Bougainville Standard Time', N'+11:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'127', N'Russia Time Zone 10', N'+11:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'128', N'Magadan Standard Time', N'+11:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'129', N'Norfolk Standard Time', N'+11:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'13', N'Mountain Standard Time', N'-06:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'130', N'Sakhalin Standard Time', N'+11:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'131', N'Central Pacific Standard Time', N'+11:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'132', N'Russia Time Zone 11', N'+12:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'133', N'New Zealand Standard Time', N'+12:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'134', N'UTC+12', N'+12:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'135', N'Fiji Standard Time', N'+12:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'136', N'Kamchatka Standard Time', N'+13:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'137', N'Chatham Islands Standard Time', N'+12:45', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'138', N'UTC+13', N'+13:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'139', N'Tonga Standard Time', N'+13:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'14', N'Yukon Standard Time', N'-07:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'140', N'Samoa Standard Time', N'+13:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'141', N'Line Islands Standard Time', N'+14:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'15', N'Central America Standard Time', N'-06:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'16', N'Central Standard Time', N'-05:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'17', N'Easter Island Standard Time', N'-06:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'18', N'Central Standard Time (Mexico)', N'-06:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'19', N'Canada Central Standard Time', N'-06:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'2', N'UTC-11', N'-11:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'20', N'SA Pacific Standard Time', N'-05:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'21', N'Eastern Standard Time (Mexico)', N'-05:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'22', N'Eastern Standard Time', N'-04:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'23', N'Haiti Standard Time', N'-04:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'24', N'Cuba Standard Time', N'-04:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'25', N'US Eastern Standard Time', N'-04:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'26', N'Turks And Caicos Standard Time', N'-04:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'27', N'Paraguay Standard Time', N'-04:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'28', N'Atlantic Standard Time', N'-03:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'29', N'Venezuela Standard Time', N'-04:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'3', N'Aleutian Standard Time', N'-09:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'30', N'Central Brazilian Standard Time', N'-04:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'31', N'SA Western Standard Time', N'-04:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'32', N'Pacific SA Standard Time', N'-04:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'33', N'Newfoundland Standard Time', N'-02:30', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'34', N'Tocantins Standard Time', N'-03:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'35', N'E. South America Standard Time', N'-03:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'36', N'SA Eastern Standard Time', N'-03:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'37', N'Argentina Standard Time', N'-03:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'38', N'Greenland Standard Time', N'-02:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'39', N'Montevideo Standard Time', N'-03:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'4', N'Hawaiian Standard Time', N'-10:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'40', N'Magallanes Standard Time', N'-03:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'41', N'Saint Pierre Standard Time', N'-02:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'42', N'Bahia Standard Time', N'-03:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'43', N'UTC-02', N'-02:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'44', N'Mid-Atlantic Standard Time', N'-01:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'45', N'Azores Standard Time', N'+00:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'46', N'Cape Verde Standard Time', N'-01:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'47', N'UTC', N'+00:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'48', N'GMT Standard Time', N'+01:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'49', N'Greenwich Standard Time', N'+00:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'5', N'Marquesas Standard Time', N'-09:30', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'50', N'Sao Tome Standard Time', N'+00:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'51', N'Morocco Standard Time', N'+01:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'52', N'W. Europe Standard Time', N'+02:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'53', N'Central Europe Standard Time', N'+02:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'54', N'Romance Standard Time', N'+02:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'55', N'Central European Standard Time', N'+02:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'56', N'W. Central Africa Standard Time', N'+01:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'57', N'GTB Standard Time', N'+03:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'58', N'Middle East Standard Time', N'+03:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'59', N'Egypt Standard Time', N'+03:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'6', N'Alaskan Standard Time', N'-08:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'60', N'E. Europe Standard Time', N'+03:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'61', N'Syria Standard Time', N'+03:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'62', N'West Bank Standard Time', N'+03:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'63', N'South Africa Standard Time', N'+02:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'64', N'FLE Standard Time', N'+03:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'65', N'Israel Standard Time', N'+03:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'66', N'South Sudan Standard Time', N'+02:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'67', N'Kaliningrad Standard Time', N'+02:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'68', N'Sudan Standard Time', N'+02:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'69', N'Libya Standard Time', N'+02:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'7', N'UTC-09', N'-09:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'70', N'Namibia Standard Time', N'+02:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'71', N'Jordan Standard Time', N'+03:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'72', N'Arabic Standard Time', N'+03:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'73', N'Turkey Standard Time', N'+03:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'74', N'Arab Standard Time', N'+03:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'75', N'Belarus Standard Time', N'+03:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'76', N'Russian Standard Time', N'+03:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'77', N'E. Africa Standard Time', N'+03:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'78', N'Volgograd Standard Time', N'+03:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'79', N'Iran Standard Time', N'+03:30', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'8', N'Pacific Standard Time (Mexico)', N'-07:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'80', N'Arabian Standard Time', N'+04:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'81', N'Astrakhan Standard Time', N'+04:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'82', N'Azerbaijan Standard Time', N'+04:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'83', N'Russia Time Zone 3', N'+04:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'84', N'Mauritius Standard Time', N'+04:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'85', N'Saratov Standard Time', N'+04:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'86', N'Georgian Standard Time', N'+04:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'87', N'Caucasus Standard Time', N'+04:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'88', N'Afghanistan Standard Time', N'+04:30', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'89', N'West Asia Standard Time', N'+05:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'9', N'UTC-08', N'-08:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'90', N'Ekaterinburg Standard Time', N'+05:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'91', N'Pakistan Standard Time', N'+05:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'92', N'Qyzylorda Standard Time', N'+05:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'93', N'India Standard Time', N'+05:30', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'94', N'Sri Lanka Standard Time', N'+05:30', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'95', N'Nepal Standard Time', N'+05:45', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'96', N'Central Asia Standard Time', N'+06:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'97', N'Bangladesh Standard Time', N'+06:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'98', N'Omsk Standard Time', N'+06:00', 0)
GO
INSERT [dbo].[TimeZoneData] ([Id], [Name], [Offset], [OffsetMinutes]) VALUES (N'99', N'Myanmar Standard Time', N'+06:30', 0)
GO
INSERT [dbo].[Users] ([Id], [UserID], [Password], [BaseURI], [timeZone], [UserName], [WelcomeText]) VALUES (N'aab1a433-ad84-4e37-b370-3c0f162524e1', N'admin', N'123', N'shahinur-kabir', N'Europe/Tirane', N'Shahinur Kabir', N'Welcome to my scheduling page. Please follow the instructions to add an event to my calendar.')
GO
INSERT [dbo].[Users] ([Id], [UserID], [Password], [BaseURI], [timeZone], [UserName], [WelcomeText]) VALUES (N'2f7ae761-c3a7-46ff-8746-6a0c3f953e3d', N'test', N'123', N'test', N'Asia/Dhaka', N'test', NULL)
GO
/****** Object:  Index [IX_ScheduleRuleAttributes_RuleId]    Script Date: 8/25/2023 2:02:44 PM ******/
CREATE NONCLUSTERED INDEX [IX_ScheduleRuleAttributes_RuleId] ON [dbo].[AvailabilityDetail]
(
	[AvailabilityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Availability] ADD  CONSTRAINT [DF_Availability_IsCustom1]  DEFAULT ((0)) FOR [IsDefault]
GO
ALTER TABLE [dbo].[Availability] ADD  CONSTRAINT [DF_Availability_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[AvailabilityDetail] ADD  CONSTRAINT [DF_ScheduleRuleAttribute_Step]  DEFAULT ((0)) FOR [StepId]
GO
ALTER TABLE [dbo].[EventType] ADD  CONSTRAINT [DF_EventType_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[EventTypeAvailabilityDetail] ADD  CONSTRAINT [DF_EventTypeAvailabilityDetail_StepId]  DEFAULT ((0)) FOR [StepId]
GO
ALTER TABLE [dbo].[EventTypeQuestion] ADD  CONSTRAINT [DF_EventTypeQuestion_OtherOptionYN]  DEFAULT ((0)) FOR [OtherOptionYN]
GO
ALTER TABLE [dbo].[EventTypeQuestion] ADD  CONSTRAINT [DF_EventTypeQuestion_ActiveYN]  DEFAULT ((1)) FOR [ActiveYN]
GO
ALTER TABLE [dbo].[EventTypeQuestion] ADD  DEFAULT ((0)) FOR [SystemDefinedYN]
GO
ALTER TABLE [dbo].[TimeZoneData] ADD  CONSTRAINT [DF__TimeZoneD__Offse__60A75C0F]  DEFAULT ((0)) FOR [OffsetMinutes]
GO
ALTER TABLE [dbo].[Appointment]  WITH CHECK ADD  CONSTRAINT [FK_CalendarAppointment_EventType] FOREIGN KEY([EventTypeId])
REFERENCES [dbo].[EventType] ([Id])
GO
ALTER TABLE [dbo].[Appointment] CHECK CONSTRAINT [FK_CalendarAppointment_EventType]
GO
ALTER TABLE [dbo].[EventType]  WITH CHECK ADD  CONSTRAINT [FK_EventType_Availability] FOREIGN KEY([AvailabilityId])
REFERENCES [dbo].[Availability] ([Id])
GO
ALTER TABLE [dbo].[EventType] CHECK CONSTRAINT [FK_EventType_Availability]
GO
ALTER TABLE [dbo].[EventTypeAvailabilityDetail]  WITH CHECK ADD  CONSTRAINT [FK_EventTypeAvailabilityDetail_EventType] FOREIGN KEY([EventTypeId])
REFERENCES [dbo].[EventType] ([Id])
GO
ALTER TABLE [dbo].[EventTypeAvailabilityDetail] CHECK CONSTRAINT [FK_EventTypeAvailabilityDetail_EventType]
GO
ALTER TABLE [dbo].[EventTypeQuestion]  WITH CHECK ADD  CONSTRAINT [FK_EventTypeQuestion_EventType] FOREIGN KEY([EventTypeId])
REFERENCES [dbo].[EventType] ([Id])
GO
ALTER TABLE [dbo].[EventTypeQuestion] CHECK CONSTRAINT [FK_EventTypeQuestion_EventType]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M : Moving 
 D : DateRange
 F : Foreever' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EventType', @level2type=N'COLUMN',@level2name=N'DateForwardKind'
GO
USE [master]
GO
ALTER DATABASE [BookingDb] SET  READ_WRITE 
GO
