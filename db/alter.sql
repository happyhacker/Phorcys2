/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases 14.6.0                     */
/* Target DBMS:           MS SQL Server 2022                              */
/* Project file:          Phorcys2.dez                                    */
/* Project name:          Phorcys2                                        */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2025-11-30 17:13                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

GO


ALTER TABLE [ChecklistItems] DROP CONSTRAINT [Checklists_ChecklistItems]
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

GO


ALTER TABLE [ChecklistItems] ADD CONSTRAINT [Checklists_ChecklistItems] 
    FOREIGN KEY ([ChecklistId]) REFERENCES [Checklists] ([ChecklistId]) ON DELETE CASCADE
GO

