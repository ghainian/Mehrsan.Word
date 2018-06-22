-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE bpRemoveRepetitiveWords 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

select count(1) as Counts,TargetWord,min(id) as MinId  into helpTbl from dbo.word 
group by TargetWord order by count(1) desc  
--where TargetWord like '%stort%'

delete from dbo.Word where 
TargetWord in (select TargetWord from dbo.helpTbl) and 
Id not in (select minId from dbo.helpTbl)

drop table dbo.helpTbl

END
