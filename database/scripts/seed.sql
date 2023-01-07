CREATE OR REPLACE FUNCTION InsertUser 
    (vId varchar(50), 
    vFirstName varchar(250), 
    vLastName varchar(250), 
    vGovernmentId varchar(250), 
    vGovernmentIdType integer, 
    vJobType integer)
RETURNS void
LANGUAGE SQL
AS $f$
    INSERT INTO "Users" ("Id", "PersonalData_FirstName", "PersonalData_LastName", "PersonalData_GovernmentId",
    "PersonalData_GovernmentIdType", "PersonalData_JobType")
    VALUES (vId, vFirstName, vLastName, vGovernmentId, vGovernmentIdType, vJobType)
$f$;

SELECT InsertUser ('TestID2', 'Jan', 'Kowalski', '72010443599', 0, 0);
