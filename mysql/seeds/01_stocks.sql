create table country
(
    id   int auto_increment
        primary key,
    name varchar(150) not null
);

create table exchange
(
    id        int auto_increment
        primary key,
    countryid int          not null,
    name      varchar(100) not null,
    created   datetime     not null,
    createdby varchar(100) not null,
    updated   datetime     null,
    updatedby varchar(100) null,
    constraint exchange_country_fk
        foreign key (countryid) references country (id)
);

create table form
(
    id   int auto_increment
        primary key,
    name varchar(100) not null
);

create table `index`
(
    id   int          not null
        primary key,
    name varchar(100) not null
);

create table regulatory
(
    id   int          not null
        primary key,
    name varchar(100) not null
);

create table sector
(
    id   int auto_increment
        primary key,
    name varchar(45) not null
);

create table company
(
    id        int auto_increment
        primary key,
    sectorid  int          not null,
    name      varchar(200) not null,
    created   datetime     not null,
    createdby varchar(100) not null,
    updated   datetime     null,
    updatedby varchar(100) null,
    constraint company_sector_fk
        foreign key (sectorid) references sector (id)
);

create table companycomplement
(
    id        int auto_increment
        primary key,
    companyid int          not null,
    countryid int          not null,
    cik       varchar(45)  null,
    irs       varchar(45)  null,
    created   datetime     not null,
    createdby varchar(100) not null,
    updated   datetime     null,
    updatedby varchar(100) null,
    constraint companycomp_company_fk
        foreign key (companyid) references company (id),
    constraint companycomp_country_fk
        foreign key (countryid) references country (id)
);

create index companycomp_company_idx
    on companycomplement (companyid);

create index companycomp_country_idx
    on companycomplement (countryid);

create table file
(
    id           int auto_increment
        primary key,
    companyid    int          not null,
    regulatoryid int          not null,
    formid       int          not null,
    name         varchar(200) not null,
    path         varchar(100) not null,
    created      datetime     not null,
    createdby    varchar(100) not null,
    updated      datetime     null,
    updatedby    varchar(100) null,
    constraint file_company_id_fk
        foreign key (companyid) references company (id),
    constraint file_form_fk
        foreign key (formid) references form (id),
    constraint file_regulatory_id_fk
        foreign key (regulatoryid) references regulatory (id)
);

create table content
(
    id        int auto_increment
        primary key,
    fileid    int          not null,
    tags      json         not null,
    created   datetime     not null,
    createdby varchar(100) not null,
    updated   datetime     null,
    updatedby varchar(100) null,
    constraint content_file_fk
        foreign key (fileid) references file (id)
);

create index content_file_idx
    on content (fileid);

create index file_form_idx
    on file (formid);

create table formula
(
    id           int auto_increment
        primary key,
    companyid    int  null,
    indexid      int  not null,
    regulatoryid int  not null,
    content      json not null,
    constraint formula_company_id_fk
        foreign key (companyid) references company (id),
    constraint formula_index_id_fk
        foreign key (indexid) references `index` (id),
    constraint formula_regulatory_id_fk
        foreign key (regulatoryid) references regulatory (id)
);

create table property
(
    id        int auto_increment
        primary key,
    fileid    int          not null,
    contentid int          null,
    json      json         not null,
    created   datetime     not null,
    createdby varchar(100) not null,
    updated   datetime     null,
    updatedby varchar(100) null,
    constraint property_content_id_fk
        foreign key (contentid) references content (id),
    constraint property_file_id_fk
        foreign key (fileid) references file (id)
);

create table indexes
(
    id         int auto_increment
        primary key,
    companyid  int          not null,
    propertyid int          not null,
    roe        decimal      null,
    created    datetime     not null,
    createdby  varchar(100) not null,
    updated    datetime     null,
    updatedby  varchar(100) null,
    constraint indexes_company_id_fk
        foreign key (companyid) references company (id),
    constraint indexes_property_id_fk
        foreign key (propertyid) references property (id)
);

create table ticker
(
    id         int auto_increment
        primary key,
    companyid  int          not null,
    exchangeid int          not null,
    name       varchar(100) not null,
    created    datetime     not null,
    createdby  varchar(100) not null,
    updated    datetime     null,
    updatedby  varchar(100) null,
    constraint ticker_company_fk
        foreign key (companyid) references company (id),
    constraint ticker_exchange_fk
        foreign key (exchangeid) references exchange (id)
);

create table price
(
    id        int auto_increment
        primary key,
    tickerid  int                        not null,
    value     decimal(4, 2) default 0.00 null,
    date      date                       not null,
    time      time                       not null,
    created   datetime                   not null,
    createdby varchar(100)               not null,
    updated   datetime                   null,
    updatedby varchar(100)               null,
    constraint price_ticker_fk
        foreign key (tickerid) references ticker (id)
);

create index price_ticker_idx
    on price (tickerid);

create index ticker_company_idx
    on ticker (companyid);

create index ticker_exchange_idx
    on ticker (exchangeid);
