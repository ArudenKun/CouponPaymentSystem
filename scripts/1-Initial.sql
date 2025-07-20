if exists (select 1
           from sys.objects
           where object_id = OBJECT_ID(N'[FK_E2C9AA30]')
             and parent_object_id = OBJECT_ID(N'UploadTransaction'))
alter table UploadTransaction
    drop constraint FK_E2C9AA30

if exists (select *
           from dbo.sysobjects
           where id = object_id(N'Setting')
             and OBJECTPROPERTY(id, N'IsUserTable') = 1)
    drop table Setting

if exists (select *
           from dbo.sysobjects
           where id = object_id(N'NotificationInfo')
             and OBJECTPROPERTY(id, N'IsUserTable') = 1)
    drop table NotificationInfo

if exists (select *
           from dbo.sysobjects
           where id = object_id(N'NotificationSubscriptionInfo')
             and OBJECTPROPERTY(id, N'IsUserTable') = 1)
    drop table NotificationSubscriptionInfo

if exists (select *
           from dbo.sysobjects
           where id = object_id(N'TenantNotificationInfo')
             and OBJECTPROPERTY(id, N'IsUserTable') = 1)
    drop table TenantNotificationInfo

if exists (select *
           from dbo.sysobjects
           where id = object_id(N'UserNotificationInfo')
             and OBJECTPROPERTY(id, N'IsUserTable') = 1)
    drop table UserNotificationInfo

if exists (select *
           from dbo.sysobjects
           where id = object_id(N'Upload')
             and OBJECTPROPERTY(id, N'IsUserTable') = 1)
    drop table Upload

if exists (select *
           from dbo.sysobjects
           where id = object_id(N'UploadTransaction')
             and OBJECTPROPERTY(id, N'IsUserTable') = 1)
    drop table UploadTransaction

create table Setting
(
    Id                   BIGINT IDENTITY NOT NULL,
    TenantId             INT             null,
    UserId               BIGINT          null,
    Name                 NVARCHAR(256)   not null,
    Value                NVARCHAR(MAX)   null,
    CreationTime         DATETIME2       null,
    CreatorUserId        BIGINT          null,
    LastModificationTime DATETIME2       null,
    LastModifierUserId   BIGINT          null,
    primary key (Id)
)

create table NotificationInfo
(
    Id                              UNIQUEIDENTIFIER not null,
    Data                            NVARCHAR(MAX)    null,
    DataTypeName                    NVARCHAR(512)    null,
    EntityId                        NVARCHAR(96)     null,
    EntityTypeAssemblyQualifiedName NVARCHAR(512)    null,
    EntityTypeName                  NVARCHAR(250)    null,
    ExcludedUserIds                 NVARCHAR(MAX)    null,
    NotificationName                NVARCHAR(96)     not null,
    Severity                        TINYINT          not null,
    TenantIds                       NVARCHAR(MAX)    null,
    UserIds                         NVARCHAR(MAX)    null,
    CreationTime                    DATETIME2        null,
    CreatorUserId                   BIGINT           null,
    primary key (Id)
)

create table NotificationSubscriptionInfo
(
    Id                              UNIQUEIDENTIFIER not null,
    EntityId                        NVARCHAR(96)     null,
    EntityTypeAssemblyQualifiedName NVARCHAR(512)    null,
    EntityTypeName                  NVARCHAR(250)    null,
    NotificationName                NVARCHAR(96)     null,
    TenantId                        INT              null,
    UserId                          BIGINT           not null,
    CreationTime                    DATETIME2        null,
    CreatorUserId                   BIGINT           null,
    primary key (Id)
)

create table TenantNotificationInfo
(
    Id                              UNIQUEIDENTIFIER not null,
    Data                            NVARCHAR(MAX)    null,
    DataTypeName                    NVARCHAR(512)    null,
    EntityId                        NVARCHAR(96)     null,
    EntityTypeAssemblyQualifiedName NVARCHAR(512)    null,
    EntityTypeName                  NVARCHAR(250)    null,
    NotificationName                NVARCHAR(96)     not null,
    Severity                        TINYINT          not null,
    TenantId                        INT              null,
    CreationTime                    DATETIME2        null,
    CreatorUserId                   BIGINT           null,
    primary key (Id)
)

create table UserNotificationInfo
(
    Id                   UNIQUEIDENTIFIER not null,
    State                INT              not null,
    TenantId             INT              null,
    TenantNotificationId UNIQUEIDENTIFIER not null,
    UserId               BIGINT           not null,
    CreationTime         DATETIME2        null,
    primary key (Id)
)

create table Upload
(
    Id           INT IDENTITY  NOT NULL,
    DomainId     NVARCHAR(16)  not null,
    TenantId     INT           null,
    JobId        NVARCHAR(255) not null,
    FileName     NVARCHAR(60)  not null unique,
    Currency     NVARCHAR(16)  not null,
    Status       NVARCHAR(60)  not null,
    CreationTime DATETIME2     null,
    primary key (Id)
)

create table UploadTransaction
(
    Id                INT IDENTITY   NOT NULL,
    AccountNumber     NVARCHAR(16)   not null,
    NewAccountNumber  NVARCHAR(16)   not null,
    AccountName       NVARCHAR(255)  not null,
    CasaAccountName   NVARCHAR(255)  not null,
    AccountType       NVARCHAR(2)    null,
    TransactionAmount DECIMAL(18, 2) not null,
    NetAmount         DECIMAL(18, 2) not null,
    IsMatch           BIT            not null,
    IsRejected        BIT            not null,
    Reason            INT            not null,
    UploadId          INT            null,
    primary key (Id)
)

create index IDX_UploadTransaction_IsMatch on UploadTransaction (IsMatch)

create index IDX_UploadTransaction_IsRejected on UploadTransaction (IsRejected)

alter table UploadTransaction
    add constraint FK_E2C9AA30
        foreign key (UploadId)
            references Upload
