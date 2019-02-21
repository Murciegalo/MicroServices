
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/14/2019 09:30:38
-- Generated from EDMX file: C:\Users\admin\Documents\GitHub\NET0119ProyectoD\ProyectoD\ProyectoD\ModelDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ProyectoD];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ConductorCoche]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PersonaSet_Conductor] DROP CONSTRAINT [FK_ConductorCoche];
GO
IF OBJECT_ID(N'[dbo].[FK_ConductorViaje]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ViajeSet] DROP CONSTRAINT [FK_ConductorViaje];
GO
IF OBJECT_ID(N'[dbo].[FK_ClienteViaje]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ViajeSet] DROP CONSTRAINT [FK_ClienteViaje];
GO
IF OBJECT_ID(N'[dbo].[FK_CocheFlota]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FlotaSet] DROP CONSTRAINT [FK_CocheFlota];
GO
IF OBJECT_ID(N'[dbo].[FK_Conductor_inherits_Persona]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PersonaSet_Conductor] DROP CONSTRAINT [FK_Conductor_inherits_Persona];
GO
IF OBJECT_ID(N'[dbo].[FK_Cliente_inherits_Persona]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PersonaSet_Cliente] DROP CONSTRAINT [FK_Cliente_inherits_Persona];
GO
IF OBJECT_ID(N'[dbo].[FK_Admin_inherits_Persona]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PersonaSet_Admin] DROP CONSTRAINT [FK_Admin_inherits_Persona];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[PersonaSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonaSet];
GO
IF OBJECT_ID(N'[dbo].[ViajeSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ViajeSet];
GO
IF OBJECT_ID(N'[dbo].[CocheSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CocheSet];
GO
IF OBJECT_ID(N'[dbo].[FlotaSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FlotaSet];
GO
IF OBJECT_ID(N'[dbo].[PersonaSet_Conductor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonaSet_Conductor];
GO
IF OBJECT_ID(N'[dbo].[PersonaSet_Cliente]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonaSet_Cliente];
GO
IF OBJECT_ID(N'[dbo].[PersonaSet_Admin]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonaSet_Admin];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'PersonaSet'
CREATE TABLE [dbo].[PersonaSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [foto] nvarchar(max)  NULL,
    [dni] nvarchar(max)  NOT NULL,
    [fechanacimiento] datetime  NOT NULL,
    [contrasena] nvarchar(max)  NOT NULL,
    [cuentabancaria] nvarchar(max)  NULL,
    [telefono] nvarchar(max)  NULL,
    [email] nvarchar(max)  NULL
);
GO

-- Creating table 'ViajeSet'
CREATE TABLE [dbo].[ViajeSet] (
    [IdViaje] int IDENTITY(1,1) NOT NULL,
    [origen] nvarchar(max)  NOT NULL,
    [destino] nvarchar(max)  NOT NULL,
    [distancia] nvarchar(max)  NOT NULL,
    [horainicio] datetime  NOT NULL,
    [horafinal] datetime  NOT NULL,
    [tarifa] nvarchar(max)  NOT NULL,
    [puntuacion] int  NULL,
    [IdCliente] int  NOT NULL,
    [IdConductor] int  NOT NULL,
    [estadoViaje] nvarchar(max)  NOT NULL,
    [comentarioCliente] nvarchar(max)  NULL
);
GO

-- Creating table 'CocheSet'
CREATE TABLE [dbo].[CocheSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [matricula] nvarchar(max)  NOT NULL,
    [modelo] nvarchar(max)  NOT NULL,
    [color] nvarchar(max)  NOT NULL,
    [plaza] nvarchar(max)  NOT NULL,
    [distintivoambiental] nvarchar(max)  NOT NULL,
    [mascota] nvarchar(max)  NOT NULL,
    [fumar] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'FlotaSet'
CREATE TABLE [dbo].[FlotaSet] (
    [IdFlota] int IDENTITY(1,1) NOT NULL,
    [idCoche] int  NOT NULL,
    [longitud] nvarchar(max)  NOT NULL,
    [timeStampUbicacion] nvarchar(max)  NOT NULL,
    [disponibilidadCoche] nvarchar(max)  NOT NULL,
    [timeStampDisponibilidad] nvarchar(max)  NOT NULL,
    [Latitud] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'PersonaSet_Conductor'
CREATE TABLE [dbo].[PersonaSet_Conductor] (
    [licencia] nvarchar(max)  NOT NULL,
    [puntuacion] int  NULL,
    [estado] nvarchar(max)  NULL,
    [IdCoche] int  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'PersonaSet_Cliente'
CREATE TABLE [dbo].[PersonaSet_Cliente] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'PersonaSet_Admin'
CREATE TABLE [dbo].[PersonaSet_Admin] (
    [Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'PersonaSet'
ALTER TABLE [dbo].[PersonaSet]
ADD CONSTRAINT [PK_PersonaSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [IdViaje] in table 'ViajeSet'
ALTER TABLE [dbo].[ViajeSet]
ADD CONSTRAINT [PK_ViajeSet]
    PRIMARY KEY CLUSTERED ([IdViaje] ASC);
GO

-- Creating primary key on [Id] in table 'CocheSet'
ALTER TABLE [dbo].[CocheSet]
ADD CONSTRAINT [PK_CocheSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [IdFlota] in table 'FlotaSet'
ALTER TABLE [dbo].[FlotaSet]
ADD CONSTRAINT [PK_FlotaSet]
    PRIMARY KEY CLUSTERED ([IdFlota] ASC);
GO

-- Creating primary key on [Id] in table 'PersonaSet_Conductor'
ALTER TABLE [dbo].[PersonaSet_Conductor]
ADD CONSTRAINT [PK_PersonaSet_Conductor]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PersonaSet_Cliente'
ALTER TABLE [dbo].[PersonaSet_Cliente]
ADD CONSTRAINT [PK_PersonaSet_Cliente]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PersonaSet_Admin'
ALTER TABLE [dbo].[PersonaSet_Admin]
ADD CONSTRAINT [PK_PersonaSet_Admin]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [IdCoche] in table 'PersonaSet_Conductor'
ALTER TABLE [dbo].[PersonaSet_Conductor]
ADD CONSTRAINT [FK_ConductorCoche]
    FOREIGN KEY ([IdCoche])
    REFERENCES [dbo].[CocheSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ConductorCoche'
CREATE INDEX [IX_FK_ConductorCoche]
ON [dbo].[PersonaSet_Conductor]
    ([IdCoche]);
GO

-- Creating foreign key on [IdConductor] in table 'ViajeSet'
ALTER TABLE [dbo].[ViajeSet]
ADD CONSTRAINT [FK_ConductorViaje]
    FOREIGN KEY ([IdConductor])
    REFERENCES [dbo].[PersonaSet_Conductor]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ConductorViaje'
CREATE INDEX [IX_FK_ConductorViaje]
ON [dbo].[ViajeSet]
    ([IdConductor]);
GO

-- Creating foreign key on [IdCliente] in table 'ViajeSet'
ALTER TABLE [dbo].[ViajeSet]
ADD CONSTRAINT [FK_ClienteViaje]
    FOREIGN KEY ([IdCliente])
    REFERENCES [dbo].[PersonaSet_Cliente]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClienteViaje'
CREATE INDEX [IX_FK_ClienteViaje]
ON [dbo].[ViajeSet]
    ([IdCliente]);
GO

-- Creating foreign key on [idCoche] in table 'FlotaSet'
ALTER TABLE [dbo].[FlotaSet]
ADD CONSTRAINT [FK_CocheFlota]
    FOREIGN KEY ([idCoche])
    REFERENCES [dbo].[CocheSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CocheFlota'
CREATE INDEX [IX_FK_CocheFlota]
ON [dbo].[FlotaSet]
    ([idCoche]);
GO

-- Creating foreign key on [Id] in table 'PersonaSet_Conductor'
ALTER TABLE [dbo].[PersonaSet_Conductor]
ADD CONSTRAINT [FK_Conductor_inherits_Persona]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[PersonaSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'PersonaSet_Cliente'
ALTER TABLE [dbo].[PersonaSet_Cliente]
ADD CONSTRAINT [FK_Cliente_inherits_Persona]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[PersonaSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'PersonaSet_Admin'
ALTER TABLE [dbo].[PersonaSet_Admin]
ADD CONSTRAINT [FK_Admin_inherits_Persona]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[PersonaSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------