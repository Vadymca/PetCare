--
-- PostgreSQL database dump
--

-- Dumped from database version 16.4 (Debian 16.4-1.pgdg110+2)
-- Dumped by pg_dump version 16.4 (Debian 16.4-1.pgdg110+2)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: tiger; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA tiger;


--
-- Name: tiger_data; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA tiger_data;


--
-- Name: topology; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA topology;


--
-- Name: SCHEMA topology; Type: COMMENT; Schema: -; Owner: -
--

COMMENT ON SCHEMA topology IS 'PostGIS Topology schema';


--
-- Name: fuzzystrmatch; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS fuzzystrmatch WITH SCHEMA public;


--
-- Name: EXTENSION fuzzystrmatch; Type: COMMENT; Schema: -; Owner: -
--

COMMENT ON EXTENSION fuzzystrmatch IS 'determine similarities and distance between strings';


--
-- Name: postgis; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS postgis WITH SCHEMA public;


--
-- Name: EXTENSION postgis; Type: COMMENT; Schema: -; Owner: -
--

COMMENT ON EXTENSION postgis IS 'PostGIS geometry and geography spatial types and functions';


--
-- Name: postgis_tiger_geocoder; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS postgis_tiger_geocoder WITH SCHEMA tiger;


--
-- Name: EXTENSION postgis_tiger_geocoder; Type: COMMENT; Schema: -; Owner: -
--

COMMENT ON EXTENSION postgis_tiger_geocoder IS 'PostGIS tiger geocoder and reverse geocoder';


--
-- Name: postgis_topology; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS postgis_topology WITH SCHEMA topology;


--
-- Name: EXTENSION postgis_topology; Type: COMMENT; Schema: -; Owner: -
--

COMMENT ON EXTENSION postgis_topology IS 'PostGIS topology spatial types and functions';


--
-- Name: adoption_status; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.adoption_status AS ENUM (
    'pending',
    'approved',
    'rejected'
);


--
-- Name: aid_category; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.aid_category AS ENUM (
    'food',
    'medical',
    'equipment',
    'other'
);


--
-- Name: aid_status; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.aid_status AS ENUM (
    'open',
    'in_progress',
    'fulfilled',
    'cancelled'
);


--
-- Name: animal_gender; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.animal_gender AS ENUM (
    'male',
    'female',
    'unknown'
);


--
-- Name: animal_status; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.animal_status AS ENUM (
    'available',
    'adopted',
    'reserved',
    'in_treatment',
    'dead',
    'euthanized'
);


--
-- Name: article_status; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.article_status AS ENUM (
    'draft',
    'published',
    'archived'
);


--
-- Name: audit_operation; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.audit_operation AS ENUM (
    'insert',
    'update',
    'delete'
);


--
-- Name: comment_status; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.comment_status AS ENUM (
    'pending',
    'approved',
    'rejected'
);


--
-- Name: donation_status; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.donation_status AS ENUM (
    'pending',
    'completed',
    'failed'
);


--
-- Name: event_status; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.event_status AS ENUM (
    'planned',
    'ongoing',
    'completed',
    'cancelled'
);


--
-- Name: event_type; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.event_type AS ENUM (
    'adoption_day',
    'fundraiser',
    'webinar',
    'volunteer_training'
);


--
-- Name: io_t_device_status; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.io_t_device_status AS ENUM (
    'active',
    'inactive',
    'error'
);


--
-- Name: io_t_device_type; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.io_t_device_type AS ENUM (
    'feeder',
    'temperature',
    'camera'
);


--
-- Name: lost_pet_status; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.lost_pet_status AS ENUM (
    'lost',
    'found',
    'reunited'
);


--
-- Name: user_role; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.user_role AS ENUM (
    'user',
    'admin',
    'moderator'
);


--
-- Name: volunteer_task_status; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE public.volunteer_task_status AS ENUM (
    'open',
    'in_progress',
    'completed',
    'cancelled'
);


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: AdoptionApplications; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."AdoptionApplications" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Status" public.adoption_status NOT NULL,
    "ApplicationDate" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "Comment" text,
    "AdminNotes" text,
    "RejectionReason" text,
    "CreatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "UserId" uuid NOT NULL,
    "AnimalId" uuid NOT NULL,
    "ApprovedBy" uuid
);


--
-- Name: AnimalAidDonations; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."AnimalAidDonations" (
    "Id" uuid NOT NULL,
    "DonationId" uuid NOT NULL,
    "AnimalAidRequestId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


--
-- Name: AnimalAidRequests; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."AnimalAidRequests" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Title" character varying(100) NOT NULL,
    "Description" text,
    "Category" public.aid_category NOT NULL,
    "Status" public.aid_status NOT NULL,
    "EstimatedCost" numeric,
    "Photos" jsonb NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "UserId" uuid,
    "ShelterId" uuid,
    CONSTRAINT "CK_Aid_EstimatedCost" CHECK (("EstimatedCost" >= (0)::numeric))
);


--
-- Name: AnimalSubscriptions; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."AnimalSubscriptions" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "AnimalId" uuid NOT NULL,
    "SubscribedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


--
-- Name: AnimalTags; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."AnimalTags" (
    "AnimalId" uuid NOT NULL,
    "TagId" uuid NOT NULL
);


--
-- Name: Animals; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Animals" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Slug" character varying(64) NOT NULL,
    "Name" character varying(50) NOT NULL,
    "Birthday" date,
    "Gender" public.animal_gender NOT NULL,
    "Description" text,
    "HealthStatus" text,
    "Photos" jsonb,
    "Videos" jsonb,
    "Status" public.animal_status NOT NULL,
    "AdoptionRequirements" text,
    "MicrochipId" character varying(50),
    "IdNumber" integer NOT NULL,
    "Weight" real,
    "Height" real,
    "Color" character varying(50),
    "IsSterilized" boolean DEFAULT false NOT NULL,
    "HaveDocuments" boolean DEFAULT false NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "UserId" uuid,
    "BreedId" uuid NOT NULL,
    "ShelterId" uuid NOT NULL,
    "ShelterId1" uuid,
    CONSTRAINT "CK_Animals_Height" CHECK (("Height" > (0)::double precision)),
    CONSTRAINT "CK_Animals_Weight" CHECK (("Weight" > (0)::double precision))
);


--
-- Name: ArticleComments; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."ArticleComments" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "ArticleId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "ParentCommentId" uuid,
    "Content" text NOT NULL,
    "Status" public.comment_status NOT NULL,
    "ModeratedById" uuid,
    "CreatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


--
-- Name: Articles; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Articles" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Title" character varying(255) NOT NULL,
    "Content" text NOT NULL,
    "Status" public.article_status NOT NULL,
    "Thumbnail" character varying(255),
    "PublishedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "CategoryId" uuid,
    "AuthorId" uuid
);


--
-- Name: AuditLogs; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."AuditLogs" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "TableName" character varying(50) NOT NULL,
    "RecordId" uuid NOT NULL,
    "Operation" public.audit_operation NOT NULL,
    "UserId" uuid,
    "Changes" jsonb,
    "CreatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


--
-- Name: Breeds; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Breeds" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Description" text,
    "SpeciesId" uuid NOT NULL
);


--
-- Name: Categories; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Categories" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Name" character varying(50) NOT NULL,
    "Description" text,
    "CreatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


