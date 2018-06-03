create table comment(
id int primary key auto_increment not null,
owner_user_id int  not null,
target_user_id int not null,
story_id int not null,
content varchar(1000) not null,
create_time datetime not null

)