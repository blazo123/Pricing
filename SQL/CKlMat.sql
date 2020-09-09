SELECT (case when a.KOD_BRANZY like '01%' then 'Producent' else 'Handel' end) as "Typ Klienta" ,
(case when a.Segment2017 between 'A' AND 'C' THEN 'Duzy' else 'Maly' END) Wielkosc,
b.ODBIORCA,b.Material,b.cena1,b.cena2,b.cena3,ROUND(b."sr90",2) "Srednia90"
FROM olap_dane.mv_sap_odbiorcy a

LEFT OUTER JOIN

(SELECT DISTINCT a.ODBIORCA,a.Material,
ROUND(MAX(( Case when a.rank = 1 then a.Cena else null end)),2) cena1,
ROUND(MAX(( Case when a.rank = 2 then a.Cena else null end)),2) cena2,
ROUND(MAX(( Case when a.rank = 3 then a.Cena else null end)),2) cena3,
a."sr90"

FROM(Select copa.ODBIORCA,copa.MATERIAL,
    (copa.netto/copa.ilosc) Cena,
    AVG((copa.netto/copa.ilosc)) OVER (PARTITION BY copa.ODBIORCA,copa.MATERIAL) "sr90",
RANK () OVER (PARTITION BY copa.ODBIORCA,copa.MATERIAL ORDER BY copa."data utworzenia faktury" DESC,copa."numer faktury" DESC, ROWID) rank
FROM olap_dane.mv_sap_copa copa
WHERE copa."data utworzenia faktury" >= SYSDATE - 90)a
GROUP BY odbiorca,material,a."sr90")b
ON a.KOD_ODBIORCY = b.ODBIORCA
WHERE b.Cena1 IS NOT NULL 
GROUP BY (case when a.KOD_BRANZY like '01%' then 'Producent' else 'Handel' end),
(case when a.Segment2017 between 'A' AND 'C' THEN 'Duzy' else 'Maly' END),
b.ODBIORCA,b.Material,b.cena1,b.cena2,b.cena3,b."sr90";