--
-- Name: Donations; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Donations" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Amount" numeric NOT NULL,
    "Status" public.donation_status NOT NULL,
    "TransactionId" character varying(255),
    "Purpose" character varying(255),
    "Recurring" boolean DEFAULT false NOT NULL,
    "Anonymous" boolean DEFAULT false NOT NULL,
    "DonationDate" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "Report" text,
    "CreatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "UserId" uuid,
    "ShelterId" uuid,
    "PaymentMethodId" uuid NOT NULL,
    CONSTRAINT "CK_Donations_Amount" CHECK (("Amount" > (0)::numeric))
);


--
-- Name: EventParticipants; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."EventParticipants" (
    "Id" uuid NOT NULL,
    "EventId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "RegisteredAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


--
-- Name: Events; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Events" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Title" character varying(100) NOT NULL,
    "Description" text,
    "EventDate" timestamp with time zone,
    "Location" public.geometry(Point,4326),
    "Address" text,
    "Type" public.event_type NOT NULL,
    "Status" public.event_status NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "ShelterId" uuid,
    "UserId" uuid
);


--
-- Name: GamificationRewards; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."GamificationRewards" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Points" integer NOT NULL,
    "Description" character varying(255),
    "Used" boolean DEFAULT false NOT NULL,
    "AwardedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "UserId" uuid NOT NULL,
    "TaskId" uuid,
    CONSTRAINT "CK_GamificationRewards_Points" CHECK (("Points" >= 0))
);


--
-- Name: IoTDevices; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."IoTDevices" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Type" public.io_t_device_type NOT NULL,
    "Name" character varying(50) NOT NULL,
    "Status" public.io_t_device_status NOT NULL,
    "Data" jsonb,
    "SerialNumber" character varying(50) NOT NULL,
    "AlertThresholds" jsonb,
    "LastUpdated" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "ShelterId" uuid NOT NULL
);


--
-- Name: Likes; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Likes" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "UserId" uuid NOT NULL,
    "LikedEntity" character varying(100) NOT NULL,
    "LikedEntityId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "ArticleCommentId" uuid,
    CONSTRAINT "CK_Likes_LikedEntity" CHECK ((length(("LikedEntity")::text) > 0)),
    CONSTRAINT "CK_Likes_LikedEntityId" CHECK (("LikedEntityId" IS NOT NULL)),
    CONSTRAINT "CK_Likes_UserId" CHECK (("UserId" IS NOT NULL))
);


--
-- Name: LostPets; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."LostPets" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Slug" character varying(64) NOT NULL,
    "Name" character varying(50),
    "Description" text,
    "LastSeenLocation" public.geometry(Point,4326),
    "LastSeenDate" timestamp with time zone,
    "Photos" jsonb NOT NULL,
    "Status" public.lost_pet_status NOT NULL,
    "AdminNotes" text,
    "Reward" numeric,
    "ContactAlternative" character varying(255),
    "MicrochipId" character varying(50),
    "CreatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "UserId" uuid NOT NULL,
    "BreedId" uuid,
    CONSTRAINT "CK_LostPets_Reward" CHECK (("Reward" >= (0)::numeric))
);


--
-- Name: NotificationTypes; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."NotificationTypes" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Name" character varying(50) NOT NULL,
    "Description" text
);


--
-- Name: Notifications; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Notifications" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Title" character varying(100) NOT NULL,
    "Message" text NOT NULL,
    "IsRead" boolean DEFAULT false NOT NULL,
    "NotifiableEntity" character varying(50),
    "NotifiableEntityId" uuid,
    "CreatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "UserId" uuid NOT NULL,
    "NotificationTypeId" uuid NOT NULL
);


--
-- Name: PaymentMethods; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."PaymentMethods" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Name" character varying(50) NOT NULL
);


--
-- Name: RoleClaims; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."RoleClaims" (
    "Id" integer NOT NULL,
    "RoleId" uuid NOT NULL,
    "ClaimType" text,
    "ClaimValue" text
);


--
-- Name: RoleClaims_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public."RoleClaims" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."RoleClaims_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Roles; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Roles" (
    "Id" uuid NOT NULL,
    "Name" character varying(256),
    "NormalizedName" character varying(256),
    "ConcurrencyStamp" text
);


--
-- Name: ShelterSubscriptions; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."ShelterSubscriptions" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "ShelterId" uuid NOT NULL,
    "SubscribedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


--
-- Name: Shelters; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Shelters" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Slug" character varying(64) NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Address" text NOT NULL,
    "Coordinates" public.geometry(Point,4326) NOT NULL,
    "ContactPhone" character varying(20) NOT NULL,
    "ContactEmail" character varying(255) NOT NULL,
    "Description" text,
    "Capacity" integer NOT NULL,
    "CurrentOccupancy" integer NOT NULL,
    "Photos" jsonb NOT NULL,
    "VirtualTourUrl" character varying(255),
    "WorkingHours" character varying(100),
    "SocialMedia" jsonb NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "ManagerId" uuid,
    CONSTRAINT "CK_Shelters_Capacity" CHECK (("Capacity" >= 0))
);


--
-- Name: Species; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Species" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Name" character varying(100) NOT NULL
);


--
-- Name: SuccessStories; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."SuccessStories" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Title" character varying(100) NOT NULL,
    "Content" text NOT NULL,
    "Photos" jsonb NOT NULL,
    "Videos" jsonb NOT NULL,
    "PublishedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "Views" integer DEFAULT 0 NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "AnimalId" uuid NOT NULL,
    "UserId" uuid,
    CONSTRAINT "CK_SuccessStories_Views" CHECK (("Views" >= 0))
);


--
-- Name: Tags; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Tags" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Name" character varying(50) NOT NULL,
    "Icon" character varying(255),
    "CreatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


--
-- Name: UserClaims; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."UserClaims" (
    "Id" integer NOT NULL,
    "UserId" uuid NOT NULL,
    "ClaimType" text,
    "ClaimValue" text
);


--
-- Name: UserClaims_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

ALTER TABLE public."UserClaims" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."UserClaims_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: UserLogins; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."UserLogins" (
    "LoginProvider" text NOT NULL,
    "ProviderKey" text NOT NULL,
    "ProviderDisplayName" text,
    "UserId" uuid NOT NULL
);


--
-- Name: UserRoles; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."UserRoles" (
    "UserId" uuid NOT NULL,
    "RoleId" uuid NOT NULL
);


--
-- Name: UserTokens; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."UserTokens" (
    "UserId" uuid NOT NULL,
    "LoginProvider" text NOT NULL,
    "Name" text NOT NULL,
    "Value" text
);


--
-- Name: Users; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Users" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Email" character varying(256),
    "PasswordHash" text,
    "FirstName" character varying(50) NOT NULL,
    "LastName" character varying(50) NOT NULL,
    "Phone" character varying(30) NOT NULL,
    "Role" public.user_role NOT NULL,
    "Preferences" jsonb NOT NULL,
    "Points" integer DEFAULT 0 NOT NULL,
    "LastLogin" timestamp with time zone,
    "ProfilePhoto" character varying(255),
    "PostalCode" character varying(20),
    "Language" text DEFAULT 'uk'::text NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "UserName" character varying(256),
    "NormalizedUserName" character varying(256),
    "NormalizedEmail" character varying(256),
    "EmailConfirmed" boolean NOT NULL,
    "SecurityStamp" text,
    "ConcurrencyStamp" text,
    "PhoneNumber" text,
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled" boolean NOT NULL,
    "LockoutEnd" timestamp with time zone,
    "LockoutEnabled" boolean NOT NULL,
    "AccessFailedCount" integer NOT NULL,
    "Address" text,
    CONSTRAINT "CK_Users_Points" CHECK (("Points" >= 0))
);


