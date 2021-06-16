	/* ---------------------------------------------------- */
/*  Generated by Enterprise Architect Version 13.5 		*/
/*  Created On : 13-jun.-2021 11:30:56 p. m. 				*/
/*  DBMS       : SQL Server 2012 						*/
/* ---------------------------------------------------- */

/* Drop Foreign Key Constraints */

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[FK_Abono_Movimiento]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1) 
ALTER TABLE [Abono] DROP CONSTRAINT [FK_Abono_Movimiento]
GO

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[FK_Cargo_Movimiento]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1) 
ALTER TABLE [Cargo] DROP CONSTRAINT [FK_Cargo_Movimiento]
GO

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[FK_Cuenta_Tiene]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1) 
ALTER TABLE [Cuenta] DROP CONSTRAINT [FK_Cuenta_Tiene]
GO

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[FK_Cuenta_Apertura]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1) 
ALTER TABLE [Cuenta] DROP CONSTRAINT [FK_Cuenta_Apertura]
GO

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[FK_Movimiento_Realiza]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1) 
ALTER TABLE [Movimiento] DROP CONSTRAINT [FK_Movimiento_Realiza]
GO

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[FK_Movimiento_Tiene]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1) 
ALTER TABLE [Movimiento] DROP CONSTRAINT [FK_Movimiento_Tiene]
GO

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[FK_Tarjeta_Cuenta]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1) 
ALTER TABLE [Tarjeta] DROP CONSTRAINT [FK_Tarjeta_Cuenta]
GO

/* Drop Tables */

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[Abono]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) 
DROP TABLE [Abono]
GO

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[Cajero]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) 
DROP TABLE [Cajero]
GO

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[Cargo]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) 
DROP TABLE [Cargo]
GO

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[Cliente]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) 
DROP TABLE [Cliente]
GO

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[Cuenta]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) 
DROP TABLE [Cuenta]
GO

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[Gerente]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) 
DROP TABLE [Gerente]
GO

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[Movimiento]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) 
DROP TABLE [Movimiento]
GO

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[Tarjeta]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) 
DROP TABLE [Tarjeta]
GO

/* Create Tables */

CREATE TABLE [Abono]
(
	[Monto] float NULL,
	[AbonoID] int NOT NULL
)
GO

CREATE TABLE [Cajero]
(
	[Apellidos] varchar(100) NULL,
	[Nombre] varchar(50) NULL,
	[Contrasenia] varchar(150) NULL,
	[CajeroID] varchar(100) NOT NULL
)
GO

CREATE TABLE [Cargo]
(
	[Interes] float NULL,
	[Monto] float NULL,
	[CargoID] int NOT NULL
)
GO

CREATE TABLE [Cliente]
(
	[Apellidos] varchar(100) NULL,
	[Direcci�n] varchar(200) NULL,
	[Nombre] varchar(50) NULL,
	[Contrasenia] varchar(150) NULL,
	[ClienteID] varchar(100) NOT NULL
)
GO

CREATE TABLE [Cuenta]
(
	[Fechacorte] date NULL,
	[Montoinicial] float NULL,
	[Saldo] float NULL,
	[CuentaID] varchar(50) NOT NULL,
	[ClienteID] varchar(100) NULL,
	[GerenteID] varchar(100) NULL
)
GO

CREATE TABLE [Gerente]
(
	[Apellidos] varchar(100) NULL,
	[Nombre] varchar(50) NULL,
	[Contrasenia] varchar(150) NULL,
	[GerenteID] varchar(100) NOT NULL
)
GO

CREATE TABLE [Movimiento]
(
	[Concepto] varchar(100) NULL,	
	[Fecha] date NULL,
	[MovimientoID] int NOT NULL IDENTITY (1, 1),
	[CajeroID] varchar(100) NULL,
	[CuentaID] varchar(50) NULL
)
GO

CREATE TABLE [Tarjeta]
(
	[Fechaexpiracion] date NULL,
	[Nombreentarjeta] varchar (150) NULL,
	[Numtarjeta] int NULL,
	[Pincode] int NULL,
	[TarjetaID] varchar(50) NOT NULL,
	[CuentaID] varchar(50) NULL
)
GO

/* Create Primary Keys, Indexes, Uniques, Checks */

ALTER TABLE [Abono] 
 ADD CONSTRAINT [PK_Abono]
	PRIMARY KEY CLUSTERED ([AbonoID] ASC)
GO

ALTER TABLE [Cajero] 
 ADD CONSTRAINT [PK_Cajero]
	PRIMARY KEY CLUSTERED ([CajeroID] ASC)
GO

ALTER TABLE [Cargo] 
 ADD CONSTRAINT [PK_Cargo]
	PRIMARY KEY CLUSTERED ([CargoID] ASC)
GO

ALTER TABLE [Cliente] 
 ADD CONSTRAINT [PK_Cliente]
	PRIMARY KEY CLUSTERED ([ClienteID] ASC)
GO

ALTER TABLE [Cuenta] 
 ADD CONSTRAINT [PK_Cuenta]
	PRIMARY KEY CLUSTERED ([CuentaID] ASC)
GO

ALTER TABLE [Gerente] 
 ADD CONSTRAINT [PK_Gerente]
	PRIMARY KEY CLUSTERED ([GerenteID] ASC)
GO

ALTER TABLE [Movimiento] 
 ADD CONSTRAINT [PK_Movimiento]
	PRIMARY KEY CLUSTERED ([MovimientoID] ASC)
GO

ALTER TABLE [Tarjeta] 
 ADD CONSTRAINT [PK_Tarjeta]
	PRIMARY KEY CLUSTERED ([TarjetaID] ASC)
GO

/* Create Foreign Key Constraints */

ALTER TABLE [Abono] ADD CONSTRAINT [FK_Abono_Movimiento]
	FOREIGN KEY ([AbonoID]) REFERENCES [Movimiento] ([MovimientoID]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [Cargo] ADD CONSTRAINT [FK_Cargo_Movimiento]
	FOREIGN KEY ([CargoID]) REFERENCES [Movimiento] ([MovimientoID]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [Cuenta] ADD CONSTRAINT [FK_Cuenta_Tiene]
	FOREIGN KEY ([ClienteID]) REFERENCES [Cliente] ([ClienteID]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [Cuenta] ADD CONSTRAINT [FK_Cuenta_Apertura]
	FOREIGN KEY ([GerenteID]) REFERENCES [Gerente] ([GerenteID]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [Movimiento] ADD CONSTRAINT [FK_Movimiento_Realiza]
	FOREIGN KEY ([CajeroID]) REFERENCES [Cajero] ([CajeroID]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [Movimiento] ADD CONSTRAINT [FK_Movimiento_Tiene]
	FOREIGN KEY ([CuentaID]) REFERENCES [Cuenta] ([CuentaID]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [Tarjeta] ADD CONSTRAINT [FK_Tarjeta_Cuenta]
	FOREIGN KEY ([CuentaID]) REFERENCES [Cuenta] ([CuentaID]) ON DELETE No Action ON UPDATE No Action
GO
