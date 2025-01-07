go
create database YourVitebskDB;
go
use YourVitebskDB;

create table Roles(
	RoleId int identity not null primary key,
	Name varchar(50) unique not null
);

create table RolePermissions(
	RolePermissionId int identity not null primary key,
	Name varchar(50) unique not null
);

create table RolePermissionLinks(
	RolePermissionLinkId int identity not null primary key,
	RoleId int not null,
	RolePermissionId int not null,
	foreign key (RoleId) references Roles (RoleId) on delete cascade,
	foreign key (RolePermissionId) references RolePermissions(RolePermissionId) on delete cascade
)

create table Users(
	UserId int identity not null primary key,
	Email varchar(100) unique not null,
	PasswordHash varbinary(max) not null,
	PasswordSalt varbinary(max) not null,
	FirstName varchar(50) not null,
	LastName varchar(50) not null,
	PhoneNumber varchar(50),
	RoleId int not null,
	IsVisible bit not null
	foreign key (RoleId) references Roles (RoleId)
);

create table News(
	NewsId int identity not null primary key,
	Title varchar(300) not null,
	Description varchar(max) not null,
	ExternalLink varchar(300)
);

create table CafeTypes(
	CafeTypeId int identity not null primary key,
	Name varchar(100) not null
);

create table Cafes(
	CafeId int identity not null primary key,
	CafeTypeId int not null,
	Title varchar(150) not null,
	Description varchar(max) not null,
	WorkingTime varchar(500) not null,
	Address varchar(300) not null,
	ExternalLink varchar(300)
	foreign key (CafeTypeId) references CafeTypes (CafeTypeId) on delete cascade
);

create table PosterTypes(
	PosterTypeId int identity not null primary key,
	Name varchar(100) not null,
);

create table Posters(
	PosterId int identity not null primary key,
	PosterTypeId int not null,
	Title varchar(150) not null,
	Description varchar(max) not null,
	DateTime varchar(500) not null,
	Address varchar(300) not null,
	ExternalLink varchar(300),
	foreign key (PosterTypeId) references PosterTypes(PosterTypeId) on delete cascade
);


create table Vacancies(
	VacancyId int identity not null primary key,
	Title varchar(150) not null,
	Description varchar(max) not null,
	Requirements varchar(max) not null,
	Conditions varchar(max) not null,
	Salary varchar(100) not null,
	CompanyName varchar(100) not null,
	Contacts varchar(300) not null,
	Address varchar(300) not null,
	PublishDate date not null
);

create table Services(
	ServiceId int identity not null primary key,
	Name varchar(100) not null
);

create table Comments(
	CommentId int identity not null primary key,
	UserId int not null,
	ServiceId int not null,
	ItemId int not null,
	IsRecommend bit not null,
	Message varchar(max),
	PublishDate smalldatetime not null,
	foreign key (UserId) references Users (UserId) on delete cascade,
	foreign key (ServiceId) references Services (ServiceId) on delete cascade
);

go
create trigger RoleDeletedAction on YourVitebskDB.dbo.Roles
instead of delete
as
update Users set RoleId = 1 where RoleId = (select RoleId from deleted);
delete from Roles where RoleId = (select RoleId from deleted);

go
create trigger CafeDeletedAction on YourVitebskDB.dbo.Cafes
after delete
as
DECLARE @id int;
SELECT @id = CafeId from deleted;
delete from Comments where ServiceId = 1 and ItemId = @id;

go
create trigger PosterDeletedAction on YourVitebskDB.dbo.Posters
after delete
as
DECLARE @id int;
SELECT @id = PosterId from deleted;
delete from Comments where ServiceId = 2 and ItemId = @id;

go
create trigger CafeTypeDeletedAction on YourVitebskDB.dbo.CafeTypes
instead of delete
as
DECLARE @id int;
SELECT @id = CafeTypeId from deleted;
delete from Comments where ServiceId = 1 and ItemId in (select CafeId from Cafes where CafeTypeId = @id);
delete from CafeTypes where CafeTypeId = @id;

go
create trigger PosterTypeDeletedAction on YourVitebskDB.dbo.PosterTypes
instead of delete
as
DECLARE @id int;
SELECT @id = PosterTypeId from deleted;
delete from Comments where ServiceId = 2 and ItemId in (select PosterId from Posters where PosterTypeId = @id);
delete from PosterTypes where PosterTypeId = @id;

go
insert into Roles values('Пользователь'), ('Администратор');
insert into RolePermissions values
('UsersGet'),('UsersCreate'),('UsersUpdate'),('UsersDelete'),
('RolesGet'),('RolesCreate'),('RolesUpdate'),('RolesDelete'),
('NewsGet'),('NewsCreate'),('NewsUpdate'),('NewsDelete'),
('CafesGet'),('CafesCreate'),('CafesUpdate'),('CafesDelete'),
('PostersGet'),('PostersCreate'),('PostersUpdate'),('PostersDelete'),
('VacanciesGet'),('VacanciesCreate'),('VacanciesUpdate'),('VacanciesDelete'),
('CommentsGet'),('CommentsDelete');
insert into RolePermissionLinks values(2,1),(2,2),(2,3),(2,4),(2,5),(2,6),(2,7),(2,8),(2,9),(2,10),(2,11),(2,12),(2,13),(2,14),(2,15),(2,16),(2,17),(2,18),(2,19),(2,20),(2,21),(2,22),(2,23),(2,24),(2,25),(2,26);
insert into Users values('admin@yourvitebsk.by', 
						0x1BE1623141791CE4A9BAC181DA72C47C27337EB76F29D1A9D9839EFE9F6197D4D1E56DE84BDCCDAA93D839E3CFCD5261B60C32CBBA77E8336CEBCF4492A254FD, 
						0x6FE7E2755BADB4F8801F07B48AFA0288DE3717A4E5EEB1D9FEA7A750A85A620CD3D6C1E516CA467D2CCE871C5E03A4C3F4B66B26B349C26B28C09EC6D94BE12697456D3A6031FB9C0C6CCE5458E24FD83A15D4E6A30663345FFFE85735A0593E6494F3928326EE5A987A63A4CE4515A64E633C8E0AF2723B5D5AF8107C181EF4,
						'Кирилл', 'Нестерович', '+375(29)311-35-00', 2, 'true');

insert into Services values('Заведения'), ('Афиша');

