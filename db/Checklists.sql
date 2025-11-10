SELECT * FROM Checklists
SELECT * FROM ChecklistInstances
SELECT * FROM ChecklistInstanceItems

INSERT INTO Checklists (UserId, Title) VALUES (3,'Sidewinder CCR Checklist')
INSERT INTO ChecklistInstances (ChecklistId, Title) VALUES (1, 'Sidewinder CCR Checklist')

SELECT c.Title AS 'Checklist', SequenceNumber, ci.Title AS 'Step'
FROM ChecklistItems ci
JOIN Checklists c ON ci.ChecklistId = c.ChecklistId


INSERT INTO ChecklistInstanceItems (ChecklistInstanceId, SequenceNumber, Title, IsChecked)
SELECT ci.ChecklistId, ci.SequenceNumber, ci.Title, 0
FROM ChecklistItems ci
WHERE ci.ChecklistId = 1
ORDER BY ci.SequenceNumber;