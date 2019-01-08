USE [LocationDB]
GO

/****** Object:  StoredProcedure [dbo].[GetParkrunsByYear]    Script Date: 08/01/2019 19:51:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[GetParkrunsByYear] 
	-- Add the parameters for the stored procedure here
	@Year int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [Id]
      ,[RaceDate]
      ,[Race]
      ,[Position]
      ,[Grade]
      ,[Minutes]
      ,[Seconds]
  FROM [LocationDB].[dbo].[Parkruns]
  where YEAR(RaceDate) = @Year
  order by Minutes asc,Seconds asc
END

GO


