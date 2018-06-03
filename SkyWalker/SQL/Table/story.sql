create table story(
id int  auto_increment primary key not null,
book_id int  not null,
user_id int not null,
title varchar(100) not null,
content varchar(500) null,
cover_photo varchar(200)  null,
create_time datetime not null,
update_time datetime null
)