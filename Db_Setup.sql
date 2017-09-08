﻿-- Table: public."DistCache"

-- DROP TABLE public."DistCache";

CREATE TABLE public."DistCache"
(
  "Id" text NOT NULL,
  "Value" bytea,
  "ExpiresAtTime" timestamp with time zone,
  "SlidingExpirationInSeconds" double precision,
  "AbsoluteExpiration" timestamp with time zone,
  CONSTRAINT "DistCache_pkey" PRIMARY KEY ("Id")
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public."DistCache"
  OWNER TO postgres;