--
-- Name: VolunteerTaskAssignments; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."VolunteerTaskAssignments" (
    "Id" uuid NOT NULL,
    "VolunteerTaskId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "AssignedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


--
-- Name: VolunteerTasks; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."VolunteerTasks" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "Title" character varying(100) NOT NULL,
    "Description" text,
    "Date" date NOT NULL,
    "Duration" integer,
    "RequiredVolunteers" integer NOT NULL,
    "Status" public.volunteer_task_status NOT NULL,
    "PointsReward" integer NOT NULL,
    "Location" public.geometry(Point,4326),
    "SkillsRequired" jsonb NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "UpdatedAt" timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "ShelterId" uuid NOT NULL,
    CONSTRAINT "CK_VolunteerTasks_Duration" CHECK (("Duration" > 0)),
    CONSTRAINT "CK_VolunteerTasks_Points" CHECK (("PointsReward" >= 0)),
    CONSTRAINT "CK_VolunteerTasks_Required" CHECK (("RequiredVolunteers" > 0))
);


--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


--
-- Data for Name: AdoptionApplications; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."AdoptionApplications" ("Id", "Status", "ApplicationDate", "Comment", "AdminNotes", "RejectionReason", "CreatedAt", "UpdatedAt", "UserId", "AnimalId", "ApprovedBy") FROM stdin;
\.


--
-- Data for Name: AnimalAidDonations; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."AnimalAidDonations" ("Id", "DonationId", "AnimalAidRequestId", "CreatedAt") FROM stdin;
\.


--
-- Data for Name: AnimalAidRequests; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."AnimalAidRequests" ("Id", "Title", "Description", "Category", "Status", "EstimatedCost", "Photos", "CreatedAt", "UpdatedAt", "UserId", "ShelterId") FROM stdin;
\.


--
-- Data for Name: AnimalSubscriptions; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."AnimalSubscriptions" ("Id", "UserId", "AnimalId", "SubscribedAt") FROM stdin;
\.


--
-- Data for Name: AnimalTags; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."AnimalTags" ("AnimalId", "TagId") FROM stdin;
\.


--
-- Data for Name: Animals; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."Animals" ("Id", "Slug", "Name", "Birthday", "Gender", "Description", "HealthStatus", "Photos", "Videos", "Status", "AdoptionRequirements", "MicrochipId", "IdNumber", "Weight", "Height", "Color", "IsSterilized", "HaveDocuments", "CreatedAt", "UpdatedAt", "UserId", "BreedId", "ShelterId", "ShelterId1") FROM stdin;
\.


--
-- Data for Name: ArticleComments; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."ArticleComments" ("Id", "ArticleId", "UserId", "ParentCommentId", "Content", "Status", "ModeratedById", "CreatedAt", "UpdatedAt") FROM stdin;
\.


--
-- Data for Name: Articles; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."Articles" ("Id", "Title", "Content", "Status", "Thumbnail", "PublishedAt", "UpdatedAt", "CategoryId", "AuthorId") FROM stdin;
\.


--
-- Data for Name: AuditLogs; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."AuditLogs" ("Id", "TableName", "RecordId", "Operation", "UserId", "Changes", "CreatedAt") FROM stdin;
\.


--
-- Data for Name: Breeds; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."Breeds" ("Id", "Name", "Description", "SpeciesId") FROM stdin;
\.


--
-- Data for Name: Categories; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."Categories" ("Id", "Name", "Description", "CreatedAt") FROM stdin;
\.


--
-- Data for Name: Donations; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."Donations" ("Id", "Amount", "Status", "TransactionId", "Purpose", "Recurring", "Anonymous", "DonationDate", "Report", "CreatedAt", "UpdatedAt", "UserId", "ShelterId", "PaymentMethodId") FROM stdin;
\.


--
-- Data for Name: EventParticipants; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."EventParticipants" ("Id", "EventId", "UserId", "RegisteredAt") FROM stdin;
\.


--
-- Data for Name: Events; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."Events" ("Id", "Title", "Description", "EventDate", "Location", "Address", "Type", "Status", "CreatedAt", "UpdatedAt", "ShelterId", "UserId") FROM stdin;
\.


--
-- Data for Name: GamificationRewards; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."GamificationRewards" ("Id", "Points", "Description", "Used", "AwardedAt", "UserId", "TaskId") FROM stdin;
\.


--
-- Data for Name: IoTDevices; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."IoTDevices" ("Id", "Type", "Name", "Status", "Data", "SerialNumber", "AlertThresholds", "LastUpdated", "ShelterId") FROM stdin;
\.


--
-- Data for Name: Likes; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."Likes" ("Id", "UserId", "LikedEntity", "LikedEntityId", "CreatedAt", "ArticleCommentId") FROM stdin;
\.


--
-- Data for Name: LostPets; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."LostPets" ("Id", "Slug", "Name", "Description", "LastSeenLocation", "LastSeenDate", "Photos", "Status", "AdminNotes", "Reward", "ContactAlternative", "MicrochipId", "CreatedAt", "UpdatedAt", "UserId", "BreedId") FROM stdin;
\.


--
-- Data for Name: NotificationTypes; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."NotificationTypes" ("Id", "Name", "Description") FROM stdin;
\.


--
-- Data for Name: Notifications; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."Notifications" ("Id", "Title", "Message", "IsRead", "NotifiableEntity", "NotifiableEntityId", "CreatedAt", "UserId", "NotificationTypeId") FROM stdin;
\.


--
-- Data for Name: PaymentMethods; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."PaymentMethods" ("Id", "Name") FROM stdin;
\.


--
-- Data for Name: RoleClaims; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."RoleClaims" ("Id", "RoleId", "ClaimType", "ClaimValue") FROM stdin;
\.


--
-- Data for Name: Roles; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."Roles" ("Id", "Name", "NormalizedName", "ConcurrencyStamp") FROM stdin;
0198e75a-2f95-7ed6-aeae-7d56739a46f4	User	USER	\N
0198e75a-2fe7-740a-a6eb-42d7dc3571c9	Admin	ADMIN	\N
0198e75a-2fee-7be6-b6d1-84d53d48816d	ShelterManager	SHELTERMANAGER	\N
0198e75a-2ff4-78e1-a8ea-f7ab575a89a4	Veterinarian	VETERINARIAN	\N
0198e75a-2ffa-7112-b752-3a719d9afcbe	Volunteer	VOLUNTEER	\N
\.


--
-- Data for Name: ShelterSubscriptions; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."ShelterSubscriptions" ("Id", "UserId", "ShelterId", "SubscribedAt") FROM stdin;
\.


--
-- Data for Name: Shelters; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."Shelters" ("Id", "Slug", "Name", "Address", "Coordinates", "ContactPhone", "ContactEmail", "Description", "Capacity", "CurrentOccupancy", "Photos", "VirtualTourUrl", "WorkingHours", "SocialMedia", "CreatedAt", "UpdatedAt", "ManagerId") FROM stdin;
\.


