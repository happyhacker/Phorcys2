/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases 14.6.0                     */
/* Target DBMS:           MS SQL Server 2022                              */
/* Project file:          Phorcys2.dez                                    */
/* Project name:          Phorcys2                                        */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2025-08-06 11:39                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

GO


ALTER TABLE [GasMixes] DROP CONSTRAINT [TanksOnDive_GasMixes]
GO


ALTER TABLE [GasMixes] DROP CONSTRAINT [Gases_GasMixes]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "GasMixes"                                                  */
/* ---------------------------------------------------------------------- */

GO


/* Drop constraints */

ALTER TABLE [GasMixes] DROP CONSTRAINT [PK__GasMixes__56FD4E221D9B5BB6]
GO


EXECUTE sp_dropextendedproperty N'MS_Description', 'SCHEMA', N'dbo', 'TABLE', N'GasMixes', 'COLUMN', N'DivePlanId'
GO


EXECUTE sp_dropextendedproperty N'MS_Description', 'SCHEMA', N'dbo', 'TABLE', N'GasMixes', 'COLUMN', N'GearId'
GO


EXECUTE sp_dropextendedproperty N'MS_Description', 'SCHEMA', N'dbo', 'TABLE', N'GasMixes', 'COLUMN', N'GasId'
GO


EXECUTE sp_dropextendedproperty N'MS_Description', 'SCHEMA', N'dbo', 'TABLE', N'GasMixes', 'COLUMN', N'VolumeAdded'
GO


EXECUTE sp_dropextendedproperty N'MS_Description', 'SCHEMA', N'dbo', 'TABLE', N'GasMixes', 'COLUMN', N'Percentage'
GO


EXECUTE sp_dropextendedproperty N'MS_Description', 'SCHEMA', N'dbo', 'TABLE', N'GasMixes', 'COLUMN', N'CostPerVolumeOfMeasure'
GO


EXECUTE sp_dropextendedproperty N'MS_Description', 'SCHEMA', N'dbo', 'TABLE', N'GasMixes', NULL, NULL
GO


DROP TABLE [GasMixes]
GO


/* ---------------------------------------------------------------------- */
/* Drop table "Gases"                                                     */
/* ---------------------------------------------------------------------- */

GO


/* Drop constraints */

ALTER TABLE [Gases] DROP CONSTRAINT [PK_Gases]
GO


DROP TABLE [Gases]
GO

