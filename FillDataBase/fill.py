from requests import get,post
from csv import DictReader
import warnings
warnings.filterwarnings("ignore")

athletes={}
countries={}
events={}
games={}
sports={}

def add(tp,**json):
    res=post('https://localhost:7011/api/'+tp,verify=False,json=json)
    try:
        res=res.json()
    except:
        print(res.text)
        raise
    try:
        return res['id']
    except:
        print(tp,json,res)

for s in get('https://en.wikipedia.org/wiki/List_of_IOC_country_codes').text.split('<tr>'):
    s=s.split('<span class="monospaced">')
    if len(s)!=2:
        continue
    s=s[1]
    noc=s[:3]
    s=s.split('" src="')[1]
    flag,s=s.split('" decoding="async"')
    name=s.split('title="')[1].split('">')[1].split('</')[0]
    countries[noc]=add('Countries',name=name,flagUrl='https:'+flag)

for item in DictReader(open('athlete_events.csv')):
    athlete=item['Name']
    if athlete not in athletes:
        athletes[athlete]=add('Athletes',name=athlete)
    athlete=athletes[athlete]
    
    team=item['Team']
    noc=item['NOC']
    if noc not in countries:
        countries[noc]=add('Countries',name=team)
    country=countries[noc]
    
    year=int(item['Year'])
    season={'Winter':0,'Summer':1}[item['Season']]
    if (year,season) not in games:
        games[year,season]=add('Games',year=year,season=season)
    game=games[year,season]
    
    sport=item['Sport']
    if sport not in sports:
        sports[sport]=add('Sports',name=sport)
    sport=sports[sport]

    event=item['Event']
    if (sport,event) not in events:
        events[sport,event]=add('Events',name=event,sportId=sport)
    event=events[sport,event]
    
    medal={'NA':0,'Gold':1,'Silver':2,'Bronze':3}[item['Medal']]
    
    add('Participations',athleteId=athlete,countryId=country,gameId=game,eventId=event,medal=medal)
