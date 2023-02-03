use SocialMedia

go

if not exists(select * from sys.server_principals where name ='SMUser')
begin 
    create login SMUser with PASSWORD='SuperUser123!@#', DEFAULT_DATABASE=SocialMedia
end

if not exists(select * from sys.database_principals where name ='SMUser')
begin
    exec sp_adduser 'SMUser','SMUser','db_owner';
end