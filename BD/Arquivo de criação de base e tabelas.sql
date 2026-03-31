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

GRANT SELECT, INSERT, UPDATE ON [Piggy] TO app_apicore;

CREATE TABLE HistoryPiggy (
     Id INT NOT NULL IDENTITY(1,1) CONSTRAINT PK_History_Piggy PRIMARY KEY,
     CreateDate DATETIME2 NOT NULL,
     [CurrenteValue] DECIMAL NOT NULL,
     ValueTransaction DECIMAL NOT NULL,
     TransactionDate DATETIME2 NOT NULL,
     UserCreate NVARCHAR(100) NOT NULL,
     TypeHistoryId int NOT NULL CONSTRAINT FK_Hitory_Piggy_Type FOREIGN KEY (TypeHistoryId) REFERENCES [HistoryType](Id),
     PiggyId UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_History_Piggy_Piggy FOREIGN KEY (PiggyId) REFERENCES [Piggy](Id) 
);

GRANT SELECT, INSERT ON [History_Piggy] TO app_apicore;

CREATE TABLE [Status] (
    Id INT NOT NULL CONSTRAINT PK_Status_Id PRIMARY KEY,
    [Description] NVARCHAR(500) NOT NULL,
    Active BIT NOT NULL DEFAULT 1
);
INSERT INTO [STATUS](Id, [Description]) VALUES(1, 'Em progresso'),
(2, 'Concluído'), (3,'Cancelado'), (4, 'Pausado');
GRANT SELECT ON [Status] TO app_apicore;

CREATE TABLE HistoryType (
    Id INT NOT NULL IDENTITY(1,1) CONSTRAINT PK_History_Type PRIMARY KEY,
    [DESCRIPTION] NVARCHAR(500) NOT NULL,
    [Name] NVARCHAR(150) NOT NULL,
    CreateDate DATETIME2 NOT NULL,
    UserCreate NVARCHAR(100) NOT NULL,
    UpdateDate DATETIME2 NULL,
    UserUpdate NVARCHAR(100) NULL
);
GRANT SELECT ON [HistoryPiggy] TO app_apicore;

INSERT INTO HistoryType([DESCRIPTION], [Name], CreateDate, UserCreate)
VALUES('Foi criado por {0} - Por: {1}','Criação', GETDATE(), 'System'),
('A meta foi atualizada por {0} para {1} - Por: {2}', 'Atualização da Meta', GETDATE(), 'System'),
('Foi alterado o nome de {0} para  {1} - Por: {2}', 'Alteração do Nome', GETDATE(), 'System'),
('Depósito realizado: {0} , está mais perto da sua meta! - Por: {1}','Depósito', GETDATE(), 'System'),
('Retirada realizada: {0} - Por: {1}','Retirada', GETDATE(), 'System'),
('Parabéns você Alcançou sua meta! - Por: {1}','Meta Alcançada', GETDATE(), 'System'),
('Seu valor rendeu: {0}','Rendimentos', GETDATE(), 'System'),
('Cofrinho excluido por {0}', 'Exclusão', GETDATE(), 'System');

CREATE TABLE [Transaction] (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [Value] DECIMAL NOT NULL,
    TypeId INT NOT NULL CONSTRAINT FK_Transaction_TypeId FOREIGN KEY (TypeId) REFERENCES [TransactionType](Id),
    IdAccountOrigin NVARCHAR(500) NULL, 
    IdAccountDestination NVARCHAR(500) NULL,
    DateCreate DATETIME2 NOT NULL,
    DateFinally DATETIME2 NOT NULL,
    IdUser UNIQUEIDENTIFIER NOT NULL FOREIGN KEY (IdUser) REFERENCES [User](Id),
    Active BIT NOT NULL DEFAULT 1
);
GRANT SELECT, INSERT, UPDATE ON [Transaction] TO app_apicore;

CREATE TABLE TransactionType (
    Id INT NOT NULL CONSTRAINT PK_Transaction_Type PRIMARY KEY,
    [Name] NVARCHAR(200) NOT NULL,
    [Active] BIT NOT NULL DEFAULT 1
);
GRANT SELECT ON [TransactionType] TO app_apicore;

