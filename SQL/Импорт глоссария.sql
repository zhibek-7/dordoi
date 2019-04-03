
--insert into   glossaries_locales ( id_locale, id_glossary) SELECT  distinct t.id_locale ,  g.id
--insert into   files_locales ( id_locale, id_file) SELECT  distinct t.id_locale ,  f.id
insert into   translation_substrings_locales ( id_locale, id_translation_substrings) SELECT  distinct t.id_locale ,  ts.id
FROM public.translation_substrings as ts 
inner join  translations as t 
on ts.id = t.id_string
inner join files as f
on ts.id_file_owner=f.id
inner join glossaries as g
on g.id_file=f.id


'd894cc4c-aa98-4785-86ca-e0e8115a38bb' - память переводов