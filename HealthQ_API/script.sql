CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TYPE public.gender AS ENUM ('female', 'male', 'special');
CREATE TYPE public.user_type AS ENUM ('administrator', 'doctor', 'patient');
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE public.users (
    email character varying(254) NOT NULL,
    username character varying(64) NOT NULL,
    password_salt character varying(64) NOT NULL,
    password_hash character varying(128) NOT NULL,
    first_name character varying(50) NOT NULL,
    last_name character varying(50) NOT NULL,
    birth_date date NOT NULL,
    gender gender NOT NULL,
    phone_number character varying(13) NOT NULL,
    user_type user_type NOT NULL,
    CONSTRAINT "PK_users" PRIMARY KEY (email)
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250105204130_Initial', '9.0.0');

CREATE TABLE public.questionnaires (
    id uuid NOT NULL,
    questionnaire_content text NOT NULL,
    CONSTRAINT "PK_questionnaires" PRIMARY KEY (id)
);

CREATE TABLE public.user_questionnaire (
    "UserId" character varying(250) NOT NULL,
    "QuestionnaireId" uuid NOT NULL,
    CONSTRAINT "PK_user_questionnaire" PRIMARY KEY ("UserId", "QuestionnaireId"),
    CONSTRAINT "FK_user_questionnaire_questionnaires_QuestionnaireId" FOREIGN KEY ("QuestionnaireId") REFERENCES public.questionnaires (id) ON DELETE CASCADE,
    CONSTRAINT "FK_user_questionnaire_users_UserId" FOREIGN KEY ("UserId") REFERENCES public.users (email) ON DELETE CASCADE
);

CREATE INDEX "IX_user_questionnaire_QuestionnaireId" ON public.user_questionnaire ("QuestionnaireId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250126170230_Questionnaire', '9.0.0');

CREATE TABLE public.doctor_patient (
    "DoctorEmail" character varying(254) NOT NULL,
    "PatientEmail" character varying(254) NOT NULL,
    CONSTRAINT "PK_doctor_patient" PRIMARY KEY ("DoctorEmail", "PatientEmail"),
    CONSTRAINT "FK_doctor_patient_users_DoctorEmail" FOREIGN KEY ("DoctorEmail") REFERENCES public.users (email) ON DELETE CASCADE,
    CONSTRAINT "FK_doctor_patient_users_PatientEmail" FOREIGN KEY ("PatientEmail") REFERENCES public.users (email) ON DELETE CASCADE
);

CREATE INDEX "IX_doctor_patient_PatientEmail" ON public.doctor_patient ("PatientEmail");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250128140717_DoctorPatient_Table', '9.0.0');

ALTER TABLE public.doctor_patient DROP CONSTRAINT "FK_doctor_patient_users_DoctorEmail";

ALTER TABLE public.doctor_patient DROP CONSTRAINT "FK_doctor_patient_users_PatientEmail";

DROP TABLE public.user_questionnaire;

ALTER TABLE public.questionnaires ADD owner_email character varying(254) NOT NULL DEFAULT '';

ALTER TABLE public.doctor_patient ADD "DoctorUserEmail" character varying(254);

ALTER TABLE public.doctor_patient ADD "PatientUserEmail" character varying(254);

CREATE TABLE public.doctors (
    user_email character varying(254) NOT NULL,
    CONSTRAINT "PK_doctors" PRIMARY KEY (user_email),
    CONSTRAINT "FK_doctors_users_user_email" FOREIGN KEY (user_email) REFERENCES public.users (email)
);

CREATE TABLE public.patients (
    user_email character varying(254) NOT NULL,
    CONSTRAINT "PK_patients" PRIMARY KEY (user_email),
    CONSTRAINT "FK_patients_users_user_email" FOREIGN KEY (user_email) REFERENCES public.users (email)
);

CREATE TABLE public.patient_questionnaire (
    "PatientEmail" character varying(254) NOT NULL,
    "QuestionnaireId" uuid NOT NULL,
    "UserEmail" character varying(254) NOT NULL,
    "QuestionnaireId1" uuid NOT NULL,
    CONSTRAINT "PK_patient_questionnaire" PRIMARY KEY ("PatientEmail", "QuestionnaireId"),
    CONSTRAINT "FK_patient_questionnaire_patients_PatientEmail" FOREIGN KEY ("PatientEmail") REFERENCES public.patients (user_email) ON DELETE CASCADE,
    CONSTRAINT "FK_patient_questionnaire_questionnaires_QuestionnaireId" FOREIGN KEY ("QuestionnaireId") REFERENCES public.questionnaires (id) ON DELETE CASCADE,
    CONSTRAINT "FK_patient_questionnaire_questionnaires_QuestionnaireId1" FOREIGN KEY ("QuestionnaireId1") REFERENCES public.questionnaires (id) ON DELETE CASCADE,
    CONSTRAINT "FK_patient_questionnaire_users_UserEmail" FOREIGN KEY ("UserEmail") REFERENCES public.users (email) ON DELETE CASCADE
);

CREATE INDEX "IX_questionnaires_owner_email" ON public.questionnaires (owner_email);

CREATE INDEX "IX_doctor_patient_DoctorUserEmail" ON public.doctor_patient ("DoctorUserEmail");

CREATE INDEX "IX_doctor_patient_PatientUserEmail" ON public.doctor_patient ("PatientUserEmail");

CREATE INDEX "IX_patient_questionnaire_QuestionnaireId" ON public.patient_questionnaire ("QuestionnaireId");

CREATE INDEX "IX_patient_questionnaire_QuestionnaireId1" ON public.patient_questionnaire ("QuestionnaireId1");

CREATE INDEX "IX_patient_questionnaire_UserEmail" ON public.patient_questionnaire ("UserEmail");

ALTER TABLE public.doctor_patient ADD CONSTRAINT "FK_doctor_patient_doctors_DoctorEmail" FOREIGN KEY ("DoctorEmail") REFERENCES public.doctors (user_email) ON DELETE CASCADE;

ALTER TABLE public.doctor_patient ADD CONSTRAINT "FK_doctor_patient_doctors_DoctorUserEmail" FOREIGN KEY ("DoctorUserEmail") REFERENCES public.doctors (user_email);

ALTER TABLE public.doctor_patient ADD CONSTRAINT "FK_doctor_patient_patients_PatientEmail" FOREIGN KEY ("PatientEmail") REFERENCES public.patients (user_email) ON DELETE CASCADE;

ALTER TABLE public.doctor_patient ADD CONSTRAINT "FK_doctor_patient_patients_PatientUserEmail" FOREIGN KEY ("PatientUserEmail") REFERENCES public.patients (user_email);

ALTER TABLE public.questionnaires ADD CONSTRAINT "FK_questionnaires_doctors_owner_email" FOREIGN KEY (owner_email) REFERENCES public.doctors (user_email) ON DELETE CASCADE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250128191945_DoctorTbl_PatientTbl', '9.0.0');

ALTER TABLE public.doctor_patient DROP CONSTRAINT "FK_doctor_patient_doctors_DoctorEmail";

ALTER TABLE public.doctor_patient DROP CONSTRAINT "FK_doctor_patient_doctors_DoctorUserEmail";

ALTER TABLE public.doctor_patient DROP CONSTRAINT "FK_doctor_patient_patients_PatientEmail";

ALTER TABLE public.doctor_patient DROP CONSTRAINT "FK_doctor_patient_patients_PatientUserEmail";

ALTER TABLE public.patient_questionnaire DROP CONSTRAINT "FK_patient_questionnaire_patients_PatientEmail";

ALTER TABLE public.patient_questionnaire DROP CONSTRAINT "FK_patient_questionnaire_questionnaires_QuestionnaireId1";

ALTER TABLE public.patient_questionnaire DROP CONSTRAINT "FK_patient_questionnaire_users_UserEmail";

DROP INDEX public."IX_patient_questionnaire_QuestionnaireId1";

DROP INDEX public."IX_patient_questionnaire_UserEmail";

DROP INDEX public."IX_doctor_patient_DoctorUserEmail";

DROP INDEX public."IX_doctor_patient_PatientUserEmail";

ALTER TABLE public.patient_questionnaire DROP COLUMN "QuestionnaireId1";

ALTER TABLE public.patient_questionnaire DROP COLUMN "UserEmail";

ALTER TABLE public.doctor_patient DROP COLUMN "DoctorUserEmail";

ALTER TABLE public.doctor_patient DROP COLUMN "PatientUserEmail";

ALTER TABLE public.patient_questionnaire RENAME COLUMN "PatientEmail" TO "PatientId";

ALTER TABLE public.doctor_patient RENAME COLUMN "PatientEmail" TO "PatientId";

ALTER TABLE public.doctor_patient RENAME COLUMN "DoctorEmail" TO "DoctorId";

ALTER INDEX public."IX_doctor_patient_PatientEmail" RENAME TO "IX_doctor_patient_PatientId";

ALTER TABLE public.doctor_patient ADD CONSTRAINT "FK_doctor_patient_doctors_DoctorId" FOREIGN KEY ("DoctorId") REFERENCES public.doctors (user_email) ON DELETE CASCADE;

ALTER TABLE public.doctor_patient ADD CONSTRAINT "FK_doctor_patient_patients_PatientId" FOREIGN KEY ("PatientId") REFERENCES public.patients (user_email) ON DELETE CASCADE;

ALTER TABLE public.patient_questionnaire ADD CONSTRAINT "FK_patient_questionnaire_patients_PatientId" FOREIGN KEY ("PatientId") REFERENCES public.patients (user_email) ON DELETE CASCADE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250129113455_ManyToMany', '9.0.0');

ALTER TABLE public.doctors DROP CONSTRAINT "FK_doctors_users_user_email";

ALTER TABLE public.patients DROP CONSTRAINT "FK_patients_users_user_email";

ALTER TABLE public.doctors ADD CONSTRAINT "FK_doctors_users_user_email" FOREIGN KEY (user_email) REFERENCES public.users (email) ON DELETE CASCADE;

ALTER TABLE public.patients ADD CONSTRAINT "FK_patients_users_user_email" FOREIGN KEY (user_email) REFERENCES public.users (email) ON DELETE CASCADE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250202234756_DeleteBehaviour', '9.0.0');

COMMIT;

