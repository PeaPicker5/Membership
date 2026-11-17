/*
    001_AddKeys.sql  -  Membership database
    Adds primary keys and foreign keys that the tables have never had.

    BEFORE RUNNING
      1. Back up the database:
           BACKUP DATABASE Membership TO DISK = 'C:\Backups\Membership_before_keys.bak' WITH INIT;
      2. Resolve the 4 members whose MemberTypeID (41, 42, 43) is not in MEMBER_Type.
         See STEP 0 - the script stops if any remain.

    The whole script runs in one transaction: if any step fails, nothing is changed.
    It is safe to run more than once (each key is only added if it doesn't exist).

    WHAT IT DOES
      Data clean-up
        - OFFICE_Assignments: fills the 107 rows (1950-1999) with no From/To dates as
          full-year terms (Jan 1 - Dec 1 of that Year, the same values the app writes
          for a full year), then makes both columns NOT NULL.
        - OFFICE_Assignments: deletes the extra copy of 3 exact duplicate rows.
      Primary keys
        - MEETING_List(MeetingID), SYS_FileStore(ImageId), MEMBER_RemovalCodes(ReasonID)
        - DUES_History(MemberID, Year), LOSAP_History(MemberID, Year)
        - MEETING_Members(MeetingID, MemberID)
        - OFFICE_Assignments(MemberId, OfficeId, Year, FromDate)
      Foreign keys
        - DUES_History, LOSAP_History, MEETING_Members, OFFICE_Assignments -> MEMBER_List
          ON DELETE CASCADE (deleting a member removes their history)
        - MEETING_Members -> MEETING_List ON DELETE CASCADE
        - OFFICE_Assignments -> OFFICE_List (no cascade: an office in use can't be deleted)
        - MEMBER_List.MemberTypeID -> MEMBER_Type

    DELIBERATELY NOT ADDED (the app stores 0 / an empty GUID for "none", which a
    foreign key would reject until the app writes NULL instead):
        - MEMBER_List.RemovalReasonId -> MEMBER_RemovalCodes   (561 members have 0)
        - MEMBER_List.PageId          -> SYS_FileStore         (all 909 are empty)
        - MEMBER_List.SponsorID1/2/3  -> MEMBER_List           (700+ are empty)
        - MEETING_List.InCharge       -> MEMBER_List

    NOTE: one assignment looks mistyped and is left as-is - member
    4C9B1615-8080-4D2F-9C3D-FC97BF17A1D7, office 25, Year 1990, FromDate 2020-01-01.
*/

SET XACT_ABORT ON;
SET NOCOUNT ON;
USE Membership;
GO

BEGIN TRANSACTION;

------------------------------------------------------------------------------
-- STEP 0  Pre-flight: member types that don't exist in MEMBER_Type
------------------------------------------------------------------------------
-- Decide what 41/42/43 should be, then either add them to MEMBER_Type or move
-- those members to an existing type, e.g.:
--     INSERT INTO MEMBER_Type (TypeId, Description, DuesAmount) VALUES (41, '...', 0);
--   or
--     UPDATE MEMBER_List SET MemberTypeID = 9 /* Unknown */ WHERE MemberTypeID IN (41, 42, 43);
IF EXISTS (SELECT 1 FROM MEMBER_List m
           WHERE NOT EXISTS (SELECT 1 FROM MEMBER_Type t WHERE t.TypeId = m.MemberTypeID))
    THROW 50001, 'Some members have a MemberTypeID that is not in MEMBER_Type. Fix them (see STEP 0) and re-run.', 1;

------------------------------------------------------------------------------
-- STEP 1  OFFICE_Assignments data clean-up
------------------------------------------------------------------------------
UPDATE OFFICE_Assignments
SET    FromDate = DATEFROMPARTS(Year, 1, 1)
WHERE  FromDate IS NULL;

UPDATE OFFICE_Assignments
SET    ToDate = DATEFROMPARTS(Year, 12, 1)
WHERE  ToDate IS NULL;

-- Remove exact duplicate rows, keeping one copy of each.
WITH Ranked AS (
    SELECT ROW_NUMBER() OVER (PARTITION BY MemberId, OfficeId, Year, FromDate, ToDate
                              ORDER BY (SELECT NULL)) AS rn
    FROM   OFFICE_Assignments
)
DELETE FROM Ranked WHERE rn > 1;

-- Any remaining rows sharing a key would differ only in ToDate; stop and review.
IF EXISTS (SELECT 1 FROM OFFICE_Assignments
           GROUP BY MemberId, OfficeId, Year, FromDate HAVING COUNT(*) > 1)
    THROW 50002, 'OFFICE_Assignments has rows with the same member, office, year and start date but different end dates. Review them and re-run.', 1;