--
-- Data for Name: Species; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."Species" ("Id", "Name") FROM stdin;
\.


--
-- Data for Name: SuccessStories; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."SuccessStories" ("Id", "Title", "Content", "Photos", "Videos", "PublishedAt", "Views", "CreatedAt", "UpdatedAt", "AnimalId", "UserId") FROM stdin;
\.


--
-- Data for Name: Tags; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."Tags" ("Id", "Name", "Icon", "CreatedAt") FROM stdin;
\.


--
-- Data for Name: UserClaims; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."UserClaims" ("Id", "UserId", "ClaimType", "ClaimValue") FROM stdin;
\.


--
-- Data for Name: UserLogins; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."UserLogins" ("LoginProvider", "ProviderKey", "ProviderDisplayName", "UserId") FROM stdin;
\.


--
-- Data for Name: UserRoles; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."UserRoles" ("UserId", "RoleId") FROM stdin;
\.


--
-- Data for Name: UserTokens; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."UserTokens" ("UserId", "LoginProvider", "Name", "Value") FROM stdin;
\.


--
-- Data for Name: Users; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."Users" ("Id", "Email", "PasswordHash", "FirstName", "LastName", "Phone", "Role", "Preferences", "Points", "LastLogin", "ProfilePhoto", "PostalCode", "Language", "CreatedAt", "UpdatedAt", "UserName", "NormalizedUserName", "NormalizedEmail", "EmailConfirmed", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnd", "LockoutEnabled", "AccessFailedCount", "Address") FROM stdin;
e04bf46c-bf7a-4bf6-a774-19113292e417	vadimancuta@gmail.com	AQAAAAIAAYagAAAAEKI8OmCyRbtz746mzgzmLu9OhTxrfeoJ/kONBfNKrq+LNDHkgJm1zDub5Ul4A98GZA==	Vadim	Ancuta	+380502223209	user	{}	0	2025-09-20 13:26:04.685873+00	\N	60300	uk	2025-09-20 11:13:13.733638+00	2025-09-20 13:26:04.686518+00	vadimancuta@gmail.com	VADIMANCUTA@GMAIL.COM	VADIMANCUTA@GMAIL.COM	t	YCY2F7HXZ5QI4CDV2SBO7X3BY6ETXP7E	0feab7c8-a405-4dda-9bd2-4a426c9adba7	\N	f	f	\N	t	0	Новоселиця, Чернівецька область
\.


--
-- Data for Name: VolunteerTaskAssignments; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."VolunteerTaskAssignments" ("Id", "VolunteerTaskId", "UserId", "AssignedAt") FROM stdin;
\.


--
-- Data for Name: VolunteerTasks; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."VolunteerTasks" ("Id", "Title", "Description", "Date", "Duration", "RequiredVolunteers", "Status", "PointsReward", "Location", "SkillsRequired", "CreatedAt", "UpdatedAt", "ShelterId") FROM stdin;
\.


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20250826170432_InitialCreate	9.0.8
20250903150846_AddTriggers	9.0.8
20250919181907_AddAddressToUser	9.0.8
\.


--
-- Data for Name: spatial_ref_sys; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public.spatial_ref_sys (srid, auth_name, auth_srid, srtext, proj4text) FROM stdin;
\.


--
-- Data for Name: geocode_settings; Type: TABLE DATA; Schema: tiger; Owner: -
--

COPY tiger.geocode_settings (name, setting, unit, category, short_desc) FROM stdin;
\.


--
-- Data for Name: pagc_gaz; Type: TABLE DATA; Schema: tiger; Owner: -
--

COPY tiger.pagc_gaz (id, seq, word, stdword, token, is_custom) FROM stdin;
\.


--
-- Data for Name: pagc_lex; Type: TABLE DATA; Schema: tiger; Owner: -
--

COPY tiger.pagc_lex (id, seq, word, stdword, token, is_custom) FROM stdin;
\.


--
-- Data for Name: pagc_rules; Type: TABLE DATA; Schema: tiger; Owner: -
--

COPY tiger.pagc_rules (id, rule, is_custom) FROM stdin;
\.


--
-- Data for Name: topology; Type: TABLE DATA; Schema: topology; Owner: -
--

COPY topology.topology (id, name, srid, "precision", hasz) FROM stdin;
\.


--
-- Data for Name: layer; Type: TABLE DATA; Schema: topology; Owner: -
--

COPY topology.layer (topology_id, layer_id, schema_name, table_name, feature_column, feature_type, level, child_id) FROM stdin;
\.


--
-- Name: RoleClaims_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."RoleClaims_Id_seq"', 1, false);


--
-- Name: UserClaims_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."UserClaims_Id_seq"', 1, false);


--
-- Name: topology_id_seq; Type: SEQUENCE SET; Schema: topology; Owner: -
--

SELECT pg_catalog.setval('topology.topology_id_seq', 1, false);


--
-- Name: AdoptionApplications PK_AdoptionApplications; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AdoptionApplications"
    ADD CONSTRAINT "PK_AdoptionApplications" PRIMARY KEY ("Id");


--
-- Name: AnimalAidDonations PK_AnimalAidDonations; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AnimalAidDonations"
    ADD CONSTRAINT "PK_AnimalAidDonations" PRIMARY KEY ("Id");


--
-- Name: AnimalAidRequests PK_AnimalAidRequests; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AnimalAidRequests"
    ADD CONSTRAINT "PK_AnimalAidRequests" PRIMARY KEY ("Id");


--
-- Name: AnimalSubscriptions PK_AnimalSubscriptions; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AnimalSubscriptions"
    ADD CONSTRAINT "PK_AnimalSubscriptions" PRIMARY KEY ("Id");


--
-- Name: AnimalTags PK_AnimalTags; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AnimalTags"
    ADD CONSTRAINT "PK_AnimalTags" PRIMARY KEY ("AnimalId", "TagId");


--
-- Name: Animals PK_Animals; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Animals"
    ADD CONSTRAINT "PK_Animals" PRIMARY KEY ("Id");


--
-- Name: ArticleComments PK_ArticleComments; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."ArticleComments"
    ADD CONSTRAINT "PK_ArticleComments" PRIMARY KEY ("Id");


--
-- Name: Articles PK_Articles; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Articles"
    ADD CONSTRAINT "PK_Articles" PRIMARY KEY ("Id");


--
-- Name: AuditLogs PK_AuditLogs; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AuditLogs"
    ADD CONSTRAINT "PK_AuditLogs" PRIMARY KEY ("Id");


--
-- Name: Breeds PK_Breeds; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Breeds"
    ADD CONSTRAINT "PK_Breeds" PRIMARY KEY ("Id");


--
-- Name: Categories PK_Categories; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Categories"
    ADD CONSTRAINT "PK_Categories" PRIMARY KEY ("Id");


--
-- Name: Donations PK_Donations; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Donations"
    ADD CONSTRAINT "PK_Donations" PRIMARY KEY ("Id");


--
-- Name: EventParticipants PK_EventParticipants; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."EventParticipants"
    ADD CONSTRAINT "PK_EventParticipants" PRIMARY KEY ("Id");


--
-- Name: Events PK_Events; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Events"
    ADD CONSTRAINT "PK_Events" PRIMARY KEY ("Id");


--
-- Name: GamificationRewards PK_GamificationRewards; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."GamificationRewards"
    ADD CONSTRAINT "PK_GamificationRewards" PRIMARY KEY ("Id");


--
-- Name: IoTDevices PK_IoTDevices; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."IoTDevices"
    ADD CONSTRAINT "PK_IoTDevices" PRIMARY KEY ("Id");


--
-- Name: Likes PK_Likes; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Likes"
    ADD CONSTRAINT "PK_Likes" PRIMARY KEY ("Id");


--
-- Name: LostPets PK_LostPets; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."LostPets"
    ADD CONSTRAINT "PK_LostPets" PRIMARY KEY ("Id");


--
-- Name: NotificationTypes PK_NotificationTypes; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."NotificationTypes"
    ADD CONSTRAINT "PK_NotificationTypes" PRIMARY KEY ("Id");


--
-- Name: Notifications PK_Notifications; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Notifications"
    ADD CONSTRAINT "PK_Notifications" PRIMARY KEY ("Id");


--
-- Name: PaymentMethods PK_PaymentMethods; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."PaymentMethods"
    ADD CONSTRAINT "PK_PaymentMethods" PRIMARY KEY ("Id");


--
-- Name: RoleClaims PK_RoleClaims; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."RoleClaims"
    ADD CONSTRAINT "PK_RoleClaims" PRIMARY KEY ("Id");


--
-- Name: Roles PK_Roles; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Roles"
    ADD CONSTRAINT "PK_Roles" PRIMARY KEY ("Id");


--
-- Name: ShelterSubscriptions PK_ShelterSubscriptions; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."ShelterSubscriptions"
    ADD CONSTRAINT "PK_ShelterSubscriptions" PRIMARY KEY ("Id");


--
-- Name: Shelters PK_Shelters; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Shelters"
    ADD CONSTRAINT "PK_Shelters" PRIMARY KEY ("Id");


--
-- Name: Species PK_Species; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Species"
    ADD CONSTRAINT "PK_Species" PRIMARY KEY ("Id");


--
-- Name: SuccessStories PK_SuccessStories; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."SuccessStories"
    ADD CONSTRAINT "PK_SuccessStories" PRIMARY KEY ("Id");


--
-- Name: Tags PK_Tags; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Tags"
    ADD CONSTRAINT "PK_Tags" PRIMARY KEY ("Id");


--
-- Name: UserClaims PK_UserClaims; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."UserClaims"
    ADD CONSTRAINT "PK_UserClaims" PRIMARY KEY ("Id");


--
-- Name: UserLogins PK_UserLogins; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."UserLogins"
    ADD CONSTRAINT "PK_UserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey");


