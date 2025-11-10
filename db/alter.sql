CREATE TABLE [ChecklistInstances] (
    [ChecklistInstanceId] INTEGER IDENTITY(1,1) NOT NULL,
    [ChecklistId] INTEGER NOT NULL,
    [Title] VARCHAR(120) NOT NULL,
    [Created] DATETIME2 CONSTRAINT [DEF_ChecklistInstances_Created] DEFAULT getdate() NOT NULL,
    CONSTRAINT [PK_ChecklistInstances] PRIMARY KEY ([ChecklistInstanceId])
)
GO

CREATE TABLE [ChecklistItems] (
    [ChecklistItemId] INTEGER IDENTITY(1,1) NOT NULL,
    [ChecklistId] INTEGER NOT NULL,
    [Title] VARCHAR(120) NOT NULL,
    [SequenceNumber] INTEGER,
    [Created] DATETIME2 CONSTRAINT [DEF_ChecklistItems_Created] DEFAULT getdate(),
    CONSTRAINT [PK_ChecklistItems] PRIMARY KEY ([ChecklistItemId])
)
GO

CREATE TABLE [ChecklistInstanceItems] (
    [ChecklistInstanceItemId] INTEGER IDENTITY(1,1) NOT NULL,
    [ChecklistInstanceId] INTEGER NOT NULL,
    [Title] VARCHAR(120) NOT NULL,
    [SequenceNumber] INTEGER,
    [IsChecked] BIT CONSTRAINT [DEF_ChecklistInstanceItems_IsChecked] DEFAULT 0 NOT NULL,
    [Created] DATETIME2 CONSTRAINT [DEF_ChecklistInstanceItems_Created] DEFAULT getdate() NOT NULL,
    CONSTRAINT [PK_ChecklistInstanceItems] PRIMARY KEY ([ChecklistInstanceItemId])
)
GO

CREATE TABLE [Checklists] (
    [ChecklistId] INTEGER IDENTITY(1,1) NOT NULL,
    [UserId] INTEGER NOT NULL,
    [Title] VARCHAR(80) NOT NULL,
    [Created] DATETIME2 CONSTRAINT [DEF_Checklists_Created] DEFAULT getdate() NOT NULL,
    [LastModified] DATETIME2,
    CONSTRAINT [PK_Checklists] PRIMARY KEY ([ChecklistId])
)
GO

ALTER TABLE [ChecklistInstances] ADD CONSTRAINT [Checklists_ChecklistInstances] 
    FOREIGN KEY ([ChecklistId]) REFERENCES [Checklists] ([ChecklistId])
GO

ALTER TABLE [ChecklistItems] ADD CONSTRAINT [Checklists_ChecklistItems] 
    FOREIGN KEY ([ChecklistId]) REFERENCES [Checklists] ([ChecklistId])
GO

ALTER TABLE [ChecklistInstanceItems] ADD CONSTRAINT [ChecklistInstances_ChecklistInstanceItems] 
    FOREIGN KEY ([ChecklistInstanceId]) REFERENCES [ChecklistInstances] ([ChecklistInstanceId]) ON DELETE CASCADE
GO

ALTER TABLE [Checklists] ADD CONSTRAINT [Users_Checklists] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO

