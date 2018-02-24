create table tblLicenses
(
	LicenseId int primary key identity,
	DateOfCreation datetime not null,
	CustomerName varchar(50) not null,
	CustomerEmail varchar(50) not null,
	LicenseKey varchar(10)
)
go

create table tblUsersWithAccessToLicensesData
(
	UserId int primary key identity,
	UserEmail varchar(50),
	UserPassword varchar(50)
)
go


insert into tblLicenses values(getdate(), 'Johan Smith', 'johansmith@a2.com',null)--1
insert into tblLicenses values(getdate(), 'Zoie Hudnall', 'zoiehudnall@a2.com',null)
insert into tblLicenses values(getdate(), 'Bob Adams', 'bobadams@a2.com',null)
insert into tblLicenses values(getdate(), 'Edward Northon', 'edwardnorthon@a2.com',null)
insert into tblLicenses values(getdate(), 'Adam Sandler', 'adamsandler@a2.com',null)

insert into tblLicenses values(getdate(), 'Chuck White', 'chuckwhite@a2.com',null)--2
insert into tblLicenses values(getdate(), 'Emma Stone', 'emmastone@a2.com',null)
insert into tblLicenses values(getdate(), 'Vigo Mortensen', 'vigomortensen@a2.com',null)
insert into tblLicenses values(getdate(), 'John Carpenter', 'johncarpenter@a2.com',null)
insert into tblLicenses values(getdate(), 'Emanuel Kant', 'emnuelkant@a2.com',null)

insert into tblLicenses values(getdate(), 'Jack Nicholson', 'jacknicholson@a2.com',null)--3
insert into tblLicenses values(getdate(), 'Rian Weaver', 'rianweaver@a2.com',null)
insert into tblLicenses values(getdate(), 'Carolyn Bernier', 'carolynbernier@a2.com',null)
insert into tblLicenses values(getdate(), 'Barbara Burris', 'barbaraburris@a2.com',null)
insert into tblLicenses values(getdate(), 'Megan Hutchinson', 'meganhutchinson@a2.com',null)

insert into tblLicenses values(getdate(), 'Cecily Smith', 'cecilysmith@a2.com',null)--4
insert into tblLicenses values(getdate(), 'Alexander Sigler', 'alexandersigler@a2.com',null)
insert into tblLicenses values(getdate(), 'Aaron Flynn', 'aaronflynn@a2.com',null)
insert into tblLicenses values(getdate(), 'Jodie Curtis', 'jodiecurtis@a2.com',null)
insert into tblLicenses values(getdate(), 'Amy Lewis', 'amylewis@a2.com',null)

insert into tblLicenses values(getdate(), 'Lily Cox', 'lilycox@a2.com',null)--5
insert into tblLicenses values(getdate(), 'Yasmin Mitchell', 'yasminmitchell@a2.com',null)
insert into tblLicenses values(getdate(), 'Alice Henry', 'alicehenry@a2.com',null)
insert into tblLicenses values(getdate(), 'Kieran Dodd', 'kierandodd@a2.com',null)
insert into tblLicenses values(getdate(), 'Isabella Davis', 'isabelladavis@a2.com',null)
go