--
-- Name: UserRoles PK_UserRoles; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."UserRoles"
    ADD CONSTRAINT "PK_UserRoles" PRIMARY KEY ("UserId", "RoleId");


--
-- Name: UserTokens PK_UserTokens; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."UserTokens"
    ADD CONSTRAINT "PK_UserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name");


--
-- Name: Users PK_Users; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "PK_Users" PRIMARY KEY ("Id");


--
-- Name: VolunteerTaskAssignments PK_VolunteerTaskAssignments; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."VolunteerTaskAssignments"
    ADD CONSTRAINT "PK_VolunteerTaskAssignments" PRIMARY KEY ("Id");


--
-- Name: VolunteerTasks PK_VolunteerTasks; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."VolunteerTasks"
    ADD CONSTRAINT "PK_VolunteerTasks" PRIMARY KEY ("Id");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: EmailIndex; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "EmailIndex" ON public."Users" USING btree ("NormalizedEmail");


--
-- Name: IX_AdoptionApplications_AnimalId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_AdoptionApplications_AnimalId" ON public."AdoptionApplications" USING btree ("AnimalId");


--
-- Name: IX_AdoptionApplications_ApplicationDate; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_AdoptionApplications_ApplicationDate" ON public."AdoptionApplications" USING btree ("ApplicationDate");


--
-- Name: IX_AdoptionApplications_ApprovedBy; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_AdoptionApplications_ApprovedBy" ON public."AdoptionApplications" USING btree ("ApprovedBy");


--
-- Name: IX_AdoptionApplications_Status; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_AdoptionApplications_Status" ON public."AdoptionApplications" USING btree ("Status");


--
-- Name: IX_AdoptionApplications_UserId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_AdoptionApplications_UserId" ON public."AdoptionApplications" USING btree ("UserId");


--
-- Name: IX_AnimalAidDonations_AnimalAidRequestId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_AnimalAidDonations_AnimalAidRequestId" ON public."AnimalAidDonations" USING btree ("AnimalAidRequestId");


--
-- Name: IX_AnimalAidDonations_DonationId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_AnimalAidDonations_DonationId" ON public."AnimalAidDonations" USING btree ("DonationId");


--
-- Name: IX_AnimalAidRequests_Category; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_AnimalAidRequests_Category" ON public."AnimalAidRequests" USING btree ("Category");


--
-- Name: IX_AnimalAidRequests_CreatedAt; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_AnimalAidRequests_CreatedAt" ON public."AnimalAidRequests" USING btree ("CreatedAt");


--
-- Name: IX_AnimalAidRequests_ShelterId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_AnimalAidRequests_ShelterId" ON public."AnimalAidRequests" USING btree ("ShelterId");


--
-- Name: IX_AnimalAidRequests_Status; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_AnimalAidRequests_Status" ON public."AnimalAidRequests" USING btree ("Status");


--
-- Name: IX_AnimalAidRequests_UserId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_AnimalAidRequests_UserId" ON public."AnimalAidRequests" USING btree ("UserId");


--
-- Name: IX_AnimalSubscriptions_AnimalId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_AnimalSubscriptions_AnimalId" ON public."AnimalSubscriptions" USING btree ("AnimalId");


--
-- Name: IX_AnimalSubscriptions_UserId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_AnimalSubscriptions_UserId" ON public."AnimalSubscriptions" USING btree ("UserId");


--
-- Name: IX_AnimalTags_TagId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_AnimalTags_TagId" ON public."AnimalTags" USING btree ("TagId");


--
-- Name: IX_Animals_BreedId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Animals_BreedId" ON public."Animals" USING btree ("BreedId");


--
-- Name: IX_Animals_MicrochipId; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Animals_MicrochipId" ON public."Animals" USING btree ("MicrochipId");


--
-- Name: IX_Animals_ShelterId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Animals_ShelterId" ON public."Animals" USING btree ("ShelterId");


--
-- Name: IX_Animals_ShelterId1; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Animals_ShelterId1" ON public."Animals" USING btree ("ShelterId1");


--
-- Name: IX_Animals_ShelterId_IdNumber; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Animals_ShelterId_IdNumber" ON public."Animals" USING btree ("ShelterId", "IdNumber");


--
-- Name: IX_Animals_Slug; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Animals_Slug" ON public."Animals" USING btree ("Slug");


--
-- Name: IX_Animals_UserId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Animals_UserId" ON public."Animals" USING btree ("UserId");


--
-- Name: IX_ArticleComments_ArticleId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_ArticleComments_ArticleId" ON public."ArticleComments" USING btree ("ArticleId");


--
-- Name: IX_ArticleComments_CreatedAt; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_ArticleComments_CreatedAt" ON public."ArticleComments" USING btree ("CreatedAt");


