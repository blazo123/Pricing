SELECT DISTINCT copa.ODBIORCA,(odb.NAZWA1 || odb.NAZWA2) "Nazwa_Firmy", (odb.Segment2018 ||'/' || odb.ZSEGMENT2018) "Segment-O/Z-2018",
(odb.Segment2019 ||'/' || odb.ZSEGMENT2019) "Segment-O/Z-2019",
TO_CHAR(odb.MAX_DATA_SPRZEDAZY,'YYYY-MM-DD') "Max. Data Faktury",

(CASE WHEN (ROUND(DECODE(SUM(CASE WHEN TRUNC(copa."data utworzenia faktury",'YYYY') IN TRUNC(SYSDATE,'YYYY') THEN copa.NETTO ELSE 0 END),0,NULL,
        SUM(CASE WHEN TRUNC(copa."data utworzenia faktury",'YYYY') IN TRUNC(SYSDATE,'YYYY') THEN copa.ZYSK ELSE 0 END)/
                            SUM(CASE WHEN TRUNC(copa."data utworzenia faktury",'YYYY') IN TRUNC(SYSDATE,'YYYY') THEN copa.NETTO ELSE 0 END)),4)*100 || '%')='%' THEN 'Brak sprzedaży' ELSE 
            (ROUND(DECODE(SUM(CASE WHEN TRUNC(copa."data utworzenia faktury",'YYYY') IN TRUNC(SYSDATE,'YYYY') THEN copa.NETTO ELSE 0 END),0,NULL,
        SUM(CASE WHEN TRUNC(copa."data utworzenia faktury",'YYYY') IN TRUNC(SYSDATE,'YYYY') THEN copa.ZYSK ELSE 0 END)/
                            SUM(CASE WHEN TRUNC(copa."data utworzenia faktury",'YYYY') IN TRUNC(SYSDATE,'YYYY') THEN copa.NETTO ELSE 0 END)),4)*100 || '%') 
                            END) "Marza_Rok_Obecny",

(CASE WHEN(ROUND(DECODE(SUM(CASE WHEN TRUNC(copa."data utworzenia faktury",'YYYY') >= '2009-01-01' THEN copa.NETTO ELSE 0 END),0,NULL,
        SUM(CASE WHEN TRUNC(copa."data utworzenia faktury",'YYYY') >= '2009-01-01' THEN copa.ZYSK ELSE 0 END)/
                            SUM(CASE WHEN TRUNC(copa."data utworzenia faktury",'YYYY') >= '2009-01-01' THEN copa.NETTO ELSE 0 END)),4)*100 || '%')='%' THEN 'Brak sprzedaży'

                            ELSE (ROUND(DECODE(SUM(CASE WHEN TRUNC(copa."data utworzenia faktury",'YYYY') >= '2009-01-01' THEN copa.NETTO ELSE 0 END),0,NULL,
        SUM(CASE WHEN TRUNC(copa."data utworzenia faktury",'YYYY') >= '2009-01-01' THEN copa.ZYSK ELSE 0 END)/
                            SUM(CASE WHEN TRUNC(copa."data utworzenia faktury",'YYYY') >= '2009-01-01' THEN copa.NETTO ELSE 0 END)),4)*100 || '%')
                            END) "Marża_TOTAL",


branze.BRANZA "Branza",
b.odbiorca as Kontrolowany,
(marza_II.marza_ii *100 ||'%') marza_ii,
(marza_II.marza_ii_bez_mag * 100 || '%') marza_II_bez_mag,
(CASE WHEN MIN(copa."data utworzenia faktury") > SYSDATE - 182 THEN 'NK'
        WHEN MAX(copa."data utworzenia faktury") < SYSDATE - 182 THEN 'UK'
        ELSE '' END) NK_UK

FROM OLAP_DANE.MV_SAP_COPA copa, OLAP_DANE.MV_SAP_ODBIORCY odb

LEFT OUTER JOIN
    ( SELECT o.KOD_ODBIORCY,br.KOD_BRANZY,br.BRANZA 
      FROM OLAP_DANE.MV_SAP_BRANZE br,olap_dane.mv_sap_odbiorcy o
      WHERE o.KOD_BRANZY = br.KOD_BRANZY
      GROUP BY o.KOD_ODBIORCY,br.KOD_BRANZY,br.BRANZA
    ) branze
ON odb.KOD_ODBIORCY = branze.KOD_ODBIORCY

LEFT OUTER JOIN
(
SELECT
    odbiorca,
    nazwa_firmy
FROM
    DWS1.kontrolowany
)b
ON b.odbiorca = odb.KOD_ODBIORCY

LEFT OUTER JOIN
(
SELECT
    odbiorca,
    rok,
    marza_ii,
    marza_ii_bez_mag
FROM
    marza_ii_rok
where rok in '2018'
)
marza_II
on odb.KOD_ODBIORCY = marza_II.odbiorca

WHERE copa.ODBIORCA = odb.KOD_ODBIORCY
GROUP BY copa.ODBIORCA,odb.NAZWA1, odb.NAZWA2, odb.Segment2018,odb.ZSEGMENT2018,odb.Segment2019,odb.ZSEGMENT2019,odb.MAX_DATA_SPRZEDAZY,branze.BRANZA,b.odbiorca,
marza_II.marza_ii,
marza_II.marza_ii_bez_mag;