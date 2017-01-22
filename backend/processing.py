import requests
import math
import json

SEARCH_RADIUS = 0.25 # miles
CHICAGO_AREA = 227.3 # square miles

def todegrees(miles):
    return miles / 69.0
def tomiles(degrees):
    return degrees * 69
    
SEARCH_OFFSET = todegrees(SEARCH_RADIUS)

def blocksincity(sidelength):
    """Calculates the number of blocks with given side length that fit in the city"""
    sidelengthmi = tomiles(sidelength)
    return CHICAGO_AREA / (sidelengthmi * sidelengthmi)

DATABASE = "https://data.cityofchicago.org/resource/6zsd-86xi.json?"
WHERE = "$where="
LAT = "latitude"
LONG = "longitude"
LT = "<"
GT = ">"
AND = "%20AND%20"
AMP = "&"
KEY = "$$app_token=0RXUfxLz5gidFRlLEnI19s4WI"
LIMIT = "$limit=100000"
OFFSET = "$offset="
ORDER = "$order=:id"
FBI_CODE = "fbi_code="
INCREMENT = 100000

CLASSIFICATION = {'02': 1, '03': 1, '01A': 1, '01B': 1, '04A': 1}

def countrows(query):
    """Counts the number of entries returned by a given query"""
    total = 0
    page = 0
    entrycount = INCREMENT
    query += AMP + LIMIT + AMP + ORDER + AMP + OFFSET
    while entrycount >= INCREMENT:
        offseturl = query + str(page*INCREMENT)
        response = requests.get(offseturl)
        if response.status_code != 200:
            print("countrows(): " + str(response))
            return
        entrycount = len(response.json())
        total += entrycount
        page += 1
    return total

def measurecrime(gps, radius):
    """Measures crime around a given location"""
    latitude, longitude = gps
    minlat = latitude - radius
    maxlat = latitude + radius
    minlong = longitude - radius
    maxlong = longitude + radius
    
    baseurl = (DATABASE + WHERE + LAT + GT + str(minlat) + AND + LAT + LT + 
            str(maxlat) + AND + LONG + GT + str(minlong) + AND + LONG + LT +
            str(maxlong) + AMP + KEY + AMP + FBI_CODE)
            
    crimescore = 0
    for code in CLASSIFICATION.keys():
        query = baseurl + code
        component = CLASSIFICATION[code] * countrows(query)
        crimescore += component
    return crimescore
    
def measurecitywide():
    """Measure citywide crime"""
    baseurl = DATABASE + KEY + AMP + FBI_CODE
    crimescore = 0
    for code in CLASSIFICATION.keys():
        query = baseurl + code
        crimescore += CLASSIFICATION[code] * countrows(query)
    return crimescore
    
CITYWIDE = 368070
    
def dangerlevel(gps, radius):
    """Calculates the danger level around a point"""
    crimescore = measurecrime(gps, radius)
    cityavg = CITYWIDE / blocksincity(radius * 2)
    return crimescore / cityavg
    
def dangerlevelcompass(gps):
    """Calculates the danger levels for each cardinal direction and center"""
    latitude, longitude = gps
    here = dangerlevel(gps, SEARCH_OFFSET)
    north = dangerlevel((latitude+SEARCH_OFFSET, longitude), SEARCH_OFFSET)
    south = dangerlevel((latitude-SEARCH_OFFSET, longitude), SEARCH_OFFSET)
    east = dangerlevel((latitude, longitude+SEARCH_OFFSET), SEARCH_OFFSET)
    west = dangerlevel((latitude, longitude-SEARCH_OFFSET), SEARCH_OFFSET)
    return here, north, south, east, west
    
def isdangerous(dangerlevel):
    """Tests against the notification threshold"""
    return dangerlevel > 1.75
    
NORTH_BORDER = 42.022772
SOUTH_BORDER = 41.644582
EAST_BORDER = -87.524841
WEST_BORDER = -87.836748
BLOCK_SIDE = 2.5
GRID_UNIT = todegrees(BLOCK_SIDE)

def generatemap():
    """Generate a grid map of crime level data points across the city"""
    datapoints = []
    for xi in range(round((EAST_BORDER - WEST_BORDER) / GRID_UNIT)):
        for yi in range(round((NORTH_BORDER - SOUTH_BORDER) / GRID_UNIT)):
            latitude = SOUTH_BORDER + yi * GRID_UNIT
            longitude = WEST_BORDER + xi * GRID_UNIT
            print((xi, yi))
            crimelevel = dangerlevel((latitude, longitude), GRID_UNIT/2)
            if crimelevel > 0:
                datapoints.append({"latitude": latitude, "longitude": longitude,
                        "crimelevel": crimelevel})
    return datapoints

# with open('map.txt', 'w') as outfile:
#     json.dump(generatemap(), outfile)