--
-- Name: IX_ArticleComments_ModeratedById; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_ArticleComments_ModeratedById" ON public."ArticleComments" USING btree ("ModeratedById");


--
-- Name: IX_ArticleComments_ParentCommentId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_ArticleComments_ParentCommentId" ON public."ArticleComments" USING btree ("ParentCommentId");


--
-- Name: IX_ArticleComments_Status; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_ArticleComments_Status" ON public."ArticleComments" USING btree ("Status");


--
-- Name: IX_ArticleComments_UserId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_ArticleComments_UserId" ON public."ArticleComments" USING btree ("UserId");


--
-- Name: IX_Articles_AuthorId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Articles_AuthorId" ON public."Articles" USING btree ("AuthorId");


--
-- Name: IX_Articles_CategoryId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Articles_CategoryId" ON public."Articles" USING btree ("CategoryId");


--
-- Name: IX_Articles_PublishedAt; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Articles_PublishedAt" ON public."Articles" USING btree ("PublishedAt");


--
-- Name: IX_Articles_Status; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Articles_Status" ON public."Articles" USING btree ("Status");


--
-- Name: IX_AuditLogs_RecordId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_AuditLogs_RecordId" ON public."AuditLogs" USING btree ("RecordId");


--
-- Name: IX_AuditLogs_TableName; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_AuditLogs_TableName" ON public."AuditLogs" USING btree ("TableName");


--
-- Name: IX_AuditLogs_TableName_RecordId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_AuditLogs_TableName_RecordId" ON public."AuditLogs" USING btree ("TableName", "RecordId");


--
-- Name: IX_AuditLogs_UserId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_AuditLogs_UserId" ON public."AuditLogs" USING btree ("UserId");


--
-- Name: IX_Breeds_Name_SpeciesId; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Breeds_Name_SpeciesId" ON public."Breeds" USING btree ("Name", "SpeciesId");


--
-- Name: IX_Breeds_SpeciesId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Breeds_SpeciesId" ON public."Breeds" USING btree ("SpeciesId");


--
-- Name: IX_Categories_Name; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Categories_Name" ON public."Categories" USING btree ("Name");


--
-- Name: IX_Donations_DonationDate; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Donations_DonationDate" ON public."Donations" USING btree ("DonationDate");


--
-- Name: IX_Donations_PaymentMethodId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Donations_PaymentMethodId" ON public."Donations" USING btree ("PaymentMethodId");


--
-- Name: IX_Donations_ShelterId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Donations_ShelterId" ON public."Donations" USING btree ("ShelterId");


--
-- Name: IX_Donations_Status; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Donations_Status" ON public."Donations" USING btree ("Status");


--
-- Name: IX_Donations_UserId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Donations_UserId" ON public."Donations" USING btree ("UserId");


--
-- Name: IX_EventParticipants_EventId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_EventParticipants_EventId" ON public."EventParticipants" USING btree ("EventId");


--
-- Name: IX_EventParticipants_UserId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_EventParticipants_UserId" ON public."EventParticipants" USING btree ("UserId");


--
-- Name: IX_Events_EventDate; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Events_EventDate" ON public."Events" USING btree ("EventDate");


--
-- Name: IX_Events_ShelterId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Events_ShelterId" ON public."Events" USING btree ("ShelterId");


--
-- Name: IX_Events_Status; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Events_Status" ON public."Events" USING btree ("Status");


--
-- Name: IX_Events_Type; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Events_Type" ON public."Events" USING btree ("Type");


--
-- Name: IX_Events_UserId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Events_UserId" ON public."Events" USING btree ("UserId");


--
-- Name: IX_GamificationRewards_TaskId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_GamificationRewards_TaskId" ON public."GamificationRewards" USING btree ("TaskId");


--
-- Name: IX_GamificationRewards_UserId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_GamificationRewards_UserId" ON public."GamificationRewards" USING btree ("UserId");


--
-- Name: IX_IoTDevices_SerialNumber; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_IoTDevices_SerialNumber" ON public."IoTDevices" USING btree ("SerialNumber");


--
-- Name: IX_IoTDevices_ShelterId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_IoTDevices_ShelterId" ON public."IoTDevices" USING btree ("ShelterId");


--
-- Name: IX_IoTDevices_Status; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_IoTDevices_Status" ON public."IoTDevices" USING btree ("Status");


--
-- Name: IX_IoTDevices_Type; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_IoTDevices_Type" ON public."IoTDevices" USING btree ("Type");


--
-- Name: IX_Likes_ArticleCommentId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Likes_ArticleCommentId" ON public."Likes" USING btree ("ArticleCommentId");


--
-- Name: IX_Likes_UserId_LikedEntity_LikedEntityId; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Likes_UserId_LikedEntity_LikedEntityId" ON public."Likes" USING btree ("UserId", "LikedEntity", "LikedEntityId");


--
-- Name: IX_LostPets_BreedId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_LostPets_BreedId" ON public."LostPets" USING btree ("BreedId");


--
-- Name: IX_LostPets_LastSeenDate; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_LostPets_LastSeenDate" ON public."LostPets" USING btree ("LastSeenDate");


--
-- Name: IX_LostPets_Slug; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_LostPets_Slug" ON public."LostPets" USING btree ("Slug");


--
-- Name: IX_LostPets_Status; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_LostPets_Status" ON public."LostPets" USING btree ("Status");


--
-- Name: IX_LostPets_UserId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_LostPets_UserId" ON public."LostPets" USING btree ("UserId");


--
-- Name: IX_NotificationTypes_Name; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_NotificationTypes_Name" ON public."NotificationTypes" USING btree ("Name");


--
-- Name: IX_Notifications_CreatedAt; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Notifications_CreatedAt" ON public."Notifications" USING btree ("CreatedAt");


--
-- Name: IX_Notifications_NotificationTypeId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Notifications_NotificationTypeId" ON public."Notifications" USING btree ("NotificationTypeId");


--
-- Name: IX_Notifications_UserId_IsRead; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Notifications_UserId_IsRead" ON public."Notifications" USING btree ("UserId", "IsRead");


--
-- Name: IX_PaymentMethods_Name; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_PaymentMethods_Name" ON public."PaymentMethods" USING btree ("Name");


--
-- Name: IX_RoleClaims_RoleId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_RoleClaims_RoleId" ON public."RoleClaims" USING btree ("RoleId");


--
-- Name: IX_ShelterSubscriptions_ShelterId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_ShelterSubscriptions_ShelterId" ON public."ShelterSubscriptions" USING btree ("ShelterId");


--
-- Name: IX_ShelterSubscriptions_UserId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_ShelterSubscriptions_UserId" ON public."ShelterSubscriptions" USING btree ("UserId");


--
-- Name: IX_Shelters_Coordinates; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Shelters_Coordinates" ON public."Shelters" USING gist ("Coordinates");


--
-- Name: IX_Shelters_ManagerId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Shelters_ManagerId" ON public."Shelters" USING btree ("ManagerId");


--
-- Name: IX_Shelters_Slug; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Shelters_Slug" ON public."Shelters" USING btree ("Slug");


--
-- Name: IX_Species_Name; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Species_Name" ON public."Species" USING btree ("Name");


--
-- Name: IX_SuccessStories_AnimalId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_SuccessStories_AnimalId" ON public."SuccessStories" USING btree ("AnimalId");


