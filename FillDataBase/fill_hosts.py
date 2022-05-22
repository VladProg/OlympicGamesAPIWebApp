from requests import get,put
from csv import reader
import warnings
warnings.filterwarnings("ignore")

LINK='https://localhost:7011/api/'

countries={c['name']:c['id'] for c in get(LINK+'Countries',verify=False).json()}
games={(g['year'],int(g['season'].split()[1])):g['id'] for g in get(LINK+'Games',verify=False).json()}

for h in reader(open('hosts.csv'),delimiter=';'):
    country,year,summer,winter=(x.strip() for x in h)
    if country=='United Kingdom':
        country='Great Britain'
    country=countries[country]
    if year:
        year=int(year)
    else:
        year=prev
    summer=bool(summer)
    winter=bool(winter)
    prev=year
    print(country,year,summer,winter,sep='\t')
    if summer and (year,1) in games:
        resp=put(LINK+'Games/'+str(games[year,1]),verify=False,json={'id':games[year,1],'year':year,'season':1,'countryId':country}).json()
        if 'id' not in resp:
            print(resp)
    if winter and (year,0) in games:
        resp=put(LINK+'Games/'+str(games[year,0]),verify=False,json={'id':games[year,0],'year':year,'season':0,'countryId':country}).json()
        if 'id' not in resp:
            print(resp)