ALTER TABLE OFFICE_Assignments ALTER COLUMN FromDate date NOT NULL;
ALTER TABLE OFFICE_Assignments ALTER COLUMN ToDate   date NOT NULL;

------------------------------------------------------------------------------
-- STEP 2  Primary keys
------------------------------------------------------------------------------
IF OBJECT_ID('PK_MEETING_List') IS NULL
    ALTER TABLE MEETING_List ADD CONSTRAINT PK_MEETING_List PRIMARY KEY (MeetingID);

IF OBJECT_ID('PK_SYS_FileStore') IS NULL
    ALTER TABLE SYS_FileStore ADD CONSTRAINT PK_SYS_FileStore PRIMARY KEY (ImageId);

IF OBJECT_ID('PK_MEMBER_RemovalCodes') IS NULL
    ALTER TABLE MEMBER_RemovalCodes ADD CONSTRAINT PK_MEMBER_RemovalCodes PRIMARY KEY (ReasonID);

IF OBJECT_ID('PK_DUES_History') IS NULL
    ALTER TABLE DUES_History ADD CONSTRAINT PK_DUES_History PRIMARY KEY (MemberID, Year);

IF OBJECT_ID('PK_LOSAP_History') IS NULL
    ALTER TABLE LOSAP_History ADD CONSTRAINT PK_LOSAP_History PRIMARY KEY (MemberID, Year);

IF OBJECT_ID('PK_MEETING_Members') IS NULL
    ALTER TABLE MEETING_Members ADD CONSTRAINT PK_MEETING_Members PRIMARY KEY (MeetingID, MemberID);

-- Dynamic SQL so this is compiled after FromDate became NOT NULL in STEP 1
-- (the whole batch is compiled up front otherwise, and would reject it).
IF OBJECT_ID('PK_OFFICE_Assignments') IS NULL
    EXEC (N'ALTER TABLE OFFICE_Assignments ADD CONSTRAINT PK_OFFICE_Assignments
              PRIMARY KEY (MemberId, OfficeId, Year, FromDate);');

------------------------------------------------------------------------------
-- STEP 3  Foreign keys
------------------------------------------------------------------------------
IF OBJECT_ID('FK_DUES_History_MEMBER_List') IS NULL
    ALTER TABLE DUES_History ADD CONSTRAINT FK_DUES_History_MEMBER_List
        FOREIGN KEY (MemberID) REFERENCES MEMBER_List (MemberID) ON DELETE CASCADE;

IF OBJECT_ID('FK_LOSAP_History_MEMBER_List') IS NULL
    ALTER TABLE LOSAP_History ADD CONSTRAINT FK_LOSAP_History_MEMBER_List
        FOREIGN KEY (MemberID) REFERENCES MEMBER_List (MemberID) ON DELETE CASCADE;

IF OBJECT_ID('FK_MEETING_Members_MEMBER_List') IS NULL
    ALTER TABLE MEETING_Members ADD CONSTRAINT FK_MEETING_Members_MEMBER_List
        FOREIGN KEY (MemberID) REFERENCES MEMBER_List (MemberID) ON DELETE CASCADE;

IF OBJECT_ID('FK_MEETING_Members_MEETING_List') IS NULL
    ALTER TABLE MEETING_Members ADD CONSTRAINT FK_MEETING_Members_MEETING_List
        FOREIGN KEY (MeetingID) REFERENCES MEETING_List (MeetingID) ON DELETE CASCADE;

IF OBJECT_ID('FK_OFFICE_Assignments_MEMBER_List') IS NULL
    ALTER TABLE OFFICE_Assignments ADD CONSTRAINT FK_OFFICE_Assignments_MEMBER_List
        FOREIGN KEY (MemberId) REFERENCES MEMBER_List (MemberID) ON DELETE CASCADE;

IF OBJECT_ID('FK_OFFICE_Assignments_OFFICE_List') IS NULL
    ALTER TABLE OFFICE_Assignments ADD CONSTRAINT FK_OFFICE_Assignments_OFFICE_List
        FOREIGN KEY (OfficeId) REFERENCES OFFICE_List (OfficeID);

IF OBJECT_ID('FK_MEMBER_List_MEMBER_Type') IS NULL
    ALTER TABLE MEMBER_List ADD CONSTRAINT FK_MEMBER_List_MEMBER_Type
        FOREIGN KEY (MemberTypeID) REFERENCES MEMBER_Type (TypeId);

COMMIT TRANSACTION;
PRINT 'Keys added successfully.';
GO