--
-- Name: IX_SuccessStories_PublishedAt; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_SuccessStories_PublishedAt" ON public."SuccessStories" USING btree ("PublishedAt");


--
-- Name: IX_SuccessStories_UserId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_SuccessStories_UserId" ON public."SuccessStories" USING btree ("UserId");


--
-- Name: IX_Tags_Name; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Tags_Name" ON public."Tags" USING btree ("Name");


--
-- Name: IX_UserClaims_UserId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_UserClaims_UserId" ON public."UserClaims" USING btree ("UserId");


--
-- Name: IX_UserLogins_UserId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_UserLogins_UserId" ON public."UserLogins" USING btree ("UserId");


--
-- Name: IX_UserRoles_RoleId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_UserRoles_RoleId" ON public."UserRoles" USING btree ("RoleId");


--
-- Name: IX_Users_Phone; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_Users_Phone" ON public."Users" USING btree ("Phone");


--
-- Name: IX_VolunteerTaskAssignments_UserId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_VolunteerTaskAssignments_UserId" ON public."VolunteerTaskAssignments" USING btree ("UserId");


--
-- Name: IX_VolunteerTaskAssignments_VolunteerTaskId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_VolunteerTaskAssignments_VolunteerTaskId" ON public."VolunteerTaskAssignments" USING btree ("VolunteerTaskId");


--
-- Name: IX_VolunteerTasks_Date; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_VolunteerTasks_Date" ON public."VolunteerTasks" USING btree ("Date");


--
-- Name: IX_VolunteerTasks_ShelterId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_VolunteerTasks_ShelterId" ON public."VolunteerTasks" USING btree ("ShelterId");


--
-- Name: IX_VolunteerTasks_Status; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_VolunteerTasks_Status" ON public."VolunteerTasks" USING btree ("Status");


--
-- Name: RoleNameIndex; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "RoleNameIndex" ON public."Roles" USING btree ("NormalizedName");


--
-- Name: UserNameIndex; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "UserNameIndex" ON public."Users" USING btree ("NormalizedUserName");


--
-- Name: AdoptionApplications FK_AdoptionApplications_Animals_AnimalId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AdoptionApplications"
    ADD CONSTRAINT "FK_AdoptionApplications_Animals_AnimalId" FOREIGN KEY ("AnimalId") REFERENCES public."Animals"("Id") ON DELETE CASCADE;


--
-- Name: AdoptionApplications FK_AdoptionApplications_Users_ApprovedBy; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AdoptionApplications"
    ADD CONSTRAINT "FK_AdoptionApplications_Users_ApprovedBy" FOREIGN KEY ("ApprovedBy") REFERENCES public."Users"("Id") ON DELETE SET NULL;


--
-- Name: AdoptionApplications FK_AdoptionApplications_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AdoptionApplications"
    ADD CONSTRAINT "FK_AdoptionApplications_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: AnimalAidDonations FK_AnimalAidDonations_AnimalAidRequests_AnimalAidRequestId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AnimalAidDonations"
    ADD CONSTRAINT "FK_AnimalAidDonations_AnimalAidRequests_AnimalAidRequestId" FOREIGN KEY ("AnimalAidRequestId") REFERENCES public."AnimalAidRequests"("Id") ON DELETE CASCADE;


--
-- Name: AnimalAidDonations FK_AnimalAidDonations_Donations_DonationId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AnimalAidDonations"
    ADD CONSTRAINT "FK_AnimalAidDonations_Donations_DonationId" FOREIGN KEY ("DonationId") REFERENCES public."Donations"("Id") ON DELETE CASCADE;


--
-- Name: AnimalAidRequests FK_AnimalAidRequests_Shelters_ShelterId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AnimalAidRequests"
    ADD CONSTRAINT "FK_AnimalAidRequests_Shelters_ShelterId" FOREIGN KEY ("ShelterId") REFERENCES public."Shelters"("Id") ON DELETE SET NULL;


--
-- Name: AnimalAidRequests FK_AnimalAidRequests_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AnimalAidRequests"
    ADD CONSTRAINT "FK_AnimalAidRequests_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE SET NULL;


--
-- Name: AnimalSubscriptions FK_AnimalSubscriptions_Animals_AnimalId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AnimalSubscriptions"
    ADD CONSTRAINT "FK_AnimalSubscriptions_Animals_AnimalId" FOREIGN KEY ("AnimalId") REFERENCES public."Animals"("Id") ON DELETE CASCADE;


--
-- Name: AnimalSubscriptions FK_AnimalSubscriptions_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AnimalSubscriptions"
    ADD CONSTRAINT "FK_AnimalSubscriptions_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: AnimalTags FK_AnimalTags_Animals_AnimalId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AnimalTags"
    ADD CONSTRAINT "FK_AnimalTags_Animals_AnimalId" FOREIGN KEY ("AnimalId") REFERENCES public."Animals"("Id") ON DELETE CASCADE;


--
-- Name: AnimalTags FK_AnimalTags_Tags_TagId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AnimalTags"
    ADD CONSTRAINT "FK_AnimalTags_Tags_TagId" FOREIGN KEY ("TagId") REFERENCES public."Tags"("Id") ON DELETE CASCADE;


--
-- Name: Animals FK_Animals_Breeds_BreedId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Animals"
    ADD CONSTRAINT "FK_Animals_Breeds_BreedId" FOREIGN KEY ("BreedId") REFERENCES public."Breeds"("Id") ON DELETE RESTRICT;


--
-- Name: Animals FK_Animals_Shelters_ShelterId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Animals"
    ADD CONSTRAINT "FK_Animals_Shelters_ShelterId" FOREIGN KEY ("ShelterId") REFERENCES public."Shelters"("Id") ON DELETE CASCADE;


--
-- Name: Animals FK_Animals_Shelters_ShelterId1; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Animals"
    ADD CONSTRAINT "FK_Animals_Shelters_ShelterId1" FOREIGN KEY ("ShelterId1") REFERENCES public."Shelters"("Id");


--
-- Name: Animals FK_Animals_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Animals"
    ADD CONSTRAINT "FK_Animals_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE SET NULL;


--
-- Name: ArticleComments FK_ArticleComments_ArticleComments_ParentCommentId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."ArticleComments"
    ADD CONSTRAINT "FK_ArticleComments_ArticleComments_ParentCommentId" FOREIGN KEY ("ParentCommentId") REFERENCES public."ArticleComments"("Id") ON DELETE CASCADE;


--
-- Name: ArticleComments FK_ArticleComments_Articles_ArticleId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."ArticleComments"
    ADD CONSTRAINT "FK_ArticleComments_Articles_ArticleId" FOREIGN KEY ("ArticleId") REFERENCES public."Articles"("Id") ON DELETE CASCADE;


--
-- Name: ArticleComments FK_ArticleComments_Users_ModeratedById; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."ArticleComments"
    ADD CONSTRAINT "FK_ArticleComments_Users_ModeratedById" FOREIGN KEY ("ModeratedById") REFERENCES public."Users"("Id") ON DELETE SET NULL;


--
-- Name: ArticleComments FK_ArticleComments_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."ArticleComments"
    ADD CONSTRAINT "FK_ArticleComments_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: Articles FK_Articles_Categories_CategoryId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Articles"
    ADD CONSTRAINT "FK_Articles_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES public."Categories"("Id") ON DELETE SET NULL;


