
SELECT DISTINCT materialy.MATERIAL,materialy.NAZWA_MATERIALU,
SUM(CASE WHEN copa."data utworzenia faktury" >= SYSDATE - 30 THEN copa.ILOSC ELSE 0 END) "Ilosc30dni",
SUM(CASE WHEN copa."data utworzenia faktury" >= SYSDATE - 90 THEN copa.ILOSC ELSE 0 END) "Ilosc90dni",
materialy.MAABC,
stany.stanatp,
stany.stan,
DECODE(((SUM(CASE WHEN copa."data utworzenia faktury" >= SYSDATE - 30 THEN copa.ILOSC ELSE 0 END))/30),0,0,
        ROUND(stany.stanatp/((SUM(CASE WHEN copa."data utworzenia faktury" >= SYSDATE - 30 THEN copa.ILOSC ELSE 0 END))/30),2)) "Zapas_30_dni",

DECODE(((SUM(CASE WHEN copa."data utworzenia faktury" >= SYSDATE - 90 THEN copa.ILOSC ELSE 0 END))/90),0,0,
        ROUND(stany.stanatp/((SUM(CASE WHEN copa."data utworzenia faktury" >= SYSDATE - 90 THEN copa.ILOSC ELSE 0 END))/90),2)) "Zapas_90_dni"

FROM OLAP_DANE.MV_SAP_COPA copa

LEFT OUTER JOIN

(SELECT DISTINCT mara.MATERIAL,mara.NAZWA_MATERIALU,mara.MAABC
FROM OLAP_DANE.MV_SAP_MARA mara
GROUP BY mara.MATERIAL,mara.NAZWA_MATERIALU,mara.MAABC) materialy
ON copa.MATERIAL = materialy.MATERIAL

LEFT OUTER JOIN
(SELECT DISTINCT stan.MATERIAL,SUM(stan.STAN_ATP) stanatp,SUM(stan.STAN) stan
FROM OLAP_DANE.MV_SAP_STAN stan
WHERE stan.ZAKLAD = '0001'
GROUP BY stan.MATERIAL)stany
ON copa.MATERIAL = stany.MATERIAL

AND copa.ZAKLAD = '0001'
WHERE stany.stanatp IS NOT NULL
GROUP BY materialy.MATERIAL,materialy.MAABC,stany.stanatp,materialy.NAZWA_MATERIALU,
stany.stan