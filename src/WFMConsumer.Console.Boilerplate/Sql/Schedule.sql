CREATE TABLE [dbo].[Schedule]
(
	[ContractTimeMinutes] INT NULL,
    [Date] DATETIME NULL,
    [IsFullDayAbsence] bit NULL,
    [Name] VARCHAR(50) NULL, 
    [PersonId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
)