--
-- Name: Articles FK_Articles_Users_AuthorId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Articles"
    ADD CONSTRAINT "FK_Articles_Users_AuthorId" FOREIGN KEY ("AuthorId") REFERENCES public."Users"("Id") ON DELETE SET NULL;


--
-- Name: AuditLogs FK_AuditLogs_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AuditLogs"
    ADD CONSTRAINT "FK_AuditLogs_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE SET NULL;


--
-- Name: Breeds FK_Breeds_Species_SpeciesId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Breeds"
    ADD CONSTRAINT "FK_Breeds_Species_SpeciesId" FOREIGN KEY ("SpeciesId") REFERENCES public."Species"("Id") ON DELETE RESTRICT;


--
-- Name: Donations FK_Donations_PaymentMethods_PaymentMethodId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Donations"
    ADD CONSTRAINT "FK_Donations_PaymentMethods_PaymentMethodId" FOREIGN KEY ("PaymentMethodId") REFERENCES public."PaymentMethods"("Id") ON DELETE RESTRICT;


--
-- Name: Donations FK_Donations_Shelters_ShelterId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Donations"
    ADD CONSTRAINT "FK_Donations_Shelters_ShelterId" FOREIGN KEY ("ShelterId") REFERENCES public."Shelters"("Id") ON DELETE SET NULL;


--
-- Name: Donations FK_Donations_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Donations"
    ADD CONSTRAINT "FK_Donations_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE SET NULL;


--
-- Name: EventParticipants FK_EventParticipants_Events_EventId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."EventParticipants"
    ADD CONSTRAINT "FK_EventParticipants_Events_EventId" FOREIGN KEY ("EventId") REFERENCES public."Events"("Id") ON DELETE CASCADE;


--
-- Name: EventParticipants FK_EventParticipants_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."EventParticipants"
    ADD CONSTRAINT "FK_EventParticipants_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: Events FK_Events_Shelters_ShelterId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Events"
    ADD CONSTRAINT "FK_Events_Shelters_ShelterId" FOREIGN KEY ("ShelterId") REFERENCES public."Shelters"("Id") ON DELETE SET NULL;


--
-- Name: Events FK_Events_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Events"
    ADD CONSTRAINT "FK_Events_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id");


--
-- Name: GamificationRewards FK_GamificationRewards_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."GamificationRewards"
    ADD CONSTRAINT "FK_GamificationRewards_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: GamificationRewards FK_GamificationRewards_VolunteerTasks_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."GamificationRewards"
    ADD CONSTRAINT "FK_GamificationRewards_VolunteerTasks_TaskId" FOREIGN KEY ("TaskId") REFERENCES public."VolunteerTasks"("Id") ON DELETE SET NULL;


--
-- Name: IoTDevices FK_IoTDevices_Shelters_ShelterId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."IoTDevices"
    ADD CONSTRAINT "FK_IoTDevices_Shelters_ShelterId" FOREIGN KEY ("ShelterId") REFERENCES public."Shelters"("Id") ON DELETE CASCADE;


--
-- Name: Likes FK_Likes_ArticleComments_ArticleCommentId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Likes"
    ADD CONSTRAINT "FK_Likes_ArticleComments_ArticleCommentId" FOREIGN KEY ("ArticleCommentId") REFERENCES public."ArticleComments"("Id") ON DELETE CASCADE;


--
-- Name: Likes FK_Likes_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Likes"
    ADD CONSTRAINT "FK_Likes_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: LostPets FK_LostPets_Breeds_BreedId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."LostPets"
    ADD CONSTRAINT "FK_LostPets_Breeds_BreedId" FOREIGN KEY ("BreedId") REFERENCES public."Breeds"("Id") ON DELETE SET NULL;


--
-- Name: LostPets FK_LostPets_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."LostPets"
    ADD CONSTRAINT "FK_LostPets_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: Notifications FK_Notifications_NotificationTypes_NotificationTypeId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Notifications"
    ADD CONSTRAINT "FK_Notifications_NotificationTypes_NotificationTypeId" FOREIGN KEY ("NotificationTypeId") REFERENCES public."NotificationTypes"("Id") ON DELETE RESTRICT;


--
-- Name: Notifications FK_Notifications_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Notifications"
    ADD CONSTRAINT "FK_Notifications_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: RoleClaims FK_RoleClaims_Roles_RoleId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."RoleClaims"
    ADD CONSTRAINT "FK_RoleClaims_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."Roles"("Id") ON DELETE CASCADE;


--
-- Name: ShelterSubscriptions FK_ShelterSubscriptions_Shelters_ShelterId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."ShelterSubscriptions"
    ADD CONSTRAINT "FK_ShelterSubscriptions_Shelters_ShelterId" FOREIGN KEY ("ShelterId") REFERENCES public."Shelters"("Id") ON DELETE CASCADE;


--
-- Name: ShelterSubscriptions FK_ShelterSubscriptions_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."ShelterSubscriptions"
    ADD CONSTRAINT "FK_ShelterSubscriptions_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: Shelters FK_Shelters_Users_ManagerId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Shelters"
    ADD CONSTRAINT "FK_Shelters_Users_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES public."Users"("Id") ON DELETE SET NULL;


--
-- Name: SuccessStories FK_SuccessStories_Animals_AnimalId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."SuccessStories"
    ADD CONSTRAINT "FK_SuccessStories_Animals_AnimalId" FOREIGN KEY ("AnimalId") REFERENCES public."Animals"("Id") ON DELETE CASCADE;


--
-- Name: SuccessStories FK_SuccessStories_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."SuccessStories"
    ADD CONSTRAINT "FK_SuccessStories_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE SET NULL;


--
-- Name: UserClaims FK_UserClaims_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."UserClaims"
    ADD CONSTRAINT "FK_UserClaims_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: UserLogins FK_UserLogins_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."UserLogins"
    ADD CONSTRAINT "FK_UserLogins_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: UserRoles FK_UserRoles_Roles_RoleId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."UserRoles"
    ADD CONSTRAINT "FK_UserRoles_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."Roles"("Id") ON DELETE CASCADE;


--
-- Name: UserRoles FK_UserRoles_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."UserRoles"
    ADD CONSTRAINT "FK_UserRoles_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: UserTokens FK_UserTokens_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."UserTokens"
    ADD CONSTRAINT "FK_UserTokens_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: VolunteerTaskAssignments FK_VolunteerTaskAssignments_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."VolunteerTaskAssignments"
    ADD CONSTRAINT "FK_VolunteerTaskAssignments_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- Name: VolunteerTaskAssignments FK_VolunteerTaskAssignments_VolunteerTasks_VolunteerTaskId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."VolunteerTaskAssignments"
    ADD CONSTRAINT "FK_VolunteerTaskAssignments_VolunteerTasks_VolunteerTaskId" FOREIGN KEY ("VolunteerTaskId") REFERENCES public."VolunteerTasks"("Id") ON DELETE CASCADE;


--
-- Name: VolunteerTasks FK_VolunteerTasks_Shelters_ShelterId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."VolunteerTasks"
    ADD CONSTRAINT "FK_VolunteerTasks_Shelters_ShelterId" FOREIGN KEY ("ShelterId") REFERENCES public."Shelters"("Id") ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

