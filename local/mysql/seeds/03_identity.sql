create table tokentype
(
	id int auto_increment,
	name varchar(150) not null,
	constraint tokentype_pk
		primary key (id)
);

create unique index tokentype_name_uindex
	on tokentype (name);

create table user
(
	id int auto_increment,
	name varchar(100) null,
	email varchar(100) not null,
	password varchar(255) not null,
	active bit default true not null,
  confirmed bit default false not null,
	created datetime not null,
	createdby varchar(100) not null,
	updated datetime default NULL null,
	updatedby varchar(100) null,
	constraint user_pk
		primary key (id)
);

create unique index user_email_uindex
	on user (email);

create table token
(
	id int auto_increment,
	userid int not null,
	typeid int not null,
	value varchar(200) not null,
	expiration datetime not null,
	used bit default false not null,
	created datetime not null,
	createdby varchar(100) not null,
	updated datetime null,
	updatedby varchar(100) null,
	constraint token_pk
		primary key (id),
	constraint token_tokentype_id_fk
		foreign key (typeid) references tokentype (id),
	constraint token_user_id_fk
		foreign key (userid) references user (id)
);

create unique index token_value_uindex
	on token (value);


