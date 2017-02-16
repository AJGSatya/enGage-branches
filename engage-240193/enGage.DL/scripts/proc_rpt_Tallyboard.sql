SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nikkho Shandittha
-- Create date: 10/04/2013
-- Description:	The following store procedure is used in the Tallyboard report
-- =============================================
ALTER PROCEDURE [dbo].[proc_rpt_Tallyboard]  (@ReportStart smalldatetime ,@ReportEnd smalldatetime ,@Region varchar(255) ,@Branch varchar(255) ,@AccountExecutiveID varchar(50) ,@BusinessTypeID int ,@ClassificationID int ,@SourceDelimitedList varchar(max) ,@ANZSICDelimitedList varchar(max), @OpportunitiesDelimitedList varchar(max) ) AS SET NOCOUNT ON

BEGIN

Set		@ReportStart  = isnull(@ReportStart,'01 jan 1900')
Set		@ReportEnd    = isnull(dateadd(minute,24*60-1,@ReportEnd),Getdate())
-- @FollowUpByDate is the date used to test if the follow-up is over due
-- Where the report is in a past period it is set to @ReportEnd otherwise it's today's date
Declare	@FollowUpByDate as smalldatetime
Set		@FollowUpByDate = Case When @ReportEnd < GetDate() Then @ReportEnd Else GetDate() End 

----------------------------------------------------------------------------------------

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------	
-- Dashboard TOTALS
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Select	-- Region, Branch
		TallyFor
		,sum(IsNull(Activities, 0)) as Activities 
		,count(DISTINCT clientIDCount) as Clients
		,count(DISTINCT OpportunityIDCount) as Opportunities
From ( Select Distinct Action, PreviousStatusID From Status ) as sl
		Left outer join (

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------	
-- Dashboard DETAIL
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Select	brae.Branch + ' (' + brae.Region + ')' as TallyFor ,brae.Region, brae.Branch, brae.FullName as AccountExecutive, c.AccountExecutiveID, c.ClientName, c.Source, o.OpportunityName, o.OpportunityDue, bt.BusinessTypeName, cl.ClassificationName, a.ActivityID, a.Added, s.StatusName, s.Action, os.currentStatus, IsNull(s.PreviousStatusID,0) as StatusOrder, a.ActivityNote,

		/* Activities		WHERE Activity Added WITHIN the Report Period */ Case When a.Added Between @ReportStart And @ReportEnd Then 1 Else 0 End as Activities,
		/* Clients			WHERE Fishing as Source or OpportunityName */ c.ClientID as ClientIDCount,
		/* Opportunities	WHERE Fishing as Source or OpportunityName */ o.OpportunityID as OpportunityIDCount


From	dbo.Activity as a
		INNER JOIN Status as s on s.StatusID = a.OpportunityStatusID
		INNER JOIN Opportunity as o on o.OpportunityID = a.OpportunityID
		Left Outer JOIN Classification as cl on cl.ClassificationID = o.ClassificationID 
		INNER JOIN BusinessType as bt on bt.BusinessTypeID = o.BusinessTypeID
		INNER JOIN Client as c on o.ClientID = c.ClientID
		INNER JOIN vw_BranchRegionAccountExecutive as brae On c.AccountExecutiveID = brae.AccountExecutiveID
		INNER JOIN ( /* Sub Select Opportunity Status From Last Activity */		
					 -- UN-COMMENT TO TEST SUB-SELECT: Declare @ReportDate as smalldatetime Set @ReportDate = GetDate() 
					 Select	a.OpportunityID, a.FollowUpDate as currentFollowUpDate, s.Action as lastAction, s.StatusName as currentStatus, s.OutcomeType as currentOutcomeType
					 From	dbo.Activity as a
							Inner Join Status as s on s.StatusID = a.OpportunityStatusID
					 Where Exists /* Select the Last Activity Based on Max(ActivityID) WHERE Outcome is S (Success) Or C (Complete) And NOT Inactive And BEFORE Last Day of Report Period */
								 (	Select 'lastActivity', a1.OpportunityID, max(a1.activityID) From dbo.Activity as a1 Inner Join Status as s ON s.StatusID = a1.OpportunityStatusID 
									Where	a1.Inactive = 0 And a1.Added <= @ReportEnd And s.OutcomeType != 'P'
									And		a1.OpportunityID = a.OpportunityID
									group by a1.OpportunityID
									Having	max(a1.ActivityID) = a.ActivityID )
					) as os on os.OpportunityID = o.OpportunityID
Where (	a.Inactive = 0 And c.Inactive = 0 )
And	 (		( brae.AccountExecutiveID = @AccountExecutiveID Or @AccountExecutiveID = '(All)' )
		And	( brae.Branch = @Branch Or @Branch = '(All)' )
		And	( brae.Region = @Region Or @Region = '(All)' )	
		)
And Exists ( Select 'these opportunities' From dbo.fun_rs_multipara_split(@OpportunitiesDelimitedList,'|') Where o.OpportunityName = Value ) 

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------	
-- END Dashboard DETAIL
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

) as DashboardDATA on DashboardDATA.Action = sl.Action
Group by -- Region, Branch 
	TallyFor 
--	,sl.Action, sl.PreviousStatusID
Order by --
	count(DISTINCT OpportunityIDCount) DESC
	-- ,sl.PreviousStatusID

END
GO
