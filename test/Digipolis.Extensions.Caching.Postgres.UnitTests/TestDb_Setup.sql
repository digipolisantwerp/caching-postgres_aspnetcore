-- Table: public."CacheTest"

-- DROP TABLE public."CacheTest";

CREATE TABLE public."CacheTest"
(
  "Id" text NOT NULL,
  "Value" bytea,
  "ExpiresAtTime" timestamp with time zone,
  "SlidingExpirationInSeconds" double precision,
  "AbsoluteExpiration" timestamp with time zone,
  CONSTRAINT "CacheTest_pkey" PRIMARY KEY ("Id")
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public."CacheTest"
  OWNER TO postgres;
