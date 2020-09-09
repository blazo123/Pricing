select
a.material,

max((case when a.lp=1 then to_char(a.DATA_REALIZACJI,'yyyy.mm.dd') else ' ' end)) "cOdtwDat1",
ROUND(max((case when a.lp=1 then a.Cena_NABYCIA_PLN else 0 end)),2) "cOdtwCen1",
max((case when a.lp=1 then a.WALUTA else ' ' end)) "cOdtwWAL1",
max((case when a.lp=2 then to_char(a.DATA_REALIZACJI,'yyyy.mm.dd') else ' ' end)) "cOdtwDat2",
ROUND(max((case when a.lp=2 then a.Cena_NABYCIA_PLN else 0 end)),2) "cOdtwCen2",
max((case when a.lp=2 then a.WALUTA else ' ' end)) "cOdtwWAL2",
max((case when a.lp=3 then to_char(a.DATA_REALIZACJI,'yyyy.mm.dd') else ' ' end)) "cOdtwDat3",
ROUND(max((case when a.lp=3 then a.Cena_NABYCIA_PLN else 0 end)),2) "cOdtwCen3",
max((case when a.lp=3 then a.WALUTA else ' ' end)) "cOdtwWAL3",
ROUND(cey.CenaMin,2) "Cena_Min90",
ROUND(cey3.sr30,2) "SredniaC30",
ROUND(cey.sr90,2) "SredniaC90"

from
(
select
zam.material,
zam.DATA_REALIZACJI,
zam.CENA_NABYCIA_PLN,
zam.WALUTA,
RANK () OVER (PARTITION BY zam.MATERIAL ORDER BY zam.DATA_REALIZACJI) lp

FROM OLAP_DANE.MV_SAP_ZAMOW zam

WHERE zam.DATA_REALIZACJI >= SYSDATE
AND zam.MATERIAL = '1101050012B58D933K'
)a

LEFT OUTER JOIN

(SELECT copa.MATERIAL, MIN(copa.NETTO/copa.ILOSC) CenaMin, avg(copa.netto/copa.ilosc) sr90
    FROM OLAP_DANE.MV_SAP_COPA copa 
    WHERE copa."data utworzenia faktury" >= sysdate - 90 and 
    copa."dzial sprzedazy" NOT IN ('0990','0010') and copa.MATERIAL IN '1101050012B58D933K'
    GROUP BY copa.MATERIAL
    )cey
    ON a.MATERIAL  = cey.MATERIAL

LEFT OUTER JOIN

(SELECT copa.MATERIAL, avg(copa.netto/copa.ilosc) sr30
    FROM OLAP_DANE.MV_SAP_COPA copa 
    WHERE copa."data utworzenia faktury" >= sysdate - 30 and copa."dzial sprzedazy" NOT IN ('0990','0010') and copa.MATERIAL IN '1101050012B58D933K'
    GROUP BY copa.MATERIAL
    )cey3
    ON a.MATERIAL  = cey3.MATERIAL    
    
group by a.material,cey.CenaMin,
cey3.sr30,
cey.sr90

ORDER BY 1;
