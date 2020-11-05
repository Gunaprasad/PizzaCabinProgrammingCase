ALTER TABLE Projection
ADD CONSTRAINT FK_ProjectionSchedule
FOREIGN KEY (PersonId) REFERENCES Schedule(PersonId);