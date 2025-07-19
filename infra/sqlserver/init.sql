CREATE DATABASE [dbstudent]
GO
USE [dbstudent]
GO
/****** Object:  Schema [mst]    Script Date: 19/07/2025 17:15:27 ******/
CREATE SCHEMA [mst]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 19/07/2025 17:15:27 ******/
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
/****** Object:  Table [dbo].[tb_student]    Script Date: 19/07/2025 17:15:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_student](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[uid] [uniqueidentifier] NOT NULL,
	[first_name] [varchar](128) NOT NULL,
	[last_name] [varchar](128) NOT NULL,
	[birth_date] [datetime2](7) NOT NULL,
	[email] [varchar](128) NOT NULL,
	[third_party_uid] [uniqueidentifier] NULL,
 CONSTRAINT [PKtbstudent] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [mst].[__EFMigrationsHistory]    Script Date: 19/07/2025 17:15:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [mst].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [mst].[tb_student_created_third_party_registration_saga_data]    Script Date: 19/07/2025 17:15:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [mst].[tb_student_created_third_party_registration_saga_data](
	[correlation_id] [uniqueidentifier] NOT NULL,
	[current_state] [varchar](64) NOT NULL,
	[saga_init_at] [datetime2](7) NULL,
	[student_uid] [uniqueidentifier] NULL,
	[request_uid] [uniqueidentifier] NULL,
	[request_create_student_third_party_uid_sended_at] [datetime2](7) NULL,
	[response_create_student_third_party_uid_not_received_last_at] [datetime2](7) NULL,
	[response_create_student_third_party_uid_schedule_token_id] [uniqueidentifier] NULL,
	[response_create_student_third_party_uid_not_received_retry_count] [int] NULL,
	[response_create_student_third_party_uid_received_at] [datetime2](7) NULL,
	[student_third_party_uid_updated_at] [datetime2](7) NULL,
 CONSTRAINT [PKstudentcreatedthirdpartyregistrationsagadata] PRIMARY KEY CLUSTERED 
(
	[correlation_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250221022238_AddTables', N'8.0.11')
GO
INSERT [mst].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250719201412_AddTables', N'8.0.11')
GO
ALTER TABLE [dbo].[tb_student] ADD  DEFAULT (newid()) FOR [uid]
GO
