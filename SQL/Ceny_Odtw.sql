select
material,

max((case when lp=1 then to_char(DATA_REALIZACJI,'yyyy.mm.dd') else ' ' end)) data1,
max((case when lp=1 then ROUND(CENA_NABYCIA_PLN,2) else null end)) cena1,
max((case when lp=1 then WALUTA else ' ' end)) wal1,

max((case when lp=2 then to_char(DATA_REALIZACJI,'yyyy.mm.dd') else ' ' end)) data2,
max((case when lp=2 then ROUND(CENA_NABYCIA_PLN,2) else null end)) cena2,
max((case when lp=2 then WALUTA else ' ' end)) wal2,


max((case when lp=3 then to_char(DATA_REALIZACJI,'yyyy.mm.dd') else ' ' end)) data3,
max((case when lp=3 then ROUND(CENA_NABYCIA_PLN,2) else null end)) cena3,
max((case when lp=4 then WALUTA else ' ' end)) wal3

from
(
select
zam.material,
zam.DATA_REALIZACJI,
zam.CENA_NABYCIA_PLN,
WALUTA,
RANK () OVER (PARTITION BY zam.MATERIAL ORDER BY zam.DATA_REALIZACJI,zam.NR_ZAMOW,rowid) lp

FROM OLAP_DANE.MV_SAP_ZAMOW zam

WHERE zam.DATA_REALIZACJI >= SYSDATE -7
--And  ((zam.STATUS = 'P' and zam.WALUTA = 'USD') OR (zam.STATUS IN('P',' ') and zam.WALUTA IN('PLN','EUR','GBP')))
)

group by material
ORDER BY 1