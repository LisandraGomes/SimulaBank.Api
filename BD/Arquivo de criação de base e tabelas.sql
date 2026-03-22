create database SimulaBankBase;

Use SimulaBankBase;
CREATE LOGIN app_apicore
WITH PASSWORD = '@simula!Bank';
CREATE USER app_apicore
FOR LOGIN app_apicore;
ALTER ROLE db_datareader ADD MEMBER app_apicore;
ALTER ROLE db_datawriter ADD MEMBER app_apicore;

CREATE TABLE [User] (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    FirstName NVARCHAR(100) NOT NULL,
    MidName NVARCHAR(100) NOT NULL,
    Cpf NVARCHAR(11) NOT NULL,
    Email NVARCHAR(500) NOT NULL,
    [Password] NVARCHAR(400) NULL,
    BirthDate DATETIME2 NOT NULL,
    RegistrationDate DATETIME2 NOT NULL,
    Active BIT NOT NULL DEFAULT 0,
    EmailAuthorization BIT NOT NULL DEFAULT 0,
    DateEmailAuthorization DATETIME2 NULL,
    IdRole INT NOT NULL CONSTRAINT FK_Role_User FOREIGN KEY (IdRole) REFERENCES [Role](Id)
);

INSERT INTO [User]
( Id, FirstName, MidName, Cpf, Email, [Password], BirthDate, RegistrationDate, Active, EmailAthorization, IdRole )
VALUES
-- (NEWID(),'Maria','Souza','98765432100','maria@email.com','hash1','1985-03-12',GETDATE(),1,1,2),
-- (NEWID(),'Carlos','Oliveira','45678912300','carlos@email.com','hash2','1992-07-20',GETDATE(),1,0,2),
(NEWID(),'Usuario', 'Master','07601499502','lisandragomes53@gmail.com',NULL,'2000-07-23 16:30:25','2026-03-14 14:30:25',1,1,1);

GRANT SELECT, INSERT, UPDATE ON [User] TO app_apicore;

--Criar tabela de permissões para o usuario
CREATE TABLE [Role] (
    Id INT NOT NULL CONSTRAINT PK_RoleUser PRIMARY KEY IDENTITY(1, 1),
    RoleName NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(255) NULL,
    Active BIT NOT NULL DEFAULT 1
);

INSERT INTO [Role] Values('Master', 'UsuarioMaster', 1);
INSERT INTO [Role] Values('InvetstorCliente', 'Inverstidor dos cofrinhos', 1);

GRANT SELECT ON [Role] TO app_apicore;

CREATE TABLE Permission (
    Id INT NOT NULL CONSTRAINT PK_Permissao PRIMARY KEY IDENTITY(1,1),
    PermissionName NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(255) NULL,
    RoleId INT NOT NULL,
    Active BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Role_id_permission FOREIGN KEY(RoleId) REFERENCES [Role](Id)
);

INSERT INTO [Permission] (PermissionName, [Description], RoleId) VALUES('little_box', 'Acesso a realizar caixinhas proprias.',2),
('information_user','Acesso as informações do usuários na tela.',2);

GRANT SELECT ON [Permission] TO app_apicore;

CREATE TABLE UserPermissions (
    Id UNIQUEIDENTIFIER NOT NULL 
        CONSTRAINT PK_Permission PRIMARY KEY 
        DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    PermissionId INT NOT NULL,
    Active BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_User_Permission
        FOREIGN KEY (UserId)
        REFERENCES [User](Id)
);

GRANT SELECT ON [UserPermissions] TO app_apicore;


CREATE TABLE Piggy (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [Title] NVARCHAR(200) NOT NULL,
    [DESCRIPTION] NVARCHAR(2000) NULL,
    GoalValue DECIMAL NULL,
    CurrenteValue DECIMAL NOT NULL DEFAULT 0,
    [Status] INT NOT NULL,
    CreateDate DATETIME2 NOT NULL,
    DueDate DATETIME2 NOT NULL,
    DayAutoDeductValueAccount INT NULL,
    ValueAutoDeductValueAccount DECIMAL NULL,
    ActiveAutoDeduct BIT NOT NULL DEFAULT 0,
    Active BIT NOT NULL DEFAULT 1,
    UserId UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT FK_UserId_Piggy FOREIGN KEY (UserId) REFERENCES [User](Id),
    CONSTRAINT FK_Status_Piggy FOREIGN KEY ([Status]) REFERENCES [Status](Id)
);

GRANT SELECT ON [Piggy] TO app_apicore;

CREATE TABLE History_Piggy (
     Id INT NOT NULL IDENTITY(1,1) CONSTRAINT PK_History_Piggy PRIMARY KEY,
     CreateDate DATETIME2 NOT NULL,
     [CurrenteValue] DECIMAL NOT NULL,
     ValueTransaction DECIMAL NOT NULL,
     TransactionDate DATETIME2 NOT NULL,
     PiggyId UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_History_Piggy_Piggy FOREIGN KEY (PiggyId) REFERENCES [Piggy](Id) 
);

GRANT SELECT ON [History_Piggy] TO app_apicore;

CREATE TABLE [Status] (
    Id INT NOT NULL CONSTRAINT PK_Status_Id PRIMARY KEY,
    [Description] NVARCHAR(500) NOT NULL,
    Active BIT NOT NULL DEFAULT 1
);
INSERT INTO [STATUS](Id, [Description]) VALUES(1, 'Em progresso'),
(2, 'Concluído'), (3,'Cancelado'), (4, 'Pausado');