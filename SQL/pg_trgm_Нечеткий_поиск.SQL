--https://eax.me/pg-trgm/
-- Extension: pg_trgm

-- DROP EXTENSION pg_trgm;

CREATE EXTENSION pg_trgm
	SCHEMA public
	VERSION 1.3


-------------


-- DROP TYPE gtrgm;

CREATE TYPE gtrgm (
	INPUT = gtrgm_in,
	OUTPUT = gtrgm_out,
	ALIGNMENT = 4,
	STORAGE = plain,
	CATEGORY = U,
	DELIMITER = ',');
-----------------------
--tbl_substingToTranslate_trgm_idx

CREATE INDEX "tbl_substingToTranslate_trgm_idx" ON public."translation_substrings" 
USING gist ("substring_to_translate" gist_trgm_ops)

