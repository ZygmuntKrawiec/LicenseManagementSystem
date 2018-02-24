CREATE PROCEDURE [dbo].[spAuthenticateUser]
	@UserEmail nvarchar(100),
	@Password nvarchar(100
AS
begin
	declare @Count int

	select @Count = count(UserEmail) from tblUsersWithAccessToLicensesData
	where UserEmail = @UserEmail and UserPassword = @Password

	if(@Count = 1)
	begin
		select 1 as ReturnCode
	end
	else
	begin
		select -1 as ReturnCode
	end
end	