INSERT INTO TransactionType (Id, [Name], [Active]) VALUES
(1, 'Depósito',1),
(2, 'Saque', 1),
(3, 'Transferência', 1);

CREATE TABLE [Account] (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    AccountNumber NVARCHAR(50) NOT NULL,
    Balance DECIMAL(18,2) NOT NULL DEFAULT 0,
    DateCreate DATETIME2 NOT NULL DEFAULT GETDATE(),
    Active BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Account_User FOREIGN KEY (UserId) REFERENCES [User](Id)
);
GRANT SELECT, INSERT, UPDATE ON [Account] TO app_apicore;

CREATE TABLE [PatternEmail](
    Id INT NOT NULL UNIQUE,
    [Subject] NVARCHAR(500) NOT NULL,
    [Body] TEXT NOT NULL,
    DateCreate DATETIME2 NOT NULL DEFAULT GETDATE()
);
GRANT SELECT ON [PatternEmail] TO app_apicore;

INSERT INTO PatternEmail (Id, [Subject], [Body])
VALUES (1,'Confirme seu cadastro', '<!DOCTYPE html>
<html lang="pt">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Confirme seu cadastro</title>
    <style>
        /* Estilos básicos para garantir responsividade */
        body { margin: 0; padding: 0; background-color: #fafafa; font-family: Arial, sans-serif; }
        .container { width: 100%; max-width: 600px; margin: 0 auto; background-color: #ffffff; }
        .content { padding: 40px 20px; text-align: center; }
        .button { 
            display: inline-block; 
            padding: 15px 30px; 
            background-color: #6aa84f; 
            color: #ffffff !important; 
            text-decoration: none; 
            border-radius: 6px; 
            font-weight: bold; 
            font-size: 18px; 
        }
        h1 { color: #333333; font-size: 28px; margin-bottom: 20px; }
        p { color: #666666; font-size: 16px; line-height: 1.5; margin-bottom: 20px; }
        .footer { font-size: 12px; color: #999999; margin-top: 30px; }
    </style>
</head>
<body>
    <table role="presentation" width="100%" cellspacing="0" cellpadding="0" border="0" bgcolor="#fafafa">
        <tr>
            <td align="center">
                <table class="container" role="presentation" width="600" cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td class="content" style="padding-bottom: 0;">
                            <img src="https://ezpwiba.stripocdn.email/content/guids/CABINET_167198e43a75a17fecce46fedce7a4f5e2c11cfe2ba36813b3ece3281207dae5/images/sbsf.png" alt="Logo" width="120" style="display: block; margin: 0 auto;">
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="content">
                            <img src="https://ezpwiba.stripocdn.email/content/guids/CABINET_67e080d830d87c17802bd9b4fe1c0912/images/55191618237638326.png" alt="" width="80" style="margin-bottom: 20px;">
                            
                            <h1>Confirme seu cadastro</h1>
                            
                            <p>Olá <strong>{0}</strong>, obrigado por se registrar em nosso Banco. Esperamos que seja uma ótima aventura!</p>
                            
                            <p>Para ativar sua conta e começar a usar o <strong>SimulaBank</strong>, clique no botão abaixo:</p>
                            
                            <div style="margin: 30px 0;">
                                <a href="https://localhost:4200/confirmacao?token={1}&email={2}&accepted=true" class="button">Confirmar Cadastro</a>
                            </div>
                            
                            <p class="footer">Se você não realizou este cadastro, pode ignorar este e-mail com segurança.</p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>');

CREATE TABLE [HistoryEmails]
(
    Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    EmailUser NVARCHAR(500) NOT NULL,
    PatternEmailId INT NOT NULL CONSTRAINT FK_PatternEmailId FOREIGN KEY (PatternEmailId) REFERENCES [PatternEmail](Id),
    [Send] BIT NOT NULL DEFAULT 0,
    [DateCreate] DateTime2 NOT NULL DEFAULT GETDATE(),
    [DateSend] DATETIME2 NULL,
    Click BIT NOT NULL DEFAULT 0
);
GRANT SELECT, INSERT, UPDATE ON [HistoryEmails] TO app_apicore;
